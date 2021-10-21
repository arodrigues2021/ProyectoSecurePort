
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

    public class PaisesController : BaseController
    {
        public PaisesController(IServiciosPaisesProvincias serviciosPaisesProvincias, 
                                PaisesRepository repoPaises, 
                                ProvinciasRepository repoProvincias,
                                CiudadesRepository repoCiudades,
                                TrazasRepository repoTrazas, ILogger log) 
            : base(serviciosPaisesProvincias, repoPaises, repoProvincias, repoCiudades, repoTrazas, log)
        {

        }

    #region Pais

       #region action
            /// <summary>
            /// Retorna una lista de todos los paises en el grid.
            /// </summary>
            public ActionResult ListadoPaises()
            {
                try
                {

                   return this.PartialView("ListadoPaises", this.UsuarioFrontalSession);
                }
                catch (Exception ex)
                {
                    this.log.PublishException(new SecureportException(ex));
                    throw;
                }
            }

            /// <summary>
        /// Visualizar datos de Pais.
        /// </summary>
        [HttpPost]
        public ActionResult VisualizarPais(int? id)
        {
            try
            {
                this.idPais = (id == null ? this.idPais : (int)id);

                Paises paises = this.repoPaises.Single(x => x.Id == this.idPais);

                ViewBag.disabled = false;

                return this.PartialView("~/Views/Paises/PlantillaEdit.cshtml", paises);
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
        public ActionResult DatosPaises()
        {
            ViewBag.disabled = true;

            ViewBag.display = "display:block";

            return this.PartialView("~/Views/Paises/Create.cshtml");
        }

         [HttpPost]
        public ActionResult VisualizarPaises(int? id)
        {
             try
            {
                this.idPais = (id == null? this.idPais: (int)id);

                ViewBag.disabled = true;

                ViewBag.display = "display:block";
                 
                Paises paises = this.repoPaises.Single(x => x.Id == this.idPais);

                if (paises == null)
                {
                    return this.View("~/Views/ErrorPages/ErrorPage400.cshtml");
                }
               
                 return this.PartialView("~/Views/Paises/_PartialPaises.cshtml", paises);
            }
             catch (Exception ex)
             {
                 this.log.PublishException(new SecureportException(ex));

                 return this.Json(new { result = false, Message = ex.Message });
             }
        }

        /// <summary>
        /// Alta de Pais.
        /// </summary>
        [HttpPost]
        public ActionResult AltaPais(PaisJson paisJson)
        {
            try
            {
                IEnumerable<Paises> result = null;
                result = _serviciosPaisesProvincias.ListPaises(paisJson.Codigo);
                bool resultado = false;
                foreach (Paises pais in result)
                {
                    if (pais.es_activo == true)
                        resultado = true;
                }
                if (!resultado)
                {

                    if (result.Count() == 0)
                    {
                        PaisAlta(paisJson);
                    }
                    else
                       return this.Json(new { result = 2, Message = "Ya existe un país " + paisJson.Codigo.ToUpper() + " desactivo ¿Desea continuar? " });                            
                } else {
                    this.trazas(29, (int)this.UsuarioFrontalSession.Usuario.id, "Error al dar de alta el País, ya existe otro país activo " + paisJson.Codigo.ToUpper() );
                    return this.Json(new { result = 1, Message = "Error al dar de alta el país, ya existe otro país activo " + paisJson.Codigo.ToUpper() });
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
        /// Editar Pais.
        /// </summary>
        [HttpPost]
        public ActionResult EditarPais(PaisJson paisJson)
        {
            try
            {
                IEnumerable<Paises> result = null;
                result = _serviciosPaisesProvincias.ListPaises(paisJson.Codigo, paisJson.ID_pais);
                if (result.Count() == 0)
                {
                    PaisEditar(paisJson);           
                }
                else
                {
                    IEnumerable<Paises> pais = result.Where(x => x.Id != paisJson.ID_pais);
                    if (pais == null)
                    {
                        PaisEditar(paisJson);
                    }
                    else
                    {
                        Paises ActPais = pais.FirstOrDefault();
                        if (ActPais.Id == paisJson.ID_pais)
                        {
                            return this.Json(new { result = 3, Message = "Ya existe un país " + paisJson.Codigo.ToUpper() + " desactivo ¿Desea continuar? " });                            
                        }
                        else
                        {
                            if (!ActPais.es_activo)
                                return this.Json(new { result = 3, Message = "Ya existe un país " + paisJson.Codigo.ToUpper() + " desactivo ¿Desea continuar? " });
                            else
                            {
                                this.trazas(31, (int)this.UsuarioFrontalSession.Usuario.id, "Error al modificar el país, ya existe otro país activo" + paisJson.Codigo.ToUpper() + "(" + paisJson.ID_pais + ")");
                                return this.Json(new { result = 1, Message = "Error al modificar el país, ya existe otro país activo " + paisJson.Codigo.ToUpper() });
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
        /// Borrar fisicamente el registro del usuario.
        /// </summary>
        [HttpPost]
        public ActionResult EliminarPais(int Id)
        {
            try
            {
                this.repoPaises.Delete(this.repoPaises.Single(x => x.Id == Id));

                this.trazas(30, (int)this.UsuarioFrontalSession.Usuario.id, "Se ha realizado la eliminacion de un registro en SPMAE_Paises con ID" + "(" + Id.ToString() + ")");
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
        public ActionResult PaisAlta(PaisJson paisJson)
        {
            try
            { 
                
                Paises paises = new Paises();
                
                paises.id_usu_alta = this.UsuarioFrontalSession.Usuario.id;
                
                paises.es_activo = true;
                
                paises.fech_activo = DateTime.Now;
                
                paises.fech_alta = DateTime.Now;
                
                paises.nombre = paisJson.Nombre;
                
                paises.cod_pais = paisJson.Codigo.ToUpper();
                
                this.repoPaises.Create(paises);

                var Id = this.db.Paises.OrderByDescending(x => x.Id).FirstOrDefault().Id;

                this.trazas(29, (int)this.UsuarioFrontalSession.Usuario.id, "Se ha dado de alta un registro en SPMAE_Paises con código con ID" + "(" + Id.ToString() + ")");
                                                
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
        public ActionResult PaisEditar(PaisJson paisJson)
        {
            try
            {
                Paises paises = this.repoPaises.Single(x => x.Id == paisJson.ID_pais);

                paises.nombre = paisJson.Nombre;

                paises.cod_pais = paisJson.Codigo.ToUpper();

                paises.es_activo = (paisJson.activo == "1" ? true : false);

                this.repoPaises.Update(paises);

                this.trazas(31, (int)this.UsuarioFrontalSession.Usuario.id, "Se ha realizado la modificaion de un registro en SPMAE_Paises con ID"  + "(" + paises.Id + ")");

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
        public ActionResult BuscadorPais([DataSourceRequest]DataSourceRequest request)
        {
            try
            {
                return this.Json(this._serviciosPaisesProvincias.GetAllPaises().ToDataSourceResult(request));
            }
            catch (Exception ex)
            {
                this.log.PublishException(new SecureportException(ex));
                throw;
            }
        }

        #endregion


    #endregion

    #region Ciudades

        #region action
        /// <summary>
            /// Retorna una lista de todos los paises en el grid.
            /// </summary>
            public ActionResult Listadociudades()
            {
                try
                {
                    return this.PartialView("ListadoCiudades", this.UsuarioFrontalSession);
                }
                catch (Exception ex)
                {
                    this.log.PublishException(new SecureportException(ex));
                    throw;
                }
            }

            /// <summary>
        /// Visualizar datos de Ciudad.
        /// </summary>
        [HttpPost]
        public ActionResult VisualizarCiudad(int? id)
        {
            try
            {
                this.idCiudad = (id == null ? this.idCiudad : (int)id);

                Ciudades ciudades = this.repoCiudades.Single(x => x.id == this.idCiudad);

                ViewBag.disabled = false;

                return this.PartialView("~/Views/Paises/_PartialCiudades.cshtml", ciudades);
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
        public ActionResult DatosCiudades()
        {
            this.ComboProvincias(null);

            this.ComboIslas(null);

            ViewBag.disabled = true;

            ViewBag.display = "display:block";

            return this.PartialView("~/Views/Paises/CreateCiudad.cshtml");
        }

         [HttpPost]
        public ActionResult VisualizarCiudades(int? id)
        {
            try
            {
                this.idCiudad = (id == null ? this.idCiudad : (int)id);

                ViewBag.disabled = true;

                ViewBag.display = "display:block";

                Ciudades ciudades = this.repoCiudades.Single(x => x.id == this.idCiudad);

                if (ciudades == null)
                {
                    return this.View("~/Views/ErrorPages/ErrorPage400.cshtml");                
                }
                ViewBag.Id_Provincia = ciudades.Id_provincia.ToString();
                
                this.ComboProvincias(null);

                this.ComboIslas(null);

                return this.PartialView("~/Views/Paises/_PartialCiudades.cshtml", ciudades);
            }
            catch (Exception ex)
            {
                this.log.PublishException(new SecureportException(ex));

                return this.Json(new { result = false, Message = ex.Message });
            }
        }

        /// <summary>
        /// Alta de Ciudad.
        /// </summary>
        [HttpPost]
        public ActionResult AltaCiudad(PaisJson paisJson)
        {
            try
            {

                IEnumerable<Ciudades> result = null;
                result = _serviciosPaisesProvincias.ListCiudades(paisJson.Codigo, paisJson.ID_Provincia);
                bool resultado = false;
                foreach (Ciudades ciudad in result)
                {
                    if (ciudad.es_activo == true)
                        resultado = true;
                }
                if (!resultado)                
                {
                       if(result.Count() ==0)
                       {
                            CiudadAlta(paisJson);                           
                       }
                       else
                           return this.Json(new { result = 2, Message = "Ya existe una ciudad " + paisJson.Codigo.ToUpper() + " desactivada ¿Desea continuar? " });                            
                  } else {
                      this.trazas(37, (int)this.UsuarioFrontalSession.Usuario.id, "Error al dar de alta la Ciudad, ya existe otra Ciudad activa con ID " + paisJson.Codigo.ToUpper() + " y Nombre " + paisJson.Nombre );
                      return this.Json(new { result = 1, Message = "Error al dar de alta la Ciudad, ya existe otra Ciudad activa " + paisJson.Codigo.ToUpper() });
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
        /// Editar Ciudad.
        /// </summary>
        [HttpPost]
        public ActionResult EditarCiudad(PaisJson paisJson)
        {
            try
            {
                IEnumerable<Ciudades> result = null;
                result = _serviciosPaisesProvincias.ListCiudades(paisJson.Codigo, paisJson.ID_pais, paisJson.ID_Provincia);
                if (result.Count() == 0)
                {
                    CiudadEditar(paisJson);
                }
                 else
                {
                    IEnumerable<Ciudades> ciudad = result.Where(x => x.id != paisJson.ID_pais);
                    if (ciudad == null)
                    {
                        CiudadEditar(paisJson);
                    }
                    else
                    {
                        Ciudades ActCiudad = ciudad.FirstOrDefault();
                        if (ActCiudad.id == paisJson.ID_pais)
                        {
                            return this.Json(new { result = 3, Message = "Ya existe una ciudad " + paisJson.Codigo.ToUpper() + " desactivada ¿Desea continuar? " });                            
                        }
                        else
                        {
                            if (!ActCiudad.es_activo)
                                return this.Json(new { result = 3, Message = "Ya existe una ciudad " + paisJson.Codigo.ToUpper() + " desactivada ¿Desea continuar? " });
                            else
                            {
                                this.trazas(37, (int)this.UsuarioFrontalSession.Usuario.id, "Error al modificar la Ciudad, ya existe otra ciudad activa" + paisJson.Codigo.ToUpper() + " y nombre " + paisJson.Nombre + "(" + paisJson.ID_pais + ")");
                                return this.Json(new { result = 1, Message = "Error al modificar la ciudad, ya existe otra ciudad activa " + paisJson.Codigo.ToUpper() });
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
        /// Borrar fisicamente el registro del usuario.
        /// </summary>
        [HttpPost]
        public ActionResult EliminarCiudad(int Id)
        {
            try
            {
                Ciudades ciudades = this.repoCiudades.Single(x => x.id == Id);

                this.repoCiudades.Delete(ciudades);

                this.trazas(38, (int)this.UsuarioFrontalSession.Usuario.id, "Se ha realizado la eliminacion de un registro en SPMAE_Ciudades con ID" + "(" + ciudades.id + ") y nombre " + ciudades.nombre);
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

                return this.Json(new { result = false, Message = "Instrucción DELETE en conflicto con la restricción REFERENCE." });
            }

            return this.Json(new { result = true });
        }

         [HttpPost]
        public ActionResult CiudadAlta(PaisJson paisJson)
        {
            try
            {
                Ciudades ciudades = new Ciudades();

                ciudades.id_usu_alta = this.UsuarioFrontalSession.Usuario.id;

                ciudades.es_activo = true;

                ciudades.fech_activo = DateTime.Now;

                ciudades.fech_alta = DateTime.Now;

                ciudades.nombre = paisJson.Nombre;

                ciudades.codigo = paisJson.Codigo;

                ciudades.Id_provincia = paisJson.ID_Provincia;

                ciudades.Id_isla = paisJson.ID_Isla == 0 ? null : paisJson.ID_Isla; 

                this.repoCiudades.Create(ciudades);

                this.trazas(37, (int)this.UsuarioFrontalSession.Usuario.id, "Se ha dado de alta un registro en SPMAE_Ciudades con ID" + "(" + ciudades.id + ")");

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
        public ActionResult CiudadEditar(PaisJson paisJson)
        {
            try
            {
                Ciudades ciudades = this.repoCiudades.Single(x => x.id == paisJson.ID_pais);

                ciudades.nombre = paisJson.Nombre;

                ciudades.codigo = paisJson.Codigo;

                ciudades.Id_provincia = paisJson.ID_Provincia;

                ciudades.es_activo = (paisJson.activo == "1" ? true : false);

                ciudades.Id_isla = paisJson.ID_Isla == 0 ? null : paisJson.ID_Isla;

                this.repoCiudades.Update(ciudades);

                this.trazas(39, (int)this.UsuarioFrontalSession.Usuario.id, "Se ha realizado la modificaion de un registro en SPMAE_Ciudades con ID" + "(" + ciudades.id + ")");

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
        public ActionResult BuscadorCiudad([DataSourceRequest]DataSourceRequest request)
        {
            try
            {
                IEnumerable<CiudadesViewModel> result =
                    (from t1 in this._serviciosPaisesProvincias.ListCiudades()
                     join b2 in this.db.Provincias.ToList() on t1.Id_provincia equals b2.id
                     join c3 in this.db.Islas.ToList() on t1.id_isla equals c3.id into TEmp
                    from te in TEmp.DefaultIfEmpty()                    
                     select new CiudadesViewModel
                     {
                         id = t1.id,
                         nombre = t1.nombre,
                         codigo = t1.codigo,
                         provincia = b2.nombre,
                         isla = (te == null) ? "" : te.nombre,
                         activado = t1.activado
                     });

                return this.Json(result.ToDataSourceResult(request));
            }
            catch (Exception ex)
            {
                this.log.PublishException(new SecureportException(ex));
                throw;
            }
        }

        #endregion

  #endregion
        
    }
}