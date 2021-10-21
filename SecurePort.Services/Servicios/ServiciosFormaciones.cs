using System.Collections.Generic;
using System;
using System.Linq;
using SecurePort.Entities;
using SecurePort.Entities.Models;

namespace SecurePort.Services.Interfaces
{
	using SecurePort.Entities.Enums;

	public class ServiciosFormaciones : IServiciosFormaciones
	{
		#region Atributos

		private readonly ILogger log;

		protected readonly SecurePortContext db = new SecurePortContext();

		#endregion

		#region Métodos públicos

		public ServiciosFormaciones(ILogger log)
		{
			this.log = log;
		}

		public IEnumerable<FormacionesViewModel> GetAllFormaciones(List<DependenciasUsuario> dependencias, int? categoria)
		{
			try
			{
				IEnumerable<FormacionesViewModel> resultado = new List<FormacionesViewModel>();


				if (categoria == (int)EnumCategoria.Puerto)
				{

					var dependenciasPuertos = dependencias.Where(x => x.categoria == (int)EnumCategoria.Puerto).ToList();


					resultado = (from t1 in this.db.Formaciones.ToList()
								 join t2 in this.db.Organismos.ToList() on t1.Id_Organismo equals t2.Id into OrganismoTemp
								 from or in OrganismoTemp.DefaultIfEmpty()
								 join t3 in this.db.Puertos.ToList() on t1.Id_Puerto equals t3.Id into PuertosTemp
								 from pt in PuertosTemp.DefaultIfEmpty()
								 join zz in dependenciasPuertos on pt.Id equals zz.id_Dependencia
								 join t4 in this.db.MOV_Instalaciones.ToList() on t1.Id_IIPP equals t4.Id into InstalacionTemp
								 from it in InstalacionTemp.DefaultIfEmpty()
								 orderby t1.Titulo
								 select new FormacionesViewModel
								 {
									 Id = t1.Id,
									 Organismo = (or == null) ? "" : or.Nombre,
									 Id_Organismo = t1.Id_Organismo,
									 Id_Puerto = t1.Id_Puerto,
									 Puerto = (pt == null) ? "" : pt.Nombre,
									 Id_IIPP = t1.Id_IIPP,
									 IIPP = (it == null) ? "" : it.Nombre,
									 Titulo = t1.Titulo,
									 Inicio = t1.Inicio,
									 Fin = t1.Fin,
									 Duracion = t1.Duracion,
									 Lugar = t1.Lugar,
									 Entidad = t1.Entidad,
									 Observaciones = t1.Observaciones
								 });

				}
				else if (categoria == (int)EnumCategoria.OrganismoGestionPortuaria)
				{

					var dependenciasOrganismos = dependencias.Where(x => x.categoria == (int)EnumCategoria.OrganismoGestionPortuaria).ToList();

					resultado = (from t1 in this.db.Formaciones.ToList()
								 join t2 in this.db.Organismos.ToList() on t1.Id_Organismo equals t2.Id into OrganismoTemp
								 from or in OrganismoTemp.DefaultIfEmpty()
								 join t3 in this.db.Puertos.ToList() on t1.Id_Puerto equals t3.Id into PuertosTemp
								 from pt in PuertosTemp.DefaultIfEmpty()
								 join zz in dependenciasOrganismos on or.Id equals zz.id_Dependencia
								 join t4 in this.db.MOV_Instalaciones.ToList() on t1.Id_IIPP equals t4.Id into InstalacionTemp
								 from it in InstalacionTemp.DefaultIfEmpty()
								 orderby t1.Titulo
								 select new FormacionesViewModel
								 {
									 Id = t1.Id,
									 Organismo = (or == null) ? "" : or.Nombre,
									 Id_Organismo = t1.Id_Organismo,
									 Id_Puerto = t1.Id_Puerto,
									 Puerto = (pt == null) ? "" : pt.Nombre,
									 Id_IIPP = t1.Id_IIPP,
									 IIPP = (it == null) ? "" : it.Nombre,
									 Titulo = t1.Titulo,
									 Inicio = t1.Inicio,
									 Fin = t1.Fin,
									 Duracion = t1.Duracion,
									 Lugar = t1.Lugar,
									 Entidad = t1.Entidad,
									 Observaciones = t1.Observaciones
								 });

				}
				else if (categoria == (int)EnumCategoria.Instalacion)
				{

					var dependenciasPuertos = dependencias.Where(x => x.categoria == (int)EnumCategoria.Puerto).ToList();

					var dependenciasInstala = dependencias.Where(x => x.categoria == (int)EnumCategoria.Instalacion).ToList();

					resultado = (from t1 in this.db.Formaciones.ToList()
								 join t2 in this.db.Organismos.ToList() on t1.Id_Organismo equals t2.Id into OrganismoTemp
								 from or in OrganismoTemp.DefaultIfEmpty()
								 join t3 in this.db.Puertos.ToList() on t1.Id_Puerto equals t3.Id into PuertosTemp
								 from pt in PuertosTemp.DefaultIfEmpty()
								 join zz in dependenciasPuertos on pt.Id equals zz.id_Dependencia
								 join t4 in this.db.MOV_Instalaciones.ToList() on t1.Id_IIPP equals t4.Id into InstalacionTemp
								 from it in InstalacionTemp.DefaultIfEmpty()
								 orderby t1.Titulo
								 select new FormacionesViewModel
								 {
									 Id = t1.Id,
									 Organismo = (or == null) ? "" : or.Nombre,
									 Id_Organismo = t1.Id_Organismo,
									 Id_Puerto = t1.Id_Puerto,
									 Puerto = (pt == null) ? "" : pt.Nombre,
									 Id_IIPP = t1.Id_IIPP,
									 IIPP = (it == null) ? "" : it.Nombre,
									 Titulo = t1.Titulo,
									 Inicio = t1.Inicio,
									 Fin = t1.Fin,
									 Duracion = t1.Duracion,
									 Lugar = t1.Lugar,
									 Entidad = t1.Entidad,
									 Observaciones = t1.Observaciones
								 });

					if (dependenciasInstala.Any())
					{
						 resultado = (from o in resultado join zz in dependenciasInstala on o.Id_IIPP equals zz.id_Dependencia 
									  select new FormacionesViewModel
										 {
											 Id = o.Id,
											 Organismo = o.Organismo,
											 Id_Organismo = o.Id_Organismo,
											 Id_Puerto = o.Id_Puerto,
											 Puerto = o.Puerto,
											 Id_IIPP = o.Id_IIPP,
											 IIPP = o.IIPP,
											 Titulo = o.Titulo,
											 Inicio = o.Inicio,
											 Fin = o.Fin,
											 Duracion = o.Duracion,
											 Lugar = o.Lugar,
											 Entidad = o.Entidad,
											 Observaciones = o.Observaciones
										 });    
					}
					
				}

				return resultado.ToList();
			}
			catch (Exception ex)
			{
				this.log.PublishException(ex);

				throw;
			}
		}

