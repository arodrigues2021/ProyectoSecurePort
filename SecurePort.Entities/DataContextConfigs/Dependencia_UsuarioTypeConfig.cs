using System.Data.Entity.ModelConfiguration;
using SecurePort.Entities.Models;

namespace SecurePort.Entities.DataContextConfigs
{
  
    public class  Dependencia_UsuarioTypeConfig : EntityTypeConfiguration<Depen_Usuario>
    {
        public Dependencia_UsuarioTypeConfig()
        {
            // Primary Key
            this.ToTable("SPCON_Depen_Usuario", "dbo")
                .HasKey(t => t.id)
                .Property(t => t.id)
                .HasColumnName("id");

            this.Property(t => t.id_Usuario)
                .IsRequired()
                .HasColumnName("id_Usuario");

            this.Property(t => t.id_Dependencia)
               .IsRequired()
               .HasColumnName("id_Dependencia");

            this.Property(t => t.id_usu_alta)
                .HasColumnName("id_usu_alta");

            this.Property(t => t.fech_alta)
                .IsRequired()
                .HasColumnName("fech_alta");
           
        }
    
    }

}
