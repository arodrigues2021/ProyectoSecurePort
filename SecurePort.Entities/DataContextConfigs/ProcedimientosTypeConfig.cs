using System.Data.Entity.ModelConfiguration;
using SecurePort.Entities.Models;

namespace SecurePort.Entities.DataContextConfigs
{
  
    public class  ProcedimientosTypeConfig : EntityTypeConfiguration<Procedimientos>
    {
        public ProcedimientosTypeConfig()
        {
            // Primary Key
            this.ToTable("SPMOV_Procedimientos", "dbo")
                 .HasKey(t => t.Id)
                 .Property(t => t.Id)
                 .HasColumnName("id");

            // Properties
            this.Property(t => t.Id_Organismo)
                .HasColumnName("Id_Organismo");

            this.Property(t => t.Ind_AP)                
                .HasColumnName("Ind_AP");

            this.Property(t => t.Titulo)
                .IsRequired()
                .HasColumnName("Titulo");

            this.Property(t => t.Fecha)
                .IsRequired()
                .HasColumnName("fecha");

            this.Property(t => t.Descripcion)
                .IsRequired()
                .HasColumnName("Descripcion");

            this.Property(t => t.Observaciones)
                .HasColumnName("Observaciones");
            
            this.Property(t => t.ID_Usu_alta)
               .HasColumnName("id_usu_alta");

            this.Property(t => t.Fech_Alta)
                .IsRequired()
                .HasColumnName("fech_alta");

            
        }
    
    }

}
