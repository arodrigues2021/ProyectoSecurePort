using System.Collections.Generic;
using System;
using System.Linq;
using SecurePort.Entities;
using SecurePort.Entities.Models;


namespace SecurePort.Services.Interfaces
{
	using System.Data.Entity.Core;

	using SecurePort.Entities.Enums;

	public class ServiciosAuditorias : IServiciosAuditorias
	{
	   
		#region Atributos
			private readonly ILogger log;
			protected readonly SecurePortContext db = new SecurePortContext();
		#endregion

		#region Métodos públicos

			public ServiciosAuditorias(ILogger log)
		{
			this.log = log;
		}
		
		#endregion

		public IEnumerable<AuditoriasViewModel> ListAuditorias(List<DependenciasUsuario> dependencias, int? categoria)
		{
			try
			{

				IEnumerable<AuditoriasViewModel> resultado = new List<AuditoriasViewModel>();

				
				if (categoria == (int)EnumCategoria.Instalacion)
				{
					   var dependenciasInstalacion = dependencias.Where(x => x.categoria == (int)EnumCategoria.Instalacion).ToList();

					   if (dependenciasInstalacion.Any())
						{
														 resultado = (from t1 in this.db.Auditorias.ToList()
																		  join t2 in this.db.Puertos.ToList() on t1.Id_Puerto equals t2.Id into TempPuertos
																	  from temp1 in TempPuertos.DefaultIfEmpty()
																		  join f5 in this.db.MOV_Instalaciones.ToList() on t1.Id_IIPP equals f5.Id into InstaTemp
                                                                      from Insta in InstaTemp.DefaultIfEmpty()
                                                                      orderby t1.Id
																  select new AuditoriasViewModel
																			   {
																				   Id = t1.Id,
																				   Id_Puerto = t1.Id_Puerto,
																				   Id_IIPP = t1.Id_IIPP,
																				   Puerto = (temp1 == null) ? "" : temp1.Nombre,
																				   IIPP = (Insta == null) ? "" : Insta.Nombre,
																				   Descripcion = t1.Descripcion,
																				   Tipo = t1.Tipo,
																				   NombreTipo =  t1.Tipo ==1? TipoAUD.INTER.ToDescription() : TipoAUD.EXT.ToDescription(), 
																				   Estado = t1.Estado,
																				   NombreEstado =t1.Estado ==1? EstadosPEA.PRO.ToDescription() : (t1.Estado == 2 ? EstadosPEA.PLA.ToDescription() : EstadosPEA.REL.ToDescription()),
																				   Ini_Programa = t1.Ini_Programa,
																				   Fin_Programa = t1.Fin_Programa,
																				   Ini_Planifica = t1.Ini_Planifica,
																				   Fin_Planifica = t1.Fin_Planifica,
																				   Ini_Real = t1.Ini_Real,
																				   Fin_Real = t1.Fin_Real,
																				   Responsable = t1.Responsable,
																				   Recomendaciones = t1.Recomendaciones,
																				   Programa = t1.Programa,
																				   Conclusiones = t1.Conclusiones,
																				   Seguimiento = t1.Seguimiento,
																				   Observaciones = t1.Observaciones
																			   });

                                                         if (dependenciasInstalacion.Any())
                                                         {
                                                             resultado = (from o in resultado
                                                                          join zz in dependenciasInstalacion on o.Id_IIPP equals zz.id_Dependencia
                                                                          select new AuditoriasViewModel
                                                                          {
                                                                              Id = o.Id,
                                                                              Id_Puerto = o.Id_Puerto,
                                                                              Id_IIPP = o.Id_IIPP,
                                                                              Puerto = o.Puerto,
                                                                              IIPP = o.IIPP,
                                                                              Descripcion = o.Descripcion,
                                                                              Tipo = o.Tipo,
                                                                              NombreTipo = o.NombreTipo,
                                                                              Estado = o.Estado,
                                                                              NombreEstado = o.NombreEstado,
                                                                              Ini_Programa = o.Ini_Programa,
                                                                              Fin_Programa = o.Fin_Programa,
                                                                              Ini_Planifica = o.Ini_Planifica,
                                                                              Fin_Planifica = o.Fin_Planifica,
                                                                              Ini_Real = o.Ini_Real,
                                                                              Fin_Real = o.Fin_Real,
                                                                              Responsable = o.Responsable,
                                                                              Recomendaciones = o.Recomendaciones,
                                                                              Programa = o.Programa,
                                                                              Conclusiones = o.Conclusiones,
                                                                              Seguimiento = o.Seguimiento,
                                                                              Observaciones = o.Observaciones
                                                                          });
                                                         }

						 }
				} 
				else if (categoria == (int)EnumCategoria.Puerto)
				{
					var dependenciasPuertos = dependencias.Where(x => x.categoria == (int)EnumCategoria.Puerto).ToList();

					if (dependenciasPuertos.Any())
					{

						resultado = (from t1 in this.db.Auditorias.ToList()
									 join t2 in this.db.Puertos.ToList() on t1.Id_Puerto equals t2.Id into TempPuertos
									 from temp1 in TempPuertos.DefaultIfEmpty()
									 join t3 in this.db.MOV_Instalaciones.ToList() on t1.Id_IIPP equals t3.Id into TempInstalaciones
									 from temp2 in TempInstalaciones.DefaultIfEmpty()
									 join c4 in dependenciasPuertos on temp1.Id equals c4.id_Dependencia
									 orderby t1.Id
									 select new AuditoriasViewModel
									 {
										 Id = t1.Id,
										 Id_Puerto = t1.Id_Puerto,
										 Id_IIPP = t1.Id_IIPP,
										 Puerto = (temp1 == null) ? "" : temp1.Nombre,
										 IIPP = (temp2 == null) ? "" : temp2.Nombre,
										 Descripcion = t1.Descripcion,
										 Tipo = t1.Tipo,
										 NombreTipo = t1.Tipo == 1 ? TipoAUD.INTER.ToDescription() : TipoAUD.EXT.ToDescription(),
										 Estado = t1.Estado,
										 NombreEstado = t1.Estado == 1 ? EstadosPEA.PRO.ToDescription() : (t1.Estado == 2 ? EstadosPEA.PLA.ToDescription() : EstadosPEA.REL.ToDescription()),
										 Ini_Programa = t1.Ini_Programa,
										 Fin_Programa = t1.Fin_Programa,
										 Ini_Planifica = t1.Ini_Planifica,
										 Fin_Planifica = t1.Fin_Planifica,
										 Ini_Real = t1.Ini_Real,
										 Fin_Real = t1.Fin_Real,
										 Responsable = t1.Responsable,
										 Recomendaciones = t1.Recomendaciones,
										 Programa = t1.Programa,
										 Conclusiones = t1.Conclusiones,
										 Seguimiento = t1.Seguimiento,
										 Observaciones = t1.Observaciones
									 });
					}
				}
				else if (categoria == (int)EnumCategoria.OrganismoGestionPortuaria)
				{
					var dependenciasOrganismos = dependencias.Where(x => x.categoria == (int)EnumCategoria.OrganismoGestionPortuaria).ToList();

					if (dependenciasOrganismos.Any())
					{

						resultado = (from t1 in this.db.Auditorias.ToList()
									 join t2 in this.db.Puertos.ToList() on t1.Id_Puerto equals t2.Id into TempPuertos
									 from temp1 in TempPuertos.DefaultIfEmpty()
									 join a1 in this.db.Organismos.ToList().Where(x => x.es_activo) on temp1.id_Organismo equals a1.Id
									 join t5 in dependenciasOrganismos on a1.Id equals t5.id_Dependencia
									 join t3 in this.db.MOV_Instalaciones.ToList() on t1.Id_IIPP equals t3.Id into TempInstalaciones
									 from temp2 in TempInstalaciones.DefaultIfEmpty()
									 orderby t1.Id
									 select new AuditoriasViewModel
									 {
										 Id = t1.Id,
										 Id_Puerto = t1.Id_Puerto,
										 Id_IIPP = t1.Id_IIPP,
										 Puerto = (temp1 == null) ? "" : temp1.Nombre,
										 IIPP = (temp2 == null) ? "" : temp2.Nombre,
										 Descripcion = t1.Descripcion,
										 Tipo = t1.Tipo,
										 NombreTipo = t1.Tipo == 1 ? TipoAUD.INTER.ToDescription() : TipoAUD.EXT.ToDescription(),
										 Estado = t1.Estado,
										 NombreEstado = t1.Estado == 1 ? EstadosPEA.PRO.ToDescription() : (t1.Estado == 2 ? EstadosPEA.PLA.ToDescription() : EstadosPEA.REL.ToDescription()),
										 Ini_Programa = t1.Ini_Programa,
										 Fin_Programa = t1.Fin_Programa,
										 Ini_Planifica = t1.Ini_Planifica,
										 Fin_Planifica = t1.Fin_Planifica,
										 Ini_Real = t1.Ini_Real,
										 Fin_Real = t1.Fin_Real,
										 Responsable = t1.Responsable,
										 Recomendaciones = t1.Recomendaciones,
										 Programa = t1.Programa,
										 Conclusiones = t1.Conclusiones,
										 Seguimiento = t1.Seguimiento,
										 Observaciones = t1.Observaciones
									 });
					}
				}
				
				//return resultado.GroupBy(c => c.Id).Select(grp => grp.First()).ToList();
				return resultado.ToList();
				
			}
			catch (Exception ex)
			{
				this.log.PublishException(ex);
				throw;
			}
		}

