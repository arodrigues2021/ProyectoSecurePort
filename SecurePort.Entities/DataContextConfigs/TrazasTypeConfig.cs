using System.Data.Entity.ModelConfiguration;
using SecurePort.Entities.Models;

namespace SecurePort.Entities.DataContextConfigs
{
  
    public class  TrazasTypeConfig : EntityTypeConfiguration<Trazas>
    {
        public TrazasTypeConfig()
        {
            // Primary Key
            this.ToTable("SPMOV_Trazas_SP", "dbo")
                .HasKey(t => t.id)
                .Property(t => t.id)
                .HasColumnName("id");

            // Properties
            this.Property(t => t.id_accion)
                .HasColumnName("id_accion");
            
            // Properties
            this.Property(t => t.id_usu_alta)
                .HasColumnName("id_usu_alta");

            this.Property(t => t.fech_alta)
               .HasColumnName("fech_alta");

            this.Property(t => t.traza)
               .HasColumnName("traza");
            
        }
    
    }
}

