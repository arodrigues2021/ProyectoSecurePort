using System.Web.Mvc;

namespace SecurePort.Web.Controllers
{
    #region Using
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.ModelConfiguration;
    using System.Data.Entity.Validation;
    using System.Linq;
    using System.Web.Mvc;
    using Kendo.Mvc.Extensions;
    using Kendo.Mvc.UI;
    using SecurePort.Entities.Enums;
    using SecurePort.Entities.Exceptions;
    using SecurePort.Entities.Models;
    using SecurePort.Entities.Models.Json;
    using SecurePort.Services.Interfaces;
    using SecurePort.Services.Repository;
    #endregion

    public class CapitaniasController : BaseController
    {
        public CapitaniasController(IServiciosPaisesProvincias serviciosPaisesProvincias,
                                      CapitaniasRepository repoCapitanias,
                                      TrazasRepository repoTrazas, ILogger log)
            : base(serviciosPaisesProvincias,
                  repoCapitanias,
                  repoTrazas,
                  log)
        {
              
        }

        #region action
        /// <summary>
        /// Retorna una lista de todos las Capitanias Maritimas.
        /// </summary>
        public ActionResult ListadoCapitanias()
        {
            try
            {
                return this.PartialView("ListadoCapitanias", this.UsuarioFrontalSession);
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
            Capitanias capitanias = new Capitanias();

            ViewBag.disabled = true;

            ViewBag.Mensaje = "Alta Capitanías Marítimas";
            
            ViewBag.action = General.AltaGeneral.ToDescription();

            capitanias.Id = 1;

            return this.PartialView("~/Views/Capitanias/_PartialCapitanias.cshtml", capitanias);
        }
        /// <summary>
        /// Visualizar los datos de las Capitanías Marítimas
        /// </summary>
        [HttpPost]
        public ActionResult VisualizarCapitanias(int? id)
        {
            try
            {

                IEnumerable<ListCapitanias> listcapitanias  = this._serviciosPaisesProvincias.ListCapitanias().Where(x=> x.Id==id);

                ViewBag.disabled = true;

                ViewBag.Mensaje = "Modificar Capitanías Marítimas";

                ViewBag.action = General.EditGeneral.ToDescription();

                Capitanias capitanias = new Capitanias();

                listcapitanias.ToList().ForEach(
                   p =>
                   {
                       capitanias.Id = p.Id;
                       capitanias.nombre = p.nombre;
                       capitanias.es_activo = p.es_activo;

                   });

                return this.PartialView("~/Views/Capitanias/_PartialCapitanias.cshtml", capitanias);
            }
            catch (Exception ex)
            {
                this.log.PublishException(new SecureportException(ex));

                return this.Json(new { result = false, Message = ex.Message });
            }
        }
        /// <summary>
        /// Alta-Editar Capitanías Marítimas.
        /// </summary>
        [HttpPost]
        public ActionResult AltaEditarCapitanias(CapitaniaJson capitaniaJson)
        {
            try
            {
                bool result;
                switch (capitaniaJson.action)
                {
                    case "Alta":
                        result = _serviciosPaisesProvincias.ListCapitanias(capitaniaJson.Nombre);
                        if (!result)
                        {
                            Capitanias capitanias = new Capitanias();

                            capitanias.id_usu_alta = this.UsuarioFrontalSession.Usuario.id;

                            capitanias.es_activo = capitaniaJson.Activo == "1";

                            capitanias.fech_activo = DateTime.Now;

                            capitanias.fech_alta = DateTime.Now;

                            capitanias.nombre = capitaniaJson.Nombre;

                            this.repoCapitanias.Create(capitanias);
                            
                            var Id = this.db.Capitanias.OrderByDescending(u => u.Id).FirstOrDefault().Id;

                            this.trazas(201, (int)this.UsuarioFrontalSession.Usuario.id, "Se ha dado de alta un registro en SPMAE_Capitanias_Maritimas con ID " + "("+ Id.ToString() + ")");

                        } else {
                            return this.Json(new { result = false, Message = "La capitania con nombre  " + capitaniaJson.Nombre + " ya esta dado de alta" });
                        }
                        break;

                    default:
                        result = _serviciosPaisesProvincias.ListCapitanias(capitaniaJson.Id, capitaniaJson.Nombre);
                        if (!result)
                        {
                            Capitanias capitania = this.repoCapitanias.Single(x => x.Id == capitaniaJson.Id);

                            capitania.nombre = capitaniaJson.Nombre;

                            capitania.es_activo = capitaniaJson.Activo == "1";

                            this.repoCapitanias.Update(capitania);

                            this.trazas(202, (int)this.UsuarioFrontalSession.Usuario.id, "Se ha realizado la modificación de un registro en SPMAE_Capitanias_Maritimasc con ID" + "(" + capitaniaJson.Id.ToString() + ")");
                        }
                        else
                        {
                            return this.Json(new { result = false, Message = "La capitania con nombre  " + capitaniaJson.Nombre + " ya esta dado de alta" });
                        }
                        break;
                }

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
        /// Borrar Capitanías Marítimas.
        /// </summary>
        [HttpPost]
        public ActionResult EliminarCapitanias(int Id)
        {
            try
            {

                this.repoCapitanias.Delete(this.repoCapitanias.Single(x => x.Id == Id));

                this.trazas(203, (int)this.UsuarioFrontalSession.Usuario.id, "Se ha realizado la eliminación de un registro en SPMAE_Capitanias_Maritimas con ID" + "(" + Id.ToString() + ")");

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
        #endregion

        #region Buscador
        /// <summary>
        /// Permite buscar por las diferentes Capitanias
        /// </summary>
        public ActionResult BuscadorCapitanias([DataSourceRequest]DataSourceRequest request)
        {
            try
            {
                IEnumerable<ListCapitanias> result = this._serviciosPaisesProvincias.ListCapitanias().OrderBy(x => x.nombre);
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