var mensaje = "Datos guardados correctamente";


function AltaEditarMantenimiento(action) {
    var mantenimientoJson = {
        "Id": $('input#ID01').val(),
        "Id_Puerto": $('input#Puerto01').val(),
        "Id_IIPP": $('input#Instalacion01').val(),
        "Descripcion" : $('input#Descripcion01').val(),
        "Fecha" : $('input#Fecha01').val(),
        "Equipo": $('input#Equipo01').val(),
        "Realizador" : $('input#Realizador01').val(),
        "Validador": $('input#Validador01').val(),
        "Fecha_Revision": $('input#FechaRevision01').val(),
        "Observaciones": $('textarea#Observaciones01').val(),
        "Validar": $('input#Validar').val(),
        "action": action
    };
    var o = eval(mantenimientoJson);
    if (o.Id_Puerto && o.Descripcion && o.Fecha && o.Equipo && o.Validar && $('input#ValidarAllCombos').val()) {
        $.ajax({
            url: '/Mantenimiento/AltaEditarMantenimiento',
            type: "POST",
            data: JSON.stringify({ MantenimientoJson: mantenimientoJson }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                if (data.result == 0) {
                    //$('#myModalCentros').modal('hide');
                    window.location.reload();
                } else if (data.result == 1) {
                    showLoading();
                    $.ajax({
                        type: "POST",
                        url: '/Mantenimiento/VisualizarMantenimiento',
                        data: { id: $('input#IdValor').val(), Accion: "Ver" },
                        success: function (data) {
                            if (data) {
                                hideLoading();
                                $('#ListMantenimientos').html(data);

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


function VolverMantenimiento() {

    $('input#Puerto01').val('');
    $('input#Centro01').val('');
    $('div#myModalMantenimientos.modal.fade.in').attr('style', 'display:none');
    hideLoading();
    $('#DatosMantenimientos').html('');
    window.location.reload();

}


function CambiarPuerto() {
    showLoading();
    $.ajax({
        type: "POST",
        url: '/Mantenimiento/CambiarCentro',
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

function ValidarMayor() {
    var fechaInicio = $('input#Fecha01').val();
    var fechaFin = $('input#FechaRevision01').val();
    if (fechaFin != "") {
        fechaInicio = fechaInicio.split('/');
        fechaFin = fechaFin.split('/');

        fechaInicio = fechaInicio[1].toString() + "/" + fechaInicio[0].toString() + "/" + fechaInicio[2].toString();
        fechaFin = fechaFin[1].toString() + "/" + fechaFin[0].toString() + "/" + fechaFin[2].toString();

        if ((Date.parse(fechaInicio)) > (Date.parse(fechaFin))) {
            $('input#Validar').val("");
            return false;
        }
        else {
            $('input#Validar').val("1");
            return true;
        }
    } else {
        $('input#Validar').val("1");
        return true;
    }
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
        datosFinValida.style.display = "none";
    }
    else{
        if (ValidarMayor()) {
            datosFecha.style.display = "none";
            datosFinValida.style.display = "none";
        } else {
            datosFinValida.style.display = "";
            datosFecha.style.display = "none";
        }
    }
}

function ValidarFechaProxima() {
          if (ValidarMayor()) {            
            datosFinValida.style.display = "none";
        } else {
            datosFinValida.style.display = "";            
        }  
}


function ValidarMantenimiento() {
    if (!ValidarTexto($('input#Descripcion01').val())) {
        datosDescripcion.style.display = "";
    }
    else
        datosDescripcion.style.display = "none";
}

function ValidarEquipo() {
    if (!ValidarTexto($('input#Equipo01').val())) {
        datosEquipo.style.display = "";
      }
    else
        datosEquipo.style.display = "none";
}

function ValidarPuerto()
{
     if (!ValidarCombo($('input#Puerto01').val())) {
         datosPuerto.style.display = "";
     }
      else
        datosPuerto.style.display = "none";
}

//function ValidarCentro()
//{
//    if (!ValidarCombo($('input#Instalacion01').val())) {
//        datosCentro.style.display = "";      
//    }
//    else
//        datosCentro.style.display = "none";

//}

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

    return comboValida;
}

//Documentos
function VolverDocMantenimiento() {
    $.ajax({

        url: '/Mantenimiento/AsociadosEditMantenimiento',
        type: "POST",
        data: { id: 1 },
        success: function (data) {
            if (data) {
                $('#MostrarMensaje').html('Visualizar Mantenimiento');
                $('#myModalUpload').modal('hide');
                $('#DocumentosRefrescar').html(data);
                $('#Edit-page').html('');
                hideLoading();
            }
        }
    });
}

function onChangeEditDocMantenimiento(e) {
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
        url: '/Mantenimiento/GuardarDocumento',
        type: "POST",
        data: { DocumentoJson: DocumentoJson },
        success: function (data) {
            if (data.result) {
                $.ajax({
                    type: "POST",
                    url: '/Mantenimiento/AsociadosEditMantenimiento',
                    data: { id: 1 },
                    success: function (data) {
                        if (data) {
                            $('#MostrarMensaje').html('Visualizar Mantenimiento');
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
        url: '/Mantenimiento/DeshacerCambios',
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

function VisualizarDocMantenimiento(Id) {
    $.ajax({
        type: "POST",
        url: '/Mantenimiento/AsociadosEditMantenimiento',
        data: { id: Id },
        success: function (data) {
            if (data) {
                $('#MostrarMensaje').html('Visualizar Mantenimiento');
                $('#myModalUpload').modal('hide');
                $('#DocumentosRefrescar').html(data);
                $('#Edit-page').html('');
                hideLoading();
            }
        }
    });

}

// fin documentos




