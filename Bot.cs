using System.Threading.Channels;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.CommandsNext;
using Ellie.Commands;

namespace Ellie;

public class Bot
{
    public static async Task RunAsync()
    {
        DiscordClient client = new DiscordClient(new DiscordConfiguration()
        {
            Token = Globals.cfg.token,
            TokenType = TokenType.Bot,
            Intents = DiscordIntents.All | DiscordIntents.MessageContents
        });

        CommandsNextConfiguration cconfig = new CommandsNextConfiguration()
        {
            EnableDms = true,
            StringPrefixes = new string[] { "ellie!" },
            DmHelp = false
        };

        DiscordActivity activity = new DiscordActivity();
        activity.ActivityType = ActivityType.Playing;
        activity.Name = "Use -generate to generate images! Use --help to learn more advanced features!";

        CommandsNextExtension cex = client.UseCommandsNext(cconfig);
            
        //cex.RegisterCommands<Basic>();

        client.MessageCreated += Ellie.Events.MessageCreated.Process;

        await client.ConnectAsync();
        await Task.Delay(1000);

        await client.UpdateStatusAsync(activity);
        await Task.Delay(-1);
    }
}