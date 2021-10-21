using System.Collections.Generic;
using SecurePort.Entities.Models;

namespace SecurePort.Services.Interfaces
{

    public interface IServiciosPuertos
    {
        IEnumerable<MOV_Instalaciones> GetPuertosInstalacion(int? id);

        IEnumerable<PuertosViewModel> ListPuertos();

        IEnumerable<PuertosViewModel> GetAllPuertos();

        IEnumerable<PuertosViewModel> GetAllPuertos(List<DependenciasUsuario> dependencias,int? categoria);

        IEnumerable<Puertos> ListPuertos(string nombre);

        IEnumerable<Puertos> ListPuertos(string nombre, int Id);

        bool ListBienes(string nombre, int Id, bool estado);

        string TraerIsla(int? id);

        IEnumerable<OperadoresPuertoViewModel> GetAllOperadoresPuerto(int? id_puerto);

        IEnumerable<OperadoresPuertoViewModel> ListOperadoresPuerto(int? id_puerto);

    }
}



