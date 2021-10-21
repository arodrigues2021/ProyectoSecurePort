using System.Collections.Generic;
using System;
using System.Linq;
using SecurePort.Entities;
using SecurePort.Entities.Models;


namespace SecurePort.Services.Interfaces
{
    public class ServiciosAmenazas : IServiciosAmenazas 
    {
       
        #region Atributos
            private readonly ILogger log;
            protected readonly SecurePortContext db = new SecurePortContext();
        #endregion

        #region Métodos públicos

        public ServiciosAmenazas(ILogger log)
        {
            this.log = log;
        }

        public IEnumerable<AmenazasViewModel> GetAllAmenazas()
        {
            try
            {
                IEnumerable<AmenazasViewModel> result =
                   (from t1 in this.db.Amenazas.ToList()
                       orderby t1.Descripcion
                    select new AmenazasViewModel { Id = t1.Id, Descripcion = t1.Descripcion, activado = (t1.es_activo == true? "Si": "No"),es_activo = t1.es_activo }).OrderBy(x => x.Descripcion);

                return result;
            }
            catch (Exception ex)
            {
                this.log.PublishException(ex);
                throw;
            }

        }

        public IEnumerable<AmenazasViewModel> ListAmenazas()
        {
            try
            {
                IEnumerable<AmenazasViewModel> result =
                   (from t1 in this.db.Amenazas.ToList()
                    orderby t1.Descripcion
                    select new AmenazasViewModel
                    {
                        Id = t1.Id,
                        Descripcion = t1.Descripcion,
                        es_activo = t1.es_activo
                    }).OrderBy(x => x.Id).ThenBy(l => l.Descripcion);

                return result;
            }
            catch (Exception ex)
            {
                this.log.PublishException(ex);
                throw;
            }

        }

        public IEnumerable<Amenazas> ListAmenazas(string nombre)
        {
            try
            {

                var result = this.db.Amenazas.Where(x => x.Descripcion == nombre).ToList();
                return result;

            }
            catch (Exception ex)
            {
                this.log.PublishException(ex);
                throw;
            }
        }

        public IEnumerable<Amenazas> ListAmenazas(string nombre, int Id)
        {
            try
            {
                var result = this.db.Amenazas.Where(x => x.Id != Id && x.Descripcion == nombre).ToList();
                return result;

            }
            catch (Exception ex)
            {
                this.log.PublishException(ex);
                throw;
            }
        }

        public bool ListAmenazas(string nombre, int Id, bool estado)
        {
            try
            {
                bool CambioEstado = false;
                if (estado == false)
                {
                    foreach (Amenazas amenazas in this.db.Amenazas.Where(x => x.Id == Id && x.Descripcion == nombre).ToList())
                    {
                        if (amenazas.es_activo)
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

        public IEnumerable<Amenazas> GetIdAmenazas(int id)
        {
            try
            {
                var ListaAmenazas = db.Amenazas.Where(x => x.Id == id && x.es_activo).ToList();
                return ListaAmenazas;
            }
            catch (Exception ex)
            {
                this.log.PublishException(ex);
                throw;
            }
        }

        #endregion




        public IEnumerable<AmenazasViewModel> GetAllAmenazasPadreHijos()
        {
            try
            {
                IEnumerable<AmenazasViewModel> result =
                   (from t1 in this.db.Amenazas.ToList()
                    orderby t1.Descripcion
                    select new AmenazasViewModel { Id = t1.Id, Descripcion = t1.Descripcion, activado = "No", es_activo = t1.es_activo }).OrderBy(x => x.Descripcion);

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
