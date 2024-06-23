using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.Extensions.Caching.Memory;
using Task.Core.Entities;
using Task.Core.Managers;
using Task.Core.Repositories;
using Task.Data.DatabaseContexts;

namespace Task.API.Controllers
{
    [Route("odata/[controller]")]
    [ApiController]
    public abstract class BaseApiController : ODataController
    {
        protected readonly IServiceManager _Service;
        protected readonly IGenericRepository<LogManagementContext> _LogRepo;
        protected readonly IGenericRepository<TaskContext> _BtsRepo;


        public BaseApiController(IServiceManager service, IGenericRepository<LogManagementContext> logRepo, IGenericRepository<TaskContext> btsRepo)
        {
            _Service = service;
            _LogRepo = logRepo;
            _BtsRepo = btsRepo;
        }

        protected async Task<IActionResult> Api<T>(ResponseBase<T> response, bool controlData = true)
        {
            if (response.Status == ServiceResponseStatuses.Success)
            {
                return Ok(response);
            }
            else
            {
                if (controlData && EqualityComparer<T>.Default.Equals(response.Data, default))
                {
                    return NotFound(response);
                }
                else
                {
                    return StatusCode(409, response);
                }
            }
        }
    }
}
