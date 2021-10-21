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

    public class ProvinciasController : BaseController
    {
        public ProvinciasController(IServiciosPaisesProvincias serviciosPaisesProvincias,
                                      ProvinciasRepository repoProvincias,
                                      TrazasRepository repoTrazas, 
                                      ILogger log)
            : base(serviciosPaisesProvincias,
                  repoProvincias,
                  repoTrazas,
                  log)
        {
              
        }

        #region action
        /// <summary>
        /// Retorna una lista de todos las comunidades autonomas en el grid.
        /// </summary>
        public ActionResult ListadoProvincias()
        {
            try
            {
                return this.PartialView("ListadoProvincias", this.UsuarioFrontalSession);
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
            this.comboComunidades(null);

            this.ComboPaises();

            Provincias provincias = new Provincias();

            ViewBag.disabled = true;

            ViewBag.Mensaje = "Alta Provincias";
            
            ViewBag.action = General.AltaGeneral.ToDescription();

            provincias.id = 1;

            return this.PartialView("~/Views/Provincias/_PartialProvincias.cshtml",provincias);
        }
        /// <summary>
        /// Visualizar los datos de la comunidad autónoma
        /// </summary>
        [HttpPost]
        public ActionResult VisualizarProvincias(int? id)
        {
            try
            {

                IEnumerable<ListadoProvincias> listprovincias = this._serviciosPaisesProvincias.ListProvincias().Where(x=> x.Id==id);

                this.comboComunidades(null);

                this.ComboPaises();

                ViewBag.disabled = true;

                ViewBag.Mensaje = "Modificar Provincias";

                ViewBag.action = General.EditGeneral.ToDescription();

                Provincias provincias = new Provincias();

                listprovincias.ToList().ForEach(
                   p =>
                   {
                       provincias.nombre    = p.nombre;
                       provincias.codigo    = p.codigo;
                       provincias.ID_ComAut = p.ID_ComAut;
                       provincias.es_activo = p.Activo;
                       ViewBag.Id_Pais      = p.Id_pais;
                   }); 

                return this.PartialView("~/Views/Provincias/_PartialProvincias.cshtml", provincias);
            }
            catch (Exception ex)
            {
                this.log.PublishException(new SecureportException(ex));

                return this.Json(new { result = false, Message = ex.Message });
            }
        }
        /// <summary>
        /// Alta-Editar Amenazas.
        /// </summary>
        [HttpPost]
        public ActionResult AltaEditarProvincias(ProvinciasJson provinciasJson)
        {
            try
            {
                IEnumerable<Provincias> result = null;

                switch (provinciasJson.action)
                {

                    case "Alta":
                        result = _serviciosPaisesProvincias.ListProvincias(provinciasJson.Nombre);
                        bool resultado = false;
                        foreach (Provincias provincia in result)
                        {
                            if (provincia.es_activo == true)
                                resultado = true;
                        }
                        if (!resultado)
                        {
                            if (result.Count() == 0)
                                AltaProvincias(provinciasJson);
                            else
                                return this.Json(new { result = 2, Message = "Error al dar de alta la provincia, ya existe otra provincia activa " + provinciasJson.Nombre + "¿Desea continuar? " });
                        }
                        else
                        {
                            this.trazas(33, (int)this.UsuarioFrontalSession.Usuario.id, "Error al dar de alta la provincia, ya existe otra provincia activa " + provinciasJson.Nombre);
                            return this.Json(new { result = 1, Message = "Error al dar de alta la provincia, ya existe otra provincia activa " + provinciasJson.Nombre });
                        }
                        break;

                    default:
                        bool CambioEstado = _serviciosPaisesProvincias.ListProvincias(provinciasJson.Codigo, provinciasJson.Id, (provinciasJson.activo == "1"));
                        if (CambioEstado)
                        {
                            EditarProvincias(provinciasJson);
                        }
                        else
                        {
                            result = _serviciosPaisesProvincias.ListProvincias(provinciasJson.Codigo, provinciasJson.Id);
                            if (result.Count() == 0)
                            {
                                EditarProvincias(provinciasJson);
                            }
                            else
                            {
                                IEnumerable<Provincias> provincia = result.Where(x => x.id != provinciasJson.Id);
                                if (provincia == null)
                                {
                                    EditarProvincias(provinciasJson);
                                }
                                else
                                {
                                    Provincias Actprovincia = provincia.FirstOrDefault();
                                    if (Actprovincia.id == provinciasJson.Id)
                                    {
                                        return this.Json(new { result = 3, Message = "Ya existe una provincia  " + provinciasJson.Nombre + " desactivo ¿Desea continuar? "});
                                    }
                                    else
                                    {
                                        if (!Actprovincia.es_activo)
                                            return this.Json(new { result = 3, Message = "Ya existe una provincia  " + provinciasJson.Nombre + " desactivo ¿Desea continuar? " });
                                        else
                                        {
                                            this.trazas(35, (int)this.UsuarioFrontalSession.Usuario.id, "Error al modificar la provincia, ya existe otra provincia activa " + provinciasJson.Nombre);
                                            return this.Json(new { result = 1, Message = "Error al modificar la provincia, ya existe otra provincia activa  " + provinciasJson.Nombre });
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

        [HttpPost]
        public ActionResult EditarProvincias(ProvinciasJson provinciasJson)
        {
            try
            {
                Provincias prov = this.repoProvincias.Single(x => x.id == provinciasJson.Id);

                prov.codigo = provinciasJson.Codigo.ToString();

                prov.nombre = provinciasJson.Nombre;

                prov.ID_ComAut = provinciasJson.ID_ComAut;

                prov.es_activo = provinciasJson.activo == "1";

                this.repoProvincias.Update(prov);

                this.trazas(35, (int)this.UsuarioFrontalSession.Usuario.id, "Se ha realizado la modificación de un registro en SPMAE_Provincias con ID" + "(" + prov.id + ")");
 
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
        /// Alta-Editar Provincias.
        /// </summary>
        [HttpPost]
        public ActionResult AltaProvincias(ProvinciasJson provinciasJson)
        {
            try
            {

                Provincias provincias = new Provincias();

                provincias.id_usu_alta = this.UsuarioFrontalSession.Usuario.id;

                provincias.es_activo   = true;

                provincias.fech_activo = DateTime.Now;

                provincias.fech_alta    = DateTime.Now;

                provincias.codigo       = provinciasJson.Codigo.ToString();

                provincias.nombre       = provinciasJson.Nombre;

                provincias.ID_ComAut    = provinciasJson.ID_ComAut;

                this.repoProvincias.Create(provincias);

                var Id = this.db.Provincias.OrderByDescending(x => x.id).FirstOrDefault().id;

                this.trazas(33, (int)this.UsuarioFrontalSession.Usuario.id, "Se ha dado de alta un registro en SPMAE_Provincias con ID" + "(" + Id.ToString() + ")");

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
        /// Borrar Provincias.
        /// </summary>
        [HttpPost]
        public ActionResult EliminarProvincias(int Id)
        {
            try
            {
                this.repoProvincias.Delete(this.repoProvincias.Single(x => x.id == Id));

                this.trazas(34, (int)this.UsuarioFrontalSession.Usuario.id, "Se ha realizado la modificación de un registro en SPMAE_Provincias con ID " +"("+Id.ToString()+")");

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
        /// Permite buscar por las diferentes Provincias
        /// </summary>
        public ActionResult BuscadorProvincias([DataSourceRequest]DataSourceRequest request)
        {
            try
            {
                IEnumerable<ListadoProvincias> result = from a in this._serviciosPaisesProvincias.ListProvincias() orderby a.Id,a.nombre select a;
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