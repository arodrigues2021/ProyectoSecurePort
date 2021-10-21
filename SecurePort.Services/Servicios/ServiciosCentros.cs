using System;
using System.Collections.Generic;
using SecurePort.Entities;
using SecurePort.Entities.Models;
using System.Linq;

namespace SecurePort.Services.Interfaces
{
    using SecurePort.Entities.Enums;

    public class ServiciosCentros : IServiciosCentros
    {

        #region Atributos
           private readonly ILogger log;
          protected readonly SecurePortContext db = new SecurePortContext();
        #endregion

        #region Métodos públicos
        
        public ServiciosCentros(ILogger log)
        {
            this.log = log;
        }

        #region Centros

        public IEnumerable<Centros> GetAllCentros()
        {
            try
            {
                var ListaCentros = db.Centros.ToList();
                return ListaCentros;
            }
            catch (Exception ex)
            {
                this.log.PublishException(ex);
                throw;
            }
        }
      
        public IEnumerable<CentrosViewModel> ListAllCentros()
        {
            try
            {
                    IEnumerable<CentrosViewModel> resultado = new List<CentrosViewModel>();

                    resultado = (from a1 in db.Centros.ToList()
                                 join b2 in this.db.Organismos.ToList() on a1.Id_Organismo equals b2.Id
                                 join c3 in this.db.Puertos.ToList() on a1.Id_Puerto equals c3.Id into temp
                                 from be in temp.DefaultIfEmpty()
                                 join d4 in this.db.Ciudades.ToList() on a1.Id_Ciudad equals d4.id into CiudadTemp
                                 from ct in CiudadTemp.DefaultIfEmpty()
                                 join e5 in this.db.Provincias.ToList() on a1.Id_Provincia equals e5.id into ProvinciaTemp
                                 from pt in ProvinciaTemp.DefaultIfEmpty()
                                 orderby b2.Nombre,  a1.Centro_24H
                                 select new CentrosViewModel
                                 {
                                     id         = a1.id,
                                     Centro_24H = a1.Centro_24H,
                                     Id_Puerto  = a1.Id_Puerto,
                                     Puerto = (be == null) ? "" : be.Nombre,
                                     Id_Organismo = a1.Id_Organismo,
                                     Organismo    = b2.Nombre,
                                     OrganismoActivo = b2.es_activo,
                                     Id_Ciudad = a1.Id_Ciudad,
                                     Ciudad = (ct==null)? "": ct.nombre,
                                     Id_Provincia = a1.Id_Provincia,
                                     Provincia = (pt == null) ? "": pt.nombre,
                                     Cod_Postal = a1.Cod_Postal,
                                     Direccion = a1.Direccion
                                 });


                    resultado = (from b1 in resultado.ToList()
                                 select new CentrosViewModel
                                 {
                                     id           = b1.id,
                                     Centro_24H   = b1.Centro_24H,
                                     Id_Puerto    = b1.Id_Puerto,
                                     Puerto       = b1.Puerto,
                                     Id_Organismo = b1.Id_Organismo,
                                     Organismo    = b1.Organismo,
                                     OrganismoActivo = b1.OrganismoActivo,
                                     operadorPuerto = new List<OperadoresPuertoViewModel>(MostrarOperador(b1.Id_Organismo, b1.Id_Puerto)),
                                     Id_Ciudad    = b1.Id_Ciudad,
                                     Ciudad       = b1.Ciudad,
                                     Id_Provincia = b1.Id_Provincia,
                                     Provincia    = b1.Provincia,
                                     Cod_Postal   = b1.Cod_Postal,
                                     Direccion    = b1.Direccion

                                 });


                    resultado = (from t1 in resultado.ToList()
                                 select new CentrosViewModel
                                 {
                                     id           = t1.id,
                                     Centro_24H   = t1.Centro_24H,
                                     Id_Puerto    = t1.Id_Puerto,
                                     Puerto       = t1.Puerto,
                                     Id_Organismo = t1.Id_Organismo,
                                     Organismo    = t1.Organismo,
                                     OrganismoActivo = t1.OrganismoActivo,
                                     Operador     = t1.operadorPuerto.FirstOrDefault() == null ? string.Empty : t1.operadorPuerto.FirstOrDefault().Nombre,
                                     Telefono     = t1.operadorPuerto.FirstOrDefault() == null ? string.Empty : t1.operadorPuerto.FirstOrDefault().Telefono,
                                     Correo       = t1.operadorPuerto.FirstOrDefault() == null ? string.Empty : t1.operadorPuerto.FirstOrDefault().Email,
                                     Id_Ciudad    = t1.Id_Ciudad,
                                     Ciudad       = t1.Ciudad,
                                     Id_Provincia = t1.Id_Provincia,
                                     Provincia    = t1.Provincia,
                                     Cod_Postal   = t1.Cod_Postal,
                                     Direccion    = t1.Direccion
                                 });

                    return resultado.GroupBy(c => c.id).Select(grp => grp.First()).ToList();

            }
            catch (Exception ex)
            {
                this.log.PublishException(ex);
                throw;
            }
        }

