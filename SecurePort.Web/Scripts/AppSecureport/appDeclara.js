var mensaje = "Datos guardados correctamente";

function AltaEditarDeclara(action) {
    var declaraMaritimaJson = {        
        "Id": $('input#ID01').val(),
        "Id_Organismo": $('input#Organismo01').val(),
        "Id_Puerto": $('input#Puerto01').val(),
        "Id_IIPP": $('input#Instalacion01').val(),
        "IMO": $('input#IMO01').val(),
        "Buque": $('input#Buque01').val(),
        "Id_Motivo": $('input#Motivos01').val(),
        "Fech_Expe": $('input#fechaexp01').val(),
        "Fech_Ini_Validez": $('input#Inicio01').val(),
        "Fech_Fin_Validez": $('input#Fin01').val(),
        "Responsable": $('input#Responsable01').val(),
        "Nivel_Buq": $('input#Nivel01').val(),
        "Nivel_IIPP": $('input#Nivel02').val(),
        "Medidas": $('textarea#Medidas01').val(),
        "Observaciones": $('textarea#Observaciones01').val(),
        "Validar": $('input#Validar').val(),
        "action": action
    };
    var o = eval(declaraMaritimaJson);
    if (o.Id_IIPP && o.Id_Motivo && o.Validar && $('input#ValidarAllCombos').val()) {
        $.ajax({
            url: '/DeclaraMaritima/AltaEditarDeclaraMaritima',
            type: "POST",
            data: JSON.stringify({ DeclaraMaritimaJson: declaraMaritimaJson }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                if (data.result==0) {
                    alert(data.Message, 'Mensaje');
                } else if (data.result==1) {
                    showLoading();
                    $.ajax({
                        url: '/DeclaraMaritima/VisualizarDeclaraMaritima',
                        type: "POST",
                        data: { id: $('input#IdValor').val(), Accion: "Ver" },
                        success: function (reg) {
                            if (data) {
                                hideLoading();
                                $('#MostrarMensaje').html('Visualizar Declaraciones de Protección Marítima');
                                $('#DatosDeclaraMaritima').html('');
                                $('#DatosDeclaraMaritima').html(reg);
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


function VolverDeclaraMaritima() {

    hideLoading();
    window.location.reload();

}

function CambiarOrganismo() {
    showLoading();
    $.ajax({
        type: "POST",
        url: '/DeclaraMaritima/CambiarPuerto',
        data: { id: $('input#Organismo01').val() },
        success: function (data) {
            if (data) {
                hideLoading();
                ValidacionCombos(document.getElementById('Organismo01'), 'errorOrganismo');
                $('#ComboPuertos').html(data);
                CambiarPuerto();
            }
        }
    });
}

function ValidarOrganismo() {

    if (!ValidarCombo($('input#Organismo01').val())) {
        datosOrganismo.style.display = "";
    }
    else
        datosOrganismo.style.display = "none";
}

function CambiarPuerto() {
    showLoading();
    $.ajax({
        type: "POST",
        url: '/DeclaraMaritima/CambiarCentro',
        data: { id: $('input#Puerto01').val() },
        success: function (data) {
            if (data) {
                hideLoading();
                ValidacionCombos(document.getElementById('Puerto01'), 'errorPuerto');
                $('#ComboInsta').html(data);
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
    if (!ValidarTexto($('input#fechaexp01').val())) {
        datosInicio.style.display = "";
        datosValidaInicio.style.display = "none";
        datosValidaFin.style.display = "none";
    }
    else {
        datosInicio.style.display = "none";
        FechaValidar();

    }
}


function ValidarInstalacion() {
    if (!ValidarCombo($('input#Instalacion01').val())) {
        datosInsta.style.display = "";
    }
    else {
        ValidacionCombos(document.getElementById('Instalacion01'), 'errorInsta');
        datosInsta.style.display = "none";
    }
}

function FechaValidar() {

    if (ValidarMayor($('input#fechaexp01').val(), $('input#Inicio01').val(), 'input#Validar'))
        datosValidaInicio.style.display = "none";
    else
        datosValidaInicio.style.display = "";

    if (ValidarMayor($('input#Inicio01').val(), $('input#Fin01').val(), 'input#ValidarFin'))
        datosValidaFin.style.display = "none";
    else
        datosValidaFin.style.display = "";

    if ($('input#Validar').val() != "" && $('input#ValidarFin').val() != "")
        $('input#Validar').val("1");
    else
        $('input#Validar').val("");
        
}

function ValidarMayor(Inicial, final, entrada) {
    var fechaInicio = Inicial;
    var fechaFin = final;
    if (fechaFin != "") {
        if (fechaInicio != "") {
            fechaInicio = fechaInicio.split('/');
            fechaFin = fechaFin.split('/');

            fechaInicio = fechaInicio[1].toString() + "/" + fechaInicio[0].toString() + "/" + fechaInicio[2].toString();
            fechaFin = fechaFin[1].toString() + "/" + fechaFin[0].toString() + "/" + fechaFin[2].toString();

            if ((Date.parse(fechaInicio)) > (Date.parse(fechaFin))) {
                $(entrada).val("");
                return false;
            }
            else {
                $(entrada).val("1");
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

function ValidarAllCombos() {
    var comboValida = true;

    if (!ValidacionCombos(document.getElementById('Puerto01'), 'errorPuerto'))
        comboValida = false;
    if (!ValidacionCombos(document.getElementById('Instalacion01'), 'errorInsta'))
        comboValida = false;
    if (!ValidacionCombos(document.getElementById('Organismo01'), 'errorOrganismo'))
        comboValida = false;
    if (!ValidacionCombos(document.getElementById('Motivos01'), 'errorMotivo'))
        comboValida = false;
    if (!ValidacionCombos(document.getElementById('Nivel01'), 'errorNivel'))
        comboValida = false;
    if (!ValidacionCombos(document.getElementById('Nivel02'), 'errorPro'))
        comboValida = false;

    return comboValida;
}



// docuemntacion
function VolverDocDeclaraMaritima() {
    $.ajax({

        url: '/DeclaraMaritima/AsociadosEditDeclara',
        type: "POST",
        data: { id: 1 },
        success: function (data) {
            if (data) {
                $('#MostrarMensaje').html('Visualizar Declaraciones de Protección Marítima');
                $('#myModalUpload').modal('hide');
                $('#DocumentosRefrescar').html(data);
                $('#Edit-page').html('');
                hideLoading();
            }
        }
    });
}

function onChangeEditDocDeclara(e) {
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
        url: '/DeclaraMaritima/GuardarDocumento',
        type: "POST",
        data: { DocumentoJson: DocumentoJson },
        success: function (data) {
            if (data.result) {
                $.ajax({
                    type: "POST",
                    url: '/DeclaraMaritima/AsociadosEditDeclara',
                    data: { id: 1 },
                    success: function (data) {
                        if (data) {
                            $('#MostrarMensaje').html('Visualizar Declaraciones de Protección Marítima');
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
        url: '/DeclaraMaritima/DeshacerCambios',
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
        url: '/DeclaraMaritima/AsociadosEditDeclara',
        data: { id: Id },
        success: function (data) {
            if (data) {
                $('#MostrarMensaje').html('Visualizar Declaraciones de Protección Marítima');
                $('#myModalUpload').modal('hide');
                $('#DocumentosRefrescar').html(data);
                $('#Edit-page').html('');
                hideLoading();
            }
        }
    });

}