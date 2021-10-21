namespace SecurePort.Web.Reportes.Centros24h
{
    partial class OPPCentro
    {
        #region Component Designer generated code
        /// <summary>
        /// Required method for telerik Reporting designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Telerik.Reporting.ReportParameter reportParameter1 = new Telerik.Reporting.ReportParameter();
            Telerik.Reporting.ReportParameter reportParameter2 = new Telerik.Reporting.ReportParameter();
            Telerik.Reporting.Drawing.StyleRule styleRule1 = new Telerik.Reporting.Drawing.StyleRule();
            this.detail = new Telerik.Reporting.DetailSection();
            this.NombreOPP = new Telerik.Reporting.TextBox();
            this.TelefonoOPP = new Telerik.Reporting.TextBox();
            this.FaxOPP = new Telerik.Reporting.TextBox();
            this.CorreoOpp = new Telerik.Reporting.TextBox();
            this.sqlOperadores = new Telerik.Reporting.SqlDataSource();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // detail
            // 
            this.detail.Height = Telerik.Reporting.Drawing.Unit.Cm(1.4000998735427856D);
            this.detail.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.NombreOPP,
            this.TelefonoOPP,
            this.FaxOPP,
            this.CorreoOpp});
            this.detail.Name = "detail";
            this.detail.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.None;
            // 
            // NombreOPP
            // 
            this.NombreOPP.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(9.9921220680698752E-05D), Telerik.Reporting.Drawing.Unit.Cm(9.9921220680698752E-05D));
            this.NombreOPP.Name = "NombreOPP";
            this.NombreOPP.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(4.3000001907348633D), Telerik.Reporting.Drawing.Unit.Cm(1.3999999761581421D));
            this.NombreOPP.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.NombreOPP.Value = "= Fields.Nombre";
            // 
            // TelefonoOPP
            // 
            this.TelefonoOPP.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(4.2999997138977051D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.TelefonoOPP.Name = "TelefonoOPP";
            this.TelefonoOPP.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1.8200000524520874D), Telerik.Reporting.Drawing.Unit.Cm(1.3999999761581421D));
            this.TelefonoOPP.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.TelefonoOPP.Value = "= Fields.Telefono";
            // 
            // FaxOPP
            // 
            this.FaxOPP.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(6.1202001571655273D), Telerik.Reporting.Drawing.Unit.Cm(0.00010012308484874666D));
            this.FaxOPP.Name = "FaxOPP";
            this.FaxOPP.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1.7999999523162842D), Telerik.Reporting.Drawing.Unit.Cm(1.3999999761581421D));
            this.FaxOPP.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.FaxOPP.Value = "= Fields.Fax";
            // 
            // CorreoOpp
            // 
            this.CorreoOpp.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(7.9204006195068359D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.CorreoOpp.Name = "CorreoOpp";
            this.CorreoOpp.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3.5999999046325684D), Telerik.Reporting.Drawing.Unit.Cm(1.3999999761581421D));
            this.CorreoOpp.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.CorreoOpp.Value = "= Fields.Email";
            // 
            // sqlOperadores
            // 
            this.sqlOperadores.ConnectionString = "SecurePortContext";
            this.sqlOperadores.Name = "sqlOperadores";
            this.sqlOperadores.SelectCommand = " ";
            // 
            // OPPCentro
            // 
            this.DataSource = this.sqlOperadores;
            this.Filters.Add(new Telerik.Reporting.Filter("= Fields.ID", Telerik.Reporting.FilterOperator.Equal, "= Parameters.IDCentro.Value"));
            this.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.detail});
            this.Name = "OPPCentro";
            this.PageSettings.Margins = new Telerik.Reporting.Drawing.MarginsU(Telerik.Reporting.Drawing.Unit.Mm(25.399999618530273D), Telerik.Reporting.Drawing.Unit.Mm(25.399999618530273D), Telerik.Reporting.Drawing.Unit.Mm(25.399999618530273D), Telerik.Reporting.Drawing.Unit.Mm(25.399999618530273D));
            this.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.A4;
            reportParameter1.Name = "IDCentro";
            reportParameter1.Type = Telerik.Reporting.ReportParameterType.Integer;
            reportParameter1.Value = "";
            reportParameter2.AllowNull = true;
            reportParameter2.Name = "IdPuerto";
            reportParameter2.Type = Telerik.Reporting.ReportParameterType.Integer;
            reportParameter2.Value = "0";
            this.ReportParameters.Add(reportParameter1);
            this.ReportParameters.Add(reportParameter2);
            styleRule1.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            new Telerik.Reporting.Drawing.TypeSelector(typeof(Telerik.Reporting.TextItemBase)),
            new Telerik.Reporting.Drawing.TypeSelector(typeof(Telerik.Reporting.HtmlTextBox))});
            styleRule1.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Point(2D);
            styleRule1.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Point(2D);
            this.StyleSheet.AddRange(new Telerik.Reporting.Drawing.StyleRule[] {
            styleRule1});
            this.Width = Telerik.Reporting.Drawing.Unit.Cm(11.5D);
            this.ItemDataBinding += new System.EventHandler(this.OPPCentro_ItemDataBinding);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }
        #endregion

        private Telerik.Reporting.DetailSection detail;
        private Telerik.Reporting.SqlDataSource sqlOperadores;
        private Telerik.Reporting.TextBox NombreOPP;
        private Telerik.Reporting.TextBox TelefonoOPP;
        private Telerik.Reporting.TextBox FaxOPP;
        private Telerik.Reporting.TextBox CorreoOpp;
    }
}