

namespace SecurePort.Web.Controllers
{
	#region using
	using System;
	using System.Collections.Generic;
	using System.Configuration;
	using System.Data.Entity.Infrastructure;
	using System.Data.Entity.ModelConfiguration;
	using System.Data.Entity.Validation;
	using System.Linq;
	using System.Transactions;
	using System.Web.Mvc;
	using Kendo.Mvc.Extensions;
	using Kendo.Mvc.UI;
	using SecurePort.Entities.Enums;
	using SecurePort.Entities.Exceptions;
	using SecurePort.Entities.Models;
	using SecurePort.Entities.Models.Json;
	using SecurePort.Services.Interfaces;
	using SecurePort.Services.Repository;
	using Incidentes = SecurePort.Entities.Models.Incidentes;

	#endregion

	public class IncidentesController : BaseController
	{
		
		public IncidentesController(IServiciosIncidentes serviciosIncidentes,
								 InstalacionesRepository repoInstalaciones,
								 IncidentesRepository repoIncidentes,
								 IServiciosTipoInstalacion serviciosInstalaciones,
								 IServiciosPuertos servicioPuertos,
								 DocumentosUsuarioRepository repoDocumentosUsuario,
								 IServiciosUsuarios servicioUsuarios,
								 IServiciosDocumentos servicioDocumentos,
								 Det_IncidenteRepository repoDet_Incidente,
								 TrazasRepository repotrazas,
								 ILogger log)
						
				: base(serviciosIncidentes,
					   repoInstalaciones,
					   repoIncidentes,
					   serviciosInstalaciones,
					   servicioPuertos,
					   repoDocumentosUsuario,                           
					   servicioUsuarios,
					   servicioDocumentos,
					   repoDet_Incidente,
					   repotrazas,log)
		{
			 
		}

