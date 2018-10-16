function toggleCommand (guildId, commandId)
{
    $.ajax
    ({
        url: "/CommandOptions/ToggleCommand?guildId=" + guildId + "&commandId=" + commandId,
        success: function (data)
        {
            var button = $('.commandEnabled a');
            if (data === true)
            {
                button.text ('Enabled');
                button.removeClass ('disabled');
                button.addClass ('enabled');
            }
            else
            {
                button.text ('Disabled');
                button.removeClass ('enabled');
                button.addClass ('disabled');
            }
        }
    });
}
