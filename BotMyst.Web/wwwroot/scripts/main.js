function displayRolePicker (guildId, commandId, optionName)
{
    $('#rolePicker').load ('CommandSettings/DisplayRolePicker', { guildId: guildId, commandId: commandId, optionName: optionName });
    setTimeout (function ()
    {
        $('#rolePicker').addClass ('visible');
    }, 200);
}

function closeRolePicker ()
{
    $('#rolePicker').removeClass ('visible');
    setTimeout (function ()
    {
        $('#rolePicker').empty ();
    }, 200);
}