using System.Data.Entity.ModelConfiguration;
using SecurePort.Entities.Models;

namespace SecurePort.Entities.DataContextConfigs
{
  
    public class  UsuariosTypeConfig : EntityTypeConfiguration<Usuarios>
    {
        public UsuariosTypeConfig()
        {
            // Primary Key
            this.ToTable("SPCON_Usuarios", "dbo")
                .HasKey(t => t.id)
                .Property(t => t.id)
                .HasColumnName("id");

            // Properties
            this.Property(t => t.login)
                .IsRequired()
                .HasColumnName("login");

            this.Property(t => t.password)
                .IsRequired()
                .HasColumnName("password");

            this.Property(t => t.nombre)
               .IsRequired()
               .HasColumnName("nombre");

            this.Property(t => t.apellidos)
               .IsRequired()
               .HasColumnName("apellidos");

            this.Property(t => t.email)
               .HasColumnName("email");

            this.Property(t => t.es_opip)
               .HasColumnName("es_opip");

            this.Property(t => t.fech_exp_opip)
                .HasColumnName("fech_exp_opip");

            this.Property(t => t.es_opp)
               .HasColumnName("es_opp");

            this.Property(t => t.categoria)
               .HasColumnName("categoria");

            this.Property(t => t.id_grupo)
               .HasColumnName("id_grupo");

            this.Property(t => t.id_usu_alta)
               .HasColumnName("id_usu_alta");

            this.Property(t => t.fech_alta)
               .HasColumnName("fech_alta");

            this.Property(t => t.es_activo)
                .IsRequired()
               .HasColumnName("es_activo");

            this.Property(t => t.fech_activo)
               .HasColumnName("fech_activo");

            this.Property(t => t.observaciones)
               .HasColumnName("observaciones");

            this.Property(t => t.empresa)
              .HasColumnName("empresa");

            this.Property(t => t.Fech_Password)
               .IsRequired()
              .HasColumnName("Fech_Password");
            
        }
    
    }

}
