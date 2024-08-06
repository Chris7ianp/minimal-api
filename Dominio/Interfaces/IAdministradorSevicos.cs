using minimal_api.Dominio.Entidades;
using minimal_api.DTOs;



namespace minimal.Dominio.Interfaces;

public interface IAdministradorServicos
{
    Administrador? Login(LoginDTO loginDTO);
    Administrador Incluir(Administrador administrador);
    Administrador? BuscarPorId(int id);
    List<Administrador> Todos(int? pagina);

}