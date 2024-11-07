using Mappix.Data;
using System.IO.Hashing;
using System.Text;
using System.Text.RegularExpressions;

namespace Mappix.Actions
{
    public class CodeMapper
    {
        public static string? FindWordAfter(string text, string word)
        {
            string escapedWord = Regex.Escape(word);
            string pattern = $@"\b{escapedWord}\b\s+(\w+)";

            Match match = Regex.Match(text, pattern, RegexOptions.IgnoreCase);

            if (match.Success)
            {
                return match.Groups[1].Value;
            }

            return null;
        }

        public static List<string> FindAllWordsAfter(string text, string word)
        {
            List<string> wordsAfter = [];
            string escapedWord = Regex.Escape(word);
            string pattern = $@"\b{escapedWord}\b\s+(\w+)";

            MatchCollection matches = Regex.Matches(text, pattern, RegexOptions.IgnoreCase);

            foreach (Match match in matches)
            {
                wordsAfter.Add(match.Groups[1].Value);
            }

            return wordsAfter;
        }

        public static List<string?> GetAllClassesNames(string text)
        {
            List<string?> classesNames = [];
            string? word = FindWordAfter(text, "class");

            while (word != null)
            {
                word = FindWordAfter(text, "class");

                if (word != null) 
                    classesNames.Add(word);
            }

            return classesNames;
        }

        public static List<string> GetAllClassesNamesLegacy(string text)
        {
            string[] split = text.Split("\r");
            List<string> classes = [];
            
            for (int i = 0; i < split.Length; i++)
            {
                string[] words = split[i].Split(" ");

                for (int j = 0; j < words.Length; j++)
                {
                    if (words[j] == "class" && j + 1 <= words.Length)
                    {
                        int borderIndex = words[j + 1].IndexOf('{');

                        if (borderIndex > -1)
                            words[j + 1] = words[j + 1].Remove(borderIndex);

                        classes.Add(words[j + 1]);
                    }
                }
            }

            return classes;
        }

        public static List<string> FindVariables(string text)
        {
            //Supports Unity Data Types
            string pattern = @"\b(int|float|double|string|bool|char|DateTime|var|Vector3|Vector2|Color)\s+(\w+)\s*(=.+?)?;";
            
            MatchCollection matches = Regex.Matches(text, pattern);
            List<string> variables = [];

            foreach (Match match in matches)
            {
                string variableName = match.Groups[2].Value;
                variables.Add(variableName);
            }

            return variables;
        }

        public static List<string> FindMethods(string text)
        {
            //Supports Unity Functions / Methods
            string pattern = @"\b(public|private|protected|internal|static|virtual|override)?\s*(\w+)\s+(\w+)\s*\(.*?\)\s*{";
            MatchCollection matches = Regex.Matches(text, pattern);

            List<string> methodNames = new();

            foreach (Match match in matches)
            {
                string methodName = match.Groups[3].Value;
                methodNames.Add(methodName);
            }

            return methodNames;
        }

        public static string[] ReplaceIdentifiersWithHash(string[] lines, NameBuilderRule nbr)
        {
            string[] keywords = { "class", "struct", "enum", "interface" };
            Dictionary<string, string> identifierHashMap = [];

            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];

                foreach (var keyword in keywords)
                {
                    int index = line.IndexOf(keyword);
                    if (index != -1)
                    {
                        // Находим начало имени после ключевого слова
                        int startIndex = index + keyword.Length + 1;
                        if (startIndex < line.Length)
                        {
                            // Извлекаем имя после ключевого слова
                            int endIndex = line.IndexOf(' ', startIndex);
                            if (endIndex == -1) endIndex = line.Length;

                            string identifier = line[startIndex..endIndex];

                            // Проверяем, есть ли идентификатор в словаре, иначе добавляем
                            if (!identifierHashMap.ContainsKey(identifier))
                            {
                                string hashedIdentifier = HashHelper.GetAutoHash(identifier, nbr);
                                identifierHashMap[identifier] = hashedIdentifier;
                            }

                            // Заменяем идентификатор на его хэш
                            line = line.Replace(identifier.TrimEnd('{').TrimEnd("{}".ToCharArray()).Replace(".", ""), identifierHashMap[identifier]);
                            lines[i] = line;
                        }
                        break;
                    }
                }
            }

            // Заменяем идентификаторы на их хэши в остальных строках
            for (int i = 0; i < lines.Length; i++)
            {
                foreach (var kvp in identifierHashMap)
                {
                    if (lines[i].Contains(kvp.Key))
                    {
                        lines[i] = lines[i].Replace(kvp.Key.TrimEnd('{').TrimEnd("{}".ToCharArray()).Replace(".", ""), kvp.Value);
                    }
                }
            }

            return lines;
        }

        public static string[] ReplaceIdentifiersWithHashRegex(string[] lines, NameBuilderRule nbr)
        {
            // Регулярное выражение для поиска ключевых слов и идентификаторов
            string pattern = @"\b(class|struct|enum|interface)\s+(\w+)\b";
            Dictionary<string, string> identifierHashMap = [];

            // Первый проход: находим все идентификаторы и создаем их хэши
            for (int i = 0; i < lines.Length; i++)
            {
                var matches = Regex.Matches(lines[i], pattern);
                foreach (Match match in matches)
                {
                    string identifier = match.Groups[2].Value;

                    // Если идентификатор еще не в словаре, добавляем его с хэшем
                    if (!identifierHashMap.ContainsKey(identifier))
                    {
                        identifierHashMap[identifier] = HashHelper.GetAutoHash(identifier, nbr);
                    }

                    // Заменяем идентификатор на хэш в этой строке
                    lines[i] = lines[i].Replace(identifier, identifierHashMap[identifier]);
                }
            }

            // Второй проход: Заменяем идентификаторы на хэши во всех контекстах (включая . и {})
            for (int i = 0; i < lines.Length; i++)
            {
                foreach (var kvp in identifierHashMap)
                {
                    // Заменяем каждое вхождение идентификатора, даже если он перед . или {
                    lines[i] = Regex.Replace(lines[i], $@"\b{Regex.Escape(kvp.Key)}\b", kvp.Value);
                }
            }

            return lines;
        }
    }
}
