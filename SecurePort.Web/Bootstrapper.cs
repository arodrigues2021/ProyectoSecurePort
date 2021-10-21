using Microsoft.Practices.Unity;
using SecurePort.Services.Interfaces;
using System.Web.Mvc;
using Unity.Mvc4;
namespace SecurePort.Web
{
  

    /// <summary>
    /// Clase encargada de inicializar la inyection de dependencias
    /// </summary>
    public static class Bootstrapper
    {
        /// <summary>
        /// Inicialización. Debe invocarse al arrancar la aplicación
        /// </summary>
        /// <returns>Contenedor responsable de IdD</returns>
        public static IUnityContainer Initialise()
        {
            var container = BuildUnityContainer();
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
            return container;
        }

        private static IUnityContainer BuildUnityContainer()
        {
            var container = new UnityContainer();
            RegisterTypes(container);
            return container;
        }

        /// <summary>
        /// Registra los tipos usados en la aplicación
        /// </summary>
        /// <param name="container">Contenedor responsable de IdD</param>
        public static void RegisterTypes(IUnityContainer container)
        {
            //Asocia una interfaz con su clase asociada 
            container.RegisterType<ILogger, Logger>();
            var logger = new Logger();
            container.RegisterInstance<ILogger>(logger);
            container.RegisterType<IServiciosBienes,           ServiciosBienes>();
            container.RegisterType<IServiciosPuertos,          ServiciosPuertos>();
            container.RegisterType<IServiciosUsuarios,         ServiciosUsuarios>();
            container.RegisterType<IServiciosGrupos,           ServiciosGrupos>();
            container.RegisterType<IServiciosOrganismo,        ServiciosOrganismos>();
            container.RegisterType<IServiciosPaisesProvincias, ServiciosPaisesProvincias>();
            container.RegisterType<IServiciosTipoInstalacion,  ServiciosTipoInstalaciones>();
            container.RegisterType<IServiciosCentros,          ServiciosCentros>();
            container.RegisterType<IServiciosAmenazas,         ServiciosAmenazas>();
            container.RegisterType<IServiciosMotivos,          ServiciosMotivos>();
            container.RegisterType<IServiciosComites,          ServiciosComites>();
            container.RegisterType<IServiciosFormaciones,      ServiciosFormaciones>();
            container.RegisterType<IServiciosPracticas,        ServiciosPracticas>();
            container.RegisterType<IServiciosIncidentes,       ServiciosIncidentes>();
            container.RegisterType<IServiciosDocumentos,       ServiciosDocumentos>();
            container.RegisterType<IServiciosAuditorias,       ServiciosAuditorias>();
            container.RegisterType<IServiciosNiveles,          ServiciosNiveles>();
            container.RegisterType<IServiciosMantenimiento,    ServiciosMantenimiento>();
            container.RegisterType<IServiciosComunicaciones,   ServiciosComunicaciones>();
            container.RegisterType<IServiciosDeclaraMaritima,  ServiciosDeclaraMaritima>();
            container.RegisterType<IServiciosGisis,            ServiciosGisis>();
            container.RegisterType<IServiciosVinculos,         ServiciosVinculos>();
            container.RegisterType<IServiciosProcedimientos,   ServiciosProcedimientos>();
            container.RegisterType<IServiciosEvaluaciones, ServiciosEvaluaciones>();            
        }
    }
} 