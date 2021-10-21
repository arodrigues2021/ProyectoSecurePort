
namespace SecurePort.Services.Interfaces
{

	#region using
	using System.Data;
	using System.Data.SqlClient;
	using System.Security.Policy;

	using SecurePort.Entities;
	using SecurePort.Entities.Enums;
	using SecurePort.Entities.Models;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	#endregion

	public class ServiciosUsuarios : IServiciosUsuarios 
	{

		#region Atributos
		private readonly ILogger log;
		protected readonly SecurePortContext db = new SecurePortContext();
		#endregion

		#region Métodos públicos

		public ServiciosUsuarios(ILogger log)
		{
			this.log = log;
		}

		public bool GetUsuarios(string Login)
		{
			try
			{

				bool existe = false;
				var result = db.Usuarios.Where(x => x.login == Login).ToList();
				return existe = (result.Any() ? true : false);
			}
			catch (Exception ex)
			{
				this.log.PublishException(ex);
				throw;
			}
		}

		public bool GetEmail(string Email)
		{
			try
			{

				bool existe = false;
				var result = db.Usuarios.Where(x => x.email == Email).ToList();
				return existe = (result.Any() ? true : false);
			}
			catch (Exception ex)
			{
				this.log.PublishException(ex);
				throw;
			}
		}

		public bool GetPassword(string Password)
		{
			try
			{

				bool existe = false;
				var result = db.Usuarios.Where(x => x.password == Password).ToList();
				return existe = (result.Any() ? true : false);
			}
			catch (Exception ex)
			{
				this.log.PublishException(ex);
				throw;
			}
		}

		public IEnumerable<Tipos_Documento> GetTiposDocumento()
		{
			try
			{
				IEnumerable<Tipos_Documento> result =
					(from t1 in this.db.Tipos_Documento.ToList()
						orderby t1.id
						select new Tipos_Documento { id = t1.id, documento = t1.documento, descripcion = t1.descripcion });
				return result;
			}
			catch (Exception ex)
			{
				this.log.PublishException(ex);
				throw;
			}
		}
		
		public IEnumerable<Categoria> GetCategorias()
		{
			try
			{
				IEnumerable<Categoria> ListaCategorias =
					(IEnumerable<Categoria>)
						new Categoria[]
						{
							new Categoria { Id = 1, Name = "Administrador" }, 
							new Categoria { Id = 2, Name = "Puertos del Estado" },
							new Categoria { Id = 3, Name = "Organismo Gestión Portuaria" }, 
							new Categoria { Id = 4, Name = "Puerto" },
							new Categoria { Id = 5, Name = "Instalación" }, 
							new Categoria { Id = 6, Name = "Ministerio del Interior" },
						};

				return ListaCategorias;
			}
			catch (Exception ex)
			{
				this.log.PublishException(ex);
				throw;
			}
		}

		public IEnumerable<AccionesViewModel> GetAcciones()
		{
			try
			{
				IEnumerable<AccionesViewModel> ListaAcciones =
					(IEnumerable<AccionesViewModel>)
						new AccionesViewModel[]
						{
							new AccionesViewModel { Id = 1, Name = Usuario.ACTIVAR_USUARIO.ToDescription() }, 
							new AccionesViewModel { Id = 2, Name = Usuario.VISUALIZAR_TODOS_USUARIOS.ToDescription() },
							new AccionesViewModel { Id = 3, Name = Usuario.MODIFICAR_DEPEN_USUARIO.ToDescription() }, 
							new AccionesViewModel { Id = 4, Name = Usuario.RESETEAR_PASS_USUARIO.ToDescription() },
						};

				return ListaAcciones;
			}
			catch (Exception ex)
			{
				this.log.PublishException(ex);
				throw;
			}
		}


