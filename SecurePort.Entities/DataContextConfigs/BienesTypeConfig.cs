using SecurePort.Entities.Models;
using System.Data.Entity.ModelConfiguration;

namespace SecurePort.Entities.DataContextConfigs
{
    public class BienesTypeConfig : EntityTypeConfiguration<Bienes>
    {
        public BienesTypeConfig()
        {

            //Primary Key
            this.ToTable("SPMAE_Bienes", "dbo")
                .HasKey(t => t.Id)
                .Property(t => t.Id)
                .HasColumnName("Id");

            //Properties
            this.Property(t => t.Descripcion)
                .IsRequired()
                .HasColumnName("Descripcion");

            //Properties
            this.Property(t => t.id_Tipo_IIPP)
                .HasColumnName("id_Tipo_IIPP");
            
            //Properties
            this.Property(t => t.id_Bien_Padre)
                .HasColumnName("id_Bien_Padre");
            
            //Properties
            this.Property(t => t.ID_Usu_alta)
                .HasColumnName("ID_Usu_alta");

            //Properties
            this.Property(t => t.Fech_Alta)
                .IsRequired()
                .HasColumnName("Fech_Alta");

            //Properties
            this.Property(t => t.es_activo)
                .IsRequired()
                .HasColumnName("es_activo");

            //Properties
            this.Property(t => t.fech_activo)
                .IsRequired()
                .HasColumnName("fech_activo");
        
        }
    }
}
