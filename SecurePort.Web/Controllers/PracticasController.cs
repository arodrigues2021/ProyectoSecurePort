
namespace SecurePort.Web.Controllers
{
	#region using
	using System;
	using System.Collections.Generic;
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
	using System.Configuration;
	using Practicas = SecurePort.Entities.Models.Practicas;
	#endregion
	
	public class PracticasController : BaseController
	{

		public PracticasController(IServiciosPracticas serviciosPracticas,
									 InstalacionesRepository repoInstalaciones,
									 PuertosRepository repoPuertos,
									 PracticasRepository repoPracticas,
									 IServiciosTipoInstalacion serviciosInstalaciones,                                     
									 IServiciosPuertos servicioPuertos,
									 DocumentosUsuarioRepository repoDocumentosUsuario,
									 IServiciosUsuarios servicioUsuarios,
									 IServiciosDocumentos servicioDocumentos,
									 Det_PracticaRepository repoDet_Practica, 
		   
									 TrazasRepository repotrazas,
									 ILogger log)
						
				: base(serviciosPracticas,
					   repoInstalaciones,
					   repoPuertos, 
					   repoPracticas, 
					   serviciosInstalaciones, 
					   servicioPuertos,
					   repoDocumentosUsuario,
					   servicioUsuarios,
					   servicioDocumentos,
					   repoDet_Practica,
					   repotrazas,
					   log)
		{
			 
		}

