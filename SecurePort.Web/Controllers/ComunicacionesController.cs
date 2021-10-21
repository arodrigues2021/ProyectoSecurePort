namespace SecurePort.Web.Controllers
{
	#region Using
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Data;
	using System.Data.Entity.Infrastructure;
	using System.Data.Entity.ModelConfiguration;
	using System.Data.Entity.Validation;
	using System.Linq;
	using System.Reflection;
	using System.Web.Mvc;
	using Kendo.Mvc.Extensions;
	using Kendo.Mvc.UI;

	using Microsoft.Ajax.Utilities;

	using SecurePort.Entities;
	using SecurePort.Entities.Enums;
	using SecurePort.Entities.Exceptions;
	using SecurePort.Entities.Models;
	using SecurePort.Entities.Models.Json;
	using SecurePort.Services.Interfaces;
	using SecurePort.Services.Repository;
	using System.Configuration;
	#endregion
	
	public class ComunicacionesController : BaseController
	{

		public ComunicacionesController(IServiciosComunicaciones serviciosComunicaciones,
									   ComunicacionesRepository repoComunicaciones,
									   InstalacionesRepository repoInstalaciones,
									   PuertosRepository repoPuertos,
									   IServiciosPuertos servicioPuertos,
									   IServiciosUsuarios servicioUsuarios,
									   DocumentosUsuarioRepository repoDocumentosUsuario,
									   IServiciosDocumentos servicioDocumentos,
									   IServiciosTipoInstalacion serviciosInstalaciones,
									   TrazasRepository repoTrazas,
									   ILogger log)

			: base(serviciosComunicaciones,
				   repoComunicaciones,
				   repoInstalaciones, 
				   repoPuertos,
				   servicioPuertos,
				   servicioUsuarios,
				   repoDocumentosUsuario, 
				   servicioDocumentos,
				   serviciosInstalaciones,
				   repoTrazas,
				   log)
		{
			 
		}

		#region Buscador
		/// <summary>
		/// Permite buscar las comunicaciones
		/// </summary>
		public ActionResult BuscadorComunicaciones([DataSourceRequest]DataSourceRequest request)
		{
			try
			{
				if (this.UsuarioFrontalSession.Usuario.categoria == (int)EnumCategoria.Administrador || this.UsuarioFrontalSession.Usuario.categoria == (int)EnumCategoria.PuertosdelEstado)
				{
					return this.Json(this._serviciosComunicacion.GetAllComunicaciones().ToDataSourceResult(request));
				}
				var categoria = this.UsuarioFrontalSession.Usuario.categoria;
				return this.Json(this._serviciosComunicacion.GetAllComunicaciones(this.UsuarioFrontalSession.ListDependenciasDepenUsuarios, categoria).ToDataSourceResult(request));
			}
			catch (Exception ex)
			{
				this.log.PublishException(new SecureportException(ex));
				throw;
			}
		}
		#endregion

		#region CRUD

		#region Comunicaciones
		
		/// <summary>
		/// Retorna una lista de todas los Mantenimientos, Calibración y Ensayos.
		/// </summary>
		public ActionResult ListadoComunicaciones()
		{
			try
			{
				ViewBag.UrlDelete = "/Comunicaciones/EliminarComunicacion";
				ViewBag.Mensaje = "Comunicaciones";
				return this.PartialView("ListadoComunicaciones", this.UsuarioFrontalSession);
			}
			catch (Exception ex)
			{
				this.log.PublishException(new SecureportException(ex));
				throw;
			}
		}
		/// <summary>
		/// Crear una Comunicación
		/// </summary>
		[HttpPost]
		public ActionResult Create()
		{

			this.comboPuertos(null);

			this.comboInstalacionesPuertos(null, null);

			ComunicacionesViewModel comunicaciones = new ComunicacionesViewModel();

			if (((MultiSelectList)(this.ViewData["puertos"])).Count() == 1)
				comunicaciones.Id_Puerto = Convert.ToInt32(((SelectList)this.ViewData["puertos"]).FirstOrDefault().Value);


			this.comboInstalacionesPuertos(null, null);

			ViewBag.disabled = true;

			ViewBag.Mensaje = "Alta Comunicaciones";

			ViewBag.disabled = true;

			ViewBag.alta = "display:none";

			ViewBag.action = General.AltaGeneral.ToDescription();

			ViewBag.ToolComunicacion = false;

			ViewBag.Combo = "display:block";

			ViewBag.Texto = "display:none";

			return this.PartialView("~/Views/Comunicaciones/Create.cshtml", comunicaciones);
		}

		/// <summary>
		/// Alta-Editar Comunicaciones.
		/// </summary>
		[HttpPost]
		public ActionResult AltaEditarComunicacion(ComunicacionesJson comunicacionesJson)
		{
			try
			{
				 int resultado = 0;

				 switch (comunicacionesJson.action)
				{

					case "Alta":

						 AltaComunicacion(comunicacionesJson);
						 resultado = 1;
						 break;

					default:
						 GuardarComunicacion(comunicacionesJson);
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
		/// Alta de Mantenimiento.
		/// </summary>
		[HttpPost]
		public ActionResult AltaComunicacion(ComunicacionesJson comunicacionesJson)
		{
			try
			{
				Comunicaciones comunicaciones = new Comunicaciones();

				comunicaciones.Id_Puerto    = comunicacionesJson.Id_Puerto;

				comunicaciones.Id_IIPP      = comunicacionesJson.Id_IIPP;

				comunicaciones.Mensaje      = this.CambiarTexto(comunicacionesJson.Mensaje);

				comunicaciones.Asunto       = this.CambiarTexto(comunicacionesJson.Asunto);

				comunicaciones.Fecha        =  comunicacionesJson.Fecha;

				comunicaciones.Emisor       = this.CambiarTexto(comunicacionesJson.Emisor);

				comunicaciones.Receptor     = this.CambiarTexto(comunicacionesJson.Receptor);

				comunicaciones.Recibido     = comunicacionesJson.Recibido;
				
				comunicaciones.Id_Usu_Alta  = this.UsuarioFrontalSession.Usuario.id;

				comunicaciones.Fech_alta    = DateTime.Now;

				this.repoComunicacion.Create(comunicaciones);

				comunicacionesJson.Id = this.db.Comunicaciones.OrderByDescending(u => u.Id).FirstOrDefault().Id;

				this.trazas(160, (int)this.UsuarioFrontalSession.Usuario.id, "Se ha dado de alta un registro en SPMOV_Comunicaciones con ID" + "(" + comunicacionesJson.Id.ToString() + ")");

				this.IdComunicacion = comunicacionesJson.Id;

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
		/// Guardar Mantenimiento, Calibración y Ensayos.
		/// </summary>
		[HttpPost]
		public ActionResult GuardarComunicacion(ComunicacionesJson comunicacionesJson)
		{
			try
			{

				Comunicaciones comunicaciones = repoComunicacion.Single(x => x.Id == comunicacionesJson.Id);

				comunicaciones.Id_Puerto = comunicacionesJson.Id_Puerto;

				comunicaciones.Id_IIPP   = comunicacionesJson.Id_IIPP;

				comunicaciones.Mensaje   = this.CambiarTexto(comunicacionesJson.Mensaje);

				comunicaciones.Asunto    = this.CambiarTexto(comunicacionesJson.Asunto);

				comunicaciones.Fecha     = Convert.ToDateTime(comunicacionesJson.Fecha);

				comunicaciones.Emisor    = this.CambiarTexto(comunicacionesJson.Emisor);

				comunicaciones.Receptor  = this.CambiarTexto(comunicacionesJson.Receptor);

				comunicaciones.Recibido = comunicacionesJson.Recibido;

				this.repoComunicacion.Update(comunicaciones);

				this.trazas(161, (int)this.UsuarioFrontalSession.Usuario.id, "Se ha dado modificado un registro en SPMOV_Comunicaciones con ID " + "(" + comunicacionesJson.Id.ToString() + ")");

				this.IdComunicacion = comunicacionesJson.Id;

				return this.Json(new { result = 1 });

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
		/// Validar Comunicacion
		/// </summary>
		[HttpPost]
		public ActionResult ValidarComunicacion(int Id)
		{
			try
			{

				var retorno = ValidarCategoria(Id, this.repoComunicacion.Single(x => x.Id == Id)) ? EliminarCategoria.MessageCategoria.ToDescription() + "  " + this.Description : string.Empty;

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
		/// Editar Comunicaciones
		/// </summary>
		[HttpPost]
		public ActionResult VisualizarComunicacion(int? id, string Accion)
		{
			try
			{
				bool valor = true;

				if (id != null)
					this.IdComunicacion = (int)id;
				else if (this.IdComunicacion != 0)
					id = this.IdComunicacion;

				valor = ValidarCategoria(this.IdComunicacion, this.repoComunicacion.Single(x => x.Id == this.IdComunicacion));

				if (Accion != General.AltaVer.ToDescription())
				{
					if(!valor) 
					return this.Json(new { result = 0, Message = ModificarCategoria.MessageCategoria.ToDescription() + "  " + this.Description });
				}

				ComunicacionesViewModel comunicaciones = new ComunicacionesViewModel();

				this._serviciosComunicacion.GetAllComunicaciones().Where(x => x.Id == id).ToList().ForEach(
				   p =>
				   {
					   comunicaciones.Id        = p.Id;

					   comunicaciones.Id_Puerto = p.Id_Puerto;

					   comunicaciones.Puerto    = p.Puerto;

					   comunicaciones.Id_IIPP   = p.Id_IIPP;

					   comunicaciones.IIPP      = p.IIPP;

					   this.ViewBag.Observaciones = this.CambiarTexto(p.Mensaje);

					   comunicaciones.Asunto    = this.CambiarTexto(p.Asunto);

					   comunicaciones.Fecha     = p.Fecha;

					   comunicaciones.Emisor    = this.CambiarTexto(p.Emisor);

					   comunicaciones.Receptor  = this.CambiarTexto(p.Receptor);

					   comunicaciones.Recibido  = p.Recibido;
					   
				   });

				this.IdComunicacion = comunicaciones.Id;
			   
				ViewBag.alta = "display:block";

				this.comboPuertos(null);

				this.comboInstalacionesPuertos(null, comunicaciones.Id_Puerto);

				if (Accion == General.EditGeneral.ToDescription())
				{
					ViewBag.disabled = true;

					ViewBag.action = General.EditGeneral.ToDescription();

					ViewBag.Mensaje = "Editar Comunicaciones";

					ViewBag.Combo = "display:block";

					ViewBag.Texto = "display:none";

					ViewBag.ToolComunicacion = valor;

				}
				else
				{
					ViewBag.disabled = false;

					ViewBag.action = "Ver";

					ViewBag.Mensaje = "Visualizar Comunicaciones";

					ViewBag.Combo = "display:none";

					ViewBag.Texto = "display:block";

					ViewBag.ToolComunicacion = valor;

				}
				
				this.ViewBag.navegador = this.Browser;

				return this.PartialView("~/Views/Comunicaciones/Create.cshtml", comunicaciones);
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
		/// Borrar Comunicaciones.
		/// </summary>
		[HttpPost]
		public ActionResult EliminarComunicacion(int Id)
		{
			try
			{
				this.repoComunicacion.Delete(this.repoComunicacion.Single(x => x.Id == Id));

				this.trazas(162, (int)this.UsuarioFrontalSession.Usuario.id, "Se ha realizado la eliminación de un registro en SPMOV_Comunicaciones con ID " + "(" + Id.ToString() + ")");
				

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
		public ActionResult CambiarCentro(int? id)
		{
			try
			{

				this.comboInstalacionesPuertos(null, id);

				return this.PartialView("~/Views/Comunicaciones/ComboCentro.cshtml");

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

			return PartialView("~/Views/Comunicaciones/Documentos/_PartialAsociadosEdit.cshtml", this.UsuarioFrontalSession);

		}

		public ActionResult AsociadosEditComunicacion(int Id)
		{
			return PartialView("~/Views/Comunicaciones/Documentos/_PartialAsociadosEdit.cshtml", this.UsuarioFrontalSession);
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

				ComunicacionesViewModel comunicacion = new ComunicacionesViewModel();

				this._serviciosComunicacion.GetAllComunicaciones().Where(x=> x.Id == this.IdComunicacion).ToList().ForEach(
				   p =>
				   {
					   comunicacion.Id = p.Id;
					   comunicacion.Puerto = p.Puerto;
					   comunicacion.Asunto = p.Asunto;
				   });

				return this.PartialView("~/Views/Comunicaciones/_PartialAsignarDocumentos.cshtml", comunicacion);

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

				IEnumerable<Doc_Asoc> result = (from t1 in this._serviciosDocumentos.GetDocs(this.IdComunicacion, 4)
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

				this.trazas(164, (int)this.UsuarioFrontalSession.Usuario.id, "Se ha dado de modificado un registro en SPCON_Depen_Usuario con ID " + "(" + DocumentoJson.id.ToString() + ")");

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

				IEnumerable<ComunicacionesViewModel> comunicacion = this._serviciosComunicacion.GetAllComunicaciones().Where(x => x.Id == this.IdComunicacion);

				ComunicacionesViewModel temp = comunicacion.FirstOrDefault();

				Docs_Usuario docsInstalacion = this.repoDocumentosUsuario.Single(x => x.id == id);

				documento.descripcion = docsInstalacion.descripcion;

				documento.tipodocumento = docsInstalacion.id_tipo_doc;

				documento.documento = docsInstalacion.documento.Substring((docsInstalacion.documento.IndexOf("_")) + 1);

				return this.PartialView("~/Views/Comunicaciones/_PartialEditDocumentos.cshtml", documento);

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

				this.trazas(165, (int)this.UsuarioFrontalSession.Usuario.id, "Se ha dado de borrado un registro en SPMOV_Documentos_SP con ID " + "(" + id.ToString() + ")");

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

				IEnumerable<ComunicacionesViewModel> comunicacion = this._serviciosComunicacion.GetAllComunicaciones().Where(x => x.Id == this.IdComunicacion);

				ComunicacionesViewModel temp = comunicacion.FirstOrDefault();

				Docs_Usuario docsFormacion = this.repoDocumentosUsuario.Single(x => x.id == id);

				documento.descripcion = docsFormacion.descripcion;

				documento.tipodocumento = docsFormacion.id_tipo_doc;

				documento.documento = docsFormacion.documento.Substring((docsFormacion.documento.IndexOf("_")) + 1); 

				return this.PartialView("~/Views/Comunicaciones/_PartialEditDocumentos.cshtml", documento);

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

			IEnumerable<Doc_Asoc> result = this._serviciosDocumentos.GetDocs(this.IdComunicacion, 4).ToList().Where(x => x.documento == nombre);

			if (result.Count() > 0)
				return this.Json(new { result = 0 });
			else
				return this.Json(new { result = 1 });
		}
		

		#endregion

		#endregion
	}
	
}