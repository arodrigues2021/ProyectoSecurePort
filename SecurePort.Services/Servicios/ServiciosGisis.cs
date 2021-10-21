using System.Collections.Generic;
using System;
using System.Linq;
using SecurePort.Entities;
using SecurePort.Entities.Models;

namespace SecurePort.Services.Interfaces
{
    using SecurePort.Entities.Enums;

    public class ServiciosGisis : IServiciosGisis  
    {
        #region Atributos

        private readonly ILogger log;

        protected readonly SecurePortContext db = new SecurePortContext();

        #endregion

        #region Métodos públicos

        public ServiciosGisis(ILogger log)
        {
            this.log = log;
        }

        public IEnumerable<GisisViewModel> GetAllGisis(List<DependenciasUsuario> dependencias,int? categoria)
        {
            try
            {

                IEnumerable<GisisViewModel> result = new List<GisisViewModel>();


                if (categoria == (int)EnumCategoria.Puerto)
                {

                          var dependenciasPuertos = dependencias.Where(x => x.categoria == (int)EnumCategoria.Puerto).ToList();
                    
                             result = (from t1 in this.db.Gisis.ToList()
                                                          join t3 in this.db.Puertos.ToList() on t1.Id_Puerto equals t3.Id into PuertosTemp
                                                          from pt in PuertosTemp.DefaultIfEmpty()
                                                          join t4 in this.db.MOV_Instalaciones.ToList() on t1.Id_IIPP equals t4.Id into InstTemp
                                                          from it in InstTemp.DefaultIfEmpty()
                                                         join zz in dependenciasPuertos on pt.Id equals zz.id_Dependencia
                                                          orderby t1.Motivo
                                                          select new GisisViewModel
                                                          {
                                                              Id = t1.Id,
                                                              Id_Puerto = t1.Id_Puerto,
                                                              Puerto = (pt == null) ? "" : pt.Nombre,
                                                              Id_IIPP = t1.Id_IIPP,
                                                              IIPP = (it == null) ? "" : it.Nombre,
                                                              Tipo_Registro = t1.Tipo_Registro,
                                                              Registro = t1.Tipo_Registro == 1 ? TipoRegistro.ALT.ToDescription() : (t1.Tipo_Registro == 2 ? TipoRegistro.MOD.ToDescription() : TipoRegistro.BAJ.ToDescription()),
                                                              Fecha_Registro = t1.Fech_Registro,
                                                              Motivo = t1.Motivo

                                                          }).OrderBy(x => x.Puerto);
                }
                else if (categoria == (int)EnumCategoria.OrganismoGestionPortuaria)
                {

                    var dependenciasOrganismo = dependencias.Where(x => x.categoria == (int)EnumCategoria.OrganismoGestionPortuaria).ToList();

                    result = (from t1 in this.db.Gisis.ToList()
                              join t3 in this.db.Puertos.ToList() on t1.Id_Puerto equals t3.Id into PuertosTemp
                              from pt in PuertosTemp.DefaultIfEmpty()
                              join t4 in this.db.MOV_Instalaciones.ToList() on t1.Id_IIPP equals t4.Id into InstTemp
                              from it in InstTemp.DefaultIfEmpty()
                              join zz in dependenciasOrganismo on pt.id_Organismo equals zz.id_Dependencia
                              orderby t1.Motivo
                              select new GisisViewModel
                              {
                                  Id = t1.Id,
                                  Id_Puerto = t1.Id_Puerto,
                                  Puerto = (pt == null) ? "" : pt.Nombre,
                                  Id_IIPP = t1.Id_IIPP,
                                  IIPP = (it == null) ? "" : it.Nombre,
                                  Tipo_Registro = t1.Tipo_Registro,
                                  Registro = t1.Tipo_Registro == 1 ? TipoRegistro.ALT.ToDescription() : (t1.Tipo_Registro == 2 ? TipoRegistro.MOD.ToDescription() : TipoRegistro.BAJ.ToDescription()),
                                  Fecha_Registro = t1.Fech_Registro,
                                  Motivo = t1.Motivo

                              }).OrderBy(x => x.Puerto);
                }
                else if (categoria == (int)EnumCategoria.Instalacion)
                {

                    var dependenciasInstala = dependencias.Where(x => x.categoria == (int)EnumCategoria.Instalacion).ToList();

                    result = (from t1 in this.db.Gisis.ToList()
                              join t3 in this.db.Puertos.ToList() on t1.Id_Puerto equals t3.Id into PuertosTemp
                              from pt in PuertosTemp.DefaultIfEmpty()
                              join t4 in this.db.MOV_Instalaciones.ToList() on t1.Id_IIPP equals t4.Id into InstTemp
                              from it in InstTemp.DefaultIfEmpty()
                              join zz in dependenciasInstala on t1.Id_IIPP equals zz.id_Dependencia
                              orderby t1.Motivo
                              select new GisisViewModel
                              {
                                  Id = t1.Id,
                                  Id_Puerto = t1.Id_Puerto,
                                  Puerto = (pt == null) ? "" : pt.Nombre,
                                  Id_IIPP = t1.Id_IIPP,
                                  IIPP = (it == null) ? "" : it.Nombre,
                                  Tipo_Registro = t1.Tipo_Registro,
                                  Registro = t1.Tipo_Registro == 1 ? TipoRegistro.ALT.ToDescription() : (t1.Tipo_Registro == 2 ? TipoRegistro.MOD.ToDescription() : TipoRegistro.BAJ.ToDescription()),
                                  Fecha_Registro = t1.Fech_Registro,
                                  Motivo = t1.Motivo

                              }).OrderBy(x => x.Puerto);
                }

                return result;
            }
            catch (Exception ex)
            {
                this.log.PublishException(ex);

                throw;
            }
        }

        public IEnumerable<GisisViewModel> GetAllGisis()
        {
            try
            {

                IEnumerable<GisisViewModel> result = (from t1 in this.db.Gisis.ToList()
                                                            join t3 in this.db.Puertos.ToList() on t1.Id_Puerto equals t3.Id into PuertosTemp
                                                            from pt in PuertosTemp.DefaultIfEmpty()
                                                            join t4 in this.db.MOV_Instalaciones.ToList() on t1.Id_IIPP equals t4.Id into InstalacionTemp
                                                            from it in InstalacionTemp.DefaultIfEmpty()
                                                            orderby t1.Motivo
                                                            select new GisisViewModel
                                                            {
                                                                Id = t1.Id,
                                                                Id_Puerto = t1.Id_Puerto,
                                                                Puerto = (pt == null) ? "" : pt.Nombre,
                                                                Id_IIPP = t1.Id_IIPP,
                                                                IIPP = (it == null) ? "" : it.Nombre,
                                                                Tipo_Registro = t1.Tipo_Registro,
                                                                Registro = t1.Tipo_Registro == 1 ? TipoRegistro.ALT.ToDescription() : (t1.Tipo_Registro == 2 ? TipoRegistro.MOD.ToDescription() : TipoRegistro.BAJ.ToDescription()),
                                                                Fecha_Registro = t1.Fech_Registro,
                                                                Motivo = t1.Motivo
                                                               
                                                            }).OrderBy(x => x.Puerto);

                return result;
            }
            catch (Exception ex)
            {
                this.log.PublishException(ex);

                throw;
            }
        }

        public override string ToString()
        {
            return base.ToString();
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }


        #endregion
    }
}
