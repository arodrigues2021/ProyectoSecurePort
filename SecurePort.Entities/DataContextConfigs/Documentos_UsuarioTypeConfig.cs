using System.Data.Entity.ModelConfiguration;
using SecurePort.Entities.Models;

namespace SecurePort.Entities.DataContextConfigs
{

    public class Docs_UsuarioTypeConfig : EntityTypeConfiguration<Docs_Usuario>
    {
        public Docs_UsuarioTypeConfig()
        {
            //Primary Key
            this.ToTable("SPMOV_Documentos_SP", "dbo")
                .HasKey(t => t.id)
                .Property(t => t.id)
                .HasColumnName("id");

            //Properties
            this.Property(t => t.id_servicio)
                .IsRequired()
                .HasColumnName("id_servicio");

            //Properties
            this.Property(t => t.id_tipo_doc)
                .IsRequired()
                .HasColumnName("id_tipo_doc");

            //Properties
            this.Property(t => t.id_tipo_serv)
               .IsRequired()
               .HasColumnName("id_tipo_serv");

            //Properties
            this.Property(t => t.documento)
               .IsRequired()
               .HasColumnName("documento");

            //Properties
            this.Property(t => t.descripcion)
                .IsRequired()
                .HasColumnName("descripcion");

            //Properties
            this.Property(t => t.fech_documento)
                 .IsRequired()
                 .HasColumnName("fech_documento");

            //Properties
            this.Property(t => t.id_usu_alta)
                .HasColumnName("id_usu_alta");

            //Properties
            this.Property(t => t.fech_alta)
                 .IsRequired()
                 .HasColumnName("fech_alta");
           
        }
    
    }

}
