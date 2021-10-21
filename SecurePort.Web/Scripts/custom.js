var modalEsperePermiteCerrar = false;
var crearNodoSeleccionado = false;
var onSeleccionandoItem = false;

$(function () {
   

    $('#Guardar').bind('click', function () {
        $.post("/Inmuebles/GuardarContactos", { NombreContacto: $('#NombreContacto').val(), TelefonoMovil: $('#TelefonoMovil').val(), TelefonoParticular: $('#TelefonoParticular').val(), TelefonoProfesional: $('#TelefonoProfesional').val(), email: $('#email').val() }, function (data) {
        });
        $('#myModalContacto').modal('hide');
    });
    
    $('#GuardarInmuebles').bind('click', function () {
        
        var Inmuebles = [{

        IdAgente                            : leerCookie("IdAgente"),
        Inmobiliaria                        : leerCookie("Alias"),
        NumeroExpediente                    : $('#Expediente').val(),
        IdMAETipoInmueble                   : "8",
        IdFamiliaTipoInmueble               : $('#TipoInmuebles').val(),
        IdTipoOperacion                     : $('#Operacion').val(),
        Precio1Euros                        : $('#BasePrecio1Euros').val(),
        IdEstado                            : $('#Estado').val(),
        IdMAEConservacion                   : $('#Conservacion').val(),
        IdMAEPoblacion                      : "538",
        IdTipoCalleInmueble                 : "1",
        IdTipoNumeroCalleInmueble           : "1",
        NombreCalleInmueble                 :"prueba",
        NumeroCalleInmueble                 : "69",
        NumplantasSobreRasante              : $('#NumplantasSobreRasante').val(),
        NumplantasBajoRasante               : $('#NumplantasBajoRasante').val(),
        SupEdificablePorPlanta              : $('#SupEdificablePorPlanta').val(),
        Superficie1                         : $('#Superficie1').val(),
        Superficie2                         : $('#Superficie2').val(),
        Superficie3                         : $('#Superficie3').val(),
        Superficie4                         : $('#Superficie4').val(),
        Superficie5                         : $('#Superficie5').val(),
        SuperficieSolar                     : $('#SuperficieSolar').val(),
        SuperficieUtil                      : $('#SuperficieUtil').val(),
        TotalSuperficie                     : "500",
        CodigoPostal                        : "08080",
        ADSLRDSI                            : $('#ADSLRDSI').val(),
        Accesominus                         : $('#Accesominus').val(),
        Sistseguridad                       : $('#Sistseguridad').val(),
        Aireacondicion                      : $('#Aireacondicion').val(),
        Camarafrigorífica                   : $('#Camarafrigorífica').val(),
        SalidaEmerg                         : $('#SalidaEmerg').val(),
        SistemaAntiinc                      : $('#SistemaAntiinc').val(),
        Calefacción                         : $('#Calefacción').val(),
        Puertaacceso                        : $('#Puertaacceso').val(),
        Divisiones                          : $('#Divisiones').val(),
        ADSL                                : $('#ADSL').val(),
        SistemaAntiincendio                 : $('#SistemaAntiincendio').val(),
        OtrasCaracteristicas                : $('#OtrasCaracteristicas').val(),
        AnoConstruccion                     : $('#AnoConstruccion').val(),
        Calefaccion                         : $('#Calefaccion').val(),
        Subterraneo                         : $('#Subterraneo').val(),
        Plazas                              : $('#Plazas').val(),
        Parcelaminima                       : $('#Parcelaminima').val(),
        Densidadmaxima                      : $('#Densidadmaxima').val(),
        Edificaciones                       : $('#Edificaciones').val(),
        Edificable                          : $('#Edificable').val(),
        Ocupacion                           : $('#Ocupacion').val(),
        AlturaPiso                          : $('#AlturaPiso').val(),
        Cerrado                             : $('#Cerrado').val(),
        Lavadero                            : $('#Lavadero').val(),
        Comedor                             : $('#Comedor').val(),
        Trastero                            : $('#Trastero').val(),
        Terraza                             : $('#Terraza').val(),
        Jardin                              : $('#Jardin').val(),
        Garaje                              : $('#Garaje').val(),
        Armarios                            : $('#Armarios').val(),
        Calefacion                          : $('#Calefacion').val(),
        Muebles                             : $('#Muebles').val(),
        PuertaBlindada                      : $('#PuertaBlindada').val(),
        VidriosDobles                       : $('#VidriosDobles').val(),
        GastosComunidad                     : $('#GastosComunidad').val(),
        Agua                                : $('#Agua').val(),
        Luz                                 : $('#Luz').val(),
        Gas                                 : $('#Gas').val(),
        Chimenea                            : $('#Chimenea').val(),
        Suelo                               : $('#Suelo').val(),
        CarpinteriaExterior                 : $('#CarpinteriaExterior').val(),
        CarpinteriaInterior                 : $('#CarpinteriaInterior').val(),
        AireAcondicionado                   : $('#AireAcondicionado').val(),
        Orientacion                         : $('#Orientacion').val(),
        Soleado                             : $('#Soleado').val(),
        Piscina                             : $('#Piscina').val(),
        SistemaSeguridad                    : $('#SistemaSeguridad').val(),
        Telefono                            : $('#Telefono').val(),
        Rejas                               : $('#Rejas').val(),
        Urbanizado                          : $('#Urbanizado').val(),
        Asfaltado                           : $('#Asfaltado').val(),
        Alumbrado                           : $('#Alumbrado').val(),
        Accesibilidad                       : $('#Accesibilidad').val(),
        Esquina                             : $('#Esquina').val(),
        CertificacionEnergia                : $('#CertificacionEnergia').val(),
        CertificacionEnergeticaEnTramite    : $('#CertificacionEnergeticaEnTramite').val(),
        NumVecinos                          : $('#NumVecinos').val(),
        Cultivable                          : $('#Cultivable').val(),
        CualificacionUrbanistica            : $('#CualificacionUrbanistica').val(),
        LicenciaObras                       : $('#LicenciaObras').val(),
        Arbolado                            : $('#Arbolado').val(),
        Exterior                            : $('#Exterior').val(),
        Interior                            : $('#Interior').val(),
        Balcon                              : $('#Balcon').val(),
        Ascensor                            : $('#Ascensor').val(),
        PorteroAutomatico                   : $('#PorteroAutomatico').val(),
        ClaseNegocio                        : $('#ClaseNegocio').val(),
        Adaptado                            : $('#Adaptado').val(),
        Oficina                             : $('#Oficina').val(),
        Divisible                           : $('#Divisible').val(),
        Escaparate                          : $('#Escaparate').val(),
        PlantaBaja                          : $('#PlantaBaja').val(),
        Patio                               : $('#Patio').val(),
        PuertasDeAcceso                     : $('#PuertasDeAcceso').val(),
        CajaFuerte                          : $('#CajaFuerte').val(),
        Vestuarios                          : $('#Vestuarios').val(),
        Altillo                             : $('#Altillo').val(),
        MuelleCarga                         : $('#MuelleCarga').val(),
        Grua                                : $('#Grua').val(),
        CamaraFrigorifica                   : $('#CamaraFrigorifica').val(),
        Bascula                             : $('#Bascula').val(),
        Aislantes                           : $('#Aislantes').val(),
        SalidaHumos                         : $('#SalidaHumos').val(),
        Cubierta                            : $('#Cubierta').val(),
        Estructura                          : $('#Estructura').val(),
        Intranet                            : $('#Intranet').val(),
        Rdsi                                : $('#Rdsi').val(),
        SistemaAntiIncendios                : $('#SistemaAntiIncendios').val(),
        SalidaEmergencia                    : $('#SalidaEmergencia').val(),
        ConjuntoNaves                       : $('#ConjuntoNaves').val(),
        Alcantarillado                      : $('#Alcantarillado').val(),
        Asfalto                             : $('#Asfalto').val(),
        Vallado                             : $('#Vallado').val(),
        Potencia                            : $('#Potencia').val(),
        AccesoMinusvalidos                  : $('#AccesoMinusvalidos').val(),
        AlturaTecho                         : $('#AlturaTecho').val(),
        Columlaterales                      : $('#Columlaterales').val(),
        Llano                               : $('#Llano').val(),
        Pozo                                : $('#Pozo').val(),
        Electricidad                        : $('#Electricidad').val(),
        Cocina                              : $('#Cocina')[0].checked,

        Plazas_txt                          : $('#Plazas_txt').val(),
        Detalle1_txt                        : $('#Detalle1_txt').val(),
        Cabecera1_txt                       : $('#Cabecera1_txt').val(),
        Edificaciones_txt                   : $('#Edificaciones_txt').val(),
        Precio1_txt                         : $('#Precio1_txt').val(),
        Subterraneo_txt                     : $('#Subterraneo_txt').val(),
        Arbolado_txt                        : $('#Arbolado_txt').val(),
        LicenciaObras_txt                   : $('#LicenciaObras_txt').val(),
        Cultivable_txt                      : $('#Cultivable_txt').val(),
        CualificacionUrbanistica_txt        : $('#CualificacionUrbanistica_txt').val(),
        NumVecinos_txt                      : $('#NumVecinos_txt').val(),
        Ocupacion_txt                       : $('#Ocupacion_txt').val(),
        NumplantasSobreRasante_txt          : $('#NumplantasSobreRasante_txt').val(),
        NumplantasBajoRasante_txt           : $('#NumplantasBajoRasante_txt').val(),
        SupEdificablePorPlanta_txt          : $('#SupEdificablePorPlanta_txt').val(),
        Edificable_txt                      : $('#Edificable_txt').val(),
        Cerrado_txt                         : $('#Cerrado_txt').val(),
        NumHabDobles_txt                    : $('#NumHabDobles_txt').val(),
        NumHabSencillas_txt                 : $('#NumHabSencillas_txt').val(),
        NumSuitsHabDobles_txt               : $('#NumSuitsHabDobles_txt').val(),
        NumBanos_txt                        : $('#NumBanos_txt').val(),
        NumAseos_txt                        : $('#NumAseos_txt').val(),
        NumHabitacionIndividual_txt         : $('#NumHabitacionIndividual_txt').val(),
        Superficie1_txt                     : $('#Superficie1_txt').val(),
        Superficie2_txt                     : $('#Superficie2_txt').val(),
        Superficie3_txt                     : $('#Superficie3_txt').val(),
        Superficie4_txt                     : $('#Superficie4_txt').val(),
        Superficie5_txt                     : $('#Superficie5_txt').val(),
        SuperficieSolar_txt                 : $('#SuperficieSolar_txt').val(),
        Cocina_txt                          : $('#Cocina_txt').val(),
        Lavadero_txt                        : $('#Lavadero_txt').val(),
        Comedor_txt                         : $('#Comedor_txt').val(),
        Terraza_txt                         : $('#Terraza_txt').val(),
        Jardin_txt                          : $('#Jardin_txt').val(),
        Garaje_txt                          : $('#Garaje_txt').val(),
        Armarios_txt                        : $('#Armarios_txt').val(),
        Calefacion_txt                      : $('#Calefacion_txt').val(),
        Muebles_txt                         : $('#Muebles_txt').val(),
        PuertaBlindada_txt                  : $('#PuertaBlindada_txt').val(),
        VidriosDobles_txt                   : $('#VidriosDobles_txt').val(),
        AnoConstraccion_txt                 : $('#AnoConstraccion_txt').val(),
        GastosComunidad_txt                 : $('#GastosComunidad_txt').val(),
        Agua_txt                            : $('#Agua_txt').val(),
        Luz_txt                             : $('#Luz_txt').val(),
        Gas_txt                             : $('#Gas_txt').val(),
        Chimenea_txt                        : $('#Chimenea_txt').val(),
        Suelo_txt                           : $('#Suelo_txt').val(),
        CarpinteriaExterior_txt             : $('#CarpinteriaExterior_txt').val(),
        CarpinteriaInterior_txt             : $('#CarpinteriaInterior_txt').val(),
        AireAcondicionado_txt               : $('#AireAcondicionado_txt').val(),
        Soleado_txt                         : $('#Soleado_txt').val(),
        Piscina_txt                         : $('#Piscina_txt').val(),
        SistemaSeguridad_txt                : $('#SistemaSeguridad_txt').val(),
        Telefono_txt                        : $('#Telefono_txt').val(),
        Urbanizado_txt                      : $('#Urbanizado_txt').val(),
        Asfaltado_txt                       : $('#Asfaltado_txt').val(),
        Alumbrado_txt                       : $('#Alumbrado_txt').val(),
        Accesibilidad_txt                   : $('#Accesibilidad_txt').val(),
        Esquina_txt                         : $('#Esquina_txt').val(),
        Calefaccion_txt                     : $('#Calefaccion_txt').val(),
        Suelos_txt                          : $('#Suelos_txt').val(),
        Orientacion_txt                     : $('#Orientacion_txt').val(),
        AnoConstruccion_txt                 : $('#AnoConstruccion_txt').val(),
        OtrasCaracteristicas_txt            : $('#OtrasCaracteristicas_txt').val(),
        AlturaPiso_txt                      : $('#AlturaPiso_txt').val(),
        PorteroAutomatico_txt               : $('#PorteroAutomatico_txt').val(),
        Exterior_txt                        : $('#Exterior_txt').val(),
        Interior_txt                        : $('#Interior_txt').val(),
        Balcon_txt                          : $('#Balcon_txt').val(),
        Ascensor_txt                        : $('#Ascensor_txt').val(),
        Trastero_txt                        : $('#Trastero_txt').val(),
        AlturaTecho_txt                     : $('#AlturaTecho_txt').val(),
        ClaseNegocio_txt                    : $('#ClaseNegocio_txt').val(),
        Adaptado_txt                        : $('#Adaptado_txt').val(),
        Oficina_txt                         : $('#Oficina_txt').val(),
        Divisible_txt                       : $('#Divisible_txt').val(),
        Escaparate_txt                      : $('#Escaparate_txt').val(),
        PlantaBaja_txt                      : $('#PlantaBaja_txt').val(),
        Patio_txt                           : $('#Patio_txt').val(),
        PuertasDeAcceso_txt                 : $('#PuertasDeAcceso_txt').val(),
        CajaFuerte_txt                      : $('#CajaFuerte_txt').val(),
        Vestuarios_txt                      : $('#Vestuarios_txt').val(),
        Altillo_txt                         : $('#Altillo_txt').val(),
        MuelleCarga_txt                     : $('#MuelleCarga_txt').val(),
        Grua_txt                            : $('#Grua_txt').val(),
        CamaraFrigorifica_txt               : $('#CamaraFrigorifica_txt').val(),
        Bascula_txt                         : $('#Bascula_txt').val(),
        Aislantes_txt                       : $('#Aislantes_txt').val(),
        SalidaHumos_txt                     : $('#SalidaHumos_txt').val(),
        Cubierta_txt                        : $('#Cubierta_txt').val(),
        Estructura_txt                      : $('#Estructura_txt').val(),
        Intranet_txt                        : $('#Intranet_txt').val(),
        Rdsi_txt                            : $('#Rdsi_txt').val(),
        SistemaAntiIncendios_txt            : $('#SistemaAntiIncendios_txt').val(),
        SalidaEmergencia_txt                : $('#SalidaEmergencia_txt').val(),
        ConjuntoNaves_txt                   : $('#ConjuntoNaves_txt').val(),
        Alcantarillado_txt                  : $('#Alcantarillado_txt').val(),
        Asfalto_txt                         : $('#Asfalto_txt').val(),
        Vallado_txt                         : $('#Vallado_txt').val(),
        Potencia_txt                        : $('#Potencia_txt').val(),
        AccesoMinusvalidos_txt              : $('#AccesoMinusvalidos_txt').val(),
        SistemaAntiincendio_txt             : $('#SistemaAntiincendio_txt').val(),
        PuertaAcceso_txt                    : $('#PuertaAcceso_txt').val(),
        Atillo_txt                          : $('#Atillo_txt').val(),
        ADSL_txt                            : $('#ADSL_txt').val(),
        Divisiones_txt                      : $('#Divisiones_txt').val(),
        Pozo_txt                            : $('#Pozo_txt').val(),
        Llano_txt                           : $('#Llano_txt').val(),
        Electricidad_txt                    : $('#Electricidad_txt').val(),
        Columlaterales_txt                  : $('#Columlaterales_txt').val(),
        DetalleAlternativo_txt              : $('#DetalleAlternativo_txt').val(),

        }];

        $.ajax({
            url: '/Inmuebles/ActualizarInmuebles',
            contentType: 'application/json; charset=utf-8',
            type: 'POST',
            dataType: 'json',
            data: JSON.stringify(Inmuebles)
        })

    });
    
    $('#seleccion_1').delegate('tr', 'click', function (event) {
        //var oTable                = $('#seleccion_1').dataTable();
        //var position              = oTable.fnGetPosition(this);
        //var IdInmueble            = oTable.fnGetData(position)[3];
        //var IdFamiliaTipoInmueble = oTable.fnGetData(position)[2];
        //$('#seleccionInmueble').val(IdInmueble);
        //$('#FamiliaInmueble').val(IdFamiliaTipoInmueble);
        //$('#seleccionInmuebles').trigger('click');
    });
    
    $('#seleccion_1').on('mouseenter', 'tr', function () {
        $(this).addClass("ui-state-hover");
    });

    $('#seleccion_1').on('mouseleave', 'tr', function () {
        $(this).removeClass("ui-state-hover");
    });

    $.fn.dataTableExt.oApi.fnLengthChange = function (oSettings, iDisplay) {

        oSettings._iDisplayLength = iDisplay;
        oSettings.oApi._fnCalculateEnd(oSettings);

        /* If we have space to show extra rows (backing up from the end point - then do so */
        if (oSettings._iDisplayEnd == oSettings.aiDisplay.length) {
            oSettings._iDisplayStart = oSettings._iDisplayEnd - oSettings._iDisplayLength;
            if (oSettings._iDisplayStart < 0) {
                oSettings._iDisplayStart = 0;
            }
        }

        if (oSettings._iDisplayLength == -1) {
            oSettings._iDisplayStart = 0;
        }

        oSettings.oApi._fnDraw(oSettings);

        if (oSettings.aanFeatures.l) {
            $('select', oSettings.aanFeatures.l).val(iDisplay);
        }
    };

    $(window).bind('resize', function () {
        if ($("#gridSelection").parent().is(':visible')) {
            $("#gridSelection").jqGrid('setGridWidth', $("#panel-default").width()-20);
        };
    }).trigger('resize');

    $('.navigation-toggler').bind('click', function () {

        if (!$('body').hasClass('navigation-small')) {
            if ($("#gridSelection").parent().is(':visible')) {
                $("#gridSelection").jqGrid('setGridWidth', $("#panel-default").width() + 40);
            };
            if ($("#list").parent().is(':visible')) {
                $("#list").jqGrid('setGridWidth', $("#panelHabitatSoft").width() + 160);
            };
        } else {
            if ($("#gridSelection").parent().is(':visible')) {
                $("#gridSelection").jqGrid('setGridWidth', $("#panel-default").width() - 90);
            };
            if ($("#list").parent().is(':visible')) {
                $("#list").jqGrid('setGridWidth', $("#panelHabitatSoft").width() - 210);
            };
        };

    });

    $('#TipoInmueble').html('');

    $.getJSON("/HabitatSoft/TodaFamilia", function (data) {
            $("#TipoInmueble").dynatree({
                checkbox: true,
                selectMode: 2,
                children: data,
                onSelect: function (select, node) {
                    $('#TipoInmuebleOpcion').remove();
                    // Display list of selected nodes
                    var selNodes = node.tree.getSelectedNodes();
                    // convert to title/key array
                    var selKeys = $.map(selNodes, function (node) {
                        var selItem = node.data.key;
                        var selId = node.data.id;
                        return node.data.key;
                    });
                    var selId = $.map(selNodes, function (node) {
                        var selItem = node.data.id;
                        return node.data.id;
                    });
                    $("form").append("<div class='modal-backdrop fade i' style='z-index: 1040;'></div>");
                    if (selId == "3,4") {
                        $('#VistaComun').attr('style', 'display:block');
                        $('#VistaPiso').attr('style', 'display:none');
                        $('#VistaCasa').attr('style', 'display:none');
                        $('#VistaSingular').attr('style', 'display:none');
                        $('#VistaNave').attr('style', 'display:none');
                        $('#VistaLocales').attr('style', 'display:none');
                        $('#VistaTerrenos').attr('style', 'display:none');
                        $('#VistaParking').attr('style', 'display:none');
                    }
                    else if (selId == "3,4,3") {
                        $('#VistaComun').attr('style', 'display:block');
                        $('#VistaPiso').attr('style', 'display:none');
                        $('#VistaCasa').attr('style', 'display:none');
                        $('#VistaSingular').attr('style', 'display:none');
                        $('#VistaNave').attr('style', 'display:none');
                        $('#VistaLocales').attr('style', 'display:none');
                        $('#VistaTerrenos').attr('style', 'display:none');
                        $('#VistaParking').attr('style', 'display:none');
                    }
                    else if (selId == "3,3") {
                        $('#VistaPiso').attr('style', 'display:block');
                        $('#VistaComun').attr('style', 'display:none');
                        $('#VistaCasa').attr('style', 'display:none');
                        $('#VistaSingular').attr('style', 'display:none');
                        $('#VistaNave').attr('style', 'display:none');
                        $('#VistaLocales').attr('style', 'display:none');
                        $('#VistaTerrenos').attr('style', 'display:none');
                        $('#VistaParking').attr('style', 'display:none');
                    }
                    else if (selId == "3,4,3,3") {
                        $('#VistaPiso').attr('style', 'display:block');
                        $('#VistaComun').attr('style', 'display:none');
                        $('#VistaCasa').attr('style', 'display:none');
                        $('#VistaSingular').attr('style', 'display:none');
                        $('#VistaNave').attr('style', 'display:none');
                        $('#VistaLocales').attr('style', 'display:none');
                        $('#VistaTerrenos').attr('style', 'display:none');
                        $('#VistaParking').attr('style', 'display:none');
                    }
                    else if (selId == "3,3,3") {
                        $('#VistaPiso').attr('style', 'display:block');
                        $('#VistaComun').attr('style', 'display:none');
                        $('#VistaCasa').attr('style', 'display:none');
                        $('#VistaSingular').attr('style', 'display:none');
                        $('#VistaNave').attr('style', 'display:none');
                        $('#VistaLocales').attr('style', 'display:none');
                        $('#VistaTerrenos').attr('style', 'display:none');
                        $('#VistaParking').attr('style', 'display:none');
                    }
                    else if (selId == "3,3,3,3") {
                        $('#VistaPiso').attr('style', 'display:block');
                        $('#VistaComun').attr('style', 'display:none');
                        $('#VistaCasa').attr('style', 'display:none');
                        $('#VistaSingular').attr('style', 'display:none');
                        $('#VistaNave').attr('style', 'display:none');
                        $('#VistaLocales').attr('style', 'display:none');
                        $('#VistaTerrenos').attr('style', 'display:none');
                        $('#VistaParking').attr('style', 'display:none');
                    }
                    else if (selId == "3,3,4,3") {
                        $('#VistaComun').attr('style', 'display:block');
                        $('#VistaPiso').attr('style', 'display:none');
                        $('#VistaCasa').attr('style', 'display:none');
                        $('#VistaSingular').attr('style', 'display:none');
                        $('#VistaNave').attr('style', 'display:none');
                        $('#VistaLocales').attr('style', 'display:none');
                        $('#VistaTerrenos').attr('style', 'display:none');
                        $('#VistaParking').attr('style', 'display:none');
                    }
                    else if (selId == "3,3,4,3,3") {
                        $('#VistaComun').attr('style', 'display:block');
                        $('#VistaPiso').attr('style', 'display:none');
                        $('#VistaCasa').attr('style', 'display:none');
                        $('#VistaSingular').attr('style', 'display:none');
                        $('#VistaNave').attr('style', 'display:none');
                        $('#VistaLocales').attr('style', 'display:none');
                        $('#VistaTerrenos').attr('style', 'display:none');
                        $('#VistaParking').attr('style', 'display:none');
                    }
                    else if (selId == "3,3,4,4,3") {
                        $('#VistaComun').attr('style', 'display:block');
                        $('#VistaPiso').attr('style', 'display:none');
                        $('#VistaCasa').attr('style', 'display:none');
                        $('#VistaSingular').attr('style', 'display:none');
                        $('#VistaNave').attr('style', 'display:none');
                        $('#VistaLocales').attr('style', 'display:none');
                        $('#VistaTerrenos').attr('style', 'display:none');
                        $('#VistaParking').attr('style', 'display:none');
                    }
                    else if (selId == "3,3,4,4,3,3") {
                        $('#VistaComun').attr('style', 'display:block');
                        $('#VistaPiso').attr('style', 'display:none');
                        $('#VistaCasa').attr('style', 'display:none');
                        $('#VistaSingular').attr('style', 'display:none');
                        $('#VistaNave').attr('style', 'display:none');
                        $('#VistaLocales').attr('style', 'display:none');
                        $('#VistaTerrenos').attr('style', 'display:none');
                        $('#VistaParking').attr('style', 'display:none');
                    }
                    else if (selId == "4,4,3,3") {
                        $('#VistaComun').attr('style', 'display:block');
                        $('#VistaPiso').attr('style', 'display:none');
                        $('#VistaCasa').attr('style', 'display:none');
                        $('#VistaSingular').attr('style', 'display:none');
                        $('#VistaNave').attr('style', 'display:none');
                        $('#VistaLocales').attr('style', 'display:none');
                        $('#VistaTerrenos').attr('style', 'display:none');
                        $('#VistaParking').attr('style', 'display:none');
                    }
                    else if (selId == "4,4,3") {
                        $('#VistaComun').attr('style', 'display:block');
                        $('#VistaPiso').attr('style', 'display:none');
                        $('#VistaCasa').attr('style', 'display:none');
                        $('#VistaSingular').attr('style', 'display:none');
                        $('#VistaNave').attr('style', 'display:none');
                        $('#VistaLocales').attr('style', 'display:none');
                        $('#VistaTerrenos').attr('style', 'display:none');
                        $('#VistaParking').attr('style', 'display:none');
                    }
                    else if (selId == "3,4,4") {
                        $('#VistaComun').attr('style', 'display:block');
                        $('#VistaPiso').attr('style', 'display:none');
                        $('#VistaCasa').attr('style', 'display:none');
                        $('#VistaSingular').attr('style', 'display:none');
                        $('#VistaNave').attr('style', 'display:none');
                        $('#VistaLocales').attr('style', 'display:none');
                        $('#VistaTerrenos').attr('style', 'display:none');
                        $('#VistaParking').attr('style', 'display:none');
                    }
                    else if (selId == "3,3,4,4") {
                        $('#VistaComun').attr('style', 'display:block');
                        $('#VistaPiso').attr('style', 'display:none');
                        $('#VistaCasa').attr('style', 'display:none');
                        $('#VistaSingular').attr('style', 'display:none');
                        $('#VistaNave').attr('style', 'display:none');
                        $('#VistaLocales').attr('style', 'display:none');
                        $('#VistaTerrenos').attr('style', 'display:none');
                        $('#VistaParking').attr('style', 'display:none');
                    }
                    else if (selId == "3,4,4,3,3") {
                        $('#VistaComun').attr('style', 'display:block');
                        $('#VistaPiso').attr('style', 'display:none');
                        $('#VistaCasa').attr('style', 'display:none');
                        $('#VistaSingular').attr('style', 'display:none');
                        $('#VistaNave').attr('style', 'display:none');
                        $('#VistaLocales').attr('style', 'display:none');
                        $('#VistaTerrenos').attr('style', 'display:none');
                        $('#VistaParking').attr('style', 'display:none');
                    }
                    else if (selId == "3,3,3,4") {
                        $('#VistaComun').attr('style', 'display:block');
                        $('#VistaPiso').attr('style', 'display:none');
                        $('#VistaCasa').attr('style', 'display:none');
                        $('#VistaSingular').attr('style', 'display:none');
                        $('#VistaNave').attr('style', 'display:none');
                        $('#VistaLocales').attr('style', 'display:none');
                        $('#VistaTerrenos').attr('style', 'display:none');
                        $('#VistaParking').attr('style', 'display:none');
                    }
                    else if (selId == "3,3,4") {
                        $('#VistaComun').attr('style', 'display:block');
                        $('#VistaPiso').attr('style', 'display:none');
                        $('#VistaCasa').attr('style', 'display:none');
                        $('#VistaSingular').attr('style', 'display:none');
                        $('#VistaNave').attr('style', 'display:none');
                        $('#VistaLocales').attr('style', 'display:none');
                        $('#VistaTerrenos').attr('style', 'display:none');
                        $('#VistaParking').attr('style', 'display:none');
                    }
                    else if (selId == "4,3") {
                        $('#VistaComun').attr('style', 'display:block');
                        $('#VistaPiso').attr('style', 'display:none');
                        $('#VistaCasa').attr('style', 'display:none');
                        $('#VistaSingular').attr('style', 'display:none');
                        $('#VistaNave').attr('style', 'display:none');
                        $('#VistaLocales').attr('style', 'display:none');
                        $('#VistaTerrenos').attr('style', 'display:none');
                        $('#VistaParking').attr('style', 'display:none');
                    }
                    else if (selId == "3") {
                        $('#VistaPiso').attr('style', 'display:block');
                        $('#VistaComun').attr('style', 'display:none');
                        $('#VistaCasa').attr('style', 'display:none');
                        $('#VistaSingular').attr('style', 'display:none');
                        $('#VistaNave').attr('style', 'display:none');
                        $('#VistaLocales').attr('style', 'display:none');
                        $('#VistaTerrenos').attr('style', 'display:none');
                        $('#VistaParking').attr('style', 'display:none');
                    }
                    else if (selId == "4,4") {
                        $('#VistaCasa').attr('style', 'display:block');
                        $('#VistaPiso').attr('style', 'display:none');
                        $('#VistaComun').attr('style', 'display:none');
                        $('#VistaSingular').attr('style', 'display:none');
                        $('#VistaNave').attr('style', 'display:none');
                        $('#VistaLocales').attr('style', 'display:none');
                        $('#VistaTerrenos').attr('style', 'display:none');
                        $('#VistaParking').attr('style', 'display:none');
                    }
                    else if (selId == "4") {
                        $('#VistaCasa').attr('style', 'display:block');
                        $('#VistaPiso').attr('style', 'display:none');
                        $('#VistaComun').attr('style', 'display:none');
                        $('#VistaSingular').attr('style', 'display:none');
                        $('#VistaNave').attr('style', 'display:none');
                        $('#VistaLocales').attr('style', 'display:none');
                        $('#VistaTerrenos').attr('style', 'display:none');
                        $('#VistaParking').attr('style', 'display:none');
                    }
                    else if (selId == "7") {
                        $('#VistaLocales').attr('style', 'display:block');
                        $('#VistaCasa').attr('style', 'display:none');
                        $('#VistaPiso').attr('style', 'display:none');
                        $('#VistaComun').attr('style', 'display:none');
                        $('#VistaSingular').attr('style', 'display:none');
                        $('#VistaNave').attr('style', 'display:none');
                        $('#VistaTerrenos').attr('style', 'display:none');
                        $('#VistaParking').attr('style', 'display:none');
                    }
                    else if (selId == "6") {
                        $('#VistaNave').attr('style', 'display:block');
                        $('#VistaLocales').attr('style', 'display:none');
                        $('#VistaCasa').attr('style', 'display:none');
                        $('#VistaPiso').attr('style', 'display:none');
                        $('#VistaComun').attr('style', 'display:none');
                        $('#VistaSingular').attr('style', 'display:none');
                        $('#VistaTerrenos').attr('style', 'display:none');
                        $('#VistaParking').attr('style', 'display:none');
                    }
                    else if (selId == "8") {
                        $('#VistaParking').attr('style', 'display:block');
                        $('#VistaNave').attr('style', 'display:none');
                        $('#VistaLocales').attr('style', 'display:none');
                        $('#VistaCasa').attr('style', 'display:none');
                        $('#VistaPiso').attr('style', 'display:none');
                        $('#VistaComun').attr('style', 'display:none');
                        $('#VistaSingular').attr('style', 'display:none');
                        $('#VistaTerrenos').attr('style', 'display:none');
                    }
                    else if (selId == "5") {
                        $('#VistaTerrenos').attr('style', 'display:block');
                        $('#VistaParking').attr('style', 'display:none');
                        $('#VistaNave').attr('style', 'display:none');
                        $('#VistaLocales').attr('style', 'display:none');
                        $('#VistaCasa').attr('style', 'display:none');
                        $('#VistaPiso').attr('style', 'display:none');
                        $('#VistaComun').attr('style', 'display:none');
                        $('#VistaSingular').attr('style', 'display:none');
                    }
                    else if (selId == "9") {
                        $('#VistaSingular').attr('style', 'display:block');
                        $('#VistaComun').attr('style', 'display:none');
                        $('#VistaParking').attr('style', 'display:none');
                        $('#VistaTerrenos').attr('style', 'display:none');
                        $('#VistaLocales').attr('style', 'display:none');
                        $('#VistaCasa').attr('style', 'display:none');
                        $('#VistaNave').attr('style', 'display:none');
                        $('#VistaPiso').attr('style', 'display:none');
                    }
                    else {
                        $('#VistaComun').attr('style', 'display:none');
                        $('#VistaSingular').attr('style', 'display:none');
                        $('#VistaParking').attr('style', 'display:none');
                        $('#VistaTerrenos').attr('style', 'display:none');
                        $('#VistaLocales').attr('style', 'display:none');
                        $('#VistaCasa').attr('style', 'display:none');
                        $('#VistaNave').attr('style', 'display:none');
                        $('#VistaPiso').attr('style', 'display:none');

                    }

                },


            });

            //$("#TipoInmueble").dynatree("getTree").selectKey("1");
        });
    
    $("#FamiliaTipoInmueble").change(function () {
        $('#TipoInmueble').html('');
        var selItem = $(this).val();
        checkboxes = document.getElementsByTagName("input");
        for (var i = 0; i < checkboxes.length; i++) {
            if (checkboxes[i].id == 'tipoinmuebles') {
                if (checkboxes[i].checked == true) {
                    checkboxes[i].checked = false;
                }
            }
        }
        $.getJSON("/HabitatSoft/TipoInmuebleFamilia", { id: $(this).val() }, function (data) {
            $.each(data, function (i, item) {
                if (selItem == '6') {
                    $('#VistaNave').attr('style', 'display:block');
                    $('#VistaCasa').attr('style', 'display:none');
                    $('#VistaSingular').attr('style', 'display:none');
                    $('#VistaPiso').attr('style', 'display:none');
                    $('#VistaLocales').attr('style', 'display:none');
                    $('#VistaTerrenos').attr('style', 'display:none');
                    $('#VistaParking').attr('style', 'display:none');
                }
                else if (selItem == '4') {
                    $('#VistaCasa').attr('style', 'display:block');
                    $('#VistaNave').attr('style', 'display:none');
                    $('#VistaSingular').attr('style', 'display:none');
                    $('#VistaPiso').attr('style', 'display:none');
                    $('#VistaLocales').attr('style', 'display:none');
                    $('#VistaTerrenos').attr('style', 'display:none');
                    $('#VistaParking').attr('style', 'display:none');
                }
                else if (selItem == '7') {
                    $('#VistaLocales').attr('style', 'display:block');
                    $('#VistaCasa').attr('style', 'display:none');
                    $('#VistaSingular').attr('style', 'display:none');
                    $('#VistaNave').attr('style', 'display:none');
                    $('#VistaPiso').attr('style', 'display:none');
                    $('#VistaTerrenos').attr('style', 'display:none');
                    $('#VistaParking').attr('style', 'display:none');
                }
                else if (selItem == '5') {
                    $('#VistaTerrenos').attr('style', 'display:block');
                    $('#VistaLocales').attr('style', 'display:none');
                    $('#VistaSingular').attr('style', 'display:none');
                    $('#VistaCasa').attr('style', 'display:none');
                    $('#VistaNave').attr('style', 'display:none');
                    $('#VistaPiso').attr('style', 'display:none');
                    $('#VistaParking').attr('style', 'display:none');
                }
                else if (selItem == '3') {
                    $('#VistaPiso').attr('style', 'display:block');
                    $('#VistaCasa').attr('style', 'display:none');
                    $('#VistaSingular').attr('style', 'display:none');
                    $('#VistaNave').attr('style', 'display:none');
                    $('#VistaLocales').attr('style', 'display:none');
                    $('#VistaTerrenos').attr('style', 'display:none');
                    $('#VistaParking').attr('style', 'display:none');
                }
                else if (selItem == '8') {
                    $('#VistaParking').attr('style', 'display:block');
                    $('#VistaTerrenos').attr('style', 'display:none');
                    $('#VistaSingular').attr('style', 'display:none');
                    $('#VistaLocales').attr('style', 'display:none');
                    $('#VistaCasa').attr('style', 'display:none');
                    $('#VistaNave').attr('style', 'display:none');
                    $('#VistaPiso').attr('style', 'display:none');
                }
                else if (selItem == '9') {
                    $('#VistaSingular').attr('style', 'display:block');
                    $('#VistaParking').attr('style', 'display:none');
                    $('#VistaTerrenos').attr('style', 'display:none');
                    $('#VistaLocales').attr('style', 'display:none');
                    $('#VistaCasa').attr('style', 'display:none');
                    $('#VistaNave').attr('style', 'display:none');
                    $('#VistaPiso').attr('style', 'display:none');
                }
                else if (selItem == 0) {
                    $('#VistaPiso').attr('style', 'display:block');
                    $('#VistaCasa').attr('style', 'display:none');
                    $('#VistaSingular').attr('style', 'display:none');
                    $('#VistaNave').attr('style', 'display:none');
                    $('#VistaLocales').attr('style', 'display:none');
                    $('#VistaTerrenos').attr('style', 'display:none');
                    $('#VistaParking').attr('style', 'display:none');
                }

                $("#TipoInmueble").dynatree({
                    checkbox: true,
                    selectMode: 2,
                    children: data,
                    onSelect: function (select, node) {
                        $('#TipoInmuebleOpcion').remove();
                        // Display list of selected nodes
                        var selNodes = node.tree.getSelectedNodes();
                        // convert to title/key array
                        var selKeys = $.map(selNodes, function (node) {
                            var selItem = node.data.key;
                            return node.data.key;
                        });

                        $("form").append("<input id='TipoInmuebleOpcion' type='text' name='TipoInmuebleOpcion' style='display:none' value='" + selKeys + "' />");
                        cargaTipoInmuebles(selKeys);

                    },
                });

                $("#TipoInmueble").dynatree("getTree").reload();

                display(selItem);

            });

        });

    });

    $("#TipoInmuebles").change(function () {


        var selItem = $(this).val();

        if (selItem == '4') {
            $('#Piso').attr('style', 'display:none');
            $('#Local').attr('style', 'display:none');
            $('#Casa').attr('style', 'display:block');
            $('#Terreno').attr('style', 'display:none');
            $('#Singulares').attr('style', 'display:none');
            $('#Nave').attr('style', 'display:none');
            $('#Parkings').attr('style', 'display:none');
        }
        if (selItem == '7') {
            $('#Local').attr('style', 'display:block');
            $('#Casa').attr('style', 'display:none');
            $('#Piso').attr('style', 'display:none');
            $('#Terreno').attr('style', 'display:none');
            $('#Singulares').attr('style', 'display:none');
            $('#Nave').attr('style', 'display:none');
            $('#Parkings').attr('style', 'display:none');
        }
        if (selItem == '3') {
            $('#Piso').attr('style', 'display:block');
            $('#Casa').attr('style', 'display:none');
            $('#Local').attr('style', 'display:none');
            $('#Terreno').attr('style', 'display:none');
            $('#Singulares').attr('style', 'display:none');
            $('#Nave').attr('style', 'display:none');
            $('#Parkings').attr('style', 'display:none');
        }
        if (selItem == '9') {
            $('#Piso').attr('style', 'display:none');
            $('#Casa').attr('style', 'display:none');
            $('#Local').attr('style', 'display:none');
            $('#Terreno').attr('style', 'display:none');
            $('#Nave').attr('style', 'display:none');
            $('#Singulares').attr('style', 'display:block');
            $('#Parkings').attr('style', 'display:none');
        }
        if (selItem == '6') {
            $('#Piso').attr('style', 'display:none');
            $('#Casa').attr('style', 'display:none');
            $('#Local').attr('style', 'display:none');
            $('#Terreno').attr('style', 'display:none');
            $('#Singulares').attr('style', 'display:none');
            $('#Nave').attr('style', 'display:block');
            $('#Parkings').attr('style', 'display:none');
        }
        if (selItem == '8') {
            $('#Piso').attr('style', 'display:none');
            $('#Casa').attr('style', 'display:none');
            $('#Local').attr('style', 'display:none');
            $('#Terreno').attr('style', 'display:none');
            $('#Singulares').attr('style', 'display:none');
            $('#Nave').attr('style', 'display:none');
            $('#Parkings').attr('style', 'display:none');
        }
        if (selItem == '5') {
            $('#Terreno').attr('style', 'display:block');
            $('#Casa').attr('style', 'display:none');
            $('#Local').attr('style', 'display:none');
            $('#Piso').attr('style', 'display:none');
            $('#Singulares').attr('style', 'display:none');
            $('#Nave').attr('style', 'display:none');
            $('#Parkings').attr('style', 'display:none');
        }
        if (selItem == '8') {
            $('#Terreno').attr('style', 'display:none');
            $('#Casa').attr('style', 'display:none');
            $('#Local').attr('style', 'display:none');
            $('#Piso').attr('style', 'display:none');
            $('#Singulares').attr('style', 'display:none');
            $('#Nave').attr('style', 'display:none');
            $('#Parkings').attr('style', 'display:block');
        }



    });

    $("#Provincias").change(function () {
        $('#treeHabita').html('');
        var selItem = $(this).val();
        $.getJSON("/HabitatSoft/Poblacion", { id: $(this).val() }, function (data) {
            $("#treeHabita").dynatree({
                    checkbox: true,
                    selectMode: 2,
                    children: data,
                    onSelect: function (select, node) {
                        //// Display list of selected nodes
                        var selNodes = node.tree.getSelectedNodes();
                        //// convert to title/key array
                        var selKeys = $.map(selNodes, function (nodeRet) {
                            return nodeRet.data.key;
                        });

                        if (node.childList == null) {
                            $.getJSON("/HabitatSoft/Zonas", { id: node.data.key }, function (dato) {
                                $.each(dato, function (i, item) {
                                    //Verificamos si no hemos cargado los hijos, los cargamos
                                    node.addChild({ title: item.title, key: item.key });
                                });
                                node.toggleExpand();
                            });
                        } else {
                            node.toggleExpand();
                        }

                        $('#Selection').remove();
                        $("form").append("<input id='Selection' type='hidden' name='Selection' value='" + selKeys + "' />");
                    }

                });

                $("#treeHabita").dynatree("getTree").reload();
            });
       });

    $.getJSON("/HabitatSoft/TodaPoblacion", function (data) {
            cargaPoblaciones(data);
        });
  
    $("#TipoOperacion").change(function () {

        var selItem = $(this).val();

        if (selItem == '3') {
            $('#OpcionCompra').attr('style', 'display:block');
        }
        else if (selItem == '6') {
            $('#Opciondiario').attr('style', 'display:block');
            $('#VistaOpcioCompraPrecio').attr('style', 'display:none');
            $('#OpcionCompra').attr('style', 'display:none');
        }
        else {
            $('#OpcionCompra').attr('style', 'display:none');
            $('#Opciondiario').attr('style', 'display:none');
        }

    });

    $("#CompraOpcion").change(function () {

        var selItem = $(this).val();

        if (selItem == '2') {
            $('#VistaOpcioCompraPrecio').attr('style', 'display:block');
        }
        else {
            $('#VistaOpcioCompraPrecio').attr('style', 'display:none');
        }

    });
   
    $("input[name='poblaciones']").on('ifChecked', function (event) {
        showEspere();
        crearNodoSeleccionado = $(this).is(':checked');
        $("#treeHabita").dynatree("getRoot").visit(function (node) {
            node.select(crearNodoSeleccionado);
        });
        hideEspere();
    });
    
    $("input[name='poblaciones']").on('ifUnchecked', function (event) {
        showEspere();
        crearNodoSeleccionado = $(this).is(':checked');
        $("#treeHabita").dynatree("getRoot").visit(function (node) {
            node.select(crearNodoSeleccionado);
        });
        hideEspere();
    });

    $("input[name='tipoinmuebles']").on('ifUnchecked', function (event) {
        $('#TipoInmueble').html('');
        var ItemFamilia = $("#FamiliaTipoInmueble").val();
        $.getJSON("/HabitatSoft/TipoInmuebleFamilia", { id: ItemFamilia }, function (data) {
            $("#TipoInmueble").dynatree({
                selectMode: 2,
                children: data,
                onSelect: function (select, node) {
                    //// Display list of selected nodes
                    var selNodes = node.tree.getSelectedNodes();
                    //// convert to title/key array
                    var selKeys = $.map(selNodes, function (nodeRet) {
                        return nodeRet.data.key;
                    });
                    $('#TipoInmuebleOpcion').remove();
                    $("form").append("<input id='TipoInmuebleOpcion' type='text' name='TipoInmuebleOpcion' style='display:none' value='" + selKeys + "' />");
                }
            });
        });
        $("#TipoInmueble").dynatree("getTree").reload();
        checkboxes = document.getElementsByTagName("input");
        var selKeys = "";
        for (var i = 0; i < checkboxes.length; i++) {
            if (checkboxes[i].id == 'tipoinmuebles') {
                if (checkboxes[i].checked == true) {
                    if (ItemFamilia == "0") {
                        $("#TipoInmueble").dynatree("getTree").selectKey("1");
                        $("#TipoInmueble").dynatree("getTree").selectKey("10");
                        $("#TipoInmueble").dynatree("getTree").selectKey("13");
                        $("#TipoInmueble").dynatree("getTree").selectKey("15");
                        $("#TipoInmueble").dynatree("getTree").selectKey("4");
                        $("#TipoInmueble").dynatree("getTree").selectKey("5");
                        $("#TipoInmueble").dynatree("getTree").selectKey("9");
                        $("#TipoInmueble").dynatree("getTree").selectKey("3");
                        $("#TipoInmueble").dynatree("getTree").selectKey("8");
                        $("#TipoInmueble").dynatree("getTree").selectKey("14");
                        $("#TipoInmueble").dynatree("getTree").selectKey("11");
                        $('#TipoInmuebleOpcion').remove();
                        var selKeys = "1,10,13,15,4,5,9,3,8,14,11";
                    }
                    else if (ItemFamilia == '3') {
                        $("#TipoInmueble").dynatree("getTree").selectKey("1");
                        $("#TipoInmueble").dynatree("getTree").selectKey("13");
                        $("#TipoInmueble").dynatree("getTree").selectKey("15");
                        $("#TipoInmueble").dynatree("getTree").selectKey("10");
                        var selKeys = "1,10,13,15";
                    }
                    else if (ItemFamilia == '4') {
                        $("#TipoInmueble").dynatree("getTree").selectKey("8");
                        $("#TipoInmueble").dynatree("getTree").selectKey("11");
                        var selKeys = "8,11";
                    }
                    else if (ItemFamilia == '5') {
                        $("#TipoInmueble").dynatree("getTree").selectKey("9");
                        var selKeys = "9";
                    }
                    else if (ItemFamilia == '6') {
                        $("#TipoInmueble").dynatree("getTree").selectKey("5");
                        var selKeys = "5";
                    }
                    else if (ItemFamilia == '7') {
                        $("#TipoInmueble").dynatree("getTree").selectKey("4");
                        var selKeys = "4";
                    }
                    else if (ItemFamilia == '4') {
                        $("#TipoInmueble").dynatree("getTree").selectKey("7");
                        var selKeys = "7";
                    }
                    else if (ItemFamilia == '8') {
                        $("#TipoInmueble").dynatree("getTree").selectKey("3");
                        var selKeys = "3";
                    }
                    else if (ItemFamilia == '9') {
                        $("#TipoInmueble").dynatree("getTree").selectKey("14");
                        var selKeys = "14";
                    }
                } else {
                    cargaTipoInmuebles(selKeys);
                }
                $('#TipoInmuebleOpcion').remove();
                $("form").append("<input id='TipoInmuebleOpcion' type='text' name='TipoInmuebleOpcion' style='display:none' value='" + selKeys + "' />");
            }
        }

    });

    $("input[name='tipoinmuebles']").on('ifChecked', function (event) {
        $('#TipoInmueble').html('');
        var ItemFamilia = $("#FamiliaTipoInmueble").val();
        $.getJSON("/HabitatSoft/TipoInmuebleFamilia", { id: ItemFamilia }, function (data) {
            $("#TipoInmueble").dynatree({
                selectMode: 2,
                children: data,
                onSelect: function (select, node) {
                    //// Display list of selected nodes
                    var selNodes = node.tree.getSelectedNodes();
                    //// convert to title/key array
                    var selKeys = $.map(selNodes, function (nodeRet) {
                        return nodeRet.data.key;
                    });
                    $('#TipoInmuebleOpcion').remove();
                    $("form").append("<input id='TipoInmuebleOpcion' type='text' name='TipoInmuebleOpcion' style='display:none' value='" + selKeys + "' />");
                }
            });
        });
        $("#TipoInmueble").dynatree("getTree").reload();
        checkboxes = document.getElementsByTagName("input");
        var selKeys = "";
        for (var i = 0; i < checkboxes.length; i++) {
            if (checkboxes[i].id == 'tipoinmuebles') {
                if (checkboxes[i].checked == true) {
                    if (ItemFamilia == "0") {
                        $("#TipoInmueble").dynatree("getTree").selectKey("1");
                        $("#TipoInmueble").dynatree("getTree").selectKey("10");
                        $("#TipoInmueble").dynatree("getTree").selectKey("13");
                        $("#TipoInmueble").dynatree("getTree").selectKey("15");
                        $("#TipoInmueble").dynatree("getTree").selectKey("4");
                        $("#TipoInmueble").dynatree("getTree").selectKey("5");
                        $("#TipoInmueble").dynatree("getTree").selectKey("9");
                        $("#TipoInmueble").dynatree("getTree").selectKey("3");
                        $("#TipoInmueble").dynatree("getTree").selectKey("8");
                        $("#TipoInmueble").dynatree("getTree").selectKey("14");
                        $("#TipoInmueble").dynatree("getTree").selectKey("11");
                        $('#TipoInmuebleOpcion').remove();
                        var selKeys = "1,10,13,15,4,5,9,3,8,14,11";
                    }
                    else if (ItemFamilia == '3') {
                        $("#TipoInmueble").dynatree("getTree").selectKey("1");
                        $("#TipoInmueble").dynatree("getTree").selectKey("13");
                        $("#TipoInmueble").dynatree("getTree").selectKey("15");
                        $("#TipoInmueble").dynatree("getTree").selectKey("10");
                        var selKeys = "1,10,13,15";
                    }
                    else if (ItemFamilia == '4') {
                        $("#TipoInmueble").dynatree("getTree").selectKey("8");
                        $("#TipoInmueble").dynatree("getTree").selectKey("11");
                        var selKeys = "8,11";
                    }
                    else if (ItemFamilia == '5') {
                        $("#TipoInmueble").dynatree("getTree").selectKey("9");
                        var selKeys = "9";
                    }
                    else if (ItemFamilia == '6') {
                        $("#TipoInmueble").dynatree("getTree").selectKey("5");
                        var selKeys = "5";
                    }
                    else if (ItemFamilia == '7') {
                        $("#TipoInmueble").dynatree("getTree").selectKey("4");
                        var selKeys = "4";
                    }
                    else if (ItemFamilia == '4') {
                        $("#TipoInmueble").dynatree("getTree").selectKey("7");
                        var selKeys = "7";
                    }
                    else if (ItemFamilia == '8') {
                        $("#TipoInmueble").dynatree("getTree").selectKey("3");
                        var selKeys = "3";
                    }
                    else if (ItemFamilia == '9') {
                        $("#TipoInmueble").dynatree("getTree").selectKey("14");
                        var selKeys = "14";
                    }
                } else {
                    cargaTipoInmuebles(selKeys);
                }
                $('#TipoInmuebleOpcion').remove();
                $("form").append("<input id='TipoInmuebleOpcion' type='text' name='TipoInmuebleOpcion' style='display:none' value='" + selKeys + "' />");
            }
        }

    });

    $('#ajax-modal').on('hide.preventHiding', function(e) {
        if (!modalEsperePermiteCerrar) {
            functionThatEndsUpDestroyingTheDOM();
        }
    });
    
});

