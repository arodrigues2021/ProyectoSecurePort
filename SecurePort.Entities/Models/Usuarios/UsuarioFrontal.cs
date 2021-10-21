namespace SecurePort.Entities.Models
{
    #region using

    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System;
    using System.IO;

    #endregion
    
    public class UsuarioFrontal
    {
        public Usuarios Usuario                                                   { get; set; }
        public Usuarios EditUsuario                                               { get; set; }
        public IEnumerable<ListGruposPerfilesPermisos> ListGruposPerfilesPermisos { get; set; }
        public List<ListGruposPerfilesPermisos> ListMaestros                      { get; set; }
        public string SupportedBrowser                                            { get; set; }
        public string fichero                                                     { get; set; }
        public string Path                                                        { get; set; }
        public string Path1                                                       { get; set; }
        public bool Usermenu                                                      { get; set; }
        public string version                                                     { get; set; }
        public string nombre                                                      { get; set; }
        public string descripcion                                                 { get; set; }
        public Boolean valor                                                      { get; set; }
        public PermisosViewModel permisosViewModel                                { get; set; }
        public List<DependenciasUsuario> ListDependenciasDepenUsuarios            { get; set; }
        public int? Id_publico                                                    { get; set; }
        public bool? activo                                                        { get; set; }
    }
}
