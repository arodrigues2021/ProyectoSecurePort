using System.Collections.Generic;
using System;
using System.Linq;
using SecurePort.Entities;
using SecurePort.Entities.Models;


namespace SecurePort.Services.Interfaces
{
    using SecurePort.Entities.Enums;

    public class ServiciosPracticas : IServiciosPracticas 
    {
       
        #region Atributos
            private readonly ILogger log;
            protected readonly SecurePortContext db = new SecurePortContext();
        #endregion

        #region Métodos públicos

        public ServiciosPracticas(ILogger log)
        {
            this.log = log;
        }
        
        #endregion

        #region Practicas

        public IEnumerable<PracticasViewModel> ListPracticas()
        {
            try
            {

                IEnumerable<PracticasViewModel> result = new List<PracticasViewModel>();
                
                         result = (from t1 in this.db.Practicas.ToList()
                            join t2 in this.db.Puertos.ToList() on t1.Id_Puerto equals t2.Id into TempPuertos
                            from temp1 in TempPuertos.DefaultIfEmpty()
                            orderby t1.Id
                            select new PracticasViewModel
                                    {
                                        Id = t1.Id,
                                        Id_Puerto = t1.Id_Puerto,
                                        Puerto = (temp1 == null) ? "" : temp1.Nombre,
                                        Descripcion = t1.Descripcion,
                                        Tipo = t1.Tipo,
                                        NombreTipo =  t1.Tipo ==1? TipoPE.PRAC.ToDescription() : TipoPE.EJER.ToDescription(), 
                                        Estado = t1.Estado,
                                        NombreEstado = t1.Estado == 1 ? EstadosPEA.PRO.ToDescription() : (t1.Estado == 2 ? EstadosPEA.PLA.ToDescription() : EstadosPEA.REL.ToDescription()),
                                        Ini_Programa = t1.Ini_Programa,
                                        Fin_Programa = t1.Fin_Programa,
                                        Ini_Planifica = t1.Ini_Planifica,
                                        Fin_Planifica = t1.Fin_Planifica,
                                        Ini_Real = t1.Ini_Real,
                                        Fin_Real = t1.Fin_Real,
                                        Responsable = t1.Responsable,
                                        Cuerpos = t1.Cuerpos,
                                        Valoracion = t1.Valoracion,
                                        NombreValoracion = t1.Valoracion == 1? ValoracionPE.OK.ToDescription() : ValoracionPE.NOOK.ToDescription(),
                                        Ratifico = t1.Ratifico,
                                        NombreRatifico = t1.Ratifico? "Si": "No",
                                        Conclusiones = t1.Conclusiones,
                                        Propuesta = t1.Propuesta,
                                        Observaciones = t1.Observaciones
                                    });

                         result = (from j1 in result.ToList()
                                   select new PracticasViewModel
                                   {
                                       Id = j1.Id,
                                       Id_Puerto = j1.Id_Puerto,
                                       Puerto = j1.Puerto,
                                       IIPP = MostarInstalaciones(j1.Id_Puerto, j1.Id, true,null) == null ? string.Empty : MostarInstalaciones(j1.Id_Puerto, j1.Id,  true, null).FirstOrDefault().Nombre,
                                       Descripcion = j1.Descripcion,
                                       Tipo = j1.Tipo,
                                       NombreTipo = j1.NombreTipo,
                                       Estado = j1.Estado,
                                       NombreEstado = j1.NombreEstado,
                                       Ini_Programa = j1.Ini_Programa,
                                       Fin_Programa = j1.Fin_Programa,
                                       Ini_Planifica = j1.Ini_Planifica,
                                       Fin_Planifica = j1.Fin_Planifica,
                                       Ini_Real = j1.Ini_Real,
                                       Fin_Real = j1.Fin_Real,
                                       Responsable = j1.Responsable,
                                       Cuerpos = j1.Cuerpos,
                                       Valoracion = j1.Valoracion,
                                       NombreValoracion = j1.NombreValoracion,
                                       Ratifico = j1.Ratifico,
                                       NombreRatifico = j1.NombreRatifico,
                                       Conclusiones = j1.Conclusiones,
                                       Propuesta = j1.Propuesta,
                                       Observaciones = j1.Observaciones
                                   }).OrderBy(x=> x.Descripcion);

                    
                return result;
            }
            catch (Exception ex)
            {
                this.log.PublishException(ex);
                throw;
            }
        }


