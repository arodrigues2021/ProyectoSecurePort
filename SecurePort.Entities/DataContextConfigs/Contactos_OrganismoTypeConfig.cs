using System.Data.Entity.ModelConfiguration;
using SecurePort.Entities.Models;

namespace SecurePort.Entities.DataContextConfigs
{
  
    public class  Contactos_OrganismoTypeConfig : EntityTypeConfiguration<Contactos_Organismo>
    {
        public Contactos_OrganismoTypeConfig()
        {
            // Primary Key
            this.ToTable("SPMAE_Contactos_Organismo", "dbo")
                .HasKey(t => t.Id)
                .Property(t => t.Id)
                .HasColumnName("Id");

            // Properties
            this.Property(t => t.Id_Organismo)
                .IsRequired()
                .HasColumnName("Id_Organismo");

            this.Property(t => t.Nombre)
                .IsRequired()
                .HasColumnName("Nombre");

            this.Property(t => t.Es_Responsable)
                .IsRequired()
                .HasColumnName("Es_Responsable");

            this.Property(t => t.Telefono)
               .HasColumnName("Telefono");

            this.Property(t => t.Fax)
               .HasColumnName("Fax");

            this.Property(t => t.Email)
               .HasColumnName("Email");
            
            this.Property(t => t.ID_Usu_Alta)
               .HasColumnName("id_usu_alta");

            this.Property(t => t.fech_alta)
                .IsRequired()
                .HasColumnName("fech_alta");

            this.Property(t => t.es_activo)
                .IsRequired()
                .HasColumnName("es_activo");

            this.Property(t => t.fech_activo)
                .IsRequired()
                .HasColumnName("fech_activo");

            this.Property(t => t.Observaciones)
                .HasColumnName("Observaciones");

            this.Property(t => t.Cargo)
                .HasColumnName("Cargo");

        }
    
    }

}
