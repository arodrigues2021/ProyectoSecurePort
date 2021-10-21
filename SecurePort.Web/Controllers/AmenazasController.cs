
namespace SecurePort.Web.Views
{
	#region Using
	using System.Data.Entity.Infrastructure;
	using System.Data.Entity.ModelConfiguration;
	using System.Data.Entity.ModelConfiguration.Configuration;
	using System.Security.Cryptography.X509Certificates;

	using Kendo.Mvc.Extensions;
	using Kendo.Mvc.UI;

	using Microsoft.Ajax.Utilities;

	using SecurePort.Entities.Enums;
	using SecurePort.Entities.Exceptions;
	using SecurePort.Entities.Models;
	using SecurePort.Entities.Models.Json;
	using SecurePort.Services.Interfaces;
	using SecurePort.Services.Repository;
	using SecurePort.Web.Controllers;
	using System;
	using System.Collections.Generic;
	using System.Data.Entity.Validation;
	using System.Linq;
	using System.Web.Mvc;

	using WebGrease.Css.Extensions;

	using ListExtensions = WebGrease.Css.Extensions.ListExtensions;

	#endregion

	public class AmenazasController : BaseController
	{

		public AmenazasController(IServiciosAmenazas serviciosAmenazas, 
								  AmenazasRepository repoAmenazas,
								  IServiciosBienes serviciosBienes,
								  MOV_Detalle_InstalacionRepository repoMovDetalleInstalaciones,
								  TrazasRepository repoTrazas, 
								  ILogger log)
			: base(serviciosAmenazas, 
				   repoAmenazas, 
				   serviciosBienes,
				   repoMovDetalleInstalaciones,
				   repoTrazas, 
				   log)
		{
			 
		}
		#region Buscador
		public ActionResult AmenazasBienesPadreHijos([DataSourceRequest]DataSourceRequest request)
		{
			try
			{
				return this.Json(this.ListAmenazasPadreHijos.ToList().Where(x => x.IdpadreHijo == this.IdpadreHijo).OrderBy(x => x.Id).ToDataSourceResult(request));
			}
			catch (Exception ex)
			{
				this.log.PublishException(new SecureportException(ex));
				throw;
			}
		}

		/// <summary>
		/// Permite buscar los diferentes Amenzas
		/// </summary>
		public ActionResult BuscadorAmenazas([DataSourceRequest]DataSourceRequest request)
		{
			try
			{

				IEnumerable<AmenazasViewModel> result = this._serviciosAmenazas.GetAllAmenazas();

				return this.Json(result.ToDataSourceResult(request));
			}
			catch (Exception ex)
			{
				this.log.PublishException(new SecureportException(ex));
				throw;
			}
		}
		#endregion

		#region action

		[HttpPost]
		public ActionResult AgregarAmenazaPadreHijo(string idAmenazas, int idPadreHijo)
		{
			try
			{
				var AmenazasPadreHijos = this.ListAmenazasPadreHijos.ToList().Where(x => x.IdPH == idPadreHijo && x.Id == Convert.ToInt32(idAmenazas)).ToList();
				
				if (AmenazasPadreHijos.Any())
				{

					this.ListAmenazasPadreHijos.Where(x => x.IdPH == idPadreHijo && x.Id == Convert.ToInt32(idAmenazas)).ToList().ForEach(
						p =>
						{
							this.ListAmenazasPadreHijos.Remove(p);
						});
					

					ListAmenazasPadreHijos listAmenazasPadreHijos = new ListAmenazasPadreHijos();

					listAmenazasPadreHijos.IdpadreHijo = AmenazasPadreHijos[0].IdpadreHijo;

					listAmenazasPadreHijos.IdPH = AmenazasPadreHijos[0].IdPH;

					listAmenazasPadreHijos.Id = Convert.ToInt32(idAmenazas);

					listAmenazasPadreHijos.Descripcion = AmenazasPadreHijos[0].Descripcion;

					listAmenazasPadreHijos.activado = "Si";

					this.ListAmenazasPadreHijos.Add(listAmenazasPadreHijos);

					this.IdpadreHijo = AmenazasPadreHijos[0].IdpadreHijo;
				}
				return this.PartialView("~/Views/Evaluaciones/_PartialAmenazas.cshtml");
			}
			catch (Exception ex)
			{
				this.log.PublishException(new SecureportException(ex));
				throw;
			}
		}