function opcion() {
    return false;
}

function display(selItem) {

    if (selItem == '3') {
        $("#TipoInmueble").dynatree("getTree").selectKey("1");
    }
    else if (selItem == '4') {
        $("#TipoInmueble").dynatree("getTree").selectKey("8");
    }
    else if (selItem == '5') {
        $("#TipoInmueble").dynatree("getTree").selectKey("9");
    }
    else if (selItem == '6') {
        $("#TipoInmueble").dynatree("getTree").selectKey("5");
    }
    else if (selItem == '7') {
        $("#TipoInmueble").dynatree("getTree").selectKey("4");
    }
    else if (selItem == '4') {
        $("#TipoInmueble").dynatree("getTree").selectKey("7");
    }
    else if (selItem == '8') {
        $("#TipoInmueble").dynatree("getTree").selectKey("3");
    }
    else if (selItem == '9') {
        $("#TipoInmueble").dynatree("getTree").selectKey("14");
    }
    else if (selItem == '0') {
        $("#TipoInmueble").dynatree("getTree").selectKey("1");
    }

}

function cargaTipoInmuebles(selKeys) {


    if (selKeys == "1") {
        $('#VistaPiso').attr('style', 'display:block');
        $('#VistaComun').attr('style', 'display:none');
        $('#VistaCasa').attr('style', 'display:none');
        $('#VistaSingular').attr('style', 'display:none');
        $('#VistaNave').attr('style', 'display:none');
        $('#VistaLocales').attr('style', 'display:none');
        $('#VistaTerrenos').attr('style', 'display:none');
        $('#VistaParking').attr('style', 'display:none');
    }
    else if (selKeys == "13,15,1") {
        $('#VistaPiso').attr('style', 'display:block');
        $('#VistaComun').attr('style', 'display:none');
        $('#VistaCasa').attr('style', 'display:none');
        $('#VistaSingular').attr('style', 'display:none');
        $('#VistaNave').attr('style', 'display:none');
        $('#VistaLocales').attr('style', 'display:none');
        $('#VistaTerrenos').attr('style', 'display:none');
        $('#VistaParking').attr('style', 'display:none');
    }
    else if (selKeys == "13") {
        $('#VistaPiso').attr('style', 'display:block');
        $('#VistaComun').attr('style', 'display:none');
        $('#VistaCasa').attr('style', 'display:none');
        $('#VistaSingular').attr('style', 'display:none');
        $('#VistaNave').attr('style', 'display:none');
        $('#VistaLocales').attr('style', 'display:none');
        $('#VistaTerrenos').attr('style', 'display:none');
        $('#VistaParking').attr('style', 'display:none');
    }
    else if (selKeys == "15") {
        $('#VistaPiso').attr('style', 'display:block');
        $('#VistaComun').attr('style', 'display:none');
        $('#VistaCasa').attr('style', 'display:none');
        $('#VistaSingular').attr('style', 'display:none');
        $('#VistaNave').attr('style', 'display:none');
        $('#VistaLocales').attr('style', 'display:none');
        $('#VistaTerrenos').attr('style', 'display:none');
        $('#VistaParking').attr('style', 'display:none');
    }
    else if (selKeys == "10") {
        $('#VistaPiso').attr('style', 'display:block');
        $('#VistaComun').attr('style', 'display:none');
        $('#VistaCasa').attr('style', 'display:none');
        $('#VistaSingular').attr('style', 'display:none');
        $('#VistaNave').attr('style', 'display:none');
        $('#VistaLocales').attr('style', 'display:none');
        $('#VistaTerrenos').attr('style', 'display:none');
        $('#VistaParking').attr('style', 'display:none');
    }
    else if (selKeys == "13,15") {
        $('#VistaPiso').attr('style', 'display:block');
        $('#VistaComun').attr('style', 'display:none');
        $('#VistaCasa').attr('style', 'display:none');
        $('#VistaSingular').attr('style', 'display:none');
        $('#VistaNave').attr('style', 'display:none');
        $('#VistaLocales').attr('style', 'display:none');
        $('#VistaTerrenos').attr('style', 'display:none');
        $('#VistaParking').attr('style', 'display:none');
    }
    else if (selKeys == "13,15,10") {
        $('#VistaPiso').attr('style', 'display:block');
        $('#VistaComun').attr('style', 'display:none');
        $('#VistaCasa').attr('style', 'display:none');
        $('#VistaSingular').attr('style', 'display:none');
        $('#VistaNave').attr('style', 'display:none');
        $('#VistaLocales').attr('style', 'display:none');
        $('#VistaTerrenos').attr('style', 'display:none');
        $('#VistaParking').attr('style', 'display:none');
    }
    else if (selKeys == "13,10") {
        $('#VistaPiso').attr('style', 'display:block');
        $('#VistaComun').attr('style', 'display:none');
        $('#VistaCasa').attr('style', 'display:none');
        $('#VistaSingular').attr('style', 'display:none');
        $('#VistaNave').attr('style', 'display:none');
        $('#VistaLocales').attr('style', 'display:none');
        $('#VistaTerrenos').attr('style', 'display:none');
        $('#VistaParking').attr('style', 'display:none');
    }
    else if (selKeys == "13,1") {
        $('#VistaPiso').attr('style', 'display:block');
        $('#VistaComun').attr('style', 'display:none');
        $('#VistaCasa').attr('style', 'display:none');
        $('#VistaSingular').attr('style', 'display:none');
        $('#VistaNave').attr('style', 'display:none');
        $('#VistaLocales').attr('style', 'display:none');
        $('#VistaTerrenos').attr('style', 'display:none');
        $('#VistaParking').attr('style', 'display:none');
    }
    else if (selKeys == "4") {
        $('#VistaLocales').attr('style', 'display:block');
        $('#VistaComun').attr('style', 'display:none');
        $('#VistaPiso').attr('style', 'display:none');
        $('#VistaCasa').attr('style', 'display:none');
        $('#VistaSingular').attr('style', 'display:none');
        $('#VistaNave').attr('style', 'display:none');
        $('#VistaTerrenos').attr('style', 'display:none');
        $('#VistaParking').attr('style', 'display:none');
    }
    else if (selKeys == "5") {
        $('#VistaNave').attr('style', 'display:block');
        $('#VistaComun').attr('style', 'display:none');
        $('#VistaPiso').attr('style', 'display:none');
        $('#VistaCasa').attr('style', 'display:none');
        $('#VistaSingular').attr('style', 'display:none');
        $('#VistaLocales').attr('style', 'display:none');
        $('#VistaTerrenos').attr('style', 'display:none');
        $('#VistaParking').attr('style', 'display:none');
    }
    else if (selKeys == "9") {
        $('#VistaTerrenos').attr('style', 'display:block');
        $('#VistaComun').attr('style', 'display:none');
        $('#VistaPiso').attr('style', 'display:none');
        $('#VistaCasa').attr('style', 'display:none');
        $('#VistaSingular').attr('style', 'display:none');
        $('#VistaNave').attr('style', 'display:none');
        $('#VistaLocales').attr('style', 'display:none');
        $('#VistaParking').attr('style', 'display:none');
    }
    else if (selKeys == "8") {
        $('#VistaCasa').attr('style', 'display:block');
        $('#VistaComun').attr('style', 'display:none');
        $('#VistaParking').attr('style', 'display:none');
        $('#VistaPiso').attr('style', 'display:none');
        $('#VistaSingular').attr('style', 'display:none');
        $('#VistaNave').attr('style', 'display:none');
        $('#VistaLocales').attr('style', 'display:none');
        $('#VistaTerrenos').attr('style', 'display:none');
    }
    else if (selKeys == "3") {
        $('#VistaParking').attr('style', 'display:block');
        $('#VistaComun').attr('style', 'display:none');
        $('#VistaPiso').attr('style', 'display:none');
        $('#VistaCasa').attr('style', 'display:none');
        $('#VistaSingular').attr('style', 'display:none');
        $('#VistaNave').attr('style', 'display:none');
        $('#VistaLocales').attr('style', 'display:none');
        $('#VistaTerrenos').attr('style', 'display:none');
    }
    else if (selKeys == "8,11") {
        $('#VistaCasa').attr('style', 'display:block');
        $('#VistaComun').attr('style', 'display:none');
        $('#VistaParking').attr('style', 'display:none');
        $('#VistaPiso').attr('style', 'display:none');
        $('#VistaSingular').attr('style', 'display:none');
        $('#VistaNave').attr('style', 'display:none');
        $('#VistaLocales').attr('style', 'display:none');
        $('#VistaTerrenos').attr('style', 'display:none');
    }
    else if (selKeys == "14") {
        $('#VistaSingular').attr('style', 'display:block');
        $('#VistaComun').attr('style', 'display:none');
        $('#VistaCasa').attr('style', 'display:none');
        $('#VistaParking').attr('style', 'display:none');
        $('#VistaPiso').attr('style', 'display:none');
        $('#VistaNave').attr('style', 'display:none');
        $('#VistaLocales').attr('style', 'display:none');
        $('#VistaTerrenos').attr('style', 'display:none');
    }
    else if (selKeys == "10,1") {
        $('#VistaPiso').attr('style', 'display:block');
        $('#VistaCasa').attr('style', 'display:none');
        $('#VistaComun').attr('style', 'display:none');
        $('#VistaParking').attr('style', 'display:none');
        $('#VistaSingular').attr('style', 'display:none');
        $('#VistaNave').attr('style', 'display:none');
        $('#VistaLocales').attr('style', 'display:none');
        $('#VistaTerrenos').attr('style', 'display:none');
    }
    else if (selKeys == "15,1") {
        $('#VistaPiso').attr('style', 'display:block');
        $('#VistaCasa').attr('style', 'display:none');
        $('#VistaComun').attr('style', 'display:none');
        $('#VistaParking').attr('style', 'display:none');
        $('#VistaSingular').attr('style', 'display:none');
        $('#VistaNave').attr('style', 'display:none');
        $('#VistaLocales').attr('style', 'display:none');
        $('#VistaTerrenos').attr('style', 'display:none');
    }
    else if (selKeys == "15,10") {
        $('#VistaPiso').attr('style', 'display:block');
        $('#VistaCasa').attr('style', 'display:none');
        $('#VistaComun').attr('style', 'display:none');
        $('#VistaParking').attr('style', 'display:none');
        $('#VistaSingular').attr('style', 'display:none');
        $('#VistaNave').attr('style', 'display:none');
        $('#VistaLocales').attr('style', 'display:none');
        $('#VistaTerrenos').attr('style', 'display:none');
    }
    else if (selKeys == "13,1") {
        $('#VistaPiso').attr('style', 'display:block');
        $('#VistaCasa').attr('style', 'display:none');
        $('#VistaComun').attr('style', 'display:none');
        $('#VistaParking').attr('style', 'display:none');
        $('#VistaSingular').attr('style', 'display:none');
        $('#VistaNave').attr('style', 'display:none');
        $('#VistaLocales').attr('style', 'display:none');
        $('#VistaTerrenos').attr('style', 'display:none');
    }
    else if (selKeys == '13,15,10,1') {
        $('#VistaPiso').attr('style', 'display:block');
        $('#VistaCasa').attr('style', 'display:none');
        $('#VistaComun').attr('style', 'display:none');
        $('#VistaParking').attr('style', 'display:none');
        $('#VistaSingular').attr('style', 'display:none');
        $('#VistaNave').attr('style', 'display:none');
        $('#VistaLocales').attr('style', 'display:none');
        $('#VistaTerrenos').attr('style', 'display:none');
    }
    else if (selKeys == '15,10,1') {
        $('#VistaPiso').attr('style', 'display:block');
        $('#VistaCasa').attr('style', 'display:none');
        $('#VistaComun').attr('style', 'display:none');
        $('#VistaParking').attr('style', 'display:none');
        $('#VistaSingular').attr('style', 'display:none');
        $('#VistaNave').attr('style', 'display:none');
        $('#VistaLocales').attr('style', 'display:none');
        $('#VistaTerrenos').attr('style', 'display:none');
    }
    else if (selKeys == '13') {
        $('#VistaPiso').attr('style', 'display:block');
        $('#VistaCasa').attr('style', 'display:none');
        $('#VistaComun').attr('style', 'display:none');
        $('#VistaParking').attr('style', 'display:none');
        $('#VistaSingular').attr('style', 'display:none');
        $('#VistaNave').attr('style', 'display:none');
        $('#VistaLocales').attr('style', 'display:none');
        $('#VistaTerrenos').attr('style', 'display:none');
    }
    else if (selKeys == '15') {
        $('#VistaPiso').attr('style', 'display:block');
        $('#VistaCasa').attr('style', 'display:none');
        $('#VistaComun').attr('style', 'display:none');
        $('#VistaParking').attr('style', 'display:none');
        $('#VistaSingular').attr('style', 'display:none');
        $('#VistaNave').attr('style', 'display:none');
        $('#VistaLocales').attr('style', 'display:none');
        $('#VistaTerrenos').attr('style', 'display:none');
    }
    else if (selKeys == '10') {
        $('#VistaPiso').attr('style', 'display:block');
        $('#VistaCasa').attr('style', 'display:none');
        $('#VistaComun').attr('style', 'display:none');
        $('#VistaParking').attr('style', 'display:none');
        $('#VistaSingular').attr('style', 'display:none');
        $('#VistaNave').attr('style', 'display:none');
        $('#VistaLocales').attr('style', 'display:none');
        $('#VistaTerrenos').attr('style', 'display:none');
    }
    else if (selKeys == '11') {
        $('#VistaCasa').attr('style', 'display:block');
        $('#VistaComun').attr('style', 'display:none');
        $('#VistaParking').attr('style', 'display:none');
        $('#VistaPiso').attr('style', 'display:none');
        $('#VistaSingular').attr('style', 'display:none');
        $('#VistaNave').attr('style', 'display:none');
        $('#VistaLocales').attr('style', 'display:none');
        $('#VistaTerrenos').attr('style', 'display:none');
    }
    else if (selKeys == '8,1') {
        $('#VistaComun').attr('style', 'display:block');
        $('#VistaCasa').attr('style', 'display:none');
        $('#VistaParking').attr('style', 'display:none');
        $('#VistaPiso').attr('style', 'display:none');
        $('#VistaSingular').attr('style', 'display:none');
        $('#VistaNave').attr('style', 'display:none');
        $('#VistaLocales').attr('style', 'display:none');
        $('#VistaTerrenos').attr('style', 'display:none');
    }
    else if (selKeys == '8,1,10') {
        $('#VistaComun').attr('style', 'display:block');
        $('#VistaCasa').attr('style', 'display:none');
        $('#VistaParking').attr('style', 'display:none');
        $('#VistaPiso').attr('style', 'display:none');
        $('#VistaSingular').attr('style', 'display:none');
        $('#VistaNave').attr('style', 'display:none');
        $('#VistaLocales').attr('style', 'display:none');
        $('#VistaTerrenos').attr('style', 'display:none');
    }
    else if (selKeys == '13,8,1') {
        $('#VistaComun').attr('style', 'display:block');
        $('#VistaCasa').attr('style', 'display:none');
        $('#VistaParking').attr('style', 'display:none');
        $('#VistaPiso').attr('style', 'display:none');
        $('#VistaSingular').attr('style', 'display:none');
        $('#VistaNave').attr('style', 'display:none');
        $('#VistaLocales').attr('style', 'display:none');
        $('#VistaTerrenos').attr('style', 'display:none');
    }
    else if (selKeys == '15,8,1') {
        $('#VistaComun').attr('style', 'display:block');
        $('#VistaCasa').attr('style', 'display:none');
        $('#VistaParking').attr('style', 'display:none');
        $('#VistaPiso').attr('style', 'display:none');
        $('#VistaSingular').attr('style', 'display:none');
        $('#VistaNave').attr('style', 'display:none');
        $('#VistaLocales').attr('style', 'display:none');
        $('#VistaTerrenos').attr('style', 'display:none');
    }
    else if (selKeys == '10,8,1') {
        $('#VistaComun').attr('style', 'display:block');
        $('#VistaCasa').attr('style', 'display:none');
        $('#VistaParking').attr('style', 'display:none');
        $('#VistaPiso').attr('style', 'display:none');
        $('#VistaSingular').attr('style', 'display:none');
        $('#VistaNave').attr('style', 'display:none');
        $('#VistaLocales').attr('style', 'display:none');
        $('#VistaTerrenos').attr('style', 'display:none');
    }
    else if (selKeys == '8,1,15') {
        $('#VistaComun').attr('style', 'display:block');
        $('#VistaCasa').attr('style', 'display:none');
        $('#VistaParking').attr('style', 'display:none');
        $('#VistaPiso').attr('style', 'display:none');
        $('#VistaSingular').attr('style', 'display:none');
        $('#VistaNave').attr('style', 'display:none');
        $('#VistaLocales').attr('style', 'display:none');
        $('#VistaTerrenos').attr('style', 'display:none');
    }
    else if (selKeys == '8,1,13') {
        $('#VistaComun').attr('style', 'display:block');
        $('#VistaCasa').attr('style', 'display:none');
        $('#VistaParking').attr('style', 'display:none');
        $('#VistaPiso').attr('style', 'display:none');
        $('#VistaSingular').attr('style', 'display:none');
        $('#VistaNave').attr('style', 'display:none');
        $('#VistaLocales').attr('style', 'display:none');
        $('#VistaTerrenos').attr('style', 'display:none');
    }
    else if (selKeys == '1,8,10') {
        $('#VistaComun').attr('style', 'display:block');
        $('#VistaCasa').attr('style', 'display:none');
        $('#VistaParking').attr('style', 'display:none');
        $('#VistaPiso').attr('style', 'display:none');
        $('#VistaSingular').attr('style', 'display:none');
        $('#VistaNave').attr('style', 'display:none');
        $('#VistaLocales').attr('style', 'display:none');
        $('#VistaTerrenos').attr('style', 'display:none');
    }
    else if (selKeys == '1,8,15') {
        $('#VistaComun').attr('style', 'display:block');
        $('#VistaCasa').attr('style', 'display:none');
        $('#VistaParking').attr('style', 'display:none');
        $('#VistaPiso').attr('style', 'display:none');
        $('#VistaSingular').attr('style', 'display:none');
        $('#VistaNave').attr('style', 'display:none');
        $('#VistaLocales').attr('style', 'display:none');
        $('#VistaTerrenos').attr('style', 'display:none');
    }
    else if (selKeys == '1,8,13') {
        $('#VistaComun').attr('style', 'display:block');
        $('#VistaCasa').attr('style', 'display:none');
        $('#VistaParking').attr('style', 'display:none');
        $('#VistaPiso').attr('style', 'display:none');
        $('#VistaSingular').attr('style', 'display:none');
        $('#VistaNave').attr('style', 'display:none');
        $('#VistaLocales').attr('style', 'display:none');
        $('#VistaTerrenos').attr('style', 'display:none');
    }
    else if (selKeys == '11,1,10') {
        $('#VistaComun').attr('style', 'display:block');
        $('#VistaCasa').attr('style', 'display:none');
        $('#VistaParking').attr('style', 'display:none');
        $('#VistaPiso').attr('style', 'display:none');
        $('#VistaSingular').attr('style', 'display:none');
        $('#VistaNave').attr('style', 'display:none');
        $('#VistaLocales').attr('style', 'display:none');
        $('#VistaTerrenos').attr('style', 'display:none');
    }
    else if (selKeys == '11,1,15') {
        $('#VistaComun').attr('style', 'display:block');
        $('#VistaCasa').attr('style', 'display:none');
        $('#VistaParking').attr('style', 'display:none');
        $('#VistaPiso').attr('style', 'display:none');
        $('#VistaSingular').attr('style', 'display:none');
        $('#VistaNave').attr('style', 'display:none');
        $('#VistaLocales').attr('style', 'display:none');
        $('#VistaTerrenos').attr('style', 'display:none');
    }
    else if (selKeys == '11,1,13') {
        $('#VistaComun').attr('style', 'display:block');
        $('#VistaCasa').attr('style', 'display:none');
        $('#VistaParking').attr('style', 'display:none');
        $('#VistaPiso').attr('style', 'display:none');
        $('#VistaSingular').attr('style', 'display:none');
        $('#VistaNave').attr('style', 'display:none');
        $('#VistaLocales').attr('style', 'display:none');
        $('#VistaTerrenos').attr('style', 'display:none');
    }
    else if (selKeys == '8,11,1,13,15,10') {
        $('#VistaComun').attr('style', 'display:block');
        $('#VistaCasa').attr('style', 'display:none');
        $('#VistaParking').attr('style', 'display:none');
        $('#VistaPiso').attr('style', 'display:none');
        $('#VistaSingular').attr('style', 'display:none');
        $('#VistaNave').attr('style', 'display:none');
        $('#VistaLocales').attr('style', 'display:none');
        $('#VistaTerrenos').attr('style', 'display:none');
    }
    else if (selKeys == '8,10') {
        $('#VistaComun').attr('style', 'display:block');
        $('#VistaCasa').attr('style', 'display:none');
        $('#VistaParking').attr('style', 'display:none');
        $('#VistaPiso').attr('style', 'display:none');
        $('#VistaSingular').attr('style', 'display:none');
        $('#VistaNave').attr('style', 'display:none');
        $('#VistaLocales').attr('style', 'display:none');
        $('#VistaTerrenos').attr('style', 'display:none');
    }
    else if (selKeys == '8,15') {
        $('#VistaComun').attr('style', 'display:block');
        $('#VistaCasa').attr('style', 'display:none');
        $('#VistaParking').attr('style', 'display:none');
        $('#VistaPiso').attr('style', 'display:none');
        $('#VistaSingular').attr('style', 'display:none');
        $('#VistaNave').attr('style', 'display:none');
        $('#VistaLocales').attr('style', 'display:none');
        $('#VistaTerrenos').attr('style', 'display:none');
    }
    else if (selKeys == '8,13') {
        $('#VistaComun').attr('style', 'display:block');
        $('#VistaCasa').attr('style', 'display:none');
        $('#VistaParking').attr('style', 'display:none');
        $('#VistaPiso').attr('style', 'display:none');
        $('#VistaSingular').attr('style', 'display:none');
        $('#VistaNave').attr('style', 'display:none');
        $('#VistaLocales').attr('style', 'display:none');
        $('#VistaTerrenos').attr('style', 'display:none');
    }
    else if (selKeys == '11,10') {
        $('#VistaComun').attr('style', 'display:block');
        $('#VistaCasa').attr('style', 'display:none');
        $('#VistaParking').attr('style', 'display:none');
        $('#VistaPiso').attr('style', 'display:none');
        $('#VistaSingular').attr('style', 'display:none');
        $('#VistaNave').attr('style', 'display:none');
        $('#VistaLocales').attr('style', 'display:none');
        $('#VistaTerrenos').attr('style', 'display:none');
    }
    else if (selKeys == '11,15') {
        $('#VistaComun').attr('style', 'display:block');
        $('#VistaCasa').attr('style', 'display:none');
        $('#VistaParking').attr('style', 'display:none');
        $('#VistaPiso').attr('style', 'display:none');
        $('#VistaSingular').attr('style', 'display:none');
        $('#VistaNave').attr('style', 'display:none');
        $('#VistaLocales').attr('style', 'display:none');
        $('#VistaTerrenos').attr('style', 'display:none');
    }
    else if (selKeys == '11,13') {
        $('#VistaComun').attr('style', 'display:block');
        $('#VistaCasa').attr('style', 'display:none');
        $('#VistaParking').attr('style', 'display:none');
        $('#VistaPiso').attr('style', 'display:none');
        $('#VistaSingular').attr('style', 'display:none');
        $('#VistaNave').attr('style', 'display:none');
        $('#VistaLocales').attr('style', 'display:none');
        $('#VistaTerrenos').attr('style', 'display:none');
    }
    else {
        $('#VistaComun').attr('style', 'display:none');
        $('#VistaPiso').attr('style', 'display:none');
        $('#VistaCasa').attr('style', 'display:none');
        $('#VistaSingular').attr('style', 'display:none');
        $('#VistaNave').attr('style', 'display:none');
        $('#VistaLocales').attr('style', 'display:none');
        $('#VistaTerrenos').attr('style', 'display:none');
        $('#VistaParking').attr('style', 'display:none');
    }
}

