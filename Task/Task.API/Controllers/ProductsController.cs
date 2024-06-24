using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Results;
using Microsoft.Extensions.Caching.Memory;
using Task.API.Models.Requests;
using Task.Core.Entities;
using Task.Core.Entities.CommonModel;
using Task.Core.Managers;
using Task.Core.Repositories;
using Task.Data.DatabaseContexts;
using Task.Data.Models.TaskDb;

namespace Task.API.Controllers
{

    public class ProductsController : BaseApiController
    {
        public ProductsController(IGenericRepository<TaskContext> taskRepo, IServiceManager serviceManager, IGenericRepository<LogManagementContext> logService) : base(serviceManager, logService, taskRepo)
        {
        }

        /// <summary>
        /// This method retrieves all active product list.
        /// </summary>
        /// <returns>ResponseBase<IQueryable<Product>></returns>
        [EnableQuery(PageSize = 15)]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var response = new ResponseBase<IQueryable<Product>>();
            IQueryable<Product> result = _BtsRepo._GetAll<Product>();
            _LogRepo.Run(ProjectEnvironment.Service, "",
                action: () =>
                {
                    result = _BtsRepo._GetAll<Product>();
                    response = _Service.SuccessServiceResponse(result);
                },
            errorAction: (ex) => response = _Service.ErrorServiceResponse<IQueryable<Product>>(ex),
            requestModel: null,
            responseModel: response);
            return await Api(response);
        }

        /// <summary>
        /// This method retrieves a product by ID.
        /// </summary>
        /// <param name="id">ProductId</param>
        /// <returns>ResponseBase<Product></returns>
        [EnableQuery]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var response = new ResponseBase<Product>();
            Product? result = null;
            _LogRepo.Run(ProjectEnvironment.Service, "",
                action: () =>
                {
                    result = _BtsRepo._Find<Product>(x => x.Id == id);
                    response = _Service.SuccessServiceResponse(result);
                },
            errorAction: (ex) => response = _Service.ErrorServiceResponse<Product>(ex),
            requestModel: null,
            responseModel: response);
            return await Api(response);
        }

        /// <summary>
        /// This method creates a product with the given values in the product object in the database.
        /// </summary>
        /// <param name="request">new product object</param>
        /// <returns>CommonUpsertModel</returns>
        [HttpPost]
        public async Task<IActionResult> Insert([FromBody] InsertProductRequest request)
        {
            var response = new ResponseBase<CommonUpsertModel>();
            var result = new CommonUpsertModel();
            _LogRepo.Run(ProjectEnvironment.Service, "",
                    action: () =>
                    {
                        var entity = new Product
                        {
                            CreatedBy = "api",
                            Description = request.Description,
                            Name = request.Name,
                            Price = request.Price,
                            StockQuantity = request.StockQuantity
                        };
                        _BtsRepo._Insert(entity);
                        if (_BtsRepo._Save() > 0)
                        {
                            result.Id = entity.Id;
                            result.Status = true;
                            response = _Service.SuccessServiceResponse(result);
                        }
                        else
                            response = _Service.ErrorServiceResponse<CommonUpsertModel>();
                    },
                errorAction: (ex) => response = _Service.ErrorServiceResponse<CommonUpsertModel>(ex),
                requestModel: request,
                responseModel: response);
            return await Api(response);
        }

        /// <summary>
        /// This method updates a product with the given values in the product object.
        /// </summary>
        /// <param name="id">ProductId</param>
        /// <param name="request">old product object</param>
        /// <returns>CommonUpsertModel</returns>
        [HttpPut("{id}")]
        public Task<IActionResult> Update(int id, [FromBody] UpdateProductRequest request)
        {
            var response = new ResponseBase<CommonUpsertModel>();
            var result = new CommonUpsertModel();
            _LogRepo.Run(ProjectEnvironment.Service, "",
                action: () =>
                {
                    var product = _BtsRepo._GetById<Product>(id);
                    if (product is not null)
                    {
                        product.Name = request.Name;
                        product.Description = request.Description;
                        product.Price = request.Price;
                        product.StockQuantity = request.StockQuantity;
                        product.ModifiedDate = DateTime.Now;
                        product.ModifiedBy = "api";
                        _BtsRepo._Update(product);
                        if (_BtsRepo._Save() > 0)
                        {
                            result.Id = product.Id;
                            result.Status = true;
                            response = _Service.SuccessServiceResponse(result);
                        }
                        else
                            response = _Service.ErrorServiceResponse<CommonUpsertModel>();
                    }
                    else _Service.WarningServiceResponse<CommonUpsertModel>("Geçerli rol bulunamadı.");
                },
            errorAction: (ex) => response = _Service.ErrorServiceResponse<CommonUpsertModel>(ex),
            requestModel: request,
            responseModel: response);
            return Api(response);
        }

        /// <summary>
        /// This method deletes(makes passive) a product by ID
        /// </summary>
        /// <param name="id">ProductId</param>
        /// <returns>CommonUpsertModel</returns>
        [HttpDelete("{id}")]
        public Task<IActionResult> Delete([FromODataUri] int id)
        {
            var response = new ResponseBase<CommonUpsertModel>();
            var result = new CommonUpsertModel();
            _LogRepo.Run(ProjectEnvironment.Service, "api",
                action: () =>
                {
                    _BtsRepo._SoftDelete<Product>(id);
                    if (_BtsRepo._Save() > 0)
                    {
                        result.Id = id;
                        result.Status = true;
                        response = _Service.SuccessServiceResponse(result);
                    }
                    else
                        response = _Service.ErrorServiceResponse<CommonUpsertModel>();
                },
            errorAction: (ex) => response = _Service.ErrorServiceResponse<CommonUpsertModel>(ex),
            requestModel: id,
            responseModel: response);
            return Api(response);
        }
    }
}