		[HttpPost]
		public ActionResult QuitarAmenazaPadreHijo(string idAmenazas, int idPadreHijo)
		{
			try
			{
				var AmenazasPadreHijos = this.ListAmenazasPadreHijos.ToList().Where(x => x.IdPH == idPadreHijo && x.Id == Convert.ToInt32(idAmenazas)).ToList();

				if (AmenazasPadreHijos.Any())
				{

					this.ListAmenazasPadreHijos.Where(x => x.IdPH == idPadreHijo && x.Id == Convert.ToInt32(idAmenazas)).ToList().ForEach(
						p =>
						{
							this.ListAmenazasPadreHijos.Remove(p);
						});


					ListAmenazasPadreHijos listAmenazasPadreHijos = new ListAmenazasPadreHijos();

					listAmenazasPadreHijos.IdpadreHijo     = AmenazasPadreHijos[0].IdpadreHijo;

					listAmenazasPadreHijos.IdPH            = AmenazasPadreHijos[0].IdPH;

					listAmenazasPadreHijos.Id              = Convert.ToInt32(idAmenazas);

					listAmenazasPadreHijos.Descripcion     = AmenazasPadreHijos[0].Descripcion;

					listAmenazasPadreHijos.activado        = "Quitar";

					this.ListAmenazasPadreHijos.Add(listAmenazasPadreHijos);

					this.IdpadreHijo = AmenazasPadreHijos[0].IdpadreHijo;
				}

				return this.PartialView("~/Views/Evaluaciones/_PartialAmenazas.cshtml");
			}
			catch (Exception ex)
			{
				this.log.PublishException(new SecureportException(ex));
				throw;
			}
		}

		[HttpPost]
		public ActionResult AgregarPadreHijo(string BienesPadreHijo)
		{
			try
			{

				string[] CodeBienPadre = BienesPadreHijo.Split(',');

				string[] CodeBienHijo = CodeBienPadre[0].Split('-');

				string BienHijo = CodeBienHijo[1];

				string BienPadre = CodeBienPadre[0];

				this.IdpadreHijo = BienPadre;

				var code = Convert.ToInt32(CodeBienHijo[0]);

				this.ListBienesPadres.Where(x => x.Id == code).ToList().ForEach(
					b =>
					{
						this.ListBienesPadres.Remove(b);
						BienesViewModel listAmena  = new BienesViewModel();
						listAmena.Id               = b.Id;
						listAmena.Descripcion      = b.Descripcion;
						listAmena.id_Tipo_IIPP     = b.id_Tipo_IIPP;
						listAmena.id_Bien_Padre    = b.id_Bien_Padre;
						listAmena.es_activo        = b.es_activo;
						listAmena.Bien_Padre       = b.Bien_Padre;
						listAmena.Tipo_instalacion = b.Tipo_instalacion;
						listAmena.activado         = "Quitar";
						listAmena.sel              = b.sel;
						this.ListBienesPadres.Add(listAmena);
						this.Idpadre               = null;
					});
				
				this.ListAmenazasPadreHijos.Where(x => x.IdPH == code).ToList().ForEach(
					p =>
					{
						this.ListAmenazasPadreHijos.Remove(p);
					});

				if (!this.ListAmenazasPadreHijos.ToList().Where(x => x.IdpadreHijo == BienPadre).Any())
				{
					IEnumerable<AmenazasViewModel> result = this._serviciosAmenazas.GetAllAmenazasPadreHijos().Where(x => x.es_activo);

					result.ToList().ForEach(
						b =>
						{

							ListAmenazasPadreHijos listAmenazasPadreHijos = new ListAmenazasPadreHijos();

							listAmenazasPadreHijos.IdpadreHijo = BienPadre;

							listAmenazasPadreHijos.Id          = b.Id;

							listAmenazasPadreHijos.IdPH        = Convert.ToInt32(CodeBienHijo[0] + CodeBienHijo[1]);

							listAmenazasPadreHijos.Descripcion = b.Descripcion;

							listAmenazasPadreHijos.activado    = "No";

							this.ListAmenazasPadreHijos.Add(listAmenazasPadreHijos);
						
						});
				}

				ViewBag.Nombre = "Amenazas:" +string.Empty + this._serviciosBienes.ListBienes().Where(x => x.Id == Convert.ToInt32(BienHijo)).ToList().FirstOrDefault().Descripcion;

				this.IdpadreHijo = BienesPadreHijo;

				return this.PartialView("~/Views/Evaluaciones/_Partial2BienPadreHijo.cshtml");
			}
			catch (Exception ex)
			{
				this.log.PublishException(new SecureportException(ex));
				throw;
			}
		}