function leerCookie(clave) {

    var valCookie = "";

    var buscar = clave + "=";

    if (document.cookie.length > 0) {

        pos = document.cookie.indexOf(buscar);

        if (pos != -1) {

            pos += buscar.length;

            fin = document.cookie.indexOf(";", pos);

            if (fin == -1)

                fin = document.cookie.length;

            valCookie = unescape(document.cookie.substring(pos, fin))

        }

    }

    return valCookie;

}

function cargaPoblaciones(data) {
    crearNodoSeleccionado = false;
    $("#treeHabita").dynatree({
        checkbox: true,
        selectMode: 2,
        children: data,
        onSelect: function (select, node) {
            if (!onSeleccionandoItem) {
                onSeleccionandoItem = true;
                //// Display list of selected nodes
                var selNodes = node.tree.getSelectedNodes();
                //// convert to title/key array
                var selKeys = $.map(selNodes, function (nodeRet) {
                    return nodeRet.data.key;
                });
                if (node.getLevel() == 2) {

                    if (node.isSelected()) {
                        node.getParent().select(true);
                    } else {
                        var seleccionado = node.isSelected();
                        var sonIguales = true;
                        var children = node.getParent().getChildren();
                        for (i = 0; i < children.length; i++) {
                            if (seleccionado != children[i].isSelected()) {
                                sonIguales = false;
                            }
                        }
                        if (sonIguales) {
                            node.getParent().select(seleccionado);
                        }
                    }
                }

                if (node.childList == null) {
                    if (node.getLevel() == 1) {
                        $.getJSON("/HabitatSoft/Zonas", { id: node.data.key }, function (dato) {
                            $.each(dato, function (i, item) {
                                //Verificamos si no hemos cargado los hijos, los cargamos
                                childNode = node.addChild({ title: item.title, key: item.key });
                                if (crearNodoSeleccionado) {
                                    childNode.select(true);
                                }
                            });
                            node.toggleExpand();
                        });
                    }
                } else {
                    if (node.getLevel() == 1) {
                        var children = node.getChildren();
                        for (i = 0; i < children.length; i++) {
                            children[i].select(node.isSelected());
                        }
                    }

                }
                onSeleccionandoItem = false;
            }

            $('#Selection').remove();
            $("form").append("<input id='Selection' type='hidden' name='Selection' value='" + selKeys + "' />");

        }
    });
}

