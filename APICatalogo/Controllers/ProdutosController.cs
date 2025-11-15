using APICatalogo.DTO;
using APICatalogo.DTOs;
using APICatalogo.Models;
using APICatalogo.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace APICatalogo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ProdutosController(IUnitOfWork unitOfWork, 
                                  IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpPatch("{id}/UpdatePartial")]
        public ActionResult<ProdutoDTOUpdateResponse> Patch(int id,
            JsonPatchDocument<ProdutoDTOUpdateRequest> patchProdutoDTO)
        {
            if(patchProdutoDTO is null || id <= 0)
                return BadRequest();

            var produto = _unitOfWork.ProdutoRepository.Get(p => p.ProdutoId == id);

            if (produto is null)
                return NotFound("Produto não encontrado!");

            var produtoDTOUpdateRequest = _mapper.Map<ProdutoDTOUpdateRequest>(produto);

            patchProdutoDTO.ApplyTo(produtoDTOUpdateRequest, ModelState);

            if (!ModelState.IsValid || !TryValidateModel(produtoDTOUpdateRequest))
                return BadRequest(ModelState);

            _mapper.Map(produtoDTOUpdateRequest, produto);

            var produtoAtualizado = _unitOfWork.ProdutoRepository.Update(produto);
            _unitOfWork.Commit();

            return Ok(_mapper.Map<ProdutoDTOUpdateResponse>(produto));

        }

        [HttpGet("produtos/{id}")]
        public ActionResult<IEnumerable<ProdutoDTO>> GetProdutosPorCategoria(int id)
        {
            var produtos = _unitOfWork.ProdutoRepository.GetProdutosPorCategoria(id).ToList();
            if (produtos is null)
                return NotFound("Produtos não encontrados para a categoria informada!");

            var produtosDTO = _mapper.Map<IEnumerable<ProdutoDTO>>(produtos);

            return Ok(produtosDTO);
        }

        //api/produtos  
        [HttpGet]
        public ActionResult<IEnumerable<ProdutoDTO>> Get()
        {
            var produtos = _unitOfWork.ProdutoRepository.GetAll();

            if (produtos is null)
                return NotFound();

            var produtosDTO = _mapper.Map<IEnumerable<ProdutoDTO>>(produtos);

            return Ok(produtosDTO);
        }

        //api/produtos/id
        [HttpGet("{id:int:min(1)}", Name = "ObterProduto")]
        public ActionResult<ProdutoDTO> Get([FromQuery] int id)
        {
            var produto = _unitOfWork.ProdutoRepository.Get(p => p.ProdutoId == id);

            if (produto == null)
                return NotFound("Produto não encontrado!");

            //var destino = _mapper.Map<Destino>(origem);
            var produtoDTO = _mapper.Map<ProdutoDTO>(produto);

            return Ok(produtoDTO);
        }

        //produtos
        [HttpPost]
        public ActionResult<ProdutoDTO> Post(ProdutoDTO produtoDTO)
        {
            if (produtoDTO is null)
                return BadRequest();

            var produto = _mapper.Map<Produto>(produtoDTO);

            var categoria = _unitOfWork.CategoriaRepository.Get(c => c.CategoriaId == produto.CategoriaId);

            if(categoria is null)
                return NotFound("Categoria não encontrada!");

            var novoProduto = _unitOfWork.ProdutoRepository.Create(produto);
            _unitOfWork.Commit();

            var novoProdutoDTO = _mapper.Map<ProdutoDTO>(novoProduto);

            return new CreatedAtRouteResult("ObterProduto",
                new { id = novoProdutoDTO.ProdutoId }, novoProdutoDTO);
        }

        //produtos/id
        [HttpPut("{id:int}")]
        public ActionResult<ProdutoDTO> Put(int id, ProdutoDTO produtoDTO)
        {
            if (id != produtoDTO.ProdutoId)
                return BadRequest();

            var produto = _mapper.Map<Produto>(produtoDTO);

            var produtoAtualizado = _unitOfWork.ProdutoRepository.Update(produto);
            _unitOfWork.Commit();

            var novoProdutoDTO = _mapper.Map<ProdutoDTO>(produtoAtualizado);

            if (novoProdutoDTO is null)
                return StatusCode(500, $"Falha ao atualizar o produto de id = {id}");

            return Ok(novoProdutoDTO);
        }

        //produtos/id
        [HttpDelete("{id:int}")]
        public ActionResult<ProdutoDTO> Delete(int id)
        {
            var produto = _unitOfWork.ProdutoRepository.Get(p => p.ProdutoId == id);

            if( produto is null)
                return NotFound("Produto não encontrado");

            var produtoDeletado = _unitOfWork.ProdutoRepository.Delete(produto);
            _unitOfWork.Commit();

            var produtoDeletadoDTO = _mapper.Map<ProdutoDTO>(produtoDeletado);

            if (produtoDeletadoDTO is null)
                return StatusCode(500, $"Falha ao remover o produto de id = {id}");

            return Ok(produtoDeletadoDTO);
        }
    }
}
