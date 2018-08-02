function doSomething (guildId, commandId, commandName)
{
    $('#rolePicker').load ('CommandSettings/DisplayRolePicker', { guildId: guildId, commandId: commandId, commandName: commandName });
}