		public IEnumerable<FormacionesViewModel> GetAllFormaciones()
		{
			try
			{

				IEnumerable<FormacionesViewModel> result = (from t1 in this.db.Formaciones.ToList()
															join t2 in this.db.Organismos.ToList() on t1.Id_Organismo equals t2.Id into OrganismoTemp
															from or in OrganismoTemp.DefaultIfEmpty()
															join t3 in this.db.Puertos.ToList() on t1.Id_Puerto equals t3.Id into PuertosTemp
															from pt in PuertosTemp.DefaultIfEmpty()
															join t4 in this.db.MOV_Instalaciones.ToList() on t1.Id_IIPP equals t4.Id into InstalacionTemp
															from it in InstalacionTemp.DefaultIfEmpty()
															orderby t1.Titulo
															select new FormacionesViewModel
															{
																Id = t1.Id,
																Organismo = (or == null) ? "" : or.Nombre,
																Id_Organismo = t1.Id_Organismo,
																Id_Puerto = t1.Id_Puerto,
																Puerto = (pt == null) ? "" : pt.Nombre,
																Id_IIPP = t1.Id_IIPP,
																IIPP = (it == null) ? "" : it.Nombre,
																Titulo = t1.Titulo,
																Inicio = t1.Inicio,
																Fin = t1.Fin,
																Duracion = t1.Duracion,
																Lugar = t1.Lugar,
																Entidad = t1.Entidad,
																Observaciones = t1.Observaciones
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
	}
}
