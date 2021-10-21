namespace SecurePort.Web
{
    #region Using

    using System.IO;
    using System.Web;
    using System.Web.Mvc;
    using ClosedXML.Excel;
    #endregion

    public class ExcelResult : ActionResult
    {
        private readonly XLWorkbook _workbook;

        private readonly string _fileName;

        public ExcelResult(XLWorkbook workbook, string fileName)
        {
            this._workbook = workbook;
            this._fileName = fileName;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            this._workbook.SaveAs("c:\\secureport\\" + this._fileName);
            
            HttpResponseBase Response = context.HttpContext.Response;

            string fileName = "c:\\secureport\\" + this._fileName;

            string filePath = "c:\\secureport\\" ;

            FileStream sourceFile = new FileStream(@"c:\\secureport\\" + this._fileName, FileMode.Open);

            float FileSize;

            FileSize = sourceFile.Length;

            byte[] getContent = new byte[(int)FileSize];

            sourceFile.Read(getContent, 0, (int)sourceFile.Length);

            sourceFile.Close();

            Response.ClearContent();

            Response.ClearHeaders();

            Response.Buffer = true;

            Response.ContentType = "application/octet-stream";

            Response.AddHeader("Content-Length", getContent.Length.ToString());

            //Response.AddHeader("Content-Disposition", "attachment; filename=" + "c:\\secureport\\" + this._fileName);

            Response.AppendHeader("content-disposition", "attachment; filename=" + fileName);
            
            Response.WriteFile(filePath);

            Response.BinaryWrite(getContent);

            Response.Flush();

            Response.End();
            
        }
        
    }
}