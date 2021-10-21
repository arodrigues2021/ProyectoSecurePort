using System.Data.Entity.ModelConfiguration;
using SecurePort.Entities.Models;

namespace SecurePort.Entities.DataContextConfigs
{

    public class Det_NivelTypeConfig : EntityTypeConfiguration<Det_Nivel>
    {

        public Det_NivelTypeConfig()
        {
            // Primary Key
            this.ToTable("SPMOV_Det_Nivel", "dbo")
                .HasKey(t => t.Id)
                .Property(t => t.Id)
                .HasColumnName("Id");

            this.Property(t => t.Id_Nivel)
               .IsRequired()
               .HasColumnName("Id_Nivel");

            this.Property(t => t.Id_IIPP)                
               .HasColumnName("Id_IIPP");

            this.Property(t => t.id_usu_alta)
              .HasColumnName("id_usu_alta");

            this.Property(t => t.fech_alta)
                .IsRequired()
                .HasColumnName("fech_alta");

        }

    }

}
