
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

    public class BienesController : BaseController
    {

        public BienesController(IServiciosBienes serviciosBienes, 
                                BienesRepository repoBienes,
                                IServiciosTipoInstalacion _serviciosTipoInstalacion,
                                TrazasRepository repoTrazas, 
                                ILogger log)
            : base(serviciosBienes, repoBienes, _serviciosTipoInstalacion,repoTrazas, log)
        {
             
        }

        #region Buscador
        /// <summary>
        /// Permite buscar los diferentes Bienes
        /// </summary>
        public ActionResult BuscadorBienes([DataSourceRequest]DataSourceRequest request)
        {
            try
            {
                IEnumerable<BienesViewModel> result = this._serviciosBienes.GetAllBienes();
                    
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
        public ActionResult ListadoBienes()
        {
            try
            {

                return this.PartialView("ListadoBienes", this.UsuarioFrontalSession);
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

            Bienes bienes = new Bienes();

            this.ComboTipos_Instalacion();

            this.comboBienes(null);

            ViewBag.disabled = true;

            ViewBag.display = "display:none";

            ViewBag.Mensaje = "Alta Tipo de Bienes";

            ViewBag.action = General.AltaGeneral.ToDescription();

            bienes.Id = 1;

            return this.PartialView("~/Views/Bienes/_PartialBienes.cshtml", bienes);
        }
        /// <summary>
        /// Visualizar los datos de los Bienes
        /// </summary>
        [HttpPost]
        public ActionResult VisualizarBienes(int id)
        {
            try
            {

                IEnumerable<BienesViewModel> listbienes = this._serviciosBienes.GetAllBienes().Where(x=> x.Id==id);

                this.ComboTipos_Instalacion();

                this.comboBienes(null);

                ViewBag.disabled = true;

                ViewBag.display = "display:block";

                ViewBag.Mensaje = "Modificar tipo de Bienes";

                ViewBag.action = General.EditGeneral.ToDescription();

                Bienes bienes = new Bienes();

                listbienes.ToList().ForEach(
                   p =>
                   {
                       bienes.Id            = p.Id;
                       bienes.Descripcion   = p.Descripcion;
                       bienes.id_Tipo_IIPP  = p.id_Tipo_IIPP;
                       bienes.id_Bien_Padre = p.id_Bien_Padre;
                       bienes.es_activo     = p.es_activo;

                   });

                return this.PartialView("~/Views/Bienes/_PartialBienes.cshtml", bienes);
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
        public ActionResult AltaEditarBienes(BienesJson bienesJson)
        {
            try
            {
                IEnumerable<Bienes> result = null;

               
                switch (bienesJson.action)
                {
                         
                    case "Alta":
                         result = _serviciosBienes.ListBienes(bienesJson.Descripcion);
                        bool resultado = false;
                        foreach (Bienes bien in result){
                           if(bien.es_activo == true)
                                resultado = true;                        
                        }
                        if (!resultado)
                        {
                            if(result.Count() ==0)
                                AltaBienes(bienesJson);
                            else
                                return this.Json(new { result = 2, Message = "Ya existe un bien " + bienesJson.Descripcion + " desactivo ¿Desea continuar? " });                            
                        } else {
                            this.trazas(25, (int)this.UsuarioFrontalSession.Usuario.id, "Error al dar de alta el bien, ya existe otro bien activo " + bienesJson.Descripcion );
                            return this.Json(new { result = 1, Message = "Error al dar de alta el Bien, ya existe otro bien activo " + bienesJson.Descripcion });
                        }
                        break;

                    default:
                            result = _serviciosBienes.ListBienes(bienesJson.Descripcion, bienesJson.Id);
                            if (result.Count() == 0)
                            {
                                EditarBienes(bienesJson);
                            }
                            else
                            {
                                IEnumerable<Bienes> bien = result.Where(x=> x.Id!=bienesJson.Id);
                                if (bien == null)
                                {
                                    EditarBienes(bienesJson);
                                }
                                else {
                                    Bienes ActBien = bien.FirstOrDefault();
                                    if (ActBien.Id == bienesJson.Id)
                                    {
                                        return this.Json(new { result = 3, Message = "Ya existe un bien  " + bienesJson.Descripcion + " desactivo ¿Desea continuar? " });
                                        //EditarBienes(bienesJson);
                                    }
                                    else
                                    {
                                        if (!ActBien.es_activo)
                                            return this.Json(new { result = 3, Message = "Ya existe un bien " + bienesJson.Descripcion + " desactivo ¿Desea continuar? " });
                                        else
                                        {
                                            this.trazas(27, (int)this.UsuarioFrontalSession.Usuario.id, "Error al modificar el Bien, ya existe otro bien activo " + bienesJson.Descripcion + "(" + bienesJson.Id + ")");
                                            return this.Json(new { result = 1, Message = "Error al modificar el Bien, ya existe otro bien activo " + bienesJson.Descripcion });
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
        /// Borrar Bienes.
        /// </summary>
        [HttpPost]
        public ActionResult EliminarBienes(int Id)
        {
            try
            {
                this.repoBienes.Delete(this.repoBienes.Single(x => x.Id == Id));

                this.trazas(26, (int)this.UsuarioFrontalSession.Usuario.id, "Se ha realizado la eliminación de un registro en SPMAE_Bienes con ID" + "("+Id.ToString()+")");

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
        public ActionResult AltaBienes(BienesJson bienesJson)
        {
            try
            {
                Bienes bienes = new Bienes();

                bienes.ID_Usu_alta = this.UsuarioFrontalSession.Usuario.id;

                bienes.es_activo = true;

                bienes.fech_activo = DateTime.Now;

                bienes.Fech_Alta = DateTime.Now;

                bienes.Descripcion = bienesJson.Descripcion;

                bienes.id_Tipo_IIPP = bienesJson.id_Tipo_IIPP;

                bienes.id_Bien_Padre = bienesJson.id_Bien_Padre == 0? null : bienesJson.id_Bien_Padre ;

                this.repoBienes.Create(bienes);

                var Id = this.db.Bienes.OrderByDescending(u => u.Id).FirstOrDefault().Id;

                this.trazas(25, (int)this.UsuarioFrontalSession.Usuario.id, "Se ha dado de alta un registro en SPMAE_Bienes con ID" + "(" + Id.ToString() + ")");

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
        public ActionResult EditarBienes(BienesJson bienesJson)
        {
            try
            {
                Bienes bien = this.repoBienes.Single(x => x.Id == bienesJson.Id);

                bien.Descripcion = bienesJson.Descripcion;

                bien.id_Tipo_IIPP = bienesJson.id_Tipo_IIPP;

                bien.id_Bien_Padre = (bienesJson.id_Bien_Padre == 0? null : bienesJson.id_Bien_Padre);

                bien.es_activo = (bienesJson.activo == "1" ? true : false);

                this.repoBienes.Update(bien);

                this.trazas(27, (int)this.UsuarioFrontalSession.Usuario.id, "Se ha realizado la modificación de un registro en SPMAE_Bienes con ID" + "(" + bienesJson.Id +")");

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
