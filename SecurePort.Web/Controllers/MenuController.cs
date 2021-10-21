namespace SecurePort.Web.Controllers
{
	#region Using
	using SecurePort.Services.Repository;
	using System.Web.Mvc;
	#endregion
	
	public class MenuController : BaseController
	{

		public MenuController(ILogger log,TrazasRepository repotrazas)
			: base(log,repotrazas)
		{
		}

		public ActionResult Index()
		{

			if (this.UsuarioFrontalSession.Usuario != null)
			{
				ViewBag.Navegador = this.Browser;

				return View("BarraMenu", this.UsuarioFrontalSession);
			}
				return RedirectToAction("Index","Home");
		}

		public ActionResult Principal()
		{

			ViewBag.Navegador = this.Browser;

			return View("BarraMenu", this.UsuarioFrontalSession);
		}

		[HttpPost]
		public ActionResult Legal()
		{

			return PartialView("~/Views/Shared/PartialLegal.cshtml", this.UsuarioFrontalSession);

		}


		[HttpPost]
		public ActionResult LegalInicio()
		{

			return PartialView("~/Views/Shared/PartialLegalInicio.cshtml", this.UsuarioFrontalSession);

		}
	}

}