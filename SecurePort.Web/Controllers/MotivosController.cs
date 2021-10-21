
namespace SecurePort.Web.Views
{
    #region Using
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.ModelConfiguration;
    using Kendo.Mvc.Extensions;
    using Kendo.Mvc.UI;
    using SecurePort.Entities.Enums;
    using SecurePort.Entities.Exceptions;
    using SecurePort.Entities.Models;
    using SecurePort.Entities.Models.Json;
    using SecurePort.Services.Interfaces;
    using SecurePort.Services.Repository;
    using SecurePort.Web.Controllers;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Validation;
    using System.Linq;
    using System.Web.Mvc;
    #endregion

    public class MotivosController : BaseController
    {

        public MotivosController(IServiciosMotivos serviciosMotivos, 
                                  MotivosRepository repoMotivos,
                                  TrazasRepository repoTrazas, 
                                  ILogger log)
            : base(serviciosMotivos, 
                   repoMotivos, 
                   repoTrazas, 
                   log)
        {
             
        }

        #region Buscador
        /// <summary>
        /// Permite buscar los diferentes Motivos de declaración de protección maritima
        /// </summary>
        public ActionResult BuscadorMotivos([DataSourceRequest]DataSourceRequest request)
        {
            try
            {
                IEnumerable<MotivosViewModel> result = this._serviciosMotivos.GetAllMotivos().OrderBy(x=> x.Motivo);
                    
                return this.Json(result.ToDataSourceResult(request));
            }
            catch (Exception ex)
            {
                this.log.PublishException(new SecureportException(ex));
                throw;
            }
        }
        #endregion

        #region action
        /// <summary>
        /// Retorna una lista de todos los Bienes.
        /// </summary>
        public ActionResult ListadoMotivos()
        {
            try
            {
                ViewBag.URLDelete = "/Motivos/EliminarMotivos";
                return this.PartialView("ListadoMotivos", this.UsuarioFrontalSession);
            }
            catch (Exception ex)
            {
                this.log.PublishException(new SecureportException(ex));
                throw;
            }
        }
        #endregion

        #region CRUD

