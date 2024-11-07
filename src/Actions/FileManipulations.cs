namespace Mappix.Actions
{
    public static class FileManipulations
    {
        public static void CreateFile(string path, string content, FileMode fileMode)
        {
            try
            {
                using (FileStream fs = new(path, fileMode, FileAccess.Write))
                {
                    using (StreamWriter writer = new(fs))
                    {
                        writer.WriteLine(content);
                        writer.Close();
                    }

                    fs.Close();
                }

                Console.WriteLine($"File '{path}' created/opened successfully with content.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        public static string ReadFileContent(string path)
        {
            try
            {
                // Read all text from the file
                string content = File.ReadAllText(path);
                return content;
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine($"File not exists: {path}");
                return string.Empty;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return string.Empty;
            }
        }

        public static void DeleteFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                try
                {
                    File.Delete(filePath);
                    Console.WriteLine($"File deleted successfully, from: {filePath}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error deleting file: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine($"File not exists, from: {filePath}");
            }
        }
    }
}
