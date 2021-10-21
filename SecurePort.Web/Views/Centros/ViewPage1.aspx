<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<%@ Register assembly="Telerik.ReportViewer.WebForms, Version=10.0.16.204, Culture=neutral, PublicKeyToken=a9d7983dfcc261be" namespace="Telerik.ReportViewer.WebForms" tagprefix="telerik" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Reportes</title>
</head>
<body>
    <script runat="server">
 //public override void VerifyRenderingInServerForm(Control control)
 // {
 //// to avoid the server form (<form runat="server"> requirement
 // }
 protected override void OnLoad(EventArgs e)
  {
      try
      {
          base.OnLoad(e);

          Telerik.Reporting.TypeReportSource instanceReportSource = new Telerik.Reporting.TypeReportSource();
          instanceReportSource.TypeName = "SecurePort.Web.Reportes.Report3, SecurePort, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
          instanceReportSource.Parameters.Add(new Telerik.Reporting.Parameter("Usuario", ((SecurePort.Entities.Models.UsuarioFrontal)this.Session["UsuarioFrontal"]).Usuario.id));
          instanceReportSource.Parameters.Add(new Telerik.Reporting.Parameter("Categoria", ((SecurePort.Entities.Models.UsuarioFrontal)this.Session["UsuarioFrontal"]).Usuario.categoria));
          
          ReportViewer1.ReportSource = instanceReportSource;
          ReportViewer1.RefreshReport();
      }
      catch (Exception ex)
      {
          throw new Exception("Fichero no encontrado");
      }
  }


</script>

    <form id="form1" runat="server">
    <div style="height: 100%;text-align:center;"  >
    
        <telerik:ReportViewer ID="ReportViewer1" runat="server" Width="90%" Height="800px" ProgressText="Generando reporte..." Resources-LabelOf="de" Resources-ProcessingReportMessage="Cargando datos..." ShowZoomSelect="True" ZoomPercent="105" ></telerik:ReportViewer>
    
    </div>
    </form>
</body>
</html>
