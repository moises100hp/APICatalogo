using APICatalogo.Models;
using APICatalogo.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace APICatalogo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly IProdutoRepository _repository;

        public ProdutosController(IProdutoRepository repository)
        {
            _repository = repository;
        }

        //api/produtos  
        [HttpGet]
        public ActionResult<IEnumerable<Produto>> Get()
        {
            var produtos =  _repository.GetProdutos().ToList();

            if (produtos is null)
                return NotFound();

            return Ok(produtos);
        }

        //api/produtos/id
        [HttpGet("{id:int:min(1)}", Name = "ObterProduto")]
        public ActionResult<Produto> Get([FromQuery] int id)
        {
            var produto = _repository.GetProduto(id);

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

           var novoProduto = _repository.Create(produto);

            return new CreatedAtRouteResult("ObterProduto",
                new { id = produto.ProdutoId }, novoProduto);
        }

        //produtos/id
        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Produto produto)
        {
            if (id != produto.ProdutoId)
                return BadRequest();

            var atualizado = _repository.Update(produto);

            if (!atualizado)
                return StatusCode(500, $"Falha ao atualizar o produto de id = {id}");

            return Ok();
        }

        //produtos/id
        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var deletado = _repository.Delete(id);

            if (!deletado)
                return StatusCode(500, $"Falha ao remover o produto de id = {id}");

            return Ok();
        }
    }
}
