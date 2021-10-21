namespace SecurePort.Services.Interfaces
{
    #region Using
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;

    using SecurePort.Entities;
    using SecurePort.Entities.Enums;
    using SecurePort.Entities.Models;
    #endregion

    public class ServiciosPuertos : IServiciosPuertos
    {
        #region Atributos

        private readonly ILogger log;

        protected readonly SecurePortContext db = new SecurePortContext();

        #endregion

        #region Métodos públicos

        public ServiciosPuertos(ILogger log)
        {
            this.log = log;
        }

        #region Puertos

        public IEnumerable<MOV_Instalaciones> GetPuertosInstalacion(int? id)
        {
            try
            {
                IEnumerable<MOV_Instalaciones> result = (from t1 in this.db.Puertos.Where(x => x.Id == id && x.es_activo == true).ToList()
                    join t2 in this.db.MOV_Instalaciones.ToList() on t1.Id equals t2.id_Puerto
                    orderby t1.Nombre
                    select new MOV_Instalaciones { Id = t2.Id, Nombre = t2.Nombre, Clasificacion = t2.Clasificacion, id_Puerto = id });

                return result;
            }
            catch (Exception ex)
            {
                this.log.PublishException(ex);
                throw;
            }
        }

        public IEnumerable<PuertosViewModel> ListPuertos()
        {
            try
            {
                IEnumerable<PuertosViewModel> result =(from t1 in this.db.Puertos.ToList().Where(x=> x.es_activo==true)
                                                         orderby t1.Id
                                                       select new PuertosViewModel
                                                       {
                                                            Id = t1.Id,
                                                            Nombre = t1.Nombre,
                                                            id_Organismo  = t1.id_Organismo                       
                                                       }).OrderBy(x => x.Id).ThenBy(l => l.Nombre);

                
                return result;
            }
            catch (Exception ex)
            {
                this.log.PublishException(ex);
                throw;
            }

        }

        public IEnumerable<PuertosViewModel> GetAllPuertos()
        {
            try
            {

                        IEnumerable<PuertosViewModel> result = new List<PuertosViewModel>();

                        result = (from a1 in db.Puertos.ToList()
                                  join b2 in this.db.Organismos.ToList() on a1.id_Organismo equals b2.Id
                                  join e2 in GetTipo() on b2.Tipo equals e2.Id
                                  join c3 in this.db.Ciudades.ToList() on a1.ID_Ciudad equals c3.id into CiudadTemp
                                  from bt in CiudadTemp.DefaultIfEmpty()
                                  join d4 in this.db.Provincias.ToList() on a1.ID_Provincia equals d4.id into ProvinciaTemp
                                  from pt in ProvinciaTemp.DefaultIfEmpty()
                                  join e5 in this.db.Capitanias.ToList() on a1.Id_CapMarit equals e5.Id into CapitaniaTemp
                                  from ct in CapitaniaTemp.DefaultIfEmpty()
                                  orderby a1.Nombre, a1.id_Organismo, a1.Responsable                          
                                    select new PuertosViewModel
                                    {
                                        Id            = a1.Id,
                                        Nombre        = a1.Nombre,
                                        id_Organismo  = a1.id_Organismo,
                                        Organismo     = b2.Nombre,
                                        Responsable   = a1.Responsable,
                                        Direccion     = a1.Direccion,
                                        ID_Ciudad     = (bt == null) ? 0 :bt.id,
                                        Ciudad        = (bt == null) ? "" : bt.nombre,
                                        Id_Isla       = (bt == null) ? 0 : bt.Id_isla,
                                        Cod_Postal    = a1.Cod_Postal,
                                        ID_Provincia  = (pt == null) ? 0 : pt.id, 
                                        Provincia     = (pt == null) ? "" : pt.nombre,
                                        activado      = a1.es_activo == true ? "Si" : "No",
                                        Observaciones = a1.Observaciones,
                                        Latitud       = a1.Latitud,
                                        Longitud      = a1.Longitud,
                                        Locode        = a1.Locode,
                                        Id_CapMarit   = (ct ==null) ? 0 : ct.Id, 
                                        Capitania     = (ct ==null) ? "": ct.nombre,
                                        es_activo     = a1.es_activo,
                                        tipo          = e2.Name,
                                        ID_ComAut     = (pt == null) ? 0 : pt.ID_ComAut 
                                    }).OrderBy(x => x.Id);

                result = (from j1 in result.ToList()
                          join f5 in this.db.Comunidades_Autonomas.ToList() on j1.ID_ComAut equals f5.Id into ComunidadesTemp
                          from p1 in ComunidadesTemp.DefaultIfEmpty()
                          orderby j1.Id      
                        select
                        new PuertosViewModel
                        {
                            Id            = j1.Id,
                            Nombre        = j1.Nombre,
                            id_Organismo  = j1.id_Organismo,
                            Organismo     = j1.Organismo,
                            Responsable   = j1.Responsable,
                            Direccion     = j1.Direccion,
                            ID_Ciudad     = j1.ID_Ciudad,
                            Ciudad        = j1.Ciudad,
                            Id_Isla       = j1.Id_Isla,
                            Isla          = TraerIsla(j1.ID_Ciudad),
                            Cod_Postal    = j1.Cod_Postal,
                            ID_Provincia  = j1.ID_Provincia,
                            Provincia     = j1.Provincia,
                            activado      = j1.activado,
                            Observaciones = j1.Observaciones,
                            Latitud       = j1.Latitud,
                            Longitud      = j1.Longitud,
                            Locode        = j1.Locode,
                            Id_CapMarit   = j1.Id_CapMarit,
                            Capitania     = j1.Capitania,
                            es_activo     = j1.es_activo,
                            tipo          = j1.tipo,
                            comunidad     = (p1 == null) ? "" : p1.Nombre
                        }).OrderBy(x => x.Id);

                return result.OrderBy(x => x.Nombre); 
            }
            catch (Exception ex)
            {
                this.log.PublishException(ex);
                throw;
            }
        
        }

        public IEnumerable<TipoOrganismo> GetTipo()
        {
            try
            {
                IEnumerable<TipoOrganismo> ListaTipoOrganismo =
                    (IEnumerable<TipoOrganismo>)
                        new TipoOrganismo[]
                        {
                            new TipoOrganismo { Id = 1, Name = TipoORG.OGA.ToDescription() }, 
                            new TipoOrganismo { Id = 2, Name = TipoORG.OIG.ToDescription() },
                            new TipoOrganismo { Id = 3, Name = TipoORG.OOT.ToDescription() }
                        };

                return ListaTipoOrganismo;
            }
            catch (Exception ex)
            {
                this.log.PublishException(ex);
                throw;
            }
        }

        public IEnumerable<PuertosViewModel> GetAllPuertos(List<DependenciasUsuario> dependencias,int? categoria)
        {
            try
            {

               
                IEnumerable<PuertosViewModel> resultado = new List<PuertosViewModel>();

                if (categoria == (int)EnumCategoria.Puerto)
                {

                    var dependenciasPuerto = dependencias.Where(x => x.categoria == (int)EnumCategoria.Puerto).ToList();

                    if (dependenciasPuerto.Any())
                    {
                        resultado = (from a1 in db.Puertos.ToList()
                                     join zz in dependenciasPuerto.ToList() on a1.Id equals zz.id_Dependencia
                                     join b2 in this.db.Organismos.ToList() on a1.id_Organismo equals b2.Id
                                     join e2 in GetTipo() on b2.Tipo equals e2.Id
                                     join c3 in this.db.Ciudades.ToList() on a1.ID_Ciudad equals c3.id into CiudadTemp
                                     from bt in CiudadTemp.DefaultIfEmpty()
                                     join d4 in this.db.Provincias.ToList() on a1.ID_Provincia equals d4.id into ProvinciaTemp
                                     from pt in ProvinciaTemp.DefaultIfEmpty()
                                     join f5 in this.db.Comunidades_Autonomas.ToList() on pt.ID_ComAut equals f5.Id into ComunidadesTemp
                                     from p1 in ComunidadesTemp.DefaultIfEmpty()
                                     join e5 in this.db.Capitanias.ToList() on a1.Id_CapMarit equals e5.Id into CapitaniaTemp
                                     from ct in CapitaniaTemp.DefaultIfEmpty()
                                     orderby a1.Nombre, a1.id_Organismo, a1.Responsable
                                     select new PuertosViewModel
                                     {
                                         Id            = a1.Id,
                                         Nombre        = a1.Nombre,
                                         id_Organismo  = a1.id_Organismo,
                                         Organismo     = b2.Nombre,
                                         Responsable   = a1.Responsable,
                                         Direccion     = a1.Direccion,
                                         ID_Ciudad     = (bt == null) ? 0 : bt.id,
                                         Ciudad        = (bt == null) ? "" : bt.nombre,
                                         Id_Isla       = (bt == null) ? 0 : bt.Id_isla,
                                         Cod_Postal    = a1.Cod_Postal,
                                         ID_Provincia  = (pt == null) ? 0 : pt.id,
                                         Provincia     = (pt == null) ? "" : pt.nombre,
                                         activado      = a1.es_activo == true ? "Si" : "No",
                                         Observaciones = a1.Observaciones,
                                         Latitud       = a1.Latitud,
                                         Longitud      = a1.Longitud,
                                         Locode        = a1.Locode,
                                         Id_CapMarit   = (ct == null) ? 0 : ct.Id,
                                         Capitania     = (ct == null) ? "" : ct.nombre,
                                         es_activo     = a1.es_activo,
                                         tipo          = e2.Name,
                                         comunidad     = (p1 == null) ? "" : p1.Nombre,
                                     }).OrderBy(x => x.Id);

                        resultado = (from j1 in resultado.ToList()
                                     select new PuertosViewModel
                                     {
                                         Id            = j1.Id,
                                         Nombre        = j1.Nombre,
                                         id_Organismo  = j1.id_Organismo,
                                         Organismo     = j1.Organismo,
                                         Responsable   = j1.Responsable,
                                         Direccion     = j1.Direccion,
                                         ID_Ciudad     = j1.ID_Ciudad,
                                         Ciudad        = j1.Ciudad,
                                         Id_Isla       = j1.Id_Isla,
                                         Isla = TraerIsla(j1.ID_Ciudad),
                                         Cod_Postal    = j1.Cod_Postal,
                                         ID_Provincia  = j1.ID_Provincia,
                                         Provincia     = j1.Provincia,
                                         activado      = j1.activado,
                                         Observaciones = j1.Observaciones,
                                         Latitud       = j1.Latitud,
                                         Longitud      = j1.Longitud,
                                         Locode        = j1.Locode,
                                         Id_CapMarit   = j1.Id_CapMarit,
                                         Capitania     = j1.Capitania,
                                         es_activo     = j1.es_activo,
                                         tipo          = j1.tipo,
                                         comunidad     = j1.comunidad
                                     }).OrderBy(x => x.Nombre).OrderBy(x=> x.Locode).OrderBy(x=> x.Organismo).OrderBy(x=> x.tipo).OrderBy(x=> x.Isla).OrderBy(x=> x.comunidad).OrderBy(x=> x.Capitania).OrderBy(x=> x.activado);
                      

                    }
                }
                else if (categoria == (int)EnumCategoria.Instalacion)
                {

                    var dependenciasInstala = dependencias.Where(x => x.categoria == (int)EnumCategoria.Instalacion).ToList();

                    if (dependenciasInstala.Any())
                    {
                        resultado = (from a1 in db.Puertos.ToList()
                                     join e1 in db.MOV_Instalaciones.ToList() on a1.Id equals e1.id_Puerto
                                     join zz in dependenciasInstala.ToList() on e1.Id equals zz.id_Dependencia
                                     join b2 in this.db.Organismos.ToList() on a1.id_Organismo equals b2.Id
                                     join e2 in GetTipo() on b2.Tipo equals e2.Id
                                     join c3 in this.db.Ciudades.ToList() on a1.ID_Ciudad equals c3.id into CiudadTemp
                                     from bt in CiudadTemp.DefaultIfEmpty()
                                     join d4 in this.db.Provincias.ToList() on a1.ID_Provincia equals d4.id into ProvinciaTemp
                                     from pt in ProvinciaTemp.DefaultIfEmpty()
                                     join f5 in this.db.Comunidades_Autonomas.ToList() on pt.ID_ComAut equals f5.Id into ComunidadesTemp
                                     from p1 in ComunidadesTemp.DefaultIfEmpty()
                                     join e5 in this.db.Capitanias.ToList() on a1.Id_CapMarit equals e5.Id into CapitaniaTemp
                                     from ct in CapitaniaTemp.DefaultIfEmpty()
                                     orderby a1.Nombre, a1.id_Organismo, a1.Responsable
                                     select new PuertosViewModel
                                     {
                                         Id             = a1.Id,
                                         Nombre         = a1.Nombre,
                                         id_Organismo   = a1.id_Organismo,
                                         Organismo      = b2.Nombre,
                                         Responsable    = a1.Responsable,
                                         Direccion      = a1.Direccion,
                                         ID_Ciudad      = (bt == null) ? 0 : bt.id,
                                         Ciudad         = (bt == null) ? "" : bt.nombre,
                                         Id_Isla        = (bt == null) ? 0 : bt.Id_isla,
                                         Cod_Postal     = a1.Cod_Postal,
                                         ID_Provincia   = (pt == null) ? 0 : pt.id,
                                         Provincia      = (pt == null) ? "" : pt.nombre,
                                         activado       = a1.es_activo == true ? "Si" : "No",
                                         Observaciones  = a1.Observaciones,
                                         Latitud        = a1.Latitud,
                                         Longitud       = a1.Longitud,
                                         Locode         = a1.Locode,
                                         Id_CapMarit    = (ct == null) ? 0 : ct.Id,
                                         Capitania      = (ct == null) ? "" : ct.nombre,
                                         es_activo      = a1.es_activo,
                                         tipo           = e2.Name,
                                         comunidad      = (p1 == null) ? "" : p1.Nombre,
                                     }).OrderBy(x => x.Id);

                        resultado = (from j1 in resultado.ToList()
                                     select new PuertosViewModel
                                     {
                                         Id             = j1.Id,
                                         Nombre         = j1.Nombre,
                                         id_Organismo   = j1.id_Organismo,
                                         Organismo      = j1.Organismo,
                                         Responsable    = j1.Responsable,
                                         Direccion      = j1.Direccion,
                                         ID_Ciudad      = j1.ID_Ciudad,
                                         Ciudad         = j1.Ciudad,
                                         Id_Isla        = j1.Id_Isla,
                                         Isla           = TraerIsla(j1.ID_Ciudad),
                                         Cod_Postal     = j1.Cod_Postal,
                                         ID_Provincia   = j1.ID_Provincia,
                                         Provincia      = j1.Provincia,
                                         activado       = j1.activado,
                                         Observaciones  = j1.Observaciones,
                                         Latitud        = j1.Latitud,
                                         Longitud       = j1.Longitud,
                                         Locode         = j1.Locode,
                                         Id_CapMarit    = j1.Id_CapMarit,
                                         Capitania      = j1.Capitania,
                                         es_activo      = j1.es_activo,
                                         tipo           = j1.tipo,
                                         comunidad      = j1.comunidad
                                     }).OrderBy(x => x.Nombre).OrderBy(x => x.Locode).OrderBy(x => x.Organismo).OrderBy(x => x.tipo).OrderBy(x => x.Isla).OrderBy(x => x.comunidad).OrderBy(x => x.Capitania).OrderBy(x => x.activado);

                    }
                }
                else if (categoria == (int)EnumCategoria.OrganismoGestionPortuaria)
                {

                    var dependenciasOrganismo= dependencias.Where(x => x.categoria == (int)EnumCategoria.OrganismoGestionPortuaria).ToList();

                    if (dependenciasOrganismo.Any())
                    {
                        resultado = (from a1 in db.Puertos.ToList()
                                     join e1 in db.MOV_Instalaciones.ToList() on a1.Id equals e1.id_Puerto
                                     join zz in dependenciasOrganismo.ToList() on a1.id_Organismo equals zz.id_Dependencia
                                     join b2 in this.db.Organismos.ToList() on a1.id_Organismo equals b2.Id
                                     join e2 in GetTipo() on b2.Tipo equals e2.Id
                                     join c3 in this.db.Ciudades.ToList() on a1.ID_Ciudad equals c3.id into CiudadTemp
                                     from bt in CiudadTemp.DefaultIfEmpty()
                                     join d4 in this.db.Provincias.ToList() on a1.ID_Provincia equals d4.id into ProvinciaTemp
                                     from pt in ProvinciaTemp.DefaultIfEmpty()
                                     join f5 in this.db.Comunidades_Autonomas.ToList() on pt.ID_ComAut equals f5.Id into ComunidadesTemp
                                     from p1 in ComunidadesTemp.DefaultIfEmpty()
                                     join e5 in this.db.Capitanias.ToList() on a1.Id_CapMarit equals e5.Id into CapitaniaTemp
                                     from ct in CapitaniaTemp.DefaultIfEmpty()
                                     orderby a1.Nombre, a1.id_Organismo, a1.Responsable
                                     select new PuertosViewModel
                                     {
                                         Id             = a1.Id,
                                         Nombre         = a1.Nombre,
                                         id_Organismo   = a1.id_Organismo,
                                         Organismo      = b2.Nombre,
                                         Responsable    = a1.Responsable,
                                         Direccion      = a1.Direccion,
                                         ID_Ciudad      = (bt == null) ? 0 : bt.id,
                                         Ciudad         = (bt == null) ? "" : bt.nombre,
                                         Id_Isla        = (bt == null) ? 0 : bt.Id_isla,
                                         Cod_Postal     = a1.Cod_Postal,
                                         ID_Provincia   = (pt == null) ? 0 : pt.id,
                                         Provincia      = (pt == null) ? "" : pt.nombre,
                                         activado       = a1.es_activo == true ? "Si" : "No",
                                         Observaciones  = a1.Observaciones,
                                         Latitud        = a1.Latitud,
                                         Longitud       = a1.Longitud,
                                         Locode         = a1.Locode,
                                         Id_CapMarit    = (ct == null) ? 0 : ct.Id,
                                         Capitania      = (ct == null) ? "" : ct.nombre,
                                         es_activo      = a1.es_activo,
                                         tipo           = e2.Name,
                                         comunidad      = (p1 == null) ? "" : p1.Nombre
                                     }).OrderBy(x => x.Id);

                        resultado = (from j1 in resultado.ToList()
                                     select new PuertosViewModel
                                     {
                                         Id             = j1.Id,
                                         Nombre         = j1.Nombre,
                                         id_Organismo   = j1.id_Organismo,
                                         Organismo      = j1.Organismo,
                                         Responsable    = j1.Responsable,
                                         Direccion      = j1.Direccion,
                                         ID_Ciudad      = j1.ID_Ciudad,
                                         Ciudad         = j1.Ciudad,
                                         Id_Isla        = j1.Id_Isla,
                                         Isla           = TraerIsla(j1.ID_Ciudad),
                                         Cod_Postal     = j1.Cod_Postal,
                                         ID_Provincia   = j1.ID_Provincia,
                                         Provincia      = j1.Provincia,
                                         activado       = j1.activado,
                                         Observaciones  = j1.Observaciones,
                                         Latitud        = j1.Latitud,
                                         Longitud       = j1.Longitud,
                                         Locode         = j1.Locode,
                                         Id_CapMarit    = j1.Id_CapMarit,
                                         Capitania      = j1.Capitania,
                                         es_activo      = j1.es_activo,
                                         tipo           = j1.tipo,
                                         comunidad      = j1.comunidad
                                     }).OrderBy(x => x.Nombre).OrderBy(x => x.Locode).OrderBy(x => x.Organismo).OrderBy(x => x.tipo).OrderBy(x => x.Isla).OrderBy(x => x.comunidad).OrderBy(x => x.Capitania).OrderBy(x => x.activado);

                    }
                }
                
                return resultado.GroupBy(c => c.Id).Select(grp => grp.First()).ToList();
                
            
            }
            catch (Exception ex)
            {
                this.log.PublishException(ex);
                throw;
            }

        }

        public IEnumerable<Puertos> ListPuertos(string nombre)
        {
            try
            {

                var result = this.db.Puertos.Where(x => x.Nombre == nombre).ToList();
                return result;

            }
            catch (Exception ex)
            {
                this.log.PublishException(ex);
                throw;
            }
        }

        public IEnumerable<Puertos> ListPuertos(string nombre, int Id)
        {
            try
            {
                var result = this.db.Puertos.Where(x => x.Id != Id && x.Nombre == nombre).ToList();
                return result;

            }
            catch (Exception ex)
            {
                this.log.PublishException(ex);
                throw;
            }
        }

        public bool ListBienes(string nombre, int Id, bool estado)
        {
            try
            {
                bool CambioEstado = false;
                if (estado == false)
                {
                    IEnumerable<Puertos> result = this.db.Puertos.Where(x => x.Id == Id && x.Nombre == nombre).ToList();
                    Puertos puerto = result.FirstOrDefault();
                    if (puerto.es_activo != estado)
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

        public string TraerIsla(int? id)
        {
            try
            {
                CiudadesViewModel ciudad = new CiudadesViewModel(); 
                if(id!= null){
                    IEnumerable<CiudadesViewModel> Result = (from t1 in this.db.Ciudades.Where(x => x.id == id)
                                        join b2 in this.db.Islas on t1.Id_isla equals b2.id into IslaTemp
                                        from pt in IslaTemp.DefaultIfEmpty()
                            select new CiudadesViewModel
                                                        {
                                                            isla = (pt == null) ? "" : pt.nombre
                                                        });
                    if(Result.Count() > 0)
                        ciudad = Result.FirstOrDefault();
                    if (ciudad == null)
                        return string.Empty;
                }
                return ciudad.isla == null ? string.Empty : ciudad.isla;
            
            }
            catch (Exception ex)
            {
                this.log.PublishException(ex);
                throw;
            }


        }

        #endregion

        #region Operadores

        public IEnumerable<OperadoresPuertoViewModel> GetAllOperadoresPuerto(int? id_Puerto)
        {
            try
            {
                IEnumerable<OperadoresPuertoViewModel> result = (from a1 in this.db.OperadoresPuerto.ToList().Where(t => t.Id_puerto == id_Puerto)
                                                                 join b2 in this.db.Puertos.ToList() on a1.Id_puerto equals b2.Id into PuertoTemp
                                                                 from puerto in PuertoTemp.DefaultIfEmpty()
                                                                 join c3 in this.db.Provincias.ToList() on a1.ID_Provincia equals c3.id into ProvinciaTemp
                                                                 from Pro in ProvinciaTemp.DefaultIfEmpty()
                                                                 join d4 in this.db.Ciudades.ToList() on a1.ID_Ciudad equals d4.id into CiudadTemp
                                                                 from Ciu in CiudadTemp.DefaultIfEmpty()
                                                           select new OperadoresPuertoViewModel
                                                           {
                                                               Id = a1.Id,
                                                               Id_Puerto = a1.Id_puerto,
                                                               Puerto = (puerto == null) ? "" : puerto.Nombre,
                                                               Nombre = a1.Nombre,
                                                               Es_Suplente = a1.Es_Suplente,
                                                               activado_Sup = a1.Es_Suplente == true ? "Si" : "No",
                                                               Telefono = a1.Telefono,
                                                               Fax = a1.Fax,
                                                               Email = a1.Email,
                                                               Id_usu_alta = a1.Id_usu_alta,
                                                               Fech_alta = a1.Fech_alta,
                                                               Es_Activo = a1.Es_Activo,
                                                               activado = a1.Es_Activo== true ? "Si": "No",
                                                               Fech_activo = a1.Fech_activo,
                                                               Observaciones = a1.Observaciones,
                                                               Direccion = a1.Direccion,
                                                               Cod_Postal = a1.Cod_Postal,
                                                               ID_Provincia = a1.ID_Provincia,
                                                               Provincia = (Pro == null) ? string.Empty: Pro.nombre,
                                                               ID_Ciudad = a1.ID_Ciudad,
                                                               Ciudad = (Ciu == null) ? string.Empty: Ciu.nombre
                                                           }).ToList().OrderBy(x => x.Nombre);
                return result;

            }
            catch (Exception ex)
            {
                this.log.PublishException(ex);
                throw;
            }
        }

        public IEnumerable<OperadoresPuertoViewModel> ListOperadoresPuerto(int? id_puerto)
        {
            try
            {
                IEnumerable<OperadoresPuertoViewModel> result = (from a1 in this.db.OperadoresPuerto.ToList().Where(t => t.Id_puerto == id_puerto)
                                                                 join b2 in this.db.Puertos.ToList() on a1.Id_puerto equals b2.Id into PuertoTemp
                                                                 from puerto in PuertoTemp.DefaultIfEmpty()
                                                                 join c3 in this.db.Provincias.ToList() on a1.ID_Provincia equals c3.id into ProvinciaTemp
                                                                 from Pro in ProvinciaTemp.DefaultIfEmpty()
                                                                 join d4 in this.db.Ciudades.ToList() on a1.ID_Ciudad equals d4.id into CiudadTemp
                                                                 from Ciu in CiudadTemp.DefaultIfEmpty()
                                                           select new OperadoresPuertoViewModel
                                                           {
                                                               Id = a1.Id,
                                                               Id_Puerto = a1.Id_puerto,
                                                               Puerto = (puerto == null) ? "" : puerto.Nombre,
                                                               Nombre = a1.Nombre,
                                                               Es_Suplente = a1.Es_Suplente,
                                                               activado_Sup = a1.Es_Suplente == true ? "Si" : "No",
                                                               Telefono = a1.Telefono,
                                                               Fax = a1.Fax,
                                                               Email = a1.Email,
                                                               Id_usu_alta = a1.Id_usu_alta,
                                                               Fech_alta = a1.Fech_alta,
                                                               Es_Activo = a1.Es_Activo,
                                                               activado = a1.Es_Activo == true ? "Si" : "No",
                                                               Fech_activo = a1.Fech_activo,
                                                               Observaciones = a1.Observaciones,
                                                                Direccion = a1.Direccion,
                                                               Cod_Postal = a1.Cod_Postal,
                                                               ID_Provincia = a1.ID_Provincia,
                                                               Provincia = (Pro == null) ? string.Empty: Pro.nombre,
                                                               ID_Ciudad = a1.ID_Ciudad,
                                                               Ciudad = (Ciu == null) ? string.Empty: Ciu.nombre
                                                           }).ToList().OrderBy(x => x.Nombre);
                return result;

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
