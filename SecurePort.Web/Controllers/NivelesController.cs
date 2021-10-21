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
	
	public class NivelesController : BaseController
	{
		
		public NivelesController(IServiciosNiveles serviciosNiveles,
									 InstalacionesRepository repoInstalaciones,
									 PuertosRepository repoPuertos,
									 NivelesRepository repoNiveles,
									 IServiciosTipoInstalacion serviciosInstalaciones,
									 IServiciosPuertos servicioPuertos,
									 IServiciosUsuarios servicioUsuarios,
									 DocumentosUsuarioRepository repoDocumentosUsuario,
									 IServiciosDocumentos servicioDocumentos,
									 Det_NivelRepository repoDet_Nivel,
									 UsuariosRepository repoUsuarios,
									 IServiciosTipoInstalacion serviciosTipoInstalacion,
									 TrazasRepository repotrazas,
									 ILogger log)
					: base(serviciosNiveles, 
						   repoInstalaciones, 
						   repoPuertos, 
						   repoNiveles, 
						   serviciosInstalaciones,
						   servicioPuertos,
						   servicioUsuarios, 
						   repoDocumentosUsuario, 
						   servicioDocumentos,
						   repoDet_Nivel,                           
						   repoUsuarios,
						   serviciosTipoInstalacion,
						   repotrazas, log)
		{
			 
		}

		#region Buscador
		/// <summary>
		/// Permite buscar los Niveles de Proeccion
		/// </summary>
		public ActionResult BuscadorNiveles([DataSourceRequest]DataSourceRequest request)
		{
			try
			{
				IEnumerable<NivelesViewModel> result;

				if (this.UsuarioFrontalSession.Usuario.categoria == (int)EnumCategoria.Administrador
					|| this.UsuarioFrontalSession.Usuario.categoria == (int)EnumCategoria.PuertosdelEstado)
				{
					result = this._serviciosNiveles.GetAllNiveles();
				}
				else
				{
					var categoria = this.UsuarioFrontalSession.Usuario.categoria;
					result = this._serviciosNiveles.GetAllNiveles(this.UsuarioFrontalSession.ListDependenciasDepenUsuarios, categoria);
				}
				
				return this.Json(result.ToDataSourceResult(request));
			}
			catch (Exception ex)
			{
				this.log.PublishException(new SecureportException(ex));
				throw;
			}
		}
		#endregion

		#region CRUD

		#region Niveles
		/// <summary>
		/// Retorna una lista de todas los Niveles.
		/// </summary>
		public ActionResult ListadoNiveles()
		{
			try
			{
				ViewBag.UrlDelete = "/Niveles/EliminarNiveles";
				ViewBag.Mensaje = "Niveles de Protección";
				return this.PartialView("ListadoNiveles", this.UsuarioFrontalSession);
			}
			catch (Exception ex)
			{
				this.log.PublishException(new SecureportException(ex));
				throw;
			}
		}
		/// <summary>
		/// Crear.
		/// </summary>
		[HttpPost]
		public ActionResult Create()
		{

			NivelesViewModel niveles = new NivelesViewModel();

			this.comboPuertos(null);

			if (((MultiSelectList)(this.ViewData["puertos"])).Count() == 1)
			{
				niveles.Id_Puerto = Convert.ToInt32(((SelectList)this.ViewData["puertos"]).FirstOrDefault().Value);
				niveles.PuertoActivo = true;
			}
			this.comboInstalacionesPuertos(null, null);

			ViewBag.disabled = true;

			ViewBag.Mensaje = "Alta Nivel de Protección";

			ViewBag.disabled = true;

			ViewBag.alta = "display:none";

			ViewBag.action = "Alta";

			ViewBag.ToolNivel = false;

			ViewBag.Combo = "display:block";

			ViewBag.Texto = "display:none";

			return this.PartialView("~/Views/Niveles/Create.cshtml", niveles);
		}

		  /// <summary>
		/// Alta-Editar .
		/// </summary>
		[HttpPost]
		public ActionResult AltaEditarNivel(NivelesJson nivelesJson)
		{
			try
			{
				 int resultado = 0;

				switch (nivelesJson.action)
				{

					case "Alta":
						
						 AltaNivel(nivelesJson);
						 resultado = 1;
						break;

					default:
						GuardarNivel(nivelesJson);
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
		/// Alta .
		/// </summary>
		[HttpPost]
		public ActionResult AltaNivel(NivelesJson nivelesJson)
		{
			try
			{
				Niveles niveles  = new Niveles();

				niveles.Id_Puerto      = nivelesJson.Id_Puerto;

				niveles.Descripcion    = this.CambiarTexto(nivelesJson.Descripcion);
				
				niveles.Fecha          = nivelesJson.Fecha;

				niveles.Emisor         = this.CambiarTexto(nivelesJson.Emisor);

				niveles.Receptor       = this.CambiarTexto(nivelesJson.Receptor);

				niveles.Nivel          = nivelesJson.Nivel;

				niveles.Emisor_Cancela = this.CambiarTexto(nivelesJson.Emisor_Cancela);
				
				niveles.Duracion      = nivelesJson.Duracion;

				niveles.Fecha_Cancela = nivelesJson.Fecha_Cancela;

				niveles.Nivel_Max     = nivelesJson.Nivel_Max;

				niveles.Relevante     = this.CambiarTexto(nivelesJson.Relevante);

				niveles.Recomendacion = this.CambiarTexto(nivelesJson.Recomendacion);

				niveles.Observaciones = this.CambiarTexto(nivelesJson.Observaciones); 
				
				niveles.ID_Usu_Alta   = this.UsuarioFrontalSession.Usuario.id;

				niveles.Fech_Alta     = DateTime.Now;

				this.repoNiveles.Create(niveles);

				nivelesJson.Id = this.db.Niveles.OrderByDescending(u => u.Id).FirstOrDefault().Id;

				this.IdNivel = nivelesJson.Id;

				this.trazas(144, (int)this.UsuarioFrontalSession.Usuario.id, "Se ha dado de alta un registro en SPMOV_Niveles con ID" + "(" + nivelesJson.Id.ToString() + ")");

				// si el usuario es de categoria 3 y tiene una unica instlacion
				if (this.UsuarioFrontalSession.Usuario.categoria == (int)EnumCategoria.Instalacion)
				{
					IEnumerable<InstalacionesViewModel> result = new List<InstalacionesViewModel>(_serviciosNiveles.GetInstalaciones(nivelesJson.Id_Puerto, this.UsuarioFrontalSession.Usuario.categoria, this.UsuarioFrontalSession.ListDependenciasDepenUsuarios));
					if (result.Count() == 1)
					{
						Det_Nivel Detalle = new Det_Nivel();

						Detalle.Id_Nivel = nivelesJson.Id;

						Detalle.Id_IIPP = result.FirstOrDefault().Id;

						Detalle.id_usu_alta = this.UsuarioFrontalSession.Usuario.id;

						Detalle.fech_alta = DateTime.Now;

						this.repoDet_Nivel.Create(Detalle);

						Detalle.Id = this.db.Det_Nivel.OrderByDescending(u => u.Id).FirstOrDefault().Id;

						this.trazas(144, (int)this.UsuarioFrontalSession.Usuario.id, "Se ha dado de alta un registro en SPMOV_Det_Nivel con ID" + "(" + Detalle.Id.ToString() + ")");
					
					}
				
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
		/// Guardar .
		/// </summary>
		[HttpPost]
		public ActionResult GuardarNivel(NivelesJson nivelesJson)
		{
			try
			{
				Niveles niveles = repoNiveles.Single(x => x.Id == nivelesJson.Id);

				niveles.Id_Puerto      = nivelesJson.Id_Puerto;

				niveles.Descripcion    = nivelesJson.Descripcion;

				niveles.Fecha          = nivelesJson.Fecha;

				niveles.Emisor         = nivelesJson.Emisor;

				niveles.Receptor       = nivelesJson.Receptor;

				niveles.Nivel          = nivelesJson.Nivel;

				niveles.Emisor_Cancela = nivelesJson.Emisor_Cancela;

				niveles.Duracion       = nivelesJson.Duracion;

				niveles.Fecha_Cancela  = nivelesJson.Fecha_Cancela;

				niveles.Nivel_Max      = nivelesJson.Nivel_Max;

				niveles.Relevante      = this.CambiarTexto(nivelesJson.Relevante);

				niveles.Recomendacion  = this.CambiarTexto(nivelesJson.Recomendacion);

				niveles.Observaciones  = this.CambiarTexto(nivelesJson.Observaciones); 
				
				this.repoNiveles.Update(niveles);

				this.trazas(145, (int)this.UsuarioFrontalSession.Usuario.id, "Se ha dado modificado un registro en SPMOV_Niveles con ID " + "(" + nivelesJson.Id.ToString() + ")");

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
		/// Editar .
		/// </summary>
		[HttpPost]
		public ActionResult VisualizarNivel(int? id, string Accion )
		{
			try
			{
				bool valor = true;

				if (id != null)
					this.IdNivel = (int)id;
				else if (this.IdNivel != 0 )
					id = this.IdNivel;

				valor = ValidarCategoria(this.IdNivel, this.repoNiveles.Single(b => b.Id == this.IdNivel));

				if (Accion != General.AltaVer.ToDescription())
				{
					if (!valor)
						return this.Json(new { result = 0, Message = ModificarCategoria.MessageCategoria.ToDescription() + "  " + this.Description });
				}

				NivelesViewModel niveles = new NivelesViewModel();

				this._serviciosNiveles.GetAllNiveles().Where(x => x.Id == id).ToList().ForEach(
				   p =>
				   {
					   niveles.Id = p.Id;
					   niveles.Id_Puerto = p.Id_Puerto;
					   niveles.Puerto = p.Puerto;
					   niveles.PuertoActivo = p.PuertoActivo;
					   niveles.Id_IIPP = p.Id_IIPP;
					   niveles.IIPP = p.IIPP;
					   niveles.Descripcion = p.Descripcion;
					   niveles.Fecha = p.Fecha;
					   niveles.Emisor = p.Emisor;
					   niveles.Receptor = p.Receptor;
					   niveles.Nivel = p.Nivel;
					   niveles.Emisor_Cancela = p.Emisor_Cancela;
					   niveles.Duracion = p.Duracion;
					   niveles.Fecha_Cancela = p.Fecha_Cancela;
					   niveles.Nivel_Max = p.Nivel_Max;
					   this.ViewBag.Relevante = p.Relevante;
					   this.ViewBag.Recomendacion = p.Recomendacion;
					   this.ViewBag.Observaciones = p.Observaciones;

				   });

			   
				ViewBag.alta = "display:block";


				this.comboPuertos(null);

				this.comboInstalacionesPuertos(null, niveles.Id_Puerto);

				if (Accion == General.EditGeneral.ToDescription())
				{
					ViewBag.disabled = true;

					ViewBag.action = "Edit";

					ViewBag.Mensaje = "Editar Nivel de Protección";

					ViewBag.Combo = "display:block";

					ViewBag.Texto = "display:none";

					ViewBag.ToolNivel = valor;

				}
				else
				{
					ViewBag.disabled = false;

					ViewBag.action = "Ver";

					ViewBag.Mensaje = "Visualizar Nivel de Protección";

					ViewBag.Combo = "display:none";

					ViewBag.Texto = "display:block";

					ViewBag.ToolNivel = valor;

				}
				this.ViewBag.navegador = this.Browser;


				this.LimpiarColeccionesLocal();

			   
				return this.PartialView("~/Views/Niveles/Create.cshtml", niveles);
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
		/// Validar Niveles
		/// </summary>
		[HttpPost]
		public ActionResult ValidarNiveles(int Id)
		{
			try
			{

				var retorno = ValidarCategoria(Id, this.repoNiveles.Single(b => b.Id == Id)) ? EliminarCategoria.MessageCategoria.ToDescription() + "  " + this.Description : string.Empty;

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
		/// Borrar Niveles.
		/// </summary>
		[HttpPost]
		public ActionResult EliminarNiveles(int Id)
		{
			try
			{
				
					this.repoNiveles.Delete(this.repoNiveles.Single(x => x.Id == Id));

					this.trazas(146, (int)this.UsuarioFrontalSession.Usuario.id, "Se ha realizado la eliminación de un registro en SPMOV_Niveles con ID " + "(" + Id.ToString() + ")");
				
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

				return this.PartialView("~/Views/Niveles/ComboCentro.cshtml");

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

			return PartialView("~/Views/Niveles/Documentos/_PartialAsociadosEdit.cshtml", this.UsuarioFrontalSession);

		}

		public ActionResult AsociadosEditNiveles(int Id)
		{
			return PartialView("~/Views/Niveles/Documentos/_PartialAsociadosEdit.cshtml", this.UsuarioFrontalSession);
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

				NivelesViewModel nivel = new NivelesViewModel();

				this._serviciosNiveles.GetAllNiveles().Where(x=> x.Id == this.IdNivel).ToList().ForEach(
				   p =>
				   {
					   nivel.Id = p.Id;
					   nivel.Puerto = p.Puerto;
					   nivel.Descripcion = p.Descripcion;

				   });

				return this.PartialView("~/Views/Niveles/_PartialAsignarDocumentos.cshtml", nivel);

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

				IEnumerable<Doc_Asoc> result = (from t1 in this._serviciosDocumentos.GetDocs(this.IdNivel, 11)
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

				this.trazas(150, (int)this.UsuarioFrontalSession.Usuario.id, "Se ha dado de modificado un registro en SPMOV_Documentos_SP con ID " + "(" + DocumentoJson.id.ToString() + ")");

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

				IEnumerable<NivelesViewModel> nivel = this._serviciosNiveles.GetAllNiveles().Where(x => x.Id == this.IdNivel);

				NivelesViewModel temp = nivel.FirstOrDefault();

				Docs_Usuario docsInstalacion = this.repoDocumentosUsuario.Single(x => x.id == id);

				documento.Nombre = temp.Puerto;

				documento.Apellidos = temp.Descripcion;

				documento.descripcion = docsInstalacion.descripcion;

				documento.tipodocumento = docsInstalacion.id_tipo_doc;

				documento.documento = docsInstalacion.documento.Substring((docsInstalacion.documento.IndexOf("_")) + 1);

				return this.PartialView("~/Views/Niveles/_PartialEditDocumentos.cshtml", documento);

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

				this.trazas(149, (int)this.UsuarioFrontalSession.Usuario.id, "Se ha dado de borrado un registro en SPMOV_Documentos_SP con ID " + "(" + id.ToString() + ")");

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

				IEnumerable<NivelesViewModel> nivel = this._serviciosNiveles.GetAllNiveles().Where(x => x.Id == this.IdNivel);

				NivelesViewModel temp = nivel.FirstOrDefault();

				Docs_Usuario docsFormacion = this.repoDocumentosUsuario.Single(x => x.id == id);

				documento.Nombre = temp.Puerto;

				documento.Apellidos = temp.Descripcion;

				documento.descripcion = docsFormacion.descripcion;

				documento.tipodocumento = docsFormacion.id_tipo_doc;

				documento.documento = docsFormacion.documento.Substring((docsFormacion.documento.IndexOf("_")) + 1); 


				return this.PartialView("~/Views/Niveles/_PartialEditDocumentos.cshtml", documento);

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

			IEnumerable<Doc_Asoc> result = this._serviciosDocumentos.GetDocs(this.IdNivel, 11).ToList().Where(x => x.documento == nombre);
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

				Niveles niveles = this.repoNiveles.Single(x => x.Id == this.IdNivel);

				IEnumerable<InstalacionViewModel> result;
				if (this.UsuarioFrontalSession.Usuario.categoria == (int)EnumCategoria.Administrador
					|| this.UsuarioFrontalSession.Usuario.categoria == (int)EnumCategoria.PuertosdelEstado)
				{
					result = new List<InstalacionViewModel>(this._serviciosTipoInstalacion.GetAllInstalaciones().Where(x => x.activado == "Si" && x.Id_puerto == niveles.Id_Puerto).ToList());
				}
				else
				{
					var categoria = this.UsuarioFrontalSession.Usuario.categoria;
					result = new List<InstalacionViewModel>(this._serviciosTipoInstalacion.GetAllInstalaciones(this.UsuarioFrontalSession.ListDependenciasDepenUsuarios, categoria).Where(x => x.activado == "Si" && x.Id_puerto == niveles.Id_Puerto).ToList());
				}


				this.listaInstalacionSession = (from a1 in result
												select new MOV_Instalaciones
												{
													Id = a1.Id,
													Nombre = a1.Nombre
												}).OrderBy(x => x.Nombre).ToList();
					

				this.listaInstalacionAsignadas = (from t1 in this._serviciosNiveles.GetInstalacionesbyNivel(this.IdNivel)
												   join b1 in result.ToList() on t1.Id_IIPP equals b1.Id 
												  select new MOV_Instalaciones
												  {
													  Id = t1.Id_IIPP,
													  Nombre = t1.IIPP
												  }).OrderBy(x => x.Nombre).ToList();





				this.listaInstalacionSession.ToList().ForEach(
					p => this.listaInstalacionAsignadas.ToList().ForEach(
						t =>
						{
							if (p.Id == t.Id)
							{
								this.listaInstalacionSession.Remove(p);
							}
						}));


				return this.PartialView("~/Views/Niveles/_PartialAsignarInstalacion.cshtml", niveles);

			}
			catch (Exception ex)
			{
				this.log.PublishException(new SecureportException(ex));

				return this.Json(new { result = false, Message = ex.Message });
			}
		}

		public ActionResult InstAsociadosEdit(int Id)
		{
			return PartialView("~/Views/Niveles/_PartialAsociadosInst.cshtml", this.UsuarioFrontalSession);
		}


		public ActionResult BuscadorInstalaciones([DataSourceRequest]DataSourceRequest request)
		{
			try
			{
				Niveles niveles = this.repoNiveles.Single(x => x.Id == this.IdNivel);
				IEnumerable<InstalacionViewModel> result;
				IEnumerable<Det_NivelViewModel> Nivel_detalle= new List<Det_NivelViewModel>();
				if (this.IdNivel != 0)
				{
					if (this.UsuarioFrontalSession.Usuario.categoria == (int)EnumCategoria.Administrador
						|| this.UsuarioFrontalSession.Usuario.categoria == (int)EnumCategoria.PuertosdelEstado)
					{
						result = new List<InstalacionViewModel>(this._serviciosTipoInstalacion.GetAllInstalaciones().Where(x => x.activado == "Si" && x.Id_puerto == niveles.Id_Puerto).ToList());
					}
					else
					{
						var categoria = this.UsuarioFrontalSession.Usuario.categoria;
						result = new List<InstalacionViewModel>(this._serviciosTipoInstalacion.GetAllInstalaciones(this.UsuarioFrontalSession.ListDependenciasDepenUsuarios, categoria).Where(x => x.activado == "Si" && x.Id_puerto == niveles.Id_Puerto).ToList());
					}

						Nivel_detalle = (from t1 in this._serviciosNiveles.GetInstalacionesbyNivel(this.IdNivel)
																	 join b1 in result.ToList() on t1.Id_IIPP equals b1.Id
																	 select new Det_NivelViewModel
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


		public ActionResult InstalacionesDisponiblesNiveles([DataSourceRequest]DataSourceRequest request)
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

		public ActionResult InstalacionesAsociadosNiveles([DataSourceRequest]DataSourceRequest request)
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

				return this.PartialView("~/Views/Niveles/Instalacion/_PartialInstalacionesAsociadas.cshtml");

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
				return this.PartialView("~/Views/Niveles/Instalacion/_PartialInstalacionDisponible.cshtml");

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

				return this.PartialView("~/Views/Niveles/Instalacion/_PartialInstalacionDisponible.cshtml");
			}
			catch (Exception ex)
			{
				this.log.PublishException(new SecureportException(ex));

				return this.Json(new { result = false, Message = ex.Message });
			}

		}

		[HttpPost]
		public ActionResult GuardarNivelInstalaciones()
		{
			try
			{
				//Asignar Instlaciones
				this.listaInstalacionAsignadas.ToList().ForEach(
					p =>
					{

						if (this.repoDet_Nivel.Single(x => x.Id_Nivel == this.IdNivel && x.Id_IIPP == p.Id) == null)
						{
							Det_Nivel depenUsuario = new Det_Nivel()
							{
								Id_Nivel = this.IdNivel,
								Id_IIPP = p.Id,
								id_usu_alta = this.UsuarioFrontalSession.Usuario.id,
								fech_alta = DateTime.Now
							};

							this.repoDet_Nivel.Create(depenUsuario);
						}
					});

				//Desasignar Instalaciones
				this.listaInstalacionSession.ToList().ForEach(
				  p =>
				  {
					  Det_Nivel depen_Usuario = this.repoDet_Nivel.Single(x => x.Id_Nivel == this.IdNivel && x.Id_IIPP == p.Id);

					  if (depen_Usuario != null)
					  {
						  this.repoDet_Nivel.Delete(depen_Usuario);

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
		public ActionResult DeshacerNivelInstalaciones()
		{
			try
			{

				Niveles niveles = this.repoNiveles.Single(x => x.Id == this.IdNivel);
				IEnumerable<InstalacionViewModel> result;
				if (this.UsuarioFrontalSession.Usuario.categoria == (int)EnumCategoria.Administrador
					|| this.UsuarioFrontalSession.Usuario.categoria == (int)EnumCategoria.PuertosdelEstado)
				{
					result = new List<InstalacionViewModel>(this._serviciosTipoInstalacion.GetAllInstalaciones().Where(x => x.activado == "Si" && x.Id_puerto == niveles.Id_Puerto).ToList());
				}
				else
				{
					var categoria = this.UsuarioFrontalSession.Usuario.categoria;
					result = new List<InstalacionViewModel>(this._serviciosTipoInstalacion.GetAllInstalaciones(this.UsuarioFrontalSession.ListDependenciasDepenUsuarios, categoria).Where(x => x.activado == "Si" && x.Id_puerto == niveles.Id_Puerto).ToList());
				}


					this.listaInstalacionSession = (from a1 in result
												select new MOV_Instalaciones
												{
													Id = a1.Id,
													Nombre = a1.Nombre
												}).ToList();


				this.listaInstalacionAsignadas = (from t1 in this._serviciosNiveles.GetInstalacionesbyNivel(this.IdNivel)
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


				return this.PartialView("~/Views/Niveles/Instalacion/_PartialInstalacionDisponible.cshtml");

			}
			catch (Exception ex)
			{
				this.log.PublishException(new SecureportException(ex));

				return this.Json(new { result = false, Message = ex.Message });
			}
		}

		[HttpPost]
		public ActionResult VolverNivelInstalacion()
		{
			try
			{
				return this.PartialView("~/Views/Niveles/List.cshtml");
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