
namespace SecurePort.Web.Views
{
    #region Using

    using System.Configuration;
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

    public class DeclaraMaritimaController : BaseController
    {

        public DeclaraMaritimaController(IServiciosDeclaraMaritima serviciosDeclaraMaritima,
                                                IServiciosUsuarios servicioUsuarios,
                                                 DeclaraMaritimaRepository repoDeclaraMaritima,
                                                 IServiciosPuertos servicioPuertos,
                                                 IServiciosTipoInstalacion serviciosInstalaciones,
                                                 IServiciosOrganismo serviciosOrganismos,
                                                 IServiciosMotivos serviciosMotivos,
                                                 DocumentosUsuarioRepository repoDocumentosUsuario,
                                                 IServiciosDocumentos servicioDocumentos,
                                                 TrazasRepository repoTrazas,
                                                 ILogger log)

            : base(serviciosDeclaraMaritima,
                   servicioUsuarios,
                   repoDeclaraMaritima, 
                   servicioPuertos,
                   serviciosInstalaciones,
                   serviciosOrganismos,
                   serviciosMotivos,
                   repoDocumentosUsuario,
                   servicioDocumentos,
                   repoTrazas,
                   log)
        {
             
        }

        #region Buscador
        /// <summary>
        /// Permite buscar las declaraciones maritimas
        /// </summary>
        public ActionResult BuscadorDeclaraMaritima([DataSourceRequest]DataSourceRequest request)
        {
            try
            {
               
                if (this.UsuarioFrontalSession.Usuario.categoria == (int)EnumCategoria.Administrador
                    || this.UsuarioFrontalSession.Usuario.categoria == (int)EnumCategoria.PuertosdelEstado)
                {
                    return this.Json(this._serviciosDeclaraMaritima.GetAllDeclaraMaritima().OrderBy(x => x.Id).ToDataSourceResult(request));
                }
                var categoria = this.UsuarioFrontalSession.Usuario.categoria;
                return this.Json(this._serviciosDeclaraMaritima.GetAllDeclaraMaritima(this.UsuarioFrontalSession.ListDependenciasDepenUsuarios, categoria).ToDataSourceResult(request));

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
        /// Retorna una lista de todas las declaraciones maritimas.
        /// </summary>
        public ActionResult ListadoDeclaraMaritima()
        {
            try
            {
                ViewBag.Mensaje = "Declaraciones de Protección Marítima";
                ViewBag.URLDelete = "/DeclaraMaritima/EliminarDeclaraMaritima";
                return this.PartialView("ListadoDeclaraMaritima", this.UsuarioFrontalSession);
            }
            catch (Exception ex)
            {
                this.log.PublishException(new SecureportException(ex));
                throw;
            }
        }
        #endregion

        [HttpPost]
        public ActionResult CambiarCentro(int? id)
        {
            try
            {

                this.comboInstalacionesPuertos(null, id);

                return this.PartialView("~/Views/DeclaraMaritima/ComboCentro.cshtml");

            }
            catch (Exception ex)
            {
                this.log.PublishException(new SecureportException(ex));

                return this.Json(new { result = false, Message = ex.Message });
            }

        }

        [HttpPost]
        public ActionResult CambiarPuerto(int? id)
        {
            try
            {

                this.comboPuertosOrganismo(null, id);

                return this.PartialView("~/Views/DeclaraMaritima/ComboPuerto.cshtml");

            }
            catch (Exception ex)
            {
                this.log.PublishException(new SecureportException(ex));

                return this.Json(new { result = false, Message = ex.Message });
            }

        }
        #region CRUD

        #region Documentos

        public ActionResult AsociadosEdit(bool ToolBar)
        {
            this.UsuarioFrontalSession.valor = ToolBar;

            return PartialView("~/Views/DeclaraMaritima/Documentos/_PartialAsociadosEdit.cshtml", this.UsuarioFrontalSession);

        }

        public ActionResult AsociadosEditDeclara(int Id)
        {
            return PartialView("~/Views/DeclaraMaritima/Documentos/_PartialAsociadosEdit.cshtml", this.UsuarioFrontalSession);
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

                DeclaraMaritimaViewModel declara = new DeclaraMaritimaViewModel();
                
                this._serviciosDeclaraMaritima.GetAllDeclaraMaritima().Where(x => x.Id == this.idDeclara).ToList().ForEach(
                   p =>
                   {
                       declara.Id = p.Id;
                       declara.NombreOrganismo = p.NombreOrganismo;
                       declara.NombrePuerto    = p.NombrePuerto;

                   });

                return this.PartialView("~/Views/DeclaraMaritima/_PartialAsignarDocumentos.cshtml", declara);

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

                IEnumerable<Doc_Asoc> result = (from t1 in this._serviciosDocumentos.GetDocs(this.idDeclara, 6)
                                                orderby t1.id
                                                select new Doc_Asoc
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

        /// <summary>
        ///     Permite guardar cambios en un documento asociado.
        /// </summary>
        [HttpPost]
        public ActionResult GuardarDocumento(DocumentoJson DocumentoJson)
        {
            try
            {

                this.ComboTipos_Documento();

                Docs_Usuario docsUsuario = this.repoDocumentosUsuario.Single(x => x.id == DocumentoJson.id);

                docsUsuario.descripcion = DocumentoJson.descripcion;

                docsUsuario.id_tipo_doc = DocumentoJson.tipodocumento;

                this.repoDocumentosUsuario.Update(docsUsuario);

                this.trazas(117, (int)this.UsuarioFrontalSession.Usuario.id, "Se ha dado de modificado un registro en SPCON_Depen_Usuario con ID " + "(" + DocumentoJson.id.ToString() + ")");

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

                IEnumerable<DeclaraMaritimaViewModel> formacion = this._serviciosDeclaraMaritima.GetAllDeclaraMaritima().Where(x => x.Id == this.idDeclara);

                DeclaraMaritimaViewModel temp = formacion.FirstOrDefault();

                Docs_Usuario docsInstalacion = this.repoDocumentosUsuario.Single(x => x.id == id);

                documento.Nombre = temp.NombreOrganismo;

                documento.Apellidos = temp.NombrePuerto;

                documento.descripcion = docsInstalacion.descripcion;

                documento.tipodocumento = docsInstalacion.id_tipo_doc;

                documento.documento = docsInstalacion.documento.Substring((docsInstalacion.documento.IndexOf("_")) + 1);

                return this.PartialView("~/Views/DeclaraMaritima/_PartialEditDocumentos.cshtml", documento);

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

                this.trazas(116, (int)this.UsuarioFrontalSession.Usuario.id, "Se ha dado de borrado un registro en SPMOV_Documentos_SP con ID " + "(" + id.ToString() + ")");

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

                IEnumerable<DeclaraMaritimaViewModel> formacion = this._serviciosDeclaraMaritima.GetAllDeclaraMaritima().Where(x => x.Id == this.idDeclara);

                DeclaraMaritimaViewModel temp = formacion.FirstOrDefault();

                Docs_Usuario docsFormacion = this.repoDocumentosUsuario.Single(x => x.id == id);

                documento.Nombre = temp.NombreOrganismo;

                documento.Apellidos = temp.NombrePuerto;

                documento.descripcion = docsFormacion.descripcion;

                documento.tipodocumento = docsFormacion.id_tipo_doc;

                documento.documento = docsFormacion.documento.Substring((docsFormacion.documento.IndexOf("_")) + 1); 

                return this.PartialView("~/Views/DeclaraMaritima/_PartialEditDocumentos.cshtml", documento);

            }
            catch (Exception ex)
            {
                this.log.PublishException(new SecureportException(ex));

                return this.Json(new { result = false, Message = ex.Message });
            }
        }

        [HttpPost]
        public ActionResult ValidarDocumento(UploadJson uploadJson)
        {
            string nombre = uploadJson.nombre_servicio + uploadJson.id_servicio.ToString() + "_" + uploadJson.nombre;

            IEnumerable<Doc_Asoc> result = this._serviciosDocumentos.GetDocs(this.idDeclara, 6).ToList().Where(x => x.documento == nombre);
            if (result.Count() > 0)
                return this.Json(new { result = 0 });
            else
                return this.Json(new { result = 1 });
        }


        #endregion

        #region DeclaMaritima

        [HttpPost]
        public ActionResult Create()
        {
            DeclaraMaritimaViewModel declaraMaritima = new DeclaraMaritimaViewModel();

            this.ComboMotivos(null);

            if (((MultiSelectList)(this.ViewData["Motivos"])).Count() == 1)
                declaraMaritima.Id_Motivo = Convert.ToInt32(((SelectList)this.ViewData["Motivos"]).FirstOrDefault().Value);

            this.comboPuertosOrganismo(null, null);

            if (((MultiSelectList)(this.ViewData["puertos"])).Count() == 1)
                declaraMaritima.Id_Puerto = Convert.ToInt32(((SelectList)this.ViewData["puertos"]).FirstOrDefault().Value);

            this.comboOrganismos(null);

            if (((MultiSelectList)(this.ViewData["organismos"])).Count() == 1)
                declaraMaritima.Id_Organismo = Convert.ToInt32(((SelectList)this.ViewData["organismos"]).FirstOrDefault().Value);

            this.comboInstalacionesPuertos(null, null);

            if (((MultiSelectList)(this.ViewData["Instalaciones"])).Count() == 1)
                declaraMaritima.Id_IIPP = Convert.ToInt32(((SelectList)this.ViewData["Instalaciones"]).FirstOrDefault().Value);

            ViewBag.disabled = true;

            ViewBag.Mensaje = "Alta Declaraciones de Protección Marítima";

            ViewBag.disabled = true;

            ViewBag.alta = "display:none";

            ViewBag.action = General.AltaGeneral.ToDescription();

            ViewBag.ToolFormacion = false;

            ViewBag.Combo = "display:block";

            ViewBag.Texto = "display:none";

            ViewBag.ToolDeclara = false;

            return this.PartialView("~/Views/DeclaraMaritima/Create.cshtml", declaraMaritima);
        }


        /// <summary>
        /// Validar Declaraciones Maritimas
        /// </summary>
        [HttpPost]
        public ActionResult ValidarDeclaraciones(int Id)
        {
            try
            {

                var retorno = ValidarCategoria(Id, this.repoDeclaraMaritima.Single(x => x.Id == Id)) ? EliminarCategoria.MessageCategoria.ToDescription() + "  " + this.Description : string.Empty;

                var valor = retorno == string.Empty;

                return this.Json(new { result = valor, Message = retorno });

            }
            catch (Exception ex)
            {
                this.log.PublishException(new SecureportException(ex));

                return this.Json(new { result = false, Message = ex.Message });
            }

        }

        /// <summary>
        /// Visualizar los datos de las Declaraciones Maritimas
        /// </summary>
        [HttpPost]
        public ActionResult VisualizarDeclaraMaritima(int? id, string Accion)
        {
            try
            {

                bool valor = true;

                this.idDeclara = (id == null ? this.idDeclara : (int)id);

                valor = ValidarCategoria(this.idDeclara, this.repoDeclaraMaritima.Single(x => x.Id == this.idDeclara));

                if (Accion != General.AltaVer.ToDescription())
                {
                    if (!valor)
                        return this.Json(new { result = 0, Message = ModificarCategoria.MessageCategoria.ToDescription() + "  " + this.Description });
                }

                ViewBag.disabled = false;

                ViewBag.Mensaje = "Visualizar Declaraciones de Protección Marítima";

                ViewBag.action = Accion;

                DeclaraMaritimaViewModel declaraMaritima = new DeclaraMaritimaViewModel();

                this._serviciosDeclaraMaritima.GetAllDeclaraMaritima().Where(x => x.Id == idDeclara).ToList().ForEach(
                   p =>
                   {
                       declaraMaritima.Id               = p.Id;

                       declaraMaritima.Id_IIPP          = p.Id_IIPP;

                       declaraMaritima.IMO              = p.IMO;

                       declaraMaritima.Buque            = p.Buque;

                       declaraMaritima.Id_Motivo        = p.Id_Motivo;

                       declaraMaritima.Fech_Expe        = p.Fech_Expe;

                       declaraMaritima.Fech_Ini_Validez = p.Fech_Ini_Validez;

                       declaraMaritima.Fech_Fin_Validez = p.Fech_Fin_Validez;

                       declaraMaritima.Responsable      = p.Responsable;

                       declaraMaritima.Nivel_Buq        = p.Nivel_Buq;

                       declaraMaritima.Nivel_IIPP       = p.Nivel_IIPP;

                       declaraMaritima.Medidas          =  p.Medidas;

                       declaraMaritima.Observaciones    = this.CambiarTexto(p.Observaciones);

                   });

                
                switch (Accion)
                {
                    case "Alta":
                        ViewBag.alta = string.Empty;
                        break;
                    case "Edit":
                        ViewBag.disabled = true;
                        ViewBag.alta = "display:none";
                        break;
                    default:
                        ViewBag.disabled = false;
                        ViewBag.alta = "display:block";
                        break;
                }

                declaraMaritima.Id_Puerto = this.db.MOV_Instalaciones.Where(x => x.Id == declaraMaritima.Id_IIPP).ToList().FirstOrDefault().id_Puerto;

                this.comboInstalacionesPuertos(null, declaraMaritima.Id_Puerto);

                declaraMaritima.Id_Organismo = this.db.Puertos.Where(x => x.Id == declaraMaritima.Id_Puerto).ToList().FirstOrDefault().id_Organismo;

                comboPuertosOrganismo(null, declaraMaritima.Id_Organismo);

                comboOrganismos(null);

                ComboMotivos(null);
                
                ViewBag.ToolDeclara = valor;

                ViewBag.Medidas = declaraMaritima.Medidas;
                
                ViewBag.Observaciones = declaraMaritima.Observaciones;

                return this.PartialView("~/Views/DeclaraMaritima/Create.cshtml", declaraMaritima);
            }
            catch (Exception ex)
            {
                this.log.PublishException(new SecureportException(ex));

                return this.Json(new { result = false, Message = ex.Message });
            }
        }
        /// <summary>
        /// Alta-Editar DeclaraMaritima.
        /// </summary>
        [HttpPost]
        public ActionResult AltaEditarDeclaraMaritima(DeclaraMaritimaJson declaraMaritimaJson)
        {
            try
            {
                int resultado ;

                if (MostrarMotivo((int)declaraMaritimaJson.Id_Motivo) == Message.Otros.ToDescription())
                {
                    if (declaraMaritimaJson.Observaciones == string.Empty || declaraMaritimaJson.Observaciones == null)
                    {
                        return this.Json(new { result = 0, Message = "Especifique en el campo Observaciones los motivos de la Declaración." });
                    }
                }

                switch (declaraMaritimaJson.action)
                {
                         
                    case "Alta":
                        
                        AltaDeclaraMaritima(declaraMaritimaJson);
                        resultado = 1;
                        break;

                    default:
                        
                        EditarDeclaraMaritima(declaraMaritimaJson);
                        resultado = 1;
                        break;
                }

                return this.Json(new { result = resultado });
            }
            catch (DbUpdateException ex)
            {
                this.log.PublishException(new SecureportExceptionDbUpdate(ex));

                return this.Json(new { result = 0, Message = ex.Message });
            }
            catch (ModelValidationException ex)
            {
                return this.Json(new { result = 0, Message = ex.Message });
            }
            catch (DbEntityValidationException ex)
            {
                this.log.PublishException(new SecureportExceptionDbEntity(ex));

                return this.Json(new { result = 0, Message = ex.Message });
            }
            catch (Exception ex)
            {
                this.log.PublishException(new SecureportException(ex));

                return this.Json(new { result = 0, Message = ex.Message });
            }

        }
        

        /// <summary>
        /// Borrar Declaración Maritima.
        /// </summary>
        [HttpPost]
        public ActionResult EliminarDeclaraMaritima(int Id)
        {
            try
            {

                if (ValidarCategoria(Id,this.repoDeclaraMaritima.Single(x => x.Id == Id)))
                {
                    this.repoDeclaraMaritima.Delete(this.repoDeclaraMaritima.Single(x => x.Id == Id));

                    this.trazas(96, (int)this.UsuarioFrontalSession.Usuario.id, "Se ha realizado la eliminación de un registro en SPMOV_Declara_Maritima con ID" + "(" + Id.ToString() + ")");
                }
                else
                {
                    return this.Json(new { result = false, Message = EliminarCategoria.MessageCategoria.ToDescription() + "  " + this.Description });
                }

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
        public ActionResult AltaDeclaraMaritima(DeclaraMaritimaJson declaraMaritimaJson)
        {
            try
            {
                DeclaraMaritima declaraMaritima = new DeclaraMaritima();

                declaraMaritima.Id_IIPP          = declaraMaritimaJson.Id_IIPP;

                declaraMaritima.IMO              = declaraMaritimaJson.IMO;

                declaraMaritima.Buque            = declaraMaritimaJson.Buque;

                declaraMaritima.Id_Motivo        = declaraMaritimaJson.Id_Motivo;

                declaraMaritima.Fech_Expe        = declaraMaritimaJson.Fech_Expe;

                declaraMaritima.Fech_Ini_Validez = declaraMaritimaJson.Fech_Ini_Validez;

                declaraMaritima.Fech_Fin_Validez = declaraMaritimaJson.Fech_Fin_Validez;

                declaraMaritima.Responsable      = declaraMaritimaJson.Responsable;

                declaraMaritima.Nivel_Buq        = declaraMaritimaJson.Nivel_Buq;

                declaraMaritima.Nivel_IIPP       = declaraMaritimaJson.Nivel_IIPP;

                declaraMaritima.Medidas          = this.CambiarTexto(declaraMaritimaJson.Medidas);
                
                declaraMaritima.Observaciones    = this.CambiarTexto(declaraMaritimaJson.Observaciones);

                declaraMaritima.ID_Usu_alta      = this.UsuarioFrontalSession.Usuario.id;

                declaraMaritima.Fech_Alta        = DateTime.Now;
                
                this.repoDeclaraMaritima.Create(declaraMaritima);

                idDeclara = this.db.DeclaraMaritima.OrderByDescending(u => u.Id).FirstOrDefault().Id;

                this.trazas(93, (int)this.UsuarioFrontalSession.Usuario.id, "Se ha dado de alta un registro en SPMOV_Declara_Maritima con ID" + "(" + idDeclara.ToString() + ")");

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

        [HttpPost]
        public ActionResult EditarDeclaraMaritima(DeclaraMaritimaJson declaraMaritimaJson)
        {
            try
            {
                DeclaraMaritima declaraMaritima = this.repoDeclaraMaritima.Single(x => x.Id == declaraMaritimaJson.Id);

                declaraMaritima.Id_IIPP          = declaraMaritimaJson.Id_IIPP;

                declaraMaritima.IMO              = declaraMaritimaJson.IMO;

                declaraMaritima.Buque            = declaraMaritimaJson.Buque;

                declaraMaritima.Id_Motivo        = declaraMaritimaJson.Id_Motivo;

                declaraMaritima.Fech_Expe        = declaraMaritimaJson.Fech_Expe;

                declaraMaritima.Fech_Ini_Validez = declaraMaritimaJson.Fech_Ini_Validez;

                declaraMaritima.Fech_Fin_Validez = declaraMaritimaJson.Fech_Fin_Validez;

                declaraMaritima.Responsable      = declaraMaritimaJson.Responsable;

                declaraMaritima.Nivel_Buq        = declaraMaritimaJson.Nivel_Buq;

                declaraMaritima.Nivel_IIPP       = declaraMaritimaJson.Nivel_IIPP;

                declaraMaritima.Medidas          = this.CambiarTexto(declaraMaritimaJson.Medidas);

                declaraMaritima.Observaciones    = this.CambiarTexto(declaraMaritimaJson.Observaciones);

                this.repoDeclaraMaritima.Update(declaraMaritima);

                this.trazas(95, (int)this.UsuarioFrontalSession.Usuario.id, "Se ha realizado la modificación de un registro en SPMOV_Declara_Maritima con ID" + "(" + declaraMaritimaJson.Id + ")");

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

        #endregion

        #endregion
    }
}