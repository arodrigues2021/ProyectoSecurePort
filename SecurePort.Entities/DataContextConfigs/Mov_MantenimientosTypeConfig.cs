using System.Data.Entity.ModelConfiguration;
using SecurePort.Entities.Models;

namespace SecurePort.Entities.DataContextConfigs
{

    public class Mov_MantenimientosTypeConfig : EntityTypeConfiguration<Mantenimientos>
    {
        public Mov_MantenimientosTypeConfig()
        {
            // Primary Key
            this.ToTable("SPMOV_Mantenimientos", "dbo")
                .HasKey(t => t.Id)
                .Property(t => t.Id)
                .HasColumnName("Id");

            // Properties
            this.Property(t => t.id_Puerto)
                .IsRequired()
                .HasColumnName("id_Puerto");

            this.Property(t => t.id_IIPP)
                .HasColumnName("id_IIPP");
        
            this.Property(t => t.Descripcion)
                .IsRequired()
                .HasColumnName("Descripcion");

            this.Property(t => t.fecha)
                .IsRequired()
                .HasColumnName("fecha");

            this.Property(t => t.Equipo)
                .IsRequired()
                .HasColumnName("Equipo");
            
            this.Property(t => t.Realizador)
                .HasColumnName("Realizador");

            this.Property(t => t.Validador)
                .HasColumnName("Validador");
            
            this.Property(t => t.fecha_Revision)
                .HasColumnName("fecha_Revision");

            this.Property(t => t.Observaciones)
                .HasColumnName("Observaciones");

            this.Property(t => t.id_Usu_Alta)
                .HasColumnName("id_Usu_Alta");

            this.Property(t => t.Fech_alta)
                .IsRequired()
                .HasColumnName("Fech_alta");
     
        }
    }
}
