
var BajoRiesgo = false;

function GenerarAccionesChange() {
    if ($('input#IdValor').val() == "") {
        alert('Debe Seleccionar un registro');
    } else {
        if ($('input#IdClasifica').val() == "5") {
            alert('No Se Puede Realizar Acciones Sobre Las Instalaciones de Categoria de Zonas Adyacentes');
        } else {
        if ($('input#Accion01').val() == "2") {
            showLoading();
            $.ajax({
                type: "POST",
                url: '/Evaluaciones/AsignarBienAmenaza',
                data: { id: $('input#IdValor').val() },
                success: function (data) {
                    if (data) {
                        hideLoading();
                        $('#Edit-Instalacion').html('');
                        $('#Edit-Instalacion').html(data);
                    }
                }
            });
        }
        else {
                showLoading();
                $.ajax({
                    type: "POST",
                    url: '/Evaluaciones/ListaBienesAmenazas',
                    data: { id: $('input#IdValor').val(),origen: '1' },
                    success: function (data) {
                        if (data) {
                            hideLoading();
                            $('#Edit-Instalacion').html('');
                            $('#Edit-Instalacion').html(data);
                        }
                    }
                });

        }
    }
    }

}

function VolverEvaluacion(origen) {
    if (origen == "1") {
        window.location.reload();
    } else {
        showLoading();
        $.ajax({
            type: "POST",
            url: '/Evaluaciones/AsignarBienAmenaza',
            data: { id: null },
            success: function(data) {
                if (data) {
                    hideLoading();
                    $('#Edit-Instalacion').html('');
                    $('#Edit-Instalacion').html(data);
                    $('#MostrarMensaje').html('Asignar Bienes/Amenazas');

                }
            }
        });


    }

}

function VisualizarVolverBien() {
    $.ajax({
        type: "POST",
        url: '/Evaluaciones/ListaBienesAmenazas',
        data: { id: $('input#IdValor').val(), origen: $('input#Origen').val() },
        success: function (data) {
            if (data) {
                hideLoading();
                $('#Edit-Instalacion').html('');
                $('#Edit-Instalacion').html(data);
                $('#myModalAmenzas').modal('hide');
                hideLoading();   
            }
        }
    });

}

function onChangeAmenaza() {

    //Seleccionamos el registro
    var nombre = this.dataItem(this.select()).Bien;
    var id = this.dataItem(this.select()).Id_Bien;
    $('input#IdNombreAmenaza').val(nombre);
    $('input#IdValorAmenaza').val(id);
}

function onToggleInstalacionBien(e) {
    //Aqui controlamos los eventos de la toolbar.
    if (e.id == "toggle1") {
        //VISUALIZAR AMENAZAS - BIEN     
        if ($('input#IdValorAmenaza').val() == "") {
            alert('Debe Seleccionar un registro', "Mensaje");
        } else {
            showLoading();
            $.ajax({
                type: "POST",
                url: '/Evaluaciones/VisualizarAmenazas',
                data: { id: $('input#IdValorAmenaza').val() },
                success: function (data) {
                    if (data) {
                        hideLoading();
                         $('#AsignarAmenazas').html('');
                        $('#AsignarAmenazas').html(data);
                        $('#myModalAmenzas').modal('show');
                    }
                }
            });
        }
    }

}

function DetailExpand(arg) {
    var grid = arg.detailRow.find("[data-role=grid]").data("kendoGrid");
    var secondGrid = $("#GridInstalaciones").data("kendoGrid");
    
    var padre = grid._cellId.split('_');
    var LastDetailsRow;

    var posicion = 0;
    var dataItem = 0;
    for (posicion; posicion < secondGrid.dataSource._data.length; posicion++){
        if (secondGrid.dataSource._data[posicion].Id_Bien.toString() == padre[1])
            dataItem = posicion;       
      }

    var uid = secondGrid.dataSource.at(dataItem).uid;
    secondGrid.select("tr[data-uid='" + uid + "']");

}


