using Mappix.Actions;
using Mappix.Data;
using System.Diagnostics;

namespace Mappix;

public class Program
{
    private string path = "empty";
    private string addPath = @"\MXO\data\";
    public Mappix mappix = new();

    static void Main()
    {
        Program program = new();
        program.Configure();
    }
    private void Configure()
    {
        mappix = new();

        if (File.Exists($@"{Environment.CurrentDirectory}\dat\lastpath.dat"))
            path = FileManipulations.ReadFileContent($@"{Environment.CurrentDirectory}\dat\lastpath.dat").TrimEnd('\r', '\n');

        SettingsProfile sp = ProfilerHelper.GetDefaultProfile() ?? new();
        mappix.settingsProfile = sp;
        mappix.nameBuilderRule.Type = sp.BuildType;

        Console.WriteLine("Programm started");

        Console.Write("\r\n█████████████████████████████████████\r\n█▄─▀█▀─▄██▀▄─██▄─▄▄─█▄─▄▄─█▄─▄█▄─▀─▄█\r\n██─█▄█─███─▀─███─▄▄▄██─▄▄▄██─███▀─▀██\r\n▀▄▄▄▀▄▄▄▀▄▄▀▄▄▀▄▄▄▀▀▀▄▄▄▀▀▀▄▄▄▀▄▄█▄▄▀\r\n\r\n\r\n");

        Console.Write("Author: Ykizakyi Zukio (YZukio)\r\n" +
                      "Github: github.com/Ykizakyi-Zukio\r\n\r\n" +
                      "Supports code languages: C#\r\n" +
                      "Mappix version: v1.6\r\n\r\n");

        StartMenu();
    }

    public void StartMenu()
    {
        Console.Write($"Configuration \n" +
            $"[1]: Using directory         :   {path} \n" +
            $"[2]: Delete spaces           :   {mappix.settingsProfile.DeleteSpaces}  \n" +
            $"[3]: Delete enter symbols    :   {mappix.settingsProfile.DeleteEnters}  \n" +
            $"[4]: Delete comments (//)    :   {mappix.settingsProfile.DeleteComments}\n" +
            $"[5]: Delete regions          :   {mappix.settingsProfile.DeleteRegions} \n" +
            $"[6]: Renmame classes         :   {mappix.settingsProfile.RenameClasses} \n" +
            $"[6.1]: Name building type    :   {mappix.nameBuilderRule.Type} \n");

        Console.Write("\n[-] Name building types: crc32, xxHash128\n");
        Console.WriteLine("To change a parameter use: <ID> <Value> value: 0/1 or (string)\n");
        Console.WriteLine("Profiler: to open profiler-menu <profiler>");
        Console.WriteLine("Start program <0>");
        Console.WriteLine("Close program <close>\r\n");

        string input = Console.ReadLine() ?? " ";
        string[] args = input.Split(" ");
        switch (args[0])
        {
            case " ":
                Console.WriteLine("Invalid arguments!");
                break;
            case "close":
                Environment.Exit(0);
                break;
            case "1":
                if (args.Length >= 2) { path = args[1].TrimStart('\"').TrimEnd('\"'); }
                break;
            case "2":
                if (args.Length >= 2) { mappix.settingsProfile.DeleteSpaces = StringManipulations.StrBool(args[1]); }
                break;
            case "3":
                if (args.Length >= 2) mappix.settingsProfile.DeleteEnters = StringManipulations.StrBool(args[1]);
                break;
            case "4":
                if (args.Length >= 2) mappix.settingsProfile.DeleteComments = StringManipulations.StrBool(args[1]);
                break;
            case "5":
                if (args.Length >= 2) mappix.settingsProfile.DeleteRegions = StringManipulations.StrBool(args[1]);
                break;
            case "6":
                if (args.Length >= 2) mappix.settingsProfile.RenameClasses = StringManipulations.StrBool(args[1]);
                break;
            case "6.1":
                if (args.Length >= 2) { mappix.nameBuilderRule.SetType(args[1]); mappix.nameBuilderRule.SetType(args[1]); }
                break;
            case "0":
                if (Directory.Exists(path) == true) { Console.Clear(); StartObfuscating(); return; }
                else { Console.WriteLine("Directory not exists!"); }
                break;
            case "profiler":
                Console.Clear();
                ProfilerHelper.Profiler(mappix);
                return;
            //break;
            default:
                Console.WriteLine("Parameter not exists.");
                break;
        }

        Console.Clear();
        StartMenu();
    }

    private void StartObfuscating()
    {
        Stopwatch sw = new();

        sw.Start();
        string[] allPathes = Directory.GetFiles(path, "*.cs*", SearchOption.AllDirectories);

        if (allPathes.Length <= 0)
        {
            Console.WriteLine("Invalid directory\n");
            StartMenu();
            return;
        }
        sw.Stop();

        FileMode fm = FileMode.Create;
        Directory.CreateDirectory($@"{Environment.CurrentDirectory}\dat");
        Directory.CreateDirectory($@"{path}{addPath}");
        FileManipulations.CreateFile($@"{Environment.CurrentDirectory}\dat\lastpath.dat", path, fm);

        Console.WriteLine($"Catched {allPathes.Length + 1} files, stealed time is: {sw.Elapsed.TotalMilliseconds}ms / {sw.Elapsed.TotalSeconds}s!");

        string[] datas = new string[allPathes.Length];

        for (int i = 0; allPathes.Length > i; i++)
        {
            datas[i] = (FileManipulations.ReadFileContent(allPathes[i]));
        }

        mappix.nameBuilderRule.Count = 9999999;
        if (mappix.settingsProfile.RenameClasses && mappix.nameBuilderRule.Type != BuildType.Empty)
            datas = CodeMapper.ReplaceIdentifiersWithHashRegex(datas, mappix.nameBuilderRule);

        for (int i = 0; i < allPathes.Length; i++)
        {
            string data = datas[i];
            string file = Path.GetFileName(allPathes[i]);

            sw.Start();
            Console.WriteLine("Starting to mapping a source code");

            mappix.Obfuscate(data);
            FileManipulations.CreateFile($@"{path}\{addPath}\{file}", mappix.totalResult, fm);
            sw.Stop();

            Console.WriteLine($"\rFinished file obfuscating, stealed time is: {sw.Elapsed.TotalMilliseconds}ms / {sw.Elapsed.TotalSeconds}s!");
        }

        Console.WriteLine("Press any key to close.");
        Console.ReadLine();
    }
}