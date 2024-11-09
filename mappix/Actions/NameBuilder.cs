using System.IO.Hashing;
using System.Text;
using Mappix.Data;

namespace Mappix.Actions
{
    public static class NameBuilder
    {
        public static string BuildName(NameBuilderRule nbr)
        {
            Random rnd = new Random();
            string randomText = rnd.Next(0, nbr.Count).ToString();
            return randomText;
        }

        public static string ReplaceAllRandom(string text, string[] replaces, NameBuilderRule nbr)
        {
            for (int i = 0; i < replaces.Length; i++)
            {
                text = text.Replace(replaces[i], BuildName(nbr));
            }

            return text;
        }
    }

    public static class HashHelper
    {
        public static string GetCrc32Hash(string input)
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            byte[] hashBytes = Crc32.Hash(inputBytes);
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }

        public static string GetXxHash128(string input)
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            byte[] hashBytes = XxHash128.Hash(inputBytes);
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }

        public static string? GetAutoHash(string input, NameBuilderRule nbr = new())
        {
            switch (nbr.Type)
            {
                case BuildType.Random:
                    return NameBuilder.BuildName(nbr);
                case BuildType.Crc32:
                    return GetCrc32Hash(input);
                case BuildType.XxHash128:
                    return GetXxHash128(input);
            }

            return null;
        }
    }
}