		[HttpPost]
		public ActionResult AgregarPadre(string BienesPadre)
		{
			try
			{

				this.IdpadreHijo = BienesPadre;
				
				ListBienHijos.ToList().Where(x => x.id_Bien_Padre == Convert.ToInt32(BienesPadre)).ForEach(
					b =>
					{
						if (b.activado == "Si")
						{
							ListBienHijos.Remove(b);    
							BienesHijosViewModel listAmena = new BienesHijosViewModel();
							listAmena.Id                   = b.Id;
							listAmena.Descripcion          = b.Descripcion;
							listAmena.id_Tipo_IIPP         = b.id_Tipo_IIPP;
							listAmena.id_Bien_Padre        = b.id_Bien_Padre;
							listAmena.es_activo            = b.es_activo;
							listAmena.Bien_Padre           = b.Bien_Padre;
							listAmena.Tipo_instalacion     = b.Tipo_instalacion;
							listAmena.activado             = "Quitar";
							listAmena.sel                  = b.sel;
							ListBienHijos.Add(listAmena);

						}
							
					});
				
				ViewBag.Nombre = "Amenazas:" + string.Empty + this._serviciosBienes.ListBienes()
																		.Where(x => x.Id == Convert.ToInt32(this.IdpadreHijo))
																			 .ToList().FirstOrDefault().Descripcion;
				this.Idpadre = BienesPadre;

				this.IdpadreHijo = null;

				return this.PartialView("~/Views/Evaluaciones/_Partial2BienPadreHijo.cshtml");
			}
			catch (Exception ex)
			{
				this.log.PublishException(new SecureportException(ex));
				throw;
			}
		}

