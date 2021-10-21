using SecurePort.Entities.Models;
using System.Data.Entity.ModelConfiguration;

namespace SecurePort.Entities.DataContextConfigs
{
    public class Versiones_EvaluacionTypeConfig : EntityTypeConfiguration<Versiones_Evaluacion>
    {
        public Versiones_EvaluacionTypeConfig()
        {

            //Primary Key
            this.ToTable("SPEVA_Versiones_Evaluacion", "dbo")
                .HasKey(t => t.Id)
                .Property(t => t.Id)
                .HasColumnName("Id");

            //Properties
             this.Property(t => t.Id_Evaluacion)
                .IsRequired()
                .HasColumnName("Id_Evaluacion");

             //Properties
            this.Property(t => t.Version)
                .IsRequired()
                .HasColumnName("Version");

            //Properties
            this.Property(t => t.Estado)
                .IsRequired()
                .HasColumnName("Estado");

            //Properties
            this.Property(t => t.Responsable)
                .IsRequired()
                .HasColumnName("Responsable");

            //Properties
            this.Property(t => t.Cargo)
                .IsRequired()
                .HasColumnName("Cargo");

            //Properties
            this.Property(t => t.Fech_Cierre)                
                .HasColumnName("Fech_Cierre");

            //Properties
            this.Property(t => t.Fech_Procesada)                
                .HasColumnName("Fech_Procesada");

            //Properties
            this.Property(t => t.Fech_Conforme)                
                .HasColumnName("Fech_Conforme");

            //Properties
            this.Property(t => t.Fech_Tramitada)                
                .HasColumnName("Fech_Tramitada");

            //Properties
            this.Property(t => t.Fech_Aprobada)
                .HasColumnName("Fech_Aprobada");

            //Properties
            this.Property(t => t.Fech_Rechazada)
                .HasColumnName("Fech_Rechazada");

            //Properties
            this.Property(t => t.Fech_NoConforme)
                .HasColumnName("Fech_NoConforme");

            //Properties
            this.Property(t => t.Fech_NoTramitada)
                .HasColumnName("Fech_NoTramitada");

            //Properties
            this.Property(t => t.Fech_Zonificacion)
                .HasColumnName("Fech_Zonificacion");

            //Properties
            this.Property(t => t.Fech_Info_Eval)
                .HasColumnName("Fech_Info_Eval");

            //Properties
            this.Property(t => t.Fech_Info_Vune)
                .HasColumnName("Fech_Info_Vune");

            //Properties
            this.Property(t => t.Observaciones)
                .HasColumnName("Observaciones");

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
