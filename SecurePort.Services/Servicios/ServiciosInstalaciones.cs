namespace SecurePort.Services.Interfaces
{
	#region using
	using SecurePort.Entities;
	using SecurePort.Entities.Enums;
	using SecurePort.Entities.Models;
	using System;
	using System.Collections.Generic;
	using System.Linq;

	#endregion

	public class ServiciosTipoInstalaciones : IServiciosTipoInstalacion
	{
	   #region Atributos

		private readonly ILogger log;

		protected readonly SecurePortContext db = new SecurePortContext();

		#endregion

	   #region Métodos públicos

		public ServiciosTipoInstalaciones(ILogger log)
		{
			this.log = log;
		}


		#region Tipos_Instlaciones


		public IEnumerable<ListadoTipoInstalaciones> ListTipoInstalaciones()
		{
			try
			{
				IEnumerable<ListadoTipoInstalaciones> result = (from a1 in this.db.Tipos_Instalacion.ToList()
																select new ListadoTipoInstalaciones
																	{
																		id            = a1.Id,
																		clasificacion = a1.Clasificacion,
																		Nombreclasificacion = this.GetClasifica(a1.Clasificacion),
																		descripcion   = a1.Descripcion,
																		activado      = a1.es_activo ? "Si" : "No"
																	}).ToList().OrderBy(x => x.descripcion);

				return result;

			}
			catch (Exception ex)
			{
				this.log.PublishException(ex);
				throw;
			}
		}


		public IEnumerable<ListadoTipoInstalaciones> ListTipoInstalaciones(int? id)
		{




			try
			{
				IEnumerable<ListadoTipoInstalaciones> result = (from a1 in this.db.Tipos_Instalacion.ToList()                                                               
																  select new ListadoTipoInstalaciones
																  {
																	  id          = a1.Id,
																	  clasificacion = a1.Clasificacion,
																	  descripcion = a1.Descripcion,
																	  activado    =  a1.es_activo ? "Si" : "No"
																  }).ToList().Where(x=> x.clasificacion==id).OrderBy(x => x.descripcion);
				if (result.Any())
				{
					return result;    
				}

				IEnumerable<ListadoTipoInstalaciones> resultado = (from a1 in this.db.Tipos_Instalacion.ToList()
																select new ListadoTipoInstalaciones
																{
																	id = a1.Id,
																	clasificacion = a1.Clasificacion,
																	descripcion = a1.Descripcion,
																	activado = a1.es_activo ? "Si" : "No"
																}).ToList().OrderBy(x => x.descripcion);
				return resultado;

			}
			catch (Exception ex)
			{
				this.log.PublishException(ex);
				throw;
			}
		}
		public IEnumerable<Tipos_Instalacion> ListTipoInstalaciones(string codigo)
		{
			try
			{

				var result = this.db.Tipos_Instalacion.Where(x => x.Descripcion == codigo).ToList();
				return result;

			}
			catch (Exception ex)
			{
				this.log.PublishException(ex);
				throw;
			}
		}

		public IEnumerable<Tipos_Instalacion> ListTipoInstalaciones(string nombre, int Id)
		{
			try
			{
				var result = this.db.Tipos_Instalacion.Where(x => x.Id != Id && x.Descripcion == nombre).ToList();
				return result;

			}
			catch (Exception ex)
			{
				this.log.PublishException(ex);
				throw;
			}
		}

		public bool ListTipoInstalaciones(string nombre, int Id, bool estado)
		{
			try
			{
				bool CambioEstado = false;
				if (estado == false)
				{
					IEnumerable<Tipos_Instalacion> result = this.db.Tipos_Instalacion.Where(x => x.Id == Id && x.Descripcion == nombre).ToList();
					Tipos_Instalacion TipoIns = result.FirstOrDefault();
					if (TipoIns.es_activo != estado)
						CambioEstado = true;
				}
				return CambioEstado;

			}
			catch (Exception ex)
			{
				this.log.PublishException(ex);
				throw;
			}
		}


	  #endregion


		#region Instalaciones

		public IEnumerable<InstalacionViewModel> GetAllInstalaciones()
		{
			try
			{

				IEnumerable<InstalacionViewModel> result = (from o in
																(from a in db.MOV_Instalaciones.ToList()
																 join b in this.db.Puertos.ToList() on a.id_Puerto equals b.Id into PuertoTemp
																 from bt in PuertoTemp.DefaultIfEmpty()
																 join c1 in db.Tipos_Instalacion on a.id_Tipo equals c1.Id into TipoTemp
																 from ct in TipoTemp.DefaultIfEmpty()
																 join c3 in this.db.Ciudades.ToList() on a.id_Ciudad equals c3.id into CiudadTemp
																 from dt in CiudadTemp.DefaultIfEmpty()
																 join d4 in this.db.Provincias.ToList() on a.id_provincia equals d4.id into ProvinciaTemp
																 from et in ProvinciaTemp.DefaultIfEmpty()
																 select new InstalacionViewModel
																 {
																	 Id = a.Id,
																	 Nombre = a.Nombre,
																	 id_organismo = (bt == null) ? 0 : bt.id_Organismo,
																	 Id_puerto = a.id_Puerto,
																	 NombrePuerto = (bt == null) ? string.Empty : bt.Nombre,
																	 NombreTipo = (ct == null) ? string.Empty : ct.Descripcion,
																	 es_concesionada = a.es_concesionada == 1 ? "Si" : (a.es_concesionada == 2 ? "No" : "Mixta"),
																	 Empresa = a.Empresa,
																	 NombreClasificacion = this.GetClasifica(a.Clasificacion),
																	 Direccion = a.Direccion,
																	 NombreCiudad = (dt == null) ? string.Empty : dt.nombre,
																	 cod_postal = a.cod_postal,
																	 NombreProvincia = (et == null) ? string.Empty : et.nombre,
																	 OMI = a.OMI,
																	 Nivel = a.Nivel,
																	 Longitud = a.Longitud,
																	 Latitud = a.Latitud,
																	 Declara_Cumpli = a.Declara_Cumpli ? "Si" : "No",
																	 Fech_Declara_Cumpli = a.Fech_Declara_Cumpli,
																	 Observaciones = a.Observaciones,
																	 activado = a.Es_Activo ? "Si" : "No",
																	 Es_Activo = a.Es_Activo,
																	 Clasificacion = a.Clasificacion
																 }).OrderBy(x => x.Nombre)
															join b1 in db.Organismos on o.id_organismo equals b1.Id
															join t1 in this.db.Contactos_Organismo.Where(x => x.Es_Responsable).ToList() on b1.Id equals (int?)t1.Id_Organismo into Contactos_OrganismoTemp
															from c1 in Contactos_OrganismoTemp.DefaultIfEmpty()
															select
																new InstalacionViewModel
																{
																	Id = o.Id,
																	Nombre = o.Nombre,
																	id_organismo = o.id_organismo,
																	Id_puerto = o.Id_puerto,
																	NombrePuerto = o.NombrePuerto,
																	NombreTipo = o.NombreTipo,
																	es_concesionada = o.es_concesionada,
																	Empresa = o.Empresa,
																	NombreClasificacion = o.NombreClasificacion,
																	Direccion = o.Direccion,
																	NombreCiudad = o.NombreCiudad,
																	cod_postal = o.cod_postal,
																	NombreProvincia = o.NombreProvincia,
																	Telefono = (c1 == null) ? string.Empty : c1.Telefono,
																	Fax = (c1 == null) ? string.Empty : c1.Fax,
																	email = (c1 == null) ? string.Empty : c1.Email,
																	OMI = o.OMI,
																	Nivel = o.Nivel,
																	Longitud = o.Longitud,
																	Latitud = o.Latitud,
																	Declara_Cumpli = o.Declara_Cumpli,
																	Fech_Declara_Cumpli = o.Fech_Declara_Cumpli,
																	Observaciones = o.Observaciones,
																	activado = o.activado,
																	Es_Activo = o.Es_Activo,
																	Organismo = b1.Nombre,
																	Clasificacion = o.Clasificacion
																}).OrderBy(x => x.Nombre);

				return result;
			}
			catch (Exception ex)
			{
				this.log.PublishException(ex);
				throw;
			}

		}

		public IEnumerable<InstalacionViewModel> GetAllInstalaciones(List<DependenciasUsuario> dependencias,int? categoria)
		{
			try
			{

				
				IEnumerable<InstalacionViewModel> resultado = new List<InstalacionViewModel>();

				if (categoria == (int)EnumCategoria.Puerto)
				{
					var dependenciasPuerto = dependencias.Where(x => x.categoria == (int)EnumCategoria.Puerto).ToList();

					if (dependenciasPuerto.Any())
					{

						resultado = (from o in
										 (from a in db.MOV_Instalaciones.ToList()
										  join b in this.db.Puertos.ToList() on a.id_Puerto equals b.Id into PuertoTemp
										  from bt in PuertoTemp.DefaultIfEmpty()
										  join c1 in db.Tipos_Instalacion on a.id_Tipo equals c1.Id into TipoTemp
										  from ct in TipoTemp.DefaultIfEmpty()
										  join ww in dependenciasPuerto.ToList() on a.id_Puerto equals ww.id_Dependencia
										  join c3 in this.db.Ciudades.ToList() on a.id_Ciudad equals c3.id into CiudadTemp
										  from dt in CiudadTemp.DefaultIfEmpty()
										  join d4 in this.db.Provincias.ToList() on a.id_provincia equals d4.id into ProvinciaTemp
										  from et in ProvinciaTemp.DefaultIfEmpty()
										  select new InstalacionViewModel
										  {
											  Id = a.Id,
											  Nombre = a.Nombre,
											  id_organismo = (bt == null) ? 0 : bt.id_Organismo,
											  Id_puerto = a.id_Puerto,
											  NombrePuerto = (bt == null) ? string.Empty : bt.Nombre,
											  NombreTipo = (ct == null) ? string.Empty : ct.Descripcion,
											  es_concesionada = a.es_concesionada == 1 ? "Si" : (a.es_concesionada == 2 ? "No" : "Mixta"),
											  Empresa = a.Empresa,
											  NombreClasificacion = this.GetClasifica(a.Clasificacion),
											  Direccion = a.Direccion,
											  NombreCiudad = (dt == null) ? string.Empty : dt.nombre,
											  cod_postal = a.cod_postal,
											  NombreProvincia = (et == null) ? string.Empty : et.nombre,
											  OMI = a.OMI,
											  Nivel = a.Nivel,
											  Longitud = a.Longitud,
											  Latitud = a.Latitud,
											  Declara_Cumpli = a.Declara_Cumpli ? "Si" : "No",
											  Fech_Declara_Cumpli = a.Fech_Declara_Cumpli,
											  Observaciones = a.Observaciones,
											  activado = a.Es_Activo ? "Si" : "No",
											  Es_Activo = a.Es_Activo,
											  Clasificacion= a.Clasificacion
										  }).OrderBy(x => x.Nombre)
									 join b1 in db.Organismos on o.id_organismo equals b1.Id
									 join t1 in this.db.Contactos_Organismo.Where(x => x.Es_Responsable).ToList() on b1.Id equals (int?)t1.Id_Organismo into Contactos_OrganismoTemp
									 from c1 in Contactos_OrganismoTemp.DefaultIfEmpty()
									 select
										 new InstalacionViewModel
										 {
											 Id = o.Id,
											 Nombre = o.Nombre,
											 id_organismo = o.id_organismo,
											 Id_puerto = o.Id_puerto,
											 NombrePuerto = o.NombrePuerto,
											 NombreTipo = o.NombreTipo,
											 es_concesionada = o.es_concesionada,
											 Empresa = o.Empresa,
											 NombreClasificacion = o.NombreClasificacion,
											 Direccion = o.Direccion,
											 NombreCiudad = o.NombreCiudad,
											 cod_postal = o.cod_postal,
											 NombreProvincia = o.NombreProvincia,
											 Telefono = (c1 == null) ? string.Empty : c1.Telefono,
											 Fax = (c1 == null) ? string.Empty : c1.Fax,
											 email = (c1 == null) ? string.Empty : c1.Email,
											 OMI = o.OMI,
											 Nivel = o.Nivel,
											 Longitud = o.Longitud,
											 Latitud = o.Latitud,
											 Declara_Cumpli = o.Declara_Cumpli,
											 Fech_Declara_Cumpli = o.Fech_Declara_Cumpli,
											 Observaciones = o.Observaciones,
											 activado = o.activado,
											 Organismo = b1.Nombre,
											 Es_Activo = o.Es_Activo,
											 Clasificacion = o.Clasificacion
										 }).OrderBy(x => x.Nombre);
					}

				}
				else if (categoria == (int)EnumCategoria.OrganismoGestionPortuaria)
				{
					var dependenciasOrganismo = dependencias.Where(x => x.categoria == (int)EnumCategoria.OrganismoGestionPortuaria).ToList();

					if (dependenciasOrganismo.Any())
					{


						resultado = (from o in
										 (from a in db.MOV_Instalaciones.ToList()
										  join b in this.db.Puertos.ToList() on a.id_Puerto equals b.Id into PuertoTemp
										  from bt in PuertoTemp.DefaultIfEmpty()
										  join c1 in db.Tipos_Instalacion on a.id_Tipo equals c1.Id into TipoTemp
										  from ct in TipoTemp.DefaultIfEmpty()
										  join ww in dependenciasOrganismo.ToList() on bt.id_Organismo equals ww.id_Dependencia
										  join c3 in this.db.Ciudades.ToList() on a.id_Ciudad equals c3.id into CiudadTemp
										  from dt in CiudadTemp.DefaultIfEmpty()
										  join d4 in this.db.Provincias.ToList() on a.id_provincia equals d4.id into ProvinciaTemp
										  from et in ProvinciaTemp.DefaultIfEmpty()
										  select new InstalacionViewModel
										  {
											  Id = a.Id,
											  Nombre = a.Nombre,
											  id_organismo = (bt == null) ? 0 : bt.id_Organismo,
											  Id_puerto = a.id_Puerto,
											  NombrePuerto = (bt == null) ? string.Empty : bt.Nombre,
											  NombreTipo = (ct == null) ? string.Empty : ct.Descripcion,
											  es_concesionada = a.es_concesionada == 1 ? "Si" : (a.es_concesionada == 2 ? "No" : "Mixta"),
											  Empresa = a.Empresa,
											  NombreClasificacion = this.GetClasifica(a.Clasificacion),
											  Direccion = a.Direccion,
											  NombreCiudad = (dt == null) ? string.Empty : dt.nombre,
											  cod_postal = a.cod_postal,
											  NombreProvincia = (et == null) ? string.Empty : et.nombre,
											  OMI = a.OMI,
											  Nivel = a.Nivel,
											  Longitud = a.Longitud,
											  Latitud = a.Latitud,
											  Declara_Cumpli = a.Declara_Cumpli ? "Si" : "No",
											  Fech_Declara_Cumpli = a.Fech_Declara_Cumpli,
											  Observaciones = a.Observaciones,
											  activado = a.Es_Activo ? "Si" : "No",
											  Es_Activo = a.Es_Activo,
											  Clasificacion = a.Clasificacion
										  }).OrderBy(x => x.Nombre)
									 join b1 in db.Organismos on o.id_organismo equals b1.Id
									 join t1 in this.db.Contactos_Organismo.Where(x => x.Es_Responsable).ToList() on b1.Id equals (int?)t1.Id_Organismo into Contactos_OrganismoTemp
									 from c1 in Contactos_OrganismoTemp.DefaultIfEmpty()
									 select
										 new InstalacionViewModel
										 {
											 Id = o.Id,
											 Nombre = o.Nombre,
											 id_organismo = o.id_organismo,
											 Id_puerto = o.Id_puerto,
											 NombrePuerto = o.NombrePuerto,
											 NombreTipo = o.NombreTipo,
											 es_concesionada = o.es_concesionada,
											 Empresa = o.Empresa,
											 NombreClasificacion = o.NombreClasificacion,
											 Direccion = o.Direccion,
											 NombreCiudad = o.NombreCiudad,
											 cod_postal = o.cod_postal,
											 NombreProvincia = o.NombreProvincia,
											 Telefono = (c1 == null) ? string.Empty : c1.Telefono,
											 Fax = (c1 == null) ? string.Empty : c1.Fax,
											 email = (c1 == null) ? string.Empty : c1.Email,
											 OMI = o.OMI,
											 Nivel = o.Nivel,
											 Longitud = o.Longitud,
											 Latitud = o.Latitud,
											 Declara_Cumpli = o.Declara_Cumpli,
											 Fech_Declara_Cumpli = o.Fech_Declara_Cumpli,
											 Observaciones = o.Observaciones,
											 activado = o.activado,
											 Organismo = b1.Nombre,
											 Es_Activo = o.Es_Activo,
											 Clasificacion = o.Clasificacion
										 }).OrderBy(x => x.Nombre);
					}

				}
				else if (categoria == (int)EnumCategoria.Instalacion)
				{
					var dependenciasInstala = dependencias.Where(x => x.categoria == (int)EnumCategoria.Instalacion).ToList();

					if (dependenciasInstala.Any())
					{


						resultado = (from o in
										 (from a in db.MOV_Instalaciones.ToList()
										  join b in this.db.Puertos.ToList() on a.id_Puerto equals b.Id into PuertoTemp
										  from bt in PuertoTemp.DefaultIfEmpty()
										  join c1 in db.Tipos_Instalacion on a.id_Tipo equals c1.Id into TipoTemp
										  from ct in TipoTemp.DefaultIfEmpty()
										  join ww in dependenciasInstala.ToList() on a.Id equals ww.id_Dependencia
										  join c3 in this.db.Ciudades.ToList() on a.id_Ciudad equals c3.id into CiudadTemp
										  from dt in CiudadTemp.DefaultIfEmpty()
										  join d4 in this.db.Provincias.ToList() on a.id_provincia equals d4.id into ProvinciaTemp
										  from et in ProvinciaTemp.DefaultIfEmpty()
										  select new InstalacionViewModel
										  {
											  Id = a.Id,
											  Nombre = a.Nombre,
											  id_organismo = (bt == null) ? 0 : bt.id_Organismo,
											  Id_puerto = a.id_Puerto,
											  NombrePuerto = (bt == null) ? string.Empty : bt.Nombre,
											  NombreTipo = (ct == null) ? string.Empty : ct.Descripcion,
											  es_concesionada = a.es_concesionada == 1 ? "Si" : (a.es_concesionada == 2 ? "No" : "Mixta"),
											  Empresa = a.Empresa,
											  NombreClasificacion = this.GetClasifica(a.Clasificacion),
											  Direccion = a.Direccion,
											  NombreCiudad = (dt == null) ? string.Empty : dt.nombre,
											  cod_postal = a.cod_postal,
											  NombreProvincia = (et == null) ? string.Empty : et.nombre,
											  OMI = a.OMI,
											  Nivel = a.Nivel,
											  Longitud = a.Longitud,
											  Latitud = a.Latitud,
											  Declara_Cumpli = a.Declara_Cumpli ? "Si" : "No",
											  Fech_Declara_Cumpli = a.Fech_Declara_Cumpli,
											  Observaciones = a.Observaciones,
											  activado = a.Es_Activo ? "Si" : "No",
											  Es_Activo = a.Es_Activo,
											  Clasificacion = a.Clasificacion
										  }).OrderBy(x => x.Nombre)
									 join b1 in db.Organismos on o.id_organismo equals b1.Id
									 join t1 in this.db.Contactos_Organismo.Where(x => x.Es_Responsable).ToList() on b1.Id equals (int?)t1.Id_Organismo into Contactos_OrganismoTemp
									 from c1 in Contactos_OrganismoTemp.DefaultIfEmpty()
									 select
										 new InstalacionViewModel
										 {
											 Id = o.Id,
											 Nombre = o.Nombre,
											 id_organismo = o.id_organismo,
											 Id_puerto = o.Id_puerto,
											 NombrePuerto = o.NombrePuerto,
											 NombreTipo = o.NombreTipo,
											 es_concesionada = o.es_concesionada,
											 Empresa = o.Empresa,
											 NombreClasificacion = o.NombreClasificacion,
											 Direccion = o.Direccion,
											 NombreCiudad = o.NombreCiudad,
											 cod_postal = o.cod_postal,
											 NombreProvincia = o.NombreProvincia,
											 Telefono = (c1 == null) ? string.Empty : c1.Telefono,
											 Fax = (c1 == null) ? string.Empty : c1.Fax,
											 email = (c1 == null) ? string.Empty : c1.Email,
											 OMI = o.OMI,
											 Nivel = o.Nivel,
											 Longitud = o.Longitud,
											 Latitud = o.Latitud,
											 Declara_Cumpli = o.Declara_Cumpli,
											 Fech_Declara_Cumpli = o.Fech_Declara_Cumpli,
											 Observaciones = o.Observaciones,
											 activado = o.activado,
											 Organismo = b1.Nombre,
											 Es_Activo = o.Es_Activo,
											 Clasificacion = o.Clasificacion
										 }).OrderBy(x => x.Nombre);
					}

				}
				
				

				return resultado.GroupBy(c => c.Id).Select(grp => grp.First()).ToList(); 
			}
			catch (Exception ex)
			{
				this.log.PublishException(ex);
				throw;
			}

		}

		
		public IEnumerable<MOV_Instalaciones> ListMOV_InstalacionesVerifica(string nombre, int IdPuerto)
		{
			try
			{

				var result = this.db.MOV_Instalaciones.Where(x => x.Nombre == nombre && x.id_Puerto == IdPuerto).ToList();
				return result;

			}
			catch (Exception ex)
			{
				this.log.PublishException(ex);
				throw;
			}
		}

		public IEnumerable<MOV_Instalaciones> ListMOV_Instalaciones(string nombre, int Id)
		{
			try
			{
				var result = this.db.MOV_Instalaciones.Where(x => x.Id != Id && x.Nombre == nombre).ToList();
				return result;

			}
			catch (Exception ex)
			{
				this.log.PublishException(ex);
				throw;
			}
		}

		public bool ListMOV_Instalaciones(string nombre, int Id, bool estado)
		{
			try
			{
				bool CambioEstado = false;
				if (estado == false)
				{
					foreach (MOV_Instalaciones mov_Instalaciones in this.db.MOV_Instalaciones.Where(x => x.Id == Id && x.Nombre == nombre).ToList())
					{
						if (mov_Instalaciones.Es_Activo)
							CambioEstado = true;
					}
				}
				return CambioEstado;

			}
			catch (Exception ex)
			{
				this.log.PublishException(ex);
				throw;
			}
		}

		public IEnumerable<MOV_Instalaciones> ListMOV_Instalaciones(int? Id)
		{
			try
			{
				var result = this.db.MOV_Instalaciones.Where(x => x.Id == Id ).ToList();
				return result;

			}
			catch (Exception ex)
			{
				this.log.PublishException(ex);
				throw;
			}
		}

		#endregion
	
		#region Operadores
		public IEnumerable<OperadoresViewModel> GetAllOperadores(int? id_instalcion)
		{
			try
			{
				IEnumerable<OperadoresViewModel> result = (from a1 in this.db.Operadores.ToList().Where(t => t.Id_Instalacion == id_instalcion)
														   join b2 in this.db.MOV_Instalaciones.ToList().Where(x => x.Es_Activo) on a1.Id_Instalacion equals b2.Id into InstalacionTemp
														   from instala in InstalacionTemp.DefaultIfEmpty()
														   join d4 in this.db.Ciudades.ToList() on a1.Id_Ciudad equals d4.id into CiuTemp
														   from ciu in CiuTemp.DefaultIfEmpty()
														   join e5 in this.db.Provincias.ToList() on a1.Id_provincia equals e5.id into ProTemp
														   from pro in ProTemp.DefaultIfEmpty()
																select new OperadoresViewModel
																{
																	Id = a1.Id,
																	Id_Instalacion = a1.Id_Instalacion,
																	Instalacion = (instala == null) ? "" : instala.Nombre,
																	Nombre = a1.Nombre,                                                                    
																	Es_Suplente = a1.Es_Suplente,
																	activado_Sup = a1.Es_Suplente == true? "Si": "No",
																	Direccion = a1.Direccion,
																	Id_Ciudad = a1.Id_Ciudad,
																	Ciudad = (ciu == null) ? "" : ciu.nombre,
																	Cod_postal = a1.Cod_postal,
																	Id_provincia = a1.Id_provincia,
																	Provincia = (pro == null) ? "" : pro.nombre,
																	Telefono1 = a1.Telefono1,
																	Fax = a1.Fax,
																	Email = a1.Email,
																	Id_usu_alta = a1.Id_usu_alta,
																	Fech_alta = a1.Fech_alta,
																	Es_Activo = a1.Es_Activo,
																	activado =  a1.Es_Activo == true ? "Si" : "No",
																	Fech_activo = a1.Fech_activo,
																	Observaciones = a1.Observaciones,
																	Cargo = a1.Cargo
																}).ToList().OrderBy(x => x.Nombre);
				return result;

			}
			catch (Exception ex)
			{
				this.log.PublishException(ex);
				throw;
			}
		}

		public IEnumerable<OperadoresViewModel> ListOperadores(int? id_instalcion)
		{
			try
			{
				IEnumerable<OperadoresViewModel> result = (from a1 in this.db.Operadores.ToList().Where(t => t.Id_Instalacion == id_instalcion)
														   join b2 in this.db.MOV_Instalaciones.ToList().Where(x => x.Es_Activo) on a1.Id_Instalacion equals b2.Id into InstalacionTemp
														   from instala in InstalacionTemp.DefaultIfEmpty()
														   select new OperadoresViewModel
														   {
															   Id = a1.Id,
															   Id_Instalacion = a1.Id_Instalacion,
															   Instalacion = (instala == null) ? "" : instala.Nombre,
															   Nombre = a1.Nombre,
															   Es_Suplente = a1.Es_Suplente,
															   activado_Sup = a1.Es_Suplente == true ? "Si" : "No",
															   Direccion = a1.Direccion,
															   Id_Ciudad = a1.Id_Ciudad,
															   Cod_postal = a1.Cod_postal,
															   Id_provincia = a1.Id_provincia,
															   Telefono1 = a1.Telefono1,
															   Fax = a1.Fax,
															   Email = a1.Email,
															   Id_usu_alta = a1.Id_usu_alta,
															   Fech_alta = a1.Fech_alta,
															   Es_Activo = a1.Es_Activo,
															   activado =  a1.Es_Activo == true ? "Si" : "No",
															   Fech_activo = a1.Fech_activo,
															   Observaciones = a1.Observaciones,
															   Cargo = a1.Cargo
														   }).ToList().OrderBy(x => x.Nombre);
				return result;

			}
			catch (Exception ex)
			{
				this.log.PublishException(ex);
				throw;
			}
		}


		#endregion

		#region Instalaciones/bienes/amenazas

		public IEnumerable<ListadoTipoInstalaciones> ListInstalaciones(int? id)
		{
			try
			{
				IEnumerable<ListadoTipoInstalaciones> result = (from o in this.db.MOV_Instalaciones.Where(x => x.Id == id).ToList()
																select
																	new ListadoTipoInstalaciones
																	{
																		Nombreclasificacion = this.GetClasifica(o.Clasificacion),
																	}).ToList().OrderBy(x => x.descripcion);

				 return result;
			}
			catch (Exception ex)
			{
				this.log.PublishException(ex);
				throw;
			}

			  
		}

		public IEnumerable<MOV_Detalle_InstalacionViewModel> GetAllBienesByInstalacion(int idInstalacion)
		{
			try
			{
				IEnumerable<MOV_Detalle_InstalacionViewModel> BienesAmenazas = new List<MOV_Detalle_InstalacionViewModel>();

				BienesAmenazas = (from ini in
									(from o in db.MOV_Detalle_Instalacion.Where(x => x.ID_Instalacion == idInstalacion).ToList()
									  join insta in db.MOV_Instalaciones.ToList() on o.ID_Instalacion equals insta.Id into InstaTemp
									  from ints in InstaTemp.DefaultIfEmpty()
									  join bi in db.Bienes.ToList() on o.ID_Bien equals bi.Id
										select new MOV_Detalle_InstalacionViewModel
										{
											Id = o.Id,
											Id_Bien = o.ID_Bien,
											Bien = bi.Descripcion,
											Id_BienPadre = bi.id_Bien_Padre,                                          
											ID_Amenaza = o.ID_Amenaza,
											IIPP = (ints == null)? "": ints.Nombre
										})
								  join biP in db.Bienes.ToList() on ini.Id_BienPadre equals biP.Id into BienTemp
								  from bp in BienTemp.DefaultIfEmpty()
								  select new MOV_Detalle_InstalacionViewModel
								  {
									  Id = ini.Id,
									  Id_Bien = ini.Id_Bien,
									  Bien = ini.Bien,
									  Id_BienPadre = ini.Id_BienPadre,
									  BienPadre = (bp == null) ? "" : bp.Descripcion,
									  ID_Amenaza = ini.ID_Amenaza,
									  IIPP = ini.IIPP
								  });

				BienesAmenazas = (from cust in BienesAmenazas
								  group cust by cust.Id_Bien into grpBien
								  select new MOV_Detalle_InstalacionViewModel
								  {
									  Id = grpBien.FirstOrDefault().Id,
									  Id_Bien = grpBien.FirstOrDefault().Id_Bien,
									  Bien = grpBien.FirstOrDefault().Bien,
									  Id_BienPadre = grpBien.FirstOrDefault().Id_BienPadre,
									  BienPadre = grpBien.FirstOrDefault().BienPadre,
									  IIPP = grpBien.FirstOrDefault().IIPP,
									  NumeroAmenazas = grpBien.Where(x => x.ID_Amenaza != null).Select(x => x.ID_Amenaza).Distinct().Count()
								  });

				return BienesAmenazas;

			}
			catch (Exception ex)
			{
				this.log.PublishException(ex);
				throw;
			}
				
		}


		public IEnumerable<AmenazasViewModel> GetAllBienesByInstalacionBien(int idInstalacion, int idBien)
		{
			try
			{
				IEnumerable<AmenazasViewModel> amenazas = new List<AmenazasViewModel>();
				amenazas = (from o in db.MOV_Detalle_Instalacion.Where(x => x.ID_Instalacion == idInstalacion && x.ID_Bien == idBien).ToList()
							 join am in db.Amenazas.ToList() on o.ID_Amenaza equals am.Id
							 select new AmenazasViewModel
							 {
								 Id = am.Id,
								 Descripcion = am.Descripcion
							 });

				return amenazas;
			}
			catch (Exception ex)
			{
				this.log.PublishException(ex);
				throw;
			}
				
		}
		#endregion

	   #endregion

	   #region Métodos privados

		public string GetClasifica(int? id)
		{
			try
			{
				var nombreclasifica = string.Empty;

				IEnumerable<ListClasificacion> ListaClasificacion = (IEnumerable<ListClasificacion>)new ListClasificacion[]
						{
							new ListClasificacion { Value = "1", Text = "725" }, 
							new ListClasificacion { Value = "2", Text = "SEVESO" },
							new ListClasificacion { Value = "3", Text = "Alto Riesgo" }, 
							new ListClasificacion { Value = "4", Text = "Bajo Riesgo" },
							new ListClasificacion { Value = "5", Text = "Zonas Adyacentes" }, 
							new ListClasificacion { Value = "6", Text = "Tráficos Esporádicos" },
						};
				
				ListaClasificacion = ListaClasificacion.Where(x => x.Value == id.ToString());

				foreach (ListClasificacion listClasificacion in ListaClasificacion)
				{
					nombreclasifica = listClasificacion.Text;
				}

				return nombreclasifica;
			}
			catch (Exception ex)
			{
				this.log.PublishException(ex);
				throw;
			}
		}
		#endregion
		
		
	}
}
