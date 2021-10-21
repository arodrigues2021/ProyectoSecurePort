namespace SecurePort.Entities
{
    #region Using
    using System.Data.Entity;
    using SecurePort.Entities.DataContextConfigs;
    using SecurePort.Entities.Models;
    #endregion

    public class SecurePortContext : DbContext
    {
        public SecurePortContext()
            : base("Name=SecurePortContext")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Configurations.Add(new BienesTypeConfig());

            modelBuilder.Configurations.Add(new PuertosTypeConfig());

            modelBuilder.Configurations.Add(new InstalacionTypeConfig());

            modelBuilder.Configurations.Add(new Dependencia_UsuarioTypeConfig());

            modelBuilder.Configurations.Add(new UsuariosTypeConfig());

            modelBuilder.Configurations.Add(new GruposTypeConfig());

            modelBuilder.Configurations.Add(new PerfilesTypeConfig());

            modelBuilder.Configurations.Add(new PerfilesGruposTypeConfig());

            modelBuilder.Configurations.Add(new PerfilesPermisosTypeConfig());

            modelBuilder.Configurations.Add(new PermisosTypeConfig());

            modelBuilder.Configurations.Add(new GestionpermisosTypeConfig());

            modelBuilder.Configurations.Add(new Docs_UsuarioTypeConfig());

            modelBuilder.Configurations.Add(new Tipos_ServicioTypeConfig());

            modelBuilder.Configurations.Add(new Tipos_DocumentoTypeConfig());

            modelBuilder.Configurations.Add(new OrganismosTypeConfig());

            modelBuilder.Configurations.Add(new Mov_InstalacionTypeConfig());

            modelBuilder.Configurations.Add(new CiudadesTypeConfig());

            modelBuilder.Configurations.Add(new TrazasTypeConfig());

            modelBuilder.Configurations.Add(new Contactos_OrganismoTypeConfig());

            modelBuilder.Configurations.Add(new PaisesTypeConfig());

            modelBuilder.Configurations.Add(new Comunidades_AutonomasTypeConfig());

            modelBuilder.Configurations.Add(new ProvinciasTypeConfig());

            modelBuilder.Configurations.Add(new CapitaniaTypeConfig());

            modelBuilder.Configurations.Add(new CentrosTypeConfig());

            modelBuilder.Configurations.Add(new AmenazasTypeConfig());

            modelBuilder.Configurations.Add(new MotivosTypeConfig());

            modelBuilder.Configurations.Add(new IslasTypeConfig());

            modelBuilder.Configurations.Add(new ComitesTypeConfig());

            modelBuilder.Configurations.Add(new OperadoresTypeConfig());

            modelBuilder.Configurations.Add(new FormacionesTypeConfig());

            modelBuilder.Configurations.Add(new OperadoresPuertoTypeConfig());

            modelBuilder.Configurations.Add(new PracticasTypeConfig());

            modelBuilder.Configurations.Add(new IncidentesTypeConfig());

            modelBuilder.Configurations.Add(new AuditoriasTypeConfig());

            modelBuilder.Configurations.Add(new NivelesTypeConfig());

            modelBuilder.Configurations.Add(new Comunica_CentroTypeConfig());

            modelBuilder.Configurations.Add(new Mov_MantenimientosTypeConfig());

            modelBuilder.Configurations.Add(new ComunicacionesTypeConfig());

            modelBuilder.Configurations.Add(new Det_NivelTypeConfig());

            modelBuilder.Configurations.Add(new Declara_MaritimaTypeConfig());

            modelBuilder.Configurations.Add(new Det_IncidenteTypeConfig());

            modelBuilder.Configurations.Add(new Det_PracticaTypeConfig());

            modelBuilder.Configurations.Add(new GisisTypeConfig());

            modelBuilder.Configurations.Add(new VinculosTypeConfig());

            modelBuilder.Configurations.Add(new ProcedimientosTypeConfig());

            modelBuilder.Configurations.Add(new MOV_Detalle_InstalacionTypeConfig());

            modelBuilder.Configurations.Add(new EvaluacionesTypeConfig());

            modelBuilder.Configurations.Add(new Versiones_EvaluacionTypeConfig());

            modelBuilder.Configurations.Add(new Historico_EvaluacionTypeConfig());

            modelBuilder.Configurations.Add(new Detalle_EvaluacionTypeConfig());
        }

        public DbSet<Operadores> Operadores { get; set; }

        public DbSet<Puertos> Puertos { get; set; }

        public DbSet<Usuarios> Usuarios { get; set; }

        public DbSet<Bienes> Bienes { get; set; }

        public DbSet<Depen_Usuario> Depen_Usuario { get; set; }

        public DbSet<Docs_Usuario> Docs_Usuario { get; set; }

        public DbSet<Grupos> Grupos { get; set; }

        public DbSet<Perfiles> Perfiles { get; set; }

        public DbSet<PerfilesGrupo> PerfilesGrupos { get; set; }

        public DbSet<AccionesPerfil> PerfilesPermisos { get; set; }

        public DbSet<Acciones> Permisos { get; set; }

        public DbSet<Gestionpermisos> Gestionpermisos { get; set; }

        public DbSet<Tipos_Documento> Tipos_Documento { get; set; }

        public DbSet<Tipos_Servicio> Tipos_Servicio { get; set; }

        public DbSet<Tipos_Instalacion> Tipos_Instalacion { get; set; }

        public DbSet<Organismos> Organismos { get; set; }

        public DbSet<MOV_Instalaciones> MOV_Instalaciones { get; set; }

        public DbSet<Trazas> Trazas { get; set; }

        public DbSet<Ciudades> Ciudades { get; set; }

        public DbSet<Contactos_Organismo> Contactos_Organismo { get; set; }

        public DbSet<Paises> Paises { get; set; }

        public DbSet<Comunidades_Autonomas> Comunidades_Autonomas { get; set; }

        public DbSet<Provincias> Provincias { get; set; }

        public DbSet<Capitanias> Capitanias { get; set; }

        public DbSet<Centros> Centros { get; set; }

        public DbSet<Amenazas> Amenazas { get; set; }

        public DbSet<Motivos> Motivos { get; set; }

        public DbSet<Islas> Islas { get; set; }

        public DbSet<Comites> Comites { get; set; }

        public DbSet<Formaciones> Formaciones { get; set; }

        public DbSet<OperadoresPuerto> OperadoresPuerto { get; set; }

        public DbSet<Practicas> Practicas { get; set; }

        public DbSet<Incidentes> Incidentes { get; set; }

        public DbSet<Auditorias> Auditorias { get; set; }

        public DbSet<Niveles> Niveles { get; set; }

        public DbSet<Comunica_Centro> Comunica_Centro { get; set; }

        public DbSet<Mantenimientos> Mantenimientos { get; set; }

        public DbSet<Comunicaciones> Comunicaciones { get; set; }

        public DbSet<Det_Nivel> Det_Nivel { get; set; }

        public DbSet<DeclaraMaritima> DeclaraMaritima { get; set; }

        public DbSet<Det_Incidente> Det_Incidente { get; set; }

        public DbSet<Det_Practica> Det_Practica { get; set; }

        public DbSet<RegistroGisis> Gisis { get; set; }

        public DbSet<Vinculos> Vinculos { get; set; }

        public DbSet<Procedimientos> Procedimientos { get; set; }

        public DbSet<MOV_Detalle_Instalacion> MOV_Detalle_Instalacion { get; set; }

        public DbSet<Evaluaciones> Evaluaciones { get; set; }

        public DbSet<Versiones_Evaluacion> Versiones_Evaluacion { get; set; }

        public DbSet<Historico_Evaluacion> Historico_Evaluacion { get; set; }

        public DbSet<Detalle_Evaluacion> Detalle_Evaluacion { get; set; }
    }
}