		public int RemoveGrupoById(int? Id)
		{
			var dbCommand = db.Database.Connection.CreateCommand();
			dbCommand.CommandType = CommandType.StoredProcedure;
			dbCommand.CommandText = "SPPROC_Borrar_Grupo";
			dbCommand.CommandTimeout = 5000;
			var p = dbCommand.CreateParameter();
			p.ParameterName = "id";
			p.Value = Id;
			p.Direction = ParameterDirection.Input;
			dbCommand.Parameters.Add(p);
			var ResultadoParam = new SqlParameter("Resultado", 0) { Direction = ParameterDirection.Output };
			dbCommand.Parameters.Add(ResultadoParam);
			db.Database.Connection.Open();
			var dataReader = dbCommand.ExecuteReader();
			var result = Convert.ToInt32(ResultadoParam.Value);
			dataReader.Close();
			db.Database.Connection.Close();
			return result;
		}

		public string RemoveUsuarioById(int? IdUsuario)
		{
			var dbCommand = db.Database.Connection.CreateCommand();
			dbCommand.CommandType = CommandType.StoredProcedure;
			dbCommand.CommandText = "SPPROC_Borrar_Usuarios";
			dbCommand.CommandTimeout = 5000;
			var p = dbCommand.CreateParameter();
			p.ParameterName = "idusuario";
			p.Value = IdUsuario;
			p.Direction = ParameterDirection.Input;
			dbCommand.Parameters.Add(p);
			SqlParameter ResultadoParam0 = new SqlParameter();
			ResultadoParam0.SqlDbType= SqlDbType.VarChar;
			ResultadoParam0.ParameterName = "@resultado";
			ResultadoParam0.Direction = ParameterDirection.Output;
			ResultadoParam0.Size = 800;
			dbCommand.Parameters.Add(ResultadoParam0);
			db.Database.Connection.Open();
			var dataReader = dbCommand.ExecuteReader();
			var result = ResultadoParam0.Value.ToString();
			dataReader.Close();
			db.Database.Connection.Close();
			return result;
		}


		public int RemovePerfilById(int? Id)
		{
			var dbCommand = db.Database.Connection.CreateCommand();
			dbCommand.CommandType = CommandType.StoredProcedure;
			dbCommand.CommandText = "SPPROC_Borrar_Perfil";
			dbCommand.CommandTimeout = 5000;
			var p = dbCommand.CreateParameter();
			p.ParameterName = "id";
			p.Value = Id;
			p.Direction = ParameterDirection.Input;
			dbCommand.Parameters.Add(p);
			var ResultadoParam = new SqlParameter("Resultado", 0) { Direction = ParameterDirection.Output };
			dbCommand.Parameters.Add(ResultadoParam);
			db.Database.Connection.Open();
			var dataReader = dbCommand.ExecuteReader();
			var result = Convert.ToInt32(ResultadoParam.Value);
			dataReader.Close();
			db.Database.Connection.Close();
			return result;
		}

