var mensaje = "Datos guardados correctamente";
function AltaEditarAuditoria(action) {
    //errorPuerto.style.display = "none";
    //errorInsta.style.display = "none";
    //errorTipo.style.display = "none";
    //errorEstado.style.display = "none";
    var auditoriasJson = {
        "Id": $('input#ID01').val(),
        "Id_Puerto": $('input#Puerto01').val(),
        "Id_IIPP": $('input#Instalacion01').val(),
        "Descripcion": $('input#Descripcion01').val(),
        "Tipo": $('input#Tipo01').val(),
        "Estado": $('input#Estado01').val(),
        "Ini_Programa" : $('input#IniPrograma01').val(),
        "Fin_Programa": $('input#FinPrograma01').val(),
        "Ini_Planifica": $('input#IniPlanifica01').val(),
        "Fin_Planifica": $('input#FinPlanifica01').val(),
        "Ini_Real": $('input#IniReal01').val(),
        "Fin_Real": $('input#FinReal01').val(),
        "Responsable": $('input#Responsable01').val(),
        "Conclusiones": $('textarea#Conclusiones01').val(),
        "Recomendaciones": $('textarea#Recomendaciones01').val(),
        "Programa": $('textarea#Programa01').val(),
        "Seguimiento": $('textarea#Seguimiento01').val(),
        "Observaciones": $('textarea#Observaciones01').val(),
        "Validar": $('input#Validar').val(),
        "action": action
    };
    var o = eval(auditoriasJson);

    if (o.Id_Puerto && o.Descripcion && o.Tipo && o.Estado && o.Validar && $('input#ValidarAllCombos').val()) {
        $.ajax({
            url: '/Auditorias/AltaEditarAuditorias',
            type: "POST",
            data: JSON.stringify({ AuditoriasJson: auditoriasJson }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                if (data.result == 0) {
                    //window.location.reload();
                    alert(data.Message, 'Eliminar');
                } else if (data.result == 1) {
                    showLoading();
                    $.ajax({
                        type: "POST",
                        url: '/Auditorias/VisualizarAuditorias',
                        data: { id: $('input#IdValor').val(), Accion: "Ver" },
                        success: function (data) {
                            if (data) {
                                hideLoading();
                                $('#ListAuditorias').html(data);
                            }
                        }
                    });
                }
            }
        });
    }
}

function VolverAuditorias() {
    $('input#Puerto01').val('');
    $('input#Instalacion01').val('');
    $('div#myModalAuditorias.modal.fade.in').attr('style', 'display:none');
    hideLoading();
    $('#DatosAuditorias').html('');
    window.location.reload();
}