		public IEnumerable<AuditoriasViewModel> ListAuditorias()
		{
			try
			{


				IEnumerable<AuditoriasViewModel> result = (from t1 in this.db.Auditorias.ToList()
														   join t2 in this.db.Puertos.ToList() on t1.Id_Puerto equals t2.Id into TempPuertos
														   from temp1 in TempPuertos.DefaultIfEmpty()
														   join t3 in this.db.MOV_Instalaciones.ToList() on t1.Id_IIPP equals t3.Id into TempInstalaciones
														   from temp2 in TempInstalaciones.DefaultIfEmpty()
														   orderby t1.Id
														   select new AuditoriasViewModel
														   {
															   Id = t1.Id,
															   Id_Puerto = t1.Id_Puerto,
															   Id_IIPP = t1.Id_IIPP,
															   Puerto = (temp1 == null) ? "" : temp1.Nombre,
															   IIPP = (temp2 == null) ? "" : temp2.Nombre,
															   Descripcion = t1.Descripcion,
															   Tipo = t1.Tipo,
															   NombreTipo = t1.Tipo == 1 ? TipoAUD.INTER.ToDescription() : TipoAUD.EXT.ToDescription(),
															   Estado = t1.Estado,
															   NombreEstado = t1.Estado == 1 ? EstadosPEA.PRO.ToDescription() : (t1.Estado == 2 ? EstadosPEA.PLA.ToDescription() : EstadosPEA.REL.ToDescription()),
															   Ini_Programa = t1.Ini_Programa,
															   Fin_Programa = t1.Fin_Programa,
															   Ini_Planifica = t1.Ini_Planifica,
															   Fin_Planifica = t1.Fin_Planifica,
															   Ini_Real = t1.Ini_Real,
															   Fin_Real = t1.Fin_Real,
															   Responsable = t1.Responsable,
															   Recomendaciones = t1.Recomendaciones,
															   Programa = t1.Programa,
															   Conclusiones = t1.Conclusiones,
															   Seguimiento = t1.Seguimiento,
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

	}
}
