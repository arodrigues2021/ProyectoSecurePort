using System.Data.Entity.ModelConfiguration;
using SecurePort.Entities.Models;

namespace SecurePort.Entities.DataContextConfigs
{
  
    public class  ProvinciasTypeConfig : EntityTypeConfiguration<Provincias>
    {
        public ProvinciasTypeConfig()
        {
            // Primary Key
            this.ToTable("SPMAE_Provincias", "dbo")
                .HasKey(t => t.id)
                .Property(t => t.id)
                .HasColumnName("id");

            // Properties
            this.Property(t => t.codigo)
                .IsRequired()
                .HasColumnName("codigo");

            this.Property(t => t.nombre)
                .IsRequired()
                .HasColumnName("nombre");
           
            this.Property(t => t.id_usu_alta)
               .HasColumnName("id_usu_alta");

            this.Property(t => t.fech_alta)
                .IsRequired()
               .HasColumnName("fech_alta");

            this.Property(t => t.es_activo)
                .IsRequired()
               .HasColumnName("es_activo");

            this.Property(t => t.fech_activo)
                .IsRequired()
               .HasColumnName("fech_activo");

            this.Property(t => t.ID_ComAut)
                .IsRequired()
               .HasColumnName("ID_ComAut");
            
        }
    
    }

}
