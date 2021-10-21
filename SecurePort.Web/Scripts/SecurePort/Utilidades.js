//.ClientTemplate("<input type='checkbox' class='icheckbox_minimal-grey' #= asignar=='true' ? checked='checked': '' # onclick= 'javascript: AsignarPerfiles(this)'/>")
//$('#' + datatable).on('click', 'tbody tr.even.selected', function (evt) {
//    $(this).removeClass('selected');
//    $('input#IdValor').val("");
//    $('input#IdNombre').val("");
//    Id = 0;
//});
//$('#' + datatable).on('click', 'tbody tr.odd.selected', function (evt) {
//    $(this).removeClass('selected');
//    $('input#IdValor').val("");
//    $('input#IdNombre').val("");
//    Id = 0;
//});
//$(function () {
//    var oTable = $('#sample_7').dataTable({
//        "bScrollInfinite": true,
//        "sScrollY": "300px",
//        "sDom": "frtiS",
//        "bDeferRender": true,
//        "iDisplayLength": 10,
//        "bServerSide": true,
//        "sAjaxSource": $('#sample_7').data('url'),
//        "bProcessing": false,
//        "bFilter": true,
//        "bAutoWidth": true,
//        "bScrollCollapse": true,
//        "bjQueryUI": true,
//        "aoColumns": [
//			{ "sName": "Id", "bVisible": false },
//			{ "sName": "Descripcion", "bSortable": true },
//			{ "sDefaultContent": '<a herf="javascript:void()"><button type="button" title="Ver Permisos" name="Permisos" class="hidden-xs btn btn btn-blue "><i class="hidden-xs icon-user-md"></i></button></a>', "bSortable": false },
//			{ "sDefaultContent": '<a herf="javascript:void()"><button type="button" title="Desactivar Perfil" name="Delete" class="hidden-xs btn btn-danger remove-event"><i class="hidden-xs icon-trash"></i></button></a>', "bSortable": false }
//        ],
//        "fnDrawCallback": function () {
//            jQuery('#sample_7').on('click', 'tbody tr', function (evt) {
//                var Id = oTable.fnGetData(this)[0];
//                var $cell = $(evt.target).closest('td');
//                if ($cell.index() == 1) {
//                    showLoading();
//                    $.ajax({
//                        type: "POST",
//                        url: '/Grupos/VerPermisos',
//                        data: { id: Id },
//                        success: function (data) {
//                            if (data) {
//                                $("#width").val("Click");
//                                location.reload();
//                            }
//                        }
//                    });
//                } else if ($cell.index() == 2) {
//                    showLoading();
//                    $.ajax({
//                        type: "POST",
//                        url: '/Usuarios/Acceso',
//                        data: { idpermiso: 10 },
//                        success: function (data) {
//                            if (data.result) {
//                                $("#width").val("Click");
//                                alert("¿Esta seguro de Desactivar el Perfil?", 'Perfiles', Id);
//                            } else {
//                                setTimeout(function () {
//                                    alert(data.Message);
//                                }, 2000);
//                            }
//                        }
//                    });
//                }
//            });
//        },
//        order: [1, 'asc'],
//        "oLanguage": {
//            "sProcessing": "Procesando...",
//            "sLengthMenu": "Mostrar _MENU_ registros",
//            "sZeroRecords": "No se encontraron resultados",
//            "sEmptyTable": "No existen Registros para consultar",
//            "sInfo": "Mostrando registros del _START_ al _END_ de un total de _TOTAL_ registros",
//            "sInfoEmpty": "Mostrando registros del 0 al 0 de un total de 0 registros",
//            "sInfoFiltered": "(filtrado de un total de _MAX_ registros)",
//            "sInfoPostFix": "",
//            "sSearch": "",
//            "sUrl": "",
//            "sInfoThousands": ",",
//            "sLoadingRecords": "Cargando...",
//            "oPaginate": {
//                "sFirst": "Primero",
//                "sLast": "Último",
//                "sNext": "",
//                "sPrevious": ""
//            },
//            "oAria": {
//                "sSortAscending": ": Activar para ordenar la columna de manera ascendente",
//                "sSortDescending": ": Activar para ordenar la columna de manera descendente"
//            }
//        },
//        "bDestroy": true
//    });
//    $('#sample_7_wrapper .dataTables_filter input').remove();
//    $('#sample_7_wrapper .dataTables_info').remove();
//    $('#sample_7_wrapper .dataTables_length').remove();
//});
//$(function () {
//    var oTable = $('#sample_9').dataTable({
//        "bScrollInfinite": true,
//        "sScrollY": "400px",
//        "sDom": "frtiS",
//        "bDeferRender": true,
//        "iDisplayLength": 50,
//        "bServerSide": true,
//        "sAjaxSource": $('#sample_9').data('url'),
//        "bProcessing": false,
//        "bFilter": true,
//        "bAutoWidth": true,
//        "bScrollCollapse": true,
//        "bscrollCollapse": true,
//        "bjQueryUI": true,
//        "aoColumns": [
//			{ "sName": "Id", "bVisible": false, "bSortable": true },
//			{ "sName": "Nombre", "bVisible": true, "bSortable": false },
//		    { "sDefaultContent": '<a herf="javascript:void()"><input type="checkbox" class="icheckbox_minimal-grey">', "bSortable": false },
//		    { "sDefaultContent": '<a herf="javascript:void()"><button type="button" title="Ver Permisos" name="Permisos" class="hidden-xs btn btn btn-blue "><i class="hidden-xs icon-user-md"></i></button></a>', "bSortable": false }
//        ],
//        "fnDrawCallback": function () {
//            jQuery('#sample_9').on('click', 'tbody tr', function (evt) {
//                var Id = oTable.fnGetData(this)[0];
//                var Des = oTable.fnGetData(this)[1];
//                var $cell = $(evt.target).closest('td');
//                if ($cell.index() == 1) {
//                    showLoading();
//                    $.ajax({
//                        type: "POST",
//                        url: '/Usuarios/Acceso',
//                        data: { idpermiso: 8 },
//                        success: function (data) {
//                            if (data.result) {
//                                $.ajax({
//                                    type: "POST",
//                                    url: '/Perfiles/AltaPermisosPerfiles',
//                                    data: { id: Id },
//                                    success: function (data) {
//                                        if (data) {
//                                            $("#width").val("Click");
//                                            location.reload();
//                                        }
//                                    }
//                                });
//                            } else {
//                                setTimeout(function () {
//                                    alert(data.Message);
//                                }, 2000);
//                            }
//                        }
//                    });
//                } else {
//                    showLoading();
//                    $("#width").val("Click");
//                    $.ajax({
//                        type: "POST",
//                        url: '/Perfiles/VerPermisosOpcion',
//                        data: { id: Id, Descrip: Des },
//                        success: function (data) {
//                            if (data) {
//                                $("#width").val("Click");
//                                location.reload();
//                            }
//                        }
//                    });
//                }

