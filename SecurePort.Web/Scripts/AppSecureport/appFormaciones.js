var mensaje = "Datos guardados correctamente";


function AltaEditarFormacion(action) {
    var formacionesJson = {
        "Id": $('input#ID01').val(),
        "Id_Organismo": $('input#Organismo01').val(),
        "Id_Puerto": $('input#Puerto01').val(),
        "Id_IIPP": $('input#Instalacion01').val(),
        "Titulo" : $('input#Titulo01').val(),
        "Inicio" : $('input#Inicio01').val(),
        "Fin" : $('input#Fin01').val(),
        "Duracion": $('input#DuracionFoma01').val(),
        "Lugar" : $('input#Lugar01').val(),
        "Entidad" : $('input#Entidad01').val(),
        "Observaciones": $('textarea#Observaciones01').val(),
        "Validar" :  $('input#Validar').val(),
        "action": action
    };
     var o = eval(formacionesJson);
     if (o.Id_Organismo && o.Id_Puerto && o.Titulo && o.Inicio && o.Fin && o.Duracion && o.Lugar && o.Entidad && o.Validar && $('input#ValidarAllCombos').val()) {
        $.ajax({
            url: '/Formaciones/AltaEditarFormacion',
            type: "POST",
            data: JSON.stringify({ FormacionesJson: formacionesJson }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                if (data.result == 0) {
                    window.location.reload();
                } else if (data.result == 1) {
                    showLoading();
                    $.ajax({
                        type: "POST",
                        url: '/Formaciones/VisualizarFormacion',
                        data: { id: $('input#IdValor').val(), Accion: "Ver" },
                        success: function (data) {
                            if (data) {
                                hideLoading();
                                $('#ListFormaciones').html(data);

                            }
                        }
                        });
                } else {
                    showLoading();
                    alert(data.Message, 'Confirm', 0, 25);
                }

            }
        });
    }
}


function VolverFomaciones() {

    $('input#Organismo01').val('');
    $('input#Puerto01').val('');
    $('input#Centro01').val('');
    $('div#myModalFormaciones.modal.fade.in').attr('style', 'display:none');
    hideLoading();
    $('#DatosFormaciones').html('');
    window.location.reload();

}

function CambiarOrganismo() {
    showLoading();
    $.ajax({
        type: "POST",
        url: '/Formaciones/CambiarPuerto',
        data: { id: $('input#Organismo01').val() },
        success: function (data) {
            if (data) {
                ValidarOrganismo();
                ValidacionCombos(document.getElementById('Organismo01'), 'errorOrganismo');
                hideLoading();
                $('#ComboPuertos').html(data);
                CambiarPuerto();
            }
        }
    });
}


function CambiarPuerto() {
    showLoading();
    $.ajax({
        type: "POST",
        url: '/Formaciones/CambiarCentro',
        data: { id: $('input#Puerto01').val() },
        success: function (data) {
            if (data) {
                //ValidarPuerto();
                ValidacionCombos(document.getElementById('Puerto01'), 'errorPuerto');
                hideLoading();
                $('#ComboCentros').html(data);
            }
        }
    });
}

function ValidarMayor() {
    var fechaInicio = $('input#Inicio01').val();
    var fechaFin = $('input#Fin01').val();
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
    }else
        return true;
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

function ValidarInicio() {
    if (!ValidarTexto($('input#Inicio01').val())) {
        datosInicio.style.display = "";
        datosFinValida.style.display = "none";
    }
    else {
        if (ValidarMayor()) {
            datosInicio.style.display = "none";
            datosFinValida.style.display = "none";
        } else {
            datosFinValida.style.display = "";
            datosInicio.style.display = "none";
        }
    }
}

function ValidarFin() {
    if (!ValidarTexto($('input#Fin01').val())) {
        datosFin.style.display = "";
        datosFinValida.style.display = "none";
    }
    else {
        if (ValidarMayor()) {
            datosFin.style.display = "none";
            datosFinValida.style.display = "none";
        } else {
            datosFinValida.style.display = "";
            datosFin.style.display = "none";
        }
    }
}

function ValidarDuracion() {
    if (!ValidarTexto($('input#DuracionFoma01').val())) {
        datosDuracion.style.display = "";
    }
    else
        datosDuracion.style.display = "none";
}

function ValidarLugar() {
    if (!ValidarTexto($('input#Lugar01').val())) {
        datosLugar.style.display = "";
      }
    else
        datosLugar.style.display = "none";
}

function ValidarEntidad() {
    if (!ValidarTexto($('input#Entidad01').val())) {
        datosEntidad.style.display = "";        
    }
    else
        datosEntidad.style.display = "none";
}

function ValidarOrganismo()
{

    if (!ValidarCombo($('input#Organismo01').val())) {
        datosOrganismo.style.display = "";       
    }
    else
        datosOrganismo.style.display = "none";
}

function ValidarPuerto()
{
     if (!ValidarCombo($('input#Puerto01').val())) {
         datosPuerto.style.display = "";
     }
      else
        datosPuerto.style.display = "none";
}

function ValidarCentro()
{
    if (!ValidarCombo($('input#Instalacion01').val())) {
        datosCentro.style.display = "";      
    }
    else
        datosCentro.style.display = "none";

}

function ValidarTitulo() {
    if (!ValidarTexto($('input#Titulo01').val())) {
        datosTitulo.style.display = "";
    }
    else
        datosTitulo.style.display = "none";
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
    if (!ValidacionCombos(document.getElementById('Organismo01'), 'errorOrganismo'))
        comboValida = false;
    
    return comboValida;
}


//Documentos
function VolverDocFormacion() {
    $.ajax({

        url: '/Formaciones/AsociadosEditFormaciones',
        type: "POST",
        data: { id: 1 },
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

function onChangeEditDocFormacion(e) {
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
        url: '/Formaciones/GuardarDocumento',
        type: "POST",
        data: { DocumentoJson: DocumentoJson },
        success: function (data) {
            if (data.result) {
                $.ajax({
                    type: "POST",
                    url: '/Formaciones/AsociadosEditFormaciones',
                    data: { id: 1 },
                    success: function (data) {
                        if (data) {
                            $('#MostrarMensaje').html('Visualizar Formación');
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
        url: '/Formaciones/DeshacerCambios',
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

function VisualizarDocFormacion(Id) {
    $.ajax({
        type: "POST",
        url: '/Formaciones/AsociadosEditFormaciones',
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

