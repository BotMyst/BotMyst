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

$(document).mouseup (function (e)
{
    var container = $('.popup');

    if ($('#rolePicker').is (':empty') === false)
    {
        if (!container.is (e.target) && container.has (e.target).length === 0)
        {
            closeRolePicker ();
        }
    }
});