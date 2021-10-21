var mensaje = "Datos guardados correctamente";
var Alert = {
    
    showAlert: function (message) {
            showLoading();
            var _this = this;
            $('#alert span#message').html(message);
            $('#alert').fadeIn(100);
            $('#alert button').click(function () {
                _this.hideAlert();
                 window.location.reload();
            });
    },
    
    showConfirmPerfilAlta: function (message) {
        showLoading();
        var _this = this;
        $('#alert span#message').html(message);
        $('#alert').fadeIn(100);
        $('#alert button').click(function () {
            hideLoading();
            _this.hideAlert();
        });
    },
   
    showCorreo: function (message) {
        showLoading();
        var _this = this;
        $('#alert span#message').html(message);
        $('#alert').fadeIn(100);
        $('#alert button').click(function () {
            hideLoading();
            _this.hideAlert();
            $('input#login').val('');
        });
    },
    
    showVisualizar: function (message) {
        var _this = this;
        $('div#myModalUpload').modal('hide');
        $('#alert span#message').html(message);
        $('#alert').fadeIn(100);
        $('#alert button').click(function () {
            $('#alert').fadeOut();
            $('#alert').attr('style', 'display:none');
            hideLoading();
        });
    },
    
    showVisualizarUsers: function (message, retorna) {
        var _this = this;
        $('div#myModalUpload').modal('hide');
        $('#alert span#message').html(message);
        $('#alert').fadeIn(100);
        $('#alert button').click(function () {
            $('#alert').fadeOut();
            window.location.href = retorna;
        });
    },

    showLogin: function (message) {
        var _this = this;
        $('#alert span#message').html(message);
        $('#alert').fadeIn(100);
        $('#alert button').click(function () {
            $('#alert').fadeOut();
            hideLoading();
            window.location.href = "/";
        });
    },
    
    showLoginPerfil: function (message) {
        var _this = this;
        $('#alert span#message').html(message);
        $('#alert').fadeIn(100);
        $('#alert button').click(function () {
            $('#alert').fadeOut();
            hideLoading();
            $('#myModalCambiarPassword').modal('show');
            $('#myModalValidarPassword').modal('hide');
            $("input#password03.form-control").val('');
            $("input#passwordagain033.form-control").val('');
            $('div#Passwordagain033.form-group').removeClass('has-success');
            $('div#password03.form-group').removeClass('has-success');
            $('span#Spanpassword03.help-block').attr('style', 'display:none');
            $('span#Spanpassword09.help-block').attr('style', 'display:none');
            $('span#Spanpassword09.help-block').attr('style', 'display:none');
            $('span#Spanpassword10.help-block').attr('style', 'display:none');
            $('span#Spanpassword11.help-block').attr('style', 'display:none');
            $('span#SpanPasswordagain033.help-block').attr('style', 'display:none');
            $('span#SpanPasswordagain05.help-block').attr('style', 'display:none');
            $('input#correo03.form-control').val('');
            $('input#correoagain03.form-control').val('');
            $('span#Spancorreoagain05.help-block').attr('style', 'display:none');
            $('div#correoagain06.form-group').removeClass('has-success');
            $('div#correoagain06.form-group').removeClass('has-error');
            $('div#correo06.form-group').removeClass('has-success');
            $('div#correo06.form-group').removeClass('has-error');
        });
    },
    
    showGrupo: function (message) {
        var _this = this;
        $('#alert span#message').html(message);
        $('#alert').fadeIn(100);
        $('#alert button').click(function () {
            $('#alert').fadeOut();
            hideLoading();
            $('#myModalAgregar').modal('show');
        });
    },
    
    showSubirFichero: function (message) {
        var _this = this;
        $('#alert span#message').html(message);
        $('#alert').fadeIn(100);
        $('#alert button').click(function () {
            $('#alert').fadeOut();
            $('#alert').attr('style', 'display:none');
            $('#myModalUpload').modal('show');
            document.getElementById("hl-start-upload").disabled = false;
        });
    },
    
    hideAlert: function () {
        $('#alert').fadeOut();
    },

    showConfirmaIndex: function(message) {
        var _this = this;
        $('#alert span#message').html(message);
        $('#alert').fadeIn(100);
        $('#alert button').click(function() {
            window.location.href = "/Usuario/Index";
        });
    },

    showConfirmaRegistro: function(message) {
        var _this = this;
        $('#alert span#message').html(message);
        $('#alert').fadeIn(100);
        $('#alert button').click(function() {
            window.location.href = "/Home";
        });
    },

    showConfirmaActivo: function(message) {
        var _this = this;
        $('div#alert span#message').html(message);
        $('div#alert').fadeIn(100);
        $('div#alert button').click(function() {
            window.location.href = "/Home";
        });
    },

    showEliminar: function (message) {
        var _this = this;
        $('div#alert span#message').html(message);
        $('div#alert').fadeIn(100);
        $('div#alert button').click(function () {
            $('#alert').attr('style', 'display:none');
            window.location.reload();
        });
    },
    
    showAgregar: function (message, divRetorna) {
        var _this = this;
        $('div#alert span#message').html(message);
        $('div#alert').fadeIn(100);
        $('div#alert button').click(function () {
            $('#alert').attr('style', 'display:none');
            $(divRetorna).modal('show'); 
        });
    },

    showMensaje: function (message) {
        showLoading();
        var _this = this;
        $('div#alert span#message').html(message);
        $('div#alert').fadeIn(100);
        $('div#alert button').click(function () {
            $('#alert').attr('style', 'display:none');
            $('#confirm').attr('style', 'display:none');
            hideLoading();
        });
    },
    
   showRegistro: function(message) {
        var _this = this;
        $('div#alert span#message').html(message);
        $('div#alert').fadeIn(100);
        $('div#alert button').click(function() {
            $('#alert').attr('style', 'display:none');
            window.location.href = "/Menu";
        });
   },   
   showOrganimos: function (message) {
       var _this = this;
       $('div#alert span#message').html(message);
       $('div#alert').fadeIn(100);
       $('div#alert button').click(function () {
           $('#alert').attr('style', 'display:none');
       });
   },
   showAlertOrganismo: function (message) {
       var _this = this;
       $('div#alert span#message').html(message);
       $('div#alert').fadeIn(100);
       $('div#alert button').click(function () {
           $('#alert').attr('style', 'display:none');
           hideLoading();
       });
   },
  
   showConfirm: function (message, id, controller, callback) {
        var _this = this;
        $('#confirm span#message').html(message);
        $('#confirm').fadeIn(100);
        $('#confirm button').off();
        $('#confirm button#no').click(function() {
            $('#confirm').attr('style', 'display:none');
            hideLoading();
        });
        $('#confirm button#yes').click(function () {
            if (controller == 0) {
                $.ajax({
                    url: '/Usuarios/BajaConfirmed',
                    type: "POST",
                    data: JSON.stringify({ id: id }),
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    success: function(data) {
                        if (data.result) {
                            $('#confirm').attr('style', 'display:none');
                            window.location.reload();
                        } else {
                            $('#confirm').attr('style', 'display:none');
                            showLoading();
                            alert(data.Message, 'Eliminar');
                        }
                    }
                });
            } else if (controller == 1) {
                $.ajax({
                    url: '/Usuarios/EliminarUsuario',
                    type: "POST",
                    data: JSON.stringify({ id: id }),
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    success: function(data) {
                        if (data.result) {
                            showLoading();
                            alert(data.Message, "VisualizarUsers");
                        } else {
                            $('#confirm').attr('style', 'display:none');
                            showLoading();
                            alert(data.Message, 'Eliminar');
                        }
                    }
                });
            } else if (controller == 4) {
                $.ajax({
                    url: '/Perfiles/EliminarPerfil',
                    type: "POST",
                    data: JSON.stringify({ id: id }),
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    success: function(data) {
                        if (data.result) {
                            $('#confirm').attr('style', 'display:none');
                            window.location.reload();
                        } else {
                            $('#confirm').attr('style', 'display:none');
                            showLoading();
                            alert(data.Message, 'Eliminar');
                        }
                    }
                });
            } else if (controller == 6) {                
                $.ajax({
                    type: "POST",
                    url: $('input#URL').val(),
                    success: function(data) {
                        if (data) {
                            $($('input#URL1').val()).html(data);
                            hideLoading();
                        }
                    }
                });

            } else if (controller == 8) {
                GuardarGrupos();
            } else if (controller == 9) {
                if (id == 0) {
                    AltaPerfilesAcciones();
                } else if (id == 1) {
                    RegistraPerfiles();
                } else {
                    RegistaGrupos();
                }
            } else if (controller == 11) {
                $.ajax({
                    url: '/Usuarios/DesahacerDatosUsuario',
                    type: "POST",
                    success: function(data) {
                        if (data) {
                            hideLoading();
                            $('#Edit-page').html(data);
                        }
                    }
                });
            } else if (controller == 12) {
                GuardarUsuarioEditar();
            } else if (controller == 13) {
                showLoading();
                $.ajax({
                    url: '/Usuarios/EditarUsuarios',
                    type: "POST",
                    success: function(data) {
                        if (data.result) {
                            $.ajax({
                                type: "POST",
                                url: '/Usuarios/Edit',
                                success: function(data) {
                                    if (data) {
                                        alert(mensaje, "PerfilAlta");
                                        $('#Edit-page').html(data);
                                    }
                                }
                            });
                        } else {
                            showLoading();
                            alert(data.Message);
                        }
                    }
                });
                ;
            } else if (controller == 14) {
                showLoading();
                $.ajax({
                    url: '/Usuarios/GuardarDependenciasPuertos',
                    type: "POST",
                    success: function(data) {
                        if (data.result == false) {
                            alert(data.Message, 'Eliminar');
                        } else {
                            alert(mensaje, "PerfilAlta");
                            $('#ListVisualizar').html(data);
                        }
                    }                                
                });

            } else if (controller == 15) {
                showLoading();
                $.ajax({
                    url: '/Usuarios/DesahacerDependencias',
                    type: "POST",
                    success: function(data) {
                        if (data) {
                            hideLoading();
                            $('#ListVisualizar').html(data);
                        }
                    }
                });
            } else if (controller == 16) {
                showLoading();
                $.ajax({
                    url: '/Usuarios/GuardarDependenciasOrganismos',
                    type: "POST",
                    success: function(data) {
                        if (data.result == false) {
                            alert(data.Message, 'Eliminar');
                        } else {
                            alert(mensaje, "PerfilAlta");
                            $('#ListVisualizar').html(data);
                        }
                    }
                });
            } else if (controller == 17) {
                showLoading();
                $.ajax({
                    url: '/Usuarios/GuardarDependenciasInstalaciones',
                    type: "POST",
                    success: function(data) {
                        if (data.result == false) {
                            alert(data.Message, 'Eliminar');
                        } else {
                            alert(mensaje, "PerfilAlta");
                            $('#ListVisualizar').html(data);
                        }
                    }
                });
            } else if (controller == 25) {
                var bienesJson = {
                    "Id": $('input#ID01').val(),
                    "Descripcion": $('input#Descripcion01').val(),
                    "id_Tipo_IIPP": $('input#TipoInstalacion01').val(),
                    "id_Bien_Padre": $('input#BienPadre01').val(),
                    "activo": $('input#Activo01').val()                   
                };
                $.ajax({
                    url: '/Bienes/AltaBienes',
                    type: "POST",
                    data: JSON.stringify({ BienesJson: bienesJson }),
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        if (data.result) {
                            $('#confirm').attr('style', 'display:none');
                            window.location.reload();
                        } else {
                            $('#confirm').attr('style', 'display:none');
                            showLoading();
                            window.location.reload();                         
                        }
                    }
                });
            } else if (controller == 26) {
                $.ajax({
                    url: '/Amenazas/EliminarAmenazas',
                    type: "POST",
                    data: JSON.stringify({ id: id }),
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        if (data.result) {
                            $('#confirm').attr('style', 'display:none');
                            window.location.reload();
                        } else {
                            $('#confirm').attr('style', 'display:none');
                            showLoading();
                            alert(data.Message, 'Eliminar');
                        }
                    }
                });
            } else if (controller == 27) {                
                jsShowWindowLoad("Ejecutando datos.....");
                $.ajax({
                    url: '/Centros/EliminarCentros',
                    type: "POST",
                    data: JSON.stringify({ id: id }),
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        if (data.result) {
                            $('#confirm').attr('style', 'display:none');
                            jsRemoveWindowLoad();
                            window.location.reload();
                        } else {
                            $('#confirm').attr('style', 'display:none');
                            showLoading();
                            jsRemoveWindowLoad();
                            alert(data.Message, 'Eliminar');
                        }
                    }
                });
            } else if (controller == 28) {
                var AmenazasJson = {
                    "Descripcion": $('input#Nombre01').val(),
                    "Id":          $('input#Id01').val(),
                    "activo":      $('input#Activo01').val()
                };
                $.ajax({
                    url: '/Amenazas/AltaAmenaza',
                    type: "POST",
                    data: JSON.stringify({ AmenazasJson: AmenazasJson }),
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    success: function(data) {
                        if (data.result) {
                            $('#confirm').attr('style', 'display:none');
                            window.location.reload();
                        } else {
                            $('#confirm').attr('style', 'display:none');
                            showLoading();
                            window.location.reload();
                        }
                    }
                });
            } else if (controller == 29) {
                var bienesJson = {
                    "Id": $('input#ID01').val(),
                    "Descripcion": $('input#Descripcion01').val(),
                    "id_Tipo_IIPP": $('input#TipoInstalacion01').val(),
                    "id_Bien_Padre": $('input#BienPadre01').val(),
                    "activo": $('input#Activo01').val()
                };
                $.ajax({
                    url: '/Bienes/EditarBienes',
                    type: "POST",
                    data: JSON.stringify({ BienesJson: bienesJson }),
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        if (data.result) {
                            $('#confirm').attr('style', 'display:none');
                            window.location.reload();
                        } else {
                            $('#confirm').attr('style', 'display:none');
                            showLoading();
                            window.location.reload();
                        }
                    }
                });
            } else if (controller == 30) {
                var AmenazasJson = {
                    "Descripcion": $('input#Nombre01').val(),
                    "Id": $('input#Id01').val(),
                    "activo": $('input#Activo01').val()
                };
                $.ajax({
                    url: '/Amenazas/EditarAmenaza',
                    type: "POST",
                    data: JSON.stringify({ AmenazasJson: AmenazasJson }),
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    success: function(data) {
                        if (data.result) {
                            $('#confirm').attr('style', 'display:none');
                            window.location.reload();
                        } else {
                            $('#confirm').attr('style', 'display:none');
                            showLoading();
                            window.location.reload();
                        }
                    }
                });

            } else if (controller == 31) {
                var ProvinciasJson = {
                    "Id": $('input#ID01').val(),
                    "Codigo": $('input#Codigo01').val(),
                    "ID_ComAut": $('input#Comunidad01').val(),
                    "Nombre": $('input#Nombre01').val(),
                    "activo": $('input#Activo01').val()
                };
                $.ajax({
                    url: '/Provincias/AltaProvincias',
                    type: "POST",
                    data: JSON.stringify({ ProvinciasJson: ProvinciasJson }),
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        if (data.result) {
                            $('#confirm').attr('style', 'display:none');
                            window.location.reload();
                        } else {
                            $('#confirm').attr('style', 'display:none');
                            showLoading();
                            window.location.reload();
                        }
                    }
                });
            } else if (controller == 32) {
                var ProvinciasJson = {
                    "Id": $('input#ID01').val(),
                    "Codigo": $('input#Codigo01').val(),
                    "ID_ComAut": $('input#Comunidad01').val(),
                    "Nombre": $('input#Nombre01').val(),
                    "activo": $('input#Activo01').val()
                };
                $.ajax({
                    url: '/Provincias/EditarProvincias',
                    type: "POST",
                    data: JSON.stringify({ ProvinciasJson: ProvinciasJson }),
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        if (data.result) {
                            $('#confirm').attr('style', 'display:none');
                            window.location.reload();
                        } else {
                            $('#confirm').attr('style', 'display:none');
                            showLoading();
                            window.location.reload();
                        }
                    }
                });

            } else if (controller == 33) {
                var paisJson = {
                    "Nombre": $('input#Nombre01').val(),
                    "Codigo": $('input#Codigo01').val(),
                    "Tipo": $('input#Tipo01').val(),
                    "ID_Provincia": $('input#Provincia01').val()
                };
                $.ajax({
                    url: '/Paises/PaisAlta',
                    type: "POST",
                    data: JSON.stringify({ PaisJson: paisJson }),
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        if (data.result) {
                            $('#confirm').attr('style', 'display:none');
                            window.location.reload();
                        } else {
                            $('#confirm').attr('style', 'display:none');
                            showLoading();
                            window.location.reload();
                        }
                    }
                });
            } else if (controller == 34) {
                var paisJson = {
                    "ID_Pais": $('input#ID01').val(),
                    "Nombre": $('input#Nombre01').val(),
                    "Codigo": $('input#Codigo01').val(),
                    "Tipo": $('input#Tipo01').val(),
                    "ID_Provincia": $('input#Provincia01').val(),
                    "activo": $('input#Activo01').val()
                };
                $.ajax({
                    url: '/Paises/PaisEditar',
                    type: "POST",
                    data: JSON.stringify({ PaisJson: paisJson }),
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        if (data.result) {
                            $('#confirm').attr('style', 'display:none');
                            window.location.reload();
                        } else {
                            $('#confirm').attr('style', 'display:none');
                            showLoading();
                            window.location.reload();
                        }
                    }
                });
            } else if (controller == 35) {
                var paisJson = {
                    "Nombre": $('input#Nombre01').val(),
                    "Codigo": $('input#Codigo01').val(),
                    "Tipo": $('input#Tipo01').val(),
                    "ID_Provincia": $('input#Provincia01').val()
                };
                $.ajax({
                    url: '/Paises/CiudadAlta',
                    type: "POST",
                    data: JSON.stringify({ PaisJson: paisJson }),
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        if (data.result) {
                            $('#confirm').attr('style', 'display:none');
                            window.location.reload();
                        } else {
                            $('#confirm').attr('style', 'display:none');
                            showLoading();
                            window.location.reload();
                        }
                    }
                });
            } else if (controller == 36) {
                var paisJson = {
                    "ID_Pais": $('input#ID01').val(),
                    "Nombre": $('input#Nombre01').val(),
                    "Codigo": $('input#Codigo01').val(),
                    "Tipo": $('input#Tipo01').val(),
                    "ID_Provincia": $('input#Provincia01').val(),
                    "activo": $('input#Activo01').val()
                };
                $.ajax({
                    url: '/Paises/CiudadEditar',
                    type: "POST",
                    data: JSON.stringify({ PaisJson: paisJson }),
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        if (data.result) {
                            $('#confirm').attr('style', 'display:none');
                            window.location.reload();
                        } else {
                            $('#confirm').attr('style', 'display:none');
                            showLoading();
                            window.location.reload();
                        }
                    }
                });
            } else if (controller == 37) {
                var tipoInstalacionJson = {
                    "Nombre": $('input#Descripcion01').val()
                };
                $.ajax({
                    url: '/TipoInstalaciones/AltaTipoInstalacion',
                    type: "POST",
                    data: JSON.stringify({ TipoInstalacionJson: tipoInstalacionJson }),
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        if (data.result) {
                            $('#confirm').attr('style', 'display:none');
                            window.location.reload();
                        } else {
                            $('#confirm').attr('style', 'display:none');
                            showLoading();
                            window.location.reload();
                        }
                    }
                });
            } else if (controller == 38) {
                var tipoInstalacionJson = {
                    "Nombre": $('input#Descripcion01').val(),
                    "activo": $('input#Activo01').val()
                };
                $.ajax({
                    url: '/TipoInstalaciones/EditarTipoInstalacion',
                    type: "POST",
                    data: JSON.stringify({ TipoInstalacionJson: tipoInstalacionJson }),
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        if (data.result) {
                            $('#confirm').attr('style', 'display:none');
                            window.location.reload();
                        } else {
                            $('#confirm').attr('style', 'display:none');
                            showLoading();
                            window.location.reload();
                        }
                    }
                });
            } else if (controller == 39) {
                var comunidadJson = {
                    "Nombre": $('input#Nombre01').val(),
                    "Pais": $('input#Pais01').val()
                };
                $.ajax({
                    url: '/Comunidades/ComunidadAlta',
                    type: "POST",
                    data: JSON.stringify({ ComunidadJson: comunidadJson }),
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        if (data.result) {
                            $('#confirm').attr('style', 'display:none');
                            window.location.reload();
                        } else {
                            $('#confirm').attr('style', 'display:none');
                            showLoading();
                            window.location.reload();
                        }
                    }
                });
            } else if (controller == 40) {
                var comunidadJson = {
                    "Id": $('input#ID01').val(),
                    "Nombre": $('input#Comunidad01').val(),
                    "Pais": $('input#Pais01').val(),
                    "activo": $('input#Activo01').val()
                };
                $.ajax({
                    url: '/Comunidades/ComunidadEditar',
                    type: "POST",
                    data: JSON.stringify({ ComunidadJson: comunidadJson }),
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        if (data.result) {
                            $('#confirm').attr('style', 'display:none');
                            window.location.reload();
                        } else {
                            $('#confirm').attr('style', 'display:none');
                            showLoading();
                            window.location.reload();
                        }
                    }
                });

            } else if (controller == 41) {
                var OrganismoJson = {
                    "Id": $('input#Id01').val(),
                    "Nombre": $('input#NombreOrganismo01.form-control').val(),
                    "Tipo": $('input#Tipo01').val(),
                    "Observaciones": $('textarea#Observaciones01').val(),
                    "ID_Ciudad": $('input#Ciudad01').val(),
                    "Cod_Postal": $('input#codPostal01').val(),
                    "ID_Provincia": $('input#Provincias01').val(),
                    "Direccion": $('input#DireccionOrganismo01').val() };
                    $.ajax({
                            url: '/Organismo/AltaOrganismo',
                            type: "POST",
                            data: JSON.stringify({ OrganismoJson: OrganismoJson }),
                            dataType: "json",
                            contentType: "application/json; charset=utf-8",
                            success: function(data) {
                                if (data.result) {
                                    $('#confirm').attr('style', 'display:none');
                                    window.location.reload();
                                } else {
                                    $('#confirm').attr('style', 'display:none');
                                    showLoading();
                                    window.location.reload();
                                }
                         }
                    });

            } else if (controller == 42) {
                var OrganismoJson = {
                    "Id": $('input#Id01').val(),
                    "Nombre": $('input#NombreOrganismo01.form-control').val(),
                    "Tipo": $('input#Tipo01').val(),
                    "Observaciones": $('textarea#Observaciones01').val(),
                    "ID_Ciudad": $('input#Ciudad01').val(),
                    "Cod_Postal": $('input#codPostal01').val(),
                    "ID_Provincia": $('input#Provincias01').val(),
                    "Direccion": $('input#DireccionOrganismo01').val()
                };

                $.ajax({
                    url: '/Organismo/GuardarOrganismo',
                    type: "POST",
                    data: JSON.stringify({ OrganismoJson: OrganismoJson }),
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        if (data.result) {
                            $('#confirm').attr('style', 'display:none');
                            window.location.reload();
                        } else {
                            $('#confirm').attr('style', 'display:none');
                            showLoading();
                            window.location.reload();
                        }
                    }
                });
            } else if (controller == 43) {
                var puertosJson = {
                    "Id": $('input#Id01').val(),
                    "Nombre": $('input#Nombre01').val(),
                    "id_Organismo": $('input#Organismo01').val(),
                    "Responsable": $('input#Responsable01').val(),
                    "Direccion": $('input#Direccion01').val(),
                    "Id_Ciudad": $('input#Ciudad01').val(),
                    "Cod_Postal": $('input#codPostal01').val(),
                    "Id_Provincia": $('input#Provincia01').val(),
                    "Observaciones": $('textarea#Observaciones01').val(),
                    "Id_CapMarit": $('input#Capitania01').val(),
                    "activo": $('input#Activo01').val()
                };
                $.ajax({
                    url: '/Puertos/AltaPuerto',
                    type: "POST",
                    data: JSON.stringify({ PuertosJson: puertosJson }),
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        if (data.result) {
                            $('#confirm').attr('style', 'display:none');
                            window.location.reload();
                        } else {
                            $('#confirm').attr('style', 'display:none');
                            showLoading();
                            window.location.reload();
                        }
                    }
                });

            } else if (controller == 44) {
                var puertosJson = {
                    "Id": $('input#Id01').val(),
                    "Nombre": $('input#Nombre01').val(),
                    "id_Organismo": $('input#Organismo01').val(),
                    "Responsable": $('input#Responsable01').val(),
                    "Direccion": $('input#Direccion01').val(),
                    "Id_Ciudad": $('input#Ciudad01').val(),
                    "Cod_Postal": $('input#codPostal01').val(),
                    "Id_Provincia": $('input#Provincia01').val(),
                    "Observaciones": $('textarea#Observaciones01').val(),
                    "Id_CapMarit": $('input#Capitania01').val(),
                    "activo": $('input#Activo01').val()
                };
                $.ajax({
                    url: '/Puertos/EditarPuerto',
                    type: "POST",
                    data: JSON.stringify({ PuertosJson: puertosJson }),
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        if (data.result) {
                            $('#confirm').attr('style', 'display:none');
                            window.location.reload();
                        } else {
                            $('#confirm').attr('style', 'display:none');
                            showLoading();
                            window.location.reload();
                        }
                    }
                });
            } else if (controller == 46) {
                var comitesJson = {
                    "Id": $('input#Id01').val(),
                    "id_Organismo": $('input#Organismo01').val(),
                    "Nivel": $('input#Nivel01').val(),
                    "Fecha": $('input#Fecha01').val(),
                    "Observaciones": $('textarea#Observaciones01').val(),
                    "id_Puerto": $('input#Puerto01').val()                    
                };
                $.ajax({
                    url: '/Puertos/AltaComite',
                    type: "POST",
                    data: JSON.stringify({ PuertosJson: puertosJson }),
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        if (data.result) {
                            $('#confirm').attr('style', 'display:none');
                            window.location.reload();
                        } else {
                            $('#confirm').attr('style', 'display:none');
                            showLoading();
                            window.location.reload();
                        }
                    }
                });

            } else if (controller == 47) {
                var comitesJson = {
                    "Id": $('input#Id01').val(),
                    "id_Organismo": $('input#Organismo01').val(),
                    "Nivel": $('input#Nivel01').val(),
                    "Fecha": $('input#Fecha01').val(),
                    "Observaciones": $('textarea#Observaciones01').val(),
                    "id_Puerto": $('input#Puerto01').val()                  
                };
                $.ajax({
                    url: '/Puertos/EditarComite',
                    type: "POST",
                    data: JSON.stringify({ PuertosJson: puertosJson }),
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        if (data.result) {
                            $('#confirm').attr('style', 'display:none');
                            window.location.reload();
                        } else {
                            $('#confirm').attr('style', 'display:none');
                            showLoading();
                            window.location.reload();
                        }
                    }
                });
            } else if (controller == 49) {                
                $.ajax({
                    url: $('input#URL').val(),
                    type: "POST",
                    data: JSON.stringify({ id: id }),
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        if (data.result) {
                            $('#confirm').attr('style', 'display:none');
                            window.location.reload();
                        } else {
                            $('#confirm').attr('style', 'display:none');
                            showLoading();
                            alert(data.Message, 'Eliminar');
                        }
                    }
                });
            } else if (controller == 50) {
                var MotivosJson = {
                    "Motivo":      $('input#Nombre01').val(),
                    "Descripcion": $('input#Descripcion01').val(),
                    "Id":          $('input#Id01').val(),
                    "activo":      $('input#Activo01').val()
                };
                $.ajax({
                    url: '/Motivos/AltaMotivo',
                    type: "POST",
                    data: JSON.stringify({ MotivosJson: MotivosJson }),
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    success: function(data) {
                        if (data.result) {
                            $('#confirm').attr('style', 'display:none');
                            window.location.reload();
                        } else {
                            $('#confirm').attr('style', 'display:none');
                            showLoading();
                            window.location.reload();
                        }
                    }
                });
            } else if (controller == 51) {
                var MotivosJson = {
                    "Motivo":      $('input#Nombre01').val(),
                    "Descripcion": $('input#Descripcion01').val(),
                    "Id":          $('input#Id01').val(),
                    "activo":      $('input#Activo01').val()
                };
                $.ajax({
                    url: '/Motivos/EditarMotivo',
                    type: "POST",
                    data: JSON.stringify({ MotivosJson: MotivosJson }),
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    success: function(data) {
                        if (data.result) {
                            $('#confirm').attr('style', 'display:none');
                            window.location.reload();
                        } else {
                            $('#confirm').attr('style', 'display:none');
                            showLoading();
                            window.location.reload();
                        }
                    }
                });
            } else if (controller == 52) {
                var InstalacionJson = {
                    "Id": $('input#Id01').val(),
                    "Nombre": $('input#NombreInstalacion01.form-control').val(),
                    "id_Puerto": $('input#Puerto01').val(),
                    "id_Tipo": $('input#Tipo01').val(),
                    "es_concesionada": $('input#Concesionada01').val(),
                    "Empresa": $('input#Empresa01').val(),
                    "Clasificacion": $('input#clasificacion01').val(),
                    "Direccion": $('input#DireccionInstalacion01').val(),
                    "ID_Ciudad": $('input#Ciudad01').val(),
                    "Cod_Postal": $('input#codPostal01').val(),
                    "ID_Provincia": $('input#Provincias01').val(),
                    "OMI": $('input#omi01').val(),
                    "Nivel": $('input#Nivel01').val(),
                    "Longitud": $('input#longitud01').val(),
                    "Latitud": $('input#latitud01').val(),
                    "Declara_Cumpli": $('input#declaracion01').val(),
                    "Fech_Declara_Cumpli": $('input#datetimepicker').val(),
                    "Activo": $('input#activo02').val(),
                    "Observaciones": $('textarea#Observaciones01').val(),
                    "action": action
                };
                $.ajax({
                    url: '/Instalaciones/AltaInstalacion',
                    type: "POST",
                    data: JSON.stringify({ InstalacionJson: InstalacionJson }),
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        if (data.result) {
                            $('#confirm').attr('style', 'display:none');
                            window.location.reload();
                        } else {
                            $('#confirm').attr('style', 'display:none');
                            showLoading();
                            window.location.reload();
                        }
                    }
                });

            } else if (controller == 53) {
                var InstalacionJson = {
                    "Id": $('input#Id01').val(),
                    "Nombre": $('input#NombreInstalacion01.form-control').val(),
                    "id_Puerto": $('input#Puerto01').val(),
                    "id_Tipo": $('input#Tipo01').val(),
                    "es_concesionada": $('input#Concesionada01').val(),
                    "Empresa": $('input#Empresa01').val(),
                    "Clasificacion": $('input#clasificacion01').val(),
                    "Direccion": $('input#DireccionInstalacion01').val(),
                    "ID_Ciudad": $('input#Ciudad01').val(),
                    "Cod_Postal": $('input#codPostal01').val(),
                    "ID_Provincia": $('input#Provincias01').val(),
                    "OMI": $('input#omi01').val(),
                    "Nivel": $('input#Nivel01').val(),
                    "Longitud": $('input#longitud01').val(),
                    "Latitud": $('input#latitud01').val(),
                    "Declara_Cumpli": $('input#declaracion01').val(),
                    "Fech_Declara_Cumpli": $('input#datetimepicker').val(),
                    "Activo": $('input#activo02').val(),
                    "Observaciones": $('textarea#Observaciones01').val(),
                    "action": action
                };
                $.ajax({
                    url: '/Instalaciones/GuardarInstalacion',
                    type: "POST",
                    data: JSON.stringify({ InstalacionJson: InstalacionJson }),
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        if (data.result) {
                            $('#confirm').attr('style', 'display:none');
                            window.location.reload();
                        } else {
                            $('#confirm').attr('style', 'display:none');
                            showLoading();
                            window.location.reload();
                        }
                    }
                });
            }

            _this.hideConfirm(300);
        });
    },
    
   showConfirmGrupos: function (message, id) {
       var _this = this;
       $('#confirm span#message').html(message);
       $('#confirm').fadeIn(100);
       $('#confirm button').off();
       $('#confirm button#no').click(function () {
           window.location.reload();
       });
       $('#confirm button#yes').click(function () {
           $.ajax({
               url: '/Grupos/BajaConfirmed',
               type: "POST",
               data: JSON.stringify({ id: id }),
               dataType: "json",
               contentType: "application/json; charset=utf-8",
               success: function (data) {
                   if (data.result) {
                       $('#confirm').attr('style', 'display:none');
                       window.location.reload();
                   } else {
                       $('#confirm').attr('style', 'display:none');
                       showLoading();
                       alert(data.Message, 'Eliminar');
                   }
               }
           });
           _this.hideConfirm(300);
       });

   },
    
    showConfirmPerfil: function (message, id) {
       var _this = this;
       $('#confirm span#message').html(message);
       $('#confirm').fadeIn(100);
       $('#confirm button').off();
       $('#confirm button#no').click(function () {
           window.location.reload();
       });
       $('#confirm button#yes').click(function () {
           $.ajax({
               url: '/Perfiles/BajaPerfilConfirmed',
               type: "POST",
               data: JSON.stringify({ id: id }),
               dataType: "json",
               contentType: "application/json; charset=utf-8",
               success: function (data) {
                   if (data.result) {
                       $('#confirm').attr('style', 'display:none');
                       location.reload();
                   } else {
                       $('#confirm').attr('style', 'display:none');
                       showLoading();
                       alert(data.Message, 'Eliminar');
                   }
               }
           });
           _this.hideConfirm(300);
       });

   },
    
   showConfirmPerfiles: function (message, id) {
       var _this = this;
       $('#confirm span#message').html(message);
       $('#confirm').fadeIn(100);
       $('#confirm button').off();
       $('#confirm button#no').click(function () {
           window.location.reload();
       });
       $('#confirm button#yes').click(function () {
           $.ajax({
               url: '/Grupos/BajaPerfilConfirmed',
               type: "POST",
               data: JSON.stringify({ id: id }),
               dataType: "json",
               contentType: "application/json; charset=utf-8",
               success: function (data) {
                   if (data.result) {
                       $('#confirm').attr('style', 'display:none');
                       location.reload();
                   } else {
                       $('#confirm').attr('style', 'display:none');
                       showLoading();
                       alert(data.Message, 'Eliminar');
                   }
               }
           });
           _this.hideConfirm(300);
       });

   },
   showConfirmReiniciar: function (message, id) {
       var _this = this;
       $('#confirm span#message').html(message);
       $('#confirm').fadeIn(100);
       $('#confirm button').off();
       $('#confirm button#no').click(function() {
           $('#confirm').attr('style', 'display:none');
           hideLoading();
       });
       $('#confirm button#yes').click(function () {
           $.ajax({
               url: '/Usuarios/ReiniciarContrasena',
               type: "POST",
               data: { id: $('input#IdUsuario').val() },
               success: function(data) {
                   if (data.result==true) {
                       $('#confirm').attr('style', 'display:none');
                       hideLoading();
                   } else {
                       $('#confirm').attr('style', 'display:none');
                       showLoading();
                       alert(data.Message);
                   }
               }
           });
           _this.hideConfirm(300);
       });
   },
       
   showConfirmCambio: function (message, id, item) {
        var _this = this;
        $('#confirm span#message').html(message);
        $('#confirm').fadeIn(100);
        $('#confirm button').off();
        $('#confirm button#no').click(function() {
            $.ajax({
                type: "POST",
                url: '/Usuarios/Edit',
                data: { id: $('input#IdUsuario').val() },
                success: function (data) {
                    if (data) {
                        $('#MostrarMensaje').html('Modificar Usuario');
                        $('#ListUsers').html(data);
                        hideLoading();
                    }
                }
            });
            _this.hideConfirm(300);
        });
        $('#confirm button#yes').click(function () {
            if (ValidarAllCombos())
                $('input#ValidarAllCombos').val("1");
            else
                $('input#ValidarAllCombos').val("");
            GuardarUsuarioEditar();
            $.ajax({
                url: '/Usuarios/EliminarDependencias',
                type: "POST",
                data: JSON.stringify({ id: id,item:item }),
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                success: function(data) {
                    if (data.result) {
                        $('#confirm').attr('style', 'display:none');
                        $.ajax({
                            type: "POST",
                            url: '/Usuarios/Visualizar',
                            data: { id: id },
                            success: function(data) {
                                if (data) {
                                    hideLoading();
                                    $('#MostrarMensaje').html('Visualizar Usuario');
                                    $('#ListUsers').html(data);
                                    $('#Edit-page-Usuario').html('');
                                }
                            }
                        });
                    } else {
                        $('#confirm').attr('style', 'display:none');
                        showLoading();
                        alert(data.Message);
                    }
                }
            });
            _this.hideConfirm(300);
        });

    },
    

   showConfirmAvisoContacto: function (message, id) {
       var _this = this;
       $('#confirm span#message').html(message);
       $('#confirm').fadeIn(100);
       $('#confirm button').off();
       $('#confirm button#no').click(function () {
           $('#confirm').attr('style', 'display:none');
           hideLoading();
       });
       $('#confirm button#yes').click(function () {
           $.ajax({
               type: "POST",
               url: '/Organismo/BorrarContactoOrganismo',
               data: { id: id },
               success: function (data) {
                   if (data.result) {
                       $.ajax({
                           type: "POST",
                           url: '/Organismo/VisualizarOrganismo',
                           data: { id: null },
                           success: function (data) {
                               if (data) {
                                   hideLoading();
                                   $('#MostrarMensaje').html('Visualizar Organismo');
                                   $('#Edit-Organismo').html(data);
                                   $('#ListOrganismo').html('');
                               }
                           }
                       });
                   }
               }
           });
           _this.hideConfirm(300);
       });
   },
    
   showConfirmAvisoOrganismo: function(message, id) {
       var _this = this;
       $('#confirm span#message').html(message);
       $('#confirm').fadeIn(100);
       $('#confirm button').off();
       $('#confirm button#no').click(function() {
           $('#confirm').attr('style', 'display:none');
           hideLoading();
       });
       $('#confirm button#yes').click(function() {
           $.ajax({
               type: "POST",
               url: '/Organismo/BorrarOrganismo',
               data: { id: id },
               success: function (data) {
                   if (data.result) {
                       location.reload();
                   } else {
                       alert(data.Message, 'Organismos');
                   }
               }
           });
           _this.hideConfirm(300);
       });
   },
   
   showConfirmAviso: function (message, id) {
       var _this = this;
       $('#confirm span#message').html(message);
       $('#confirm').fadeIn(100);
       $('#confirm button').off();
       $('#confirm button#no').click(function () {
           VisualizarUsuario();
           hideLoading();
           $('#confirm').attr('style', 'display:none');
       });
       $('#confirm button#yes').click(function () {
           $.ajax({
               url: '/Usuarios/EliminarDocumentos',
               type: "POST",
               data: JSON.stringify({ id: id}),
               dataType: "json",
               contentType: "application/json; charset=utf-8",
               success: function (data) {
                   if (data.result) {
                       $('#confirm').attr('style', 'display:none');
                       //VISUALIZAR USUARIO
                       $.ajax({
                           type: "POST",
                           url: '/Usuarios/Visualizar',
                           data: { id: $('input#id01.form-control').val() },
                           success: function (data) {
                               if (data) {
                                   hideLoading();
                                   $('#MostrarMensaje').html('Visualizar Usuario');
                                   $('#ListUsers').html(data);
                                   $('#Edit-page').html('');
                               }
                           }
                       });
                   } else {
                       $('#confirm').attr('style', 'display:none');
                       showLoading();
                       alert(data.Message, 'Eliminar');
                   }
               }
           });
           _this.hideConfirm(300);
       });

   },
   showConfirmSesion: function (message) {
        var _this = this;
        $('#confirm span#message').html(message);
        $('#confirm').fadeIn(100);
        $('#confirm button').off();
        $('#confirm button#no').click(function() {
            window.location.reload();
        });
        $('#confirm button#yes').click(function () {
            $.ajax({
                url: '/Login/LogOff',
                type: "POST",
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    if (data.result) {
                        closeWindows();
                        $('#confirm').attr('style', 'display:none');
                        window.location.href = "/Menu";
                    } else {
                        $('#confirm').attr('style', 'display:none');
                        showLoading();
                        alert(data.Message);
                    }
                }
            });
        });

   },

  
   showConfirmAvisoOperador: function (message, id) {
           var _this = this;
           $('#confirm span#message').html(message);
           $('#confirm').fadeIn(100);
           $('#confirm button').off();
           $('#confirm button#no').click(function () {
               $('#confirm').attr('style', 'display:none');
               hideLoading();
           });
           $('#confirm button#yes').click(function () {
               jsShowWindowLoad("Ejecutando datos.....");
               $.ajax({
                   type: "POST",
                   url: '/Operadores/EliminarOperador',
                   data: { id: id },
                   success: function (data) {
                       if (data.result) {
                           $.ajax({
                               type: "POST",
                               url: '/Operadores/OperadoresEditar',
                               data: { id: id },
                               success: function (data) {
                                   if (data) {
                                       $('#MostrarMensaje').html('Visualizar Instalacion');
                                       $('#myModalOperadores').modal('hide');
                                       $('#OperadoresRefrescar').html(data);
                                       $('#Edit-page').html('');
                                       hideLoading();
                                       jsRemoveWindowLoad();
                                   }
                               }
                           });
                       }
                   }
               });
               _this.hideConfirm(300);
           });
   },
    
   showConfirmAvisoOperadorPuerto: function (message, id) {
       var _this = this;
       $('#confirm span#message').html(message);
       $('#confirm').fadeIn(100);
       $('#confirm button').off();
       $('#confirm button#no').click(function () {
           $('#confirm').attr('style', 'display:none');
           hideLoading();
       });
       $('#confirm button#yes').click(function () {
           jsShowWindowLoad("Ejecutando datos.....");
           $.ajax({
               type: "POST",
               url: '/Puertos/EliminarOperador',
               data: { id: id },
               success: function (data) {
                   if (data.result) {
                       $.ajax({
                           type: "POST",
                           url: '/Puertos/OperadoresEditar',
                           data: { id: id },
                           success: function (data) {
                               if (data) {
                                   $('#MostrarMensaje').html('Visualizar Puerto');
                                   $('#myModalOperadores').modal('hide');
                                   $('#OperadoresRefrescar').html(data);
                                   $('#Edit-page').html('');
                                   hideLoading();
                                   jsRemoveWindowLoad();
                               }
                           }
                       });
                   }
               }
           });
           _this.hideConfirm(300);
       });
   },

   showConfirmAvisoDoc: function (message, id, url1, url2, mensaje, VentanaModal, VentanaRefrescar) {
       var _this = this;
       $('#confirm span#message').html(message);
       $('#confirm').fadeIn(100);
       $('#confirm button').off();
       $('#confirm button#no').click(function () {
           $('#confirm').attr('style', 'display:none');
           hideLoading();
       });
       $('#confirm button#yes').click(function () {
           $.ajax({
               type: "POST",
               url: url1,
               data: { id: id },
               success: function (data) {
                   if (data.result) {                    
                       $.ajax({
                           type: "POST",
                           url: url2,
                           data: { id: id },
                           success: function (data) {
                               if (data) {                                  
                                   $('#MostrarMensaje').html(mensaje);
                                   $(VentanaModal).modal('hide');
                                   $(VentanaRefrescar).html(data);
                                   $('#Edit-page').html('');
                                   hideLoading();
                               }
                           }
                       });
                   }
               }
           });
           _this.hideConfirm(300);
       });
   },

   showConfirmAvisoAdicional: function (message, id, Canal) {
       var _this = this;
       $('#confirm span#message').html(message);
       $('#confirm').fadeIn(100);
       $('#confirm button').off();
       $('#confirm button#no').click(function () {
           $('#confirm').attr('style', 'display:none');
           hideLoading();
       });
       $('#confirm button#yes').click(function () {
           jsShowWindowLoad("Ejecutando datos.....");
           $.ajax({               
               type: "POST",
               url: '/Centros/EliminarAdicional',
               data: { id: id },
               success: function (data) {
                   if (data.result ==1) {                      
                       $.ajax({
                           type: "POST",
                           url: '/Centros/AsociadosEditTel',
                           data: { id: id },
                           success: function (data) {
                               if (data) {
                                   $('#MostrarMensaje').html('Visualizar Centro');
                                   $('#myModalDatos').modal('hide');
                                   $('#TelRefrescar').html(data);
                                   $('#Edit-page').html('');
                                   hideLoading();
                                   jsRemoveWindowLoad();
                               }
                           }
                       });
                   }
                   else if (data.result == 2) {
                       $.ajax({
                           type: "POST",
                           url: '/Centros/AsociadosEditFax',
                           data: { id: id },
                           success: function (data) {
                               if (data) {
                                   $('#MostrarMensaje').html('Visualizar Centro');
                                   $('#myModalDatos').modal('hide');
                                   $('#FaxRefrescar').html(data);
                                   $('#Edit-page').html('');
                                   hideLoading();
                                   jsRemoveWindowLoad();
                               }
                           }
                       });
                   }
                   else {                      
                           $.ajax({
                               type: "POST",
                               url: '/Centros/AsociadosEditMail',
                               data: { id: id },
                               success: function (data) {
                                   if (data) {
                                       $('#MostrarMensaje').html('Visualizar Centro');
                                       $('#myModalDatos').modal('hide');
                                       $('#MailRefrescar').html(data);
                                       $('#Edit-page').html('');
                                       hideLoading();
                                       jsRemoveWindowLoad();
                                   }
                               }
                           });
                       }
               }
           });
           _this.hideConfirm(300);
       });
   },


    hideConfirm: function(time) {
        $('#confirm').fadeOut(time);
    }
};

