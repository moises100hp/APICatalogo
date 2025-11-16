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

        public PagedList<Categoria> GetCategorias(CategoriasParameters categoriasParameters)
        {
            var categorias = GetAll().OrderBy(c => c.CategoriaId).AsQueryable();
            var categoriasOrdenadas = PagedList<Categoria>.ToPagedList(categorias,
                categoriasParameters.PageNumber,
                categoriasParameters.PageSize);

            return categoriasOrdenadas;
        }

        public PagedList<Categoria> GetCategoriasFiltadas(CategoriasFiltroNome categoriasParams)
        {
            var categorias = GetAll().AsQueryable();

            if(!string.IsNullOrEmpty(categoriasParams.Nome))
            {
                categorias = categorias.Where(c =>
                    c.Nome!.ToLower()
                    .Contains(categoriasParams.Nome.ToLower()));
            }

            var categoriasFiltradas = PagedList<Categoria>.ToPagedList(categorias, categoriasParams.PageNumber,
                categoriasParams.PageSize);

            return categoriasFiltradas;
        }
    }
}
