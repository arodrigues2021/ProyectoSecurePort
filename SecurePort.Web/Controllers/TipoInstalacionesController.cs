
namespace SecurePort.Web.Controllers
{

    #region Using
    using Kendo.Mvc.Extensions;
    using Kendo.Mvc.UI;
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

    public class TipoInstalacionesController : BaseController
    {
        public TipoInstalacionesController(IServiciosTipoInstalacion _serviciosTipoInstalacion, 
                                TipoInstalacionRepository repoTipoInstalaciones, 
                                TrazasRepository repoTrazas, ILogger log) 
            : base(_serviciosTipoInstalacion, repoTipoInstalaciones, repoTrazas, log)
        {

        }

        #region action
        /// <summary>
        /// Retorna una lista de todos los tipos instlaciones en el grid.
        /// </summary>
        public ActionResult ListadoInstalaciones()
        {
            try
            {
                ViewBag.UrlDelete = "/TipoInstalaciones/EliminarInstalacion";

                return this.PartialView("ListadoInstalaciones", this.UsuarioFrontalSession);
            }
            catch (Exception ex)
            {
                this.log.PublishException(new SecureportException(ex));
                throw;
            }
        }
        
        [HttpPost]
        public ActionResult DatosInstalaciones()
        {
            return this.PartialView("~/Views/TipoInstalaciones/Create.cshtml");
        }

        [HttpPost]
        public ActionResult VisualizarInstalaciones(int? id)
        {
             try
            {
                this.idInstalacion = (id == null? this.idInstalacion: (int)id);

                Tipos_Instalacion tipos_instalacion = this.repoTipoInstalaciones.Single(x => x.Id == this.idInstalacion);

                ViewBag.disabled = true;

                return this.PartialView("~/Views/TipoInstalaciones/_PartialInstalaciones.cshtml", tipos_instalacion);
            }
             catch (Exception ex)
             {
                 this.log.PublishException(new SecureportException(ex));

                 return this.Json(new { result = false, Message = ex.Message });
             }
        }