        public IEnumerable<PracticasViewModel> ListPracticas(List<DependenciasUsuario> dependencias,int? categoria)
        {

            
            try
            {
                IEnumerable<PracticasViewModel> result = new List<PracticasViewModel>();
                 var dependenciasInstalacion  = new List<DependenciasUsuario>();

                if (categoria == (int)EnumCategoria.Puerto)
                {
                    var dependenciasPuerto = dependencias.Where(x => x.categoria == (int)EnumCategoria.Puerto).ToList();

                    if (dependenciasPuerto.Any())
                    {

                        result = (from t1 in this.db.Practicas.ToList()
                            join t2 in this.db.Puertos.ToList() on t1.Id_Puerto equals t2.Id into TempPuertos
                            from temp1 in TempPuertos.DefaultIfEmpty()
                            join t5 in dependenciasPuerto on temp1.Id equals t5.id_Dependencia
                            orderby t1.Id
                            select new PracticasViewModel
                                   {
                                       Id = t1.Id,
                                       Id_Puerto = t1.Id_Puerto,
                                       Puerto = (temp1 == null)
                                           ? ""
                                           : temp1.Nombre,
                                       Descripcion = t1.Descripcion,
                                       Tipo = t1.Tipo,
                                       NombreTipo = t1.Tipo == 1
                                           ? TipoPE.PRAC.ToDescription()
                                           : TipoPE.EJER.ToDescription(),
                                       Estado = t1.Estado,
                                       NombreEstado = t1.Estado == 1
                                           ? EstadosPEA.PRO.ToDescription()
                                           : (t1.Estado == 2
                                               ? EstadosPEA.PLA.ToDescription()
                                               : EstadosPEA.REL.ToDescription()),
                                       Ini_Programa = t1.Ini_Programa,
                                       Fin_Programa = t1.Fin_Programa,
                                       Ini_Planifica = t1.Ini_Planifica,
                                       Fin_Planifica = t1.Fin_Planifica,
                                       Ini_Real = t1.Ini_Real,
                                       Fin_Real = t1.Fin_Real,
                                       Responsable = t1.Responsable,
                                       Cuerpos = t1.Cuerpos,
                                       Valoracion = t1.Valoracion,
                                       NombreValoracion = t1.Valoracion == 1
                                           ? ValoracionPE.OK.ToDescription()
                                           : ValoracionPE.NOOK.ToDescription(),
                                       Ratifico = t1.Ratifico,
                                       NombreRatifico = t1.Ratifico
                                           ? "Si"
                                           : "No",
                                       Conclusiones = t1.Conclusiones,
                                       Propuesta = t1.Propuesta,
                                       Observaciones = t1.Observaciones
                                   });
                        
                    }
                }
                else if (categoria == (int)EnumCategoria.OrganismoGestionPortuaria)
                {
                    var dependenciasOrganismo = dependencias.Where(x => x.categoria == (int)EnumCategoria.OrganismoGestionPortuaria).ToList();

                    if (dependenciasOrganismo.Any())
                    {

                        result = (from t1 in this.db.Practicas.ToList()
                                  join t2 in this.db.Puertos.ToList() on t1.Id_Puerto equals t2.Id into TempPuertos
                                  from temp1 in TempPuertos.DefaultIfEmpty()
                                  join a1 in this.db.Organismos.ToList().Where(x => x.es_activo) on temp1.id_Organismo equals a1.Id
                                  join t5 in dependenciasOrganismo on a1.Id equals t5.id_Dependencia
                                  orderby t1.Id
                                  select new PracticasViewModel
                                  {
                                      Id = t1.Id,
                                      Id_Puerto = t1.Id_Puerto,
                                      Puerto = (temp1 == null)
                                          ? ""
                                          : temp1.Nombre,
                                      Descripcion = t1.Descripcion,
                                      Tipo = t1.Tipo,
                                      NombreTipo = t1.Tipo == 1
                                          ? TipoPE.PRAC.ToDescription()
                                          : TipoPE.EJER.ToDescription(),
                                      Estado = t1.Estado,
                                      NombreEstado = t1.Estado == 1
                                          ? EstadosPEA.PRO.ToDescription()
                                          : (t1.Estado == 2
                                              ? EstadosPEA.PLA.ToDescription()
                                              : EstadosPEA.REL.ToDescription()),
                                      Ini_Programa = t1.Ini_Programa,
                                      Fin_Programa = t1.Fin_Programa,
                                      Ini_Planifica = t1.Ini_Planifica,
                                      Fin_Planifica = t1.Fin_Planifica,
                                      Ini_Real = t1.Ini_Real,
                                      Fin_Real = t1.Fin_Real,
                                      Responsable = t1.Responsable,
                                      Cuerpos = t1.Cuerpos,
                                      Valoracion = t1.Valoracion,
                                      NombreValoracion = t1.Valoracion == 1
                                          ? ValoracionPE.OK.ToDescription()
                                          : ValoracionPE.NOOK.ToDescription(),
                                      Ratifico = t1.Ratifico,
                                      NombreRatifico = t1.Ratifico
                                          ? "Si"
                                          : "No",
                                      Conclusiones = t1.Conclusiones,
                                      Propuesta = t1.Propuesta,
                                      Observaciones = t1.Observaciones
                                  });

                        
                    }
                }
                else if (categoria == (int)EnumCategoria.Instalacion)
                {
                     dependenciasInstalacion = dependencias.Where(x => x.categoria == (int)EnumCategoria.Instalacion).ToList();

                    if (dependenciasInstalacion.Any())
                    {

                        result = (from t1 in this.db.Practicas.ToList()
                                  join t2 in this.db.Puertos.ToList() on t1.Id_Puerto equals t2.Id into TempPuertos
                                  from temp1 in TempPuertos.DefaultIfEmpty()
                                  join f5 in this.db.MOV_Instalaciones.ToList() on temp1.Id equals f5.id_Puerto into InstaTemp
                                  from Insta in InstaTemp.DefaultIfEmpty()
                                  orderby t1.Id
                                  select new PracticasViewModel
                                  {
                                      Id = t1.Id,
                                      Id_Puerto = t1.Id_Puerto,
                                      Puerto = (temp1 == null)? "": temp1.Nombre,
                                      Id_IIPP = (Insta == null)? 0: Insta.Id,
                                      Descripcion = t1.Descripcion,
                                      Tipo = t1.Tipo,
                                      NombreTipo = t1.Tipo == 1? TipoPE.PRAC.ToDescription(): TipoPE.EJER.ToDescription(),
                                      Estado = t1.Estado,
                                      NombreEstado = t1.Estado == 1? EstadosPEA.PRO.ToDescription(): (t1.Estado == 2 ? EstadosPEA.PLA.ToDescription(): EstadosPEA.REL.ToDescription()),
                                      Ini_Programa = t1.Ini_Programa,
                                      Fin_Programa = t1.Fin_Programa,
                                      Ini_Planifica = t1.Ini_Planifica,
                                      Fin_Planifica = t1.Fin_Planifica,
                                      Ini_Real = t1.Ini_Real,
                                      Fin_Real = t1.Fin_Real,
                                      Responsable = t1.Responsable,
                                      Cuerpos = t1.Cuerpos,
                                      Valoracion = t1.Valoracion,
                                      NombreValoracion = t1.Valoracion == 1? ValoracionPE.OK.ToDescription(): ValoracionPE.NOOK.ToDescription(),
                                      Ratifico = t1.Ratifico,
                                      NombreRatifico = t1.Ratifico? "Si": "No",
                                      Conclusiones = t1.Conclusiones,
                                      Propuesta = t1.Propuesta,
                                      Observaciones = t1.Observaciones
                                  });

                        result = (from o in result                                     
                                     join zz in dependenciasInstalacion on o.Id_IIPP equals zz.id_Dependencia                                    
                                       select new PracticasViewModel
                                      {
                                          Id = o.Id,
                                          Id_Puerto = o.Id_Puerto,
                                          Puerto = o.Puerto,
                                          Id_IIPP = o.Id_IIPP,
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
                                          Cuerpos = o.Cuerpos,
                                          Valoracion = o.Valoracion,
                                          NombreValoracion = o.NombreValoracion,
                                          Ratifico = o.Ratifico,
                                          NombreRatifico = o.NombreRatifico,
                                          Conclusiones = o.Conclusiones,
                                          Propuesta = o.Propuesta,
                                          Observaciones = o.Observaciones
                                      });

                    }
                }

                result = (from j1 in result.ToList()
                          select new PracticasViewModel
                          {
                              Id = j1.Id,
                              Id_Puerto = j1.Id_Puerto,
                              Puerto = j1.Puerto,
                              IIPP = MostarInstalaciones(j1.Id_Puerto, j1.Id, true, dependenciasInstalacion) == null ? string.Empty : MostarInstalaciones(j1.Id_Puerto, j1.Id, true, dependenciasInstalacion).FirstOrDefault().Nombre,
                              Descripcion = j1.Descripcion,
                              Tipo = j1.Tipo,
                              NombreTipo = j1.NombreTipo,
                              Estado = j1.Estado,
                              NombreEstado = j1.NombreEstado,
                              Ini_Programa = j1.Ini_Programa,
                              Fin_Programa = j1.Fin_Programa,
                              Ini_Planifica = j1.Ini_Planifica,
                              Fin_Planifica = j1.Fin_Planifica,
                              Ini_Real = j1.Ini_Real,
                              Fin_Real = j1.Fin_Real,
                              Responsable = j1.Responsable,
                              Cuerpos = j1.Cuerpos,
                              Valoracion = j1.Valoracion,
                              NombreValoracion = j1.NombreValoracion,
                              Ratifico = j1.Ratifico,
                              NombreRatifico = j1.NombreRatifico,
                              Conclusiones = j1.Conclusiones,
                              Propuesta = j1.Propuesta,
                              Observaciones = j1.Observaciones
                          }).OrderBy(x => x.Descripcion);

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

        #endregion

        #region Instalaciones

        private IEnumerable<InstalacionesViewModel> MostarInstalaciones(int IdPuerto, int? IdPractica, bool Consulta, List<DependenciasUsuario> dependencias)
        {
            try
            {
                

                IEnumerable<InstalacionesViewModel> resultado = new List<InstalacionesViewModel>();

                if(IdPractica != null)
                {
                  resultado = (from a1 in db.MOV_Instalaciones.ToList().Where(x => x.id_Puerto == IdPuerto && x.Es_Activo == true).ToList()
                                join b1 in db.Det_Practica.ToList().Where(x => x.Id_Practica == IdPractica) on a1.Id equals b1.Id_IIPP
                                select new InstalacionesViewModel
                                {
                                    Id = a1.Id,
                                    Nombre = a1.Nombre
                                }).OrderBy(x => x.Nombre);
                } else {
                    resultado = (from a1 in db.MOV_Instalaciones.ToList().Where(x => x.id_Puerto == IdPuerto && x.Es_Activo == true).ToList()
                                 select new InstalacionesViewModel
                                 {
                                     Id = a1.Id,
                                     Nombre = a1.Nombre
                                 }).OrderBy(x => x.Nombre);
                
                }
                if (dependencias != null)
                { 
                   resultado =  (from f1 in resultado.ToList()
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

        public IEnumerable<Det_PracticaViewModel> GetInstalacionesbyPractica(int IdPractica)
        {
            try
            {

                IEnumerable<Det_PracticaViewModel> resultado = (from a1 in db.Det_Practica.ToList().Where(x => x.Id_Practica == IdPractica)
                                                                 join b1 in db.MOV_Instalaciones.ToList() on a1.Id_IIPP equals b1.Id
                                                                 select new Det_PracticaViewModel
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
