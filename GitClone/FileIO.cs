using System.Text;

namespace GitClone;

public class FileIO
{
    private static FileIO? Instance; 

    private FileIO()
    {
    }

    public static FileIO GetInstance()
    {
        if (Instance == null)
        {
            Instance = new FileIO();
        }
        return Instance;
    }
    
    public string ReadContent(string filePath)
    {
        try
        {
            using StreamReader reader = new(filePath);

            string text = reader.ReadToEnd();

            return text;
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.ToString());
            throw ex;
        }
    }

    public bool IsHashExistant(string filePath)
    {
        return File.Exists(filePath);
    }

    public void CreateFile(string createPath, string content)
    {
        try
        {
            using (FileStream fs = File.Create(createPath))
            {
                byte[] info = new UTF8Encoding(true).GetBytes(content);
                fs.Write(info, 0, info.Length);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
    }
    
    public void AppendToFile(string filePath, string contentToAdd)
    {
        using var fs = new FileStream(filePath, FileMode.Append);
        using var sw = new StreamWriter(fs);
        sw.WriteLine(contentToAdd);
    }

    public void ReplaceFileLines(string filePath, string[] linesToAdd, int startline)
    {
        var lines = File.ReadAllLines(filePath).ToList();

        if (startline >= 0 && startline < lines.Count)
        {
            for (int i = startline; i < startline+linesToAdd.Length; i++)
            {
                lines[i] = linesToAdd[i - startline];
            }
            File.WriteAllLines(filePath, lines);
        }
        else
        {
            throw new IndexOutOfRangeException("Line index is out of range.");
        }
    }
   
    public void CreateFolder(string path)
    {
        try
        {
            if (Directory.Exists(path))
            {
                Console.WriteLine("Folder already exists");
                return;
            }

            DirectoryInfo di = Directory.CreateDirectory(path);

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            throw ex;
        }
    }

    public int GetLineOfExactMatch(string pathToFile, string contentToMatch)
    {
        try
        {
            var lines = File.ReadAllLines(pathToFile);
            for (int i = 0; i < lines.Length; i++)
            {
                if(lines[i].Contains(contentToMatch))
                {
                    return i;
                }
            }

            return -1;
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.ToString());
            throw ex;
        }
        
    }

    public string[] GetFilesInDirectory(string pathsCommitsDir)
    {
        return Directory.GetFiles(pathsCommitsDir);
    }

    public string[] GetLinesFromFile(string path, int inclusiveStart, int exclusiveEnd)
    {
        string[] lines = File.ReadAllLines(path);
        return lines[new Range(inclusiveStart, exclusiveEnd)];
    }
}