using SecurePort.Entities.Models;
using System.Data.Entity.ModelConfiguration;

namespace SecurePort.Entities.DataContextConfigs
{
    public class Historico_EvaluacionTypeConfig : EntityTypeConfiguration<Historico_Evaluacion>
    {
        public Historico_EvaluacionTypeConfig()
        {

            //Primary Key
            this.ToTable("SPEVA_Historico_Evaluacion", "dbo")
                .HasKey(t => t.Id)
                .Property(t => t.Id)
                .HasColumnName("Id");

            //Properties
            this.Property(t => t.Id_Evaluacion)
                .IsRequired()
                .HasColumnName("Id_Evaluacion");

            //Properties
            this.Property(t => t.Id_Version_Eval)
                .IsRequired()
                .HasColumnName("Id_Version_Eval");

            //Properties
            this.Property(t => t.Accion)
                .HasColumnName("Accion");

            //Properties
            this.Property(t => t.Observaciones)
                .IsRequired()
                .HasColumnName("Observaciones");
            
            //Properties
            this.Property(t => t.ID_Usu_Accion)
                .HasColumnName("ID_Usu_Accion");

            //Properties
            this.Property(t => t.Fech_Accion)                
                .HasColumnName("Fech_Accion");

        
        }
    }
}
