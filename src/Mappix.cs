using Mappix.Actions;
using Mappix.Data;

namespace Mappix
{
    public class Mappix
    {
        public SettingsProfile settingsProfile;
        public CodeMapper codeMapper = new();
        public RegionMapper regionMapper = new();
        public NameBuilderRule nameBuilderRule = new();

        public string totalResult { get; private set; } = "";

        readonly StringManipulations stringManipulations = new();
        readonly StringManipulationsLegacy sml = new();

        public void Obfuscate(string source)
        {
            stringManipulations.Profile = settingsProfile;
            sml.Profile = settingsProfile;

            string noneComments = stringManipulations.DeleteComments(source);
            string noneSpaces = stringManipulations.DeleteSpaces(noneComments);
            string noneEnters = stringManipulations.DeleteEnters(noneSpaces);

            string noneRegions = noneEnters;
            regionMapper.Profile = settingsProfile;

            if (settingsProfile.DeleteRegions)
            {
                noneRegions = RegionMapper.RemoveAllCommentsRegex(noneRegions);
            }

            totalResult = noneRegions;
        }
    }
}
