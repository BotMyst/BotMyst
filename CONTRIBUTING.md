# Contributing

This guide will help you with setting up BotMyst in your local environment and running it. It's meant to be run in a command line, but everything should be simple enough to do with GUI apps without much difficulty. If a guide for GUI applications is needed please open a new issue.

## Creating a Discord app

Before even building the project you need to have a Discord app.

Open the Discord [developer portal](https://discordapp.com/developers/applications/) and create a new application.

Go to the OAuth2 section and add a new redirect which should point to `http://localhost:5000/signin-discord`. This is used for logging into the BotMyst website using Discord. That url points to your local instance of BotMyst, if you're running it on another port or even a public website you shouldn't forget to change that as you won't be able to log in.

## Required dependencies

All you need to have installed to run BotMyst is .Net Core 2.1. You can get it from here: https://dotnet.github.io/

## Clone the repository

Clone the repository (make sure to clone the [v2 branch](https://github.com/CodeMyst/BotMyst/tree/v2)).

```
git clone https://github.com/CodeMyst/BotMyst.git --branch v2
```

or if you're using ssh:
```
git clone git@github.com:CodeMyst/BotMyst.git --branch v2
```

## Building

To build the project, go to the root directory of BotMyst and run `dotnet build`

```
cd BotMyst
dotnet build
```

## Configuring the settings

Both `BotMyst.Web` and `BotMyst.Bot` have their own settings that need to be configured.

### Configuring `BotMyst.Web`

Create an `appsettings.json` file in the `BotMyst/BotMyst.Web` directory and paste this in:

```json
{
    "Jwt":
    {
        "Key": "",
        "Issuer": "http://localhost:5000/"
    },
    "Discord":
    {
        "ClientId": "",
        "ClientSecret": "",
        "BotToken": ""
    }
}
```

The `Jwt` object is used for setting up the BotMyst website API. The `Key` field is a password of sorts, it will use that key to generate an API key you can use so `BotMyst.Bot` can access the API. The `Issuer` field should be set to the address of the website, since this is a local instance it's set to `localhost`.

The `Discord` object is used for storing the credentials of your [Discord application](#creating-a-discord-app). All fields here are self explanatory.

After this open the [`Startup`](BotMyst.Web/Startup.cs) file and un-comment these lines in the `Configure` method:

```csharp
var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]));
var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

var token = new JwtSecurityToken(Configuration["Jwt:Issuer"],
Configuration["Jwt:Issuer"],
expires: DateTime.Now.AddDays(30),
signingCredentials: creds);

System.Console.WriteLine(new JwtSecurityTokenHandler ().WriteToken (token));
```

That will generate a new API key which you'll need in just a second.

*NOTE: The process of generating an API key isn't the most pleasant. It's being worked on.

### Configuring `BotMyst.Bot`

Create an `appsettings.json` file in the `BotMyst/BotMyst.Bot` directory and paste this in:

```json
{
    "prefix": ">",
    "discordToken": "",
    "BotMystApiUrl": "http://localhost:5000/api/",
    "BotMystApiToken": ""
}
```

The `prefix` field is the bot prefix for running commands. `discordToken` is the bot token key from your Discord Application Bot. `BotMystApiUrl` is the url to the website's api, this is the address with the `/api/` sub route. `BotMystApiToken` is the token you generated earlier.

## Running database migrations

BotMyst uses SQlite for databases. You need to run database migrations.

```
cd BotMyst.Web
dotnet ef migrations add Migration1 --context ModulesContext
dotnet ef migrations add Migration1 --context ModuleOptionsContext
dotnet ef database update --context ModulesContext
dotnet ef database update --context ModuleOptionsContext
```

## Running BotMyst

Now you should be all set to run BotMyst. You should first launch `BotMyst.Web` then `BotMyst.Bot`.

```
cd BotMyst.Web
dotnet run
```
```
cd BotMyst.Bot
dotnet run
```