		public IEnumerable<UsuariosCategoriasGrupos> GetAll(List<DependenciasUsuario> dependencias,int? categoria)
		{
			try
			{

				IEnumerable<UsuariosCategoriasGrupos> resultado = new List<UsuariosCategoriasGrupos>();

				IEnumerable<UsuariosCategoriasGrupos> ResultadoOrganismo = new List<UsuariosCategoriasGrupos>();

				IEnumerable<UsuariosCategoriasGrupos> ResultadoPuerto = new List<UsuariosCategoriasGrupos>();

				IEnumerable<UsuariosCategoriasGrupos> ResultadoInstalacion = new List<UsuariosCategoriasGrupos>();

				if (dependencias.Any())
				{

					if (dependencias.ToList().FirstOrDefault().categoria == (int)EnumCategoria.Administrador || dependencias.ToList().FirstOrDefault().categoria == (int)EnumCategoria.PuertosdelEstado)
					{
						return GetAllUsuarios().GroupBy(c => c.Id).Select(grp => grp.First()).ToList(); 
					}
					
					switch (categoria)
					{
						case  (int)EnumCategoria.Puerto:
							//Puerto
							 var dependenciasPuerto = dependencias.Where(x => x.categoria == (int)EnumCategoria.Puerto).ToList();

							 if (dependenciasPuerto.Any())
							 {

														resultado = (from t1 in db.Usuarios.Where(x => x.categoria == (int)EnumCategoria.Puerto).ToList()
																						   join t2 in GetCategorias()  on t1.categoria equals t2.Id
																						   join t3 in this.db.Grupos on t1.id_grupo equals t3.Id
																						   join t4 in this.db.Depen_Usuario on t1.id equals t4.id_Usuario
																						   join t5 in dependenciasPuerto on t4.id_Dependencia equals t5.id_Dependencia
																						   orderby t1.login
																						   select
																							   new UsuariosCategoriasGrupos
																							   {
																								   Id            = t1.id,
																								   Login         = t1.login,
																								   Apellidos     = t1.apellidos,
																								   Email         = t1.email,
																								   IdGrupo       = t1.id_grupo,
																								   idDependencia = (t5 == null) ? 0 : (int)t5.id_Dependencia,
																								   Categoria     = t2.Name,
																								   IdCategoria   = t2.Id,
																								   Nombre        = t1.nombre,
																								   NombreGrupo   = (t1.id_grupo == null ? "No Asignado" : t3.Nombre),
																								   opp           = (t1.es_opp == true ? "Si" : "No"),
																								   opip          = (t1.es_opip == true ? "Si" : "No"),
																								   es_activo     = (t1.es_activo == true ? "Si" : "No"),
																								   fech_exp_opip = (t1.fech_exp_opip.ToString().Length > 0 ? t1.fech_exp_opip.ToString().Substring(0, 10) : string.Empty)
																							   });

															 ResultadoPuerto = resultado;
								}
								
									var dependInstalacion = dependencias.Where(x => x.categoria == (int)EnumCategoria.Instalacion).ToList();

									 if (dependInstalacion.Any())
									 {
																			 resultado = (from t1 in db.Usuarios.Where(x => x.categoria == (int)EnumCategoria.Instalacion).ToList()
																										 join t2 in GetCategorias() on t1.categoria equals t2.Id
																										 join t3 in this.db.Grupos on t1.id_grupo equals t3.Id
																										 join t4 in this.db.Depen_Usuario on t1.id equals t4.id_Usuario
																										 join t5 in dependInstalacion on t4.id_Dependencia equals t5.id_Dependencia
																										 orderby t1.login
																										 select
																											 new UsuariosCategoriasGrupos
																											 {
																												 Id            = t1.id,
																												 Login         = t1.login,
																												 Apellidos     = t1.apellidos,
																												 Email         = t1.email,
																												 IdGrupo       = t1.id_grupo,
																												 idDependencia = (t4 == null) ? 0 : (int)t4.id_Dependencia,
																												 Categoria     = t2.Name,
																												 IdCategoria   = t2.Id,
																												 Nombre        = t1.nombre,
																												 NombreGrupo   = (t1.id_grupo == null ? "No Asignado" : t3.Nombre),
																												 opp           = (t1.es_opp == true ? "Si" : "No"),
																												 opip          = (t1.es_opip == true ? "Si" : "No"),
																												 es_activo     = (t1.es_activo == true ? "Si" : "No"),
																												 fech_exp_opip = (t1.fech_exp_opip.ToString().Length > 0 ? t1.fech_exp_opip.ToString().Substring(0, 10) : string.Empty)
																											 });

																				ResultadoInstalacion = resultado;
									 }      
									
									 break;

						case (int)EnumCategoria.OrganismoGestionPortuaria:

							 var dependenciasOrganismo = dependencias.Where(x => x.categoria == (int)EnumCategoria.OrganismoGestionPortuaria).ToList();

						 //Organismo
						 if (dependenciasOrganismo.Any())
						 {
														resultado = (from t1 in db.Usuarios.Where(x => x.categoria == (int)EnumCategoria.OrganismoGestionPortuaria).ToList()
																				  join t2 in GetCategorias() on t1.categoria equals t2.Id
																				  join t3 in this.db.Grupos on t1.id_grupo equals t3.Id
																				  join t4 in this.db.Depen_Usuario on t1.id equals t4.id_Usuario
																				  join t5 in dependenciasOrganismo on t4.id_Dependencia equals t5.id_Dependencia
																				  orderby t1.login
																				  select
																					  new UsuariosCategoriasGrupos
																					  {
																						  Id            = t1.id,
																						  Login         = t1.login,
																						  Apellidos     = t1.apellidos,
																						  Email         = t1.email,
																						  IdGrupo       = t1.id_grupo,
																						  idDependencia = (t4 == null) ? 0 : (int)t5.id_Dependencia,
																						  Categoria     = t2.Name,
																						  IdCategoria   = t2.Id,
																						  Nombre        = t1.nombre,
																						  NombreGrupo   = (t1.id_grupo == null ? "No Asignado" : t3.Nombre),
																						  opp           = (t1.es_opp == true ? "Si" : "No"),
																						  opip          = (t1.es_opip == true ? "Si" : "No"),
																						  es_activo     = (t1.es_activo == true ? "Si" : "No"),
																						  fech_exp_opip = (t1.fech_exp_opip.ToString().Length > 0 ? t1.fech_exp_opip.ToString().Substring(0, 10) : string.Empty)
																					  });

							  ResultadoOrganismo = resultado;
						 }

							var _dependenciasPuerto = dependencias.Where(x => x.categoria == (int)EnumCategoria.Puerto).ToList();

							 if (_dependenciasPuerto.Any())
							 {

																	resultado = (from t1 in db.Usuarios.Where(x => x.categoria == (int)EnumCategoria.Puerto).ToList()
																						   join t2 in GetCategorias()       on t1.categoria equals t2.Id
																						   join t3 in this.db.Grupos on t1.id_grupo equals t3.Id
																						   join t4 in this.db.Depen_Usuario on t1.id equals t4.id_Usuario
																						   join t5 in _dependenciasPuerto on t4.id_Dependencia equals t5.id_Dependencia
																						   orderby t1.login
																						   select
																							   new UsuariosCategoriasGrupos
																							   {
																								   Id            = t1.id,
																								   Login         = t1.login,
																								   Apellidos     = t1.apellidos,
																								   Email         = t1.email,
																								   IdGrupo       = t1.id_grupo,
																								   idDependencia = (t5 == null) ? 0 : (int)t5.id_Dependencia,
																								   Categoria     = t2.Name,
																								   IdCategoria   = t2.Id,
																								   Nombre        = t1.nombre,
																								   NombreGrupo   = (t1.id_grupo == null ? "No Asignado" : t3.Nombre),
																								   opp           = (t1.es_opp == true ? "Si" : "No"),
																								   opip          = (t1.es_opip == true ? "Si" : "No"),
																								   es_activo     = (t1.es_activo == true ? "Si" : "No"),
																								   fech_exp_opip = (t1.fech_exp_opip.ToString().Length > 0 ? t1.fech_exp_opip.ToString().Substring(0, 10) : string.Empty)
																							   });
								  ResultadoPuerto = resultado;

								}

							var _dependenciasInstalacion = dependencias.Where(x => x.categoria == (int)EnumCategoria.Instalacion).ToList();

									 if (_dependenciasInstalacion.Any())
									 {
																			resultado = (from t1 in db.Usuarios.Where(x => x.categoria == (int)EnumCategoria.Instalacion).ToList()
																										 join t2 in GetCategorias() on t1.categoria equals t2.Id
																										 join t3 in this.db.Grupos on t1.id_grupo equals t3.Id
																										 join t4 in this.db.Depen_Usuario on t1.id equals t4.id_Usuario
																										 join t5 in _dependenciasInstalacion on t4.id_Dependencia equals t5.id_Dependencia
																										 orderby t1.login
																										 select
																											 new UsuariosCategoriasGrupos
																											 {
																												 Id            = t1.id,
																												 Login         = t1.login,
																												 Apellidos     = t1.apellidos,
																												 Email         = t1.email,
																												 IdGrupo       = t1.id_grupo,
																												 idDependencia = (t4 == null) ? 0 : (int)t4.id_Dependencia,
																												 Categoria     = t2.Name,
																												 IdCategoria   = t2.Id,
																												 Nombre        = t1.nombre,
																												 NombreGrupo   = (t1.id_grupo == null ? "No Asignado" : t3.Nombre),
																												 opp           = (t1.es_opp == true ? "Si" : "No"),
																												 opip          = (t1.es_opip == true ? "Si" : "No"),
																												 es_activo     = (t1.es_activo == true ? "Si" : "No"),
																												 fech_exp_opip = (t1.fech_exp_opip.ToString().Length > 0 ? t1.fech_exp_opip.ToString().Substring(0, 10) : string.Empty)
																											 });

									  ResultadoInstalacion = resultado;
									 }       
							break;

						case (int)EnumCategoria.Instalacion:

									 var dependenciasInstalacion = dependencias.Where(x => x.categoria == (int)EnumCategoria.Instalacion).ToList();

									 if (dependenciasInstalacion.Any())
									 {
																resultado = (from t1 in db.Usuarios.Where(x => x.categoria == (int)EnumCategoria.Instalacion).ToList()
																										 join t2 in GetCategorias() on t1.categoria equals t2.Id
																										 join t3 in this.db.Grupos on t1.id_grupo equals t3.Id
																										 join t4 in this.db.Depen_Usuario on t1.id equals t4.id_Usuario
																										 join t5 in dependenciasInstalacion on t4.id_Dependencia equals t5.id_Dependencia
																										 orderby t1.login
																										 select
																											 new UsuariosCategoriasGrupos
																											 {
																												 Id            = t1.id,
																												 Login         = t1.login,
																												 Apellidos     = t1.apellidos,
																												 Email         = t1.email,
																												 IdGrupo       = t1.id_grupo,
																												 idDependencia = (t4 == null) ? 0 : (int)t4.id_Dependencia,
																												 Categoria     = t2.Name,
																												 IdCategoria   = t2.Id,
																												 Nombre        = t1.nombre,
																												 NombreGrupo   = (t1.id_grupo == null ? "No Asignado" : t3.Nombre),
																												 opp           = (t1.es_opp == true ? "Si" : "No"),
																												 opip          = (t1.es_opip == true ? "Si" : "No"),
																												 es_activo     = (t1.es_activo == true ? "Si" : "No"),
																												 fech_exp_opip = (t1.fech_exp_opip.ToString().Length > 0 ? t1.fech_exp_opip.ToString().Substring(0, 10) : string.Empty)
																											 });

																				ResultadoInstalacion = resultado;
									 }       
			
									 break;
					}


					return ResultadoOrganismo.Union(ResultadoPuerto).Union(ResultadoInstalacion).GroupBy(c => c.Id).Select(grp => grp.First()).ToList();
				}

				return resultado;
			}
			catch (Exception ex)
			{
				this.log.PublishException(ex);
				throw;
			}

		}

