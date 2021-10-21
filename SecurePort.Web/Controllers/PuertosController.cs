
namespace SecurePort.Web.Views
{

	#region Using
	using System.Data.Entity.Infrastructure;
	using System.Data.Entity.ModelConfiguration;
	using Kendo.Mvc.Extensions;
	using Kendo.Mvc.UI;
	using SecurePort.Entities.Enums;
	using SecurePort.Entities.Exceptions;
	using SecurePort.Entities.Models;
	using SecurePort.Entities.Models.Json;
	using SecurePort.Services.Interfaces;
	using SecurePort.Services.Repository;
	using SecurePort.Web.Controllers;
	using System;
	using System.Collections.Generic;
	using System.Data.Entity.Validation;
	using System.Linq;
	using System.Web.Mvc;
	using System.Configuration;
	#endregion

	public class PuertosController : BaseController
	{

		public PuertosController(IServiciosPuertos serviciosPuertos,
								 PuertosRepository repoPuertos,
								 IServiciosPaisesProvincias serviciosPaisesProvincias,
								 IServiciosOrganismo servicioOrganismos,
								 ProvinciasRepository repoProvincias,
								 CiudadesRepository repoCiudades,  
								 OperadoresPuertoRepository repoOperadoresPuerto,
								 IServiciosUsuarios servicioUsuarios,
								 TrazasRepository repotrazas, 
								 ILogger logger)
					: base(serviciosPuertos, 
						   repoPuertos, 
						   serviciosPaisesProvincias, 
						   servicioOrganismos, 
						   repoProvincias, 
						   repoCiudades, 
						   repoOperadoresPuerto, 
						   servicioUsuarios,
						   repotrazas, 
						   logger)
		{
		}

		#region Buscador
		/// <summary>
		/// Permite buscar los diferentes Bienes
		/// </summary>
		public ActionResult BuscadorPuertos([DataSourceRequest]DataSourceRequest request)
		{
			try
			{
				if (this.UsuarioFrontalSession.Usuario.categoria == (int)EnumCategoria.Administrador
					|| this.UsuarioFrontalSession.Usuario.categoria == (int)EnumCategoria.PuertosdelEstado)
				{
					return this.Json(this._serviciosPuertos.GetAllPuertos().ToDataSourceResult(request));
				}
				var categoria = this.UsuarioFrontalSession.Usuario.categoria;
				return this.Json(this._serviciosPuertos.GetAllPuertos(this.UsuarioFrontalSession.ListDependenciasDepenUsuarios,categoria).ToDataSourceResult(request));
			}
			catch (Exception ex)
			{
				this.log.PublishException(new SecureportException(ex));
				throw;
			}
		}
		#endregion

		#region action

		#region Puertos
		/// <summary>
		/// Retorna una lista de todos los Puertos.
		/// </summary>
		public ActionResult ListadoPuertos()
		{
			try
			{
				ViewBag.Mensaje = "Mantenimiento de  Puertos";
				return this.PartialView("ListadoPuertos", this.UsuarioFrontalSession);
			}
			catch (Exception ex)
			{
				this.log.PublishException(new SecureportException(ex));
				throw;
			}
		}


		[HttpPost]
		public ActionResult CambiarProvincia(int? id)
		{
			try
			{

				this.idProvincia = id;

				this.ComboCiudades(this.idProvincia, null);

				this.ViewData["Isla"]  = string.Empty;

				return this.PartialView("~/Views/Puertos/ComboCiudad.cshtml");

			}
			catch (Exception ex)
			{
				this.log.PublishException(new SecureportException(ex));

				return this.Json(new { result = false, Message = ex.Message });
			}

		}

		[HttpPost]
		public ActionResult CambiarCiudad(int? id)
		{
			try
			{

				this.ViewData["Isla"] = this._serviciosPuertos.TraerIsla(id);
				return this.PartialView("~/Views/Puertos/ComboIsla.cshtml");

			}
			catch (Exception ex)
			{
				this.log.PublishException(new SecureportException(ex));

				return this.Json(new { result = false, Message = ex.Message });
			}

		}

