namespace SecurePort.Entities.DataContextConfigs
{
    #region Using

    using System.Data.Entity.ModelConfiguration;

    using SecurePort.Entities.Models;

    #endregion

    public class OperadoresPuertoTypeConfig : EntityTypeConfiguration<OperadoresPuerto>
    {
        public OperadoresPuertoTypeConfig()
        {
            
            // Primary Key
            this.ToTable("SPMAE_Operadores_Puerto", "dbo")
                .HasKey(t => t.Id)
                .Property(t => t.Id)
                .HasColumnName("Id");
            
            // Properties
            this.Property(t => t.Id_puerto)
                .IsRequired()
                .HasColumnName("Id_puerto");

            this.Property(t => t.Nombre)
                .IsRequired()
                .HasColumnName("Nombre");

            this.Property(t => t.Es_Suplente)
                .IsRequired()
                .HasColumnName("Es_Suplente");

            this.Property(t => t.Telefono)
                .HasColumnName("Telefono");

            this.Property(t => t.Fax)
                .HasColumnName("Fax");

            this.Property(t => t.Email)
                .HasColumnName("Email");
            
            this.Property(t => t.ID_Ciudad)
                .HasColumnName("ID_Ciudad");

            this.Property(t => t.ID_Provincia)
               .HasColumnName("ID_Provincia");

            this.Property(t => t.Cod_Postal)
              .HasColumnName("Cod_Postal");

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
            
        }
    }
}