        public IEnumerable<CentrosViewModel> ListAllCentros(List<DependenciasUsuario> dependencias)
        {
            try
            {
                IEnumerable<CentrosViewModel> resultado = new List<CentrosViewModel>();

                var dependenciasOrganismo = dependencias.Where(x => x.categoria == (int)EnumCategoria.OrganismoGestionPortuaria).ToList();

                if (dependenciasOrganismo.Any())
                {

                         resultado = (from a1 in db.Centros.ToList()
                                              join b2 in this.db.Organismos.ToList() on a1.Id_Organismo equals b2.Id
                                              join c3 in this.db.Puertos.ToList() on a1.Id_Puerto equals c3.Id into temp
                                              from be in temp.DefaultIfEmpty()
                                              join d4 in this.db.Ciudades.ToList() on a1.Id_Ciudad equals d4.id into CiudadTemp
                                              from ct in CiudadTemp.DefaultIfEmpty()
                                              join e5 in this.db.Provincias.ToList() on a1.Id_Provincia equals e5.id into ProvinciaTemp
                                              from pt in ProvinciaTemp.DefaultIfEmpty()
                                              join zz in dependenciasOrganismo on b2.Id equals zz.id_Dependencia
                                              orderby b2.Nombre, a1.Centro_24H
                                       select new CentrosViewModel
                                       {
                                              id             = a1.id,
                                              Centro_24H     = a1.Centro_24H,
                                              Id_Puerto      = a1.Id_Puerto,
                                              Puerto         = (be == null) ? "" : be.Nombre,
                                              Id_Organismo   = a1.Id_Organismo,
                                              Organismo      = b2.Nombre,
                                              OrganismoActivo = b2.es_activo,     
                                              Id_Ciudad       = a1.Id_Ciudad,
                                              Ciudad          = (ct==null)? "": ct.nombre,
                                              Id_Provincia    = a1.Id_Provincia,
                                              Provincia       = (pt == null) ? "": pt.nombre,
                                              Cod_Postal      = a1.Cod_Postal,
                                              Direccion       = a1.Direccion     
                                        });

                    
                         resultado = (from b1 in resultado.ToList()
                                              select new CentrosViewModel
                                              {
                                                  id              = b1.id,
                                                  Centro_24H      = b1.Centro_24H,
                                                  Id_Puerto       = b1.Id_Puerto,
                                                  Puerto          = b1.Puerto,
                                                  Id_Organismo    = b1.Id_Organismo,
                                                  Organismo       = b1.Organismo,
                                                  OrganismoActivo = b1.OrganismoActivo,
                                                  operadorPuerto  = new List<OperadoresPuertoViewModel>(MostrarOperador(b1.Id_Organismo, b1.Id_Puerto)),
                                                  Id_Ciudad       = b1.Id_Ciudad,
                                                  Ciudad          = b1.Ciudad,
                                                  Id_Provincia    = b1.Id_Provincia,
                                                  Provincia       = b1.Provincia,
                                                  Cod_Postal      = b1.Cod_Postal,
                                                  Direccion       = b1.Direccion
                                              });


                         resultado = (from t1 in resultado.ToList()
                                           select new CentrosViewModel
                                           {
                                                id              = t1.id,
                                                Centro_24H      = t1.Centro_24H,
                                                Id_Puerto       = t1.Id_Puerto,
                                                Puerto          = t1.Puerto,
                                                Id_Organismo    = t1.Id_Organismo,
                                                Organismo       = t1.Organismo,
                                                OrganismoActivo = t1.OrganismoActivo,
                                                Operador        = t1.operadorPuerto.FirstOrDefault() == null ? string.Empty : t1.operadorPuerto.FirstOrDefault().Nombre,
                                                Telefono        = t1.operadorPuerto.FirstOrDefault() == null ? string.Empty : t1.operadorPuerto.FirstOrDefault().Telefono,
                                                Correo          = t1.operadorPuerto.FirstOrDefault() == null ? string.Empty : t1.operadorPuerto.FirstOrDefault().Email,
                                                Id_Ciudad       = t1.Id_Ciudad,
                                                Ciudad          = t1.Ciudad,
                                                Id_Provincia    = t1.Id_Provincia,
                                                Provincia       = t1.Provincia,
                                                Cod_Postal      = t1.Cod_Postal,
                                                Direccion       = t1.Direccion
                                          });
                }
                
                return resultado.GroupBy(c => c.id).Select(grp => grp.First()).ToList();
            }
            catch (Exception ex)
            {
                this.log.PublishException(ex);
                throw;
            }
        }

