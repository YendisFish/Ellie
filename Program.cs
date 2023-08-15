using System.Diagnostics;
using DSharpPlus;
using Newtonsoft.Json;

namespace Ellie;

class Program
{
    public static async Task Main()
    {
        Globals.cfg = JsonConvert.DeserializeObject<Config>(File.ReadAllText("./config.json")) ?? new("", new(), false, "");
        Globals.currentModel = Globals.cfg.defaultmodel;

        await Bot.RunAsync();
    }
}