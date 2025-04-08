using GitClone;
using Xunit.Abstractions;

namespace UnitTests;

public class UnitTest1
{
    private readonly ITestOutputHelper _testOutputHelper;

    public UnitTest1(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public void GitOperations()
    {
        var testEnv = new TestRepoEnvironment();
        
        //init repo
        testEnv.GitController.Init();
        
        //verify init creation
        Assert.True(Directory.Exists(testEnv.Paths.CommitsDir));
        Assert.True(File.Exists(testEnv.Paths.IndexFile));
        Assert.True(Directory.Exists(testEnv.Paths.BlobsDir));

        var file1Name = "gimpy.txt";
        var file1Content = "Hi, I am a gimp";

        string file1Path = TestFileHelper.CreateFile(testEnv.Paths.BaseDir,file1Name, file1Content);

        _testOutputHelper.WriteLine(file1Path);
        //sanity chekc the file is created
        Assert.True(File.Exists(file1Path), "Initial file was not created");
        
        //add working
        string blobPath = testEnv.GitController.Add(file1Name);
        Assert.True(File.Exists(blobPath));
        
        //check add content
        Assert.True(File.ReadAllText(blobPath).Equals(File.ReadAllText(file1Path)));
        
        //check index file correctly wrote to
        
        
        
        //cleanup
        testEnv.Dispose();
        
    }

    [Fact]
    public void IsFileLineCheckerWorking()
    {
        var testEnv = new TestRepoEnvironment();
        string file1Name = "testfile.txt";
        string file1Content = "12345\nabcd\nTHIS";
        string file1Path = TestFileHelper.CreateFile(testEnv.Paths.BaseDir,file1Name, file1Content);
        
        int matchLine = FileIO.GetInstance().GetLineOfExactMatch(file1Path,"abcd");
        int noLineMatch = FileIO.GetInstance().GetLineOfExactMatch(file1Path,"NON");
        
        Assert.Equal(1, matchLine);
        
        Assert.Equal(3, noLineMatch);
    }
}