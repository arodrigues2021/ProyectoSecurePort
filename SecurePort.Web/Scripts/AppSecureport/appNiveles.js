var mensaje = "Datos guardados correctamente";


function AltaEditarNivel(action) {
    var nivelesJson = {
        "Id": $('input#ID01').val(),
        "Id_Puerto": $('input#Puerto01').val(),
        //"Id_IIPP": $('input#Instalacion01').val(),
        "Descripcion" : $('input#Descripcion01').val(),
        "Fecha": $('input#FechaNivel01').val(),
        "Emisor": $('input#Emisor01').val(),
        "Receptor": $('input#Receptor01').val(),
        "Nivel": $('input#Nivel01').val(),
        "Emisor_Cancela": $('input#EmisorCancela01').val(),
        "Duracion": $('input#Duracion01').val(),
        "Fecha_Cancela": $('input#FechaCancela01').val(),
        "Nivel_max": $('input#NivelMax01').val(),
        "Relevante": $('textarea#Relevante01').val(),
        "Recomendacion": $('textarea#Recomendacion01').val(),
        "Observaciones": $('textarea#Observaciones01').val(),
        "action": action
    };
    var o = eval(nivelesJson);
    if (o.Id_Puerto && o.Descripcion && o.Fecha && o.Nivel && $('input#ValidarAllCombos').val()) {
        $.ajax({
            url: '/Niveles/AltaEditarNivel',
            type: "POST",
            data: JSON.stringify({ NivelesJson: nivelesJson }),
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
                        url: '/Niveles/VisualizarNivel',
                        data: { id: $('input#IdValor').val(), Accion: "Ver" },
                        success: function (data) {
                            if (data) {
                                hideLoading();
                                $('#ListNiveles').html(data);

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


function VolverNiveles() {

    $('input#Puerto01').val('');
    $('input#Centro01').val('');
    $('div#myModalNiveles.modal.fade.in').attr('style', 'display:none');
    hideLoading();
    $('#DatosNiveles').html('');
    window.location.reload();

}


function CambiarPuerto() {
    showLoading();
    $.ajax({
        type: "POST",
        url: '/Niveles/CambiarCentro',
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
    if (!ValidarTexto($('input#FechaNivel01').val())) {
        datosFecha.style.display = "";
    }
    else
        datosFecha.style.display = "none";
}

function ValidarDescripcion() {
    if (!ValidarTexto($('input#Descripcion01').val())) {
        datosDescripcion.style.display = "";
    }
    else
        datosDescripcion.style.display = "none";
}

function ValidarNivel() {
    if (!ValidarTexto($('input#Nivel01').val())) {
        datosNivel.style.display = "";
    }
    else {
        ValidacionCombos(document.getElementById('Nivel01'), 'errorNivel');
        datosNivel.style.display = "none";
    }
}

function ValidarPuerto()
{
     if (!ValidarCombo($('input#Puerto01').val())) {
         datosPuerto.style.display = "";
     }
      else
        datosPuerto.style.display = "none";
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
    if (!ValidacionCombos(document.getElementById('Nivel01'), 'errorNivel'))
        comboValida = false;
    if (!ValidacionCombos(document.getElementById('NivelMax01'), 'errorNivelAlc'))
        comboValida = false;    

    return comboValida;
}



//Documentos
function VolverDocNivel() {
    $.ajax({

        url: '/Niveles/AsociadosEditNiveles',
        type: "POST",
        data: { id: 1 },
        success: function (data) {
            if (data) {
                $('#MostrarMensaje').html('Visualizar Nivel');
                $('#myModalUpload').modal('hide');
                $('#DocumentosRefrescar').html(data);
                $('#Edit-page').html('');
                hideLoading();
            }
        }
    });
}

function onChangeEditDocNivel(e) {
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
        url: '/Niveles/GuardarDocumento',
        type: "POST",
        data: { DocumentoJson: DocumentoJson },
        success: function (data) {
            if (data.result) {
                $.ajax({
                    type: "POST",
                    url: '/Niveles/AsociadosEditNiveles',
                    data: { id: 1 },
                    success: function (data) {
                        if (data) {
                            $('#MostrarMensaje').html('Visualizar Nivel');
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
        url: '/Niveles/DeshacerCambios',
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

function VisualizarDocNivel(Id) {
    $.ajax({
        type: "POST",
        url: '/Niveles/AsociadosEditNiveles',
        data: { id: Id },
        success: function (data) {
            if (data) {
                $('#MostrarMensaje').html('Visualizar Nivel');
                $('#myModalUpload').modal('hide');
                $('#DocumentosRefrescar').html(data);
                $('#Edit-page').html('');
                hideLoading();
            }
        }
    });

}

// fin documentos

// intalaciones
function InstalacionesVisualizarAccionesChange() {
    var value = this.value();
        $.ajax({
            type: "POST",
            url: '/Niveles/AsignarInstalacion',
            success: function (data) {
                if (data) {                    
                    hideLoading();
                    $('#ListNiveles').html(data);

                }
            }
        });    
}


function onClickInstalacionNivel() {
    datosInstalaciones = new Array();
    var y = 0;
    var a;
    var selectedItem;
    var grid = $("#GridInstalaciones").data("kendoGrid");
    var rows = grid.select();
    rows.each(function (index, row) {
        selectedItem = grid.dataItem(row);
        for (var i in selectedItem) {
            if (i == 'Id') {
                datosInstalaciones[y] = selectedItem.Id;
                y++;
            }
        }
    });
    if (datosInstalaciones.length == 0) {
        showLoading();
        alert('Debe Seleccionar la Instalación', "Mensaje");
    } else {
        console.log("Asignar==>" + datosInstalaciones);
        showLoading();
        for (a = 0; a < datosInstalaciones.length; a++) {
            var id = datosInstalaciones[a];
            if (id != null) {
                $.ajax({
                    type: "POST",
                    url: '/Niveles/ActualizarInstalacion',
                    data: { id: id },
                    success: function (data) {
                        if (data.result == false) {
                            showLoading();
                            alert(data.Message, 'Eliminar');
                        } else {
                            $('#InstaDisponibles').html(data);
                            $.ajax({
                                type: "POST",
                                url: '/Niveles/ActualizarInstalacionAsignada',
                                success: function (data) {
                                    if (data.result == false) {
                                        showLoading();
                                        alert(data.Message, 'Eliminar');
                                    } else {
                                        $('#InstaAsignadas').html(data);
                                        hideLoading();
                                    }
                                }
                            });
                        }
                    }
                });
            }
        };
        console.log("Resultado OK datosInstalaciones");
        hideLoading();
    }
}

function onClickInstalacionQuitarNivel() {
    datosInstalacionesQuitar = new Array();
    var y = 0;
    var c;
    var selectedItem;
    var grid = $("#GridInstalacionesAsociados").data("kendoGrid");
    var rows = grid.select();
    rows.each(function (index, row) {
        selectedItem = grid.dataItem(row);
        for (var i in selectedItem) {
            if (i == 'Id') {
                datosInstalacionesQuitar[y] = selectedItem.Id;
                y++;
            }
        }
    });
    if (datosInstalacionesQuitar.length == 0) {
        showLoading();
        alert('Debe Seleccionar la Instalación', "Mensaje");
    } else {
        showLoading();
        for (c = 0; c < datosInstalacionesQuitar.length; c++) {
            var id = datosInstalacionesQuitar[c];
            if (id != null) {
                $.ajax({
                    type: "POST",
                    url: '/Niveles/QuitarInstalacion',
                    data: { id: id },
                    success: function (data) {
                        if (data.result == false) {
                            showLoading();
                            alert(data.Message, 'Eliminar');
                        } else {
                            $('#InstaDisponibles').html(data);
                            $.ajax({
                                type: "POST",
                                url: '/Niveles/ActualizarInstalacionAsignada',
                                success: function (data) {
                                    if (data.result == false) {
                                        showLoading();
                                        alert(data.Message, 'Eliminar');
                                    } else {
                                        $('#InstaAsignadas').html(data);
                                        hideLoading();
                                    }
                                }
                            });
                        }
                    }
                });
            }
        };
        console.log("Resultado OK datosInstalacionesQuitar");
        hideLoading();
    }

}

function GuardarNivelInst()
{  
    $.ajax({
        url: '/Niveles/GuardarNivelInstalaciones',
        type: "POST",
        data: { id: 1 },
        success: function (data) {
            showLoading();
            $.ajax({
                type: "POST",
                url: '/Niveles/VisualizarNivel',
                data: { id: $('input#IdValor').val(), Accion: "Ver" },
                success: function (data) {
                    if (data) {
                        hideLoading();
                        $('#ListNiveles').html(data);

                    }
                }
            });
        }
        
    });
}

function DeshacerNivelInst()
{ 
    $.ajax({
        url: '/Niveles/DeshacerNivelInstalaciones',
        type: "POST",
        data: { id: 1 },
        success: function (data) {
            if (data) {
                $('#InstaDisponibles').html(data);
                    $.ajax({
                        type: "POST",
                        url: '/Niveles/ActualizarInstalacionAsignada',
                        success: function (data) {
                            if (data.result == false) {
                                showLoading();
                                alert(data.Message, 'Eliminar');
                            } else {
                                $('#InstaAsignadas').html(data);
                                hideLoading();
                            }
                        }
                    });
                
            }
        }
    });


}

function VolverNivelInst()
{ 
    showLoading();
    $.ajax({
        type: "POST",
        url: '/Niveles/VisualizarNivel',
        data: { id: $('input#IdValor').val(), Accion: "Ver" },
        success: function (data) {
            if (data) {
                hideLoading();
                $('#ListNiveles').html(data);

            }
        }
    });


}

