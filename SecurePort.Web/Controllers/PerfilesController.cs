
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
    using System.Web.Mvc;
    #endregion

    public class PerfilesController : BaseController
    {
        public PerfilesController(
                    PerfilesPermisosRepository repoPerfilesPermisos,
                    GestionpermisosRepository repogestionpermisos,
                    GruposRepository repoGrupos,
                    PerfilesRepository repoPerfiles,
                    PerfilesGruposRepository repoPerfilesGrupos,
                    UsuariosRepository repoUsuarios,
                    PermisosRepository repoPermisos,
                    IServiciosGrupos serviciosGrupos,
                    IServiciosUsuarios serviciosUsuarios,
                    TrazasRepository repoTrazas,
                    ILogger log)
                    : base(repoPerfilesPermisos,
                        repogestionpermisos,
                        repoGrupos,
                        repoPerfiles,
                        repoPerfilesGrupos,
                        repoUsuarios,
                        repoPermisos,
                        serviciosGrupos,
                        serviciosUsuarios,
                        repoTrazas,
                        log)
        {
        }


        /// <summary>
        /// Vista de Grupos
        /// </summary>
        public ActionResult Index()
        {
            return this.View();
        }

        #region Buscador

        /// <summary>
        /// Permite buscar por las diferentes colummnas del grid (List.cshtml)
        /// </summary>
        public ActionResult BuscadorPerfil([DataSourceRequest]DataSourceRequest request)
        {
            try
            {
                //this.listagestionpermisosSession = null;
                this.PermisosRemovidosSession.Clear();
                //this.listapermisosSession = null;
            

                IEnumerable<PerfilesViewModel> result =
                    (from t1 in this.repoPerfiles.GetAll()
                        orderby t1.Nombre, t1.descripcion
                     select new PerfilesViewModel{
                                              Id = t1.Id, 
                                              Nombre = t1.Nombre, 
                                              descripcion = t1.descripcion,
                                              es_activo = t1.es_activo==true?"Si":"No"});

                return this.Json(result.ToDataSourceResult(request));
            }
            catch (Exception ex)
            {
                this.log.PublishException(new SecureportException(ex));
                throw;
            }
        }
        
        /// <summary>
        /// Permite visualizar los permisos asociados aun perfil.
        /// </summary>
        public ActionResult Permisos([DataSourceRequest]DataSourceRequest request)
        {
            try
            {
                IEnumerable<Gestionpermisos> listagestionpermisos = this.repogestionpermisos.GetAll().OrderBy(d=> d.Nombre);

                IEnumerable<AltaAcciones> listapermisos = (from t1 in this.repoPerfilesPermisos.GetAll().
                                                               Where(x => x.Id_perfil == Convert.ToInt32(this.idPerfil))
                                                                    join t2 in this.repoPermisos.GetAll() on t1.Id_permiso equals t2.Id
                                                                        select new AltaAcciones { Id = t2.Id, Nombre = t2.descripcion, idConjunto = t2.id_grupo_accion, alta = false }).ToList().OrderBy(t => t.Nombre);

                if (listapermisos.Any())
                {
                    this.idConjunto = listapermisos.ToList().FirstOrDefault().idConjunto;

                    this.listagestionpermisosSession = listapermisos.Where(x=> x.idConjunto==this.idConjunto).ToList();
                }

                IEnumerable<GestionpermisosViewModel> result = from c in listagestionpermisos select new GestionpermisosViewModel { Id = c.Id, Nombre = c.Nombre ,asignar = false};

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
        /// Permite remover una acción aun perfil.
        /// </summary>
        [HttpPost]
        public ActionResult RemoverAccion(int? id, int idconjunto)
        {
            if (id == null)
            {
                return this.Json(new { result = false, Message = "Id NULL" });
            }

            var removidos = this.PermisosRemovidosSession.Where(x => x.idConjunto == this.idConjunto && x.origen == "1").ToList();

            this.listagestionpermisosSession.ToList().ForEach(
                p =>
                {
                    if (p.Id == id && p.idConjunto == idconjunto)
                    {
                        this.listagestionpermisosSession.Remove(p);

                        if (this.PermisosRemovidosSession.Where(x => x.Id == id && x.idConjunto == idconjunto).Any() == false)
                        {
                            this.PermisosRemovidosSession.Add(new AltaAcciones() { Id = p.Id, Nombre = p.Nombre, alta = true, idConjunto = p.idConjunto, origen = "2" });
                        }                        
                    }
                });

            removidos.ToList().ForEach(
            p =>
            {
                if (p.Id == id && p.idConjunto == idconjunto)
                    this.PermisosRemovidosSession.Remove(p);

            });

            this.idConjunto = idconjunto;

            return this.PartialView("~/Views/Perfiles/SeleccionarAccion.cshtml");
        }

        /// <summary>
        /// visualizar los diferentes permisos de la opción seleccionado.
        /// </summary>
        public ActionResult PermisosOpcion([DataSourceRequest]DataSourceRequest request)
        {
            try
            {
                IEnumerable<PermisosOpcionViewModel> result = new List<PermisosOpcionViewModel>();

                var removidos = this.PermisosRemovidosSession.Where(x => x.idConjunto == this.idConjunto && x.origen != "1").ToList();

                if (removidos.Any())
                {
                    this.listagestionpermisosSession.Where(x => x.idConjunto == this.idConjunto).ToList().ForEach(
                        p => removidos.ToList().ForEach(
                            t =>
                            {
                                if (p.Id == t.Id
                                    && p.idConjunto == this.idConjunto)
                                {
                                    this.listagestionpermisosSession.Remove(t);
                                }
                            }));

                    result = from c in this.listagestionpermisosSession.Where(x => x.idConjunto == this.idConjunto).ToList()
                                        select new PermisosOpcionViewModel { Id = c.Id, Nombre = c.Nombre, idConjunto = c.idConjunto };
                }
                else
                {
                         result = from c in this.listagestionpermisosSession.Where(x => x.idConjunto == this.idConjunto).ToList()
                                      select new PermisosOpcionViewModel
                                      {
                                        Id = c.Id,
                                        Nombre = c.Nombre,
                                        idConjunto = c.idConjunto
                                      };
                }

                
                return this.Json(result.ToDataSourceResult(request));
                
                }
            catch (Exception ex)
            {
                this.log.PublishException(new SecureportException(ex));
                throw;
            }
        }

        /// <summary>
        /// Permite agregar una acción aun perfil.
        /// </summary>
        [HttpPost]
        public ActionResult AgregarAccion(int? id, int idconjunto)
        {
            if (id == null)
            {
                return this.Json(new { result = false, Message = "Id NULL" });
            }

            var removidos = this.PermisosRemovidosSession.Where(x => x.idConjunto == this.idConjunto && x.origen == "2").ToList();

            this.listapermisosSession.ToList().ForEach(
                p =>
                {
                    if (p.Id == id && p.idConjunto == idconjunto)
                    {
                        this.listapermisosSession.Remove(p);
                        
                        this.listagestionpermisosSession.Add(new AltaAcciones() { Id = p.Id, Nombre = p.Nombre, alta = true, idConjunto = p.idConjunto });
                        if (this.PermisosRemovidosSession.Where(x => x.Id == id && x.idConjunto == idconjunto).Any() == false)
                            this.PermisosRemovidosSession.Add(new AltaAcciones() { Id = p.Id, Nombre = p.Nombre, alta = true, idConjunto = p.idConjunto, origen ="1" });
                        
                    }
                });


            
            
            removidos.ToList().ForEach(
             p =>
             {
                 if (p.Id == id && p.idConjunto == idconjunto)
                     this.PermisosRemovidosSession.Remove(p);
                 
            });


            this.idConjunto = idconjunto;

            return this.PartialView("~/Views/Perfiles/SeleccionarAccion.cshtml");
        }
        
        /// <summary>
        /// Permite eliminar un perfil de la aplicación.
        /// </summary>
        [HttpPost]
        public ActionResult EliminarPerfil(int? id)
        {
            try
            {
                if (id == null)
                {
                    return this.Json(new { result = false, Message = "Id NULL" });
                }

                Perfiles perfiles = this.repoPerfiles.Single(x => x.Id == id);

                if (perfiles == null)
                {
                    return this.Json(new { result = false, Message = "El Perfil no existe." });
                }

                //Eliminamos el usuario por Store Procedure.
                if (this._serviciosUsuarios.RemovePerfilById(id) == 1)
                {
                    this.trazas(17, (int)id, "Se ha elimado un registro en SPCON_Perfiles con ID " + "(" + id.ToString() + ")");

                    return this.Json(new { result = false, Message = "Se ha eliminado el Perfil." });
                }

                return this.Json(new { result = false, Message = "Existen registros asociados al Perfil." });

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

            //return this.Json(new { result = true });
        }

        [HttpPost]
        public ActionResult RegisterPerfiles(PerfilesJson PerfilesJson)
        {
            try
            {
                    Perfiles perfiles = new Perfiles()
                    {
                        Nombre = PerfilesJson.Nombre.ToUpper(),
                        descripcion = PerfilesJson.Descripcion,
                        es_activo = true,
                        id_usu_alta = this.UsuarioFrontalSession.Usuario.id,
                        fech_alta = DateTime.Now,
                        fech_activo = DateTime.Now
                    };

                    this.repoPerfiles.Create(perfiles);
                    var Id = this.db.Perfiles.OrderByDescending(x => x.Id).FirstOrDefault().Id;
                    this.trazas(15, (int)this.UsuarioFrontalSession.Usuario.id, "Se ha agregado un registro en SPCON_Perfiles con ID " + "(" + Id.ToString() + ")");
                    return this.Json(new { result = true });
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
        /// Permite Alta de un perfil en la aplicación.
        /// </summary>
        [HttpPost]
        public ActionResult AltaPerfiles(PerfilesJson PerfilesJson)
        {
            try
            {
                //Encuentra el mismo perfil 
                Perfiles perfiles_ant = this.repoPerfiles.Single(x => x.Nombre == PerfilesJson.Nombre.ToUpper()  && x.es_activo == true);
                Perfiles perfiles_ant_id = this.repoPerfiles.Single(x => x.Nombre == PerfilesJson.Nombre.ToUpper() && x.es_activo == false); 

                // perfil nuevo
                if (perfiles_ant == null && perfiles_ant_id == null)
                {
                    Perfiles perfiles = new Perfiles()
                        {
                            Nombre = PerfilesJson.Nombre.ToUpper(),
                            descripcion = PerfilesJson.Descripcion,
                            es_activo = true,
                            id_usu_alta = this.UsuarioFrontalSession.Usuario.id,
                            fech_alta = DateTime.Now,
                            fech_activo = DateTime.Now
                        };

                    this.repoPerfiles.Create(perfiles);
                    var Id = this.db.Perfiles.OrderByDescending(x => x.Id).FirstOrDefault().Id;
                    this.trazas(15, (int)this.UsuarioFrontalSession.Usuario.id, "Se ha agregado un registro en SPCON_Perfiles con ID " + "(" + Id.ToString() + ")");
                }
                else
                {
                    //Encuentra el mismo perfil desactivo.
                    if (perfiles_ant_id != null && perfiles_ant != null)
                    {
                        if (perfiles_ant.es_activo == true)
                        {
                            return this.Json(new { result = true, Message = "Error al dar de alta un Perfil, ya existe otro perfil activo " + PerfilesJson.Descripcion });
                        }
                    }
                    else if (perfiles_ant_id != null)
                    {
                        return this.Json(new { result = false, Message = "Error al dar de alta un Perfil, ya existe otro perfil desactivo " + PerfilesJson.Descripcion + "¿Desea continuar? " });

                    }
                    else
                    {
                        return this.Json(new { result = true, Message = "No se permite el alta de un Perfil que ya existe" });

                    }
                }
                return this.Json(new { result = true, Message="Datos guardados correctamente" });

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

            Perfiles perfiles = this.repoPerfiles.Single(x => x.Id == id);

            if (perfiles == null)
            {
                return this.Json(new { result = false, Message = "El Perfil no existe." });
            }

            this.idPerfil = id;

            IEnumerable<AltaAcciones> listapermisos = (from t1 in this.repoPerfilesPermisos.GetAll().Where(x => x.Id_perfil == Convert.ToInt32(this.idPerfil))
                                                       join t2 in this.repoPermisos.GetAll() on t1.Id_permiso equals t2.Id
                                                       select new AltaAcciones { Id = t2.Id, Nombre = t2.descripcion, idConjunto = t2.id_grupo_accion, alta = false }).ToList()
                                                       .OrderBy(t=> t.Nombre);

            if (listapermisos.Any())
            {
                this.idConjunto = listapermisos.ToList().FirstOrDefault().idConjunto;

                var Grupo = this.repogestionpermisos.GetAll().Where(x => x.Id == this.idConjunto).ToList();

                this.TempData["Grupo"] = Grupo[0].Nombre;
            }
            
            return this.PartialView("~/Views/perfiles/PlantillaEdit.cshtml",perfiles);
        }

        /// <summary>
        /// Permite deshacer los cambios en el grid de perfiles.
        /// </summary>
        [HttpPost]
        public ActionResult DeshacerAcciones()
        {

            if (this.listagestionpermisosSession.Any())
            {
                this.listagestionpermisosSession.ToList().ForEach(
                    p =>
                    {
                        this.listagestionpermisosSession.Remove(p);
                    });
            }


            if (this.listapermisosSession.Any())
            {
                this.listapermisosSession.ToList().ForEach(
                    p =>
                    {
                        this.listapermisosSession.Remove(p);
                    });
            }

            if (this.PermisosRemovidosSession.Any())
            {
                this.PermisosRemovidosSession.ToList().ForEach(
                    p =>
                    {
                        this.PermisosRemovidosSession.Remove(p);
                    });
            }


            if (this.PermisosAltaSession.Any())
            {
                this.PermisosAltaSession.ToList().ForEach(
                    p =>
                    {
                        this.PermisosAltaSession.Remove(p);
                    });
            }
            
            Perfiles perfiles = this.repoPerfiles.Single(x => x.Id == this.idPerfil);

            return this.PartialView("~/Views/perfiles/PlantillaEdit.cshtml",perfiles);
        }

        /// <summary>
        /// Permite guardar las acciones de los permisos del perfil.
        /// </summary>
        [HttpPost]
        public ActionResult GuardarAcciones(PerfilesJson perfilesJson)
        {
            try
            {

                if (this.PermisosAltaSession.Any())
                {
                    this.PermisosAltaSession.ToList().ForEach(
                        p =>
                        {
                            var existe = this.repoPerfilesPermisos.GetAll().Where(x => x.Id_perfil == Convert.ToInt32(this.idPerfil) && x.Id_permiso == p.Id);

                            if (!existe.Any())
                            {
                                this.repoPerfilesPermisos.Create(
                                    new AccionesPerfil()
                                    {
                                        Id_perfil = Convert.ToInt32(this.idPerfil),
                                        Id_permiso = p.Id,
                                        id_usu_alta = this.UsuarioFrontalSession.Usuario.id,
                                        fech_alta = DateTime.Now
                                    });
                            }
                        });
                }
                else
                {
                    this.listagestionpermisosSession.Where(x=> x.alta).ToList().ForEach(
                        p =>
                        {
                            var existe = this.repoPerfilesPermisos.GetAll().Where(x => x.Id_perfil == Convert.ToInt32(this.idPerfil) && x.Id_permiso == p.Id);

                            if (!existe.Any())
                            {
                                this.repoPerfilesPermisos.Create(
                                    new AccionesPerfil()
                                    {
                                        Id_perfil = Convert.ToInt32(this.idPerfil),
                                        Id_permiso = p.Id,
                                        id_usu_alta = this.UsuarioFrontalSession.Usuario.id,
                                        fech_alta = DateTime.Now
                                    });
                            }
                        });
                }

                this.PermisosRemovidosSession.Where(x => x.origen != "1").ToList()
                    .ForEach(
                        p => this.repoPerfilesPermisos.GetAll().Where(x => x.Id_perfil == Convert.ToInt32(this.idPerfil)).ToList().ForEach(
                            t =>
                            {
                                if (p.Id == t.Id_permiso)
                                {
                                    this.repoPerfilesPermisos.Delete(t);
                                }
                            }));

                this.repoPerfiles.GetAll()
                    .Where(x => x.Id == this.idPerfil)
                    .ToList()
                    .ForEach(
                        x =>
                            this.repoPerfiles.Update(
                                new Perfiles()
                                {
                                    Id          = x.Id,
                                    Nombre      = perfilesJson.Nombre.ToUpper(),
                                    descripcion = perfilesJson.Descripcion,
                                    fech_activo = x.fech_activo,
                                    es_activo   = perfilesJson.Activo=="1",
                                    fech_alta   = x.fech_alta,
                                    id_usu_alta = x.id_usu_alta
                                }));


                if (this.PermisosRemovidosSession.Any())
                {
                    this.PermisosRemovidosSession = new List<AltaAcciones>();
                }

                if (this.PermisosAltaSession.Any())
                {
                    this.PermisosAltaSession = new List<AltaAcciones>();
                }

                if (this.listaPermisosAsignadosSession.Any())
                {
                    this.listaPermisosAsignadosSession= new List<ListGruposPerfilesPermisos>();
                }

                this.listagestionpermisosSession  = (from t1 in this.repoPerfilesPermisos.GetAll().Where(x => x.Id_perfil == Convert.ToInt32(this.idPerfil))
                                                           join t2 in this.repoPermisos.GetAll() on t1.Id_permiso equals t2.Id
                                                           select new AltaAcciones { Id = t2.Id, Nombre = t2.descripcion, idConjunto = t2.id_grupo_accion, alta = false }).ToList();

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

        /// <summary>
        /// Permite dar alta aun perfil.
        /// </summary>
        [HttpPost]
        public ActionResult AltaPermisosPerfiles(int id)
        {
            try
            {
                IEnumerable<Acciones> listPermisos = this.repoPermisos.GetAll().Where(x => x.id_grupo_accion == id);

                if (listPermisos.Any())
                {
                    foreach (Acciones Obj in this.repoPermisos.GetAll().Where(x => x.id_grupo_accion == id))
                    {
                        if (this.listagestionpermisosSession.Any())
                        {
                            List<AltaAcciones> listapermisos = this.listagestionpermisosSession.Where(x => x.Id == Obj.Id).ToList();

                            if (listapermisos.Any() == false)
                            {
                                this.listagestionpermisosSession.Add(new AltaAcciones() { Id = Obj.Id, Nombre = Obj.descripcion, idConjunto = id, alta = true });
                             }
                        }
                        else
                        {
                            this.listagestionpermisosSession.Add(new AltaAcciones() { Id = Obj.Id, Nombre = Obj.descripcion, idConjunto = id, alta = true });
                        }
                    }
                }

                if (this.listapermisosSession.Any())
                {
                    this.listapermisosSession = new List<AltaAcciones>();
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

            this.idConjunto = (int)id;

            return this.Json(new { result = true });
        }

        #endregion

        #region Acciones

        /// <summary>
        /// Permite Visualizar los perfiles asociado a sus acciones.
        /// </summary>
        [HttpPost]
        public ActionResult VisualizarPerfil(int? id)
        {
            this.idPerfil = (id != null) ? (int)id : this.idPerfil;
           
             if (this.listagestionpermisosSession.Any())
             {
                this.listagestionpermisosSession = new List<AltaAcciones>();
             }

             if (this.listapermisosSession.Any())
             {
                this.listapermisosSession = new List<AltaAcciones>();
             }

            IEnumerable<Perfiles> listaPerfiles = this.repoPerfiles.GetAll().Where(x => x.Id==this.idPerfil).ToList();

            if (listaPerfiles.Any())
            {
                this.UsuarioFrontalSession.nombre = listaPerfiles.ToList()[0].Nombre;

                this.UsuarioFrontalSession.descripcion = listaPerfiles.ToList()[0].descripcion;

                this.UsuarioFrontalSession.activo = listaPerfiles.ToList()[0].es_activo;
   
            }
            //this.listagestionpermisosSession = null;
            this.PermisosRemovidosSession.Clear();
            //this.listapermisosSession = null;
            
            return this.PartialView("~/Views/Perfiles/VisualizarPerfil.cshtml",this.UsuarioFrontalSession);
        }

        /// <summary>
        /// Permite Refrescar la vista ListPermisos.cshtml .
        /// </summary>
        [HttpPost]
        public ActionResult RefrescarAccionDisponible(int? id,string grupo)
        {
            this.idConjunto = (id != null) ? (int)id : this.idConjunto;
            


            this.listapermisosSession = (from t1 in this.repoPermisos.GetAll().Where(x => x.id_grupo_accion == this.idConjunto)
                                         select new AltaAcciones { Id = t1.Id, Nombre = t1.descripcion, idConjunto = this.idConjunto, alta = false }).ToList();



                IEnumerable<AltaAcciones> listapermisos = (from t1 in this.repoPerfilesPermisos.GetAll().Where(x => x.Id_perfil == Convert.ToInt32(this.idPerfil))
                    join t2 in this.repoPermisos.GetAll() on t1.Id_permiso equals t2.Id
                    select new AltaAcciones { Id = t2.Id, Nombre = t2.descripcion, idConjunto = t2.id_grupo_accion, alta = true }).ToList();

                listapermisos.ToList().ForEach(
                    p =>
                    {
                        if (this.listagestionpermisosSession.Where(x=> x.Id==p.Id && x.idConjunto==p.idConjunto).Any() == false)
                        {
                            this.listagestionpermisosSession.Add(p);
                        }
  
                    });

                if (this.PermisosRemovidosSession.Any())
                {
                    this.listagestionpermisosSession.Where(x => x.idConjunto == this.idConjunto).ToList().ForEach(

                    p => this.PermisosRemovidosSession.Where(x => x.idConjunto == this.idConjunto && x.origen != "1").ToList().ForEach(
                       t =>
                        {
                            if (p.Id == t.Id && p.idConjunto == this.idConjunto)
                            {
                                this.listagestionpermisosSession.Remove(p);
                            }
                        }));
                }


            this.listagestionpermisosSession.ToList().ForEach(
                    p => this.listapermisosSession.ToList().ForEach(
                        t =>
                        {
                            if (p.Id == t.Id && p.idConjunto == this.idConjunto)
                            {
                                this.listapermisosSession.Remove(t);

                            }
                        }));

           
            this.TempData["Grupo"] = grupo;

            return this.PartialView("~/Views/Perfiles/ListPermisos.cshtml");
        }

        public ActionResult AccionesRefrescar([DataSourceRequest]DataSourceRequest request)
        {
            try
            {
                IEnumerable<AltaAcciones> result = from c in this.listapermisosSession.Where(x => x.idConjunto == this.idConjunto)
                    select new AltaAcciones { Id = c.Id, Nombre = c.Nombre, idConjunto = c.idConjunto };
                return this.Json(result.ToDataSourceResult(request));
            }
            catch (Exception ex)
            {
                this.log.PublishException(new SecureportException(ex));
                throw;
            }

        }
        
        /// <summary>
        /// visualizar los diferentes permisos que no se encuentran asignados.
        /// </summary>
        public ActionResult AccionesDisponibles([DataSourceRequest]DataSourceRequest request)
        {
            try
            {

                this.listapermisosSession = (from t1 in this.repoPermisos.GetAll().Where(x => x.id_grupo_accion == this.idConjunto)
                                                                     orderby t1.Nombre
                                                                     select new AltaAcciones { Id = t1.Id, Nombre = t1.descripcion, idConjunto = this.idConjunto, alta = false }).ToList();

                if (this.listapermisosSession.Any() == false)
                {
                    this.listapermisosSession = (from t1 in this.repoPermisos.GetAll().Where(x => x.id_grupo_accion == this.idConjunto)
                                                 select new AltaAcciones { Id = t1.Id, Nombre = t1.descripcion, idConjunto = this.idConjunto, alta = false }).ToList();

                }
                
                this.listagestionpermisosSession.Where(x=> x.idConjunto==this.idConjunto).ToList().ForEach(

                    p => this.listapermisosSession.Where(x=> x.idConjunto==this.idConjunto).ToList().ForEach(
                        t =>
                        {
                            if (p.Id == t.Id && p.idConjunto == this.idConjunto)
                            {
                                this.listapermisosSession.Remove(t);
                            }
                        }));

                
                IEnumerable<AltaAcciones> result = from c in this.listapermisosSession.Where(x=> x.idConjunto==this.idConjunto) 
                                                   select new AltaAcciones
                                                   {
                                                       Id = c.Id,
                                                       Nombre = c.Nombre,
                                                       idConjunto = c.idConjunto
                                                   };

                return this.Json(result.ToDataSourceResult(request));

            }
            catch (Exception ex)
            {
                this.log.PublishException(new SecureportException(ex));
                throw;
            }
        }


        /// <summary>
        /// Permite Refrescar la vista ListPermisos.cshtml .
        /// </summary>
        [HttpPost]
        public ActionResult RefrescarAccionAlta(int? id)
        {
            return this.PartialView("~/Views/Perfiles/SeleccionarAccion.cshtml");}

        /// <summary>
        /// Permite Refrescar la vista SeleccionarAccion.cshtml .
        /// </summary>
        [HttpPost]
        public ActionResult RefrescarAsignada()
        {
            
            IEnumerable<AltaAcciones> listapermisos = (from t1 in this.repoPerfilesPermisos.GetAll().Where(x => x.Id_perfil == Convert.ToInt32(this.idPerfil))
                                                       join t2 in this.repoPermisos.GetAll() on t1.Id_permiso equals t2.Id
                                                       select new AltaAcciones { Id = t2.Id, Nombre = t2.descripcion, idConjunto = t2.id_grupo_accion, alta = false }).ToList();


            listapermisos.Where(x => x.idConjunto == this.idConjunto).ToList().ForEach(
                t => this.listapermisosSession.Where(x => x.idConjunto == this.idConjunto).ToList().ForEach(
                       p =>
                       {
                           if (t.Id == p.Id)
                           {
                               this.listagestionpermisosSession.Remove(t);
                           }

                       }));

            
            return this.PartialView("~/Views/Perfiles/SeleccionarAccion.cshtml");
        }


        /// <summary>
        /// Permite Refrescar la vista SeleccionarAccion.cshtml .
        /// </summary>
        [HttpPost]
        public ActionResult RefrescarListPermisos()
        {
            return this.PartialView("~/Views/Perfiles/RefrescarListPermisos.cshtml");
        }
        
        /// <summary>
        /// Permite Refrescar la vista ListPermisos.cshtml .
        /// </summary>
        [HttpPost]
        public ActionResult RefrescarAccion()
        {
            return this.PartialView("~/Views/Perfiles/ListPermisos.cshtml");
        }

        /// <summary>
        /// Permite Refrescar la vista List.cshtml .
        /// </summary>
        [HttpPost]
        public ActionResult RefrescarPerfiles()
        {
            this.UsuarioFrontalSession.fichero = string.Empty;

            return this.PartialView("~/Views/Perfiles/List.cshtml", this.UsuarioFrontalSession);
        }


        /// <summary>
        /// Permite asignar el idPerfil
        /// </summary>
        [HttpPost]
        public ActionResult AsignarValor(int? id)
        {
            if (id == null)
            {
                return this.Json(new { result = false, Message = "Id NULL" });
            }

            this.idPerfil = id;

            return this.Json(new { result = true });
        }

        
        /// <summary>
        /// visualizar el detalle de los permisos asociados al perfil.
        /// </summary>
        public ActionResult BuscadorDetalle([DataSourceRequest]DataSourceRequest request)
        {
            try
            {


                if (this.listagestionpermisosSession.Any())
                {
                    this.listagestionpermisosSession= new List<AltaAcciones>();
                }

                IEnumerable<AltaAcciones> listapermisos = (from t1 in this.repoPerfilesPermisos.GetAll().Where(x => x.Id_perfil == Convert.ToInt32(this.idPerfil))
                                                                join t2 in this.repoPermisos.GetAll() on t1.Id_permiso equals t2.Id
                                                                join t3 in this.repogestionpermisos.GetAll() on t2.id_grupo_accion equals t3.Id
                                                           select new AltaAcciones { Id           = t2.Id, 
                                                                                     Nombre       = t2.descripcion, 
                                                                                     idConjunto   = t2.id_grupo_accion, 
                                                                                     alta         = false, 
                                                                                     NombreAccion = t3.Nombre}).ToList();

                if (listapermisos.Any())
                {

                    this.listagestionpermisosSession = listapermisos.ToList();
                }

                IEnumerable<AltaAcciones> result = from c in this.listagestionpermisosSession.OrderBy(x=> x.idConjunto) select new AltaAcciones { Id = c.Id, NombreAccion = c.NombreAccion, 
                                                                                                                              Nombre = c.Nombre, idConjunto = c.idConjunto };

                return this.Json(result.OrderBy(x=> x.Nombre).ToDataSourceResult(request));
            }
            catch (Exception ex)
            {
                this.log.PublishException(new SecureportException(ex));
                throw;
            }
        }

        /// <summary>
        /// visualizar los diferentes Perfiles en el grid.
        /// </summary>
        public ActionResult ListadoPerfiles()
        {
            try
            {
                this.UsuarioFrontalSession.fichero = string.Empty;

                if (this.PermisosRemovidosSession.Any())
                {
                    this.PermisosRemovidosSession= new List<AltaAcciones>();
                }

                if (this.PermisosAltaSession.Any())
                {
                    this.PermisosAltaSession = new List<AltaAcciones>();
                }

                if (this.listagestionpermisosSession.Any())
                {
                    this.listagestionpermisosSession = new List<AltaAcciones>();
                }

                if (this.listapermisosSession.Any())
                {
                    this.listapermisosSession = new List<AltaAcciones>();
                }
                
                this.ViewBag.Navegador = this.Browser;

                return this.View("ListadoPerfiles", this.UsuarioFrontalSession);
            }
            catch (Exception ex)
            {
                this.UsuarioFrontalSession.fichero = string.Empty;
                this.log.PublishException(new SecureportException(ex));
                throw;
            }
        }

        #endregion
    }
}