function resize() {
    //Utilizado para Maximizar la Parrilla
    if ($('#Max').val() == "false") {
        $('#PanelIzquierdo').attr('style', 'display:none');
        $('#PanelDerecho').attr('style', 'display:none');
        $('#Max').val("true");
    } else if ($('#Max').val() == "true") {
        $('#PanelIzquierdo').attr('style', 'display:block');
        $('#PanelDerecho').attr('style', 'display:block');
        $('#Max').val("false");
    };
    $('#filtrar').trigger('click');
    return false;
}

function showEspere() {
    modalEsperePermiteCerrar = false;
    $('#ajax-modal').modal({
        keyboard: false,
    });
}

function hideEspere() {
    modalEsperePermiteCerrar = true;
    $('#ajax-modal').modal('hide');
}

function AplicarFiltroTodos() {
    //Aqui se ejecuta el Submit del controlador
    $('#filtro').val("Filtrar");
    $('#filtrar').trigger('click');
    return false;
}

function AplicarFiltro() {

    if ($('#filtrarMapa').val() == "true") {
        $('#filtro').val("MapaFiltro");
    } else {
        $('#filtro').val("AplicarFiltro");
    }
    $('#sbmt').trigger('click');
    return false;
}

function AplicarBuscadorPoblacion() {
    document.cookie = "Contacto=1;path=/";
    var texto = $("#Provincias").val();
    var oTable = $('#sample_8').dataTable({
        "bServerSide": true,
        "sAjaxSource": "/HabitatSoft/BuscadorPoblacion",
        "fnServerParams": function (aoData) {
            aoData.push({ "name": "mensaje", "value": texto });
        },
        "sServerMethod": "POST",
        "bProcessing": true,
        "iDisplayLength": 25,
        "aoColumns": [
                     { "sName": "Nombre" }
        ],
        "oLanguage": {
            "sProcessing": "Procesando...",
            "sLengthMenu": "Mostrar _MENU_ registros",
            "sZeroRecords": "No se encontraron resultados",
            "sEmptyTable": "Ningún dato disponible en esta tabla",
            "sInfo": "Mostrando registros del _START_ al _END_ de un total de _TOTAL_ registros",
            "sInfoEmpty": "Mostrando registros del 0 al 0 de un total de 0 registros",
            "sInfoFiltered": "(filtrado de un total de _MAX_ registros)",
            "sInfoPostFix": "",
            "sSearch": "",
            "sUrl": "",
            "sInfoThousands": ",",
            "sLoadingRecords": "Cargando...",
            "oPaginate": {
                "sFirst": "Primero",
                "sLast": "Último",
                "sNext": "Siguiente",
                "sPrevious": "Anterior"
            },
            "oAria": {
                "sSortAscending": ": Activar para ordenar la columna de manera ascendente",
                "sSortDescending": ": Activar para ordenar la columna de manera descendente"
            }
        },
        "sScrollY": "200px",
        "sDom": "frtiS",
        "bDestroy": true

    });
    $('#sample_8_wrapper .dataTables_filter input').remove();
    $('#sample_8_wrapper .dataTables_info').remove();


}

