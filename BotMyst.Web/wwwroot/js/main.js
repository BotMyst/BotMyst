var navigationActiveClassName = "navigationActive";

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