using System.Collections.Generic;
using System;
using System.Linq;
using SecurePort.Entities;
using SecurePort.Entities.Models;

namespace SecurePort.Services.Interfaces
{
    using SecurePort.Entities.Enums;

    public class ServiciosEvaluaciones : IServiciosEvaluaciones  
    {
        #region Atributos

        private readonly ILogger log;

        protected readonly SecurePortContext db = new SecurePortContext();

        #endregion

        #region Métodos públicos

        public ServiciosEvaluaciones(ILogger log)
        {
            this.log = log;
        }

        #region Evaluaciones
        public IEnumerable<EvaluacionesViewModel> GetAllEvaluaciones(List<DependenciasUsuario> dependencias,int? categoria)
        {
            try
            {

                IEnumerable<EvaluacionesViewModel> result = new List<EvaluacionesViewModel>();


                if (categoria == (int)EnumCategoria.Puerto)
                {

                    var dependenciasPuertos = dependencias.Where(x => x.categoria == (int)EnumCategoria.Puerto).ToList();
                    result = (from t1 in this.db.Evaluaciones.ToList()
                                join t3 in this.db.Puertos.ToList() on t1.Id_Puerto equals t3.Id into PuertosTemp
                                from pt in PuertosTemp.DefaultIfEmpty()
                                join t4 in this.db.MOV_Instalaciones.ToList() on t1.Id_IIPP equals t4.Id into InstalacionTemp
                                from it in InstalacionTemp.DefaultIfEmpty()
                                join zz in dependenciasPuertos on pt.Id equals zz.id_Dependencia
                                orderby t1.Nombre
                                select new EvaluacionesViewModel
                                {
                                    Id = t1.Id,
                                    Nombre = t1.Nombre,
                                    Id_Puerto = t1.Id_Puerto,
                                    Puerto = (pt == null) ? "" : pt.Nombre,
                                    Id_IIPP = t1.Id_IIPP,
                                    IIPP = (it == null) ? "" : it.Nombre,
                                    Fech_Alta = t1.Fech_Alta
                                });
                            
                }
                else if (categoria == (int)EnumCategoria.OrganismoGestionPortuaria)
                {

                    var dependenciasOrganismo = dependencias.Where(x => x.categoria == (int)EnumCategoria.OrganismoGestionPortuaria).ToList();

                    result =  (from t1 in this.db.Evaluaciones.ToList()
                                join t3 in this.db.Puertos.ToList() on t1.Id_Puerto equals t3.Id into PuertosTemp
                                from pt in PuertosTemp.DefaultIfEmpty()
                                join t4 in this.db.MOV_Instalaciones.ToList() on t1.Id_IIPP equals t4.Id into InstalacionTemp
                                from it in InstalacionTemp.DefaultIfEmpty()
                                join zz in dependenciasOrganismo on pt.id_Organismo equals zz.id_Dependencia
                                orderby t1.Nombre
                                select new EvaluacionesViewModel
                                {
                                    Id = t1.Id,
                                    Nombre = t1.Nombre,
                                    Id_Puerto = t1.Id_Puerto,
                                    Puerto = (pt == null) ? "" : pt.Nombre,
                                    Id_IIPP = t1.Id_IIPP,
                                    IIPP = (it == null) ? "" : it.Nombre,
                                    Fech_Alta = t1.Fech_Alta
                                });                         
                
                }
                else if (categoria == (int)EnumCategoria.Instalacion)
                {

                    var dependenciasInstala = dependencias.Where(x => x.categoria == (int)EnumCategoria.Instalacion).ToList();

                    result = (from t1 in this.db.Evaluaciones.ToList()
                              join t3 in this.db.Puertos.ToList() on t1.Id_Puerto equals t3.Id into PuertosTemp
                              from pt in PuertosTemp.DefaultIfEmpty()
                              join t4 in this.db.MOV_Instalaciones.ToList() on t1.Id_IIPP equals t4.Id into InstalacionTemp
                              from it in InstalacionTemp.DefaultIfEmpty()
                              join zz in dependenciasInstala on t1.Id_IIPP equals zz.id_Dependencia
                              orderby t1.Nombre
                              select new EvaluacionesViewModel
                              {
                                  Id = t1.Id,
                                  Nombre = t1.Nombre,
                                  Id_Puerto = t1.Id_Puerto,
                                  Puerto = (pt == null) ? "" : pt.Nombre,
                                  Id_IIPP = t1.Id_IIPP,
                                  IIPP = (it == null) ? "" : it.Nombre,
                                  Fech_Alta = t1.Fech_Alta
                              });
                              
                }

               result =  (from o in result
                                from a1 in this.db.Versiones_Evaluacion.ToList().Where(c => c.Id_Evaluacion == o.Id).OrderByDescending(c => c.Version).Take(1)
                                select new EvaluacionesViewModel
                                {
                                    Id = o.Id,
                                    Nombre = o.Nombre,
                                    Id_Puerto = o.Id_Puerto,
                                    Puerto = o.Puerto,
                                    Id_IIPP = o.Id_IIPP,
                                    IIPP = o.IIPP,
                                    Version = a1.Version,
                                    Estado = a1.Estado,
                                    NombreEstado = a1.Estado== 1? EstadoEvaluacion.ALT.ToDescription():(a1.Estado ==2? EstadoEvaluacion.PRO.ToDescription(): (a1.Estado ==3 ? EstadoEvaluacion.CON.ToDescription(): (a1.Estado==4 ? EstadoEvaluacion.TRA.ToDescription():(a1.Estado==5? EstadoEvaluacion.APR.ToDescription(): EstadoEvaluacion.REC.ToDescription())))),
                                    Responsable = a1.Responsable,
                                    Cargo = a1.Cargo,
                                    Fech_Cierre = a1.Fech_Cierre,
                                    Fech_Procesada = a1.Fech_Procesada,
                                    Fech_Conforme = a1.Fech_Conforme,
                                    Fech_Tramitada = a1.Fech_Tramitada,
                                    Fech_Aprobada = a1.Fech_Aprobada,
                                    Fech_Rechazada = a1.Fech_Rechazada,
                                    Fech_NoConforme = a1.Fech_NoConforme,
                                    Fech_NoTramitada = a1.Fech_NoTramitada,
                                    Fech_Zonificacion = a1.Fech_Zonificacion,
                                    Fech_Info_Eval = a1.Fech_Info_Eval,
                                    Fech_Info_Vune = a1.Fech_Info_Vune,
                                    Observaciones = a1.Observaciones,
                                    Fech_Alta = o.Fech_Alta
                                }).OrderBy(x => x.Nombre);


                return result;
            }
            catch (Exception ex)
            {
                this.log.PublishException(ex);

                throw;
            }
        }