function CambiarPuerto() {    
    showLoading();
    $.ajax({
        type: "POST",
        url: '/Auditorias/CambiarCentro',
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

function ValidarDescripcion() {
    if (!ValidarTexto($('input#Descripcion01').val())) {
        datosDescripcion.style.display = "";
        return false;
    }
    else {
        datosDescripcion.style.display = "none";
        return true;
    }
}

function ValidarTipo() {
    if (!ValidarTexto($('input#Tipo01').val())) {
        datosTipo.style.display = "";
        return false;
    }
    else {
        datosTipo.style.display = "none";
        return true;
    }
}

function ValidarEstado() {
    if (!ValidarTexto($('input#Estado01').val())) {
        datosEstado.style.display = "";
        return false;
    }
    else {
        datosEstado.style.display = "none";
        ValidarFecha();
        return true;
    }
}

function ValidarPuerto()
{
    if (!ValidarCombo($('input#Puerto01').val())) {
        datosPuerto.style.display = "";
        return false;
    }
    else {
        datosPuerto.style.display = "none";
        return true;
    }
}

function ValidarCentro()
{
    if (!ValidarCombo($('input#Instalacion01').val())) {
        datosCentro.style.display = "";
        return false;
    }
    else {
        datosCentro.style.display = "none";
        return true;
    }

}

function ValidarFecha(fecha, campoerror)
{
    var estado = $('input#Estado01').val();
    if (estado == 1) {
        if (ValidarMayor($('input#IniPrograma01').val(), $('input#FinPrograma01').val()))
            datosFinValidaPro.style.display = "none";
        else
            datosFinValidaPro.style.display = "";
    } else if (estado == 2) {
        if (ValidarMayor($('input#IniPlanifica01').val(), $('input#FinPlanifica01').val()))
            datosFinValidaPla.style.display = "none";
        else
            datosFinValidaPla.style.display = "";
    } else if (estado == 3) {
        if (ValidarMayor($('input#IniReal01').val(), $('input#FinReal01').val()))
            datosFinValidaRea.style.display = "none";
        else
            datosFinValidaRea.style.display = "";
    }
}

function CambiarEstado(estado) {
    if (ValidacionCombos(document.getElementById('Estado01'), 'errorEstado')) {
        showLoading();
        $.ajax({
            type: "POST",
            url: '/Auditorias/CambiarEstadoPlan',
            data: { fechaIni: $('input#IniPlanifica01').val(), fechaFin: $('input#FinPlanifica01').val(), Estado: estado.value },
            success: function (data) {
                if (data) {
                    hideLoading();
                    $('#EstadoPlanifica').html(data);
                    $.ajax({
                        type: "POST",
                        url: '/Auditorias/CambiarEstadoReal',
                        data: { fechaIni: $('input#IniReal01').val(), fechaFin: $('input#FinReal01').val(), Estado: estado.value },
                        success: function (data) {
                            if (data) {
                                hideLoading();
                                $('#EstadoReal').html(data);
                                $.ajax({
                                    type: "POST",
                                    url: '/Auditorias/CambiarEstadoProm',
                                    data: { fechaIni: $('input#IniPrograma01').val(), fechaFin: $('input#FinPrograma01').val(), Estado: estado.value },
                                    success: function (data) {
                                        if (data) {
                                            hideLoading();
                                            $('#EstadoProm').html(data);
                                        }
                                    }
                                });

                            }
                        }
                    });
                }
            }
        });
    }
}

function ValidarMayor(Inicial, final) {
    var fechaInicio = Inicial;
    var fechaFin = final;
    if (fechaFin != "") {
        if (fechaInicio != "") {
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
            $('input#Validar').val("");
            return false;
        }
    } else {
        $('input#Validar').val("1");
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

    function ValidarAllCombos()
    {
        var comboValida = true;

        if (!ValidacionCombos(document.getElementById('Puerto01'), 'errorPuerto'))
            comboValida = false;
        if(!ValidacionCombos(document.getElementById('Instalacion01'), 'errorInsta'))
            comboValida = false;
        if (!ValidacionCombos(document.getElementById('Tipo01'), 'errorTipo'))
            comboValida = false;
        if (!ValidacionCombos(document.getElementById('Estado01'), 'errorEstado'))
            comboValida = false;

        return comboValida;
    }


    //Documentos
    function VolverDocAuditoria() {
        $.ajax({
            url: '/Auditorias/AsociadosEditAuditorias',
            type: "POST",
            data: { id: 1 },
            success: function (data) {
                if (data) {
                    $('#MostrarMensaje').html('Visualizar Auditoria');
                    $('#myModalUpload').modal('hide');
                    $('#DocumentosRefrescar').html(data);
                    $('#Edit-page').html('');
                    hideLoading();
                }
            }
        });
    }
    function onChangeEditDocAuditoria(e) {
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
            url: '/Auditorias/GuardarDocumento',
            type: "POST",
            data: { DocumentoJson: DocumentoJson },
            success: function (data) {
                if (data.result) {
                    $.ajax({
                        type: "POST",
                        url: '/Auditorias/AsociadosEditAuditorias',
                        data: { id: 1 },
                        success: function (data) {
                            if (data) {
                                $('#MostrarMensaje').html('Visualizar Auditoria');
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
            url: '/Auditorias/DeshacerCambios',
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
    function VisualizarDocAuditoria(Id) {
        $.ajax({
            type: "POST",
            url: '/Auditorias/AsociadosEditAuditorias',
            data: { id: Id },
            success: function (data) {
                if (data) {
                    $('#MostrarMensaje').html('Visualizar Auditoria');
                    $('#myModalUpload').modal('hide');
                    $('#DocumentosRefrescar').html(data);
                    $('#Edit-page').html('');
                    hideLoading();
                }
            }
        });
    }


