using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Task.Data.Models.TaskDb;
using System.Threading.Tasks;
using Xunit;
using System.Net.Http.Json;
using System.Text.Json;
using Task.Core.Entities;
using Task.Core.Entities.CommonModel;

namespace Task.Tests
{
    public class ProductTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public ProductTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        /// <summary>
        /// This method tests retrieving all products.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async System.Threading.Tasks.Task GetProductsReturnsOkResponse()
        {
            var response = await _client.GetAsync("/odata/products");
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<ResponseBase<Product[]>>();
            Assert.NotEmpty(result?.Data);
        }

        /// <summary>
        ///  This method tests retrieving a product by ID.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async System.Threading.Tasks.Task GetProductByIdReturnsOkResponse()
        {
            var response = await _client.GetAsync("/odata/products/1");
            response.EnsureSuccessStatusCode();
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
            var result = await response.Content.ReadFromJsonAsync<ResponseBase<Product>>();
            Assert.NotNull(result?.Data);
            Assert.Equal(1, result.Data.Id);
        }

        /// <summary>
        /// This method tests creating a new product object.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async System.Threading.Tasks.Task PostProductReturnsCommonUpsertModel()
        {
            var product = new Product { Name = "New Product", Description = "New Description", Price = 9.99M, StockQuantity = 10, CreatedBy = "xUnit" };

            var response = await _client.PostAsJsonAsync("/odata/products", product);
            response.EnsureSuccessStatusCode();
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
            var result = await response.Content.ReadFromJsonAsync<ResponseBase<CommonUpsertModel>>();
            Assert.NotNull(result);
            Assert.True(result.Data?.Status);
        }

        /// <summary>
        /// This method tests upating an existing product object
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async System.Threading.Tasks.Task PutProductReturnsCommonUpsertModel()
        {
            var product = new Product { Id = 1, Name = "Updated Product", Description = "Updated Description", Price = 19.99M, StockQuantity = 20, ModifiedBy = "xUnit", ModifiedDate = DateTime.Now };

            var response = await _client.PutAsJsonAsync("/odata/products/1", product);
            response.EnsureSuccessStatusCode();
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
            var result = await response.Content.ReadFromJsonAsync<ResponseBase<CommonUpsertModel>>();
            Assert.NotNull(result);
            Assert.True(result.Data?.Status);
            Assert.Equal(1, result.Data?.Id);
        }

        /// <summary>
        /// This method tests deleting a product by ID.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async System.Threading.Tasks.Task DeleteProductReturnsCommonUpsertModel()
        {
            var response = await _client.DeleteAsync("/odata/products/1");
            response.EnsureSuccessStatusCode();
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
            var result = await response.Content.ReadFromJsonAsync<ResponseBase<CommonUpsertModel>>();
            Assert.NotNull(result);
            Assert.True(result.Data?.Status);
            Assert.Equal(1, result.Data?.Id);
        }
    }
}