		[HttpPost]
		public ActionResult ListHijo(string BienesPadreHijo, string nombre)
		{
			try
			{
				this.IdpadreHijo = BienesPadreHijo;

				var listaHijos = this.ListAmenazasPadreHijos.ToList().Where(x => x.IdpadreHijo == this.IdpadreHijo).OrderBy(x => x.Id);

				if (this.IdpadreHijo != null)
				{
					string[] codeHijo = this.IdpadreHijo.Split('-');

					if (!listaHijos.ToList().Any() && codeHijo.Count() > 0)
					{
						int code = Convert.ToInt32(codeHijo[0] + codeHijo[1]);

						List<MOV_Detalle_Instalacion> amenazas =
							this.repoMovDetalleInstalaciones.GetAll().Where(x => x.ID_Instalacion == this.idInstalacion && x.ID_Bien == Convert.ToInt32(codeHijo[1])).ToList();

						amenazas.ToList().ForEach(
							b =>
							{
							    if (b.ID_Amenaza!=null)
							    {
							        if (!this.ListAmenazasPadreHijos.Where(x => x.IdPH == code && x.Id == Convert.ToInt32(b.ID_Amenaza)).Any())
							        {
							            ListAmenazasPadreHijos listAmenazasPadreHijos = new ListAmenazasPadreHijos();

							            listAmenazasPadreHijos.IdpadreHijo = this.IdpadreHijo;

							            listAmenazasPadreHijos.Id = Convert.ToInt32(b.ID_Amenaza);

							            listAmenazasPadreHijos.IdPH = code;

							            listAmenazasPadreHijos.Descripcion = this._serviciosAmenazas.GetAllAmenazas().Where(x => x.Id == b.ID_Amenaza).ToList().FirstOrDefault().Descripcion;

							            listAmenazasPadreHijos.activado = "Si";

							            this.ListAmenazasPadreHijos.Add(listAmenazasPadreHijos);
							        }
							    }
								
							});
						IEnumerable<AmenazasViewModel> result = this._serviciosAmenazas.GetAllAmenazasPadreHijos().Where(x => x.es_activo);

						result.ToList().ForEach(
							b =>
							{
								if (!this.ListAmenazasPadreHijos.Where(x => x.Id == b.Id && x.IdpadreHijo == this.IdpadreHijo).Any())
								{
									ListAmenazasPadreHijos listAmenazasPadreHijos = new ListAmenazasPadreHijos();

									listAmenazasPadreHijos.IdpadreHijo = this.IdpadreHijo;

									listAmenazasPadreHijos.Id = b.Id;

									listAmenazasPadreHijos.IdPH = Convert.ToInt32(codeHijo[0] + codeHijo[1]);

									listAmenazasPadreHijos.Descripcion = b.Descripcion;

									listAmenazasPadreHijos.activado = "No";

									this.ListAmenazasPadreHijos.Add(listAmenazasPadreHijos);
								}
							});
					}
					else
					{
						IEnumerable<AmenazasViewModel> result = this._serviciosAmenazas.GetAllAmenazasPadreHijos().Where(x => x.es_activo);

						result.ToList().ForEach(
							b =>
							{
								if (!this.ListAmenazasPadreHijos.Where(x => x.Id == b.Id && x.IdpadreHijo == this.IdpadreHijo).Any())
								{
									ListAmenazasPadreHijos listAmenazasPadreHijos = new ListAmenazasPadreHijos();

									listAmenazasPadreHijos.IdpadreHijo = this.IdpadreHijo;

									listAmenazasPadreHijos.Id = b.Id;

									listAmenazasPadreHijos.IdPH = Convert.ToInt32(codeHijo[0] + codeHijo[1]);

									listAmenazasPadreHijos.Descripcion = b.Descripcion;

									listAmenazasPadreHijos.activado = "No";

									this.ListAmenazasPadreHijos.Add(listAmenazasPadreHijos);
								}
							});
					}
				}

				if (this.Idpadre != null)
				{

					string[] codeHijo = this.IdpadreHijo.Split('-');

					int code = Convert.ToInt32(codeHijo[0] + codeHijo[1]);

					var listarHijos = this.ListAmenazasPadreHijos.Where(x => x.IdPH == code && x.activado == "Si").ToList();

						 listarHijos.ToList().ForEach(
							 b =>
							 {

								 ListAmenazasPadreHijos listAmenazasPadreHijos = new ListAmenazasPadreHijos();

								 listAmenazasPadreHijos.IdpadreHijo = this.IdpadreHijo;

								 listAmenazasPadreHijos.Id = b.Id;

								 listAmenazasPadreHijos.IdPH = code;

								 listAmenazasPadreHijos.Descripcion = b.Descripcion;

								 listAmenazasPadreHijos.activado = "Quitar";

								 this.ListAmenazasPadreHijos.Remove(b);

								 this.ListAmenazasPadreHijos.Add(listAmenazasPadreHijos);

							 });

					}
				
				return this.PartialView("~/Views/Evaluaciones/_PartialAmenazas.cshtml");
			}
			catch (Exception ex)
			{
				this.log.PublishException(new SecureportException(ex));
				throw;
			}
		}

