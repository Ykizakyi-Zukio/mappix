using Mappix.Actions;
using Mappix.Data;

namespace Mappix
{
    public static class ProfilerHelper
    {
       public static void Profiler(Mappix mappix)
        {
            Console.Write("Profiler\n" +
                "[1] Create profile      <ID>\n" +
                "[2] Set default profile <ID>\n" +
                "[3] Delete profile      <ID>\n" +
                "[4] All profiles");
            Console.WriteLine("\nBack <0>");

            string input = Console.ReadLine() ?? " ";
            string[] args = input.Split(" ");

            switch (args[0])
            {
                case "1":
                    if (args[1] != " ")
                    {
                        string json = mappix.settingsProfile.SerializeProfileJson();
                        FileMode fm = FileMode.Create;

                        Directory.CreateDirectory($@"{Environment.CurrentDirectory}\dat\profiles\");
                        FileManipulations.CreateFile($@"{Environment.CurrentDirectory}\dat\profiles\{args[1]}.json", json, fm);

                        Console.WriteLine($"\nProfile {args[1]} created!");

                        Console.Clear();
                        Program pro = new();
                        pro.mappix = mappix;
                        pro.StartMenu();
                        return;
                    }
                    break;
                case "2":
                    if (args[1] != " ")
                    {
                        try
                        {
                            mappix.settingsProfile = mappix.settingsProfile.DeserializeProfileJson(FileManipulations.ReadFileContent($@"{Environment.CurrentDirectory}\dat\profiles\{args[1]}.json"));
                            string json = mappix.settingsProfile.SerializeProfileJson();
                            FileMode fm = FileMode.Create;

                            Directory.CreateDirectory($@"{Environment.CurrentDirectory}\dat\profiles\");
                            FileManipulations.CreateFile($@"{Environment.CurrentDirectory}\dat\profiles\defaultProfile.json", json, fm);

                            Console.Clear();
                            Program prm = new()
                            {
                                mappix = mappix
                            };
                            prm.StartMenu();
                            return;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Couldn't to set this profile default: {ex}");
                        }
                    }
                    break;
                case "3":
                    if (args[1] != " ")
                    {
                        FileManipulations.DeleteFile($@"{Environment.CurrentDirectory}\dat\profiles\{args[1]}.json");
                    }
                    break;
                case "4":
                    if (Directory.Exists($@"{Environment.CurrentDirectory}\dat\profiles"))
                    {
                        string[] profiles = Directory.GetFiles($@"{Environment.CurrentDirectory}\dat\profiles", "*.json*", SearchOption.TopDirectoryOnly);
                        if (profiles.Length > 0)
                        {
                            for (int i = 0; i < profiles.Length; i++)
                            {
                                Console.WriteLine(Path.GetFileName(profiles[i]));
                            }

                            Console.ReadLine();
                        }
                    }
                    break;
                case "0":
                    Console.Clear();
                    Program pr = new()
                    {
                        mappix = mappix
                    };
                    pr.StartMenu();
                    return;
            }

            Console.Clear();
            Profiler(mappix);
        }

        public static SettingsProfile? GetDefaultProfile()
        {
            try
            {
                if (File.Exists($@"{Environment.CurrentDirectory}\dat\profiles\defaultProfile.json"))
                {
                    SettingsProfile sp = new();
                    return sp.DeserializeProfileJson(FileManipulations.ReadFileContent($@"{Environment.CurrentDirectory}\dat\profiles\defaultProfile.json"));
                }
                else
                    throw new Exception("File not exists");
            }
            catch
            {
                return null;
            }
        }
    }
}
