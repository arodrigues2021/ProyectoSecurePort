namespace SecurePort.Services.Interfaces
{
    #region using

    using System.Collections;
    using System.Data;
    using System.Data.SqlClient;
    using System.Security.Cryptography;

    using SecurePort.Entities;
    using SecurePort.Entities.Enums;
    using SecurePort.Entities.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    #endregion

    public class ServiciosPaisesProvincias : IServiciosPaisesProvincias
    {
        #region Atributos

        private readonly ILogger log;

        protected readonly SecurePortContext db = new SecurePortContext();

        #endregion
         
        #region Métodos públicos

        public ServiciosPaisesProvincias(ILogger log)
        {
            this.log = log;
        }

        public IEnumerable<ListPaises> GetAllPaises()
        {
            try
            {
                IEnumerable<ListPaises> result = (from a1 in this.db.Paises.ToList()
                                                        orderby a1.cod_pais, 
                                                                a1.nombre
                                                          select new ListPaises
                                                          {
                                                             Id       = a1.Id,
                                                             nombre   = a1.nombre,
                                                             cod_pais = a1.cod_pais,
                                                             activado = a1.es_activo ? "Si" : "No"
                                                          });
                return result;

            }
            catch (Exception ex)
            {
                this.log.PublishException(ex);
                throw;
            }
        }

        public IEnumerable<Paises> ListPaises(string codigo)
        {
            try
            {
                var result = this.db.Paises.Where(x => x.cod_pais == codigo).ToList();
                return result;

            }
            catch (Exception ex)
            {
                this.log.PublishException(ex);
                throw;
            }
        }

        public IEnumerable<Paises> ListPaises(string codigo, int ID)
        {
            try
            {
                var result = this.db.Paises.Where(x => x.Id != ID && x.cod_pais == codigo).ToList();
                return result;

            }
            catch (Exception ex)
            {
                this.log.PublishException(ex);
                throw;
            }
        }

        public bool ListPaises(string nombre, int Id, bool estado)
        {
            try
            {
                bool CambioEstado = false;
                if (estado == false)
                {
                    IEnumerable<Paises> result = this.db.Paises.Where(x => x.Id == Id && x.cod_pais == nombre).ToList();
                    Paises pais = result.FirstOrDefault();
                    if (pais.es_activo != estado)
                        CambioEstado = true;
                }
                return CambioEstado;

            }
            catch (Exception ex)
            {
                this.log.PublishException(ex);
                throw;
            }
        }


        public IEnumerable<ListadoProvincias> GetAllProvincias(int ID_ComAut)
        {
            try
            {
                IEnumerable<ListadoProvincias> result = (from a1 in this.db.Provincias.Where(x => x.es_activo && x.ID_ComAut == ID_ComAut).ToList()
                                                             select new ListadoProvincias
                                                             {
                                                                 Id        = a1.id,
                                                                 codigo    = a1.codigo,
                                                                 nombre    = a1.nombre,
                                                                 ID_ComAut = a1.ID_ComAut
                                                             });
                return result;

            }
            catch (Exception ex)
            {
                this.log.PublishException(ex);
                throw;
            }
        }

        public IEnumerable<Comunidades_Autonomas> ListComunidades()
        {
            try
            {
                IEnumerable<Comunidades_Autonomas> result = this.db.Comunidades_Autonomas.Where(x => x.es_activo).ToList();
                                                         
                return result;

            }
            catch (Exception ex)
            {
                this.log.PublishException(ex);
                throw;
            }
        }

        public IEnumerable<Comunidades_Autonomas> ListComunidades(string nombre)
        {
            try
            {
                var result = this.db.Comunidades_Autonomas.Where(x => x.Nombre == nombre).ToList();
                return result;

            }
            catch (Exception ex)
            {
                this.log.PublishException(ex);
                throw;
            }
        }

        public IEnumerable<Comunidades_Autonomas> ListComunidades(string nombre, int Id)
        {
            try
            {
                var result = this.db.Comunidades_Autonomas.Where(x => x.Id != Id && x.Nombre == nombre).ToList();
                return result;

            }
            catch (Exception ex)
            {
                this.log.PublishException(ex);
                throw;
            }
        }

        public bool ListComunidades(string nombre, int Id, bool estado)
        {
            try
            {
                bool CambioEstado = false;
                if (estado == false)
                {
                    IEnumerable<Comunidades_Autonomas> result = this.db.Comunidades_Autonomas.Where(x => x.Id == Id && x.Nombre == nombre).ToList();
                    Comunidades_Autonomas comunidad = result.FirstOrDefault();
                    if (comunidad.es_activo != estado)
                        CambioEstado = true;
                }
                return CambioEstado;

            }
            catch (Exception ex)
            {
                this.log.PublishException(ex);
                throw;
            }
        }


        public IEnumerable<ListadoCiudades> ListCiudades()
        {
            try
            {
                IEnumerable<ListadoCiudades> result = (from a1 in this.db.Ciudades.ToList()
                                                        orderby a1.id
                                                          select new ListadoCiudades
                                                             {
                                                                 id           = a1.id,
                                                                 codigo       = a1.codigo,
                                                                 nombre       = a1.nombre,
                                                                 Id_provincia = a1.Id_provincia,
                                                                 activado     = a1.es_activo ? "Si" : "No",
                                                                 id_isla      = a1.Id_isla
                                                             });
                return result;

            }
            catch (Exception ex)
            {
                this.log.PublishException(ex);
                throw;
            }
        }

        public IEnumerable<Ciudades> ListCiudades(string Id_ciudad, int Id_provincia)
        {
            try
            {
                var result = this.db.Ciudades.Where(x => x.codigo == Id_ciudad && x.Id_provincia == Id_provincia).ToList();
                return result;

            }
            catch (Exception ex)
            {
                this.log.PublishException(ex);
                throw;
            }
        }

        public IEnumerable<Ciudades> ListCiudades(string Id_ciudad, int Id, int Id_provincia)
        {
            try
            {
                var result = this.db.Ciudades.Where(x => x.id != Id && x.codigo == Id_ciudad && x.Id_provincia ==  Id_provincia).ToList();
                return result;

            }
            catch (Exception ex)
            {
                this.log.PublishException(ex);
                throw;
            }
        }

        public bool ListCiudades(string nombre, int Id, bool estado)
        {
            try
            {
                bool CambioEstado = false;
                if (estado == false)
                {
                    IEnumerable<Ciudades> result = this.db.Ciudades.Where(x => x.id == Id && x.codigo == nombre).ToList();
                    Ciudades ciudad = result.FirstOrDefault();
                    if (ciudad.es_activo != estado)
                        CambioEstado = true;
                }
                return CambioEstado;

            }
            catch (Exception ex)
            {
                this.log.PublishException(ex);
                throw;
            }
        }

        public bool GetIdProvincias(string codigo)
        {
            try
            {
                bool result = this.db.Provincias.Any(x => x.es_activo && x.codigo == codigo);

                return result;
            }
            catch (Exception ex)
            {
                this.log.PublishException(ex);
                throw;
            }
        }

        public bool GetIdProvincias(string codigo,int id)
        {
            try
            {
                bool result = this.db.Provincias.Any(x => x.es_activo && x.codigo == codigo && x.id != id);
               
                return result;
            }
            catch (Exception ex)
            {
                this.log.PublishException(ex);
                throw;
            }
        }


        public IEnumerable<ListCapitanias> GetAllCapitanias()
        {
            try
            {
                IEnumerable<ListCapitanias> result = (from a1 in this.db.Capitanias.ToList().Where(x => x.es_activo)
                                                      select new ListCapitanias
                                                      {
                                                          Id = a1.Id,
                                                          nombre = a1.nombre,
                                                          activado = a1.es_activo ? "Si" : "No"
                                                      });
                return result;
            }
            catch (Exception ex)
            {
                this.log.PublishException(ex);
                throw;
            }
        }

        public IEnumerable<ListCapitanias> ListCapitanias()
        {
            try
            {
                IEnumerable<ListCapitanias> result = (from a1 in this.db.Capitanias.ToList()
                                                          select new ListCapitanias
                                                          {
                                                              Id = a1.Id,
                                                              nombre = a1.nombre,
                                                              es_activo = a1.es_activo,
                                                              activado = a1.es_activo ? "Si" : "No"
                                                          });
                return result;
            }
            catch (Exception ex)
            {
                this.log.PublishException(ex);
                throw;
            }
        }
        
        public bool ListCapitanias(string Nombre)
        {
            try
            {
                bool result = this.db.Capitanias.Any(x => x.es_activo && x.nombre == Nombre);
                return result;

            }
            catch (Exception ex)
            {
                this.log.PublishException(ex);
                throw;
            }
        }

        public bool ListCapitanias(int Id, string Nombre)
        {
            try
            {
                bool result = this.db.Capitanias.Any(x => x.es_activo && x.nombre == Nombre && x.Id != Id);
                return result;

            }
            catch (Exception ex)
            {
                this.log.PublishException(ex);
                throw;
            }
        }


        public IEnumerable<Islas> GetallIslas()
        {
            try
            {
                IEnumerable<Islas> result = (from a1 in this.db.Islas.ToList()
                                                       select new Islas
                                                       {
                                                           id = a1.id,
                                                           nombre = a1.nombre                                                           
                                                       });
                return result;

            }
            catch (Exception ex)
            {
                this.log.PublishException(ex);
                throw;
            }
        
        }


        public IEnumerable<Provincias> ListProvincias(string Nombre)
        {
            try
            {
                IEnumerable<Provincias> result = this.db.Provincias.Where(x => x.nombre == Nombre);

                return result;

            }
            catch (Exception ex)
            {
                this.log.PublishException(ex);
                throw;
            }
        }

        public bool ListProvincias(string nombre, int Id, bool estado)
        {
            try
            {
                bool CambioEstado = false;
                if (estado == false)
                {
                    foreach (Provincias provincias in this.db.Provincias.Where(x => x.id == Id && x.nombre == nombre).ToList())
                    {
                        if (provincias.es_activo)
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

        public IEnumerable<Provincias> ListProvincias(string nombre, int Id)
        {
            try
            {
                var result = this.db.Provincias.Where(x => x.id != Id && x.nombre == nombre).ToList();
                return result;

            }
            catch (Exception ex)
            {
                this.log.PublishException(ex);
                throw;
            }
        }

        public IEnumerable<ListadoProvincias> ListProvincias()
        {
            try
            {
                IEnumerable<ListadoProvincias> result = (from a1 in this.db.Provincias.ToList()
                                                         join b1 in this.db.Comunidades_Autonomas on a1.ID_ComAut equals b1.Id
                                                         join c1 in this.db.Paises on b1.ID_Pais equals c1.Id
                                                         select new ListadoProvincias
                                                         {
                                                             Id = a1.id,
                                                             codigo = a1.codigo,
                                                             nombre = a1.nombre,
                                                             ID_ComAut = b1.Id,
                                                             comunidad = b1.Nombre,
                                                             pais = c1.nombre,
                                                             Id_pais = c1.Id.ToString(),
                                                             Activo = a1.es_activo,
                                                             activado = a1.es_activo ? "Si" : "No"
                                                         });
                return result;

            }
            catch (Exception ex)
            {
                this.log.PublishException(ex);
                throw;
            }
        }

        public bool ListCapitanias(string Nombre, int Id)
        {
            try
            {
                bool result = this.db.Capitanias.Any(x => x.es_activo && x.nombre == Nombre && x.Id != Id);
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
