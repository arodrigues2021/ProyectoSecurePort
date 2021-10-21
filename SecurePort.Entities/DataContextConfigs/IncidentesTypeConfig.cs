using SecurePort.Entities.Models;
using System.Data.Entity.ModelConfiguration;

namespace SecurePort.Entities.DataContextConfigs
{
    public class IncidentesTypeConfig : EntityTypeConfiguration<Incidentes>
    {
        public IncidentesTypeConfig()
        {

            //Primary Key
            this.ToTable("SPMOV_Incidentes", "dbo")
                .HasKey(t => t.Id)
                .Property(t => t.Id)
                .HasColumnName("Id");
            
            //Properties
            this.Property(t => t.Id_Puerto)
                .IsRequired()
                .HasColumnName("Id_Puerto");

            this.Property(t => t.Descripcion)
                .IsRequired()
                .HasColumnName("Descripcion");

            this.Property(t => t.Tipo)
               .IsRequired()
               .HasColumnName("Tipo");

            this.Property(t => t.Fecha)
               .IsRequired()
               .HasColumnName("Fecha");

            this.Property(t => t.Observaciones)
               .HasColumnName("Observaciones");

            this.Property(t => t.ID_Usu_alta)
                .HasColumnName("ID_Usu_alta");

            //Properties
            this.Property(t => t.Fech_Alta)
                .IsRequired()
                .HasColumnName("Fech_Alta");

        
        }
    }
}
