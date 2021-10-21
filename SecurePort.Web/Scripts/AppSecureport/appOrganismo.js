var sw=true;
$(function () {
    
    $('button#RegisterOrganismo.btn.btn-primary.hidden-xs').click(function () {
        FormValidator.init();
        if (ValidarAllCombos("0"))
            $('input#ValidarAllCombos').val("1");
        else
            $('input#ValidarAllCombos').val("");
        AltaEditarOrganismo('Alta');
    });

    $('button#RegisterEditOrganismo.btn.btn-primary.hidden-xs').click(function () {
        FormValidator.init();
        if (ValidarAllCombos("1"))
            $('input#ValidarAllCombos').val("1");
        else
            $('input#ValidarAllCombos').val("");
        AltaEditarOrganismo('Edit');
    });
    
});

function GuardarEditContacto() {
    var ContactoJson = {
        "Id":             $('input#Id01.k-textbox.k-valid').val(),
        "Nombre":         $('input#Nombre01').val(),
        "Es_Responsable": ($('input#es-responsable01').val() == "1" ? "Si" : "No"),
        "Observaciones":  $('textarea#Observaciones022').val(),
        "Telefono":       $('input#tel01').val(),
        "Fax":            $('input#fax01').val(),
        "Email":          $('input#email01').val(),
        "Cargo": $('input#cargo01').val(),
        "es_activo": ($('input#ActivoOperador01').val()=="1" ? "true": "false")
    };
    var o = eval(ContactoJson);
    if (o.Nombre && o.Es_Responsable && o.es_activo && $('input#ValidarAllCombosOP').val()) {
        $.ajax({
            url: '/Organismo/GuardarContactoOrganismo',
            type: "POST",
            data: JSON.stringify({ ContactoJson: ContactoJson }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                if (data.result) {
                    showLoading();
                    $.ajax({
                        type: "POST",
                        url: '/Organismo/VisualizarOrganismo',
                        data: { id: null },
                        success: function (data) {
                            if (data) {                                
                                $('div#myEditModalContactos').modal('hide');
                                $('#MostrarMensaje').html('Visualizar Organismo');
                                $('#Edit-Organismo').html(data);
                                $('#ListOrganismo').html('');
                                hideLoading();
                            }
                        }
                    });
                } else {
                    showLoading();
                    $('div#myEditModalContactos.modal.fade.in').attr('style', 'display:none');
                    alert(data.Message, 'Mensaje');
                }
            }
        });
    }
}

function AltaContacto() {
    var ContactoJson = {
        "Nombre":         $('input#Nombre01').val(),
        "Es_Responsable": ($('input#es-responsable01').val()=="1" ? "Si": "No"),
        "Observaciones":  $('textarea#Observaciones011').val(),
        "Telefono":       $('input#tel01').val(),
        "Fax":            $('input#fax01').val(),
        "Email":          $('input#email01').val(),
        "Cargo":          $('input#cargo01').val(),
    };
    var o = eval(ContactoJson);
    if (o.Nombre && o.Es_Responsable && $('input#ValidarAllCombosOP').val()) {
        $.ajax({
            url: '/Organismo/AltaContactoOrganismo',
            type: "POST",
            data: JSON.stringify({ ContactoJson: ContactoJson }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                if (data.result) {
                    showLoading();
                    $.ajax({
                        type: "POST",
                        url: '/Organismo/VisualizarOrganismo',
                        data: { id: null },
                        success: function (data) {
                            if (data) {                                
                                $('#MostrarMensaje').html('Visualizar Organismo');
                                $('div#myModalContactos').modal('hide');
                                $('#Edit-Organismo').html(data);
                                $('#ListOrganismo').html('');
                                //$('div#myModalContactos.modal.fade.in').attr('style', 'display:none');
                                hideLoading();
                            }
                        }
                    });
                } else {
                    $('div#myModalContactos.modal.fade.in').attr('style', 'display:none');
                    showLoading();
                    alert(data.Message, 'Mensaje');
                }
            }
        });
    }
}

