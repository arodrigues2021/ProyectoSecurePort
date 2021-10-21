namespace SecurePort.Web.Views
{
	#region Using
	using System.Web.Mvc;
	using SecurePort.Services.Interfaces;
	using SecurePort.Web.Controllers;
	using System.Data.Entity;
	using System.Net;
	using System.Linq;
	using JQueryDataTables.Models;
	using System.Collections.Generic;
	using System;
	using SecurePort.Entities.Exceptions;
	using SecurePort.Entities.Models;
	using System.Data.Entity.Validation;
	using System.Data.Entity.Infrastructure;
	using System.Data.Entity.ModelConfiguration;
	using SecurePort.Entities.Models.Json;
	#endregion

	public class CNAEController : BaseController
	{
		
		public CNAEController(IServiciosCnae serviciosCnae,ILogger log) 
			:base(serviciosCnae,log)
		{}

		[HttpGet]
		[OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
		public ActionResult Buscador(JQueryDataTableParamModel param)
		{
			try
			{
				IEnumerable<CNAE> listaCNAE = _serviciosCnae.GetAllCNAE().OrderBy(c => c.Id).ToList();

				if (!string.IsNullOrEmpty(param.sSearch))
				{
					var isBienSearchable = Convert.ToBoolean(Request["bSearchable_1"]);

					var isDescripcionSearchable = Convert.ToBoolean(Request["bSearchable_2"]);

					var filteredBienes = listaCNAE
									 .Where(c => isBienSearchable && Convert.ToString(c.IdCodigo).ToLower().Contains(param.sSearch.ToLower())
											||
											isDescripcionSearchable && c.Descripcion.ToLower().Contains(param.sSearch.ToLower()));

					listaCNAE = filteredBienes;
				}

				var isDescripcionSortable = Convert.ToBoolean(Request["bSortable_2"]);

				Func<CNAE, string> orderingFunction = (c => isDescripcionSortable ? c.Descripcion : "");

				var sortDirection = Request["sSortDir_0"]; // asc or desc

				IEnumerable<CNAE> resultlistaBienes = sortDirection == "asc"
									? listaCNAE.OrderBy(orderingFunction)
									: listaCNAE.OrderByDescending(orderingFunction);

				var result = from c in resultlistaBienes select new[] { c.IdCodigo, c.Descripcion, Convert.ToString(c.Id) };

				result = result.Skip(param.iDisplayStart).Take(param.iDisplayLength).ToList();

				return Json(new
				{
					sEcho = param.sEcho,
					iTotalRecords = (listaCNAE.Any() ? listaCNAE.Count() : 0),
					iTotalDisplayRecords = (listaCNAE.Any() ? listaCNAE.Count() : 0),
					aaData = result
				}, JsonRequestBehavior.AllowGet);

			}
			catch (Exception ex)
			{
				this.log.PublishException(new SecureportException(ex));
				throw;
			}
		}
		
		[OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
		public ActionResult Index()
		{

			return View(_serviciosCnae.GetAllCNAE().ToList());
		}

		public ActionResult ListadoCNAE()
		{
			try
			{
				this.ViewData["usuario"] = this.UsuarioSession;
				return this.View();
			}
			catch (Exception ex)
			{
				this.log.PublishException(new SecureportException(ex));
				throw;
			}
		}
		
		[HttpPost]
		public ActionResult Edit(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			CNAE cnae = this.db.CNAE.Find(id);

			if (cnae == null)
			{
				return this.View("~/Views/ErrorPages/ErrorPage400.cshtml");
			}

			return this.PartialView("~/Views/CNAE/PlantillaCNAE.cshtml", cnae);
		}

		[HttpPost]
		public ActionResult EditCNAE(CNAEJson CNAEJson)
		{
			try
			{
				CNAE cnae = this.db.CNAE.Find(CNAEJson.Id);
				cnae.IdCodigo = CNAEJson.IdCodigo;
				cnae.Descripcion = CNAEJson.Descripcion;
				this.db.Entry(cnae).State = EntityState.Modified;
				this.db.SaveChanges();
				this.log.WriteInfo("Editar CNAE", cnae.IdCodigo + '|' + cnae.Descripcion + '|' + cnae.Id );

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

			return this.Json(new { result = true });
		}
		
		[HttpPost]
		public ActionResult DeleteConfirmed(int id)
		{
			try
			{
				CNAE CNAE = db.CNAE.Find(id);

				db.CNAE.Remove(CNAE);
				
				db.SaveChanges();

				this.log.WriteInfo("Eliminar CNAE", CNAE.Descripcion + '|' + CNAE.Id );
			}
			catch (DbEntityValidationException ex)
			{
				this.log.PublishException(new SecureportExceptionDbEntity(ex));
			 
				return this.Json(new { result = false, Message = ex.Message });
			}
			return this.Json(new { result = true });
		}

		[HttpPost]
		public ActionResult AltaCNAE(CNAEJson CNAEJson)
		{
			try
			{

				bool existe = this._serviciosCnae.GetCNAE(CNAEJson.IdCodigo);

				if (existe == false){
	
					CNAE cnae = new CNAE()
					{
						IdCodigo    = CNAEJson.IdCodigo,
						Descripcion = CNAEJson.Descripcion
					};

					this.db.CNAE.Add(cnae);

					this.db.SaveChanges();

					this.log.WriteInfo("Alta CNAE", CNAEJson.Id + '|' + CNAEJson.IdCodigo + '|' + CNAEJson.Descripcion);

				}
				else
				{
					return this.Json(new { result = false, Message = "Verifique el Código CNAE ya existe." });
				}
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

			return this.Json(new { result = true, Message = "Alta de CNAE exitosa." });
		}

	}
}