window.alert = function (message, retorno, id, controller) {
    if (retorno == undefined) {
        Alert.showAlert(message);
    } else if (retorno == "Mensaje") {
        Alert.showMensaje(message);
    } else if (retorno == "Confirm") {
        Alert.showConfirm(message, id, controller);
    } else if (retorno == "Grupos") {
        Alert.showConfirmGrupos(message, id);
    } else if (retorno == "PerfilAlta") {
        Alert.showConfirmPerfilAlta(message);
    } else if (retorno == "Perfiles") {
        Alert.showConfirmPerfiles(message, id);
    } else if (retorno == "Perfil") {
        Alert.showConfirmPerfil(message, id);
    } else if (retorno == "Sesion") {
        Alert.showConfirmSesion(message);
    } else if (retorno == "Registro") {
        Alert.showVisualizarUsers(message, '/Menu');  
    } else if (retorno == "Agregar") {
        Alert.showAgregar(message, '#myModalAgregar');
    } else if (retorno == "Eliminar") {
        Alert.showEliminar(message);
    } else if (retorno == "Login") {
        Alert.showLogin(message);
    } else if (retorno == "Visualizar") {
        Alert.showVisualizar(message);
    } else if (retorno == "VisualizarUsers") {
        Alert.showVisualizarUsers(message, '/Usuarios/ListadoUsuarios');
    } else if (retorno == "LoginPerfil") {
        Alert.showLoginPerfil(message);
    } else if (retorno == "Grupo") {
        Alert.showGrupo(message);
    } else if (retorno == "SubirFichero") {
        Alert.showSubirFichero(message);
    } else if (retorno == "TipoInstalaciones") {
        Alert.showAgregar(message, '#myModalInstalaciones'); 
    } else if (retorno == "Bienes") {
        Alert.showAgregar(message, '#myModalBienes');  
    } else if (retorno == "Amenazas") {
        Alert.showAgregar(message, '#myModalAmenazas');
    } else if (retorno == "Centros") {
        Alert.showAgregar(message, '#myModalCentros');
    } else if (retorno == "Paises") {
        Alert.showAgregar(message, '#myModalPaises');
    } else if (retorno == "Ciudades") {
        Alert.showAgregar(message, '#myModalCiudades');
    } else if (retorno == "Provincias") {
        Alert.showAgregar(message, '#myModalProvincias');
    } else if (retorno == "Comunidades") {
        Alert.showAgregar(message, '#myModalComunidad'); 
    } else if (retorno == "Organismos") {
        Alert.showAlertOrganismo(message);
    } else if (retorno == "Puertos") {
        Alert.showAlertOrganismo(message);
    } else if (retorno == "Comites") {
        Alert.showAlertOrganismo(message);
    } else if (retorno == "Motivos") {
        Alert.showAlertOrganismo(message);
    } else if (retorno == "ComitesDoc") {
        Alert.showVisualizarUsers(message, '/Comites/ListadoComites');
    } else if (retorno == "Operadores") {
        Alert.showAgregar(message, '#myModalOperadores');
    } else if (retorno == "Documentos") {
        Alert.showVisualizar(message);
    } else if (retorno == "Correo") {
        Alert.showCorreo(message);
    } else if (retorno == "Procedimientos") {
        Alert.showEliminar(message); 
    }

    else {
        Alert.showConfirmaRegistro(message);
    }
};

