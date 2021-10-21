namespace SecurePort.Entities.DataContextConfigs
{
    #region Using

    using System.Data.Entity.ModelConfiguration;

    using SecurePort.Entities.Models;

    #endregion

    public class Mov_InstalacionTypeConfig : EntityTypeConfiguration<MOV_Instalaciones>
    {
        public Mov_InstalacionTypeConfig()
        {
            
            // Primary Key
            this.ToTable("SPMOV_Instalaciones","dbo")
                .HasKey(t => t.Id)
                .Property(t => t.Id)
                .HasColumnName("Id");
            
            // Properties
            this.Property(t => t.Nombre)
                .IsRequired()
                .HasColumnName("Nombre");

            this.Property(t => t.id_Puerto)
                .IsRequired()
                .HasColumnName("id_Puerto");

            this.Property(t => t.id_Tipo)
                .IsRequired()
                .HasColumnName("id_Tipo");

            this.Property(t => t.es_concesionada)
                .IsRequired()
                .HasColumnName("es_concesionada");

            this.Property(t => t.Empresa)
                .HasColumnName("Empresa");

            this.Property(t => t.Clasificacion)
                .IsRequired()
                .HasColumnName("Clasificacion");

            this.Property(t => t.Direccion)
                .HasColumnName("Direccion");

            this.Property(t => t.id_Ciudad)
                .HasColumnName("id_Ciudad");

            this.Property(t => t.cod_postal)
                .HasColumnName("cod_postal");

            this.Property(t => t.id_provincia)
                .HasColumnName("id_provincia");

            this.Property(t => t.OMI)
                .HasColumnName("OMI");

            this.Property(t => t.Nivel)
                .IsRequired()
                .HasColumnName("Nivel");

            this.Property(t => t.Longitud)
                .HasColumnName("Longitud");

            this.Property(t => t.Latitud)
                .HasColumnName("Latitud");

            this.Property(t => t.Declara_Cumpli)
                .HasColumnName("Declara_Cumpli");

            this.Property(t => t.Fech_Declara_Cumpli)
                .HasColumnName("Fech_Declara_Cumpli");

            this.Property(t => t.id_usu_alta)
                .HasColumnName("id_usu_alta");

            this.Property(t => t.fech_alta)
                .IsRequired()
                .HasColumnName("fech_alta");

            this.Property(t => t.fech_Activo)
               .IsRequired()
               .HasColumnName("fech_Activo");

            this.Property(t => t.Es_Activo)
                .HasColumnName("Es_Activo");
            
            this.Property(t => t.Observaciones)
                .HasColumnName("Observaciones");
        }
    }
}

