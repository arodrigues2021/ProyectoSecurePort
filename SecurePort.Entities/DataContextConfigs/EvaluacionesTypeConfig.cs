using SecurePort.Entities.Models;
using System.Data.Entity.ModelConfiguration;

namespace SecurePort.Entities.DataContextConfigs
{
    public class EvaluacionesTypeConfig : EntityTypeConfiguration<Evaluaciones>
    {
        public EvaluacionesTypeConfig()
        {

            //Primary Key
            this.ToTable("SPEVA_Evaluaciones", "dbo")
                .HasKey(t => t.Id)
                .Property(t => t.Id)
                .HasColumnName("Id");

            //Properties
            this.Property(t => t.Nombre)
                .IsRequired()
                .HasColumnName("Nombre");

            //Properties
            this.Property(t => t.Id_Puerto)
                .IsRequired()
                .HasColumnName("Id_puerto");

            //Properties
            this.Property(t => t.Id_IIPP)
                .HasColumnName("Id_IIPP");
            
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
