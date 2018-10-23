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

function openRolePicker (guildId, commandId, optionName)
{
    var blobPicker = document.getElementById ('blobPicker');
    var title = blobPicker.getElementsByTagName ('h6') [0];
    title.innerText = 'Select roles for: ' + optionName;
    var content = blobPicker.getElementsByClassName ('content') [0];
    var close = blobPicker.getElementsByClassName ('close') [0];
    
    content.innerHTML = 'Fetching roles...';

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

    $.ajax
    ({
        url: "/Discord/AvailableRoles?guildId=" + guildId + "&commandId=" + commandId + "&optionName=" + optionName,
        success: function (data)
        {
            content.innerHTML = '<ul class="roleList">\n</ul>';
            var list = content.getElementsByTagName ('ul') [0];

            for (var i = 0; i < data.length; i++)
            {
                var color = data [i].color.toString (16);
                list.innerHTML += '\n' + '<li class="role">' + 
                                            `<a style="border-color: #${color};" onclick="addBlobToBlobList ('${guildId}', ${commandId}, '${optionName}', '${data [i].name}')">` +
                                                '<img style="border-color: #' + color + ';" class="roleAdd" src="/img/remove.svg" />' +
                                                '<p>' + data [i].name + '</p>' +
                                            '</a>' +
                                            '</li>';
            }
        }
    });
}

function addBlobToBlobList (guildId, commandId, optionName, blob)
{
    $.ajax
    ({
        url: "CommandOptions/AddBlobToBlobList?guildId=" + guildId + "&commandId=" + commandId + "&optionName=" + optionName + "&blob=" + blob,
        success: function ()
        {
            console.log ("success");
        }
    });
}
