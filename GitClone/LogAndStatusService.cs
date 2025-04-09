namespace GitClone;

public class LogAndStatusService
{
    private readonly FileIO _fileIo;
    private readonly GitClonePaths _paths;
    
    public LogAndStatusService(GitClonePaths paths)
    {
        _fileIo = FileIO.GetInstance();
        _paths = paths;
    }

    public string Log()
    {
        var fileEntries = _fileIo.GetFilesInDirectory(_paths.CommitsDir);

        var commitInfos = fileEntries
            .Select(CommitInfo)
            .ToArray();

        return string.Join(Environment.NewLine + Environment.NewLine, commitInfos);
        
    }

    public string CommitInfo(string commitObjPath)
    {
        var lines = _fileIo.GetLinesFromFile(commitObjPath, 0, 3);
        return commitObjPath.Split("/").Last() +"\n"+ String.Join("\n", lines); // or \n if you want multi-line per commit
    }


}