//Evaluaciones
function AltaEditarEvaluacion(action) {
    var evaluacionesJson = {
        "Id": $('input#Id01').val(),
        "Nombre": $('input#Nombre01').val(),
        "Id_Puerto": $('input#Puerto01').val(),
        "Id_IIPP": $('input#Instalacion01').val(),
        "Responsable": $('input#Responsable01').val(),
        "Cargo": $('input#Cargo01').val(),
        "Observaciones": $('textarea#Observaciones01').val(),
        "Version": $('input#Version01').val(),
        "action": action
    };    
    var o = eval(evaluacionesJson);
    if (o.Nombre && o.Id_Puerto && o.Responsable && o.Cargo && $('input#ValidarAllCombos').val()) {        
        $.ajax({
            url: '/Evaluaciones/AltaEditarEvaluacion',
            type: "POST",
            data: JSON.stringify({ EvaluacionesJson: evaluacionesJson }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                if (data.result == 0) {                   
                    window.location.reload();
                } else if (data.result == 1) {
                    showLoading();
                    $.ajax({
                        type: "POST",
                        url: '/Evaluaciones/VisualizarEvaluacion',
                        data: { id: $('input#IdValor').val(), Accion: "Ver" },
                        success: function (data) {
                            if (data) {
                                hideLoading();
                                $('#Edit-Instalacion').html(data);                                
                            }
                        }
                    });
                } else {
                    
                        alert(data.Message);                    
                }

            }
        });
    }
}


function VolverEvaluacionBien() {

showLoading();
$.ajax({
    type: "POST",
    url: '/Evaluaciones/VisualizarEvaluacion',
    data: { id: $('input#IdEvalua').val(), Accion: "Ver" },
    success: function (data) {
        if (data) {
            hideLoading();
            $('#Edit-Instalacion').html(data);
        }
    }
});

}

