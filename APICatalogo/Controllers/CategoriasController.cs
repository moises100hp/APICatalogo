using APICatalogo.Context;
using APICatalogo.Filters;
using APICatalogo.Models;
using APICatalogo.Repository;
using APICatalogo.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;
using System.Threading.Tasks;

namespace APICatalogo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly ICategoriaRepository _repository;
        private readonly IConfiguration _configuration;
        private readonly ILogger<CategoriasController> _logger;

        public CategoriasController(ICategoriaRepository repository, IConfiguration configuration,
            ILogger<CategoriasController> logger)
        {
            _repository = repository;
            _configuration = configuration;
            _logger = logger;
        }

        [HttpGet("LerArquivoConfiguracao")]
        public string GetValores()
        {
            var valor1 = _configuration["chave1"];
            var valor2 = _configuration["chave2"];

            var secao1 = _configuration["secao1:chave2"];

            return $"Chave1 = {valor1} Chave 2 = {valor2} Seção1 = {secao1}";
        }

        //Antes .NET 7
        [HttpGet("UsandoFromService/{nome}")]
        public ActionResult<string> GetSaudacaoFromServices([FromServices] IMeuServico meuServico,
                                                            string nome)
        {
            return meuServico.Saudacao(nome);
        }


        [HttpGet("SemUsarFromService/{nome}")]
        public ActionResult<string> GetSaudacaoSemFromServices(IMeuServico meuServico,
                                                          string nome)
        {
            return meuServico.Saudacao(nome);
        }

        //[HttpGet("produtos")]
        //public ActionResult<IEnumerable<Categoria>> GetCategoriasProduto()
        //{
        //    _logger.LogInformation(" ============================= GET api/categorias/produtos ======================");

        //    //return _context.Categorias.Include(p => p.Produtos).ToList();
        //    //Nunca retornar todos os registros. Limitar a quantidade de registros retornados com filtro.
        //    return _context.Categorias.Include(p => p.Produtos).Where(c => c.CategoriaId <= 5).ToList();
        //}

        [HttpGet]
        [ServiceFilter(typeof(ApiLoggingFilter))]
        public ActionResult<IEnumerable<Categoria>> Get()
        {
            _logger.LogInformation(" ============================= GET api/categorias ======================");

            //Nunca retornar todos os registros. Limitar a quantidade de registros retornados com filtro.
            var categorias = _repository.GetCategorias();

            return Ok(categorias);
        }

        [HttpGet("{id:int}", Name = "ObterCategoria")]
        public ActionResult<Categoria> Get(int id)
        {
            _logger.LogInformation($" ============================= GET api/categorias/{id} ======================");

            var categoria = _repository.GetCategoria(id);

            if (categoria is null)
            {
                _logger.LogInformation($" ============================= GET api/categorias/{id} NOT FOUND ======================");
                return NotFound("Categoria não encontrada!");
            }

            return Ok(categoria);
        }

        [HttpPost]
        public ActionResult<Categoria> Post(Categoria categoria)
        {
            if (categoria is null)
            {
                _logger.LogWarning("Dados inválidos");
                return NotFound("Dados inválidos");
            }


            var categoriaCriada = _repository.Create(categoria);

            return new CreatedAtRouteResult("ObterCategoria",
                new { id = categoriaCriada.CategoriaId }, categoriaCriada);
        }

        [HttpPut]
        public ActionResult<Categoria> Put(int id, Categoria categoria)
        {
            if (id != categoria.CategoriaId)
            {
                _logger.LogWarning("Dados inválidos");
                return NotFound("Dados inválidos");
            }

            _repository.Update(categoria);
            return Ok(categoria);
        }

        [HttpDelete("{id:int}")]
        public ActionResult<Categoria> Delete(int id)
        {
            var categoria = _repository.GetCategoria(id);

            if (categoria is null)
            {
                _logger.LogWarning($"Categoria com id={id} não encontrada!");
                return NotFound($"Categoria com id={id} não encontrada!");
            }

            var categoriaExcluida = _repository.Delete(id);

            return Ok(categoriaExcluida);
        }
    }
}