		public IEnumerable<UsuariosCategoriasGrupos> GetAllUsuarios()
		{
			try
			{
				IEnumerable<UsuariosCategoriasGrupos> result = (from t1 in this.db.Usuarios.ToList()
																	join t2 in this.GetCategorias() on t1.categoria equals t2.Id
																	join t3 in this.db.Grupos on t1.id_grupo equals t3.Id
																	orderby t1.login, t1.nombre, t1.apellidos, t1.es_opip
																	select
																		new UsuariosCategoriasGrupos
																		{
																			Id            = t1.id,
																			Login         = t1.login,
																			Apellidos     = t1.apellidos,
																			Email         = t1.email,
																			IdGrupo       = t1.id_grupo,
																			Categoria     = t2.Name,
																			Nombre        = t1.nombre,
																			NombreGrupo   = t3.Nombre,
																			opp           = (t1.es_opp == true? "Si": "No"),
																			opip          = (t1.es_opip == true? "Si": "No"),
																			es_activo     = (t1.es_activo == true? "Si": "No"),
																			fech_exp_opip = (t1.fech_exp_opip.ToString().Length > 0 ? t1.fech_exp_opip.ToString().Substring(0, 10) : string.Empty)
																		});

				return result;
			}
			catch (Exception ex)
			{
				this.log.PublishException(ex);
				throw;
			}

		}
	
