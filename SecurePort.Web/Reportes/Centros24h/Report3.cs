namespace SecurePort.Web.Reportes
{
    #region Using
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using Telerik.Reporting;
    using Telerik.Reporting.Drawing;

    using SecurePort.Web.Controllers;
    using SecurePort.Entities.Enums;
    using SecurePort.Entities.Exceptions;
    using SecurePort.Entities.Models;
    using SecurePort.Entities.Models.Json;
    using SecurePort.Services.Interfaces;
    using SecurePort.Services.Repository;
    using SecurePort.Entities;
    using System.Collections.Generic;  

    #endregion

    /// <summary>
    /// Summary description for Report3.
    /// </summary>
    public partial class Report3 : Telerik.Reporting.Report
    {
        #region Definiciones
        int idUsuario;
        int Categoria;
        #endregion

        public Report3()
        {
            //
            // Required for telerik Reporting designer support
            //
            InitializeComponent();

            //
            // TODO: Add any constructor code after InitializeComponent call
            //

            this.Fecha.Value = "Fecha " + DateTime.Now.ToShortDateString();


        }

       


        private void table1_ItemDataBinding(object sender, EventArgs e)
        {
           
        }
             

        private void Report3_ItemDataBinding(object sender, EventArgs e)
        {
            Telerik.Reporting.Processing.Report report = (Telerik.Reporting.Processing.Report)sender;

              idUsuario = Convert.ToInt32(report.Parameters["Usuario"].Value.ToString());
              Categoria = Convert.ToInt32(report.Parameters["Categoria"].Value.ToString());
            string  consulta = string.Empty;

            switch(Categoria){
                case 3:
                    consulta = "SELECT a.ID, c.Nombre AS Organismo, b.Nombre AS Puerto, a.Centro_24H, a.Direccion,a.Cod_Postal, a.ID_Puerto ";
                    consulta += " FROM  SPMOV_Centros_24H AS a ";
                    consulta += " INNER JOIN SPMAE_Organismos AS c ON a.ID_Organismo = c.ID ";
                    consulta += " LEFT OUTER JOIN ";
                    consulta += " SPMAE_Puertos AS b ON a.ID_Puerto = b.ID ";
                    consulta += " Left join SPCON_Depen_Usuario e on c.ID = e.ID_Dependencia ";
                    consulta += " WHERE e.ID_Usuario = " + idUsuario;
                    consulta += " ORDER BY c.Nombre, a.Centro_24H ";
                    break;
                case 4:
                    consulta = "SELECT a.ID, c.Nombre AS Organismo, f.Nombre AS Puerto, a.Centro_24H, a.Direccion,a.Cod_Postal, a.ID_Puerto ";
                    consulta += " FROM  SPMOV_Centros_24H AS a ";
                    consulta += " LEFT JOIN SPMAE_Puertos AS f ON f.ID = a.ID_Puerto ";
                    consulta += " INNER JOIN SPMAE_Organismos AS c ON a.ID_Organismo = c.ID ";
                    consulta += " LEFT OUTER JOIN SPMAE_Puertos AS b ON c.ID = b.ID_Organismo ";
                    consulta += " LEFT JOIN SPCON_Depen_Usuario e on b.ID = e.ID_Dependencia ";
                    consulta += " WHERE e.ID_Usuario = " + idUsuario;
                    consulta += " ORDER BY c.Nombre, a.Centro_24H";
                    break;
                case 5:
                    consulta = "SELECT a.ID, c.Nombre AS Organismo, f.Nombre AS Puerto, a.Centro_24H, a.Direccion,a.Cod_Postal, a.ID_Puerto, a.ID_Organismo ";
                    consulta +=" FROM  SPMOV_Centros_24H AS a ";
                    consulta += " LEFT JOIN SPMAE_Puertos AS f ON f.ID = a.ID_Puerto ";
                    consulta +=" INNER JOIN SPMAE_Organismos AS c ON a.ID_Organismo = c.ID ";
                    consulta +=" LEFT OUTER JOIN SPMAE_Puertos AS b ON c.ID = b.ID_Organismo ";
                    consulta +=" RIGHT JOIN SPMOV_Instalaciones d ON  b.ID = d.ID_Puerto ";
                    consulta +=" LEFT JOIN SPCON_Depen_Usuario e on d.ID = e.ID_Dependencia ";
                    consulta +=" WHERE e.ID_Usuario = " + idUsuario;
                    consulta += " ORDER BY c.Nombre, a.Centro_24H ";
                    break;
               default:
                    consulta = "SELECT a.ID, c.Nombre AS Organismo, b.Nombre AS Puerto, a.Centro_24H, a.Direccion, e.Nombre AS Ciudad, a.Cod_Postal, d.Nombre AS Provincia, a.ID_Puerto";
                    consulta +=" FROM  SPMOV_Centros_24H AS a LEFT OUTER JOIN";
                    consulta +=" SPMAE_Puertos AS b ON a.ID_Puerto = b.ID INNER JOIN";
                    consulta +=" SPMAE_Organismos AS c ON a.ID_Organismo = c.ID LEFT OUTER JOIN";
                    consulta +=" SPMAE_Provincias AS d ON a.ID_Provincia = d.ID LEFT OUTER JOIN";
                    consulta += " SPMAE_Ciudades AS e ON a.ID_Ciudad = e.ID";
                    consulta += " ORDER BY c.Nombre, a.Centro_24H ";
                    break;
            }
            
            this.Centros.SelectCommand = consulta;

           
        }

       

       

    }
}