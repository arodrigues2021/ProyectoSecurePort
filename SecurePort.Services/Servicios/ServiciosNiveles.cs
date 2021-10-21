using System.Collections.Generic;
using System;
using System.Linq;
using SecurePort.Entities;
using SecurePort.Entities.Models;

namespace SecurePort.Services.Interfaces
{
    using SecurePort.Entities.Enums;

    public class ServiciosNiveles : IServiciosNiveles  
    {
        #region Atributos

        private readonly ILogger log;

        protected readonly SecurePortContext db = new SecurePortContext();

        #endregion

        #region Métodos públicos

        public ServiciosNiveles(ILogger log)
        {
            this.log = log;
        }

        #region Niveles

        public IEnumerable<NivelesViewModel> GetAllNiveles(List<DependenciasUsuario> dependencias,int? categoria)
        {
            try
            {
                IEnumerable<NivelesViewModel> resultado = new List<NivelesViewModel>();
                
                var dependenciasNiveles = new List<DependenciasUsuario>();

                if (categoria == (int)EnumCategoria.Puerto)
                {
                   var dependenciasPuNiveles = dependencias.Where(x => x.categoria == (int)EnumCategoria.Puerto).ToList();

                   if (dependenciasPuNiveles.Any())
                    {
                        resultado = (from t1 in this.db.Niveles.ToList()
                                     join t3 in this.db.Puertos.ToList() on t1.Id_Puerto equals t3.Id into PuertosTemp
                                     from pt in PuertosTemp.DefaultIfEmpty()
                                     join zz in dependenciasPuNiveles on pt.Id equals zz.id_Dependencia
                                     orderby t1.Descripcion
                                     select new NivelesViewModel
                                     {
                                         Id = t1.Id,
                                         Id_Puerto = t1.Id_Puerto,
                                         Puerto = (pt == null) ? "" : pt.Nombre,
                                         PuertoActivo = (pt == null) ? false : (bool)pt.es_activo,
                                         Descripcion = t1.Descripcion,
                                         Fecha = t1.Fecha,
                                         Emisor = t1.Emisor,
                                         Receptor = t1.Receptor,
                                         Nivel = t1.Nivel,
                                         Emisor_Cancela = t1.Emisor_Cancela,
                                         Duracion = t1.Duracion,
                                         Fecha_Cancela = t1.Fecha_Cancela,
                                         Nivel_Max = t1.Nivel_Max,
                                         Relevante = t1.Relevante,
                                         Recomendacion = t1.Recomendacion,
                                         Observaciones = t1.Observaciones
                                     });


                    }
                }
                else if (categoria == (int)EnumCategoria.OrganismoGestionPortuaria)
                {
                    var dependenciasOrgNiveles = dependencias.Where(x => x.categoria == (int)EnumCategoria.OrganismoGestionPortuaria).ToList();

                    if (dependenciasOrgNiveles.Any())
                    {
                        resultado = (from t1 in this.db.Niveles.ToList()
                                     join t3 in this.db.Puertos.ToList() on t1.Id_Puerto equals t3.Id into PuertosTemp
                                     from pt in PuertosTemp.DefaultIfEmpty()
                                     join a1 in this.db.Organismos.ToList().Where(x => x.es_activo) on pt.id_Organismo equals a1.Id
                                     join zz in dependenciasOrgNiveles on a1.Id equals zz.id_Dependencia
                                     orderby t1.Descripcion
                                     select new NivelesViewModel
                                     {
                                         Id = t1.Id,
                                         Id_Puerto = t1.Id_Puerto,
                                         Puerto = (pt == null) ? "" : pt.Nombre,
                                         PuertoActivo = (pt == null) ? false : (bool)pt.es_activo,
                                         Descripcion = t1.Descripcion,
                                         Fecha = t1.Fecha,
                                         Emisor = t1.Emisor,
                                         Receptor = t1.Receptor,
                                         Nivel = t1.Nivel,
                                         Emisor_Cancela = t1.Emisor_Cancela,
                                         Duracion = t1.Duracion,
                                         Fecha_Cancela = t1.Fecha_Cancela,
                                         Nivel_Max = t1.Nivel_Max,
                                         Relevante = t1.Relevante,
                                         Recomendacion = t1.Recomendacion,
                                         Observaciones = t1.Observaciones
                                     });


                    }
                }
                else if (categoria == (int)EnumCategoria.Instalacion)
                {
                    dependenciasNiveles = dependencias.Where(x => x.categoria == (int)EnumCategoria.Instalacion).ToList();

                    if (dependenciasNiveles.Any())
                    {
                        resultado = (from t1 in this.db.Niveles.ToList()
                                     join t3 in this.db.Puertos.ToList() on t1.Id_Puerto equals t3.Id into PuertosTemp
                                     from pt in PuertosTemp.DefaultIfEmpty()
                                     join f5 in this.db.MOV_Instalaciones.ToList() on pt.Id equals f5.id_Puerto into InstaTemp
                                     from Insta in InstaTemp.DefaultIfEmpty()
                                     join a1 in this.db.Organismos.ToList().Where(x => x.es_activo) on pt.id_Organismo equals a1.Id
                                     orderby t1.Descripcion
                                     select new NivelesViewModel
                                     {
                                         Id = t1.Id,
                                         Id_Puerto = t1.Id_Puerto,
                                         Id_IIPP = (Insta == null) ? 0 : Insta.Id,
                                         Puerto = (pt == null) ? "" : pt.Nombre,
                                         PuertoActivo = (pt == null) ? false : (bool)pt.es_activo,
                                         Descripcion = t1.Descripcion,
                                         Fecha = t1.Fecha,
                                         Emisor = t1.Emisor,
                                         Receptor = t1.Receptor,
                                         Nivel = t1.Nivel,
                                         Emisor_Cancela = t1.Emisor_Cancela,
                                         Duracion = t1.Duracion,
                                         Fecha_Cancela = t1.Fecha_Cancela,
                                         Nivel_Max = t1.Nivel_Max,
                                         Relevante = t1.Relevante,
                                         Recomendacion = t1.Recomendacion,
                                         Observaciones = t1.Observaciones
                                     });

                        if (dependenciasNiveles.Any())
                        {
                            resultado = (from o in resultado
                                         join zz in dependenciasNiveles on o.Id_IIPP equals zz.id_Dependencia
                                         join tt in db.Det_Nivel on zz.id_Dependencia equals tt.Id_IIPP
                                         select new NivelesViewModel
                                         {
                                             Id = o.Id,
                                             Id_Puerto = o.Id_Puerto,
                                             Id_IIPP = o.Id,
                                             Puerto = o.Puerto,
                                             PuertoActivo = o.PuertoActivo,
                                             Descripcion = o.Descripcion,
                                             Fecha = o.Fecha,
                                             Emisor = o.Emisor,
                                             Receptor = o.Receptor,
                                             Nivel = o.Nivel,
                                             Emisor_Cancela = o.Emisor_Cancela,
                                             Duracion = o.Duracion,
                                             Fecha_Cancela = o.Fecha_Cancela,
                                             Nivel_Max = o.Nivel_Max,
                                             Relevante = o.Relevante,
                                             Recomendacion = o.Recomendacion,
                                             Observaciones = o.Observaciones
                                         });
                        }

                    }
                }
                
                    resultado = (from j1 in resultado.ToList()
                                    select new NivelesViewModel
                                    {
                                        Id = j1.Id,
                                        Id_Puerto = j1.Id_Puerto,
                                        Puerto = j1.Puerto,
                                        PuertoActivo = j1.PuertoActivo,
                                        Descripcion = j1.Descripcion,
                                        IIPP = MostarInstalaciones(j1.Id_Puerto, j1.Id, true, dependenciasNiveles) == null ? string.Empty : MostarInstalaciones(j1.Id_Puerto, j1.Id, true,dependenciasNiveles).FirstOrDefault().Nombre,
                                        Fecha = j1.Fecha,
                                        Emisor = j1.Emisor,
                                        Receptor = j1.Receptor,
                                        Nivel = j1.Nivel,
                                        Emisor_Cancela = j1.Emisor_Cancela,
                                        Duracion = j1.Duracion,
                                        Fecha_Cancela = j1.Fecha_Cancela,
                                        Nivel_Max = j1.Nivel_Max,
                                        Relevante = j1.Relevante,
                                        Recomendacion = j1.Recomendacion,
                                        Observaciones = j1.Observaciones
                                    });

                    if (categoria == (int)EnumCategoria.Instalacion)
                    {
                        resultado = resultado.ToList().Where(x => x.IIPP != string.Empty);
                    }

                return resultado.GroupBy(c => c.Id).Select(grp => grp.First()).ToList();
             
            }
            catch (Exception ex)
            {
                this.log.PublishException(ex);

                throw;
            }
        }

