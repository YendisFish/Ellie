using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using Ellie;

namespace Ellie.API;

public class Prompt
{
    public Dictionary<string, object> request { get; set; } = new Dictionary<string, object>();

    public Prompt()
    {
        request = new();
    }

    public async Task<string[]?> SendAndGet(string prompt, string negative, int steps = 40, int cfg = 7, int x = 512, int y = 512, bool upscale = false, string model = "", string lora = "")
    {
        Globals.currentModel = model;
        Console.WriteLine($"Model is: {Globals.currentModel}");

        HttpClient cli = new();
        
        Dictionary<string, object> options = new Dictionary<string, object>();
        options.Add("sd_model_checkpoint", model);

        if(lora is not "")
        {
            prompt = $"<lora:{lora}:1> {prompt}";
            Console.WriteLine(prompt);
        }

        request.Add("prompt", prompt);
        request.Add("steps", steps);
        request.Add("cfg_scale", cfg);
        request.Add("save_images", true);
        request.Add("restore_faces", true);
        request.Add("override_settings", options);
        request.Add("width", x);
        request.Add("height", y);

        if(upscale)
        {
            request.Add("enable_hr", true);
            request.Add("denoising_strength", 0.7);
            request.Add("hr_upscaler", "Latent");
            request.Add("hr_scale", 2);
            request.Add("hr_resize_x", 1024);
            request.Add("hr_resize_y", 1024);
        }

        cli.Timeout = new TimeSpan(0, 5, 0);
        HttpResponseMessage msg = await cli.PostAsJsonAsync("http://127.0.0.1:7860/sdapi/v1/txt2img", request);

        string cur = DateTime.Now.ToString("yyyy-MM-dd");

        JObject obj = JObject.Parse(await msg.Content.ReadAsStringAsync());
        JToken tok = obj["info"];

        JObject obj2 = JObject.Parse((string)tok);
        JArray allseeds = obj2["all_seeds"] as JArray ?? new JArray();

        FileInfo[] path = new DirectoryInfo($"C:/Users/{Environment.UserName}/stable-diffusion-webui/outputs/txt2img-images/{cur}/").GetFiles();

        foreach(FileInfo inf in path)
        {
            if(inf.FullName.Contains((string)allseeds[0]))
            {
                return new string[] { inf.FullName };
            }
        }

        return null;
    }
}