using SecurePort.Entities.Models;
using System.Data.Entity.ModelConfiguration;

namespace SecurePort.Entities.DataContextConfigs
{
    public class GisisTypeConfig : EntityTypeConfiguration<RegistroGisis>
    {
        public GisisTypeConfig()
        {

            //Primary Key
            this.ToTable("SPMOV_Registro_GISIS", "dbo")
                .HasKey(t => t.Id)
                .Property(t => t.Id)
                .HasColumnName("Id");

            //Properties
            this.Property(t => t.Id_Puerto)
                .IsRequired()
                .HasColumnName("Id_puerto");

            //Properties
            this.Property(t => t.Id_IIPP)
                .HasColumnName("Id_IIPP");
            
            //Properties
            this.Property(t => t.Tipo_Registro)
                 .IsRequired()
                .HasColumnName("Tipo_Registro");
            
            //Properties
            this.Property(t => t.Fech_Registro)
                .HasColumnName("Fech_Registro");

            //Properties
            this.Property(t => t.Motivo)
                .HasColumnName("Motivo");

            //Properties
            this.Property(t => t.ID_Usu_alta)
                .HasColumnName("ID_Usu_alta");

            //Properties
            this.Property(t => t.Fech_Alta)
                .IsRequired()
                .HasColumnName("Fech_Alta");

        
        }
    }
}