		#region action
		/// <summary>
		/// Retorna una lista de todas las practicas o ejercicios.
		/// </summary>
		public ActionResult ListadoPracticas()
		{
			try
			{
				ViewBag.UrlDelete = "/Practicas/EliminarPracticas";
				ViewBag.Mensaje = "Prácticas o Ejercicios";
				return this.PartialView("ListadoPracticas", this.UsuarioFrontalSession);
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
		/// Visualizar las Prácticas o Ejercicios.
		/// </summary>
		public ActionResult BuscadorPracticas([DataSourceRequest]DataSourceRequest request)
		{
			try
			{
				if (this.UsuarioFrontalSession.Usuario.categoria == (int)EnumCategoria.Administrador
					|| this.UsuarioFrontalSession.Usuario.categoria == (int)EnumCategoria.PuertosdelEstado)
				{
					return this.Json(this._serviciosPracticas.ListPracticas().ToDataSourceResult(request));
				}

				var categoria = this.UsuarioFrontalSession.Usuario.categoria;
				return this.Json(this._serviciosPracticas.ListPracticas(this.UsuarioFrontalSession.ListDependenciasDepenUsuarios, categoria).ToDataSourceResult(request));
			}
			catch (Exception ex)
			{
				this.log.PublishException(new SecureportException(ex));
				throw;
			}
		}
		#endregion

		#region CRUD

		#region Practicas
		/// <summary>
		/// Crear una Prácticas o Ejercicios.
		/// </summary>
		[HttpPost]
		public ActionResult Create()
		{

			PracticasViewModel practicas = new PracticasViewModel();

			this.comboPuertos(null);

			if (((MultiSelectList)(this.ViewData["puertos"])).Count() == 1)
				practicas.Id_Puerto = Convert.ToInt32(((SelectList)this.ViewData["puertos"]).FirstOrDefault().Value);
			
			//this.comboInstalacionesPuertos(null, null);

			ViewBag.disabled = true;

			ViewBag.Mensaje = "Alta Prácticas o Ejercicios";

			ViewBag.disabled = true;

			ViewBag.alta = "display:none";

			ViewBag.action = "Alta";

			ViewBag.ToolPractica = false;

			ViewBag.Combo = "display:block";

			ViewBag.Texto = "display:none";

			this.ViewData["EstadoFechaPromEne"] = "1";
			this.ViewData["EstadoFechaPromCol"] = "#FFFFFF";
			this.ViewData["EstadoFechaPlanEne"] = "1";
			this.ViewData["EstadoFechaPlanCol"] = "#FFFFFF";
			this.ViewData["EstadoFechaRealEne"] = "1";
			this.ViewData["EstadoFechaRealCol"] = "#FFFFFF";

			return this.PartialView("~/Views/Practicas/Create.cshtml", practicas);
		}

		/// <summary>
		/// Alta-Editar Prácticas o Ejercicios.
		/// </summary>
		[HttpPost]
		public ActionResult AltaEditarPractica(PracticasJson practicasJson)
		{
			try
			{
				int resultado = 0;

				switch (practicasJson.action)
				{
					case "Alta":
						AltaPractica(practicasJson);
						resultado = 1;
						break;

					default:
						GuardarPracticas(practicasJson);
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
		/// Alta de Practicas.
		/// </summary>
		[HttpPost]
		public ActionResult AltaPractica(PracticasJson practicasJson)
		{
			try
			{
				Practicas practicas     = new Practicas();

				practicas.Id_Puerto     = practicasJson.Id_Puerto;

				practicas.Ini_Programa  = practicasJson.Ini_Programa;

				practicas.Ini_Real      = practicasJson.Ini_Real;
				
				practicas.Ini_Planifica = practicasJson.Ini_Planifica;

				practicas.Fin_Programa  = practicasJson.Fin_Programa;

				practicas.Fin_Real      = practicasJson.Fin_Real;

				practicas.Fin_Planifica = practicasJson.Fin_Planifica;

				practicas.Propuesta     = CambiarTexto(practicasJson.Propuesta);

				practicas.Ratifico      = practicasJson.Ratifico; 

				practicas.Observaciones = CambiarTexto(practicasJson.Observaciones); 

				practicas.Estado        = practicasJson.Estado;

				practicas.Tipo          = practicasJson.Tipo;

				practicas.Responsable   = practicasJson.Responsable;

				practicas.Cuerpos       = practicasJson.Cuerpos;

				practicas.Descripcion   = CambiarTexto(practicasJson.Descripcion); 

				practicas.Conclusiones  = CambiarTexto(practicasJson.Conclusiones);

				practicas.Valoracion    = practicasJson.Valoracion;
				
				practicas.ID_Usu_alta   = this.UsuarioFrontalSession.Usuario.id;

				practicas.Fech_Alta     = DateTime.Now;

				this.repoPracticas.Create(practicas);

				practicasJson.Id = this.db.Practicas.OrderByDescending(u => u.Id).FirstOrDefault().Id;
				this.IdPractica = practicasJson.Id;

				this.trazas(118, (int)this.UsuarioFrontalSession.Usuario.id, "Se ha dado de alta un registro en SPMOV_Practicas con ID"  + practicasJson.Id);

				// si el usuario es de categoria 3 y tiene una unica instlacion
				if (this.UsuarioFrontalSession.Usuario.categoria == (int)EnumCategoria.Instalacion)
				{
					IEnumerable<InstalacionesViewModel> result = new List<InstalacionesViewModel>(_serviciosPracticas.GetInstalaciones(practicasJson.Id_Puerto, this.UsuarioFrontalSession.Usuario.categoria, this.UsuarioFrontalSession.ListDependenciasDepenUsuarios));
					if (result.Count() == 1)
					{
						Det_Practica Detalle = new Det_Practica();

						Detalle.Id_Practica = practicasJson.Id;

						Detalle.Id_IIPP = result.FirstOrDefault().Id;

						Detalle.id_usu_alta = this.UsuarioFrontalSession.Usuario.id;

						Detalle.fech_alta = DateTime.Now;

						this.repoDet_Practica.Create(Detalle);

						Detalle.Id = this.db.Det_Practica.OrderByDescending(u => u.Id).FirstOrDefault().Id;

						this.trazas(118, (int)this.UsuarioFrontalSession.Usuario.id, "Se ha dado de alta un registro en SPMOV_Det_Practica con ID" + "(" + Detalle.Id.ToString() + ")");

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
		/// Guardar Prácticas o Ejercicios..
		/// </summary>
		[HttpPost]
		public ActionResult GuardarPracticas(PracticasJson practicasJson)
		{
			try
			{
				Practicas practicas      = repoPracticas.Single(x => x.Id == practicasJson.Id);

				practicas.Id_Puerto      = practicasJson.Id_Puerto;

				practicas.Ini_Programa   = practicasJson.Ini_Programa;

				practicas.Ini_Real       = practicasJson.Ini_Real;

				practicas.Ini_Planifica  = practicasJson.Ini_Planifica;

				practicas.Fin_Programa   = practicasJson.Fin_Programa;

				practicas.Fin_Real       = practicasJson.Fin_Real;

				practicas.Fin_Planifica  = practicasJson.Fin_Planifica;

				practicas.Propuesta      = CambiarTexto(practicasJson.Propuesta); 

				practicas.Ratifico       = practicasJson.Ratifico; 

				practicas.Observaciones  = CambiarTexto(practicasJson.Observaciones); 

				practicas.Estado         = practicasJson.Estado;

				practicas.Tipo           = practicasJson.Tipo;

				practicas.Responsable    = practicasJson.Responsable;

				practicas.Cuerpos        = practicasJson.Cuerpos;

				practicas.Descripcion    = CambiarTexto(practicasJson.Descripcion); 

				practicas.Conclusiones   = CambiarTexto(practicasJson.Conclusiones); 

				practicas.Valoracion     = practicasJson.Valoracion; 

				this.repoPracticas.Update(practicas);

				this.trazas(121, (int)this.UsuarioFrontalSession.Usuario.id, "Se ha modificado un registro en SPMOV_Practicas con ID" + "(" + practicasJson.Id.ToString() + ")");

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
		/// Editar Prácticas o Ejercicios.
		/// </summary>
		[HttpPost]
		public ActionResult VisualizarPracticas(int? id, string Accion)
		{
			try
			{
				bool valor = true;
				if (id != null)
					this.IdPractica = (int)id;
				else if (this.IdPractica != 0)
					id = this.IdPractica;

				valor = ValidarCategoria(this.IdPractica, this.repoPracticas.Single(b => b.Id == this.IdPractica));

				if (Accion != General.AltaVer.ToDescription())
				{
					if (!valor)
						return this.Json(new { result = 0, Message = ModificarCategoria.MessageCategoria.ToDescription() + "  " + this.Description });
				}

				PracticasViewModel practicas = new PracticasViewModel();

				this._serviciosPracticas.ListPracticas().Where(x => x.Id == id).ToList().ForEach(
				   p =>
				   {
					   practicas.Id               = p.Id;
					   practicas.IIPP             = p.IIPP;
					   practicas.Id_Puerto        = p.Id_Puerto;
					   practicas.Puerto           = p.Puerto;
					   practicas.Descripcion      = p.Descripcion;
					   practicas.Tipo             = p.Tipo;
					   practicas.NombreTipo       = p.NombreTipo;
					   practicas.Estado           = p.Estado;
					   practicas.NombreEstado     = p.NombreEstado;
					   practicas.Ini_Programa     = p.Ini_Programa;
					   practicas.Fin_Programa     = p.Fin_Programa;
					   practicas.Ini_Planifica    = p.Ini_Planifica;
					   practicas.Fin_Planifica    = p.Fin_Planifica;
					   practicas.Ini_Real         = p.Ini_Real;
					   practicas.Fin_Real         = p.Fin_Real;
					   practicas.Responsable      = p.Responsable;
					   practicas.Cuerpos          = p.Cuerpos;                  
					   practicas.Valoracion       = p.Valoracion;
					   practicas.NombreValoracion = p.NombreValoracion;
					   practicas.Ratifico         = p.Ratifico;
					   practicas.NombreRatifico   = p.NombreRatifico;
					   this.ViewBag.Conclusiones  = p.Conclusiones;
					   this.ViewBag.Propuesta     = p.Propuesta;
					   this.ViewBag.Observaciones = p.Observaciones;

				   });

				

				ViewBag.alta = "display:block";

				this.comboPuertos(null);

				//this.comboInstalacionesPuertos(null, practicas.Id_Puerto);

				if (Accion == General.EditGeneral.ToDescription())
				{
					ViewBag.disabled = true;

					ViewBag.action = "Edit";

					ViewBag.Mensaje = "Editar Prácticas o Ejercicios";

					ViewBag.Combo = "display:block";

					ViewBag.Texto = "display:none";

					ViewBag.ToolPractica = valor;
					// Estado
					if (practicas.Estado == 1) {
						this.ViewData["EstadoFechaPromEne"] = "1";
						this.ViewData["EstadoFechaPromCol"] = "#FFFFFF";
					} else {
						this.ViewData["EstadoFechaPromEne"] = "0";
						this.ViewData["EstadoFechaPromCol"] = "#eeeeee";
					}
					if (practicas.Estado == 2) {
						this.ViewData["EstadoFechaPlanEne"] = "1";
						this.ViewData["EstadoFechaPlanCol"] = "#FFFFFF";
					} else {                        
						this.ViewData["EstadoFechaPlanEne"] = "0";
						this.ViewData["EstadoFechaPlanCol"] = "#eeeeee";
					}
					if (practicas.Estado == 3)
					{
						this.ViewData["EstadoFechaRealEne"] = "1";
						this.ViewData["EstadoFechaRealCol"] = "#FFFFFF";
					} else {
						this.ViewData["EstadoFechaRealEne"] = "0";
						this.ViewData["EstadoFechaRealCol"] = "#eeeeee";
					}



				}
				else
				{
					ViewBag.disabled = false;

					ViewBag.action = "Ver";

					ViewBag.Mensaje = "Visualizar Prácticas o Ejercicios";

					ViewBag.Combo = "display:none";

					ViewBag.Texto = "display:block";

					ViewBag.ToolPractica = valor;
					// estados
					this.ViewData["EstadoFechaPromEne"] = "0";
					this.ViewData["EstadoFechaPromCol"] = "#eeeeee";
					this.ViewData["EstadoFechaPlanEne"] = "0";
					this.ViewData["EstadoFechaPlanCol"] = "#eeeeee";
					this.ViewData["EstadoFechaRealEne"] = "0";
					this.ViewData["EstadoFechaRealCol"] = "#eeeeee";


				}
				this.ViewBag.navegador = this.Browser;

				LimpiarColeccionesLocal();


				return this.PartialView("~/Views/Practicas/Create.cshtml", practicas);
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
		/// Validar Practicas.
		/// </summary>
		[HttpPost]
		public ActionResult ValidarPracticas(int Id)
		{
			try
			{

				var retorno = ValidarCategoria(Id, this.repoPracticas.Single(b => b.Id == Id)) ? EliminarCategoria.MessageCategoria.ToDescription() + "  " + this.Description : string.Empty;

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
		/// Borrar Prácticas o Ejercicios.
		/// </summary>
		[HttpPost]
		public ActionResult EliminarPracticas(int Id)
		{
			try
			{
				int? Id_Usu = this.repoPracticas.Single(x => x.Id == Id).ID_Usu_alta;

				IEnumerable<Docs_Usuario> Docs_Usuario = this.repoDocumentosUsuario.GetAll().Where(x => x.id_servicio == (int)EnumTiposServicio.Practica && x.id_usu_alta == Id_Usu);

				IEnumerable<Det_Practica> Det_Practica = this.repoDet_Practica.GetAll().Where(x => x.Id_Practica == Id).ToList();

				using (TransactionScope scope = new TransactionScope(
					TransactionScopeOption.Required,
					new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted }))
				{
					//Hijos
					Det_Practica.ToList().ForEach(p => { this.repoDet_Practica.Delete(p); });

					//Cabecera
					this.repoPracticas.Delete(this.repoPracticas.Single(x => x.Id == Id));

					Docs_Usuario.ToList().ForEach(
						z =>
						{
							this.repoDocumentosUsuario.Delete(z);
							this.BorrarFichero(ConfigurationManager.AppSettings["RutaSecurePorDoc"].ToString() + "/" + z.documento);
						});

					this.trazas(
						122,
						(int)this.UsuarioFrontalSession.Usuario.id,
						"Se ha realizado la eliminación de un registro en SPMOV_Practicas con ID" + Id.ToString());

					scope.Complete();
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
		public ActionResult CambiarCentro(int? id)
		{
			try
			{

				this.comboInstalacionesPuertos(null, id);

				return this.PartialView("~/Views/Practicas/ComboCentro.cshtml");

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
					// panificacion false
					this.ViewData["EstadoFechaPlanEne"] = "0";
					this.ViewData["EstadoFechaPlanCol"] = "#eeeeee";
				   
				}
				return this.PartialView("~/Views/Practicas/EstadoPlanifica.cshtml");
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
				return this.PartialView("~/Views/Practicas/EstadoReal.cshtml");

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
				return this.PartialView("~/Views/Practicas/EstadoProm.cshtml");

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

			return PartialView("~/Views/Practicas/Documentos/_PartialAsociadosEdit.cshtml", this.UsuarioFrontalSession);

		}

		public ActionResult AsociadosEditPracticas(int Id)
		{
			return PartialView("~/Views/Practicas/Documentos/_PartialAsociadosEdit.cshtml", this.UsuarioFrontalSession);
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

				PracticasViewModel practica = new PracticasViewModel();

				this._serviciosPracticas.ListPracticas().Where(x => x.Id == this.IdPractica).ToList().ForEach(
				   p =>
				   {
					   practica.Id = p.Id;
					   practica.Descripcion = p.Descripcion;                       

				   });

				return this.PartialView("~/Views/Practicas/_PartialAsignarDocumentos.cshtml", practica);

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

				IEnumerable<Doc_Asoc> result = (from t1 in this._serviciosDocumentos.GetDocs(this.IdPractica, 12)
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

				this.trazas(126, (int)this.UsuarioFrontalSession.Usuario.id, "Se ha dado de modificado un registro en SPMOV_Documentos_SP " + this.UsuarioFrontalSession.Usuario.login);

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

				IEnumerable<PracticasViewModel> practica = this._serviciosPracticas.ListPracticas().Where(x => x.Id == this.IdPractica);

				PracticasViewModel temp = practica.FirstOrDefault();

				Docs_Usuario docsInstalacion = this.repoDocumentosUsuario.Single(x => x.id == id);

				documento.Nombre = temp.Descripcion;

				documento.Apellidos = temp.Descripcion;

				documento.descripcion = docsInstalacion.descripcion;

				documento.tipodocumento = docsInstalacion.id_tipo_doc;

				documento.documento = docsInstalacion.documento.Substring((docsInstalacion.documento.IndexOf("_")) + 1);

				return this.PartialView("~/Views/Practicas/_PartialEditDocumentos.cshtml", documento);

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

				this.trazas(125, (int)this.UsuarioFrontalSession.Usuario.id, "Se ha dado de borrado un registro en SPMOV_Documentos_SP " + this.UsuarioFrontalSession.Usuario.login);

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

				IEnumerable<PracticasViewModel> practica = this._serviciosPracticas.ListPracticas().Where(x => x.Id == this.IdPractica);

				PracticasViewModel temp = practica.FirstOrDefault();

				Docs_Usuario docsFormacion = this.repoDocumentosUsuario.Single(x => x.id == id);

				documento.Nombre = temp.Descripcion;

				documento.Apellidos = temp.Descripcion;

				documento.descripcion = docsFormacion.descripcion;

				documento.tipodocumento = docsFormacion.id_tipo_doc;

				documento.documento = docsFormacion.documento.Substring((docsFormacion.documento.IndexOf("_")) + 1); 


				return this.PartialView("~/Views/Practicas/_PartialEditDocumentos.cshtml", documento);

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

			IEnumerable<Doc_Asoc> result = this._serviciosDocumentos.GetDocs(this.IdPractica, 12).ToList().Where(x => x.documento == nombre);
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

				Practicas practica = this.repoPracticas.Single(x => x.Id == this.IdPractica);

				IEnumerable<InstalacionViewModel> result;
				if (this.UsuarioFrontalSession.Usuario.categoria == (int)EnumCategoria.Administrador
					|| this.UsuarioFrontalSession.Usuario.categoria == (int)EnumCategoria.PuertosdelEstado)
				{
					result = new List<InstalacionViewModel>(this._serviciosTipoInstalacion.GetAllInstalaciones().Where(x => x.activado == "Si" && x.Id_puerto == practica.Id_Puerto).ToList());
				}
				else
				{
					var categoria = this.UsuarioFrontalSession.Usuario.categoria;
					result = new List<InstalacionViewModel>(this._serviciosTipoInstalacion.GetAllInstalaciones(this.UsuarioFrontalSession.ListDependenciasDepenUsuarios, categoria).Where(x => x.activado == "Si" && x.Id_puerto == practica.Id_Puerto).ToList());
				}


				this.listaInstalacionSession = (from a1 in result
												select new MOV_Instalaciones
												{
													Id = a1.Id,
													Nombre = a1.Nombre
												}).ToList();


				this.listaInstalacionAsignadas = (from t1 in this._serviciosPracticas.GetInstalacionesbyPractica(this.IdPractica)
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


				return this.PartialView("~/Views/Practicas/_PartialAsignarInstalacion.cshtml", practica);

			}
			catch (Exception ex)
			{
				this.log.PublishException(new SecureportException(ex));

				return this.Json(new { result = false, Message = ex.Message });
			}
		}

		public ActionResult InstAsociadosEdit(int Id)
		{
			return PartialView("~/Views/Practicas/_PartialAsociadosInst.cshtml", this.UsuarioFrontalSession);
		}


		public ActionResult BuscadorInstalaciones([DataSourceRequest]DataSourceRequest request)
		{
			try
			{
				Practicas practica = this.repoPracticas.Single(x => x.Id == this.IdPractica);
				IEnumerable<InstalacionViewModel> result;
				IEnumerable<Det_PracticaViewModel> Nivel_detalle = new List<Det_PracticaViewModel>();
				if (this.IdPractica != 0)
				{
					if (this.UsuarioFrontalSession.Usuario.categoria == (int)EnumCategoria.Administrador
						|| this.UsuarioFrontalSession.Usuario.categoria == (int)EnumCategoria.PuertosdelEstado)
					{
						result = new List<InstalacionViewModel>(this._serviciosTipoInstalacion.GetAllInstalaciones().Where(x => x.activado == "Si" && x.Id_puerto == practica.Id_Puerto).ToList());
					}
					else
					{
						var categoria = this.UsuarioFrontalSession.Usuario.categoria;
						result = new List<InstalacionViewModel>(this._serviciosTipoInstalacion.GetAllInstalaciones(this.UsuarioFrontalSession.ListDependenciasDepenUsuarios, categoria).Where(x => x.activado == "Si" && x.Id_puerto == practica.Id_Puerto).ToList());
					}


					Nivel_detalle = (from t1 in this._serviciosPracticas.GetInstalacionesbyPractica(this.IdPractica)
									 join b1 in result.ToList() on t1.Id_IIPP equals b1.Id
									 select new Det_PracticaViewModel
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


		public ActionResult InstalacionesDisponiblesPracticas([DataSourceRequest]DataSourceRequest request)
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

		public ActionResult InstalacionesAsociadosPracticas([DataSourceRequest]DataSourceRequest request)
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

				return this.PartialView("~/Views/Practicas/Instalacion/_PartialInstalacionesAsociadas.cshtml");

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
				return this.PartialView("~/Views/Practicas/Instalacion/_PartialInstalacionDisponible.cshtml");

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

				return this.PartialView("~/Views/Practicas/Instalacion/_PartialInstalacionDisponible.cshtml");
			}
			catch (Exception ex)
			{
				this.log.PublishException(new SecureportException(ex));

				return this.Json(new { result = false, Message = ex.Message });
			}

		}

		[HttpPost]
		public ActionResult GuardarPracticaInstalaciones()
		{
			try
			{
				//Asignar Instlaciones
				this.listaInstalacionAsignadas.ToList().ForEach(
					p =>
					{

						if (this.repoDet_Practica.Single(x => x.Id_Practica == this.IdPractica && x.Id_IIPP == p.Id) == null)
						{
							Det_Practica depenUsuario = new Det_Practica()
							{
								Id_Practica = this.IdPractica,
								Id_IIPP = p.Id,
								id_usu_alta = this.UsuarioFrontalSession.Usuario.id,
								fech_alta = DateTime.Now
							};

							this.repoDet_Practica.Create(depenUsuario);
						}
					});

				//Desasignar Instalaciones
				this.listaInstalacionSession.ToList().ForEach(
				  p =>
				  {
					  Det_Practica depen_Usuario = this.repoDet_Practica.Single(x => x.Id_Practica == this.IdPractica && x.Id_IIPP == p.Id);

					  if (depen_Usuario != null)
					  {
						  this.repoDet_Practica.Delete(depen_Usuario);

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
		public ActionResult DeshacerPracticaInstalaciones()
		{
			try
			{

				Practicas practica = this.repoPracticas.Single(x => x.Id == this.IdPractica);
				IEnumerable<InstalacionViewModel> result;
				if (this.UsuarioFrontalSession.Usuario.categoria == (int)EnumCategoria.Administrador
					|| this.UsuarioFrontalSession.Usuario.categoria == (int)EnumCategoria.PuertosdelEstado)
				{
					result = new List<InstalacionViewModel>(this._serviciosTipoInstalacion.GetAllInstalaciones().Where(x => x.activado == "Si" && x.Id_puerto == practica.Id_Puerto).ToList());
				}
				else
				{
					var categoria = this.UsuarioFrontalSession.Usuario.categoria;
					result = new List<InstalacionViewModel>(this._serviciosTipoInstalacion.GetAllInstalaciones(this.UsuarioFrontalSession.ListDependenciasDepenUsuarios, categoria).Where(x => x.activado == "Si" && x.Id_puerto == practica.Id_Puerto).ToList());
				}


				this.listaInstalacionSession = (from a1 in result
												select new MOV_Instalaciones
												{
													Id = a1.Id,
													Nombre = a1.Nombre
												}).ToList();


				this.listaInstalacionAsignadas = (from t1 in this._serviciosPracticas.GetInstalacionesbyPractica(this.IdPractica)
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


				return this.PartialView("~/Views/Practicas/Instalacion/_PartialInstalacionDisponible.cshtml");

			}
			catch (Exception ex)
			{
				this.log.PublishException(new SecureportException(ex));

				return this.Json(new { result = false, Message = ex.Message });
			}
		}

		[HttpPost]
		public ActionResult VolverPracticaInstalacion()
		{
			try
			{
				return this.PartialView("~/Views/Practicas/List.cshtml");
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