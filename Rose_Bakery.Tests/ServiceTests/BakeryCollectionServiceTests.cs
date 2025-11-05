using Microsoft.Extensions.Logging;
using Moq;
using Rose_Bakery.Data.Interface;
using Rose_Bakery.Service.Implementation;
using Rose_Bakery.Tests.Mocks;
using Xunit;

namespace Rose_Bakery.Tests.ServiceTests
{
    public class BakeryCollectionServiceTests
    {
        private readonly Mock<IBakeryDbContext> _bakeryDbContextMock;
        private readonly Mock<ILogger<BakeryCollectionService>> _loggerMock;
        private readonly BakeryCollectionService _sut;

        public BakeryCollectionServiceTests()
        {
            _bakeryDbContextMock = new Mock<IBakeryDbContext>();
            _loggerMock = new Mock<ILogger<BakeryCollectionService>>();
            _sut = new BakeryCollectionService(_loggerMock.Object, _bakeryDbContextMock.Object);
        }

        [Fact]
        public async Task GetBakeryCollectionAsync_ShouldReturnCollection_WhenDataExists()
        {
            // Arrange
            var categories = new List<Models.CategoryModel>
            {
                new() {
                    Name = "Cakes",
                    Products = new List<Models.ProductModel>
                    {
                        new() { Name = "Chocolate Cake", Price = 25.00m }
                    }
                }
            };
            _bakeryDbContextMock.Setup(x => x.Categories).Returns(DbSetMock.Create(categories));

            // Act
            var result = await _sut.GetBakeryCollectionAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            var collection = result.First();
            Assert.Equal(200, collection.StatusCode);
            Assert.Equal("Success", collection.StatusMessage);
            Assert.Equal("Cakes", collection.CatgoryName);
            Assert.Single(collection.Products);
            var product = collection.Products.First();
            Assert.Equal("Chocolate Cake", product.Name);
        }

        [Fact]
        public async Task GetBakeryCollectionAsync_ShouldReturnNotFound_WhenNoDataExists()
        {
            // Arrange
            var categories = new List<Models.CategoryModel>();
            _bakeryDbContextMock.Setup(x => x.Categories).Returns(DbSetMock.Create(categories));

            // Act
            var result = await _sut.GetBakeryCollectionAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            var response = result.First();
            Assert.Equal(404, response.StatusCode);
            Assert.Equal("No Data Found, Failed", response.StatusMessage);
        }

        [Fact]
        public async Task GetBakeryCollectionAsync_ShouldReturnInternalServerError_WhenExceptionIsThrown()
        {
            // Arrange
            _bakeryDbContextMock.Setup(x => x.Categories).Throws(new System.Exception("Test Exception"));

            // Act
            var result = await _sut.GetBakeryCollectionAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            var response = result.First();
            Assert.Equal(500, response.StatusCode);
            Assert.Equal("Test Exception", response.StatusMessage);
        }
    }
}
