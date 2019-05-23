
var objType = "";
$(document).ready(function () {
    //init loadie.js
    window.loadProgress();

    $('.search-field').on('keypress', function (e) {
        if (e.keyCode == 13) {
            var q = $(this).val();
            doSearch(objType, q);
        }
    });

    $('.search.icon').bind('click', function (e) {
        var q = $(this).parent().find('input').val();
        console.log(q);
        doSearch(objType, q);
    });

    function doSearch(obj, q) {
        let action = "search";

        //default order=top
        let order = "top";

        obj = obj || "all";

        //search all object
        let url = window.location.protocol + "//" + window.location.host + "/" + action + "/" + obj + "?q=" + q;
        window.location = url;
    };

    //tab
    $('.menu .item').tab();

    $('.ui .item').on('click', function () {
        $('.ui .item').removeClass('active');
        $(this).addClass('active');
    });

    $('.special.cards .image').dimmer({
        on: 'hover'
    });
});