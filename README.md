# Ellie Bot

This is a discord bot that uses the AUTOMATIC1111 stable-diffusion-webui API
to connect discord users with a locally run instance of stable diffusion! You
will need a number of dependencies for this project!

### Programs you will need installed
- dotnet 7.0 (Check the Ellie.csproj file for an entirely accurate dotnet version)
- Git
- Python
- stable-diffusion-webui

### Running the program

While I plan to make a binary release of this program, I have not yet done so. However
It is very easy to run this program! You simply have to clone it in your command line
like so:

```
git clone https://github.com/YendisFish/Ellie
```

After doing this navigate to the ``Ellie`` directory that git just created on your
machine. Now you will need to make a config file in this directory called config.json.
config.json must include this text:

```json
{
    "token": "Your Discord Bot Token",
    "blacklist": [ "Put Blacklisted Keywords Here" ],
    "useblacklist": true,
    "defaultmodel": "Name Of The Model You Want To Be Loaded By Default"
}
```

After you have configured this you can get Ellie running with:

```
dotnet run
```

You are now ready for the next step!

### Running AUTOMATIC1111s stable-diffusion-webui

First you will need to clone the stable-diffusion-webui repository from
AUTOMATIC1111:

<a href="https://github.com/AUTOMATIC1111/stable-diffusion-webui">stable-diffusion-webui</a>

Go ahead and follow the instructions on how to run this! It should be very
easy! Just make sure you are running with the ``--api`` parameter. After this
you should be ready to use Ellie!

# How to operate Ellie

Ellie includes a help command! just type "--help" into your discord chat
and Ellie will respond with a help message! There is no prefix for her
or anything like that.