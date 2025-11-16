using APICatalogo.Models;
using APICatalogo.Pagination;

namespace APICatalogo.Repositories
{
    public interface IProdutoRepository : IRepository<Produto>
    {
        Task<IEnumerable<Produto>> GetProdutosPorCategoriaAsync(int id);
        //IEnumerable<Produto> GetProdutos(ProdutosParameters produtosParameters);
        Task<PagedList<Produto>> GetProdutosAsync(ProdutosParameters produtosParameters);

        Task<PagedList<Produto>> GetProdutosFiltroPrecoAsync(ProdutosFiltroPreco produtosFiltroPreco);
    }
}