		public IEnumerable<Doc_Usuario_Asoc> GetDocs_Usuario(int id)
		{
			try
			{
				IEnumerable<Doc_Usuario_Asoc> result = (from t1 in this.db.Docs_Usuario.Where(x => x.id_servicio == id).ToList()
					join t2 in this.GetTiposDocumento() on t1.id_tipo_doc equals t2.id
					orderby t2.id
					select
						new Doc_Usuario_Asoc
						{
							id = t1.id,
							id_servicio = t1.id_servicio,
							id_tipo_doc = t1.id_tipo_doc,
							TipoNombre = t2.documento,
							id_tipo_serv = t1.id_tipo_serv,
							documento = t1.documento,
							descripcion = t1.descripcion,
							fech_documento = t1.fech_documento
						});

				return result;
			}
			catch (Exception ex)
			{
				this.log.PublishException(ex);
				throw;
			}
		}

		public IEnumerable<Tipos_Instalacion> GetTipos_Instalacion(int id,int? idCategoria)
		{
			try
			{
				IEnumerable<Tipos_Instalacion> result = (from t1 in this.db.Usuarios.Where(x => x.id == id && x.categoria == idCategoria).ToList()
					join t2 in this.db.Depen_Usuario.ToList() on t1.id equals t2.id_Usuario
					join t3 in this.db.MOV_Instalaciones.ToList() on t2.id_Dependencia equals t3.Id
					orderby t3.Id
					select new Tipos_Instalacion { Id = t3.Id, Descripcion = t3.Nombre });

				return result;
			}
			catch (Exception ex)
			{
				this.log.PublishException(ex);
				throw;
			}
		}

