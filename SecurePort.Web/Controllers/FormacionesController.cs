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
	using System.IO;
	#endregion
	
	public class FormacionesController : BaseController
	{
		
		public FormacionesController(IServiciosFormaciones serviciosFormaciones,
									 IServiciosOrganismo serviciosOrganismos,						 
									 InstalacionesRepository repoInstalaciones,
									 PuertosRepository repoPuertos,
									 FormacionesRepository repoFormaciones,
									 IServiciosTipoInstalacion serviciosInstalaciones,
									 IServiciosPuertos servicioPuertos,
									 IServiciosUsuarios servicioUsuarios,
									 DocumentosUsuarioRepository repoDocumentosUsuario,
									 IServiciosDocumentos servicioDocumentos,
									 TrazasRepository repotrazas,
									 ILogger log)
					: base(serviciosFormaciones, 
						   serviciosOrganismos,
						   repoInstalaciones, 
						   repoPuertos, 
						   repoFormaciones, 
						   serviciosInstalaciones,
						   servicioPuertos,
						   servicioUsuarios, 
						   repoDocumentosUsuario, 
						   servicioDocumentos,
						   repotrazas, log)
		{
			 
		}

		#region Buscador
		/// <summary>
		/// Permite buscar las formaciones del personal
		/// </summary>
		public ActionResult BuscadorFormaciones([DataSourceRequest]DataSourceRequest request)
		{
			try
			{
				if (this.UsuarioFrontalSession.Usuario.categoria == (int)EnumCategoria.Administrador
					|| this.UsuarioFrontalSession.Usuario.categoria == (int)EnumCategoria.PuertosdelEstado)
				{
					return this.Json(this._serviciosFormaciones.GetAllFormaciones().ToDataSourceResult(request));
				}

				var categoria = this.UsuarioFrontalSession.Usuario.categoria;
				return this.Json(this._serviciosFormaciones.GetAllFormaciones(this.UsuarioFrontalSession.ListDependenciasDepenUsuarios, categoria).ToDataSourceResult(request));
			}
			catch (Exception ex)
			{
				this.log.PublishException(new SecureportException(ex));
				throw;
			}
		}
		#endregion

		#region CRUD

		#region Formaciones
		/// <summary>
		/// Retorna una lista de todas las formaciones.
		/// </summary>
		public ActionResult ListadoFormaciones()
		{
			try
			{
				ViewBag.UrlDelete = "/Formaciones/EliminarFormaciones";
				ViewBag.Mensaje = "Formación del personal";
				return this.PartialView("ListadoFormaciones", this.UsuarioFrontalSession);
			}
			catch (Exception ex)
			{
				this.log.PublishException(new SecureportException(ex));
				throw;
			}
		}
		/// <summary>
		/// Crear una Formación.
		/// </summary>
		[HttpPost]
		public ActionResult Create()
		{

			FormacionesViewModel formaciones = new FormacionesViewModel();

			this.comboPuertosOrganismo(null, null);

			if (((MultiSelectList)(this.ViewData["puertos"])).Count() == 1)
				formaciones.Id_Puerto = Convert.ToInt32(((SelectList)this.ViewData["puertos"]).FirstOrDefault().Value);

			this.comboOrganismos(null);

			if (((MultiSelectList)(this.ViewData["organismos"])).Count() == 1)
				formaciones.Id_Organismo = Convert.ToInt32(((SelectList)this.ViewData["organismos"]).FirstOrDefault().Value);


			this.comboInstalacionesPuertos(null, null);

			ViewBag.disabled = true;

			ViewBag.Mensaje = "Alta Formación del Personal";

			ViewBag.disabled = true;

			ViewBag.alta = "display:none";

			ViewBag.action = "Alta";

			ViewBag.ToolFormacion = false;

			ViewBag.Combo = "display:block";

			ViewBag.Texto = "display:none";

			return this.PartialView("~/Views/Formaciones/Create.cshtml", formaciones);
		}

		  /// <summary>
		/// Alta-Editar Amenazas.
		/// </summary>
		[HttpPost]
		public ActionResult AltaEditarFormacion(FormacionesJson formacionesJson)
		{
			try
			{
				 int resultado = 0;

				switch (formacionesJson.action)
				{

					case "Alta":
						
						 AltaFormacion(formacionesJson);
						 resultado = 1;
						break;

					default:
						GuardarFormacion(formacionesJson);
						resultado = 1;
						break;
				}

				return this.Json(new { result = resultado });
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
		/// Alta de Formaciones.
		/// </summary>
		[HttpPost]
		public ActionResult AltaFormacion(FormacionesJson formacionesJson)
		{
			try
			{
				Formaciones formaciones  = new Formaciones();

				formaciones.Id_IIPP      = formacionesJson.Id_IIPP;

				formaciones.Id_Organismo = formacionesJson.Id_Organismo;

				formaciones.Id_Puerto    = formacionesJson.Id_Puerto;

				formaciones.Inicio       = Convert.ToDateTime(formacionesJson.Inicio);

				formaciones.Fin          = Convert.ToDateTime(formacionesJson.Fin);

				formaciones.Lugar        = formacionesJson.Lugar;

				formaciones.Entidad      = formacionesJson.Entidad;

				formaciones.Titulo       = formacionesJson.Titulo;

				formaciones.Duracion     = formacionesJson.Duracion;

				formaciones.Observaciones = this.CambiarTexto(formacionesJson.Observaciones); 
				
				formaciones.ID_Usu_Alta   = this.UsuarioFrontalSession.Usuario.id;

				formaciones.Fech_Alta     = DateTime.Now;

				this.repoFormaciones.Create(formaciones);

				formacionesJson.Id = this.db.Formaciones.OrderByDescending(u => u.Id).FirstOrDefault().Id;

				this.trazas(111, (int)this.UsuarioFrontalSession.Usuario.id, "Se ha dado de alta un registro en SPMOV_Formaciones con ID" + "(" + formacionesJson.Id.ToString() + ")");

				this.IdFormacion = formacionesJson.Id;

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
		/// Guardar Formaciones.
		/// </summary>
		[HttpPost]
		public ActionResult GuardarFormacion(FormacionesJson formacionesJson)
		{
			try
			{
				Formaciones formaciones = repoFormaciones.Single(x => x.Id == formacionesJson.Id);

				formaciones.Id_IIPP       = formacionesJson.Id_IIPP;

				formaciones.Id_Organismo  = formacionesJson.Id_Organismo;

				formaciones.Id_Puerto     = formacionesJson.Id_Puerto;

				formaciones.Inicio        = Convert.ToDateTime(formacionesJson.Inicio);

				formaciones.Fin           = Convert.ToDateTime(formacionesJson.Fin);

				formaciones.Lugar         = formacionesJson.Lugar;

				formaciones.Entidad       = formacionesJson.Entidad;

				formaciones.Titulo        = formacionesJson.Titulo;

				formaciones.Duracion      = formacionesJson.Duracion;

				formaciones.Observaciones = this.CambiarTexto(formacionesJson.Observaciones);
				
				this.repoFormaciones.Update(formaciones);

				this.trazas(112, (int)this.UsuarioFrontalSession.Usuario.id, "Se ha dado modificado un registro en SPMOV_Formaciones con ID " +"(" +formacionesJson.Id.ToString()+")");

				this.IdFormacion = formacionesJson.Id;

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
		/// Editar Formaciones.
		/// </summary>
		[HttpPost]
		public ActionResult VisualizarFormacion(int? id, string Accion )
		{
			try
			{

				bool valor = true;
				if (id != null)
					this.IdFormacion = (int)id;
				else if (this.IdFormacion != 0)
					id = this.IdFormacion;

				valor = ValidarCategoria(this.IdFormacion, this.repoFormaciones.Single(b => b.Id == this.IdFormacion));

				if (Accion != General.AltaVer.ToDescription())
				{
					if (!valor)
						return this.Json(new { result = 0, Message = ModificarCategoria.MessageCategoria.ToDescription() + "  " + this.Description });
				}
				
				
				FormacionesViewModel formaciones = new FormacionesViewModel();

				this._serviciosFormaciones.GetAllFormaciones().Where(x => x.Id == id).ToList().ForEach(
				   p =>
				   {
					   formaciones.Id = p.Id;
					   formaciones.Id_Organismo = p.Id_Organismo;
					   formaciones.Organismo  = p.Organismo;
					   formaciones.Id_Puerto = p.Id_Puerto;
					   formaciones.Puerto = p.Puerto;
					   formaciones.Id_IIPP = p.Id_IIPP;
					   formaciones.IIPP = p.IIPP;
					   formaciones.Titulo = p.Titulo;
					   formaciones.Inicio = p.Inicio;
					   formaciones.Fin = p.Fin;
					   formaciones.Duracion = p.Duracion;
					   formaciones.Entidad = p.Entidad;
					   formaciones.Lugar = p.Lugar;                       
					   this.ViewBag.Observaciones = p.Observaciones;

				   });

			   

				ViewBag.alta = "display:block";

				this.comboOrganismos(null);
				
				this.comboPuertosOrganismo(null, formaciones.Id_Organismo);

				this.comboInstalacionesPuertos(null, formaciones.Id_Puerto);

				if (Accion == General.EditGeneral.ToDescription())
				{
					ViewBag.disabled = true;

					ViewBag.action = "Edit";

					ViewBag.Mensaje = "Editar Formación del Personal";

					ViewBag.Combo = "display:block";

					ViewBag.Texto = "display:none";

					ViewBag.ToolFormacion = valor;

				}
				else
				{
					ViewBag.disabled = false;

					ViewBag.action = "Ver";

					ViewBag.Mensaje = "Visualizar Formación del Personal";

					ViewBag.Combo = "display:none";

					ViewBag.Texto = "display:block";

					ViewBag.ToolFormacion = valor;

				}
				this.ViewBag.navegador = this.Browser;


				return this.PartialView("~/Views/Formaciones/Create.cshtml", formaciones);
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
		public ActionResult ValidarFormaciones(int Id)
		{
			try
			{

				var retorno = ValidarCategoria(Id, this.repoFormaciones.Single(x => x.Id == Id)) ? EliminarCategoria.MessageCategoria.ToDescription() + "  " + this.Description : string.Empty;

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
		/// Borrar Formaciones.
		/// </summary>
		[HttpPost]
		public ActionResult EliminarFormaciones(int Id)
		{
			try
			{

				this.repoFormaciones.Delete(this.repoFormaciones.Single(x => x.Id == Id));

				this.trazas(113, (int)this.UsuarioFrontalSession.Usuario.id, "Se ha realizado la eliminación de un registro en SPMOV_Formacionescon con ID " + "(" + Id.ToString() + ")");
				

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

				this.comboPuertosOrganismo(null, id);

				return this.PartialView("~/Views/Formaciones/ComboPuerto.cshtml");

			}
			catch (Exception ex)
			{
				this.log.PublishException(new SecureportException(ex));

				return this.Json(new { result = false, Message = ex.Message });
			}

		}

		[HttpPost]
		public ActionResult CambiarCentro(int? id)
		{
			try
			{

				this.comboInstalacionesPuertos(null, id);

				return this.PartialView("~/Views/Formaciones/ComboCentro.cshtml");

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

			return PartialView("~/Views/Formaciones/Documentos/_PartialAsociadosEdit.cshtml", this.UsuarioFrontalSession);

		}

		public ActionResult AsociadosEditFormaciones(int Id)
		{
			return PartialView("~/Views/Formaciones/Documentos/_PartialAsociadosEdit.cshtml", this.UsuarioFrontalSession);
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

				FormacionesViewModel formacion = new FormacionesViewModel();

				this._serviciosFormaciones.GetAllFormaciones().Where(x=> x.Id == this.IdFormacion).ToList().ForEach(
				   p =>
				   {
					   formacion.Id = p.Id;
					   formacion.Organismo = p.Organismo;
					   formacion.Puerto = p.Puerto;

				   });

				return this.PartialView("~/Views/Formaciones/_PartialAsignarDocumentos.cshtml", formacion);

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

				IEnumerable<Doc_Asoc> result = (from t1 in this._serviciosDocumentos.GetDocs(this.IdFormacion, 7)
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

				this.trazas(117, (int)this.UsuarioFrontalSession.Usuario.id, "Se ha dado de modificado un registro en SPCON_Depen_Usuario con ID " + "(" + DocumentoJson.id.ToString() + ")");

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

				IEnumerable<FormacionesViewModel> formacion = this._serviciosFormaciones.GetAllFormaciones().Where(x => x.Id == this.IdFormacion);

				FormacionesViewModel temp = formacion.FirstOrDefault();

				Docs_Usuario docsInstalacion = this.repoDocumentosUsuario.Single(x => x.id == id);

				documento.Nombre = temp.Organismo;

				documento.Apellidos = temp.Puerto;

				documento.descripcion = docsInstalacion.descripcion;

				documento.tipodocumento = docsInstalacion.id_tipo_doc;

				documento.documento = docsInstalacion.documento.Substring((docsInstalacion.documento.IndexOf("_")) + 1);

				return this.PartialView("~/Views/Formaciones/_PartialEditDocumentos.cshtml", documento);

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

				this.trazas(116, (int)this.UsuarioFrontalSession.Usuario.id, "Se ha dado de borrado un registro en SPMOV_Documentos_SP con ID " + "(" + id.ToString() + ")");

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

				IEnumerable<FormacionesViewModel> formacion = this._serviciosFormaciones.GetAllFormaciones().Where(x => x.Id == this.IdFormacion);

				FormacionesViewModel temp = formacion.FirstOrDefault();

				Docs_Usuario docsFormacion = this.repoDocumentosUsuario.Single(x => x.id == id);

				documento.Nombre = temp.Organismo;

				documento.Apellidos = temp.Puerto;

				documento.descripcion = docsFormacion.descripcion;

				documento.tipodocumento = docsFormacion.id_tipo_doc;

				documento.documento = docsFormacion.documento.Substring((docsFormacion.documento.IndexOf("_")) + 1); 
				
				return this.PartialView("~/Views/Formaciones/_PartialEditDocumentos.cshtml", documento);

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

			IEnumerable<Doc_Asoc> result = this._serviciosDocumentos.GetDocs(this.IdFormacion, 7).ToList().Where(x => x.documento == nombre);
			if (result.Count() > 0)
				return this.Json(new { result = 0 });
			else
				return this.Json(new { result = 1 });
		}
		

		#endregion

		#endregion
	}
	
}