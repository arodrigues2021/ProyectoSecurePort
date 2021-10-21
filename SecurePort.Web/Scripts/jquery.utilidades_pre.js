jQuery.load_noCache = function(urlAccion, datos, successFuncName, divName) {
    if (successFuncName == null) {
        $.ajax({
            cache: false,
            dataType: "html",
            url: urlAccion,
            data: datos,
            success: function (html) {
                $(divName).html(html);
            }
        });
    }
    else {
        $.ajax({
            cache: false,
            dataType: "html",
            url: urlAccion,
            data: datos,
            success: function (html) {
                $(divName).html(html);
                successFuncName();
            }
        });
    }
}


jQuery.post_noCache = function(urlAccion, datos, successFuncName) {
    if (successFuncName == null) {
        $.ajax({
            cache: false,
            dataType: "html",
            type: "POST",
            url: urlAccion,
            data: datos
        });
    }
    else {
        $.ajax({
            cache: false,
            dataType: "html",
            type: "POST",
            url: urlAccion,
            data: datos,
            success: function () {
                successFuncName();
            }
        });
    }
}

jQuery.get_noCache = function(urlAccion, datos, successFuncName) {
    if (successFuncName == null) {
        $.ajax({
            cache: false,
            dataType: "html",
            type: "GET",
            url: url,
            data: data
        });
    }
    else {
        $.ajax({
            cache: false,
            dataType: "html",
            type: "GET",
            url: url,
            data: data,
            success: function () {
                successFuncName();
            }
        });
    }
}