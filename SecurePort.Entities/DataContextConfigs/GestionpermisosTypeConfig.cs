using System.Data.Entity.ModelConfiguration;
using SecurePort.Entities.Models;

namespace SecurePort.Entities.DataContextConfigs
{
   
    public class  GestionpermisosTypeConfig : EntityTypeConfiguration<Gestionpermisos>
    {
        public GestionpermisosTypeConfig()
        {
            // Primary Key
            this.ToTable("SPCON_Grupos_Acciones", "dbo")
                .HasKey(t => t.Id)
                .Property(t => t.Id)
                .HasColumnName("Id");
            
            this.Property(t => t.Nombre)
               .IsRequired()
               .HasColumnName("Nombre");

            this.Property(t => t.Descripcion)
              .HasColumnName("Descripcion");
        }
    
    }

}
