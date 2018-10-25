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

    if (roles.length === 0)
    {
        content.innerHTML = '<p class="warning">No roles left to add, all roles are already added to the option.</p>';
    }

    for (var i = 0; i < roles.length; i++)
    {
        var color = roles [i].color.toString (16);
        list.innerHTML += '\n' + '<li class="role">' + 
                                    `<a style="border-color: #${color};" onclick="addRoleToRoleList ('${guildId}', ${commandId}, '${optionName}', '${roles [i].name}', '${color}')">` +
                                        `<img style="border-color: #${color};" class="roleAdd" src="/img/remove.svg" />` +
                                        `<p>${roles [i].name}</p>` +
                                    '</a>' +
                                  '</li>';
    }
}

async function addRoleToRoleList (guildId, commandId, optionName, roleName, color)
{
    await fetch (`/CommandOptions/AddBlobToBlobList?guildId=${guildId}&commandId=${commandId}&optionName=${optionName}&blob=${roleName}`);

    var blobPicker = document.getElementById ('blobPicker');
    var content = blobPicker.getElementsByClassName ('content') [0];
    var list = content.getElementsByTagName ('ul') [0];

    for (var i = 0; i < list.children.length; i++)
    {
        var blobValue = list.children [i].getElementsByTagName ('p') [0].innerText;
        if (blobValue === roleName)
        {
            list.removeChild (list.children [i]);
            break;
        }
    }

    if (list.children.length === 0)
    {
        content.innerHTML = '<p class="warning">No roles left to add, all roles are already added to the option.</p>';
    }

    var allOptions = document.getElementsByClassName ('option');
    for (var i = 0; i < allOptions.length; i++)
    {
        var name = allOptions [i].getElementsByClassName ('name') [0].innerText.toLowerCase ();
        if (name === optionName.toLowerCase ())
        {
            var blobList = allOptions [i].getElementsByClassName ('blobList') [0].getElementsByTagName ('ul') [0];

            if (blobList.children.length > 0)
            {
                if (blobList.children [0].getElementsByClassName ('blobText') [0].innerText === '...')
                {
                    blobList.removeChild (blobList.children [0]);
                }
            }

            var role = document.createElement ('li');
            var roleA = document.createElement ('a');
            var roleRemove = roleA.appendChild (document.createElement ('img'));
            var roleNameP = roleA.appendChild (document.createElement ('p'));
            role.style = `border-color: #${color};`;
            roleA.addEventListener ('click', function ()
            {
                removeRoleFromRoleList (guildId, commandId, optionName, roleName);
            });
            roleRemove.style = `border-color: #${color}`;
            roleRemove.classList.add ('blobRemove');
            roleRemove.src = '/img/remove.svg';
            roleNameP.classList.add ('blobText');
            roleNameP.innerText = roleName;

            role.appendChild (roleA);

            blobList.insertBefore (role, blobList.childNodes [blobList.childNodes.length - 2]);

            break;
        }
    }
}

async function removeRoleFromRoleList (guildId, commandId, optionName, roleName)
{
    await fetch (`/CommandOptions/RemoveBlobFromBlobList?guildId=${guildId}&commandId=${commandId}&optionName=${optionName}&blob=${roleName}`);

    var allOptions = document.getElementsByClassName ('option');
    for (var i = 0; i < allOptions.length; i++)
    {
        var name = allOptions [i].getElementsByClassName ('name') [0].innerText.toLowerCase ();
        if (name === optionName.toLowerCase ())
        {
            var blobList = allOptions [i].getElementsByClassName ('blobList') [0].getElementsByTagName ('ul') [0];

            for (var j = 0; j < blobList.children.length; j++)
            {
                var blobValue = blobList.children [j].getElementsByClassName ('blobText') [0].innerText;
                if (blobValue === roleName)
                {
                    blobList.removeChild (blobList.children [j]);
                    break;
                }
            }

            if (blobList.children.length === 1)
            {
                var emptyRole = document.createElement ('li');
                var emptyRoleA = document.createElement ('a');
                emptyRole.appendChild (emptyRoleA);
                var emptyRoleP = document.createElement ('p');
                emptyRoleA.appendChild (emptyRoleP);
                
                emptyRoleP.classList.add ('blobText');
                emptyRoleP.innerText = '...';

                blobList.insertBefore (emptyRole, blobList.childNodes [blobList.childNodes.length - 2]);
            }

            break;
        }
    }
}
