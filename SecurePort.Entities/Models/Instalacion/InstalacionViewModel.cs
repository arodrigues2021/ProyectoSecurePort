namespace SecurePort.Entities.Models
{
    #region using

    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    #endregion

    public class InstalacionViewModel
    {

        public int Id { get; set; }

        public string Nombre { get; set; }

        public int id_organismo { get; set; }

        public string NombrePuerto { get; set; }

        public string NombreTipo { get; set; }

        public string es_concesionada { get; set; }

        public string Empresa { get; set; }

        public string NombreClasificacion { get; set; }

        public string Direccion { get; set; }

        public string NombreCiudad { get; set; }
        
        public int? cod_postal { get; set; }

        public string NombreProvincia { get; set; }

        public string Telefono { get; set; }

        public string Fax { get; set; }

        public string OMI { get; set; }

        public string Nivel { get; set; }

        public int? Longitud { get; set; }

        public int? Latitud { get; set; }

        public string Declara_Cumpli { get; set; }

        public DateTime?  Fech_Declara_Cumpli  { get; set; }
        
        public string Observaciones { get; set; }

        public string activado { get; set; }

        public string email { get; set; }

        public int? Id_puerto { get; set; }

        public Boolean Es_Activo { get; set; }

        public string Organismo { get; set; }

        public int Clasificacion { get; set; }

    }
}