        /// <summary>
        /// Alta de Instalacion.
        /// </summary>
        [HttpPost]
        public ActionResult AltaInstalacion(TipoInstalacionJson tipoInstalacionJson)
        {
            try
            {
                IEnumerable<Tipos_Instalacion> result = null;

                result = _serviciosTipoInstalacion.ListTipoInstalaciones(tipoInstalacionJson.Nombre,tipoInstalacionJson.ID);

                bool resultado = false;
                foreach (Tipos_Instalacion  tipoInstalacion in result)
                {
                    if (tipoInstalacion.es_activo == true)
                        resultado = true;
                }
                if (!resultado)
                {
                      if(result.Count() ==0)
                          AltaTipoInstalacion(tipoInstalacionJson);
                       else
                           return this.Json(new { result = 2, Message = "Ya existe un tipo de instalación " + tipoInstalacionJson.Nombre + " desactivado ¿Desea continuar? " });                            
                 } else {
                      this.trazas(68, (int)this.UsuarioFrontalSession.Usuario.id, "Error al dar de alta el tipo de instalación, ya existe otro tipo de instalación activo " + tipoInstalacionJson.Nombre);
                      return this.Json(new { result = 1, Message = "Error al dar de alta el tipo de instalación, ya existe otro tipo de instalación activo " + tipoInstalacionJson.Nombre });
                 }
                return this.Json(new { result = 0 });
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
        /// Editar Instalacion.
        /// </summary>
        [HttpPost]
        public ActionResult EditarInstalacion(TipoInstalacionJson tipoInstalacionJson)
        {
            try
            {
                IEnumerable<Tipos_Instalacion> result = null;

                result = _serviciosTipoInstalacion.ListTipoInstalaciones(tipoInstalacionJson.Nombre, tipoInstalacionJson.ID);

                if (result.Count() == 0)
                {
                    EditarTipoInstalacion(tipoInstalacionJson);
                }
                else
                {
                    IEnumerable<Tipos_Instalacion> TipoIns = result.Where(x => x.Id != tipoInstalacionJson.ID);
                    if (TipoIns == null)
                    {
                        EditarTipoInstalacion(tipoInstalacionJson);
                    }
                    else
                    {
                        Tipos_Instalacion ActTipo = TipoIns.FirstOrDefault();
                        if (ActTipo.Id == tipoInstalacionJson.ID)
                        {
                            return this.Json(new { result = 3, Message = "Ya existe un tipo de instalación " + tipoInstalacionJson.Nombre + " desactivado ¿Desea continuar? " });
                        }
                        else
                        {
                            if (!ActTipo.es_activo)
                                return this.Json(new { result = 3, Message = "Ya existe un tipo de instalación " + tipoInstalacionJson.Nombre + " desactivado ¿Desea continuar? " });
                            else
                            {
                                this.trazas(70, (int)this.UsuarioFrontalSession.Usuario.id, "Error al modificar el tipo de instalación, ya existe otro tipo de instalación activo " + tipoInstalacionJson.Nombre + "(" + tipoInstalacionJson.ID + ")");
                                return this.Json(new { result = 1, Message = "Error al modificar el tipo de instalación, ya existe otro  tipo de instalación activo " + tipoInstalacionJson.Nombre });
                            }
                        }
                    }
                }
                return this.Json(new { result = 0 });
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
        /// Borrar fisicamente el registro. 
        /// </summary>
        [HttpPost]
        public ActionResult EliminarInstalacion(int Id)
        {
            try
            {

                this.repoTipoInstalaciones.Delete(this.repoTipoInstalaciones.Single(x => x.Id == Id));

                this.trazas(69, (int)this.UsuarioFrontalSession.Usuario.id, "Se ha realizado la eliminacion de un registro en SPMAE_tipos_Instalacion con ID" + "("+Id.ToString()+")");
            }
            catch (DbUpdateException ex)
            {
                this.log.PublishException(new SecureportExceptionDbUpdate(ex));

                return this.Json(new { result = false, Message = MessageDelete(ex) });
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

      

        [HttpPost]
        public ActionResult AltaTipoInstalacion(TipoInstalacionJson tipoInstalacionJson)
        {
            try
            {
                Tipos_Instalacion Instalacion = new Tipos_Instalacion();

                Instalacion.id_usu_alta = this.UsuarioFrontalSession.Usuario.id;

                Instalacion.Clasificacion = tipoInstalacionJson.Clasificacion;

                Instalacion.es_activo = true;

                Instalacion.fech_activo = DateTime.Now;

                Instalacion.fech_alta = DateTime.Now;

                Instalacion.Descripcion = tipoInstalacionJson.Nombre;

                this.repoTipoInstalaciones.Create(Instalacion);

                var Id = this.db.Tipos_Instalacion.OrderByDescending(x => x.Id).FirstOrDefault().Id;

                this.trazas(68, (int)this.UsuarioFrontalSession.Usuario.id, "Se ha dado de alta un registro en SPMAE_Tipos_instalacion con ID" + "(" + Id.ToString() + ")");

                return this.Json(new { result = 0 });
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
        public ActionResult EditarTipoInstalacion(TipoInstalacionJson tipoInstalacionJson)
        {
            try
            {
                Tipos_Instalacion instalaciones = this.repoTipoInstalaciones.Single(x => x.Id == tipoInstalacionJson.ID);

                instalaciones.Descripcion = tipoInstalacionJson.Nombre;

                instalaciones.Clasificacion = tipoInstalacionJson.Clasificacion;

                instalaciones.es_activo = (tipoInstalacionJson.activo == "1" ? true : false);

                this.repoTipoInstalaciones.Update(instalaciones);

                this.trazas(70, (int)this.UsuarioFrontalSession.Usuario.id, "Se ha realizado la modificaion de un registro en SPMAE_Tipos_Instalacion con ID" + "(" + tipoInstalacionJson.ID.ToString() + ")");

                return this.Json(new { result = 0 });
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

        #endregion

        #region Buscador

        /// <summary>
        /// Permite buscar por las diferentes colummnas del grid (List.cshtml)
        /// </summary>
        public ActionResult BuscadorInstalaciones([DataSourceRequest]DataSourceRequest request)
        {
            try
            {

                IEnumerable<ListadoTipoInstalaciones> result = this._serviciosTipoInstalacion.ListTipoInstalaciones().ToList();

                return this.Json(result.ToDataSourceResult(request));
            }
            catch (Exception ex)
            {
                this.log.PublishException(new SecureportException(ex));
                throw;
            }
        }

        

        #endregion
   

    }
}