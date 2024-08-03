using minimal_api.Dominio.Entidades;
using minimal_api.DTOs;



namespace minimal.Dominio.Interfaces;

public interface IAdministradorServicos
{
    Administrador? Login(LoginDTO loginDTO);
}