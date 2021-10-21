namespace SecurePort.Entities.Models
{
    #region using
    using System.ComponentModel.DataAnnotations;
    #endregion

    public class AltaAcciones
    {
        public int Id { get; set; }

        public string Nombre { get; set; }

        public int idConjunto { get; set; }

        public bool alta { get; set; }

        public string NombreAccion { get; set; }

        public bool asignar { get; set; }

        public string origen { get; set; } // 1 sin asignas, 2 asignados
        
    }
}
