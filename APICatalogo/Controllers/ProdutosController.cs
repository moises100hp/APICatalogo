using APICatalogo.Models;
using APICatalogo.Repositories;
using APICatalogo.Repository;
using Microsoft.AspNetCore.Mvc;

namespace APICatalogo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProdutosController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("produtos/{id}")]
        public ActionResult<IEnumerable<Produto>> GetProdutosPorCategoria(int id)
        {
            var produtos = _unitOfWork.ProdutoRepository.GetProdutosPorCategoria(id).ToList();
            if (produtos is null)
                return NotFound("Produtos não encontrados para a categoria informada!");

            return Ok(produtos);
        }

        //api/produtos  
        [HttpGet]
        public ActionResult<IEnumerable<Produto>> Get()
        {
            var produtos = _unitOfWork.ProdutoRepository.GetAll();

            if (produtos is null)
                return NotFound();

            return Ok(produtos);
        }

        //api/produtos/id
        [HttpGet("{id:int:min(1)}", Name = "ObterProduto")]
        public ActionResult<Produto> Get([FromQuery] int id)
        {
            var produto = _unitOfWork.ProdutoRepository.Get(p => p.ProdutoId == id);

            if (produto == null)
                return NotFound("Produto não encontrado!");

            return Ok(produto);
        }

        //produtos
        [HttpPost]
        public ActionResult Post(Produto produto)
        {
            if (produto is null)
                return BadRequest();

            var categoria = _unitOfWork.CategoriaRepository.Get(c => c.CategoriaId == produto.CategoriaId);

            if(categoria is null)
                return NotFound("Categoria não encontrada!");

            var novoProduto = _unitOfWork.ProdutoRepository.Create(produto);
            _unitOfWork.Commit();

            return new CreatedAtRouteResult("ObterProduto",
                new { id = produto.ProdutoId }, novoProduto);
        }

        //produtos/id
        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Produto produto)
        {
            if (id != produto.ProdutoId)
                return BadRequest();

            var produtoAtualizado = _unitOfWork.ProdutoRepository.Update(produto);
            _unitOfWork.Commit();

            if (produtoAtualizado is null)
                return StatusCode(500, $"Falha ao atualizar o produto de id = {id}");

            return Ok(produtoAtualizado);
        }

        //produtos/id
        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var produto = _unitOfWork.ProdutoRepository.Get(p => p.ProdutoId == id);

            if( produto is null)
                return NotFound("Produto não encontrado");

            var produtoDeletado = _unitOfWork.ProdutoRepository.Delete(produto);
            _unitOfWork.Commit();

            if (produtoDeletado is null)
                return StatusCode(500, $"Falha ao remover o produto de id = {id}");

            return Ok(produtoDeletado);
        }
    }
}
