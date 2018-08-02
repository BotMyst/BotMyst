function doSomething (guildId, commandId, optionName)
{
    $('#rolePicker').load ('CommandSettings/DisplayRolePicker', { guildId: guildId, commandId: commandId, optionName: optionName });
}

function closeRolePicker ()
{
    $('#rolePicker').empty ();
}