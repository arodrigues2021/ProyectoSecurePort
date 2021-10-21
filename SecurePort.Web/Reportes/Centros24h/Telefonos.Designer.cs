namespace SecurePort.Web.Reportes
{
    partial class Telefonos
    {
        #region Component Designer generated code
        /// <summary>
        /// Required method for telerik Reporting designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Telefonos));
            Telerik.Reporting.ReportParameter reportParameter1 = new Telerik.Reporting.ReportParameter();
            Telerik.Reporting.Drawing.StyleRule styleRule1 = new Telerik.Reporting.Drawing.StyleRule();
            this.detail = new Telerik.Reporting.DetailSection();
            this.TelNum = new Telerik.Reporting.TextBox();
            this.NotaTel = new Telerik.Reporting.TextBox();
            this.TelefonoCentro = new Telerik.Reporting.SqlDataSource();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // detail
            // 
            this.detail.Height = Telerik.Reporting.Drawing.Unit.Cm(1.4001997709274292D);
            this.detail.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.TelNum,
            this.NotaTel});
            this.detail.Name = "detail";
            this.detail.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            // 
            // TelNum
            // 
            this.TelNum.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.TelNum.Name = "TelNum";
            this.TelNum.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.7000000476837158D), Telerik.Reporting.Drawing.Unit.Cm(0.60000002384185791D));
            this.TelNum.Value = "= Fields.Dato";
            // 
            // NotaTel
            // 
            this.NotaTel.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.00010012308484874666D), Telerik.Reporting.Drawing.Unit.Cm(0.60019975900650024D));
            this.NotaTel.Name = "NotaTel";
            this.NotaTel.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.7000000476837158D), Telerik.Reporting.Drawing.Unit.Cm(0.800000011920929D));
            this.NotaTel.Value = "= Fields.Nota";
            // 
            // TelefonoCentro
            // 
            this.TelefonoCentro.ConnectionString = "SecurePortContext";
            this.TelefonoCentro.Name = "TelefonoCentro";
            this.TelefonoCentro.SelectCommand = resources.GetString("TelefonoCentro.SelectCommand");
            // 
            // Telefonos
            // 
            this.DataSource = this.TelefonoCentro;
            this.Filters.Add(new Telerik.Reporting.Filter("= Fields.ID_Centro24h", Telerik.Reporting.FilterOperator.Equal, "= Parameters.IdCentros.Value"));
            this.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.detail});
            this.Name = "Telefonos";
            this.PageSettings.Margins = new Telerik.Reporting.Drawing.MarginsU(Telerik.Reporting.Drawing.Unit.Mm(25.399999618530273D), Telerik.Reporting.Drawing.Unit.Mm(25.399999618530273D), Telerik.Reporting.Drawing.Unit.Mm(25.399999618530273D), Telerik.Reporting.Drawing.Unit.Mm(25.399999618530273D));
            this.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.A4;
            reportParameter1.Name = "IdCentros";
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
            this.Width = Telerik.Reporting.Drawing.Unit.Cm(2.7000000476837158D);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }
        #endregion

        private Telerik.Reporting.DetailSection detail;
        private Telerik.Reporting.SqlDataSource TelefonoCentro;
        private Telerik.Reporting.TextBox TelNum;
        private Telerik.Reporting.TextBox NotaTel;
    }
}