using System.Collections.Generic;
using System;
using System.Linq;
using SecurePort.Entities;
using SecurePort.Entities.Models;

namespace SecurePort.Services.Interfaces
{
    using SecurePort.Entities.Enums;

    public class ServiciosIncidentes : IServiciosIncidentes
    {
        #region Atributos

        private readonly ILogger log;

        protected readonly SecurePortContext db = new SecurePortContext();

        #endregion

        #region Métodos públicos

        public ServiciosIncidentes(ILogger log)
        {
            this.log = log;
        }

         #endregion

        #region Incidentes
        public IEnumerable<IncidentesViewModel> GetAllIncidentes(List<DependenciasUsuario> dependencias, int? categoria)
        {
            try
            {

                IEnumerable<IncidentesViewModel> result = new List<IncidentesViewModel>();

                var dependenciasInstala = new List<DependenciasUsuario>();

                if (categoria == (int)EnumCategoria.OrganismoGestionPortuaria)
                {

                    result = (from t1 in this.db.Incidentes.ToList()
                              join t3 in this.db.Puertos.ToList() on t1.Id_Puerto equals t3.Id into PuertosTemp
                              from pt in PuertosTemp.DefaultIfEmpty()
                              join t4 in this.db.Organismos.ToList() on pt.id_Organismo equals t4.Id into OrganismoTemp
                              from py in OrganismoTemp.DefaultIfEmpty()
                              join zz in dependencias.Where(x => x.categoria == (int)EnumCategoria.OrganismoGestionPortuaria) on py.Id equals zz.id_Dependencia
                              orderby t1.Descripcion
                              select new IncidentesViewModel
                              {
                                  Id = t1.Id,
                                  Id_Puerto = t1.Id_Puerto,
                                  Puerto = (pt == null) ? "" : pt.Nombre,
                                  Tipo = t1.Tipo,
                                  NombreTipo = t1.Tipo == 1 ? TipoINC.VIOL.ToDescription() : TipoINC.AMEN.ToDescription(),
                                  Descripcion = t1.Descripcion,
                                  Fecha = t1.Fecha,
                                  Observaciones = t1.Observaciones
                              });

                }else if (categoria == (int)EnumCategoria.Puerto)
                {


                             result =(from t1 in this.db.Incidentes.ToList()
                                                                            join t3 in this.db.Puertos.ToList() on t1.Id_Puerto equals t3.Id into PuertosTemp
                                                                            from pt in PuertosTemp.DefaultIfEmpty()
                                                                            join zz in dependencias.Where(x => x.categoria == (int)EnumCategoria.Puerto) on pt.Id equals zz.id_Dependencia
                                                                            orderby t1.Descripcion
                                                                        select new IncidentesViewModel
                                                                        {
                                                                            Id            = t1.Id,
                                                                            Id_Puerto     = t1.Id_Puerto,
                                                                            Puerto        = (pt == null) ? "" : pt.Nombre,
                                                                             Tipo          = t1.Tipo,
                                                                            NombreTipo    = t1.Tipo ==1? TipoINC.VIOL.ToDescription() : TipoINC.AMEN.ToDescription(),
                                                                            Descripcion   = t1.Descripcion, 
                                                                            Fecha         = t1.Fecha,
                                                                            Observaciones = t1.Observaciones
                                                                        });





                }
                else if (categoria == (int)EnumCategoria.Instalacion)
                {

                    dependenciasInstala = dependencias.Where(x => x.categoria == (int)EnumCategoria.Instalacion).ToList();

                    result = (from t1 in this.db.Incidentes.ToList()
                              join t3 in this.db.Puertos.ToList() on t1.Id_Puerto equals t3.Id into PuertosTemp
                              from pt in PuertosTemp.DefaultIfEmpty()
                              join f5 in this.db.MOV_Instalaciones.ToList() on pt.Id equals f5.id_Puerto into InstaTemp
                              from Insta in InstaTemp.DefaultIfEmpty()
                              orderby t1.Descripcion
                              select new IncidentesViewModel
                              {
                                  Id = t1.Id,
                                  Id_Puerto = t1.Id_Puerto,
                                  Id_IIPP = (Insta == null) ? 0 : Insta.Id,
                                  Puerto = (pt == null) ? "" : pt.Nombre,
                                  Tipo = t1.Tipo,
                                  NombreTipo = t1.Tipo == 1 ? TipoINC.VIOL.ToDescription() : TipoINC.AMEN.ToDescription(),
                                  Descripcion = t1.Descripcion,
                                  Fecha = t1.Fecha,
                                  Observaciones = t1.Observaciones
                              });

                    if (dependenciasInstala.Any())
                    {
                        result = (from o in result
                                     join zz in dependenciasInstala on o.Id_IIPP equals zz.id_Dependencia
                                  select new IncidentesViewModel
                                  {
                                      Id = o.Id,
                                      Id_Puerto = o.Id_Puerto,
                                      Id_IIPP = o.Id_IIPP,
                                      Puerto = o.Puerto,
                                      Tipo = o.Tipo,
                                      NombreTipo = o.NombreTipo,
                                      Descripcion = o.Descripcion,
                                      Fecha = o.Fecha,
                                      Observaciones = o.Observaciones
                                  });
                    }



                }
                
                result = (from j1 in result
                              select new IncidentesViewModel
                                                            {
                                                                Id            = j1.Id,
                                                                Id_Puerto     = j1.Id_Puerto,
                                                                Puerto        = j1.Puerto,
                                                                IIPP = MostarInstalaciones(j1.Id_Puerto, j1.Id, true, dependenciasInstala) == null ? string.Empty : MostarInstalaciones(j1.Id_Puerto, j1.Id, true, dependenciasInstala).FirstOrDefault().Nombre,
                                                                Tipo          = j1.Tipo,
                                                                NombreTipo    = j1.Tipo ==1? TipoINC.VIOL.ToDescription() : TipoINC.AMEN.ToDescription(),
                                                                Descripcion   = j1.Descripcion, 
                                                                Fecha         = j1.Fecha,
                                                                Observaciones = j1.Observaciones
                                                            });
                if (categoria == (int)EnumCategoria.Instalacion)
                {
                    result = result.ToList().Where(x => x.IIPP != string.Empty);
                }



                return result;
            }
            catch (Exception ex)
            {
                this.log.PublishException(ex);

                throw;
            }
        }

