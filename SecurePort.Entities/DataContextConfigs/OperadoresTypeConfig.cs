namespace SecurePort.Entities.DataContextConfigs
{
    #region Using

    using System.Data.Entity.ModelConfiguration;

    using SecurePort.Entities.Models;

    #endregion

    public class OperadoresTypeConfig : EntityTypeConfiguration<Operadores>
    {
        public OperadoresTypeConfig()
        {
            
            // Primary Key
            this.ToTable("SPMOV_Operadores_Instalacion", "dbo")
                .HasKey(t => t.Id)
                .Property(t => t.Id)
                .HasColumnName("Id");
            
            // Properties
            this.Property(t => t.Id_Instalacion)
                .IsRequired()
                .HasColumnName("Id_Instalacion");

            this.Property(t => t.Nombre)
                .HasColumnName("Nombre");
           
            this.Property(t => t.Es_Suplente)
                .IsRequired()
                .HasColumnName("Es_Suplente");

            this.Property(t => t.Direccion)
                .HasColumnName("Direccion");

            this.Property(t => t.Id_Ciudad)
                .HasColumnName("Id_Ciudad");

            this.Property(t => t.Cod_postal)
                .HasColumnName("Cod_postal");

            this.Property(t => t.Id_provincia)
                .HasColumnName("Id_provincia");
            
            this.Property(t => t.Telefono1)
                .HasColumnName("Telefono1");
            
            this.Property(t => t.Fax)
                .HasColumnName("Fax");

            this.Property(t => t.Email)
                .HasColumnName("Email");
            
            this.Property(t => t.Id_usu_alta)
                .HasColumnName("Id_usu_alta");

            this.Property(t => t.Fech_alta)
                .IsRequired()
                .HasColumnName("Fech_alta");

            this.Property(t => t.Es_Activo)
                .HasColumnName("Es_Activo");

            this.Property(t => t.Fech_activo)
                .HasColumnName("Fech_activo");
            
            this.Property(t => t.Observaciones)
                .HasColumnName("Observaciones");

            this.Property(t => t.Cargo)
               .HasColumnName("Cargo");
        }
    }
}

