namespace SecurePort.Web.Controllers
{
	#region Using

	using SecurePort.Entities.Models;
	using SecurePort.Entities.Models.Json;
	using SecurePort.Services.Interfaces;
	using SecurePort.Services.Repository;
	using System;
	using System.Configuration;
	using System.IO;
	using System.Web;
	using System.Web.Mvc;
	using System.Net;

	#endregion

	public class HomeController : BaseController
	{


		public HomeController(IServiciosUsuarios serviciosUsuarios, 
							  TiposDocumentosRepository repoTiposDocumentos,
							  TrazasRepository repoTrazas,
							  ILogger log)
			: base(serviciosUsuarios,
				   repoTiposDocumentos,
				   repoTrazas,
				   log)
		{

		}

		private const string InternetExplorerUserAgent = "internetexplorer";

		//private const string SafariUserAgent = "safari";

		private const string chromeUserAgent = "chrome";

		[OutputCache(NoStore = true, Duration = 1)]
		public ActionResult Index()
		{

			//Obtenemos el navagador
			HttpRequestBase request = Request;

			HttpBrowserCapabilitiesBase browser = request.Browser;

			bool isWindowsOs = browser.Win32 || browser.Win16;

			if ((browser.Browser.ToLower() == InternetExplorerUserAgent))
			{
				this.Browser = InternetExplorerUserAgent;
			}
			if ((browser.Browser.ToLower() == chromeUserAgent))
			{
				this.Browser = chromeUserAgent;
			}
			if (this.UsuarioFrontalSession.Usuario != null)
			{
				this.trazas(null, (int)this.UsuarioFrontalSession.Usuario.id, "Se ha Conectado al sistema el usuario " + this.UsuarioFrontalSession.Usuario.login);

				return RedirectToAction("Principal","Menu");
			}

			return this.View();
		}

		public ActionResult UpLoad()
		{
			return this.View("UpLoad");
		}


		/// <summary>
		/// Validamos el fichero que vamos a subir.
		/// </summary>  
		[HttpPost]
        public ActionResult FileUpload()
		{

            int Id = Convert.ToInt32(Request["IdVista"]);
            string TipServ = Request["TipoSer"];
            
            bool isUploaded = false;

            string mensaje = string.Empty;

            for (int i = 0; i < Request.Files.Count; i++)
            {
                var myFile = Request.Files[i];

                string extension = Path.GetExtension(myFile.FileName);

                if (myFile != null && myFile.ContentLength == 0)
                {
                    mensaje = "Error en el fichero tiene longitud 0";

                    return Json(new { isUploaded = false, Message = mensaje }, "text/html");
                }
                if (myFile.ContentLength > 1024 * 1024 * 100)
                {
                    mensaje = "El fichero no debe ser mayor 100MB";

                    return Json(new { isUploaded = false, Message = mensaje }, "text/html");
                }
                if (extension == ".doc" || extension == ".xls" || extension == ".xlsx" || extension == ".pdf" || extension == ".docx" || extension == ".png" || extension == ".jpg" || extension == ".gif" || extension == ".odt" || extension == ".ods")
                {
             
                    var NombreRuta = ConfigurationManager.AppSettings["RutaSecurePorDoc"].ToString(); 
                    var NombreFichero = TipServ + Id.ToString() + "_" + CambiarTexto(Request.Files[i].FileName);

                    isUploaded = true;

                    myFile.SaveAs(NombreRuta + "\\"   + NombreFichero);
                }

            }

            return Json(new { isUploaded = isUploaded }, "text/html");
			
		}


		/// <summary>
		/// Ejecutamos la subida del fichero.
		/// </summary> 
		[HttpPost]
		public ActionResult SubirFichero(UploadJson UploadJson)
		{
			bool isUploaded = false;

			string mensaje = string.Empty;
			
			try
			{

				if (UploadJson.descripcion==null)
				{
					mensaje = "La Descripción es un campo requerido";

				}else if (UploadJson.nombre != null)
				{
					isUploaded = true;

					mensaje = "El fichero ha subido correctamente";

					Docs_Usuario docs_Usuario = new Docs_Usuario();

                    docs_Usuario.documento = UploadJson.nombre_servicio + UploadJson.id_servicio.ToString() + "_" + UploadJson.nombre;

					docs_Usuario.descripcion = CambiarTexto(UploadJson.descripcion);

                    docs_Usuario.id_servicio = UploadJson.id_servicio; //this.IdUsuario;

                    docs_Usuario.id_tipo_serv = UploadJson.servicioAlta; //1;

					docs_Usuario.id_tipo_doc = Convert.ToInt32(UploadJson.tipo);

					docs_Usuario.fech_alta = DateTime.Now;

                    docs_Usuario.id_usu_alta = this.UsuarioFrontalSession.Usuario.id; //this.IdUsuario;

					docs_Usuario.fech_documento = DateTime.Now;

					this.repoTiposDocumentos.Create(docs_Usuario);

					this.log.WriteInfo("Alta documentos", docs_Usuario.documento + '|' + docs_Usuario.descripcion + '|' + docs_Usuario.id + '|' + docs_Usuario.id_tipo_doc);
				}

				return this.Json(new { isUploaded = isUploaded, Message = mensaje },"text/html");
			}
			catch (Exception ex)
			{
				mensaje = string.Format("Fallo la subida del fichero: {0}",ex.Message);
				isUploaded = false;
				if (System.IO.File.Exists(Server.MapPath("~/Exportar/" + UploadJson.nombre)))
				{
					System.IO.File.Delete(Server.MapPath("~/Exportar/" + UploadJson.nombre));
				}
			}
			return this.Json(new { isUploaded = isUploaded, Message = mensaje }, "text/html");
		}
	}
 }

   



	