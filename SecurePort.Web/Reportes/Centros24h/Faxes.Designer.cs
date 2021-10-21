namespace SecurePort.Web.Reportes
{
    partial class Faxes
    {
        #region Component Designer generated code
        /// <summary>
        /// Required method for telerik Reporting designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Faxes));
            Telerik.Reporting.ReportParameter reportParameter1 = new Telerik.Reporting.ReportParameter();
            Telerik.Reporting.Drawing.StyleRule styleRule1 = new Telerik.Reporting.Drawing.StyleRule();
            this.detail = new Telerik.Reporting.DetailSection();
            this.Fax = new Telerik.Reporting.TextBox();
            this.NotaFax = new Telerik.Reporting.TextBox();
            this.sqlFaxes = new Telerik.Reporting.SqlDataSource();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // detail
            // 
            this.detail.Height = Telerik.Reporting.Drawing.Unit.Cm(1.4002000093460083D);
            this.detail.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.Fax,
            this.NotaFax});
            this.detail.Name = "detail";
            this.detail.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            // 
            // Fax
            // 
            this.Fax.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(9.9921220680698752E-05D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.Fax.Name = "Fax";
            this.Fax.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.5999999046325684D), Telerik.Reporting.Drawing.Unit.Cm(0.60000002384185791D));
            this.Fax.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.None;
            this.Fax.Value = "= Fields.Dato";
            // 
            // NotaFax
            // 
            this.NotaFax.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0D), Telerik.Reporting.Drawing.Unit.Cm(0.60019993782043457D));
            this.NotaFax.Name = "NotaFax";
            this.NotaFax.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.5999999046325684D), Telerik.Reporting.Drawing.Unit.Cm(0.800000011920929D));
            this.NotaFax.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.None;
            this.NotaFax.Value = "= Fields.Nota";
            // 
            // sqlFaxes
            // 
            this.sqlFaxes.ConnectionString = "SecurePortContext";
            this.sqlFaxes.Name = "sqlFaxes";
            this.sqlFaxes.SelectCommand = resources.GetString("sqlFaxes.SelectCommand");
            // 
            // Faxes
            // 
            this.DataSource = this.sqlFaxes;
            this.Filters.Add(new Telerik.Reporting.Filter("= Fields.ID_Centro24h", Telerik.Reporting.FilterOperator.Equal, "= Parameters.IdCentros.Value"));
            this.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.detail});
            this.Name = "Faxes";
            this.PageSettings.Margins = new Telerik.Reporting.Drawing.MarginsU(Telerik.Reporting.Drawing.Unit.Mm(25.399999618530273D), Telerik.Reporting.Drawing.Unit.Mm(25.399999618530273D), Telerik.Reporting.Drawing.Unit.Mm(25.399999618530273D), Telerik.Reporting.Drawing.Unit.Mm(25.399999618530273D));
            this.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.A4;
            reportParameter1.Name = "IdCentros";
            reportParameter1.Value = "0";
            this.ReportParameters.Add(reportParameter1);
            styleRule1.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            new Telerik.Reporting.Drawing.TypeSelector(typeof(Telerik.Reporting.TextItemBase)),
            new Telerik.Reporting.Drawing.TypeSelector(typeof(Telerik.Reporting.HtmlTextBox))});
            styleRule1.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Point(2D);
            styleRule1.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Point(2D);
            this.StyleSheet.AddRange(new Telerik.Reporting.Drawing.StyleRule[] {
            styleRule1});
            this.Width = Telerik.Reporting.Drawing.Unit.Cm(2.5999999046325684D);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }
        #endregion

        private Telerik.Reporting.DetailSection detail;
        private Telerik.Reporting.TextBox Fax;
        private Telerik.Reporting.TextBox NotaFax;
        private Telerik.Reporting.SqlDataSource sqlFaxes;
    }
}