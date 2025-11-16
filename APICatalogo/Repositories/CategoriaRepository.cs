using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Pagination;
using APICatalogo.Repositories;

namespace APICatalogo.Repository
{
    public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
    {

        public CategoriaRepository(AppDbContext context) : base(context)
        {
            
        }

        public async Task<PagedList<Categoria>> GetCategoriasAsync(CategoriasParameters categoriasParameters)
        {
            var categorias = await GetAllAsync();

            var categoriasOrdenadas = categorias.OrderBy(c => c.CategoriaId).AsQueryable();

            var resultado = PagedList<Categoria>.ToPagedList(categoriasOrdenadas,
                categoriasParameters.PageNumber,
                categoriasParameters.PageSize);

            return resultado;
        }

        public async Task<PagedList<Categoria>> GetCategoriasFiltadasAsync(CategoriasFiltroNome categoriasParams)
        {
            var categorias = await GetAllAsync();

            var categoriasQueryable = categorias.AsQueryable();

            if (!string.IsNullOrEmpty(categoriasParams.Nome))
            {
                categoriasQueryable = categoriasQueryable.Where(c =>
                    c.Nome!.ToLower()
                    .Contains(categoriasParams.Nome.ToLower()));
            }

            var categoriasFiltradas = PagedList<Categoria>.ToPagedList(categoriasQueryable, categoriasParams.PageNumber,
                categoriasParams.PageSize);

            return categoriasFiltradas;
        }
    }
}