window.ReiniciarContrasena = function (message, id) {
    Alert.showConfirmReiniciar(message, id);
};

window.mensajeOK = function (message,id,item) {
    Alert.showConfirmCambio(message,id,item);
};

window.AvisoOK = function (message, id) {
    Alert.showConfirmAviso(message, id);
};

window.alerta = function (message) {
    Alert.showConfirmaIndex(message);
};

window.AvisoOKOrganismo = function (message, id) {
    Alert.showConfirmAvisoOrganismo(message, id);
};

window.AvisoOKContacto = function (message, id) {
    Alert.showConfirmAvisoContacto(message, id);
};

window.MensajeriaOrganismo = function (message, id) {
    Alert.showOrganismo(message, id);
};
/*
 callback es lo que se ejecutará si el usuario pulsa en el boton de ok
*/
window.confirm = function (message, callback) {
    Alert.showConfirm(message, callback);
};

window.AvisoOKComite = function (message, id) {
    Alert.showConfirmAvisoDoc(message, id, '/Comites/EliminarDocumentos', '/Comites/AsociadosEditComites', 'Visualizar Comite', '#myModalUpload', '#MinisteriodelInteriorRefrescar');
};

window.AvisoOKDocOperador = function (message, id) {
    Alert.showConfirmAvisoDoc(message, id, '/Operadores/EliminarDocumentos', '/Operadores/AsociadosEditOperadores', 'Visualizar Instalacion', '#myModalUpload', '#DocumentosRefrescar');
};

