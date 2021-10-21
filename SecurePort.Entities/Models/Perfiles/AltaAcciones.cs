namespace SecurePort.Entities.Models
{
    #region using
    using System.ComponentModel.DataAnnotations;
    #endregion

    public class AltaPerfiles
    {
        public int Id { get; set; }

        public string Nombre { get; set; }

        public string Description  { get; set; }

        public bool alta { get; set; }

        public bool asignar { get; set; }
    }
}