        public IEnumerable<NivelesViewModel> GetAllNiveles()
        {
            try
            {
                 IEnumerable<NivelesViewModel> result = new List<NivelesViewModel>();
                result = (from t1 in this.db.Niveles.ToList()
                                join t3 in this.db.Puertos.ToList() on t1.Id_Puerto equals t3.Id into PuertosTemp
                                from pt in PuertosTemp.DefaultIfEmpty()
                                orderby t1.Descripcion
                                select new NivelesViewModel
                                {
                                    Id = t1.Id,
                                    Id_Puerto = t1.Id_Puerto,
                                    Puerto = (pt == null) ? "" : pt.Nombre,
                                    PuertoActivo = (pt == null) ? false : (bool)pt.es_activo,
                                    Descripcion = t1.Descripcion,
                                    Fecha = t1.Fecha,
                                    Emisor = t1.Emisor,
                                    Receptor = t1.Receptor,
                                    Nivel = t1.Nivel,
                                    Emisor_Cancela = t1.Emisor_Cancela,
                                    Duracion = t1.Duracion,
                                    Fecha_Cancela = t1.Fecha_Cancela,
                                    Nivel_Max = t1.Nivel_Max,
                                    Relevante = t1.Relevante,
                                    Recomendacion = t1.Recomendacion,
                                    Observaciones = t1.Observaciones
                                });

                result = (from j1 in result.ToList()
                                    select new NivelesViewModel
                                    {
                                        Id = j1.Id,
                                        Id_Puerto = j1.Id_Puerto,
                                        Puerto = j1.Puerto,
                                        PuertoActivo = j1.PuertoActivo,
                                        Descripcion = j1.Descripcion,
                                        IIPP = MostarInstalaciones(j1.Id_Puerto, j1.Id, true, null) == null ? string.Empty : MostarInstalaciones(j1.Id_Puerto, j1.Id, true,null).FirstOrDefault().Nombre,
                                        Fecha = j1.Fecha,
                                        Emisor = j1.Emisor,
                                        Receptor = j1.Receptor,
                                        Nivel = j1.Nivel,
                                        Emisor_Cancela = j1.Emisor_Cancela,
                                        Duracion = j1.Duracion,
                                        Fecha_Cancela = j1.Fecha_Cancela,
                                        Nivel_Max = j1.Nivel_Max,
                                        Relevante = j1.Relevante,
                                        Recomendacion = j1.Recomendacion,
                                        Observaciones = j1.Observaciones
                                    });
                                     
                                     //)

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

        private IEnumerable<InstalacionesViewModel> MostarInstalaciones(int IdPuerto, int? IdNivel,  bool Consulta, List<DependenciasUsuario> dependencias)
        {
            try
            {
                IEnumerable<InstalacionesViewModel> resultado = new List<InstalacionesViewModel>();
                if (IdNivel != null) 
                { 
                    resultado =(from a1 in db.MOV_Instalaciones.ToList().Where(x => x.id_Puerto == IdPuerto && x.Es_Activo == true)
                     join b1 in db.Det_Nivel.ToList().Where(j=> j.Id_Nivel == IdNivel) on a1.Id equals b1.Id_IIPP
                                                                    select new InstalacionesViewModel
                                                                    {
                                                                        Id = a1.Id,
                                                                        Nombre = a1.Nombre
                                                                    }).OrderBy(x => x.Nombre);
                }
                else {
                    resultado = (from a1 in db.MOV_Instalaciones.ToList().Where(x => x.id_Puerto == IdPuerto && x.Es_Activo == true)
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
                }

                if (resultado != null) 
                    if (resultado.Count() == 0)
                        resultado = null;   

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

        public IEnumerable<Det_NivelViewModel> GetInstalacionesbyNivel(int IdNivel)
        {
            try
            {

                IEnumerable<Det_NivelViewModel> resultado = (from a1 in db.Det_Nivel.ToList().Where(x => x.Id_Nivel == IdNivel)
                                                             join b1 in db.MOV_Instalaciones.ToList() on a1.Id_IIPP equals b1.Id
                                                             select new Det_NivelViewModel
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
                IEnumerable<MOV_Instalaciones> result ;
                if (idCategoria == 5)
                {
                    result = (from t1 in this.db.Depen_Usuario.Where(x=> x.id_Usuario == id)
                              join t3 in this.db.MOV_Instalaciones.ToList() on t1.id_Dependencia equals t3.Id
                              select new MOV_Instalaciones { Id = t3.Id, Nombre = t3.Nombre, id_Puerto = t3.id_Puerto }).OrderBy(z=> z.Nombre);
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


        #endregion
    }
}
