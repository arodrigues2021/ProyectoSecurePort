namespace SecurePort.Entities.Models
{
    public class UsuariosCategoriasGrupos
    {

        public int Id { get; set; }

        public string Login { get; set; }

        public int? IdCategoria { get; set; }

        public string Categoria { get; set; }

        public int? IdGrupo { get; set; }

        public int idDependencia { get; set; }

        public string Nombre { get; set; }

        public string Apellidos { get; set; }

        public string Email { get; set; }

        public string NombreGrupo { get; set; }

        public string opp { get; set; }

        public string opip { get; set; }

        public string es_activo { get; set; }

        public string fech_exp_opip { get; set; }

        public string correo { get; set; }

    }
}
