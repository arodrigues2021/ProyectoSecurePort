using System.Data.Entity.ModelConfiguration;
using SecurePort.Entities.Models;

namespace SecurePort.Entities.DataContextConfigs
{

    public class PermisosTypeConfig : EntityTypeConfiguration<Acciones>
    {

        public PermisosTypeConfig()
        {
            // Primary Key
            this.ToTable("SPCON_Acciones", "dbo")
                .HasKey(t => t.Id)
                .Property(t => t.Id)
                .HasColumnName("Id");

            this.Property(t => t.Nombre)
               .IsRequired()
               .HasColumnName("Nombre");

            this.Property(t => t.descripcion)
               .HasColumnName("Descripcion");

            this.Property(t => t.id_grupo_accion)
               .IsRequired()
               .HasColumnName("ID_Grupo_Accion");

            this.Property(t => t.orden)
               .HasColumnName("orden");
        }

    }

}