function CambiarPuerto() {
    ValidarPuerto();
    if (ValidacionCombos(document.getElementById('Puerto01'), 'errorPuerto')) {
        showLoading();
        $.ajax({
            type: "POST",
            url: '/Evaluaciones/CambiarPuerto ',
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

function ValidarNombre() {
    if (!ValidarTexto($('input#Nombre01').val())) {
        datosNombre.style.display = "";
    }
    else {
        datosNombre.style.display = "none";
    }
}


function ValidarEstado() {
    if (!ValidarTexto($('input#Estado01').val())) {
        datosEstado.style.display = "";
    }
    else {
        ValidacionCombos(document.getElementById('Estado01'), 'errorEstado');
        datosEstado.style.display = "none";
    }
}

function ValidarResponsable() {
    if (!ValidarTexto($('input#Responsable01').val())) {
        datosResponsable.style.display = "";
    }
    else {
        datosResponsable.style.display = "none";
    }
}

function ValidarCargo() {
    if (!ValidarTexto($('input#Cargo01').val())) {
        datosCargo.style.display = "";
    }
    else {
        datosCargo.style.display = "none";
    }
}



function ValidarPuerto() {
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
    return comboValida;
}

// Fin Evaluaciones

//Documentos
function VolverDocEvaluacion() {
    $.ajax({

        url: '/Evaluaciones/AsociadosEditEvaluaciones',
        type: "POST",
        data: { id: 1 },
        success: function (data) {
            if (data) {
                $('#MostrarMensaje').html('Visualizar Evaluación');
                $('#myModalUpload').modal('hide');
                $('#DocumentosRefrescar').html(data);
                $('#Edit-page').html('');
                hideLoading();
            }
        }
    });
}

function onChangeEditDocEvaluacion(e) {
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
        url: '/Evaluaciones/GuardarDocumento',
        type: "POST",
        data: { DocumentoJson: DocumentoJson },
        success: function (data) {
            if (data.result) {
                $.ajax({
                    type: "POST",
                    url: '/Evaluaciones/AsociadosEditEvaluaciones',
                    data: { id: 1 },
                    success: function (data) {
                        if (data) {
                            $('#MostrarMensaje').html('Visualizar Evaluación');
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
        url: '/Evaluaciones/DeshacerCambios',
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

function VisualizarDocEvaluacion(Id) {
    $.ajax({
        type: "POST",
        url: '/Evaluaciones/AsociadosEditEvaluaciones',
        data: { id: Id },
        success: function (data) {
            if (data) {
                $('#MostrarMensaje').html('Visualizar Evaluación');
                $('#myModalUpload').modal('hide');
                $('#DocumentosRefrescar').html(data);
                $('#Edit-page').html('');
                hideLoading();
            }
        }
    });

}

// fin documentos

//Historico

function onChangeHisEvaluacion(e) {
    //Seleccionamos el registro
    var code = this.dataItem(this.select()).id;
    var documento = this.dataItem(this.select()).Id_Version;
    $('input#IdHis').val(code);
    $('input#VesionEva').val(documento);
}

function DetalleExpand(arg) {
    var grid = arg.detailRow.find("[data-role=grid]").data("kendoGrid");
    var secondGrid = $("#grid_00").data("kendoGrid");
    var padre = grid._cellId.split('_');
    var LastDetailsRow;
    var posicion = 0;
    var dataItem = 0;
    for (posicion; posicion < secondGrid.dataSource._data.length; posicion++) {
        if (secondGrid.dataSource._data[posicion].Id.toString() == padre[1])
            dataItem = posicion;
    }
    var uid = secondGrid.dataSource.at(dataItem).uid;
    secondGrid.select("tr[data-uid='" + uid + "']");

}

/// Matriz de evaluacion 
function GenerarAccionesEvaluaChange() {
    if ($('input#Accion01').val() == "2") {
        showLoading();
        $.ajax({
            type: "POST",
            url: '/Evaluaciones/GenerarMatriz',
            data: { id: $('input#IdValor').val(), accion: 'Edit' },
            success: function(data) {
                if (data) {
                    hideLoading();
                    // $('#Edit-Instalacion').html('');
                    $('#Edit-Instalacion').html(data);
                }
            }
        });
    } else {
        showLoading();
        $.ajax({
            type: "POST",
            url: '/Evaluaciones/GenerarMatriz',
            data: { id: $('input#IdValor').val(), accion: 'Ver' },
            success: function(data) {
                if (data) {
                    hideLoading();
                    $('#Edit-Instalacion').html('');
                    $('#Edit-Instalacion').html(data);
                }
            }
        });
    }

}

function onChangeInstalacion() {
    //Seleccionamos el registro
    var nombre = this.dataItem(this.select()).Nombre;
    var id = this.dataItem(this.select()).Id;  
    $('input#IdNombreIns').val(nombre);
    $('input#IdValorIns').val(id);
    this.collapseRow(this.tbody.find(":not(tr.k-state-selected)"));
}

function onChangeBien() {
    //Seleccionamos el registro
    var nombre = this.dataItem(this.select()).Bien;
    var id = this.dataItem(this.select()).Id_Bien;
    var Namenazas = this.dataItem(this.select()).NumeroAmenazas;
    if (Namenazas == 0) {
        alert('No existen amenazas para el bien ' + nombre + ' perteneciente a la instalación ' + $('input#IdNombreIns').val(), 'Mensaje');
    } else {
        $('input#IdNombreBien').val(nombre);
        $('input#IdValorBien').val(id);
        showLoading();
        $.ajax({
            url: '/Evaluaciones/BuscadorBienAmenazasInstalacion1',
            type: "POST",
            data: { BienId: $('input#IdValorBien').val(), InstalacionId: $('input#IdValorIns').val(), NombreIns: $('input#IdNombreIns').val(), nombreBien: $('input#IdNombreBien').val() },
            success: function (data) {
                if (data) {
                    hideLoading();
                    $('#AsignarAmenazas').html('');
                    $('#AsignarAmenazas').html(data);
                    $('#myModalAmenazas').modal('show');
                }
            }
        });
    }
}

function DetailExpandIns(arg) {    
    var grid = arg.detailRow.find("[data-role=grid]").data("kendoGrid");
    var secondGrid = $("#GridInstalaciones").data("kendoGrid");
    var padre = grid._cellId.split('_');
    var LastDetailsRow;
    var posicion = 0;
    var dataItem = 0;
    for (posicion; posicion < secondGrid.dataSource._data.length; posicion++) {
        if (secondGrid.dataSource._data[posicion].Id.toString() == padre[1]) {
            dataItem = posicion;
            $('input#IdNombreIns').val(secondGrid.dataSource._data[posicion].Id);
            $('input#IdNombreBien').val(secondGrid.dataSource._data[posicion].Nombre);
        }
    }
    //mantenemos en id del bien selecionado anteriormente
    if ($('input#IdBienAnt').val() != $('input#IdNombreIns').val()) {
       // QuitarSelecionHijos($('input#IdBienAnt').val());
        $('input#IdBienAnt').val($('input#IdNombreIns').val());        
    }
    var uid = secondGrid.dataSource.at(dataItem).uid;
    secondGrid.select("tr[data-uid='" + uid + "']");

}

function volverAmenazasIns()
{
    hideLoading();
    $('#AsignarAmenazas').html('');
    //$('#AsignarAmenazas').html(data);
    $('#myModalAmenazas').modal('hide');
}

function QuitarSelecionHijos(Id)
{   
    var grid = document.getElementById("grid_" + Id);
    if (grid != null) {
        //var tercerGrid = $("#grid_" + Id).data("kendoGrid");
        var tercerGrid = $("#GridInstalaciones").data("kendoGrid");
        tercerGrid.tbody.find("tr.k-grouping-row").each(function (index) {
            tercerGrid.collapseGroup(this);
        });       
    }  
}


function ModificarDetalle(arg)
{
    var amenazaID = arg.id.split('|');
    var campo = amenazaID[0].substring(0, amenazaID[0].length - 1);
    var valor = arg.checked;
    var posicion = 0;
    for (posicion; posicion < TotalAmenazas.length; posicion++) {
        document.getElementById("resultado1_|" + TotalAmenazas[posicion].Id_Amenaza).style.display = "none";
        document.getElementById("resultado2_|" + TotalAmenazas[posicion].Id_Amenaza).style.display = "none";
        document.getElementById("resultado3_|" + TotalAmenazas[posicion].Id_Amenaza).style.display = "none";
        document.getElementById("titulo_|" + TotalAmenazas[posicion].Id_Amenaza).style.display = "none";
        if (TotalAmenazas[posicion].Id_Amenaza == amenazaID[1]) {
            var AmenazaPadre = TotalAmenazas[posicion];
            AmenazaPadre["" + campo + ""] = valor;
            AmenazaPadre.Estado = "M";
        }
    }

}

function ModificarDetalleText(arg) {    
    var amenazaID = arg.id.split('|');
    var CampoValido = true;
    if (amenazaID[2] == 1)
        CampoValido = ValidarTipo1(arg);
    if (amenazaID[2] == 2)
        CampoValido = ValidarTipo2(arg);
    if (amenazaID[2] == 3)
        CampoValido = ValidarTipo3(arg);
    if (amenazaID[2] == 0)
        CampoValido = ValidarTipo0(arg);
    if (CampoValido) {
        var campo = amenazaID[0].substring(0, amenazaID[0].length - 1);
        var valor = arg.value;
        var posicion = 0;
        for (posicion; posicion < TotalAmenazas.length; posicion++) {
            document.getElementById("resultado1_|" + TotalAmenazas[posicion].Id_Amenaza).style.display = "none";
            document.getElementById("resultado2_|" + TotalAmenazas[posicion].Id_Amenaza).style.display = "none";
            document.getElementById("resultado3_|" + TotalAmenazas[posicion].Id_Amenaza).style.display = "none";
            document.getElementById("titulo_|" + TotalAmenazas[posicion].Id_Amenaza).style.display = "none";
            if (TotalAmenazas[posicion].Id_Amenaza == amenazaID[1]) {
                var AmenazaPadre = TotalAmenazas[posicion]
                AmenazaPadre["" + campo + ""] = valor;
                AmenazaPadre.Estado = "M";
            }

        }
    }
}


function ModificarDetalleTextBajoRiesgo(arg) {
    var amenazaID = arg.id.split('|');
    var CampoValido = true;
    if (amenazaID[2] == 1)
        CampoValido = ValidarBajoRiesgo1(arg);
    if (amenazaID[2] == 2)
        CampoValido = ValidarBajoRiesgo2(arg);
    if (amenazaID[2] == 3)
        CampoValido = ValidarBajoRiesgo3(arg);
    if (amenazaID[2] == 0)
        CampoValido = ValidarTipo0(arg);
    if (CampoValido) {
        var campo = amenazaID[0].substring(0, amenazaID[0].length - 1);
        var valor = arg.value;
        var posicion = 0;
        for (posicion; posicion < TotalAmenazasBajoRiesgo.length; posicion++) {
            document.getElementById("resultado1_|" + TotalAmenazasBajoRiesgo[posicion].Id_Amenaza).style.display = "none";
            document.getElementById("resultado2_|" + TotalAmenazasBajoRiesgo[posicion].Id_Amenaza).style.display = "none";
            document.getElementById("resultado3_|" + TotalAmenazasBajoRiesgo[posicion].Id_Amenaza).style.display = "none";
            document.getElementById("titulo_|" + TotalAmenazasBajoRiesgo[posicion].Id_Amenaza).style.display = "none";
            if (TotalAmenazasBajoRiesgo[posicion].Id_Amenaza == amenazaID[1]) {
                var AmenazaPadre = TotalAmenazasBajoRiesgo[posicion];
                AmenazaPadre["" + campo + ""] = valor;
                AmenazaPadre.Estado = "M";
            }

        }
    }
}

function GuardarmatrizEvaluacion() {
    var detalle_EvaluacionViewModel = JSON.stringify({ 'detalle_EvaluacionViewModel': TotalAmenazas });
    $.ajax({
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        type: "POST",
        url: '/Evaluaciones/GuardarMatrizEvalua',
        data: detalle_EvaluacionViewModel,
        success: function (data) {
            if (data) {
                hideLoading();                
            }
        }
    });

}

function GuardarmatrizEvaluacionBajoRiesgo() {
    var detalle_EvaluacionViewModel = JSON.stringify({ 'detalle_EvaluacionViewModelBajoRiesgo': TotalAmenazasBajoRiesgo });
    $.ajax({
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        type: "POST",
        url: '/Evaluaciones/GuardarMatrizEvaluaBajoRiesgo',
        data: detalle_EvaluacionViewModel,
        success: function (data) {
            if (data) {
                hideLoading();
            }
        }
    });
}
function ValidarBajoRiesgo1(arg) {
    if (arg.value != "1" && arg.value != "2" && arg.value != "3") {
        Alerta("El valor debe estar entre 1 y 3");
        document.getElementById("" + arg.id + "").focus();
        return false;
    }
    else
        return true;

}
function ValidarBajoRiesgo2(arg) {
    if (arg.value != "1" && arg.value != "2" && arg.value != "3" && arg.value != "4") {
        Alerta("El valor debe estar entre 1 y 4");
        document.getElementById("" + arg.id + "").focus();
        return false;
    }
    else
        return true;

}
function ValidarBajoRiesgo3(arg) {
    if (arg.value != "1" && arg.value != "2" && arg.value != "3" && arg.value != "4" && arg.value != "5") {
        Alerta("El valor debe estar entre 1 y 5");
        document.getElementById("" + arg.id + "").focus();
        showLoading();
        return false;
    }
    else
        hideLoading();
        return true;

}

function ValidarTipo1(arg)
{   
    if (arg.value != 1 && arg.value != 2 && arg.value != 3 && arg.value != 4) {
        Alerta("El valor debe estar entre 1 y 4");
        document.getElementById("" + arg.id + "").focus();
        return false;
    }
    else
        return true;

}

function ValidarTipo2(arg) {

    if (arg.value != 1 && arg.value != 2 && arg.value != 3 && arg.value != 4 && arg.value != 5) {
        Alerta("El valor debe estar entre 1 y 5");
        document.getElementById("" + arg.id + "").focus();
        return false;
    }
    else
        return true;

}

function ValidarTipo3(arg) {
    if (arg.value != 1 && arg.value != 2 && arg.value != 3 && arg.value != 5) {
        Alerta("El valor debe ser 1, 2, 3 ó 5");
        document.getElementById("" + arg.id + "").focus();
        return false;
    }
    else
        return true;
}

function ValidarTipo0(arg) {
    var valido = true;
    if (arg.value == "")
        valido = false;
    else {
        var numero = parseInt(arg.value);
        if (numero < 0)
            valido = false;
    }
    if (!valido) {
        Alerta("El valor debe ser mayor o igual que cero(0)");
        document.getElementById("" + arg.id + "").focus();
        return false;
    }
    else
        return true;

}

function CalcularRiesgosBajo() {
    var detalle_EvaluacionViewModel = JSON.stringify({ 'detalle_EvaluacionViewModel': TotalAmenazasBajoRiesgo });
    $.ajax({
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        type: "POST",
        url: '/Evaluaciones/CalcularBajoRiesgo',
        data: detalle_EvaluacionViewModel,
        success: function (data) {
            if (data) {
                var lista = data.detalle_EvaluacionViewModel;
                var posicion = 0;
                for (posicion; posicion < lista.length; posicion++) {
                    document.getElementById("NP1_RESULTADO_|" + lista[posicion].Id_Amenaza).value = lista[posicion].NP1_RESULTADO;
                    document.getElementById("NP2_RESULTADO_|" + lista[posicion].Id_Amenaza).value = lista[posicion].NP2_RESULTADO;
                    document.getElementById("NP3_RESULTADO_|" + lista[posicion].Id_Amenaza).value = lista[posicion].NP3_RESULTADO;
                    document.getElementById("resultado1_|" + lista[posicion].Id_Amenaza).style.display = "block";
                    document.getElementById("resultado2_|" + lista[posicion].Id_Amenaza).style.display = "block";
                    document.getElementById("resultado3_|" + lista[posicion].Id_Amenaza).style.display = "block";
                    document.getElementById("titulo_|" + lista[posicion].Id_Amenaza).style.display = "block";
                }
            }
        }
    });
}

function CalcularRiesgosPanel() {
    var detalle_EvaluacionViewModel = JSON.stringify({ 'detalle_EvaluacionViewModel': TotalAmenazas });
    $.ajax({
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        type: "POST",
        url: '/Evaluaciones/CalcularRisgosPanel',
        data: detalle_EvaluacionViewModel,
        success: function (data) {           
            if (data) {
                var lista = data.detalle_EvaluacionViewModel;
                var posicion = 0;
                for (posicion; posicion < lista.length; posicion++) {
                    document.getElementById("NP1_RESULTADO_|" + lista[posicion].Id_Amenaza).value = lista[posicion].NP1_RESULTADO;
                    document.getElementById("NP2_RESULTADO_|" + lista[posicion].Id_Amenaza).value = lista[posicion].NP2_RESULTADO;
                    document.getElementById("NP3_RESULTADO_|" + lista[posicion].Id_Amenaza).value = lista[posicion].NP3_RESULTADO;
                    document.getElementById("resultado1_|" + lista[posicion].Id_Amenaza).style.display = "block";
                    document.getElementById("resultado2_|" + lista[posicion].Id_Amenaza).style.display = "block";
                    document.getElementById("resultado3_|" + lista[posicion].Id_Amenaza).style.display = "block";
                    document.getElementById("titulo_|" + lista[posicion].Id_Amenaza).style.display     = "block";                   
                }
            }
        }
    });
}

function Alerta(message) {
    //showLoading();
    var _this = this;
    $('div#alerta span#message').html(message);
    $('div#alerta').fadeIn(100);
    $('div#alerta button').click(function () {
        $('#alerta').attr('style', 'display:none');        
    });
}


function DeshacerAmenazas()
{
        showLoading();
        $('#myModalAmenazas').modal('hide');
        $.ajax({
            url: '/Evaluaciones/BuscadorBienAmenazasInstalacionDeshacer',
            type: "POST",
            success: function (data) {             
                if (data) {
                    $('#AsignarAmenazas').html('');
                    $('#AsignarAmenazas').html(data);
                }
            }
        });
        $('#myModalAmenazas').modal('show');
}