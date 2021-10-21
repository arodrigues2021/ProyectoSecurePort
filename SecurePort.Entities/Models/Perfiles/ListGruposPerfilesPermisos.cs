namespace SecurePort.Entities.Models
{
   public class ListGruposPerfilesPermisos
    {

        public int IdGrupo { get; set; }

        public string Nombre { get; set; }

        public int IdPerfil { get; set; }

        public int IdPermiso { get; set; }

        public string NombrePermiso { get; set; }

        public int id_grupo_accion { get; set; }

        public int? orden { get; set; }
    }
}
