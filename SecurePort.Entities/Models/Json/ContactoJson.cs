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
using System;
namespace SecurePort.Entities.Models
{
    public class ContactoJson
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Telefono { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string Observaciones { get; set; }
        public string Cargo { get; set; }
        public bool es_activo { get; set; }
        public string Es_responsable { get; set; }
       
    }
}