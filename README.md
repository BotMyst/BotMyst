# BotMyst [![Discord invite](https://discordapp.com/api/guilds/298510542535000065/widget.png)](https://discord.gg/k2WEr)

**BotMyst V2 is in works. All new commits will be done to the V2 branch. There's no proper documentation or guides on contributing yet. If you want to contribute (I really need contributors) join the Discord server: https://discord.gg/rNQBq9**

BotMyst is a bot written in C# using <a href="https://github.com/RogueException/Discord.Net">Discord.NET</a>. This is an open source project and you're welcome to contribute. Report any issues in the <a href="https://github.com/LeonLaci/BotMyst/issues">issues section</a>.

This project is under the MIT license. You're free to use any part of this project without prior approval from the project's creator. Check out <a href="https://github.com/LeonLaci/BotMyst/blob/master/LICENSE">LICENSE</a> for more information about permissions, limitations and liability.

Join the BotMyst development server on Discord: https://discord.gg/rNQBq9<br>
Check out our Trello board to contribute to it or see planned features / changes: https://trello.com/b/bybAwGwQ/botmyst

## Add BotMyst to your server
You can invite the original version of BotMyst, that is self hosted by me, to your server by clicking on this link:
[Invite BotMyst to your server](https://discordapp.com/oauth2/authorize?client_id=357196134298419200&scope=bot)

## Features
BotMyst can do these things:
* [Dictionary](https://github.com/CodeMyst/BotMyst/wiki/Dictionary-command)
* [Thesaurus](https://github.com/CodeMyst/BotMyst/wiki/Thesaurus)
* [Random Video](https://github.com/CodeMyst/BotMyst/wiki/Random-YouTube-video-command)
* [Give / Remove Roles](https://github.com/CodeMyst/BotMyst/wiki/Give---Remove-Role)

## Setup
BotMyst is built on top of .Net Core using Discord.Net. You should have <a href="https://www.microsoft.com/net/core#windowscmd">.Net Core SDK</a> installed (current recommended version for the project is 2.0).

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
