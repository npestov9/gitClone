namespace UnitTests;

public class TestFileHelper
{
    public static string CreateFile(string dir, string name, string content)
    {
        var path = Path.Combine(dir, name);
        Directory.CreateDirectory(Path.GetDirectoryName(path));
        File.WriteAllText(path, content);
        return path;
    }


    public static void AddToFile(string filePath, string content)
    {
        using var fs = new FileStream(filePath, FileMode.Append);
        using var sw = new StreamWriter(fs);
        sw.WriteLine(content);
    }
    
}