        public IEnumerable<EvaluacionesViewModel> GetAllEvaluaciones()
        {
            try
            {

                IEnumerable<EvaluacionesViewModel> result = (from o in
                                                              (from t1 in this.db.Evaluaciones.ToList()
                                                               join t3 in this.db.Puertos.ToList() on t1.Id_Puerto equals t3.Id into PuertosTemp
                                                               from pt in PuertosTemp.DefaultIfEmpty()
                                                               join t4 in this.db.MOV_Instalaciones.ToList() on t1.Id_IIPP equals t4.Id into InstalacionTemp
                                                               from it in InstalacionTemp.DefaultIfEmpty()
                                                               orderby t1.Nombre
                                                               select new EvaluacionesViewModel
                                                               {
                                                                   Id = t1.Id,
                                                                   Nombre = t1.Nombre,
                                                                   Id_Puerto = t1.Id_Puerto,
                                                                   Puerto = (pt == null) ? "" : pt.Nombre,
                                                                   Id_IIPP = t1.Id_IIPP,
                                                                   IIPP = (it == null) ? "" : it.Nombre,
                                                                   Fech_Alta = t1.Fech_Alta
                                                               })
                                                             from a1 in this.db.Versiones_Evaluacion.ToList().Where(c=> c.Id_Evaluacion == o.Id).OrderByDescending(c => c.Version).Take(1)
                                                              select new EvaluacionesViewModel 
                                                              {
                                                                 Id = o.Id,
                                                                 Nombre = o.Nombre,
                                                                 Id_Puerto = o.Id_Puerto,
                                                                 Puerto = o.Puerto,
                                                                 Id_IIPP = o.Id_IIPP,
                                                                 IIPP = o.IIPP,
                                                                 Version  = a1.Version,
                                                                 Estado = a1.Estado,
                                                                 NombreEstado = a1.Estado == 1 ? EstadoEvaluacion.ALT.ToDescription() : (a1.Estado == 2 ? EstadoEvaluacion.PRO.ToDescription() : (a1.Estado == 3 ? EstadoEvaluacion.CON.ToDescription() : (a1.Estado == 4 ? EstadoEvaluacion.TRA.ToDescription() : (a1.Estado == 5 ? EstadoEvaluacion.APR.ToDescription() : EstadoEvaluacion.REC.ToDescription())))),
                                                                 Responsable = a1.Responsable,
                                                                 Cargo = a1.Cargo,
                                                                 Fech_Cierre = a1.Fech_Cierre,
                                                                 Fech_Procesada = a1.Fech_Procesada,
                                                                 Fech_Conforme = a1.Fech_Conforme,
                                                                 Fech_Tramitada = a1.Fech_Tramitada,
                                                                 Fech_Aprobada = a1.Fech_Aprobada,
                                                                 Fech_Rechazada = a1.Fech_Rechazada,
                                                                 Fech_NoConforme = a1.Fech_NoConforme,
                                                                 Fech_NoTramitada = a1.Fech_NoTramitada,
                                                                 Fech_Zonificacion = a1.Fech_Zonificacion,
                                                                 Fech_Info_Eval = a1.Fech_Info_Eval,
                                                                 Fech_Info_Vune = a1.Fech_Info_Vune,
                                                                 Observaciones = a1.Observaciones,
                                                                 Fech_Alta = o.Fech_Alta
                                                              }).OrderBy(x => x.Nombre);

                return result;
            }
            catch (Exception ex)
            {
                this.log.PublishException(ex);

                throw;
            }
        }

