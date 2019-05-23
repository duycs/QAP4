//on ready
$(function () {
    $('.tabular.menu .item').tab();
});

// common  js
function getParameterByName(name, url) {
    if (!url) url = window.location.href;
    name = name.replace(/[\[\]]/g, "\\$&");
    var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"),
        results = regex.exec(url);
    if (!results) return null;
    if (!results[2]) return '';
    return decodeURIComponent(results[2].replace(/\+/g, " "));
}


function getLastSlugOfLink() {
    var slug = window.location.href.split('/');
    slug = slug[slug.length - 1];
    return slug;
};


function showToast(type, message) {
    let title = "";
    console.log(type);
    switch (type) {
        case "msg":
            title = "Hii!";
            iziToast.success({
                title: title,
                message: message,
            });
            break;
        case "err":
            title = "Ấy lỗi rồi";
            iziToast.error({
                title: title,
                message: message,
            });
            break;
        case "war":
            title = "Oop!";
            iziToast.warning({
                title: title,
                message: message,
            });
            break;
        default:
            title = "Hii!";
            iziToast.message({
                title: title,
                message: message,
            });
    }

}

function loadProgress() {
    $('body').loadie();
    $('body').loadie(1);
}

