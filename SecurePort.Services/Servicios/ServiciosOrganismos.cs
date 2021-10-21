namespace SecurePort.Services.Interfaces
{
    #region Using
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Cryptography;

    using SecurePort.Entities;
    using SecurePort.Entities.Enums;
    using SecurePort.Entities.Models;
    #endregion

    public class ServiciosOrganismos : IServiciosOrganismo
    {
        #region Atributos

        private readonly ILogger log;

        protected readonly SecurePortContext db = new SecurePortContext();

        #endregion

        #region Métodos públicos

        public ServiciosOrganismos(ILogger log)
        {
            this.log = log;
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

        public IEnumerable<ListadoOrganismo> GetAllOrganismos()
        {
            try
            {

                //Organismo+Contactos+Responsable
                IEnumerable<ListadoOrganismo> result = (from o in 
                                                            (from a1 in this.db.Organismos.ToList()
                                                              join e2 in GetTipo() on a1.Tipo equals e2.Id
                                                              join b2 in this.db.Provincias.ToList() on a1.ID_Provincia equals b2.id
                                                              join f5 in this.db.Comunidades_Autonomas.ToList() on b2.ID_ComAut equals f5.Id
                                                              join c3 in this.db.Ciudades.ToList() on a1.ID_Ciudad equals c3.id
                                                              select new ListadoOrganismo
                                                              {
                                                                  id        = a1.Id,
                                                                  Organismo = a1.Nombre,
                                                                  Tipo      = e2.Name,
                                                                  Direccion = a1.Direccion,
                                                                  Ciudad    = c3.nombre,
                                                                  CodigoPostal = a1.Cod_Postal.ToString(),
                                                                  Provincia    = b2.nombre,
                                                                  comunidad    = f5.Nombre,
                                                                  Activo       = a1.es_activo,
                                                                  activado     = a1.es_activo ? "Si" : "No"
                                                              }).OrderBy(x => x.Organismo)
                                                                  join t1 in this.db.Contactos_Organismo.Where(x => x.Es_Responsable && x.es_activo == true).ToList()
                                                                       on o.id equals (int?)t1.Id_Organismo into temp
                                                                        from be in temp.DefaultIfEmpty()
                                                                        select new ListadoOrganismo
                                                                        {
                                                                            id = o.id,
                                                                            Organismo = o.Organismo,
                                                                            Tipo      = o.Tipo,
                                                                            Direccion = o.Direccion,
                                                                            Ciudad    = o.Ciudad,
                                                                            CodigoPostal = o.CodigoPostal,
                                                                            Provincia = o.Provincia,
                                                                            comunidad = o.comunidad,
                                                                            Activo    = o.Activo,
                                                                            activado  = o.activado,
                                                                            responsable = (be == null) ? "" : be.Nombre
                                                                        }).OrderBy(x => x.Organismo);
             return result;

            }
            catch (Exception ex)
            {
                this.log.PublishException(ex);
                throw;
            }
        }

        public IEnumerable<ListadoOrganismo> GetAllOrganismos(List<DependenciasUsuario> dependencias, int? categoria)
        {
            try
            {
                
                IEnumerable<ListadoOrganismo> result = new List<ListadoOrganismo>();

                if (categoria == (int)EnumCategoria.Puerto)
                {
                    var dependenciasPuerto = dependencias.Where(x => x.categoria == (int)EnumCategoria.Puerto).ToList();

                    if (dependenciasPuerto.Any())
                    {
                            //Organismo+Contactos+Responsable
                            result = (from o in
                                          (from a1 in this.db.Organismos.ToList()
                                           join z1 in this.db.Puertos.ToList() on a1.Id equals z1.id_Organismo
                                           join x1 in dependenciasPuerto.ToList() on z1.Id equals x1.id_Dependencia
                                           join e2 in GetTipo() on a1.Tipo equals e2.Id
                                           join b2 in this.db.Provincias.ToList() on a1.ID_Provincia equals b2.id
                                           join f5 in this.db.Comunidades_Autonomas.ToList() on b2.ID_ComAut equals f5.Id
                                           join c3 in this.db.Ciudades.ToList() on a1.ID_Ciudad equals c3.id
                                           select new ListadoOrganismo
                                           {
                                               id = a1.Id,
                                               Organismo = a1.Nombre,
                                               Tipo = e2.Name,
                                               Direccion = a1.Direccion,
                                               Ciudad = c3.nombre,
                                               CodigoPostal = a1.Cod_Postal.ToString(),
                                               Provincia = b2.nombre,
                                               comunidad = f5.Nombre,
                                               Activo = a1.es_activo,
                                               activado = a1.es_activo ? "Si" : "No"
                                           }).OrderBy(x => x.Organismo)
                                      join t1 in this.db.Contactos_Organismo.Where(x => x.Es_Responsable && x.es_activo == true).ToList()
                                            on o.id equals (int?)t1.Id_Organismo into temp
                                      from be in temp.DefaultIfEmpty()
                                      select new ListadoOrganismo
                                      {
                                          id = o.id,
                                          Organismo = o.Organismo,
                                          Tipo = o.Tipo,
                                          Direccion = o.Direccion,
                                          Ciudad = o.Ciudad,
                                          CodigoPostal = o.CodigoPostal,
                                          Provincia = o.Provincia,
                                          comunidad = o.comunidad,
                                          Activo = o.Activo,
                                          activado = o.activado,
                                          responsable = (be == null) ? "" : be.Nombre
                                      }).OrderBy(x => x.Organismo);

                        
                    }
                }
                else if (categoria == (int)EnumCategoria.OrganismoGestionPortuaria)
                {

                    var dependenciasOrganismo = dependencias.Where(x => x.categoria == (int)EnumCategoria.OrganismoGestionPortuaria).ToList();

                    if (dependenciasOrganismo.Any())
                    {
                        //Organismo+Contactos+Responsable
                        result = (from o in
                                      (from a1 in this.db.Organismos.ToList()
                                       join z1 in this.db.Puertos.ToList() on a1.Id equals z1.id_Organismo
                                       join x1 in dependenciasOrganismo.ToList() on a1.Id equals x1.id_Dependencia
                                       join e2 in GetTipo() on a1.Tipo equals e2.Id
                                       join b2 in this.db.Provincias.ToList() on a1.ID_Provincia equals b2.id
                                       join f5 in this.db.Comunidades_Autonomas.ToList() on b2.ID_ComAut equals f5.Id
                                       join c3 in this.db.Ciudades.ToList() on a1.ID_Ciudad equals c3.id
                                       select new ListadoOrganismo
                                       {
                                           id = a1.Id,
                                           Organismo = a1.Nombre,
                                           Tipo = e2.Name,
                                           Direccion = a1.Direccion,
                                           Ciudad = c3.nombre,
                                           CodigoPostal = a1.Cod_Postal.ToString(),
                                           Provincia = b2.nombre,
                                           comunidad = f5.Nombre,
                                           Activo = a1.es_activo,
                                           activado = a1.es_activo ? "Si" : "No"
                                       }).OrderBy(x => x.Organismo)
                                  join t1 in this.db.Contactos_Organismo.Where(x => x.Es_Responsable && x.es_activo == true).ToList()
                                        on o.id equals (int?)t1.Id_Organismo into temp
                                  from be in temp.DefaultIfEmpty()
                                  select new ListadoOrganismo
                                  {
                                      id = o.id,
                                      Organismo = o.Organismo,
                                      Tipo = o.Tipo,
                                      Direccion = o.Direccion,
                                      Ciudad = o.Ciudad,
                                      CodigoPostal = o.CodigoPostal,
                                      Provincia = o.Provincia,
                                      comunidad = o.comunidad,
                                      Activo = o.Activo,
                                      activado = o.activado,
                                      responsable = (be == null) ? "" : be.Nombre
                                  }).OrderBy(x => x.Organismo);

                    }
                }
                else if (categoria == (int)EnumCategoria.Instalacion)
                {

                    var dependenciasInstala = dependencias.Where(x => x.categoria == (int)EnumCategoria.Instalacion).ToList();

                    if (dependenciasInstala.Any())
                    {
                        //Organismo+Contactos+Responsable
                        result = (from o in
                                      (from a1 in this.db.Organismos.ToList()
                                       join z1 in this.db.Puertos.ToList() on a1.Id equals z1.id_Organismo
                                       join y1 in this.db.MOV_Instalaciones.ToList() on z1.Id equals y1.id_Puerto
                                       join x1 in dependenciasInstala.ToList() on y1.Id equals x1.id_Dependencia
                                       join e2 in GetTipo() on a1.Tipo equals e2.Id
                                       join b2 in this.db.Provincias.ToList() on a1.ID_Provincia equals b2.id
                                       join f5 in this.db.Comunidades_Autonomas.ToList() on b2.ID_ComAut equals f5.Id
                                       join c3 in this.db.Ciudades.ToList() on a1.ID_Ciudad equals c3.id
                                       select new ListadoOrganismo
                                       {
                                           id = a1.Id,
                                           Organismo = a1.Nombre,
                                           Tipo = e2.Name,
                                           Direccion = a1.Direccion,
                                           Ciudad = c3.nombre,
                                           CodigoPostal = a1.Cod_Postal.ToString(),
                                           Provincia = b2.nombre,
                                           comunidad = f5.Nombre,
                                           Activo = a1.es_activo,
                                           activado = a1.es_activo ? "Si" : "No"
                                       }).OrderBy(x => x.Organismo)
                                  join t1 in this.db.Contactos_Organismo.Where(x => x.Es_Responsable && x.es_activo == true).ToList()
                                        on o.id equals (int?)t1.Id_Organismo into temp
                                  from be in temp.DefaultIfEmpty()
                                  select new ListadoOrganismo
                                  {
                                      id = o.id,
                                      Organismo = o.Organismo,
                                      Tipo = o.Tipo,
                                      Direccion = o.Direccion,
                                      Ciudad = o.Ciudad,
                                      CodigoPostal = o.CodigoPostal,
                                      Provincia = o.Provincia,
                                      comunidad = o.comunidad,
                                      Activo = o.Activo,
                                      activado = o.activado,
                                      responsable = (be == null) ? "" : be.Nombre
                                  }).OrderBy(x => x.Organismo);

                    }
                }
                
                return result.GroupBy(c => c.id).Select(grp => grp.First()).ToList();

            }
            catch (Exception ex)
            {
                this.log.PublishException(ex);
                throw;
            }
        }

        public IEnumerable<ContactosViewModel> GetAllContactos_Organismo(int? id)
        {
            try
            {
                IEnumerable<ContactosViewModel> result = (from a in this.db.Contactos_Organismo.Where(x => x.Id_Organismo == id).ToList()
                                                          select new ContactosViewModel
                                                           {
                                                               Id = a.Id,
                                                               Id_Organismo = a.Id_Organismo,
                                                               Nombre = a.Nombre,
                                                               Es_Responsable = a.Es_Responsable,
                                                               Telefono = a.Telefono,
                                                               Fax = a.Fax,
                                                               Email = a.Email,
                                                               Observaciones = a.Observaciones,
                                                               Cargo = a.Cargo,
                                                               es_activo = a.es_activo,
                                                               activado = a.es_activo ? "Si": "No"

                                                           });
                return result;
            }
            catch (Exception ex)
            {
                this.log.PublishException(ex);
                throw;
            }
        }

        public bool NombreOrganismo(string Nombre)
        {
            try
            {
                bool result = this.db.Organismos.Any(x => x.es_activo && x.Nombre == Nombre);
                return result;

            }
            catch (Exception ex)
            {
                this.log.PublishException(ex);
                throw;
            }
        }

        public bool NombreOrganismo(string Nombre, int? ID)
        {
            try
            {
                bool result = this.db.Organismos.Any(x => x.es_activo && x.Nombre == Nombre && x.Id != ID);
                return result;

            }
            catch (Exception ex)
            {
                this.log.PublishException(ex);
                throw;
            }
        }

        public IEnumerable<OrganismosViewModel> ListOrganismos()
        {
            try
            {
                IEnumerable<OrganismosViewModel> result =
                   (from t1 in this.db.Organismos.ToList().Where(x=> x.es_activo)
                    orderby t1.Id
                    select new OrganismosViewModel
                    {
                        Id = t1.Id,
                        Nombre = t1.Nombre
                    }).OrderBy(x => x.Id).ThenBy(l => l.Nombre);

                return result;
            }
            catch (Exception ex)
            {
                this.log.PublishException(ex);
                throw;
            }

        }
        
        public IEnumerable<Organismos> ListOrganismo(string nombre)
        {
            try
            {

                var result = this.db.Organismos.Where(x => x.Nombre == nombre).ToList();
                return result;

            }
            catch (Exception ex)
            {
                this.log.PublishException(ex);
                throw;
            }
        }

        public IEnumerable<Organismos> ListOrganismo(string nombre, int Id)
        {
            try
            {
                var result = this.db.Organismos.Where(x => x.Id != Id && x.Nombre == nombre).ToList();
                return result;

            }
            catch (Exception ex)
            {
                this.log.PublishException(ex);
                throw;
            }
        }

        public bool ListOrganismo(string nombre, int Id, bool estado)
        {
            try
            {
                bool CambioEstado = false;
                if (estado == false)
                {
                    foreach (Organismos organismo in this.db.Organismos.Where(x => x.Id == Id && x.Nombre == nombre).ToList())
                    {
                        if (organismo.es_activo)
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

        public IEnumerable<Comunidades_Autonomas> ListComunidades(int? Id)
        {
            try
            {
                IEnumerable<Provincias> result =this.db.Provincias.Where(x => x.id == Id);

                IEnumerable<Comunidades_Autonomas> resultado = null;

                foreach (Provincias prov in result)
                {
                    resultado = this.db.Comunidades_Autonomas.Where(x => x.Id ==prov.ID_ComAut );
                }

                return resultado;
            }
            catch (Exception ex)
            {
                this.log.PublishException(ex);
                throw;
            }
        }

        public IEnumerable<OrganismosViewModel> ListOrganismos(List<DependenciasUsuario> dependencias)
        {
            try
            {
                IEnumerable<OrganismosViewModel> result = (from t1 in this.db.Organismos.ToList().Where(x => x.es_activo)
                                                           join t3 in this.db.Puertos.ToList() on  t1.Id equals  t3.id_Organismo
                                                           join t2 in dependencias.Where(x=> x.categoria==(int)EnumCategoria.Puerto) on t3.Id equals t2.id_Dependencia
                                                               orderby t1.Id
                                                                select new OrganismosViewModel
                                                                {
                                                                    Id = t1.Id,
                                                                    Nombre = t1.Nombre
                                                                }).OrderBy(x => x.Id).ThenBy(l => l.Nombre);

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