		///<summary>
		/// Permite el Alta del usuario.
		///</summary>
		[HttpPost]
		public ActionResult Create()
		{
			try
			{
				PuertosViewModel puerto = new PuertosViewModel();

				puerto.es_activo = true;

				this.comboOrganismos(null);

				if (((MultiSelectList)(this.ViewData["organismos"])).Count() == 1)
					puerto.id_Organismo = Convert.ToInt32(((SelectList)this.ViewData["organismos"]).FirstOrDefault().Value);

				this.ComboCiudades(null, null);

				this.ComboProvincias(null);

				this.ComboCapitanias(null);

				ViewBag.Combo = "display:block";
				
				ViewBag.Texto = "display:none";
			   
				ViewBag.disabled = true;

				ViewBag.display = "display:none";

				ViewBag.Mensaje = "Alta Puertos";

				ViewBag.ToolOperador = true;

				ViewBag.Operador = "display:none";

				ViewBag.action = General.AltaGeneral.ToDescription();

				this.ViewBag.Observaciones = string.Empty;

				return this.PartialView("~/Views/Puertos/_PartialPuertos.cshtml",puerto);
				

			}
			catch (Exception ex)
			{
				this.log.PublishException(new SecureportException(ex));

				return this.Json(new { result = false, Message = ex.Message });
			}

		}


