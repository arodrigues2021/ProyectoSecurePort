using System.Collections.Generic;
using SecurePort.Entities.Models;

namespace SecurePort.Services.Interfaces
{
    public interface IServiciosOrganismo
    {
        IEnumerable<ListadoOrganismo> GetAllOrganismos();

        IEnumerable<ListadoOrganismo> GetAllOrganismos(List<DependenciasUsuario> dependencias,int? categoria);

        IEnumerable<ContactosViewModel> GetAllContactos_Organismo(int? id);

        IEnumerable<TipoOrganismo> GetTipo();

        bool NombreOrganismo(string Nombre);

        bool NombreOrganismo(string Nombre, int? ID);

        IEnumerable<OrganismosViewModel> ListOrganismos();

        IEnumerable<Organismos> ListOrganismo(string nombre);

        IEnumerable<Organismos> ListOrganismo(string nombre, int Id);

        bool ListOrganismo(string nombre, int Id, bool estado);

        IEnumerable<Comunidades_Autonomas> ListComunidades(int? Id);

        IEnumerable<OrganismosViewModel> ListOrganismos(List<DependenciasUsuario> dependencias);
    }
}