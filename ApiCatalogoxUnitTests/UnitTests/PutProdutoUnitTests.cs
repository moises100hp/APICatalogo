using APICatalogo.Controllers;
using APICatalogo.DTO;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;

namespace ApiCatalogoxUnitTests.UnitTests
{
    public class PutProdutoUnitTests : IClassFixture<ProdutosUnitTestController>
    {
        private readonly ProdutosController _controller;

        public PutProdutoUnitTests(ProdutosUnitTestController controller)
        {
            _controller = new ProdutosController(controller.repository, controller.mapper);
        }

        [Fact]
        public async Task PutProduto_Update_Return_OkResult()
        {
            //Arrange
            var prodid = 14;

            var updatedProdutoDto = new ProdutoDTO
            {
                ProdutoId = prodid,
                Nome = "Produto - Testes",
                 Descricao = "minha Descricao",
                 ImagemUrl = "imagem1.jpg",
                 CategoriaId = 2
            };

            //Act
            var result = await _controller.PutAsync(prodid, updatedProdutoDto) as ActionResult<ProdutoDTO>;

            //Assert
            result.Should().NotBeNull();
            result.Result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task PuProduto_Update_Return_BadRequest()
        {
            //Arrange
            var prodId = 1000;

            var meuProduto = new ProdutoDTO
            {
                ProdutoId = 14,
                Nome = "Produto Teste",
                Descricao = "Minha Descricao alterada",
                ImagemUrl = "imagem11.jpg",
                CategoriaId = 2
            };

            //Act
            var data = await _controller.PutAsync(prodId, meuProduto);

            //Assert
            data.Result.Should().BeOfType<BadRequestResult>().Which.StatusCode.Should().Be(400);
        }
    }
}