        public IEnumerable<Versiones_Evaluacion> GetAllVersiones(int IdEvaluacion)
        {
            try
            {

                IEnumerable<Versiones_Evaluacion> result = (from a1 in this.db.Versiones_Evaluacion.ToList().Where(c => c.Id_Evaluacion == IdEvaluacion)
                                                             select new Versiones_Evaluacion
                                                             {
                                                                 Id = a1.Id,                                                               
                                                                 Version = a1.Version,
                                                                 Estado = a1.Estado,
                                                                 Responsable = a1.Responsable,
                                                                 Cargo = a1.Cargo,
                                                                 Fech_Cierre = a1.Fech_Cierre,
                                                                 Fech_Procesada = a1.Fech_Procesada,
                                                                 Fech_Conforme = a1.Fech_Conforme,
                                                                 Fech_Tramitada = a1.Fech_Tramitada,
                                                                 Fech_Aprobada = a1.Fech_Aprobada,
                                                                 Fech_Rechazada = a1.Fech_Rechazada,
                                                                 Fech_NoConforme = a1.Fech_NoConforme,
                                                                 Fech_NoTramitada = a1.Fech_NoTramitada,
                                                                 Fech_Zonificacion = a1.Fech_Zonificacion,
                                                                 Fech_Info_Eval = a1.Fech_Info_Eval,
                                                                 Fech_Info_Vune = a1.Fech_Info_Vune,
                                                                 Observaciones = a1.Observaciones
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

        #region Historico Evaluaciones
        public IEnumerable<Historico_Evaluacion> GetEvaluacionesByIdEvaluaVersion(int idEvaluacion, int IdVersion )
        {
            try
            {

                IEnumerable<Historico_Evaluacion> result =  (from t1 in this.db.Historico_Evaluacion.ToList().Where(x=> x.Id_Evaluacion == idEvaluacion && x.Id_Version_Eval == IdVersion)                                                                  
                                                                  select new Historico_Evaluacion
                                                                  {
                                                                      Id = t1.Id,
                                                                      Id_Evaluacion = t1.Id_Evaluacion,
                                                                      Id_Version_Eval = t1.Id_Version_Eval,
                                                                      Accion = t1.Accion,
                                                                      Observaciones = t1.Observaciones, 
                                                                      Fech_Accion = t1.Fech_Accion
                                                                  }).OrderBy(x => x.Id);

                return result;
            }
            catch (Exception ex)
            {
                this.log.PublishException(ex);

                throw;
            }
        }


        #endregion

        #region DetalleEvaluaciones

        public IEnumerable<Detalle_EvaluacionViewModel> GetAllBienesByInstalacionBienDetail(int idInstalacion, int idBien)
        {
            try
            {
                IEnumerable<Detalle_EvaluacionViewModel> amenazas = new List<Detalle_EvaluacionViewModel>();
                amenazas = (from o in db.MOV_Detalle_Instalacion.Where(x => x.ID_Instalacion == idInstalacion && x.ID_Bien == idBien).ToList()
                             join am in db.Amenazas.ToList() on o.ID_Amenaza equals am.Id
                            select new Detalle_EvaluacionViewModel
                             {
                                 Id_Amenaza = am.Id,
                                 Amenaza = am.Descripcion                                 
                             });

                return amenazas;
            }
            catch (Exception ex)
            {
                this.log.PublishException(ex);
                throw;
            }
                
        }


        public IEnumerable<Detalle_EvaluacionViewModel> GetAllBienesByInstalacionBienEvaluacionDetail(int idInstalacion, int idBien, int IdEvaluacion)
        {
            try
            {
                IEnumerable<Detalle_EvaluacionViewModel> amenazas = new List<Detalle_EvaluacionViewModel>();
                amenazas = (from dev in
                            (from o in db.MOV_Detalle_Instalacion.Where(x => x.ID_Instalacion == idInstalacion && x.ID_Bien == idBien).ToList()
                                join ev in GetAllEvaluaciones().Where(x => x.Id == IdEvaluacion).ToList() on o.ID_Instalacion equals ev.Id_IIPP 
                                join am in db.Amenazas.ToList() on o.ID_Amenaza equals am.Id
                                select new Detalle_EvaluacionViewModel
                                {
                                    Id_Amenaza = am.Id,
                                    Amenaza = am.Descripcion,
                                    Id_Evaluacion = ev.Id,
                                    Id_Instalacion = ev.Id_IIPP,
                                    Id_Version_Eval = ev.Version,
                                    Id_Bien = o.ID_Bien
                                })
                                join de in db.Detalle_Evaluacion on new { dev.Id_Evaluacion, dev.Id_Version_Eval,dev.Id_Instalacion, dev.Id_Bien,dev.Id_Amenaza } equals new { de.Id_Evaluacion, de.Id_Version_Eval,de.Id_Instalacion,de.Id_Bien,de.Id_Amenaza } into DetalleTemp
                                 from devInsBien in DetalleTemp.DefaultIfEmpty()
                                    select new Detalle_EvaluacionViewModel
                                    {
                                        Id_Amenaza = dev.Id_Amenaza,
                                        Amenaza = dev.Amenaza,
                                        Id_Evaluacion = dev.Id_Evaluacion,
                                        Id_Instalacion = dev.Id_Instalacion,
                                        Id_Version_Eval = dev.Id_Version_Eval,
                                        Id_Bien = dev.Id_Bien,
                                        tipo = (devInsBien == null) ? "N": "M",
                                        Id = (devInsBien == null) ? 0 : devInsBien.Id,
                                        IND_IAS_ITP = (devInsBien == null) ? false : (devInsBien.IND_IAS_ITP == (bool?)null ? false : devInsBien.IND_IAS_ITP),
                                        EstadoChk = (devInsBien == null) ? "" : (devInsBien.IND_IAS_ITP == (bool?)null ? "" : (devInsBien.IND_IAS_ITP == true ? "checked = 'checked'" : "")),
                                        NP1_IAI = (devInsBien == null) ? 1 : (devInsBien.NP1_IAI == null ? 1 : devInsBien.NP1_IAI),
                                        NP1_ISD = (devInsBien == null) ? 1: (devInsBien.NP1_ISD  == null ? 1 : devInsBien.NP1_ISD),
                                        NP1_IIO = (devInsBien == null) ? 1: (devInsBien.NP1_IIO  == null ? 1 : devInsBien.NP1_IIO),
                                        NP1_IDV = (devInsBien == null) ? 1: (devInsBien.NP1_IDV == null ? 1 : devInsBien.NP1_IDV),
                                        NP1_IDE_D = (devInsBien == null) ? 0: (devInsBien.NP1_IDE_D == null ? 0 : devInsBien.NP1_IDE_D),
                                        NP1_IDE_I = (devInsBien == null) ? 0: (devInsBien.NP1_IDE_I == null ? 0 : devInsBien.NP1_IDE_I),
                                        NP1_IRD = (devInsBien == null) ? 1: (devInsBien.NP1_IRD == null ? 1 : devInsBien.NP1_IRD),
                                        NP1_IRR = (devInsBien == null) ? 1: (devInsBien.NP1_IRR == null ? 1 : devInsBien.NP1_IRR),
                                        NP1_ISA_MA  = (devInsBien == null) ? 0: (devInsBien.NP1_ISA_MA == null ? 0 : devInsBien.NP1_ISA_MA),
                                        NP1_ISA_PA  = (devInsBien == null) ? 0: (devInsBien.NP1_ISA_PA == null ? 0 : devInsBien.NP1_ISA_PA),
                                        NP1_ISA_AS  = (devInsBien == null) ? 0: (devInsBien.NP1_ISA_AS == null ? 0 : devInsBien.NP1_ISA_AS),
                                        NP1_RESULTADO = (devInsBien == null) ? null: devInsBien.NP1_RESULTADO,
                                        NP1_ID = (devInsBien == null) ? null: devInsBien.NP1_ID, 
                                        NP1_IC  = (devInsBien == null) ? null: devInsBien.NP1_IC, 
                                        NP1_IV  = (devInsBien == null) ? null: devInsBien.NP1_IV,
                                        NP2_IAI = (devInsBien == null) ? 1: (devInsBien.NP2_IAI == null ? 1 : devInsBien.NP2_IAI),
                                        NP2_ISD = (devInsBien == null) ? 1: (devInsBien.NP2_ISD == null ? 1 : devInsBien.NP2_ISD),
                                        NP2_IIO = (devInsBien == null) ? 1: (devInsBien.NP2_IIO == null ? 1 : devInsBien.NP2_IIO),
                                        NP2_IDV = (devInsBien == null) ? 1: (devInsBien.NP2_IDV == null ? 1 : devInsBien.NP2_IDV),
                                        NP2_IDE_D = (devInsBien == null) ? 0: (devInsBien.NP2_IDE_D == null ? 0 : devInsBien.NP2_IDE_D),
                                        NP2_IDE_I = (devInsBien == null) ? 0: (devInsBien.NP2_IDE_I == null ? 0 : devInsBien.NP2_IDE_I),
                                        NP2_IRD = (devInsBien == null) ? 1: (devInsBien.NP2_IRD == null ? 1 : devInsBien.NP2_IRD),
                                        NP2_IRR = (devInsBien == null) ? 1: (devInsBien.NP2_IRR == null ? 1 : devInsBien.NP2_IRR),
                                        NP2_ISA_MA = (devInsBien == null) ? 0: (devInsBien.NP2_ISA_MA == null ? 1 : devInsBien.NP2_ISA_MA),
                                        NP2_ISA_PA = (devInsBien == null) ? 0: (devInsBien.NP2_ISA_PA == null ? 1 : devInsBien.NP2_ISA_PA),
                                        NP2_ISA_AS = (devInsBien == null) ? 0: (devInsBien.NP2_ISA_AS == null ? 1 : devInsBien.NP2_ISA_AS),
                                        NP2_RESULTADO = (devInsBien == null) ? null: devInsBien.NP2_RESULTADO,
                                        NP2_ID = (devInsBien == null) ? null: devInsBien.NP2_ID,
                                        NP2_IC = (devInsBien == null) ? null: devInsBien.NP2_IC,
                                        NP2_IV = (devInsBien == null) ? null: devInsBien.NP2_IV,
                                        NP3_IAI = (devInsBien == null) ? 1: (devInsBien.NP3_IAI == null ? 1 : devInsBien.NP3_IAI),
                                        NP3_ISD = (devInsBien == null) ? 1: (devInsBien.NP3_ISD == null ? 1 : devInsBien.NP3_ISD),
                                        NP3_IIO = (devInsBien == null) ? 1: (devInsBien.NP3_IIO == null ? 1 : devInsBien.NP3_IIO),
                                        NP3_IDV = (devInsBien == null) ? 1: (devInsBien.NP3_IDV == null ? 1 : devInsBien.NP3_IDV),
                                        NP3_IDE_D = (devInsBien == null) ? 0: (devInsBien.NP3_IDE_D == null ? 0 : devInsBien.NP3_IDE_D),
                                        NP3_IDE_I = (devInsBien == null) ? 0: (devInsBien.NP3_IDE_I == null ? 0 : devInsBien.NP3_IDE_I),
                                        NP3_IRD = (devInsBien == null) ? 1: (devInsBien.NP3_IRD == null ? 1 : devInsBien.NP3_IRD),
                                        NP3_IRR = (devInsBien == null) ? 1: (devInsBien.NP3_IRR == null ? 1 : devInsBien.NP3_IRR),
                                        NP3_ISA_MA = (devInsBien == null) ? 0: (devInsBien.NP3_ISA_MA == null ? 1 : devInsBien.NP3_ISA_MA),
                                        NP3_ISA_PA = (devInsBien == null) ? 0: (devInsBien.NP3_ISA_PA == null ? 1 : devInsBien.NP3_ISA_PA),
                                        NP3_ISA_AS = (devInsBien == null) ? 0: (devInsBien.NP3_ISA_AS == null ? 1 : devInsBien.NP3_ISA_AS),
                                        NP3_RESULTADO = (devInsBien == null) ? null: devInsBien.NP3_RESULTADO,
                                        NP3_ID = (devInsBien == null) ? null: devInsBien.NP3_ID,
                                        NP3_IC = (devInsBien == null) ? null: devInsBien.NP3_IC,
                                        NP3_IV = (devInsBien == null) ? null: devInsBien.NP3_IV
        
                                    });

                return amenazas;
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
