
namespace SecurePort.Web.Controllers
{
	#region Using

	using System.Security.Cryptography.X509Certificates;

	using Kendo.Mvc.Extensions;
	using Kendo.Mvc.UI;
	using SecurePort.Entities.Enums;
	using SecurePort.Entities.Exceptions;
	using SecurePort.Entities.Models;
	using SecurePort.Entities.Models.Json;
	using SecurePort.Services.Interfaces;
	using SecurePort.Services.Repository;
	using System;
	using System.Collections.Generic;
	using System.Data.Entity.Infrastructure;
	using System.Data.Entity.ModelConfiguration;
	using System.Data.Entity.Validation;
	using System.Linq;
	using System.Web.Mvc;
	#endregion

	public class OrganismoController : BaseController
	{
		public OrganismoController(
						Comunidades_AutonomasRepository repoComunidades_Autonomas,
						OrganismoRepository  repoOrganismo,
						ProvinciasRepository repoProvincias,
						CiudadesRepository   repoCiudades,
						Contactos_OrganismoRepository repoContactos_Organismo,
						IServiciosPaisesProvincias serviciosPaisesProvincias,
						IServiciosOrganismo serviciosOrganismos,
						IServiciosUsuarios serviciosUsuarios, 
						TrazasRepository repoTrazas,
						ILogger log)

			: base(repoComunidades_Autonomas,
				   repoOrganismo,
				   repoProvincias,
				   repoCiudades,
				   repoContactos_Organismo,
				   serviciosPaisesProvincias,
				   serviciosOrganismos,
				   serviciosUsuarios,
				   repoTrazas,
				   log)
		{
		}
		
		//
		// GET: /Organismo/
		public ActionResult Index()
		{
			return View();
		}

		#region Buscador

		public ActionResult Buscador([DataSourceRequest]DataSourceRequest request)
		{
			if (this.UsuarioFrontalSession.Usuario.categoria == (int)EnumCategoria.Administrador
				|| this.UsuarioFrontalSession.Usuario.categoria == (int)EnumCategoria.PuertosdelEstado)
			{
				return Json(this._serviciosOrganismos.GetAllOrganismos().ToDataSourceResult(request));
			}
			var categoria = this.UsuarioFrontalSession.Usuario.categoria;		
			return Json(this._serviciosOrganismos.GetAllOrganismos(this.UsuarioFrontalSession.ListDependenciasDepenUsuarios,categoria).ToDataSourceResult(request));
		}


		/// <summary>
		/// Permite buscar por contacto.
		/// </summary>
		public ActionResult BuscadorContacto([DataSourceRequest]DataSourceRequest request)
		{
			try
			{

				IEnumerable<ContactosViewModel> contactos_organismo = this._serviciosOrganismos.GetAllContactos_Organismo(this.idOrganismo).OrderBy(x => x.Nombre);
				return Json(contactos_organismo.ToDataSourceResult(request));
			}
			catch (Exception ex)
			{
				this.log.PublishException(new SecureportException(ex));
				throw;
			}
		}
		
		#endregion
		
