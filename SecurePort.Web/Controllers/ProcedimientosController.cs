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
	using Kendo.Mvc.Extensions;
	using Kendo.Mvc.UI;
	using SecurePort.Entities.Enums;
	using SecurePort.Entities.Exceptions;
	using SecurePort.Entities.Models;
	using SecurePort.Entities.Models.Json;
	using SecurePort.Services.Interfaces;
	using SecurePort.Services.Repository;
	using System.Configuration;
	using System.Web;
	using System.Web.UI.WebControls;
    using Newtonsoft.Json.Linq; 

	#endregion

	public class ProcedimientosController : BaseController
	{
		public ProcedimientosController(IServiciosProcedimientos serviciosProcedimientos,
								 ProcedimientosRepository   repoProcedimientos,
                                 IServiciosOrganismo serviciosOrganismos,
                                 DocumentosUsuarioRepository repoDocumentosUsuario,
                                 IServiciosDocumentos servicioDocumentos,
                                 IServiciosUsuarios servicioUsuarios,
								 TrazasRepository     repotrazas, 
								 ILogger logger)
			
			: base(serviciosProcedimientos,
				   repoProcedimientos,
                   serviciosOrganismos,
                   repoDocumentosUsuario,
                   servicioDocumentos,
                   servicioUsuarios, 
				   repotrazas,logger)

		{
		}

		#region Buscador
		/// <summary>
		/// Permite buscar PROCEDIMIENTOS_AP 
		/// </summary>
		public ActionResult BuscadorProcedimientos([DataSourceRequest]DataSourceRequest request)
		{
			try
			{
                IEnumerable<ProcedimientosViewModel> procedimiento = new List<ProcedimientosViewModel>();

                if (this.UsuarioFrontalSession.Usuario.categoria == (int)EnumCategoria.Administrador
                    || this.UsuarioFrontalSession.Usuario.categoria == (int)EnumCategoria.PuertosdelEstado)
                {
                    procedimiento = this._serviciosProcedimientos.GetAllProcedimientos();
                }
                else
                {
                    var categoria = this.UsuarioFrontalSession.Usuario.categoria;
                    if (this.Id_TipoAP != 1)
                        procedimiento = this._serviciosProcedimientos.GetAllProcedimientos();
                    else
                        procedimiento = this._serviciosProcedimientos.GetAllProcedimientos(this.UsuarioFrontalSession.ListDependenciasDepenUsuarios, categoria);

                }

                if (this.Id_TipoAP == 1) // Autoridades protuarias AP
                {
                    procedimiento = procedimiento.ToList().Where(x => x.Id_Organismo != null && x.Ind_AP == null);
                   
                }
                if (this.Id_TipoAP == 2) // Puertos del estado OPPE
                {
                    procedimiento = procedimiento.ToList().Where(x => x.Id_Organismo == null && x.Ind_AP == null);
                   
                }
                if (this.Id_TipoAP == 3) // Puertos del estado OPPE para AP
                {
                    procedimiento = procedimiento.ToList().Where(x => x.Id_Organismo == null && x.Ind_AP == true);
                   
                }

                return this.Json(procedimiento.ToDataSourceResult(request));
                
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
		/// Retorna una lista de todos los Procedimientos de autoridades portuarias AP
		/// </summary>
		public ActionResult ListadoProcedimientos()
		{
			try
			{
                ViewBag.Mensaje = "Procedimientos de Autoridades Portuarias";
				ViewBag.UrlDelete = "/Procedimientos/EliminarProcedimiento";
                this.Id_TipoAP = 1;
                ViewBag.VistaAP = true;
                ViewBag.VistaOPPE = false;
                ViewBag.VistaOPPEAP = false;
                ViewBag.CodigoMenu = "#Id043";
				return this.PartialView("ListadoProcedimientos", this.UsuarioFrontalSession);
			}
			catch (Exception ex)
			{
				this.log.PublishException(new SecureportException(ex));
				throw;
			}
		}

        /// <summary>
        /// Retorna una lista de todos los Procedimientos de autoridades portuarias OPPE
        /// </summary>
        public ActionResult ListadoProcedimientosOPPE()
        {
            try
            {
                ViewBag.Mensaje = "Procedimientos de Puertos del Estado";
                ViewBag.UrlDelete = "/Procedimientos/EliminarProcedimiento";
                this.Id_TipoAP = 2;
                ViewBag.VistaAP = false;
                ViewBag.VistaOPPE = true;
                ViewBag.VistaOPPEAP = false;
                ViewBag.CodigoMenu = "#Id044";
                return this.PartialView("ListadoProcedimientos", this.UsuarioFrontalSession);
            }
            catch (Exception ex)
            {
                this.log.PublishException(new SecureportException(ex));
                throw;
            }
        }

        /// <summary>
        /// Retorna una lista de todos los Procedimientos de autoridades portuarias OPPE
        /// </summary>
        public ActionResult ListadoProcedimientosOPPEAP()
        {
            try
            {
                ViewBag.Mensaje = "Procedimientos de Puertos del Estado para Autoridades Portuarias";
                ViewBag.UrlDelete = "/Procedimientos/EliminarProcedimiento";
                this.Id_TipoAP = 3;
                ViewBag.VistaAP = false;
                ViewBag.VistaOPPE = false;
                ViewBag.VistaOPPEAP = true;
                ViewBag.CodigoMenu = "#Id045";
                return this.PartialView("ListadoProcedimientos", this.UsuarioFrontalSession);
            }
            catch (Exception ex)
            {
                this.log.PublishException(new SecureportException(ex));
                throw;
            }
        }


		#endregion

		#region Procedimientos
		/// <summary>
		/// Permite el Alta Registros Gisis.
		/// </summary>
		[HttpPost]
		public ActionResult Create()
		{
			ProcedimientosViewModel Procedimiento = new ProcedimientosViewModel();

			if (this.Id_TipoAP == 1)
                ViewBag.Mensaje = "Alta Procedimiento de Autoridades Portuarias";
            if (this.Id_TipoAP == 2)
                ViewBag.Mensaje = "Alta Procedimiento de Puertos del Estado";
            if (this.Id_TipoAP == 3)
                ViewBag.Mensaje = "Alta Procedimiento de Puertos del Estado Para Autoridades Portuarias";

			ViewBag.action = General.AltaGeneral.ToDescription();

			Procedimiento.Id = 1;
			
            Procedimiento.Fecha = DateTime.MinValue;

			ViewBag.alta = "display:none";

			this.ViewBag.Observaciones = string.Empty;

            ViewBag.disabled = true;

            ViewBag.Combo = "display:block";

            ViewBag.Texto = "display:none";

            ViewBag.display = "display:none";

            ViewBag.ToolProcedimiento = false;

			return this.PartialView("~/Views/Procedimientos/_PartialProcedimientos.cshtml", Procedimiento);
		}
		/// <summary>
		/// Visualizar los datos de Resgistro Gisis
		/// </summary>
		[HttpPost]
		public ActionResult VisualizarProcedimiento(int? id, string Accion)
		{
            if (id != null)
                this.IdProcedimiento = (int)id;
            else if (this.IdProcedimiento != 0)
                id = this.IdProcedimiento;

			try
			{
				if (Accion == General.EditGeneral.ToDescription())
				{
					ViewBag.disabled = true;
					ViewBag.Combo = "display:block";
					ViewBag.Texto = "display:none";
                    if (this.Id_TipoAP == 1)
                        ViewBag.Mensaje = "Editar Procedimiento de Autoridades Portuarias";
                    if (this.Id_TipoAP == 2)
                        ViewBag.Mensaje = "Editar Procedimiento de Puertos del Estado";
                    if (this.Id_TipoAP == 3)
                        ViewBag.Mensaje = "Editar Procedimiento de Puertos del Estado Para Autoridades Portuarias";
					ViewBag.ToolProcedimiento = false;
				}
				else
				{
					ViewBag.disabled = false;
					ViewBag.Combo = "display:none";
					ViewBag.Texto = "display:block";
                    if (this.Id_TipoAP == 1)
                        ViewBag.Mensaje = "Visualizar Procedimiento de Autoridades Portuarias";
                    if (this.Id_TipoAP == 2)
                        ViewBag.Mensaje = "Visualizar Procedimiento de Puertos del Estado";
                    if (this.Id_TipoAP == 3)
                        ViewBag.Mensaje = "Visualizar Procedimiento de Puertos del Estado Para Autoridades Portuarias";
					ViewBag.ToolProcedimiento = true;
				}

				ViewBag.alta = "display:block";

				ViewBag.action = General.EditGeneral.ToDescription();

				this.ViewBag.navegador = this.Browser;

				ProcedimientosViewModel Procedimiento = new ProcedimientosViewModel();

				this._serviciosProcedimientos.GetAllProcedimientos().Where(x => x.Id == id).ToList().ForEach(
				   p =>
				   {
					   Procedimiento.Id           = p.Id;
                       Procedimiento.Id_Organismo = p.Id_Organismo;
					   Procedimiento.Organismo    = p.Organismo;
					   Procedimiento.Ind_AP       = p.Ind_AP;
					   Procedimiento.Titulo       = p.Titulo;
					   Procedimiento.Fecha        = p.Fecha;
					   Procedimiento.Descripcion  = p.Descripcion;
					   this.ViewBag.Observaciones = p.Observaciones;
				   });

				
				return this.PartialView("~/Views/Procedimientos/_PartialProcedimientos.cshtml", Procedimiento);
			}
			catch (Exception ex)
			{
				this.log.PublishException(new SecureportException(ex));

				return this.Json(new { result = false, Message = ex.Message });
			}
		}
		/// <summary>
		/// Alta-Editar Procedimientos.
		/// </summary>
		[HttpPost]
		public ActionResult AltaEditarProcedimiento(ProcedimientosJson procedimientosJson)
		{
			try
			{

                System.Web.Mvc.ActionResult Respuesta = null;
                int control =1;
				switch (procedimientosJson.action)
				{

					case "Alta":
                        Respuesta = AltaProcedimiento(procedimientosJson);
                            
						break;

					default:
                        Respuesta = EditarProcedimiento(procedimientosJson);
						break;
				}
                var datos = ((System.Web.Mvc.JsonResult)(Respuesta)).Data.ToString();

                if (datos.IndexOf("true") == -1 && datos.IndexOf("True") == -1)
                    return this.Json(new { result = 2, Message = "El usuario no puede dar de alta el procedimiento " + procedimientosJson.Titulo });


				return this.Json(new { result = control });
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
		/// Borrar Gisis.
		/// </summary>
		[HttpPost]
		public ActionResult EliminarProcedimiento(int Id)
		{
			try
			{
				this.repoProcedimientos.Delete(this.repoProcedimientos.Single(x => x.Id == Id));

                this.trazas(188, (int)this.UsuarioFrontalSession.Usuario.id, "Se ha realizado la eliminación de un registro en SPMOV_Procedimientos con ID" + "(" + Id.ToString() + ")");

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

				return this.Json(new { result = false, Message = "Instrucción DELETE en conflicto con la restricción REFERENCE." });
			}

			return this.Json(new { result = true });
		}

        [HttpPost]
        public ActionResult AltaProcedimiento(ProcedimientosJson procedimientoJson)
		{
			try{
                // organismo al que pertenece el usuario
                int? organismoId = null;
                    if(this.Id_TipoAP ==1)
                        organismoId = this.UsuarioFrontalSession.ListDependenciasDepenUsuarios.ToList().Where(x=> x.categoria == (int)EnumCategoria.OrganismoGestionPortuaria).FirstOrDefault().id_Dependencia;

                    bool? IndAP = null;
                    if (this.Id_TipoAP == 3)
                        IndAP = true;

				Procedimientos procedimiento = new Procedimientos();

				procedimiento.ID_Usu_alta = this.UsuarioFrontalSession.Usuario.id;

				procedimiento.Fech_Alta = DateTime.Now;

                procedimiento.Id_Organismo = organismoId;

				procedimiento.Ind_AP = IndAP;

				procedimiento.Titulo = procedimientoJson.Titulo;

				procedimiento.Fecha = procedimientoJson.Fecha;

				procedimiento.Descripcion =  CambiarTexto(procedimientoJson.Descripcion);

                procedimiento.Observaciones = CambiarTexto(procedimientoJson.Observaciones);

				this.repoProcedimientos.Create(procedimiento);

				procedimientoJson.Id= this.db.Procedimientos.OrderByDescending(u => u.Id).FirstOrDefault().Id;

                this.trazas(186, (int)this.UsuarioFrontalSession.Usuario.id, "Se ha dado de alta un registro en SPMOV_Procedimientos con ID" + "(" + procedimientoJson.Id.ToString() + ")");
				
                this.IdProcedimiento = procedimientoJson.Id;

                return this.Json(new { result = true });
				
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

				return this.Json(new { result = false, Message = "Instrucción DELETE en conflicto con la restricción REFERENCE." });
			}

			//return this.Json(new { result = true });
		}

		[HttpPost]
        public ActionResult EditarProcedimiento(ProcedimientosJson procedimientoJson)
		{
			try
			{
				Procedimientos procedimiento = this.repoProcedimientos.Single(x => x.Id == procedimientoJson.Id);

                procedimiento.Titulo = procedimientoJson.Titulo;

                procedimiento.Fecha = procedimientoJson.Fecha;

                procedimiento.Descripcion = CambiarTexto(procedimientoJson.Descripcion);

                procedimiento.Observaciones = CambiarTexto(procedimientoJson.Observaciones);

				this.repoProcedimientos.Update(procedimiento);

                this.trazas(187, (int)this.UsuarioFrontalSession.Usuario.id, "Se ha realizado la modificación de un registro en SPMOV_Procedimientos con ID" + "(" + procedimientoJson.Id.ToString() + ")");

                this.IdProcedimiento = procedimientoJson.Id;

                return this.Json(new { result = true });
                
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

				return this.Json(new { result = false, Message = "Instrucción DELETE en conflicto con la restricción REFERENCE." });
			}

			//return this.Json(new { result = true });
		}

	
		#endregion

        #region Documentos

        public ActionResult AsociadosEdit(bool ToolBar)
        {
            this.UsuarioFrontalSession.valor = ToolBar;
            if (this.Id_TipoAP == 1)
            {
                ViewBag.VistaAP_DOC = true;
                ViewBag.VistaOPPE_DOC = false;
                ViewBag.VistaOPPEAP_DOC = false;
                ViewBag.MensajeDoc = "Asignar Documentos Asociados de Procedimientos de Autoridades Portuarias";
            }
            else if (this.Id_TipoAP == 2)
            {
                ViewBag.VistaAP_DOC = false;
                ViewBag.VistaOPPE_DOC = true;
                ViewBag.VistaOPPEAP_DOC = false;
                ViewBag.MensajeDoc = "Asignar Documentos Asociados de Procedimientos de Puertos del Estado";
            }
            else if (this.Id_TipoAP == 3)
            {
                ViewBag.VistaAP_DOC = false;
                ViewBag.VistaOPPE_DOC = false;
                ViewBag.VistaOPPEAP_DOC = true;
                ViewBag.MensajeDoc = "Asignar Documentos Asociados de Procedimientos de Puertos del Estado Para Autoridades Portuarias";
            }

            return PartialView("~/Views/Procedimientos/Documentos/_PartialAsociadosEdit.cshtml", this.UsuarioFrontalSession);

        }

        public ActionResult AsociadosEditProcedimientos(int Id)
        {
            if (this.Id_TipoAP == 1)
            {
                ViewBag.VistaAP_DOC = true;
                ViewBag.VistaOPPE_DOC = false;
                ViewBag.VistaOPPEAP_DOC = false;               
            }
            else if (this.Id_TipoAP == 2)
            {
                ViewBag.VistaAP_DOC = false;
                ViewBag.VistaOPPE_DOC = true;
                ViewBag.VistaOPPEAP_DOC = false;                
            }
            else if (this.Id_TipoAP == 3)
            {
                ViewBag.VistaAP_DOC = false;
                ViewBag.VistaOPPE_DOC = false;
                ViewBag.VistaOPPEAP_DOC = true;                
            }
            return PartialView("~/Views/Procedimientos/Documentos/_PartialAsociadosEdit.cshtml", this.UsuarioFrontalSession);
        }
        /// <summary>
        /// Permite visualizar los datos de un usuario con documentos.
        /// </summary>
        [HttpPost]
        public ActionResult AsignarDocumentos()
        {
            try
            {
                if (this.Id_TipoAP == 1)
                    ViewBag.MensajeDoc = "Asignar Documentos Asociados de Procedimientos de Autoridades Portuarias";
                if (this.Id_TipoAP == 2)
                    ViewBag.MensajeDoc = "Asignar Documentos Asociados de Procedimientos de Puertos del Estado";
                if (this.Id_TipoAP == 3)
                    ViewBag.MensajeDoc = "Asignar Documentos Asociados de Procedimientos de Puertos del Estado Para Autoridades Portuarias";

                this.ComboTipos_Documento();

                ProcedimientosViewModel procedimiento = new ProcedimientosViewModel();

                this._serviciosProcedimientos.GetAllProcedimientos().Where(x => x.Id == this.IdProcedimiento).ToList().ForEach(
                   p =>
                   {
                       procedimiento.Id = p.Id;
                       procedimiento.Organismo = p.Organismo;
                       procedimiento.Titulo = p.Titulo;                       

                   });

                return this.PartialView("~/Views/Procedimientos/_PartialAsignarDocumentos.cshtml", procedimiento);

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

                IEnumerable<Doc_Asoc> result = (from t1 in this._serviciosDocumentos.GetDocs(this.IdProcedimiento, 13)
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

                this.trazas(191, (int)this.UsuarioFrontalSession.Usuario.id, "Se ha dado de modificado un registro en SPCON_Depen_Usuario con ID " + "(" + DocumentoJson.id.ToString() + ")");

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

                string doc = CambiarTexto(docsUsuario.documento);

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
                //return PartialView("~/Views/Shared/PartialMostrar.cshtml", this.UsuarioFrontalSession);

            }
            catch (Exception ex)
            {
                this.log.PublishException(new SecureportException(ex));

                return this.Json(new { result = false, Message = ex.Message });
            }

        }

        [HttpPost]
        public ActionResult visualizarDocumentoNew(int? id)
        {
            try
            {
                Documento documento = new Documento();

                Docs_Usuario docsUsuario = this.repoDocumentosUsuario.Single(x => x.id == id);

                string doc = CambiarTexto(docsUsuario.documento);

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
               // return PartialView("~/Views/Shared/PartialMostrar.cshtml", this.UsuarioFrontalSession);

            }
            catch (Exception ex)
            {
                this.log.PublishException(new SecureportException(ex));

                return this.Json(new { result = false, Message = ex.Message });
            }

        }




        /// <summary>
        ///     Permite editar un documento asociado.
        /// </summary>
        [HttpPost]
        public ActionResult EditarDocumento(int? id)
        {
            try
            {

                this.ComboTipos_Documento();

                Documento documento = new Documento();

                IEnumerable<ProcedimientosViewModel> procedimiento = this._serviciosProcedimientos.GetAllProcedimientos().Where(x => x.Id == this.IdProcedimiento);

                ProcedimientosViewModel temp = procedimiento.FirstOrDefault();

                Docs_Usuario docsInstalacion = this.repoDocumentosUsuario.Single(x => x.id == id);

                documento.Nombre = temp.Organismo;

                documento.Apellidos = temp.Titulo;

                documento.descripcion = docsInstalacion.descripcion;

                documento.tipodocumento = docsInstalacion.id_tipo_doc;

                documento.documento = docsInstalacion.documento.Substring((docsInstalacion.documento.IndexOf("_")) + 1);

                return this.PartialView("~/Views/Procedimientos/_PartialEditDocumentos.cshtml", documento);

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

                this.trazas(192, (int)this.UsuarioFrontalSession.Usuario.id, "Se ha dado de borrado un registro en SPMOV_Documentos_SP con ID " + "(" + id.ToString() + ")");

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

                IEnumerable<ProcedimientosViewModel> procedimiento = this._serviciosProcedimientos.GetAllProcedimientos().Where(x => x.Id == this.IdProcedimiento);

                ProcedimientosViewModel temp = procedimiento.FirstOrDefault();

                Docs_Usuario docsProcedimiento = this.repoDocumentosUsuario.Single(x => x.id == id);

                documento.Nombre = temp.Organismo;

                documento.Apellidos = temp.Titulo;

                documento.descripcion = docsProcedimiento.descripcion;

                documento.tipodocumento = docsProcedimiento.id_tipo_doc;

                documento.documento = docsProcedimiento.documento.Substring((docsProcedimiento.documento.IndexOf("_")) + 1); 

                return this.PartialView("~/Views/Procedimientos/_PartialEditDocumentos.cshtml", documento);

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

            IEnumerable<Doc_Asoc> result = this._serviciosDocumentos.GetDocs(this.IdProcedimiento, 13).ToList().Where(x => x.documento == nombre);
            if (result.Count() > 0)
                return this.Json(new { result = 0 });
            else
                return this.Json(new { result = 1 });
        }


        #endregion
		
	}
}