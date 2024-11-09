using System.Text.Json.Serialization;
using System.Text.Json;

namespace Mappix.Data
{
    public struct SettingsProfile(bool deleteSpaces, bool deleteEnters, bool deleteComments, bool deleteRegions, bool renameClasses)
    {
        [JsonPropertyName("DelSpaces")]
        public bool DeleteSpaces { get; set; } = deleteSpaces;
        [JsonPropertyName("DelEnters")]
        public bool DeleteEnters { get; set; } = deleteEnters;
        [JsonPropertyName("DelComments")]
        public bool DeleteComments { get; set; } = deleteComments;
        [JsonPropertyName("DelRegions")]
        public bool DeleteRegions { get; set; } = deleteRegions;
        [JsonPropertyName("RenameClasses")]
        public bool RenameClasses { get; set; } = renameClasses;
        [JsonPropertyName("BuildType")]
        public BuildType BuildType { get; set; }

        public string SerializeProfileJson()
        {
            string json = JsonSerializer.Serialize(this);
            return json;
        }

        public SettingsProfile DeserializeProfileJson(string json)
        {
            SettingsProfile profile = JsonSerializer.Deserialize<SettingsProfile>(json);
            return profile;
        }
        public static SettingsProfile DeserializeProfile(string json)
        {
            SettingsProfile profile = JsonSerializer.Deserialize<SettingsProfile>(json);
            return profile;
        }
    }
}