		public IEnumerable<Puertos> GetPuertos()
		{
			try
			{
				IEnumerable<Puertos> result =
					(from t1 in this.db.Puertos.Where(x => x.es_activo == true).ToList() orderby t1.Id select new Puertos { Id = t1.Id, Nombre = t1.Nombre });
				return result;
			}
			catch (Exception ex)
			{
				this.log.PublishException(ex);
				throw;
			}
		}

		public IEnumerable<PuertosViewModel> GetDependenciaPuertos(int? id, int? idCategoria)
		{
			try
			{
				IEnumerable<PuertosViewModel> result = (from t1 in this.db.Usuarios.Where(x => x.id == id && x.categoria == idCategoria).ToList()
					join t2 in this.db.Depen_Usuario.ToList() on t1.id equals t2.id_Usuario
					join t3 in this.db.Puertos.Where(x => x.es_activo == true).ToList() on t2.id_Dependencia equals t3.Id
					orderby t3.Id
					 select new PuertosViewModel { Id = t3.Id, Nombre = t3.Nombre });

				return result;
			}
			catch (Exception ex)
			{
				this.log.PublishException(ex);
				throw;
			}
		}

		public IEnumerable<Organismos> GetDependenciaOrganismos(int id, int? idCategoria)
		{
			try
			{
				IEnumerable<Organismos> result = (from t1 in this.db.Usuarios.Where(x => x.id == id && x.categoria == idCategoria).ToList()
					join t2 in this.db.Depen_Usuario.ToList() on t1.id equals t2.id_Usuario
					join t3 in this.db.Organismos.Where(x => x.es_activo == true).ToList() on t2.id_Dependencia equals t3.Id
					orderby t3.Id
					select new Organismos { Id = t3.Id, Nombre = t3.Nombre });

				return result;
			}
			catch (Exception ex)
			{
				this.log.PublishException(ex);
				throw;
			}
		}

		public IEnumerable<Organismos> GetOrganismos()
		{
			try
			{
				IEnumerable<Organismos> result =
					(from t1 in this.db.Organismos.Where(x => x.es_activo == true).ToList() orderby t1.Id select new Organismos { Id = t1.Id, Nombre = t1.Nombre });
				return result;
			}
			catch (Exception ex)
			{
				this.log.PublishException(ex);
				throw;
			}
		}

		public IEnumerable<MOV_Instalaciones> GetDependenciaInstalaciones(int id, int? idCategoria)
		{
			try
			{
				IEnumerable<MOV_Instalaciones> result = (from t1 in this.db.Usuarios.Where(x => x.id == id && x.categoria == idCategoria).ToList()
					join t2 in this.db.Depen_Usuario.ToList() on t1.id equals t2.id_Usuario
					join t3 in this.db.MOV_Instalaciones.ToList() on t2.id_Dependencia equals t3.Id
					orderby t3.Id
					select new MOV_Instalaciones { Id = t3.Id, Nombre = t3.Nombre,Clasificacion = t3.Clasificacion,id_Puerto = t3.id_Puerto});

				return result;
			}
			catch (Exception ex)
			{
				this.log.PublishException(ex);
				throw;
			}
		}
		
