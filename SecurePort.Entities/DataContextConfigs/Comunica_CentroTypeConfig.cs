using System.Data.Entity.ModelConfiguration;
using SecurePort.Entities.Models;

namespace SecurePort.Entities.DataContextConfigs
{
  
    public class  Comunica_CentroTypeConfig : EntityTypeConfiguration<Comunica_Centro>
    {
        public Comunica_CentroTypeConfig()
        {
            // Primary Key
            this.ToTable("SPMOV_Comunica_Centro24h", "dbo")
                 .HasKey(t => t.id)
                 .Property(t => t.id)
                 .HasColumnName("id");

            // Properties
            this.Property(t => t.Id_Centro24h)
                .IsRequired()
                .HasColumnName("Id_Centro24h");

            this.Property(t => t.Tipo_Canal)
                .IsRequired()
                .HasColumnName("Tipo_Canal");

            this.Property(t => t.Dato)
                .IsRequired()
                .HasColumnName("Dato");

            this.Property(t => t.id_usu_alta)
               .HasColumnName("id_usu_alta");

            this.Property(t => t.fech_alta)
                .IsRequired()
                .HasColumnName("fech_alta");

            this.Property(t => t.Nota)
               .HasColumnName("Nota");
            
        }
    
    }

}
