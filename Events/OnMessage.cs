using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using Newtonsoft.Json;

namespace Ellie.Events;

public class MessageCreated
{
    public static async Task Process(DiscordClient cli, MessageCreateEventArgs e)
    {
        foreach(DisabledServer serv in Globals.disabledServers)
        {
            if(e.Guild.Id == serv.id)
            {
                return;
            }
        }

        if(e.Message.Content.Contains("--disable"))
        {
            if(Globals.cfg.adminids.Contains(e.Guild.Id))
            {
                try
                {
                    ulong id = ulong.Parse(e.Message.Content.Split(' ')[1]);
                    
                    DisabledServer nServ = new(id);
                    Globals.disabledServers.Add(nServ);

                    await Globals.WriteDisabled();
                    await e.Message.RespondAsync($"Disabled {id}!");

                    return;
                } catch { return; }
            } else {
                await e.Message.RespondAsync("You do not have permission to do this!");
            }
        }

        if(e.Message.Content.Contains("--enable"))
        {
            if(Globals.cfg.adminids.Contains(e.Guild.Id))
            {
                try
                {
                    ulong id = ulong.Parse(e.Message.Content.Split(' ')[1]);

                    Globals.disabledServers = Globals.disabledServers.Where(x => x.id != id).ToList();

                    await Globals.WriteDisabled();
                    await e.Message.RespondAsync($"Enabled {id}!");

                    return;
                } catch { return; }
            } else {
                await e.Message.RespondAsync("You do not have permission to do this!");
            }
        }

        if(e.Message.Content.Contains("--makeadmin"))
        {
            if(Globals.cfg.adminids.Contains(e.Guild.Id))
            {
                try
                {
                    ulong id = ulong.Parse(e.Message.Content.Split(' ')[1]);
                    
                    Globals.cfg.adminids.Add(id);

                    await Globals.WriteCfg();
                    await e.Message.RespondAsync($"Made {id} admin!");

                    return;
                } catch { return; }
            } else {
                await e.Message.RespondAsync("You do not have permission to do this!");
            }
        }

        if(e.Message.Content.Contains("--revokeadmin"))
        {
            if(Globals.cfg.adminids.Contains(e.Guild.Id))
            {
                try
                {
                    ulong id = ulong.Parse(e.Message.Content.Split(' ')[1]);
                    
                    Globals.cfg.adminids.Remove(id);

                    await Globals.WriteCfg();
                    await e.Message.RespondAsync($"Removed {id} admin privileges!");

                    return;
                } catch { return; }
            } else {
                await e.Message.RespondAsync("You do not have permission to do this!");
            }
        }

        if(e.Message.Content.Contains("--prompt="))
        {
            await Ellie.Commands.AI.PromptAI(e);
        }

        if(e.Message.Content.Contains("--help"))
        {
            DiscordMessageBuilder bldr = new();
            
            DiscordEmbedBuilder ebldr = new DiscordEmbedBuilder();
            ebldr.Title = "Ellie Help Menu";
            ebldr.Description = "This is the help page for Ellie! Values that tell you the default numbers do not need to be specified to generate an image! Meaning you dont need to use them.";
            ebldr.AddField("-generate Enter Text Here!", "This is how you generate a basic image with Ellie!");
            ebldr.AddField("-setnegprompt Enter Text Here", "You can set a static negative prompt using this option");
            ebldr.AddField("--prompt=\"Enter Text Here!\"", "This is where you tell Ellie what you want to generate");
            ebldr.AddField("--negprompt=\"Enter Text Here!\"", "This is where you tell Ellie what should not be included in the image");
            ebldr.AddField("--steps=NUMBER", "This is how many times Ellie goes over the image to make sure it looks real. DEFAULT NUMBER IS 40");
            ebldr.AddField("--cfg=0-30", "This is where you tell Ellie how seriously she should follow your prompt! DEFAULT NUMBER IS 7");
            ebldr.AddField("--x=NUMBER", "This is the width of the image Ellie will generate. DEFAULT NUMBER IS 512");
            ebldr.AddField("--y=NUMBER", "This is the height of the image Ellie will generate. DEFAULT NUMBER IS 512");
            ebldr.AddField("--upscale=true/false", "This tells Ellie if you want to make your image super high quality or not! DEFAULT VALUE IS FALSE");
            ebldr.AddField("--model=MODEL_NAME.ckpt", "This is how you specify a model that Ellie should use (Advanced) DEFAULT IS WHATEVER THE CONFIG SAYS");
            ebldr.AddField("--lora=LORA_NAME.ckpt", "This is how you specify a lora/extension that Ellie should use (Advanced) DEFAULT IS EMPTY");
            ebldr.AddField("Example:", "--prompt=\"photorealistic picture of a huge mountain surrounded by a forest\" --negprompt=\"bad artist, unrealistic, bad quality\"");
            ebldr.AddField("Developer Notes", "Have fun with this bot! If you have any questions feel free to add me on discord: yendisfish (formerly YendisFish#1334, death to pomelo)");
            ebldr.Color = DiscordColor.SpringGreen;

            bldr.WithEmbed(ebldr);

            await e.Message.RespondAsync(bldr);
        }

        if(e.Message.Content.Contains("-generate"))
        {
            UserSettings setting = new(1, "bad artist, bad quality, distorted");
            foreach(UserSettings s in Globals.userSettings)
            {
                if(e.Author.Id == s.id)
                {
                    setting = s;
                }
            }

            await Ellie.Commands.AI.PromptAISimple(e, setting.prompt);
        }

        if(e.Message.Content.Contains("-setnegprompt"))
        {
            UserSettings setting = new(e.Author.Id, e.Message.Content.Split("-setnegprompt")[1].Trim());
            
            for(int i = 0; i < Globals.userSettings.Count; i++)
            {
                if(Globals.userSettings[i].id == setting.id)
                {
                    Globals.userSettings[i] = setting;

                    File.WriteAllText("./usersettings.json", JsonConvert.SerializeObject(Globals.userSettings));
                    return;
                }
            }

            Globals.userSettings.Add(setting);
            File.WriteAllText("./usersettings.json", JsonConvert.SerializeObject(Globals.userSettings));
        }
    }
}