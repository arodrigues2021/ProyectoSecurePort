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

	public class ServiciosComunicaciones : IServiciosComunicaciones
	{
	   
		#region Atributos

		private readonly ILogger log;

		protected readonly SecurePortContext db = new SecurePortContext();

		#endregion

		#region Métodos públicos

		public ServiciosComunicaciones(ILogger log)
		{
			this.log = log;
		}
		
	   #endregion

		public IEnumerable<ComunicacionesViewModel> GetAllComunicaciones()
		{
			try
			{

				IEnumerable<ComunicacionesViewModel> result = (from a1 in this.db.Comunicaciones.ToList()
																 join b2 in this.db.Puertos.ToList() on a1.Id_Puerto equals b2.Id into PuertoTemp
																 from pu in PuertoTemp.DefaultIfEmpty()
																 join f5 in this.db.MOV_Instalaciones.ToList() on a1.Id_IIPP equals f5.Id into InstaTemp
																 from Insta in InstaTemp.DefaultIfEmpty()
																		  select new ComunicacionesViewModel
																		  {
																			  Id        = a1.Id,
																			  Id_Puerto = a1.Id_Puerto,
																			  Puerto    = (pu== null) ? string.Empty : pu.Nombre, 
																			  Id_IIPP   = a1.Id_IIPP,
																			  IIPP      = (Insta == null)? string.Empty : Insta.Nombre, 
																			  Asunto    = a1.Asunto,
																			  Fecha     = a1.Fecha,
																			  Emisor    = a1.Emisor,
																			  Receptor  = a1.Receptor,
																			  Mensaje   = a1.Mensaje,
																			  Recibido  = a1.Recibido
																		  }).OrderBy(x => x.Asunto);
				return result;

			}
			catch (Exception ex)
			{
				this.log.PublishException(ex);
				throw;
			}
		}

		public IEnumerable<ComunicacionesViewModel> GetAllComunicaciones(List<DependenciasUsuario> dependencias, int? categoria)
		{
			try
			{
				IEnumerable<ComunicacionesViewModel> resultado = new List<ComunicacionesViewModel>();

				if (categoria == (int)EnumCategoria.Puerto)
				{
					var dependenciasPuerto = dependencias.Where(x => x.categoria == (int)EnumCategoria.Puerto).ToList();

					if (dependenciasPuerto.Any())
					{

						resultado = (from a1 in this.db.Comunicaciones.ToList()
									 join b2 in this.db.Puertos.ToList() on a1.Id_Puerto equals b2.Id into PuertoTemp
									 from pu in PuertoTemp.DefaultIfEmpty()
									 join f5 in this.db.MOV_Instalaciones.ToList() on a1.Id_IIPP equals f5.Id into InstaTemp
									 from Insta in InstaTemp.DefaultIfEmpty()
									 join t5 in dependenciasPuerto on pu.Id equals t5.id_Dependencia
									 select new ComunicacionesViewModel
									 {
										 Id         = a1.Id,
										 Id_Puerto  = a1.Id_Puerto,
										 Puerto     = (pu == null) ? string.Empty : pu.Nombre,
										 Id_IIPP    = a1.Id_IIPP,
										 IIPP       = (Insta == null) ? string.Empty : Insta.Nombre,
										 Asunto     = a1.Asunto,
										 Fecha      = a1.Fecha,
										 Emisor     = a1.Emisor,
										 Receptor   = a1.Receptor,
										 Mensaje    = a1.Mensaje,
										 Recibido   = a1.Recibido
									 }).OrderBy(x => x.Asunto);
					}    
				}

				else if (categoria == (int)EnumCategoria.OrganismoGestionPortuaria)
				{
					var dependenciasOrganismo = dependencias.Where(x => x.categoria == (int)EnumCategoria.OrganismoGestionPortuaria).ToList();

					if (dependenciasOrganismo.Any())
					{
						resultado = (from a1 in this.db.Comunicaciones.ToList()
									 join b2 in this.db.Puertos.ToList() on a1.Id_Puerto equals b2.Id into PuertoTemp
									 from pu in PuertoTemp.DefaultIfEmpty()
									 join f5 in this.db.MOV_Instalaciones.ToList() on a1.Id_IIPP equals f5.Id into InstaTemp
									 from Insta in InstaTemp.DefaultIfEmpty()
									 join a2 in this.db.Organismos.ToList().Where(x => x.es_activo) on pu.id_Organismo equals a2.Id
									 join t5 in dependenciasOrganismo on a2.Id equals t5.id_Dependencia
									 select new ComunicacionesViewModel
									 {
										 Id         = a1.Id,
										 Id_Puerto  = a1.Id_Puerto,
										 Puerto     = (pu == null) ? string.Empty : pu.Nombre,
										 Id_IIPP    = a1.Id_IIPP,
										 IIPP       = (Insta == null) ? string.Empty : Insta.Nombre,
										 Asunto     = a1.Asunto,
										 Fecha      = a1.Fecha,
										 Emisor     = a1.Emisor,
										 Receptor   = a1.Receptor,
										 Mensaje    = a1.Mensaje,
										 Recibido   = a1.Recibido
									 }).OrderBy(x => x.Asunto);
					}


				}
				else if (categoria == (int)EnumCategoria.Instalacion)
				{
					var dependenciasInsta = dependencias.Where(x => x.categoria == (int)EnumCategoria.Instalacion).ToList();

					if (dependenciasInsta.Any())
					{
						resultado = (from a1 in this.db.Comunicaciones.ToList()
									 join b2 in this.db.Puertos.ToList() on a1.Id_Puerto equals b2.Id into PuertoTemp
									 from pu in PuertoTemp.DefaultIfEmpty()
									 join f5 in this.db.MOV_Instalaciones.ToList() on a1.Id_IIPP equals f5.Id into InstaTemp
									 from Insta in InstaTemp.DefaultIfEmpty()
									 join a2 in this.db.Organismos.ToList().Where(x => x.es_activo) on pu.id_Organismo equals a2.Id
									 select new ComunicacionesViewModel
									 {
										 Id         = a1.Id,
										 Id_Puerto  = a1.Id_Puerto,
										 Puerto     = (pu == null) ? string.Empty : pu.Nombre,
										 Id_IIPP    = a1.Id_IIPP,
										 IIPP       = (Insta == null) ? string.Empty : Insta.Nombre,
										 Asunto     = a1.Asunto,
										 Fecha      = a1.Fecha,
										 Emisor     = a1.Emisor,
										 Receptor   = a1.Receptor,
										 Mensaje    = a1.Mensaje,
										 Recibido   = a1.Recibido
									 }).OrderBy(x => x.Asunto);

                        if (dependenciasInsta.Any())
                        {
                            resultado = (from o in resultado
                                         join zz in dependenciasInsta on o.Id_IIPP equals zz.id_Dependencia
                                         select new ComunicacionesViewModel
                                         {
                                             Id         = o.Id,
                                             Id_Puerto  = o.Id_Puerto,
                                             Puerto     = o.Puerto,
                                             Id_IIPP    = o.Id_IIPP,
                                             IIPP       = o.IIPP,
                                             Asunto     = o.Asunto,
                                             Fecha      = o.Fecha,
                                             Emisor     = o.Emisor,
                                             Receptor   = o.Receptor,
                                             Mensaje    = o.Mensaje,
                                             Recibido   = o.Recibido
                                         }).OrderBy(x => x.Asunto);
                        }
					}

				}

				return resultado;

			}
			catch (Exception ex)
			{
				this.log.PublishException(ex);
				throw;
			}
		}
	}
}
