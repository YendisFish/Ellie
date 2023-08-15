using DSharpPlus.Entities;
using DSharpPlus.CommandsNext;
using DSharpPlus;
using DSharpPlus.CommandsNext.Attributes;
using Ellie.API;
using System.Net.Http.Headers;
using System.ComponentModel;
using DSharpPlus.EventArgs;
using System.IO;
using System.Linq.Expressions;

namespace Ellie.Commands;

public class AI
{
    public static async Task PromptAI(MessageCreateEventArgs ctx)
    {
        //object[] blacklist = new object[] {"porn", "nsfw", "boob", "ass", "tits", "pussy", "vagina", "cock", "dick", "penis", "nude", "naked", "bottomless", "topless", "breast", "furry", "daddy", "mommy", "sexy", "cum", "orgasm", "semen", "baddie", "hottie", "nudity", "panties", "nipple", "titties", "butt", "bum", "pp", "gwa", "sex", "fucking", "coitus", "intercourse", "bust", "coom", "large member", "large bust", "nutted on", "nut", "white liquid", "sperm", "ejaculate", "testicle", "testies", "testys", "nips", "teet", "loli"};
        //bool filterdms = true;
        /*
        How to blacklist Matthew:
        if(ctx.Author.Id == 380526092634816542)
        {
            Console.WriteLine("Matthew tried to generate an image");
            return;
        }*/

        Console.WriteLine("\nGenerating Image!");
        string args = ctx.Message.Content;

        object[] cmdargs = new object[] { "", "", 40, 7, 512, 512, false, Globals.currentModel, "" };

        cmdargs[0] = args.Split("--prompt=\"")[1].Split("\"")[0];
        cmdargs[1] = args.Split("--negprompt=\"")[1].Split("\"")[0];
        
        Console.WriteLine(cmdargs[0]);

        string time = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
        string user = ctx.Author.Id.ToString() + " " + ctx.Author.Username;
        string output = "[" + time + "] " + user + "\n" + cmdargs[0] + " | " + cmdargs[1];


        if(!ctx.Channel.IsPrivate)
        {
            if(!Filter.IsSafe((string)cmdargs[0]) && Globals.cfg.useblacklist)
            {
                await ctx.Message.RespondAsync($"Your request was blocked for using blacklisted words!");
                return;
            }
        }
        
        try
        {
            using(FileStream fs = File.OpenWrite("./log.txt")) using(StreamWriter sw = new StreamWriter(fs))
            {
                sw.WriteLine(output + "\n");
            }
        }catch(Exception e)
        {
            Console.WriteLine("Exception: " + e.Message);
        }

        Console.WriteLine(output);
        
        try
        {
            foreach(string val in args.Split(" "))
            {
                if(val.Contains("--steps="))
                {
                    cmdargs[2] = int.Parse(val.Replace("--steps=", ""));
                }

                if(val.Contains("--cfg="))
                {
                    cmdargs[3] = int.Parse(val.Replace("--cfg=", ""));
                }

                if(val.Contains("--x="))
                {
                    cmdargs[4] = int.Parse(val.Replace("--x=", ""));
                }

                if(val.Contains("--y="))
                {
                    cmdargs[5] = int.Parse(val.Replace("--y=", ""));
                }

                if(val.Contains("--upscale="))
                {
                    cmdargs[6] = Convert.ToBoolean(val.Replace("--upscale=", ""));
                }

                if(val.Contains("--model="))
                {
                    cmdargs[7] = val.Replace("--model=", "");
                }

                if(val.Contains("--lora="))
                {
                    cmdargs[8] = val.Replace("--lora=", "");
                }
            }

            Prompt p = new();
            string[]? img = await p.SendAndGet((string)cmdargs[0], (string)cmdargs[1], (int)cmdargs[2], (int)cmdargs[3], (int)cmdargs[4], (int)cmdargs[5], Convert.ToBoolean(cmdargs[6]), (string)cmdargs[7], (string)cmdargs[8]);

            if(img is null) {Console.ForegroundColor = ConsoleColor.Red; Console.WriteLine("Could not generate image!"); Console.ForegroundColor = ConsoleColor.White; throw new Exception("Could not generate image!"); }

            using(FileStream fs = File.OpenRead(img[0]))
            {
                DiscordMessageBuilder bldr = new DiscordMessageBuilder().AddFile(fs);
                await ctx.Message.RespondAsync(bldr);
            }
            
            //File.Delete(img[0]);            Delete image from drive
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Image sent!");
            Console.ForegroundColor = ConsoleColor.White;
        } catch(Exception e)
        {
            await ctx.Message.RespondAsync("Failed to generate image!");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Failed to generate image!");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(e.Message);
        }
        
    }
}