namespace SecurePort.Entities.DataContextConfigs
{
    #region Using

    using System.Data.Entity.ModelConfiguration;

    using SecurePort.Entities.Models;

    #endregion

    public class MOV_Detalle_InstalacionTypeConfig : EntityTypeConfiguration<MOV_Detalle_Instalacion>
    {
        public MOV_Detalle_InstalacionTypeConfig()
        {
            
            // Primary Key
            this.ToTable("SPMOV_Detalle_Instalacion", "dbo")
                .HasKey(t => t.Id)
                .Property(t => t.Id)
                .HasColumnName("Id");

            // Properties
            this.Property(t => t.ID_Amenaza)
                .HasColumnName("ID_Amenaza");

            this.Property(t => t.ID_Bien)
               .IsRequired()
               .HasColumnName("ID_Bien");
            
            this.Property(t => t.ID_Instalacion)              
              .HasColumnName("ID_Instalacion");    

            this.Property(t => t.ID_Usu_Alta)
                .HasColumnName("ID_Usu_Alta");
            
            this.Property(t => t.Fech_Alta)
                .IsRequired()
                .HasColumnName("Fech_Alta");

        }
    }
}

