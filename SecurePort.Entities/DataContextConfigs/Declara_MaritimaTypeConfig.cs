using SecurePort.Entities.Models;
using System.Data.Entity.ModelConfiguration;

namespace SecurePort.Entities.DataContextConfigs
{
    public class Declara_MaritimaTypeConfig : EntityTypeConfiguration<DeclaraMaritima>
    {
        public Declara_MaritimaTypeConfig()
        {

            //Primary Key
            this.ToTable("SPMOV_Declara_Maritima", "dbo")
                .HasKey(t => t.Id)
                .Property(t => t.Id)
                .HasColumnName("Id");

            //Properties
            this.Property(t => t.Id_IIPP)
                .IsRequired()
                .HasColumnName("Id_IIPP");

            this.Property(t => t.IMO)
                .HasColumnName("IMO");

            this.Property(t => t.Buque)
                .HasColumnName("Buque");

            this.Property(t => t.Id_Motivo)
                .IsRequired()
                .HasColumnName("Id_Motivo");

            this.Property(t => t.Fech_Expe)
                .IsRequired()
                .HasColumnName("Fech_Expe");

            this.Property(t => t.Fech_Ini_Validez)
                .HasColumnName("Fech_Ini_Validez");

            this.Property(t => t.Fech_Fin_Validez)
                .HasColumnName("Fech_Fin_Validez");
            
            this.Property(t => t.Responsable)
                .HasColumnName("Responsable");

            this.Property(t => t.Nivel_Buq)
                .HasColumnName("Nivel_Buq");

            this.Property(t => t.Nivel_IIPP)
                .HasColumnName("Nivel_IIPP");

            this.Property(t => t.Medidas)
                .HasColumnName("Medidas");

            this.Property(t => t.Observaciones)
                .HasColumnName("Observaciones");

            this.Property(t => t.ID_Usu_alta)
                .HasColumnName("ID_Usu_alta");

            this.Property(t => t.Fech_Alta)
                .IsRequired()
                .HasColumnName("Fech_Alta");
            
        }
    }
}
