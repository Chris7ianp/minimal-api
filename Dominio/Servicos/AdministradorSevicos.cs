
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

    public Administrador? BuscarPorId(int id)
    {
        return _contexto.Administradores.Where(w => w.Id == id).FirstOrDefault();
    }

    public Administrador Incluir(Administrador administrador)
    {
        _contexto.Administradores.Add(administrador);
        _contexto.SaveChanges();
        return administrador;
    }

    public Administrador? Login(LoginDTO loginDTO)
    {
        var adm = _contexto.Administradores.Where(
                                                 a => a.Email == loginDTO.Email && a.Senha == loginDTO.Senha
                                                 ).FirstOrDefault();
        
        return adm;
    }

    public List<Administrador> Todos(int? pagina)
    {
        var query = _contexto.Administradores.AsQueryable();
        int itensPorPagina = 10;

        if(pagina != null){
            query = query.Skip(((int)pagina -1) * itensPorPagina).Take(itensPorPagina);
        }

        return query.ToList();
    }
}
