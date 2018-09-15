function displayBlobPicker (guildId, commandId, optionName, blobType)
{
    $('#' + blobType).load ('CommandSettings/DisplayBlobPicker', { guildId: guildId, commandId: commandId, optionName: optionName, blobType: blobType });
    setTimeout (function ()
    {
        $('#' + blobType).addClass ('visible');
    }, 200);
}

function closeBlobPicker (blobType)
{
    $('#' + blobType).removeClass ('visible');
    setTimeout(function ()
    {
        $('#' + blobType).empty ();    
    }, 200);
}

function checkMouseup (container, e, blobType)
{
    if ($('#' + blobType).is (':empty') === false)
    {
        if (!container.is (e.target) && container.has (e.target).length === 0)
        {
            closeBlobPicker (blobType);
        }
    }
}

$(document).mouseup (function (e)
{
    var container = $('.popup');

    checkMouseup (container, e, 'RolePicker');
    checkMouseup (container, e, 'ChannelPicker');
});
