
function AltaEditarInstalacion(action) {
    var InstalacionJson = {
        "Id":        $('input#Id01').val(),
        "Nombre":    $('input#NombreInstalacion01.form-control').val(),
        "id_Puerto": $('input#Puerto01').val(),
        "id_Tipo":   $('input#Tipo01').val(),
        "es_concesionada": $('input#Concesionada01').val(),
        "Empresa":         $('input#Empresa01').val(),
        "Clasificacion":   $('input#clasificacion01').val(),
        "Direccion":       $('input#DireccionInstalacion01').val(),
        "ID_Ciudad":       $('input#Ciudad01').val(),
        "Cod_Postal":      $('input#codPostal01').val(),
        "ID_Provincia":    $('input#Provincias01').val(),
        "OMI":             $('input#omi01').val(),
        "Nivel":           $('input#Nivel01').val(),
        "Longitud":        $('input#longitud01').val(),
        "Latitud":         $('input#latitud01').val(),
        "Declara_Cumpli":  $('input#declaracion01').val(),
        "Fech_Declara_Cumpli": $('input#datetimepicker').val(),
        "Activo":           $('input#activo02').val(),
        "Observaciones":    $('textarea#Observaciones01').val(),
        "action":           action
    };
    var o = eval(InstalacionJson);
     if (o.Nombre && o.id_Puerto && o.id_Tipo && o.es_concesionada && o.Clasificacion && o.Nivel && $('input#ValidarAllCombos').val()) {
        jsShowWindowLoad("Ejecutando datos.....");
        $.ajax({
            url: '/Instalaciones/AltaEditarInstalaciones',
            type: "POST",
            data: JSON.stringify({ InstalacionJson: InstalacionJson }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                if (data.result == 0) {
                    showLoading();
                    $.ajax({
                        type: "POST",
                        url: '/Instalaciones/VisualizarInstalacion',
                        data: { id: null },
                        success: function (data) {
                            if (data) {
                                hideLoading();
                                $('#MostrarMensaje').html('Visualizar Instalaciones');
                                $('#Edit-Instalacion').html('');
                                $('#Edit-Instalacion').html(data);
                                jsRemoveWindowLoad();
                            }
                        }
                    });
                } else if (data.result == 1) {
                    showLoading();
                    alert(data.Message, "Comites");
                } else if (data.result == 2) {
                    showLoading();
                    alert(data.Message, 'Confirm',0,52);
                } else {
                    showLoading();
                    alert(data.Message, 'Confirm',0,53);
                }
            }

        });
    }
}

function CambiarProvincia() {
    showLoading();
    $.ajax({
        type: "POST",
        url: '/Instalaciones/CambiarProvincia',
        data: { id: $('input#Provincias01').val() },
        success: function (data) {
            if (data) {
                ValidacionCombos(document.getElementById('Provincias01'), 'errorProvincia');
                hideLoading();
                $('#ComboCiudad').html(data);
            }
        }
    });
}


function CambiarTipo() {
    showLoading();
    $.ajax({
        type: "POST",
        url: '/Instalaciones/CambiarTipo',
        data: { id: $('input#clasificacion01').val() },
        success: function (data) {
            if (data) {
                ValidacionCombos(document.getElementById('clasificacion01'), 'errorClasifica');
                hideLoading();
                $('#ComboTipo').html(data);
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

    if (!ValidacionCombos(document.getElementById('Puerto01'), 'errorPuerto'))
        comboValida = false;
    if (!ValidacionCombos(document.getElementById('clasificacion01'), 'errorClasifica'))
        comboValida = false;
    if (!ValidacionCombos(document.getElementById('Concesionada01'), 'errorConcesion'))
        comboValida = false;
    if (!ValidacionCombos(document.getElementById('Tipo01'), 'errorTipo'))
        comboValida = false;   
    if (!ValidacionCombos(document.getElementById('Provincias01'), 'errorProvincia'))
        comboValida = false;
    if (!ValidacionCombos(document.getElementById('Ciudad01'), 'errorCiudad'))
        comboValida = false;
    if (!ValidacionCombos(document.getElementById('Nivel01'), 'errorNivel'))
        comboValida = false;
    if (!ValidacionCombos(document.getElementById('declaracion01'), 'errorDeclara'))
        comboValida = false;
    if (estado == "Edit")
        if (!ValidacionCombos(document.getElementById('activo02'), 'errorActivo'))
            comboValida = false;

    return comboValida;
}
