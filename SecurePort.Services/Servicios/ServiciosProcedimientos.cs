using System.Collections.Generic;
using System;
using System.Linq;
using SecurePort.Entities;
using SecurePort.Entities.Models;

namespace SecurePort.Services.Interfaces
{
    using SecurePort.Entities.Enums;

    public class ServiciosProcedimientos : IServiciosProcedimientos  
    {
        #region Atributos

        private readonly ILogger log;

        protected readonly SecurePortContext db = new SecurePortContext();

        #endregion

        #region Métodos públicos

        public ServiciosProcedimientos(ILogger log)
        {
            this.log = log;
        }

        public IEnumerable<ProcedimientosViewModel> GetAllProcedimientos(List<DependenciasUsuario> dependencias,int? categoria)
        {
            try
            {

                IEnumerable<ProcedimientosViewModel> result = new List<ProcedimientosViewModel>();


                if (categoria == (int)EnumCategoria.Puerto)
                {

                          var dependenciasPuertos = dependencias.Where(x => x.categoria == (int)EnumCategoria.Puerto).ToList();
                    
                             result = (from t1 in this.db.Procedimientos.ToList()
                                       join t2 in this.db.Organismos.ToList() on t1.Id_Organismo equals t2.Id
                                       join t3 in this.db.Puertos.ToList() on t2.Id equals t3.id_Organismo 
                                        join zz in dependenciasPuertos on t3.Id equals zz.id_Dependencia
                                        orderby t1.Titulo
                                        select new ProcedimientosViewModel
                                        {
                                            Id = t1.Id,
                                            Id_Organismo = t1.Id_Organismo,
                                            Ind_AP = t1.Ind_AP,
                                            Titulo = t1.Titulo,
                                            Fecha = t1.Fecha,
                                            Descripcion = t1.Descripcion,
                                            Observaciones = t1.Observaciones

                                        }).OrderBy(x => x.Titulo);
                }
                else if (categoria == (int)EnumCategoria.OrganismoGestionPortuaria)
                {

                    var dependenciasOrganismo = dependencias.Where(x => x.categoria == (int)EnumCategoria.OrganismoGestionPortuaria).ToList();

                    result = (from t1 in this.db.Procedimientos.ToList()
                              join t2 in this.db.Organismos.ToList() on t1.Id_Organismo equals t2.Id 
                              join zz in dependenciasOrganismo on t2.Id equals zz.id_Dependencia
                              orderby t1.Titulo
                              select new ProcedimientosViewModel
                              {
                                  Id = t1.Id,
                                  Id_Organismo = t1.Id_Organismo,
                                  Ind_AP = t1.Ind_AP,
                                  Titulo = t1.Titulo,
                                  Fecha = t1.Fecha,
                                  Descripcion = t1.Descripcion,
                                  Observaciones = t1.Observaciones

                              }).OrderBy(x => x.Titulo);
                }
                else if (categoria == (int)EnumCategoria.Instalacion)
                {

                    var dependenciasInstala = dependencias.Where(x => x.categoria == (int)EnumCategoria.Instalacion).ToList();

                    result = (from t1 in this.db.Procedimientos.ToList()
                              join t2 in this.db.Organismos.ToList() on t1.Id_Organismo equals t2.Id 
                              join t3 in this.db.Puertos.ToList() on t2.Id equals t3.id_Organismo 
                              join t4 in this.db.MOV_Instalaciones.ToList() on t3.Id equals t4.id_Puerto
                              join zz in dependenciasInstala on t4.Id equals zz.id_Dependencia
                              orderby t1.Titulo
                              select new ProcedimientosViewModel
                              {
                                  Id = t1.Id,
                                  Id_Organismo = t1.Id_Organismo,
                                  Ind_AP = t1.Ind_AP,
                                  Titulo = t1.Titulo,
                                  Fecha = t1.Fecha,
                                  Descripcion = t1.Descripcion,
                                  Observaciones = t1.Observaciones

                              }).OrderBy(x => x.Titulo);
                }

                return result;
            }
            catch (Exception ex)
            {
                this.log.PublishException(ex);

                throw;
            }
        }

        public IEnumerable<ProcedimientosViewModel> GetAllProcedimientos()
        {
            try
            {

                IEnumerable<ProcedimientosViewModel> result = (from t1 in this.db.Procedimientos.ToList()
                                                               join t2 in this.db.Organismos.ToList() on t1.Id_Organismo equals t2.Id into OrganismoTemp
                                                               from or in OrganismoTemp.DefaultIfEmpty()
                                                               orderby t1.Titulo
                                                            select new ProcedimientosViewModel
                                                            {
                                                                Id = t1.Id,
                                                                Id_Organismo = t1.Id_Organismo,
                                                                Organismo = (or==null)? string.Empty: or.Nombre,
                                                                Ind_AP = t1.Ind_AP,
                                                                Titulo = t1.Titulo,
                                                                Fecha = t1.Fecha,
                                                                Descripcion = t1.Descripcion,
                                                                Observaciones = t1.Observaciones

                                                            }).OrderBy(x => x.Titulo);

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