        [HttpPost]
        public ActionResult Create()
        {

            Motivos motivos = new Motivos();

            ViewBag.disabled = true;

            ViewBag.Mensaje = "Alta Motivos de Declaracón Marítima";

            ViewBag.action = General.AltaGeneral.ToDescription();

            motivos.Id = 1;

            return this.PartialView("~/Views/Motivos/_PartialMotivos.cshtml", motivos);
        }
        /// <summary>
        /// Visualizar los datos de los Motivos
        /// </summary>
        [HttpPost]
        public ActionResult VisualizarMotivos(int id)
        {
            try
            {

                ViewBag.disabled = true;

                ViewBag.Mensaje = "Modificar Motivos de Declaracón Marítima";

                ViewBag.action = General.EditGeneral.ToDescription();

                Motivos motivos = new Motivos();

                this._serviciosMotivos.GetAllMotivos().Where(x => x.Id == id).ToList().ForEach(
                   p =>
                   {
                       motivos.Id = p.Id;
                       motivos.Motivo = p.Motivo;
                       motivos.Descripcion = p.Descripcion;
                       motivos.es_activo = p.es_activo;

                   });

                return this.PartialView("~/Views/Motivos/_PartialMotivos.cshtml", motivos);
            }
            catch (Exception ex)
            {
                this.log.PublishException(new SecureportException(ex));

                return this.Json(new { result = false, Message = ex.Message });
            }
        }
        /// <summary>
        /// Alta-Editar Bienes.
        /// </summary>
        [HttpPost]
        public ActionResult AltaEditarMotivos(MotivosJson motivosJson)
        {
            try
            {
                IEnumerable<Motivos> result = null;

                switch (motivosJson.action)
                {
                         
                    case "Alta":
                        result = _serviciosMotivos.ListMotivos(motivosJson.Motivo);
                        bool resultado = false;
                        foreach (Motivos motivo in result){
                           if(motivo.es_activo == true)
                                resultado = true;                        
                        }
                        if (!resultado)
                        {
                            if(result.Count() ==0)
                                AltaMotivo(motivosJson);
                            else
                                return this.Json(new { result = 2, Message = "Error al dar de alta el Motivo de la Declaración Marítima, ya existe otro Motivo de Declaración activo " + motivosJson.Motivo + "¿Desea continuar? " });                            
                        } else {
                            this.trazas(89, (int)this.UsuarioFrontalSession.Usuario.id, "Error al dar de alta el Motivo de Declaración Marítima, ya existe otro Motivo de Declaración activo " + motivosJson.Motivo);
                            return this.Json(new { result = 1, Message = "Error al dar de alta el Motivo de Declaración Marítima, ya existe otro Motivo de Declaración activo " + motivosJson.Motivo });
                        }
                        break;

                    default:
                         bool CambioEstado = _serviciosMotivos.ListMotivos(motivosJson.Motivo, motivosJson.Id, (motivosJson.activo == "1" ? true : false));
                        if (CambioEstado)
                        {
                            EditarMotivo(motivosJson);
                        }
                        else
                        {
                            result = _serviciosMotivos.ListMotivos(motivosJson.Motivo, motivosJson.Id);
                            if (result.Count() == 0)
                            {
                                EditarMotivo(motivosJson);
                            }
                            else
                            {
                                IEnumerable<Motivos> motivo = result.Where(x => x.Id != motivosJson.Id);                                
                                if (motivo == null)
                                {
                                    EditarMotivo(motivosJson);
                                }
                                else{
                                    Motivos ActMotivo = motivo.FirstOrDefault();
                                    if (ActMotivo.Id == motivosJson.Id)
                                    {
                                        return this.Json(new { result = 3, Message = "Ya existe un Motivo de Declaración Marítima " + motivosJson.Motivo + " desactivado ¿desea continuar? " });
                                    }
                                    else
                                    {
                                        if (!ActMotivo.es_activo)
                                            return this.Json(new { result = 3, Message = "Ya existe un Motivo de Declaración Marítima " + motivosJson.Motivo + " desactivado ¿desea continuar? " });
                                        else
                                        {
                                            this.trazas(91, (int)this.UsuarioFrontalSession.Usuario.id, "Error al modificar el Motivo de Declaración Marítima, ya existe otro Motivo de Declaración activo " + motivosJson.Motivo);
                                            return this.Json(new { result = 1, Message = "Error al modificar el Motivo de Declaración Marítima, ya existe otro Motivo de Declaración activo  " + motivosJson.Motivo });
                                        }
                                    }
                                }                               

                            }
                        }
                        break;
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
        /// Borrar Motivos de Declaracion.
        /// </summary>
        [HttpPost]
        public ActionResult EliminarMotivos(int Id)
        {
            try
            {

                this.repoMotivos.Delete(this.repoMotivos.Single(x => x.Id == Id));

                this.trazas(90, (int)this.UsuarioFrontalSession.Usuario.id, "Se ha realizado la eliminación de un registro en SPMAE_Motivos_Declara_Maritima con ID" + "(" + Id.ToString() + ")");

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
        public ActionResult AltaMotivo(MotivosJson motivosJson)
        {
            try
            {
                Motivos motivos = new Motivos();

                motivos.ID_Usu_alta = this.UsuarioFrontalSession.Usuario.id;

                motivos.es_activo = true;

                motivos.fech_activo = DateTime.Now;

                motivos.Fech_Alta = DateTime.Now;

                motivos.Descripcion = motivosJson.Descripcion;

                motivos.Motivo = motivosJson.Motivo;

                this.repoMotivos.Create(motivos);

                var Id = this.db.Motivos.OrderByDescending(u => u.Id).FirstOrDefault().Id;

                this.trazas(89, (int)this.UsuarioFrontalSession.Usuario.id, "Se ha dado de alta un registro en SPMAE_Motivos_Declara_Maritima con ID" + "(" + Id.ToString() + ")");

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
        public ActionResult EditarMotivo(MotivosJson motivosJson)
        {
            try
            {
                Motivos motivo = this.repoMotivos.Single(x => x.Id == motivosJson.Id);

                motivo.Motivo = motivosJson.Motivo;

                motivo.Descripcion = motivosJson.Descripcion;

                motivo.es_activo = (motivosJson.activo == "1" ? true : false);

                this.repoMotivos.Update(motivo);

                this.trazas(91, (int)this.UsuarioFrontalSession.Usuario.id, "Se ha realizado la modificación de un registro en SPMAE_Motivos_Declara_Maritima con ID" + "(" + motivosJson.Id + ")");

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
    }
}