using System.Collections.Generic;
using System;
using System.Linq;
using SecurePort.Entities;
using SecurePort.Entities.Models;


namespace SecurePort.Services.Interfaces
{
	using SecurePort.Entities.Enums;

	public class ServiciosDeclaraMaritima : IServiciosDeclaraMaritima 
	{
	   
		#region Atributos
			private readonly ILogger log;
			protected readonly SecurePortContext db = new SecurePortContext();
		#endregion

		#region Métodos públicos
		public ServiciosDeclaraMaritima(ILogger log)
		{
			this.log = log;
		}

		public IEnumerable<DeclaraMaritimaViewModel> GetAllDeclaraMaritima()
		{
			try
			{
			   
				IEnumerable<DeclaraMaritimaViewModel> result = (from t1 in this.db.DeclaraMaritima.ToList()
																join f5 in this.db.MOV_Instalaciones.ToList() on t1.Id_IIPP equals f5.Id into InstaTemp
																from Insta in InstaTemp.DefaultIfEmpty()
																join b2 in this.db.Puertos.ToList() on Insta.id_Puerto equals b2.Id into PuertoTemp
																from pu in PuertoTemp.DefaultIfEmpty()
																join a1 in this.db.Organismos.ToList().Where(x=> x.es_activo) on pu.id_Organismo equals a1.Id
																orderby t1.Id
																select
																	new DeclaraMaritimaViewModel
																	{
																		Id               = t1.Id,
																		Id_Organismo     = a1.Id,
																		NombreOrganismo  = a1.Nombre,
																		Id_Puerto        = pu.Id, 
																		NombrePuerto     = pu.Nombre,
																		Id_IIPP          = t1.Id_IIPP,
																		NombreIIPP       = Insta.Nombre,
																		IMO              = t1.IMO,
																		Buque            = t1.Buque,
																		Id_Motivo        = t1.Id_Motivo,
																		Fech_Expe        = t1.Fech_Expe,
																		Fech_Ini_Validez = t1.Fech_Ini_Validez,
																		Fech_Fin_Validez = t1.Fech_Fin_Validez,
																		Responsable      = t1.Responsable,
																		Nivel_Buq        = t1.Nivel_Buq,
																		Nivel_IIPP       = t1.Nivel_IIPP,
																		Medidas          = t1.Medidas,
																		Observaciones    = t1.Observaciones
																	});

				return result;
			}
			catch (Exception ex)
			{
				this.log.PublishException(ex);
				throw;
			}

		}

		#endregion

