namespace SecurePort.Web.Controllers
{
	#region Using
	using System;
	using System.Collections.Generic;
	using System.Data.Entity.Infrastructure;
	using System.Data.Entity.ModelConfiguration;
	using System.Data.Entity.Validation;
	using System.Linq;
	using System.Web.Mvc;
	using Kendo.Mvc.Extensions;
	using Kendo.Mvc.UI;
	using SecurePort.Entities.Enums;
	using SecurePort.Entities.Exceptions;
	using SecurePort.Entities.Models;
	using SecurePort.Entities.Models.Json;
	using SecurePort.Services.Interfaces;
	using SecurePort.Services.Repository;
	using System.Configuration;
	using System.Web;
    using System.Web.UI.WebControls; 

	#endregion

	public class VinculosController : BaseController
	{
		public VinculosController(IServiciosVinculos   serviciosVinculos,
								 VinculosRepository   repoVinculos,
								 TrazasRepository     repotrazas, 
								 ILogger logger)
			
			: base(serviciosVinculos,
				   repoVinculos,
				   repotrazas,logger)

		{
		}

		#region Buscador
		/// <summary>
		/// Permite buscar las plantillas o documentos 
		/// </summary>
		public ActionResult BuscadorVinculos([DataSourceRequest]DataSourceRequest request)
		{
			try
			{
				if (this.UsuarioFrontalSession.Usuario.categoria == (int)EnumCategoria.Administrador
					|| this.UsuarioFrontalSession.Usuario.categoria == (int)EnumCategoria.PuertosdelEstado)
				{
					return this.Json(this._serviciosVinculos.GetAllVinculos(this.TipoDocumento).OrderBy(x=> x.Nombre).ToDataSourceResult(request));
				}
				return this.Json(this._serviciosVinculos.GetAllVinculos(this.TipoDocumento, this.UsuarioFrontalSession.Usuario.categoria).OrderBy(x => x.Nombre).ToDataSourceResult(request));
			}
			catch (Exception ex)
			{
				this.log.PublishException(new SecureportException(ex));
				throw;
			}
		}
		#endregion

		#region action
		/// <summary>
		/// Retorna una lista de todas las plantillas
		/// </summary>
		public ActionResult ListadoPlantillas()
		{
			try
			{
				ViewBag.Mensaje = "Gestión de Plantillas";
                ViewBag.CodigoMenu = "#Id039";
                this.TipoDocumento = "PLN";
                //ViewBag.UrlDelete = "/Gisis/EliminarGisis";
				return this.PartialView("ListadoVinculos", this.UsuarioFrontalSession);
			}
			catch (Exception ex)
			{
				this.log.PublishException(new SecureportException(ex));
				throw;
			}
		}

        /// <summary>
		/// Retorna una lista de todos los documentos
		/// </summary>
		public ActionResult ListadoDocumentos()
		{
			try
			{
				ViewBag.Mensaje = "Gestión de Documentación";
                ViewBag.CodigoMenu = "#Id040";
                this.TipoDocumento = "DOC";
                //ViewBag.UrlDelete = "/Gisis/EliminarGisis";
				return this.PartialView("ListadoVinculos", this.UsuarioFrontalSession);
			}
			catch (Exception ex)
			{
				this.log.PublishException(new SecureportException(ex));
				throw;
			}
		}
		#endregion

		#region CRUD
		[HttpPost]
		public ActionResult visualizarDocumento(int? id)
		{
			try
			{

                Vinculos vinculo = this.repoVinculos.Single(x => x.Id == id);

				string doc = vinculo.Nombre;

                string nombreRuta = ConfigurationManager.AppSettings["RutaSecureDoc"];

				this.UsuarioFrontalSession.Path = nombreRuta + "/" + CambiarTexto(doc);

				return PartialView("~/Views/Shared/PartialDoc.cshtml", this.UsuarioFrontalSession);

			}
			catch (Exception ex)
			{
				this.log.PublishException(new SecureportException(ex));

				return this.Json(new { result = false, Message = ex.Message });
			}

		}
		
		
		#endregion

	
	}
}