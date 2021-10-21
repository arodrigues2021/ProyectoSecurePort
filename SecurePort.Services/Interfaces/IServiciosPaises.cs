using System.Collections.Generic;
using SecurePort.Entities.Models;

namespace SecurePort.Services.Interfaces
{
    public interface IServiciosPaisesProvincias
    {
        IEnumerable<ListPaises> GetAllPaises();

        IEnumerable<ListadoProvincias> GetAllProvincias(int ID_ComAut);

        IEnumerable<ListadoProvincias> ListProvincias();

        IEnumerable<Comunidades_Autonomas> ListComunidades();

        IEnumerable<Comunidades_Autonomas> ListComunidades(string nombre);

        IEnumerable<Comunidades_Autonomas> ListComunidades(string nombre, int Id);

        bool ListComunidades(string nombre, int Id, bool estado);

        IEnumerable<ListadoCiudades> ListCiudades();

        IEnumerable<ListCapitanias> ListCapitanias();

        IEnumerable<Ciudades> ListCiudades(string Codigo, int Id_provincia);

        IEnumerable<Paises> ListPaises(string codigo);

        IEnumerable<Ciudades> ListCiudades(string Codigo, int ID,  int Id_provincia);

        IEnumerable<Paises> ListPaises(string codigo, int ID);

        bool GetIdProvincias(string Codigo);

        bool GetIdProvincias(string Codigo, int ID);

        bool ListCapitanias(string Nombre);

        bool ListCapitanias(string Nombre, int Id);

        IEnumerable<Provincias> ListProvincias(string Nombre);

        bool ListProvincias(string nombre, int Id, bool estado);

        IEnumerable<Provincias> ListProvincias(string nombre, int Id);

        bool ListCapitanias(int Id,string Nombre);

        bool ListCiudades(string nombre, int Id, bool estado);

        bool ListPaises(string nombre, int Id, bool estado);

        IEnumerable<Islas> GetallIslas();

        IEnumerable<ListCapitanias> GetAllCapitanias();
    }
}
