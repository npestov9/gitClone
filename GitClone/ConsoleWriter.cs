namespace GitClone;

public class ConsoleWriter
{
    public void WriteToConsole(string[] lines)
    {
        foreach (var line in lines)
        {
            Console.WriteLine(line);
        }
    }
    
    public void WriteToConsole(string lines)
    {
        Console.WriteLine(lines);
    } 
}