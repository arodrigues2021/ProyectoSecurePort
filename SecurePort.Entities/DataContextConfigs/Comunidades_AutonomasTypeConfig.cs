using SecurePort.Entities.Models;
using System.Data.Entity.ModelConfiguration;

namespace SecurePort.Entities.DataContextConfigs
{
    public class Comunidades_AutonomasTypeConfig : EntityTypeConfiguration<Comunidades_Autonomas>
    {
        public Comunidades_AutonomasTypeConfig()
        {

            //Primary Key
            this.ToTable("SPMAE_Comunidades_Autonomas", "dbo")
                .HasKey(t => t.Id)
                .Property(t => t.Id)
                .HasColumnName("Id");
          
            //Properties
            this.Property(t => t.Nombre)
                .IsRequired()
                .HasColumnName("Nombre");
            
            //Properties
            this.Property(t => t.ID_Pais)
                .IsRequired()
                .HasColumnName("ID_Pais");

            //Properties
            this.Property(t => t.ID_Usu_Alta)
                .HasColumnName("ID_Usu_Alta");

            //Properties
            this.Property(t => t.fech_alta)
                .IsRequired()
                .HasColumnName("fech_alta");

             //Properties
            this.Property(t => t.es_activo)
                .HasColumnName("es_activo");

            //Properties
            this.Property(t => t.fech_activo)
                .HasColumnName("fech_activo");
        
        }
    }
}
