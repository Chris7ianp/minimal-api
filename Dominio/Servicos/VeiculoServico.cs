
using Microsoft.EntityFrameworkCore;
using minimal.Dominio.Interfaces;
using minimal_api.Dominio.Entidades;
using minimal_api.DTOs;
using minimal_api.Infraestrutura.Db;

namespace minimal.Dominio.Servicos;

public class VeiculoServico : IVeiculosServico
{
    private readonly DbContexto _contexto;

    public VeiculoServico(DbContexto contexto)
    {
        _contexto = contexto;
    }

    public void Apagar(Veiculo veiculo)
    {
        _contexto.Veiculos.Remove(veiculo);
        _contexto.SaveChanges();
    }

    public void Atualizar(Veiculo veiculo)
    {
        _contexto.Veiculos.Update(veiculo);
        _contexto.SaveChanges();
    }

    public void Incluir(Veiculo veiculo)
    {
        _contexto.Veiculos.Add(veiculo);
        _contexto.SaveChanges();
    }

    public Veiculo? BuscaPorId(int id)
    {
        return _contexto.Veiculos.Where(w => w.Id == id).FirstOrDefault();
    }

    public List<Veiculo> Todos(int? pagina = 1, string? nome = null, string? marca = null)
    {
        var query = _contexto.Veiculos.AsQueryable();

        if (!string.IsNullOrEmpty(nome))
        {
            query = query.Where(w => EF.Functions.Like(w.Nome.ToLower(), $"%{nome.ToLower()}%"));
        }


        int itensPorPagina = 10;

        if (pagina != null)

            query = query.Skip(((int)pagina - 1) * itensPorPagina).Take(itensPorPagina);

        return query.ToList();
    }


}
