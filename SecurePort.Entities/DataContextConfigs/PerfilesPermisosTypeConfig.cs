using System.Data.Entity.ModelConfiguration;
using SecurePort.Entities.Models;

namespace SecurePort.Entities.DataContextConfigs
{

    public class PerfilesPermisosTypeConfig : EntityTypeConfiguration<AccionesPerfil>
    {
        public PerfilesPermisosTypeConfig()
        {
            // Primary Key
            this.ToTable("SPCON_Acciones_Perfil", "dbo")
                .HasKey(t => t.id)
                .Property(t => t.id)
                .HasColumnName("Id");

            this.Property(t => t.Id_perfil)
               .IsRequired()
               .HasColumnName("Id_perfil");

            this.Property(t => t.Id_permiso)
               .IsRequired()
               .HasColumnName("Id_permiso");

            this.Property(t => t.id_usu_alta)
              .HasColumnName("id_usu_alta");
            
            this.Property(t => t.fech_alta)
               .IsRequired()
               .HasColumnName("fech_alta");
            
        }
    
    }

}
