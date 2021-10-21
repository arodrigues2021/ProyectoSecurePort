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
	using SecurePort.Entities.Exceptions;
	using SecurePort.Entities.Models;
	using SecurePort.Entities.Models.Json;
	using SecurePort.Services.Interfaces;
	using SecurePort.Services.Repository;
	#endregion
	public class ComunidadesController : BaseController
	{
		public ComunidadesController(IServiciosPaisesProvincias serviciosPaisesProvincias,
									  Comunidades_AutonomasRepository repoComunidades_Autonomas, 
									  TrazasRepository repoTrazas, ILogger log)
									  :base(serviciosPaisesProvincias,
											repoComunidades_Autonomas, 
											repoTrazas, 
											log)
								{

								}

		#region action
		/// <summary>
		/// Retorna una lista de todos las comunidades autonomas en el grid.
		/// </summary>
		public ActionResult ListadoComunidades()
		{
			try
			{
				return this.PartialView("ListadoComunidades", this.UsuarioFrontalSession);
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
		public ActionResult DatosComunidad()
		{
			this.ComboPaises();
			return this.PartialView("~/Views/Comunidades/Create.cshtml");
		}
		/// <summary>
		/// Visualizar los datos de la comunidad autónoma
		/// </summary>
		[HttpPost]
		public ActionResult VisualizarComunidad(int? id)
		{
			try
			{
				Comunidades_Autonomas comunidades_autonomas = this.repoComunidades_Autonomas.Single(x => x.Id == id);
				this.ComboPaises();
				ViewBag.disabled = true;
				return this.PartialView("~/Views/Comunidades/_PartialComunidad.cshtml", comunidades_autonomas);
			}
			catch (Exception ex)
			{
				this.log.PublishException(new SecureportException(ex));

				return this.Json(new { result = false, Message = ex.Message });
			}
		}
		/// <summary>
		/// Alta de Comunidad.
		/// </summary>
		[HttpPost]
		public ActionResult AltaComunidad(ComunidadJson comunidadJson)
		{
			try
			{
				IEnumerable<Comunidades_Autonomas> result = null;

				result = _serviciosPaisesProvincias.ListComunidades(comunidadJson.Nombre, comunidadJson.Id);
				 bool resultado = false;
				 foreach (Comunidades_Autonomas Comunidad in result)
				{
					if (Comunidad.es_activo == true)
						resultado = true;
				}
				if (!resultado)
				{
					  if(result.Count() ==0)
						  ComunidadAlta(comunidadJson);
					   else
						   return this.Json(new { result = 2, Message = "Ya existe una Comunidad Autónoma  " + comunidadJson.Nombre + " desactiva ¿Desea continuar? " });                            
				 } else {
					  this.trazas(196, (int)this.UsuarioFrontalSession.Usuario.id, "El nombre para la comunidad autónoma " + comunidadJson.Nombre + " ya esta dado de alta");
					  return this.Json(new { result = 1, Message = "Error al dar de alta la comunidad autónoma, ya existe otra comunidad autónoma activa " + comunidadJson.Nombre });
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
		/// Editar Pais.
		/// </summary>
		[HttpPost]
		public ActionResult EditarComunidad(ComunidadJson comunidadJson)
		{
			try
			{
				IEnumerable<Comunidades_Autonomas> result = null;

				result = _serviciosPaisesProvincias.ListComunidades(comunidadJson.Nombre, comunidadJson.Id);

				if (result.Count() == 0)
				{
					ComunidadEditar(comunidadJson);
				}
				else
				{
					IEnumerable<Comunidades_Autonomas> ComunAu = result.Where(x => x.Id != comunidadJson.Id);
					if (ComunAu == null)
					{
						ComunidadEditar(comunidadJson);
					}
					else
					{
						Comunidades_Autonomas ActComunidad = ComunAu.FirstOrDefault();
						if (ActComunidad.Id == comunidadJson.Id)
						{
							return this.Json(new { result = 3, Message = "El nombre de la comunidad autónoma ya existe " + comunidadJson.Nombre + " desactivo ¿Desea continuar? " });
						}
						else
						{
							if (!ActComunidad.es_activo)
								return this.Json(new { result = 3, Message = "El nombre de la comunidad autónoma ya existe " + comunidadJson.Nombre + " desactivo ¿Desea continuar? " });
							else
							{
								this.trazas(197, (int)this.UsuarioFrontalSession.Usuario.id, "Error al modificar el nombre de la comunidad autónima, ya existe otra comunidad autónima activo " + comunidadJson.Nombre + "(" + comunidadJson.Id + ")");
								return this.Json(new { result = 1, Message = "Error al modificar la comunidad autónima, ya existe otra comunidad autónima activa " + comunidadJson.Nombre });
							}
						}
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
		/// Borrar fisicamente el registro.
		/// </summary>
		[HttpPost]
		public ActionResult Eliminarcomunidad(int Id)
		{
			try
			{
				this.repoComunidades_Autonomas.Delete(this.repoComunidades_Autonomas.Single(x => x.Id == Id));

				this.trazas(198, (int)this.UsuarioFrontalSession.Usuario.id, "Se ha realizado la modificación de un registro en SPMAE_Comunidades_Autonomas con ID" + Id.ToString());

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
		public ActionResult ComunidadAlta(ComunidadJson comunidadJson)
		{
			try
			{
				Comunidades_Autonomas comunidades = new Comunidades_Autonomas();

				comunidades.ID_Usu_Alta = this.UsuarioFrontalSession.Usuario.id;

				comunidades.es_activo = true;

				comunidades.fech_activo = DateTime.Now;

				comunidades.fech_alta = DateTime.Now;

				comunidades.Nombre = comunidadJson.Nombre;

				comunidades.ID_Pais = comunidadJson.Pais;

				this.repoComunidades_Autonomas.Create(comunidades);

				var Id = this.db.Comunidades_Autonomas.OrderByDescending(u => u.Id).FirstOrDefault().Id;

				this.trazas(196, (int)this.UsuarioFrontalSession.Usuario.id, "Se ha dado de alta un registro en SPMAE_Comunidades_Autonomas con ID" + "(" + Id.ToString() + ")");

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
		public ActionResult ComunidadEditar(ComunidadJson comunidadJson)
		{
			try
			{

				Comunidades_Autonomas comunidades_autonomas = this.repoComunidades_Autonomas.Single(x => x.Id == comunidadJson.Id);

				comunidades_autonomas.Nombre = comunidadJson.Nombre;

				comunidades_autonomas.ID_Pais = comunidadJson.Pais;

				comunidades_autonomas.es_activo = (comunidadJson.activo == "1" ? true : false);

				this.repoComunidades_Autonomas.Update(comunidades_autonomas);

				this.trazas(197, comunidades_autonomas.Id, "Se ha realizado la modificación de un registro en SPMAE_Comunidades_Autonomas con ID" + "(" + comunidadJson.Id.ToString() + ")");

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

		#endregion

		#region Buscador
		/// <summary>
		/// Permite buscar por las diferentes colummnas del grid (List.cshtml)
		/// </summary>
		public ActionResult BuscadorComunidad([DataSourceRequest]DataSourceRequest request)
		{
			try
			{

				IEnumerable<ListadoComunidadesAutonomas> result = (from t1 in this.repoComunidades_Autonomas.GetAll().ToList()
																	 join t2 in db.Paises on t1.ID_Pais equals t2.Id
																	   select 
																		  new ListadoComunidadesAutonomas
																		  {
																			Id         = t1.Id,
																			Nombre     = t1.Nombre,
																			NombrePais = t2.nombre,
																			activado   = t1.es_activo ? "Si" : "No",
																		  }).OrderBy(n => n.Nombre);
															  
				return this.Json(result.ToDataSourceResult(request));
			}
			catch (Exception ex)
			{
				this.log.PublishException(new SecureportException(ex));
				throw;
			}
		}
		#endregion
	}
}


