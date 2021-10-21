 namespace SecurePort.Web.Controllers
{
    #region Using

    using System.Reflection;
    using SecurePort.Entities;
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
    using System.Data.Entity.Validation;
    using System.Linq;
    using System.Net;
    using System.Net.Mail;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Web;
    using System.Web.Mvc;

    #endregion

    public class BaseController : Controller
    {
        #region Atributos

        protected readonly IServiciosDeclaraMaritima _serviciosDeclaraMaritima;

        protected readonly IServiciosMantenimiento _serviciosMantenimiento;

        protected readonly IServiciosDocumentos _serviciosDocumentos;
        
        protected readonly IServiciosAuditorias _serviciosAuditorias;

        protected readonly IServiciosIncidentes _serviciosIncidentes;

        protected readonly IServiciosPracticas _serviciosPracticas;

        protected readonly IServiciosFormaciones _serviciosFormaciones;

        protected readonly IServiciosComites _serviciosComites;

        protected readonly ComitesRepository repoComites;

        protected readonly PuertosRepository repoPuertos;

        protected readonly CentrosRepository repoCentros;

        protected readonly CapitaniasRepository repoCapitanias;

        protected readonly BienesRepository repoBienes;

        protected readonly Comunidades_AutonomasRepository repoComunidades_Autonomas;

        protected readonly IServiciosOrganismo _serviciosOrganismos;

        protected readonly IServiciosBienes _serviciosBienes;

        protected readonly Contactos_OrganismoRepository repoContactos_Organismo;

        protected readonly CiudadesRepository repoCiudades;

        protected readonly ProvinciasRepository repoProvincias;

        protected readonly OrganismoRepository repoOrganismo;

        protected readonly TrazasRepository repoTrazas;

        protected readonly DocumentosUsuarioRepository repoDocumentosUsuario;

        protected readonly DependenciaUsuarioRepository repoDependenciaUsuario;

        protected readonly IServiciosUsuarios _serviciosUsuarios;

        protected readonly SecurePortContext db = new SecurePortContext();

        protected readonly UsuariosRepository repoUsuarios;

        protected readonly GruposRepository repoGrupos;

        protected readonly PerfilesRepository repoPerfiles;

        protected readonly PerfilesGruposRepository repoPerfilesGrupos;

        protected readonly PerfilesPermisosRepository repoPerfilesPermisos;

        protected readonly GestionpermisosRepository repogestionpermisos;

        protected readonly PermisosRepository repoPermisos;

        protected readonly TiposDocumentosRepository repoTiposDocumentos;
        
        protected readonly IServiciosGrupos _serviciosGrupos;

        protected readonly IServiciosPuertos _serviciosPuertos;

        protected readonly IServiciosNiveles _serviciosNiveles;

        protected readonly ILogger log;

        public ExceptionContext filterContext { get; set; }

        protected readonly IServiciosPaisesProvincias _serviciosPaisesProvincias;

        protected readonly PaisesRepository repoPaises;

        protected readonly TipoInstalacionRepository repoTipoInstalaciones;

        protected readonly IServiciosTipoInstalacion _serviciosTipoInstalacion;

        protected readonly IServiciosCentros _serviciosCentros;

        protected readonly AmenazasRepository repoAmenazas;

        protected readonly IServiciosAmenazas _serviciosAmenazas;

        protected readonly MotivosRepository repoMotivos;

        protected readonly IServiciosMotivos _serviciosMotivos;

        protected readonly InstalacionesRepository repoInstalaciones;

        protected readonly OperadoresRepository repoOperadores;

        protected readonly FormacionesRepository repoFormaciones;

        protected readonly OperadoresPuertoRepository repoOperadoresPuerto;

        protected readonly PracticasRepository repoPracticas;

        protected readonly IncidentesRepository repoIncidentes;

        protected readonly AuditoriasRepository repoAuditorias;

        protected readonly NivelesRepository repoNiveles;

        protected readonly Comunica_CentroRepository repoComunica_Centro;

        protected readonly MantenimientosRepository repoMantenimiento;

        protected readonly Det_NivelRepository repoDet_Nivel;

        protected readonly ComunicacionesRepository repoComunicacion;

        protected readonly IServiciosComunicaciones _serviciosComunicacion;

        protected readonly DeclaraMaritimaRepository repoDeclaraMaritima;

        protected readonly Det_IncidenteRepository repoDet_Incidente;

        protected readonly Det_PracticaRepository repoDet_Practica;

        protected readonly IServiciosGisis _serviciosGisis;

        protected readonly GisisRepository repoGisis;

        protected readonly IServiciosVinculos  _serviciosVinculos;

        protected readonly VinculosRepository repoVinculos;

        protected readonly IServiciosProcedimientos _serviciosProcedimientos;

        protected readonly ProcedimientosRepository repoProcedimientos;

        protected readonly MOV_Detalle_InstalacionRepository repoMovDetalleInstalaciones;

        protected readonly IServiciosEvaluaciones _serviciosEvaluaciones;

        protected readonly EvaluacionesRepository repoEvaluaciones;

        protected readonly Versiones_EvaluacionRepository repoVersionesEvaluacion;

        protected readonly Detalle_EvaluacionRepository repoDetalle_Evaluacion;

        protected readonly Historico_EvaluacionRepository repoHistorico_Evaluacion;

        #endregion

        #region constructores

        
        protected BaseController(ILogger logger,TrazasRepository repoTrazas)
        {
            this.log = logger;
            this.repoTrazas = repoTrazas;
        }

        protected BaseController(IServiciosBienes serviciosBienes,
                                 BienesRepository repoBienes,
                                 IServiciosTipoInstalacion serviciosTipoInstalacion,
                                 TrazasRepository repoTrazas,
                                 ILogger logger)
        {
            this._serviciosBienes = serviciosBienes;
            this.repoBienes = repoBienes;
            this._serviciosTipoInstalacion = serviciosTipoInstalacion;
            this.repoTrazas = repoTrazas;
            this.log = logger;
        }
        
        protected BaseController(GruposRepository repoGrupos, 
                                 IServiciosUsuarios serviciosUsuarios, 
                                 ILogger logger, 
                                 UsuariosRepository repoUsuarios)
        {
            this._serviciosUsuarios = serviciosUsuarios;
            this.log = logger;
            this.repoUsuarios = repoUsuarios;
            this.repoGrupos = repoGrupos;
        }

        protected BaseController(
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
                        ILogger logger)
        {
            this._serviciosUsuarios = serviciosUsuarios;
            this._serviciosGrupos = serviciosGrupos;
            this.log = logger;
            this.repoUsuarios = repoUsuarios;
            this.repoGrupos = repoGrupos;
            this.repoDependenciaUsuario = repoDependenciaUsuario;
            this._serviciosPuertos = serviciosPuertos;
            this.repoDocumentosUsuario = repoDocumentosUsuario;
            this._serviciosDocumentos = servicioDocumentos;
            this.repoTrazas = repoTrazas;
            this._serviciosOrganismos = serviciosOrganismos;
            this._serviciosPuertos = servicioPuertos;
        }

        protected BaseController(
                    PerfilesPermisosRepository repoPerfilesPermisos,
                    PermisosRepository repoPermisos,
                    GruposRepository repoGrupos,
                    PerfilesRepository repoPerfiles,
                    PerfilesGruposRepository repoPerfilesGrupos,
                    IServiciosGrupos serviciosGrupos,
                    IServiciosUsuarios serviciosUsuarios,
                    TrazasRepository repoTrazas,
                    ILogger logger)
        {
            this.log = logger;
            this.repoPerfiles = repoPerfiles;
            this._serviciosGrupos = serviciosGrupos;
            this.repoPerfilesGrupos = repoPerfilesGrupos;
            this.repoGrupos = repoGrupos;
            this.repoPermisos = repoPermisos;
            this.repoPerfilesPermisos = repoPerfilesPermisos;
            this._serviciosUsuarios = serviciosUsuarios;
            this.repoTrazas = repoTrazas;

        }

        protected BaseController(
                    PerfilesPermisosRepository repoPerfilesPermisos,
                    PermisosRepository repoPermisos,
                    GruposRepository repoGrupos,
                    PerfilesRepository repoPerfiles,
                    PerfilesGruposRepository repoPerfilesGrupos,
                    UsuariosRepository repoUsuarios,
                    IServiciosGrupos serviciosGrupos,
                    IServiciosUsuarios serviciosUsuarios,
                    TrazasRepository repoTrazas,
                    ILogger logger)
        {
            this.log = logger;
            this.repoPerfiles = repoPerfiles;
            this._serviciosGrupos = serviciosGrupos;
            this.repoPerfilesGrupos = repoPerfilesGrupos;
            this.repoGrupos = repoGrupos;
            this.repoPermisos = repoPermisos;
            this.repoPerfilesPermisos = repoPerfilesPermisos;
            this.repoUsuarios = repoUsuarios;
            this._serviciosUsuarios = serviciosUsuarios;
            this.repoTrazas = repoTrazas;
        }

        protected BaseController(
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
                    ILogger logger)
        {
            this.log = logger;
            this.repoPerfiles = repoPerfiles;
            this._serviciosGrupos = serviciosGrupos;
            this.repoPerfilesGrupos = repoPerfilesGrupos;
            this.repoGrupos = repoGrupos;
            this.repogestionpermisos = repogestionpermisos;
            this.repoPerfilesPermisos = repoPerfilesPermisos;
            this.repoPermisos = repoPermisos;
            this.repoUsuarios = repoUsuarios;
            this._serviciosUsuarios = serviciosUsuarios;
            this.repoTrazas = repoTrazas;
      }

        protected BaseController(IServiciosUsuarios serviciosUsuarios, 
                                 TiposDocumentosRepository repoTiposDocumentos,
                                 TrazasRepository repoTrazas,
                                 ILogger logger)
        {
            this._serviciosUsuarios = serviciosUsuarios;
            this.repoTiposDocumentos = repoTiposDocumentos;
            this.repoTrazas = repoTrazas;
            this.log = logger;
        }

        protected BaseController(IServiciosUsuarios serviciosUsuarios, UsuariosRepository repoUsuarios, ILogger logger)
        {
            this._serviciosUsuarios = serviciosUsuarios;
            this.log = logger;
            this.repoUsuarios = repoUsuarios;
        }


        protected BaseController(Comunidades_AutonomasRepository repoComunidades_Autonomas,
                                    OrganismoRepository repoOrganismo, 
                                    ProvinciasRepository repoProvincias, 
                                    CiudadesRepository repoCiudades, 
                                    Contactos_OrganismoRepository repoContactos_Organismo,
                                    IServiciosPaisesProvincias serviciosPaisesProvincias,
                                    IServiciosOrganismo serviciosOrganismos,
                                    IServiciosUsuarios serviciosUsuarios, 
                                    TrazasRepository repoTrazas, 
                                    ILogger logger)
        {
            this.log = logger;
            this.repoOrganismo = repoOrganismo;
            this.repoTrazas = repoTrazas;
            this.repoProvincias = repoProvincias;
            this.repoCiudades = repoCiudades;
            this.repoContactos_Organismo = repoContactos_Organismo;
            this._serviciosOrganismos = serviciosOrganismos;
            this._serviciosUsuarios = serviciosUsuarios;
            this.repoComunidades_Autonomas = repoComunidades_Autonomas;
            this._serviciosPaisesProvincias = serviciosPaisesProvincias;
        }

        protected BaseController(IServiciosPaisesProvincias serviciosPaisesProvincias, 
                                    PaisesRepository repoPaises, 
                                    ProvinciasRepository repoProvincias, 
                                    CiudadesRepository repoCiudades,
                                    TrazasRepository repoTrazas, ILogger logger)
        {
            this._serviciosPaisesProvincias = serviciosPaisesProvincias;
            this.repoPaises = repoPaises;
            this.repoProvincias = repoProvincias;
            this.repoCiudades = repoCiudades;
            this.repoTrazas = repoTrazas;
            this.log = logger;
        }


        protected BaseController(IServiciosPaisesProvincias serviciosPaisesProvincias, 
                                 Comunidades_AutonomasRepository repoComunidades_Autonomas, 
                                 TrazasRepository repoTrazas, 
                                 ILogger logger)
        {
            this._serviciosPaisesProvincias = serviciosPaisesProvincias;
            this.repoComunidades_Autonomas = repoComunidades_Autonomas;
            this.repoTrazas = repoTrazas;
            this.log = logger;
        }

        protected BaseController(IServiciosPaisesProvincias serviciosPaisesProvincias, 
                                 ProvinciasRepository repoProvincias, 
                                 TrazasRepository repoTrazas, 
                                 ILogger logger)
        {
            this._serviciosPaisesProvincias = serviciosPaisesProvincias;
            this.repoProvincias = repoProvincias;
            this.repoTrazas = repoTrazas;
            this.log = logger;
        }

        protected BaseController(IServiciosPaisesProvincias serviciosPaisesProvincias, 
                                 CapitaniasRepository repoCapitanias, 
                                 TrazasRepository repoTrazas, 
                                 ILogger logger)
        {
            this._serviciosPaisesProvincias = serviciosPaisesProvincias;
            this.repoCapitanias = repoCapitanias;
            this.repoTrazas = repoTrazas;
            this.log = logger;
        }

         protected BaseController(IServiciosTipoInstalacion _serviciosTipoInstalacion, 
                                TipoInstalacionRepository repoTipoInstalaciones,
                                TrazasRepository repoTrazas, ILogger logger) 
        {
            this._serviciosTipoInstalacion = _serviciosTipoInstalacion;
             this.repoTipoInstalaciones = repoTipoInstalaciones;
            this.repoTrazas = repoTrazas;
            this.log = logger;
        }

         protected BaseController(IServiciosCentros _serviciosCentros,
                                  IServiciosPuertos servicioPuertos,
                                  CentrosRepository repoCentros,
                                  IServiciosOrganismo servicioOrganismos,
                                  PuertosRepository repoPuertos,
                                  Comunica_CentroRepository repoComunica_Centro,
                                  ProvinciasRepository repoProvincias,
                                  CiudadesRepository repoCiudades,
                                  TrazasRepository repoTrazas, 
                                  ILogger logger)
         {
             this._serviciosCentros = _serviciosCentros;
             this._serviciosPuertos = servicioPuertos;
             this.repoCentros = repoCentros;
             this._serviciosOrganismos = servicioOrganismos;
             this.repoPuertos = repoPuertos;
             this.repoComunica_Centro = repoComunica_Centro;
             this.repoProvincias = repoProvincias;
             this.repoCiudades = repoCiudades;
             this.repoTrazas = repoTrazas;
             this.log = logger;
         }

         protected BaseController(IServiciosAmenazas serviciosAmenazas,
                                 AmenazasRepository repoAmenazas,
                                 IServiciosBienes serviciosBienes,
                                 MOV_Detalle_InstalacionRepository repoMOV_Detalle_Instalacion,
                                 TrazasRepository repoTrazas,
                                 ILogger logger)
         {
             this._serviciosAmenazas = serviciosAmenazas;
             this._serviciosBienes = serviciosBienes;
             this.repoAmenazas = repoAmenazas;
             this.repoMovDetalleInstalaciones = repoMOV_Detalle_Instalacion;
             this.repoTrazas = repoTrazas;
             this.log = logger;
         }

         protected BaseController(IServiciosMotivos serviciosMotivos,
                                 MotivosRepository repoMotivos,
                                 TrazasRepository repoTrazas,
                                 ILogger logger)
         {
             this._serviciosMotivos = serviciosMotivos;
             this.repoMotivos = repoMotivos;
             this.repoTrazas = repoTrazas;
             this.log = logger;
         }
        
         protected BaseController(IServiciosPuertos serviciosPuertos,
                                   PuertosRepository repoPuertos,
                                   IServiciosPaisesProvincias serviciosPaisesProvincias,
                                   IServiciosOrganismo servicioOrganismos,
                                   ProvinciasRepository repoProvincias,
                                   CiudadesRepository repoCiudades,
                                   OperadoresPuertoRepository repoOperadoresPuerto,
                                   IServiciosUsuarios servicioUsuarios,
                                   TrazasRepository repoTrazas, ILogger logger)
         {
             this._serviciosPuertos = serviciosPuertos;
             this.repoPuertos = repoPuertos;
             this._serviciosPaisesProvincias = serviciosPaisesProvincias;
             this._serviciosOrganismos = servicioOrganismos;
             this.repoProvincias = repoProvincias;
             this.repoCiudades = repoCiudades;
             this.repoOperadoresPuerto = repoOperadoresPuerto;
             this._serviciosUsuarios = servicioUsuarios;
             this.repoTrazas = repoTrazas;
             this.log = logger;
         }

         protected BaseController(IServiciosComites serviciosComites,
                                   ComitesRepository repoComites,
                                   IServiciosOrganismo servicioOrganismos,
                                   IServiciosPuertos servicioPuertos,
                                   IServiciosUsuarios servicioUsuarios,
                                   DocumentosUsuarioRepository repoDocumentosUsuario,
                                   IServiciosDocumentos servicioDocumentos, 
                                   TrazasRepository repoTrazas, ILogger logger)
         {
             this._serviciosComites = serviciosComites;
             this.repoComites = repoComites;
             this._serviciosOrganismos = servicioOrganismos;
             this._serviciosPuertos = servicioPuertos;
             this._serviciosUsuarios = servicioUsuarios;
             this.repoDocumentosUsuario = repoDocumentosUsuario;
             this._serviciosDocumentos = servicioDocumentos;
             this.repoTrazas = repoTrazas;
             this.log = logger;
         }
        
         protected BaseController(IServiciosPuertos servicioPuertos,
                                  IServiciosTipoInstalacion serviciosTipoInstalacion,
                                  InstalacionesRepository repoInstalaciones,
                                  TipoInstalacionRepository repoTipoInstalaciones,
                                  OperadoresRepository repoOperadores,
                                  ProvinciasRepository repoProvincias,
                                  CiudadesRepository repoCiudades,
                                  IServiciosUsuarios servicioUsuarios,
                                  IServiciosBienes serviciosBienes,
                                  PuertosRepository repoPuertos,
                                  MOV_Detalle_InstalacionRepository repoMOV_Detalle_Instalacion,
                                  TrazasRepository repoTrazas, 
                                  ILogger logger)
         {
             this._serviciosPuertos = servicioPuertos;
             this._serviciosTipoInstalacion = serviciosTipoInstalacion;
             this.repoInstalaciones = repoInstalaciones;
             this.repoOperadores = repoOperadores;
             this.repoProvincias = repoProvincias;
             this.repoCiudades = repoCiudades;
             this.repoTipoInstalaciones = repoTipoInstalaciones;
             this._serviciosUsuarios = servicioUsuarios;
             this._serviciosBienes = serviciosBienes;
             this.repoPuertos = repoPuertos;
             this.repoMovDetalleInstalaciones = repoMOV_Detalle_Instalacion;
             this.repoTrazas = repoTrazas;
             this.log = logger;
         }

         protected BaseController(IServiciosPuertos servicioPuertos,
                                   IServiciosTipoInstalacion serviciosTipoInstalacion,
                                   InstalacionesRepository repoInstalaciones,
                                   OperadoresRepository repoOperadores,
                                   ProvinciasRepository repoProvincias,
                                   CiudadesRepository repoCiudades,
                                   UsuariosRepository repoUsuarios,
                                   DocumentosUsuarioRepository repoDocumentosUsuario, 
                                   IServiciosUsuarios serviciosUsuarios,
                                   IServiciosDocumentos servicioDocumentos,
                                   PuertosRepository repoPuertos,
                                   TrazasRepository repoTrazas,
                                   ILogger logger)
         {
             this._serviciosPuertos = servicioPuertos;
             this._serviciosTipoInstalacion = serviciosTipoInstalacion;
             this.repoInstalaciones = repoInstalaciones;
             this.repoOperadores = repoOperadores;
             this.repoProvincias = repoProvincias;
             this.repoCiudades = repoCiudades;
             this.repoUsuarios = repoUsuarios;
             this.repoDocumentosUsuario = repoDocumentosUsuario;
             this._serviciosUsuarios = serviciosUsuarios;
             this._serviciosDocumentos = servicioDocumentos;
             this.repoPuertos = repoPuertos;
             this.repoTrazas = repoTrazas;
             this.log = logger;
         }

        protected BaseController(IServiciosFormaciones serviciosFormaciones,
                                 IServiciosOrganismo serviciosOrganismos,
                                 InstalacionesRepository repoInstalaciones,
                                 PuertosRepository repoPuertos,
                                 FormacionesRepository repoFormaciones,
                                 IServiciosTipoInstalacion serviciosInstalaciones,
                                 IServiciosPuertos servicioPuertos,
                                 IServiciosUsuarios servicioUsuarios,
                                 DocumentosUsuarioRepository repoDocumentosUsuario,
                                 IServiciosDocumentos servicioDocumentos,
                                 TrazasRepository repoTrazas, 
                                 ILogger logger)
        {
            this._serviciosFormaciones = serviciosFormaciones;
            this._serviciosOrganismos  = serviciosOrganismos;
            this.repoInstalaciones     = repoInstalaciones;
            this.repoPuertos           = repoPuertos;
            this.repoFormaciones       = repoFormaciones;
            this._serviciosTipoInstalacion = serviciosInstalaciones;
            this._serviciosPuertos     = servicioPuertos;
            this._serviciosUsuarios    = servicioUsuarios;
            this.repoDocumentosUsuario = repoDocumentosUsuario;
            this._serviciosDocumentos = servicioDocumentos;
            this.repoTrazas            = repoTrazas;
            this.log                   = logger;
        }

        protected BaseController(IServiciosPracticas serviciosPracticas,
                                 InstalacionesRepository repoInstalaciones,
                                 PuertosRepository repoPuertos,
                                 PracticasRepository repoPracticas,
                                 IServiciosTipoInstalacion serviciosInstalaciones,
                                 IServiciosPuertos servicioPuertos,
                                 DocumentosUsuarioRepository repoDocumentosUsuario,
                                 IServiciosUsuarios servicioUsuarios,
                                 IServiciosDocumentos servicioDocumentos,
                                 Det_PracticaRepository repoDet_Practica, 
                                 TrazasRepository repoTrazas,
                                 ILogger logger)
        {
            this._serviciosPracticas = serviciosPracticas;
            this.repoInstalaciones = repoInstalaciones;
            this.repoPuertos = repoPuertos;
            this.repoPracticas = repoPracticas;
            this._serviciosTipoInstalacion = serviciosInstalaciones;
            this._serviciosPuertos = servicioPuertos;
            this.repoDocumentosUsuario = repoDocumentosUsuario;
            this._serviciosUsuarios = servicioUsuarios;
            this._serviciosDocumentos = servicioDocumentos;
            this.repoDet_Practica = repoDet_Practica;
            this.repoTrazas = repoTrazas;
            this.log = logger;
        }
        
        protected BaseController(IServiciosIncidentes serviciosIncidentes,
                                 InstalacionesRepository repoInstalaciones,
                                 IncidentesRepository repoIncidentes,
                                 IServiciosTipoInstalacion serviciosInstalaciones,
                                 IServiciosPuertos servicioPuertos,
                                 DocumentosUsuarioRepository repoDocumentosUsuario,
                                 IServiciosUsuarios servicioUsuarios,
                                 IServiciosDocumentos servicioDocumentos,
                                 Det_IncidenteRepository repoDet_Incidente,
                                 TrazasRepository repoTrazas,
                                 ILogger logger)
        {
            this._serviciosIncidentes = serviciosIncidentes;
            this.repoInstalaciones = repoInstalaciones;
            this.repoIncidentes = repoIncidentes;
            this._serviciosTipoInstalacion = serviciosInstalaciones;
            this._serviciosPuertos = servicioPuertos;
            this.repoDocumentosUsuario = repoDocumentosUsuario;
            this._serviciosUsuarios = servicioUsuarios;
            this._serviciosDocumentos = servicioDocumentos;
            this.repoDet_Incidente = repoDet_Incidente;
            this.repoTrazas = repoTrazas;
            this.log = logger;
        }

        protected BaseController(IServiciosAuditorias serviciosAuditorias,
                                 InstalacionesRepository repoInstalaciones,
                                 PuertosRepository repoPuertos,
                                 AuditoriasRepository repoAuditorias,
                                 IServiciosTipoInstalacion serviciosInstalaciones,
                                 IServiciosPuertos servicioPuertos,
                                 DocumentosUsuarioRepository repoDocumentosUsuario,
                                 IServiciosUsuarios servicioUsuarios,
                                 IServiciosDocumentos servicioDocumentos,
                                 TrazasRepository repoTrazas,
                                 ILogger logger)
        {
            this._serviciosAuditorias = serviciosAuditorias;
            this.repoInstalaciones = repoInstalaciones;
            this.repoPuertos = repoPuertos;
            this.repoAuditorias = repoAuditorias;
            this._serviciosTipoInstalacion = serviciosInstalaciones;
            this._serviciosPuertos = servicioPuertos;
            this.repoDocumentosUsuario = repoDocumentosUsuario;
            this._serviciosUsuarios = servicioUsuarios;
            this._serviciosDocumentos = servicioDocumentos;
            this.repoTrazas = repoTrazas;
            this.log = logger;
        }


        protected BaseController(IServiciosNiveles serviciosNiveles,
                                InstalacionesRepository repoInstalaciones,
                                PuertosRepository repoPuertos,
                                NivelesRepository repoNiveles,
                                IServiciosTipoInstalacion serviciosInstalaciones,
                                IServiciosPuertos servicioPuertos,
                                IServiciosUsuarios servicioUsuarios,
                                DocumentosUsuarioRepository repoDocumentosUsuario,
                                IServiciosDocumentos servicioDocumentos,
                                Det_NivelRepository repoDet_Nivel,
                                UsuariosRepository repoUsuario,
                                IServiciosTipoInstalacion serviciosTipoInstalacion,
                                TrazasRepository repoTrazas,
                                ILogger logger)
        {
            this._serviciosNiveles = serviciosNiveles;
            this.repoInstalaciones = repoInstalaciones;
            this.repoPuertos = repoPuertos;
            this.repoNiveles = repoNiveles;
            this._serviciosTipoInstalacion = serviciosInstalaciones;
            this._serviciosPuertos = servicioPuertos;
            this._serviciosUsuarios = servicioUsuarios;
            this.repoDocumentosUsuario = repoDocumentosUsuario;
            this._serviciosDocumentos = servicioDocumentos;
            this.repoDet_Nivel = repoDet_Nivel;
            this.repoUsuarios = repoUsuario;
            this._serviciosTipoInstalacion = serviciosTipoInstalacion;
            this.repoTrazas = repoTrazas;
            this.log = logger;
        }

        protected BaseController(IServiciosMantenimiento serviciosMantenimiento,
                                MantenimientosRepository repoMantenimiento,
                                InstalacionesRepository repoInstalaciones,
                                PuertosRepository repoPuertos,
                                IServiciosPuertos servicioPuertos,
                                IServiciosUsuarios servicioUsuarios,
                                DocumentosUsuarioRepository repoDocumentosUsuario,
                                IServiciosDocumentos servicioDocumentos,
                                IServiciosTipoInstalacion serviciosInstalaciones,
                                TrazasRepository repoTrazas,
                                ILogger logger)
        {
            this._serviciosMantenimiento = serviciosMantenimiento;
            this.repoInstalaciones = repoInstalaciones;
            this.repoMantenimiento = repoMantenimiento;
            this.repoPuertos = repoPuertos;
            this._serviciosPuertos = servicioPuertos;
            this._serviciosUsuarios = servicioUsuarios;
            this.repoDocumentosUsuario = repoDocumentosUsuario;
            this._serviciosDocumentos = servicioDocumentos;
            this._serviciosTipoInstalacion = serviciosInstalaciones;
            this.repoTrazas = repoTrazas;
            this.log = logger;
        }

        protected BaseController(IServiciosComunicaciones serviciosComunicaciones,
                                ComunicacionesRepository repoComunicaciones,
                                InstalacionesRepository repoInstalaciones,
                                PuertosRepository repoPuertos,
                                IServiciosPuertos servicioPuertos,
                                IServiciosUsuarios servicioUsuarios,
                                DocumentosUsuarioRepository repoDocumentosUsuario,
                                IServiciosDocumentos servicioDocumentos,
                                IServiciosTipoInstalacion serviciosInstalaciones,                  
                                TrazasRepository repoTrazas,
                                ILogger logger)
        {
            this._serviciosComunicacion = serviciosComunicaciones;
            this.repoInstalaciones = repoInstalaciones;
            this.repoComunicacion = repoComunicaciones;
            this.repoPuertos = repoPuertos;
            this._serviciosPuertos = servicioPuertos;
            this._serviciosUsuarios = servicioUsuarios;
            this.repoDocumentosUsuario = repoDocumentosUsuario;
            this._serviciosDocumentos = servicioDocumentos;
            this._serviciosTipoInstalacion = serviciosInstalaciones;
            this.repoTrazas = repoTrazas;
            this.log = logger;
        }

        protected BaseController(IServiciosDeclaraMaritima serviciosDeclaraMaritima,
                                 IServiciosUsuarios servicioUsuarios,
                                 DeclaraMaritimaRepository repoDeclaraMaritima,
                                 IServiciosPuertos servicioPuertos,
                                 IServiciosTipoInstalacion serviciosInstalaciones,
                                 IServiciosOrganismo serviciosOrganismos,
                                 IServiciosMotivos serviciosMotivos,
                                 DocumentosUsuarioRepository repoDocumentosUsuario,
                                 IServiciosDocumentos servicioDocumentos,
                                 TrazasRepository repoTrazas,
                                 ILogger logger)
        {
            this._serviciosDeclaraMaritima = serviciosDeclaraMaritima;
            this._serviciosUsuarios = servicioUsuarios;
            this.repoDeclaraMaritima = repoDeclaraMaritima;
            this._serviciosPuertos = servicioPuertos;
            this._serviciosTipoInstalacion = serviciosInstalaciones;
            this._serviciosOrganismos = serviciosOrganismos;
            this._serviciosMotivos = serviciosMotivos;
            this._serviciosDocumentos = servicioDocumentos;
            this.repoDocumentosUsuario = repoDocumentosUsuario;
            this.repoTrazas = repoTrazas;
            this.log = logger;
        }


        protected BaseController(IServiciosGisis serviciosGisis,
                                 GisisRepository repoGisis,
                                 IServiciosPuertos servicioPuertos,
                                 IServiciosTipoInstalacion serviciosInstalaciones,
                                 TrazasRepository repoTrazas,
                                 ILogger logger)
        {
            this._serviciosGisis = serviciosGisis;
            this.repoGisis = repoGisis;
            this._serviciosPuertos = servicioPuertos;
            this._serviciosTipoInstalacion = serviciosInstalaciones;
            this.repoTrazas = repoTrazas;
            this.log = logger;
        }

        protected BaseController(IServiciosVinculos serviciosVinculos,
                                 VinculosRepository repoVinculos,
                                 TrazasRepository repoTrazas,
                                 ILogger logger)
        {
            this._serviciosVinculos = serviciosVinculos;
            this.repoVinculos = repoVinculos;
            this.repoTrazas = repoTrazas;
            this.log = logger;
        }
        
        protected BaseController(IServiciosProcedimientos serviciosProcedimientos,
                                 ProcedimientosRepository repoProcedimientos,
                                 IServiciosOrganismo serviciosOrganismos,
                                 DocumentosUsuarioRepository repoDocumentosUsuario,
                                 IServiciosDocumentos servicioDocumentos,
                                 IServiciosUsuarios servicioUsuarios,
                                 TrazasRepository repoTrazas,
                                 ILogger logger)
        {
            this._serviciosProcedimientos = serviciosProcedimientos;
            this.repoProcedimientos = repoProcedimientos;
            this._serviciosOrganismos = serviciosOrganismos;
            this.repoDocumentosUsuario = repoDocumentosUsuario;
            this._serviciosDocumentos = servicioDocumentos;
            this._serviciosUsuarios = servicioUsuarios;
            this.repoTrazas = repoTrazas;
            this.log = logger;
        }

        protected BaseController(IServiciosTipoInstalacion serviciosTipoInstalacion,
                                  InstalacionesRepository repoInstalaciones,
                                  TipoInstalacionRepository repoTipoInstalaciones,
                                  IServiciosUsuarios servicioUsuarios,
                                  IServiciosBienes serviciosBienes,
                                  IServiciosAmenazas serviciosAmenazas,
                                  MOV_Detalle_InstalacionRepository repoMovDetalleInstalaciones,
                                  IServiciosEvaluaciones seviciosEvaluaciones,
                                  EvaluacionesRepository repoEvaluaciones,
                                  Versiones_EvaluacionRepository repoVersionesEvaluacion,
                                  IServiciosPuertos servicioPuertos,
                                  DocumentosUsuarioRepository repoDocumentosUsuario,
                                  IServiciosDocumentos servicioDocumentos,
                                  Historico_EvaluacionRepository repoHistorico_Evalacion,
                                  Detalle_EvaluacionRepository repoDetalle_Evaluacion,
                                  TrazasRepository repoTrazas,
                                  ILogger logger)
        {
            this._serviciosTipoInstalacion = serviciosTipoInstalacion;
            this.repoInstalaciones = repoInstalaciones;
            this.repoTipoInstalaciones = repoTipoInstalaciones;
            this._serviciosUsuarios = servicioUsuarios;
            this._serviciosBienes = serviciosBienes;
            this._serviciosAmenazas = serviciosAmenazas;
            this.repoMovDetalleInstalaciones = repoMovDetalleInstalaciones;
            this._serviciosEvaluaciones = seviciosEvaluaciones;
            this.repoEvaluaciones = repoEvaluaciones;
            this.repoVersionesEvaluacion = repoVersionesEvaluacion;
            this._serviciosPuertos = servicioPuertos;
            this.repoDocumentosUsuario = repoDocumentosUsuario;
            this._serviciosDocumentos = servicioDocumentos;
            this.repoHistorico_Evaluacion = repoHistorico_Evalacion;
            this.repoDetalle_Evaluacion = repoDetalle_Evaluacion;
            this.repoTrazas = repoTrazas;
            this.log = logger;
        }

        #endregion

        #region control errores
        
        ///<summary>
        ///Controlamos el error de Session caducada.
        ///</summary>
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (SessionTimeOut.IsSessionTimedOut())
            {
                this.UsuarioFrontalSession.Usuario = null;
                filterContext.Result = new ViewResult { ViewName = "~/Views/ErrorPages/SessionCaduca.cshtml" };
            }
            //filterContext.HttpContext.Response.Cache.SetExpires(DateTime.UtcNow.AddDays(-1));
            //filterContext.HttpContext.Response.Cache.SetValidUntilExpires(false);
            //filterContext.HttpContext.Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
            //filterContext.HttpContext.Response.Cache.SetCacheability(HttpCacheability.ServerAndNoCache);
            //filterContext.HttpContext.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            //filterContext.HttpContext.Response.Cache.SetNoStore();
            base.OnActionExecuting(filterContext);
        }
        
        ///<summary>
        ///Controlamos los Errores internos de la aplicación.
        ///</summary>
        protected override void OnException(ExceptionContext filterContext)
        {
            string viewName = string.Empty;
            int statusCode = (int)HttpStatusCode.InternalServerError;
            if (filterContext.Exception is HttpException)
            {
                statusCode = (int)HttpStatusCode.BadRequest;
            }
            else if (filterContext.Exception is UnauthorizedAccessException)
            {
                statusCode = (int)HttpStatusCode.Forbidden;
            }

            this.log.PublishException(filterContext.Exception);
            //Existe un error se cierran las session
            this.UsuarioFrontalSession.Usuario = null;

            switch (statusCode)
            {
                case 500:
                    viewName = "~/Views/ErrorPages/ErrorPage400.cshtml";
                    break;
                case 400:
                    viewName = "~/Views/ErrorPages/ErrorPage400.cshtml";
                    break;
                default:
                    viewName = "~/Views/ErrorPages/ErrorPage500.cshtml";
                    break;
            }

            filterContext.Result = new ViewResult { ViewName = viewName };

            filterContext.ExceptionHandled = true;

            filterContext.HttpContext.Response.Clear();

            filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.db.Dispose();
            }
            base.Dispose(disposing);
        }

        #endregion

        #region métodos comunes

        public string MessageDelete(Exception ex)
        {
            string[] stringSeparator = new string[] { " tabla \"" };
            
            String[] cadenas;

            var Messaje = ex.Message;
            
            try
            {
                string[] result = ex.Message.Split(stringSeparator, StringSplitOptions.None);

                string cadenaTexto = result[1].ToString();

                cadenas = cadenaTexto.Split('"');
            }
            catch (Exception)
            {
                return Messaje;
            }
            
            return "No se puede eliminar el registro existen registros asociados en la tabla:" + cadenas[0];
        }
        
        public Boolean EnviarCorreo(string login, string password, string texto)
        {
            MailMessage _Correo = new MailMessage();

            _Correo.From = new MailAddress(ConfigurationManager.AppSettings["CorreoFROM"]);

            if (ConfigurationManager.AppSettings["CorreoUsuarioProvisional"] != string.Empty)
            {
                string CorreoA = ConfigurationManager.AppSettings["CorreoUsuarioProvisional"];

                string[] cad = null;

                cad = CorreoA.Split(';');

                for (int i = 0; i <= cad.GetUpperBound(0); i++)
                {
                    _Correo.To.Add(cad[i]);
                }
            }
            else
            {
                _Correo.To.Add(login);
            }

            _Correo.Subject = texto;

            _Correo.Body = "Hola:" + login + " te enviamos tu nueva constraseña ==>> " + password;

            _Correo.IsBodyHtml = false;

            _Correo.Priority = MailPriority.High;
            
            SmtpClient smtp = new SmtpClient();

            smtp.Host = ConfigurationManager.AppSettings["Servidorcorreo"];

            smtp.Port = Convert.ToInt32(ConfigurationManager.AppSettings["puerto"]);

            smtp.EnableSsl = false;

            try
            {
                smtp.Send(_Correo);
                return true;
            }
            catch (Exception ex)
            {
                this.log.PublishException(new SecureportException(ex));
                return false;
            }
        }

        public Boolean EnviarCorreoPuertos(string login, string nombre)
        {
            MailMessage _Correo = new MailMessage();

            _Correo.From = new MailAddress(ConfigurationManager.AppSettings["CorreoFROM"]);

            string CorreoA = ConfigurationManager.AppSettings["CorreoOPPE"];

            string[] cad = null;

            cad = CorreoA.Split(';');

            for (int i = 0; i <= cad.GetUpperBound(0); i++)
            {
                _Correo.To.Add(cad[i]);
            }

            _Correo.Subject = "SecurePort -  Alta Usuarios";

            _Correo.Body = "Hemos dado de Alta al siguiente Usuario:" + login + " " + "Nombre:" + nombre + " " + "Fecha:" + DateTime.Now.ToString();

            _Correo.IsBodyHtml = false;

            _Correo.Priority = MailPriority.High;

            SmtpClient smtp = new SmtpClient();

            smtp.Host = ConfigurationManager.AppSettings["Servidorcorreo"];

            smtp.Port = Convert.ToInt32(ConfigurationManager.AppSettings["puerto"]);

            smtp.EnableSsl = false;

            try
            {
                smtp.Send(_Correo);
                return true;
            }
            catch (Exception ex)
            {
                this.log.PublishException(new SecureportException(ex));
                return false;
            }
        }

        public Boolean EnviarCorreoPuertos(string Asunto, string Texto, string CorreoA)
        {
            MailMessage _Correo = new MailMessage();

            _Correo.From = new MailAddress(ConfigurationManager.AppSettings["CorreoFROM"]);

            string[] cad = null;

            cad = CorreoA.Split(';');

            for (int i = 0; i <= cad.GetUpperBound(0); i++)
            {
                _Correo.To.Add(cad[i]);
            }

            _Correo.Subject = Asunto;

            _Correo.Body = Texto;

            _Correo.IsBodyHtml = false;

            _Correo.Priority = MailPriority.High;

            SmtpClient smtp = new SmtpClient();
            
            smtp.Host = ConfigurationManager.AppSettings["Servidorcorreo"];

            smtp.Port = Convert.ToInt32(ConfigurationManager.AppSettings["puerto"]);

            smtp.EnableSsl = false;

            try
            {
                smtp.Send(_Correo);
                return true;
            }
            catch (Exception ex)
            {
                this.log.PublishException(new SecureportException(ex));
                return false;
            }
        }

        public string CambiarTexto(string texto)
        {
            texto = (texto == null ? string.Empty : texto).Replace("&amp;aacute;", "á").Replace("&amp;eacute;", "é").Replace("&amp;iacute;", "í").Replace("&amp;oacute;", "ó").Replace("&amp;uacute;", "ú").Replace("&amp;nbsp;", " ").Replace("&amp;ntilde;", "ñ").Replace("&amp;uuml;", "ü").Replace("&lt;", "<").Replace("&gt;", ">").Trim();
            return texto;
        }

        public bool BorrarFichero(String archivo)
        {
            if (System.IO.File.Exists(@archivo))
            {
                try
                {
                    System.IO.File.Delete(@archivo);
                    return true;
                }
                catch (Exception ex)
                {
                    this.log.PublishException(new SecureportException(ex));
                    return false;
                }
            }
            return false;
        }

        public bool ComprobarFormatoEmail(string seMailAComprobar)
        {
            String sFormato;
            sFormato = "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";
            if (Regex.IsMatch(seMailAComprobar, sFormato))
            {
                if (Regex.Replace(seMailAComprobar, sFormato, String.Empty).Length == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        ///<summary>
        ///Trazas de la apliación
        ///</summary>
        public void trazas(int? id_accion, int id_usu_alta, string traza)
        {

            try
            {
                Trazas trazas = new Trazas();
                trazas.id_accion = id_accion;
                trazas.fech_alta = DateTime.Now;
                trazas.id_usu_alta = id_usu_alta;
                trazas.traza = traza;
                this.repoTrazas.Create(trazas);
            }
            catch (DbUpdateException ex)
            {
                this.log.PublishException(new SecureportExceptionDbUpdate(ex));

            }
            catch (DbEntityValidationException ex)
            {
                this.log.PublishException(new SecureportExceptionDbEntity(ex));

            }
            catch (Exception ex)
            {
                this.log.PublishException(new SecureportException(ex));

            }

        }

       
        public string CreatePassword(int length)
        {
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }

        /// <summary>
        /// Retorna los tipos de instalación.
        /// </summary> 
        public void ComboTipos_Instalacion()
        {
            IEnumerable<ListadoTipoInstalaciones> listTipoInstalaciones = _serviciosTipoInstalacion.ListTipoInstalaciones().ToList().Where(x=> x.es_activo = true);

            SelectList selectList = new SelectList(listTipoInstalaciones, "Id", "descripcion", 1);

            this.ViewData["TipoInstalaciones"] = selectList;
        }
        /// <summary>
        /// Retorna los tipos de documentos registrados en el aplicación.
        /// </summary> 
        public void ComboPaises()
        {
            IEnumerable<ListPaises> listpaises = _serviciosPaisesProvincias.GetAllPaises().OrderBy(x=> x.nombre).ToList();

            SelectList selectList = new SelectList(listpaises, "Id", "Nombre", 1);

            this.ViewData["Paises"] = selectList;
        }

        /// <summary>
        /// Retorna los tipos de documentos registrados en el aplicación.
        /// </summary> 
        public void ComboTipos_Documento()
        {
            IEnumerable<Tipos_Documento> listTiposDocumento = _serviciosUsuarios.GetTiposDocumento().ToList();

            SelectList selectList = new SelectList(listTiposDocumento, "Id", "descripcion", 1);

            this.ViewData["Tipos_Documento"] = selectList;
        }
        /// <summary>
        /// Retorna las provincias registradas en el aplicación.
        /// </summary> 
        public void ComboCiudades(int? id,int? idCiudad)
        {

            List<CiudadesProvincias> ciud = new List<CiudadesProvincias>();

            if (id!=null)
            {
                this.repoCiudades.GetAll().Where(x => x.Id_provincia == id && x.es_activo).ToList().ForEach(
                    p =>
                    {

                        CiudadesProvincias ciudades = new CiudadesProvincias() { Id = p.id, Name = p.nombre };

                        ciud.Add(ciudades);
                    });
            }
            else if (idCiudad != null && idCiudad != 0) 
            {

                var Id_provincia = this.repoCiudades.Single(x => x.id == idCiudad).Id_provincia;

                this.repoCiudades.GetAll().Where(x => x.Id_provincia == Id_provincia && x.es_activo).ToList().ForEach(
                    p =>
                    {

                        CiudadesProvincias ciudades = new CiudadesProvincias() { Id = p.id, Name = p.nombre };

                        ciud.Add(ciudades);
                    });
            }
            
            SelectList selectList = new SelectList(ciud.OrderBy(x=> x.Name), "id", "Name", 1);

            this.ViewData["Ciudades"] = selectList;
        }
        
        public void comboComunidades(int? Id)
        {

            IEnumerable<Comunidades_Autonomas> comunidades = this._serviciosPaisesProvincias.ListComunidades().Where(x=> x.es_activo).OrderBy(x=> x.Nombre); 
            
            var ID_ComAut = 0;

            if (Id != null)
            {
                ID_ComAut = this.repoProvincias.GetAll().Single(x => x.id == Id && x.es_activo).ID_ComAut;

                comunidades = this._serviciosPaisesProvincias.ListComunidades().Where(x => x.Id == ID_ComAut && x.es_activo).OrderBy(x=> x.Nombre).ToList();

            }
            
            SelectList selectList = new SelectList(comunidades, "id", "nombre", 1);

            this.ViewData["Comunidades"] = selectList;

            this.ViewBag.ID_ComAut = ID_ComAut;
        }

        /// <summary>
        /// Retorna las provincias registradas en el aplicación.
        /// </summary> 
        public void ComboProvincias(int? id)
        {

            List<Provincias> prov = new List<Provincias>();

            if (id == null)
            {
                this.repoProvincias.GetAll().Where(x=> x.es_activo).ToList().ForEach(
                    p =>
                    {

                        Provincias provincias = new Provincias() { id = p.id, nombre = p.nombre };

                        prov.Add(provincias);
                    });


                this.idProvincia = prov.OrderBy(x => x.nombre).ToList().First().id;
                
            }
            else
            {
                prov = this.repoProvincias.GetAll().Where(x=> x.id==id && x.es_activo).OrderBy(x => x.nombre).ToList();

            }

            SelectList selectList = new SelectList(prov.OrderBy(x=> x.nombre), "id", "nombre", 1);

            this.ViewData["Provincias"] = selectList;
        }
        /// <summary>
        /// Retorna las categorias registradas en el aplicación.
        /// </summary> 
        public void ComboCategorias()
        {

            switch (this.UsuarioFrontalSession.Usuario.categoria)
            {

                case (int)EnumCategoria.Administrador:
                    ComboCategoriasAdministrador();
                    break;

                case (int)EnumCategoria.Puerto:
                    ComboCategoriasPuerto();
                    break;

                case (int)EnumCategoria.OrganismoGestionPortuaria:
                    ComboCategoriasOrganismo();
                    break;

                case (int)EnumCategoria.Instalacion:
                    ComboCategoriasInstalacion();
                    break;
              }
        }

        public void ComboCategoriasInstalacion()
        {
            ListCategorias[] listCategoria = new[]
                                        {
                                          new ListCategorias { Value = "5", Text = "Instalación" }
                                       };

            this.ViewData["Categoria"] = listCategoria.OrderBy(x => x.Text);
        }

        public void ComboCategoriasOrganismo()
        {
            ListCategorias[] listCategoria = new[]
                                        {
                                          new ListCategorias { Value = "3", Text = "Organismo Gestión Portuaria" }, 
                                          new ListCategorias { Value = "4", Text = "Puerto" }, 
                                          new ListCategorias { Value = "5", Text = "Instalación" }
                                       };

            this.ViewData["Categoria"] = listCategoria.OrderBy(x => x.Text);
        }

        public void ComboCategoriasAdministrador()
        {
            ListCategorias[] listCategoria = new[]
                                        {
                                          new ListCategorias { Value = "1", Text = "Administrador" }, 
                                          new ListCategorias { Value = "2", Text = "Puertos del Estado" },
                                          new ListCategorias { Value = "3", Text = "Organismo Gestión Portuaria" }, 
                                          new ListCategorias { Value = "4", Text = "Puerto" }, 
                                          new ListCategorias { Value = "5", Text = "Instalación" },
                                          new ListCategorias { Value = "6", Text = "Ministerio del Interior" },
                                       };

            this.ViewData["Categoria"] = listCategoria.OrderBy(x => x.Text);
        }

        public void ComboCategoriasPuerto()
        {
            ListCategorias[] listCategoria = new[]
                                       {
                                          new ListCategorias { Value = "4", Text = "Puerto" }, 
                                          new ListCategorias { Value = "5", Text = "Instalación" }
                                       };

            this.ViewData["Categoria"] = listCategoria.OrderBy(x => x.Text);
        }
         /// <summary>
        /// Retorna los diversos tipos de Clasificación a la que pertenece la instalación.
        /// </summary> 
        public void ComboClasificacion()
        {
            ListClasificacion[] listClasificacion = new[]
                                        {
                                            new ListClasificacion { Value = "1", Text = "725" }, 
                                            new ListClasificacion { Value = "2", Text = "SEVESO" },
                                            new ListClasificacion { Value = "3", Text = "Alto Riesgo" }, 
                                            new ListClasificacion { Value = "4", Text = "Bajo Riesgo" }, 
                                            new ListClasificacion { Value = "5", Text = "Zonas Adyacentes" },
                                            new ListClasificacion { Value = "6", Text = "Tráficos Esporádicos" },
                                       };

            this.ViewData["Clasificacion"] = listClasificacion;
        }
        /// <summary>
        /// Tipo de Organismo.
        /// </summary> 
        public void ComboTipo(int? id)
        {


            List<TipoOrganismo> listTipo = _serviciosOrganismos.GetTipo().ToList();

            if (id != null)
            {
                listTipo = listTipo.Where(x => x.Id == id).ToList();
            }

            SelectList selectList = new SelectList(listTipo, "Id", "Name", 1);

            this.ViewData["Tipo"] = selectList;
        }


        /// <summary>
        /// Tipo de Motivos.
        /// </summary> 
        public void ComboMotivos(int? id)
        {
            
            IEnumerable<MotivosViewModel> listTipo = _serviciosMotivos.GetAllMotivos().ToList().Where(x => x.es_activo);

            if (id != null)
            {
                listTipo = listTipo.Where(x => x.Id == id).ToList();
            }

            SelectList selectList = new SelectList(listTipo, "Id", "Motivo", 1);

            this.ViewData["Motivos"] = selectList;
        }

        public void comboTipoInstalaciones(int? Id)
        {

            IEnumerable<ListadoTipoInstalaciones> tipoInstalaciones = new List<ListadoTipoInstalaciones>();

            if (Id != null)
            {
                tipoInstalaciones = this._serviciosTipoInstalacion.ListTipoInstalaciones(Id).Where(x=> x.activado=="Si").OrderBy(x => x.descripcion);

            }

            SelectList selectList = new SelectList(tipoInstalaciones, "Id", "Descripcion", 0);

            this.ViewData["TipoInstalaciones"] = selectList;

        }

        public void comboBienes(int? Id)
        {

            IEnumerable<BienesViewModel> bienes = this._serviciosBienes.ListBienes().Where(x => x.id_Bien_Padre == null && x.es_activo).OrderBy(x=> x.Descripcion);

            var ID_Bien = 0;

            if (Id != null)
            {
                ID_Bien = (int)Id;

                bienes = this._serviciosBienes.ListBienes().Where(x => x.Id == Id).ToList().Where(x => x.es_activo);

            }

            SelectList selectList = new SelectList(bienes, "Id", "Descripcion", 1);

            this.ViewData["bienes"] = selectList;

            this.ViewBag.Id_Bienes = ID_Bien;
        }

        public void comboInstalaciones(int? Id)
        {

            IEnumerable<InstalacionViewModel> instalacion = this._serviciosTipoInstalacion.GetAllInstalaciones().Where(x=> x.activado=="Si");

            var ID_Instalacion = 0;

            if (Id != null)
            {
                ID_Instalacion = (int)Id;

                instalacion = this._serviciosTipoInstalacion.GetAllInstalaciones().Where(x => x.Id == ID_Instalacion && x.activado=="Si").ToList().OrderBy(x=> x.Nombre);

            }

            SelectList selectList = new SelectList(instalacion, "Id", "Nombre", 1);

            this.ViewData["Instalaciones"] = selectList;

            this.ViewBag.idInstalacion = ID_Instalacion;
        }

        public void comboOrganismos(int? Id)
        {
            IEnumerable<OrganismosViewModel> organismos = new List<OrganismosViewModel>();

            if (Id == null)
            {
                if (this.UsuarioFrontalSession.Usuario.categoria == (int)EnumCategoria.Administrador
                    || this.UsuarioFrontalSession.Usuario.categoria == (int)EnumCategoria.PuertosdelEstado)
                {
                    organismos = this._serviciosOrganismos.ListOrganismos().ToList().OrderBy(x => x.Nombre);
                }
                else
                {
                   List<DependenciasUsuario> dependencias = this.UsuarioFrontalSession.ListDependenciasDepenUsuarios;

                    if (this.UsuarioFrontalSession.Usuario.categoria == (int)EnumCategoria.OrganismoGestionPortuaria)
                    {
                            dependencias = dependencias.Where(x => x.categoria == (int)EnumCategoria.OrganismoGestionPortuaria).ToList();     
                    
                            if (dependencias.Any())
                            {
                                organismos = (from t1 in this._serviciosOrganismos.ListOrganismos().OrderBy(x => x.Nombre) 
                                                            join t5 in dependencias.ToList() on t1.Id equals t5.id_Dependencia
                                                                        select new OrganismosViewModel
                                                                        {
                                                                            Id = t1.Id,
                                                                            Nombre = t1.Nombre
                                                                        })
                                                                        .GroupBy(c => c.Id).Select(grp => grp.First()).ToList().OrderBy(l => l.Nombre);
                            }
                   }
                   else if(this.UsuarioFrontalSession.Usuario.categoria == (int)EnumCategoria.Puerto)
                   {
                       dependencias = dependencias.Where(x => x.categoria == (int)EnumCategoria.Puerto).ToList();

                       if (dependencias.Any())
                       {
                           organismos = (from t1 in this._serviciosOrganismos.ListOrganismos().OrderBy(x => x.Id)
                                         join t2 in this._serviciosPuertos.GetAllPuertos().Where(x=> x.es_activo == true).OrderBy(x => x.Id) on t1.Id equals  t2.id_Organismo
                                         join t5 in dependencias.ToList() on t2.Id equals t5.id_Dependencia
                                         select new OrganismosViewModel
                                         {
                                             Id = t1.Id,
                                             Nombre = t1.Nombre
                                         })
                                         .GroupBy(c => c.Id).Select(grp => grp.First()).ToList().OrderBy(l => l.Nombre);
                       }
                   } //
                    else if (this.UsuarioFrontalSession.Usuario.categoria == (int)EnumCategoria.Instalacion)
                    {
                        dependencias = dependencias.Where(x => x.categoria == (int)EnumCategoria.OrganismoGestionPortuaria).ToList();

                        if (dependencias.Any())
                        {
                            organismos = (from t1 in this._serviciosOrganismos.ListOrganismos().OrderBy(x => x.Id)
                                          join t2 in this._serviciosPuertos.GetAllPuertos().Where(x => x.es_activo == true).OrderBy(x => x.Id) on t1.Id equals t2.id_Organismo
                                          join t5 in dependencias.ToList() on t1.Id equals t5.id_Dependencia
                                          select new OrganismosViewModel
                                          {
                                              Id = t1.Id,
                                              Nombre = t1.Nombre
                                          })
                                          .GroupBy(c => c.Id).Select(grp => grp.First()).ToList().OrderBy(l => l.Nombre);
                        }
                    }

                } 
            }
            
            var ID_Organismo = 0;

            if (Id != null)
            {
                ID_Organismo = (int)Id;

                organismos = this._serviciosOrganismos.ListOrganismos().Where(x => x.Id == ID_Organismo).ToList().OrderBy(x => x.Nombre);

            }

            SelectList selectList = new SelectList(organismos, "Id", "Nombre", 1);

            this.ViewData["organismos"] = selectList;

            this.ViewBag.Id_Organismos = ID_Organismo;
        }

         public void comboPuertos(int? Id)
         {

             IEnumerable<PuertosViewModel> puertos = new List<PuertosViewModel>();

             if (this.UsuarioFrontalSession.Usuario.categoria == (int)EnumCategoria.Administrador
                 && this.UsuarioFrontalSession.Usuario.categoria == (int)EnumCategoria.PuertosdelEstado)
             {
                    puertos = this._serviciosPuertos.ListPuertos().ToList().OrderBy(x => x.Nombre);
                 
             }
             else
             {
                  puertos = this._serviciosPuertos.ListPuertos().ToList().OrderBy(x => x.Nombre);
               
                 List<DependenciasUsuario> dependencias = this.UsuarioFrontalSession.ListDependenciasDepenUsuarios;
             
                 if (dependencias.Any())
                 {
                         if (this.UsuarioFrontalSession.Usuario.categoria == (int)EnumCategoria.Puerto || this.UsuarioFrontalSession.Usuario.categoria == (int)EnumCategoria.OrganismoGestionPortuaria)
                         {

                             puertos = (from t1 in this._serviciosPuertos.ListPuertos().OrderBy(x => x.Nombre) 
                                                join t5 in dependencias.ToList()
                                                       .Where(x=> x.categoria==(int)EnumCategoria.Puerto) on t1.Id equals t5.id_Dependencia
                                                            select new PuertosViewModel
                                                            {
                                                                Id = t1.Id,
                                                                Nombre = t1.Nombre
                                                            })
                                                       .OrderBy(l => l.Nombre);
                         } else if (this.UsuarioFrontalSession.Usuario.categoria == (int)EnumCategoria.Instalacion)
                         {

                             puertos = (from t1 in this._serviciosPuertos.ListPuertos().OrderBy(x => x.Nombre)
                                        join t5 in dependencias.ToList().Where(x => x.categoria == (int)EnumCategoria.Puerto) on t1.Id equals t5.id_Dependencia
                                        select new PuertosViewModel
                                        {
                                            Id = t1.Id,
                                            Nombre = t1.Nombre
                                        })
                                                       .OrderBy(l => l.Nombre);
                         } 



                 }
             }
             
             var ID_Puerto = 0;

             if (Id != null)
             {
                 ID_Puerto = (int)Id;

                 puertos = this._serviciosPuertos.ListPuertos().Where(x => x.Id == ID_Puerto).ToList().OrderBy(x => x.Nombre);

             }

             SelectList selectList = new SelectList(puertos, "Id", "Nombre", 1);

             this.ViewData["puertos"] = selectList;

             this.ViewBag.Id_Puertos = ID_Puerto;
         }

         public void comboPuertosOrganismo(int? Id,  int? idOrganismo)
         {
             IEnumerable<PuertosViewModel> puertos = new List<PuertosViewModel>();

                 if (idOrganismo != null)
                 {
                     puertos = this._serviciosPuertos.ListPuertos().Where(x => x.id_Organismo == idOrganismo).OrderBy(x => x.Nombre);
                 }
                 else
                 {
                     puertos = this._serviciosPuertos.ListPuertos();
                 }

             if(this.UsuarioFrontalSession.Usuario.categoria != (int)EnumCategoria.Administrador
                 && this.UsuarioFrontalSession.Usuario.categoria != (int)EnumCategoria.PuertosdelEstado)
             {
                 List<DependenciasUsuario> dependencias = this.UsuarioFrontalSession.ListDependenciasDepenUsuarios;
                 if (dependencias.Any())
                 {
                     if (this.UsuarioFrontalSession.Usuario.categoria == (int)EnumCategoria.Puerto || this.UsuarioFrontalSession.Usuario.categoria == (int)EnumCategoria.OrganismoGestionPortuaria || this.UsuarioFrontalSession.Usuario.categoria == (int)EnumCategoria.Instalacion)
                     {
                         puertos = (from a1 in puertos.ToList()
                                    join b1 in dependencias.ToList().Where(z => z.categoria == (int)EnumCategoria.Puerto) on a1.Id equals b1.id_Dependencia
                                    select new PuertosViewModel
                                    {
                                        Id = a1.Id,
                                        Nombre = a1.Nombre                                        
                                    }
                               );

                         }
                 }

               }

              
             var ID_Puerto = 0;

             if (Id != null)
             {
                 ID_Puerto = (int)Id;

                 puertos = this._serviciosPuertos.ListPuertos().Where(x => x.Id == ID_Puerto).ToList().OrderBy(x => x.Nombre);

             }

             SelectList selectList = new SelectList(puertos, "Id", "Nombre", 1);

             this.ViewData["puertos"] = selectList;

             this.ViewBag.Id_Puertos = ID_Puerto;
         }

         /// <summary>
         /// Retorna las provincias registradas en el aplicación.
         /// </summary> 
         public void ComboIslas(int? Id)
         {

             IEnumerable <Islas> isla = this._serviciosPaisesProvincias.GetallIslas();

             var ID_Isla = 0;

             if (Id != null)
             {
                 ID_Isla = (int)Id;

                 isla = this._serviciosPaisesProvincias.GetallIslas().Where(x => x.id == ID_Isla).ToList();

             }

             SelectList selectList = new SelectList(isla, "id", "nombre", 1);

             this.ViewData["Islas"] = selectList;

             this.ViewBag.Id_Isla = ID_Isla;
         }

         /// <summary>
         /// Retorna las capitanias registradas en el aplicación.
         /// </summary> 
         public void ComboCapitanias(int? Id)
         {

             IEnumerable<ListCapitanias> capitania = this._serviciosPaisesProvincias.GetAllCapitanias().OrderBy(x => x.nombre); ;

             var ID_Capitania = 0;

             if (Id != null)
             {
                 ID_Capitania = (int)Id;

                 capitania = this._serviciosPaisesProvincias.GetAllCapitanias().Where(x => x.Id == ID_Capitania).ToList().OrderBy(x=> x.nombre);

             }

             SelectList selectList = new SelectList(capitania, "Id", "Nombre", 1);

             this.ViewData["Capitanias"] = selectList;

             this.ViewBag.Id_Capitania = ID_Capitania;
         }


         /// <summary>
         /// Retorna las provincias registradas en el aplicación.
         /// </summary> 
         public void ComboUsuarios(int? id)
         {

             List<Usuarios> Usua = new List<Usuarios>();

             if (id == null)
             {
                 this.repoUsuarios.GetAll().Where(x=> x.es_activo == true).ToList().ForEach(
                     p =>
                     {

                         Usuarios usuario = new Usuarios() { id = p.id, nombre = p.nombre };

                         Usua.Add(usuario);
                     });


                 this.IdUsuario = Usua.OrderBy(x => x.id).ToList().First().id;

             }
             else
             {
                 Usua = this.repoUsuarios.GetAll().Where(x => x.id == id).ToList();

             }

             SelectList selectList = new SelectList(Usua, "id", "nombre", 1);

             this.ViewData["UsuariosCombo"] = selectList;
         }

         /// <summary>
         /// Retorna las provincias registradas en el aplicación.
         /// </summary> 
         public void ComboUsuariosOPIP(int? id)
         {

             List<Usuarios> Usua = new List<Usuarios>();

             if (id != null)
             {
                 this.repoUsuarios.GetAll().Where(x => x.es_opip == true && x.es_activo == true).ToList().ForEach(
                     p =>
                     {

                         Usuarios usuario = new Usuarios() { id = p.id, nombre = p.nombre };

                         Usua.Add(usuario);
                     });


                 this.IdUsuario = Usua.OrderBy(x => x.id).ToList().First().id;

             }
             else
             {
                 this.repoUsuarios.GetAll().Where(x => x.es_opip == true && x.es_activo == true).ToList().ForEach(
                       p =>
                       {

                           Usuarios usuario = new Usuarios() { id = p.id, nombre = p.nombre };

                           Usua.Add(usuario);
                       });
             
             }
             

             SelectList selectList = new SelectList(Usua, "id", "nombre", 1);

             this.ViewData["UsuariosCombo"] = selectList;
         }


         public void comboInstalacionesPuertos(int? Id, int? idPuerto)
         {

             IEnumerable<InstalacionViewModel> Instalaciones = new List<InstalacionViewModel>();

            Instalaciones = this._serviciosTipoInstalacion.GetAllInstalaciones().Where(x=> x.Es_Activo == true);

             if (this.UsuarioFrontalSession.Usuario.categoria != (int)EnumCategoria.Administrador
                && this.UsuarioFrontalSession.Usuario.categoria != (int)EnumCategoria.PuertosdelEstado)
             {
                 List<DependenciasUsuario> dependencias = this.UsuarioFrontalSession.ListDependenciasDepenUsuarios;
                 if (dependencias.Any())
                 {
                     if (this.UsuarioFrontalSession.Usuario.categoria == (int)EnumCategoria.Puerto || this.UsuarioFrontalSession.Usuario.categoria == (int)EnumCategoria.OrganismoGestionPortuaria || this.UsuarioFrontalSession.Usuario.categoria == (int)EnumCategoria.Instalacion)
                     {
                         Instalaciones = (from a1 in Instalaciones.ToList()
                                    join b1 in dependencias.ToList().Where(z => z.categoria == (int)EnumCategoria.Instalacion) on a1.Id equals b1.id_Dependencia
                                    select new InstalacionViewModel
                                    {
                                        Id = a1.Id,
                                        Nombre = a1.Nombre,
                                        Id_puerto = a1.Id_puerto
                                    }
                               ).OrderBy(x => x.Nombre);

                     }

                 }
             }

             if (idPuerto != null)
             {
                 Instalaciones = Instalaciones.ToList().Where(x => x.Id_puerto == idPuerto).OrderBy(x => x.Nombre);
             }
             if (Id != null)
             {
                 Instalaciones = Instalaciones.Where(x => x.Id == Id).OrderBy(x => x.Nombre);
             }


             SelectList selectList = new SelectList(Instalaciones, "Id", "Nombre", 1);

             this.ViewData["Instalaciones"] = selectList;

         }

         public string MostrarProvincias(int? id)
         {
             var resultado = this.db.Provincias.Where(x => x.id == id).ToList().FirstOrDefault().nombre;

             return resultado;
         }

         public string MostrarOrganismo(int? id)
         {
             var resultado = this.db.Organismos.Where(x => x.Id == id).ToList().FirstOrDefault().Nombre;

             return resultado;
         }

         public string MostrarCapitania(int? id)
         {
             var resultado = this.db.Capitanias.Where(x => x.Id == id).ToList().FirstOrDefault().nombre;

             return resultado;
         }

         public string MostrarInstalacion(string instala)
         {

             var resultado = string.Empty;

             ListClasificacion[] listClasificacion = new[]
                                        {
                                            new ListClasificacion { Value = "1", Text = "725" }, 
                                            new ListClasificacion { Value = "2", Text = "SEVESO" },
                                            new ListClasificacion { Value = "3", Text = "Alto Riesgo" }, 
                                            new ListClasificacion { Value = "4", Text = "Bajo Riesgo" }, 
                                            new ListClasificacion { Value = "5", Text = "Zonas Adyacentes" },
                                            new ListClasificacion { Value = "6", Text = "Tráficos Esporádicos" },
                                       };


             if (instala!="0")
             {
                resultado = listClasificacion.Where(x => x.Value == instala).ToList()[0].Text;    
             }
             return resultado;
         }


        public string MostrarCategoria(int? categoria)
         {
             var resultado = string.Empty;

             switch (categoria)
             {
                 case (int)EnumCategoria.OrganismoGestionPortuaria:

                     resultado = TipoCategoria.Organismo.ToDescription();

                     break;

                 case (int)EnumCategoria.Puerto:

                     resultado = TipoCategoria.Puerto.ToDescription();

                     break;

                 case (int)EnumCategoria.Instalacion:

                     resultado = TipoCategoria.Instalacion.ToDescription();

                     break;
             }

             return resultado;
         }

        #endregion

        public bool ValidarCategoria(int? Id, object myobject)
        {
            if (myobject == null) return false;
            bool sw = true;
            Type t = myobject.GetType();
            PropertyInfo[] props = t.GetProperties();
            Dictionary<string, object> dict = new Dictionary<string, object>();
            foreach (PropertyInfo prp in props)
            {
                object value = prp.GetValue(myobject, new object[] { });
                dict.Add(prp.Name, value);
            }
            foreach (var key in dict)
            {
                if (key.Key.ToUpper() == "ID_USU_ALTA")
                {
                    var valor = (int)key.Value;
                    var users = this.db.Usuarios.Where(y => y.id == valor);
                    this.Description = MostrarCategoria(users.ToList().FirstOrDefault().categoria);
                    if (users.ToList().FirstOrDefault().categoria != this.UsuarioFrontalSession.Usuario.categoria)
                    {
                        sw = false;
                        break;
                    }
                }
            }
            return sw;
        }

        /// <summary>
        /// Limpiar las colecciones.
        /// </summary> 
        public void LimpiarColecciones()
        {
            if (this.listaPuertosSession.Any())
            {
                this.listaPuertosSession.ToList().ForEach(p => { this.listaPuertosSession.Remove(p); });
            }

            if (this.listaPuertosSessionAsignadas.Any())
            {
                this.listaPuertosSessionAsignadas.ToList().ForEach(p => { this.listaPuertosSessionAsignadas.Remove(p); });
            }

            if (this.listaOrganismosSession.Any())
            {
                this.listaOrganismosSession.ToList().ForEach(p => { this.listaOrganismosSession.Remove(p); });
            }

            if (this.listaOrganismosSessionAsignadas.Any())
            {
                this.listaOrganismosSessionAsignadas.ToList().ForEach(p => { this.listaOrganismosSessionAsignadas.Remove(p); });
            }
            
            if (this.listaInstalacionSession.Any())
            {
                this.listaInstalacionSession.ToList().ForEach(p => { this.listaInstalacionSession.Remove(p); });
            }
            if (this.listaInstalacionAsignadas.Any())
            
                this.listaInstalacionAsignadas.ToList().ForEach(p => { this.listaInstalacionAsignadas.Remove(p); });
            }
        
        #region propiedades


        ///<summary>
        ///Colección de Lista de Bien Padre
        ///</summary>
        protected IList<BienesViewModel> ListBienPadre
        {
            get
            {
                if (this.Session["ListBienPadre"] == null)
                {
                    this.Session["ListBienPadre"] = new List<BienesViewModel>();
                }
                return this.Session["ListBienPadre"] as IList<BienesViewModel>;
            }
            set
            {
                this.Session["ListBienPadre"] = value;
            }

        }

        ///<summary>
        ///Colección de Lista de Amenazas Padre e Hijos
        ///</summary>
        protected IList<BienesHijosViewModel> ListBienHijos
        {
            get
            {
                if (this.Session["ListBienHijos"] == null)
                {
                    this.Session["ListBienHijos"] = new List<BienesHijosViewModel>();
                }
                return this.Session["ListBienHijos"] as IList<BienesHijosViewModel>;
            }
            set
            {
                this.Session["ListBienHijos"] = value;
            }
        }

        ///<summary>
        ///Colección de Lista de Amenazas Padre e Hijos
        ///</summary>
        protected IList<ListAmenazasPadreHijos> ListAmenazasPadreHijos
        {
            get
            {
                if (this.Session["ListAmenazasPadreHijos"] == null)
                {
                    this.Session["ListAmenazasPadreHijos"] = new List<ListAmenazasPadreHijos>();
                }
                return this.Session["ListAmenazasPadreHijos"] as IList<ListAmenazasPadreHijos>;
            }
            set
            {
                this.Session["ListAmenazasPadreHijos"] = value;
            }
        }



        ///<summary>
        ///Colección de ListPermisos
        ///</summary>
        protected IList<ListPermisos> ListPermisos
        {
            get
            {
                if (this.Session["ListPermisosSession"] == null)
                {
                    this.Session["ListPermisosSession"] = new List<ListPermisos>();
                }
                return this.Session["ListPermisosSession"] as IList<ListPermisos>;
            }
            set
            {
                this.Session["ListPermisosSession"] = value;
            }
        }

        ///<summary>
        ///Colección de ListBienesPadres
        ///</summary>
        protected IList<BienesViewModel> ListBienesPadres
        {
            get
            {
                if (this.Session["ListBienesPadres"] == null)
                {
                    this.Session["ListBienesPadres"] = new List<BienesViewModel>();
                }
                return this.Session["ListBienesPadres"] as IList<BienesViewModel>;
            }
            set
            {
                this.Session["ListBienesPadres"] = value;
            }
        }
        ///<summary>
        ///Colección de Instalaciones
        ///</summary>
        protected IList<AccionesViewModel> listaAcciones
        {
            get
            {
                if (this.Session["listaAccionesSession"] == null)
                {
                    this.Session["listaAccionesSession"] = new List<AccionesViewModel>();
                }
                return this.Session["listaAccionesSession"] as IList<AccionesViewModel>;
            }
            set
            {
                this.Session["listaAccionesSession"] = value;
            }
        }

        ///<summary>
        ///Colección de Instalaciones
        ///</summary>
        protected IList<MOV_Instalaciones> listaInstalacionSession
        {
            get
            {
                if (this.Session["listaInstalacionSession"] == null)
                {
                    this.Session["listaInstalacionSession"] = new List<MOV_Instalaciones>();
                }
                return this.Session["listaInstalacionSession"] as IList<MOV_Instalaciones>;
            }
            set
            {
                this.Session["listaInstalacionSession"] = value;
            }
        }
        
        ///<summary>
        ///Colección de Organismos
        ///</summary>
        protected IList<Organismos> listaOrganismosSession
        {
            get
            {
                if (this.Session["listaOrganismosSession"] == null)
                {
                    this.Session["listaOrganismosSession"] = new List<Organismos>();
                }
                return this.Session["listaOrganismosSession"] as IList<Organismos>;
            }
            set
            {
                this.Session["listaOrganismosSession"] = value;
            }
        }

        ///<summary>
        ///Colección de Organismos Asignados.
        ///</summary>
        protected IList<Organismos> listaOrganismosSessionAsignadas
        {
            get
            {
                if (this.Session["listaOrganismosSessionAsignadas"] == null)
                {
                    this.Session["listaOrganismosSessionAsignadas"] = new List<Organismos>();
                }
                return this.Session["listaOrganismosSessionAsignadas"] as IList<Organismos>;
            }
            set
            {
                this.Session["listaOrganismosSessionAsignadas"] = value;
            }
        }


        ///<summary>
        ///Colección de Puertos
        ///</summary>
        protected IList<PuertosViewModel> listaPuertosSession
        {
            get
            {
                if (this.Session["listaPuertosSession"] == null)
                {
                    this.Session["listaPuertosSession"] = new List<PuertosViewModel>();
                }
                return this.Session["listaPuertosSession"] as IList<PuertosViewModel>;
            }
            set
            {
                this.Session["listaPuertosSession"] = value;
            }
        }

        ///<summary>
        ///Colección de Puertos Asignados.
        ///</summary>
        protected IList<PuertosViewModel> listaPuertosSessionAsignadas
        {
            get
            {
                if (this.Session["listaPuertosSessionAsignadas"] == null)
                {
                    this.Session["listaPuertosSessionAsignadas"] = new List<PuertosViewModel>();
                }
                return this.Session["listaPuertosSessionAsignadas"] as IList<PuertosViewModel>;
            }
            set
            {
                this.Session["listaPuertosSessionAsignadas"] = value;
            }
        }

        
        ///<summary>
        ///Colección de instalación Asignadas.
        ///</summary>
        protected IList<MOV_Instalaciones> listaInstalacionAsignadas
        {
            get
            {
                if (this.Session["listaInstalacionAsignadas"] == null)
                {
                    this.Session["listaInstalacionAsignadas"] = new List<MOV_Instalaciones>();
                }
                return this.Session["listaInstalacionAsignadas"] as IList<MOV_Instalaciones>;
            }
            set
            {
                this.Session["listaInstalacionAsignadas"] = value;
            }
        }

        /// <summary>
        /// Retorna los diferentes grupos que se pueden asociar a los usuarios.
        /// </summary> 
        public void ComboGrupos()
        {
            IEnumerable<ListGrupos> listaGrupos = new List<ListGrupos>();
            if (this.UsuarioFrontalSession.Usuario.categoria == (int)EnumCategoria.Administrador
                 || this.UsuarioFrontalSession.Usuario.categoria == (int)EnumCategoria.PuertosdelEstado)
            {

                listaGrupos = (from t1 in this.repoGrupos.GetAll().OrderBy(x => x.Id)
                                                       select new
                                                           ListGrupos
                                                           {
                                                               Value = t1.Id.ToString(),
                                                               Text = t1.Nombre
                                                           });
            }
            else if (this.UsuarioFrontalSession.Usuario.categoria == (int)EnumCategoria.OrganismoGestionPortuaria)
            {
                listaGrupos = (from t1 in this.repoGrupos.GetAll().Where(y=> y.Id_publico == (int)EnumPublico.Publico || y.Id_publico == (int)EnumPublico.Organismo).OrderBy(x => x.Id )
                               select new
                                   ListGrupos
                               {
                                   Value = t1.Id.ToString(),
                                   Text = t1.Nombre
                               });
            }
            else
            {
                listaGrupos = (from t1 in this.repoGrupos.GetAll().Where(y => y.Id_publico == (int)EnumPublico.Publico).OrderBy(x => x.Id)
                               select new
                                   ListGrupos
                               {
                                   Value = t1.Id.ToString(),
                                   Text = t1.Nombre
                               });            
            }


               this.ViewData["Grupos"] = listaGrupos.OrderBy(x=>x.Text); 
        }
        ///<summary>
        ///Almacenamos los datos del usuario.
        ///</summary>
        ///
        protected UsuariosEditJson DatosUsuario
        {
            get
            {
                if (this.Session["DatosUsuario"] == null)
                {
                    this.Session["DatosUsuario"] = new UsuariosEditJson();
                }
                return this.Session["DatosUsuario"] as UsuariosEditJson;
            }
            set
            {
                this.Session["DatosUsuario"] = value;
            }
        }

        ///<summary>
        ///Colección de los grupos activos.
        ///</summary>
        protected IList<AltaPerfiles> listaGruposSession
        {
            get
            {
                if (this.Session["listaGruposSession"] == null)
                {
                    this.Session["listaGruposSession"] = new List<AltaPerfiles>();
                }
                return this.Session["listaGruposSession"] as IList<AltaPerfiles>;
            }
            set
            {
                this.Session["listaGruposSession"] = value;
            }
        }



        /// <summary>
        /// Colección de los perfiles asignados del grupo.
        /// </summary>
        protected IList<AltaPerfiles> listaGruposAsignadosSession
        {
            get
            {
                if (this.Session["listaGruposAsignadosSession"] == null)
                {
                    this.Session["listaGruposAsignadosSession"] = new List<AltaPerfiles>();
                }
                return this.Session["listaGruposAsignadosSession"] as IList<AltaPerfiles>;
            }
            set
            {
                this.Session["listaGruposAsignadosSession"] = value;
            }
        }

        protected IList<AltaAcciones> PermisosRemovidosSession
        {
            get
            {
                if (this.Session["PermisosRemovidosSession"] == null)
                {
                    this.Session["PermisosRemovidosSession"] = new List<AltaAcciones>();
                }
                return this.Session["PermisosRemovidosSession"] as IList<AltaAcciones>;
            }
            set
            {
                this.Session["listagestionpermisosSession"] = value;
            }
        }

        protected IList<AltaAcciones> PermisosAltaSession
        {
            get
            {
                if (this.Session["PermisosAltaSession"] == null)
                {
                    this.Session["PermisosAltaSession"] = new List<AltaAcciones>();
                }
                return this.Session["PermisosAltaSession"] as IList<AltaAcciones>;
            }
            set
            {
                this.Session["listagestionpermisosSession"] = value;
            }
        }

        protected IList<ListGruposPerfilesPermisos> listaPermisosAsignadosSession
        {
            get
            {
                if (this.Session["listaPermisosAsiganadosSession"] == null)
                {
                    this.Session["listaPermisosAsiganadosSession"] = new List<ListGruposPerfilesPermisos>();
                }
                return this.Session["listaPermisosAsiganadosSession"] as IList<ListGruposPerfilesPermisos>;
            }
            set
            {
                this.Session["listagestionpermisosSession"] = value;
            }
        }

        protected IList<AltaAcciones> listagestionpermisosSession
        {
            get
            {
                if (this.Session["listagestionpermisosSession"] == null)
                {
                    this.Session["listagestionpermisosSession"] = new List<AltaAcciones>();
                }
                return this.Session["listagestionpermisosSession"] as IList<AltaAcciones>;
            }
            set
            {
                this.Session["listagestionpermisosSession"] = value;
            }
        }



        protected IList<AltaAcciones> FiltrolistagestionpermisosSession
        {
            get
            {
                if (this.Session["FiltrolistagestionpermisosSession"] == null)
                {
                    this.Session["FiltrolistagestionpermisosSession"] = new List<AltaAcciones>();
                }
                return this.Session["FiltrolistagestionpermisosSession"] as IList<AltaAcciones>;
            }
            set
            {
                this.Session["FiltrolistagestionpermisosSession"] = value;
            }
        }


        protected IList<AltaAcciones> listapermisosSession
        {
            get
            {
                if (this.Session["listapermisosSession"] == null)
                {
                    this.Session["listapermisosSession"] = new List<AltaAcciones>();
                }
                return this.Session["listapermisosSession"] as IList<AltaAcciones>;
            }
            set
            {
                this.Session["listapermisosSession"] = value;
            }
        }


        protected IList<Docs_Usuario> listadocumentosAsociados
        {
            get
            {
                if (this.Session["listadocumentosAsociados"] == null)
                {
                    this.Session["listadocumentosAsociados"] = new List<Docs_Usuario>();
                }
                return this.Session["listadocumentosAsociados"] as IList<Docs_Usuario>;
            }
            set
            {
                this.Session["listadocumentosAsociados"] = value;
            }
        }

        protected UsuarioFrontal UsuarioFrontalSession
        {
            get
            {

                if (this.Session["UsuarioFrontal"] == null)
                {
                    this.Session["UsuarioFrontal"] = new UsuarioFrontal();
                }
                return this.Session["UsuarioFrontal"] as UsuarioFrontal;
            }
            set
            {
                this.Session["UsuarioFrontal"] = value;
            }
        }

        /// <summary>
        ///Contiene Información del Navegador.
        /// </summary>
        protected string Browser
        {
            get
            {
                return this.Session["Browser"] as string;
            }
            set
            {
                this.Session["Browser"] = value;
            }
        }


        protected string NombreHijo
        {
            get
            {
                return this.Session["NombreHijo"] as string;
            }
            set
            {
                this.Session["NombreHijo"] = value;
            }
        }

        /// <summary>
        /// Contiene el idProvincia seleccionado.
        /// </summary>
        protected int? idDeclara
        {
            get
            {
                return this.Session["idDeclara"] as int?;
            }
            set
            {
                this.Session["idDeclara"] = value;
            }
        }
        /// <summary>
        /// Contiene el idOrganismo seleccionado.
        /// </summary>
        protected int? idOrganismo
        {
            get
            {
                return this.Session["idOrganismo"] as int?;
            }
            set
            {
                this.Session["idOrganismo"] = value;
            }
        }

        /// <summary>
        /// Contiene el idProvincia seleccionado.
        /// </summary>
        protected int? idProvincia
        {
            get
            {
                return this.Session["idProvincia"] as int?;
            }
            set
            {
                this.Session["idProvincia"] = value;
            }
        }

        /// <summary>
        /// Contiene el idPuerto seleccionado de dependencia.
        /// </summary>
        protected int? idPuerto
        {
            get
            {
                return this.Session["idPuerto"] as int?;
            }
            set
            {
                this.Session["idPuerto"] = value;
            }
        }

        /// <summary>
        /// Contiene el idDependencia seleccionado de dependencia.
        /// </summary>
        protected int? idInstalacion
        {
            get
            {
                return this.Session["idInstalacion"] as int?;
            }
            set
            {
                this.Session["idInstalacion"] = value;
            }
        }

        /// <summary>
        /// Contiene el idComunicacion seleccionado de dependencia.
        /// </summary>
        protected int? IdComunicacion
        {
            get
            {
                return this.Session["idComunicacion"] as int?;
            }
            set
            {
                this.Session["idComunicacion"] = value;
            }
        }

        /// <summary>
        /// Contiene el idDependencia seleccionado de dependencia.
        /// </summary>
        protected int? idDependencia
        {
            get
            {
                return this.Session["idDependencia"] as int?;
            }
            set
            {
                this.Session["idDependencia"] = value;
            }
        }

        /// <summary>
        /// Contiene el idGrupo seleccionado de Grupos.
        /// </summary>
        protected double? idGrupo
        {
            get
            {
                return this.Session["idGrupo"] as double?;
            }
            set
            {
                this.Session["idGrupo"] = value;
            }
        }

        /// <summary>
        /// Contiene el idFilter para filtrar el buscador
        /// </summary>
        protected double? idFilter
        {
            get
            {
                return this.Session["idFilter"] as double?;
            }
            set
            {
                this.Session["idFilter"] = value;
            }
        }
        
        /// <summary>
        /// Contiene el Id_Grupo_Accion seleccionada.
        /// </summary>
        protected int idConjunto
        {
            get
            {
                if (this.Session["idConjunto"] == null)
                {
                    return 0;
                }
                return (int)this.Session["idConjunto"];
            }
            set
            {
                this.Session["idConjunto"] = value;
            }
        }


        /// <summary>
        /// Contiene el IdUsuario seleccionado
        /// </summary>
        protected int IdUsuario
        {
            get
            {
                if (this.Session["IdUsuario"] == null)
                {
                    return 0;
                }
                return (int)this.Session["IdUsuario"];
            }
            set
            {
                this.Session["IdUsuario"] = value;
            }
        }


        /// <summary>
        /// Contiene el idClasificacion seleccionado de Instalacion.
        /// </summary>
        protected int? idClasificacion
        {
            get
            {
                return this.Session["idClasificacion"] as int?;
            }
            set
            {
                this.Session["idClasificacion"] = value;
            }
        }

        /// <summary>
        /// Contiene el isortDirection
        /// </summary>
        protected string sortDirection
        {
            get
            {
                return this.Session["sortDirection"] as string;
            }
            set
            {
                this.Session["sortDirection"] = value;
            }
        }


        protected string IdpadreHijo
        {
            get
            {
                return this.Session["IdpadreHijo"] as string;
            }
            set
            {
                this.Session["IdpadreHijo"] = value;
            }
        }

        protected string Idpadre
        {
            get
            {
                return this.Session["Idpadre"] as string;
            }
            set
            {
                this.Session["Idpadre"] = value;
            }
        }

        /// <summary>
        /// Contiene el isortDirection
        /// </summary>
        protected string Description
        {
            get
            {
                return this.Session["Description"] as string;
            }
            set
            {
                this.Session["Description"] = value;
            }
        }

        /// <summary>
        /// Contiene el isNombreSortable
        /// </summary>
        protected bool BienHijo2
        {
            get
            {
                if (this.Session["BienHijo2"] == null)
                {
                    return false;
                }
                return (bool)this.Session["BienHijo2"];
            }
            set
            {
                this.Session["BienHijo2"] = value;
            }
        }

        /// <summary>
        /// Contiene el isNombreSortable
        /// </summary>
        protected bool isNombreSortable
        {
            get
            {
                if (this.Session["isNombreSortable"] == null)
                {
                    return false;
                }
                return (bool)this.Session["isNombreSortable"];
            }
            set
            {
                this.Session["isNombreSortable"] = value;
            }
        }
        /// <summary>
        /// Contiene el idPerfil seleccionado de Perfil.
        /// </summary>
        protected double? idPerfil
        {
            get
            {
                return this.Session["idPerfil"] as double?;
            }
            set
            {
                this.Session["idPerfil"] = value;
            }
        }

        /// <summary>
        /// Contiene el idPais seleccionado.
        /// </summary>
        protected int? idPais
        {
            get
            {
                return this.Session["idPais"] as int?;
            }
            set
            {
                this.Session["idPais"] = value;
            }
        }

        /// <summary>
        /// Contiene el idtipoDocumento seleccionado de docuemntos de usuario.
        /// </summary>
        protected string idTipoDocumento
        {
            get
            {
                return this.Session["idTipoDocumento"].ToString();
            }
            set
            {
                this.Session["idTipoDocumento"] = value;
            }
        }



        /// <summary>
        /// Contiene el descripcionOpcion seleccionado de la opción menú.
        /// </summary>
        protected string descripcionOpcion
        {
            get
            {
                return this.Session["descripcionOpcion"] as string;
            }
            set
            {
                this.Session["descripcionOpcion"] = value;
            }
        }


        /// <summary>
        /// Contiene el idPais seleccionado.
        /// </summary>
        protected int? idCiudad
        {
            get
            {
                return this.Session["idCiudad"] as int?;
            }
            set
            {
                this.Session["idCiudad"] = value;
            }
        }

        /// <summary>
        /// Contiene el IdUsuario seleccionado
        /// </summary>
        protected int IdComite
        {
            get
            {
                if (this.Session["IdComite"] == null)
                {
                    return 0;
                }
                return (int)this.Session["IdComite"];
            }
            set
            {
                this.Session["IdComite"] = value;
            }
        }

        /// <summary>
        /// Contiene el IdUsuario seleccionado
        /// </summary>
        protected int IdFormacion
        {
            get
            {
                if (this.Session["IdFormacion"] == null)
                {
                    return 0;
                }
                return (int)this.Session["IdFormacion"];
            }
            set
            {
                this.Session["IdFormacion"] = value;
            }
        }

        protected int IdPractica
        {
            get
            {
                if (this.Session["IdPractica"] == null)
                {
                    return 0;
                }
                return (int)this.Session["IdPractica"];
            }
            set
            {
                this.Session["IdPractica"] = value;
            }
        }


        protected int IdIncidente
        {
            get
            {
                if (this.Session["IdIncidente"] == null)
                {
                    return 0;
                }
                return (int)this.Session["IdIncidente"];
            }
            set
            {
                this.Session["IdIncidente"] = value;
            }
        }

        protected int IdMantenimiento
        {
            get
            {
                if (this.Session["IdMantenimiento"] == null)
                {
                    return 0;
                }
                return (int)this.Session["IdMantenimiento"];
            }
            set
            {
                this.Session["IdMantenimiento"] = value;
            }
        }

        protected int IdAuditoria
        {
            get
            {
                if (this.Session["IdAuditoria"] == null)
                {
                    return 0;
                }
                return (int)this.Session["IdAuditoria"];
            }
            set
            {
                this.Session["IdAuditoria"] = value;
            }
        }

        protected int IdNivel
        {
            get
            {
                if (this.Session["IdNivel"] == null)
                {
                    return 0;
                }
                return (int)this.Session["IdNivel"];
            }
            set
            {
                this.Session["IdNivel"] = value;
            }
        }

        protected int IdCentro
        {
            get
            {
                if (this.Session["IdCentro"] == null)
                {
                    return 0;
                }
                return (int)this.Session["IdCentro"];
            }
            set
            {
                this.Session["IdCentro"] = value;
            }
        }

        protected IEnumerable<Comunica_Centro> ComunicaCentro
        {
            get
            {
                if (this.Session["ComunicaCentro"] == null)
                {
                    return null;
                }
                return (IEnumerable<Comunica_Centro>)this.Session["ComunicaCentro"];
            }
            set
            {
                this.Session["ComunicaCentro"] = value;
            }
        }

        /// <summary>
        /// Contiene el IdGisis seleccionado
        /// </summary>
        protected int IdGisis
        {
            get
            {
                if (this.Session["IdGisis"] == null)
                {
                    return 0;
                }
                return (int)this.Session["IdGisis"];
            }
            set
            {
                this.Session["IdGisis"] = value;
            }
        }

         /// <summary>
        /// Contiene el IdProcedimiento seleccionado
        /// </summary>
        protected int IdProcedimiento
        {
            get
            {
                if (this.Session["IdProcedimiento"] == null)
                {
                    return 0;
                }
                return (int)this.Session["IdProcedimiento"];
            }
            set
            {
                this.Session["IdProcedimiento"] = value;
            }
        }

        /// <summary>
        /// Contiene el SI es par AP  
        /// </summary>
        protected int Id_TipoAP
        {
            get
            {
                if (this.Session["Id_TipoAP"] == null)
                {
                    return 0;
                }
                return (int)this.Session["Id_TipoAP"];
            }
            set
            {
                this.Session["Id_TipoAP"] = value;
            }
        }


        /// <summary>
        /// Contiene el idBien seleccionado.
        /// </summary>
        protected int? idBien
        {
            get
            {
                return this.Session["idBien"] as int?;
            }
            set
            {
                this.Session["idBien"] = value;
            }
        }

        /// <summary>
        /// Contiene el idBien seleccionado.
        /// </summary>
        protected int? idEvaluaBien
        {
            get
            {
                return this.Session["idEvaluaBien"] as int?;
            }
            set
            {
                this.Session["idEvaluaBien"] = value;
            }
        }



        /// <summary>
        /// Contiene el IdGisis seleccionado
        /// </summary>
        protected string TipoDocumento
        {
            get
            {
                if (this.Session["TipoDocumento"] == null)
                {
                    return string.Empty;
                }
                return (string)this.Session["TipoDocumento"];
            }
            set
            {
                this.Session["TipoDocumento"] = value;
            }
        }


        /// <summary>
        /// Contiene el idBien seleccionado.
        /// </summary>
        protected int? idEvaluacion
        {
            get
            {
                return this.Session["idEvaluacion"] as int?;
            }
            set
            {
                this.Session["idEvaluacion"] = value;
            }
        }

        protected int? VersionEval
        {
            get
            {
                return this.Session["VersionEval"] as int?;
            }
            set
            {
                this.Session["VersionEval"] = value;
            }
        }

        protected IEnumerable<Detalle_EvaluacionViewModel> DetalleUpdate
        {
            get
            {
                if (this.Session["DetalleUpdate"] == null)
                {
                    return null;
                }
                return (IEnumerable<Detalle_EvaluacionViewModel>)this.Session["DetalleUpdate"];
            }
            set
            {
                this.Session["DetalleUpdate"] = value;
            }
        }


        protected string AccionEstado
        {
            get
            {
                if (this.Session["AccionEstado"] == null)
                {
                    return string.Empty;
                }
                return this.Session["AccionEstado"].ToString();
            }
            set
            {
                this.Session["AccionEstado"] = value;
            }
        }


        
        public int EnConstruction
        {
            get
            {
                return int.Parse(ConfigurationManager.AppSettings["EnConstruction"]);
            }
        }


        protected int AsignadoPadre
        {
            get
            {
                if (this.Session["AsignadoPadre"] == null)
                {
                    return 0;
                }
                return (int)this.Session["AsignadoPadre"];
            }
            set
            {
                this.Session["AsignadoPadre"] = value;
            }
        }

        #endregion

        public string MostrarMotivo(int id)
        {
            return this.db.Motivos.Where(x => x.Id == id).ToList().FirstOrDefault().Motivo;
        }
    }
}