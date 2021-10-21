using System.Collections.Generic;
using System;
using System.Linq;
using SecurePort.Entities;
using SecurePort.Entities.Models;


namespace SecurePort.Services.Interfaces
{
    using SecurePort.Entities.Enums;

    public class ServiciosComites : IServiciosComites 
    {
       
        #region Atributos
            private readonly ILogger log;
            protected readonly SecurePortContext db = new SecurePortContext();
        #endregion

        #region Métodos públicos

        public ServiciosComites(ILogger log)
        {
            this.log = log;
        }


        public IEnumerable<ComitesViewModel> GetAllComites()
        {
            try
            {

                IEnumerable<ComitesViewModel> result = (from a1 in db.Comites.ToList()
                                                        join b2 in this.db.Organismos.ToList() on a1.Id_Organismo equals b2.Id into OrganismoTemp
                                                        from or in OrganismoTemp.DefaultIfEmpty()
                                                        join c3 in this.db.Puertos.ToList() on a1.Id_Puerto equals c3.Id into PuertoTemp
                                                        from pu in PuertoTemp.DefaultIfEmpty()
                                                        select new ComitesViewModel
                                                        {
                                                            id = a1.id,
                                                            Nombre_Organismo = (or == null) ? "" : or.Nombre,
                                                            Nombre_Puerto = (pu == null) ? "" : pu.Nombre,
                                                            Nivel = a1.Nivel,
                                                            NombreNivel = a1.Nivel == 1 ? NivelCOM.CMT.ToDescription() : NivelCOM.SCMT.ToDescription(),
                                                            Id_Puerto = a1.Id_Puerto,
                                                            Id_Organismo = a1.Id_Organismo,
                                                            Convocado = a1.Convocado,
                                                            Observaciones = a1.Observaciones,
                                                            activoOrg = (or == null) ? false : or.es_activo,
                                                            activoPue = (pu == null) ? false : pu.es_activo,
                                                        }).OrderBy(x => x.Nombre_Organismo);
                return result;
            }
            catch (Exception ex)
            {
                this.log.PublishException(ex);
                throw;
            }
        }
        public IEnumerable<ComitesViewModel> GetAllComites(List<DependenciasUsuario> dependencias)
        {
            try
            {

                IEnumerable<ComitesViewModel> resultado = new List<ComitesViewModel>();

                var dependenciascomite = dependencias.Where(x => x.categoria == (int)EnumCategoria.Puerto).ToList();

                if (dependenciascomite.Any())
                {
                    resultado = (from a1 in db.Comites.ToList()
                                 join b2 in this.db.Organismos.ToList() on a1.Id_Organismo equals b2.Id into OrganismoTemp
                                 from or in OrganismoTemp.DefaultIfEmpty()
                                 join c3 in this.db.Puertos.ToList() on a1.Id_Puerto equals c3.Id into PuertoTemp
                                 from pu in PuertoTemp.DefaultIfEmpty()
                                 join c4 in dependenciascomite on pu.Id equals c4.id_Dependencia
                                 select new ComitesViewModel
                                       {
                                           id = a1.id,
                                           Nombre_Organismo = (or == null) ? "" : or.Nombre,
                                           Nombre_Puerto = (pu == null) ? "" : pu.Nombre,
                                           Nivel = a1.Nivel,
                                           NombreNivel = a1.Nivel == 1 ? NivelCOM.CMT.ToDescription() : NivelCOM.SCMT.ToDescription(),
                                           Id_Puerto = a1.Id_Puerto,
                                           Id_Organismo = a1.Id_Organismo,
                                           Convocado = a1.Convocado,
                                           Observaciones = a1.Observaciones
                                       }).OrderBy(x => x.Nombre_Organismo);

                }
                
                return resultado.GroupBy(c => c.id).Select(grp => grp.First()).ToList();
            }
            catch (Exception ex)
            {
                this.log.PublishException(ex);
                throw;
            }
        }

        public IEnumerable<Comites> ListComites()
        {
            try
            {
                var ListaComites = db.Comites.ToList();
                return ListaComites;
            }
            catch (Exception ex)
            {
                this.log.PublishException(ex);
                throw;
            }
        }

        public IEnumerable<Comites> ListComites(string nombre, int Id)
        {
                throw new NotImplementedException();
        }

        public bool ListComites(int Organismo, int Puerto, DateTime fecha)
        {
                 try
            {
                bool CambioEstado = false;
                     IEnumerable<Comites> result = this.db.Comites.Where(x => x.Id_Organismo == Organismo && x.Id_Puerto == Puerto  && x.Convocado == fecha).ToList();
                    if (result.Count() != 0)
                        CambioEstado = true;
               
                return CambioEstado;

            }
            catch (Exception ex)
            {
                this.log.PublishException(ex);
                throw;
            }
        }

        public bool ListComites(int Organismo, int Puerto, int ID, DateTime fecha)
        {
            try
            {
                bool CambioEstado = false;
                IEnumerable<Comites> result = this.db.Comites.Where(x => x.Id_Organismo == Organismo && x.Id_Puerto == Puerto  && x.Convocado == fecha && x.id != ID).ToList();
                if (result.Count() != 0)
                    CambioEstado = true;

                return CambioEstado;

            }
            catch (Exception ex)
            {
                this.log.PublishException(ex);
                throw;
            }
        }

        public bool ListComites(string nombre, int Id, bool estado)
        {
                throw new NotImplementedException();
        }

      

        #endregion
    }
}
