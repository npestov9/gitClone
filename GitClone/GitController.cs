namespace GitClone;

public class GitController
{
    private HashGenerator _hashGenerator;
    private FileGenService _fileGenService;
    private readonly GitClonePaths _gitClonePaths;
    private LogAndStatusService _logAndStatusService;
    private ConsoleWriter _consoleWriter;

    public GitController(GitClonePaths gitClonePaths)
    {
        _gitClonePaths = gitClonePaths;
        _hashGenerator = new HashGenerator();
        _fileGenService = new FileGenService(gitClonePaths);
        _logAndStatusService = new LogAndStatusService(gitClonePaths);
        _consoleWriter = new ConsoleWriter();
    }
    
    public string Init()
    {
        return _fileGenService.InitialiseFolders();
    }

    public string Add(string pathToFile)
    {
        return _fileGenService.GenBlob(pathToFile);
    }

    public string Commit(string message)
    {
        return _fileGenService.GenCommit(message);
    }

    public string Log()
    {
        string logOutput =  _logAndStatusService.Log();
        _consoleWriter.WriteToConsole(logOutput);
        return logOutput;
    }
}