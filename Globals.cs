namespace Ellie;

public static class Globals
{
    public static string currentModel = "";
    public static Config cfg = new("", new(), false, "");
    public static List<UserSettings> userSettings = new();
}