		public IEnumerable<DeclaraMaritimaViewModel> GetAllDeclaraMaritima(List<DependenciasUsuario> dependencias,int? categoria)
		{
			try
			{

				IEnumerable<DeclaraMaritimaViewModel> resultado = new List<DeclaraMaritimaViewModel>();

				if (categoria == (int)EnumCategoria.OrganismoGestionPortuaria)
				{
					var dependenciasOrganismo = dependencias.Where(x => x.categoria == (int)EnumCategoria.OrganismoGestionPortuaria).ToList();
			
					if (dependenciasOrganismo.Any())
					{

										 resultado = (from t1 in this.db.DeclaraMaritima.ToList()
																	join f5 in this.db.MOV_Instalaciones.ToList() on t1.Id_IIPP equals f5.Id into InstaTemp
																	from Insta in InstaTemp.DefaultIfEmpty()
																	join b2 in this.db.Puertos.ToList() on Insta.id_Puerto equals b2.Id into PuertoTemp
																	from pu in PuertoTemp.DefaultIfEmpty()
																	join a1 in this.db.Organismos.ToList().Where(x => x.es_activo) on pu.id_Organismo equals a1.Id
																	join t5 in dependenciasOrganismo on a1.Id equals t5.id_Dependencia
																	orderby t1.Id
																	select
																		new DeclaraMaritimaViewModel
																		{
																			Id               = t1.Id,
																			Id_Organismo     = a1.Id,
																			NombreOrganismo  = a1.Nombre,
																			Id_Puerto        = pu.Id,
																			NombrePuerto     = pu.Nombre,
																			Id_IIPP          = t1.Id_IIPP,
																			NombreIIPP       = Insta.Nombre,
																			IMO              = t1.IMO,
																			Buque            = t1.Buque,
																			Id_Motivo        = t1.Id_Motivo,
																			Fech_Expe        = t1.Fech_Expe,
																			Fech_Ini_Validez = t1.Fech_Ini_Validez,
																			Fech_Fin_Validez = t1.Fech_Fin_Validez,
																			Responsable      = t1.Responsable,
																			Nivel_Buq        = t1.Nivel_Buq,
																			Nivel_IIPP       = t1.Nivel_IIPP,
																			Medidas          = t1.Medidas,
																			Observaciones    = t1.Observaciones
																		});
					}
				}
				else if (categoria == (int)EnumCategoria.Puerto)
				{
					var dependenciasPuerto = dependencias.Where(x => x.categoria == (int)EnumCategoria.Puerto).ToList();

					if (dependenciasPuerto.Any())
					{

						resultado = (from t1 in this.db.DeclaraMaritima.ToList()
									     join f5 in this.db.MOV_Instalaciones.ToList() on t1.Id_IIPP equals f5.Id into InstaTemp
									     from Insta in InstaTemp.DefaultIfEmpty()
									     join b2 in this.db.Puertos.ToList() on Insta.id_Puerto equals b2.Id into PuertoTemp
									     from pu in PuertoTemp.DefaultIfEmpty()
									     join a1 in this.db.Organismos.ToList().Where(x => x.es_activo) on pu.id_Organismo equals a1.Id
									     join t5 in dependenciasPuerto on pu.Id equals t5.id_Dependencia
									     orderby t1.Id
									 select
										 new DeclaraMaritimaViewModel
										 {
											 Id = t1.Id,
											 Id_Organismo = a1.Id,
											 NombreOrganismo = a1.Nombre,
											 Id_Puerto = pu.Id,
											 NombrePuerto = pu.Nombre,
											 Id_IIPP = t1.Id_IIPP,
											 NombreIIPP = Insta.Nombre,
											 IMO = t1.IMO,
											 Buque = t1.Buque,
											 Id_Motivo = t1.Id_Motivo,
											 Fech_Expe = t1.Fech_Expe,
											 Fech_Ini_Validez = t1.Fech_Ini_Validez,
											 Fech_Fin_Validez = t1.Fech_Fin_Validez,
											 Responsable = t1.Responsable,
											 Nivel_Buq = t1.Nivel_Buq,
											 Nivel_IIPP = t1.Nivel_IIPP,
											 Medidas = t1.Medidas,
											 Observaciones = t1.Observaciones
										 });
					}
				}
				else if (categoria == (int)EnumCategoria.Instalacion)
				{

					var dependenciasInstalacion = dependencias.Where(x => x.categoria == (int)EnumCategoria.Instalacion).ToList();

					resultado = (from t1 in this.db.DeclaraMaritima.ToList()
								     join f5 in this.db.MOV_Instalaciones.ToList() on t1.Id_IIPP equals f5.Id into InstaTemp
								     from Insta in InstaTemp.DefaultIfEmpty()
								     join b2 in this.db.Puertos.ToList() on Insta.id_Puerto equals b2.Id into PuertoTemp
								     from pu in PuertoTemp.DefaultIfEmpty()
								     join a1 in this.db.Organismos.ToList().Where(x => x.es_activo) on pu.id_Organismo equals a1.Id
								     join t5 in dependenciasInstalacion on Insta.Id equals t5.id_Dependencia
								 orderby t1.Id
								 select
									 new DeclaraMaritimaViewModel
									 {
										 Id = t1.Id,
										 Id_Organismo = a1.Id,
										 NombreOrganismo = a1.Nombre,
										 Id_Puerto = pu.Id,
										 NombrePuerto = pu.Nombre,
										 Id_IIPP = t1.Id_IIPP,
										 NombreIIPP = Insta.Nombre,
										 IMO = t1.IMO,
										 Buque = t1.Buque,
										 Id_Motivo = t1.Id_Motivo,
										 Fech_Expe = t1.Fech_Expe,
										 Fech_Ini_Validez = t1.Fech_Ini_Validez,
										 Fech_Fin_Validez = t1.Fech_Fin_Validez,
										 Responsable = t1.Responsable,
										 Nivel_Buq = t1.Nivel_Buq,
										 Nivel_IIPP = t1.Nivel_IIPP,
										 Medidas = t1.Medidas,
										 Observaciones = t1.Observaciones
									 });

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



		