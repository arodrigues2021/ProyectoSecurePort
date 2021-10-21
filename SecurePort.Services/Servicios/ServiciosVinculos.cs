using System.Collections.Generic;
using System;
using System.Linq;
using SecurePort.Entities;
using SecurePort.Entities.Models;

namespace SecurePort.Services.Interfaces
{
    using SecurePort.Entities.Enums;

    public class ServiciosVinculos : IServiciosVinculos  
    {
        #region Atributos

        private readonly ILogger log;

        protected readonly SecurePortContext db = new SecurePortContext();

        #endregion

        #region Métodos públicos

        public ServiciosVinculos(ILogger log)
        {
            this.log = log;
        }

        public IEnumerable<Vinculos> GetAllVinculos(string Tipo, int? IdCategoria)
        {
            try
            {

                IEnumerable<Vinculos> result = (from t1 in this.db.Vinculos.ToList().Where(x=> x.Tipo == Tipo && (x.Id_Categoria == IdCategoria || x.Id_Categoria == null))
                                                            select new Vinculos
                                                                   {
                                                                       Id = t1.Id,
                                                                       Tipo = t1.Tipo,
                                                                       Id_Categoria = t1.Id_Categoria,
                                                                       Nombre = t1.Nombre,
                                                                       Descripcion = t1.Descripcion,
                                                                       Orden = t1.Orden
                                                                   }).OrderBy(x=> x.Orden);

                return result;
            }
            catch (Exception ex)
            {
                this.log.PublishException(ex);

                throw;
            }
        }

        public IEnumerable<Vinculos> GetAllVinculos(string Tipo)
        {
            try
            {

                IEnumerable<Vinculos> result = (from t1 in this.db.Vinculos.ToList().Where(x=> x.Tipo == Tipo)
                                                            select new Vinculos
                                                            {
                                                                Id = t1.Id,
                                                                Tipo = t1.Tipo,
                                                                Id_Categoria = t1.Id_Categoria,
                                                                Nombre = t1.Nombre,
                                                                Descripcion = t1.Descripcion,
                                                                Orden = t1.Orden
                                                            }).OrderBy(x => x.Orden);

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