		/// <summary>
		/// Alta-Editar Bienes.
		/// </summary>
		[HttpPost]
		public ActionResult AltaEditarPuertos(PuertosJson puertosJson)
		{
			try
			{
				IEnumerable<Puertos> result = null;

				switch (puertosJson.action)
				{

					case "Alta":
						result = _serviciosPuertos.ListPuertos(puertosJson.Nombre);
						bool resultado = false;
						foreach (Puertos Puerto in result)
						{
							if (Puerto.es_activo == true)
								resultado = true;
						}
						if (!resultado)
						{
							if (result.Count() == 0)
								AltaPuerto(puertosJson);
							else
								return this.Json(new { result = 2, Message = "Ya existe un Puerto " + puertosJson.Nombre + " desactivado ¿Desea continuar? " });
						}
						else
						{
							this.trazas(50, (int)this.UsuarioFrontalSession.Usuario.id, "Error al dar de alta el puerto, ya existe otro puerto activo " + puertosJson.Nombre );
							return this.Json(new { result = 1, Message = "Error al dar de alta el puerto, ya existe otro puerto activo " + puertosJson.Nombre });
						}
						break;

					default:
						result = _serviciosPuertos.ListPuertos(puertosJson.Nombre, puertosJson.Id);
						if (result.Count() == 0)
						{
							EditarPuerto(puertosJson);
						}
						else
						{
							IEnumerable<Puertos> puerto = result.Where(x => x.Id != puertosJson.Id);
							if (puerto == null)
							{
								EditarPuerto(puertosJson);
							}
							else
							{
								Puertos ActPuerto = puerto.FirstOrDefault();
								if (ActPuerto.Id == puertosJson.Id)
								{
									return this.Json(new { result = 3, Message = "Ya existe un puerto " + puertosJson.Nombre + " desactivado ¿Desea continuar? " });                                    
								}
								else
								{
									if (ActPuerto.es_activo == false)
										return this.Json(new { result = 3, Message = "Ya existe un puerto " + puertosJson.Nombre + " desactivado ¿Desea continuar? " });
									else
									{
										this.trazas(53, (int)this.UsuarioFrontalSession.Usuario.id, "Error al modificar el puerto, ya existe otro puerto activo " + puertosJson.Nombre + "(" + puertosJson.Id + ")");
										return this.Json(new { result = 1, Message = "Error al modificar el puerto, ya existe otro puerto activo " + puertosJson.Nombre });
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
		public ActionResult AltaPuerto(PuertosJson puertosJson)
		{
			try
			{
				Puertos puertos = new Puertos();

				puertos.ID_Usu_alta     = this.UsuarioFrontalSession.Usuario.id;

				puertos.es_activo       = true;

				puertos.fech_activo     = DateTime.Now;

				puertos.Fech_Alta       = DateTime.Now;

				puertos.Nombre          = puertosJson.Nombre;

				puertos.id_Organismo    = puertosJson.id_Organismo;

				puertos.Responsable     = puertosJson.Responsable;

				puertos.Direccion       = puertosJson.Direccion;

				puertos.ID_Provincia    = puertosJson.Id_Provincia == 0 ? null : puertosJson.Id_Provincia;

				puertos.ID_Ciudad       = puertosJson.Id_Ciudad == 0 ? null : puertosJson.Id_Ciudad;

				puertos.Cod_Postal      = puertosJson.Cod_Postal;

				puertos.Id_CapMarit     = puertosJson.Id_CapMarit == 0 ? null : puertosJson.Id_CapMarit;

				puertos.Observaciones   = this.CambiarTexto(puertosJson.Observaciones);

				puertos.Latitud         = puertosJson.Latitud == null ? 0 : puertosJson.Latitud;

				puertos.Longitud        = puertosJson.Longitud == null ? 0 : puertosJson.Longitud;

				puertos.Locode          = puertosJson.Locode;

				this.repoPuertos.Create(puertos);

				 this.idPuerto = this.db.Puertos.OrderByDescending(x => x.Id).FirstOrDefault().Id;

				 this.trazas(50, (int)this.UsuarioFrontalSession.Usuario.id, "Se ha dado de alta un registro en SPMAE_Puertos con ID " + "(" + this.idPuerto.ToString() + ")");

				List<DependenciasUsuario> ListDependenciasDepenUsuarios = this._serviciosUsuarios.GetDependenciaUsuarios(this.UsuarioFrontalSession.Usuario.id, this.UsuarioFrontalSession.Usuario.categoria);

						
				this.UsuarioFrontalSession.ListDependenciasDepenUsuarios = ListDependenciasDepenUsuarios;

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
		public ActionResult EditarPuerto(PuertosJson puertosJson)
		{
			try
			{
				Puertos puerto          = this.repoPuertos.Single(x => x.Id == puertosJson.Id);

				puerto.Nombre           = puertosJson.Nombre;

				puerto.id_Organismo     = puertosJson.id_Organismo;

				puerto.Responsable      = puertosJson.Responsable;

				puerto.Direccion        = puertosJson.Direccion;

				puerto.ID_Provincia     = puertosJson.Id_Provincia == 0 ? null : puertosJson.Id_Provincia;

				puerto.ID_Ciudad        = puertosJson.Id_Ciudad == 0 ? null : puertosJson.Id_Ciudad;

				puerto.Cod_Postal       = puertosJson.Cod_Postal;

				puerto.Id_CapMarit      = puertosJson.Id_CapMarit == 0 ? null : puertosJson.Id_CapMarit;

				puerto.Observaciones    = this.CambiarTexto(puertosJson.Observaciones);

				puerto.es_activo        = (puertosJson.activo == "1" ? true : false);

				puerto.Latitud          = puertosJson.Latitud == null ? null : puertosJson.Latitud;

				puerto.Longitud         = puertosJson.Longitud == null ? null : puertosJson.Longitud;

				puerto.Locode           = puertosJson.Locode;

				this.repoPuertos.Update(puerto);

				this.trazas(53, (int)this.UsuarioFrontalSession.Usuario.id, "Se ha realizado la modificación de un registro en SPMAE_Puertos con ID" + "(" + puertosJson.Id + ")");

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
		public ActionResult EliminarPuerto(int Id)
		{
			try
			{
				this.repoPuertos.Delete(this.repoPuertos.Single(x => x.Id == Id));

				this.trazas(52, (int)this.UsuarioFrontalSession.Usuario.id, "Se ha realizado la eliminación de un registro en SPMAE_Pueros con ID" + "("+Id.ToString()+")");

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
		public ActionResult VisualizarPuertos(int? id, string Accion)
		{
			try
			{
				if (id != null)
					this.idPuerto = (int)id;
				else if (this.idPuerto != 0)
					id = this.idPuerto;


				IEnumerable<PuertosViewModel> listpuertos = this._serviciosPuertos.GetAllPuertos().Where(x => x.Id == id);

				if (Accion == General.EditGeneral.ToDescription())
				{
					ViewBag.disabled = true;
					ViewBag.Combo = "display:block";
					ViewBag.Texto = "display:none";
					ViewBag.Mensaje = "Editar Puertos";
					ViewBag.ToolOperador = false;                    
				}
				else
				{
					ViewBag.disabled = false;
					ViewBag.Combo = "display:none";
					ViewBag.Texto = "display:block";
					ViewBag.Mensaje = "Visualizar Puertos";
					ViewBag.ToolOperador = true;                    
				}
				ViewBag.display = "display:block";
				ViewBag.Operador = "display:block";

				ViewBag.action = General.EditGeneral.ToDescription();

				this.ViewBag.navegador = this.Browser;

				PuertosViewModel puertos = new PuertosViewModel();

				listpuertos.ToList().ForEach(
				   p =>
				   {
					   puertos.Id           = p.Id;
					   puertos.Nombre       = p.Nombre;
					   puertos.id_Organismo = p.id_Organismo;
					   puertos.Responsable  = p.Responsable;
					   puertos.Direccion    = p.Direccion;
					   puertos.ID_Provincia = p.ID_Provincia;
					   puertos.ID_Ciudad    = p.ID_Ciudad;
					   puertos.Id_CapMarit  = p.Id_CapMarit;
					   puertos.Cod_Postal   = p.Cod_Postal;
					   puertos.Organismo    = p.Organismo;
					   this.ViewBag.Observaciones = p.Observaciones;
					   puertos.Provincia    = p.Provincia;
					   puertos.Ciudad       = p.Ciudad;
					   puertos.Capitania    = p.Capitania;
					   puertos.es_activo    = p.es_activo;
					   puertos.Latitud      = p.Latitud;
					   puertos.Longitud     = p.Longitud;
					   puertos.Locode       = p.Locode;
					   puertos.Isla         = p.Isla;
				   });


				this.comboOrganismos(null);

				this.ComboProvincias(null);

				this.ComboCiudades(null, puertos.ID_Ciudad);                

				this.ComboCapitanias(null);

				return this.PartialView("~/Views/Puertos/_PartialPuertos.cshtml", puertos);
			}
			catch (Exception ex)
			{
				this.log.PublishException(new SecureportException(ex));

				return this.Json(new { result = false, Message = ex.Message });
			}
		}

		#endregion

		#region Operadores

		#region action
	   
		#endregion

		/// <summary>
		/// Permite buscar las instalaciones
		/// </summary>
		public ActionResult BuscadorOperadores([DataSourceRequest]DataSourceRequest request)
		{
			try
			{
				IEnumerable<OperadoresPuertoViewModel> result = this._serviciosPuertos.GetAllOperadoresPuerto(this.idPuerto);

				return this.Json(result.ToDataSourceResult(request));
			}
			catch (Exception ex)
			{
				this.log.PublishException(new SecureportException(ex));
				throw;
			}
		}

		public ActionResult OperadoresEdit(bool ToolBar)
		{
			this.UsuarioFrontalSession.valor = ToolBar;


			return PartialView("~/Views/Puertos/_PartialOperadores.cshtml", this.UsuarioFrontalSession);

		}

		public ActionResult OperadoresEditar(int Id)
		{
			return PartialView("~/Views/Puertos/_PartialOperadores.cshtml", this.UsuarioFrontalSession);
		}


		/// <summary>
		/// Permite visualizar los datos de un usuario con documentos.
		/// </summary>
		[HttpPost]
		public ActionResult AsignarOperador()
		{
			try
			{
				ViewBag.Combo = "display:block";

				ViewBag.Texto = "display:none";

				ViewBag.Operadordisabled = true;

				ViewBag.display = "display:none";

				ViewBag.Mensaje = "Alta Contacto Puerto";

				ViewBag.actionOperador = "Alta";

				OperadoresPuertoViewModel operadores = new OperadoresPuertoViewModel();

				operadores.Id = 1;

				operadores.Id_Puerto = (int)this.idPuerto;

				this.ViewBag.Observacion = string.Empty;

				this.ComboCiudades(null, null);

				this.ComboProvincias(null);

				return this.PartialView("~/Views/Puertos/_PartialAsignarOperador.cshtml", operadores);

			}
			catch (Exception ex)
			{
				this.log.PublishException(new SecureportException(ex));

				return this.Json(new { result = false, Message = ex.Message });
			}
		}

		/// <summary>
		/// Alta-Editar operador puerto.
		/// </summary>
		[HttpPost]
		public ActionResult AltaEditarOperador(OperadoresPuertoJson operadoresPuertoJson)
		{
			try
			{
				//IEnumerable<Operadores> result = null;

				switch (operadoresPuertoJson.action)
				{

					case "Alta":
						
						AltaOperador(operadoresPuertoJson);
						
						break;

					default:
						
						EditarOperador(operadoresPuertoJson);
						
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
		/// Borrar Operadores.
		/// </summary>
		[HttpPost]
		public ActionResult EliminarOperador(int Id)
		{
			try
			{
				OperadoresPuerto operador = this.repoOperadoresPuerto.Single(x => x.Id == Id);

				this.repoOperadoresPuerto.Delete(operador);

				this.trazas(56, (int)this.UsuarioFrontalSession.Usuario.id, "Se ha realizado la eliminación de un registro en SPMAE_Operadores_Puerto " + operador.Id);

				Puertos puerto = this.repoPuertos.Single(x => x.Id == operador.Id_puerto);

				EnviarCorreoLocal(puerto.Nombre, operador.Nombre, "Baja");

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
		public ActionResult AltaOperador(OperadoresPuertoJson operadoresPuertoJson)
		{
			try
			{
				OperadoresPuerto operador = new OperadoresPuerto();

				operador.Id_usu_alta = this.UsuarioFrontalSession.Usuario.id;

				operador.Es_Activo = true;

				operador.Fech_activo = DateTime.Now;

				operador.Fech_alta = DateTime.Now;

				operador.Nombre = operadoresPuertoJson.Nombre;

				operador.Es_Suplente = operadoresPuertoJson.Es_Suplente;

				operador.Telefono = operadoresPuertoJson.Telefono;

				operador.Fax = operadoresPuertoJson.Fax;

				operador.Email = operadoresPuertoJson.Email;

				operador.Id_puerto = operadoresPuertoJson.Id_Puerto;

				operador.Observaciones = (operadoresPuertoJson.Observaciones == null ? string.Empty : operadoresPuertoJson.Observaciones).Replace("&amp;aacute;", "á").Replace("&amp;eacute;", "é").Replace("&amp;iacute;", "í").Replace("&amp;oacute;", "ó").Replace("&amp;uacute;", "ú").Replace("&nbsp;", " ").Trim();

				operador.Direccion = operadoresPuertoJson.Direccion;

				operador.ID_Provincia = operadoresPuertoJson.ID_Provincia;

				operador.ID_Ciudad = operadoresPuertoJson.ID_Ciudad;

				operador.Cod_Postal = operadoresPuertoJson.Cod_Postal;

				this.repoOperadoresPuerto.Create(operador);

				this.trazas(54, (int)this.UsuarioFrontalSession.Usuario.id, "Se ha dado de alta un registro en SPMAE_Operadores_Puerto " + operadoresPuertoJson.Nombre);

				Puertos puerto = this.repoPuertos.Single(x => x.Id == operadoresPuertoJson.Id_Puerto);

				EnviarCorreoLocal(puerto.Nombre, operadoresPuertoJson.Nombre, "Alta");


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
		public ActionResult EditarOperador(OperadoresPuertoJson operadoresPuertoJson)
		{
			try
			{
				OperadoresPuerto operadorInicial = this.repoOperadoresPuerto.Single(x => x.Id == operadoresPuertoJson.Id);

				OperadoresPuerto operador = this.repoOperadoresPuerto.Single(x => x.Id == operadoresPuertoJson.Id);

				operador.Nombre = operadoresPuertoJson.Nombre;

				operador.Es_Suplente = operadoresPuertoJson.Es_Suplente;

				operador.Telefono = operadoresPuertoJson.Telefono;

				operador.Fax = operadoresPuertoJson.Fax;

				operador.Email = operadoresPuertoJson.Email;

				operador.Observaciones = (operadoresPuertoJson.Observaciones == null ? string.Empty : operadoresPuertoJson.Observaciones).Replace("&amp;aacute;", "á").Replace("&amp;eacute;", "é").Replace("&amp;iacute;", "í").Replace("&amp;oacute;", "ó").Replace("&amp;uacute;", "ú").Replace("&nbsp;", " ").Trim();

				operador.Direccion = operadoresPuertoJson.Direccion;

				operador.ID_Provincia = operadoresPuertoJson.ID_Provincia;

				operador.ID_Ciudad = operadoresPuertoJson.ID_Ciudad;

				operador.Cod_Postal = operadoresPuertoJson.Cod_Postal;

				operador.Es_Activo = operadoresPuertoJson.Es_Activo;

				this.repoOperadoresPuerto.Update(operador);

				this.trazas(57, (int)this.UsuarioFrontalSession.Usuario.id, "Se ha realizado la modificación de un registro en SPMAE_Operadores_Puerto " + operadoresPuertoJson.Nombre + "(" + operadoresPuertoJson.Id + ")");

				bool Verificacambio = false;
				if (operador.Nombre != operadorInicial.Nombre)
					Verificacambio = true;
				else if (operador.Es_Suplente != operadorInicial.Es_Suplente)
					Verificacambio = true;
				else if (operador.Telefono != operadorInicial.Telefono)
					Verificacambio = true;
				else if (operador.Fax != operadorInicial.Fax)
					Verificacambio = true;
				else if (operador.Email != operadorInicial.Email)
					Verificacambio = true;
				else if (operador.Es_Activo != operadorInicial.Es_Activo)
					Verificacambio = true;

				if(Verificacambio){
				   Puertos puerto = this.repoPuertos.Single(x => x.Id == operador.Id_puerto);

					EnviarCorreoLocal(puerto.Nombre, operadoresPuertoJson.Nombre, "Modificar");
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
		public ActionResult VisualizarOperador(int id, string Accion)
		{
			try
			{
				IEnumerable<OperadoresPuertoViewModel> listOperdores = this._serviciosPuertos.GetAllOperadoresPuerto(this.idPuerto).Where(x => x.Id == id);

				if (Accion == General.EditGeneral.ToDescription())
				{
					ViewBag.Operadordisabled = true;
					ViewBag.Combo = "display:block";
					ViewBag.Texto = "display:none";
					ViewBag.Mensaje = "Editar Contacto Puerto";
				
					
				}
				else
				{
					ViewBag.Operadordisabled = false;
					ViewBag.Combo = "display:none";
					ViewBag.Texto = "display:block";
					ViewBag.Mensaje = "Visualizar Contacto Puerto";
				
				}
				ViewBag.display = "display:block";

				ViewBag.action = General.EditGeneral.ToDescription();

				this.ViewBag.navegador = this.Browser;


				OperadoresPuertoViewModel operador = new OperadoresPuertoViewModel();

				listOperdores.ToList().ForEach(
				   p =>
				   {
					   operador.Id = p.Id;
					   operador.Es_Suplente = p.Es_Suplente;
					   operador.Es_Activo = p.Es_Activo;
					   operador.Telefono = p.Telefono;
					   operador.Fax = p.Fax;
					   operador.Email = p.Email;
					   operador.Nombre = p.Nombre;
					   this.ViewBag.Observacion = p.Observaciones;
					   operador.Cod_Postal = p.Cod_Postal;
					   operador.ID_Ciudad = p.ID_Ciudad;
					   operador.Ciudad = p.Ciudad;
					   operador.ID_Provincia = p.ID_Provincia;
					   operador.Provincia = p.Provincia;
					   operador.Direccion = p.Direccion;
					   operador.activado = p.activado;

				   });

				this.ComboProvincias(null);

				this.ComboCiudades(null, operador.ID_Ciudad);


				return this.PartialView("~/Views/Puertos/_PartialAsignarOperador.cshtml", operador);
			}
			catch (Exception ex)
			{
				this.log.PublishException(new SecureportException(ex));

				return this.Json(new { result = false, Message = ex.Message });
			}
		}

		[HttpPost]
		public ActionResult CambiarProvinciaOperador(int? id)
		{
			try
			{

				this.idProvincia = id;

				this.ComboCiudades(this.idProvincia, null);

				return this.PartialView("~/Views/Puertos/ComboCiudadOperador.cshtml");

			}
			catch (Exception ex)
			{
				this.log.PublishException(new SecureportException(ex));

				return this.Json(new { result = false, Message = ex.Message });
			}

		}

		public void EnviarCorreoLocal(string Puerto, string Operador, string Tipo)
		{
			try
			{
				string CorreoA = ConfigurationManager.AppSettings["CorreoPuertos"];
				string Texto = string.Empty;
				switch (Tipo)
				{
					case "Alta":
						Texto = "Se le notifica que ha sido dado de alta un contacto de puerto con los siguientes datos: \r\n\r\n Puerto: " + Puerto + ", \r\n Contacto " + Operador + ". \r\n";
						break;
					case "Modificar":
						Texto = "Se le notifica que ha sido modificado un contacto de puerto con los siguientes datos: \r\n\r\n Puerto: " + Puerto + ", \r\n Contacto " + Operador + ". \r\n";
						break;
					case "Baja":
						Texto = "Se le notifica que ha sido eliminado  un contacto de puerto con los siguientes datos: \r\n\r\n Puerto: " + Puerto + ", \r\n Contacto " + Operador + ". \r\n";
						break;
				}
				
				Texto += "\r\n Deberá comprobar las modificaciones realizadas.";
				string Asunto = "SecurePort – " + (Tipo == "Alta" ? "Alta" : (Tipo == "Modificar" ? "Modificación" : "Baja")) + " Datos Contactos Puerto";
				this.EnviarCorreoPuertos(Asunto, Texto, CorreoA);
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
