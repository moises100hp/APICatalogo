using APICatalogo.Models;
using APICatalogo.Pagination;
using APICatalogo.Repositories;
using X.PagedList;

namespace APICatalogo.Repository
{
    public interface ICategoriaRepository : IRepository<Categoria>
    {
        Task<IPagedList<Categoria>> GetCategoriasAsync(CategoriasParameters categoriasParameters);

        Task<IPagedList<Categoria>> GetCategoriasFiltadasAsync(CategoriasFiltroNome categoriasParams);
    }
}
