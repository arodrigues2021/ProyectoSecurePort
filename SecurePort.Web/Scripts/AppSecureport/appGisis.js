var mensaje = "Datos guardados correctamente";


function AltaEditarGisis(action) {
    var gisisJson = {
        "Id": $('input#Id01').val(),
        "Id_Puerto": $('input#Puerto01').val(),
        "Id_IIPP": $('input#Instalacion01').val(),
        "Tipo_Registro": $('input#TipoRegistro01').val(),
        "Fecha_Registro": $('input#FechaRegistrado01').val(),
        "Motivo": $('textarea#Motivo01').val(),
        "action": action
    };  
    var o = eval(gisisJson);
    if (o.Id_Puerto && o.Tipo_Registro && o.Fecha_Registro && $('input#ValidarAllCombos').val()) {
        jsShowWindowLoad("Ejecutando datos.....");
        $.ajax({
            url: '/Gisis/AltaEditarGisis',
            type: "POST",
            data: JSON.stringify({ GisisJson: gisisJson }),
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
                        url: '/Gisis/VisualizarGisis',
                        data: { id: $('input#IdValor').val(), Accion: "Ver" },
                        success: function (data) {                            
                            if (data) {
                                hideLoading();
                                $('#ListGisis').html(data);
                                jsRemoveWindowLoad();
                            }
                        }
                        });
                } else {
                    if (data.status == "error") {
                        alert(data.Message, "Mensaje");
                        $.ajax({
                            type: "POST",
                            url: '/Gisis/VisualizarGisis',
                            data: { id: $('input#IdValor').val(), Accion: "Ver" },
                            success: function (data) {                                
                                if (data) {
                                    hideLoading();
                                    $('#ListGisis').html(data);
                                    jsRemoveWindowLoad();
                                }
                            }
                        });
                    } else {
                        alert(data.Message);
                    }
                }

            }
        });
    }
}


function VolverGisis() {

    $('input#Puerto01').val('');
    $('div#myModalGisis.modal.fade.in').attr('style', 'display:none');
    hideLoading();
    $('#DatosGisis').html('');
    window.location.reload();

}

function CambiarPuerto() {
    ValidarPuerto();
    if (ValidacionCombos(document.getElementById('Puerto01'), 'errorPuerto')) {
        showLoading();
        $.ajax({
            type: "POST",
            url: '/Gisis/CambiarPuerto ',
            data: { id: $('input#Puerto01').val() },
            success: function (data) {
                if (data) {
                    hideLoading();
                    $('#ComboTipo').html(data);
                }
            }
        });
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

function ValidarFechaRegistro() {
    if (!ValidarTexto($('input#FechaRegistrado01').val())) {
        datosFechaRegistro.style.display = "";
    }
    else {        
        datosFechaRegistro.style.display = "none";
    }
}


function ValidarTipoRegistro() {
    if (!ValidarTexto($('input#TipoRegistro01').val())) {
        datosTipoRegistro.style.display = "";
    }
    else {
        ValidacionCombos(document.getElementById('TipoRegistro01'), 'errorTipo');
        datosTipoRegistro.style.display = "none";
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
    if (!ValidacionCombos(document.getElementById('Instalacion01'), 'errorInsta'))
        comboValida = false;
    if (!ValidacionCombos(document.getElementById('TipoRegistro01'), 'errorTipo'))
        comboValida = false;    
    return comboValida;
}


function sleepFor(sleepDuration) {
    var start = new Date().getTime();
    for (var i = 0; i < 1e7; i++) {
        if ((new Date().getTime() - start) > sleepDuration) {
            break;
        }
    }
}