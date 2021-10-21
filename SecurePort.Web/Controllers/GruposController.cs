namespace SecurePort.Web.Controllers
{
    #region Using

    using Kendo.Mvc.Extensions;
    using Kendo.Mvc.UI;
    using SecurePort.Entities.Enums;
    using SecurePort.Entities.Exceptions;
    using SecurePort.Entities.Models;
    using SecurePort.Entities.Models.Json;
    using SecurePort.Services.Interfaces;
    using SecurePort.Services.Repository;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.ModelConfiguration;
    using System.Data.Entity.Validation;
    using System.Linq;
    using System.Net;
    using System.Web.Mvc;

    using WebGrease.Css.Extensions;

    #endregion

    public class GruposController : BaseController
    {
        public GruposController(
                PerfilesPermisosRepository repoPerfilesPermisos,
                PermisosRepository repoPermisos,
                GruposRepository repoGrupos,
                PerfilesRepository repoPerfiles,
                PerfilesGruposRepository repoPerfilesGrupos,
                IServiciosGrupos serviciosGrupos,
                IServiciosUsuarios serviciosUsuarios,
                TrazasRepository repoTrazas,
                ILogger log)
                : base(repoPerfilesPermisos,
                       repoPermisos,
                       repoGrupos,
                       repoPerfiles,
                       repoPerfilesGrupos,
                       serviciosGrupos,
                       serviciosUsuarios,
                       repoTrazas,
                       log)
        {
        }

        // GET: /Grupos/
        public ActionResult Index()
        {
            return this.View();
        }

        #region Buscador
        

        /// <summary>
        ///  Action que se ejecuta desde la vista ListPerfiles.cshtml
        /// </summary>
        public ActionResult BuscadorPerfiles([DataSourceRequest]DataSourceRequest request)
        {
            try
            {
                

                IEnumerable<AltaPerfilesViewModel> result = from c in this.listaGruposAsignadosSession
                                                            select new AltaPerfilesViewModel
                                                   {
                                                       Id = (c.Id),
                                                       Nombre = c.Nombre,
                                                       alta = false,
                                                       asignar = false
                                                   };

                return this.Json(result.OrderBy(x=> x.Nombre).ToDataSourceResult(request));
            }
            catch (Exception ex)
            {
                this.log.PublishException(new SecureportException(ex));
                throw;
            }
        }


        /// <summary>
        ///  Action que se ejecuta desde la vista List.cshtml
        /// </summary>
        public ActionResult BuscadorDetalle([DataSourceRequest]DataSourceRequest request)
        {
            try
            {

                if (this.listaGruposAsignadosSession.Any())
                {
                    this.listaGruposAsignadosSession.ToList().ForEach(
                        p =>
                        {
                            this.listaGruposAsignadosSession.Remove(p);
                        });
                }

                IEnumerable<PerfilesBuscador> listaPerfiles = (from t1 in this.repoPerfilesGrupos.GetAll()
                                                                  .Where(x => x.Id_grupo == Convert.ToInt32(this.idGrupo))
                                                                     join t2 in this.repoPerfiles.GetAll().Where(x=> x.es_activo==true) on t1.Id_perfil equals t2.Id
                                                                        select new PerfilesBuscador { Id_perfil = t1.Id_perfil, Nombre = t2.Nombre,Description = t2.descripcion});

                listaPerfiles.ToList().ForEach(
                    t =>
                    {
                        this.listaGruposAsignadosSession.Add(new AltaPerfiles() { Id = t.Id_perfil, Nombre = t.Nombre, Description = t.Description, alta = false });

                    });
                
                
                IEnumerable<AltaPerfiles> result = from c in this.listaGruposAsignadosSession 
                                                     select new AltaPerfiles { Id=c.Id,
                                                                                Nombre = c.Nombre,
                                                                                Description = c.Description};

                return this.Json(result.ToDataSourceResult(request));
            }
            catch (Exception ex)
            {
                this.log.PublishException(new SecureportException(ex));
                throw;
            }
        }

        /// <summary>
        ///  Action que se ejecuta desde la vista List.cshtml
        /// </summary>
        public ActionResult Buscador([DataSourceRequest]DataSourceRequest request)
        {
            try
            {
                IEnumerable<Grupos> result = new List<Grupos>();
                if (this.UsuarioFrontalSession.Usuario.categoria == (int)EnumCategoria.Administrador
                 || this.UsuarioFrontalSession.Usuario.categoria == (int)EnumCategoria.PuertosdelEstado)
                {
                   result = this.repoGrupos.GetAll().OrderBy(d => d.Nombre);
                }
                else if (this.UsuarioFrontalSession.Usuario.categoria == (int)EnumCategoria.OrganismoGestionPortuaria)
                {
                    result = this.repoGrupos.GetAll().Where(x => (x.Id_publico == (int)EnumPublico.Organismo || x.Id_publico == (int)EnumPublico.Publico)).OrderBy(d => d.Nombre);
                }
                else
                    result = this.repoGrupos.GetAll().Where(x => x.Id_publico == (int)EnumPublico.Publico).OrderBy(d => d.Nombre);


                this.idGrupo = result.Count()>0? result.ToList().FirstOrDefault().Id: 0;

                IEnumerable<GruposViewModel> resultado = from c in result.OrderBy(z => z.Nombre)
                                                         select new GruposViewModel
                                                      {
                                                          Id = c.Id,
                                                          Nombre = c.Nombre,
                                                          descripcion = c.descripcion,
                                                          Id_publico = c.Id_publico,
                                                          es_activo = c.es_activo==true?"Si":"No"
                                                      };

                return this.Json(resultado.ToDataSourceResult(request));
            }
            catch (Exception ex)
            {
                this.log.PublishException(new SecureportException(ex));
                throw;
            }
        }

        /// <summary>
        ///  Action que se ejecuta desde la vista Seleccionar.cshtml
        /// </summary>
        public ActionResult SeleccionarPerfil([DataSourceRequest]DataSourceRequest request)
        {
            try
            {
                if (this.listaGruposSession.Any() == false && this.listaGruposAsignadosSession.Any() == false)
                {

                    this.repoPerfiles.GetAll().Where(x => x.es_activo == true).ToList().ForEach(
                        t =>
                        {
                            this.listaGruposSession.Add(new AltaPerfiles() { Id = t.Id, Nombre = t.Nombre, alta = true });
                        });


                    IEnumerable<PerfilesBuscador> listaPerfiles = (from t1 in this.repoPerfilesGrupos.GetAll()
                                                                     .Where(x => x.Id_grupo == Convert.ToInt32(this.idGrupo))
                                                                     join t2 in this.repoPerfiles.GetAll().Where(x=> x.es_activo==true) on t1.Id_perfil equals t2.Id
                                                                     select new PerfilesBuscador { Id_perfil = t1.Id_perfil, Nombre = t2.Nombre });

                    this.listaGruposSession.ToList().ForEach(
                        t => listaPerfiles.ToList().ForEach(
                            p =>
                            {
                                if (p.Id_perfil == t.Id)
                                {
                                    this.listaGruposSession.Remove(t);
                                }
                            }));

                    listaPerfiles.ToList().ForEach(
                        t =>
                        {
                            this.listaGruposAsignadosSession.Add(new AltaPerfiles() { Id = t.Id_perfil, Nombre = t.Nombre, alta = false });

                        });
                }

                IEnumerable<AltaPerfiles> result = from c in this.listaGruposSession.OrderBy(z=> z.Nombre) select new AltaPerfiles
                                                                                            {
                                                                                                Id =  (c.Id),
                                                                                                Nombre = c.Nombre,
                                                                                                alta=false,
                                                                                                asignar = false
                                                                                            };

                return this.Json(result.ToDataSourceResult(request));
            }
            catch (Exception ex)
            {
                this.log.PublishException(new SecureportException(ex));
                throw;
            }
        }

        #endregion

        #region CRUD

        /// <summary>
        ///  Borrar fisicamente el registro del grupo.
        /// </summary>
        [HttpPost]
        public ActionResult EliminarGrupo(int? id)
        {
            try
            {
                if (id == null)
                {
                    return this.Json(new { result = false, Message = "Id NULL" });
                }

                Grupos grupos = this.repoGrupos.Single(x => x.Id == id);

                if (grupos == null)
                {
                    return this.Json(new { result = false, Message = "El Grupo no existe." });
                }

                //Eliminamos el usuario por Store Procedure.
                if (this._serviciosUsuarios.RemoveGrupoById(id) == 1)
                {
                    this.trazas(22, (int)this.UsuarioFrontalSession.Usuario.id, "Se ha eliminado un registro en SPCON_Grupos con ID" + "(" + id.ToString() + ")");

                    return this.Json(new { result = false, Message = "Se ha eliminado el Grupo" });
                }

                return this.Json(new { result = false, Message = "Existen registros asociados al Grupo." });
            }
            catch (ModelValidationException ex)
            {
                return this.Json(new { result = false, Message = ex.Message });
            }
            catch (DbEntityValidationException ex)
            {
                this.log.PublishException(new SecureportExceptionDbEntity(ex));

                return this.Json(new { result = false, Message = ex.Message });
            }
            catch (Exception ex)
            {
                this.log.PublishException(new SecureportException(ex));

                return this.Json(new { result = false, Message = "Instrucción DELETE en conflicto con la restricción REFERENCE" });
            }

            return this.Json(new { result = true });
        }

        /// <summary>
        ///     Permite guardar los cambios efectuados aun grupo.
        /// </summary>
        [HttpPost]
        public ActionResult GuardarGrupos(GruposJson gruposJson)
        {
            try
            {
                this.listaGruposAsignadosSession.Where(x => x.alta)
                    .ForEach(
                        x =>
                            this.repoPerfilesGrupos.Create(
                                new PerfilesGrupo()
                                {
                                    Id_perfil = Convert.ToInt32(x.Id),
                                    Id_grupo = Convert.ToInt32(this.idGrupo),
                                    id_usu_alta = this.UsuarioFrontalSession.Usuario.id,
                                    fech_alta = DateTime.Now
                                }));

                this.listaGruposSession
                   .ToList()
                   .ForEach(
                       p =>
                           this.repoPerfilesGrupos.GetAll().Where(x => x.Id_grupo == Convert.ToInt32(this.idGrupo) && x.Id_perfil == p.Id).ToList().ForEach(
                               t =>
                               {
                                   if (t.Id_perfil==p.Id && p.alta == false)
                                   {
                                       this.repoPerfilesGrupos.Delete(t);
                                   }
                               }));

                
                this.repoGrupos.GetAll()
                    .Where(x => x.Id == this.idGrupo)
                    .ToList()
                    .ForEach(
                        x =>
                            this.repoGrupos.Update(
                                new Grupos()
                                {
                                    Id          = x.Id,
                                    Nombre      = gruposJson.Nombre.ToUpper(),
                                    descripcion = gruposJson.Description,
                                    fech_activo = x.fech_activo,
                                    es_activo   = gruposJson.Activo=="1",
                                    fech_alta   = x.fech_alta,
                                    id_usu_alta = x.id_usu_alta,
                                    Id_publico  = gruposJson.Id_publico
                                }));

                if (this.listaGruposAsignadosSession.Any())
                {
                    this.listaGruposAsignadosSession.ToList().ForEach(
                        p =>
                        {
                            this.listaGruposAsignadosSession.Remove(p);
                        });
                }


                if (this.listaGruposSession.Any())
                {
                    this.listaGruposSession.ToList().ForEach(
                        p =>
                        {
                            this.listaGruposSession.Remove(p);
                        });
                }

                IEnumerable<PerfilesBuscador> listaPerfiles = (from t1 in this.repoPerfilesGrupos.GetAll().Where(x => x.Id_grupo == Convert.ToInt32(this.idGrupo))
                                                               join t2 in this.repoPerfiles.GetAll().Where(x=> x.es_activo==true) on t1.Id_perfil equals t2.Id
                                                               select new PerfilesBuscador { Id_perfil = t1.Id_perfil, Nombre = t2.Nombre });
                

                listaPerfiles.ToList().ForEach(
                    t =>
                    {
                        this.listaGruposAsignadosSession.Add(new AltaPerfiles() { Id = t.Id_perfil, Nombre = t.Nombre, alta = false });

                    });

                this.trazas(20, (int)this.UsuarioFrontalSession.Usuario.id, "Se ha guardado un registro en SPCON_Grupos con ID" + "(" + this.idGrupo.ToString() + ")");
            }
            catch (ModelValidationException ex)
            {
                return this.Json(new { result = false, Message = ex.Message });
            }
            catch (DbEntityValidationException ex)
            {
                this.log.PublishException(new SecureportExceptionDbEntity(ex));

                return this.Json(new { result = false, Message = ex.Message });
            }
            catch (Exception ex)
            {
                this.log.PublishException(new SecureportException(ex));

                return this.Json(new { result = false, Message = ex.Message });
            }

            return this.Json(new { result = true, Message = Message.Alta.ToDescription() });
        }

        [HttpPost]
        public ActionResult RegistaGrupos(GruposJson gruposJson)
        {
            try
            {

                    Grupos grupos = new Grupos()
                    {
                        Nombre = gruposJson.Nombre.ToUpper(),
                        descripcion = gruposJson.Description,
                        es_activo = true,
                        fech_activo = DateTime.Now,
                        id_usu_alta = this.UsuarioFrontalSession.Usuario.id,
                        fech_alta = DateTime.Now,
                        Id_publico = gruposJson.Id_publico
                    };

                    this.repoGrupos.Create(grupos);

                    var id = this.db.Grupos.OrderByDescending(u => u.Id).FirstOrDefault().Id;

                    trazas(20, id, "Se ha guardado un registro en SPCON_Grupos con ID" + "(" + id.ToString() + ")");

                    return this.Json(new { result = true, Message = "Alta de Grupos exitosa." });
            }
            catch (DbUpdateException ex)
            {
                this.log.PublishException(new SecureportExceptionDbUpdate(ex));

                return this.Json(new { result = false, Message = ex.Message });
            }
            catch (ModelValidationException ex)
            {
                return this.Json(new { result = false, Message = ex.Message });
            }
            catch (DbEntityValidationException ex)
            {
                this.log.PublishException(new SecureportExceptionDbEntity(ex));

                return this.Json(new { result = false, Message = ex.Message });
            }
            catch (Exception ex)
            {
                this.log.PublishException(new SecureportException(ex));

                return this.Json(new { result = false, Message = ex.Message });
            }


        }


        /// <summary>
        ///     Permite hacer una alta de un grupo.
        /// </summary>
        [HttpPost]
        public ActionResult AltaGrupos(GruposJson gruposJson)
        {
            try
            {
                // No se permite grupos duplicados activos  
                Grupos grupo_ant = this.repoGrupos.Single(x => x.Nombre == gruposJson.Nombre.ToUpper() &&  x.es_activo == true);
                Grupos grupo_ant_id = this.repoGrupos.Single(x => x.Nombre == gruposJson.Nombre.ToUpper()&& x.es_activo == false ); 
                int id = this.UsuarioFrontalSession.Usuario.id;
                
                  if (grupo_ant == null && grupo_ant_id == null)
                  {
                        Grupos grupos = new Grupos()
                                        {
                                            Nombre = gruposJson.Nombre.ToUpper(),
                                            descripcion = gruposJson.Description,
                                            es_activo = true,
                                            fech_activo = DateTime.Now,
                                            id_usu_alta = this.UsuarioFrontalSession.Usuario.id,
                                            fech_alta = DateTime.Now,
                                            Id_publico = gruposJson.Id_publico
                                        };

                        this.repoGrupos.Create(grupos);

                        var Id = this.db.Grupos.OrderByDescending(u => u.Id).FirstOrDefault().Id;

                        trazas(20, id, "Se ha guardado un registro en SPCON_Grupos con ID" + "(" + Id.ToString() + ")");

                        return this.Json(new { result = true, Message = "Alta de Grupos exitosa." });
                  } 
                  else
                  {
                      if (grupo_ant_id != null && grupo_ant != null)
                      {
                          if (grupo_ant.es_activo == true)
                          {
                              return this.Json(new { result = true, Message = "Error al dar de alta un Grupo, ya existe otro grupo activo " + gruposJson.Description });
                          }
                      }
                      else if (grupo_ant_id != null)
                      {
                          return this.Json(new { result = false, Message = "Error al dar de alta un Grupo, ya existe otro grupo desactivo " + gruposJson.Description + "¿Desea continuar? " });

                      }
                      else
                      {
                          return this.Json(new { result = true, Message = "No se permite el alta de un Grupo que ya existe" });

                      }

                  }
                  
                  return this.Json(new { result = true, Message = "Datos guardados correctamente" });
             }
            catch (DbUpdateException ex)
            {
                this.log.PublishException(new SecureportExceptionDbUpdate(ex));

                return this.Json(new { result = false, Message = ex.Message });
            }
            catch (ModelValidationException ex)
            {
                return this.Json(new { result = false, Message = ex.Message });
            }
            catch (DbEntityValidationException ex)
            {
                this.log.PublishException(new SecureportExceptionDbEntity(ex));

                return this.Json(new { result = false, Message = ex.Message });
            }
            catch (Exception ex)
            {
                this.log.PublishException(new SecureportException(ex));

                return this.Json(new { result = false, Message = ex.Message });
            }

            
        }

        [HttpPost]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return this.Json(new { result = false, Message = "Id NULL" });
            }

            Grupos grupos = this.repoGrupos.Single(x => x.Id == id);

            if (grupos == null)
            {
                return this.Json(new { result = false, Message = "El Grupo no existe." });
            }

            this.idGrupo = id;

            return this.PartialView("~/Views/Grupos/PlantillaEdit.cshtml",grupos);
        }
        
        [HttpPost]
        public ActionResult BajaConfirmed(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                Grupos grupos = this.repoGrupos.Single(x => x.Id == id);

                if (grupos == null)
                {
                    return this.Json(new { result = false, Message = "El Grupo No existe." });
                }

                grupos.es_activo = false;

                grupos.fech_activo = DateTime.Now;

                this.repoGrupos.Update(grupos);

                this.trazas(21, (int)this.UsuarioFrontalSession.Usuario.id, "Se ha guardado un registro en SPCON_Grupos " + grupos.Nombre);
            }
            catch (ModelValidationException ex)
            {
                return this.Json(new { result = false, Message = ex.Message });
            }
            catch (DbEntityValidationException ex)
            {
                this.log.PublishException(new SecureportExceptionDbEntity(ex));

                return this.Json(new { result = false, Message = ex.Message });
            }
            catch (Exception ex)
            {
                this.log.PublishException(new SecureportException(ex));

                return this.Json(new { result = false, Message = "Instrucción DELETE en conflicto con la restricción REFERENCE" });
            }

            return this.Json(new { result = true });
        }

        /// <summary>
        /// Permite Asginar el perfil al grupo.
        /// </summary>
        [HttpPost]
        public ActionResult AgregarPerfil(int? id)
        {

            this.listaGruposSession.ToList().ForEach(
                p =>
                {
                    if (p.Id == id)
                    {
                        this.listaGruposSession.Remove(p);
                        this.listaGruposAsignadosSession.Add(new AltaPerfiles() { Id = p.Id, Nombre = p.Nombre,alta=true });
                    }
                });

            return this.PartialView("~/Views/Grupos/Seleccionar.cshtml");
        }

        /// <summary>
        /// Permite deshacer los cambios realizados al grupo.
        /// </summary>
        [HttpPost]
        public ActionResult DeshacerGrupos()
        {
            if (this.listaGruposAsignadosSession.Any())
            {
                this.listaGruposAsignadosSession.Where(x => x.alta)
                    .ToList()
                    .ForEach(t => { this.listaGruposSession.Add(new AltaPerfiles() { Id = t.Id, Nombre = t.Nombre, alta = true }); });

                this.listaGruposAsignadosSession = new List<AltaPerfiles>();
            }

            Grupos grupos = this.repoGrupos.Single(x => x.Id == this.idGrupo);

            if (grupos == null)
            {
                return this.Json(new { result = false, Message = "El Grupo no existe." });
            }

            IEnumerable<PerfilesBuscador> listaPerfiles = (from t1 in this.repoPerfilesGrupos.GetAll().Where(x => x.Id_grupo == Convert.ToInt32(this.idGrupo))
                                                           join t2 in this.repoPerfiles.GetAll().Where(x => x.es_activo == true) on t1.Id_perfil equals t2.Id
                                                           select new PerfilesBuscador { Id_perfil = t1.Id_perfil, Nombre = t2.Nombre });

            listaPerfiles.ToList().ForEach(t => { this.listaGruposAsignadosSession.Add(new AltaPerfiles() { Id = t.Id_perfil, Nombre = t.Nombre, alta = false }); });

            return this.PartialView("~/Views/Grupos/PlantillaEdit.cshtml",grupos);
        }

        /// <summary>
        /// Permite Remover el perfil al grupo.
        /// </summary>
        [HttpPost]
        public ActionResult RemoverPerfil(int? id)
        {
            if (id == null)
            {
                return this.Json(new { result = false, Message = "Id NULL" });
            }

            this.listaGruposAsignadosSession.ToList().ForEach(
                p =>
                {
                    if (p.Id == id)
                    {
                        this.listaGruposAsignadosSession.Remove(p);
                        this.listaGruposSession.Add(new AltaPerfiles() { Id = p.Id, Nombre = p.Nombre, alta=false});
                    }
                });

            return this.PartialView("~/Views/Grupos/Seleccionar.cshtml");
        }
        
        /// <summary>
        /// Permite asignar el idGrupo
        /// </summary>
        [HttpPost]
        public ActionResult AsignarValor(int? id)
        {
            if (id == null)
            {
                return this.Json(new { result = false, Message = "Id NULL" });
            }

            this.idGrupo = id;

            return this.Json(new { result = true });
        }
        
       /// <summary>
        /// Permite Visualizar los grupos y Perfiles asociados.
        /// </summary>
        [HttpPost]
        public ActionResult VisualizarGrupo(int? id)
        {

           this.idGrupo = (id != null) ? (int)id : this.idGrupo;

           if (this.listaGruposAsignadosSession.Any())
           {
               this.listaGruposAsignadosSession.ToList().ForEach(p => { this.listaGruposAsignadosSession.Remove(p); });
           }

           if (this.listaGruposSession.Any())
           {
               this.listaGruposSession.ToList().ForEach(p => { this.listaGruposSession.Remove(p); });
           }

           IEnumerable<Grupos> listaGrupos = this.repoGrupos.GetAll().Where(x => x.Id==this.idGrupo);

           this.UsuarioFrontalSession.nombre = listaGrupos.ToList()[0].Nombre;
           
           this.UsuarioFrontalSession.descripcion = listaGrupos.ToList()[0].descripcion;

           this.UsuarioFrontalSession.Id_publico = listaGrupos.ToList()[0].Id_publico;

           this.UsuarioFrontalSession.activo = listaGrupos.ToList()[0].es_activo;
           
           return this.PartialView("~/Views/Grupos/VisualizarGrupo.cshtml",this.UsuarioFrontalSession);
        }
        
        /// <summary>
        /// Permite asociar los perfiles aun grupo.
        /// </summary>
        [HttpPost]
        public ActionResult AltaGruposPerfiles(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                PerfilesGrupo Listperfilesgrupos = this.repoPerfilesGrupos.Single(x => x.Id_grupo == this.idGrupo && x.Id_perfil == id);

                if (Listperfilesgrupos == null)
                {
 
                    this.repoPerfilesGrupos.Create(new PerfilesGrupo()
                                                   {
                                                       Id_grupo = Convert.ToInt32(this.idGrupo),
                                                       Id_perfil = Convert.ToInt32(id)
                                                   });

                    this.trazas(20, (int)this.UsuarioFrontalSession.Usuario.id, "Se ha agregado un registro en SPCON_Grupos " + Listperfilesgrupos.Id_grupo.ToString());
                }
            }
            catch (ModelValidationException ex)
            {
                return this.Json(new { result = false, Message = ex.Message });
            }
            catch (DbEntityValidationException ex)
            {
                this.log.PublishException(new SecureportExceptionDbEntity(ex));

                return this.Json(new { result = false, Message = ex.Message });
            }
            catch (Exception ex)
            {
                this.log.PublishException(new SecureportException(ex));

                return this.Json(new { result = false, Message = ex.Message });
            }

            return this.Json(new { result = true });
        }

        #endregion

        #region Acciones

        /// <summary>
        ///     Permite actualizar los datos de grupos en el grid.
        /// </summary>
        [HttpPost]
        public ActionResult RefrescarGrupos()
        {
            this.UsuarioFrontalSession.fichero = string.Empty;

            return this.PartialView("~/Views/Grupos/List.cshtml", this.UsuarioFrontalSession);
        }

        /// <summary>
        /// Permite visualizar en el grid los grupos activos.
        /// </summary>
        public ActionResult ListadoGrupos()
        {
            try
            {
                this.UsuarioFrontalSession.fichero = string.Empty;

                if (this.listaGruposAsignadosSession.Any())
                {
                    this.listaGruposAsignadosSession.ToList().ForEach(
                        p =>
                        {
                            this.listaGruposAsignadosSession.Remove(p);
                        });
                }


                if (this.listaGruposSession.Any())
                {
                    this.listaGruposSession.ToList().ForEach(
                        p =>
                        {
                            this.listaGruposSession.Remove(p);
                        });
                }
                
                this.ViewBag.Navegador = this.Browser;
                
                return this.View("ListadoGrupos", this.UsuarioFrontalSession);
            }
            catch (Exception ex)
            {
                this.UsuarioFrontalSession.fichero = string.Empty;
                this.log.PublishException(new SecureportException(ex));
                throw;
            }
        }

        /// <summary>
        /// Permite refrescar los datos de la vista ListPerfiles.cshtml.
        /// </summary>
        [HttpPost]
        public ActionResult RefrescarPerfilDisponible()
        {
            return this.PartialView("~/Views/Grupos/ListPerfiles.cshtml");
        }

        /// <summary>
        /// Permite refrescar los datos de la vista Seleccionar.cshtml.
        /// </summary>
        [HttpPost]
        public ActionResult RefrescarPerfilDisponibles()
        {
            return this.PartialView("~/Views/Grupos/Seleccionar.cshtml");
        }
  
        #endregion
    }
}