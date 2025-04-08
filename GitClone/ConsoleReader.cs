namespace GitClone;

public class ConsoleReader
{
    public void ReadLine(string[] args)
    {
        GitClonePaths gitClonePaths = new GitClonePaths(Environment.CurrentDirectory);
        GitController controller = new GitController(gitClonePaths);

        string command = args[0];

        switch (command)
        {
            case "init":
                controller.Init();
                break;
            case "add":
                string fileName = args[1];
                controller.Add(fileName);
                break;
            case "commit":
                string message = args[1];
                message.Replace("\"", "");
                controller.Commit(message);
                break;
        }
    }
}