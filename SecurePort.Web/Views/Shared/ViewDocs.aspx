<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>


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

          string filName = "C:/Publicado/SecurePortDoc/Real.pdf";
           System.IO.FileInfo _fileInfo = new System.IO.FileInfo(filName);
           if (_fileInfo.Exists)
           {

               if (_fileInfo.Length < 1006496)
               {

                   // Clear the whatever
                   Response.Buffer = false;
                   Response.Clear();
                   String sContentType = "";
                   // Determine the content type

                   // Set the headers ??? don't ask
                   Response.AddHeader("Content-Disposition", "inline; filename=\"" + _fileInfo.Name + "\"");
                   Response.AddHeader("Content-Length", _fileInfo.Length.ToString());
                   // Set the content type
                   Response.ContentType = "application/pdf";
                   // Write the file to the browser
                   Response.Flush();

                   Response.WriteFile(_fileInfo.FullName);
                   Response.End();
               }
               else
                   Response.Write("File size must be less than 2MB.");

           }
      }
      catch (Exception ex)
      {
          throw new Exception("Fichero no encontrado");
      }
  }


</script>

    <form id="form1" runat="server">
    <div style="height: 100%">
    
      
    
    </div>
    </form>
</body>
</html>
