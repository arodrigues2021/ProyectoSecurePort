
namespace SecurePort.Entities.Models
{

    public class PuertosViewModel
    {
        public int Id { get; set; }

        public string Nombre { get; set; }

        public bool asignar { get; set; }

        public int id_Organismo { get; set; }

        public string Organismo { get; set; }

        public string Responsable { get; set; }

        public string Direccion { get; set; }

        public int? ID_Ciudad { get; set; }

        public string Ciudad { get; set; }

        public int? Cod_Postal { get; set; }

        public int? ID_Provincia { get; set; }

        public string Provincia { get; set; }

        public string Observaciones { get; set; }

        public int? Latitud { get; set; }

        public int? Longitud { get; set; }

        public string Locode { get; set; }

        public int? Id_CapMarit { get; set; }

        public string Capitania { get; set; }

        public string activado { get; set; }

        public bool? es_activo { get; set; }

        public int? Id_Isla { get; set; }

        public string Isla { get; set; }

        public string IIPP { get; set; }

        public string comunidad { get; set; }

        public string tipo { get; set; }

        public int ID_ComAut { get; set; }


    }
}