        public bool ListCentros(string centro)
        {
            try
            {
                bool result = this.db.Centros.Any(x => x.Centro_24H == centro);
                return result;

            }
            catch (Exception ex)
            {
                this.log.PublishException(ex);
                throw;
            }
        }

        public bool ListCentros(string centro, int Id)
        {
            try
            {
                bool result = this.db.Centros.Any(x => x.id != Id && x.Centro_24H == centro);
                return result;
            }
            catch (Exception ex)
            {
                this.log.PublishException(ex);
                throw;
            }
        }


        public Centros ListCentros(int Id)
        {
            try
            {
                Centros CentroConsultado = new Centros();
                  this.db.Centros.Where(x=> x.id == Id).ToList().ForEach(
                                  a1=>
                                  {
                                      CentroConsultado.id         = a1.id;
                                      CentroConsultado.Centro_24H = a1.Centro_24H;
                                      CentroConsultado.Id_Puerto = a1.Id_Puerto;                                    
                                      CentroConsultado.Id_Organismo = a1.Id_Organismo;
                                      CentroConsultado.Id_Ciudad = a1.Id_Ciudad;
                                      CentroConsultado.Id_Provincia = a1.Id_Provincia;
                                      CentroConsultado.Cod_Postal = a1.Cod_Postal;
                                      CentroConsultado.Direccion = a1.Direccion;
                                      
                                  });

                  return CentroConsultado;
            }
            catch (Exception ex)
            {
                this.log.PublishException(ex);
                throw;
            }
        }

