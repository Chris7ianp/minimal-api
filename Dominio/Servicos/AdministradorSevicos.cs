
using minimal.Dominio.Interfaces;
using minimal_api.Dominio.Entidades;
using minimal_api.DTOs;
using minimal_api.Infraestrutura.Db;

namespace minimal.Dominio.Servicos;

public class AdministradorServicos : IAdministradorServicos
{
    private readonly DbContexto _contexto;

    public AdministradorServicos(DbContexto contexto)
    {
        _contexto = contexto;
    }
    public Administrador? Login(LoginDTO loginDTO)
    {
        var adm = _contexto.Administradores.Where(
                                                 a => a.Email == loginDTO.Email && a.Senha == loginDTO.Senha
                                                 ).FirstOrDefault();
        
        return adm;
    }
}
