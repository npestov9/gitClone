namespace GitClone;

public class StagingManager
{
    private FileIO _fileIo;
    private readonly GitClonePaths _gitClonePaths;
    
    public StagingManager(GitClonePaths gitClonePaths)
    {
        _fileIo = FileIO.GetInstance();
        _gitClonePaths = gitClonePaths;
    }
    
    public void AddOrUpdateBlob(string pathToFile, string hash)
    {   
        int startLine = GetStartLine(pathToFile, hash);
        string contentForIndex = pathToFile + "\n" + hash;
        if (startLine == -1)
        {
            _fileIo.AppendToFile(_gitClonePaths.IndexFile,contentForIndex); 
        }
        else
        {
            _fileIo.ReplaceFileLines(_gitClonePaths.IndexFile, contentForIndex.Split("\n"), startLine);
        }
    }
    
    //will check weather file already exists in index, if it does returns that line
    //If it does not then return the end line
    private int GetStartLine(string pathToFile, string hash)
    {
        return _fileIo.GetLineOfExactMatch(pathToFile, hash);
    }
}