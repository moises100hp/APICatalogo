using APICatalogo.Context;
using APICatalogo.Models;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Repositories
{
    public class ProdutoRepository : IProdutoRepository
    {
        private readonly AppDbContext _context;

        public ProdutoRepository(AppDbContext context)
        {
            _context = context;
        }

        public IQueryable<Produto> GetProdutos()
        {
            return _context.Produtos.AsNoTracking();
        }

        public Produto GetProduto(int id)
        {
            var produto = _context.Produtos.FirstOrDefault(p => p.ProdutoId == id);

            if(produto is null)
                throw new InvalidOperationException("Produto não encontrado.");

            return produto;
        }

        public Produto Create(Produto produto)
        {
            if(produto is null)
                throw new ArgumentNullException(nameof(produto));

            _context.Produtos.Add(produto);
            _context.SaveChanges();
            return produto;
        }

        public bool Update(Produto produto)
        {
            if(produto is null)
                throw new ArgumentNullException(nameof(produto));
            
            if(_context.Produtos.Any(i => i.ProdutoId == produto.ProdutoId))
                return false;

            _context.Produtos.Update(produto);
            _context.SaveChanges();
            return true;
        }

        public bool Delete(int id)
        {
            var produto = _context.Produtos.Find(id);

            if (produto is null)
                return false;

            _context.Produtos.Remove(produto);
            _context.SaveChanges();
            return true;
        }
    }
}