		[HttpPost]
		public ActionResult ListPadreHijo(string BienesPadreHijo, string nombre, string SelBienPadre)
		{
			try
			{
				this.IdpadreHijo = BienesPadreHijo;

				if (SelBienPadre=="Si")
				{
					List<MOV_Detalle_Instalacion> amenazas =
						this.repoMovDetalleInstalaciones.GetAll().Where(x => x.ID_Instalacion == this.idInstalacion && x.ID_Bien == Convert.ToInt32(BienesPadreHijo)).ToList();

					amenazas.ToList().ForEach(
						b =>
						{
							this.ListAmenazasPadreHijos.Where(x => x.IdpadreHijo == b.ID_Bien.ToString() && x.Id == Convert.ToInt32(b.ID_Amenaza)).ToList().ForEach(
								p =>
								{
									string desc = p.Descripcion;

									string PadreHijo = p.IdpadreHijo;

									int Id_Amenaza = p.Id;

									int IdPH = p.IdPH;

									this.ListAmenazasPadreHijos.Remove(p);

									ListAmenazasPadreHijos listAmenazasPadreHijos = new ListAmenazasPadreHijos();

									listAmenazasPadreHijos.IdpadreHijo = PadreHijo;

									listAmenazasPadreHijos.Id = Id_Amenaza;

									listAmenazasPadreHijos.IdPH = IdPH;

									listAmenazasPadreHijos.Descripcion = desc;

									listAmenazasPadreHijos.activado = "Si";

									this.ListAmenazasPadreHijos.Add(listAmenazasPadreHijos);
								});
						});
				}
				else
				{
					List<MOV_Detalle_Instalacion> amenazas =
						this.repoMovDetalleInstalaciones.GetAll().Where(x => x.ID_Instalacion == this.idInstalacion && x.ID_Bien == Convert.ToInt32(BienesPadreHijo)).ToList();

					amenazas.ToList().ForEach(
						b =>
						{
							this.ListAmenazasPadreHijos.Where(x => x.IdpadreHijo == b.ID_Bien.ToString() && x.Id == Convert.ToInt32(b.ID_Amenaza)).ToList().ForEach(
								p =>
								{
									string desc = p.Descripcion;

									string PadreHijo = p.IdpadreHijo;

									int Id_Amenaza = p.Id;

									int IdPH = p.IdPH;

									this.ListAmenazasPadreHijos.Remove(p);

									ListAmenazasPadreHijos listAmenazasPadreHijos = new ListAmenazasPadreHijos();

									listAmenazasPadreHijos.IdpadreHijo = PadreHijo;

									listAmenazasPadreHijos.Id = Id_Amenaza;

									listAmenazasPadreHijos.IdPH = IdPH;

									listAmenazasPadreHijos.Descripcion = desc;

									listAmenazasPadreHijos.activado = "Quitar";

									this.ListAmenazasPadreHijos.Add(listAmenazasPadreHijos);
								});
						});
				}

				var listarhijosQuitar = ListBienHijos.ToList().Where(x => x.id_Bien_Padre == Convert.ToInt32(this.IdpadreHijo) && x.activado == "Quitar");

				listarhijosQuitar.ToList().ForEach(
					b =>
					{

						var code = Convert.ToInt32(b.id_Bien_Padre + b.Id);

						this.ListAmenazasPadreHijos.Where(x => x.IdPH == code && x.activado == "Si").ToList().ForEach(
							x =>
							{

								ListAmenazasPadreHijos listAmenazasPadreHijos = new ListAmenazasPadreHijos();

								listAmenazasPadreHijos.IdpadreHijo = this.IdpadreHijo;

								listAmenazasPadreHijos.Id = b.Id;

								listAmenazasPadreHijos.IdPH = code;

								listAmenazasPadreHijos.Descripcion = b.Descripcion;

								listAmenazasPadreHijos.activado = "Quitar";

								this.ListAmenazasPadreHijos.Remove(x);

								this.ListAmenazasPadreHijos.Add(listAmenazasPadreHijos);

							});

					});
				
				return this.PartialView("~/Views/Evaluaciones/_PartialAmenazas.cshtml");
			}
			catch (Exception ex)
			{
				this.log.PublishException(new SecureportException(ex));
				throw;
			}
		}

