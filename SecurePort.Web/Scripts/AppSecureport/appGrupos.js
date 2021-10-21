var mensaje = "Datos guardados correctamente";
$(function () {

    $('#IdGrupos').addClass('active open');

    $('#IdGestionUsuarios').attr('style', 'display:block');

    $('#IdGestionUsuarios').addClass('open');

    $('#IdSubMenuMaestros').attr('style', 'display:none');

    $('#confirm').attr('style', 'display:none');

    if (history.forward(1)) {

        location.replace(history.forward(1));
    }

    Ladda.bind('.button-ladda button', { timeout: 20000 });
    
    $('a.clip-chevron-right').attr('style', 'display:none');

    $('i.clip-chevron-left').attr('style', 'display:block;font-size: 25px;');

    $('a.clip-chevron-right').click(function () {
        $.ajax({
            type: "POST",
            url: '/Grupos/RefrescarGrupos',
            success: function (data) {
                if (data) {
                    $('#ListGrupos').html(data);
                }
            }
        });
        $('body').addClass('navigation-small');
        $('a.clip-chevron-right').attr('style', 'display:none');
        $('i.clip-chevron-left').attr('style', 'display:block;font-size: 25px;');
    });

    $('i.clip-chevron-left').click(function () {
        $.ajax({
            type: "POST",
            url: '/Grupos/RefrescarGrupos',
            success: function (data) {
                if (data) {
                    $('#ListGrupos').html(data);
                }
            }
        });
        $('body').removeClass('navigation-small');
        $('a.clip-chevron-right').attr('style', 'display:block;font-size: 25px;');
        $('i.clip-chevron-left').attr('style', 'display:none');
    });
});

function RegistaGrupos() {

    var GruposJson = {
        "Nombre": $('input#IdGrupoNombre01').val(),
        "Description": $('input#IdGrupoDescription01').val(),
        "Id_publico": $('input#Publico01').val()
    };

    var o = eval(GruposJson);

    if (o.Nombre !== "" ) {
        //Estructura url: '/CONTROLLER/ACTION'
        $.ajax({
            url: '/Usuarios/UsuarioLogueado',
            type: "POST",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                if (data.result) {
                    $.ajax({
                        url: '/Grupos/RegistaGrupos',
                        type: "POST",
                        data: JSON.stringify({ GruposJson: GruposJson }),
                        dataType: "json",
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            if (data.result) {
                                showLoading();
                                $('#myModalAgregar').modal('hide');
                                alert(mensaje);
                            } else {
                                $('#myModalAgregar').modal('hide');
                                showLoading();
                                alert(data.Message);
                            }
                        }
                    });
                }
            }
        });
    } else {
        showLoading();
        $('#myModalAgregar').modal('hide');
        //alert("El Nombre del Grupo es un campo requerido.", "Grupo");
    }
    if (o.Descripcion == "") {
        o.Descripcion = '';
    }
}

function RegisterGrupos() {

    var GruposJson = {
        "Nombre": $('input#IdGrupoNombre01').val(),
        "Description": $('input#IdGrupoDescription01').val(),
        "Id_publico" :  $('input#Publico01').val()
    };

    var o = eval(GruposJson);

    if (o.Nombre !== "" && $('input#ValidarAllCombos').val()) {
        //Estructura url: '/CONTROLLER/ACTION'
        $.ajax({
            url: '/Usuarios/UsuarioLogueado',
            type: "POST",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                if (data.result) {
                    $.ajax({
                        url: '/Grupos/AltaGrupos',
                        type: "POST",
                        data: JSON.stringify({ GruposJson: GruposJson }),
                        dataType: "json",
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            if (data.result) {
                                showLoading();
                                $('#myModalAgregar').modal('hide');
                                alert(data.Message);
                            } else {
                                $('#myModalAgregar').modal('hide');
                                showLoading();
                                alert(data.Message, 'Confirm', 2, 9);
                            }
                        }
                    });
                }
            }
        });
    } //else {
        //showLoading();
      //  $('#myModalAgregar').modal('hide');
        //alert("El Nombre del Grupo es un campo requerido.", "Grupo");
    //}
    if (o.Descripcion =="") {
        o.Descripcion = '';
    }
}
function VolverGrupo(){
    $('input#IdGrupoNombre.form-control').val('');
    $('input#IdGrupoDescription.form-control').val('');
    window.location.reload();
}

function RegistrarGrupos() {
    showLoading();
    if ($('input#Descripcion001.form-control').val() == '') {
        alert("El Nombre es un campo requerido", "PerfilAlta");
    } else {
        if ($('input#ValidarAllCombos').val())
            alert("¿Esta Seguro de Guardar los cambios?", 'Confirm', 0, 8);
        else
            hideLoading();
    }
}

function GuardarGrupos() {
    //Alta de Grupos
    var GruposJson = {
        "Description": $('input#IdGrupoDescription01').val(),
        "Nombre": $('input#IdGrupoNombre01').val(),
        "Id_publico": $('input#Publico01').val(),
        "Activo": $('input#Activo01').val()
    };
    $.ajax({
        url: '/Grupos/GuardarGrupos',
        type: "POST",
        data: JSON.stringify({ GruposJson: GruposJson }),
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            if (data.result) {
                $.ajax({
                    type: "POST",
                    url: '/Grupos/VisualizarGrupo',
                    data: { id: null },
                    success: function (data) {
                        if (data) {
                            hideLoading();
                            $('#ListGrupos').html(data);
                            $('#MostrarMensaje').html('Visualizar Grupo');
                        }
                    }
                });
            } else {
                setTimeout(function () {
                    showLoading();
                    alert(data.Message);
                }, 2000);
            }
        }
    });
}

function DeshacerGrupos() {
    showLoading();
    $('input#URL').val("/Grupos/DeshacerGrupos");
    $('input#URL1').val("#ListGrupos");
    alert("¿Esta Seguro de Deshacer los cambios?", 'Confirm', 0, 6);  //7
}

function ValidacionCombos(datos, campoerror) {
    if (isNaN(datos.value)) {
        $('span#' + campoerror).attr('style', 'display:block;color:#b94a48;');
        return false;
    }
    else {
        var combo = $('#' + datos.id).data("kendoComboBox");
        var existe = false;
        if (datos.value != "") {
            for (var obj in combo.dataSource._pristineData) {
                if (combo.dataSource._pristineData[obj].Value == datos.value)
                    existe = true;
            }
            if (existe) {
                $('span#' + campoerror).attr('style', 'display:none;');
                //combo.select(parseInt(datos.value));
                return true;
            }
            else {
                $('span#' + campoerror).attr('style', 'display:block;color:#b94a48;');
                return false;
            }
        }
        else {
            $('span#' + campoerror).attr('style', 'display:none;');
            return true;
        }
    }
}

function ValidarAllCombos(estado) {
    var comboValida = true;
    if (!ValidacionCombos(document.getElementById('Publico01'), 'errorPublico'))
        comboValida = false;

    if(estado=="1")
        if (!ValidacionCombos(document.getElementById('Activo01'), 'errorActivo'))
            comboValida = false;

    return comboValida;
}