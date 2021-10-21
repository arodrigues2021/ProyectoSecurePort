var mensaje = "Datos guardados correctamente";


function AltaEditarComunicacion(action) {
    var comunicacionesJson = {
        "Id": $('input#ID01').val(),
        "Id_Puerto": $('input#Puerto01').val(),
        "Id_IIPP": $('input#Instalacion01').val(),
        "Asunto" : $('input#Asunto01').val(),
        "Fecha" : $('input#Fecha01').val(),
        "Emisor": $('input#Emisor01').val(),
        "Receptor" : $('input#Receptor01').val(),
        "Recibido": ($('input#Recibido01').val() == '1' ? 'true' : 'false'),
        "Mensaje": $('textarea#Mensaje01').val(),
        "action": action
    };
    var o = eval(comunicacionesJson);
    if (o.Id_Puerto && o.Asunto && o.Fecha && o.Emisor && o.Receptor && o.Recibido && $('input#ValidarAllCombos').val()) {
        $.ajax({
            url: '/Comunicaciones/AltaEditarComunicacion',
            type: "POST",
            data: JSON.stringify({ ComunicacionesJson: comunicacionesJson }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                if (data.result == 0) {
                    alert(data.Message, 'Eliminar');
                } else if (data.result == 1) {
                    showLoading();
                    $.ajax({
                        type: "POST",
                        url: '/Comunicaciones/VisualizarComunicacion',
                        data: { id: $('input#IdValor').val(), Accion: "Ver" },
                        success: function (data) {
                            if (data) {
                                hideLoading();
                                $('#ListComunicaciones').html(data);
                            }
                        }
                        });
                } else {
                    //$('#myModalCentros').modal('hide');
                    showLoading();
                    alert(data.Message, 'Confirm', 0, 25);
                }

            }
        });
    }
}


function VolverComunicacion() {

    $('input#Puerto01').val('');
    $('input#Centro01').val('');
    $('div#myModalComunicaciones.modal.fade.in').attr('style', 'display:none');
    hideLoading();
    $('#DatosComunicaciones').html('');
    window.location.reload();

}


function CambiarPuerto() {
    showLoading();
    $.ajax({
        type: "POST",
        url: '/Comunicaciones/CambiarCentro',
        data: { id: $('input#Puerto01').val() },
        success: function (data) {
            if (data) {
                ValidarPuerto();
                ValidacionCombos(document.getElementById('Puerto01'), 'errorPuerto');
                hideLoading();
                $('#ComboCentros').html(data);
            }
        }
    });
}

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

function ValidarFecha() {
    if (!ValidarTexto($('input#Fecha01').val())) {
        datosFecha.style.display = "";
    }
    else
        datosFecha.style.display = "none";
}


function ValidarAsunto() {
    if (!ValidarTexto($('input#Asunto01').val())) {
        datosAsunto.style.display = "";
    }
    else
        datosAsunto.style.display = "none";
}

function ValidarEmisor() {
    if (!ValidarTexto($('input#Emisor01').val())) {
        datosEmisor.style.display = "";
      }
    else
        datosEmisor.style.display = "none";
}

function ValidarPuerto()
{
     if (!ValidarCombo($('input#Puerto01').val())) {
         datosPuerto.style.display = "";
     }
      else
        datosPuerto.style.display = "none";
}

function ValidarReceptor() {
    if (!ValidarTexto($('input#Receptor01').val())) {
        datosReceptor.style.display = "";
    }
    else
        datosReceptor.style.display = "none";
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
    if (!ValidacionCombos(document.getElementById('Instalacion01'), 'errorInsta'))
        comboValida = false;
    if (!ValidacionCombos(document.getElementById('Recibido01'), 'errorRecibido'))
        comboValida = false;
    
    return comboValida;
}


//Documentos
function VolverDocComunicacion() {
    $.ajax({

        url: '/Comunicaciones/AsociadosEditComunicacion',
        type: "POST",
        data: { id: 1 },
        success: function (data) {
            if (data) {
                $('#MostrarMensaje').html('Visualizar Comunicación');
                $('#myModalUpload').modal('hide');
                $('#DocumentosRefrescar').html(data);
                $('#Edit-page').html('');
                hideLoading();
            }
        }
    });
}

function onChangeEditDocComunicacion(e) {
    //Seleccionamos el registro
    var code = this.dataItem(this.select()).id;
    var documento = this.dataItem(this.select()).documento;
    $('input#IdDoc').val(code);
    $('input#NombreDocumento').val(documento);
}

function GuardarCambios() {
    showLoading();
    var DocumentoJson = {
        "Id": $('input#idDocumento.form-control').val(),
        "tipodocumento": $('select#TiposDocumento01.form-control').val(),
        "descripcion": $('input#descripcion01.form-control').val()
    };
    $.ajax({
        url: '/Comunicaciones/GuardarDocumento',
        type: "POST",
        data: { DocumentoJson: DocumentoJson },
        success: function (data) {
            if (data.result) {
                $.ajax({
                    type: "POST",
                    url: '/Comunicaciones/AsociadosEditComunicacion',
                    data: { id: 1 },
                    success: function (data) {
                        if (data) {
                            $('#MostrarMensaje').html('Visualizar Comunicación');
                            $('#myModalUpload').modal('hide');
                            $('#DocumentosRefrescar').html(data);
                            $('#Edit-page').html('');
                            hideLoading();
                        }
                    }
                });
            }
        }
    });
}

function DeshacerEdicion() {
    showLoading();
    $.ajax({
        url: '/Comunicaciones/DeshacerCambios',
        type: "POST",
        data: { id: $('input#idDocumento.form-control').val() },
        success: function (data) {
            if (data) {
                hideLoading();
                $('#myModalUpload').modal('show');
                $('#AsignarDocumentos').html(data);
                $('#Edit-page').html('');
            }
        }
    });
}

function VisualizarDocComunicacion(Id) {
    $.ajax({
        type: "POST",
        url: '/Comunicaciones/AsociadosEditComunicacion',
        data: { id: Id },
        success: function (data) {
            if (data) {
                $('#MostrarMensaje').html('Visualizar Comunicación');
                $('#myModalUpload').modal('hide');
                $('#DocumentosRefrescar').html(data);
                $('#Edit-page').html('');
                hideLoading();
            }
        }
    });

}

// fin documentos




