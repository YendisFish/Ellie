using System.Diagnostics;
using DSharpPlus;
using Newtonsoft.Json;

namespace Ellie;

class Program
{
    public static async Task Main()
    {
        if(!File.Exists("./usersettings.json"))
        {
            using(FileStream fs = File.Create("./usersettings.json")) using(StreamWriter wrt = new(fs))
            {
                wrt.Write(JsonConvert.SerializeObject(new List<UserSettings>()));
            }

            Globals.userSettings = JsonConvert.DeserializeObject<List<UserSettings>>(File.ReadAllText("./usersettings.json")) ?? new();
        } else {
            Globals.userSettings = JsonConvert.DeserializeObject<List<UserSettings>>(File.ReadAllText("./usersettings.json")) ?? new();
        }

        if(!File.Exists("./disabledservers.json"))
        {
            using(FileStream fs = File.Create("./disabledservers.json")) using(StreamWriter wrt = new(fs))
            {
                wrt.Write(JsonConvert.SerializeObject(new List<DisabledServer>()));
            }
        } else {
            Globals.disabledServers = JsonConvert.DeserializeObject<List<DisabledServer>>(File.ReadAllText("./disabledservers.json")) ?? new();
        }

        Globals.cfg = JsonConvert.DeserializeObject<Config>(File.ReadAllText("./config.json")) ?? new("", new(), false, "", new());
        Globals.currentModel = Globals.cfg.defaultmodel;

        await Bot.RunAsync();
    }
}