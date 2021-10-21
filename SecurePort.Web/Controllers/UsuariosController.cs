namespace SecurePort.Web.Controllers
{
    #region Using

    using System.Threading.Tasks;

    using Kendo.Mvc.Extensions;
    using Kendo.Mvc.UI;
    using SecurePort.Encriptador;
    using SecurePort.Entities.Enums;
    using SecurePort.Entities.Exceptions;
    using SecurePort.Entities.Models;
    using SecurePort.Entities.Models.Json;
    using SecurePort.Services.Interfaces;
    using SecurePort.Services.Repository;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.ModelConfiguration;
    using System.Data.Entity.Validation;
    using System.Linq;
    using System.Web.Mvc;
    using System.Web;
    using WebGrease.Css.Extensions;

    #endregion


    public class UsuariosController : BaseController
    {
        public UsuariosController(
                    GruposRepository repoGrupos,
                    IServiciosUsuarios serviciosUsuarios,
                    IServiciosPuertos servicioPuertos,
                    IServiciosGrupos serviciosGrupos,
                    UsuariosRepository repoUsuarios,
                    IServiciosPuertos serviciosPuertos,
                    DependenciaUsuarioRepository repoDependenciaUsuario,
                    DocumentosUsuarioRepository repoDocumentosUsuario,
                    IServiciosDocumentos servicioDocumentos,
                    IServiciosOrganismo serviciosOrganismos,
                    TrazasRepository repoTrazas,
                    ILogger log)
            : base(repoGrupos,
                   serviciosUsuarios,
                   servicioPuertos,
                   serviciosGrupos,
                   repoUsuarios,
                   serviciosPuertos,
                   repoDependenciaUsuario,
                   repoDocumentosUsuario,
                   servicioDocumentos,
                   serviciosOrganismos,
                   repoTrazas,
                   log)
        {
        }

        #region Buscador

        public ActionResult BuscadorUsuarios([DataSourceRequest]DataSourceRequest request)
        {
            //Default Activados
            IEnumerable<UsuariosCategoriasGrupos> result = this._serviciosUsuarios.GetAll(this.UsuarioFrontalSession.ListDependenciasDepenUsuarios, this.UsuarioFrontalSession.Usuario.categoria);

            return Json(result.ToDataSourceResult(request));
        }

        public ActionResult PlantillaVisualizarAcciones()
        {
            this.listaAcciones = new List<AccionesViewModel>();

            IEnumerable<AccionesViewModel> accionesViewModel = this._serviciosUsuarios.GetAcciones().ToList();

            AccionesViewModel acciones0 = new AccionesViewModel()
            {
                Id = 0,
                Name = "Seleccionar Opción"
            };
            this.listaAcciones.Add(acciones0);

            this.UsuarioFrontalSession.ListGruposPerfilesPermisos.Where(x => x.id_grupo_accion == (int)GrupoAccion.Usuario).OrderBy(x => x.NombrePermiso).ForEach(
            p =>
            {
                accionesViewModel.ForEach(
                 c =>
                 {


                     if (p.NombrePermiso == c.Name)
                     {
                         if (c.Name == Usuario.MODIFICAR_DEPEN_USUARIO.ToDescription())
                         {
                             AccionesViewModel acciones = new AccionesViewModel()
                             {
                                 Id = 1,
                                 Name = "Asignar Dependencia"
                             };

                             this.listaAcciones.Add(acciones);
                         }
                         else if (c.Name == Usuario.RESETEAR_PASS_USUARIO.ToDescription())
                         {
                             AccionesViewModel acciones01 = new AccionesViewModel()
                             {
                                 Id = 2,
                                 Name = "Reiniciar Contraseña"
                             };

                             this.listaAcciones.Add(acciones01);

                         }
                     }
                 });
            });

            return Json(this.listaAcciones, JsonRequestBehavior.AllowGet);

        }

        public ActionResult Acciones()
        {

            this.listaAcciones = new List<AccionesViewModel>();

            IEnumerable<AccionesViewModel> accionesViewModel = this._serviciosUsuarios.GetAcciones().ToList();

            AccionesViewModel acciones0 = new AccionesViewModel()
            {
                Id = 0,
                Name = "Seleccionar Opción"
            };

            this.listaAcciones.Add(acciones0);

            this.UsuarioFrontalSession.ListGruposPerfilesPermisos.Where(x => x.id_grupo_accion == (int)GrupoAccion.Usuario).OrderBy(x => x.NombrePermiso).ForEach(
            p =>
            {
                accionesViewModel.ForEach(
                 c =>
                 {
                     if (p.NombrePermiso == c.Name)
                     {
                         if (c.Name == Usuario.ACTIVAR_USUARIO.ToDescription())
                         {
                             AccionesViewModel acciones = new AccionesViewModel()
                             {
                                 Id = 1,
                                 Name = "Activar/Desactivar Usuario"
                             };

                             this.listaAcciones.Add(acciones);
                         }
                     }
                 });
            });

            return Json(this.listaAcciones, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AsociadosEdit(bool ToolBar)
        {
            this.UsuarioFrontalSession.valor = ToolBar;

            return PartialView("~/Views/Usuarios/Documentos/_PartialAsociadosEdit.cshtml", this.UsuarioFrontalSession);

        }

        #endregion

        #region Action CRUD

        /// <summary>
        /// Permite validar la contraseña y usuario logueado.
        /// </summary>
        /// <param name="CambiarLoginJson"></param>
        /// <returns>
        /// Verifica que la contraseña es valida.
        /// </returns>
        [HttpPost]
        public ActionResult ValidarUsuario(CambiarLoginJson CambiarLoginJson)
        {
            try
            {
                Usuarios usuarios = this.repoUsuarios.Single(x => x.id == this.UsuarioFrontalSession.Usuario.id);

                encriptador clave = new encriptador();

                if (clave.DesEncriptacion(usuarios.password) != CambiarLoginJson.Passwordactual)
                {
                    return this.Json(new { result = false, Message = "La contraseña actual es errónea." });

                }
                return this.Json(new { result = true });
            }
            catch (Exception ex)
            {
                this.log.PublishException(new SecureportException(ex));

                return this.Json(new { result = false, Message = ex.Message });
            }
        }

        /// <summary>
        /// Permite validar el correo y usuario logueado.
        /// </summary>
        /// <param name="CorreoJson"></param>
        /// <returns>
        /// Verifica que si el correo es valido.
        /// </returns>
        [HttpPost]
        public ActionResult ValidarUsuarioCorreo(CorreoJson CorreoJson)
        {
            try
            {

                Usuarios validCorreo = this.repoUsuarios.Single(x => x.email == CorreoJson.Correo);

                if (validCorreo != null)
                {
                    return this.Json(new { result = false, Message = "Verifique el Correo ya existe." });

                }

                return this.Json(new { result = true });
            }
            catch (Exception ex)
            {
                this.log.PublishException(new SecureportException(ex));

                return this.Json(new { result = false, Message = ex.Message });
            }
        }

        /// <summary>
        ///     Permite reiniciar la contraseña de un usuario.
        /// </summary>
        [HttpPost]
        public ActionResult ReiniciarContrasena()
        {

            string nuevoPassword = this.CreatePassword(10);

            try
            {
                Usuarios usuarios = this.repoUsuarios.Single(x => x.id == this.IdUsuario);

                encriptador clave = new encriptador();

                usuarios.password = clave.Encriptacion(nuevoPassword);

                usuarios.Fech_Password = DateTime.Now;

                this.repoUsuarios.Update(usuarios);

                string texto = "SecurePort - Reiniciar Contraseña";

                if (this.EnviarCorreo(usuarios.email, nuevoPassword, texto))
                {
                    this.trazas(3, (int)this.IdUsuario, "Se ha reiniciado la contraseña en SPCON_Usuarios " + usuarios.login);
                }
                else
                {
                    this.trazas(3, (int)this.IdUsuario, "Error al enviar correo electrónico." + usuarios.login);

                    return this.Json(new { result = false, Message = "Error al enviar correo electrónico." });
                }

                return this.Json(new { result = true });
            }
            catch (Exception ex)
            {
                this.log.PublishException(new SecureportException(ex));

                return this.Json(new { result = false, Message = ex.Message });
            }

        }

        /// <summary>
        ///     Permite validar la contraseña y usuario logueado.
        /// </summary>
        [HttpPost]
        public ActionResult ValidarCorreoUsuario(CorreoJson CorreoJson)
        {
            try
            {
                Usuarios usuarios = this.repoUsuarios.Single(x => x.id == this.UsuarioFrontalSession.Usuario.id);

                encriptador clave = new encriptador();

                if (usuarios.login != CorreoJson.CorreoActual)
                {
                    return this.Json(new { result = false, Message = "La correo actual es erróneo." });

                }
                return this.Json(new { result = true });
            }
            catch (Exception ex)
            {
                this.log.PublishException(new SecureportException(ex));

                return this.Json(new { result = false, Message = ex.Message });
            }
        }

        /// <summary>
        ///     Permite guardar cambios en un documento asociado.
        /// </summary>
        [HttpPost]
        public ActionResult GuardarCambios(DocumentoJson DocumentoJson)
        {
            try
            {

                this.ComboTipos_Documento();

                Docs_Usuario docsUsuario = this.repoDocumentosUsuario.Single(x => x.id == DocumentoJson.id);

                docsUsuario.descripcion = DocumentoJson.descripcion;

                docsUsuario.id_tipo_doc = DocumentoJson.tipodocumento;

                this.repoDocumentosUsuario.Update(docsUsuario);

                this.trazas(13, (int)this.UsuarioFrontalSession.Usuario.id, "Se ha dado de modificado un registro en SPCON_Depen_Usuario con Id" + "(" + this.UsuarioFrontalSession.Usuario.id.ToString() + ")");

                return this.Json(new { result = true });

            }
            catch (Exception ex)
            {
                this.log.PublishException(new SecureportException(ex));

                return this.Json(new { result = false, Message = ex.Message });
            }

        }

        /// <summary>
        ///     Permite eliminar un documento asociado.
        /// </summary>
        [HttpPost]
        public ActionResult visualizarDocumento(int? id)
        {
            try
            {

                Documento documento = new Documento();

                Docs_Usuario docsUsuario = this.repoDocumentosUsuario.Single(x => x.id == id);

                string doc = docsUsuario.documento;

                string nombreRuta = ConfigurationManager.AppSettings["Ficheros"];

                this.UsuarioFrontalSession.Path = nombreRuta + "/" + doc;

                return PartialView("~/Views/Shared/PartialDoc.cshtml", this.UsuarioFrontalSession);

            }
            catch (Exception ex)
            {
                this.log.PublishException(new SecureportException(ex));

                return this.Json(new { result = false, Message = ex.Message });
            }

        }

        public ActionResult DownloadFile()
        {
            string filename = "File.pdf";
            string filepath = AppDomain.CurrentDomain.BaseDirectory + "/Path/To/File/" + filename;
            byte[] filedata = System.IO.File.ReadAllBytes(filepath);
            string contentType = MimeMapping.GetMimeMapping(filepath);

            var cd = new System.Net.Mime.ContentDisposition
            {
                FileName = filename,
                Inline = true,
            };

            Response.AppendHeader("Content-Disposition", cd.ToString());

            return File(filedata, contentType);
        }

        /// <summary>
        ///     Permite eliminar un documento asociado.
        /// </summary>
        [HttpPost]
        public ActionResult EditarDocumento(int? id)
        {
            try
            {

                this.ComboTipos_Documento();

                Documento documento = new Documento();

                Usuarios usuarios = this.repoUsuarios.Single(x => x.id == this.IdUsuario);

                Docs_Usuario docsUsuario = this.repoDocumentosUsuario.Single(x => x.id == id);

                documento.Nombre = usuarios.nombre;

                documento.Apellidos = usuarios.apellidos;

                documento.descripcion = docsUsuario.descripcion;

                documento.tipodocumento = docsUsuario.id_tipo_doc;

                documento.documento = docsUsuario.documento.Substring((docsUsuario.documento.IndexOf("_")) + 1);

                return this.PartialView("~/Views/Usuarios/_PartialEditDocumentos.cshtml", documento);

            }
            catch (Exception ex)
            {
                this.log.PublishException(new SecureportException(ex));

                return this.Json(new { result = false, Message = ex.Message });
            }

        }

        /// <summary>
        ///     Permite eliminar un documento asociado.
        /// </summary>
        [HttpPost]
        public ActionResult EliminarDocumentos(int? id)
        {
            try
            {

                Docs_Usuario docsUsuario = this.repoDocumentosUsuario.Single(x => x.id == id);

                this.repoDocumentosUsuario.Delete(docsUsuario);

                this.trazas(12, (int)this.UsuarioFrontalSession.Usuario.id, "Se ha dado de borrado un registro en SPCON_Depen_Usuario con Id" + "(" + this.UsuarioFrontalSession.Usuario.id.ToString() + ")");

                BorrarFichero(ConfigurationManager.AppSettings["RutaSecurePorDoc"].ToString() + "/" + docsUsuario.documento);

                return this.Json(new { result = true });

            }
            catch (Exception ex)
            {
                this.log.PublishException(new SecureportException(ex));

                return this.Json(new { result = false, Message = ex.Message });
            }

        }

        /// <summary>
        ///     Permite el Alta del usuario.
        /// </summary>
        [HttpPost]
        public ActionResult create()
        {
            try
            {
                this.ComboCategorias();

                this.ComboGrupos();

                return this.PartialView("~/Views/Usuarios/Create.cshtml");

            }
            catch (Exception ex)
            {
                this.log.PublishException(new SecureportException(ex));

                return this.Json(new { result = false, Message = ex.Message });
            }

        }

        /// <summary>
        ///     Permite el Alta de un nuevo usuario en la apliación.
        /// </summary>
        [HttpPost]
        public ActionResult AltaUsuarios(UsuariosJson usuariosJson)
        {
            try
            {

                if (usuariosJson.Login != usuariosJson.Email)
                {
                    return this.Json(new { result = false, Message = "Verificar el Correo Electrónico no coincide." });
                }

                if (!this.ComprobarFormatoEmail(usuariosJson.Email))
                {
                    return this.Json(new { result = false, Message = "Formato del Correo Electrónico es erróneo." });
                }

                if (!this.ComprobarFormatoEmail(usuariosJson.Login))
                {
                    return this.Json(new { result = false, Message = "Formato del Correo Electrónico es erróneo." });
                }

                bool existe = this._serviciosUsuarios.GetUsuarios(usuariosJson.Login);

                if (existe == false)
                {
                    bool opip = false;

                    bool opp = false;

                    DateTime? fecha;

                    fecha = null;

                    if (usuariosJson.opp == General.OPP.ToDescription())
                    {
                        opp = true;
                    }
                    else if (usuariosJson.opp == General.OPIP.ToDescription() && usuariosJson.fech_exp_OPIP != null)
                    {
                        fecha = Convert.ToDateTime(usuariosJson.fech_exp_OPIP);
                        opip = true;
                    }

                    encriptador clave = new encriptador();

                    string nuevoPassword = this.CreatePassword(10);

                    int? idGrupo = null;

                    if (usuariosJson.grupo != 0)
                    {
                        idGrupo = usuariosJson.grupo;
                    }
                    Usuarios usuarios = new Usuarios()
                    {
                        nombre      = usuariosJson.Nombre,
                        apellidos   = usuariosJson.Apellidos,
                        password    = clave.Encriptacion(nuevoPassword),
                        login       = usuariosJson.Email,
                        email       = usuariosJson.Email,
                        fech_alta   = DateTime.Now,
                        observaciones   = this.CambiarTexto(usuariosJson.Observaciones),
                        id_usu_alta     = this.UsuarioFrontalSession.Usuario.id,
                        es_activo       = true,
                        fech_activo     = DateTime.Now,
                        es_opip         = opip,
                        es_opp          = opp,
                        aviso_legal     = false,
                        categoria       = usuariosJson.categoria,
                        fech_exp_opip   = fecha,
                        Fech_Password   = DateTime.Now,
                        id_grupo        = idGrupo,
                        empresa         = usuariosJson.empresa
                    };

                    repoUsuarios.Create(usuarios);

                    if (this.EnviarCorreo(usuariosJson.Email, nuevoPassword, "SecurePort - Alta de Usuarios."))
                    {
                        this.trazas(3, (int)this.UsuarioFrontalSession.Usuario.id, "Se ha dado de alta un registro en SPCON_Usuarios con ID" + "(" + usuarios.login + ")");

                        this.IdUsuario = this.repoUsuarios.Single(x => x.login == usuariosJson.Email).id;

                        string nombre = usuariosJson.Nombre + " " + usuariosJson.Apellidos;

                        if (this.EnviarCorreoPuertos(usuariosJson.Email, nombre) == false)
                        {
                            return this.Json(new { result = false, Message = "Se ha realizado el Alta de Usuario.(Error al enviar correo electrónico.)" });
                        }

                    }
                    else
                    {
                        return this.Json(new { result = false, Message = "Se ha realizado el Alta de Usuario.(Error al enviar correo electrónico.)" });
                    }

                }
                else
                {
                    return this.Json(new { result = false, Message = "Verifique el Correo Electrónico ya existe." });
                }

                return this.Json(new { result = true, Message = string.Empty });
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
        /// Permite editar los datos de un usuario ya registrado.
        /// </summary>
        [HttpPost]
        public ActionResult Edit(int? id)
        {
            try
            {
                Usuarios usuarios = new Usuarios();

                this.ComboCategorias();

                this.ComboGrupos();

                if (id == null && this.DatosUsuario.Id > 0)
                {
                    usuarios.id = this.DatosUsuario.Id;
                    usuarios.apellidos = this.DatosUsuario.Apellidos;
                    usuarios.nombre = this.DatosUsuario.Nombre;
                    usuarios.categoria = this.DatosUsuario.categoria;
                    usuarios.id_grupo = this.DatosUsuario.id_grupo;
                    usuarios.email = this.DatosUsuario.Email;
                    usuarios.login = this.DatosUsuario.Email;
                    usuarios.password = this.DatosUsuario.password_;
                    usuarios.observaciones = this.CambiarTexto(this.DatosUsuario.Observaciones);
                    usuarios.fech_exp_opip = DateTime.Parse(this.DatosUsuario.fech_exp_OPIP);
                }
                else
                {
                    this.IdUsuario = (id == null ? this.IdUsuario : (int)id);
                    usuarios = this.repoUsuarios.Single(x => x.id == this.IdUsuario);
                    this.ViewBag.navegador = this.Browser;
                    encriptador clave = new encriptador();
                    usuarios.password = clave.DesEncriptacion(usuarios.password);
                }
                this.ViewBag.Nombre = "Modificar Usuario";
                this.ViewBag.valor = this.repoDependenciaUsuario.GetAll().Where(x => x.id_Usuario == this.IdUsuario).ToList().Any();
                this.UsuarioFrontalSession.EditUsuario = usuarios;
                return this.PartialView("~/Views/Usuarios/PlantillaEdit.cshtml", UsuarioFrontalSession);

            }
            catch (Exception ex)
            {
                this.log.PublishException(new SecureportException(ex));

                return this.Json(new { result = false, Message = ex.Message });
            }
        }

        /// <summary>
        /// Permite editar los datos de un usuario ya registrado.
        /// </summary>
        [HttpPost]
        public ActionResult DesahacerDatosUsuario()
        {
            try
            {
                Usuarios usuarios = this.repoUsuarios.Single(x => x.id == this.IdUsuario);

                this.ComboCategorias();

                this.ComboGrupos();

                this.ViewBag.navegador = this.Browser;

                encriptador clave = new encriptador();

                usuarios.password = clave.DesEncriptacion(usuarios.password);

                return this.PartialView("~/Views/Usuarios/PlantillaEdit.cshtml", usuarios);
            }
            catch (Exception ex)
            {
                this.log.PublishException(new SecureportException(ex));

                return this.Json(new { result = false, Message = ex.Message });
            }
        }

        /// <summary>
        /// Permite seleccionar los puertos asociados al usuario.
        /// </summary>
        [HttpPost]
        public ActionResult SeleccionarPuertos()
        {
            try
            {
                return this.PartialView("~/Views/Usuarios/_PartialPuertos.cshtml");
            }
            catch (Exception ex)
            {
                this.log.PublishException(new SecureportException(ex));

                return this.Json(new { result = false, Message = ex.Message });
            }
        }

        /// <summary>
        /// Permite almacenar los datos del usuario.
        /// </summary>
        [HttpPost]
        public ActionResult DatosUsuarios(UsuariosEditJson UsuariosEditJson)
        {
            try
            {
                this.DatosUsuario = UsuariosEditJson;

                return this.Json(new { result = true });
            }
            catch (Exception ex)
            {
                this.log.PublishException(new SecureportException(ex));

                return this.Json(new { result = false, Message = ex.Message });
            }
        }

        /// <summary>
        /// Permite seleccionar los organismos asociados al usuario.
        /// </summary>
        [HttpPost]
        public ActionResult SeleccionarOrganismo()
        {
            try
            {
                return this.PartialView("~/Views/Usuarios/_PartialOrganismo.cshtml");
            }
            catch (Exception ex)
            {
                this.log.PublishException(new SecureportException(ex));

                return this.Json(new { result = false, Message = ex.Message });
            }
        }

        /// <summary>
        /// Permite seleccionar las instalaciones asociados al usuario.
        /// </summary>
        [HttpPost]
        public ActionResult SeleccionarInstalacion()
        {
            try
            {
                return this.PartialView("~/Views/Usuarios/_PartialInstalacion.cshtml");
            }
            catch (Exception ex)
            {
                this.log.PublishException(new SecureportException(ex));

                return this.Json(new { result = false, Message = ex.Message });
            }
        }

        /// <summary>
        /// Permite actualizar los datos del usuario en el grid.
        /// </summary>
        [HttpPost]
        public ActionResult RefrescarUsuarios(int? id)
        {

            try
            {
                if (id == null)
                {
                    return this.Json(new { result = false, Message = "Id NULL" });
                }

                this.UsuarioFrontalSession.fichero = string.Empty;

                return this.PartialView("~/Views/Usuarios/ListUsers.cshtml", this.UsuarioFrontalSession);

            }
            catch (Exception ex)
            {
                this.log.PublishException(new SecureportException(ex));

                return this.Json(new { result = false, Message = ex.Message });
            }
        }

        /// <summary>
        /// Salvamos los datos editados del usuario.
        /// </summary>
        public JsonResult SaveUsuario(UsuariosEditJson usuariosEditJson)
        {
            try
            {
                if (usuariosEditJson.Id == 0)
                {
                    return this.Json(new { result = false, Message = "Id NULL" });
                }

                Usuarios usuarios = this.repoUsuarios.Single(x => x.id == usuariosEditJson.Id);

                if (usuarios.email != usuariosEditJson.Email)
                {
                    bool existe = this._serviciosUsuarios.GetEmail(usuariosEditJson.Email);

                    if (existe)
                    {

                        return this.Json(new { result = false, Message = "El Correo Electrónico ya existe.El registro no se ha podido modificar" });
                    }

                    usuarios.email = usuariosEditJson.Email;
                    usuarios.login = usuariosEditJson.Email;
                }

                bool opip = false;
                bool opp = false;
                DateTime? fecha;
                fecha = null;
                if (usuariosEditJson.fech_exp_OPIP != null)
                {
                    fecha = Convert.ToDateTime(usuariosEditJson.fech_exp_OPIP);
                    opip = true;
                }
                else
                {
                    opp = true;
                }

                usuarios.nombre = usuariosEditJson.Nombre;
                usuarios.apellidos = usuariosEditJson.Apellidos;
                usuarios.observaciones = this.CambiarTexto(usuariosEditJson.Observaciones);
                usuarios.categoria = usuariosEditJson.categoria;
                usuarios.id_grupo = usuariosEditJson.id_grupo;
                usuarios.fech_exp_opip = fecha;
                usuarios.es_opip = opip;
                usuarios.es_opp = opp;
                usuarios.empresa = usuariosEditJson.Empresa;

                this.repoUsuarios.Update(usuarios);

                if (this.UsuarioFrontalSession.Usuario.id == usuariosEditJson.Id)
                {
                    IList<Usuarios> lista = this.repoUsuarios.Filter(x => x.login == usuarios.login && x.es_activo == true);

                    Usuarios usuario = new Usuarios()
                    {
                        id = lista[0].id,
                        categoria = lista[0].categoria,
                        id_grupo = lista[0].id_grupo,
                        nombre = lista[0].nombre,
                        apellidos = lista[0].apellidos,
                        login = lista[0].email,
                        email = lista[0].email
                    };

                    this.UsuarioFrontalSession.Usuario = usuario;
                    this.IdUsuario = usuariosEditJson.Id;
                }

                this.trazas(4, (int)this.UsuarioFrontalSession.Usuario.id, "Se ha dado modificado un registro en SPCON_Usuarios con ID" + "(" + usuarios.id.ToString() + ")");
                return this.Json(new { result = true });
            }
            catch (DbUpdateException ex)
            {
                this.log.PublishException(new SecureportExceptionDbUpdate(ex));

                return this.Json(new { result = false, Message = ex.Message });
            }
            catch (ModelValidationException ex)
            {
                this.log.PublishException(new SecureportExceptionModelValidation(ex));

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
        /// Permite actualizar los datos de un usuario.
        /// </summary>
        [HttpPost]
        public ActionResult EditUsuarios(UsuariosEditJson usuariosEditJson)
        {
            try
            {
                return this.SaveUsuario(usuariosEditJson);
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
        public ActionResult EliminarUsuario(int? id)
        {
            try
            {
                if (id == null)
                {
                    return this.Json(new { result = false, Message = "Id NULL" });
                }
                Usuarios usuarios = this.repoUsuarios.Single(x => x.id == id);

                if (usuarios == null)
                {
                    return this.Json(new { result = false, Message = "El usuario no existe." });
                }
                else if (this.UsuarioFrontalSession.Usuario.id == id)
                {
                    return this.Json(new { result = false, Message = "No puede eliminar el usuario logueado." });
                }
                //Eliminamos el usuario por Store Procedure.
                var resultado = this._serviciosUsuarios.RemoveUsuarioById(id);
                if (resultado == "1")
                {
                    this.trazas(5, (int)this.UsuarioFrontalSession.Usuario.id, "Se ha eliminado un registro en SPCON_Usuarios con ID" + "(" + usuarios.id.ToString() + ")");

                    return this.Json(new { result = true, Message = "Se ha eliminado el Usuario." });
                }

                return this.Json(new { result = false, Message = MessageDelete(new Exception(resultado)) });

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
        /// Limpiar las colecciones de dependencias
        /// </summary>
        private void Actualizardependencias(int? id, int? item)
        {
            this.listaPuertosSessionAsignadas = this._serviciosUsuarios.GetDependenciaPuertos(id, item).ToList();
        }

        /// <summary>
        /// Borrar fisicamente la dependencia del usuario.  
        /// </summary>
        [HttpPost]
        public ActionResult EliminarDependencias(int? id, int? item)
        {
            try
            {
                if (id == null || item == null)
                {
                    return this.Json(new { result = false, Message = "Id NULL" });
                }

                if (this.repoDependenciaUsuario.GetAll().Where(x => x.id_Usuario == id).ToList().Any())
                {
                    this.repoDependenciaUsuario.GetAll().Where(x => x.id_Usuario == id).ToList().ForEach(
                        p =>
                        {
                            this.repoDependenciaUsuario.Delete(p);

                        });

                    this.Actualizardependencias(id, item);
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

                return this.Json(new { result = false, Message = "Instrucción DELETE en conflicto con la restricción REFERENCE." });
            }

            return this.Json(new { result = true });
        }

        /// <summary>
        /// Borrado lógico del usuario.
        /// </summary>
        [HttpPost]
        public ActionResult BajaConfirmed(int? id)
        {
            try
            {
                if (id == null)
                {
                    return this.Json(new { result = false, Message = "Id NULL" });
                }

                string accion = string.Empty;

                Usuarios usuarios = this.repoUsuarios.Single(x => x.id == id);

                if (usuarios == null)
                {
                    return this.Json(new { result = false, Message = "El usuario no existe." });
                }
                else if (this.UsuarioFrontalSession.Usuario.id == id)
                {
                    return this.Json(new { result = false, Message = "No puede desactivar el usuario logueado." });
                }

                if (usuarios.es_activo == true)
                {
                    usuarios.es_activo = false;

                    accion = "Desactivado usuario";
                }
                else
                {
                    usuarios.es_activo = true;

                    accion = "Activado usuario";
                }

                usuarios.fech_activo = DateTime.Now;

                this.repoUsuarios.Update(usuarios);

                this.trazas(8, (int)this.UsuarioFrontalSession.Usuario.id, "Se ha " + accion + " en SPCON_Usuarios " + usuarios.login + "(" + usuarios.id.ToString() + ")");
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

        #endregion

        #region Accion
        /// <summary>
        /// Validar Categoria.
        /// </summary>
        public ActionResult ValidarCategoriaInstalacion()
        {
            try
            {

                bool result = true;

                var message = "La categoría asociada con su usuario no tiene permisos para ejecutar la acción seleccionada";

                switch (this.UsuarioFrontalSession.Usuario.categoria)
                {

                    case (int)EnumCategoria.Instalacion:

                        result = false;

                        break;

                }

                return this.Json(new { result = result, Message = result ? string.Empty : message });
            }
            catch (Exception ex)
            {
                this.log.PublishException(new SecureportException(ex));

                throw;
            }
        }
        /// <summary>
        /// Validar Categoria.
        /// </summary>
        public ActionResult ValidarCategoriaPuertos()
        {
            try
            {

                bool result = true;

                var message = "La categoría asociada con su usuario no tiene permisos para ejecutar la acción seleccionada";

                switch (this.UsuarioFrontalSession.Usuario.categoria)
                {
                    case (int)EnumCategoria.Puerto:

                        result = false;

                        break;

                    case (int)EnumCategoria.Instalacion:

                        result = false;

                        break;

                }

                return this.Json(new { result = result, Message = result ? string.Empty : message });
            }
            catch (Exception ex)
            {
                this.log.PublishException(new SecureportException(ex));

                throw;
            }
        }

        /// <summary>
        /// Validar Categoria.
        /// </summary>
        public ActionResult ValidarCategoriaOrganismos()
        {
            try
            {

                bool result = true;

                var message = "La categoría asociada con su usuario no tiene permisos para ejecutar la acción seleccionada";

                switch (this.UsuarioFrontalSession.Usuario.categoria)
                {
                    case (int)EnumCategoria.Puerto:

                        result = false;

                        break;

                    case (int)EnumCategoria.OrganismoGestionPortuaria:

                        result = false;

                        break;

                    case (int)EnumCategoria.Instalacion:

                        result = false;

                        break;

                }

                return this.Json(new { result = result, Message = result ? string.Empty : message });
            }
            catch (Exception ex)
            {
                this.log.PublishException(new SecureportException(ex));

                throw;
            }
        }

        /// <summary>
        /// Cambio de Correo electrónico.
        /// </summary>
        public ActionResult CambiarCorreo()
        {
            try
            {
                return this.Json(new { result = true });
            }
            catch (Exception ex)
            {
                this.log.PublishException(new SecureportException(ex));

                throw;
            }
        }

        /// <summary>
        /// Retorna una lista de todos los usuarios en el grid.
        /// </summary>
        public ActionResult ListadoUsuarios()
        {
            try
            {
                this.UsuarioFrontalSession.fichero = string.Empty;

                this.ViewBag.Navegador = this.Browser;

                this.ViewBag.IdUsuario = 0;

                this.ComboCategorias();

                this.LimpiarColecciones();

                return this.PartialView("ListadoUsuarios", this.UsuarioFrontalSession);
            }
            catch (Exception ex)
            {
                this.log.PublishException(new SecureportException(ex));
                throw;
            }
        }

        /// <summary>
        /// Permite actualizar los puertos asociados a una instalación.
        /// </summary>
        [HttpPost]
        public ActionResult ActualizarPuertos(int? id)
        {
            try
            {

                this.idPuerto = (id != null) ? (int)id : this.idPuerto;

                this.listaInstalacionSession = this._serviciosPuertos.GetPuertosInstalacion(this.idPuerto).ToList();

                return this.PartialView("~/Views/Usuarios/Documentos/_PartialInstalacionDisponible.cshtml");

            }
            catch (Exception ex)
            {
                this.log.PublishException(new SecureportException(ex));

                return this.Json(new { result = false, Message = ex.Message });
            }

        }

        /// <summary>
        /// Permite actualizar los puertos asociados a una instalación.
        /// </summary>
        [HttpPost]
        public ActionResult ActualizarInstalacionAsignada()
        {
            try
            {

                return this.PartialView("~/Views/Usuarios/Documentos/_PartialInstalacionesAsociados.cshtml");

            }
            catch (Exception ex)
            {
                this.log.PublishException(new SecureportException(ex));

                return this.Json(new { result = false, Message = ex.Message });
            }

        }

        /// <summary>
        /// Permite actualizar los puertos asociados a una instalación.
        /// </summary>
        [HttpPost]
        public ActionResult ActualizarInstalacion(int id)
        {
            try
            {

                this.listaInstalacionSession.ToList().ForEach(
                    p =>
                    {
                        if (p.Id == id)
                        {
                            this.listaInstalacionSession.Remove(p);
                            this.listaInstalacionAsignadas.Add(p);
                        }
                    });
                return this.PartialView("~/Views/Usuarios/Documentos/_PartialInstalacionDisponible.cshtml");

            }
            catch (Exception ex)
            {
                this.log.PublishException(new SecureportException(ex));

                return this.Json(new { result = false, Message = ex.Message });
            }

        }

        /// <summary>
        /// Permite Quitar las instalaciones asociadas aun puerto.
        /// </summary>
        [HttpPost]
        public ActionResult QuitarInstalacion(int id)
        {
            try
            {
                this.listaInstalacionAsignadas.ToList().ForEach(
                    p =>
                    {
                        if (p.Id == id)
                        {
                            this.listaInstalacionSession.ToList().ForEach(
                                t =>
                                {
                                    if (t.Id == id)
                                    {
                                        this.listaInstalacionSession.Remove(p);
                                    }
                                    else
                                    {
                                        this.listaInstalacionSession.Add(p);
                                    }
                                });

                            this.listaInstalacionAsignadas.Remove(p);
                        }
                    });

                this.listaInstalacionSession = this._serviciosPuertos.GetPuertosInstalacion((int)this.idPuerto).ToList();

                return this.PartialView("~/Views/Usuarios/Documentos/_PartialInstalacionDisponible.cshtml");
            }
            catch (Exception ex)
            {
                this.log.PublishException(new SecureportException(ex));

                return this.Json(new { result = false, Message = ex.Message });
            }

        }

        /// <summary>
        /// Permite deshacer las dependencias.
        /// </summary>
        [HttpPost]
        public ActionResult DesahacerDependencias()
        {
            try
            {

                Usuarios usuarios = this.repoUsuarios.Single(x => x.id == this.IdUsuario);

                this.ComboCategorias();

                this.ComboGrupos();

                this.LimpiarColecciones();

                return this.PartialView("~/Views/Usuarios/PlantillaVisualizar.cshtml", usuarios);

            }
            catch (Exception ex)
            {
                this.log.PublishException(new SecureportException(ex));

                return this.Json(new { result = false, Message = ex.Message });
            }
        }

        /// <summary>
        /// Permite deshacer los cambios.
        /// </summary>
        [HttpPost]
        public ActionResult DeshacerCambios(int id)
        {
            try
            {

                this.ComboTipos_Documento();

                Documento documento = new Documento();

                Usuarios usuarios = this.repoUsuarios.Single(x => x.id == this.IdUsuario);

                Docs_Usuario docsUsuario = this.repoDocumentosUsuario.Single(x => x.id == id);

                documento.Nombre = usuarios.nombre;

                documento.Apellidos = usuarios.apellidos;

                documento.descripcion = docsUsuario.descripcion;

                documento.tipodocumento = docsUsuario.id_tipo_doc;

                return this.PartialView("~/Views/Usuarios/_PartialEditDocumentos.cshtml", documento);

            }
            catch (Exception ex)
            {
                this.log.PublishException(new SecureportException(ex));

                return this.Json(new { result = false, Message = ex.Message });
            }
        }

        /// <summary>
        /// Permite desahacer las dependencias Organismos.
        ///</summary>
        [HttpPost]
        public ActionResult DesasignarOrganismos(int id)
        {
            try
            {

                Usuarios usuarios = this.repoUsuarios.Single(x => x.id == this.IdUsuario);

                this.ComboCategorias();

                this.ComboGrupos();

                this.listaOrganismosSessionAsignadas.ToList().ForEach(
                    p =>
                    {
                        if (p.Id == id)
                        {
                            this.listaOrganismosSessionAsignadas.Remove(p);
                            this.listaOrganismosSession.Add(p);
                        }
                    });

                return this.PartialView("~/Views/Usuarios/_PartialAsignarDependencia.cshtml", usuarios);

            }
            catch (Exception ex)
            {
                this.log.PublishException(new SecureportException(ex));

                return this.Json(new { result = false, Message = ex.Message });
            }
        }

        /// <summary>
        /// Permite guardar las instalaciones asociadas aun puerto.
        /// </summary>
        [HttpPost]
        public ActionResult GuardarDependenciasInstalaciones()
        {
            Usuarios usuarios = this.repoUsuarios.Single(x => x.id == this.IdUsuario);

            try
            {
                //Asignar las instalaciones
                this.listaInstalacionAsignadas.ToList().ForEach(
                    p =>
                    {

                        if (this.repoDependenciaUsuario.Single(x => x.id_Usuario == this.IdUsuario && x.id_Dependencia == p.Id) == null)
                        {
                            Depen_Usuario depenUsuario = new Depen_Usuario()
                            {
                                id_Usuario = this.IdUsuario,
                                id_Dependencia = p.Id,
                                id_usu_alta = this.UsuarioFrontalSession.Usuario.id,
                                fech_alta = DateTime.Now
                            };

                            this.repoDependenciaUsuario.Create(depenUsuario);
                        }
                    });

                //Desasignar las instalaciones
                this.listaInstalacionSession.ToList().ForEach(
                  p =>
                  {
                      Depen_Usuario depen_Usuario = this.repoDependenciaUsuario.Single(x => x.id_Usuario == this.IdUsuario && x.id_Dependencia == p.Id);

                      if (depen_Usuario != null)
                      {
                          this.repoDependenciaUsuario.Delete(depen_Usuario);

                          this.log.WriteInfo("Baja Dependencia", usuarios.nombre + '|' + usuarios.apellidos + '|' + usuarios.id + '|' + usuarios.login);
                      }
                  });

                this.ComboCategorias();

                this.ComboGrupos();

                this.LimpiarColecciones();

                return this.PartialView("~/Views/Usuarios/PlantillaVisualizar.cshtml", usuarios);
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
        /// Permite guardar las dependencias de puertos asignadas al usuario.
        /// </summary>
        [HttpPost]
        public ActionResult GuardarDependenciasOrganismos()
        {
            Usuarios usuarios = this.repoUsuarios.Single(x => x.id == this.IdUsuario);

            try
            {
                //Asignar los puertos
                this.listaOrganismosSessionAsignadas.ToList().ForEach(
                    p =>
                    {

                        if (this.repoDependenciaUsuario.Single(x => x.id_Usuario == this.IdUsuario && x.id_Dependencia == p.Id) == null)
                        {
                            Depen_Usuario depenUsuario = new Depen_Usuario()
                            {
                                id_Usuario = this.IdUsuario,
                                id_Dependencia = p.Id,
                                id_usu_alta = this.UsuarioFrontalSession.Usuario.id,
                                fech_alta = DateTime.Now
                            };

                            this.repoDependenciaUsuario.Create(depenUsuario);
                        }
                    });

                //Desasignar puertos
                this.listaOrganismosSession.ToList().ForEach(
                  p =>
                  {
                      Depen_Usuario depen_Usuario = this.repoDependenciaUsuario.Single(x => x.id_Usuario == this.IdUsuario && x.id_Dependencia == p.Id);

                      if (depen_Usuario != null)
                      {
                          this.repoDependenciaUsuario.Delete(depen_Usuario);

                          this.log.WriteInfo("Baja Dependencia", usuarios.nombre + '|' + usuarios.apellidos + '|' + usuarios.id + '|' + usuarios.login);
                      }
                  });

                this.ComboCategorias();

                this.ComboGrupos();

                this.LimpiarColecciones();

                return this.PartialView("~/Views/Usuarios/PlantillaVisualizar.cshtml", usuarios);
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
        public ActionResult RefrescarOrganismosAsignados()
        {
            return this.PartialView("~/Views/Usuarios/Documentos/_PartialDesasignarOrganismos.cshtml");
        }

        [HttpPost]
        public ActionResult RefrescarPuertosAsignados()
        {
            return this.PartialView("~/Views/Usuarios/Documentos/_PartialDesasignarPuertos.cshtml");
        }

        /// <summary>
        /// Permite desahacer las dependencias puertos.
        ///</summary>
        [HttpPost]
        public ActionResult DesasignarPuertos(int id)
        {
            try
            {

                Usuarios usuarios = this.repoUsuarios.Single(x => x.id == this.IdUsuario);

                this.ComboCategorias();

                this.ComboGrupos();

                this.listaPuertosSessionAsignadas.ToList().ForEach(
                    p =>
                    {
                        if (p.Id == id)
                        {
                            this.listaPuertosSessionAsignadas.Remove(p);
                            this.listaPuertosSession.Add(p);
                        }
                    });

                return this.PartialView("~/Views/Usuarios/_PartialAsignarDependencia.cshtml", usuarios);

            }
            catch (Exception ex)
            {
                this.log.PublishException(new SecureportException(ex));

                return this.Json(new { result = false, Message = ex.Message });
            }
        }

        [HttpPost]
        public ActionResult ValidarDependencias(int categoria)
        {
            if (this.UsuarioFrontalSession.Usuario.categoria == (int)EnumCategoria.Administrador)
            {
                return this.Json(new { result = true });
            }
            if (categoria == (int)EnumCategoria.Puerto)
            {
                if (!this.listaPuertosSessionAsignadas.Any())
                {
                    return this.Json(new { result = false, Message = MensajeError.NOK.ToDescription() + MostrarCategoria(categoria) });
                }
            }
            else if (categoria == (int)EnumCategoria.OrganismoGestionPortuaria)
            {
                if (!this.listaOrganismosSessionAsignadas.Any())
                {
                    return this.Json(new { result = false, Message = MensajeError.NOK.ToDescription() + MostrarCategoria(categoria) });
                }
            }
            else if (categoria == (int)EnumCategoria.Instalacion)
            {
                if (!this.listaInstalacionAsignadas.Any())
                {
                    return this.Json(new { result = false, Message = MensajeError.NOK.ToDescription() + MostrarCategoria(categoria) });
                }
            }
            return this.Json(new { result = true });
        }

        /// <summary>
        /// Permite guardar las dependencias de puertos asignadas al usuario.
        /// </summary>
        [HttpPost]
        public ActionResult GuardarDependenciasPuertos()
        {
            Usuarios usuarios = this.repoUsuarios.Single(x => x.id == this.IdUsuario);

            try
            {
                //Asignar los puertos
                this.listaPuertosSessionAsignadas.ToList().ForEach(
                    p =>
                    {

                        if (this.repoDependenciaUsuario.Single(x => x.id_Usuario == this.IdUsuario && x.id_Dependencia == p.Id) == null)
                        {
                            Depen_Usuario depenUsuario = new Depen_Usuario()
                            {
                                id_Usuario = this.IdUsuario,
                                id_Dependencia = p.Id,
                                id_usu_alta = this.UsuarioFrontalSession.Usuario.id,
                                fech_alta = DateTime.Now
                            };

                            this.repoDependenciaUsuario.Create(depenUsuario);
                        }
                    });

                //Desasignar puertos
                this.listaPuertosSession.ToList().ForEach(
                  p =>
                  {
                      Depen_Usuario depen_Usuario = this.repoDependenciaUsuario.Single(x => x.id_Usuario == this.IdUsuario && x.id_Dependencia == p.Id);

                      if (depen_Usuario != null)
                      {
                          this.repoDependenciaUsuario.Delete(depen_Usuario);

                          this.log.WriteInfo("Baja Dependencia", usuarios.nombre + '|' + usuarios.apellidos + '|' + usuarios.id + '|' + usuarios.login);
                      }
                  });

                this.ComboCategorias();

                this.ComboGrupos();

                this.LimpiarColecciones();

                return this.PartialView("~/Views/Usuarios/PlantillaVisualizar.cshtml", usuarios);
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
        /// Permite asignar puertos al usuario.
        /// </summary>
        [HttpPost]
        public ActionResult AsignarOrganismos(int id)
        {
            try
            {

                this.idDependencia = id;

                Usuarios usuarios = this.repoUsuarios.Single(x => x.id == this.IdUsuario);

                this.listaOrganismosSession.ToList().ForEach(
                    p =>
                    {
                        if (p.Id == id)
                        {
                            this.listaOrganismosSession.Remove(p);
                            this.listaOrganismosSessionAsignadas.Add(p);
                        }
                    });

                this.ComboCategorias();

                return this.PartialView("~/Views/Usuarios/_PartialAsignarDependencia.cshtml", usuarios);

            }
            catch (Exception ex)
            {
                this.log.PublishException(new SecureportException(ex));

                return this.Json(new { result = false, Message = ex.Message });
            }
        }

        /// <summary>
        /// Permite asignar puertos al usuario.
        /// </summary>
        [HttpPost]
        public ActionResult AsignarPuertos(int id)
        {
            try
            {

                this.idDependencia = id;

                Usuarios usuarios = this.repoUsuarios.Single(x => x.id == this.IdUsuario);

                this.listaPuertosSession.ToList().ForEach(
                    p =>
                    {
                        if (p.Id == id)
                        {
                            this.listaPuertosSession.Remove(p);
                            this.listaPuertosSessionAsignadas.Add(p);
                        }
                    });

                this.ComboCategorias();

                return this.PartialView("~/Views/Usuarios/_PartialAsignarDependencia.cshtml", usuarios);

            }
            catch (Exception ex)
            {
                this.log.PublishException(new SecureportException(ex));

                return this.Json(new { result = false, Message = ex.Message });
            }
        }


        /// <summary>
        /// Permite visualizar los datos de un usuario con dependencias.
        /// </summary>
        [HttpPost]
        public ActionResult AsignarDependenciaAdmin(UsuariosJson usuariosJson)
        {
            try
            {
                if (usuariosJson.categoria == (int)EnumCategoria.OrganismoGestionPortuaria)
                {
                    this.listaOrganismosSession = this._serviciosUsuarios.GetOrganismos().ToList();
                }
                else
                {
                    this.listaPuertosSessionAsignadas = this._serviciosPuertos.GetAllPuertos().ToList();
                }

                this.ComboCategorias();

                Usuarios usuarios = this.repoUsuarios.Single(x => x.id == this.IdUsuario);

                return this.PartialView("~/Views/Usuarios/_PartialAsignarDependencia.cshtml", usuarios);

            }
            catch (Exception ex)
            {
                this.log.PublishException(new SecureportException(ex));

                return this.Json(new { result = false, Message = ex.Message });
            }
        }

        /// <summary>
        /// Permite visualizar los datos de un usuario con dependencias.
        /// </summary>
        [HttpPost]
        public ActionResult AsignarDependencia(UsuariosJson usuariosJson)
        {
            try
            {
                IEnumerable<DependenciasUsuario> dependencias = new List<DependenciasUsuario>();

                IEnumerable<PuertosViewModel> listaPuertos = new List<PuertosViewModel>();

                this.ComboCategorias();

                Usuarios usuarios = this.repoUsuarios.Single(x => x.id == this.IdUsuario);

                this.listaPuertosSessionAsignadas = this._serviciosUsuarios.GetDependenciaPuertos(usuarios.id, usuarios.categoria).ToList();

                if (this.UsuarioFrontalSession.Usuario.categoria == (int)EnumCategoria.Administrador || this.UsuarioFrontalSession.Usuario.categoria == (int)EnumCategoria.PuertosdelEstado)
                {
                    listaPuertos = this._serviciosPuertos.GetAllPuertos().ToList();

                }
                else
                {
                    listaPuertos = (from t1 in this._serviciosPuertos.GetAllPuertos().ToList()
                                    join t2 in this.UsuarioFrontalSession.ListDependenciasDepenUsuarios.Where(x => x.categoria == (int)EnumCategoria.Puerto).ToList()
                                      on t1.Id equals t2.id_Dependencia
                                    select new PuertosViewModel
                                    {
                                        Id = t1.Id,
                                        Nombre = t1.Nombre,
                                        id_Organismo = t1.id_Organismo,
                                        Isla = t1.Isla,
                                        ID_Provincia = t1.ID_Provincia,
                                        Id_CapMarit = t1.Id_CapMarit
                                    });
                }


                 listaPuertos.ToList().ForEach(
                 p =>
                    {
                        p.Organismo = p.id_Organismo != 0 ? MostrarOrganismo(p.id_Organismo) : string.Empty;
                        p.Provincia = p.ID_Provincia != 0 ? MostrarProvincias(p.ID_Provincia): string.Empty;
                        p.Capitania = p.Id_CapMarit  != 0 ? MostrarCapitania(p.Id_CapMarit)  : string.Empty;
                    });
                
                this.listaPuertosSession = listaPuertos.ToList();
                
                this.listaPuertosSession.ToList().ForEach(
                    p => this.listaPuertosSessionAsignadas.ToList().ForEach(
                        t =>
                        {
                            if (p.Id == t.Id)
                            {
                                this.listaPuertosSession.Remove(p);
                            }
                        }));


                return this.PartialView("~/Views/Usuarios/_PartialAsignarDependencia.cshtml", usuarios);

            }
            catch (Exception ex)
            {
                this.log.PublishException(new SecureportException(ex));

                return this.Json(new { result = false, Message = ex.Message });
            }
        }

        /// <summary>
        /// Permite desahacer los datos de un usuario con dependencias tipo Instalacion.
        /// </summary>
        [HttpPost]
        public ActionResult DesahacerInstalacion()
        {
            try
            {

                IEnumerable<PuertosViewModel> listaPuertos = this._serviciosPuertos.GetAllPuertos().ToList();

                this.listaPuertosSession = listaPuertos.ToList();

                this.idPuerto = listaPuertos.ToList().FirstOrDefault().Id;

                IEnumerable<PuertosViewModel> result = (from t1 in this.listaPuertosSession
                                                        orderby t1.Id
                                                        select
                                                            new PuertosViewModel
                                                            {
                                                                Id = t1.Id,
                                                                Nombre = t1.Nombre,
                                                                asignar = false
                                                            });

                this.ComboCategorias();

                Usuarios usuarios = this.repoUsuarios.Single(x => x.id == this.IdUsuario);

                this.listaInstalacionAsignadas = this._serviciosUsuarios.GetDependenciaInstalaciones(this.IdUsuario, usuarios.categoria).ToList();

                return this.PartialView("~/Views/Usuarios/_PartialAsignarDependencia.cshtml", usuarios);

            }
            catch (Exception ex)
            {
                this.log.PublishException(new SecureportException(ex));

                return this.Json(new { result = false, Message = ex.Message });
            }
        }

        /// <summary>
        /// Permite desahacer los datos de un usuario con dependencias tipo organismo.
        /// </summary>
        [HttpPost]
        public ActionResult DesahacerOrganismos()
        {
            try
            {
                this.ComboCategorias();

                Usuarios usuarios = this.repoUsuarios.Single(x => x.id == this.IdUsuario);

                this.listaOrganismosSessionAsignadas = this._serviciosUsuarios.GetDependenciaOrganismos(this.IdUsuario, usuarios.categoria).ToList();

                IEnumerable<Organismos> listaOrganismos = this._serviciosUsuarios.GetOrganismos().ToList();

                this.listaOrganismosSession = listaOrganismos.ToList();

                this.listaOrganismosSession.ToList().ForEach(
                    p => this.listaOrganismosSessionAsignadas.ToList().ForEach(
                        t =>
                        {
                            if (p.Id == t.Id)
                            {
                                this.listaOrganismosSession.Remove(p);
                            }
                        }));

                return this.PartialView("~/Views/Usuarios/_PartialAsignarDependencia.cshtml", usuarios);

            }
            catch (Exception ex)
            {
                this.log.PublishException(new SecureportException(ex));

                return this.Json(new { result = false, Message = ex.Message });
            }
        }

        /// <summary>
        /// Permite visualizar los datos de un usuario con documentos.
        /// </summary>
        [HttpPost]
        public ActionResult AsignarDocumentos()
        {
            try
            {

                this.ComboTipos_Documento();

                Usuarios usuarios = this.repoUsuarios.Single(x => x.id == this.IdUsuario);

                return this.PartialView("~/Views/Usuarios/_PartialAsignarDocumentos.cshtml", usuarios);

            }
            catch (Exception ex)
            {
                this.log.PublishException(new SecureportException(ex));

                return this.Json(new { result = false, Message = ex.Message });
            }
        }

        /// <summary>
        /// Permite visualizar los datos de un usuario ya registrado.
        /// </summary>
        [HttpPost]
        public ActionResult Visualizar(int? id)
        {
            try
            {
                this.IdUsuario = (id == null ? this.IdUsuario : (int)id);

                Usuarios usuarios = this.repoUsuarios.Single(x => x.id == this.IdUsuario);

                if (usuarios == null)
                {
                    return this.View("~/Views/ErrorPages/ErrorPage400.cshtml");
                }

                this.ComboCategorias();

                this.ComboGrupos();

                this.LimpiarColecciones();

                this.ViewBag.navegador = this.Browser;

                encriptador clave = new encriptador();

                usuarios.password = clave.DesEncriptacion(usuarios.password);

                return this.PartialView("~/Views/Usuarios/PlantillaVisualizar.cshtml", usuarios);

            }
            catch (Exception ex)
            {
                this.log.PublishException(new SecureportException(ex));

                return this.Json(new { result = false, Message = ex.Message });
            }
        }

        public ActionResult OrganismosDisponibles([DataSourceRequest]DataSourceRequest request)
        {
            try
            {
                Usuarios usuarios = this.repoUsuarios.Single(x => x.id == this.UsuarioFrontalSession.Usuario.id);

                IEnumerable<Organismos> listaOrganismos = new List<Organismos>();

                if (usuarios.categoria == (int)EnumCategoria.Administrador
                    || usuarios.categoria == (int)EnumCategoria.PuertosdelEstado)
                {

                    listaOrganismos = this._serviciosUsuarios.GetOrganismos().ToList();
                }
                else
                {
                    listaOrganismos = (from t1 in this._serviciosUsuarios.GetOrganismos().ToList()
                                       join t2 in this.UsuarioFrontalSession.ListDependenciasDepenUsuarios.Where(x => x.categoria == (int)EnumCategoria.OrganismoGestionPortuaria).ToList()
                                       on t1.Id equals t2.id_Dependencia
                                       select new Organismos { Id = t1.Id, Nombre = t1.Nombre });
                }

                if (!this.listaOrganismosSession.Any())
                {
                    this.listaOrganismosSession = listaOrganismos.ToList();

                    this.listaOrganismosSession.ToList().ForEach(
                        p => this.listaOrganismosSessionAsignadas.ToList().ForEach(
                            t =>
                            {
                                if (p.Id == t.Id)
                                {
                                    this.listaOrganismosSession.Remove(p);
                                }
                            }));
                }

                IEnumerable<OrganismosViewModel> result = (from t1 in this.listaOrganismosSession
                                                           orderby t1.Id
                                                           select
                                                               new OrganismosViewModel()
                                                               {
                                                                   Id = t1.Id,
                                                                   Nombre = t1.Nombre,
                                                                   asignar = false
                                                               });


                return Json(result.ToDataSourceResult(request));

            }
            catch (Exception ex)
            {
                this.log.PublishException(new SecureportException(ex));

                return this.Json(new { result = false, Message = ex.Message });
            }
        }

        public ActionResult DesasignarOrganismosAsociados([DataSourceRequest]DataSourceRequest request)
        {
            try
            {
                Usuarios usuarios = this.repoUsuarios.Single(x => x.id == this.IdUsuario);

                if (this.listaOrganismosSessionAsignadas.Any() == false)
                {
                    this.listaOrganismosSessionAsignadas = this._serviciosUsuarios.GetDependenciaOrganismos(this.IdUsuario, usuarios.categoria).ToList();
                }

                this.listaOrganismosSessionAsignadas.ToList().ForEach(
                       p => this.listaOrganismosSession.ToList().ForEach(
                           t =>
                           {
                               if (p.Id == t.Id)
                               {
                                   this.listaOrganismosSessionAsignadas.Remove(p);
                               }
                           }));

                IEnumerable<OrganismosAsociadosViewModel> result = (from t1 in this.listaOrganismosSessionAsignadas
                                                                    orderby t1.Id
                                                                    select
                                                                        new OrganismosAsociadosViewModel()
                                                                        {
                                                                            Id = t1.Id,
                                                                            Nombre = t1.Nombre,
                                                                            asignar = false
                                                                        });


                return Json(result.ToDataSourceResult(request));

            }
            catch (Exception ex)
            {
                this.log.PublishException(new SecureportException(ex));

                return this.Json(new { result = false, Message = ex.Message });
            }
        }

        public ActionResult OrganismosAsociados([DataSourceRequest]DataSourceRequest request)
        {
            try
            {
                Usuarios usuarios = this.repoUsuarios.Single(x => x.id == this.IdUsuario);

                if (!this.listaOrganismosSessionAsignadas.Any())
                {
                    this.listaOrganismosSessionAsignadas = this._serviciosUsuarios.GetDependenciaOrganismos(
                        this.IdUsuario,
                        usuarios.categoria).ToList();
                }

                IEnumerable<Organismos> result = (from t1 in this.listaOrganismosSessionAsignadas
                                                  orderby t1.Id
                                                  select
                                                      new Organismos()
                                                      {
                                                          Id = t1.Id,
                                                          Nombre = t1.Nombre
                                                      });


                return Json(result.OrderBy(x=> x.Nombre).ToDataSourceResult(request));

            }
            catch (Exception ex)
            {
                this.log.PublishException(new SecureportException(ex));

                return this.Json(new { result = false, Message = ex.Message });
            }
        }

        public ActionResult PuertosDisponibles([DataSourceRequest]DataSourceRequest request)
        {
            try
            {

                Usuarios usuarios = this.repoUsuarios.Single(x => x.id == this.UsuarioFrontalSession.Usuario.id);

                IEnumerable<PuertosViewModel> listaPuertos = new List<PuertosViewModel>();

                if (usuarios.categoria == (int)EnumCategoria.Administrador || usuarios.categoria == (int)EnumCategoria.PuertosdelEstado)
                {
                   listaPuertos = this._serviciosPuertos.GetAllPuertos().ToList();
                    
                }
                else
                {
                    listaPuertos = (from t1 in _serviciosPuertos.GetAllPuertos().ToList()
                                    join t2 in this.UsuarioFrontalSession.ListDependenciasDepenUsuarios.Where(x => x.categoria == (int)EnumCategoria.Puerto).ToList()
                                    on t1.Id equals t2.id_Dependencia
                                    select new PuertosViewModel { Id = t1.Id, 
                                                                  Nombre = t1.Nombre, 
                                                                  id_Organismo = t1.id_Organismo, 
                                                                  Isla = t1.Isla, 
                                                                  ID_Provincia = t1.ID_Provincia, 
                                                                  Id_CapMarit = t1.Id_CapMarit});
                
                }

                listaPuertos.ToList().ForEach(
                  p =>
                  {
                      p.Organismo = p.id_Organismo != 0 ? MostrarOrganismo(p.id_Organismo) : string.Empty;
                      p.Provincia = p.ID_Provincia != 0 ? MostrarProvincias(p.ID_Provincia) : string.Empty;
                      p.Capitania = p.Id_CapMarit  != 0 ? MostrarCapitania(p.Id_CapMarit) : string.Empty;
                  });


                if (this.listaPuertosSessionAsignadas.Any())
                {
                    this.listaPuertosSession = listaPuertos.ToList();

                    this.listaPuertosSession.ToList().ForEach(
                        p => this.listaPuertosSessionAsignadas.ToList().ForEach(
                            t =>
                            {
                                if (p.Id == t.Id)
                                {
                                    this.listaPuertosSession.Remove(p);
                                }
                            }));

                    this.idPuerto = listaPuertos.ToList().FirstOrDefault().Id;
                }

                IEnumerable<PuertosViewModel> result = (from t1 in this.listaPuertosSession
                                                        orderby t1.Id
                                                        select
                                                            new PuertosViewModel
                                                            {
                                                                Id = t1.Id,
                                                                Nombre = t1.Nombre,
                                                                asignar = false,
                                                                Isla = t1.Isla,
                                                                Organismo = t1.Organismo,
                                                                Provincia = t1.Provincia,
                                                                Capitania = t1.Capitania
                                                            }).OrderBy(x=> x.Nombre);


                return Json(result.ToDataSourceResult(request));

            }
            catch (Exception ex)
            {
                this.log.PublishException(new SecureportException(ex));

                return this.Json(new { result = false, Message = ex.Message });
            }
        }

        public ActionResult PuertosAsociados([DataSourceRequest]DataSourceRequest request)
        {

            try
            {
                Usuarios usuarios = this.repoUsuarios.Single(x => x.id == this.UsuarioFrontalSession.Usuario.id);

                if (this.listaPuertosSessionAsignadas.Any() == false)
                {
                    IEnumerable<PuertosViewModel> listaPuertos = new List<PuertosViewModel>();

                    if (usuarios.categoria == (int)EnumCategoria.Administrador || usuarios.categoria == (int)EnumCategoria.PuertosdelEstado)
                    {
                        listaPuertos = this._serviciosPuertos.GetAllPuertos().ToList();

                    }
                    else
                    {
                        listaPuertos = (from t1 in _serviciosPuertos.GetAllPuertos().ToList()
                                        join t2 in this.UsuarioFrontalSession.ListDependenciasDepenUsuarios.Where(x => x.categoria == (int)EnumCategoria.Puerto).ToList()
                                        on t1.Id equals t2.id_Dependencia
                                        select new PuertosViewModel
                                        {
                                            Id = t1.Id,
                                            Nombre = t1.Nombre,
                                            id_Organismo = t1.id_Organismo,
                                            Isla = t1.Isla,
                                            ID_Provincia = t1.ID_Provincia,
                                            Id_CapMarit = t1.Id_CapMarit
                                        });

                    }

                    if (listaPuertos.ToList().Count() != this.listaPuertosSession.ToList().Count())
                    {
                        Usuarios user = this.repoUsuarios.Single(x => x.id == this.IdUsuario);
                        this.listaPuertosSessionAsignadas = this._serviciosUsuarios.GetDependenciaPuertos(this.IdUsuario, user.categoria).ToList();    
                    }
                    
                }

                this.listaPuertosSessionAsignadas.ToList().ForEach(
                        p => this.listaPuertosSession.ToList().ForEach(
                            t =>
                            {
                                if (p.Id == t.Id)
                                {
                                    this.listaPuertosSession.Remove(t);
                                }
                            }));


                IEnumerable<PuertosAsociadosViewModel> result = (from t1 in this.listaPuertosSessionAsignadas
                                                                 orderby t1.Id
                                                                 select
                                                                    new PuertosAsociadosViewModel
                                                                    {
                                                                        Id = t1.Id,
                                                                        Nombre = t1.Nombre,
                                                                        asignar = false
                                                                    });


                return Json(result.OrderBy(x=> x.Nombre).ToDataSourceResult(request));


            }
            catch (Exception ex)
            {
                this.log.PublishException(new SecureportException(ex));

                return this.Json(new { result = false, Message = ex.Message });
            }
        }

        public ActionResult InstalacionesDisponibles([DataSourceRequest]DataSourceRequest request)
        {
            try
            {

                if (this.listaInstalacionAsignadas.Any())
                {
                    this.listaInstalacionSession.ToList().ForEach(
                        p => this.listaInstalacionAsignadas.ToList().ForEach(
                            t =>
                            {
                                if (p.Id == t.Id && p.id_Puerto == t.id_Puerto)
                                {
                                    this.listaInstalacionSession.Remove(p);
                                }
                            }));
                }


                IEnumerable<InstalacionesViewModel> result = (from t1 in this.listaInstalacionSession
                                                              orderby t1.Id
                                                              select
                                                                  new InstalacionesViewModel
                                                                  {
                                                                      Id = t1.Id,
                                                                      Nombre = t1.Nombre,
                                                                      IIPP = MostrarInstalacion(t1.Clasificacion.ToString()),
                                                                      asignar = false
                                                                  }).OrderBy(x=> x.Nombre);
                return Json(result.ToDataSourceResult(request));

            }
            catch (Exception ex)
            {
                this.log.PublishException(new SecureportException(ex));

                return this.Json(new { result = false, Message = ex.Message });
            }
        }

        public ActionResult InstalacionesAsociadosPuertos([DataSourceRequest]DataSourceRequest request)
        {

            try
            {
                Usuarios usuarios = this.repoUsuarios.Single(x => x.id == this.IdUsuario);

                if (this.listaInstalacionAsignadas.Any() == false)
                {
                    this.listaInstalacionAsignadas = this._serviciosUsuarios.GetDependenciaInstalaciones(this.IdUsuario, usuarios.categoria).ToList();
                }

                this.listaInstalacionAsignadas.ToList().ForEach(
                        p => this.listaInstalacionSession.ToList().ForEach(
                            t =>
                            {
                                if (p.Id == t.Id && p.id_Puerto == t.id_Puerto)
                                {
                                    this.listaInstalacionAsignadas.Remove(p);
                                }
                            }));


                IEnumerable<InstalacionesAsociadosViewModel> result = (from t1 in this.listaInstalacionAsignadas
                                                                       where t1.id_Puerto == this.idPuerto
                                                                       orderby t1.Id
                                                                       select
                                                                           new InstalacionesAsociadosViewModel
                                                                           {
                                                                               Id = t1.Id,
                                                                               Nombre = t1.Nombre,
                                                                               IIPP = MostrarInstalacion(t1.Clasificacion.ToString()),
                                                                               asignar = false
                                                                           });

                return Json(result.OrderBy(x=> x.Nombre).ToDataSourceResult(request));
            }
            catch (Exception ex)
            {
                this.log.PublishException(new SecureportException(ex));

                return this.Json(new { result = false, Message = ex.Message });
            }
        }

        public ActionResult InstalacionesAsociados([DataSourceRequest]DataSourceRequest request)
        {
            try
            {
                Usuarios usuarios = this.repoUsuarios.Single(x => x.id == this.IdUsuario);

                if (!this.listaInstalacionAsignadas.Any())
                {
                    this.listaInstalacionAsignadas = this._serviciosUsuarios.GetDependenciaInstalaciones(this.IdUsuario, usuarios.categoria).ToList();
                }


                IEnumerable<MOV_Instalaciones> result = (from t1 in this.listaInstalacionAsignadas
                                                         orderby t1.Id
                                                         select
                                                            new MOV_Instalaciones
                                                            {
                                                                Id = t1.Id,
                                                                Clasificacion = t1.Clasificacion,
                                                                Nombre = t1.Nombre
                                                            });

                
                return Json(result.OrderBy(x=> x.Nombre).ToDataSourceResult(request));

            }
            catch (Exception ex)
            {
                this.log.PublishException(new SecureportException(ex));

                return this.Json(new { result = false, Message = ex.Message });
            }
        }

        public ActionResult DocumentosAsociados([DataSourceRequest]DataSourceRequest request)
        {
            try
            {

                IEnumerable<Doc_Usuario_Asoc> result = (from t1 in this._serviciosUsuarios.GetDocs_Usuario(this.IdUsuario)
                                                        orderby t1.id
                                                        select new Doc_Usuario_Asoc
                                                        {
                                                            id = t1.id,
                                                            TipoNombre = t1.TipoNombre,
                                                            documento = t1.documento.Substring((t1.documento.IndexOf("_")) + 1),
                                                            descripcion = t1.descripcion
                                                        }).OrderBy(x=> x.documento);

                return Json(result.ToDataSourceResult(request));
            }
            catch (Exception ex)
            {
                this.log.PublishException(new SecureportException(ex));

                return this.Json(new { result = false, Message = ex.Message });
            }
        }

        [HttpPost]
        public ActionResult Excel_Export_Save(string contentType, string base64, string fileName)
        {
            var fileContents = Convert.FromBase64String(base64);

            return File(fileContents, contentType, fileName);
        }

        /// <summary>
        /// Retorna si el usuario esta logeado en la aplicación.
        /// </summary>
        [HttpPost]
        public ActionResult UsuarioLogueado()
        {
            try
            {
                bool logueado = this.UsuarioFrontalSession.Usuario != null;

                return this.Json(new { result = logueado });

            }
            catch (Exception ex)
            {
                this.log.PublishException(new SecureportException(ex));

                return this.Json(new { result = false, Message = ex.Message });
            }

        }

        /// <summary>
        /// Retorna todos los usuarios activos en el grid.
        /// </summary>
        public ActionResult Index()
        {
            return this.View(this.db.Usuarios.ToList());
        }

        /// <summary>
        /// Permite visualizar los datos de un usuario ya registrado.
        /// </summary>
        [HttpPost]
        public void AsignarTipoDocumento(UploadJson uploadJson)
        {
            try
            {
                this.idTipoDocumento = uploadJson.tipo;

            }
            catch (Exception ex)
            {
                this.log.PublishException(new SecureportException(ex));
            }
        }

        [HttpPost]
        public ActionResult ValidarDocumento(UploadJson uploadJson)
        {
            string nombre = uploadJson.nombre_servicio + uploadJson.id_servicio.ToString() + "_" + uploadJson.nombre;

            IEnumerable<Doc_Asoc> result = this._serviciosDocumentos.GetDocs(this.IdUsuario, 1).ToList().Where(x => x.documento == nombre);
            if (result.Count() > 0)
                return this.Json(new { result = 0 });
            else
                return this.Json(new { result = 1 });
        }

        #endregion
    }
}
