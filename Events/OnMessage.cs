using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using Newtonsoft.Json;

namespace Ellie.Events;

public class MessageCreated
{
    public static async Task Process(DiscordClient cli, MessageCreateEventArgs e)
    {
        if(e.Message.Content.Contains("--prompt"))
        {
            await Ellie.Commands.AI.PromptAI(e);
        }

        if(e.Message.Content.Contains("--help"))
        {
            DiscordMessageBuilder bldr = new();
            
            DiscordEmbedBuilder ebldr = new DiscordEmbedBuilder();
            ebldr.Title = "Ellie Help Menu";
            ebldr.Description = "This is the help page for Ellie! Values that tell you the default numbers do not need to be specified to generate an image! Meaning you dont need to use them.";
            ebldr.AddField("--prompt=\"Enter Text Here!\"", "This is where you tell Ellie what you want to generate");
            ebldr.AddField("--negprompt=\"Enter Text Here!\"", "This is where you tell Ellie what should not be included in the image");
            ebldr.AddField("--steps=NUMBER", "This is how many times Ellie goes over the image to make sure it looks real. DEFAULT NUMBER IS 40");
            ebldr.AddField("--cfg=0-15", "This is where you tell Ellie how seriously she should follow your prompt! DEFAULT NUMBER IS 7");
            ebldr.AddField("--x=NUMBER", "This is the width of the image Ellie will generate. DEFAULT NUMBER IS 512");
            ebldr.AddField("--y=NUMBER", "This is the height of the image Ellie will generate. DEFAULT NUMBER IS 512");
            ebldr.AddField("--upscale=true/false", "This tells Ellie if you want to make your image super high quality or not! DEFAULT VALUE IS FALSE");
            ebldr.AddField("--model=MODEL_NAME.ckpt", "This is how you specify a model that Ellie should use (Advanced) DEFAULT IS WHATEVER THE CONFIG SAYS");
            ebldr.AddField("--lora=LORA_NAME.ckpt", "This is how you specify a lora/extension that Ellie should use (Advanced) DEFAULT IS EMPTY");
            ebldr.AddField("Example:", "--prompt=\"photorealistic picture of a huge mountain surrounded by a forest\" --negprompt=\"bad artist, unrealistic, bad quality\"");
            ebldr.Color = DiscordColor.SpringGreen;

            bldr.WithEmbed(ebldr);

            await e.Message.RespondAsync(bldr);
        }
    }
}