		#region action
		/// <summary>
		/// Retorna una lista de todos los Incidentes.
		/// </summary>
		public ActionResult ListadoIncidentes()
		{
			try
			{
				ViewBag.UrlDelete = "/Incidentes/EliminarIncidentes";
				ViewBag.Mensaje = "Violaciones o Amenazas/Incidencias";
				return this.PartialView("ListadoIncidentes", this.UsuarioFrontalSession);
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
		/// Visualizar los Incidentes.
		/// </summary>
		public ActionResult BuscadorIncidentes([DataSourceRequest]DataSourceRequest request)
		{
			try
			{

				if (this.UsuarioFrontalSession.Usuario.categoria == (int)EnumCategoria.Administrador
					|| this.UsuarioFrontalSession.Usuario.categoria == (int)EnumCategoria.PuertosdelEstado)
				{
					return this.Json(this._serviciosIncidentes.GetAllIncidentes().ToDataSourceResult(request));
				}

				var categoria = this.UsuarioFrontalSession.Usuario.categoria;
				return this.Json(this._serviciosIncidentes.GetAllIncidentes(this.UsuarioFrontalSession.ListDependenciasDepenUsuarios,categoria).ToDataSourceResult(request));
			}
			catch (Exception ex)
			{
				this.log.PublishException(new SecureportException(ex));

				throw;
			}
		}
		#endregion

		#region CRUD

		#region Incidentes
		/// <summary>
		/// Crear una Violaciones o Amenazas/Incidencias.
		/// </summary>
		[HttpPost]
		public ActionResult Create()
		{

			IncidentesViewModel incidentes = new IncidentesViewModel();

			this.comboPuertos(null);

			if (((MultiSelectList)(this.ViewData["puertos"])).Count() == 1)
				incidentes.Id_Puerto = Convert.ToInt32(((SelectList)this.ViewData["puertos"]).FirstOrDefault().Value);

			ViewBag.disabled = true;

			ViewBag.Mensaje = "Alta Violaciones o Amenazas/Incidencias";

			ViewBag.disabled = true;

			ViewBag.alta = "display:none";

			ViewBag.action = "Alta";

			ViewBag.ToolIncidente = false;

			ViewBag.Combo = "display:block";

			ViewBag.Texto = "display:none";

			return this.PartialView("~/Views/Incidentes/Create.cshtml", incidentes);
		}

		/// <summary>
		/// Alta-Editar Violaciones o Amenazas/Incidencias.
		/// </summary>
		[HttpPost]
		public ActionResult AltaEditarIncidente(IncidentesJson incidentesJson)
		{
			try
			{
				int Resultado = 0;

				switch (incidentesJson.action)
				{
					case "Alta":
						AltaIncidentes(incidentesJson);
						Resultado = 1;
						break;

					default:
						GuardarIncidentes(incidentesJson);
						Resultado = 1;
						break;
				}

				return this.Json(new { result = Resultado });
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
		/// Alta de Incidentes.
		/// </summary>
		[HttpPost]
		public ActionResult AltaIncidentes(IncidentesJson incidentesJson)
		{
			try
			{

				Incidentes incidentes = new Incidentes();

				incidentes.Id_Puerto     = incidentesJson.Id_Puerto;

				incidentes.Tipo          = incidentesJson.Tipo;

				incidentes.Descripcion   = incidentesJson.Descripcion;

				incidentes.Fecha         = incidentesJson.Fecha;

				incidentes.ID_Usu_alta   = this.UsuarioFrontalSession.Usuario.id;

				incidentes.Fech_Alta     = DateTime.Now;

				incidentes.Observaciones = this.CambiarTexto(incidentesJson.Observaciones);

				this.repoIncidentes.Create(incidentes);

				incidentesJson.Id = this.db.Incidentes.OrderByDescending(u => u.Id).FirstOrDefault().Id;

				this.IdIncidente = incidentesJson.Id;

				this.trazas(128, (int)this.UsuarioFrontalSession.Usuario.id, "Se ha dado de alta un registro en SPMOV_Incidentes con ID" + "(" + incidentesJson.Id.ToString() + ")");

				// si el usuario es de categoria 3 y tiene una unica instlacion
				if (this.UsuarioFrontalSession.Usuario.categoria == (int)EnumCategoria.Instalacion)
				{
					IEnumerable<InstalacionesViewModel> result = new List<InstalacionesViewModel>(_serviciosIncidentes.GetInstalaciones(incidentesJson.Id_Puerto, this.UsuarioFrontalSession.Usuario.categoria, this.UsuarioFrontalSession.ListDependenciasDepenUsuarios));
					if (result.Count() == 1)
					{
						Det_Incidente Detalle = new Det_Incidente();

						Detalle.Id_Incidente = incidentesJson.Id;

						Detalle.Id_IIPP = result.FirstOrDefault().Id;

						Detalle.id_usu_alta = this.UsuarioFrontalSession.Usuario.id;

						Detalle.fech_alta = DateTime.Now;

						this.repoDet_Incidente.Create(Detalle);

						Detalle.Id = this.db.Det_Incidente.OrderByDescending(u => u.Id).FirstOrDefault().Id;

						this.trazas(128, (int)this.UsuarioFrontalSession.Usuario.id, "Se ha dado de alta un registro en SPMOV_Det_Incidente con ID" + "(" + Detalle.Id.ToString() + ")");

					}

				}


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
		/// Guardar Incidentes.
		/// </summary>
		[HttpPost]
		public ActionResult GuardarIncidentes(IncidentesJson incidentesJson)
		{
			try
			{
				Incidentes incidentes    = this.repoIncidentes.Single(x => x.Id == incidentesJson.Id);

				incidentes.Id_Puerto     = incidentesJson.Id_Puerto;

				incidentes.Tipo          = incidentesJson.Tipo;

				incidentes.Descripcion   = incidentesJson.Descripcion;

				incidentes.Observaciones = this.CambiarTexto(incidentesJson.Observaciones);

				incidentes.Fecha         = incidentesJson.Fecha;

				this.repoIncidentes.Update(incidentes);

				this.trazas(129, (int)this.UsuarioFrontalSession.Usuario.id, "Se ha dado la modificación de un registro en SPMOV_Incidentes con ID" + "(" + incidentes.Id.ToString() + ")");

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
		/// Editar incidentes.
		/// </summary>
		[HttpPost]
		public ActionResult Visualizarincidentes(int? id, string Accion)
		{
			try
			{
				bool valor = true;

				if (id != null)
					this.IdIncidente = (int)id;
				else if (this.IdIncidente != 0)
					id = this.IdIncidente;

				valor = ValidarCategoria(this.IdIncidente, this.repoIncidentes.Single(b => b.Id == this.IdIncidente));

				if (Accion != General.AltaVer.ToDescription())
				{
					if (!valor)
						return this.Json(new { result = 0, Message = ModificarCategoria.MessageCategoria.ToDescription() + "  " + this.Description });
				}

				IncidentesViewModel incidentes = new IncidentesViewModel();

				this._serviciosIncidentes.GetAllIncidentes().Where(x => x.Id == id).ToList().ForEach(
				   p =>
				   {
					   incidentes.Id            = p.Id;
					   incidentes.IIPP          = p.IIPP;
					   incidentes.Id_Puerto     = p.Id_Puerto;
					   incidentes.Puerto        = p.Puerto;
					   incidentes.Descripcion   = p.Descripcion;
					   incidentes.Tipo          = p.Tipo;
					   incidentes.NombreTipo    = p.NombreTipo;
					   incidentes.Fecha         = p.Fecha;
					   ViewBag.Observaciones    = p.Observaciones;

				   });

				ViewBag.alta = "display:block";

				this.comboPuertos(null);

				if (Accion == General.EditGeneral.ToDescription())
				{
					ViewBag.disabled = true;

					ViewBag.action = "Edit";

					ViewBag.Mensaje = "Editar Violaciones o Amenazas/Incidencias";

					ViewBag.Combo = "display:block";

					ViewBag.Texto = "display:none";

					ViewBag.ToolIncidente = valor;

				}
				else
				{
					ViewBag.disabled = false;

					ViewBag.action = "Ver";

					ViewBag.Mensaje = "Visualizar Violaciones o Amenazas/Incidencias";

					ViewBag.Combo = "display:none";

					ViewBag.Texto = "display:block";

					ViewBag.ToolIncidente = valor;

				}

				this.ViewBag.navegador = this.Browser;

				LimpiarColeccionesLocal();

				return this.PartialView("~/Views/Incidentes/Create.cshtml", incidentes);
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
		public ActionResult Validarincidentes(int Id)
		{
			try
			{

				var retorno = ValidarCategoria(Id, this.repoIncidentes.Single(b => b.Id == Id)) ? EliminarCategoria.MessageCategoria.ToDescription() + "  " + this.Description : string.Empty;

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
		/// Borrar incidentes.
		/// </summary>
		[HttpPost]
		public ActionResult Eliminarincidentes(int Id)
		{
			try
			{
	
					var Id_Usu = this.repoIncidentes.Single(x => x.Id == Id).ID_Usu_alta;

					IEnumerable<Det_Incidente> Det_Incidente = this.repoDet_Incidente.GetAll().Where(x => x.Id_Incidente == Id).ToList();

					IEnumerable<Docs_Usuario> Docs_Usuario = this.repoDocumentosUsuario.GetAll().Where(x => x.id_servicio == (int)EnumTiposServicio.Incidentes && x.id_usu_alta == Id_Usu);

					using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required,
										new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted }))
					{
						//Hijos
						Det_Incidente.ToList().ForEach(p => { this.repoDet_Incidente.Delete(p); });

						//Cabecera
						this.repoIncidentes.Delete(this.repoIncidentes.Single(x => x.Id == Id));

						Docs_Usuario.ToList().ForEach(
							z =>
							{
								this.repoDocumentosUsuario.Delete(z);
								BorrarFichero(ConfigurationManager.AppSettings["RutaSecurePorDoc"].ToString() + "/" + z.documento);
							});

						this.trazas(130, (int)this.UsuarioFrontalSession.Usuario.id, "Se ha realizado la eliminación de un registro en SPMOV_Incidentes con ID" + "(" + Id.ToString() + ")");

						scope.Complete();

					}
				
			}
			catch (DbUpdateException ex)
			{
				this.log.PublishException(new SecureportExceptionDbUpdate(ex));

				return this.Json(new { result = false, Message = MessageDelete(ex) });
			}
			catch (ModelValidationException MessageError)
			{
				this.log.PublishException(new SecureportExceptionModelValidation(MessageError));

				return this.Json(new { result = false, Message = MessageError.ToString() });
			}
			catch (DbEntityValidationException MessageError)
			{
				this.log.PublishException(new SecureportExceptionDbEntity(MessageError));

				return this.Json(new { result = false, Message = MessageError.ToString() });
			}
			catch (Exception MessageError)
			{
				this.log.PublishException(new SecureportException(MessageError));

				return this.Json(new { result = false, Message = MessageError.ToString() });
			}
				return this.Json(new { result = true });
		}
		#endregion

		#region Documentos

		public ActionResult AsociadosEdit(bool ToolBar)
		{
			this.UsuarioFrontalSession.valor = ToolBar;

			return PartialView("~/Views/Incidentes/Documentos/_PartialAsociadosEdit.cshtml", this.UsuarioFrontalSession);

		}

		public ActionResult AsociadosEditIncidentes(int Id)
		{
			return PartialView("~/Views/Incidentes/Documentos/_PartialAsociadosEdit.cshtml", this.UsuarioFrontalSession);
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

				IncidentesViewModel incidente = new IncidentesViewModel();

				this._serviciosIncidentes.GetAllIncidentes().Where(x => x.Id == this.IdIncidente).ToList().ForEach(
				   p =>
				   {
					   incidente.Id = p.Id;
					   incidente.Descripcion = p.Descripcion;

				   });

				return this.PartialView("~/Views/Incidentes/_PartialAsignarDocumentos.cshtml", incidente);

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

				IEnumerable<Doc_Asoc> result = (from t1 in this._serviciosDocumentos.GetDocs(this.IdIncidente, 8)
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

				this.trazas(134, (int)this.UsuarioFrontalSession.Usuario.id, "Se ha dado de modificado un registro en SPMOV_Documentos_SP " + this.UsuarioFrontalSession.Usuario.login);

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

				IEnumerable<IncidentesViewModel> incidente = this._serviciosIncidentes.GetAllIncidentes().Where(x => x.Id == this.IdIncidente);

				IncidentesViewModel temp = incidente.FirstOrDefault();

				Docs_Usuario docsInstalacion = this.repoDocumentosUsuario.Single(x => x.id == id);

				documento.Nombre = temp.Descripcion;

				documento.Apellidos = temp.Descripcion;

				documento.descripcion = docsInstalacion.descripcion;

				documento.tipodocumento = docsInstalacion.id_tipo_doc;

				documento.documento = docsInstalacion.documento.Substring((docsInstalacion.documento.IndexOf("_")) + 1);

				return this.PartialView("~/Views/Incidentes/_PartialEditDocumentos.cshtml", documento);

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

				this.trazas(133, (int)this.UsuarioFrontalSession.Usuario.id, "Se ha dado de borrado un registro en SPMOV_Documentos_SP " + this.UsuarioFrontalSession.Usuario.login);

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

				IEnumerable<IncidentesViewModel> incidente = this._serviciosIncidentes.GetAllIncidentes().Where(x => x.Id == this.IdIncidente);

				IncidentesViewModel temp = incidente.FirstOrDefault();

				Docs_Usuario docsFormacion = this.repoDocumentosUsuario.Single(x => x.id == id);

				documento.Nombre = temp.Descripcion;

				documento.Apellidos = temp.Descripcion;

				documento.descripcion = docsFormacion.descripcion;

				documento.tipodocumento = docsFormacion.id_tipo_doc;

				documento.documento = docsFormacion.documento.Substring((docsFormacion.documento.IndexOf("_")) + 1); 


				return this.PartialView("~/Views/Incidentes/_PartialEditDocumentos.cshtml", documento);

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

			IEnumerable<Doc_Asoc> result = this._serviciosDocumentos.GetDocs(this.IdIncidente, 8).ToList().Where(x => x.documento == nombre);
			if (result.Count() > 0)
				return this.Json(new { result = 0 });
			else
				return this.Json(new { result = 1 });
		}
		

		#endregion

		#region Instalaciones

		public ActionResult InstalacionVisualizarAcciones()
		{
			this.listaAcciones = new List<AccionesViewModel>();

			IEnumerable<AccionesViewModel> accionesViewModel = this._serviciosUsuarios.GetAcciones().ToList();

			AccionesViewModel acciones0 = new AccionesViewModel()
			{
				Id = 0,
				Name = "Seleccionar Opción"
			};
			this.listaAcciones.Add(acciones0);

			AccionesViewModel acciones = new AccionesViewModel()
			{
				Id = 1,
				Name = "Asignar Instalaciones"
			};

			this.listaAcciones.Add(acciones);

			return Json(this.listaAcciones, JsonRequestBehavior.AllowGet);

		}

		/// <summary>
		/// Permite visualizar los datos de un usuario con dependencias.
		/// </summary>
		[HttpPost]
		public ActionResult AsignarInstalacion()
		{
			try
			{

				Incidentes incidente = this.repoIncidentes.Single(x => x.Id == this.IdIncidente);

				IEnumerable<InstalacionViewModel> result;
				if (this.UsuarioFrontalSession.Usuario.categoria == (int)EnumCategoria.Administrador
					|| this.UsuarioFrontalSession.Usuario.categoria == (int)EnumCategoria.PuertosdelEstado)
				{
					result = new List<InstalacionViewModel>(this._serviciosTipoInstalacion.GetAllInstalaciones().Where(x => x.activado == "Si" && x.Id_puerto == incidente.Id_Puerto).ToList());
				}
				else
				{
					var categoria = this.UsuarioFrontalSession.Usuario.categoria;
					result = new List<InstalacionViewModel>(this._serviciosTipoInstalacion.GetAllInstalaciones(this.UsuarioFrontalSession.ListDependenciasDepenUsuarios, categoria).Where(x => x.activado == "Si" && x.Id_puerto == incidente.Id_Puerto).ToList());
				}


				this.listaInstalacionSession = (from a1 in result
												select new MOV_Instalaciones
												{
													Id = a1.Id,
													Nombre = a1.Nombre
												}).ToList();


				this.listaInstalacionAsignadas = (from t1 in this._serviciosIncidentes.GetInstalacionesbyIncidente(this.IdIncidente)
												  join b1 in result.ToList() on t1.Id_IIPP equals b1.Id
												  select new MOV_Instalaciones
												  {
													  Id = t1.Id_IIPP,
													  Nombre = t1.IIPP
												  }).ToList();





				this.listaInstalacionSession.ToList().ForEach(
					p => this.listaInstalacionAsignadas.ToList().ForEach(
						t =>
						{
							if (p.Id == t.Id)
							{
								this.listaInstalacionSession.Remove(p);
							}
						}));


				return this.PartialView("~/Views/Incidentes/_PartialAsignarInstalacion.cshtml", incidente);

			}
			catch (Exception ex)
			{
				this.log.PublishException(new SecureportException(ex));

				return this.Json(new { result = false, Message = ex.Message });
			}
		}

		public ActionResult InstAsociadosEdit(int Id)
		{
			return PartialView("~/Views/Incidentes/_PartialAsociadosInst.cshtml", this.UsuarioFrontalSession);
		}


		public ActionResult BuscadorInstalaciones([DataSourceRequest]DataSourceRequest request)
		{
			try
			{
				Incidentes incidente = this.repoIncidentes.Single(x => x.Id == this.IdIncidente);
				IEnumerable<InstalacionViewModel> result;
				IEnumerable<Det_IncidenteViewModel> Nivel_detalle = new List<Det_IncidenteViewModel>();
				if (this.IdIncidente != 0)
				{
					if (this.UsuarioFrontalSession.Usuario.categoria == (int)EnumCategoria.Administrador
						|| this.UsuarioFrontalSession.Usuario.categoria == (int)EnumCategoria.PuertosdelEstado)
					{
						result = new List<InstalacionViewModel>(this._serviciosTipoInstalacion.GetAllInstalaciones().Where(x => x.activado == "Si" && x.Id_puerto == incidente.Id_Puerto).ToList());
					}
					else
					{
						var categoria = this.UsuarioFrontalSession.Usuario.categoria;
						result = new List<InstalacionViewModel>(this._serviciosTipoInstalacion.GetAllInstalaciones(this.UsuarioFrontalSession.ListDependenciasDepenUsuarios, categoria).Where(x => x.activado == "Si" && x.Id_puerto == incidente.Id_Puerto).ToList());
					}

					Nivel_detalle = (from t1 in this._serviciosIncidentes.GetInstalacionesbyIncidente(this.IdIncidente)
									 join b1 in result.ToList() on t1.Id_IIPP equals b1.Id
									 select new Det_IncidenteViewModel
									 {
										 Id = t1.Id,
										 IIPP = t1.IIPP

									 }).OrderBy(x=> x.IIPP);

				}

				return this.Json(Nivel_detalle.ToDataSourceResult(request));
			}
			catch (Exception ex)
			{
				this.log.PublishException(new SecureportException(ex));
				throw;
			}
		}


		public ActionResult InstalacionesDisponiblesIncidentes([DataSourceRequest]DataSourceRequest request)
		{
			try
			{

				if (this.listaInstalacionAsignadas.Any())
				{
					this.listaInstalacionSession.ToList().ForEach(
						p => this.listaInstalacionAsignadas.ToList().ForEach(
							t =>
							{
								if (p.Id == t.Id && p.id_Puerto == t.id_Puerto)
								{
									this.listaInstalacionSession.Remove(p);
								}
							}));
				}


				IEnumerable<InstalacionesViewModel> result = (from t1 in this.listaInstalacionSession
															  orderby t1.Id
															  select
																  new InstalacionesViewModel
																  {
																	  Id = t1.Id,
																	  Nombre = t1.Nombre,
																	  asignar = false
																  }).OrderBy(x=> x.Nombre);
				return Json(result.ToDataSourceResult(request));

			}
			catch (Exception ex)
			{
				this.log.PublishException(new SecureportException(ex));

				return this.Json(new { result = false, Message = ex.Message });
			}
		}

		public ActionResult InstalacionesAsociadosIncidentes([DataSourceRequest]DataSourceRequest request)
		{

			try
			{

				IEnumerable<InstalacionesAsociadosViewModel> result = (from t1 in this.listaInstalacionAsignadas
																	   select
																		   new InstalacionesAsociadosViewModel
																		   {
																			   Id = t1.Id,
																			   Nombre = t1.Nombre,
																			   asignar = false
																		   }).OrderBy(x=> x.Nombre);


				return Json(result.ToDataSourceResult(request));
			}
			catch (Exception ex)
			{
				this.log.PublishException(new SecureportException(ex));

				return this.Json(new { result = false, Message = ex.Message });
			}
		}

		[HttpPost]
		public ActionResult ActualizarInstalacionAsignada()
		{
			try
			{

				return this.PartialView("~/Views/Incidentes/Instalacion/_PartialInstalacionesAsociadas.cshtml");

			}
			catch (Exception ex)
			{
				this.log.PublishException(new SecureportException(ex));

				return this.Json(new { result = false, Message = ex.Message });
			}

		}

		[HttpPost]
		public ActionResult ActualizarInstalacion(int id)
		{
			try
			{

				this.listaInstalacionSession.ToList().ForEach(
					p =>
					{
						if (p.Id == id)
						{
							this.listaInstalacionSession.Remove(p);
							this.listaInstalacionAsignadas.Add(p);
						}
					});
				return this.PartialView("~/Views/Incidentes/Instalacion/_PartialInstalacionDisponible.cshtml");

			}
			catch (Exception ex)
			{
				this.log.PublishException(new SecureportException(ex));

				return this.Json(new { result = false, Message = ex.Message });
			}

		}

		[HttpPost]
		public ActionResult QuitarInstalacion(int id)
		{
			try
			{
				this.listaInstalacionAsignadas.ToList().ForEach(
					p =>
					{
						if (p.Id == id)
						{
							this.listaInstalacionAsignadas.Remove(p);
							this.listaInstalacionSession.Add(p);
						}
					});

				return this.PartialView("~/Views/Incidentes/Instalacion/_PartialInstalacionDisponible.cshtml");
			}
			catch (Exception ex)
			{
				this.log.PublishException(new SecureportException(ex));

				return this.Json(new { result = false, Message = ex.Message });
			}

		}

		[HttpPost]
		public ActionResult GuardarIncidenteInstalaciones()
		{
			try
			{
				//Asignar Instlaciones
				this.listaInstalacionAsignadas.ToList().ForEach(
					p =>
					{

						if (this.repoDet_Incidente.Single(x => x.Id_Incidente == this.IdIncidente && x.Id_IIPP == p.Id) == null)
						{
							Det_Incidente depenUsuario = new Det_Incidente()
							{
								Id_Incidente = this.IdIncidente,
								Id_IIPP = p.Id,
								id_usu_alta = this.UsuarioFrontalSession.Usuario.id,
								fech_alta = DateTime.Now
							};

							this.repoDet_Incidente.Create(depenUsuario);
						}
					});

				//Desasignar Instalaciones
				this.listaInstalacionSession.ToList().ForEach(
				  p =>
				  {
					  Det_Incidente depen_Usuario = this.repoDet_Incidente.Single(x => x.Id_Incidente == this.IdIncidente && x.Id_IIPP == p.Id);

					  if (depen_Usuario != null)
					  {
						  this.repoDet_Incidente.Delete(depen_Usuario);

						  this.log.WriteInfo("Baja Dependencia", this.UsuarioFrontalSession.Usuario.nombre + '|' + this.UsuarioFrontalSession.Usuario.apellidos + '|' + this.UsuarioFrontalSession.Usuario.id + '|' + this.UsuarioFrontalSession.Usuario.login);
					  }
				  });




				return this.Json(new { result = true });
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
		public ActionResult DeshacerIncidenteInstalaciones()
		{
			try
			{

				Incidentes incidentes = this.repoIncidentes.Single(x => x.Id == this.IdIncidente);
				IEnumerable<InstalacionViewModel> result;
				if (this.UsuarioFrontalSession.Usuario.categoria == (int)EnumCategoria.Administrador
					|| this.UsuarioFrontalSession.Usuario.categoria == (int)EnumCategoria.PuertosdelEstado)
				{
					result = new List<InstalacionViewModel>(this._serviciosTipoInstalacion.GetAllInstalaciones().Where(x => x.activado == "Si" && x.Id_puerto == incidentes.Id_Puerto).ToList());
				}
				else
				{
					var categoria = this.UsuarioFrontalSession.Usuario.categoria;
					result = new List<InstalacionViewModel>(this._serviciosTipoInstalacion.GetAllInstalaciones(this.UsuarioFrontalSession.ListDependenciasDepenUsuarios, categoria).Where(x => x.activado == "Si" && x.Id_puerto == incidentes.Id_Puerto).ToList());
				}


				this.listaInstalacionSession = (from a1 in result
												select new MOV_Instalaciones
												{
													Id = a1.Id,
													Nombre = a1.Nombre
												}).ToList();


				this.listaInstalacionAsignadas = (from t1 in this._serviciosIncidentes.GetInstalacionesbyIncidente(this.IdIncidente)
												  join b1 in result.ToList() on t1.Id_IIPP equals b1.Id
												  select new MOV_Instalaciones
												  {
													  Id = t1.Id_IIPP,
													  Nombre = t1.IIPP
												  }).ToList();

				// quita los que ya han sido asignados
				this.listaInstalacionSession.ToList().ForEach(
				   p => this.listaInstalacionAsignadas.ToList().ForEach(
					   t =>
					   {
						   if (p.Id == t.Id)
						   {
							   this.listaInstalacionSession.Remove(p);
						   }
					   }));


				return this.PartialView("~/Views/Incidentes/Instalacion/_PartialInstalacionDisponible.cshtml");

			}
			catch (Exception ex)
			{
				this.log.PublishException(new SecureportException(ex));

				return this.Json(new { result = false, Message = ex.Message });
			}
		}

		[HttpPost]
		public ActionResult VolverIncidenteInstalacion()
		{
			try
			{
				return this.PartialView("~/Views/Incidentes/List.cshtml");
			}
			catch (Exception ex)
			{
				this.log.PublishException(new SecureportException(ex));

				return this.Json(new { result = false, Message = ex.Message });
			}

		}

		private void LimpiarColeccionesLocal()
		{

			try
			{
				this.listaInstalacionSession = null;
				this.listaInstalacionAsignadas = null;

			}
			catch (Exception ex)
			{
				this.log.PublishException(new SecureportException(ex));

			}

		}

		#endregion

		#endregion
	}
}