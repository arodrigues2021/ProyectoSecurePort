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
    using System.IO;
	using System.Configuration;
    using System.Collections;
	#endregion

	public class CentrosController :  BaseController
	{
		public CentrosController(IServiciosCentros serviciosCentros,
								 IServiciosPuertos servicioPuertos,
								 CentrosRepository repoCentros, 
								 IServiciosOrganismo servicioOrganismos,                                 
								 PuertosRepository repoPuertos,
								 Comunica_CentroRepository repoComunica_Centro,
                                 ProvinciasRepository repoProvincias,
                                 CiudadesRepository repoCiudades,
								 TrazasRepository  repoTrazas, 
								 ILogger log)
			: base(serviciosCentros,
				   servicioPuertos,
				   repoCentros,
				   servicioOrganismos,
				   repoPuertos,
				   repoComunica_Centro,
                   repoProvincias,
                   repoCiudades,
				   repoTrazas,
				   log)
		{
		}

		#region Buscador
		/// <summary>
		/// Permite buscar Centros 24h
		/// </summary>
		public ActionResult BuscadorCentros([DataSourceRequest]DataSourceRequest request)
		{
			try
			{

				if (this.UsuarioFrontalSession.Usuario.categoria == (int)EnumCategoria.Administrador || this.UsuarioFrontalSession.Usuario.categoria == (int)EnumCategoria.PuertosdelEstado)
				{
					return this.Json(this._serviciosCentros.ListAllCentros().ToDataSourceResult(request));
				}

				return this.Json(this._serviciosCentros.ListAllCentros(this.UsuarioFrontalSession.ListDependenciasDepenUsuarios).ToDataSourceResult(request));
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
		/// Retorna una lista de todos los Centros 24h.
		/// </summary>
		public ActionResult ListadoCentros()
		{
			try
			{
                ViewBag.Mensaje = "Centros 24H";
				return this.PartialView("ListadoCentros", this.UsuarioFrontalSession);
			}
			catch (Exception ex)
			{
				this.log.PublishException(new SecureportException(ex));
				throw;
			}
		}
		#endregion

		#region CRUD

		#region Centros
		/// <summary>
		/// Permite el Alta del Centro24.
		/// </summary>
		[HttpPost]
		public ActionResult Create()
		{
			Centros centros = new Centros();

            ComboProvincias(null);

            ComboCiudades(0, null);

			ViewBag.disabled = true;

			ViewBag.Mensaje = "Alta Centros 24H";

			ViewBag.action = General.AltaGeneral.ToDescription();           

			centros.id = 1;

			this.comboOrganismos(null);

            if (((MultiSelectList)(this.ViewData["organismos"])).Count() == 1)
                centros.Id_Organismo = Convert.ToInt32(((SelectList)this.ViewData["organismos"]).FirstOrDefault().Value);

			this.comboPuertos(null);

            if (((MultiSelectList)(this.ViewData["puertos"])).Count() == 1)
                centros.Id_Puerto = Convert.ToInt32(((SelectList)this.ViewData["puertos"]).FirstOrDefault().Value);

			return this.PartialView("~/Views/Centros/_PartialCentros.cshtml", centros);
		}
		/// <summary>
		/// Visualizar los datos de los Centros 24H
		/// </summary>
		[HttpPost]
		public ActionResult VisualizarCentros(int? id, string Accion)
		{
			try
			{
                if (id != null)
                    this.IdCentro = (int)id;
                else if (this.IdCentro != 0)
                    id = this.IdCentro;


				if (Accion == General.EditGeneral.ToDescription())
				{
					ViewBag.disabled = true;

					ViewBag.Mensaje = "Modificar Centros 24H";

					ViewBag.ToolCentro = false;

					ViewBag.Combo_ad = "display:block";

					ViewBag.Texto_ad = "display:none";


				}
				else
				{
					ViewBag.disabled = false;

					ViewBag.Mensaje = "Visualizar Centros 24H";

					ViewBag.ToolCentro = true;

					ViewBag.Combo_ad = "display:none";

					ViewBag.Texto_ad = "display:block";
				
				}

				@ViewBag.alta = "display:block";
                
				ViewBag.action = General.EditGeneral.ToDescription();

				CentrosViewModel centros = new CentrosViewModel();

				this._serviciosCentros.ListAllCentros().Where(x => x.id == id).ToList().ForEach(
				   p =>
				   {
					   centros.id = p.id;
					   centros.Centro_24H = p.Centro_24H;
					   centros.Id_Organismo = p.Id_Organismo;
					   centros.Id_Puerto = p.Id_Puerto;
					   centros.Puerto = p.Puerto;
					   centros.Organismo = p.Organismo;
					   centros.OrganismoActivo = p.OrganismoActivo;
                       centros.Id_Provincia = p.Id_Provincia;
                       centros.Id_Ciudad = p.Id_Ciudad;
                       centros.Cod_Postal = p.Cod_Postal;
                       centros.Direccion = p.Direccion;
                       centros.Provincia = p.Provincia;
                       centros.Ciudad = p.Ciudad;
					});


                ComboProvincias(null);

                ComboCiudades(centros.Id_Provincia, null);

				// Traemos los adicionales (tel, fax, mail), de acuerdo al ID seleccionado
				IEnumerable<Comunica_Centro> result = (from t1 in this._serviciosCentros.GetAdiocionalbyTipo(centros.id)
												orderby t1.id
												select new Comunica_Centro
												{
													id = t1.id,
													Id_Centro24h = t1.Id_Centro24h,
													Tipo_Canal = t1.Tipo_Canal,
													Dato = t1.Dato,
													Nota = t1.Nota                                                    
												});


				this.ComunicaCentro = result;

			   this.comboOrganismos(null);

			   this.comboPuertos(null);

			   return this.PartialView("~/Views/Centros/_PartialEditCentros.cshtml", centros);
			}
			catch (Exception ex)
			{
				this.log.PublishException(new SecureportException(ex));

				return this.Json(new { result = false, Message = ex.Message });
			}
		}
		/// <summary>
		/// Alta-Editar Centros.
		/// </summary>
		[HttpPost]
		public ActionResult AltaEditarCentros(CentrosJson centrosJson)
		{
			try
			{
				bool result;

				switch (centrosJson.action)
				{

					case "Alta":
						result = _serviciosCentros.ListCentros(centrosJson.Centro_24H);
						if (!result)
						{
							Centros centros = new Centros();

							centros.id_usu_alta = this.UsuarioFrontalSession.Usuario.id;

							centros.fech_alta = DateTime.Now;

							centros.Centro_24H = centrosJson.Centro_24H;

							centros.Id_Organismo = centrosJson.ID_Organismo;

							centros.Id_Puerto = centrosJson.ID_Puerto;

                            centros.Direccion = centrosJson.Direccion;

                            centros.Id_Provincia = centrosJson.Id_Provincia;

                            centros.Id_Ciudad = centrosJson.Id_Ciudad;

                            centros.Cod_Postal = centrosJson.Cod_Postal;

							this.repoCentros.Create(centros);

                            centrosJson.Id = this.db.Centros.OrderByDescending(u => u.id).FirstOrDefault().id;

                            this.trazas(76, (int)this.UsuarioFrontalSession.Usuario.id, "Se ha dado de alta un registro en SPMOV_Centros_24H con ID" + "(" + centrosJson.Id.ToString() + ")");

                            this.IdCentro = centrosJson.Id;

							Puertos puerto = repoPuertos.Single(x => x.Id == (int)centros.Id_Puerto);


							this.EnviarCorreoLocal(puerto.Nombre, centros.Centro_24H, "Alta");
						}
						else
						{
							return this.Json(new { result = 1, Message = "'Error al dar de alta el centro 24h, ya existe otro centro 24h activo " + centrosJson.Centro_24H });
						}
						break;

					default:

						result = _serviciosCentros.ListCentros(centrosJson.Centro_24H,centrosJson.Id);
						if (!result)
						{
							Centros centros = this.repoCentros.Single(x => x.id == centrosJson.Id);

							// verificar que el centro no se ha modificado

							Centros centrosInicial = this.repoCentros.Single(x => x.id == centrosJson.Id); 

							centros.Centro_24H = centrosJson.Centro_24H;

							centros.Id_Organismo = centrosJson.ID_Organismo;

							centros.Id_Puerto = centrosJson.ID_Puerto;

                            centros.Direccion = centrosJson.Direccion;

                            centros.Id_Provincia = centrosJson.Id_Provincia;

                            centros.Id_Ciudad = centrosJson.Id_Ciudad;

                            centros.Cod_Postal = centrosJson.Cod_Postal;

							this.repoCentros.Update(centros);

							this.trazas(79, (int)this.UsuarioFrontalSession.Usuario.id, "Se ha realizado la modificación de un registro en SPMOV_Centros_24H con ID" + "(" + centrosJson.Id.ToString() + ")");

                            bool verifica = false;
                           if(centrosInicial.Id_Ciudad != centros.Id_Ciudad)
                              verifica = true;
                           else if (centrosInicial.Id_Organismo != centros.Id_Organismo)
                               verifica = true;
                           else if(centrosInicial.Id_Provincia != centros.Id_Provincia)
                               verifica = true;
                           else if( centrosInicial.Id_Puerto != centros.Id_Puerto)
                               verifica = true;
                           else if(centrosInicial.Centro_24H != centros.Centro_24H)
                               verifica = true;
                           else if(centrosInicial.Direccion != centros.Direccion)
                               verifica = true;
                           else if(centrosInicial.Cod_Postal != centros.Cod_Postal)
                            verifica = true;

                            if (verifica)
                            {
								Puertos puerto = repoPuertos.Single(x => x.Id == (int)centros.Id_Puerto);
								this.EnviarCorreoLocal(puerto.Nombre, centros.Centro_24H, "Modificar");
							}
						}
						else
						{
							return this.Json(new { result = 1, Message = "Error al modificar el centro 24h, ya existe otro centro 24h activo  " + centrosJson.Centro_24H });
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
		/// Borrar Centros.
		/// </summary>
		[HttpPost]
		public ActionResult EliminarCentros(int Id)
		{
			try
			{
				Centros centro = repoCentros.Single(x => x.id == Id);

				this.repoCentros.Delete(centro);

				this.trazas(78, (int)this.UsuarioFrontalSession.Usuario.id, "Se ha realizado la eliminación de un registro en SPMOV_Centros_24H con ID" + "(" + Id.ToString() + ")");

				Puertos puerto = repoPuertos.Single(x => x.Id == (int)centro.Id_Puerto);

				this.EnviarCorreoLocal(puerto.Nombre, centro.Centro_24H, "Baja");

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

				return this.Json(new { result = false, Message = "Instrucción DELETE en conflicto con la restricción REFERENCE." });
			}

			return this.Json(new { result = true });
		}

		[HttpPost]
		public ActionResult CambiarPuerto(int? id, string acciona)
		{
			try
			{
                if(acciona == "1")
                    ViewBag.largo = "col-md-8";
                else
                    ViewBag.largo = "col-md-3";

				this.comboPuertosOrganismo(null, id);

				return this.PartialView("~/Views/Centros/ComboPuerto.cshtml");

			}
			catch (Exception ex)
			{
				this.log.PublishException(new SecureportException(ex));

				return this.Json(new { result = false, Message = ex.Message });
			}

		}

		public void EnviarCorreoLocal(string Puerto, string Centromodi, string Tipo)
		{
			try
			{
				string CorreoA = ConfigurationManager.AppSettings["CorreoCentros24H"];
				string Texto = string.Empty;
				switch (Tipo)
				{
					case "Alta":
                        Texto = "Se le notifica que ha sido dado de alta un Centro 24H con los siguientes datos:  \r\n\r\n Centro 24H: " + Centromodi + " \r\n Puerto: " + Puerto + ". \r\n";
						break;
					case "Modificar":
                        Texto = "Se le notifica que ha sido modificado  un Centro 24H con los siguientes datos:  \r\n\r\n Centro 24H: " + Centromodi + " \r\n Puerto: " + Puerto + ". \r\n";
						break;
					case "Baja":
                        Texto = "Se le notifica que ha sido eliminado  un Centro 24H con los siguientes datos:  \r\n\r\n Centro 24H: " + Centromodi + " \r\n Puerto: " + Puerto + ". \r\n";
						break;
				}

				Texto += "\r\n Deberá comprobar las modificaciones realizadas y en caso de ser necesario volver a imprimir el informe de Centros de 24H para tenerlo actualizado.";
                string Asunto = "SecurePort – " + (Tipo == "Alta" ? "Alta" : (Tipo == "Modificar" ? "Modificación" : "Baja")) + " Datos Centros 24h";
				this.EnviarCorreoPuertos(Asunto, Texto, CorreoA);
			}
			catch (Exception ex)
			{
				this.log.PublishException(new SecureportException(ex));                
			}
		}

        [HttpPost]
        public ActionResult CambiarProvincia(int? id, string acciona)
        {
            try
            {
                this.idProvincia = id;

                this.ComboCiudades(this.idProvincia, null);

                if (acciona == "1")
                    ViewBag.largoC = "col-md-4";
                else
                    ViewBag.largoC = "col-md-2";

                return this.PartialView("~/Views/Centros/ComboCiudadAlta.cshtml");

            }
            catch (Exception ex)
            {
                this.log.PublishException(new SecureportException(ex));

                return this.Json(new { result = false, Message = ex.Message });
            }

        }


		#endregion 

		#region Adicionales

		#region Tel
		public ActionResult AsociadosTelEdit(bool ToolBar)
		{
			this.UsuarioFrontalSession.valor = ToolBar;

			return PartialView("~/Views/Centros/_PartialAsociadosTelEdit.cshtml", this.UsuarioFrontalSession);

		}

		public ActionResult AsociadosEditTel(int Id)
		{
			return PartialView("~/Views/Centros/_PartialAsociadosTelEdit.cshtml", this.UsuarioFrontalSession);
		}

		public ActionResult TelAsociados([DataSourceRequest]DataSourceRequest request)
		{
			try
			{

				IEnumerable<Comunica_Centro> result = this.ComunicaCentro.Where(x => x.Tipo_Canal == 1); //CanalTipo.TEL);
				return Json(result.ToDataSourceResult(request));
			}
			catch (Exception ex)
			{
				this.log.PublishException(new SecureportException(ex));

				return this.Json(new { result = false, Message = ex.Message });
			}
		}


		#endregion

		#region Fax

		public ActionResult AsociadosFaxEdit(bool ToolBar)
		{
			this.UsuarioFrontalSession.valor = ToolBar;

			return PartialView("~/Views/Centros/_PartialAsociadosFaxEdit.cshtml", this.UsuarioFrontalSession);

		}

		public ActionResult AsociadosEditFax(int Id)
		{
			return PartialView("~/Views/Centros/_PartialAsociadosFaxEdit.cshtml", this.UsuarioFrontalSession);
		}

		public ActionResult FaxAsociados([DataSourceRequest]DataSourceRequest request)
		{
			try
			{

				IEnumerable<Comunica_Centro> result = this.ComunicaCentro.Where(x => x.Tipo_Canal == 2); //CanalTipo.FAX);
				return Json(result.ToDataSourceResult(request));
			}
			catch (Exception ex)
			{
				this.log.PublishException(new SecureportException(ex));

				return this.Json(new { result = false, Message = ex.Message });
			}
		}

		#endregion

		#region Mail

		public ActionResult AsociadosMailEdit(bool ToolBar)
		{
			this.UsuarioFrontalSession.valor = ToolBar;

			return PartialView("~/Views/Centros/_PartialAsociadosMailEdit.cshtml", this.UsuarioFrontalSession);

		}

		public ActionResult AsociadosEditMail(int Id)
		{
			return PartialView("~/Views/Centros/_PartialAsociadosMailEdit.cshtml", this.UsuarioFrontalSession);
		}

		public ActionResult MailAsociados([DataSourceRequest]DataSourceRequest request)
		{
			try
			{

				IEnumerable<Comunica_Centro> result = this.ComunicaCentro.Where(x => x.Tipo_Canal == 3); //CanalTipo.MAI);
				return Json(result.ToDataSourceResult(request));
			}
			catch (Exception ex)
			{
				this.log.PublishException(new SecureportException(ex));

				return this.Json(new { result = false, Message = ex.Message });
			}
		}

		#endregion

		[HttpPost]
		public ActionResult AltaEditarComunicaCentro(Comunica_CentroJson comunica_CentroJson)
		{
			try
			{
				int resultado = 0;

				switch (comunica_CentroJson.action)
				{
					case "Alta":
						altaCentroComunicacion(comunica_CentroJson);
						resultado = comunica_CentroJson.Tipo_Canal ==1? 0: comunica_CentroJson.Tipo_Canal ==2? 1 : 2;                        
						break;

					default:
						GuardarCambios(comunica_CentroJson);
						resultado = comunica_CentroJson.Tipo_Canal == 1 ? 0 : comunica_CentroJson.Tipo_Canal == 2 ? 1 : 2;                       
						break;
				}

				return this.Json(new { result = resultado });
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
		public ActionResult AsignarTipos(string TipoCanal)
		{
			
			try
			{
				Comunica_Centro comunica = new Comunica_Centro();

				ViewBag.disabled_Ad = true;

				ViewBag.MensajeAdi = "Alta " + (TipoCanal== CanalTipo.TEL.ToString()? CanalTipo.TEL.ToDescription() :(TipoCanal == CanalTipo.FAX.ToString()? CanalTipo.FAX.ToDescription(): CanalTipo.MAI.ToDescription()));

				ViewBag.actionAdi = General.AltaGeneral.ToDescription();

				ViewBag.ToolAdicional = false;

				ViewBag.TipoCanal = TipoCanal== CanalTipo.TEL.ToString()? CanalTipo.TEL.ToDescription():(TipoCanal == CanalTipo.FAX.ToString()? CanalTipo.FAX.ToDescription(): CanalTipo.MAI.ToDescription()) ;

				ViewBag.TipoCanalValor = TipoCanal == CanalTipo.TEL.ToString() ? "1" : (TipoCanal == CanalTipo.FAX.ToString() ? "2" : "3");

				comunica.id = 1;

				comunica.Tipo_Canal = Convert.ToInt32(ViewBag.TipoCanalValor);

				return this.PartialView("~/Views/Centros/_PartialAsignarAdicionales.cshtml", comunica);
			}
			catch (Exception ex)
			{
				this.log.PublishException(new SecureportException(ex));

				return this.Json(new { result = false, Message = ex.Message });
			}

		}


		[HttpPost]
		public ActionResult VisualizarTipos(string TipoCanal, int Id, string Accion)
		{
			try
			{

				if (Accion == General.EditGeneral.ToDescription())
				{
					ViewBag.MensajeAdi = "Modificar " + (TipoCanal == CanalTipo.TEL.ToString()? CanalTipo.TEL.ToDescription() : (TipoCanal == CanalTipo.FAX.ToString() ? CanalTipo.FAX.ToDescription() : CanalTipo.MAI.ToDescription()));
				   
					ViewBag.actionAdi = General.EditGeneral.ToDescription();

					ViewBag.ToolAdicional = false;

					ViewBag.TipoCanal = TipoCanal == CanalTipo.TEL.ToString() ? CanalTipo.TEL.ToDescription() : (TipoCanal == CanalTipo.FAX.ToString() ? CanalTipo.FAX.ToDescription() : CanalTipo.MAI.ToDescription());

					ViewBag.TipoCanalValor = TipoCanal == CanalTipo.TEL.ToString() ? "1" : (TipoCanal == CanalTipo.FAX.ToString() ? "2" : "3");

					ViewBag.disabled_Ad = true;

					this.ViewBag.Combo_po = "display:block";

					this.ViewBag.Texto_po = "display:none";

				} else
				{
					ViewBag.MensajeAdi = "Visualizar " + (TipoCanal == CanalTipo.TEL.ToString() ? CanalTipo.TEL.ToDescription() : (TipoCanal == CanalTipo.FAX.ToString() ? CanalTipo.FAX.ToDescription() : CanalTipo.MAI.ToDescription()));

					ViewBag.actionAdi = General.EditGeneral.ToDescription();

					ViewBag.ToolAdicional = true;

					ViewBag.TipoCanal = TipoCanal == CanalTipo.TEL.ToString() ? CanalTipo.TEL.ToDescription() : (TipoCanal == CanalTipo.FAX.ToString() ? CanalTipo.FAX.ToDescription() : CanalTipo.MAI.ToDescription());

					ViewBag.TipoCanalValor = TipoCanal == CanalTipo.TEL.ToString() ? "1" : (TipoCanal == CanalTipo.FAX.ToString() ? "2" : "3");

					ViewBag.disabled_Ad = false;

					this.ViewBag.Combo_po = "display:none";

					this.ViewBag.Texto_po = "display:block";
				}

				Comunica_Centro comunica = new Comunica_Centro();

				this._serviciosCentros.GetAdiocionalbyTipo(IdCentro).Where(x => x.Tipo_Canal ==  Convert.ToInt32(ViewBag.TipoCanalValor) && x.id == Id).ToList().ForEach(
				   p =>
				   {   comunica.id = p.id;
					   comunica.Id_Centro24h = p.Id_Centro24h;
					   comunica.Tipo_Canal = p.Tipo_Canal;
					   comunica.Dato = p.Dato;
					   comunica.Nota = p.Nota;   
				   });

				return this.PartialView("~/Views/Centros/_PartialAsignarAdicionales.cshtml", comunica);

			}
			catch (Exception ex)
			{
				this.log.PublishException(new SecureportException(ex));

				return this.Json(new { result = false, Message = ex.Message });
			}
		}

		[HttpPost]
		public ActionResult GuardarCambios(Comunica_CentroJson comunicaCentroJson)
		{
			try
			{
				Comunica_Centro comunica = this.repoComunica_Centro.Single(x => x.id == comunicaCentroJson.id);

				Comunica_Centro comunicaInicial = this.repoComunica_Centro.Single(x => x.id == comunicaCentroJson.id);

				comunica.Dato = comunicaCentroJson.Dato;

				comunica.Nota = comunicaCentroJson.Nota;

				this.repoComunica_Centro.Update(comunica);

				this.trazas(76, (int)this.UsuarioFrontalSession.Usuario.id, "Se ha dado de modificado un registro en SPMOV_Comunica_Centro24h " + this.UsuarioFrontalSession.Usuario.login);

				// Traemos los adicionales (tel, fax, mail), de acuerdo al ID seleccionado
				IEnumerable<Comunica_Centro> result = (from t1 in this._serviciosCentros.GetAdiocionalbyTipo(this.IdCentro)
													   orderby t1.id
													   select new Comunica_Centro
													   {
														   id = t1.id,
														   Id_Centro24h = t1.Id_Centro24h,
														   Tipo_Canal = t1.Tipo_Canal,
														   Dato = t1.Dato,
														   Nota = t1.Nota
													   });


				this.ComunicaCentro = result;

                bool Verifica = false;
                if (comunica.Dato != comunicaInicial.Dato)
                    Verifica = true;
                if(comunica.Nota != comunicaInicial.Nota)
                    Verifica= true;


				if (Verifica)					
				{
					Centros centro = repoCentros.Single(x=> x.id == (int)comunica.Id_Centro24h);
					Puertos puerto = repoPuertos.Single(z=> z.Id == (int)centro.Id_Puerto);
					this.EnviarCorreoLocal(puerto.Nombre, centro.Centro_24H,"Modificar");
				}
				return this.Json(new { result = true });

			}
			catch (Exception ex)
			{
				this.log.PublishException(new SecureportException(ex));

				return this.Json(new { result = false, Message = ex.Message });
			}

		}

		[HttpPost]
		public ActionResult EliminarAdicional(int? id)
		{
			try
			{

				Comunica_Centro comunica = this.repoComunica_Centro.Single(x => x.id == id);
				int resultado = comunica.Tipo_Canal;
				
				this.repoComunica_Centro.Delete(comunica);

				this.trazas(78, (int)this.UsuarioFrontalSession.Usuario.id, "Se ha dado de borrado un registro en SPMOV_Comunica_Centro24h " + this.UsuarioFrontalSession.Usuario.login);

				// Traemos los adicionales (tel, fax, mail), de acuerdo al ID seleccionado
				IEnumerable<Comunica_Centro> result = (from t1 in this._serviciosCentros.GetAdiocionalbyTipo(this.IdCentro)
													   orderby t1.id
													   select new Comunica_Centro
													   {
														   id = t1.id,
														   Id_Centro24h = t1.Id_Centro24h,
														   Tipo_Canal = t1.Tipo_Canal,
														   Dato = t1.Dato,
														   Nota = t1.Nota
													   });


				this.ComunicaCentro = result;

				Centros centro = repoCentros.Single(x=> x.id==(int)comunica.Id_Centro24h);
				Puertos puerto = this.repoPuertos.Single(z => z.Id == (int)centro.Id_Puerto);
				this.EnviarCorreoLocal(puerto.Nombre, centro.Centro_24H, "Modificar");

			  

				return this.Json(new { result = resultado });

			}
			catch (Exception ex)
			{
				this.log.PublishException(new SecureportException(ex));

				return this.Json(new { result = false, Message = ex.Message });
			}

		}

		[HttpPost]
		public ActionResult altaCentroComunicacion(Comunica_CentroJson comunicaCentroJson)
		{
			try
			{
				Comunica_Centro comunica = new Comunica_Centro();

				comunica.id_usu_alta = this.UsuarioFrontalSession.Usuario.id;

				comunica.fech_alta = DateTime.Now;

				comunica.Id_Centro24h = this.IdCentro;

				comunica.Tipo_Canal = comunicaCentroJson.Tipo_Canal;

				comunica.Dato = comunicaCentroJson.Dato;

				comunica.Nota = comunicaCentroJson.Nota;

				this.repoComunica_Centro.Create(comunica);

				comunicaCentroJson.id = this.db.Comunica_Centro.OrderByDescending(u => u.id).FirstOrDefault().id;

				this.trazas(76, (int)this.UsuarioFrontalSession.Usuario.id, "Se ha dado de alta un registro en SPMOV_Comunica_Centro24h con ID" + "(" + comunicaCentroJson.id.ToString() + ")");

				// Traemos los adicionales (tel, fax, mail), de acuerdo al ID seleccionado
				IEnumerable<Comunica_Centro> result = (from t1 in this._serviciosCentros.GetAdiocionalbyTipo(this.IdCentro)
													   orderby t1.id
													   select new Comunica_Centro
													   {
														   id = t1.id,
														   Id_Centro24h = t1.Id_Centro24h,
														   Tipo_Canal = t1.Tipo_Canal,
														   Dato = t1.Dato,
														   Nota = t1.Nota
													   });


				this.ComunicaCentro = result;

				Centros centro = repoCentros.Single(x => x.id == (int)comunica.Id_Centro24h);
				Puertos puerto = this.repoPuertos.Single(z => z.Id == (int)centro.Id_Puerto);
				this.EnviarCorreoLocal(puerto.Nombre, centro.Centro_24H, "Modificar");

				return this.Json(new { result = true });

			}
			catch (Exception ex)
			{
				this.log.PublishException(new SecureportException(ex));

				return this.Json(new { result = false, Message = ex.Message });
			}

		}
		#endregion

		#region Operadores

		public ActionResult BuscadorOperadores([DataSourceRequest]DataSourceRequest request)
		{
			try
			{
				IEnumerable<OperadoresPuertoViewModel> result = this._serviciosCentros.GetAllOperadores(this.IdCentro).Where(x=>x.Es_Suplente==false).OrderBy(x=> x.Nombre);

				return this.Json(result.ToDataSourceResult(request));
			}
			catch (Exception ex)
			{
				this.log.PublishException(new SecureportException(ex));
				throw;
			}
		}


		public ActionResult OperadoresEditar(int Id)
		{
			return PartialView("~/Views/Centros/_PartialOperadores.cshtml", this.UsuarioFrontalSession);
		}

		#endregion

        #region Report

        public ActionResult Index()
        {
            return PartialView("~/Views/Centros/_PartialReportes.cshtml", this.UsuarioFrontalSession);

        }

       


        #endregion


        #endregion
    }
}