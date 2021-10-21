using System.Collections.Generic;
using System;
using System.Linq;
using SecurePort.Entities;
using SecurePort.Entities.Models;


namespace SecurePort.Services.Interfaces
{
    public class ServiciosCnae : IServiciosCnae
    {


        #region Atributos
        private readonly ILogger log;
        protected readonly SecurePortContext db = new SecurePortContext();
        #endregion

        #region Métodos públicos
        public ServiciosCnae(ILogger log)
        {
            this.log = log;
        }

        public IEnumerable<CNAE> GetAllCNAE()
        {
            try
            {   
                //var ListaCNAE = db.CNAE.ToList();
                //return ListaCNAE;
            }
            catch (Exception ex)
            {
                this.log.PublishException(ex);
                throw;
            }

        }
        #endregion



        public bool GetCNAE(string IdCodigo)
        {
            try
            {
                bool existe = false;
                var result = db.CNAE.Where(x => x.IdCodigo == IdCodigo).ToList();
                return existe = (result.Any() ? true : false);
            }
            catch (Exception ex)
            {
                this.log.PublishException(ex);
                throw;
            }
        }
    }
}