function VisualizarOrganismo(){
    //$('div#myModalContactos.modal.fade.in').attr('style', 'display:none');
    //$('div#myEditModalContactos.modal.fade.in').attr('style', 'display:none');
    $('div#myModalContactos').modal('hide');
    $('div#myEditModalContactos').modal('hide');
    hideLoading();
    $('#DatosContactos').html('');
    $('#EditContactos').html('');

}

function AltaEditarOrganismo(action) {
    var OrganismoJson = {
        "Id": $('input#Id01').val(),
        "Nombre": $('input#NombreOrganismo01.form-control').val(),
        "Tipo": $('input#Tipo01').val(),
        "Observaciones": $('textarea#Observaciones01').val(),
        "ID_Ciudad": $('input#Ciudad01').val(),
        "Cod_Postal": $('input#codPostal01').val(),
        "ID_Provincia": $('input#Provincias01').val(),
        "Direccion": $('input#DireccionOrganismo01').val(),
        "activo": $('input#Activo01').val(),
        "action":action
    };
    var o = eval(OrganismoJson);
    if (o.Cod_Postal && o.Nombre && o.Tipo && o.ID_Ciudad && o.ID_Provincia && o.Direccion && $('input#ValidarAllCombos').val()) {
        $.ajax({
            url: '/Organismo/AltaEditarOrganismo',
            type: "POST",
            data: JSON.stringify({ OrganismoJson: OrganismoJson }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                if (data.result == 0) {
                    showLoading();
                    $.ajax({
                        type: "POST",
                        url: '/Organismo/VisualizarOrganismo',
                        data: { id: null },
                        success: function (data) {
                            if (data) {
                                hideLoading();
                                $('#MostrarMensaje').html('Visualizar Organismo');
                                $('#Edit-Organismo').html(data);
                                $('#ListOrganismo').html('');
                            }
                        }
                    });
                } else if (data.result == 1) {
                    showLoading();
                    alert(data.Message, "Organismos");
                } else if (data.result == 2) {
                    showLoading();
                    alert(data.Message, 'Confirm',0,41);
                } else {
                    showLoading();
                    alert(data.Message, 'Confirm',0,42);
                }
            }

        });
    }
}

function CambiarProvincia() {
    showLoading();
    $.ajax({
        type: "POST",
        url: '/Organismo/CambiarProvincia',
        data: { id: $('input#Provincias01').val() },
        success: function (data) {
            if (data) {
                hideLoading();
                $('#ComboCiudad').html(data);
                $.ajax({
                    type: "POST",
                    url: '/Organismo/CambiarComunidad',
                    data: { id: $('input#Provincias01').val() },
                    success: function (data) {
                        if (data) {
                            ValidacionCombos(document.getElementById('Provincias01'), 'errorProvincia');
                            hideLoading();
                            $('#ComboComunidad').html(data);
                        }
                    }
                });
            }
        }
    });
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

    if (!ValidacionCombos(document.getElementById('Tipo01'), 'errorTipo'))
        comboValida = false;
    if (!ValidacionCombos(document.getElementById('Provincias01'), 'errorProvincia'))
        comboValida = false;
    if (!ValidacionCombos(document.getElementById('Ciudad01'), 'errorCiudad'))
        comboValida = false;
    if (estado == "1")
    if (!ValidacionCombos(document.getElementById('Activo01'), 'errorActivo'))
        comboValida = false;


    return comboValida;
}

function ValidarAllCombosOP(estado) {
    var comboValida = true;

    if (!ValidacionCombos(document.getElementById('es-responsable01'), 'errorResponsable'))
        comboValida = false;
    if (estado == "1")
        if (!ValidacionCombos(document.getElementById('ActivoOperador01'), 'errorActivoOP'))
            comboValida = false;
    

    return comboValida;
}