		[HttpPost]
		public ActionResult QuitarPadreHijo(string BienesPadre)
		{
			try
			{
				this.IdpadreHijo = BienesPadre;

				this.ListAmenazasPadreHijos.Where(x => x.IdpadreHijo == BienesPadre).ToList().ForEach(
					p =>
					{
						this.ListAmenazasPadreHijos.Remove(p);
					});

				this.ListBienesPadres.Where(x => x.Id == Convert.ToInt32(BienesPadre)).ToList().ForEach(
					b =>
					{
						this.ListBienesPadres.Remove(b);
						BienesViewModel listAmena  = new BienesViewModel();
						listAmena.Id               = b.Id;
						listAmena.Descripcion      = b.Descripcion;
						listAmena.id_Tipo_IIPP     = b.id_Tipo_IIPP;
						listAmena.id_Bien_Padre    = b.id_Bien_Padre;
						listAmena.es_activo        = b.es_activo;
						listAmena.Bien_Padre       = b.Bien_Padre;
						listAmena.Tipo_instalacion = b.Tipo_instalacion;
						listAmena.activado         = "Quitar";
						listAmena.sel              = b.sel;
						this.ListBienesPadres.Add(listAmena);
					});

				return this.PartialView("~/Views/Evaluaciones/_PartialAmenazas.cshtml");
			}
			catch (Exception ex)
			{
				this.log.PublishException(new SecureportException(ex));
				throw;
			}
		}

