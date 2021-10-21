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
	#endregion
	
	public class MantenimientoController : BaseController
	{
		
		public MantenimientoController(IServiciosMantenimiento serviciosMantenimiento,
									   MantenimientosRepository repoMantenimiento,
									   InstalacionesRepository repoInstalaciones,
									   PuertosRepository repoPuertos,
									   IServiciosPuertos servicioPuertos,
									   IServiciosUsuarios servicioUsuarios,
									   DocumentosUsuarioRepository repoDocumentosUsuario,
									   IServiciosDocumentos servicioDocumentos,
									   IServiciosTipoInstalacion serviciosInstalaciones,
									   TrazasRepository repoTrazas,
									   ILogger log)

			: base(serviciosMantenimiento,
				   repoMantenimiento,
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
		/// Permite buscar los Mantenimientos, Calibración y Ensayos
		/// </summary>
		public ActionResult BuscadorMantenimiento([DataSourceRequest]DataSourceRequest request)
		{
			try
			{
				if (this.UsuarioFrontalSession.Usuario.categoria == (int)EnumCategoria.Administrador || this.UsuarioFrontalSession.Usuario.categoria == (int)EnumCategoria.PuertosdelEstado)
				{
					return this.Json(this._serviciosMantenimiento.GetAllMantenimiento().ToDataSourceResult(request));
				}

				var categoria = this.UsuarioFrontalSession.Usuario.categoria;
				return this.Json(this._serviciosMantenimiento.GetAllMantenimiento(this.UsuarioFrontalSession.ListDependenciasDepenUsuarios,categoria).ToDataSourceResult(request));
			}
			catch (Exception ex)
			{
				this.log.PublishException(new SecureportException(ex));
				throw;
			}
		}
		#endregion

		#region CRUD

		#region Mantenimientos
		/// <summary>
		/// Retorna una lista de todas los Mantenimientos, Calibración y Ensayos.
		/// </summary>
		public ActionResult ListadoMantenimientos()
		{
			try
			{
				ViewBag.UrlDelete = "/Mantenimiento/EliminarMantenimiento";
				ViewBag.Mensaje = "Mantenimiento, Calibración y Ensayos";
				return this.PartialView("ListadoMantenimientos", this.UsuarioFrontalSession);
			}
			catch (Exception ex)
			{
				this.log.PublishException(new SecureportException(ex));
				throw;
			}
		}
		/// <summary>
		/// Crear un Mantenimientos, Calibración y Ensayos.
		/// </summary>
		[HttpPost]
		public ActionResult Create()
		{

			MantenimientoViewModel mantenimiento = new MantenimientoViewModel();

			mantenimiento.Fecha = DateTime.MinValue;

			this.comboPuertos(null);

			if (((MultiSelectList)(this.ViewData["puertos"])).Count() == 1)
				mantenimiento.Id_Puerto = Convert.ToInt32(((SelectList)this.ViewData["puertos"]).FirstOrDefault().Value);

			this.comboInstalacionesPuertos(null, null);

			ViewBag.disabled = true;

			ViewBag.Mensaje = "Alta Mantenimiento, Calibración y Ensayos";

			ViewBag.disabled = true;

			ViewBag.alta = "display:none";

			ViewBag.action = "Alta";

			ViewBag.ToolMantenimiento = false;

			ViewBag.Combo = "display:block";

			ViewBag.Texto = "display:none";

			return this.PartialView("~/Views/Mantenimiento/Create.cshtml", mantenimiento);
		}

		/// <summary>
		/// Alta-Editar un Mantenimiento, Calibración y Ensayos.
		/// </summary>
		[HttpPost]
		public ActionResult AltaEditarMantenimiento(MantenimientoJson mantenimientoJson)
		{
			try
			{
				 int resultado = 0;

				 switch (mantenimientoJson.action)
				{

					case "Alta":

						 AltaMantenimiento(mantenimientoJson);
						 resultado = 1;
						 break;

					default:
						 GuardarMantenimiento(mantenimientoJson);
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
		/// Alta de Mantenimiento.
		/// </summary>
		[HttpPost]
		public ActionResult AltaMantenimiento(MantenimientoJson mantenimientoJson)
		{
			try
			{
				Mantenimientos mantenimientos = new Mantenimientos();

				mantenimientos.id_Puerto      = mantenimientoJson.Id_Puerto;

				mantenimientos.id_IIPP        = mantenimientoJson.Id_IIPP;

				mantenimientos.Descripcion    = this.CambiarTexto(mantenimientoJson.Descripcion);

				mantenimientos.fecha          = mantenimientoJson.Fecha;

				mantenimientos.Equipo         = this.CambiarTexto(mantenimientoJson.Equipo);

				mantenimientos.Realizador     = this.CambiarTexto(mantenimientoJson.Realizador);

				mantenimientos.Validador      = this.CambiarTexto(mantenimientoJson.Validador);

				mantenimientos.fecha_Revision = mantenimientoJson.Fecha_Revision;

				mantenimientos.Observaciones  = this.CambiarTexto(mantenimientoJson.Observaciones); 
				
				mantenimientos.id_Usu_Alta    = this.UsuarioFrontalSession.Usuario.id;

				mantenimientos.Fech_alta      = DateTime.Now;

				this.repoMantenimiento.Create(mantenimientos);

				mantenimientoJson.Id = this.db.Mantenimientos.OrderByDescending(u => u.Id).FirstOrDefault().Id;

				this.IdMantenimiento = mantenimientoJson.Id;

				this.trazas(152, (int)this.UsuarioFrontalSession.Usuario.id, "Se ha dado de alta un registro en SPMOV_Mantenimientos con ID" + "(" + mantenimientoJson.Id.ToString() + ")");

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
		public ActionResult GuardarMantenimiento(MantenimientoJson mantenimientoJson)
		{
			try
			{
				Mantenimientos mantenimientos   = repoMantenimiento.Single(x => x.Id == mantenimientoJson.Id);

				mantenimientos.id_Puerto        = mantenimientoJson.Id_Puerto;

				mantenimientos.id_IIPP          = mantenimientoJson.Id_IIPP;

				mantenimientos.Descripcion      = this.CambiarTexto(mantenimientoJson.Descripcion);

				mantenimientos.fecha            = mantenimientoJson.Fecha;

				mantenimientos.Equipo           = this.CambiarTexto(mantenimientoJson.Equipo);

				mantenimientos.Realizador       = this.CambiarTexto(mantenimientoJson.Realizador);

				mantenimientos.Validador        = this.CambiarTexto(mantenimientoJson.Validador);

				mantenimientos.fecha_Revision   = mantenimientoJson.Fecha_Revision;

				mantenimientos.Observaciones    = this.CambiarTexto(mantenimientoJson.Observaciones); 

				this.repoMantenimiento.Update(mantenimientos);

				this.trazas(153, (int)this.UsuarioFrontalSession.Usuario.id, "Se ha dado modificado un registro en SPMOV_Mantenimientos con ID " + "(" + mantenimientoJson.Id.ToString() + ")");

				this.IdMantenimiento = mantenimientoJson.Id;

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
		/// Editar Mantenimiento, Calibración y Ensayos.
		/// </summary>
		[HttpPost]
		public ActionResult VisualizarMantenimiento(int? id, string Accion)
		{
			try
			{
				
				bool valor = true;

				if (id != null)
					this.IdMantenimiento = (int)id;
				else if (this.IdMantenimiento != 0)
					id = this.IdMantenimiento;

				valor = ValidarCategoria(this.IdMantenimiento, this.repoMantenimiento.Single(b => b.Id == id));

				if (Accion != General.AltaVer.ToDescription())
				{
					if (!valor)
						return this.Json(new { result = 0, Message = ModificarCategoria.MessageCategoria.ToDescription() + "  " + this.Description });
				}

				MantenimientoViewModel mantenimientos = new MantenimientoViewModel();

				this._serviciosMantenimiento.GetAllMantenimiento().Where(x => x.Id == id).ToList().ForEach(
				   p =>
				   {
					   mantenimientos.Id          = p.Id;

					   mantenimientos.Id_Puerto   = p.Id_Puerto;

					   mantenimientos.Puerto      = p.Puerto; 

					   mantenimientos.Id_IIPP     = p.Id_IIPP;

					   mantenimientos.IIPP        = p.IIPP;

					   mantenimientos.Descripcion = this.CambiarTexto(p.Descripcion);

					   mantenimientos.Fecha       = p.Fecha;

					   mantenimientos.Equipo      = this.CambiarTexto(p.Equipo);

					   mantenimientos.Realizador  = this.CambiarTexto(p.Realizador);

					   mantenimientos.Validador    = this.CambiarTexto(p.Validador);

					   mantenimientos.Fecha_Revision = p.Fecha_Revision;
				 
					   this.ViewBag.Observaciones = p.Observaciones;

				   });

			   
				ViewBag.alta = "display:block";

				this.comboInstalacionesPuertos(null, mantenimientos.Id_Puerto);

				if (Accion == General.EditGeneral.ToDescription())
				{
					ViewBag.disabled = true;

					ViewBag.action = General.EditGeneral.ToDescription();

					ViewBag.Mensaje = "Editar Mantenimiento, Calibración y Ensayos";

					ViewBag.Combo = "display:block";

					ViewBag.Texto = "display:none";

					ViewBag.ToolMantenimiento = valor;

				}
				else
				{
					ViewBag.disabled = false;

					ViewBag.action = "Ver";

					ViewBag.Mensaje = "Visualizar Mantenimiento, Calibración y Ensayos";

					ViewBag.Combo = "display:none";

					ViewBag.Texto = "display:block";

					ViewBag.ToolMantenimiento = valor;

				}
				this.ViewBag.navegador = this.Browser;

				this.comboPuertos(null);

			   
				return this.PartialView("~/Views/Mantenimiento/Create.cshtml", mantenimientos);
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
		/// Validar Mantenimiento
		/// </summary>
		[HttpPost]
		public ActionResult ValidarMantenimiento(int Id)
		{
			try
			{

				var retorno = ValidarCategoria(Id, this.repoMantenimiento.Single(b => b.Id == Id)) ? EliminarCategoria.MessageCategoria.ToDescription() + "  " + this.Description : string.Empty;

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
		/// Borrar Mantenimiento, Calibración y Ensayos.
		/// </summary>
		[HttpPost]
		public ActionResult EliminarMantenimiento(int Id)
		{
			try
			{
					this.repoMantenimiento.Delete(this.repoMantenimiento.Single(x => x.Id == Id));

					this.trazas(154, (int)this.UsuarioFrontalSession.Usuario.id, "Se ha realizado la eliminación de un registro en SPMOV_Mantenimientos con ID " + "(" + Id.ToString() + ")");
				
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

				return this.PartialView("~/Views/Mantenimiento/ComboCentro.cshtml");

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

			return PartialView("~/Views/Mantenimiento/Documentos/_PartialAsociadosEdit.cshtml", this.UsuarioFrontalSession);

		}

		public ActionResult AsociadosEditMantenimiento(int Id)
		{
			return PartialView("~/Views/Mantenimiento/Documentos/_PartialAsociadosEdit.cshtml", this.UsuarioFrontalSession);
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

				MantenimientoViewModel mantenimiento = new MantenimientoViewModel();

				this._serviciosMantenimiento.GetAllMantenimiento().Where(x=> x.Id == this.IdMantenimiento).ToList().ForEach(
				   p =>
				   {
					   mantenimiento.Id = p.Id;
					   mantenimiento.Puerto = p.Puerto;
					   mantenimiento.Descripcion = p.Descripcion;
				   });

				return this.PartialView("~/Views/Mantenimiento/_PartialAsignarDocumentos.cshtml", mantenimiento);

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

				IEnumerable<Doc_Asoc> result = (from t1 in this._serviciosDocumentos.GetDocs(this.IdMantenimiento, 10)
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

				this.trazas(158, (int)this.UsuarioFrontalSession.Usuario.id, "Se ha dado de modificado un registro en SPCON_Depen_Usuario con ID " + "(" + DocumentoJson.id.ToString() + ")");

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

				IEnumerable<MantenimientoViewModel> mantenimiento = this._serviciosMantenimiento.GetAllMantenimiento().Where(x => x.Id == this.IdMantenimiento);

				MantenimientoViewModel temp = mantenimiento.FirstOrDefault();

				Docs_Usuario docsInstalacion = this.repoDocumentosUsuario.Single(x => x.id == id);

				documento.descripcion = docsInstalacion.descripcion;

				documento.tipodocumento = docsInstalacion.id_tipo_doc;

				documento.documento = docsInstalacion.documento.Substring((docsInstalacion.documento.IndexOf("_")) + 1);

				return this.PartialView("~/Views/Mantenimiento/_PartialEditDocumentos.cshtml", documento);

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

				this.trazas(157, (int)this.UsuarioFrontalSession.Usuario.id, "Se ha dado de borrado un registro en SPMOV_Documentos_SP con ID " + "(" + id.ToString() + ")");

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

				IEnumerable<MantenimientoViewModel> mantenimiento = this._serviciosMantenimiento.GetAllMantenimiento().Where(x => x.Id == this.IdMantenimiento);

				MantenimientoViewModel temp = mantenimiento.FirstOrDefault();

				Docs_Usuario docsFormacion = this.repoDocumentosUsuario.Single(x => x.id == id);

				documento.descripcion = docsFormacion.descripcion;

				documento.tipodocumento = docsFormacion.id_tipo_doc;

				documento.documento = docsFormacion.documento.Substring((docsFormacion.documento.IndexOf("_")) + 1); 
				
				return this.PartialView("~/Views/Mantenimiento/_PartialEditDocumentos.cshtml", documento);

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

			IEnumerable<Doc_Asoc> result = this._serviciosDocumentos.GetDocs(this.IdMantenimiento, 10).ToList().Where(x => x.documento == nombre);

			if (result.Count() > 0)
				return this.Json(new { result = 0 });
			else
				return this.Json(new { result = 1 });
		}
		

		#endregion

		#endregion
	}
	
}