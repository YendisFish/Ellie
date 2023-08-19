using System.Diagnostics;
using DSharpPlus;
using Newtonsoft.Json;

namespace Ellie;

class Program
{
    public static async Task Main()
    {
        if(!File.Exists("usersettings.json"))
        {
            using(FileStream fs = File.Create("./usersettings.json")) using(StreamWriter wrt = new(fs))
            {
                wrt.Write(JsonConvert.SerializeObject(new List<UserSettings>()));
            }

            Globals.userSettings = JsonConvert.DeserializeObject<List<UserSettings>>(File.ReadAllText("./usersettings.json")) ?? new();
        } else {
            Globals.userSettings = JsonConvert.DeserializeObject<List<UserSettings>>(File.ReadAllText("./usersettings.json")) ?? new();
        }

        Globals.cfg = JsonConvert.DeserializeObject<Config>(File.ReadAllText("./config.json")) ?? new("", new(), false, "");
        Globals.currentModel = Globals.cfg.defaultmodel;

        await Bot.RunAsync();
    }
}