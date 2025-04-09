namespace GitClone;

public class FileGenService
{
    private HashGenerator _hashGenerator;
    private FileIO _fileIo;
    private StagingManager _stagingManager;
    private GitClonePaths _gitClonePaths;
    
    public FileGenService(GitClonePaths gitClonePaths)
    {
        _hashGenerator = new HashGenerator();
        _fileIo = FileIO.GetInstance();
        _gitClonePaths = gitClonePaths;
        _stagingManager = new StagingManager(gitClonePaths);
    }

    public string InitialiseFolders()
    {
        try
        {
            //file initialisation
            _fileIo.CreateFolder(_gitClonePaths.RepoDir);
            _fileIo.CreateFolder(_gitClonePaths.ObjectDir);
            _fileIo.CreateFolder(_gitClonePaths.CommitsDir);
            _fileIo.CreateFolder(_gitClonePaths.BlobsDir);
            //index file 
            _fileIo.CreateFile( _gitClonePaths.IndexFile, "");
            return _gitClonePaths.RepoDir;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            throw ex;
        }
    }
    
    public string GenBlob(string localPathToFile)
    {
        try
        {
            string globalPathToFile = Path.Combine(_gitClonePaths.BaseDir, localPathToFile);
            string fileContent = _fileIo.ReadContent(globalPathToFile);
            string hash = _hashGenerator.GenHash("blob", fileContent);
            string pathToWriteTo = Path.Combine(_gitClonePaths.BlobsDir, hash); 
            bool isAlreadyCreated = _fileIo.IsHashExistant(pathToWriteTo);
            if (isAlreadyCreated)
            {
                return pathToWriteTo;
            }
            //creating the blob file
            _fileIo.CreateFile(pathToWriteTo, fileContent);
            
            //updating index
            _stagingManager.AddOrUpdateBlob(globalPathToFile, hash);

            return pathToWriteTo;
        }
        catch(Exception ex)
        {
            Console.WriteLine("Something went wrong: " + localPathToFile + " " +ex.ToString());
            throw;
        }
    }

    public string GenCommit(string message)
    {
        string indexContent = _fileIo.ReadContent(_gitClonePaths.IndexFile);
        string fileContent = message +"\n"+ DateTime.Now + "\n" + "Nikita\n\n" + indexContent;

        string hash = _hashGenerator.GenHash("commit", fileContent);
        string pathToCreate = Path.Combine(_gitClonePaths.CommitsDir, hash);
        _fileIo.CreateFile(pathToCreate, fileContent);

        return pathToCreate;

    }
}