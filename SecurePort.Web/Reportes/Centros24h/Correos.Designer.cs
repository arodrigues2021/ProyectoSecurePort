namespace SecurePort.Web.Reportes.Centros24h
{
    partial class Correos
    {
        #region Component Designer generated code
        /// <summary>
        /// Required method for telerik Reporting designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Correos));
            Telerik.Reporting.ReportParameter reportParameter1 = new Telerik.Reporting.ReportParameter();
            Telerik.Reporting.Drawing.StyleRule styleRule1 = new Telerik.Reporting.Drawing.StyleRule();
            this.detail = new Telerik.Reporting.DetailSection();
            this.CorreoCentro = new Telerik.Reporting.TextBox();
            this.NotaCorreo = new Telerik.Reporting.TextBox();
            this.sqlCorreos = new Telerik.Reporting.SqlDataSource();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // detail
            // 
            this.detail.Height = Telerik.Reporting.Drawing.Unit.Cm(1.4002996683120728D);
            this.detail.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.CorreoCentro,
            this.NotaCorreo});
            this.detail.Name = "detail";
            this.detail.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            // 
            // CorreoCentro
            // 
            this.CorreoCentro.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0D), Telerik.Reporting.Drawing.Unit.Cm(9.9921220680698752E-05D));
            this.CorreoCentro.Name = "CorreoCentro";
            this.CorreoCentro.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(4.0999999046325684D), Telerik.Reporting.Drawing.Unit.Cm(0.60000002384185791D));
            this.CorreoCentro.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.None;
            this.CorreoCentro.Value = "= Fields.Dato";
            // 
            // NotaCorreo
            // 
            this.NotaCorreo.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0D), Telerik.Reporting.Drawing.Unit.Cm(0.6002996563911438D));
            this.NotaCorreo.Name = "NotaCorreo";
            this.NotaCorreo.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(4.0999999046325684D), Telerik.Reporting.Drawing.Unit.Cm(0.800000011920929D));
            this.NotaCorreo.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.None;
            this.NotaCorreo.Value = "= Fields.Nota";
            // 
            // sqlCorreos
            // 
            this.sqlCorreos.ConnectionString = "SecurePortContext";
            this.sqlCorreos.Name = "sqlCorreos";
            this.sqlCorreos.SelectCommand = resources.GetString("sqlCorreos.SelectCommand");
            // 
            // Correos
            // 
            this.DataSource = this.sqlCorreos;
            this.Filters.Add(new Telerik.Reporting.Filter("= Fields.ID_Centro24h", Telerik.Reporting.FilterOperator.Equal, "= Parameters.IdCentro.Value"));
            this.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.detail});
            this.Name = "Correos";
            this.PageSettings.Margins = new Telerik.Reporting.Drawing.MarginsU(Telerik.Reporting.Drawing.Unit.Mm(25.399999618530273D), Telerik.Reporting.Drawing.Unit.Mm(25.399999618530273D), Telerik.Reporting.Drawing.Unit.Mm(25.399999618530273D), Telerik.Reporting.Drawing.Unit.Mm(25.399999618530273D));
            this.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.A4;
            reportParameter1.Name = "IdCentro";
            reportParameter1.Type = Telerik.Reporting.ReportParameterType.Integer;
            reportParameter1.Value = "0";
            this.ReportParameters.Add(reportParameter1);
            styleRule1.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            new Telerik.Reporting.Drawing.TypeSelector(typeof(Telerik.Reporting.TextItemBase)),
            new Telerik.Reporting.Drawing.TypeSelector(typeof(Telerik.Reporting.HtmlTextBox))});
            styleRule1.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Point(2D);
            styleRule1.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Point(2D);
            this.StyleSheet.AddRange(new Telerik.Reporting.Drawing.StyleRule[] {
            styleRule1});
            this.Width = Telerik.Reporting.Drawing.Unit.Cm(4.0999999046325684D);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }
        #endregion

        private Telerik.Reporting.DetailSection detail;
        private Telerik.Reporting.SqlDataSource sqlCorreos;
        private Telerik.Reporting.TextBox CorreoCentro;
        private Telerik.Reporting.TextBox NotaCorreo;
    }
}