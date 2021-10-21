namespace SecurePort.Web.Controllers
{
    #region Using
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.ModelConfiguration;
    using System.Data.Entity.Validation;
    using System.Linq;
    using System.Web.Mvc;

    using DocumentFormat.OpenXml.Spreadsheet;

    using Kendo.Mvc.Extensions;
    using Kendo.Mvc.UI;
    using SecurePort.Entities.Enums;
    using SecurePort.Entities.Exceptions;
    using SecurePort.Entities.Models;
    using SecurePort.Entities.Models.Json;
    using SecurePort.Services.Interfaces;
    using SecurePort.Services.Repository;
    using System.Configuration;
    #endregion

    public class AuditoriasController : BaseController
    {

        public AuditoriasController(IServiciosAuditorias serviciosAuditorias,
                                     InstalacionesRepository repoInstalaciones,
                                     PuertosRepository repoPuertos,
                                     AuditoriasRepository repoAuditorias,
                                     IServiciosTipoInstalacion serviciosInstalaciones,
                                     IServiciosPuertos servicioPuertos,
                                     DocumentosUsuarioRepository repoDocumentosUsuario,
                                     IServiciosUsuarios servicioUsuarios,
                                     IServiciosDocumentos servicioDocumentos,
                                     TrazasRepository repotrazas,
                                     ILogger log)

            : base(serviciosAuditorias,
                   repoInstalaciones,
                   repoPuertos,
                   repoAuditorias,
                   serviciosInstalaciones,
                   servicioPuertos,
                   repoDocumentosUsuario,
                   servicioUsuarios,
                   servicioDocumentos,
                   repotrazas,
                   log)
        {

        }

        #region action
        /// <summary>
        /// Retorna una lista de todas las auditorias.
        /// </summary>
        public ActionResult ListadoAuditorias()
        {
            try
            {
                ViewBag.UrlDelete = "/Auditorias/EliminarAuditorias";
                
                ViewBag.Mensaje = "Auditorías";

                return this.PartialView("ListadoAuditorias", this.UsuarioFrontalSession);
            }
            catch (Exception ex)
            {
                this.log.PublishException(new SecureportException(ex));
                throw;
            }
        }
        #endregion

        #region Buscador
        /// <summary>
        /// Visualizar las Auditorias.
        /// </summary>
        public ActionResult BuscadorAuditorias([DataSourceRequest]DataSourceRequest request)
        {
            try
            {
                if (this.UsuarioFrontalSession.Usuario.categoria == (int)EnumCategoria.Administrador
                    || this.UsuarioFrontalSession.Usuario.categoria == (int)EnumCategoria.PuertosdelEstado)
                {
                    return this.Json(this._serviciosAuditorias.ListAuditorias().ToDataSourceResult(request));
                }
                var categoria = this.UsuarioFrontalSession.Usuario.categoria;
                return this.Json(this._serviciosAuditorias.ListAuditorias(this.UsuarioFrontalSession.ListDependenciasDepenUsuarios,categoria).ToDataSourceResult(request));
            }
            catch (Exception ex)
            {
                this.log.PublishException(new SecureportException(ex));
                throw;
            }
        }
        #endregion

        #region CRUD

        #region Auditorias
        /// <summary>
        /// Crear una Prácticas o Ejercicios.
        /// </summary>
        [HttpPost]
        public ActionResult Create()
        {

            AuditoriasViewModel auditorias = new AuditoriasViewModel();

            this.comboPuertos(null);

            if (((MultiSelectList)(this.ViewData["puertos"])).Count() == 1)
                auditorias.Id_Puerto = Convert.ToInt32(((SelectList)this.ViewData["puertos"]).FirstOrDefault().Value);

            this.comboInstalacionesPuertos(null, null);

            ViewBag.disabled = true;

            ViewBag.Mensaje = "Alta de Auditorias";

            ViewBag.disabled = true;

            ViewBag.alta = "display:none";

            ViewBag.action = "Alta";

            ViewBag.ToolAuditoria = false;

            ViewBag.Combo = "display:block";

            ViewBag.Texto = "display:none";

            this.ViewData["EstadoFechaPromEne"] = "1";
            this.ViewData["EstadoFechaPromCol"] = "#FFFFFF";
            this.ViewData["EstadoFechaPlanEne"] = "1";
            this.ViewData["EstadoFechaPlanCol"] = "#FFFFFF";
            this.ViewData["EstadoFechaRealEne"] = "1";
            this.ViewData["EstadoFechaRealCol"] = "#FFFFFF";

            return this.PartialView("~/Views/Auditorias/Create.cshtml", auditorias);
        }

        /// <summary>
        /// Alta-Editar Prácticas o Ejercicios.
        /// </summary>
        [HttpPost]
        public ActionResult AltaEditarAuditorias(AuditoriasJson auditoriasJson)
        {
            try
            {
                int resultado = 0;

                switch (auditoriasJson.action)
                {
                    case "Alta":
                        AltaAuditorias(auditoriasJson);
                        resultado = 1;
                        break;

                    default:
                        GuardarAuditorias(auditoriasJson);
                        resultado = 1;
                        break;
                }

                return this.Json(new { result = resultado });
            }
            catch (DbUpdateException ex)
            {
                this.log.PublishException(new SecureportExceptionDbUpdate(ex));

                return this.Json(new { result = 0, Message = ex.Message });
            }
            catch (ModelValidationException ex)
            {
                return this.Json(new { result = 0, Message = ex.Message });
            }
            catch (DbEntityValidationException ex)
            {
                this.log.PublishException(new SecureportExceptionDbEntity(ex));

                return this.Json(new { result = 0, Message = ex.Message });
            }
            catch (Exception ex)
            {
                this.log.PublishException(new SecureportException(ex));

                return this.Json(new { result = 0, Message = ex.Message });
            }

        }

        /// <summary>
        /// Alta de Auditorias.
        /// </summary>
        [HttpPost]
        public ActionResult AltaAuditorias(AuditoriasJson auditoriasJson)
        {
            try
            {
                Auditorias auditorias = new Auditorias();

                auditorias.Id_Puerto = auditoriasJson.Id_Puerto;

                auditorias.Id_IIPP = auditoriasJson.Id_IIPP;
                
                auditorias.Descripcion = CambiarTexto(auditoriasJson.Descripcion);

                auditorias.Tipo = auditoriasJson.Tipo;

                auditorias.Estado = auditoriasJson.Estado;

                auditorias.Ini_Programa = auditoriasJson.Ini_Programa;

                auditorias.Fin_Programa = auditoriasJson.Fin_Programa;

                auditorias.Ini_Planifica = auditoriasJson.Ini_Planifica;

                auditorias.Fin_Planifica = auditoriasJson.Fin_Planifica;
                
                auditorias.Ini_Real = auditoriasJson.Ini_Real;

                auditorias.Fin_Real = auditoriasJson.Fin_Real;

                auditorias.Responsable = CambiarTexto(auditoriasJson.Responsable);

                auditorias.Conclusiones = CambiarTexto(auditoriasJson.Conclusiones);

                auditorias.Recomendaciones = CambiarTexto(auditoriasJson.Recomendaciones);

                auditorias.Programa = CambiarTexto(auditoriasJson.Programa);

                auditorias.Seguimiento = CambiarTexto(auditoriasJson.Seguimiento);

                auditorias.Observaciones = CambiarTexto(auditoriasJson.Observaciones);

                auditorias.ID_Usu_alta = this.UsuarioFrontalSession.Usuario.id;

                auditorias.Fech_Alta = DateTime.Now;

                this.repoAuditorias.Create(auditorias);

                auditoriasJson.Id = this.db.Auditorias.OrderByDescending(u => u.Id).FirstOrDefault().Id;

                this.IdAuditoria = auditoriasJson.Id;

                 this.trazas(136, (int)this.UsuarioFrontalSession.Usuario.id, "Se ha dado de alta un registro en SPMOV_Auditorias con ID" + "(" + auditoriasJson.Id.ToString() + ")");

                return this.Json(new { result = 0 });
            }

            catch (DbUpdateException ex)
            {
                this.log.PublishException(new SecureportExceptionDbUpdate(ex));

                return this.Json(new { result = 2, Message = ex.Message });
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

                return this.Json(new { result = 2, Message = ex.Message });
            }
        }

        /// <summary>
        /// Guardar Auditorias.
        /// </summary>
        [HttpPost]
        public ActionResult GuardarAuditorias(AuditoriasJson auditoriasJson)
        {
            try
            {
                Auditorias auditorias = repoAuditorias.Single(x => x.Id == auditoriasJson.Id);

                auditorias.Id_Puerto = auditoriasJson.Id_Puerto;

                auditorias.Id_IIPP = auditoriasJson.Id_IIPP;

                auditorias.Descripcion = CambiarTexto(auditoriasJson.Descripcion);

                auditorias.Tipo = auditoriasJson.Tipo;

                auditorias.Estado = auditoriasJson.Estado;
                
                auditorias.Ini_Programa = auditoriasJson.Ini_Programa;

                auditorias.Fin_Programa = auditoriasJson.Fin_Programa;
                
                auditorias.Ini_Planifica = auditoriasJson.Ini_Planifica;

                auditorias.Fin_Planifica = auditoriasJson.Fin_Planifica;

                auditorias.Ini_Real = auditoriasJson.Ini_Real;

                auditorias.Fin_Real = auditoriasJson.Fin_Real;

                auditorias.Responsable = CambiarTexto(auditoriasJson.Responsable);

                auditorias.Conclusiones = CambiarTexto(auditoriasJson.Conclusiones);

                auditorias.Recomendaciones = CambiarTexto(auditoriasJson.Recomendaciones);
                
                auditorias.Programa = CambiarTexto(auditoriasJson.Programa);
                
                auditorias.Seguimiento = CambiarTexto(auditoriasJson.Seguimiento);

                auditorias.Observaciones = CambiarTexto(auditoriasJson.Observaciones);
                
                this.repoAuditorias.Update(auditorias);

                this.trazas(137, (int)this.UsuarioFrontalSession.Usuario.id, "Se ha dado modificado un registro en SPMOV_Auditorias con ID" + "(" + auditoriasJson.Id + ")");

                this.IdAuditoria = auditoriasJson.Id;

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

        /// <summary>
        /// Editar Auditorias.
        /// </summary>
        [HttpPost]
        public ActionResult VisualizarAuditorias(int? id, string Accion)
        {
            try
            {
                bool valor = true;

                if (id != null)
                {
                    this.IdAuditoria = (int)id;
                }
                else if (this.IdAuditoria != 0)
                {
                    id = this.IdAuditoria;
                }

                valor = this.ValidarCategoria(this.IdAuditoria, this.repoAuditorias.Single(b => b.Id == this.IdAuditoria));

                if (Accion != General.AltaVer.ToDescription())
                {
                    if (!valor) 
                      return this.Json(new { result = 0, Message = ModificarCategoria.MessageCategoria.ToDescription() + "  " + this.Description });
                }

                AuditoriasViewModel auditorias = new AuditoriasViewModel();

                this._serviciosAuditorias.ListAuditorias().Where(x => x.Id == id).ToList().ForEach(
                   p =>
                   {
                       auditorias.Id            = p.Id;
                       auditorias.Id_IIPP       = p.Id_IIPP;
                       auditorias.IIPP          = p.IIPP;
                       auditorias.Id_Puerto     = p.Id_Puerto;
                       auditorias.Puerto        = p.Puerto;
                       auditorias.Descripcion   = p.Descripcion;
                       auditorias.Tipo          = p.Tipo;
                       auditorias.NombreTipo    = p.NombreTipo;
                       auditorias.Estado        = p.Estado;
                       auditorias.NombreEstado  = p.NombreEstado;
                       auditorias.Ini_Programa  = p.Ini_Programa;
                       auditorias.Fin_Programa  = p.Fin_Programa;
                       auditorias.Ini_Planifica = p.Ini_Planifica;
                       auditorias.Fin_Planifica = p.Fin_Planifica;
                       auditorias.Ini_Real      = p.Ini_Real;
                       auditorias.Fin_Real      = p.Fin_Real;
                       auditorias.Responsable   = p.Responsable;
                       this.ViewBag.Conclusiones    = p.Conclusiones;
                       this.ViewBag.Recomendaciones = p.Recomendaciones;
                       this.ViewBag.Programa        = p.Programa;
                       this.ViewBag.Seguimiento   = p.Seguimiento;
                       this.ViewBag.Observaciones = p.Observaciones;

                   });

                ViewBag.alta = "display:block";

                this.comboPuertos(null);

                this.comboInstalacionesPuertos(null, auditorias.Id_Puerto);

                if (Accion == General.EditGeneral.ToDescription())
                {
                    ViewBag.disabled = true;

                    ViewBag.action = "Edit";

                    ViewBag.Mensaje = "Editar Auditorias";

                    ViewBag.Combo = "display:block";

                    ViewBag.Texto = "display:none";

                    ViewBag.ToolAuditoria = valor;

                    // Estado
                    if (auditorias.Estado == 1)
                    {
                        this.ViewData["EstadoFechaPromEne"] = "1";
                        this.ViewData["EstadoFechaPromCol"] = "#FFFFFF";
                    }
                    else
                    {
                        this.ViewData["EstadoFechaPromEne"] = "0";
                        this.ViewData["EstadoFechaPromCol"] = "#eeeeee";
                    }
                    if (auditorias.Estado == 2)
                    {
                        this.ViewData["EstadoFechaPlanEne"] = "1";
                        this.ViewData["EstadoFechaPlanCol"] = "#FFFFFF";
                    }
                    else
                    {
                        this.ViewData["EstadoFechaPlanEne"] = "0";
                        this.ViewData["EstadoFechaPlanCol"] = "#eeeeee";
                    }
                    if (auditorias.Estado == 3)
                    {
                        this.ViewData["EstadoFechaRealEne"] = "1";
                        this.ViewData["EstadoFechaRealCol"] = "#FFFFFF";
                    }
                    else
                    {
                        this.ViewData["EstadoFechaRealEne"] = "0";
                        this.ViewData["EstadoFechaRealCol"] = "#eeeeee";
                    }

                }
                else
                {
                    ViewBag.disabled = false;

                    ViewBag.action = "Ver";

                    ViewBag.Mensaje = "Visualizar Auditorias";

                    ViewBag.Combo = "display:none";

                    ViewBag.Texto = "display:block";

                    ViewBag.ToolAuditoria = valor;

                    // estados
                    this.ViewData["EstadoFechaPromEne"] = "0";
                    this.ViewData["EstadoFechaPromCol"] = "#eeeeee";
                    this.ViewData["EstadoFechaPlanEne"] = "0";
                    this.ViewData["EstadoFechaPlanCol"] = "#eeeeee";
                    this.ViewData["EstadoFechaRealEne"] = "0";
                    this.ViewData["EstadoFechaRealCol"] = "#eeeeee";

                }
                this.ViewBag.navegador = this.Browser;


                return this.PartialView("~/Views/Auditorias/Create.cshtml", auditorias);
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

        
        /// <summary>
        /// Validar Auditorias.
        /// </summary>
        [HttpPost]
        public ActionResult ValidarAuditorias(int Id)
        {
            try
            {

                var retorno = ValidarCategoria(Id,this.repoAuditorias.Single(b => b.Id == Id)) ? EliminarCategoria.MessageCategoria.ToDescription() + "  " + this.Description: string.Empty;

                var valor = retorno == string.Empty;
                
                return this.Json(new { result = valor, Message = retorno });

            }
            catch (Exception ex)
            {
                this.log.PublishException(new SecureportException(ex));

                return this.Json(new { result = false, Message = ex.Message });
            }

        }
        
        /// <summary>
        /// Borrar Auditorias.
        /// </summary>
        [HttpPost]
        public ActionResult EliminarAuditorias(int Id)
        {
            try
            {
                    this.repoAuditorias.Delete(this.repoAuditorias.Single(b => b.Id == Id));

                    this.trazas(138,(int)this.UsuarioFrontalSession.Usuario.id,"Se ha realizado la eliminación de un registro en SPMOV_Auditorias con ID" + "(" + Id.ToString() + ")");
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
        public ActionResult Observaciones()
        {
            this.ViewBag.Observaciones = this._serviciosAuditorias.ListAuditorias().Where(x => x.Id == this.IdAuditoria).ToList().FirstOrDefault().Observaciones;

            return this.PartialView("~/Views/Auditorias/_PartialObservaciones.cshtml");
        }

        [HttpPost]
        public ActionResult CambiarCentro(int? id)
        {
            try
            {

                this.comboInstalacionesPuertos(null, id);

                return this.PartialView("~/Views/Auditorias/ComboCentro.cshtml");

            }
            catch (Exception ex)
            {
                this.log.PublishException(new SecureportException(ex));

                return this.Json(new { result = false, Message = ex.Message });
            }

        }

        [HttpPost]
        public ActionResult CambiarEstadoPlan(string FechaIni, string FechaFin, int? Estado)
        {
            try
            {
                this.ViewData["FechaIniPla"] = FechaIni;
                this.ViewData["FechaFinPla"] = FechaFin;

                if (Estado == 2)
                {
                    this.ViewData["EstadoFechaPlanEne"] = "1";
                    this.ViewData["EstadoFechaPlanCol"] = "#FFFFFF";
                }
                else
                {
                    this.ViewData["EstadoFechaPlanEne"] = "0";
                    this.ViewData["EstadoFechaPlanCol"] = "#eeeeee";
                    this.ViewData["EstadoFechaRealEne"] = "0";
                    this.ViewData["EstadoFechaRealCol"] = "#eeeeee";
                }
                return this.PartialView("~/Views/Auditorias/EstadoPlanifica.cshtml");

            }
            catch (Exception ex)
            {
                this.log.PublishException(new SecureportException(ex));

                return this.Json(new { result = false, Message = ex.Message });
            }

        }

        [HttpPost]
        public ActionResult CambiarEstadoReal(string FechaIni, string FechaFin, int? Estado)
        {
            try
            {
                this.ViewData["FechaIniRea"] = FechaIni;
                this.ViewData["FechaFinRea"] = FechaFin;

                if (Estado == 3)
                {
                    this.ViewData["EstadoFechaRealEne"] = "1";
                    this.ViewData["EstadoFechaRealCol"] = "#FFFFFF";
                }
                else
                {
                    this.ViewData["EstadoFechaRealEne"] = "0";
                    this.ViewData["EstadoFechaRealCol"] = "#eeeeee";

                }
                return this.PartialView("~/Views/Auditorias/EstadoReal.cshtml");

            }
            catch (Exception ex)
            {
                this.log.PublishException(new SecureportException(ex));

                return this.Json(new { result = false, Message = ex.Message });
            }

        }

        [HttpPost]
        public ActionResult CambiarEstadoProm(string FechaIni, string FechaFin, int? Estado)
        {
            try
            {
                this.ViewData["FechaIniPro"] = FechaIni;
                this.ViewData["FechaFinPro"] = FechaFin;

                if (Estado == 1)
                {
                    this.ViewData["EstadoFechaPromEne"] = "1";
                    this.ViewData["EstadoFechaPromCol"] = "#FFFFFF";
                }
                else
                {
                    this.ViewData["EstadoFechaPromEne"] = "0";
                    this.ViewData["EstadoFechaPromCol"] = "#eeeeee";

                }
                return this.PartialView("~/Views/Auditorias/EstadoProm.cshtml");

            }
            catch (Exception ex)
            {
                this.log.PublishException(new SecureportException(ex));

                return this.Json(new { result = false, Message = ex.Message });
            }

        }


        #endregion

        #region Documentos

        public ActionResult AsociadosEdit(bool ToolBar)
        {
            this.UsuarioFrontalSession.valor = ToolBar;

            return PartialView("~/Views/Auditorias/Documentos/_PartialAsociadosEdit.cshtml", this.UsuarioFrontalSession);

        }

        public ActionResult AsociadosEditAuditorias(int Id)
        {
            return PartialView("~/Views/Auditorias/Documentos/_PartialAsociadosEdit.cshtml", this.UsuarioFrontalSession);
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

                AuditoriasViewModel Auditorias = new AuditoriasViewModel();

                this._serviciosAuditorias.ListAuditorias().Where(x => x.Id == this.IdAuditoria).ToList().ForEach(
                   p =>
                   {
                       Auditorias.Id = p.Id;
                       Auditorias.Descripcion = p.Descripcion;

                   });

                return this.PartialView("~/Views/Auditorias/_PartialAsignarDocumentos.cshtml", Auditorias);

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
                

                IEnumerable<Doc_Asoc> result = (from t1 in this._serviciosDocumentos.GetDocs(this.IdAuditoria, 3)
                                                         orderby t1.id
                                                         select new Doc_Asoc
                                                         {
                                                             id = t1.id,
                                                             TipoNombre = t1.TipoNombre,
                                                             documento = t1.documento.Substring((t1.documento.IndexOf("_")) + 1),
                                                             descripcion = t1.descripcion
                                                         }).OrderBy(x=> x.documento);

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

                this.trazas(142, (int)this.UsuarioFrontalSession.Usuario.id, "Se ha dado de modificado un registro en SPMOV_Documentos_SP " + this.UsuarioFrontalSession.Usuario.login);

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
                //string nombreRuta = ConfigurationManager.AppSettings["RutaDOC"];

                //string nombreFichero = ConfigurationManager.AppSettings["Ficheros"];
                //string ruta = Path.Combine(nombreRuta, nombreFichero, doc);
                //string Extencion = Path.GetExtension(ruta);
                //string filepath = AppDomain.CurrentDomain.BaseDirectory + Path.Combine(ConfigurationManager.AppSettings["Ficheros"], doc);
                //byte[] filedata = System.IO.File.ReadAllBytes(filepath);
                //string contentType = MimeMapping.GetMimeMapping(filepath);

                //var cd = new System.Net.Mime.ContentDisposition
                //{
                //    FileName = doc,
                //    Inline = true,
                //};

                //Response.AppendHeader("Content-Disposition", cd.ToString());

                //return File(filedata, contentType);


                //this.UsuarioFrontalSession.Path = Path.Combine(nombreRuta,nombreFichero,doc);

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

                IEnumerable<AuditoriasViewModel> Auditorias = this._serviciosAuditorias.ListAuditorias().Where(x => x.Id == this.IdAuditoria);

                AuditoriasViewModel temp = Auditorias.FirstOrDefault();

                Docs_Usuario docsInstalacion = this.repoDocumentosUsuario.Single(x => x.id == id);

                documento.Nombre = temp.Descripcion;

                documento.Apellidos = temp.Descripcion;

                documento.descripcion = docsInstalacion.descripcion;

                documento.tipodocumento = docsInstalacion.id_tipo_doc;

                documento.documento = docsInstalacion.documento.Substring((docsInstalacion.documento.IndexOf("_")) + 1);

                return this.PartialView("~/Views/Auditorias/_PartialEditDocumentos.cshtml", documento);

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

                this.trazas(141, (int)this.UsuarioFrontalSession.Usuario.id, "Se ha dado de borrado un registro en SPMOV_Documentos_SP " + this.UsuarioFrontalSession.Usuario.login);

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

                IEnumerable<AuditoriasViewModel> Auditorias = this._serviciosAuditorias.ListAuditorias().Where(x => x.Id == this.IdAuditoria);

                AuditoriasViewModel temp = Auditorias.FirstOrDefault();

                Docs_Usuario docsFormacion = this.repoDocumentosUsuario.Single(x => x.id == id);

                documento.Nombre = temp.Descripcion;

                documento.Apellidos = temp.Descripcion;

                documento.descripcion = docsFormacion.descripcion;

                documento.tipodocumento = docsFormacion.id_tipo_doc;

                documento.documento = docsFormacion.documento.Substring((docsFormacion.documento.IndexOf("_")) + 1); 


                return this.PartialView("~/Views/Auditorias/_PartialEditDocumentos.cshtml", documento);

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

             IEnumerable<Doc_Asoc> result = this._serviciosDocumentos.GetDocs(this.IdAuditoria, 3).ToList().Where(x => x.documento == nombre);
             if(result.Count() >0)
                return this.Json(new { result = 0 });
             else
                return this.Json(new { result = 1 }); 
        }
        


        #endregion

        #endregion
    }
}