		[HttpPost]
		public ActionResult QuitarHijo(string BienesPadre)
		{
			try
			{
				string[] codePadreHijo = BienesPadre.Split('-');

				this.IdpadreHijo = BienesPadre;

				//List<ListAmenazasPadreHijos> Amenazas = this.ListAmenazasPadreHijos.Where(x => x.IdPH == Convert.ToInt32(codePadreHijo[0] + codePadreHijo[1]) && x.activado=="Si").ToList();

				this.ListAmenazasPadreHijos.ToList().Where(x => x.IdPH == Convert.ToInt32(codePadreHijo[0] + codePadreHijo[1]) && x.activado == "Si").ForEach(
							s =>
							{
								this.ListAmenazasPadreHijos.Remove(s);
							});

				this.ListAmenazasPadreHijos.Where(x => x.IdPH == Convert.ToInt32(codePadreHijo[0] + codePadreHijo[1]) && x.activado == "Si").ToList().ToList().ForEach(
					p =>
					{

						ListAmenazasPadreHijos listAmenazasPadreHijos = new ListAmenazasPadreHijos();

						listAmenazasPadreHijos.IdpadreHijo = p.IdpadreHijo;

						listAmenazasPadreHijos.IdPH = p.IdPH;

						listAmenazasPadreHijos.Id = Convert.ToInt32(p.Id);

						listAmenazasPadreHijos.Descripcion = p.Descripcion;

						listAmenazasPadreHijos.activado = "Quitar";

						this.ListAmenazasPadreHijos.Add(listAmenazasPadreHijos);
					});

				int CodePadre = Convert.ToInt32(codePadreHijo[0]);

				int CodeHijo = Convert.ToInt32(codePadreHijo[1]);

				this.ListBienHijos.Where(x => x.Id == CodeHijo && x.id_Bien_Padre == CodePadre).ToList().ForEach(
					b =>
					{
						this.ListBienHijos.Remove(b);
						BienesHijosViewModel listAmena = new BienesHijosViewModel();
						listAmena.Id                = b.Id;
						listAmena.Descripcion       = b.Descripcion;
						listAmena.id_Tipo_IIPP      = b.id_Tipo_IIPP;
						listAmena.id_Bien_Padre     = b.id_Bien_Padre;
						listAmena.es_activo         = b.es_activo;
						listAmena.Bien_Padre        = b.Bien_Padre;
						listAmena.Tipo_instalacion  = b.Tipo_instalacion;
						listAmena.activado          = "Quitar";
						listAmena.sel               = b.sel;
						this.ListBienHijos.Add(listAmena);

					});
	   
				return this.PartialView("~/Views/Evaluaciones/_PartialAmenazas.cshtml");
			}
			catch (Exception ex)
			{
				this.log.PublishException(new SecureportException(ex));
				throw;
			}
		}
		/// <summary>
		/// Retorna una lista de todos los Bienes.
		/// </summary>
		public ActionResult ListadoAmenazas()
		{
			try
			{
				return this.PartialView("ListadoAmenazas", this.UsuarioFrontalSession);
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
		public ActionResult Create()
		{

			Amenazas amenaza = new Amenazas();

			ViewBag.disabled = true;

			ViewBag.Mensaje = "Alta Amenazas";

			ViewBag.action = General.AltaGeneral.ToDescription();

			amenaza.Id = 1;

			return this.PartialView("~/Views/Amenazas/_PartialAmenazas.cshtml", amenaza);
		}
		/// <summary>
		/// Visualizar los datos de los Amenazas
		/// </summary>
		[HttpPost]
		public ActionResult VisualizarAmenazas(int id)
		{
			try
			{

				ViewBag.disabled = true;

				ViewBag.Mensaje = "Modificar Amenazas";

				ViewBag.action = General.EditGeneral.ToDescription();

				Amenazas amenazas = new Amenazas();

				this._serviciosAmenazas.GetAllAmenazas().Where(x => x.Id == id).ToList().ForEach(
				   p =>
				   {
					   amenazas.Id = p.Id;
					   amenazas.Descripcion = p.Descripcion;
					   amenazas.es_activo = p.es_activo;

				   });

				return this.PartialView("~/Views/Amenazas/_PartialAmenazas.cshtml", amenazas);
			}
			catch (Exception ex)
			{
				this.log.PublishException(new SecureportException(ex));

				return this.Json(new { result = false, Message = ex.Message });
			}
		}
		/// <summary>
		/// Alta-Editar Amenazas.
		/// </summary>
		[HttpPost]
		public ActionResult AltaEditarAmenazas(AmenazasJson amenazasJson)
		{
			try
			{
				IEnumerable<Amenazas> result = null;

				switch (amenazasJson.action)
				{
						 
					case "Alta":
						result = _serviciosAmenazas.ListAmenazas(amenazasJson.Descripcion);
						bool resultado = false;
						foreach (Amenazas amenaza in result){
						   if(amenaza.es_activo == true)
								resultado = true;                        
						}
						if (!resultado)
						{
							if(result.Count() ==0)
								AltaAmenaza(amenazasJson);
							else
								return this.Json(new { result = 2, Message = "Error al dar de alta la amenaza, ya existe otra amenaza activa " + amenazasJson.Descripcion + "¿Desea continuar? " });                            
						} else {
							this.trazas(72, (int)this.UsuarioFrontalSession.Usuario.id, "Error al dar de alta la amenaza, ya existe otra amenaza activa " + amenazasJson.Descripcion);
							return this.Json(new { result = 1, Message = "Error al dar de alta la amenaza, ya existe otra amenaza activa " + amenazasJson.Descripcion });
						}
						break;

					default:
						 bool CambioEstado = _serviciosAmenazas.ListAmenazas(amenazasJson.Descripcion, amenazasJson.Id, (amenazasJson.activo == "1" ? true : false));
						if (CambioEstado)
						{
							EditarAmenaza(amenazasJson);
						}
						else
						{
							result = _serviciosAmenazas.ListAmenazas(amenazasJson.Descripcion, amenazasJson.Id);
							if (result.Count() == 0)
							{
								EditarAmenaza(amenazasJson);
							}
							else
							{
								IEnumerable<Amenazas> amenaza = result.Where(x => x.Id != amenazasJson.Id);                                
								if (amenaza == null)
								{
									EditarAmenaza(amenazasJson);
								}
								else{
									Amenazas ActAmenaza = amenaza.FirstOrDefault();
									if (ActAmenaza.Id == amenazasJson.Id)
									{
										return this.Json(new { result = 3, Message = "Ya existe una Amenaza  " + amenazasJson.Descripcion + " desactivado ¿desea continuar? " });
									}
									else
									{
										if (!ActAmenaza.es_activo)
											return this.Json(new { result = 3, Message = "Ya existe una Amenaza " + amenazasJson.Descripcion + " desactivado ¿desea continuar? " });
										else
										{
											this.trazas(74, (int)this.UsuarioFrontalSession.Usuario.id, "Error al modificar la amenaza, ya existe otra amenaza activa " + amenazasJson.Descripcion);
											return this.Json(new { result = 1, Message = "Error al modificar la amenaza, ya existe otra amenaza activa  " + amenazasJson.Descripcion });
										}
									}
								}                               

							}
						}
						break;
				}

				return this.Json(new { result = 0 });
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
		/// Borrar Amenazas.
		/// </summary>
		[HttpPost]
		public ActionResult EliminarAmenazas(int Id)
		{
			try
			{
				this.repoAmenazas.Delete(this.repoAmenazas.Single(x => x.Id == Id));

				this.trazas(73, (int)this.UsuarioFrontalSession.Usuario.id, "Se ha realizado la eliminación de un registro en SPMAE_Amenazas con ID" + "(" + Id.ToString() + ")");

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
		public ActionResult AltaAmenaza(AmenazasJson amenazasJson)
		{
			try
			{
				Amenazas amenazas = new Amenazas();

				amenazas.ID_Usu_alta = this.UsuarioFrontalSession.Usuario.id;

				amenazas.es_activo = true;

				amenazas.fech_activo = DateTime.Now;

				amenazas.Fech_Alta = DateTime.Now;

				amenazas.Descripcion = amenazasJson.Descripcion;

				this.repoAmenazas.Create(amenazas);

				var Id = this.db.Amenazas.OrderByDescending(u => u.Id).FirstOrDefault().Id;

				this.trazas(72, (int)this.UsuarioFrontalSession.Usuario.id, "Se ha dado de alta un registro en SPMAE_Amenazas con ID" + "(" + Id.ToString() + ")");

				return this.Json(new { result = 0 });
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

		[HttpPost]
		public ActionResult EditarAmenaza(AmenazasJson amenazasJson)
		{
			try
			{
				Amenazas amenaza = this.repoAmenazas.Single(x => x.Id == amenazasJson.Id);

				amenaza.Descripcion = amenazasJson.Descripcion;

				amenaza.es_activo = (amenazasJson.activo == "1");

				this.repoAmenazas.Update(amenaza);

				this.trazas(74, (int)this.UsuarioFrontalSession.Usuario.id, "Se ha realizado la modificación de un registro en SPMAE_Amenazas con ID" + "(" + amenazasJson.Id +")");

				return this.Json(new { result = 0 });
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

		#endregion
	}
}