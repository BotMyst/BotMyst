# BotMyst

BotMyst is a bot written in C# using <a href="https://github.com/RogueException/Discord.Net">Discord.NET</a>. This is an open source project and you're welcomed to contribute. Report any issues in the <a href="https://github.com/LeonLaci/BotMyst/issues">issues section</a>.

This project is under the MIT license. You're free to use any part of this project without any permission. Check out <a href="https://github.com/LeonLaci/BotMyst/blob/master/LICENSE">LICENESE</a> for more information about permissions, limitations and liability.

Join the BotMyst development server on Discord: https://discord.gg/rNQBq9

## Features
There are current'y no features. BotMyst can only appear online. Don't worry, we will be adding a ton of features.

## Setup
BotMyst is build on top of .Net Core using Discord.Net. You should have <a href="https://www.microsoft.com/net/core#windowscmd">.Net Core SDK</a> installed (current recommended version for the project is 2.0).

To use BotMyst you should clone or download this repo and make a new file called BotMystConfig.json. For now it is used for setting up the token for the bot.

This is how the config file should look like:
```json
{
  "token": "<your-token-here>"
}
```

If you want to run the project using the CLI (recommended way as you don't have to have an IDE open to run the bot) use the following command:
```
dotnet run
```

## Contributing
Information about contributing to the project can be found at <a href="https://github.com/LeonLaci/BotMyst/blob/master/CONTRIBUTING.md">CONTRIBUTING</a>.
