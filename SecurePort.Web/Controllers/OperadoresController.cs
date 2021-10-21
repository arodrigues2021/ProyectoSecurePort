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
	#endregion

	public class OperadoresController : BaseController
	{
		public OperadoresController(IServiciosPuertos serviciosPuertos,
										IServiciosTipoInstalacion serviciosTipoInstalacion,
										InstalacionesRepository repoInstalaciones,
										OperadoresRepository repoOperadores,
										ProvinciasRepository repoProvincias,
										CiudadesRepository repoCiudades,
										UsuariosRepository repoUsuarios,
										DocumentosUsuarioRepository repoDocumentosUsuario,  
										IServiciosUsuarios serviciosUsuarios,
										IServiciosDocumentos servicioDocumentos,
                                        PuertosRepository repoPuertos,
										TrazasRepository repotrazas, 
										ILogger logger)
			: base(serviciosPuertos,
					 serviciosTipoInstalacion, 
					 repoInstalaciones,
					 repoOperadores,
					 repoProvincias,
					 repoCiudades,
					 repoUsuarios,                     
					 repoDocumentosUsuario,
					 serviciosUsuarios,
					 servicioDocumentos,
                     repoPuertos,   
					 repotrazas, 
					 logger)
		{
		}

		#region Operadores
		/// <summary>
		/// Permite buscar las instalaciones
		/// </summary>
		public ActionResult BuscadorOperadores([DataSourceRequest]DataSourceRequest request)
		{
			try
			{            
				IEnumerable<OperadoresViewModel> result = this._serviciosTipoInstalacion.GetAllOperadores(this.idInstalacion).OrderBy(x=> x.Nombre);

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
		/// <summary>
		/// Retorna una lista de todas las Instalaciones.
		/// </summary>
		public ActionResult ListadoOperadores()
		{
			try
			{
				return this.PartialView("ListadoOperadores", this.UsuarioFrontalSession);
			}
			catch (Exception ex)
			{
				this.log.PublishException(new SecureportException(ex));
				throw;
			}
		}
		#endregion

		#region CRUD

		#region Operadores

		public ActionResult OperadoresEdit(bool ToolBar)
		{
			this.UsuarioFrontalSession.valor = ToolBar;


			return PartialView("~/Views/Instalaciones/_PartialOperadores.cshtml", this.UsuarioFrontalSession);

		}

		public ActionResult OperadoresEditar(int Id)
		{
			return PartialView("~/Views/Instalaciones/_PartialOperadores.cshtml", this.UsuarioFrontalSession);
		}

	   
		 /// <summary>
		/// Permite visualizar los datos de un usuario con documentos.
		/// </summary>
	   [HttpPost]
		public ActionResult AsignarOperador()
		{
			try
			{
				this.ComboUsuariosOPIP(null);

				this.ComboCiudades(null, null);

				this.ComboProvincias(null);

				ViewBag.Combo = "display:block";

				ViewBag.Texto = "display:none";

				ViewBag.Operadordisabled = true;

				ViewBag.display = "display:none";

				ViewBag.Mensaje = "Alta Contacto";

				ViewBag.actionOperador = "Alta";

				ViewBag.Clasifica = "display:block";

				//ViewBag.ClasificaCombo = ((this.idClasificacion == 1 || this.idClasificacion == 6) ? "display:block" : "display:none");

				OperadoresViewModel operadores = new OperadoresViewModel();

				operadores.Id = 1;

				operadores.Id_Instalacion = (int)this.idInstalacion;

				this.ViewBag.Observacion = string.Empty;

				return this.PartialView("~/Views/Instalaciones/_PartialAsignarOperador.cshtml", operadores);

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
	   public ActionResult AltaEditarOperador(OperadoresJson operadoresJson)
	   {
		   try
		   {
			  
			   switch (operadoresJson.action)
			   {

				   case "Alta":

					   AltaOperador(operadoresJson);
					   break;

				   default:
					   
					   EditarOperador(operadoresJson);
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


	   [HttpPost]
	   public ActionResult CambiarProvincia(int? id)
	   {
		   try
		   {

			   this.idProvincia = id;

			   this.ComboCiudades(this.idProvincia, null);

			   return this.PartialView("~/Views/Instalaciones/ComboCiudadOperador.cshtml");

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
	   public ActionResult EliminarOperador(int Id)
	   {
		   try
		   {
               Operadores operador = this.repoOperadores.Single(x => x.Id == Id);

			   this.repoOperadores.Delete(operador);

			   this.trazas(65, (int)this.UsuarioFrontalSession.Usuario.id, "Se ha realizado la eliminación de un registro en SPMOV_Operadores_Instalacion con ID" + "(" + Id.ToString() + ")");

               MOV_Instalaciones Instalacion = repoInstalaciones.Single(z => z.Id == operador.Id_Instalacion);

               Puertos puerto = this.repoPuertos.Single(x => x.Id == Instalacion.id_Puerto);               

               EnviarCorreoLocal(puerto.Nombre, operador.Nombre, "Baja", Instalacion.Nombre);

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
	   public ActionResult AltaOperador(OperadoresJson operadoresJson)
	   {
		   try
		   {
			   Operadores operador = new Operadores();

			   operador.Id_usu_alta = this.UsuarioFrontalSession.Usuario.id;

			   operador.Es_Activo = true;

			   operador.Fech_activo = DateTime.Now;

			   operador.Fech_alta = DateTime.Now;

			   operador.Nombre = operadoresJson.Nombre;

			   operador.Es_Suplente = operadoresJson.Es_Suplente;

			   operador.Direccion = operadoresJson.Direccion;

			   operador.Id_provincia = operadoresJson.Id_provincia;

			   operador.Id_Ciudad = operadoresJson.Id_Ciudad;

			   operador.Cod_postal = operadoresJson.Cod_postal;

			   operador.Telefono1 = operadoresJson.Telefono1;

			   operador.Fax = operadoresJson.Fax;

			   operador.Email = operadoresJson.Email;

			   operador.Id_Instalacion = operadoresJson.Id_Instalacion;

			   operador.Observaciones = this.CambiarTexto(operadoresJson.Observaciones);

		       operador.Cargo = operadoresJson.cargo;
			   
			   this.repoOperadores.Create(operador);

			   var Id = this.db.Operadores.OrderByDescending(u => u.Id).FirstOrDefault().Id;

			   this.trazas(63, (int)this.UsuarioFrontalSession.Usuario.id, "Se ha dado de alta un registro en SPMOV_Operadores_Instalacion con ID" + "(" + Id.ToString() + ")");

               MOV_Instalaciones Instalacion = repoInstalaciones.Single(z => z.Id == operadoresJson.Id_Instalacion);

               Puertos puerto = this.repoPuertos.Single(x => x.Id == Instalacion.id_Puerto);

               EnviarCorreoLocal(puerto.Nombre, operadoresJson.Nombre, "Alta", Instalacion.Nombre);


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
	   public ActionResult EditarOperador(OperadoresJson operadoresJson)
	   {
		   try
		   {
               Operadores operadorInicial = this.repoOperadores.Single(x => x.Id == operadoresJson.Id);

			   Operadores operador = this.repoOperadores.Single(x => x.Id == operadoresJson.Id);

			  operador.Nombre = operadoresJson.Nombre;

			   operador.Es_Suplente = operadoresJson.Es_Suplente;

			   operador.Direccion = operadoresJson.Direccion;

			   operador.Id_provincia = operadoresJson.Id_provincia;

			   operador.Id_Ciudad = operadoresJson.Id_Ciudad;

			   operador.Cod_postal = operadoresJson.Cod_postal;

			   operador.Telefono1 = operadoresJson.Telefono1;

               operador.Es_Activo = operadoresJson.Es_Activo;

			   operador.Fax = operadoresJson.Fax;

			   operador.Email = operadoresJson.Email;

			   operador.Observaciones = this.CambiarTexto(operadoresJson.Observaciones);

		       operador.Cargo = operadoresJson.cargo;
			   
			   this.repoOperadores.Update(operador);

			   this.trazas(66, (int)this.UsuarioFrontalSession.Usuario.id, "Se ha realizado la modificación de un registro en SPMOV_Operadores_Instalacion con ID" + "(" + operadoresJson.Id + ")");

               bool verifica = false;
               if (operadorInicial.Nombre != operador.Nombre)
                   verifica = true;
               else if(operadorInicial.Es_Suplente != operador.Es_Suplente)
                   verifica = true;
               else if(operadorInicial.Cargo != operador.Cargo)
                   verifica = true;
               else if(operadorInicial.Cod_postal != operador.Cod_postal)
                   verifica = true;
               else if(operadorInicial.Direccion != operador.Direccion)
                   verifica = true;
               else if(operadorInicial.Email != operador.Email)
                   verifica = true;
               else if(operadorInicial.Es_Activo != operador.Es_Activo)
                   verifica = true;
               else if(operadorInicial.Fax != operador.Fax)
                   verifica = true;
               else if(operadorInicial.Id_Ciudad != operador.Id_Ciudad)
                   verifica = true;
               else if(operadorInicial.Id_Instalacion != operador.Id_Instalacion)
                   verifica = true;
               else if (operadorInicial.Id_provincia != operador.Id_provincia)
                   verifica = true;
               else if(operadorInicial.Nombre != operador.Nombre)
                   verifica = true;
               else if(operadorInicial.Observaciones != operador.Observaciones)
                   verifica = true;
               else if(operadorInicial.Telefono1 != operador.Telefono1)
                   verifica = true;               

               if (verifica)
               {
                   MOV_Instalaciones Instalacion = repoInstalaciones.Single(z => z.Id == operador.Id_Instalacion);

                   Puertos puerto = this.repoPuertos.Single(x => x.Id == Instalacion.id_Puerto);

                   EnviarCorreoLocal(puerto.Nombre, operadoresJson.Nombre, "Modificar", Instalacion.Nombre);
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

		   [HttpPost]
		public ActionResult VisualizarOperador(int id, string Accion)
		{
			try
			{
				  IEnumerable<OperadoresViewModel> listOperdores = this._serviciosTipoInstalacion.GetAllOperadores(this.idInstalacion).Where(x => x.Id == id);

				if (Accion == General.EditGeneral.ToDescription())
				{
					ViewBag.Operadordisabled = true;
					ViewBag.Combo = "display:block";
					ViewBag.Texto = "display:none";
					ViewBag.Mensaje = "Editar Contacto";
					ViewBag.Clasifica = "display:block";
				}
				else
				{
					ViewBag.Operadordisabled = false;
					ViewBag.Combo = "display:none";
					ViewBag.Texto = "display:block";
					ViewBag.Mensaje = "Visualizar Contacto";
                    ViewBag.Clasifica = "display:block";
				}
				ViewBag.display = "display:block";

				ViewBag.action = General.EditGeneral.ToDescription();

				this.ViewBag.navegador = this.Browser;
				
				OperadoresViewModel operador = new OperadoresViewModel();

				listOperdores.ToList().ForEach(
				   p =>
				   {
					   operador.Id = p.Id;
					   operador.Id_Usuario = p.Id_Usuario;
					   operador.Usuario = p.Usuario;
					   operador.Es_Suplente = p.Es_Suplente;
					   operador.Es_Activo = p.Es_Activo;
					   operador.Direccion = p.Direccion;
					   operador.Id_provincia = p.Id_provincia;
					   operador.Provincia = p.Provincia;
					   operador.Id_Ciudad = p.Id_Ciudad;
					   operador.Ciudad = p.Ciudad;
					   operador.Cod_postal = p.Cod_postal;
					   operador.Telefono1 = p.Telefono1;
					   operador.Fax = p.Fax;
					   operador.Email = p.Email;
                       operador.Nombre = p.Nombre;
                       operador.activado = p.activado;
				       operador.Cargo = p.Cargo;
					   this.ViewBag.Observacion = p.Observaciones.Trim() ;

				   });

				this.ComboProvincias(null);

				this.ComboCiudades(null, operador.Id_Ciudad);

				return this.PartialView("~/Views/Instalaciones/_PartialAsignarOperador.cshtml", operador);
			}
			catch (Exception ex)
			{
				this.log.PublishException(new SecureportException(ex));

				return this.Json(new { result = false, Message = ex.Message });
			}
		}

           public void EnviarCorreoLocal(string Puerto, string Operador, string Tipo, string centro)
           {
               try
               {

                   string CorreoA = ConfigurationManager.AppSettings["CorreoInstalaciones"];
                   string Texto = string.Empty;
                   switch (Tipo)
                   {
                       case "Alta":
                           Texto = "Se le notifica que ha sido dado de alta un contacto de instalación Con los siguientes datos:  \r\n\r\n Instalación:  " + centro + " \r\n Puerto: " + Puerto + " \r\n Contacto: " + Operador + ". \r\n";
                           break;
                       case "Modificar":
                           Texto = "Se le notifica que ha sido modificado  un contacto de instalación Con los siguientes datos:  \r\n\r\n Instalación:  " + centro + " \r\n Puerto: " + Puerto + " \r\n Contacto: " + Operador + ". \r\n";
                           break;
                       case "Baja":
                           Texto = "Se le notifica que ha sido eliminado  un contacto de Instalación Con los siguientes datos:  \r\n\r\n Instalación:  " + centro + " \r\n Puerto: " + Puerto + " \r\n Contacto: " + Operador + ". \r\n";
                           break;
                   }
                   Texto += "\r\n Deberá comprobar las modificaciones realizadas.";
                   string Asunto = "SecurePort – "+ (Tipo=="Alta"?"Alta":(Tipo == "Modificar"? "Modificación": "Baja"))  +" Datos Contactos Instalación";
                   this.EnviarCorreoPuertos(Asunto, Texto, CorreoA);
               }
               catch (Exception ex)
               {
                   this.log.PublishException(new SecureportException(ex));
               }
           }


		#endregion

		#region Documentos

		public ActionResult AsociadosEdit(bool ToolBar)
		{
			this.UsuarioFrontalSession.valor = ToolBar;

			return PartialView("~/Views/Instalaciones/Documentos/_PartialAsociadosEdit.cshtml", this.UsuarioFrontalSession);

		}

		public ActionResult AsociadosEditOperadores(int Id)
		{
			return PartialView("~/Views/Instalaciones/Documentos/_PartialAsociadosEdit.cshtml", this.UsuarioFrontalSession);
		}
		/// <summary>
		/// Permite visualizar los datos de un usuario con documentos.
		/// </summary>
		[HttpPost]
		public ActionResult AsignarDocumentos()
		{
			try
			{

				this.ComboTipos_Documento();

				MOV_Instalaciones instalcion = new MOV_Instalaciones();

				this._serviciosTipoInstalacion.ListMOV_Instalaciones(idInstalacion).ToList().ForEach(
				   p =>
				   {
					   instalcion.Id = p.Id;
					   instalcion.Nombre = p.Nombre;                       
					 
				   });

				return this.PartialView("~/Views/Instalaciones/_PartialAsignarDocumentos.cshtml", instalcion);

			}
			catch (Exception ex)
			{
				this.log.PublishException(new SecureportException(ex));

				return this.Json(new { result = false, Message = ex.Message });
			}
		}


		public ActionResult DocumentosAsociados([DataSourceRequest]DataSourceRequest request)
		{
			try
			{

				IEnumerable<Doc_Asoc> result = (from t1 in this._serviciosDocumentos.GetDocs(this.idInstalacion, 9)
													   orderby t1.id
													   select new Doc_Asoc
													   {
														   id = t1.id,
														   TipoNombre = t1.TipoNombre,
														   documento = t1.documento.Substring((t1.documento.IndexOf("_")) + 1),
														   descripcion = t1.descripcion
													   });

				return Json(result.OrderBy(x=> x.documento).ToDataSourceResult(request));
			}
			catch (Exception ex)
			{
				this.log.PublishException(new SecureportException(ex));

				return this.Json(new { result = false, Message = ex.Message });
			}
		}

		/// <summary>
		///     Permite guardar cambios en un documento asociado.
		/// </summary>
		[HttpPost]
		public ActionResult GuardarCambios(DocumentoJson DocumentoJson)
		{
			try
			{

				this.ComboTipos_Documento();

				Docs_Usuario docsUsuario = this.repoDocumentosUsuario.Single(x => x.id == DocumentoJson.id);

				docsUsuario.descripcion = DocumentoJson.descripcion;

				docsUsuario.id_tipo_doc = DocumentoJson.tipodocumento;

				this.repoDocumentosUsuario.Update(docsUsuario);

				this.trazas(13, (int)this.UsuarioFrontalSession.Usuario.id, "Se ha dado de modificado un registro en SPCON_Depen_Usuario " + this.UsuarioFrontalSession.Usuario.login);

				return this.Json(new { result = true });

			}
			catch (Exception ex)
			{
				this.log.PublishException(new SecureportException(ex));

				return this.Json(new { result = false, Message = ex.Message });
			}

		}

		/// <summary>
		///     Permite eliminar un documento asociado.
		/// </summary>
		[HttpPost]
		public ActionResult visualizarDocumento(int? id)
		{
			try
			{
				Documento documento = new Documento();

				Docs_Usuario docsUsuario = this.repoDocumentosUsuario.Single(x => x.id == id);

				string doc = docsUsuario.documento;

				string nombreRuta = ConfigurationManager.AppSettings["Ficheros"];
				//string nombreRuta = ConfigurationManager.AppSettings["RutaDOC"];

				//string nombreFichero = ConfigurationManager.AppSettings["Ficheros"];
				//string ruta = Path.Combine(nombreRuta, nombreFichero, doc);
				//string Extencion = Path.GetExtension(ruta);
				//string filepath = AppDomain.CurrentDomain.BaseDirectory + Path.Combine(ConfigurationManager.AppSettings["Ficheros"], doc);
				//byte[] filedata = System.IO.File.ReadAllBytes(filepath);
				//string contentType = MimeMapping.GetMimeMapping(filepath);

				//var cd = new System.Net.Mime.ContentDisposition
				//{
				//    FileName = doc,
				//    Inline = true,
				//};

				//Response.AppendHeader("Content-Disposition", cd.ToString());

				//return File(filedata, contentType);


				//this.UsuarioFrontalSession.Path = Path.Combine(nombreRuta,nombreFichero,doc);

				this.UsuarioFrontalSession.Path = nombreRuta + "/" + doc;

				return PartialView("~/Views/Shared/PartialDoc.cshtml", this.UsuarioFrontalSession);

			}
			catch (Exception ex)
			{
				this.log.PublishException(new SecureportException(ex));

				return this.Json(new { result = false, Message = ex.Message });
			}

		}

		/// <summary>
		///     Permite eliminar un documento asociado.
		/// </summary>
		[HttpPost]
		public ActionResult EditarDocumento(int? id)
		{
			try
			{

				this.ComboTipos_Documento();

				Documento documento = new Documento();

				IEnumerable<InstalacionViewModel> instalacion = this._serviciosTipoInstalacion.GetAllInstalaciones().Where(x => x.Id == this.idInstalacion);

				InstalacionViewModel temp = instalacion.FirstOrDefault();

				Docs_Usuario docsInstalacion = this.repoDocumentosUsuario.Single(x => x.id == id);

				documento.Nombre = temp.Nombre;

				documento.Apellidos = temp.Nombre;

				documento.descripcion = docsInstalacion.descripcion;

				documento.tipodocumento = docsInstalacion.id_tipo_doc;

                documento.documento = docsInstalacion.documento.Substring((docsInstalacion.documento.IndexOf("_")) + 1);

				return this.PartialView("~/Views/Instalaciones/_PartialEditDocumentos.cshtml", documento);

			}
			catch (Exception ex)
			{
				this.log.PublishException(new SecureportException(ex));

				return this.Json(new { result = false, Message = ex.Message });
			}

		}

		/// <summary>
		///     Permite eliminar un documento asociado.
		/// </summary>
		[HttpPost]
		public ActionResult EliminarDocumentos(int? id)
		{
			try
			{
                Docs_Usuario docsUsuario = this.repoDocumentosUsuario.Single(x => x.id == id);

                this.repoDocumentosUsuario.Delete(docsUsuario);

				this.trazas(12, (int)this.UsuarioFrontalSession.Usuario.id, "Se ha dado de borrado un registro en SPCON_Depen_Usuario " + this.UsuarioFrontalSession.Usuario.login);

                BorrarFichero(ConfigurationManager.AppSettings["RutaSecurePorDoc"].ToString() + "/" + docsUsuario.documento);

				return this.Json(new { result = true });

			}
			catch (Exception ex)
			{
				this.log.PublishException(new SecureportException(ex));

				return this.Json(new { result = false, Message = ex.Message });
			}

		}

		/// <summary>
		/// Permite visualizar los datos de un usuario ya registrado.
		/// </summary>
		[HttpPost]
		public void AsignarTipoDocumento(UploadJson uploadJson)
		{
			try
			{
				this.idTipoDocumento = uploadJson.tipo;

			}
			catch (Exception ex)
			{
				this.log.PublishException(new SecureportException(ex));
			}
		}

		/// <summary>
		/// Permite deshacer los cambios.
		/// </summary>
		[HttpPost]
		public ActionResult DeshacerCambios(int id)
		{
			try
			{

				this.ComboTipos_Documento();

				Documento documento = new Documento();

				IEnumerable<InstalacionViewModel> instalacion = this._serviciosTipoInstalacion.GetAllInstalaciones().Where(x => x.Id == this.idInstalacion);

				InstalacionViewModel temp = instalacion.FirstOrDefault();

				Docs_Usuario docsInstalacion = this.repoDocumentosUsuario.Single(x => x.id == id);

				documento.Nombre = temp.Nombre;

				documento.Apellidos = temp.Nombre;

				documento.descripcion = docsInstalacion.descripcion;

				documento.tipodocumento = docsInstalacion.id_tipo_doc;

                documento.documento = docsInstalacion.documento.Substring((docsInstalacion.documento.IndexOf("_")) + 1); 


				return this.PartialView("~/Views/Instalaciones/_PartialEditDocumentos.cshtml", documento);

			}
			catch (Exception ex)
			{
				this.log.PublishException(new SecureportException(ex));

				return this.Json(new { result = false, Message = ex.Message });
			}
		}

		[HttpPost]
		public ActionResult ValidarDocumento(UploadJson uploadJson)
		{
			string nombre = uploadJson.nombre_servicio + uploadJson.id_servicio.ToString() + "_" + uploadJson.nombre;

			IEnumerable<Doc_Asoc> result = this._serviciosDocumentos.GetDocs(this.idInstalacion, 9).ToList().Where(x => x.documento == nombre);
			if (result.Count() > 0)
				return this.Json(new { result = 0 });
			else
				return this.Json(new { result = 1 });
		}
		

		#endregion

		#endregion
	}
}