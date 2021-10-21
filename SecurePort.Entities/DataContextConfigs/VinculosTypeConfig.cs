using SecurePort.Entities.Models;
using System.Data.Entity.ModelConfiguration;

namespace SecurePort.Entities.DataContextConfigs
{
    public class VinculosTypeConfig : EntityTypeConfiguration<Vinculos>
    {
        public VinculosTypeConfig()
        {

            //Primary Key
            this.ToTable("SPMOV_Vinculos_SP", "dbo")
                .HasKey(t => t.Id)
                .Property(t => t.Id)
                .HasColumnName("Id");
             
            //Properties
            this.Property(t => t.Tipo)
                .IsRequired()
                .HasColumnName("Tipo");

            //Properties
            this.Property(t => t.Id_Categoria)
                .HasColumnName("ID_Categoría");
            
            //Properties
            this.Property(t => t.Nombre)
                 .IsRequired()
                .HasColumnName("Nombre");
            
            //Properties
            this.Property(t => t.Descripcion)
                .IsRequired()
                .HasColumnName("Descripcion");

            //Properties
            this.Property(t => t.Orden)
                .HasColumnName("Orden");

            
        
        }
    }
}
