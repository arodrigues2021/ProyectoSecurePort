#region Cabecera
// ------------------------------------------------------------------------------------------------
// <copyright archivo="OrganismoViewModel.cs" >
// Jedren 2016
// </copyright>
// <summary>
// Descripcion:
// </summary>
// Usuario: Armando Rodriguez Mano
// Creado: 24-02-2016 9:13
// Modificado: 24-02-2016 9:13
// ------------------------------------------------------------------------------------------------
#endregion
namespace SecurePort.Entities.Models
{
    public class OrganismoViewModel
    {
        public int Id              { get; set; }
        public string Organismo    { get; set; }
        public string Tipo         { get; set; }
        public string Responsable  { get; set; }
        public string Direccion    { get; set; }
        public string Ciudad       { get; set; }
        public string Postal       { get; set; }
        public string Provincia    { get; set; }
        public string Comunidad    { get; set; }
        public bool   activo       { get; set; }
        public string activado     { get; set; }
       
    }
}