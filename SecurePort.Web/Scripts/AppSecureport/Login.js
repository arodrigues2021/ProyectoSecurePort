$(function () {

    $('#confirm').attr('style', 'display:none');

    $('#IdUsuario').addClass('active open');

    $('#IdGestionUsuarios').attr('style', 'display:block');

    $('#IdGestionUsuarios').addClass('open');

    if (history.forward(1)) {
        location.replace(history.forward(1));
    }

    $('a.clip-chevron-right').attr('style', 'display:none');

    $('i.clip-chevron-left').attr('style', 'display:block;font-size: 25px;');

    $('a#myLink').click(function () {
        $.post("/Login/validarlogin", { email: $('input#login.form-control').val(), password: $('input#password.form-control.password').val(), parametro: 1 }, function (data) {
            $.each(data, function (i, item) {
                if (item.nombre != "" && item.id != "4") {
                    $('input#login.form-control').blur();
                    $('input#password.form-control.password').blur();
                    $('#Idloading').attr('style', 'display:none');
                    setTimeout(function () {
                        showLoading();
                        alert(item.nombre, "Login");
                    }, 1000);
                } else {
                    if (item.id == "1") {
                        $('button.btn.btn-primary').attr('style', 'display:none');
                        $('div#myModalCondiciones.modal.container.fade.in').attr('style', 'margin-top:-50%');
                        $('div#myModalCondiciones.modal.container.fade.in').attr('style', 'display:block');

                    } else if (item.id == "4") {
                        $('button.btn.btn-primary').attr('style', 'display:block');
                        $('div#myModalCondiciones.modal.container.fade.in').attr('style', 'margin-top:-50%');
                        $('div#myModalCondiciones.modal.container.fade.in').attr('style', 'display:block');
                    }
                }
            });
        });
    });

    $('input#checkbox0.btn.tooltips.btn-primary').click(function () {
        $('div#IdFecha').attr('style', 'display:block');
        $('#IdOpp').val("false");

    });

    $('input#checkbox001.btn.tooltips.btn-primary').click(function () {
        $('div#IdFecha').attr('style', 'display:none');
        $('input.form-control-b.date-picker').val('');
        $('#IdOpp').val("true");
    });

    $("#send").click(function () {
        var LoginJson = {
            "Email": $('input#login.form-control').val(),
            "Password": $('input#password.form-control').val(),
        };
        comprobar(LoginJson);
   });

});

function comprobar(LoginJson)
    {
    var o = eval(LoginJson);
    if (o.Password == "") {
        showLoading();
        alert('Contraseña no puede estar vacio.', "Login");
    }
    if (o.Email == "") {
        showLoading();
        alert('Usuario no puede estar vacio.', "Login");
    }
    if (o.Email && o.Password ) {
        Loading();
        $.post("/Login/validarlogin", { email: o.Email, password: o.Password, parametro: '0' }, function (data) {
            $.each(data, function (i, item) {
                if (item.nombre != "" && item.id != "4") {
                    $('input#login.form-control').blur();
                    $('input#password.form-control').blur();
                    $('#Idloading').attr('style', 'display:none');
                    setTimeout(function () {
                        showLoading();
                        alert(item.nombre, "Login");
                    }, 2000);
                } else if (item.id == "4") {
                    $('#loading').hide();
                    showLoading();
                    AvisoLegalInicio();
                } else {
                    location.reload();
                }

            });

        });
    }
}