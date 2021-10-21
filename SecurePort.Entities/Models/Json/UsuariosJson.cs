namespace SecurePort.Entities.Models.Json
{
    #region Using
    using System;
    #endregion
    
    public class UsuariosJson
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public string Email { get; set; }
        public string password_ { get; set; }
        public string password_again_ { get; set; }
        public string Observaciones { get; set; }
        public int categoria { get; set; }
        public int grupo { get; set; }
        public string fech_exp_OPIP { get; set; }
        public string opp { get; set; }
        public string empresa { get; set; }
    }
}
