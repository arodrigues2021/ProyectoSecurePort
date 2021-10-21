using SecurePort.Entities.Models;
using System.Data.Entity.ModelConfiguration;

namespace SecurePort.Entities.DataContextConfigs
{
    public class NivelesTypeConfig : EntityTypeConfiguration<Niveles>
    {
        public NivelesTypeConfig()
        {

            //Primary Key
            this.ToTable("SPMOV_Niveles", "dbo")
                .HasKey(t => t.Id)
                .Property(t => t.Id)
                .HasColumnName("Id");

            this.Property(t => t.Id_Puerto)
                .IsRequired()
                .HasColumnName("Id_Puerto");
           
            this.Property(t => t.Descripcion)
                .IsRequired()
                .HasColumnName("Descripcion");
            
            this.Property(t => t.Fecha)
                .IsRequired()
                .HasColumnName("Fecha");

            this.Property(t => t.Emisor)
                .HasColumnName("Emisor");

            this.Property(t => t.Receptor)
                .HasColumnName("Receptor");

            this.Property(t => t.Nivel)
                .IsRequired()
                .HasColumnName("Nivel");

            this.Property(t => t.Emisor_Cancela)
                .HasColumnName("Emisor_Cancela");

            this.Property(t => t.Duracion)                
                .HasColumnName("Duracion");

            this.Property(t => t.Fecha_Cancela)
                .HasColumnName("Fecha_Cancela");

            this.Property(t => t.Nivel_Max)
                .HasColumnName("Nivel_Max");

            this.Property(t => t.Relevante)
                .HasColumnName("Relevante");

            this.Property(t => t.Recomendacion)
                .HasColumnName("Recomendacion");
            
            this.Property(t => t.Observaciones)
                .HasColumnName("Observaciones");
            
            this.Property(t => t.ID_Usu_Alta)
                .HasColumnName("ID_Usu_Alta");

            this.Property(t => t.Fech_Alta)
                .IsRequired()
                .HasColumnName("Fech_Alta");
        }
    }
}
