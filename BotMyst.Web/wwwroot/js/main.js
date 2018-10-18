const navigationActiveClassName = "navigationActive";

function toggleNavigation ()
{
    var nav = document.getElementById ("navigation");

    if (nav.classList.contains (navigationActiveClassName))
    {
        nav.classList.remove (navigationActiveClassName);
    }
    else
    {
        nav.classList.add (navigationActiveClassName);
    }
}

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

function openBlobPicker ()
{
    var blobPicker = document.getElementById ('blobPicker');
    var close = blobPicker.getElementsByClassName ('close') [0];

    blobPicker.style.display = 'block';

    close.onclick = function ()
    {
        blobPicker.style.display = 'none';
    }

    window.onclick = function (event)
    {
        if (event.target == blobPicker)
            blobPicker.style.display = 'none';
    }
}
