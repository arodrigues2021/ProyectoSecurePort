var mensaje = "Datos guardados correctamente";


function AltaEditarProcedimiento(action) {
    var procedimientoJson = {
        "Id": $('input#ID01').val(),
        "Ind_AP": $('input#IndAP01').val(),
        "Titulo" : $('input#Titulo01').val(),
        "Fecha" : $('input#Fecha01').val(),
        "Descripcion": $('input#Descripcion01').val(),
        "Observaciones": $('textarea#Observaciones01').val(),       
        "action": action
    };
    var o = eval(procedimientoJson);
    if (o.Titulo && o.Fecha && o.Descripcion) {
        $.ajax({
            url: '/Procedimientos/AltaEditarProcedimiento',
            type: "POST",
            data: JSON.stringify({ ProcedimientosJson: procedimientoJson }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                if (data.result == 0) {
                    window.location.reload();
                } else if (data.result == 1) {
                    showLoading();
                    $.ajax({
                        type: "POST",
                        url: '/Procedimientos/VisualizarProcedimiento',
                        data: { id: $('input#IdValor').val(), Accion: "Ver" },
                        success: function (data) {
                            if (data) {
                                hideLoading();
                                $('#ListProcedimientos').html(data);

                            }
                        }
                        });
                } else {                   
                    showLoading();                    
                    alert(data.Message, 'Procedimientos');                    
                }

            }
        });
    }
}


function VolverProcedimientos() {

    $('input#Organismo01').val('');
    $('div#myModalProcedimientos.modal.fade.in').attr('style', 'display:none');
    hideLoading();
    $('#DatosProcedimientos').html('');
    window.location.reload();

}

function CambiarOrganismo() {
    showLoading();
    $.ajax({
        type: "POST",
        url: '/Procedimientos/CambiarPuerto',
        data: { id: $('input#Organismo01').val() },
        success: function (data) {
            if (data) {
                ValidarOrganismo();
                hideLoading();
                $('#ComboPuertos').html(data);
                CambiarPuerto();
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
    else {
        datosFecha.style.display = "none";        
    }
}

function ValidarDescripcion() {
    if (!ValidarTexto($('input#Descripcion01').val())) {
        datosDescripcion.style.display = "";
    }
    else
        datosDescripcion.style.display = "none";
}

function ValidarTitulo() {
    if (!ValidarTexto($('input#Titulo01').val())) {
        datosTitulo.style.display = "";
      }
    else
        datosTitulo.style.display = "none";
}

function ValidarOrganismo()
{

    if (!ValidarCombo($('input#Organismo01').val())) {
        datosOrganismo.style.display = "";       
    }
    else
        datosOrganismo.style.display = "none";
}


//Documentos
function VolverDocProcedimiento() {
    $.ajax({

        url: '/Procedimientos/AsociadosEditProcedimientos',
        type: "POST",
        data: { id: 1 },
        success: function (data) {
            if (data) {
                $('#MostrarMensaje').html('Visualizar Procedimiento de Autoridades Portuarias');
                $('#myModalUpload').modal('hide');
                $('#DocumentosRefrescar').html(data);
                $('#Edit-page').html('');
                hideLoading();
            }
        }
    });
}

function onChangeEditDocProcedimiento(e) {
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
        url: '/Procedimientos/GuardarDocumento',
        type: "POST",
        data: { DocumentoJson: DocumentoJson },
        success: function (data) {
            if (data.result) {
                $.ajax({
                    type: "POST",
                    url: '/Procedimientos/AsociadosEditProcedimientos',
                    data: { id: 1 },
                    success: function (data) {
                        if (data) {
                            $('#MostrarMensaje').html('Visualizar Procedimiento de Autoridades Portuarias');
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
        url: '/Procedimientos/DeshacerCambios',
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

function VisualizarDocProcedimiento(Id) {
    $.ajax({
        type: "POST",
        url: '/Procedimientos/AsociadosEditProcedimientos',
        data: { id: Id },
        success: function (data) {
            if (data) {
                $('#MostrarMensaje').html('Visualizar Formacion');
                $('#myModalUpload').modal('hide');
                $('#DocumentosRefrescar').html(data);
                $('#Edit-page').html('');
                hideLoading();
            }
        }
    });

}

