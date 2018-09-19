function toggleNavigation ()
{
    var nav = document.getElementsByTagName ("nav") [0];
    var ul = nav.getElementsByTagName ("ul") [0];

    if (ul.classList.contains ("navigationActive"))
    {
        ul.classList.remove ("navigationActive");
    }
    else
    {
        ul.classList.add ("navigationActive");
    }
}