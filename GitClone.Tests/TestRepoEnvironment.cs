using GitClone;

namespace UnitTests;

public class TestRepoEnvironment
{

    public readonly GitClonePaths Paths;
    public GitController GitController;

    public TestRepoEnvironment()
    {
        var workingDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Paths = new GitClonePaths(workingDir);
        GitController = new GitController(Paths);
    }

    public void Dispose()
    {
        Directory.Delete(Paths.BaseDir, true);
    }
}