using System.Collections.Generic;
using System;
using System.Linq;
using SecurePort.Entities;
using SecurePort.Entities.Models;


namespace SecurePort.Services.Interfaces
{
    public class ServiciosMotivos : IServiciosMotivos 
    {
       
        #region Atributos
            private readonly ILogger log;
            protected readonly SecurePortContext db = new SecurePortContext();
        #endregion

        #region Métodos públicos
        public ServiciosMotivos(ILogger log)
        {
            this.log = log;
        }

        public IEnumerable<MotivosViewModel> GetAllMotivos()
        {
            try
            {
                IEnumerable<MotivosViewModel> result =
                   (from t1 in this.db.Motivos.ToList().ToList()
                     orderby t1.Id
                    select new MotivosViewModel { Id = t1.Id, Descripcion = t1.Descripcion, Motivo = t1.Motivo, activado = (t1.es_activo == true? "Si": "No"),
                                                es_activo = t1.es_activo }).OrderBy(x => x.Id).ThenBy(l => l.Motivo);

                return result;
            }
            catch (Exception ex)
            {
                this.log.PublishException(ex);
                throw;
            }

        }

        public IEnumerable<MotivosViewModel> ListMotivos()
        {
            try
            {
                IEnumerable<MotivosViewModel> result =
                   (from t1 in this.db.Motivos.ToList()
                    orderby t1.Id
                    select new MotivosViewModel
                    {
                        Id = t1.Id,
                        Descripcion = t1.Descripcion,
                        Motivo = t1.Motivo,
                        es_activo = t1.es_activo
                    }).OrderBy(x => x.Id).ThenBy(l => l.Motivo);

                return result;
            }
            catch (Exception ex)
            {
                this.log.PublishException(ex);
                throw;
            }

        }

        #endregion

        public IEnumerable<Motivos> ListMotivos(string codigo)
        {
            try
            {
               
                var result = this.db.Motivos.Where(x => x.Motivo  == codigo).ToList();
                return result;

            }
            catch (Exception ex)
            {
                this.log.PublishException(ex);
                throw;
            }
        }

        public IEnumerable<Motivos> ListMotivos(string motivo, int Id)
        {
            try
            {
                var result = this.db.Motivos.Where(x => x.Id != Id && x.Motivo == motivo).ToList();
                return result;

            }
            catch (Exception ex)
            {
                this.log.PublishException(ex);
                throw;
            }
        }

        public bool ListMotivos(string motivo, int Id, bool estado)
        {
            try
            {
                bool CambioEstado = false;
                if (estado == false)
                {
                    foreach (Motivos motivos in this.db.Motivos.Where(x => x.Id == Id && x.Motivo == motivo).ToList())
                    {
                        if (motivos.es_activo)
                            CambioEstado = true;
                    }
                }
                return CambioEstado;

            }
            catch (Exception ex)
            {
                this.log.PublishException(ex);
                throw;
            }
        }


        public IEnumerable<Motivos> GetIdMotivos(int id)
        {
            try
            {
                var ListaMotivos = db.Motivos.Where(x=> x.Id==id && x.es_activo).ToList();
                return ListaMotivos;
            }
            catch (Exception ex)
            {
                this.log.PublishException(ex);
                throw;
            }
        }
    }
}
