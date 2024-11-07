namespace Mappix.Data
{
    public struct NameBuilderRule(int count = 10, BuildType type = BuildType.Empty)
    {
        public int Count = count;
        public BuildType Type = type;
        public void SetType(string type)
        {
            type = type.Trim();
            try
            {
                Type = (BuildType)Enum.Parse(typeof(BuildType), type);
            }
            catch { Console.WriteLine($"Couldn't to parse a {type} to BuildType"); }
        }
    }

    public enum BuildType
    { 
        Empty = 0,
        Random = 1,
        Crc32 = 2,
        XxHash128 = 3
    }

}
