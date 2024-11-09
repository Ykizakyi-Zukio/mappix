using Mappix.Data;

namespace Mappix.Actions;

internal class StringManipulationsLegacy
{
    public SettingsProfile Profile { get; set; }
    public char es = '\r';

    public string DeleteSpacesLegacy(string text)
    {
        var strokes = text.Split(es);

        if (!Profile.DeleteSpaces)
            return text;

        for (int i = 0; i < strokes.Length; i++)
        {
            strokes[i] = strokes[i].Trim();
            strokes[i] = strokes[i].TrimStart('\t');
        }

        var result = string.Join("", strokes);
        return result;
    }


    public string DeleteEntersLegacy(string text)
    {
        string[] strokes = text.Split(es);

        if (!Profile.DeleteEnters)
            return text;

        for (int i = 0; i < strokes.Length; i++)
        {
            strokes[i] = strokes[i].TrimEnd('\r');
            strokes[i] = strokes[i].TrimEnd('\n');
            strokes[i] = strokes[i].TrimEnd('\v');
            Console.WriteLine($"Delete new line end symbols deleted at {i}!");
        }

        Console.WriteLine("Finished to deleting new line end symbols!");
        text = string.Join("", strokes);
        return text;
    }

    public string DeleteCommentsLegacy(string text)
    {
        string[] splitted = text.Split(es);

        if (!Profile.DeleteComments)
            return text;

        for (int i = 0; i < splitted.Length; i++)
        {
            int commentIndex = splitted[i].IndexOf("//");

            if (commentIndex > -1)
            {
                splitted[i] = splitted[i].Remove(commentIndex);
                Console.WriteLine($"\r Removing comment at: {commentIndex}");
            }
        }

        return string.Join("\r", splitted);
    }
}