        public IEnumerable<IncidentesViewModel> GetAllIncidentes()
        {
            try
            {

                IEnumerable<IncidentesViewModel> result = (from t1 in this.db.Incidentes.ToList()
                                                           join t3 in this.db.Puertos.ToList() on t1.Id_Puerto equals t3.Id into PuertosTemp
                                                           from pt in PuertosTemp.DefaultIfEmpty()
                                                           orderby t1.Descripcion
                                                           select new IncidentesViewModel
                                                           {
                                                               Id = t1.Id,
                                                               Id_Puerto = t1.Id_Puerto,
                                                               Puerto = (pt == null) ? "" : pt.Nombre,
                                                               Tipo = t1.Tipo,
                                                               NombreTipo = t1.Tipo == 1 ? TipoINC.VIOL.ToDescription() : TipoINC.AMEN.ToDescription(),
                                                               Descripcion = t1.Descripcion,
                                                               Fecha = t1.Fecha,
                                                               Observaciones = t1.Observaciones
                                                           });
                 result = (from j1 in result
                              select new IncidentesViewModel
                                                            {
                                                                Id            = j1.Id,
                                                                Id_Puerto     = j1.Id_Puerto,
                                                                Puerto        = j1.Puerto,
                                                                IIPP          = MostarInstalaciones(j1.Id_Puerto, j1.Id, true, null) == null ? string.Empty : MostarInstalaciones(j1.Id_Puerto, j1.Id, true,null).FirstOrDefault().Nombre,
                                                                Tipo          = j1.Tipo,
                                                                NombreTipo    = j1.Tipo ==1? TipoINC.VIOL.ToDescription() : TipoINC.AMEN.ToDescription(),
                                                                Descripcion   = j1.Descripcion, 
                                                                Fecha         = j1.Fecha,
                                                                Observaciones = j1.Observaciones
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

        #region Instalaciones

        private IEnumerable<InstalacionesViewModel> MostarInstalaciones(int IdPuerto, int? IdIncidente, bool Consulta, List<DependenciasUsuario> dependencias)
        {
            try
            {
                    IEnumerable<InstalacionesViewModel> resultado = new List<InstalacionesViewModel>();

                    if (IdIncidente != null)
                    {
                        resultado = (from a1 in db.MOV_Instalaciones.ToList().Where(x => x.id_Puerto == IdPuerto && x.Es_Activo == true).OrderBy(x => x.Nombre).ToList()
                                     join b1 in db.Det_Incidente.ToList().Where(j=> j.Id_Incidente == IdIncidente) on a1.Id equals b1.Id_IIPP
                                     select new InstalacionesViewModel
                                     {
                                         Id = a1.Id,
                                         Nombre = a1.Nombre
                                     }).OrderBy(x=> x.Nombre);    
                    }
                    else
                    {
                      resultado = (from a1 in db.MOV_Instalaciones.ToList().Where(x => x.id_Puerto == IdPuerto && x.Es_Activo == true).OrderBy(x => x.Nombre)
                                    select new InstalacionesViewModel
                                    {
                                        Id = a1.Id,
                                        Nombre = a1.Nombre
                                    }).OrderBy(x => x.Nombre);
                    }

                    if (dependencias != null)
                    {
                        resultado = (from f1 in resultado.ToList()
                                     join z1 in dependencias on f1.Id equals z1.id_Dependencia
                                     select new InstalacionesViewModel
                                     {
                                         Id = f1.Id,
                                         Nombre = f1.Nombre
                                     }).OrderBy(x => x.Nombre);
                    }

                if (Consulta)
                {
                    if (resultado.Count() > 1)
                        resultado = null;

                    if (resultado != null)
                        if (resultado.Count() == 0)
                            resultado = null;

                }
                
                return resultado;

            }
            catch (Exception ex)
            {
                this.log.PublishException(ex);
                throw;
            }


        }

        public IEnumerable<InstalacionesViewModel> GetInstalaciones(int IdPuerto, int? IdCategoria, List<DependenciasUsuario> dependencias)
        {

            try
            {
                IEnumerable<InstalacionesViewModel> resultado = new List<InstalacionesViewModel>(MostarInstalaciones(IdPuerto, null,  false, null));

                if (IdCategoria == (int)EnumCategoria.OrganismoGestionPortuaria || IdCategoria == (int)EnumCategoria.Puerto || IdCategoria == (int)EnumCategoria.Instalacion)
                {
                    resultado = (from q1 in resultado.ToList()
                                 join zz in dependencias.ToList().Where(x => x.categoria == (int)EnumCategoria.Instalacion).ToList() on q1.Id equals zz.id_Dependencia
                                 select new InstalacionesViewModel
                                 {
                                     Id = q1.Id,
                                     Nombre = q1.Nombre
                                 }).OrderBy(x => x.Nombre);
                }

                return resultado;
            }
            catch (Exception ex)
            {
                this.log.PublishException(ex);
                throw;
            }

        }

        public IEnumerable<Det_IncidenteViewModel> GetInstalacionesbyIncidente(int IdIncidente)
        {
            try
            {

                IEnumerable<Det_IncidenteViewModel> resultado = (from a1 in db.Det_Incidente.ToList().Where(x => x.Id_Incidente == IdIncidente)
                                                             join b1 in db.MOV_Instalaciones.ToList() on a1.Id_IIPP equals b1.Id
                                                             select new Det_IncidenteViewModel
                                                             {
                                                                 Id = a1.Id,
                                                                 Id_IIPP = a1.Id_IIPP,
                                                                 IIPP = b1.Nombre
                                                             });

                return resultado;
            }
            catch (Exception ex)
            {
                this.log.PublishException(ex);
                throw;
            }
        }

        public IEnumerable<MOV_Instalaciones> GetDependenciaInstalaciones(int id, int? idCategoria)
        {
            try
            {
                IEnumerable<MOV_Instalaciones> result;
                if (idCategoria == 5)
                {
                    result = (from t1 in this.db.Depen_Usuario.Where(x => x.id_Usuario == id)
                              join t3 in this.db.MOV_Instalaciones.ToList() on t1.id_Dependencia equals t3.Id
                              select new MOV_Instalaciones { Id = t3.Id, Nombre = t3.Nombre, id_Puerto = t3.id_Puerto }).OrderBy(z => z.Nombre);
                }
                else
                {
                    result = (from t1 in this.db.Usuarios.Where(x => x.id == id && x.categoria == idCategoria).ToList()
                              join t2 in this.db.Depen_Usuario.ToList() on t1.id equals t2.id_Usuario
                              join t3 in this.db.MOV_Instalaciones.ToList() on t2.id_Dependencia equals t3.Id
                              orderby t3.Id
                              select new MOV_Instalaciones { Id = t3.Id, Nombre = t3.Nombre, id_Puerto = t3.id_Puerto });
                }

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