//            });
//        },
//        order: [1, 'asc'],
//        "oLanguage": {
//            "sProcessing": "Procesando...",
//            "sLengthMenu": "Mostrar _MENU_ registros",
//            "sZeroRecords": "No se encontraron resultados",
//            "sEmptyTable": "No existen Registros para consultar",
//            "sInfo": "Mostrando registros del _START_ al _END_ de un total de _TOTAL_ registros",
//            "sInfoEmpty": "Mostrando registros del 0 al 0 de un total de 0 registros",
//            "sInfoFiltered": "(filtrado de un total de _MAX_ registros)",
//            "sInfoPostFix": "",
//            "sSearch": "",
//            "sUrl": "",
//            "sInfoThousands": ",",
//            "sLoadingRecords": "Cargando...",
//            "oPaginate": {
//                "sFirst": "",
//                "sLast": "",
//                "sNext": "Siguiente",
//                "sPrevious": "Anterior"
//            },
//            "oAria": {
//                "sSortAscending": ": Activar para ordenar la columna de manera ascendente",
//                "sSortDescending": ": Activar para ordenar la columna de manera descendente"
//            }
//        },
//        "bDestroy": true
//    });
//    $('#sample_9_wrapper .dataTables_filter input').remove();
//    $('#sample_9_wrapper .dataTables_info').remove();
//    $('#sample_9_wrapper .dataTables_length').remove();
//});