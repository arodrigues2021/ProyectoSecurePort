namespace SecurePort.Web.Controllers
{
	#region Using
	using System.Data.Entity.Infrastructure;
	using System.Data.Entity.ModelConfiguration;
	using System.Security.Cryptography.X509Certificates;

	using DocumentFormat.OpenXml.Office2010.PowerPoint;

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

	using WebGrease.Css.Extensions;

	#endregion
	
	public class InstalacionesController : BaseController
	{
		public InstalacionesController(IServiciosPuertos serviciosPuertos,
										IServiciosTipoInstalacion serviciosTipoInstalacion,
										InstalacionesRepository repoInstalaciones,
										TipoInstalacionRepository repoTipoInstalaciones,
										OperadoresRepository repoOperadores,
										ProvinciasRepository repoProvincias,
										CiudadesRepository repoCiudades,
										IServiciosUsuarios servicioUsuarios,
										IServiciosBienes serviciosBienes,
										PuertosRepository repoPuertos,
										MOV_Detalle_InstalacionRepository repoMOV_Detalle_Instalacion,
										TrazasRepository repotrazas, 
										ILogger logger)
			: base(serviciosPuertos,
					 serviciosTipoInstalacion, 
					 repoInstalaciones,
					 repoTipoInstalaciones,
					 repoOperadores,
					 repoProvincias,
					 repoCiudades,
					 servicioUsuarios,
					 serviciosBienes,
					 repoPuertos,
					 repoMOV_Detalle_Instalacion,
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
					return this.Json(this._serviciosTipoInstalacion.GetAllInstalaciones().ToDataSourceResult(request));
				}
				var categoria = this.UsuarioFrontalSession.Usuario.categoria;
				return this.Json(this._serviciosTipoInstalacion.GetAllInstalaciones(this.UsuarioFrontalSession.ListDependenciasDepenUsuarios,categoria).ToDataSourceResult(request));
			}
			catch (Exception ex)
			{
				this.log.PublishException(new SecureportException(ex));
				throw;
			}
		}

		#endregion

		#region action
		[HttpPost]
		public ActionResult CambiarTipo(int? id)
		{
			try
			{

				comboTipoInstalaciones(id);

				return this.PartialView("~/Views/Instalaciones/ComboTipo.cshtml");

			}
			catch (Exception ex)
			{
				this.log.PublishException(new SecureportException(ex));

				return this.Json(new { result = false, Message = ex.Message });
			}

		}
		/// <summary>
		/// Retorna una lista de todas las Instalaciones.
		/// </summary>
		public ActionResult ListadoInstalaciones()
		{
			try
			{
				ViewBag.UrlDelete = "/Instalaciones/EliminarInstalaciones";
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

		#region CRUD
		[HttpPost]
		public ActionResult SaveBienPadreHijo(string BienPadre, string QuitarPadre)
		{
			try
			{
				string[] CodeBienPadre = BienPadre.Split(',');

				string[] CodeQuitarBienPadre = QuitarPadre.Split(',');

				if (!this.repoMovDetalleInstalaciones.GetAll().Where(x => x.ID_Instalacion == this.idInstalacion).Any())
				{
					this.ListBienesPadres = this._serviciosBienes.GetAllBienesPadre(this.idInstalacion).ToList();

					this.ListBienesPadres.ToList().ForEach(
						y =>
						{
							if (Array.FindAll(CodeQuitarBienPadre, s => s.Equals(y.Id.ToString())).Length > 0)
							{
								this.ListBienesPadres.Remove(y);
							}

						});

					this.ListBienesPadres.ToList().ForEach(
						x =>
						{

							MOV_Detalle_Instalacion movDetalle = new MOV_Detalle_Instalacion();
							movDetalle.ID_Bien          = x.Id;
							movDetalle.ID_Instalacion   = Convert.ToInt32(this.idInstalacion);
							movDetalle.ID_Usu_Alta      = this.UsuarioFrontalSession.Usuario.id;
							movDetalle.Fech_Alta        = DateTime.Now;
							this.repoMovDetalleInstalaciones.Create(movDetalle);

						});

				}
				
				//Actualizo los bines Padres y hijos (Detalle de las amenazas)
				var listAmenazasSi = this.ListAmenazasPadreHijos.Where(x => x.activado == "Si").ToList();

				listAmenazasSi.ToList().ForEach(
					z =>
					{
						string[] codeHijo = z.IdpadreHijo.Split('-');

						int codeBien = codeHijo.Length == 1? Convert.ToInt32(z.IdpadreHijo): Convert.ToInt32(codeHijo[1]);

						if (!this.repoMovDetalleInstalaciones.GetAll().Where(x => x.ID_Bien == codeBien && x.ID_Instalacion == Convert.ToInt32(this.idInstalacion) && x.ID_Amenaza==z.Id).ToList().Any())
						{
							MOV_Detalle_Instalacion movDetalle = new MOV_Detalle_Instalacion();
							movDetalle.ID_Bien                 = codeBien;
							movDetalle.ID_Amenaza              = z.Id;
							movDetalle.ID_Instalacion          = Convert.ToInt32(this.idInstalacion);
							movDetalle.ID_Usu_Alta             = this.UsuarioFrontalSession.Usuario.id;
							movDetalle.Fech_Alta               = DateTime.Now;
							this.repoMovDetalleInstalaciones.Create(movDetalle);
						}
						this.repoMovDetalleInstalaciones.GetAll().Where(x => x.ID_Bien == codeBien && x.ID_Instalacion == Convert.ToInt32(this.idInstalacion) && x.ID_Amenaza == null).ToList().ForEach(
						 S =>
						 {
							this.repoMovDetalleInstalaciones.Delete(S);  
						 });
						
					});

				var listAmenazasQuitar = this.ListAmenazasPadreHijos.Where(x => x.activado == "Quitar").ToList();

				listAmenazasQuitar.ToList().ForEach(
					z =>
					{
						string[] codeHijo = z.IdpadreHijo.Split('-');

						int codeBien = codeHijo.Length == 1 ? Convert.ToInt32(z.IdpadreHijo) : Convert.ToInt32(codeHijo[1]);

						this.repoMovDetalleInstalaciones.GetAll().Where(x => x.ID_Bien == codeBien && x.ID_Instalacion == Convert.ToInt32(this.idInstalacion) && x.ID_Amenaza == z.Id).ToList().ForEach(
						p =>
						{
							this.repoMovDetalleInstalaciones.Delete(p);
						});

					});

				ListBienHijos.Where(x => x.activado == "Si").ToList().ForEach(
					c =>
					{
						if (!this.repoMovDetalleInstalaciones.GetAll().Where(x => x.ID_Bien == c.id_Bien_Padre && x.ID_Instalacion == Convert.ToInt32(this.idInstalacion)).ToList().Any())
						{
							MOV_Detalle_Instalacion movDetalle = new MOV_Detalle_Instalacion();
							movDetalle.ID_Bien                 = Convert.ToInt32(c.Id);
							movDetalle.ID_Instalacion          = Convert.ToInt32(this.idInstalacion);
							movDetalle.ID_Usu_Alta             = this.UsuarioFrontalSession.Usuario.id;
							movDetalle.Fech_Alta               = DateTime.Now;
							this.repoMovDetalleInstalaciones.Create(movDetalle);
						}        
					});
				
				

				var ListHijos = ListBienHijos.Where(x => x.activado == "Quitar").ToList();

				ListHijos.ToList().ForEach(
					z =>
					{
						var idInsta = Convert.ToInt32(this.idInstalacion);
						this.repoMovDetalleInstalaciones.Delete(c => c.ID_Bien == z.Id && c.ID_Instalacion == idInsta);

					});

				var ListPadres = this.ListBienesPadres.Where(x => x.activado == "Quitar" || x.activado == "No").ToList();

				ListPadres.ToList().ForEach(
						y =>
						{
							int code = Convert.ToInt32(this.idInstalacion);

							if (this.repoMovDetalleInstalaciones.Single(x => x.ID_Bien == y.Id && x.ID_Instalacion == code) != null)
							{
								int id_insta = Convert.ToInt32(this.idInstalacion);

								this.repoMovDetalleInstalaciones.Delete(x => x.ID_Bien == y.Id && x.ID_Instalacion == id_insta);
							}
							
						});

				return this.Json(new { result = true, Message = MensajeBienPadreHijo.MessageCategoria.ToDescription() });

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
		/// Editar datos de Organismos.
		/// </summary>
		[HttpPost]
		public ActionResult EditarInstalacion(int? id)
		{
			try
			{
				comboPuertos(null);

				ComboClasificacion();
				
				this.idInstalacion = (id == null ? this.idInstalacion : (int)id);

				MOV_Instalaciones mov_instalaciones = this.repoInstalaciones.Single(x => x.Id == this.idInstalacion);

				this.ComboProvincias(null);

				this.ComboCiudades(mov_instalaciones.id_provincia, null);

				comboTipoInstalaciones(mov_instalaciones.Clasificacion);

				ViewBag.disabled = true;
				
				ViewBag.action = General.EditGeneral.ToDescription();

				ViewBag.Observaciones = mov_instalaciones.Observaciones;

				ViewBag.Combo = "display:block";

				ViewBag.Texto = "display:none";

				ViewBag.ToolOperador = false;

				ViewBag.alta = "display:none";

				return this.PartialView("~/Views/Instalaciones/Visualizar.cshtml", mov_instalaciones);

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
		/// Alta-Editar Instalaciones.
		/// </summary>
		[HttpPost]
		public ActionResult AltaEditarInstalaciones(InstalacionJson instalacionJson)
		{
			try
			{

				IEnumerable<MOV_Instalaciones> result = null;

				switch (instalacionJson.action)
				{

					case "Alta":
						result = _serviciosTipoInstalacion.ListMOV_InstalacionesVerifica(instalacionJson.Nombre, instalacionJson.id_Puerto);
						bool resultado = false;
						foreach (MOV_Instalaciones Instalaciones in result)
						{
							if (Instalaciones.Es_Activo)
								resultado = true;
						}
						if (!resultado)
						{
							if (result.Count() == 0)
								AltaInstalacion(instalacionJson);
							else
								return this.Json(new { result = 2, Message = "Error al dar de alta la instalación, ya existe otra instalación activa " + instalacionJson.Nombre + "¿Desea continuar? " });
						}
						else
						{
							this.trazas(41, (int)this.UsuarioFrontalSession.Usuario.id, "Error al dar de alta la instalación, ya existe otra instalación activa " + instalacionJson.Nombre);
							return this.Json(new { result = 1, Message = "Error al dar de alta la instalación, ya existe otra instalación activa " + instalacionJson.Nombre });
						}
						break;

					default:
						this.idInstalacion = instalacionJson.Id;
						bool CambioEstado = _serviciosTipoInstalacion.ListMOV_Instalaciones(instalacionJson.Nombre, instalacionJson.Id, (instalacionJson.Activo == "1"));
						if (CambioEstado)
						{
							GuardarInstalacion(instalacionJson);
						}
						else
						{
							result = _serviciosTipoInstalacion.ListMOV_Instalaciones(instalacionJson.Nombre, instalacionJson.Id);
							if (result.Count() == 0)
							{
								GuardarInstalacion(instalacionJson);
							}
							else
							{
								IEnumerable<MOV_Instalaciones> Instalaciones = result.Where(x => x.Id != instalacionJson.Id);
								if (Instalaciones == null)
								{
									GuardarInstalacion(instalacionJson);
								}
								else
								{
									MOV_Instalaciones Actinstalacion = Instalaciones.FirstOrDefault();
									if (Actinstalacion.Id == instalacionJson.Id)
									{
										return this.Json(new { result = 3, Message = "Ya existe una Instalación " + instalacionJson.Nombre + " desactivo ¿Desea continuar? " });
									}
									else
									{
										if (!Actinstalacion.Es_Activo)
											return this.Json(new { result = 3, Message = "Ya existe una Instalación " + instalacionJson.Nombre + " desactivo ¿Desea continuar? " });
										else
										{
											this.trazas(44, (int)this.UsuarioFrontalSession.Usuario.id, "Error al modificar la instalación, ya existe otra instalación activo " + instalacionJson.Nombre);
											return this.Json(new { result = 1, Message = "Error al modificar la instalación, ya existe otra instalación activa  " + instalacionJson.Nombre });
										}
									}
								}
							}
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
	
		[HttpPost]
		public ActionResult CambiarProvincia(int? id)
		{
			try
			{

				this.idProvincia = id;

				this.ComboCiudades(this.idProvincia, null);

				return this.PartialView("~/Views/Instalaciones/ComboCiudad.cshtml");

			}
			catch (Exception ex)
			{
				this.log.PublishException(new SecureportException(ex));

				return this.Json(new { result = false, Message = ex.Message });
			}

		}
		///<summary>
		/// Permite el Alta de la instlación.
		///</summary>
		[HttpPost]
		public ActionResult create()
		{
			try
			{

				MOV_Instalaciones mov_instalaciones = new MOV_Instalaciones();

				comboPuertos(null);

				if (((MultiSelectList)(this.ViewData["puertos"])).Count() == 1)
					mov_instalaciones.id_Puerto = Convert.ToInt32(((SelectList)this.ViewData["puertos"]).FirstOrDefault().Value);

				ComboProvincias(null);

				ComboCiudades(0, null);

				ComboClasificacion();
				
				IEnumerable<ListadoTipoInstalaciones> tipoInstalaciones = new List<ListadoTipoInstalaciones>();

				SelectList selectList = new SelectList(tipoInstalaciones, "Id", "Descripcion", 0);

				this.ViewData["TipoInstalaciones"] = selectList;
				
				ViewBag.disabled = true;

				ViewBag.action = General.AltaGeneral.ToDescription();

				ViewBag.Observaciones = string.Empty;

				ViewBag.Combo = "display:block";

				ViewBag.Texto = "display:none";

				ViewBag.alta = "display:block";

			  return this.PartialView("~/Views/Instalaciones/Create.cshtml", mov_instalaciones);

			}
			catch (Exception ex)
			{
				this.log.PublishException(new SecureportException(ex));

				return this.Json(new { result = false, Message = ex.Message });
			}

		}
		/// <summary>
		/// Guardar la instalación.
		/// </summary>
		[HttpPost]
		public ActionResult GuardarInstalacion(InstalacionJson instalacionJson)
		{
			try
			{

				MOV_Instalaciones mov_instalacionesInicial = this.repoInstalaciones.Single(x => x.Id == this.idInstalacion);

				MOV_Instalaciones mov_instalaciones = this.repoInstalaciones.Single(x => x.Id == this.idInstalacion);

				mov_instalaciones.Nombre = instalacionJson.Nombre;

				mov_instalaciones.id_Puerto = instalacionJson.id_Puerto;

				mov_instalaciones.es_concesionada = instalacionJson.es_concesionada;

				mov_instalaciones.id_Tipo = instalacionJson.id_Tipo;

				mov_instalaciones.id_Ciudad = instalacionJson.id_Ciudad;

				mov_instalaciones.id_provincia = instalacionJson.id_provincia;

				mov_instalaciones.Direccion = instalacionJson.Direccion;

				mov_instalaciones.cod_postal = instalacionJson.cod_postal;

				mov_instalaciones.Observaciones = this.CambiarTexto(instalacionJson.Observaciones);

				mov_instalaciones.Empresa = instalacionJson.Empresa;

				mov_instalaciones.Clasificacion = instalacionJson.Clasificacion;

				mov_instalaciones.OMI = instalacionJson.OMI;

				mov_instalaciones.Nivel = instalacionJson.Nivel;

				mov_instalaciones.Longitud = instalacionJson.Longitud;

				mov_instalaciones.Latitud = instalacionJson.Latitud;

				mov_instalaciones.Declara_Cumpli = instalacionJson.Declara_Cumpli == "Si";

				mov_instalaciones.Fech_Declara_Cumpli = instalacionJson.Fech_Declara_Cumpli;

				mov_instalaciones.Es_Activo = (instalacionJson.Activo == "1" || instalacionJson.Activo == "Si");

				this.repoInstalaciones.Update(mov_instalaciones);

				this.trazas(62, (int)this.UsuarioFrontalSession.Usuario.id, "Se ha modificado un registro en SPMOV_Instalaciones con ID" + "(" + instalacionJson.Id.ToString() + ")");

				bool verifica = false;
				if (mov_instalacionesInicial.id_Ciudad != mov_instalaciones.id_Ciudad)
					verifica = true;
				else if(mov_instalacionesInicial.id_provincia != mov_instalaciones.id_provincia)
					verifica = true;
				else if(mov_instalacionesInicial.id_Puerto != mov_instalaciones.id_Puerto)
					verifica = true;
				else if(mov_instalacionesInicial.id_Tipo != mov_instalaciones.id_Tipo)
					verifica = true;
				else if(mov_instalacionesInicial.Clasificacion != mov_instalaciones.Clasificacion)
					verifica = true;
				else if(mov_instalacionesInicial.cod_postal != mov_instalaciones.cod_postal)
					verifica = true;
				else if(mov_instalacionesInicial.Declara_Cumpli != mov_instalaciones.Declara_Cumpli)
					verifica = true;
				else if(mov_instalacionesInicial.Direccion != mov_instalaciones.Direccion)
					verifica = true;
				else if(mov_instalacionesInicial.Empresa != mov_instalaciones.Empresa)
					verifica = true;
				else if(mov_instalacionesInicial.Es_Activo != mov_instalaciones.Es_Activo)
					verifica = true;
				else if(mov_instalacionesInicial.es_concesionada != mov_instalaciones.es_concesionada)
					verifica = true;
				else if(mov_instalacionesInicial.Fech_Declara_Cumpli != mov_instalaciones.Fech_Declara_Cumpli)
					verifica = true;
				else if(mov_instalacionesInicial.Latitud != mov_instalaciones.Latitud)
					verifica = true;
				else if(mov_instalacionesInicial.Longitud != mov_instalaciones.Longitud)
					verifica = true;
				else if(mov_instalacionesInicial.Nivel != mov_instalaciones.Nivel)
					verifica = true;
				else if(mov_instalacionesInicial.Nombre != mov_instalaciones.Nombre)
					verifica = true;
				else if(mov_instalacionesInicial.Observaciones != mov_instalaciones.Observaciones)
					verifica = true;
				else if(mov_instalacionesInicial.OMI != mov_instalaciones.OMI)
					verifica = true;

				if (verifica)
				{
					Puertos puerto = repoPuertos.Single(x => x.Id == (int)mov_instalaciones.id_Puerto);
					this.EnviarCorreoLocal(puerto.Nombre, mov_instalaciones.Nombre, "Modificar");
				}

				return this.Json(new { result = true, Message = "Datos guardados correctamente." });
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
		/// Alta de Instalación
		/// </summary>
		[HttpPost]
		public ActionResult AltaInstalacion(InstalacionJson instalacionJson)
		{
			try
			{

				MOV_Instalaciones mov_instalaciones = new MOV_Instalaciones();

				mov_instalaciones.Nombre = instalacionJson.Nombre;

				mov_instalaciones.id_Puerto = instalacionJson.id_Puerto;

				mov_instalaciones.es_concesionada = instalacionJson.es_concesionada; 

				mov_instalaciones.id_Tipo = instalacionJson.id_Tipo;

				mov_instalaciones.id_Ciudad = instalacionJson.id_Ciudad;

				mov_instalaciones.id_provincia = instalacionJson.id_provincia;

				mov_instalaciones.Direccion = instalacionJson.Direccion;

				mov_instalaciones.cod_postal = instalacionJson.cod_postal;

				mov_instalaciones.Observaciones = this.CambiarTexto(instalacionJson.Observaciones);

				mov_instalaciones.Empresa = instalacionJson.Empresa;

				mov_instalaciones.Clasificacion = instalacionJson.Clasificacion;

				mov_instalaciones.OMI = instalacionJson.OMI;

				mov_instalaciones.Nivel = instalacionJson.Nivel;

				mov_instalaciones.Longitud = instalacionJson.Longitud;

				mov_instalaciones.Latitud = instalacionJson.Latitud;

				mov_instalaciones.Declara_Cumpli = instalacionJson.Declara_Cumpli == "Si";

				mov_instalaciones.Fech_Declara_Cumpli = instalacionJson.Fech_Declara_Cumpli;
				
				mov_instalaciones.id_usu_alta = this.UsuarioFrontalSession.Usuario.id;

				mov_instalaciones.Es_Activo = true;

				mov_instalaciones.fech_alta = DateTime.Now.Date;

				mov_instalaciones.fech_Activo = DateTime.Now.Date;

				this.repoInstalaciones.Create(mov_instalaciones);

				this.idInstalacion = this.db.MOV_Instalaciones.OrderByDescending(x => x.Id).FirstOrDefault().Id;

				this.trazas(59, (int)this.UsuarioFrontalSession.Usuario.id, "Se ha dado de alta un registro en SPMOV_Instalaciones con ID" + "(" + this.idInstalacion.ToString() + ")");

				List<DependenciasUsuario> ListDependenciasDepenUsuarios = this._serviciosUsuarios.GetDependenciaUsuarios(this.UsuarioFrontalSession.Usuario.id, this.UsuarioFrontalSession.Usuario.categoria);

				this.UsuarioFrontalSession.ListDependenciasDepenUsuarios = ListDependenciasDepenUsuarios;

				Puertos puerto = repoPuertos.Single(x => x.Id == (int)mov_instalaciones.id_Puerto);

				this.EnviarCorreoLocal(puerto.Nombre, mov_instalaciones.Nombre, "Alta");

				return this.Json(new { result = true, Message = "Datos guardados correctamente." });
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
		/// Borrar Instalaciones.
		/// </summary>
		[HttpPost]
		public ActionResult EliminarInstalaciones(int Id)
		{
			try
			{
				MOV_Instalaciones instalacion = this.repoInstalaciones.Single(x => x.Id == Id);

				this.repoInstalaciones.Delete(instalacion);

				this.trazas(61, (int)this.UsuarioFrontalSession.Usuario.id, "Se ha realizado la eliminación de un registro en SPMOV_Instalaciones con ID" + "(" + Id.ToString() + ")");

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

				return this.Json(new { result = false, Message = "Instrucción DELETE en conflicto con la restricción REFERENCE." });
			}

			return this.Json(new { result = true });
		}

		[HttpPost]
		public ActionResult VisualizarInstalacion(int? id)
		{
			try
			{
				this.idInstalacion = (id == null ? this.idInstalacion : (int)id);

				comboPuertos(null);

				ComboProvincias(null);

				ComboCiudades(0, null);

				ComboClasificacion();

				ComboTipos_Instalacion();

				MOV_Instalaciones mov_instalaciones = this.repoInstalaciones.Single(x => x.Id == this.idInstalacion);

				this.idClasificacion = mov_instalaciones.Clasificacion;

				this.ViewBag.Observaciones = mov_instalaciones.Observaciones;

				ViewBag.disabled = false;

				ViewBag.Combo = "display:none";

				ViewBag.Texto = "display:block";

				ViewBag.ToolOperador = true;

				ViewBag.alta = "display:none";

				return this.PartialView("~/Views/Instalaciones/Visualizar.cshtml", mov_instalaciones);
			}
			catch (Exception ex)
			{
				this.log.PublishException(new SecureportException(ex));

				return this.Json(new { result = false, Message = ex.Message });
			}
		}

		public void EnviarCorreoLocal(string Puerto, string Instalacion, string Tipo)
		{
			try
			{
				string CorreoA = ConfigurationManager.AppSettings["CorreoInstalaciones"];
				string Texto = string.Empty;
				switch (Tipo)
				{
					case "Alta":
						Texto = "Se le notifica que ha sido dado de alta una IIPP con los siguientes datos:  \r\n\r\n IIPP: " + Instalacion + " \r\n Puerto: " + Puerto + ". \r\n";
						break;
					case "Modificar":
						Texto = "Se le notifica que ha sido modificado  una IIPP con los siguientes datos:  \r\n\r\n IIPP: " + Instalacion + " \r\n Puerto: " + Puerto + ". \r\n";
						break;
					case "Baja":
						Texto = "Se le notifica que ha sido eliminado  una IIPP con los siguientes datos:  \r\n\r\n IIPP: " + Instalacion + " \r\n Puerto: " + Puerto + ". \r\n";
						break;
				}

				Texto += "\r\n Deberá comprobar las modificaciones realizadas.";
				string Asunto = "SecurePort – " + (Tipo == "Alta" ? "Alta" : (Tipo == "Modificar" ? "Modificación" : "Baja")) + " Datos Instalaciones";
				this.EnviarCorreoPuertos(Asunto, Texto, CorreoA);
			}
			catch (Exception ex)
			{
				this.log.PublishException(new SecureportException(ex));
			}
		}


		#endregion
	}
}