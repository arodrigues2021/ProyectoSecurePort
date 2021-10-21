namespace SecurePort.Web.Controllers
{
    #region Using
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.ModelConfiguration;
    using Kendo.Mvc.Extensions;
    using Kendo.Mvc.UI;
    using SecurePort.Entities.Enums;
    using SecurePort.Entities.Exceptions;
    using SecurePort.Entities.Models;
    using SecurePort.Services.Interfaces;
    using SecurePort.Services.Repository;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Validation;
    using System.Linq;
    using System.Web.Mvc;
    using System.Configuration;
    using SecurePort.Entities.Models.Json;  
    using System.Collections;

    #endregion
    
    public class EvaluacionesController : BaseController
    {
        public EvaluacionesController(IServiciosTipoInstalacion serviciosTipoInstalacion, 
                                        InstalacionesRepository repoInstalaciones,  
                                        TipoInstalacionRepository repoTipoInstalaciones, 
                                        IServiciosUsuarios servicioUsuarios,
                                        IServiciosBienes serviciosBienes,
                                        IServiciosAmenazas serviciosAmenazas,
                                        MOV_Detalle_InstalacionRepository repoMovDetalleInstalaciones,
                                        IServiciosEvaluaciones seviciosEvaluaciones,
                                        EvaluacionesRepository repoEvaluaciones,
                                        Versiones_EvaluacionRepository repoVersionesEvaluacion,
                                        IServiciosPuertos servicioPuertos,
                                        DocumentosUsuarioRepository repoDocumentosUsuario,
                                        IServiciosDocumentos servicioDocumentos,
                                        Historico_EvaluacionRepository repoHistorico_Evalacion,
                                        Detalle_EvaluacionRepository repoDetalle_Evaluacion,
                                        TrazasRepository repotrazas, 
                                        ILogger logger)
            : base( serviciosTipoInstalacion, 
                     repoInstalaciones,
                     repoTipoInstalaciones,
                     servicioUsuarios,
                     serviciosBienes,
                     serviciosAmenazas,
                     repoMovDetalleInstalaciones,
                     seviciosEvaluaciones,
                     repoEvaluaciones, 
                     repoVersionesEvaluacion,
                     servicioPuertos,
                     repoDocumentosUsuario,
                     servicioDocumentos,
                     repoHistorico_Evalacion,
                     repoDetalle_Evaluacion,
                     repotrazas, 
                     logger)
            {
            }

        #region Buscador
        
        /// <summary>
        /// Permite buscar las instalaciones
        /// </summary>
        public ActionResult BuscadorInstalaciones([DataSourceRequest]DataSourceRequest request)
        {
            try
            {
                if (this.UsuarioFrontalSession.Usuario.categoria == (int)EnumCategoria.Administrador
                    || this.UsuarioFrontalSession.Usuario.categoria == (int)EnumCategoria.PuertosdelEstado)
                {
                    return this.Json(this._serviciosTipoInstalacion.GetAllInstalaciones().Where(x=> x.Es_Activo == true).OrderBy(x => x.Organismo).ToDataSourceResult(request));
                }
                var categoria = this.UsuarioFrontalSession.Usuario.categoria;
                return this.Json(this._serviciosTipoInstalacion.GetAllInstalaciones(this.UsuarioFrontalSession.ListDependenciasDepenUsuarios,categoria).Where(x=> x.Es_Activo == true).ToDataSourceResult(request));
            }
            catch (Exception ex)
            {
                this.log.PublishException(new SecureportException(ex));
                throw;
            }
        }

        /// <summary>
        /// Permite buscar las instalaciones/Bienes
        /// </summary>
        public ActionResult BuscadorInstalacionesBienes([DataSourceRequest]DataSourceRequest request)
        {
            try
            {
                    return this.Json(this._serviciosTipoInstalacion.GetAllBienesByInstalacion(Convert.ToInt32(this.idInstalacion)).OrderBy(z=> z.Bien).ToDataSourceResult(request));                
            }
            catch (Exception ex)
            {
                this.log.PublishException(new SecureportException(ex));
                throw;
            }
        }

        public int CalculoRiesgos(int _ID, int _Ai, int _Sd, int _Io, int _Dv, int _De1, int _De2, int _Re, int _Pr, int _ISA1, int _ISA2, int _ISA3)
        {
            var resultado = 0;

            try
            {
                int _IV = 0;

                int _IC = 0;

                double _ISA = 0;

                int _De = 0;

                int CtDe = 3000000;

                _IV = Math.Min(4, Convert.ToInt32((_Ai + _Sd + _Io / 3) + 0.5));

                _De = Convert.ToInt32((_De1 + _De2 / CtDe) + 0.5);

                if (_De > 30)
                {
                    _De = 5;
                }
                else if (20 < _De && _De <= 30)
                {
                    _De = 4;
                }
                else if (5 < _De && _De <= 20)
                {
                    _De = 3;
                }
                else if (0.1 < _De && _De <= 5)
                {
                    _De = 2;
                }
                else
                {
                    _De = 1;
                }

                int _IC01 = (((_Dv + _De + _Re + _Pr) / 3) / 2);

                _IC = Math.Min(5, Convert.ToInt32(_IC01 * Math.Min(1.375, (1 + ((_ISA1 + _ISA2 + _ISA3) / 40))) + 0.5));

                 resultado = _ID * _IV * _IC;
            }
            catch (Exception ex)
            {
                this.log.PublishException(new SecureportException(ex));
                throw;
            }
            
            return resultado;

        }


        /// <summary>
        /// Permite buscar los bienes padres
        /// </summary>
        public ActionResult HierarchyBinding_BienPadre([DataSourceRequest]DataSourceRequest request)
        {
            try
            {
                if (!this.ListBienesPadres.Any())
                {
                    this.ListBienesPadres = this._serviciosBienes.GetAllBienesPadre(Convert.ToInt32(this.idInstalacion)).GroupBy(c => c.Id).Select(grp => grp.First()).ToList();
                }

                this.ListBienesPadres.Where(x => x.activado!="Quitar").ToList().ForEach(
                    b =>
                    {
                        IEnumerable<AmenazasViewModel> result = this._serviciosAmenazas.GetAllAmenazasPadreHijos().Where(x => x.es_activo);

                        result.ToList().ForEach(
                            x =>
                            {
                                ListAmenazasPadreHijos listAmenazasPadreHijos = new ListAmenazasPadreHijos();

                                listAmenazasPadreHijos.IdpadreHijo  = b.Id.ToString();

                                listAmenazasPadreHijos.Id           = x.Id;

                                listAmenazasPadreHijos.IdPH         = Convert.ToInt32(b.Id);

                                listAmenazasPadreHijos.Descripcion  = x.Descripcion;

                                listAmenazasPadreHijos.activado     = "No";

                                if (!this.ListAmenazasPadreHijos.ToList().Where(z => z.IdpadreHijo == b.Id.ToString() && z.Id == x.Id).Any())
                                {
                                    this.ListAmenazasPadreHijos.Add(listAmenazasPadreHijos);
                                }

                            });
                    });

                if (this.Idpadre != null)
                {
                    this.ListBienesPadres.Where(x => x.Id == Convert.ToInt32(this.Idpadre)).ToList().ForEach(
                        b =>
                        {
                                    this.ListBienesPadres.Remove(b);
                                    BienesViewModel listAmena  = new BienesViewModel();
                                    listAmena.Id               = b.Id;
                                    listAmena.Descripcion      = b.Descripcion;
                                    listAmena.id_Tipo_IIPP     = b.id_Tipo_IIPP;
                                    listAmena.id_Bien_Padre    = b.id_Bien_Padre;
                                    listAmena.es_activo        = b.es_activo;
                                    listAmena.Bien_Padre       = b.Bien_Padre;
                                    listAmena.Tipo_instalacion = b.Tipo_instalacion;
                                    listAmena.activado         = "Si";
                                    listAmena.sel              = b.sel;
                                    this.ListBienesPadres.Add(listAmena); 
                        });
                }
                
                return this.Json(this.ListBienesPadres.ToList().OrderBy(x => x.Id).ToDataSourceResult(request));
            }
            catch (Exception ex)
            {
                this.log.PublishException(new SecureportException(ex));
                throw;
            }
        }

        /// <summary>
        /// Permite buscar los bienes hijos
        /// </summary>
        public ActionResult HierarchyBinding_BienHijo2([DataSourceRequest]DataSourceRequest request, int? BienPadreId)
        {

            try
            {
                this._serviciosBienes.GetAllBienesHijos(BienPadreId).ToList().ForEach(
                    b =>
                    {
                        BienesHijosViewModel listAmena = new BienesHijosViewModel();
                        listAmena.Id                = b.Id;
                        listAmena.Descripcion       = b.Descripcion;
                        listAmena.id_Tipo_IIPP      = b.id_Tipo_IIPP;
                        listAmena.id_Bien_Padre     = b.id_Bien_Padre;
                        listAmena.es_activo         = b.es_activo;
                        listAmena.Bien_Padre        = b.Bien_Padre;
                        listAmena.Tipo_instalacion  = b.Tipo_instalacion;
                        listAmena.activado          = "No";
                        listAmena.sel               = b.sel;

                        if (!this.ListBienHijos.ToList().Where(x => x.id_Bien_Padre == BienPadreId && x.Id == b.Id).Any())
                        {
                            this.ListBienHijos.Add(listAmena);

                            this.IdpadreHijo = null;
                        }
                    });

                this.ListAmenazasPadreHijos.Where(z => z.IdPH == BienPadreId).ToList().ForEach(
                    b =>
                    {
                        ListAmenazasPadreHijos listAmenazasPadreHijos = new ListAmenazasPadreHijos();

                        listAmenazasPadreHijos.IdpadreHijo  = BienPadreId.ToString();

                        listAmenazasPadreHijos.Id           = b.Id;

                        listAmenazasPadreHijos.IdPH         = Convert.ToInt32(BienPadreId);

                        listAmenazasPadreHijos.Descripcion  = b.Descripcion;

                        listAmenazasPadreHijos.activado     = "No";

                        this.ListAmenazasPadreHijos.Remove(b);

                        this.ListAmenazasPadreHijos.Add(listAmenazasPadreHijos);
                    });

                if (this.IdpadreHijo != null)
                {
                    string[] Code = this.IdpadreHijo.Split('-');

                    if (Code.Count() > 1)
                    {
                        int CodePadre = Convert.ToInt32(Code[0]);

                        int CodeHijo = Convert.ToInt32(Code[1]);

                        this.ListBienHijos.Where(x => x.Id == CodeHijo && x.id_Bien_Padre == CodePadre).ToList().ForEach(
                            b =>
                            {
                                this.ListBienHijos.Remove(b);
                                BienesHijosViewModel listAmena = new BienesHijosViewModel();
                                listAmena.Id               = b.Id;
                                listAmena.Descripcion      = b.Descripcion;
                                listAmena.id_Tipo_IIPP     = b.id_Tipo_IIPP;
                                listAmena.id_Bien_Padre    = b.id_Bien_Padre;
                                listAmena.es_activo        = b.es_activo;
                                listAmena.Bien_Padre       = b.Bien_Padre;
                                listAmena.Tipo_instalacion = b.Tipo_instalacion;
                                listAmena.activado         = "Si";
                                listAmena.sel              = b.sel;
                                this.ListBienHijos.Add(listAmena);
                            });
                    }
                }

                var resultado = ListBienHijos.Where(b => b.id_Bien_Padre == BienPadreId).ToList();

                return this.Json(resultado.OrderBy(x => x.Id).ToDataSourceResult(request));
            }
            catch (Exception ex)
            {
                this.log.PublishException(new SecureportException(ex));
                
                throw;
            }
        }

        /// <summary>
        /// Permite buscar los bienes hijos
        /// </summary>
        public ActionResult HierarchyBinding_BienHijo([DataSourceRequest]DataSourceRequest request, int? BienPadreId)
        {

            try
            {
                this._serviciosBienes.GetAllBienesHijos(BienPadreId).ToList().ForEach(
                    b =>
                    {
                        BienesHijosViewModel listAmena = new BienesHijosViewModel();
                        listAmena.Id                = b.Id;
                        listAmena.Descripcion       = b.Descripcion;
                        listAmena.id_Tipo_IIPP      = b.id_Tipo_IIPP;
                        listAmena.id_Bien_Padre     = b.id_Bien_Padre;
                        listAmena.es_activo         = b.es_activo;
                        listAmena.Bien_Padre        = b.Bien_Padre;
                        listAmena.Tipo_instalacion  = b.Tipo_instalacion;
                        listAmena.activado          = "No";
                        listAmena.sel               = b.sel;

                        if (!this.ListBienHijos.ToList().Where(x => x.id_Bien_Padre == BienPadreId && x.Id == b.Id).Any())
                        {
                            this.ListBienHijos.Add(listAmena);
                        }
                    });

                //Guardado en Base de datos.
                this.ListBienHijos.ToList().ForEach(
                    b =>
                    {
                        this._serviciosBienes.GetAllBienesHijosAmenazas(BienPadreId,this.idInstalacion).Where(x => x.Id == b.Id).ToList().ForEach(
                                S =>
                                {
                                    this.ListBienHijos.Remove(b);
                                    BienesHijosViewModel listAmena = new BienesHijosViewModel();
                                    listAmena.Id               = b.Id;
                                    listAmena.Descripcion      = b.Descripcion;
                                    listAmena.id_Tipo_IIPP     = b.id_Tipo_IIPP;
                                    listAmena.id_Bien_Padre    = b.id_Bien_Padre;
                                    listAmena.es_activo        = b.es_activo;
                                    listAmena.Bien_Padre       = b.Bien_Padre;
                                    listAmena.Tipo_instalacion = b.Tipo_instalacion;
                                    listAmena.activado         = "Si";
                                    listAmena.sel              = b.sel;
                                    this.ListBienHijos.Add(listAmena);
                                });
                    });
                
                    if (this.IdpadreHijo != null)
                    {
                        string[] Code = this.IdpadreHijo.Split('-');

                        if (Code.Count() > 1)
                        {
                            int CodePadre = Convert.ToInt32(Code[0]);

                            int CodeHijo = Convert.ToInt32(Code[1]);

                            this.ListBienHijos.Where(x => x.Id == CodeHijo && x.id_Bien_Padre == CodePadre).ToList().ForEach(
                                b =>
                                {
                                    this.ListBienHijos.Where(x => x.Id == b.Id && x.id_Bien_Padre == b.id_Bien_Padre).ToList().ForEach(
                                        S =>
                                        {
                                            this.ListBienHijos.Remove(b);
                                            BienesHijosViewModel listAmena = new BienesHijosViewModel();
                                            listAmena.Id               = b.Id;
                                            listAmena.Descripcion      = b.Descripcion;
                                            listAmena.id_Tipo_IIPP     = b.id_Tipo_IIPP;
                                            listAmena.id_Bien_Padre    = b.id_Bien_Padre;
                                            listAmena.es_activo        = b.es_activo;
                                            listAmena.Bien_Padre       = b.Bien_Padre;
                                            listAmena.Tipo_instalacion = b.Tipo_instalacion;
                                            listAmena.activado         = "Si";
                                            listAmena.sel              = b.sel;
                                            this.ListBienHijos.Add(listAmena);
                                        });
                                });
                        }
                    }

                var resultado = ListBienHijos.Where(b => b.id_Bien_Padre == BienPadreId).ToList();

                return this.Json(resultado.OrderBy(x => x.Id).ToDataSourceResult(request));
            }
            catch (Exception ex)
            {
                this.log.PublishException(new SecureportException(ex));
                throw;
            }
        }
        
            /// <summary>
        /// Permite buscar los Bien/Amenazas
        /// </summary>
        public ActionResult BuscadorBienAmenazas([DataSourceRequest]DataSourceRequest request, int? BienId)
        {
            try
            {
                return this.Json(this._serviciosTipoInstalacion.GetAllBienesByInstalacionBien(Convert.ToInt32(this.idInstalacion), Convert.ToInt32(BienId)).OrderBy(z => z.Descripcion).ToDataSourceResult(request));                
            }
            catch (Exception ex)
            {
                this.log.PublishException(new SecureportException(ex));
                throw;
            }
        }
        #endregion

        #region action
        
        /// <summary>
        /// Retorna una lista de todas las Instalaciones.
        /// </summary>
        public ActionResult ListadoInstalaciones()
        {
            try
            {
                ViewBag.Entrada = "Instalacion";
                return this.PartialView("ListadoInstalaciones", this.UsuarioFrontalSession);
            }
            catch (Exception ex)
            {
                this.log.PublishException(new SecureportException(ex));
                throw;
            }
        }


        public ActionResult ListadoBienesPadreHijos()
        {
            try
            {
                return this.PartialView("BienPadreHijo", this.UsuarioFrontalSession);
            }
            catch (Exception ex)
            {
                this.log.PublishException(new SecureportException(ex));
                throw;
            }
        }
        #endregion

        #region Asignacionn Instalaciones/Bienes/Amenazas
        [HttpPost]
        public ActionResult AsignarEncabezado(string nombre)
        {
            try
            {
                InstalacionViewModel Instalacion = new InstalacionViewModel();

                Instalacion = this._serviciosTipoInstalacion.GetAllInstalaciones().Where(x => x.Id == Convert.ToInt32(this.idInstalacion)).FirstOrDefault();

                BienPadreHijo BienPadreHijo = new BienPadreHijo();

                BienPadreHijo.NombreInstalacion = Instalacion.Nombre;

                BienPadreHijo.organismo = Instalacion.Organismo;

                BienPadreHijo.puerto = Instalacion.NombrePuerto;

                BienPadreHijo.clasificacion = Instalacion.NombreClasificacion;

                BienPadreHijo.bien = nombre;

                return this.PartialView("~/Views/Evaluaciones/PartialEncabezado.cshtml", BienPadreHijo);
            }
            catch (Exception ex)
            {
                this.log.PublishException(new SecureportException(ex));

                return this.Json(new { result = false, Message = ex.Message });
            }
        }
        /// <summary>
        /// Visualizar lista de Bienes/amenazas para asignar
        /// </summary>
        [HttpPost]
        public ActionResult AsignarBienAmenaza(int? id)
        {
            try
            {
                if (id == null)
                    id = this.idInstalacion;
                else
                    this.idInstalacion = id;

                if (this.ListAmenazasPadreHijos.Any())
                {
                    this.ListAmenazasPadreHijos.ToList().ForEach(p => { this.ListAmenazasPadreHijos.Remove(p); });
                }
                
                if (this.ListBienHijos.Any())
                {
                    this.ListBienHijos.ToList().ForEach(a => { this.ListBienHijos.Remove(a); });
                }

                if (this.ListBienesPadres.Any())
                {
                    this.ListBienesPadres.ToList().ForEach(a => { this.ListBienesPadres.Remove(a); });
                }

                this.IdpadreHijo = null;

                this.Idpadre = null;

                return this.PartialView("~/Views/Evaluaciones/BienPadreHijo.cshtml");
            }
            catch (Exception ex)
            {
                this.log.PublishException(new SecureportException(ex));

                return this.Json(new { result = false, Message = ex.Message });
            }
        }

        /// <summary>
        /// Visualizar lista de Bienes/amenazas para asignar
        /// </summary>
        [HttpPost]
        public ActionResult ListaBienesAmenazas(int? id, string origen)
        {
            try
            {
                if (id == null)
                    id = this.idInstalacion;

                InstalacionViewModel Instalacion = new InstalacionViewModel();

                Instalacion = this._serviciosTipoInstalacion.GetAllInstalaciones().Where(x => x.Id == (int)id).FirstOrDefault();

                ViewBag.Instalacion = Instalacion.Nombre;

                ViewBag.Organismo = Instalacion.Organismo;

                ViewBag.Puerto = Instalacion.NombrePuerto;

                this.idInstalacion = Instalacion.Id;

                ViewBag.Retorno = origen;

                return this.PartialView("~/Views/Evaluaciones/ListaInstalacionBienes.cshtml", this.UsuarioFrontalSession);
            }
            catch (Exception ex)
            {
                this.log.PublishException(new SecureportException(ex));

                return this.Json(new { result = false, Message = ex.Message });
            }
        }

        [HttpPost]
        public ActionResult VisualizarAmenazas(int? id)
        {
            try
            {

                if (id != null)
                    this.idEvaluaBien = id;

                return this.PartialView("~/Views/Evaluaciones/_PartialAmenazasBien.cshtml", this.UsuarioFrontalSession);

            }
            catch (Exception ex)
            {
                this.log.PublishException(new SecureportException(ex));

                return this.Json(new { result = false, Message = ex.Message });
            }
        }

        public ActionResult VisualizarAcciones()
        {
            this.listaAcciones = new List<AccionesViewModel>();

            AccionesViewModel acciones0 = new AccionesViewModel()
            {
                Id = 1,
                Name = "Seleccionar Opción"
            };
            this.listaAcciones.Add(acciones0);

            if (this.UsuarioFrontalSession.ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Mov_Evaluaciones.GESTION_IIPP.ToDescription()).Count() > 0)
            {
                AccionesViewModel acciones1 = new AccionesViewModel()
                {
                    Id = 2,
                    Name = "Asignar Bienes/Amenazas"
                };
                this.listaAcciones.Add(acciones1);
            }
            if (this.UsuarioFrontalSession.ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Mov_Evaluaciones.DETALLE_IIPP.ToDescription()).Count() > 0)
            {
                AccionesViewModel acciones2 = new AccionesViewModel()
                {
                    Id = 3,
                    Name = "Lista de Bienes/Amenazas"
                };
                this.listaAcciones.Add(acciones2);
            }

            return Json(this.listaAcciones, JsonRequestBehavior.AllowGet);

        }

        #endregion

        #region Evaluacion

        /// <summary>
        /// Retorna una lista de todas las Evaluaciones.
        /// </summary>
        public ActionResult ListadoEvaluaciones()
        {
            try
            {
                ViewBag.UrlDelete     = "/Evaluaciones/EliminarEvaluacion";    
                ViewBag.Mensaje       = "Lista de Evaluaciones IIPP";
                ViewBag.Entrada       = "Evaluacion";
                return this.PartialView("ListadoInstalaciones", this.UsuarioFrontalSession);
            }
            catch (Exception ex)
            {
                this.log.PublishException(new SecureportException(ex));
                throw;
            }
        }

        /// <summary>
        /// Permite buscar las Evaluaciones
        /// </summary>
        public ActionResult BuscadorEvaluaciones([DataSourceRequest]DataSourceRequest request)
        {
            try
            {
                if (this.UsuarioFrontalSession.Usuario.categoria == (int)EnumCategoria.Administrador
                    || this.UsuarioFrontalSession.Usuario.categoria == (int)EnumCategoria.PuertosdelEstado)
                {
                    return this.Json(this._serviciosEvaluaciones.GetAllEvaluaciones().ToDataSourceResult(request));
                }
                var categoria = this.UsuarioFrontalSession.Usuario.categoria;
                return this.Json(this._serviciosEvaluaciones.GetAllEvaluaciones(this.UsuarioFrontalSession.ListDependenciasDepenUsuarios, categoria).ToDataSourceResult(request));
            }
            catch (Exception ex)
            {
                this.log.PublishException(new SecureportException(ex));
                throw;
            }
        }

        /// <summary>
        /// Permite mostrar la pantalla para el Alta Evaluaciones.
        /// </summary>
        [HttpPost]
        public ActionResult Create()
        {
            EvaluacionesViewModel evaluacion = new EvaluacionesViewModel();

            this.comboPuertos(null);

            if (((MultiSelectList)(this.ViewData["puertos"])).Count() == 1)
                evaluacion.Id_Puerto = Convert.ToInt32(((SelectList)this.ViewData["puertos"]).FirstOrDefault().Value);

            this.comboInstalacionesPuertos(null, null);

            ViewBag.disabled = true;

            ViewBag.Combo = "display:block";

            ViewBag.Texto = "display:none";

            ViewBag.display = "display:none";

            ViewBag.Mensaje = "Alta Evaluación";

            ViewBag.action = General.AltaGeneral.ToDescription();

            evaluacion.Id = 1;

            evaluacion.Fech_Alta = DateTime.MinValue;

            ViewBag.ToolEvalua = false;

            ViewBag.altaHis = "display:none";

            ViewBag.altaDoc = "display:none";

            this.ViewBag.Observaciones = string.Empty;

            ViewBag.FechaMostrar = "display:none";

            ViewBag.MostrarLugar = "display:block";

            return this.PartialView("~/Views/Evaluaciones/_PartialEvaluaciones.cshtml", evaluacion);
        }

        /// <summary>
        /// Visualizar los datos de Evaluacionces
        /// </summary>
        [HttpPost]
        public ActionResult VisualizarEvaluacion(int? id, string Accion)
        {
            bool valor = true;

            if (id != null)
                this.idEvaluacion = (int)id;
            else if (this.idEvaluacion != 0)
                id = this.idEvaluacion;

            valor = ValidarCategoria(this.idEvaluacion, this.repoEvaluaciones.Single(b => b.Id == this.idEvaluacion));

            if (Accion != General.AltaVer.ToDescription())
            {
                if (!valor)
                    return this.Json(new { result = 0, Message = ModificarCategoria.MessageCategoria.ToDescription() + "  " + this.Description });
            }

            try
            {
                if (Accion == General.EditGeneral.ToDescription())
                {
                    ViewBag.disabled = true;
                    ViewBag.Combo = "display:block";
                    ViewBag.Texto = "display:none";
                    ViewBag.Mensaje = "Editar Evaluación";
                    ViewBag.ToolEvalua = false;
                    ViewBag.altaHis = "display:none";
                    ViewBag.altaDoc = "display:block";                  
                }
                else
                {
                    ViewBag.disabled = false;
                    ViewBag.Combo = "display:none";
                    ViewBag.Texto = "display:block";
                    ViewBag.Mensaje = "Visualizar Evaluación";
                    ViewBag.ToolEvalua = true;
                    ViewBag.altaHis = "display:block";
                    ViewBag.altaDoc = "display:block";
                }

                ViewBag.FechaMostrar = "display:block";
                ViewBag.MostrarLugar = "display:none";
                
                ViewBag.action = General.EditGeneral.ToDescription();

                this.ViewBag.navegador = this.Browser;

                EvaluacionesViewModel evaluacion = new EvaluacionesViewModel();

                this._serviciosEvaluaciones.GetAllEvaluaciones().Where(x => x.Id == id).ToList().ForEach(
                   p =>
                   {
                       evaluacion.Id = p.Id;
                       evaluacion.Nombre = p.Nombre;
                       evaluacion.Id_Puerto = p.Id_Puerto;
                       evaluacion.Puerto = p.Puerto;
                       evaluacion.Id_IIPP = p.Id_IIPP;
                       evaluacion.IIPP = p.IIPP;
                       evaluacion.Estado = p.Estado;
                       evaluacion.NombreEstado = p.NombreEstado;
                       evaluacion.Responsable = p.Responsable;
                       evaluacion.Cargo = p.Cargo;
                       evaluacion.Version = p.Version;
                       this.ViewBag.Observaciones = p.Observaciones;
                       evaluacion.Fech_Alta = p.Fech_Alta;
                   });

                

                this.VersionEval = evaluacion.Version;

                this.comboPuertos(null);

                this.comboInstalacionesPuertos(null, evaluacion.Id_Puerto);



                return this.PartialView("~/Views/Evaluaciones/_PartialEvaluaciones.cshtml", evaluacion);
            }
            catch (Exception ex)
            {
                this.log.PublishException(new SecureportException(ex));

                return this.Json(new { result = false, Message = ex.Message });
            }
        }

        /// <summary>
        /// Alta-Editar Evaluaicones.
        /// </summary>
        [HttpPost]
        public ActionResult AltaEditarEvaluacion(EvaluacionesJson evaluacionesJson)
        {
            try
            {
                switch (evaluacionesJson.action)
                {

                    case "Alta":
                        AltaEvaluacion(evaluacionesJson);
                        break;
                    default:
                        EditarEvaluacion(evaluacionesJson);
                        break;
                }

                return this.Json(new { result = 0 });
            }
            catch (DbUpdateException ex)
            {
                this.log.PublishException(new SecureportExceptionDbUpdate(ex));

                return this.Json(new { result = false, Message = ex.Message });
            }
            catch (ModelValidationException ex)
            {
                return this.Json(new { result = false, Message = ex.Message });
            }
            catch (DbEntityValidationException ex)
            {
                this.log.PublishException(new SecureportExceptionDbEntity(ex));

                return this.Json(new { result = false, Message = ex.Message });
            }
            catch (Exception ex)
            {
                this.log.PublishException(new SecureportException(ex));

                return this.Json(new { result = false, Message = ex.Message });
            }

        }

        [HttpPost]
        public ActionResult AltaEvaluacion(EvaluacionesJson evaluacionJson)
        {
            try
            {
                Evaluaciones evaluacion = new Evaluaciones();

                evaluacion.ID_Usu_alta = this.UsuarioFrontalSession.Usuario.id;

                evaluacion.Fech_Alta = DateTime.Now;

                evaluacion.Nombre = evaluacionJson.Nombre;

                evaluacion.Id_Puerto = evaluacionJson.Id_Puerto;

                evaluacion.Id_IIPP = evaluacionJson.Id_IIPP;
                
                this.repoEvaluaciones.Create(evaluacion);

                evaluacionJson.Id = this.db.Evaluaciones.OrderByDescending(u => u.Id).FirstOrDefault().Id;

                this.trazas(230, (int)this.UsuarioFrontalSession.Usuario.id, "Se ha dado de alta un registro en SPEVA_Evaluaciones con ID" + "(" + evaluacionJson.Id.ToString() + ")");

                Versiones_Evaluacion versionEval = new Versiones_Evaluacion();

                versionEval.Id_Evaluacion = evaluacionJson.Id;

                versionEval.Version = 1;

                versionEval.Estado = 1;

                versionEval.Responsable = evaluacionJson.Responsable;

                versionEval.Cargo = evaluacionJson.Cargo;

                versionEval.Observaciones = CambiarTexto(evaluacionJson.Observaciones);

                versionEval.ID_Usu_alta = this.UsuarioFrontalSession.Usuario.id;

                versionEval.Fech_Alta = DateTime.Now;

                repoVersionesEvaluacion.Create(versionEval);

               this.trazas(230, (int)this.UsuarioFrontalSession.Usuario.id, "Se ha dado de alta un registro en SPEVA_Versiones_Evaluacion con ID" + "(" +this.db.Versiones_Evaluacion.OrderByDescending(u => u.Id).FirstOrDefault().Id.ToString() + ")");
                                
                this.idEvaluacion = evaluacionJson.Id;
                
                return this.Json(new { result = 1 });

            }
            catch (ModelValidationException ex)
            {
                return this.Json(new { result = false, Message = ex.Message });
            }
            catch (DbEntityValidationException ex)
            {
                this.log.PublishException(new SecureportExceptionDbEntity(ex));

                return this.Json(new { result = false, Message = ex.Message });
            }
            catch (Exception ex)
            {
                this.log.PublishException(new SecureportException(ex));

                return Json(new { status = "error", Message = ex.Message, result = 2 });
            }


        }

        [HttpPost]
        public ActionResult EditarEvaluacion(EvaluacionesJson evaluacionJson)
        {
            try
            {
                Evaluaciones evaluacion = this.repoEvaluaciones.Single(x => x.Id == evaluacionJson.Id);

                evaluacion.Nombre = evaluacionJson.Nombre;

                evaluacion.Id_Puerto = evaluacionJson.Id_Puerto;

                evaluacion.Id_IIPP = evaluacionJson.Id_IIPP;
                
                this.repoEvaluaciones.Update(evaluacion);

                this.trazas(231, (int)this.UsuarioFrontalSession.Usuario.id, "Se ha realizado la modificación de un registro en SPEVA_Evaluaciones con ID" + "(" + evaluacionJson.Id.ToString() + ")");
                
                Versiones_Evaluacion versionEvalua = this.repoVersionesEvaluacion.Single(x => x.Id_Evaluacion == evaluacionJson.Id && x.Version == evaluacionJson.Version);

                //versionEvalua.Estado = evaluacionJson.Estado;

                versionEvalua.Responsable = evaluacionJson.Responsable;

                versionEvalua.Cargo = evaluacionJson.Cargo;

                versionEvalua.Observaciones = CambiarTexto(evaluacionJson.Observaciones);

                this.repoVersionesEvaluacion.Update(versionEvalua);

                this.trazas(231, (int)this.UsuarioFrontalSession.Usuario.id, "Se ha realizado la modificación de un registro en SPEVA_Versiones_Evaluacion con ID" + "(" + versionEvalua.Id.ToString() + ")");
                
                this.idEvaluacion = evaluacionJson.Id;

                return this.Json(new { result = 1});


            }
            catch (ModelValidationException ex)
            {
                return this.Json(new { result = 2, Message = ex.Message });
            }
            catch (DbEntityValidationException ex)
            {
                this.log.PublishException(new SecureportExceptionDbEntity(ex));

                return this.Json(new { result = 2, Message = ex.Message });
            }
            catch (Exception ex)
            {
                this.log.PublishException(new SecureportException(ex));

                //return Json(new { status = "error", Message = ex.Message, result = 2 });
                return this.Json(new { result = 2, Message = ex.Message });
            }


        }

        /// <summary>
        /// Borrar Gisis.
        /// </summary>
        [HttpPost]
        public ActionResult EliminarEvaluacion(int Id)
        {
            try
            {

                if (ValidarCategoria(Id, this.repoEvaluaciones.Single(x => x.Id == Id)))
                {
                    IEnumerable<Versiones_Evaluacion> versionesEvaluacion = _serviciosEvaluaciones.GetAllVersiones(Id);
                    EvaluacionesViewModel VersionActual = _serviciosEvaluaciones.GetAllEvaluaciones().Where(x => x.Id == Id).FirstOrDefault();

                    // verifica que la version no esta con la fecha de generacion del informe
                    if (VersionActual.Fech_Info_Eval == null)
                    {
                        //Borra Historico
                        Historico_Evaluacion HistEvalua = this.repoHistorico_Evaluacion.Single(x => x.Id_Evaluacion == Id && x.Id_Version_Eval == VersionActual.Version);
                        if (HistEvalua != null){
                            this.repoHistorico_Evaluacion.Delete(HistEvalua);
                            this.trazas(276, (int)this.UsuarioFrontalSession.Usuario.id, "Se ha realizado la eliminación de un registro en SPEVA_Historico_Evaluacion con Id_evaluación" + "(" + Id.ToString() + ") e Id_Versión" + "(" + VersionActual.Version.ToString() + ")");
                        }
    
                        // Borra detalle Evalucion
                        Detalle_Evaluacion DetailEvalua = this.repoDetalle_Evaluacion.Single(x => x.Id_Evaluacion == Id && x.Id_Version_Eval == VersionActual.Version);
                        if (DetailEvalua != null){
                            this.repoDetalle_Evaluacion.Delete(DetailEvalua);
                            this.trazas(276, (int)this.UsuarioFrontalSession.Usuario.id, "Se ha realizado la eliminación de un registro en SPEVA_Detalle_Evaluacion con Id_evaluación" + "(" + Id.ToString() + ") e Id_Versión" + "(" + VersionActual.Version.ToString() + ")");
                        }

                        // Borra la version Evaluacion
                        Versiones_Evaluacion VersEvalua = this.repoVersionesEvaluacion.Single(x => x.Id_Evaluacion == Id && x.Version == VersionActual.Version);
                        if(VersEvalua != null){
                            this.repoVersionesEvaluacion.Delete(VersEvalua);
                            this.trazas(276, (int)this.UsuarioFrontalSession.Usuario.id, "Se ha realizado la eliminación de un registro en SPEVA_Versiones_Evaluacion con Id_evaluación" + "(" + Id.ToString() + ") e Id_Versión" + "(" + VersionActual.Version.ToString() + ")");
                        }

                        if (versionesEvaluacion.Count() == 1)
                        {
                            this.repoEvaluaciones.Delete(this.repoEvaluaciones.Single(x => x.Id == Id));
                            this.trazas(276, (int)this.UsuarioFrontalSession.Usuario.id, "Se ha realizado la eliminación de un registro en SPEVA_Evaluaciones con ID" + "(" + Id.ToString() + ")");
                        }

                    } else {
                        return this.Json(new { result = false, Message = "No se puede eliminar esta evaluacion porque ya se ha generado el informe de evaluacion"});
                    }

                }
                else
                {
                    return this.Json(new { result = false, Message = EliminarCategoria.MessageCategoria.ToDescription() + "  " + this.Description });
                }

            }
            catch (DbUpdateException ex)
            {
                this.log.PublishException(new SecureportExceptionDbUpdate(ex));

                return this.Json(new { result = false, Message = MessageDelete(ex) });
            }
            catch (ModelValidationException ex)
            {
                return this.Json(new { result = false, Message = ex.Message });
            }
            catch (DbEntityValidationException ex)
            {
                this.log.PublishException(new SecureportExceptionDbEntity(ex));

                return this.Json(new { result = false, Message = ex.Message });
            }
            catch (Exception ex)
            {
                this.log.PublishException(new SecureportException(ex));

                return this.Json(new { result = false, Message = ex.Message });
            }

            return this.Json(new { result = true });
        }
        
        [HttpPost]
        public ActionResult CambiarPuerto(int? id)
        {
            try
            {

                this.comboInstalacionesPuertos(null, id);

                return this.PartialView("~/Views/Gisis/ComboPuerto.cshtml");

            }
            catch (Exception ex)
            {
                this.log.PublishException(new SecureportException(ex));

                return this.Json(new { result = false, Message = ex.Message });
            }

        }


        public ActionResult AccionesEvaluacion()
        {
            this.listaAcciones = new List<AccionesViewModel>();
            AccionesViewModel acciones0 = new AccionesViewModel()
            {
                Id = 1,
                Name = "Seleccionar Opción"
            };
            this.listaAcciones.Add(acciones0);

            if (this.UsuarioFrontalSession.ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Mov_Evaluaciones.COMP_EVAL.ToDescription()).Count() > 0)
            {
                AccionesViewModel acciones1 = new AccionesViewModel()
                {
                    Id = 2,
                    Name = "Completar matriz riesgos"
                };
                this.listaAcciones.Add(acciones1);
            }
            if (this.UsuarioFrontalSession.ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Mov_Evaluaciones.CONSULTA_EVAL.ToDescription()).Count() > 0)
            {
                AccionesViewModel acciones2 = new AccionesViewModel()
                {
                    Id = 3,
                    Name = "Visualizar Matriz riesgos"
                };
                this.listaAcciones.Add(acciones2);
            }

            return Json(this.listaAcciones, JsonRequestBehavior.AllowGet);

        }


        #endregion

        #region Historico Evalaciones

        public ActionResult AsociadosHist()
        {
            return PartialView("~/Views/Evaluaciones/_PartialHistoricoEvalua.cshtml", this.UsuarioFrontalSession);

        }

        public ActionResult BuscadorHistorico([DataSourceRequest]DataSourceRequest request,int? IdVersion)
        {
            try
            {
               this.idEvaluacion = this.idEvaluacion == null ? 0 :(int)this.idEvaluacion;
               this.VersionEval = this.VersionEval == null ? 0 : (int)this.VersionEval;

                return this.Json(this._serviciosEvaluaciones.GetEvaluacionesByIdEvaluaVersion((int)this.idEvaluacion, (int)this.VersionEval).ToDataSourceResult(request));
            }
            catch (Exception ex)
            {
                this.log.PublishException(new SecureportException(ex));
                throw;
            }
        }


        #endregion

        #region Documentos

        public ActionResult AsociadosEdit(bool ToolBar)
        {
            this.UsuarioFrontalSession.valor = ToolBar;

            return PartialView("~/Views/Evaluaciones/Documentos/_PartialAsociadosEdit.cshtml", this.UsuarioFrontalSession);

        }

        public ActionResult AsociadosEditEvaluaciones(int Id)
        {
            return PartialView("~/Views/Evaluaciones/Documentos/_PartialAsociadosEdit.cshtml", this.UsuarioFrontalSession);
        }
        /// <summary>
        /// Permite visualizar los datos de un usuario con documentos.
        /// </summary>
        [HttpPost]
        public ActionResult AsignarDocumentos()
        {
            try
            {

                this.ComboTipos_Documento();

                EvaluacionesViewModel evaluacion = new EvaluacionesViewModel();

                this._serviciosEvaluaciones.GetAllEvaluaciones().Where(x => x.Id == this.idEvaluacion).ToList().ForEach(
                   p =>
                   {
                       evaluacion.Id = p.Id;
                       evaluacion.Nombre = p.Nombre;

                   });

                return this.PartialView("~/Views/Evaluaciones/_PartialAsignarDocumentos.cshtml", evaluacion);

            }
            catch (Exception ex)
            {
                this.log.PublishException(new SecureportException(ex));

                return this.Json(new { result = false, Message = ex.Message });
            }
        }


        public ActionResult DocumentosAsociados([DataSourceRequest]DataSourceRequest request)
        {
            try
            {

                IEnumerable<Doc_Asoc> result = (from t1 in this._serviciosDocumentos.GetDocs(this.idEvaluacion, 14)
                                                orderby t1.id
                                                select new Doc_Asoc
                                                {
                                                    id = t1.id,
                                                    TipoNombre = t1.TipoNombre,
                                                    documento = t1.documento.Substring((t1.documento.IndexOf("_")) + 1),
                                                    descripcion = t1.descripcion
                                                }).OrderBy(x => x.documento);

                return Json(result.ToDataSourceResult(request));
            }
            catch (Exception ex)
            {
                this.log.PublishException(new SecureportException(ex));

                return this.Json(new { result = false, Message = ex.Message });
            }
        }

        /// <summary>
        ///     Permite guardar cambios en un documento asociado.
        /// </summary>
        [HttpPost]
        public ActionResult GuardarDocumento(DocumentoJson DocumentoJson)
        {
            try
            {

                this.ComboTipos_Documento();

                Docs_Usuario docsUsuario = this.repoDocumentosUsuario.Single(x => x.id == DocumentoJson.id);

                docsUsuario.descripcion = DocumentoJson.descripcion;

                docsUsuario.id_tipo_doc = DocumentoJson.tipodocumento;

                this.repoDocumentosUsuario.Update(docsUsuario);

                this.trazas(275, (int)this.UsuarioFrontalSession.Usuario.id, "Se ha dado de modificado un registro en SPMOV_Documentos_SP " + this.UsuarioFrontalSession.Usuario.login);

                return this.Json(new { result = true });

            }
            catch (Exception ex)
            {
                this.log.PublishException(new SecureportException(ex));

                return this.Json(new { result = false, Message = ex.Message });
            }

        }

        /// <summary>
        ///     Permite eliminar un documento asociado.
        /// </summary>
        [HttpPost]
        public ActionResult visualizarDocumento(int? id)
        {
            try
            {
                Documento documento = new Documento();

                Docs_Usuario docsUsuario = this.repoDocumentosUsuario.Single(x => x.id == id);

                string doc = docsUsuario.documento;

                string nombreRuta = ConfigurationManager.AppSettings["Ficheros"];
                
                this.UsuarioFrontalSession.Path = nombreRuta + "/" + doc;

                return PartialView("~/Views/Shared/PartialDoc.cshtml", this.UsuarioFrontalSession);

            }
            catch (Exception ex)
            {
                this.log.PublishException(new SecureportException(ex));

                return this.Json(new { result = false, Message = ex.Message });
            }

        }

        /// <summary>
        ///     Permite eliminar un documento asociado.
        /// </summary>
        [HttpPost]
        public ActionResult EditarDocumento(int? id)
        {
            try
            {

                this.ComboTipos_Documento();

                Documento documento = new Documento();

                IEnumerable<EvaluacionesViewModel> evaluacion = this._serviciosEvaluaciones.GetAllEvaluaciones().Where(x => x.Id == this.idEvaluacion);

                EvaluacionesViewModel temp = evaluacion.FirstOrDefault();

                Docs_Usuario docsInstalacion = this.repoDocumentosUsuario.Single(x => x.id == id);

                documento.Nombre = temp.Nombre;

                documento.Apellidos = temp.Nombre;

                documento.descripcion = docsInstalacion.descripcion;

                documento.tipodocumento = docsInstalacion.id_tipo_doc;

                documento.documento = docsInstalacion.documento.Substring((docsInstalacion.documento.IndexOf("_")) + 1);

                return this.PartialView("~/Views/Evaluaciones/_PartialEditDocumentos.cshtml", documento);

            }
            catch (Exception ex)
            {
                this.log.PublishException(new SecureportException(ex));

                return this.Json(new { result = false, Message = ex.Message });
            }

        }

        /// <summary>
        ///     Permite eliminar un documento asociado.
        /// </summary>
        [HttpPost]
        public ActionResult EliminarDocumentos(int? id)
        {
            try
            {

                Docs_Usuario docsUsuario = this.repoDocumentosUsuario.Single(x => x.id == id);

                this.repoDocumentosUsuario.Delete(docsUsuario);

                this.trazas(274, (int)this.UsuarioFrontalSession.Usuario.id, "Se ha dado de borrado un registro en SPMOV_Documentos_SP " + this.UsuarioFrontalSession.Usuario.login);

                BorrarFichero(ConfigurationManager.AppSettings["RutaSecurePorDoc"].ToString() + "/" + docsUsuario.documento);


                return this.Json(new { result = true });

            }
            catch (Exception ex)
            {
                this.log.PublishException(new SecureportException(ex));

                return this.Json(new { result = false, Message = ex.Message });
            }

        }

        /// <summary>
        /// Permite visualizar los datos de un usuario ya registrado.
        /// </summary>
        [HttpPost]
        public void AsignarTipoDocumento(UploadJson uploadJson)
        {
            try
            {
                this.idTipoDocumento = uploadJson.tipo;

            }
            catch (Exception ex)
            {
                this.log.PublishException(new SecureportException(ex));
            }
        }

        /// <summary>
        /// Permite deshacer los cambios.
        /// </summary>
        [HttpPost]
        public ActionResult DeshacerCambios(int id)
        {
            try
            {

                this.ComboTipos_Documento();

                Documento documento = new Documento();

                IEnumerable<EvaluacionesViewModel> evaluacion = this._serviciosEvaluaciones.GetAllEvaluaciones().Where(x => x.Id == this.idEvaluacion);

                EvaluacionesViewModel temp = evaluacion.FirstOrDefault();

                Docs_Usuario docsFormacion = this.repoDocumentosUsuario.Single(x => x.id == id);

                documento.Nombre = temp.Nombre;

                documento.Apellidos = temp.Nombre;

                documento.descripcion = docsFormacion.descripcion;

                documento.tipodocumento = docsFormacion.id_tipo_doc;

                documento.documento = docsFormacion.documento.Substring((docsFormacion.documento.IndexOf("_")) + 1);


                return this.PartialView("~/Views/Evaluaciones/_PartialEditDocumentos.cshtml", documento);

            }
            catch (Exception ex)
            {
                this.log.PublishException(new SecureportException(ex));

                return this.Json(new { result = false, Message = ex.Message });
            }
        }

        [HttpPost]
        public ActionResult ValidarDocumento(UploadJson uploadJson)
        {
            string nombre = uploadJson.nombre_servicio + uploadJson.id_servicio.ToString() + "_" + uploadJson.nombre;

            IEnumerable<Doc_Asoc> result = this._serviciosDocumentos.GetDocs(this.idEvaluacion, 14).ToList().Where(x => x.documento == nombre);
            if (result.Count() > 0)
                return this.Json(new { result = 0 });
            else
                return this.Json(new { result = 1 });
        }


        #endregion

        #region Matriz de evaluacion
        
            /// <summary>
        /// Visualizar lista de Bienes/amenazas para asignar
        /// </summary>
        [HttpPost]
        public ActionResult GenerarMatriz(int? id, string accion)
        {
            try
            {
                EvaluacionesViewModel evaluacion = _serviciosEvaluaciones.GetAllEvaluaciones().Where(x => x.Id == (int)this.idEvaluacion).FirstOrDefault();
                ViewBag.IdInstalacion = evaluacion.Id_IIPP;
                ViewBag.Evaluacion = evaluacion.Nombre;
                ViewBag.Instalacion = evaluacion.IIPP;
                ViewBag.IdEvalua = evaluacion.Id;
                this.AccionEstado = accion;
                
                return this.PartialView("~/Views/Evaluaciones/ListaBienInstalacionAmenazas.cshtml", this.UsuarioFrontalSession);               
            }
            catch (Exception ex)
            {
                this.log.PublishException(new SecureportException(ex));

                return this.Json(new { result = false, Message = ex.Message });
            }
        }


        /// <summary>
        /// Permite buscar las instalaciones/Bienes
        /// </summary>
        public ActionResult BuscadorBienesByInstalacion([DataSourceRequest]DataSourceRequest request, int IdInstalacion)
        {
            try
            {
                this.idInstalacion = IdInstalacion;
                return this.Json(this._serviciosTipoInstalacion.GetAllBienesByInstalacion(IdInstalacion).OrderBy(z => z.Bien).ToDataSourceResult(request));
            }
            catch (Exception ex)
            {
                this.log.PublishException(new SecureportException(ex));
                throw;
            }
        }

        [HttpPost]
        public ActionResult BuscadorBienAmenazasInstalacion([DataSourceRequest]DataSourceRequest request)
        {
            try
            {
                return this.Json(this._serviciosTipoInstalacion.GetAllBienesByInstalacionBien((int)this.idInstalacion, (int)this.idBien).OrderBy(z => z.Descripcion).ToDataSourceResult(request));
            }
            catch (Exception ex)
            {
                this.log.PublishException(new SecureportException(ex));
                throw;
            }
        }


        
        public ActionResult BuscadorBienAmenazasInstalacion1([DataSourceRequest]DataSourceRequest request, int? BienId, int? InstalacionId, string nombreIns, string nombreBien)
        {
            try
            {
                this.idBien = BienId;
                if (this.idInstalacion  == null)
                    this.idInstalacion = InstalacionId;
                if (this.idInstalacion != InstalacionId)
                    this.idInstalacion = InstalacionId;
                BuscadorBienAmenazasInstalacion2();
                ViewBag.Instalacion = nombreIns;

                ViewBag.Bien = nombreBien;
                ViewBag.Mensaje = "Matriz de Evaluación";
                if (this.AccionEstado != General.EditGeneral.ToDescription())
                {
                    ViewBag.resulta = "display:block;";
                    ViewBag.viualiza = "true";
                }
                else
                {
                    ViewBag.viualiza = "false";
                    ViewBag.resulta = "display:none;";
                }

                if (this.UsuarioFrontalSession.ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Mov_Evaluaciones.RIESGOS_EVAL.ToDescription()).Count() > 0)
                {
                    if (ViewBag.viualiza == "false")
                        ViewBag.BtnCalculo = "display:block;";
                    else
                        ViewBag.BtnCalculo = "display:none;";
                }
                else
                    ViewBag.BtnCalculo = "display:none;";

                //var tipo = this._serviciosTipoInstalacion.ListInstalaciones(Convert.ToInt32(this.idInstalacion)).ToList();
                if (this._serviciosTipoInstalacion.ListInstalaciones(Convert.ToInt32(this.idInstalacion)).ToList().FirstOrDefault().Nombreclasificacion == BajoRiesgo.MessageBajoRiesgo.ToDescription())
                {
                   return this.PartialView("~/Views/Evaluaciones/_PartialAmenazasBienBajoRiesgo.cshtml", this.UsuarioFrontalSession);
                } 
                return this.PartialView("~/Views/Evaluaciones/_PartialAmenazasBienInstalacion.cshtml", this.UsuarioFrontalSession);
            }
            catch (Exception ex)
            {
                this.log.PublishException(new SecureportException(ex));
                throw;
            }
        }


        public void BuscadorBienAmenazasInstalacion2()
        {
            try
            {               
                ViewBag.AmenazasBien = this._serviciosTipoInstalacion.GetAllBienesByInstalacionBien((int)this.idInstalacion, (int)this.idBien).OrderBy(z => z.Descripcion);
                this.DetalleUpdate = this._serviciosEvaluaciones.GetAllBienesByInstalacionBienEvaluacionDetail((int)this.idInstalacion, (int)this.idBien, (int)this.idEvaluacion);
                ViewBag.DetalleAmenazaBien = this.DetalleUpdate;

                var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                string datos = serializer.Serialize(this.DetalleUpdate);
                
                ViewBag.DetalleAmenazaBienDatail = datos;               
            }
            catch (Exception ex)
            {
                this.log.PublishException(new SecureportException(ex));
                throw;
            }
        }

    
        [HttpPost]
        public ActionResult GuardarMatrizEvalua(List<Detalle_EvaluacionViewModel> detalle_EvaluacionViewModel)
        {
            try
            {
                foreach (Detalle_EvaluacionViewModel Detalle in detalle_EvaluacionViewModel)
                {                  
                       if (Detalle.tipo == "N")
                            AltaDetalleEvaluacion(Detalle);
                        else
                           if (Detalle.Estado == "M")
                                ModificarDetalleEvaluacion(Detalle);           
                
                } 


                 return this.Json(new { result = 0 });
             }
            catch (Exception ex)
            {
                this.log.PublishException(new SecureportException(ex));
                throw;
            }

        
        }

        [HttpPost]
        public ActionResult GuardarMatrizEvaluaBajoRiesgo(List<Detalle_EvaluacionViewModel> detalle_EvaluacionViewModelBajoRiesgo)
        {
            try
            {
                foreach (Detalle_EvaluacionViewModel Detalle in detalle_EvaluacionViewModelBajoRiesgo)
                {
                    if (Detalle.tipo == "N")
                        AltaDetalleEvaluacionBajoRiesgo(Detalle);
                    else
                        if (Detalle.Estado == "M")
                            ModificarDetalleEvaluacionBajoRiesgo(Detalle);

                }


                return this.Json(new { result = 0 });
            }
            catch (Exception ex)
            {
                this.log.PublishException(new SecureportException(ex));
                throw;
            }


        }

        [HttpPost]
        public ActionResult AltaDetalleEvaluacionBajoRiesgo(Detalle_EvaluacionViewModel detalle_EvaluacionViewModel)
        {
            try
            {

                Detalle_Evaluacion EvaluaDetalle = new Detalle_Evaluacion();
                EvaluaDetalle.Id_Evaluacion = detalle_EvaluacionViewModel.Id_Evaluacion;
                EvaluaDetalle.Id_Version_Eval = detalle_EvaluacionViewModel.Id_Version_Eval;
                EvaluaDetalle.Id_Instalacion = detalle_EvaluacionViewModel.Id_Instalacion;
                EvaluaDetalle.Id_Bien = detalle_EvaluacionViewModel.Id_Bien;
                EvaluaDetalle.Id_Amenaza = detalle_EvaluacionViewModel.Id_Amenaza;
                EvaluaDetalle.NP1_RESULTADO = detalle_EvaluacionViewModel.NP1_ID * detalle_EvaluacionViewModel.NP1_IC * detalle_EvaluacionViewModel.NP1_IV; 
                EvaluaDetalle.NP1_ID = detalle_EvaluacionViewModel.NP1_ID;
                EvaluaDetalle.NP1_IC = detalle_EvaluacionViewModel.NP1_IC;
                EvaluaDetalle.NP1_IV = detalle_EvaluacionViewModel.NP1_IV;
                EvaluaDetalle.NP2_RESULTADO = detalle_EvaluacionViewModel.NP2_ID * detalle_EvaluacionViewModel.NP2_IC * detalle_EvaluacionViewModel.NP2_IV;
                EvaluaDetalle.NP2_ID = detalle_EvaluacionViewModel.NP2_ID;
                EvaluaDetalle.NP2_IC = detalle_EvaluacionViewModel.NP2_IC;
                EvaluaDetalle.NP2_IV = detalle_EvaluacionViewModel.NP2_IV;
                EvaluaDetalle.NP3_RESULTADO = detalle_EvaluacionViewModel.NP3_ID * detalle_EvaluacionViewModel.NP3_IC * detalle_EvaluacionViewModel.NP3_IV;
                EvaluaDetalle.NP3_ID = detalle_EvaluacionViewModel.NP3_ID;
                EvaluaDetalle.NP3_IC = detalle_EvaluacionViewModel.NP3_IC;
                EvaluaDetalle.NP3_IV = detalle_EvaluacionViewModel.NP3_IV;
                EvaluaDetalle.ID_Usu_alta = this.UsuarioFrontalSession.Usuario.id;
                EvaluaDetalle.Fech_Alta = DateTime.Now;
                this.repoDetalle_Evaluacion.Create(EvaluaDetalle);
                EvaluaDetalle.Id = this.db.Detalle_Evaluacion.OrderByDescending(u => u.Id).FirstOrDefault().Id;
                this.trazas(232, (int)this.UsuarioFrontalSession.Usuario.id, "Se ha dado de alta un registro en SPEVA_Detalle_Evaluacion con ID" + "(" + EvaluaDetalle.Id.ToString() + ")");
                return this.Json(new { result = 1 });

            }
            catch (ModelValidationException ex)
            {
                return this.Json(new { result = false, Message = ex.Message });
            }
            catch (DbEntityValidationException ex)
            {
                this.log.PublishException(new SecureportExceptionDbEntity(ex));

                return this.Json(new { result = false, Message = ex.Message });
            }
            catch (Exception ex)
            {
                this.log.PublishException(new SecureportException(ex));

                return Json(new { status = "error", Message = ex.Message, result = 2 });
            }


        }

        [HttpPost]
        public ActionResult ModificarDetalleEvaluacionBajoRiesgo(Detalle_EvaluacionViewModel detalle_EvaluacionViewModel)
        {
            try
            {

                Detalle_Evaluacion EvaluaDetalle = repoDetalle_Evaluacion.Single(x => x.Id == detalle_EvaluacionViewModel.Id);
                EvaluaDetalle.Id_Evaluacion = detalle_EvaluacionViewModel.Id_Evaluacion;
                EvaluaDetalle.Id_Version_Eval = detalle_EvaluacionViewModel.Id_Version_Eval;
                EvaluaDetalle.Id_Instalacion = detalle_EvaluacionViewModel.Id_Instalacion;
                EvaluaDetalle.Id_Bien = detalle_EvaluacionViewModel.Id_Bien;
                EvaluaDetalle.Id_Amenaza = detalle_EvaluacionViewModel.Id_Amenaza;
                EvaluaDetalle.NP1_RESULTADO = detalle_EvaluacionViewModel.NP1_ID * detalle_EvaluacionViewModel.NP1_IC * detalle_EvaluacionViewModel.NP1_IV;
                EvaluaDetalle.NP1_ID = detalle_EvaluacionViewModel.NP1_ID;
                EvaluaDetalle.NP1_IC = detalle_EvaluacionViewModel.NP1_IC;
                EvaluaDetalle.NP1_IV = detalle_EvaluacionViewModel.NP1_IV;
                EvaluaDetalle.NP2_RESULTADO = detalle_EvaluacionViewModel.NP2_ID * detalle_EvaluacionViewModel.NP2_IC * detalle_EvaluacionViewModel.NP2_IV;
                EvaluaDetalle.NP2_ID = detalle_EvaluacionViewModel.NP2_ID;
                EvaluaDetalle.NP2_IC = detalle_EvaluacionViewModel.NP2_IC;
                EvaluaDetalle.NP2_IV = detalle_EvaluacionViewModel.NP2_IV;
                EvaluaDetalle.NP3_RESULTADO = detalle_EvaluacionViewModel.NP3_ID * detalle_EvaluacionViewModel.NP3_IC * detalle_EvaluacionViewModel.NP3_IV;
                EvaluaDetalle.NP3_ID = detalle_EvaluacionViewModel.NP3_ID;
                EvaluaDetalle.NP3_IC = detalle_EvaluacionViewModel.NP3_IC;
                EvaluaDetalle.NP3_IV = detalle_EvaluacionViewModel.NP3_IV;
                this.repoDetalle_Evaluacion.Update(EvaluaDetalle);
                this.trazas(232, (int)this.UsuarioFrontalSession.Usuario.id, "Se ha modificado un registro en SPEVA_Detalle_Evaluacion con ID" + "(" + EvaluaDetalle.Id.ToString() + ")");
                return this.Json(new { result = 1 });

            }
            catch (ModelValidationException ex)
            {
                return this.Json(new { result = false, Message = ex.Message });
            }
            catch (DbEntityValidationException ex)
            {
                this.log.PublishException(new SecureportExceptionDbEntity(ex));

                return this.Json(new { result = false, Message = ex.Message });
            }
            catch (Exception ex)
            {
                this.log.PublishException(new SecureportException(ex));

                return Json(new { status = "error", Message = ex.Message, result = 2 });
            }


        }


         [HttpPost]
         public ActionResult AltaDetalleEvaluacion(Detalle_EvaluacionViewModel detalle_EvaluacionViewModel)
         {
             try
             {

                 Detalle_Evaluacion EvaluaDetalle = new Detalle_Evaluacion();
                 EvaluaDetalle.Id_Evaluacion = detalle_EvaluacionViewModel.Id_Evaluacion;
                 EvaluaDetalle.Id_Version_Eval = detalle_EvaluacionViewModel.Id_Version_Eval;
                 EvaluaDetalle.Id_Instalacion = detalle_EvaluacionViewModel.Id_Instalacion;
                 EvaluaDetalle.Id_Bien = detalle_EvaluacionViewModel.Id_Bien;
                 EvaluaDetalle.Id_Amenaza = detalle_EvaluacionViewModel.Id_Amenaza;
                 EvaluaDetalle.IND_IAS_ITP = detalle_EvaluacionViewModel.IND_IAS_ITP;
                 EvaluaDetalle.NP1_IAI = detalle_EvaluacionViewModel.NP1_IAI;
                 EvaluaDetalle.NP1_ISD = detalle_EvaluacionViewModel.NP1_ISD;
                 EvaluaDetalle.NP1_IIO = detalle_EvaluacionViewModel.NP1_IIO;
                 EvaluaDetalle.NP1_IDV = detalle_EvaluacionViewModel.NP1_IDV;
                 EvaluaDetalle.NP1_IDE_D = detalle_EvaluacionViewModel.NP1_IDE_D;
                 EvaluaDetalle.NP1_IDE_I = detalle_EvaluacionViewModel.NP1_IDE_I;
                 EvaluaDetalle.NP1_IRD = detalle_EvaluacionViewModel.NP1_IRD;
                 EvaluaDetalle.NP1_IRR = detalle_EvaluacionViewModel.NP1_IRR;
                 EvaluaDetalle.NP1_ISA_MA = detalle_EvaluacionViewModel.NP1_ISA_MA;
                 EvaluaDetalle.NP1_ISA_PA = detalle_EvaluacionViewModel.NP1_ISA_PA;
                 EvaluaDetalle.NP1_ISA_AS = detalle_EvaluacionViewModel.NP1_ISA_AS;
                 EvaluaDetalle.NP1_RESULTADO = CalcularRisgo(detalle_EvaluacionViewModel,"RP1"); //detalle_EvaluacionViewModel.NP1_RESULTADO;
                 EvaluaDetalle.NP1_ID = detalle_EvaluacionViewModel.NP1_ID;
                 EvaluaDetalle.NP1_IC = detalle_EvaluacionViewModel.NP1_IC;
                 EvaluaDetalle.NP1_IV = detalle_EvaluacionViewModel.NP1_IV;
                 EvaluaDetalle.NP2_IAI = detalle_EvaluacionViewModel.NP2_IAI;
                 EvaluaDetalle.NP2_ISD = detalle_EvaluacionViewModel.NP2_ISD;
                 EvaluaDetalle.NP2_IIO = detalle_EvaluacionViewModel.NP2_IIO;
                 EvaluaDetalle.NP2_IDV = detalle_EvaluacionViewModel.NP2_IDV;
                 EvaluaDetalle.NP2_IDE_D = detalle_EvaluacionViewModel.NP2_IDE_D;
                 EvaluaDetalle.NP2_IDE_I = detalle_EvaluacionViewModel.NP2_IDE_I;
                 EvaluaDetalle.NP2_IRD = detalle_EvaluacionViewModel.NP2_IRD;
                 EvaluaDetalle.NP2_IRR = detalle_EvaluacionViewModel.NP2_IRR;
                 EvaluaDetalle.NP2_ISA_MA = detalle_EvaluacionViewModel.NP2_ISA_MA;
                 EvaluaDetalle.NP2_ISA_PA = detalle_EvaluacionViewModel.NP2_ISA_PA;
                 EvaluaDetalle.NP2_ISA_AS = detalle_EvaluacionViewModel.NP2_ISA_AS;
                 EvaluaDetalle.NP2_RESULTADO = CalcularRisgo(detalle_EvaluacionViewModel, "RP2"); 
                 EvaluaDetalle.NP2_ID = detalle_EvaluacionViewModel.NP2_ID;
                 EvaluaDetalle.NP2_IC = detalle_EvaluacionViewModel.NP2_IC;
                 EvaluaDetalle.NP2_IV = detalle_EvaluacionViewModel.NP2_IV;
                 EvaluaDetalle.NP3_IAI = detalle_EvaluacionViewModel.NP3_IAI;
                 EvaluaDetalle.NP3_ISD = detalle_EvaluacionViewModel.NP3_ISD;
                 EvaluaDetalle.NP3_IIO = detalle_EvaluacionViewModel.NP3_IIO;
                 EvaluaDetalle.NP3_IDV = detalle_EvaluacionViewModel.NP3_IDV;
                 EvaluaDetalle.NP3_IDE_D = detalle_EvaluacionViewModel.NP3_IDE_D;
                 EvaluaDetalle.NP3_IDE_I = detalle_EvaluacionViewModel.NP3_IDE_I;
                 EvaluaDetalle.NP3_IRD = detalle_EvaluacionViewModel.NP3_IRD;
                 EvaluaDetalle.NP3_IRR = detalle_EvaluacionViewModel.NP3_IRR;
                 EvaluaDetalle.NP3_ISA_MA = detalle_EvaluacionViewModel.NP3_ISA_MA;
                 EvaluaDetalle.NP3_ISA_PA = detalle_EvaluacionViewModel.NP3_ISA_PA;
                 EvaluaDetalle.NP3_ISA_AS = detalle_EvaluacionViewModel.NP3_ISA_AS;
                 EvaluaDetalle.NP3_RESULTADO = CalcularRisgo(detalle_EvaluacionViewModel, "RP3");
                 EvaluaDetalle.NP3_ID = detalle_EvaluacionViewModel.NP3_ID;
                 EvaluaDetalle.NP3_IC = detalle_EvaluacionViewModel.NP3_IC;
                 EvaluaDetalle.NP3_IV = detalle_EvaluacionViewModel.NP3_IV;
                 EvaluaDetalle.ID_Usu_alta = this.UsuarioFrontalSession.Usuario.id;
                 EvaluaDetalle.Fech_Alta = DateTime.Now;

                 this.repoDetalle_Evaluacion.Create(EvaluaDetalle);

                 EvaluaDetalle.Id = this.db.Detalle_Evaluacion.OrderByDescending(u => u.Id).FirstOrDefault().Id;

                 this.trazas(232, (int)this.UsuarioFrontalSession.Usuario.id, "Se ha dado de alta un registro en SPEVA_Detalle_Evaluacion con ID" + "(" + EvaluaDetalle.Id.ToString() + ")");


                 return this.Json(new { result = 1 });

             }
             catch (ModelValidationException ex)
             {
                 return this.Json(new { result = false, Message = ex.Message });
             }
             catch (DbEntityValidationException ex)
             {
                 this.log.PublishException(new SecureportExceptionDbEntity(ex));

                 return this.Json(new { result = false, Message = ex.Message });
             }
             catch (Exception ex)
             {
                 this.log.PublishException(new SecureportException(ex));

                 return Json(new { status = "error", Message = ex.Message, result = 2 });
             }


         }


         [HttpPost]
         public ActionResult ModificarDetalleEvaluacion(Detalle_EvaluacionViewModel detalle_EvaluacionViewModel)
         {
             try
             {

                 Detalle_Evaluacion EvaluaDetalle = repoDetalle_Evaluacion.Single(x=> x.Id == detalle_EvaluacionViewModel.Id);
                 EvaluaDetalle.Id_Evaluacion = detalle_EvaluacionViewModel.Id_Evaluacion;
                 EvaluaDetalle.Id_Version_Eval = detalle_EvaluacionViewModel.Id_Version_Eval;
                 EvaluaDetalle.Id_Instalacion = detalle_EvaluacionViewModel.Id_Instalacion;
                 EvaluaDetalle.Id_Bien = detalle_EvaluacionViewModel.Id_Bien;
                 EvaluaDetalle.Id_Amenaza = detalle_EvaluacionViewModel.Id_Amenaza;
                 EvaluaDetalle.IND_IAS_ITP = detalle_EvaluacionViewModel.IND_IAS_ITP;
                 EvaluaDetalle.NP1_IAI = detalle_EvaluacionViewModel.NP1_IAI;
                 EvaluaDetalle.NP1_ISD = detalle_EvaluacionViewModel.NP1_ISD;
                 EvaluaDetalle.NP1_IIO = detalle_EvaluacionViewModel.NP1_IIO;
                 EvaluaDetalle.NP1_IDV = detalle_EvaluacionViewModel.NP1_IDV;
                 EvaluaDetalle.NP1_IDE_D = detalle_EvaluacionViewModel.NP1_IDE_D;
                 EvaluaDetalle.NP1_IDE_I = detalle_EvaluacionViewModel.NP1_IDE_I;
                 EvaluaDetalle.NP1_IRD = detalle_EvaluacionViewModel.NP1_IRD;
                 EvaluaDetalle.NP1_IRR = detalle_EvaluacionViewModel.NP1_IRR;
                 EvaluaDetalle.NP1_ISA_MA = detalle_EvaluacionViewModel.NP1_ISA_MA;
                 EvaluaDetalle.NP1_ISA_PA = detalle_EvaluacionViewModel.NP1_ISA_PA;
                 EvaluaDetalle.NP1_ISA_AS = detalle_EvaluacionViewModel.NP1_ISA_AS;
                 EvaluaDetalle.NP1_RESULTADO = CalcularRisgo(detalle_EvaluacionViewModel, "RP1");  //detalle_EvaluacionViewModel.NP1_RESULTADO;
                 EvaluaDetalle.NP1_ID = detalle_EvaluacionViewModel.NP1_ID;
                 EvaluaDetalle.NP1_IC = detalle_EvaluacionViewModel.NP1_IC;
                 EvaluaDetalle.NP1_IV = detalle_EvaluacionViewModel.NP1_IV;
                 EvaluaDetalle.NP2_IAI = detalle_EvaluacionViewModel.NP2_IAI;
                 EvaluaDetalle.NP2_ISD = detalle_EvaluacionViewModel.NP2_ISD;
                 EvaluaDetalle.NP2_IIO = detalle_EvaluacionViewModel.NP2_IIO;
                 EvaluaDetalle.NP2_IDV = detalle_EvaluacionViewModel.NP2_IDV;
                 EvaluaDetalle.NP2_IDE_D = detalle_EvaluacionViewModel.NP2_IDE_D;
                 EvaluaDetalle.NP2_IDE_I = detalle_EvaluacionViewModel.NP2_IDE_I;
                 EvaluaDetalle.NP2_IRD = detalle_EvaluacionViewModel.NP2_IRD;
                 EvaluaDetalle.NP2_IRR = detalle_EvaluacionViewModel.NP2_IRR;
                 EvaluaDetalle.NP2_ISA_MA = detalle_EvaluacionViewModel.NP2_ISA_MA;
                 EvaluaDetalle.NP2_ISA_PA = detalle_EvaluacionViewModel.NP2_ISA_PA;
                 EvaluaDetalle.NP2_ISA_AS = detalle_EvaluacionViewModel.NP2_ISA_AS;
                 EvaluaDetalle.NP2_RESULTADO = CalcularRisgo(detalle_EvaluacionViewModel, "RP2"); //detalle_EvaluacionViewModel.NP2_RESULTADO;
                 EvaluaDetalle.NP2_ID = detalle_EvaluacionViewModel.NP2_ID;
                 EvaluaDetalle.NP2_IC = detalle_EvaluacionViewModel.NP2_IC;
                 EvaluaDetalle.NP2_IV = detalle_EvaluacionViewModel.NP2_IV;
                 EvaluaDetalle.NP3_IAI = detalle_EvaluacionViewModel.NP3_IAI;
                 EvaluaDetalle.NP3_ISD = detalle_EvaluacionViewModel.NP3_ISD;
                 EvaluaDetalle.NP3_IIO = detalle_EvaluacionViewModel.NP3_IIO;
                 EvaluaDetalle.NP3_IDV = detalle_EvaluacionViewModel.NP3_IDV;
                 EvaluaDetalle.NP3_IDE_D = detalle_EvaluacionViewModel.NP3_IDE_D;
                 EvaluaDetalle.NP3_IDE_I = detalle_EvaluacionViewModel.NP3_IDE_I;
                 EvaluaDetalle.NP3_IRD = detalle_EvaluacionViewModel.NP3_IRD;
                 EvaluaDetalle.NP3_IRR = detalle_EvaluacionViewModel.NP3_IRR;
                 EvaluaDetalle.NP3_ISA_MA = detalle_EvaluacionViewModel.NP3_ISA_MA;
                 EvaluaDetalle.NP3_ISA_PA = detalle_EvaluacionViewModel.NP3_ISA_PA;
                 EvaluaDetalle.NP3_ISA_AS = detalle_EvaluacionViewModel.NP3_ISA_AS;
                 EvaluaDetalle.NP3_RESULTADO = CalcularRisgo(detalle_EvaluacionViewModel, "RP3"); //detalle_EvaluacionViewModel.NP3_RESULTADO;
                 EvaluaDetalle.NP3_ID = detalle_EvaluacionViewModel.NP3_ID;
                 EvaluaDetalle.NP3_IC = detalle_EvaluacionViewModel.NP3_IC;
                 EvaluaDetalle.NP3_IV = detalle_EvaluacionViewModel.NP3_IV;                 

                 this.repoDetalle_Evaluacion.Update(EvaluaDetalle);

                 this.trazas(232, (int)this.UsuarioFrontalSession.Usuario.id, "Se ha modificado un registro en SPEVA_Detalle_Evaluacion con ID" + "(" + EvaluaDetalle.Id.ToString() + ")");


                 return this.Json(new { result = 1 });

             }
             catch (ModelValidationException ex)
             {
                 return this.Json(new { result = false, Message = ex.Message });
             }
             catch (DbEntityValidationException ex)
             {
                 this.log.PublishException(new SecureportExceptionDbEntity(ex));

                 return this.Json(new { result = false, Message = ex.Message });
             }
             catch (Exception ex)
             {
                 this.log.PublishException(new SecureportException(ex));

                 return Json(new { status = "error", Message = ex.Message, result = 2 });
             }


         }


         public int CalcularRisgo(Detalle_EvaluacionViewModel detalleView, string calculo)
         {
             int valorCalculado=0;
             switch (calculo)
             { 
                 case "RP1":
                     valorCalculado = CalculoRiesgos(detalleView.IND_IAS_ITP == true ? 2 : 1, (int)detalleView.NP1_IAI,
                            (int)detalleView.NP1_ISD,
                            (int)detalleView.NP1_IIO,
                            (int)detalleView.NP1_IDV,
                            (int)detalleView.NP1_IDE_D,
                            (int)detalleView.NP1_IDE_I,
                            (int)detalleView.NP1_IRD,
                            (int)detalleView.NP1_IRR,
                            (int)detalleView.NP1_ISA_MA,
                            (int)detalleView.NP1_ISA_PA,
                            (int)detalleView.NP1_ISA_AS);
                     break;
                 case "RP2":
                     valorCalculado = CalculoRiesgos(detalleView.IND_IAS_ITP == true ? 3 : 2, (int)detalleView.NP2_IAI,
                            (int)detalleView.NP2_ISD,
                            (int)detalleView.NP2_IIO,
                            (int)detalleView.NP2_IDV,
                            (int)detalleView.NP2_IDE_D,
                            (int)detalleView.NP2_IDE_I,
                            (int)detalleView.NP2_IRD,
                            (int)detalleView.NP2_IRR,
                            (int)detalleView.NP2_ISA_MA,
                            (int)detalleView.NP2_ISA_PA,
                            (int)detalleView.NP2_ISA_AS);
                     break;
                     case "RP3":
                     valorCalculado = CalculoRiesgos(detalleView.IND_IAS_ITP == true ? 3 : 3, (int)detalleView.NP3_IAI,
                            (int)detalleView.NP3_ISD,
                            (int)detalleView.NP3_IIO,
                            (int)detalleView.NP3_IDV,
                            (int)detalleView.NP3_IDE_D,
                            (int)detalleView.NP3_IDE_I,
                            (int)detalleView.NP3_IRD,
                            (int)detalleView.NP3_IRR,
                            (int)detalleView.NP3_ISA_MA,
                            (int)detalleView.NP3_ISA_PA,
                            (int)detalleView.NP3_ISA_AS);
                     break;
             }


             return valorCalculado;

         }

         [HttpPost]
         public ActionResult CalcularBajoRiesgo(List<Detalle_EvaluacionViewModel> detalle_EvaluacionViewModel)
         {
             try
             {
                 foreach (Detalle_EvaluacionViewModel Detalle in detalle_EvaluacionViewModel)
                 {
                     Detalle.NP1_RESULTADO = Detalle.NP1_ID * Detalle.NP1_IC * Detalle.NP1_IV;
                     Detalle.NP2_RESULTADO = Detalle.NP2_ID * Detalle.NP2_IC * Detalle.NP2_IV;
                     Detalle.NP3_RESULTADO = Detalle.NP3_ID * Detalle.NP3_IC * Detalle.NP3_IV;
                 }

                 return this.Json(new { detalle_EvaluacionViewModel });
             }
             catch (Exception ex)
             {
                 this.log.PublishException(new SecureportException(ex));
                 throw;
             }


         }

         [HttpPost]
         public ActionResult CalcularRisgosPanel(List<Detalle_EvaluacionViewModel> detalle_EvaluacionViewModel)
         {
             try
             {
                 foreach (Detalle_EvaluacionViewModel Detalle in detalle_EvaluacionViewModel)
                 {
                    Detalle.NP1_RESULTADO = CalcularRisgo(Detalle, "RP1");
                    Detalle.NP2_RESULTADO = CalcularRisgo(Detalle, "RP2");
                    Detalle.NP3_RESULTADO = CalcularRisgo(Detalle, "RP3");
                 }

                 return this.Json(new { detalle_EvaluacionViewModel });
             }
             catch (Exception ex)
             {
                 this.log.PublishException(new SecureportException(ex));
                 throw;
             }


         }


         public ActionResult BuscadorBienAmenazasInstalacionDeshacer([DataSourceRequest]DataSourceRequest request)
        {
            try
            {               
                MOV_Detalle_InstalacionViewModel evalua = this._serviciosTipoInstalacion.GetAllBienesByInstalacion((int)this.idInstalacion).Where(x=> x.Id_Bien == (int)this.idBien).First();
                BuscadorBienAmenazasInstalacion2();
                ViewBag.Instalacion = evalua.IIPP;
                ViewBag.Bien = evalua.Bien;
                ViewBag.viualiza = "false";
                ViewBag.Mensaje = "Matriz de Evaluación";
                ViewBag.resulta = "display:none;";
                if (this._serviciosTipoInstalacion.ListInstalaciones(Convert.ToInt32(this.idInstalacion)).ToList().FirstOrDefault().Nombreclasificacion == BajoRiesgo.MessageBajoRiesgo.ToDescription())
                {
                    return this.PartialView("~/Views/Evaluaciones/_PartialAmenazasBienBajoRiesgo.cshtml", this.UsuarioFrontalSession);
                }
                return this.PartialView("~/Views/Evaluaciones/_PartialAmenazasBienInstalacion.cshtml", this.UsuarioFrontalSession);
            }
            catch (Exception ex)
            {
                this.log.PublishException(new SecureportException(ex));
                throw;
            }
        }

       
        #endregion

  
    }
} 