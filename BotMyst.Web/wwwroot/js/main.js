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

async function toggleCommand (guildId, commandId)
{
    var enabled = await fetch (`/CommandOptions/ToggleCommand?guildId=${guildId}&commandId=${commandId}`)
        .then (function (response)
        {
            return response.json ();
        });

    var button = document.getElementsByClassName ('commandEnabled') [0].getElementsByTagName ('a') [0];

    button.innerText = enabled ? 'Enabled' : 'Disabled';
    button.classList.remove (enabled ? 'disabled' : 'enabled');
    button.classList.add (enabled ? 'enabled' : 'disabled');
}

async function openRolePicker (guildId, commandId, optionName)
{
    var blobPicker = document.getElementById ('blobPicker');
    var title = blobPicker.getElementsByTagName ('h6') [0];
    title.innerText = 'Select roles for: ' + optionName;
    var content = blobPicker.getElementsByClassName ('content') [0];
    var close = blobPicker.getElementsByClassName ('close') [0];
    
    content.innerHTML = 'Fetching roles...';

    blobPicker.style.display = 'block';

    close.addEventListener ('click', function ()
    {
        blobPicker.style.display = 'none';
    });

    window.addEventListener ('click', function (event)
    {
        if (event.target == blobPicker)
            blobPicker.style.display = 'none';
    });

    var roles = await fetch (`/Discord/AvailableRoles?guildId=${guildId}&commandId=${commandId}&optionName=${optionName}`)
        .then (function (response)
        {
            return response.json ();
        });

    content.innerHTML = '<ul class="roleList">\n</ul>';
    var list = content.getElementsByTagName ('ul') [0];

    for (var i = 0; i < roles.length; i++)
    {
        var color = roles [i].color.toString (16);
        list.innerHTML += '\n' + '<li class="role">' + 
                                    `<a style="border-color: #${color};" onclick="addBlobToBlobList ('${guildId}', ${commandId}, '${optionName}', '${roles [i].name}')">` +
                                        `<img style="border-color: #${color};" class="roleAdd" src="/img/remove.svg" />` +
                                        `<p>${roles [i].name}</p>` +
                                    '</a>' +
                                  '</li>';
    }
}

async function addBlobToBlobList (guildId, commandId, optionName, blob)
{
    await fetch (`/CommandOptions/AddBlobToBlobList?guildId=${guildId}&commandId=${commandId}&optionName=${optionName}&blob=${blob}`);
}
