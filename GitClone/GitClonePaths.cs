using System.Threading.Channels;

namespace GitClone;

public class GitClonePaths
{
    public string BaseDir { get; }
    public string RepoDir { get; }

    public GitClonePaths(string workingDir)
    {
        BaseDir = workingDir;
        RepoDir = Path.Combine(workingDir, ".gitClone");
    }

    public string ObjectDir => Path.Combine(RepoDir, "objects");
    public string IndexFile => Path.Combine(RepoDir, "index");
    public string BlobsDir => Path.Combine(ObjectDir, "blobs");
    public string CommitsDir => Path.Combine(ObjectDir, "commits");

}