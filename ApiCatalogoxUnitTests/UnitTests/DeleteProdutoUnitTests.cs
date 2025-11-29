using APICatalogo.Controllers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;

namespace ApiCatalogoxUnitTests.UnitTests
{
    public class DeleteProdutoUnitTests : IClassFixture<ProdutosUnitTestController>
    {
        private readonly ProdutosController _controller;

        public DeleteProdutoUnitTests(ProdutosUnitTestController controller)
        {
            _controller = new ProdutosController(controller.repository, controller.mapper);
        }

        [Fact]
        public async Task DeleteProdutoByid_Return_OkResult()
        {
            //Arrange
            var produtoId = 6;

            //Act
            var result = await _controller.DeleteAsync(produtoId);

            //Assert
            result.Result.Should().NotBeNull();
            result.Result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task DeleteProdutoById_Return_NotFound()
        {
            //Arrange
            var produtoId = 999;

            //Act
            var result = await _controller.DeleteAsync(produtoId);

            //Assert
            result.Result.Should().NotBeNull();
            result.Result.Should().BeOfType<NotFoundObjectResult>();
        }
    }
}
