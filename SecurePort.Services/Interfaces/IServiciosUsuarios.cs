
namespace SecurePort.Services.Interfaces
{
    using System.Collections.Generic;
    using SecurePort.Entities.Models;

    public interface IServiciosUsuarios
    {
        IEnumerable<UsuariosCategoriasGrupos> GetAllUsuarios();

        bool GetUsuarios(string Login);

        bool GetEmail(string Email);

        bool GetPassword(string Password);

        IEnumerable<Tipos_Documento> GetTiposDocumento();

        IEnumerable<Doc_Usuario_Asoc> GetDocs_Usuario(int id);

        IEnumerable<Tipos_Instalacion> GetTipos_Instalacion(int id, int? idCategoria);

        IEnumerable<PuertosViewModel> GetDependenciaPuertos(int? id, int? idCategoria);

        IEnumerable<Puertos> GetPuertos();

        IEnumerable<Organismos> GetDependenciaOrganismos(int id, int? idCategoria);

        IEnumerable<Organismos> GetOrganismos();

        IEnumerable<MOV_Instalaciones> GetDependenciaInstalaciones(int id, int? idCategoria);

        IEnumerable<AccionesViewModel> GetAcciones();

        string RemoveUsuarioById(int? IdUsuario);

        int RemovePerfilById(int? Id);

        int RemoveGrupoById(int? Id);

        List<DependenciasUsuario> GetDependenciaUsuarios(int id_usuario, int? categoria);

        IEnumerable<UsuariosCategoriasGrupos> GetAll(List<DependenciasUsuario> dependencias,int? categoria );
    }
}
