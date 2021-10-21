namespace SecurePort.Web.Controllers
{
    #region Using
    using SecurePort.Encriptador;
    using SecurePort.Entities.Enums;
    using SecurePort.Entities.Exceptions;
    using SecurePort.Entities.Models;
    using SecurePort.Services.Interfaces;
    using SecurePort.Services.Repository;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.ModelConfiguration;
    using System.Data.Entity.Validation;
    using System.IO;
    using System.Linq;
    using System.Web.Mvc;

    using WebGrease.Css.Extensions;

    #endregion

    public class LoginController : BaseController
    {
        public LoginController(
                                PerfilesPermisosRepository repoPerfilesPermisos,
                                PermisosRepository repoPermisos,
                                GruposRepository repoGrupos,
                                PerfilesRepository repoPerfiles,
                                PerfilesGruposRepository repoPerfilesGrupos,
                                UsuariosRepository repoUsuarios,
                                IServiciosGrupos serviciosGrupos,
                                IServiciosUsuarios serviciosUsuarios,
                                TrazasRepository repoTrazas,
                                ILogger log)
            : base(repoPerfilesPermisos, 
                        repoPermisos, 
                        repoGrupos, 
                        repoPerfiles, 
                        repoPerfilesGrupos, 
                        repoUsuarios, 
                        serviciosGrupos,
                        serviciosUsuarios,
                        repoTrazas, 
                        log)
        {
        }

        #region Accion

        public ActionResult showimage()
        {
            var path = Server.MapPath("~/content/images/");
            var fullPath = Path.Combine(path, "logoPuertosEstado.png");
            return File(fullPath, "image/png", "logoPuertosEstado.png");
        }


        [HttpPost]
        public ActionResult LogOff()
        {
            try
            {

                this.trazas(null, (int)this.UsuarioFrontalSession.Usuario.id, "Se ha Desconectado al sistema el usuario " + this.UsuarioFrontalSession.Usuario.login);

                this.UsuarioFrontalSession.Usuario = null;

                VaciarColeccion();

            }
            catch (Exception ex)
            {
                this.log.PublishException(new SecureportException(ex));

                return this.Json(new { result = false });
            }
            return this.Json(new { result = true });
        }

        [HttpPost]
        public ActionResult ExisteCorreo(string correo)
        {
            try
            {
                Usuarios usuarios = this.repoUsuarios.Single(x => x.email == correo);

                if (correo != string.Empty)
                {
                    if (usuarios != null)
                    {
                        return this.Json(new { result = false, Message = "El Correo Electrónico ya existe." });
                    }
                }
                else
                {
                    return this.Json(new { result = true }, JsonRequestBehavior.AllowGet);
                }

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

            return this.Json(new { result = true }, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public ActionResult CambiarCorreo(string correo)
        {
            try
            {
                Usuarios usuarios = this.repoUsuarios.Single(x => x.email == correo);

                if (correo != string.Empty)
                {
                    if (usuarios == null)
                    {
                        Usuarios _users = this.repoUsuarios.Single(x => x.id == this.UsuarioFrontalSession.Usuario.id);

                        _users.login = correo;

                        _users.email = correo;

                        this.repoUsuarios.Update(_users);

                        Usuarios _usuarios = this.repoUsuarios.Single(x => x.id == this.UsuarioFrontalSession.Usuario.id);

                        this.UsuarioFrontalSession.Usuario = _usuarios;

                        this.trazas(6,this.UsuarioFrontalSession.Usuario.id,"Se ha modificado el correo en SPCON_Usuarios " + correo);
                    }
                    else
                    {
                        return this.Json(new { result = false, Message = "El Correo Electrónico ya existe." });
                    }
                }
                else
                {
                    return this.Json(new { result = true}, JsonRequestBehavior.AllowGet);
                }
                
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

            return this.Json(new { result = true}, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ValidarCambiologin(string password)
        {
            try
            {
                if (password != string.Empty)
                {
                    Usuarios usuarios = this.repoUsuarios.Single(x => x.id == this.UsuarioFrontalSession.Usuario.id);

                    encriptador clave = new encriptador();

                    if (password == clave.DesEncriptacion(usuarios.password))
                    {
                        return this.Json(new { result = true },JsonRequestBehavior.AllowGet);
                    }

                }

                return this.Json(new { result = false, Message = "Contraseña es errónea." }, JsonRequestBehavior.AllowGet);
                
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
        public ActionResult Cambiarlogin(string password)
        {
            try
            {

                if (password != string.Empty)
                {
                    Usuarios usuarios = this.repoUsuarios.Single(x => x.id == this.UsuarioFrontalSession.Usuario.id);

                    encriptador clave = new encriptador();

                    usuarios.password = clave.Encriptacion(password);

                    usuarios.Fech_Password = DateTime.Now;

                    this.repoUsuarios.Update(usuarios);

                    this.trazas(6,this.UsuarioFrontalSession.Usuario.id,"Se ha modificado el password en SPCON_Usuarios " + usuarios.login);
                }
                else
                {
                    return this.Json(new { result = true}, JsonRequestBehavior.AllowGet);
                }
                
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

            return this.Json(new { result = true, Message = "Se cambio la contraseña exitosamente." }, JsonRequestBehavior.AllowGet);
        }

        private void CrearVariableSession(Usuarios usuario)
        {

                    VaciarColeccion();

 
                    IEnumerable<ListGruposPerfilesPermisos> ListGruposPerfilesPermisos = (from t1 in this.repoGrupos.GetAll().Where(x => x.Id == usuario.id_grupo && x.es_activo==true)
                        join t2 in this.repoPerfilesGrupos.GetAll() on t1.Id equals t2.Id_grupo
                        join t4 in this.repoPerfilesPermisos.GetAll() on t2.Id_perfil equals t4.Id_perfil
                        join t6 in this.repoPerfiles.GetAll().Where(x=> x.es_activo==true) on t2.Id_perfil equals t6.Id
                        join t5 in this.repoPermisos.GetAll() on t4.Id_permiso equals t5.Id
                        select
                            new ListGruposPerfilesPermisos
                            {
                                Nombre          = t1.Nombre,
                                IdGrupo         = t1.Id,
                                IdPerfil        = t2.Id_perfil,
                                IdPermiso       = t4.Id_permiso,
                                NombrePermiso   = t5.Nombre,
                                id_grupo_accion = t5.id_grupo_accion,
                                orden           = t5.orden
                            });

                    PermisosViewModel permisos = new PermisosViewModel();

                    //Permisos Usuarios
                    permisos.CON_ACTIVA_USU           = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Usuario.ACTIVAR_USUARIO.ToDescription()).Any() ? false : true;
                    permisos.CON_ADJUNTA_DOC_USU      = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Usuario.ADJUNTAR_DOCUMENTO_USUARIO.ToDescription()).Any() ? false : true;
                    permisos.CON_ALTA_USU             = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Usuario.ALTA_USUARIO.ToDescription()).Any() ? false : true;
                    permisos.CON_MODIF_USU            = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Usuario.MODIFICAR_USUARIO.ToDescription()).Any() ? false : true;
                    permisos.CON_BORRA_USU            = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Usuario.BORRAR_USUARIO.ToDescription()).Any() ? false : true;
                    permisos.CON_CONSULTA_USU         = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Usuario.CONSULTAR_USUARIO.ToDescription()).Any() ? false : true;
                    permisos.CON_LISTA_USU            = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Usuario.LISTAR_OPCION_USUARIO.ToDescription()).Any() ? false : true;
                    permisos.CON_MODIF_PASS_USU       = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Usuario.MODIFICAR_PASS_USUARIO.ToDescription()).Any() ? false : true;
                    permisos.CON_RESET_PASS_USU       = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Usuario.RESETEAR_PASS_USUARIO.ToDescription()).Any() ? false : true;
                    permisos.CON_BORRA_DOC_USU        = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Usuario.BORRAR_DOCUMENTO_USUARIO.ToDescription()).Any() ? false : true;
                    permisos.CON_MODIF_DOC_USU        = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Usuario.MODIFICAR_DOCUMENTO_USUARIO.ToDescription()).Any() ? false : true;
                    permisos.CON_VISUALIZAR_TODOS_USU = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Usuario.VISUALIZAR_TODOS_USUARIOS.ToDescription()).Any() ? false : true;
                    permisos.CON_MODIF_DEPEN_USU      = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Usuario.MODIFICAR_DEPEN_USUARIO.ToDescription()).Any() ? false : true;
                    //Permisos Perfil
                    permisos.CON_ALTA_PERFIL          = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Perfil.ALTA_PERFIL.ToDescription()).Any() ? false : true;
                    permisos.CON_MODIF_PERFIL         = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Perfil.MODIFICAR_PERFIL.ToDescription()).Any() ? false : true;
                    permisos.CON_BORRA_PERFIL         = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Perfil.BORRAR_PERFIL.ToDescription()).Any() ? false : true;
                    permisos.CON_CONSULTA_PERFIL      = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Perfil.CONSULTAR_PERFIL.ToDescription()).Any() ? false : true;
                    permisos.CON_LISTA_PERFIL         = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Perfil.LISTAR_OPCION_PERFIL.ToDescription()).Any() ? false : true;
                    //Permisos Grupo
                    permisos.CON_ALTA_GRUPO           = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Grupo.ALTA_GRUPO.ToDescription()).Any() ? false : true;
                    permisos.CON_MODIF_GRUPO          = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Grupo.MODIFICAR_GRUPO.ToDescription()).Any() ? false : true;
                    permisos.CON_BORRA_GRUPO          = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Grupo.BORRAR_GRUPO.ToDescription()).Any() ? false : true;
                    permisos.CON_CONSULTA_GRUPO       = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Grupo.CONSULTAR_GRUPO.ToDescription()).Any() ? false : true;
                    permisos.CON_LISTA_GRUPO          = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Grupo.LISTAR_OPCION_GRUPO.ToDescription()).Any() ? false : true;
                    //Permisos Organismo
                    permisos.CON_ALTA_ORGANISMO       = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Organismo.ALTA_ORGANISMO.ToDescription()).Any() ? false : true;
                    permisos.CON_MODIF_ORGANISMO      = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Organismo.MODIF_ORGANISMO.ToDescription()).Any() ? false : true;
                    permisos.CON_BORRA_ORGANISMO      = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Organismo.BORRA_ORGANISMO.ToDescription()).Any() ? false : true;
                    permisos.CON_CONSULTA_ORGANISMO   = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Organismo.CONSULTA_ORGANISMO.ToDescription()).Any() ? false : true;
                    permisos.CON_LISTA_ORGANISMO      = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Organismo.LISTA_ORGANISMO.ToDescription()).Any() ? false : true;
                    //Permisos Contacto
                    permisos.CON_ALTA_CONT_OGP        = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Organismo.ALTA_CONT_OGP.ToDescription()).Any() ? false : true;
                    permisos.CON_MODIF_CONT_OGP       = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Organismo.MODIF_CONT_OGP.ToDescription()).Any() ? false : true;
                    permisos.CON_BORRA_CONT_OGP       = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Organismo.BORRA_CONT_OGP.ToDescription()).Any() ? false : true;
                    permisos.CON_CONSULTA_CONT_OGP    = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Organismo.CONSULTA_CONT_OGP.ToDescription()).Any() ? false : true;
                    //Permisos Paises
                    permisos.CON_ALTA_PAIS            = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Pais.ALTA_PAIS.ToDescription()).Any() ? false : true;
                    permisos.CON_MODIF_PAIS           = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Pais.MODIF_PAIS.ToDescription()).Any() ? false : true;
                    permisos.CON_BORRA_PAIS           = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Pais.BORRA_PAIS.ToDescription()).Any() ? false : true;
                    permisos.CON_LISTA_PAIS           = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Pais.LISTA_PAIS.ToDescription()).Any() ? false : true;
                    //Comunidades Autonómas
                    permisos.CON_ALTA_COMAUT          = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Comunidad.ALTA_COMAUT.ToDescription()).Any() ? false : true;
                    permisos.CON_BORRA_COMAUT         = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Comunidad.BORRA_COMAUT.ToDescription()).Any() ? false : true;
                    permisos.CON_CONSULTA_COMAUT      = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Comunidad.CONSULTA_COMAUT.ToDescription()).Any() ? false : true;
                    permisos.CON_LISTA_COMAUT         = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Comunidad.LISTA_COMAUT.ToDescription()).Any() ? false : true;
                    permisos.CON_MODIF_COMAUT         = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Comunidad.MODIF_COMAUT.ToDescription()).Any() ? false : true;
                    //Permisos Provincias
                    permisos.CON_ALTA_Provincia       = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Provincia.ALTA_PROVINCIA.ToDescription()).Any() ? false : true;
                    permisos.CON_MODIF_Provincia      = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Provincia.MODIF_PROVINCIA.ToDescription()).Any() ? false : true;
                    permisos.CON_BORRA_Provincia      = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Provincia.BORRA_PROVINCIA.ToDescription()).Any() ? false : true;
                    permisos.CON_LISTA_Provincia      = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Pais.LISTA_PAIS.ToDescription()).Any() ? false : true;
                    //Permisos Capitania
                    permisos.CON_ALTA_CAPITANIA       = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Capitania.ALTA_CAPITANIA.ToDescription()).Any() ? false : true;
                    permisos.CON_MODIF_CAPITANIA      = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Capitania.MODIF_CAPITANIA.ToDescription()).Any() ? false : true;
                    permisos.CON_BORRA_CAPITANIA      = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Capitania.BORRA_CAPITANIA.ToDescription()).Any() ? false : true;
                    permisos.CON_LISTA_CAPITANIA      = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Capitania.LISTA_CAPITANIA.ToDescription()).Any() ? false : true;
                    //Permisos Bienes
                    permisos.CON_ALTA_BIEN            = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Bien.ALTA_BIEN.ToDescription()).Any() ? false : true;
                    permisos.CON_MODIF_BIEN           = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Bien.MODIF_BIEN.ToDescription()).Any() ? false : true;
                    permisos.CON_BORRA_BIEN           = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Bien.BORRA_BIEN.ToDescription()).Any() ? false : true;
                    permisos.CON_LISTA_BIEN           = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Bien.LISTA_BIEN.ToDescription()).Any() ? false : true;
                    //Permisos Centros
                    permisos.CON_ALTA_CENTRO          = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Centro.ALTA_CENTRO.ToDescription()).Any() ? false : true;
                    permisos.CON_MODIF_CENTRO         = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Centro.MODIF_CENTRO.ToDescription()).Any() ? false : true;
                    permisos.CON_BORRA_CENTRO         = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Centro.BORRA_CENTRO.ToDescription()).Any() ? false : true;
                    permisos.CON_LISTA_CENTRO         = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Centro.LISTAR_CENTRO.ToDescription()).Any() ? false : true;
                    permisos.CON_CONSULTA_CENTRO      = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Centro.CONSULTA_CENTRO.ToDescription()).Any() ? false : true;
                    //Permisos Motivos
                    permisos.CON_ALTA_MOTIVO          = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Motivo.ALTA_MOTIVO_DECLARA.ToDescription()).Any() ? false : true;
                    permisos.CON_MODIF_MOTIVO         = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Motivo.MODIF_MOTIVO_DECLARA.ToDescription()).Any() ? false : true;
                    permisos.CON_BORRA_MOTIVO         = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Motivo.BORRA_MOTIVO_DECLARA.ToDescription()).Any() ? false : true;
                    permisos.CON_LISTA_MOTIVO         = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Motivo.LISTAR_OPCION_MOTIVO_DECLARA.ToDescription()).Any() ? false : true;
                    //Puertos
                    permisos.CON_ALTA_PUERTO          = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Puerto.ALTA_PUERTO.ToDescription()).Any() ? false : true;
                    permisos.CON_MODIF_PUERTO         = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Puerto.MODIF_PUERTO.ToDescription()).Any() ? false : true;
                    permisos.CON_BORRA_PUERTO         = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Puerto.BORRA_PUERTO.ToDescription()).Any() ? false : true;
                    permisos.CON_LISTA_PUERTO         = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Puerto.LISTAR_OPCION_PUERTO.ToDescription()).Any() ? false : true;
                    permisos.CON_CONSULTA_PUERTO      = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Puerto.CONSULTA_PUERTO.ToDescription()).Any() ? false : true;
                    //Comites
                    permisos.CON_ALTA_COMITE          = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Comite.ALTA_COMITE.ToDescription()).Any() ? false : true;
                    permisos.CON_MODIF_COMITE         = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Comite.MODIF_COMITE.ToDescription()).Any() ? false : true;
                    permisos.CON_BORRA_COMITE         = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Comite.BORRA_COMITE.ToDescription()).Any() ? false : true;
                    permisos.CON_LISTA_COMITE         = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Comite.LISTAR_OPCION_COMITES.ToDescription()).Any() ? false : true;
                    permisos.CON_CONSULTA_COMITE      = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Comite.CONSULTA_COMITE.ToDescription()).Any() ? false : true;
                    permisos.CON_ADJUNTA_DOC_COMITE   = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Comite.ADJUNTA_DOC_COMITE.ToDescription()).Any() ? false : true;
                    permisos.CON_MODIF_DOC_COMITE     = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Comite.MODIF_DOC_COMITE.ToDescription()).Any() ? false : true;
                    permisos.CON_BORRA_DOC_COMITE     = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Comite.BORRA_DOC_COMITE.ToDescription()).Any() ? false : true;
                    //Permisos Amenazas
                    permisos.CON_ALTA_AMENAZA         = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Amenaza.ALTA_AMENAZA.ToDescription()).Any() ? false : true;
                    permisos.CON_MODIF_AMENAZA        = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Amenaza.MODIF_AMENAZA.ToDescription()).Any() ? false : true;
                    permisos.CON_BORRA_AMENAZA        = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Amenaza.BORRA_AMENAZA.ToDescription()).Any() ? false : true;
                    permisos.CON_LISTA_AMENAZA        = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Amenaza.LISTAR_OPCION_AMENAZA.ToDescription()).Any() ? false : true;
                    //Permisos Ciudades
                    permisos.CON_ALTA_CIUDAD          = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Ciudad.ALTA_CIUDAD.ToDescription()).Any() ? false : true;
                    permisos.CON_MODIF_CIUDAD         = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Ciudad.MODIF_CIUDAD.ToDescription()).Any() ? false : true;
                    permisos.CON_BORRA_CIUDAD         = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Ciudad.BORRA_CIUDAD.ToDescription()).Any() ? false : true;
                    permisos.CON_LISTA_CIUDAD         = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Ciudad.LISTA_CIUDAD.ToDescription()).Any() ? false : true;
                    //Operadores
                    permisos.CON_ALTA_OPIP_IIPP       = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Instalacion_Operadores.ALTA_OPIP_IIPP.ToDescription()).Any() ? false : true;
                    permisos.CON_MODIF_OPIP_IIPP      = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Instalacion_Operadores.MODIF_OPIP_IIPP.ToDescription()).Any() ? false : true;
                    permisos.CON_BORRA_OPIP_IIPP      = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Instalacion_Operadores.BORRA_OPIP_IIPP.ToDescription()).Any() ? false : true;
                    permisos.CON_CONSULTA_OPIP_IIPP   = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Instalacion_Operadores.CONSULTA_OPIP_IIPP.ToDescription()).Any() ? false : true;     
                    //Doc Operadores
                    permisos.CON_ADJUNTA_DOC_IIPP      = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Instalacion_Operadores.ADJUNTA_DOC_IIPP.ToDescription()).Any() ? false : true;
                    permisos.CON_MODIF_DOC_IIPP        = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Instalacion_Operadores.MODIF_DOC_IIPP.ToDescription()).Any() ? false : true;
                    permisos.CON_BORRA_DOC_IIPP        = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Instalacion_Operadores.BORRA_DOC_IIPP.ToDescription()).Any() ? false : true;
                    permisos.CON_CONSULTA_OPIP_IIPP    = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Instalacion_Operadores.CONSULTA_OPIP_IIPP.ToDescription()).Any() ? false : true;     
                    //Formaciones
                    permisos.CON_LISTA_FORMACION       = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Formacion.LISTA_FORMACION.ToDescription()).Any() ? false : true;
                    permisos.CON_ALTA_FORMACION        = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Formacion.ALTA_FORMACION.ToDescription()).Any() ? false : true;
                    permisos.CON_MODIF_FORMACION       = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Formacion.MODIF_FORMACION.ToDescription()).Any() ? false : true;
                    permisos.CON_BORRA_FORMACION       = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Formacion.BORRA_FORMACION.ToDescription()).Any() ? false : true;
                    permisos.CON_CONSULTA_FORMACION    = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Formacion.CONSULTA_FORMACION.ToDescription()).Any() ? false : true;
                    permisos.CON_ADJUNTA_DOC_FORMACION = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Formacion.ADJUNTA_DOC_FORMACION.ToDescription()).Any() ? false : true;
                    permisos.CON_BORRA_DOC_FORMACION   = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Formacion.BORRA_DOC_FORMACION.ToDescription()).Any() ? false : true;
                    permisos.CON_MODIF_DOC_FORMACION   = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Formacion.MODIF_DOC_FORMACION.ToDescription()).Any() ? false : true;
                    //Operadores puerto
                    permisos.CON_ALTA_OPP_PUERTO       = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Puerto.ALTA_OPP_PUERTO.ToDescription()).Any() ? false : true;
                    permisos.CON_MODIF_OPP_PUERTO      = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Puerto.MODIF_OPP_PUERTO.ToDescription()).Any() ? false : true;
                    permisos.CON_BORRA_OPP_PUERTO      = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Puerto.BORRA_OPP_PUERTO.ToDescription()).Any() ? false : true;
                    permisos.CON_CONSULTA_OPP_PUERTO   = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Puerto.CONSULTA_OPP_PUERTO.ToDescription()).Any() ? false : true;
                    //Practicas
                    permisos.CON_ALTA_PRACTICA          = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Practica.ALTA_PRACTICA.ToDescription()).Any() ? false : true;
                    permisos.CON_MODIF_PRACTICA         = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Practica.MODIF_PRACTICA.ToDescription()).Any() ? false : true;
                    permisos.CON_BORRA_PRACTICA         = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Practica.BORRA_PRACTICA.ToDescription()).Any() ? false : true;
                    permisos.CON_CONSULTA_PRACTICA      = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Practica.CONSULTA_PRACTICA.ToDescription()).Any() ? false : true;
                    permisos.CON_ADJUNTA_DOC_PRACTICA   = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Practica.ADJUNTA_DOC_PRACTICA.ToDescription()).Any() ? false : true;
                    permisos.CON_BORRA_DOC_PRACTICA     = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Practica.BORRA_DOC_PRACTICA.ToDescription()).Any() ? false : true;
                    permisos.CON_MODIF_DOC_PRACTICA     = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Practica.MODIF_DOC_PRACTICA.ToDescription()).Any() ? false : true;
                    permisos.CON_LISTA_PRACTICA         = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Practica.LISTA_PRACTICA.ToDescription()).Any() ? false : true;
                    //Incidentes
                    permisos.CON_ALTA_INCIDENTE         = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Incidente.ALTA_INCIDENTE.ToDescription()).Any() ? false : true;
                    permisos.CON_MODIF_INCIDENTE        = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Incidente.MODIF_INCIDENTE.ToDescription()).Any() ? false : true;
                    permisos.CON_BORRA_INCIDENTE        = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Incidente.BORRA_INCIDENTE.ToDescription()).Any() ? false : true;
                    permisos.CON_CONSULTA_INCIDENTE     = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Incidente.CONSULTA_INCIDENTE.ToDescription()).Any() ? false : true;
                    permisos.CON_LISTA_INCIDENTE        = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Incidente.LISTA_INCIDENTE.ToDescription()).Any() ? false : true;
                    permisos.CON_ADJUNTA_DOC_INCIDENTE  = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Incidente.ADJUNTA_DOC_INCIDENTE.ToDescription()).Any() ? false : true;
                    permisos.CON_BORRA_DOC_INCIDENTE    = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Incidente.BORRA_DOC_INCIDENTE.ToDescription()).Any() ? false : true;
                    permisos.CON_MODIF_DOC_INCIDENTE    = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Incidente.MODIF_DOC_INCIDENTE.ToDescription()).Any() ? false : true;
                    //Auditorias
                    permisos.CON_ALTA_AUDITORIA         = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Auditoria.ALTA_AUDITORIA.ToDescription()).Any() ? false : true;
                    permisos.CON_MODIF_AUDITORIA        = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Auditoria.MODIF_AUDITORIA.ToDescription()).Any() ? false : true;
                    permisos.CON_BORRA_AUDITORIA        = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Auditoria.BORRA_AUDITORIA.ToDescription()).Any() ? false : true;
                    permisos.CON_CONSULTA_AUDITORIA     = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Auditoria.CONSULTA_AUDITORIA.ToDescription()).Any() ? false : true;
                    permisos.CON_ADJUNTA_DOC_AUDITORIA  = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Auditoria.ADJUNTA_DOC_AUDITORIA.ToDescription()).Any() ? false : true;
                    permisos.CON_BORRA_DOC_AUDITORIA    = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Auditoria.BORRA_DOC_AUDITORIA.ToDescription()).Any() ? false : true;
                    permisos.CON_MODIF_DOC_AUDITORIA    = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Auditoria.MODIF_DOC_AUDITORIA.ToDescription()).Any() ? false : true;
                    permisos.CON_LISTA_AUDITORIA        = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Auditoria.LISTA_AUDITORIA.ToDescription()).Any() ? false : true;
                    //Niveles
                    permisos.CON_LISTA_NIVEL            = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Nivel.LISTA_NIVEL.ToDescription()).Any() ? false : true;
                    permisos.CON_ALTA_NIVEL             = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Nivel.ALTA_NIVEL.ToDescription()).Any() ? false : true;
                    permisos.CON_MODIF_NIVEL            = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Nivel.MODIF_NIVEL.ToDescription()).Any() ? false : true;
                    permisos.CON_BORRA_NIVEL            = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Nivel.BORRA_NIVEL.ToDescription()).Any() ? false : true;
                    permisos.CON_CONSULTA_NIVEL         = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Nivel.CONSULTA_NIVEL.ToDescription()).Any() ? false : true;
                    permisos.CON_ADJUNTA_DOC_NIVEL      = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Nivel.ADJUNTA_DOC_NIVEL.ToDescription()).Any() ? false : true;
                    permisos.CON_BORRA_DOC_NIVEL        = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Nivel.BORRA_DOC_NIVEL.ToDescription()).Any() ? false : true;
                    permisos.CON_MODIF_DOC_NIVEL        = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Nivel.MODIF_DOC_NIVEL.ToDescription()).Any() ? false : true;
                    //Mantenimiento
                    permisos.CON_LISTA_MANTEN           = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Mantenimiento.LISTA_MANTEN.ToDescription()).Any() ? false : true;
                    permisos.CON_MODIF_MANTEN           = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Mantenimiento.MODIF_MANTEN.ToDescription()).Any() ? false : true;
                    permisos.CON_BORRA_MANTEN           = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Mantenimiento.BORRA_MANTEN.ToDescription()).Any() ? false : true;
                    permisos.CON_CONSULTA_MANTEN        = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Mantenimiento.CONSULTA_MANTEN.ToDescription()).Any() ? false : true;
                    permisos.CON_ADJUNTA_DOC_MANTEN     = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Mantenimiento.ADJUNTA_DOC_MANTEN.ToDescription()).Any() ? false : true;
                    permisos.CON_BORRA_DOC_MANTEN       = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Mantenimiento.BORRA_DOC_MANTEN.ToDescription()).Any() ? false : true;
                    permisos.CON_MODIF_DOC_MANTEN       = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Mantenimiento.MODIF_DOC_MANTEN.ToDescription()).Any() ? false : true;
                    //Instalacion
                    permisos.CON_LISTA_IIPP             = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Instalacion_Operadores.LISTA_IIPP.ToDescription()).Any() ? false : true;
                    permisos.CON_ALTA_IIPP              = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Instalacion_Operadores.ALTA_IIPP.ToDescription()).Any() ? false : true;
                    permisos.CON_BORRA_IIPP             = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Instalacion_Operadores.BORRA_IIPP.ToDescription()).Any() ? false : true;
                    permisos.CON_MODIF_IIPP             = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Instalacion_Operadores.MODIF_IIPP.ToDescription()).Any() ? false : true;
                    permisos.CON_CONSULTA_IIPP          = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Instalacion_Operadores.CONSULTA_IIPP.ToDescription()).Any() ? false : true;
                    //TiposInstalacion
                    permisos.CON_LISTA_IIPP             = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == TipoInstalacion.LISTA_TIPO_IIPP.ToDescription()).Any() ? false : true;
                    permisos.CON_ALTA_IIPP              = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == TipoInstalacion.ALTA_TIPO_IIPP.ToDescription()).Any() ? false : true;
                    permisos.CON_BORRA_IIPP             = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == TipoInstalacion.BORRA_TIPO_IIPP.ToDescription()).Any() ? false : true;
                    permisos.CON_MODIF_IIPP             = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == TipoInstalacion.MODIF_TIPO_IIPP.ToDescription()).Any() ? false : true;
                    //Comunicaciones
                    permisos.CON_LISTA_COMUNICA         = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Comunicacion.LISTA_COMUNICA.ToDescription()).Any() ? false : true;
                    permisos.CON_MODIF_COMUNICA         = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Comunicacion.MODIF_COMUNICA.ToDescription()).Any() ? false : true;
                    permisos.CON_BORRA_COMUNICA         = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Comunicacion.BORRA_COMUNICA.ToDescription()).Any() ? false : true;
                    permisos.CON_CONSULTA_COMUNICA      = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Comunicacion.CONSULTA_COMUNICA.ToDescription()).Any() ? false : true;
                    permisos.CON_ADJUNTA_DOC_COMUNICA   = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Comunicacion.ADJUNTA_DOC_COMUNICA.ToDescription()).Any() ? false : true;
                    permisos.CON_BORRA_DOC_COMUNICA     = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Comunicacion.BORRA_DOC_COMUNICA.ToDescription()).Any() ? false : true;
                    permisos.CON_MODIF_DOC_COMUNICA     = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Comunicacion.MODIF_DOC_COMUNICA.ToDescription()).Any() ? false : true;
                    //DeclaraMaritima
                    permisos.CON_LISTA_DECLARA_MARIT    = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Declara_Maritima.LISTA_DECLARA.ToDescription()).Any() ? false : true;
                    permisos.CON_MODIF_DECLARA_MARIT    = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Declara_Maritima.MODIF_DECLARA.ToDescription()).Any() ? false : true;
                    permisos.CON_BORRA_DECLARA_MARIT    = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Declara_Maritima.BORRA_DOC_DECLARA.ToDescription()).Any() ? false : true;
                    permisos.CON_CONSULTA_DECLARA_MARIT = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Declara_Maritima.CONSULTA_DECLARA.ToDescription()).Any() ? false : true;
                    permisos.CON_BORRA_DECLARA_MARIT    = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Declara_Maritima.ADJUNTA_DOC_DECLARA.ToDescription()).Any() ? false : true;
                    permisos.CON_BORRA_DOC_DECLARA      = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Declara_Maritima.BORRA_DOC_DECLARA.ToDescription()).Any() ? false : true;
                    permisos.CON_MODIF_DOC_DECLARA      = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Declara_Maritima.MODIF_DOC_DECLARA.ToDescription()).Any() ? false : true;
                    //Registro Gisis
                    permisos.CON_LISTA_GISIS            = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Gisis.LISTA_GISIS.ToDescription()).Any() ? false : true;
                    permisos.CON_ALTA_GISIS             = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Gisis.ALTA_GISIS.ToDescription()).Any() ? false : true;
                    permisos.CON_BORRA_GISIS            = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Gisis.BORRA_GISIS.ToDescription()).Any() ? false : true;
                    permisos.CON_MODIF_GISIS            = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Gisis.MODIF_GISIS.ToDescription()).Any() ? false : true;
                    //Gestión plantillas
                    permisos.CON_LISTA_PLANTILLA        = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Gestion_Plantilla.LISTA_PLANTILLA.ToDescription()).Any() ? false : true;
                    //Gestión Documentacion
                    permisos.CON_LISTA_DOCUMENTO        = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Gestion_Documento.LISTA_DOCUMENTO.ToDescription()).Any() ? false : true;
                    //Procedimientos de Autoridades Portuarias
                    permisos.CON_LISTA_PROCEDIM_AP      = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Procedimiento_AP.LISTA_PROCEDIM_AP.ToDescription()).Any() ? false : true;
                    permisos.CON_ALTA_PROCEDIM_AP       = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Procedimiento_AP.ALTA_PROCEDIM_AP.ToDescription()).Any() ? false : true;
                    permisos.CON_MODIF_PROCEDIM_AP      = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Procedimiento_AP.MODIF_PROCEDIM_AP.ToDescription()).Any() ? false : true;
                    permisos.CON_BORRA_PROCEDIM_AP      = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Procedimiento_AP.BORRA_PROCEDIM_AP.ToDescription()).Any() ? false : true;
                    permisos.CON_CONSULTA_PROCEDIM_AP   = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Procedimiento_AP.CONSULTA_PROCEDIM_AP.ToDescription()).Any() ? false : true;
                    permisos.CON_ADJUNTA_DOC_PROCEDIM_AP = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Procedimiento_AP.ADJUNTA_DOC_PROCEDIM_AP.ToDescription()).Any() ? false : true;
                    permisos.CON_BORRA_DOC_PROCEDIM_AP  = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Procedimiento_AP.BORRA_DOC_PROCEDIM_AP.ToDescription()).Any() ? false : true;
                    permisos.CON_MODIF_DOC_PROCEDIM_AP  = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Procedimiento_AP.MODIF_DOC_PROCEDIM_AP.ToDescription()).Any() ? false : true;
                    //Procedimientos de OPPE
                    permisos.CON_LISTA_PROCEDIM_OPPE    = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Procedimiento_OPPE.LISTA_PROCEDIM_OPPE.ToDescription()).Any() ? false : true;
                    permisos.CON_ALTA_PROCEDIM_OPPE     = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Procedimiento_OPPE.ALTA_PROCEDIM_OPPE.ToDescription()).Any() ? false : true;
                    permisos.CON_MODIF_PROCEDIM_OPPE    = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Procedimiento_OPPE.MODIF_PROCEDIM_OPPE.ToDescription()).Any() ? false : true;
                    permisos.CON_BORRA_PROCEDIM_OPPE    = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Procedimiento_OPPE.BORRA_PROCEDIM_OPPE.ToDescription()).Any() ? false : true;
                    permisos.CON_CONSULTA_PROCEDIM_OPPE = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Procedimiento_OPPE.CONSULTA_PROCEDIM_OPPE.ToDescription()).Any() ? false : true;
                    permisos.CON_ADJUNTA_DOC_PROCEDIM_OPPE = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Procedimiento_OPPE.ADJUNTA_DOC_PROCEDIM_OPPE.ToDescription()).Any() ? false : true;
                    permisos.CON_BORRA_DOC_PROCEDIM_OPPE = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Procedimiento_OPPE.BORRA_DOC_PROCEDIM_OPPE.ToDescription()).Any() ? false : true;
                    permisos.CON_MODIF_DOC_PROCEDIM_OPPE = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Procedimiento_OPPE.MODIF_DOC_PROCEDIM_OPPE.ToDescription()).Any() ? false : true;
                    //Procedimientos de OPPE_AP
                    permisos.CON_LISTA_PROCEDIM_OPPE_AP  = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Procedimiento_OPPE_AP.LISTA_PROCEDIM_OPPE_AP.ToDescription()).Any() ? false : true;
                    permisos.CON_ALTA_PROCEDIM_OPPE_AP   = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Procedimiento_OPPE_AP.ALTA_PROCEDIM_OPPE_AP.ToDescription()).Any() ? false : true;
                    permisos.CON_MODIF_PROCEDIM_OPPE_AP  = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Procedimiento_OPPE_AP.MODIF_PROCEDIM_OPPE_AP.ToDescription()).Any() ? false : true;
                    permisos.CON_BORRA_PROCEDIM_OPPE_AP  = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Procedimiento_OPPE_AP.BORRA_PROCEDIM_OPPE_AP.ToDescription()).Any() ? false : true;
                    permisos.CON_CONSULTA_PROCEDIM_OPPE_AP = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Procedimiento_OPPE_AP.CONSULTA_PROCEDIM_OPPE_AP.ToDescription()).Any() ? false : true;
                    permisos.CON_ADJUNTA_DOC_PROCEDIM_OPPE_AP = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Procedimiento_OPPE_AP.ADJUNTA_DOC_PROCEDIM_OPPE_AP.ToDescription()).Any() ? false : true;
                    permisos.CON_BORRA_DOC_PROCEDIM_OPPE_AP = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Procedimiento_OPPE_AP.BORRA_DOC_PROCEDIM_OPPE_AP.ToDescription()).Any() ? false : true;
                    permisos.CON_MODIF_DOC_PROCEDIM_OPPE_AP = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Procedimiento_OPPE_AP.MODIF_DOC_PROCEDIM_OPPE_AP.ToDescription()).Any() ? false : true;
                    // Evaluaciones
                    permisos.EVAL_LISTA_IIPP             = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Mov_Evaluaciones.LISTA_IIPP.ToDescription()).Any() ? false : true;
                    permisos.EVAL_GESTION_IIPP           = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Mov_Evaluaciones.GESTION_IIPP.ToDescription()).Any() ? false : true;
                    permisos.EVAL_DETALLE_IIPP           = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Mov_Evaluaciones.DETALLE_IIPP.ToDescription()).Any() ? false : true;

                    permisos.EVAL_LISTA_EVAL             = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Mov_Evaluaciones.LISTA_EVAL.ToDescription()).Any() ? false : true;
                    permisos.EVAL_ALTA_EVAL              = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Mov_Evaluaciones.ALTA_EVAL.ToDescription()).Any() ? false : true;
                    permisos.EVAL_MODIF_EVAL             = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Mov_Evaluaciones.MODIF_EVAL.ToDescription()).Any() ? false : true;
                    permisos.EVAL_CONSULTA_EVAL          = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Mov_Evaluaciones.CONSULTA_EVAL.ToDescription()).Any() ? false : true;
                    permisos.EVAL_BORRA_EVAL             = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Mov_Evaluaciones.BORRA_EVAL.ToDescription()).Any() ? false : true;
                    permisos.EVAL_COMP_EVAL              = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Mov_Evaluaciones.COMP_EVAL.ToDescription()).Any() ? false : true;
                    permisos.EVAL_RIESGOS_EVAL           = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Mov_Evaluaciones.RIESGOS_EVAL.ToDescription()).Any() ? false : true;
            
                    // Evaluaciones documentos
                    permisos.EVAL_ADJUNTA_DOC_EVAL       = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Mov_Evaluaciones.ADJUNTA_DOC_EVAL.ToDescription()).Any() ? false : true;
                    permisos.EVAL_BORRA_DOC_EVAL         = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Mov_Evaluaciones.BORRA_DOC_EVAL.ToDescription()).Any() ? false : true;
                    permisos.EVAL_MODIF_DOC_EVAL         = ListGruposPerfilesPermisos.Where(x => x.NombrePermiso == Mov_Evaluaciones.MODIF_DOC_EVAL.ToDescription()).Any() ? false : true;

                    
                    List<DependenciasUsuario> ListDependenciasDepenUsuarios = this._serviciosUsuarios.GetDependenciaUsuarios(usuario.id,usuario.categoria);
            
                   
                    //Maestros
                    List<ListGruposPerfilesPermisos> ListMaestros = new List<ListGruposPerfilesPermisos>();
                     ListGruposPerfilesPermisos.ToList().ForEach(
                      p =>
                    {
                   
                        foreach (string maestro in Enum.GetNames(typeof(Maestros)))
                        {
                            if (p.NombrePermiso == maestro)
                            {
                                ListGruposPerfilesPermisos List = new ListGruposPerfilesPermisos();
                                List.Nombre = p.Nombre;
                                List.IdGrupo = p.IdGrupo;
                                List.IdPerfil = p.IdPerfil;
                                List.IdPermiso = p.IdPerfil;
                                List.NombrePermiso = p.NombrePermiso;
                                List.id_grupo_accion = p.id_grupo_accion;
                                List.orden = p.orden;
                                if (ListMaestros.Where(x => x.NombrePermiso == p.NombrePermiso).Any() == false)
                                {
                                    ListMaestros.Add(List);
                                }

                            }
                        }

                    });
            
                    UsuarioFrontal UsuarioFrontal = new UsuarioFrontal()
                                                    {
                                                        Usuario                       = usuario,
                                                        SupportedBrowser              = this.Browser,
                                                        ListGruposPerfilesPermisos    = ListGruposPerfilesPermisos.GroupBy(i => i.NombrePermiso).Select(group => group.First()).OrderBy(i=> i.IdPermiso),
                                                        fichero                       = string.Empty,
                                                        Path                          = HttpContext.Request.ApplicationPath + "/Exportar/",
                                                        Path1                         = HttpContext.Request.ApplicationPath + "/Help/",
                                                        version                       = ConfigurationManager.AppSettings["Version"],
                                                        permisosViewModel             = permisos,
                                                        ListDependenciasDepenUsuarios = ListDependenciasDepenUsuarios,
                                                        ListMaestros = ListMaestros
                                                    };

                     this.UsuarioFrontalSession = UsuarioFrontal;
        }

        [HttpPost]
        public JsonResult AceptarAvisoLegal(string email)
        {
            
            try
            {
                
                Usuarios usuario = this.repoUsuarios.Single(x => x.email == email);
                
                usuario.aviso_legal = true;
                
                this.repoUsuarios.Update(usuario);
                
                CrearVariableSession(usuario);    
            }
             catch (Exception ex)
             {
                 this.log.PublishException(new SecureportException(ex));

                 return this.Json(new { result = false, Message = ex.Message });
             }

            return this.Json(new { result = true});
        }

        [HttpPost]
        public JsonResult validarlogin(string email, string password)
        {
            
            var numdiascambio = ConfigurationManager.AppSettings["numdiascambio"];

            var numdiasantes = Convert.ToInt32(ConfigurationManager.AppSettings["numdiasantes"]);

            var añostituloOPIP = Convert.ToInt32(ConfigurationManager.AppSettings["tiempoOPIP"]);

            var numdiasavisoOPIP = Convert.ToInt32(ConfigurationManager.AppSettings["tiempoantesOPIP"]);

            List<Usuarios> users = new List<Usuarios> { new Usuarios { id = 1, nombre = "" } };

            try
            {

                Usuarios usuario = this.repoUsuarios.Single(x => x.email == email);
                
                encriptador clave = new encriptador();

                if (usuario == null)
                {
                    users = new List<Usuarios> { new Usuarios { id = 3, nombre = "El login no existe en el sistema." } };

                    return this.Json(users,JsonRequestBehavior.AllowGet);
                }
                else if (usuario.es_activo != true)
                {
                    users = new List<Usuarios> { new Usuarios { id = 2, nombre = "El Usuario No esta Activo." } };

                    return this.Json(users, JsonRequestBehavior.AllowGet);
                }
                else if (clave.DesEncriptacion(usuario.password) != password)
                {
                    users = new List<Usuarios> { new Usuarios { id = 5, nombre = "La Contraseña es inválida." } };

                    return this.Json(users, JsonRequestBehavior.AllowGet);
                }
                TimeSpan diascaduca = usuario.Fech_Password.Date - System.DateTime.Now.Date;

                if (System.DateTime.Now > usuario.Fech_Password.AddDays(Convert.ToInt32(numdiascambio))){

                    users = new List<Usuarios> { new Usuarios { id = 5, nombre = "Su contraseña ha caducado, contacte con el administrador." } };

                    return this.Json(users, JsonRequestBehavior.AllowGet);
                }
                
                int dias = (int)diascaduca.TotalDays + (Convert.ToInt32(numdiascambio)); 
                
                if (dias <= numdiasantes )
                {
                    users = new List<Usuarios> { new Usuarios { id = 5, nombre = "Su contraseña caduca en " + dias + " día" } };
                }
                 
                CrearVariableSession(usuario);

                if (!this.UsuarioFrontalSession.ListDependenciasDepenUsuarios.Any())
                {
                    users = new List<Usuarios> { new Usuarios { id = 3, nombre = "El Usuario no tiene Dependencias Asignadas." } };

                    this.UsuarioFrontalSession.Usuario = null;

                    return this.Json(users, JsonRequestBehavior.AllowGet);
                }
                if (!this.UsuarioFrontalSession.ListGruposPerfilesPermisos.Any())
                {
                    users = new List<Usuarios> { new Usuarios { id = 3, nombre = "El Usuario no tiene Permisos Asignados." } };

                    this.UsuarioFrontalSession.Usuario = null;

                    return this.Json(users, JsonRequestBehavior.AllowGet);
                }
                if (usuario.aviso_legal != true)
                {
                    users = new List<Usuarios> { new Usuarios { id = 4, nombre = "Aceptar las Condiciones." } };

                    return this.Json(users, JsonRequestBehavior.AllowGet);
                }
                if (usuario.es_opip == true)
                {
                    // Validar que no han pasado más de 5 años desde la expedición OPIP
                    // Avisar 180 días antes que va a caducar el que no han pasado más de 5 años desde la expedición OPIP

                    DateTime tcaducado;
                    DateTime taviso;
                    tcaducado =  usuario.fech_exp_opip.Value.AddYears(añostituloOPIP);
                    taviso = tcaducado.AddDays(-numdiasavisoOPIP);

                    if (DateTime.Now.Date > tcaducado || DateTime.Now.Date == tcaducado)
                    {
                        users = new List<Usuarios> { new Usuarios { id = 5, nombre = "Su título OPIP ha caducado, contacte con el administrador." } };
                        return this.Json(users, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        if (DateTime.Now.Date > taviso)
                        {
                            TimeSpan diascaducadosOPIP = System.DateTime.Now.Date - taviso ;
                            int AvisoCaducidadOPIP = 180 - diascaducadosOPIP.Days;
                            users = new List<Usuarios> { new Usuarios { id = 5, nombre = "Su título OPIP caduca en " + AvisoCaducidadOPIP + " día" } };
                            return this.Json(users, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
                
            }
            catch (Exception ex)
            {
                this.log.PublishException(new SecureportException(ex));

                users = new List<Usuarios> { new Usuarios { id = 3, nombre = "Instancia de DB 'SecurePort' no existe." } };

                return this.Json(users, JsonRequestBehavior.AllowGet);
            }

            return this.Json(users,JsonRequestBehavior.AllowGet);
        }

        private void VaciarColeccion()
        {
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

            if (this.listaPermisosAsignadosSession.Any())
            {
                this.listaPermisosAsignadosSession.ToList().ForEach(
                    p =>
                    {
                        this.listaPermisosAsignadosSession.Remove(p);
                    });
            }
        }

        #endregion

    }
    
}