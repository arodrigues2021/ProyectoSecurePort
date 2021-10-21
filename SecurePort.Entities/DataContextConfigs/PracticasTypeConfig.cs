using SecurePort.Entities.Models;
using System.Data.Entity.ModelConfiguration;

namespace SecurePort.Entities.DataContextConfigs
{
    public class PracticasTypeConfig : EntityTypeConfiguration<Practicas>
    {
        public PracticasTypeConfig()
        {

            //Primary Key
            this.ToTable("SPMOV_Practicas", "dbo")
                .HasKey(t => t.Id)
                .Property(t => t.Id)
                .HasColumnName("Id");

            //Properties
            this.Property(t => t.Id_Puerto)
                .IsRequired()
                .HasColumnName("Id_Puerto");

            this.Property(t => t.Tipo)
                .IsRequired()
                .HasColumnName("Tipo");

            this.Property(t => t.Estado)
                .IsRequired()
                .HasColumnName("Estado ");

            this.Property(t => t.Descripcion)
                .HasColumnName("Descripcion");

            this.Property(t => t.Ini_Programa)
                .HasColumnName("Ini_Programa");

            this.Property(t => t.Fin_Programa)
                .HasColumnName("Fin_Programa");

            this.Property(t => t.Ini_Planifica)
               .HasColumnName("Ini_Planifica");

            this.Property(t => t.Fin_Planifica)
                .HasColumnName("Fin_Planifica");

            this.Property(t => t.Ini_Real)
               .HasColumnName("Ini_Real");

            this.Property(t => t.Fin_Real)
                .HasColumnName("Fin_Real");

            this.Property(t => t.Responsable)
                .HasColumnName("Responsable");

            this.Property(t => t.Cuerpos)
               .HasColumnName("Cuerpos");
            
            this.Property(t => t.Valoracion)
              .HasColumnName("Valoracion");

            this.Property(t => t.Ratifico)
              .HasColumnName("Ratifico");

            this.Property(t => t.Conclusiones)
              .HasColumnName("Conclusiones");

            this.Property(t => t.Propuesta)
              .HasColumnName("Propuesta");

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
