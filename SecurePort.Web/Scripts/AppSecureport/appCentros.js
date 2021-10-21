var mensaje = "Datos guardados correctamente";


function AltaEditarCentro(action) {
    var centrosJson = {
        "Id": $('input#Id01').val(),
        "ID_Organismo": $('input#Organismo01').val(),
        "ID_Puerto": $('input#Puerto01').val(),
        "Centro_24H": $('input#Centro01').val(),
        "Direccion": $('input#Direccion01').val(),
        "Id_Ciudad": $('input#Ciudad01').val(),
        "Id_Provincia": $('input#Provincia01').val(),
        "Cod_Postal": $('input#Postal01').val(),
        "action": action
    };  
    var o = eval(centrosJson);
    if (o.ID_Organismo && o.Centro_24H && $('input#ValidarAllCombos').val()) {
        jsShowWindowLoad("Ejecutando datos.....");
        $.ajax({
            url: '/Centros/AltaEditarCentros',
            type: "POST",
            data: JSON.stringify({ CentrosJson: centrosJson }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function(data) {
                if (data.result == 0) {
                    $('#myModalCentros').modal('hide');
                    //showLoading();
                    //window.location.reload();
                    $.ajax({
                        type: "POST",
                        url: '/Centros/VisualizarCentros',
                        data: { id: null, Accion: "Ver" },
                        success: function (data) {
                            if (data) {
                                hideLoading();
                                $('#ListCentros').html(data);
                                jsRemoveWindowLoad();
                            }
                        }
                    });
                } else if (data.result == 1) {
                    $('#myModalCentros').modal('hide');
                    showLoading();
                    alert(data.Message);
                } else {
                    $('#myModalCentros').modal('hide');
                    showLoading();
                    alert(data.Message, 'Confirm', 0, 25);
                }

            }
        });
    }
}


function VolverCentros() {

    $('input#Organismo01').val('');
    $('input#Puerto01').val('');
    $('input#Centro01').val('');
    $('div#myModalCentros.modal.fade.in').attr('style', 'display:none');
    hideLoading();
    $('#DatosCentros').html('');
    window.location.reload();

}

function CambiarPuerto(acciona) {
    showLoading();
    $.ajax({
        type: "POST",
        url: '/Centros/CambiarPuerto',
        data: { id: $('input#Organismo01').val(),"acciona": acciona },
        success: function (data) {
            if (data) {
                ValidacionCombos(document.getElementById('Organismo01'), 'errorOrganismo');
                hideLoading();
                $('#ComboPuertos').html(data);
            }
        }
    });
}

function CambiarProvincia(acciona) {
    showLoading();
    $.ajax({
        type: "POST",
        url: '/Centros/CambiarProvincia',
        data: { id: $('input#Provincia01').val(), "acciona": acciona },
        success: function (data) {
            if (data) {
                ValidacionCombos(document.getElementById('Provincia01'), 'errorProvincia');
                hideLoading();
                $('#ComboCiudad').html(data);
            }
        }
    });
}

function GenerarAccionesChange() {
    var value = this.value();
    $.ajax({
        type: "POST",
        url: '/Centros/Index',
        success: function (data) {
            if (data) {
                hideLoading();
                $('#ListCentros').html('');
                $('#ListCentros').html(data);
            }
        }
    });
}


// Validadciones
function ValidarTexto(Texto) {
    if (Texto == "")
        return false;
    else
        return true;

}

function ValidarCombo(Combo) {
    if (Combo == null || Combo == "")
        return false;
    else
        return true;

}

function ValidarOrganismo() {
    if (!ValidarCombo($('input#Organismo01').val())) {
        datosOrganismo.style.display = "";
        return false;
    }
    else {
        datosOrganismo.style.display = "none";
        return true;
    }
}

function ValidarCentro() {
    if (!ValidarTexto($('input#Centro01').val())) {
        datosCentro.style.display = "";
        return false;
    }
    else {
        datosCentro.style.display = "none";
        return true;
    }
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

function ValidarAllCombos() {
    var comboValida = true;

    if (!ValidacionCombos(document.getElementById('Puerto01'), 'errorPuerto'))
        comboValida = false;
    if (!ValidacionCombos(document.getElementById('Organismo01'), 'errorOrganismo'))
        comboValida = false;
    if (!ValidacionCombos(document.getElementById('Provincia01'), 'errorProvincia'))
        comboValida = false;
    if (!ValidacionCombos(document.getElementById('Ciudad01'), 'errorCiudad'))
        comboValida = false;

    return comboValida;
}


// adicionales
function ValidarDato() {
    if (!ValidarTexto($('input#Dato01').val())) {
        datosDato.style.display = "";
        return false;
    }
    else {
        datosDato.style.display = "none";
        return true;
    }
}

function Validar() {
    if (!ValidarTexto($('input#Dato01').val())) {
        datosDato.style.display = "";
        return false;
    }
    else {
        datosDato.style.display = "none";
        if ($('input#Canal01').val() == 3) {
            var expr = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
            if (!expr.test($('input#Dato01').val())) {
                datosMail.style.display = "";
            }
            else {
                $('input#ValidadoTipo01').val(1);
                datosMail.style.display = "none";
            }
        }
        else {
            $('input#ValidadoTipo01').val(1);
        }
        return true;
    }
}




function AltaEditarAdicional(action) {
    var comunica_CentroJson = {
        "Id": $('input#IDComunica01').val(),
        "Tipo_Canal": $('input#Canal01').val(),
        "Dato": $('input#Dato01').val(),
        "Nota": $('input#Nota01').val(),
        "Tipo_Validado" :  $('input#ValidadoTipo01').val(),
        "action": action
    };
    var o = eval(comunica_CentroJson);
    if (o.Tipo_Canal && o.Dato && o.Tipo_Validado) {
        jsShowWindowLoad("Ejecutando datos.....");
        $.ajax({
            url: '/Centros/AltaEditarComunicaCentro',
            type: "POST",
            data: JSON.stringify({ Comunica_CentroJson: comunica_CentroJson }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {                
                if (data.result == 0) {  // telefono
                    $.ajax({
                        type: "POST",
                        url: '/Centros/AsociadosEditTel',
                        data: { id: 1 },
                        success: function (data) {
                            if (data) {
                                $('#MostrarMensaje').html('Visualizar Centro');
                                $('#myModalDatos').modal('hide');
                                $('#TelRefrescar').html(data);
                                $('#Edit-page').html('');
                                hideLoading();
                                jsRemoveWindowLoad();
                            }
                        }
                    });
                } else if (data.result == 1) {
                    $.ajax({
                        type: "POST",
                        url: '/Centros/AsociadosEditFax',
                        data: { id: 1 },
                        success: function (data) {
                            if (data) {
                                $('#MostrarMensaje').html('Visualizar Centro');
                                $('#myModalDatos').modal('hide');
                                $('#FaxRefrescar').html(data);
                                $('#Edit-page').html('');
                                hideLoading();
                                jsRemoveWindowLoad();
                            }
                        }
                    });
                } else {
                    $.ajax({
                        type: "POST",
                        url: '/Centros/AsociadosEditMail',
                        data: { id: 1 },
                        success: function (data) {
                            if (data) {
                                $('#MostrarMensaje').html('Visualizar Centro');
                                $('#myModalDatos').modal('hide');
                                $('#MailRefrescar').html(data);
                                $('#Edit-page').html('');
                                hideLoading();
                                jsRemoveWindowLoad();
                            }
                        }
                    });
                }

            }
        });
    }
}

function onChangeEditTel(e) {
    //Seleccionamos el registro
    var code = this.dataItem(this.select()).id;
    var documento = this.dataItem(this.select()).Dato;
    $('input#IdTel').val(code);
    $('input#NombreTel').val(documento);
}

function onChangeEditFax(e) {
    //Seleccionamos el registro
    var code = this.dataItem(this.select()).id;
    var documento = this.dataItem(this.select()).Dato;
    $('input#IdFax').val(code);
    $('input#NombreFax').val(documento);
}


function onChangeEditMail(e) {
    //Seleccionamos el registro
    var code = this.dataItem(this.select()).id;
    var documento = this.dataItem(this.select()).Dato;
    $('input#IdMail').val(code);
    $('input#NombreMail').val(documento);
}

function VisualizarVolver(id, canal) {

    var retornoUrl;
    if (canal == 1)
    {
        retornoUrl = '/Centros/AsociadosEditTel';
    } else if (canal == 2) {
        retornoUrl = '/Centros/AsociadosEditFax';
    } else {
        retornoUrl = '/Centros/AsociadosEditMail';
    }
    $.ajax({
        url: retornoUrl,
        type: "POST",
        data: { id: 1 },
        success: function (data) {
            if (data) {
                $('#MostrarMensaje').html('Visualizar Centro');
                $('#myModalDatos').modal('hide');
                if (canal == 1) {                    
                    $('#TelRefrescar').html('');
                    $('#TelRefrescar').html(data);
                    $('#Edit-page').html('');
                } else if (canal == 2) {                  
                    $('#FaxRefrescar').html('');
                    $('#FaxRefrescar').html(data);
                    $('#Edit-page').html('');
                } else if (canal == 3) {
                    $('#MailRefrescar').html('');
                    $('#MailRefrescar').html(data);
                    $('#Edit-page').html('');
                }                
                hideLoading();
            }
        }
    });
}

function sleepFor(sleepDuration) {
    var start = new Date().getTime();
    for (var i = 0; i < 1e7; i++) {
        if ((new Date().getTime() - start) > sleepDuration) {
            break;
        }
    }
}