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
	#endregion

	public class ComitesController : BaseController
	{
		public ComitesController(IServiciosComites   serviciosComites,
								 ComitesRepository   repoComites,
								 IServiciosOrganismo servicioOrganismos,
								 IServiciosPuertos   servicioPuertos,
								 IServiciosUsuarios  serviciosUsuarios,
								 DocumentosUsuarioRepository repoDocumentosUsuario,
								 IServiciosDocumentos servicioDocumentos,
								 TrazasRepository     repotrazas, 
								 ILogger logger)
			
			: base(serviciosComites,
				   repoComites,
				   servicioOrganismos,
				   servicioPuertos,
				   serviciosUsuarios,
				   repoDocumentosUsuario,
				   servicioDocumentos,
				   repotrazas,logger)

		{
		}

		#region Buscador
		/// <summary>
		/// Permite buscar comites 
		/// </summary>
		public ActionResult BuscadorComites([DataSourceRequest]DataSourceRequest request)
		{
			try
			{
				if (this.UsuarioFrontalSession.Usuario.categoria == (int)EnumCategoria.Administrador
					|| this.UsuarioFrontalSession.Usuario.categoria == (int)EnumCategoria.PuertosdelEstado)
				{
					return this.Json(this._serviciosComites.GetAllComites().ToDataSourceResult(request));
				}
				return this.Json(this._serviciosComites.GetAllComites(this.UsuarioFrontalSession.ListDependenciasDepenUsuarios).ToDataSourceResult(request));
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
		/// Retorna una lista de todos los Comites
		/// </summary>
		public ActionResult ListadoComites()
		{
			try
			{
				ViewBag.Mensaje = "Reuniones de Comités Consultivos";
				return this.PartialView("ListadoComites", this.UsuarioFrontalSession);
			}
			catch (Exception ex)
			{
				this.log.PublishException(new SecureportException(ex));
				throw;
			}
		}
		#endregion

		#region CRUD
		/// <summary>
		/// Permite el Alta Gestión de Reuniones Comités Consultivos.
		/// </summary>
		[HttpPost]
		public ActionResult Create()
		{
			ComitesViewModel comites = new ComitesViewModel();

			this.comboOrganismos(null);

			if (((MultiSelectList)(this.ViewData["organismos"])).Count() == 1)
				comites.Id_Organismo = Convert.ToInt32(((SelectList)this.ViewData["organismos"]).FirstOrDefault().Value);

			this.comboPuertos(null);

			 if (((MultiSelectList)(this.ViewData["puertos"])).Count() == 1)
				comites.Id_Puerto = Convert.ToInt32(((SelectList)this.ViewData["puertos"]).FirstOrDefault().Value);

			ViewBag.disabled = true;

			ViewBag.Combo = "display:block";

			ViewBag.Texto = "display:none";

			ViewBag.display = "display:none";

			ViewBag.Mensaje = "Alta Gestión de Reuniones Comités Consultivos";

			ViewBag.action = General.AltaGeneral.ToDescription();

			comites.id = 1;

			ViewBag.ToolComite = false;

			ViewBag.alta = "display:none";

			this.ViewBag.Observaciones = string.Empty;

			return this.PartialView("~/Views/Comites/_PartialComites.cshtml", comites);
		}
		/// <summary>
		/// Visualizar los datos de los Centros 24H
		/// </summary>
		[HttpPost]
		public ActionResult VisualizarComites(int id, string Accion)
		{
			try
			{
				this.IdComite = (id != 0) ? (int)id : this.IdComite;
				
				bool valor = true;

				valor = ValidarCategoria(this.IdComite, this.repoComites.Single(b => b.id == this.IdComite));

				if (Accion != General.AltaVer.ToDescription())
				{
					if (!valor)
						return this.Json(new { result = 0, Message = ModificarCategoria.MessageCategoria.ToDescription() + "  " + this.Description });
				}

				if (Accion == General.EditGeneral.ToDescription())
				{

					ViewBag.disabled = true;
					ViewBag.Combo = "display:block";
					ViewBag.Texto = "display:none";
					ViewBag.Mensaje = "Editar Comites";
					ViewBag.ToolComite = valor;
				}
				else
				{
					ViewBag.disabled = false;
					ViewBag.Combo = "display:none";
					ViewBag.Texto = "display:block";
					ViewBag.Mensaje = "Visualizar Comites";
					ViewBag.ToolComite = valor;
				}

				ViewBag.alta = "display:block";

				ViewBag.action = General.EditGeneral.ToDescription();

				this.ViewBag.navegador = this.Browser;

				ComitesViewModel comites = new ComitesViewModel();

				this._serviciosComites.GetAllComites().Where(x => x.id == this.IdComite).ToList().ForEach(
				   p =>
				   {
					   comites.id            = p.id;
					   comites.Nivel         = p.Nivel;
					   comites.Id_Organismo  = p.Id_Organismo;
					   comites.Id_Puerto     = p.Id_Puerto;
					   comites.Convocado     = p.Convocado;
					   this.ViewBag.Observaciones = p.Observaciones;
					   comites.Nombre_Organismo = p.Nombre_Organismo;
					   comites.Nombre_Puerto = p.Nombre_Puerto;
					   comites.activoOrg = p.activoOrg;
					   comites.activoPue = p.activoPue;
				   });


				this.comboOrganismos(null);

				this.comboPuertosOrganismo(null, comites.Id_Organismo);

				return this.PartialView("~/Views/Comites/_PartialComites.cshtml", comites);
			}
			catch (Exception ex)
			{
				this.log.PublishException(new SecureportException(ex));

				return this.Json(new { result = false, Message = ex.Message });
			}
		}
		/// <summary>
		/// Alta-Editar comites.
		/// </summary>
		[HttpPost]
		public ActionResult AltaEditarComites(ComitesJson comitesJson)
		{
			try
			{
				bool result;
				DateTime FechaConvocado;

				switch (comitesJson.action)
				{

					case "Alta":
						FechaConvocado = Convert.ToDateTime(comitesJson.Convocado);
						result = _serviciosComites.ListComites(comitesJson.ID_Organismo, comitesJson.ID_Puerto, FechaConvocado);
					 if (!result)
					 {
						 AltaComite(comitesJson);                         
					 }
					 else
					 {
						 this.trazas(81, (int)this.UsuarioFrontalSession.Usuario.id, "Error al dar de alta el comite, ya existe otro comite con  el mismo organismo " + comitesJson.ID_Organismo + "y puerto " + comitesJson.ID_Puerto + " y fecha en SPMOV_Comites");
						 return this.Json(new { result = 1, Message = "Error al dar de alta el comite, ya existe otro comite con  el mismo organismo y puerto  y fecha"  });
					 }

						break;

					default:

						FechaConvocado = Convert.ToDateTime(comitesJson.Convocado);
						result = _serviciosComites.ListComites(comitesJson.ID_Organismo, comitesJson.ID_Puerto, comitesJson.Id, FechaConvocado);
						  if (!result)
						  {
							  EditarComite(comitesJson);

						  }
						  else
						  {
							  this.trazas(83, (int)this.UsuarioFrontalSession.Usuario.id, "Error al modificar un registro en SPMOV_Comites" + "(" + comitesJson.Id.ToString() + ")");
							  return this.Json(new { result = 1, Message = "Error al editar el comite, ya existe otro comite con  el mismo organismo, puerto y fecha " });
						  }
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

		/// <summary>
		/// Validar Comites
		/// </summary>
		[HttpPost]
		public ActionResult ValidarComites(int Id)
		{
			try
			{

				var retorno = ValidarCategoria(Id, this.repoComites.Single(b => b.id == Id)) ? EliminarCategoria.MessageCategoria.ToDescription() + "  " + this.Description : string.Empty;

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
		/// Borrar Comites.
		/// </summary>
		[HttpPost]
		public ActionResult EliminarComites(int Id)
		{
			try
			{
				this.repoComites.Delete(this.repoComites.Single(x => x.id == Id));

				this.trazas(84, (int)this.UsuarioFrontalSession.Usuario.id, "Se ha realizado la eliminación de un registro en SPMOV_Comites con ID" + "(" + Id.ToString() + ")");

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
		public ActionResult AltaComite(ComitesJson comitesJson)
		{
			try{
				Comites comites = new Comites();

				comites.id_usu_alta = this.UsuarioFrontalSession.Usuario.id;

				comites.fech_alta = DateTime.Now;

				comites.Observaciones = this.CambiarTexto(comitesJson.Observaciones);

				comites.Id_Organismo = comitesJson.ID_Organismo;

				comites.Id_Puerto = comitesJson.ID_Puerto;

				comites.Convocado = Convert.ToDateTime(comitesJson.Convocado);

				comites.Nivel = comitesJson.Nivel;

				this.repoComites.Create(comites);

				var Id = this.db.Comites.OrderByDescending(u => u.id).FirstOrDefault().id;
				
				this.IdComite = Id;

				this.trazas(81, (int)this.UsuarioFrontalSession.Usuario.id, "Se ha dado de alta un registro en SPMOV_Comites con ID" + "(" + Id.ToString() + ")");

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
		public ActionResult EditarComite(ComitesJson comitesJson)
		{
			try
			{
				this.IdComite = comitesJson.Id;

				Comites comite = this.repoComites.Single(x => x.id == comitesJson.Id);

				comite.Observaciones = this.CambiarTexto(comitesJson.Observaciones);
					
				comite.Convocado = DateTime.Parse(comitesJson.Convocado);

				comite.Id_Organismo = comitesJson.ID_Organismo;

				comite.Id_Puerto = comitesJson.ID_Puerto;

				comite.Nivel = comitesJson.Nivel;

				this.repoComites.Update(comite);

				this.trazas(83, (int)this.UsuarioFrontalSession.Usuario.id, "Se ha realizado la modificación de un registro en SPMOV_Comites con ID" + "(" + comitesJson.Id.ToString() + ")");


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
		public ActionResult CambiarPuerto(int? id)
		{
			try
			{

				this.comboPuertosOrganismo(null, id);

				return this.PartialView("~/Views/Comites/ComboPuerto.cshtml");

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
		   

			return PartialView("~/Views/Comites/Documentos/_PartialAsociadosEdit.cshtml", this.UsuarioFrontalSession);

		}

		public ActionResult AsociadosEditComites(int Id)
		{
			return PartialView("~/Views/Comites/Documentos/_PartialAsociadosEdit.cshtml", this.UsuarioFrontalSession);
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

				ComitesViewModel comites = new ComitesViewModel();

				this._serviciosComites.GetAllComites().Where(x => x.id == IdComite).ToList().ForEach(
				   p =>
				   {
					   comites.id = p.id;
					   comites.Nivel = p.Nivel;
					   comites.Id_Organismo = p.Id_Organismo;
					   comites.Id_Puerto = p.Id_Puerto;
					   comites.Convocado = p.Convocado;
					   this.ViewBag.Observaciones = p.Observaciones;
					   comites.Nombre_Organismo = p.Nombre_Organismo;
					   comites.Nombre_Puerto = p.Nombre_Puerto;
				   });

				return this.PartialView("~/Views/Comites/_PartialAsignarDocumentos.cshtml", comites);

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

				IEnumerable<Doc_Asoc> result = (from t1 in this._serviciosDocumentos.GetDocs(this.IdComite, 5 )
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
		public ActionResult GuardarCambios(DocumentoJson DocumentoJson)
		{
			try
			{

				this.ComboTipos_Documento();

				Docs_Usuario docsUsuario = this.repoDocumentosUsuario.Single(x => x.id == DocumentoJson.id);

				docsUsuario.descripcion = DocumentoJson.descripcion;

				docsUsuario.id_tipo_doc = DocumentoJson.tipodocumento;

				this.repoDocumentosUsuario.Update(docsUsuario);

				this.trazas(13, (int)this.UsuarioFrontalSession.Usuario.id, "Se ha dado de modificado un registro en SPCON_Depen_Usuario " + this.UsuarioFrontalSession.Usuario.login);

				return this.Json(new { result = true });

			}
			catch (Exception ex)
			{
				this.log.PublishException(new SecureportException(ex));

				return this.Json(new { result = false, Message = ex.Message });
			}

		}

		/// <summary>
		///     Permite visualizar un documento asociado.
		/// </summary>
		[HttpPost]
		public ActionResult visualizarDocumento(int? id)
		{
			try
			{

				Documento documento = new Documento();

				Docs_Usuario docsUsuario = this.repoDocumentosUsuario.Single(x => x.id == id);

				string doc = docsUsuario.documento;

				string nombreRuta =ConfigurationManager.AppSettings["Ficheros"];

				this.UsuarioFrontalSession.Path = nombreRuta + "/" + doc;

				return PartialView("~/Views/Shared/PartialDoc.cshtml", this.UsuarioFrontalSession);

			}
			catch (Exception ex)
			{
				this.log.PublishException(new SecureportException(ex));

				return this.Json(new { result = false, Message = ex.Message });
			}

		}

		public ActionResult DownloadFile()
		{
			string filename = "File.pdf";
			string filepath = AppDomain.CurrentDomain.BaseDirectory + "/Path/To/File/" + filename;
			byte[] filedata = System.IO.File.ReadAllBytes(filepath);
			string contentType = MimeMapping.GetMimeMapping(filepath);

			var cd = new System.Net.Mime.ContentDisposition
			{
				FileName = filename,
				Inline = true,
			};

			Response.AppendHeader("Content-Disposition", cd.ToString());

			return File(filedata, contentType);
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

				IEnumerable<ComitesViewModel> comites = this._serviciosComites.GetAllComites().Where(x => x.id == this.IdComite);

				ComitesViewModel temp =comites.FirstOrDefault();

				Docs_Usuario docsComite = this.repoDocumentosUsuario.Single(x => x.id == id);

				documento.Nombre = temp.Nombre_Organismo;

				documento.Apellidos = temp.Nombre_Puerto;

				documento.descripcion = docsComite.descripcion;

				documento.tipodocumento = docsComite.id_tipo_doc;

				documento.documento = docsComite.documento.Substring((docsComite.documento.IndexOf("_")) + 1);

				return this.PartialView("~/Views/Comites/_PartialEditDocumentos.cshtml", documento);

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

				this.trazas(12, (int)this.UsuarioFrontalSession.Usuario.id, "Se ha dado de borrado un registro en SPCON_Depen_Usuario " + this.UsuarioFrontalSession.Usuario.login);

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

				ComitesViewModel comites = new ComitesViewModel();

				this._serviciosComites.GetAllComites().Where(x => x.id == IdComite).ToList().ForEach(
				   p =>
				   {
					   comites.id = p.id;
					   comites.Nivel = p.Nivel;
					   comites.Id_Organismo = p.Id_Organismo;
					   comites.Id_Puerto = p.Id_Puerto;
					   comites.Convocado = p.Convocado;
					   this.ViewBag.Observaciones = p.Observaciones;
					   comites.Nombre_Organismo = p.Nombre_Organismo;
					   comites.Nombre_Puerto = p.Nombre_Puerto;
				   });

				Docs_Usuario docsUsuario = this.repoDocumentosUsuario.Single(x => x.id == id);

				documento.Nombre = comites.Nombre_Organismo;

				documento.Apellidos = comites.Nombre_Puerto;

				documento.descripcion = docsUsuario.descripcion;

				documento.tipodocumento = docsUsuario.id_tipo_doc;

				documento.documento = docsUsuario.documento.Substring((docsUsuario.documento.IndexOf("_")) + 1); 

				return this.PartialView("~/Views/Comites/_PartialEditDocumentos.cshtml", documento);

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

			IEnumerable<Doc_Asoc> result = this._serviciosDocumentos.GetDocs(this.IdComite, 5).ToList().Where(x => x.documento == nombre);
			if (result.Count() > 0)
				return this.Json(new { result = 0 });
			else
				return this.Json(new { result = 1 });
		}
		


		#endregion
	}
}