function AplicarBuscador() {
    document.cookie = "Contacto=1;path=/";
    var texto= $("#tbuscar").val();
    var oTable = $('#sample_4').dataTable({
        "bServerSide": true,
        "sAjaxSource": "/HabitatSoft/BuscadorContacto",
        "fnServerParams": function (aoData) {
            aoData.push({ "name": "mensaje", "value": texto });
        },
        "sServerMethod": "POST",
        "bProcessing": true,
        "iDisplayLength": 25,
        "aoColumns": [
                    { "sName": "Contacto", "bVisible": false },
                    { "sName": "Mombre" },
                    { "sName": "TelefonoMovil" },
                    { "sName": "TelefonoParticular" },
                    { "sName": "TelefonoProfesional ", "sClass": "hidden-xs" },
                    { "sName": "Email", "sClass": "hidden-xs" }
        ],
        "oLanguage": {
            "sProcessing": "Procesando...",
            "sLengthMenu": "Mostrar _MENU_ registros",
            "sZeroRecords": "No se encontraron resultados",
            "sEmptyTable": "Ningún dato disponible en esta tabla",
            "sInfo": "Mostrando registros del _START_ al _END_ de un total de _TOTAL_ registros",
            "sInfoEmpty": "Mostrando registros del 0 al 0 de un total de 0 registros",
            "sInfoFiltered": "(filtrado de un total de _MAX_ registros)",
            "sInfoPostFix": "",
            "sSearch": "",
            "sUrl": "",
            "sInfoThousands": ",",
            "sLoadingRecords": "Cargando...",
            "oPaginate": {
                "sFirst": "Primero",
                "sLast": "Último",
                "sNext": "Siguiente",
                "sPrevious": "Anterior"
            },
            "oAria": {
                "sSortAscending": ": Activar para ordenar la columna de manera ascendente",
                "sSortDescending": ": Activar para ordenar la columna de manera descendente"
            }
        },
        "sScrollY": "200px",
        "sDom": "frtiS",
        "bDestroy": true
    
    });
    $('#sample_4_wrapper .dataTables_filter input').remove();
    $('#sample_4_wrapper .dataTables_info').remove();
}

function FiltroTodos() {
    $("#treeHabita").dynatree("getRoot").visit(function (node) {
        node.select(true);
        var children = node.getParent().getChildren();
        for (i = 0; i < children.length; i++) {
             children[i].select(node.isSelected());
             node.getParent().select(true);
        }
    });
}