        private IEnumerable<OperadoresPuertoViewModel> MostrarOperador(int? Organismo, int? Puerto)
        {
            try
            {
                IEnumerable<OperadoresPuertoViewModel> resultado = (from a1 in db.OperadoresPuerto.ToList().Where(x => x.Id_puerto == Puerto && x.Es_Suplente == false).OrderBy(x => x.Id)
                                                                    select new OperadoresPuertoViewModel
                                                           {
                                                               Nombre = a1.Nombre,
                                                               Telefono = a1.Telefono,
                                                               Email = a1.Email

                                                           });
                if (resultado.Count() == 0)
                {
                    var itemQuery = from t in db.Puertos
                                    where (t.id_Organismo == Organismo)
                                    select t.Id;

                    resultado = (from a1 in db.OperadoresPuerto.ToList()
                                 where a1.Es_Suplente == false && itemQuery.Contains(a1.Id_puerto)
                                 select new OperadoresPuertoViewModel
                                                            {
                                                                Nombre = a1.Nombre,
                                                                Telefono = a1.Telefono,
                                                                Email = a1.Email
                                                            });
                }

        


                return resultado;
            }
            catch (Exception ex)
            {
                this.log.PublishException(ex);
                throw;
            }


        }

        private IEnumerable<OperadoresPuertoViewModel> MostrarOperador(int Organismo, int? Puerto, bool supente)
        {
            try
            {
                IEnumerable<OperadoresPuertoViewModel> resultado = (from a1 in db.OperadoresPuerto.ToList().Where(x => x.Id_puerto == Puerto).OrderBy(x => x.Id)
                                                                    select new OperadoresPuertoViewModel
                                                                    {
                                                                        Nombre = a1.Nombre,
                                                                        Telefono = a1.Telefono,
                                                                        Fax= a1.Fax,
                                                                        Email = a1.Email,
                                                                        activado_Sup  = a1.Es_Suplente == true? "Si": "No",
                                                                        Es_Suplente = a1.Es_Suplente

                                                                    });
                if (resultado.Count() == 0 && Puerto == null)
                {
                    var itemQuery = from t in db.Puertos
                                    where (t.id_Organismo == Organismo)
                                    select t.Id;

                    resultado = (from a1 in db.OperadoresPuerto.ToList()
                                 where a1.Es_Suplente == false && itemQuery.Contains(a1.Id_puerto)
                                 select new OperadoresPuertoViewModel
                                 {
                                     Nombre = a1.Nombre,
                                     Telefono = a1.Telefono,
                                     Email = a1.Email
                                 });
                }




                return resultado;
            }
            catch (Exception ex)
            {
                this.log.PublishException(ex);
                throw;
            }


        }


        #endregion

        #region Adicionales

        public IEnumerable<Comunica_Centro> GetAdiocionalbyTipo(int TipoCanal, int ID)
        {
            try
            {


                IEnumerable<Comunica_Centro> result = (from a1 in db.Comunica_Centro.ToList().Where(x=> x.Id_Centro24h == ID && x.Tipo_Canal == TipoCanal)
                                                        select new Comunica_Centro
                                                        {
                                                            id = a1.id,
                                                            Id_Centro24h = a1.Id_Centro24h,
                                                            Tipo_Canal = a1.Tipo_Canal,
                                                            Dato = a1.Dato,
                                                            Nota = a1.Nota
                                                        });
                return result;
            }
            catch (Exception ex)
            {
                this.log.PublishException(ex);
                throw;
            }
        
        }

        public IEnumerable<Comunica_Centro> GetAdiocionalbyTipo( int ID)
        {
            try
            {


                IEnumerable<Comunica_Centro> result = (from a1 in db.Comunica_Centro.ToList().Where(x => x.Id_Centro24h == ID)
                                                       select new Comunica_Centro
                                                       {
                                                           id = a1.id,
                                                           Id_Centro24h = a1.Id_Centro24h,
                                                           Tipo_Canal = a1.Tipo_Canal,
                                                           Dato = a1.Dato,
                                                           Nota = a1.Nota
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

        #region Operadores
        public IEnumerable<OperadoresPuertoViewModel> GetAllOperadores(int IDCentro)
        {
            Centros CentroConsulta = ListCentros(IDCentro);
            IEnumerable<OperadoresPuertoViewModel> result = MostrarOperador(CentroConsulta.Id_Organismo, CentroConsulta.Id_Puerto, false);

            return result;
        
        }

        #endregion

     

        #endregion

    }
}
