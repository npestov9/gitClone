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
        List<FileAndBlob> allFilesAdded = new();

        testEnv.GitController.Init();
        AssertRepoStructure(testEnv);

        var fileName = "gimpy.txt";
        var fileContent = "Hi, I am a gimp";
        string filePath = CreateTestFile(testEnv, fileName, fileContent);
        AssertFileCreated(filePath);

        string blobPath = AddHelper(testEnv, filePath, ref allFilesAdded);
        AssertBlobMatchesOriginal(filePath, blobPath);

        
        AssertIndexFileCorrect(testEnv, allFilesAdded.ToArray());

        string commitMsg = "init commit";
        string commitPath = testEnv.GitController.Commit(commitMsg);
        AssertCommitFileCorrect(commitPath, commitMsg, allFilesAdded.ToArray());
        
        fileName = "newText.txt";
        fileContent = "hello world";
        filePath = CreateTestFile(testEnv, fileName, fileContent);
        AssertFileCreated(filePath);
        
        blobPath = AddHelper(testEnv, filePath, ref allFilesAdded);
        AssertBlobMatchesOriginal(filePath, blobPath);
        AssertIndexFileCorrect(testEnv, allFilesAdded.ToArray());
        
        commitMsg = "second commit";
        commitPath = testEnv.GitController.Commit(commitMsg);
        AssertCommitFileCorrect(commitPath, commitMsg, allFilesAdded.ToArray());
        
        _testOutputHelper.WriteLine("Doing git log");
        _testOutputHelper.WriteLine(testEnv.GitController.Log());

        testEnv.Dispose();
    }

    [Fact]
    public void IsFileLineCheckerWorking()
    {
        var testEnv = new TestRepoEnvironment();
        string fileName = "testfile.txt";
        string content = "12345\nabcd\nTHIS";
        string filePath = TestFileHelper.CreateFile(testEnv.Paths.BaseDir, fileName, content);

        var fileIO = FileIO.GetInstance();
        Assert.Equal(1, fileIO.GetLineOfExactMatch(filePath, "abcd"));
        Assert.Equal(-1, fileIO.GetLineOfExactMatch(filePath, "NON"));
    }

    // ─── Helpers ─────────────────────────────────────────
    
    private static void AssertRepoStructure(TestRepoEnvironment testEnv)
    {
        Assert.True(Directory.Exists(testEnv.Paths.CommitsDir), "Commits directory missing.");
        Assert.True(File.Exists(testEnv.Paths.IndexFile), "Index file missing.");
        Assert.True(Directory.Exists(testEnv.Paths.BlobsDir), "Blobs directory missing.");
    }

    private string AddHelper(TestRepoEnvironment testEnv,string filePath, ref List<FileAndBlob> fileAndBlob)
    {
        string blobPath = testEnv.GitController.Add(filePath);
        fileAndBlob.Add(new FileAndBlob(filePath,blobPath));

        return blobPath;
    }
    
    private string CreateTestFile(TestRepoEnvironment env, string name, string content)
    {
        string path = TestFileHelper.CreateFile(env.Paths.BaseDir, name, content);
        _testOutputHelper.WriteLine($"Created file: {path}");
        return path;
    }

    private static void AssertFileCreated(string path)
    {
        Assert.True(File.Exists(path), "Expected test file to be created.");
    }

    private static void AssertBlobMatchesOriginal(string filePath, string blobPath)
    {
        string originalContent = File.ReadAllText(filePath);
        string blobContent = File.ReadAllText(blobPath);
        Assert.Equal(originalContent, blobContent);
    }

    public struct FileAndBlob(string filePath, string blobPath)
    {
        public string FilePath { get; } = filePath;
        public string BlobPath { get; } = blobPath;
    }

    
    private void AssertIndexFileCorrect(TestRepoEnvironment env,FileAndBlob[] fileAndBlobPaths )
    {
        var lines = File.ReadAllLines(env.Paths.IndexFile);

        try
        {
            for (int i = 0; i < fileAndBlobPaths.Length; i++)
            {
                 Assert.Equal(fileAndBlobPaths[i].FilePath, lines[i*2]);
                 Assert.Equal(fileAndBlobPaths[i].BlobPath.Split(Path.DirectorySeparatorChar).Last(), lines[i*2+1]);
            }
        }
        catch (Exception ex)
        {
            _testOutputHelper.WriteLine("❌ Index file assertion failed!");
            _testOutputHelper.WriteLine("📄 Index file contents:");
            for (int i = 0; i < lines.Length; i++)
            {
                _testOutputHelper.WriteLine($"[{i}] {lines[i]}");
            }

            throw; // Re-throw to keep the test red
        }
    }


    private static void AssertCommitFileCorrect(string commitPath, string message, FileAndBlob[] fileAndBlobPaths)
    {
        Assert.True(File.Exists(commitPath), "Commit file not created.");
        var lines = File.ReadAllLines(commitPath);

        Assert.Equal(message, lines[0]);
        
        for (int i = 0; i < fileAndBlobPaths.Length; i++)
        {
            Assert.Equal(fileAndBlobPaths[i].FilePath, lines[4+i*2]);
            Assert.Equal(fileAndBlobPaths[i].BlobPath.Split(Path.DirectorySeparatorChar).Last(), lines[i*2+5]);
        }
        
    }
    
}
