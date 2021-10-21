using SecurePort.Entities.Models;
using System.Data.Entity.ModelConfiguration;

namespace SecurePort.Entities.DataContextConfigs
{
    public class OrganismosTypeConfig : EntityTypeConfiguration<Organismos>
    {
        public OrganismosTypeConfig()
        {

            // Primary Key
            this.ToTable("SPMAE_Organismos", "dbo")
                .HasKey(t => t.Id)
                .Property(t => t.Id)
                .HasColumnName("Id");

            // Properties
            this.Property(t => t.Nombre)
                .IsRequired()
                .HasColumnName("Nombre");
            
            // Properties
            this.Property(t => t.Tipo)
                .IsRequired()
                .HasColumnName("Tipo");
            
            // Properties
            this.Property(t => t.ID_Usu_Alta)
                .HasColumnName("ID_Usu_Alta");

            // Properties
            this.Property(t => t.fech_alta)
                .IsRequired()
                .HasColumnName("fech_alta");

            // Properties
            this.Property(t => t.es_activo)
                .IsRequired()
                .HasColumnName("es_activo");

            // Properties
            this.Property(t => t.fech_activo)
                .IsRequired()
                .HasColumnName("fech_activo");

            // Properties
            this.Property(t => t.Observaciones)
                .HasColumnName("Observaciones");
   
            // Properties
            this.Property(t => t.ID_Ciudad)
                .IsRequired()
                .HasColumnName("ID_Ciudad");

            // Properties
            this.Property(t => t.Cod_Postal)
                .IsRequired()
                .HasColumnName("Cod_Postal");

            // Properties
            this.Property(t => t.ID_Provincia)
                .IsRequired()
                .HasColumnName("ID_Provincia");

            // Properties
            this.Property(t => t.Direccion)
                .HasColumnName("Direccion");

         }
    }
}