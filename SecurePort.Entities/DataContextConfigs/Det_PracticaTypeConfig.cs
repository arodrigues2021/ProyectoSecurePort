using System.Data.Entity.ModelConfiguration;
using SecurePort.Entities.Models;

namespace SecurePort.Entities.DataContextConfigs
{

    public class Det_PracticaTypeConfig : EntityTypeConfiguration<Det_Practica>
    {

        public Det_PracticaTypeConfig()
        {
            // Primary Key
            this.ToTable("SPMOV_Det_Practica", "dbo")
                .HasKey(t => t.Id)
                .Property(t => t.Id)
                .HasColumnName("Id");

            this.Property(t => t.Id_Practica)
               .IsRequired()
               .HasColumnName("Id_Practica");

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
