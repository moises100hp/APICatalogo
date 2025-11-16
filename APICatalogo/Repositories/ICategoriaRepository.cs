using APICatalogo.Models;
using APICatalogo.Pagination;
using APICatalogo.Repositories;

namespace APICatalogo.Repository
{
    public interface ICategoriaRepository : IRepository<Categoria>
    {
        PagedList<Categoria> GetCategorias(CategoriasParameters categoriasParameters);
    }
}
