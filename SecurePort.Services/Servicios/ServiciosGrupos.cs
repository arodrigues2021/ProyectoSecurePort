
namespace SecurePort.Services.Interfaces
{

    #region using

    using System.Data.Entity.Core;

    using SecurePort.Entities;
    using SecurePort.Entities.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using SecurePort.Services.Repository;

    #endregion

    public class ServiciosGrupos : IServiciosGrupos
    {

        #region Atributos
        private readonly ILogger log;
        private readonly PerfilesRepository repoPerfiles;
        private readonly GruposRepository repoGrupos;
        private readonly PerfilesGruposRepository repoPerfilesGrupos;
        protected readonly SecurePortContext db = new SecurePortContext();
        #endregion

        #region Métodos públicos
        public ServiciosGrupos(ILogger log, PerfilesRepository repoPerfiles, GruposRepository repoGrupos, PerfilesGruposRepository repoPerfilesGrupos)
        {
            this.log = log;
            this.repoPerfiles = repoPerfiles;
            this.repoGrupos = repoGrupos;
            this.repoPerfilesGrupos = repoPerfilesGrupos;
        }


        public IEnumerable<GruposPerfiles> GetAllPerfiles()
        {
            try
            {

                IEnumerable<GruposPerfiles> result = (from t1 in repoGrupos.GetAll()
                                                          join
                                                               t2 in repoPerfilesGrupos.GetAll() on
                                                               t1.Id equals t2.Id_grupo 
                                                          join 
                                                               t3 in repoPerfiles.GetAll() on
                                                               t2.Id_perfil equals  t3.Id
                                                          select new GruposPerfiles
                                                          {
                                                              Id = t1.Id,
                                                              Nombre = t1.Nombre,
                                                              NombrePerfil = t3.Nombre
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

        public IEnumerable<Grupos> GetAllGrupos()
        {
            try
            {
                IEnumerable<Grupos> result = this.db.Grupos.ToList();
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


