using APICatalogo.Models;
using APICatalogo.Pagination;
using APICatalogo.Repositories;

namespace APICatalogo.Repository
{
    public interface ICategoriaRepository : IRepository<Categoria>
    {
        Task<PagedList<Categoria>> GetCategoriasAsync(CategoriasParameters categoriasParameters);

        Task<PagedList<Categoria>> GetCategoriasFiltadasAsync(CategoriasFiltroNome categoriasParams);
    }
}
