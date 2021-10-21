namespace SecurePort.Web.Reportes.Centros24h
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using Telerik.Reporting;
    using Telerik.Reporting.Drawing;

    /// <summary>
    /// Summary description for OPPCentro.
    /// </summary>
    public partial class OPPCentro : Telerik.Reporting.Report
    {
        public OPPCentro()
        {
            //
            // Required for telerik Reporting designer support
            //
            InitializeComponent();

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }

        private void OPPCentro_ItemDataBinding(object sender, EventArgs e)
        {
            try
            {
                int? idPuerto = 0;
                Telerik.Reporting.Processing.Report report = (Telerik.Reporting.Processing.Report)sender;
                if (report.Parameters["IdPuerto"].Value != null)
                    idPuerto = Convert.ToInt32(report.Parameters["IdPuerto"].Value.ToString());

                string consulta = string.Empty;
                if (idPuerto != 0)
                {
                    consulta = "SELECT a.ID, a.ID_Puerto, b.Nombre, b.Telefono, b.Fax, b.Email ";
                    consulta += " FROM SPMOV_Centros_24H AS a INNER JOIN ";
                    consulta += " SPMAE_Organismos AS c ON c.ID = a.ID_Organismo LEFT OUTER JOIN ";
                    consulta += " SPMAE_Operadores_Puerto AS b ON a.ID_Puerto = b.ID_Puerto ";
                    consulta += " WHERE (b.Es_Suplente = 0) AND (b.Es_Activo = 1)";
                    consulta += " ORDER BY b.Nombre";
                }
                else
                {
                    consulta = "SELECT a.ID, a.ID_Puerto, b.Nombre, b.Telefono, b.Fax, b.Email ";
                    consulta += " FROM SPMOV_Centros_24H AS a INNER JOIN ";
                    consulta += " SPMAE_Organismos AS c ON c.ID = a.ID_Organismo LEFT OUTER JOIN ";
                    consulta += " SPMAE_Puertos AS d ON d.ID_Organismo = c.ID LEFT OUTER JOIN ";
                    consulta += " SPMAE_Operadores_Puerto AS b ON d.ID = b.ID_Puerto ";
                    consulta += " WHERE (b.Es_Suplente = 0) AND (b.Es_Activo = 1) ";
                    consulta += " ORDER BY b.Nombre";
                }

                this.sqlOperadores.SelectCommand = consulta;          
           }
            catch (Exception ex)
            {
                throw new Exception("error" + ex.Message);
            }

        }
    }
}