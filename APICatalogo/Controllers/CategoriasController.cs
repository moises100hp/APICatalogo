using APICatalogo.DTO;
using APICatalogo.DTOs.Mappings;
using APICatalogo.Filters;
using APICatalogo.Models;
using APICatalogo.Pagination;
using APICatalogo.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Newtonsoft.Json;
using X.PagedList;

namespace APICatalogo.Controllers
{
    [ApiController]
    [EnableRateLimiting("fixed")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Produces("application/json")]
    public class CategoriasController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CategoriasController> _logger;

        public CategoriasController(IUnitOfWork unitOfWork,
            ILogger<CategoriasController> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        #region Comentado
        //[HttpGet("LerArquivoConfiguracao")]
        //public string GetValores()
        //{
        //    var valor1 = _configuration["chave1"];
        //    var valor2 = _configuration["chave2"];

        //    var secao1 = _configuration["secao1:chave2"];

        //    return $"Chave1 = {valor1} Chave 2 = {valor2} Seção1 = {secao1}";
        //}

        ////Antes .NET 7
        //[HttpGet("UsandoFromService/{nome}")]
        //public ActionResult<string> GetSaudacaoFromServices([FromServices] IMeuServico meuServico,
        //                                                    string nome)
        //{
        //    return meuServico.Saudacao(nome);
        //}


        //[HttpGet("SemUsarFromService/{nome}")]
        //public ActionResult<string> GetSaudacaoSemFromServices(IMeuServico meuServico,
        //                                                  string nome)
        //{
        //    return meuServico.Saudacao(nome);
        //}

        //[HttpGet("produtos")]
        //public ActionResult<IEnumerable<Categoria>> GetCategoriasProduto()
        //{
        //    _logger.LogInformation(" ============================= GET api/categorias/produtos ======================");

        //    //return _context.Categorias.Include(p => p.Produtos).ToList();
        //    //Nunca retornar todos os registros. Limitar a quantidade de registros retornados com filtro.
        //    return _context.Categorias.Include(p => p.Produtos).Where(c => c.CategoriaId <= 5).ToList();
        //}
        #endregion

        /// <summary>
        /// Obtem uma lista de Categorias
        /// </summary>
        /// <returns>Objetos Categoria</returns>
        //[Authorize]
        [HttpGet]
        [ServiceFilter(typeof(ApiLoggingFilter))]
        [DisableRateLimiting]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> GetAsync()
        {
            _logger.LogInformation(" ============================= GET api/categorias ======================");

            //Nunca retornar todos os registros. Limitar a quantidade de registros retornados com filtro.
            var categorias = await _unitOfWork.CategoriaRepository.GetAllAsync();

            if (categorias is null)
                return NotFound("Categorias não encontradas!");

            var categoriasDTO =  categorias.ToCategoriaDTOList();

            return Ok(categoriasDTO);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="categoriasParameters"></param>
        /// <returns></returns>
        [HttpGet("pagination")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> GetAsync([FromQuery] CategoriasParameters categoriasParameters)
        {
            _logger.LogInformation(" ============================= GET api/categorias/pagination ======================");
            var categorias = await _unitOfWork.CategoriaRepository.GetCategoriasAsync(categoriasParameters);
            if (categorias is null)
                return NotFound("Categorias não encontradas!");

            return ObterCategorias(categorias);
        }

        [HttpGet("filter/nome/pagination")]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> GetCategoriasFiltradas([FromQuery] CategoriasFiltroNome categoriasFiltro)
        {
            var categoriasFiltradas = await _unitOfWork.CategoriaRepository.GetCategoriasFiltadasAsync(categoriasFiltro);

            return ObterCategorias(categoriasFiltradas);
        }

        private ActionResult<IEnumerable<CategoriaDTO>> ObterCategorias(IPagedList<Categoria> categorias)
        {
            var metadata = new
            {
                categorias.Count,
                categorias.PageSize,
                categorias.PageCount,
                categorias.TotalItemCount,
                categorias.HasNextPage,
                categorias.HasPreviousPage
            };

            Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));
            var categoriasDTO = categorias.ToCategoriaDTOList();

            return Ok(categoriasDTO);
        }

        /// <summary>
        /// Obter uma categoria pelo seu ID.
        /// </summary>
        /// <param name="id">Codigo do produto</param>
        /// <returns>Um objeto Produto</returns>
        [HttpGet("{id:int}", Name = "ObterCategoria")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CategoriaDTO>> GetAsync(int id)
        {
            _logger.LogInformation($" ============================= GET api/categorias/{id} ======================");

            var categoria = await _unitOfWork.CategoriaRepository.GetAsync(c => c.Id == id);

            if (categoria is null)
            {
                _logger.LogInformation($" ============================= GET api/categorias/{id} NOT FOUND ======================");
                return NotFound("Categoria não encontrada!");
            }

            var categoriaDTO = categoria.ToCategoriaDTO();

            return Ok(categoriaDTO);
        }

        /// <summary>
        /// Incluir uma nova categoria
        /// </summary>
        /// <remarks>Exemplo de request: 
        /// 
        ///     POST api/categorias
        ///     {
        ///         "categoriaIs": 1,
        ///         "nome": "categoria1",
        ///         "imagemUrl": "http://teste.net/1;jpg"
        ///     }
        /// </remarks>
        /// <param name="categoriaDTO"></param>
        /// <returns>O objeto Categoria incluida</returns>
        /// <remarks>Retorna um objeto Categoria incluido</remarks>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CategoriaDTO>> PostAsync(CategoriaDTO categoriaDTO)
        {
            if (categoriaDTO == null)
            {
                _logger.LogWarning("Dados inválidos");
                return NotFound("Dados inválidos");
            }

            var categoria = categoriaDTO.ToCategoria();

            var categoriaCriada = _unitOfWork.CategoriaRepository.Create(categoria);
            await _unitOfWork.CommitAsync();

            var novaCategoriaDTO = categoriaCriada.ToCategoriaDTO();

            return new CreatedAtRouteResult("ObterCategoria",
                new { id = novaCategoriaDTO.CategoriaId }, novaCategoriaDTO);
        }

        [HttpPut]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Put))]
        public async Task<ActionResult<CategoriaDTO>> PutAsync(int id, CategoriaDTO categoriaDTO)
        {
            if (id != categoriaDTO.CategoriaId)
            {
                _logger.LogWarning("Dados inválidos");
                return NotFound("Dados inválidos");
            }

            var categoria = categoriaDTO.ToCategoria();

            _unitOfWork.CategoriaRepository.Update(categoria);
            await _unitOfWork.CommitAsync();

            var categoriaAtualizadaDTO = categoria.ToCategoriaDTO();

            return Ok(categoriaAtualizadaDTO);
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        //[Authorize(Roles = "AdminOnly", AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<CategoriaDTO>> Delete(int id)
        {
            var categoria = await _unitOfWork.CategoriaRepository.GetAsync(c => c.Id == id);

            if (categoria is null)
            {
                _logger.LogWarning($"Categoria com id={id} não encontrada!");
                return NotFound($"Categoria com id={id} não encontrada!");
            }

            var categoriaExcluida = _unitOfWork.CategoriaRepository.Delete(categoria);
            await _unitOfWork.CommitAsync();

            var categoriaDTO = categoriaExcluida.ToCategoriaDTO();

            return Ok(categoriaDTO);
        }
    }
}
