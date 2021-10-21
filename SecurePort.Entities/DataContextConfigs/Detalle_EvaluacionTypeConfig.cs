using SecurePort.Entities.Models;
using System.Data.Entity.ModelConfiguration;

namespace SecurePort.Entities.DataContextConfigs
{
    public class Detalle_EvaluacionTypeConfig : EntityTypeConfiguration<Detalle_Evaluacion>
    {
        public Detalle_EvaluacionTypeConfig()
        {

            //Primary Key
            this.ToTable("SPEVA_Detalle_Evaluacion", "dbo")
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
            this.Property(t => t.Id_Instalacion)
                .HasColumnName("Id_Instalacion");

            //Properties
            this.Property(t => t.Id_Bien)
                .IsRequired()
                .HasColumnName("Id_Bien");

             //Properties
            this.Property(t => t.Id_Amenaza)
                .IsRequired()
                .HasColumnName("Id_Amenaza");
        
              //Properties
            this.Property(t => t.IND_IAS_ITP)
                .HasColumnName("IND_IAS_ITP");

              //Properties
            this.Property(t => t.NP1_IAI)
                .HasColumnName("NP1_IAI");

              //Properties
            this.Property(t => t.NP1_ISD)
                .HasColumnName("NP1_ISD");

             //Properties
            this.Property(t => t.NP1_IIO)
                .HasColumnName("NP1_IIO");

             //Properties
            this.Property(t => t.NP1_IDV)
                .HasColumnName("NP1_IDV");

             //Properties
            this.Property(t => t.NP1_IDE_D)
                .HasColumnName("NP1_IDE_D");

             //Properties
            this.Property(t => t.NP1_IDE_I)
                .HasColumnName("NP1_IDE_I");

             //Properties
            this.Property(t => t.NP1_IRD)
                .HasColumnName("NP1_IRD");

             //Properties
            this.Property(t => t.NP1_IRR)
                .HasColumnName("NP1_IRR");

             //Properties
            this.Property(t => t.NP1_ISA_MA)
                .HasColumnName("NP1_ISA_MA");

             //Properties
            this.Property(t => t.NP1_ISA_PA)
                .HasColumnName("NP1_ISA_PA");

             //Properties
            this.Property(t => t.NP1_ISA_AS)
                .HasColumnName("NP1_ISA_AS");

             //Properties
            this.Property(t => t.NP1_RESULTADO)
                .HasColumnName("NP1_RESULTADO");

             //Properties
            this.Property(t => t.NP1_ID)
                .HasColumnName("NP1_ID");
             //Properties
            this.Property(t => t.NP1_IC)
                .HasColumnName("NP1_IC");

             //Properties
            this.Property(t => t.NP1_IV)
                .HasColumnName("NP1_IV");

             //Properties
            this.Property(t => t.NP2_IAI)
                .HasColumnName("NP2_IAI");

             //Properties
            this.Property(t => t.NP2_ISD)
                .HasColumnName("NP2_ISD");

             //Properties
            this.Property(t => t.NP2_IIO)
                .HasColumnName("NP2_IIO");

             //Properties
            this.Property(t => t.NP2_IDV)
                .HasColumnName("NP2_IDV");

             //Properties
            this.Property(t => t.NP2_IDE_D)
                .HasColumnName("NP2_IDE_D");

             //Properties
            this.Property(t => t.NP2_IDE_I)
                .HasColumnName("NP2_IDE_I");

            //Properties
            this.Property(t => t.NP2_IRD)
                .HasColumnName("NP2_IRD");

            //Properties
            this.Property(t => t.NP2_IRR)
                .HasColumnName("NP2_IRR");

            //Properties
            this.Property(t => t.NP2_ISA_MA)
                .HasColumnName("NP2_ISA_MA");

            //Properties
            this.Property(t => t.NP2_ISA_PA)
                .HasColumnName("NP2_ISA_PA");

            //Properties
            this.Property(t => t.NP2_ISA_AS)
                .HasColumnName("NP2_ISA_AS");

            //Properties
            this.Property(t => t.NP2_RESULTADO)
                .HasColumnName("NP2_RESULTADO");

            //Properties
            this.Property(t => t.NP2_ID)
                .HasColumnName("NP2_ID");

            //Properties
            this.Property(t => t.NP2_IC)
                .HasColumnName("NP2_IC");

            //Properties
            this.Property(t => t.NP2_IV)
                .HasColumnName("NP2_IV");

            //Properties
            this.Property(t => t.NP3_IAI)
                .HasColumnName("NP3_IAI");

            //Properties
            this.Property(t => t.NP3_ISD)
                .HasColumnName("NP3_ISD");

            //Properties
            this.Property(t => t.NP3_IIO)
                .HasColumnName("NP3_IIO");

            //Properties
            this.Property(t => t.NP3_IDV)
                .HasColumnName("NP3_IDV");

            //Properties
            this.Property(t => t.NP3_IDE_D)
                .HasColumnName("NP3_IDE_D");

            //Properties
            this.Property(t => t.NP3_IDE_I)
                .HasColumnName("NP3_IDE_I");

            //Properties
            this.Property(t => t.NP3_IRD)
                .HasColumnName("NP3_IRD");

            //Properties
            this.Property(t => t.NP3_IRR)
                .HasColumnName("NP3_IRR");

            //Properties
            this.Property(t => t.NP3_ISA_MA)
                .HasColumnName("NP3_ISA_MA");

            //Properties
            this.Property(t => t.NP3_ISA_PA)
                .HasColumnName("NP3_ISA_PA");

            //Properties
            this.Property(t => t.NP3_ISA_AS)
                .HasColumnName("NP3_ISA_AS");

             //Properties
            this.Property(t => t.NP3_RESULTADO)
                .HasColumnName("NP3_RESULTADO");

             //Properties
            this.Property(t => t.NP3_ID)
                .HasColumnName("NP3_ID");

             //Properties
            this.Property(t => t.NP3_IC)
                .HasColumnName("NP3_IC");

             //Properties
            this.Property(t => t.NP3_IV)
                .HasColumnName("NP3_IV");

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
