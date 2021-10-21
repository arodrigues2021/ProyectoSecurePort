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

	public class GisisController : BaseController
	{
		public GisisController(IServiciosGisis   serviciosGisis,
								 GisisRepository   repoGisis,
								 IServiciosPuertos   servicioPuertos,
								 IServiciosTipoInstalacion serviciosInstalaciones,
								 TrazasRepository     repotrazas, 
								 ILogger logger)
			
			: base(serviciosGisis,
				   repoGisis,
				   servicioPuertos,
				   serviciosInstalaciones,
				   repotrazas,logger)

		{
		}

		#region Buscador
		/// <summary>
		/// Permite buscar GISIS 
		/// </summary>
		public ActionResult BuscadorGisis([DataSourceRequest]DataSourceRequest request)
		{
			try
			{
				if (this.UsuarioFrontalSession.Usuario.categoria == (int)EnumCategoria.Administrador
					|| this.UsuarioFrontalSession.Usuario.categoria == (int)EnumCategoria.PuertosdelEstado)
				{
					return this.Json(this._serviciosGisis.GetAllGisis().ToDataSourceResult(request));
				}
				var categoria = this.UsuarioFrontalSession.Usuario.categoria;
				return this.Json(this._serviciosGisis.GetAllGisis(this.UsuarioFrontalSession.ListDependenciasDepenUsuarios, categoria).ToDataSourceResult(request));
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
		/// Retorna una lista de todos los Gisis
		/// </summary>
		public ActionResult ListadoGisis()
		{
			try
			{
				ViewBag.Mensaje = "Registros Gisis";
				ViewBag.UrlDelete = "/Gisis/EliminarGisis";
				return this.PartialView("ListadoGisis", this.UsuarioFrontalSession);
			}
			catch (Exception ex)
			{
				this.log.PublishException(new SecureportException(ex));
				throw;
			}
		}
		#endregion

		#region CRUD
		/// <summary>
		/// Permite el Alta Registros Gisis.
		/// </summary>
		[HttpPost]
		public ActionResult Create()
		{
			GisisViewModel gisis = new GisisViewModel();

			this.comboPuertos(null);

			if (((MultiSelectList)(this.ViewData["puertos"])).Count() == 1)
				gisis.Id_Puerto = Convert.ToInt32(((SelectList)this.ViewData["puertos"]).FirstOrDefault().Value);

			this.comboInstalacionesPuertos(null, null);

			ViewBag.disabled = true;

			ViewBag.Combo = "display:block";

			ViewBag.Texto = "display:none";

			ViewBag.display = "display:none";

			ViewBag.Mensaje = "Alta Registro GISIS";

			ViewBag.action = General.AltaGeneral.ToDescription();

			gisis.Id = 1;
			gisis.Fecha_Registro = DateTime.MinValue;

			//ViewBag.ToolComite = false;

			//ViewBag.alta = "display:none";

			this.ViewBag.Observaciones = string.Empty;

			return this.PartialView("~/Views/Gisis/_PartialGisis.cshtml", gisis);
		}
		/// <summary>
		/// Visualizar los datos de Resgistro Gisis
		/// </summary>
		[HttpPost]
		public ActionResult VisualizarGisis(int? id, string Accion)
		{
			bool valor = true;
			
			if (id != null)
				this.IdGisis = (int)id;
			else if (this.IdGisis != 0)
				id = this.IdGisis;

			valor = ValidarCategoria(this.IdPractica, this.repoGisis.Single(b => b.Id == this.IdGisis));

			if (Accion != General.AltaVer.ToDescription())
			{
				if (!valor)
					return this.Json(new { result = 0, Message = ModificarCategoria.MessageCategoria.ToDescription() + "  " + this.Description });
			}
				
			try
			{
				if (Accion == General.EditGeneral.ToDescription())
				{
					ViewBag.disabled = true;
					ViewBag.Combo = "display:block";
					ViewBag.Texto = "display:none";
					ViewBag.Mensaje = "Editar Registro GISIS";
					//ViewBag.ToolComite = false;
				}
				else
				{
					ViewBag.disabled = false;
					ViewBag.Combo = "display:none";
					ViewBag.Texto = "display:block";
					ViewBag.Mensaje = "Visualizar Registro GISIS";
					//ViewBag.ToolComite = true;
				}

				ViewBag.alta = "display:block";

				ViewBag.action = General.EditGeneral.ToDescription();

				this.ViewBag.navegador = this.Browser;

				GisisViewModel gisis = new GisisViewModel();

				this._serviciosGisis.GetAllGisis().Where(x => x.Id == id).ToList().ForEach(
				   p =>
				   {
					   gisis.Id             = p.Id;
					   gisis.Id_Puerto      = p.Id_Puerto;
					   gisis.Puerto         = p.Puerto;
					   gisis.Id_IIPP        = p.Id_IIPP;
					   gisis.IIPP           = p.IIPP;
					   gisis.Tipo_Registro  = p.Tipo_Registro;
					   gisis.Fecha_Registro = p.Fecha_Registro;
					   gisis.Registro       = p.Registro;                      
					   this.ViewBag.Observaciones = p.Motivo;
				   });


				this.comboPuertos(null);

				this.comboInstalacionesPuertos(null, gisis.Id_Puerto);

				
				
				return this.PartialView("~/Views/Gisis/_PartialGisis.cshtml", gisis);
			}
			catch (Exception ex)
			{
				this.log.PublishException(new SecureportException(ex));

				return this.Json(new { result = false, Message = ex.Message });
			}
		}
		/// <summary>
		/// Alta-Editar comites.
		/// </summary>
		[HttpPost]
		public ActionResult AltaEditarGisis(GisisJson gisisJson)
		{
			try
			{
				switch (gisisJson.action)
				{

					case "Alta":
						return AltaGisis(gisisJson);
						break;
					default:
						 EditarGisis(gisisJson);
						break;
				}

				return this.Json(new { result =1 });
			}
			catch (DbUpdateException ex)
			{
				this.log.PublishException(new SecureportExceptionDbUpdate(ex));

				return this.Json(new { result = false, Message = ex.Message });
			}
			catch (ModelValidationException ex)
			{
				return this.Json(new { result = false, Message = ex.Message });
			}
			catch (DbEntityValidationException ex)
			{
				this.log.PublishException(new SecureportExceptionDbEntity(ex));

				return this.Json(new { result = false, Message = ex.Message });
			}
			catch (Exception ex)
			{
				this.log.PublishException(new SecureportException(ex));

				return this.Json(new { result = false, Message = ex.Message });
			}

		}

		/// <summary>
		/// Validar Gisis.
		/// </summary>
		[HttpPost]
		public ActionResult ValidarGisis(int Id)
		{
			try
			{

				var retorno = ValidarCategoria(Id, this.repoGisis.Single(x => x.Id == Id)) ? EliminarCategoria.MessageCategoria.ToDescription() + "  " + this.Description : string.Empty;

				var valor = retorno == string.Empty;

				return this.Json(new { result = valor, Message = retorno });

			}
			catch (Exception ex)
			{
				this.log.PublishException(new SecureportException(ex));

				return this.Json(new { result = false, Message = ex.Message });
			}

		}


		/// <summary>
		/// Borrar Gisis.
		/// </summary>
		[HttpPost]
		public ActionResult EliminarGisis(int Id)
		{
			try
			{
				this.repoGisis.Delete(this.repoGisis.Single(x => x.Id == Id));

				this.trazas(216, (int)this.UsuarioFrontalSession.Usuario.id, "Se ha realizado la eliminación de un registro en SPMOV_Registro_GISIS con ID" + "(" + Id.ToString() + ")");

			}
			catch (DbUpdateException ex)
			{
				this.log.PublishException(new SecureportExceptionDbUpdate(ex));

				return this.Json(new { result = false, Message = MessageDelete(ex) });
			}
			catch (ModelValidationException ex)
			{
				return this.Json(new { result = false, Message = ex.Message });
			}
			catch (DbEntityValidationException ex)
			{
				this.log.PublishException(new SecureportExceptionDbEntity(ex));

				return this.Json(new { result = false, Message = ex.Message });
			}
			catch (Exception ex)
			{
				this.log.PublishException(new SecureportException(ex));

				return this.Json(new { result = false, Message = ex.Message });
			}

			return this.Json(new { result = true });
		}


		[HttpPost]
		public ActionResult AltaGisis(GisisJson gisisJson)
		{
			try{
				RegistroGisis gisis = new RegistroGisis();

				gisis.ID_Usu_alta = this.UsuarioFrontalSession.Usuario.id;

				gisis.Fech_Alta = DateTime.Now;

				gisis.Id_Puerto = gisisJson.Id_Puerto;

				gisis.Id_IIPP = gisisJson.Id_IIPP;

				gisis.Tipo_Registro = gisisJson.Tipo_Registro;

				gisis.Fech_Registro = gisisJson.Fecha_Registro;

				gisis.Motivo =  CambiarTexto( gisisJson.Motivo);

				this.repoGisis.Create(gisis);

				gisisJson.Id= this.db.Gisis.OrderByDescending(u => u.Id).FirstOrDefault().Id;

				this.trazas(214, (int)this.UsuarioFrontalSession.Usuario.id, "Se ha dado de alta un registro en SPMOV_Registro_GISIS con ID" + "(" + gisisJson.Id.ToString() + ")");
				
				IEnumerable<GisisViewModel> gisisEnv = _serviciosGisis.GetAllGisis().ToList().Where(x => x.Id == (int)gisisJson.Id);

				this.IdGisis = gisisJson.Id;

				if (EnviarCorreoLocal(gisisEnv.FirstOrDefault(), gisisJson.Motivo, "Alta"))
					return Json(new { status = "success", message = "customer created", result = 1 }); //return this.Json(new { result = true });
				else
					throw new Exception("No se puede enviar correo");
				

				//return this.Json(new { result = 1 });

			}
			catch (ModelValidationException ex)
			{
				return this.Json(new { result = false, Message = ex.Message });
			}
			catch (DbEntityValidationException ex)
			{
				this.log.PublishException(new SecureportExceptionDbEntity(ex));

				return this.Json(new { result = false, Message = ex.Message });
			}
			catch (Exception ex)
			{
				this.log.PublishException(new SecureportException(ex));

				return Json(new { status = "error", Message = ex.Message, result = 2 });				
			}

			
		}

		[HttpPost]
		public ActionResult EditarGisis(GisisJson gisisJson)
		{
			try
			{
				RegistroGisis gisis = this.repoGisis.Single(x => x.Id == gisisJson.Id);

				gisis.Id_Puerto = gisisJson.Id_Puerto;

				gisis.Id_IIPP = gisisJson.Id_IIPP;

				gisis.Tipo_Registro = gisisJson.Tipo_Registro;

				gisis.Fech_Registro = gisisJson.Fecha_Registro;

				gisis.Motivo = CambiarTexto(gisisJson.Motivo);
				
				this.repoGisis.Update(gisis);

				this.trazas(217, (int)this.UsuarioFrontalSession.Usuario.id, "Se ha realizado la modificación de un registro en SPMOV_Registro_GISIS con ID" + "(" + gisisJson.Id.ToString() + ")");

				this.IdGisis = gisisJson.Id;

				GisisViewModel gisisEnv = new GisisViewModel();

				return this.Json(new { result = 1, Message = "Edición Correcta" });
			 
				
			}
			catch (ModelValidationException ex)
			{
				return this.Json(new { result = 2, Message = ex.Message });
			}
			catch (DbEntityValidationException ex)
			{
				this.log.PublishException(new SecureportExceptionDbEntity(ex));

				return this.Json(new { result = 2, Message = ex.Message });
			}
			catch (Exception ex)
			{
				this.log.PublishException(new SecureportException(ex));

				//return Json(new { status = "error", Message = ex.Message, result = 2 });
				return this.Json(new { result = 2, Message = ex.Message });
			}

			
		}


		[HttpPost]
		public ActionResult CambiarPuerto(int? id)
		{
			try
			{

				this.comboInstalacionesPuertos(null, id);

				return this.PartialView("~/Views/Gisis/ComboPuerto.cshtml");

			}
			catch (Exception ex)
			{
				this.log.PublishException(new SecureportException(ex));

				return this.Json(new { result = false, Message = ex.Message });
			}

		}

	   
		public bool EnviarCorreoLocal(GisisViewModel gisis, string RegistroG, string Tipo)
		{
			try
			{
				string CorreoA = ConfigurationManager.AppSettings["CorreoGisis"];

			   
				string Texto = string.Empty;
				switch (Tipo)
				{
					case "Alta":
						Texto = "Se le notifica que ha sido dado de alta un Registro GISIS  con los siguientes datos: \r\n\r\n Puerto: " + gisis.Puerto + " " + (gisis.Id_IIPP == null ? string.Empty : "\r\n Instalación: " + gisis.IIPP) + " \r\n Tipo: " + gisis.Registro + " \r\n Registrado: " + ((DateTime)gisis.Fecha_Registro).ToShortDateString() + " \r\n";
						break;
					case "Modificar":
						Texto = "Se le notifica que ha sido modificado el Registro GISIS con los siguientes datos: \r\n\r\n Puerto: " + gisis.Puerto + " " + (gisis.Id_IIPP == null ? string.Empty : "\r\n Instalación: " + gisis.IIPP) + " \r\n Tipo: " + gisis.Registro + " \r\n Registrado: " + ((DateTime)gisis.Fecha_Registro).ToShortDateString() + " \r\n";
						break;
					case "Baja":
						Texto = "Se le notifica que ha sido eliminados el registro GISIS con los siguientes datos: \r\n\r\n Puerto: " + gisis.Puerto + " " + (gisis.Id_IIPP == null ? string.Empty : "\r\n Instalación: " + gisis.IIPP) + " \r\n Tipo: " + gisis.Registro + " \r\n Registrado: " + ((DateTime)gisis.Fecha_Registro).ToShortDateString() + " \r\n";
						break;
				}

				Texto += "\r\n Deberá comprobar las modificaciones realizadas.";
				string Asunto = "SecurePort – " + (Tipo == "Alta" ? "Alta" : (Tipo == "Modificar" ? "Modificación" : "Baja")) + " Datos Registro GISIS";
				//Todo Armando: Verificar esta cosa extraña-----
				throw new Exception("no se puede enviar correo");
				return this.EnviarCorreoPuertos(Asunto, Texto, CorreoA);
			}
			catch (Exception ex)
			{
				this.log.PublishException(new SecureportException(ex));
				return false;
			}
		}

		#endregion
		
	}
}