		#region Action CRUD
		[HttpPost]
		public ActionResult CambiarProvincia(int? id)
		{
			try
			{

				this.idProvincia = id;

				this.ComboCiudades(this.idProvincia,null);

				this.comboComunidades(id);

				return this.PartialView("~/Views/Organismo/ComboCiudad.cshtml");

			}
			catch (Exception ex)
			{
				this.log.PublishException(new SecureportException(ex));

				return this.Json(new { result = false, Message = ex.Message });
			}

		}
		[HttpPost]
		public ActionResult CambiarComunidad(int? id)
		{
			try
			{

				this.idProvincia = id;

				this.comboComunidades(id);
				
				ViewBag.disabled = false;

				IEnumerable<Comunidades_Autonomas> result = _serviciosOrganismos.ListComunidades(this.idProvincia).ToList();

				foreach (Comunidades_Autonomas Comunidades in result)
				{
					ViewBag.Comunidad = Comunidades.Nombre;
				}

				return this.PartialView("~/Views/Organismo/ComboComunidad.cshtml");

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
		public ActionResult create()
		{
			try
			{
				this.ComboProvincias(null);

				this.ComboTipo(null);

				this.ComboCiudades(this.idProvincia,null);

				this.comboComunidades(null);

				this.ViewBag.Observaciones = string.Empty;

				this.ViewBag.Direccion = string.Empty;
				
				ViewBag.disabled = false;

				return this.PartialView("~/Views/Organismo/Create.cshtml");

			}
			catch (Exception ex)
			{
				this.log.PublishException(new SecureportException(ex));

				return this.Json(new { result = false, Message = ex.Message });
			}

		}
		/// <summary>
		/// Retorna una lista de todos los organismos en el grid.
		/// </summary>
		public ActionResult ListadoOrganismo()
		{
			try
			{
				
				return this.PartialView("ListadoOrganismos", this.UsuarioFrontalSession);
			}
			catch (Exception ex)
			{
				this.log.PublishException(new SecureportException(ex));
				throw;
			}
		}
		/// <summary>
		/// Guardar datos de Organismos.
		/// </summary>
		[HttpPost]
		public ActionResult GuardarOrganismo(OrganismoJson organismoJson)
		{
			try
			{
			   
				Organismos organismos = this.repoOrganismo.Single(x => x.Id == this.idOrganismo);
				
				organismos.Nombre = organismoJson.Nombre;

				organismos.Tipo = organismoJson.Tipo;

				organismos.ID_Ciudad = organismoJson.ID_Ciudad;

				organismos.ID_Provincia = organismoJson.ID_Provincia;

				organismos.Direccion = organismoJson.Direccion;

				organismos.Cod_Postal = organismoJson.Cod_Postal;

				organismos.es_activo = (organismoJson.activo == "1");

				organismos.Observaciones = CambiarTexto(organismoJson.Observaciones);

				this.repoOrganismo.Update(organismos);

				this.trazas(44, (int)this.UsuarioFrontalSession.Usuario.id, "Se ha modificado un registro en SPMAE_Organismos con ID" + "(" + organismos.Id.ToString() + ")");

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
		/// Alta-Editar Amenazas.
		/// </summary>
		[HttpPost]
		public ActionResult AltaEditarOrganismo(OrganismoJson organismoJson)
		{
			try
			{

				IEnumerable<Organismos> result = null;

				switch (organismoJson.action)
				{

					case "Alta":
						result = _serviciosOrganismos.ListOrganismo(organismoJson.Nombre);
						bool resultado = false;
						foreach (Organismos organismo in result)
						{
							if (organismo.es_activo)
								resultado = true;
						}
						if (!resultado)
						{
							if (result.Count() == 0)
								AltaOrganismo(organismoJson);
							else
								return this.Json(new { result = 2, Message = "Error al dar de alta el organismo, ya existe otro organismo activo " + organismoJson.Nombre + "¿Desea continuar? " });
						}
						else
						{
							this.trazas(41, (int)this.UsuarioFrontalSession.Usuario.id, "Error al dar de alta el organismo, ya existe otro organismo activo " + organismoJson.Nombre);
							return this.Json(new { result = 1, Message = "Error al dar de alta el organismo, ya existe otro organismo activo " + organismoJson.Nombre });
						}
						break;

					default:
						this.idOrganismo = organismoJson.Id;
						bool CambioEstado = _serviciosOrganismos.ListOrganismo(organismoJson.Nombre, organismoJson.Id, (organismoJson.activo == "1"));
						if (CambioEstado)
						{
							GuardarOrganismo(organismoJson);
						}
						else
						{
							result = _serviciosOrganismos.ListOrganismo(organismoJson.Nombre, organismoJson.Id);
							if (result.Count() == 0)
							{
								GuardarOrganismo(organismoJson);
							}
							else
							{
								IEnumerable<Organismos> organismo = result.Where(x => x.Id != organismoJson.Id);
								if (organismo == null)
								{
									GuardarOrganismo(organismoJson);
								}
								else
								{
									Organismos Actorganismo = organismo.FirstOrDefault();
									if (Actorganismo.Id == organismoJson.Id)
									{
										return this.Json(new { result = 3, Message =  "Ya existe un Organismo " + organismoJson.Nombre + " desactivo ¿Desea continuar? " });
									}
									else
									{
										if (!Actorganismo.es_activo)
											return this.Json(new { result = 3, Message = "Ya existe un Organismo " + organismoJson.Nombre + " desactivo ¿Desea continuar? " });
										else
										{
											this.trazas(44, (int)this.UsuarioFrontalSession.Usuario.id, "Error al modificar el organismo, ya existe otro organismo activo " + organismoJson.Nombre);
											return this.Json(new { result = 1, Message = "Error al modificar el organismo, ya existe otro organismo activo  " + organismoJson.Nombre });
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
		
		/// <summary>
		/// Alta de Organismos.
		/// </summary>
		[HttpPost]
		public ActionResult AltaOrganismo(OrganismoJson organismoJson)
		{
			try
			{
				Organismos organismos = new Organismos();

				organismos.Nombre = organismoJson.Nombre;

				organismos.Tipo = organismoJson.Tipo;

				organismos.ID_Ciudad = organismoJson.ID_Ciudad;

				organismos.ID_Provincia = organismoJson.ID_Provincia;

				organismos.Direccion = organismoJson.Direccion;

				organismos.Cod_Postal = organismoJson.Cod_Postal;

				organismos.Observaciones = CambiarTexto(organismoJson.Observaciones);

				organismos.ID_Usu_Alta = this.UsuarioFrontalSession.Usuario.id;

				organismos.es_activo = true;

				organismos.fech_activo = DateTime.Now.Date;

				organismos.fech_alta = DateTime.Now.Date;

				this.repoOrganismo.Create(organismos);

				this.idOrganismo = this.db.Organismos.OrderByDescending(x => x.Id).FirstOrDefault().Id;

				this.trazas(41, (int)this.UsuarioFrontalSession.Usuario.id, "Se ha dado de alta un registro en SPMAE_Organismos con ID" + "(" + this.idOrganismo.ToString() + ")");
				
				List<DependenciasUsuario> ListDependenciasDepenUsuarios = this._serviciosUsuarios.GetDependenciaUsuarios(this.UsuarioFrontalSession.Usuario.id, this.UsuarioFrontalSession.Usuario.categoria);

				this.UsuarioFrontalSession.ListDependenciasDepenUsuarios = ListDependenciasDepenUsuarios;
				
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
		/// Editar datos de Organismos.
		/// </summary>
		[HttpPost]
		public ActionResult EditarOrganismo(int? id)
		{
			try
			{
				this.ComboTipo(null);

				this.idOrganismo = (id == null ? this.idOrganismo : (int)id);

				Organismos organismos = this.repoOrganismo.Single(x => x.Id == this.idOrganismo);

				this.ComboProvincias(null);

				this.ComboCiudades(organismos.ID_Provincia, null);

				ViewBag.disabled = true;

				ViewBag.Observaciones = organismos.Observaciones;

				IEnumerable<Comunidades_Autonomas> result = _serviciosOrganismos.ListComunidades(organismos.ID_Provincia).ToList();

				foreach (Comunidades_Autonomas Comunidades in result)
				{
					ViewBag.Comunidad = Comunidades.Nombre;
				}
				
				return this.PartialView("~/Views/Organismo/PlantillaEdit.cshtml", organismos);
				
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
		///Borrar datos de Organismos.
		/// </summary>
		[HttpPost]
		public ActionResult BorrarOrganismo(int? id)
		{
			try
			{
				this.idOrganismo = (id == null ? this.idOrganismo : (int)id);
				
				this.repoContactos_Organismo.GetAll().Where(x => x.Id_Organismo == this.idOrganismo).ToList().ForEach(
				p=>
				{
					  Contactos_Organismo contactos = this.repoContactos_Organismo.Single(x => x.Id_Organismo == p.Id_Organismo);

					  this.repoContactos_Organismo.Delete(contactos);

					  this.trazas(47, (int)this.UsuarioFrontalSession.Usuario.id, "Se ha eliminado un registro en SPMAE_Contactos_Organismo " + p.Nombre + "(" + contactos.Id.ToString() + ")");
				  
				});
				
				this.repoOrganismo.Delete(this.repoOrganismo.Single(x => x.Id == this.idOrganismo));

				this.trazas(43, (int)this.UsuarioFrontalSession.Usuario.id, "Se ha eliminado un registro en SPMAE_Organismos con ID" + "(" + this.idOrganismo.ToString() + ")");

				return this.Json(new { result = true, Message ="Se ha eliminado correctamente." });
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

				return this.Json(new { result = false, Message = MessageDelete(ex) });
			}

		}
		
		[HttpPost]
		public ActionResult DatosOrganismo()
		{
			return this.PartialView("~/Views/Organismo/_PartialCreateContactos.cshtml");
		}
		
		/// <summary>
		/// Visualizar datos de Organismos.
		/// </summary>
		[HttpPost]
		public ActionResult VisualizarOrganismo(int? id)
		{
			try
			{
				this.idOrganismo = (id == null ? this.idOrganismo : (int)id);

				Organismos organismos = this.repoOrganismo.Single(x => x.Id == this.idOrganismo);
				
				this.ViewBag.Observaciones = organismos.Observaciones;

				this.ComboTipo(organismos.Tipo);

				this.ComboProvincias(organismos.ID_Provincia);

				this.ComboCiudades(null, organismos.ID_Ciudad);
				
				ViewBag.disabled = false;
				
				this.ViewBag.ALTA_CONT_OGP = true;

				this.ViewBag.MODIF_CONT_OGP = true;
				
				this.ViewBag.BORRA_CONT_OGP = true;

				this.ViewBag.CONSULTA_ORGANISMO = true;
				
				this.UsuarioFrontalSession.ListGruposPerfilesPermisos.ToList().ForEach(
					t =>
					{
						if (t.NombrePermiso == Organismo.ALTA_CONT_OGP.ToDescription())
						{
							this.ViewBag.ALTA_CONT_OGP = false;
						}
						if (t.NombrePermiso == Organismo.MODIF_CONT_OGP.ToDescription())
						{
							this.ViewBag.MODIF_CONT_OGP = false;
						}
						if (t.NombrePermiso == Organismo.BORRA_CONT_OGP.ToDescription())
						{
							this.ViewBag.BORRA_CONT_OGP = false;
						}
						if (t.NombrePermiso == Organismo.CONSULTA_ORGANISMO.ToDescription())
						{
							this.ViewBag.CONSULTA_ORGANISMO = false;
						}
					});
				
				IEnumerable<Comunidades_Autonomas> result = _serviciosOrganismos.ListComunidades(organismos.ID_Provincia).ToList();

				foreach (Comunidades_Autonomas Comunidades in result)
				{
					ViewBag.Comunidad = Comunidades.Nombre;
				}

				return this.PartialView("~/Views/Organismo/PlantillaEdit.cshtml", organismos); 
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

		#region Contactos
		/// <summary>
		/// Alta de Contacto Organismo.
		/// </summary>
		[HttpPost]
		public ActionResult GuardarContactoOrganismo(ContactoJson ContactoJson)
		{
			try
			{

			   var organismo = this.repoContactos_Organismo.GetAll().Where(x => x.Id_Organismo == this.idOrganismo);
			   organismo.ToList().ForEach(
			   p =>
			   {
				   p.Es_Responsable = false;
				   this.repoContactos_Organismo.Update(p);
			   });

				Contactos_Organismo contactosorganismo = this.repoContactos_Organismo.Single(x => x.Id == ContactoJson.Id);

				ContactoJson.Observaciones = this.CambiarTexto(ContactoJson.Observaciones);

				contactosorganismo.Nombre = ContactoJson.Nombre;

				contactosorganismo.Telefono = ContactoJson.Telefono;

				contactosorganismo.Fax = ContactoJson.Fax;

				contactosorganismo.Cargo = ContactoJson.Cargo;

				contactosorganismo.Email = ContactoJson.Email;

				contactosorganismo.Es_Responsable = ContactoJson.Es_responsable=="Si"? true:false;

				contactosorganismo.Observaciones = CambiarTexto(ContactoJson.Observaciones);

				contactosorganismo.es_activo = ContactoJson.es_activo;

				this.repoContactos_Organismo.Update(contactosorganismo);

				this.trazas(48, (int)this.UsuarioFrontalSession.Usuario.id, "Se ha modificado un registro en SPMAE_Contactos_Organismo " + contactosorganismo.Nombre + "(" + contactosorganismo.Id.ToString() + ")");

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
		/// Alta de Contacto Organismo.
		/// </summary>
		[HttpPost]
		public ActionResult AltaContactoOrganismo(ContactoJson ContactoJson)
		{
			try
			{

				Contactos_Organismo contactosorganismo = new Contactos_Organismo();
				
				contactosorganismo.Nombre = ContactoJson.Nombre;

				contactosorganismo.Telefono = ContactoJson.Telefono;

				contactosorganismo.Fax = ContactoJson.Fax;

				contactosorganismo.Cargo = ContactoJson.Cargo;

				contactosorganismo.Email = ContactoJson.Email;

				contactosorganismo.Es_Responsable = ContactoJson.Es_responsable == "Si" ? true : false;

				contactosorganismo.Observaciones = CambiarTexto(ContactoJson.Observaciones);
				
				contactosorganismo.ID_Usu_Alta = this.UsuarioFrontalSession.Usuario.id;

				contactosorganismo.es_activo = true;

				contactosorganismo.fech_activo = DateTime.Now;

				contactosorganismo.fech_alta = DateTime.Now;

				contactosorganismo.ID_Usu_Alta = this.UsuarioFrontalSession.Usuario.id;

				contactosorganismo.Id_Organismo = Convert.ToInt32(this.idOrganismo);

				this.repoContactos_Organismo.Create(contactosorganismo);

				this.trazas(45, (int)this.UsuarioFrontalSession.Usuario.id, "Se ha dado de alta un registro en SPMAE_Contactos_Organismo " + contactosorganismo.Nombre);

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
		[HttpPost]
		public ActionResult CreateContactos()
		{
			try
			{
				return this.PartialView("~/Views/Organismo/_PartialCreateContactos.cshtml");
			}
			catch (Exception ex)
			{
				this.log.PublishException(new SecureportException(ex));

				return this.Json(new { result = false, Message = ex.Message });
			}
		}
		[HttpPost]
		public ActionResult Edit(int id, string Accion)
		{
			try
			{
				if (Accion == General.EditGeneral.ToDescription())
				{
					ViewBag.Operadordisabled = true;
					ViewBag.Combo = "display:block";
					ViewBag.Texto = "display:none";
					ViewBag.Mensaje = "Modificar Contacto";
				}
				else
				{
					ViewBag.Operadordisabled = false;
					ViewBag.Combo = "display:none";
					ViewBag.Texto = "display:block";
					ViewBag.Mensaje = "Visualizar Contacto";

				}
				ViewBag.display = "display:block";

				Contactos_Organismo contactos_organismo = this.repoContactos_Organismo.Single(x => x.Id == id);
				ViewBag.Observaciones                   = contactos_organismo.Observaciones;
				return this.PartialView("~/Views/Organismo/_PartialEditContactos.cshtml", contactos_organismo); 
			}
			catch (Exception ex)
			{
				this.log.PublishException(new SecureportException(ex));

				return this.Json(new { result = false, Message = ex.Message });
			}
		}
		/// <summary>
		///Borrar datos de Contacto Organismo.
		/// </summary>
		[HttpPost]
		public ActionResult BorrarContactoOrganismo(int? id)
		{
			try
			{
				Contactos_Organismo contactos_organismos = this.repoContactos_Organismo.Single(x => x.Id == id);

				this.repoContactos_Organismo.Delete(contactos_organismos);

				this.trazas(47, (int)this.UsuarioFrontalSession.Usuario.id, "Se ha eliminado un registro en SPMAE_Contactos_Organismo" + contactos_organismos.Nombre + "(" + contactos_organismos.Id.ToString()+")");

				return this.Json(new { result = true});
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

		#endregion

		#endregion
	}
}