window.AvisoOKOperador = function (message, id) {
    Alert.showConfirmAvisoOperador(message, id);
};

window.AvisoOKOperadorPuerto = function (message, id) {
    Alert.showConfirmAvisoOperadorPuerto(message, id);
};

window.AvisoOKDocFormacion = function (message, id) {
    Alert.showConfirmAvisoDoc(message, id, '/Formaciones/EliminarDocumentos', '/Formaciones/AsociadosEditFormaciones', 'Visualizar Formacion', '#myModalUpload', '#DocumentosRefrescar');
};

window.AvisoOKDocDeclara = function (message, id) {
    Alert.showConfirmAvisoDoc(message, id, '/DeclaraMaritima/EliminarDocumentos', '/DeclaraMaritima/AsociadosEditDeclara', 'Visualizar Declaración Marítima', '#myModalUpload', '#DocumentosRefrescar');
};

window.AvisoOKDocPracticas = function (message, id) {
    Alert.showConfirmAvisoDoc(message, id, '/Practicas/EliminarDocumentos', '/Practicas/AsociadosEditPracticas', 'Visualizar Practica', '#myModalUpload', '#DocumentosRefrescar');
};

window.AvisoOKDocIncidentes = function (message, id) {
    Alert.showConfirmAvisoDoc(message, id, '/Incidentes/EliminarDocumentos', '/Incidentes/AsociadosEditIncidentes', 'Visualizar Incidente', '#myModalUpload', '#DocumentosRefrescar');
};
window.AvisoOKDocAuditorias = function (message, id) {
    Alert.showConfirmAvisoDoc(message, id, '/Auditorias/EliminarDocumentos', '/Auditorias/AsociadosEditAuditorias', 'Visualizar Auditoria', '#myModalUpload', '#DocumentosRefrescar');
};
window.AvisoOKDocNiveles = function (message, id) {
    Alert.showConfirmAvisoDoc(message, id, '/Niveles/EliminarDocumentos', '/Niveles/AsociadosEditNiveles', 'Visualizar Nivel', '#myModalUpload', '#DocumentosRefrescar');  
};
window.AvisoOKDocMantenimiento = function (message, id) {
    Alert.showConfirmAvisoDoc(message, id, '/Mantenimiento/EliminarDocumentos', '/Mantenimiento/AsociadosEditMantenimiento', 'Visualizar Mantenimiento', '#myModalUpload', '#DocumentosRefrescar');
};
window.AvisoOKDocComunicacion = function (message, id) {
    Alert.showConfirmAvisoDoc(message, id, '/Comunicaciones/EliminarDocumentos', '/Comunicaciones/AsociadosEditComunicacion', 'Visualizar Comunicación', '#myModalUpload', '#DocumentosRefrescar');
};
window.AvisoOKDocProcedimiento = function (message, id) {
    Alert.showConfirmAvisoDoc(message, id, '/Procedimientos/EliminarDocumentos', '/Procedimientos/AsociadosEditProcedimientos', 'Visualizar Procedimiento', '#myModalUpload', '#DocumentosRefrescar');
};
window.AvisoOKEvaluacion = function (message, id) {
    Alert.showConfirmAvisoDoc(message, id, '/Evaluaciones/EliminarDocumentos', '/Evaluaciones/AsociadosEditEvaluaciones', 'Visualizar Evaluación', '#myModalUpload', '#DocumentosRefrescar');
};


window.AvisoOKAdicional = function (message, id, Canal) {
    Alert.showConfirmAvisoAdicional(message, id, Canal);
};