		public List<DependenciasUsuario> GetDependenciaUsuarios(int id_usuario, int? categoria)
		{

			List<DependenciasUsuario> dependencias_Usuarios = new List<DependenciasUsuario>();
			
			if (categoria == (int)EnumCategoria.Administrador || categoria == (int)EnumCategoria.PuertosdelEstado)
			{
				DependenciasUsuario dependenciasUsuarios = new DependenciasUsuario();
				dependenciasUsuarios.id_Usuario          = id_usuario;
				dependenciasUsuarios.categoria           = categoria;
				dependencias_Usuarios.Add(dependenciasUsuarios);
				return dependencias_Usuarios;
			}

					switch (categoria)
					{
						
						case (int)EnumCategoria.Puerto:

									this.db.Depen_Usuario.Where(x=> x.id_Usuario==id_usuario).ToList().ForEach(
									p =>
									{
									  this.db.Puertos.Where(x => x.Id == p.id_Dependencia && x.es_activo == true).ToList()
										.ForEach(
										a =>
										{
											DependenciasUsuario dependenciasUsuarios = new DependenciasUsuario();
											dependenciasUsuarios.id_Dependencia     = (int)p.id_Dependencia;
											dependenciasUsuarios.id_Usuario         = id_usuario;
											dependenciasUsuarios.categoria          = categoria;
											dependenciasUsuarios.Nombre_Dependencia = a.Nombre;
											dependencias_Usuarios.Add(dependenciasUsuarios);

											var Organismos = this.db.Organismos.Where(x => x.Id == a.id_Organismo && x.es_activo== true ).ToList();

											if (Organismos.Any())
											{
												DependenciasUsuario dependenciasOrganismos = new DependenciasUsuario();
												dependenciasOrganismos.id_Dependencia = Organismos.ToList().FirstOrDefault().Id;
												dependenciasOrganismos.id_Usuario = id_usuario;
												dependenciasOrganismos.categoria = (int)EnumCategoria.OrganismoGestionPortuaria;
												dependenciasOrganismos.Nombre_Dependencia = Organismos.ToList().FirstOrDefault().Nombre;
												dependencias_Usuarios.Add(dependenciasOrganismos);
											}

											// instalaciones
											this.db.MOV_Instalaciones.Where(x => x.id_Puerto == a.Id && x.Es_Activo == true).ToList().ForEach(
											 j =>
											 {
												 DependenciasUsuario dependenciasInstalaciones = new DependenciasUsuario();
												 dependenciasInstalaciones.id_Dependencia = j.Id;
												 dependenciasInstalaciones.id_Usuario = id_usuario;
												 dependenciasInstalaciones.categoria = (int)EnumCategoria.Instalacion;
												 dependenciasInstalaciones.Nombre_Dependencia = j.Nombre;
												 dependencias_Usuarios.Add(dependenciasInstalaciones);
											 });

										});
									});
							
								break;

						case (int)EnumCategoria.OrganismoGestionPortuaria:
							
									this.db.Depen_Usuario.Where(x=> x.id_Usuario==id_usuario).ToList().ForEach(
									p =>
									{
									this.db.Organismos.Where(x => x.Id == p.id_Dependencia && x.es_activo).ToList()
										.ForEach(
											 b =>
											 this.db.Puertos.Where(x => x.es_activo == true).ToList()
												 .ForEach(
													 t =>
													 {
														 if (t.id_Organismo == b.Id)
														 {
															 this.db.MOV_Instalaciones.Where(x => x.id_Puerto == t.Id && x.Es_Activo == true).ToList()
																 .ForEach(
																	 j =>
																	 {
																		 DependenciasUsuario dependenciasUsuarios = new DependenciasUsuario();
																		 dependenciasUsuarios.id_Dependencia = j.Id;
																		 dependenciasUsuarios.id_Usuario = id_usuario;
																		 dependenciasUsuarios.categoria = (int)EnumCategoria.Instalacion;
																		 dependenciasUsuarios.Nombre_Dependencia = j.Nombre;
																		 dependencias_Usuarios.Add(dependenciasUsuarios);
																	 });
														 
														 }

													 }));
										});
									
									this.db.Depen_Usuario.Where(x=> x.id_Usuario==id_usuario).ToList().ForEach(
									p =>
									{
									this.db.Organismos.Where(x => x.Id == p.id_Dependencia && x.es_activo).ToList()
									.ForEach(
										 b =>
										 this.db.Puertos.Where(x=> x.es_activo == true).ToList()
											 .ForEach(
												 t=> 
												 {
													 if (t.id_Organismo==b.Id)
													 {
														 DependenciasUsuario dependenciasUsuarios = new DependenciasUsuario();
														 dependenciasUsuarios.id_Dependencia     = t.Id;
														 dependenciasUsuarios.id_Usuario         = id_usuario;
														 dependenciasUsuarios.categoria          = (int)EnumCategoria.Puerto;
														 dependenciasUsuarios.Nombre_Dependencia = t.Nombre;
														 dependencias_Usuarios.Add(dependenciasUsuarios);
													 }
													
												 }));
									});
									
									this.db.Depen_Usuario.Where(x=> x.id_Usuario==id_usuario).ToList().ForEach(
									p =>
									{
									this.db.Organismos.Where(x => x.Id == p.id_Dependencia && x.es_activo).ToList()
									.ForEach(
										 b =>
										 {
											 DependenciasUsuario dependenciasUsuarios = new DependenciasUsuario();
											 dependenciasUsuarios.id_Dependencia     = (int)p.id_Dependencia;
											 dependenciasUsuarios.id_Usuario         = id_usuario;
											 dependenciasUsuarios.categoria          = (int)EnumCategoria.OrganismoGestionPortuaria;
											 dependenciasUsuarios.Nombre_Dependencia = b.Nombre;
											 dependencias_Usuarios.Add(dependenciasUsuarios);
											 
										 });

									 });

									break;

						case (int)EnumCategoria.Instalacion:
							
								this.db.Depen_Usuario.Where(x => x.id_Usuario==id_usuario).ToList().ForEach(
								p =>
								{
									this.db.MOV_Instalaciones.Where(x => x.Id == p.id_Dependencia && x.Es_Activo).ToList()
									.ForEach(
									c =>
									{
										DependenciasUsuario dependenciasUsuarios = new DependenciasUsuario();
										dependenciasUsuarios.id_Dependencia     = (int)p.id_Dependencia;
										dependenciasUsuarios.id_Usuario         = id_usuario;
										dependenciasUsuarios.categoria          = categoria;
										dependenciasUsuarios.Nombre_Dependencia = c.Nombre;
										dependencias_Usuarios.Add(dependenciasUsuarios);

										var Puertos = this.db.Puertos.Where(x => x.Id ==c.id_Puerto).ToList();

										if (Puertos.Any())
										{
											DependenciasUsuario dependenciasPuertos = new DependenciasUsuario();
											dependenciasPuertos.id_Dependencia = Puertos.ToList().FirstOrDefault().Id;
											dependenciasPuertos.id_Usuario = id_usuario;
											dependenciasPuertos.categoria = (int)EnumCategoria.Puerto;
											dependenciasPuertos.Nombre_Dependencia = Puertos.ToList().FirstOrDefault().Nombre;
											dependencias_Usuarios.Add(dependenciasPuertos);
											
											var Id_Organismo = Puertos.ToList().FirstOrDefault().id_Organismo;

											var Organismos = this.db.Organismos.Where(x => x.Id == Id_Organismo).ToList();

											if (Organismos.Any())
											{
												DependenciasUsuario dependenciasOrganismos = new DependenciasUsuario();
												dependenciasOrganismos.id_Dependencia = Organismos.ToList().FirstOrDefault().Id;
												dependenciasOrganismos.id_Usuario = id_usuario;
												dependenciasOrganismos.categoria = (int)EnumCategoria.OrganismoGestionPortuaria;
												dependenciasOrganismos.Nombre_Dependencia = Organismos.ToList().FirstOrDefault().Nombre;
												dependencias_Usuarios.Add(dependenciasOrganismos);
											}

										}
									
									
									});

								});
								
								break;
					
					}

			return dependencias_Usuarios;
		}
		
	}

	#endregion
}



 

