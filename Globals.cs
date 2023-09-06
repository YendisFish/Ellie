using Newtonsoft.Json;

namespace Ellie;

public static class Globals
{
    public static string currentModel = "";
    public static Config cfg = new("", new(), false, "", new());
    public static List<UserSettings> userSettings = new();
    public static List<DisabledServer> disabledServers = new();

    public static async Task WriteDisabled()
    {
        using(FileStream fs = File.Create("./disabledservers.json")) using(StreamWriter wrt = new(fs))
        {
            await wrt.WriteAsync(JsonConvert.SerializeObject(disabledServers));
        }
    }

    public static async Task WriteCfg()
    {
        using(FileStream fs = File.Create("./config.json")) using(StreamWriter wrt = new(fs))
        {
            await wrt.WriteAsync(JsonConvert.SerializeObject(cfg));
        }
    }
}