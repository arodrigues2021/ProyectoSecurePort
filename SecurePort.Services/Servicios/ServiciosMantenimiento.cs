namespace SecurePort.Services.Interfaces
{
	#region using
	using SecurePort.Entities;
	using SecurePort.Entities.Enums;
	using SecurePort.Entities.Models;
	using System;
	using System.Collections.Generic;
	using System.Linq;

	#endregion

	public class ServiciosMantenimiento : IServiciosMantenimiento
	{
	   
		#region Atributos

		private readonly ILogger log;

		protected readonly SecurePortContext db = new SecurePortContext();

		#endregion

		#region Métodos públicos

		public ServiciosMantenimiento(ILogger log)
		{
			this.log = log;
		}
		
	   #endregion

		public IEnumerable<MantenimientoViewModel> GetAllMantenimiento()
		{
			try
			{

					IEnumerable<MantenimientoViewModel> result = (from a1 in this.db.Mantenimientos.ToList()
																 join b2 in this.db.Puertos.ToList() on a1.id_Puerto equals b2.Id into PuertoTemp
																  from pu in PuertoTemp.DefaultIfEmpty()
																 join f5 in this.db.MOV_Instalaciones.ToList() on a1.id_IIPP equals f5.Id into InstaTemp
																  from Insta in InstaTemp.DefaultIfEmpty()
																 select new MantenimientoViewModel
																 {
																	 Id             = a1.Id,
																	 Id_Puerto      = a1.id_Puerto,
																	 Puerto         = (pu == null) ? string.Empty : pu.Nombre,
																	 Id_IIPP        = a1.id_IIPP,
																	 IIPP           = (Insta == null) ? string.Empty : Insta.Nombre,
																	 Descripcion    = a1.Descripcion,
																	 Fecha          = a1.fecha,
																	 Equipo         = a1.Equipo,
																	 Realizador     = a1.Realizador,
																	 Validador      = a1.Validador,
																	 Fecha_Revision = a1.fecha_Revision,
																	 Observaciones  = a1.Observaciones
																 }).OrderBy(x => x.Descripcion);
				return result;

			}
			catch (Exception ex)
			{
				this.log.PublishException(ex);
				throw;
			}
		}

		public IEnumerable<MantenimientoViewModel> GetAllMantenimiento(List<DependenciasUsuario> dependencias,int? categoria)
		{
			try
			{

				IEnumerable<MantenimientoViewModel> resultado = new List<MantenimientoViewModel>();


				if (categoria == (int)EnumCategoria.Puerto)
				{
				
					var dependenciasPuerto = dependencias.Where(x => x.categoria == (int)EnumCategoria.Puerto).ToList();

					if (dependenciasPuerto.Any())
					{

						resultado = (from a1 in this.db.Mantenimientos.ToList()
									 join b2 in this.db.Puertos.ToList() on a1.id_Puerto equals b2.Id into PuertoTemp
									 from pu in PuertoTemp.DefaultIfEmpty()
									 join f5 in this.db.MOV_Instalaciones.ToList() on a1.id_IIPP equals f5.Id into InstaTemp
									 from Insta in InstaTemp.DefaultIfEmpty()                                
									 join t5 in dependenciasPuerto on pu.Id equals t5.id_Dependencia
													  select new MantenimientoViewModel
													  {
															  Id             = a1.Id,
															  Id_Puerto      = a1.id_Puerto,
															  Puerto         = (pu == null) ? string.Empty : pu.Nombre,  
															  Id_IIPP        = a1.id_IIPP,
															  IIPP           = (Insta == null) ? string.Empty : Insta.Nombre,
															  Descripcion    = a1.Descripcion,
															  Fecha          = a1.fecha,
															  Equipo         = a1.Equipo,
															  Realizador     = a1.Realizador,
															  Validador      = a1.Validador,
															  Fecha_Revision = a1.fecha_Revision,
															  Observaciones  = a1.Observaciones
													  }).OrderBy(x => x.Descripcion);
					}

				}
				else if (categoria == (int)EnumCategoria.Instalacion)
				{

					var dependenciasInstala = dependencias.Where(x => x.categoria == (int)EnumCategoria.Instalacion).ToList();

					if (dependenciasInstala.Any())
					{

						resultado = (from a1 in this.db.Mantenimientos.ToList()
									 join b2 in this.db.Puertos.ToList() on a1.id_Puerto equals b2.Id into PuertoTemp
									 from pu in PuertoTemp.DefaultIfEmpty()
									 join f5 in this.db.MOV_Instalaciones.ToList() on a1.id_IIPP equals f5.Id into InstaTemp
									 from Insta in InstaTemp.DefaultIfEmpty()
									 join t5 in dependenciasInstala on a1.id_IIPP equals t5.id_Dependencia
									 select new MantenimientoViewModel
									 {
										 Id = a1.Id,
										 Id_Puerto = a1.id_Puerto,
										 Puerto = (pu == null) ? string.Empty : pu.Nombre,
										 Id_IIPP = a1.id_IIPP,
										 IIPP = (Insta == null) ? string.Empty : Insta.Nombre,
										 Descripcion = a1.Descripcion,
										 Fecha = a1.fecha,
										 Equipo = a1.Equipo,
										 Realizador = a1.Realizador,
										 Validador = a1.Validador,
										 Fecha_Revision = a1.fecha_Revision,
										 Observaciones = a1.Observaciones
									 }).OrderBy(x => x.Descripcion);
					}

				}
				else if (categoria == (int)EnumCategoria.OrganismoGestionPortuaria)
				{

					var dependenciasOrganismo = dependencias.Where(x => x.categoria == (int)EnumCategoria.OrganismoGestionPortuaria).ToList();

					if (dependenciasOrganismo.Any())
					{

						resultado = (from a1 in this.db.Mantenimientos.ToList()
									 join b2 in this.db.Puertos.ToList() on a1.id_Puerto equals b2.Id into PuertoTemp
									 from pu in PuertoTemp.DefaultIfEmpty()
									 join f5 in this.db.MOV_Instalaciones.ToList() on a1.id_IIPP equals f5.Id into InstaTemp
									 from Insta in InstaTemp.DefaultIfEmpty()
									 join t5 in dependenciasOrganismo on pu.id_Organismo equals t5.id_Dependencia
									 select new MantenimientoViewModel
									 {
										 Id = a1.Id,
										 Id_Puerto = a1.id_Puerto,
										 Puerto = (pu == null) ? string.Empty : pu.Nombre,
										 Id_IIPP = a1.id_IIPP,
										 IIPP = (Insta == null) ? string.Empty : Insta.Nombre,
										 Descripcion = a1.Descripcion,
										 Fecha = a1.fecha,
										 Equipo = a1.Equipo,
										 Realizador = a1.Realizador,
										 Validador = a1.Validador,
										 Fecha_Revision = a1.fecha_Revision,
										 Observaciones = a1.Observaciones
									 }).OrderBy(x => x.Descripcion);
					}

				}
					return resultado.GroupBy(c => c.Id).Select(grp => grp.First()).ToList();
				

			}
			catch (Exception ex)
			{
				this.log.PublishException(ex);
				throw;
			}
		}
	}
}
