using SecurePort.Entities.Models;
using System.Data.Entity.ModelConfiguration;

namespace SecurePort.Entities.DataContextConfigs
{
    public class FormacionesTypeConfig : EntityTypeConfiguration<Formaciones>
    {
        public FormacionesTypeConfig()
        {

            //Primary Key
            this.ToTable("SPMOV_Formaciones", "dbo")
                .HasKey(t => t.Id)
                .Property(t => t.Id)
                .HasColumnName("Id");

            this.Property(t => t.Id_Organismo)
                .IsRequired()
                .HasColumnName("Id_Organismo");

            this.Property(t => t.Id_Puerto)
                .IsRequired()
                .HasColumnName("Id_Puerto");

            this.Property(t => t.Id_IIPP)
                .HasColumnName("Id_IIPP");
            
            this.Property(t => t.Titulo)
                .HasColumnName("Titulo");
            
            this.Property(t => t.Inicio)
                .IsRequired()
                .HasColumnName("Inicio");

            this.Property(t => t.Fin)
                .IsRequired()
                .HasColumnName("Fin");
            
            this.Property(t => t.Duracion)
                .IsRequired()
                .HasColumnName("Duracion");

            this.Property(t => t.Lugar)
                .IsRequired()
                .HasColumnName("Lugar");

            this.Property(t => t.Entidad)
                .IsRequired()
                .HasColumnName("Entidad");
            
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
