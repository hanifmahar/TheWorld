//site.js

(
    function ()
    {
        
        //var username = $("#username");
        //username.text("Muhammad Habib Mahar");

        //var main = $("#main");
        //main.on("mouseenter", function () {
        //    main.css('background-color', '#888');
        //});

        //main.on("mouseleave", function () {
        //    main.css('background-color', '');
        //});

        //var menuItems = $("ul.menu li a");

        //menuItems.on("click", function () {
        //  //var me = $(this);
        //    //alert(me.text());
        //    alert('ok');
        //});

        var $sidebarAndWrapper = $("#sidebar,#wrapper");

        $("#sdebarToggle").on("click", function ()
        {

            $sidebarAndWrapper.toggleClass("hide-sidebar");

            if ($sidebarAndWrapper.hasClass("hide-sidebar"))
            {
                $(this).text("Show Sidebar");
            }
            else
            {
                $(this).text("Hide Sidebar");
            }
        });
        
    }

)();
