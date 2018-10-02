# Building

In this article you will learn how to set up BotMyst, build it, and get it running.

BotMyst is cross-platform, meaning you can develop and run it on any platform that .Net Core supports (so far only tested on Linux and Windows).

## Requirements

* Git - [https://git-scm.com/downloads](https://git-scm.com/downloads)
* .Net Core - [https://www.microsoft.com/net/download](https://www.microsoft.com/net/download)

## Project structure

It's all pretty self explanatory.

* BotMyst.Bot - The bot application, handles the Discord side of things.
* BotMyst.Web - The website and data storage of all the options.
* BotMyst.Shared - Shared data that is needed by other two projects, mostly contains models.

## Creating the Discord app

Open the Discord [developer portal](https://discordapp.com/developers/applications/) and create a new application.

Go to the OAuth2 section and add a new redirect which should point to `http://localhost:5000/discord`. This is used for logging into the BotMyst website using Discord. That url points to your local instance of BotMyst, if you're running it on another port or even a public website you shouldn't forget to change that as you won't be able to log in. By default the project runs on the `5000` port for HTTP and `5001` for HTTPS, so if you want you can also use the HTTPS port like so: `https://localhost:5001/discord`.

You also need to enable the bot user in your Discord app so you can get a bot token

## Building the project

```sh
git clone https://github.com/CodeMyst/BotMyst.git
cd BotMyst
dotnet build
```

## Setting up the project

### BotMyst.Web

Create the `appsettings.json` file inside the `BotMyst.Web` folder. It should contain these values:

```json
{
    "ConnectionStrings":
    {
        "ModuleDescriptions": "Data Source=Databases/ModuleDescriptions.db"
    },
    "Auth0":
    {
        "Domain": "",
        "ApiIdentifier": "https://localhost:5001/api"
    },
    "Discord":
    {
        "ClientId": "",
        "ClientSecret": "",
        "BotToken": ""
    }
}
```

* The `ModuleDescriptions` key shouldn't be changed, it's the location to the SQLite database.
* The `Auth0`'s `Domain` object will be discussed later in the [API Authentication](#api-authentication) section.
* The `ApiIndentifier` key under the `Auth0` object should have the endpoint to the API, this is the address on which the `BotMyst.Web` project is running + `/api` added to the end.
* Under the `Discord` object you need to specify the values which you can get from your Discord app, which you can created earlier in the [Creating the Discord app](#creating-the-discord-app) section.

### BotMyst.Bot

Create the `appsettings.json` file inside the `BotMyst.Bot` folder. It should contain these values:

```json
{
    "Discord":
    {
        "Token": ""
    },
    "Bot":
    {
        "Prefix": ">"
    },
    "BotMystApi":
    {
        "AccessToken": ""
    }
}
```

* Under the `Discord` object you need to specify the Bot `Token` you created eariler in the [Creating the Discord app](#creating-the-discord-app) section.
* The `Bot` `Prefix` is the prefix for running command in Discord, example: `>dictionary programming`.
* The `AccessToken` key will be discussed later in the [API Authentication](#api-authentication) section.

## API Authentication

BotMyst.Web stores all the options that user set up for their servers, and the bot project needs a way to access that data in a secure way, so we are using JWT Based authentication for the website REST API.

### Creating the Auth0 app

You need to make an account on [Auth0](https://auth0.com/) and create a new app. With a free account you will get 2 machine to machine applications, which we will use so the bot and web app can communicate.

Open the API section and create a new API. For the `Name` field put anything you want. In the `Indentifier` field you should put the endpoint to the API. It could be the url to the website + `/api`, or `localhost`. Examples: `https://example.com/api`, `http://localhost:5000/api` or `https://localhost:5001/api`.

> NOTE: The `Indentifier` field cannot be changed once the API app is created. So don't change the port the app is running on once it's set.

Now once you created the Auth0 API app you can get the values we need to set up the API. Open your Auth0 API app's page, go to the `Quick Start` section and select C# as the language. From that code snippet you should get the `Authority` field, example:

![Example code snippet](https://i.imgur.com/m2TPgav.png)

The `Authority` field should look something like: `https://username.region.auth0.com/`. That value should be set as the key in the `BotMyst.Web`'s `appsettings.json` file in the `Auth0:Domain` object.

> NOTE: I recommend that you go to your Auth0 API App's settings and change the token expiration to something like `2592000` (which is 30 days) because the default one expires after one only day.

Go to the `Scopes` section and create a new scope named `master`.

Go to the `Machine to Machine Applications` section and enable the test applications, and give them the `master` scope.

### Getting the access token

Depending on your OS, you will have to manually install [curl](https://curl.haxx.se/download.html) and add it to your `PATH` if you are on Windows. I recommend you use the [curl Download Wizard](https://curl.haxx.se/dlwiz/?type=bin) if you are on Windows.

Now open the command line and run this command:

```sh
curl --request POST \
  --url 'https://YOUR_AUTH0_DOMAIN/oauth/token' \
  --header 'content-type: application/json' \
  --data '{"grant_type":"client_credentials","client_id": "YOUR_CLIENT_ID","client_secret": "YOUR_CLIENT_SECRET","audience": "YOUR_API_IDENTIFIER"}'
```

* `YOUR_AUTH0_DOMAIN` should be something like: `username.region.oauth.com`.
* `YOUR_CLIENT_ID` should be the client ID of your machine to machine application.
* `YOUR_CLIENT_SECRET` should be the client secret of your machine to machine application.
* `YOUR_API_INDENTIFIER` should be the api endpoint you set earlier.

After running this command you will get a JSON response. From that response you should copy the content under the `access_token` field, and use that key in `BotMyst.Web`'s `application.json` file in the `BotMystApi:AccessToken` object.

## Running

You should now be set to run the applications.

> NOTE: `BotMyst.Web` should be ran first as `BotMyst.Bot` sends some requests to the API immediately at startup to generate some info.

```sh
cd BotMyst.Web
dotnet run
```

```sh
cd BotMyst.Bot
dotnet run
```
