using APICatalogo.Controllers;
using APICatalogo.DTO;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ApiCatalogoxUnitTests.UnitTests
{
    public class GetProdutoUnitTests : IClassFixture<ProdutosUnitTestController>
    {
        private readonly ProdutosController _controller;
        public GetProdutoUnitTests(ProdutosUnitTestController controller)
        {
            _controller = new ProdutosController(controller.repository, controller.mapper);
        }

        [Fact]
        public async Task GetProdutoById_Return_OkResult()
        {
            //Arrange
            var produtoId = 2;

            //Act
            var data = await _controller.GetAsync(produtoId);

            //Assert (xunit)
            //var okResult = Assert.IsType<ProdutoDTO>(data.Result);
            //Assert.Equal(200, okResult.StatusCode);

            //Assert (fluentassertions)
            data.Result.Should().BeOfType<OkObjectResult>() //verifica se o resultado é do tipo OkObjetcResult.
                .Which.StatusCode.Should().Be(200);//verifica se o código de status do OkObjetcResult é 200;
        }

        [Fact]
        public async Task GetProdutoById_Return_NotFound()
        {
            //Arrange
            var produtoId = 999;

            //Act
            var data = await _controller.GetAsync(produtoId);

            //Assert
            data.Result.Should().BeOfType<NotFoundObjectResult>()
                .Which.StatusCode.Should().Be(404);
        }

        [Fact]
        public async Task GetProdutoById_Return_BadRequest()
        {
            //Arrange
            int produtoId = -1;

            //Act
            var data = await _controller.GetAsync(produtoId);

            //Assert
            data.Result.Should().BeOfType<BadRequestObjectResult>()
                .Which.StatusCode.Should().Be(400);
        }

        [Fact]
        public async Task GetProdutoById_Return_ListOfProdutoDTO()
        {
            //Act
            var data = await _controller.GetAsync();

            //Assert
            data.Result.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeAssignableTo<IEnumerable<ProdutoDTO>>()
                .And.NotBeNull();
        }
    }
}
