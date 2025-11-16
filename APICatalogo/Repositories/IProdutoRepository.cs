using APICatalogo.Models;
using APICatalogo.Pagination;
using X.PagedList;

namespace APICatalogo.Repositories
{
    public interface IProdutoRepository : IRepository<Produto>
    {
        Task<IEnumerable<Produto>> GetProdutosPorCategoriaAsync(int id);
        //IEnumerable<Produto> GetProdutos(ProdutosParameters produtosParameters);
        Task<IPagedList<Produto>> GetProdutosAsync(ProdutosParameters produtosParameters);

        Task<IPagedList<Produto>> GetProdutosFiltroPrecoAsync(ProdutosFiltroPreco produtosFiltroPreco);
    }
}
