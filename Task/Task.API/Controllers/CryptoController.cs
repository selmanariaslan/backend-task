using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Task.Core.Utils.Security;

namespace Task.API.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class CryptoController : ControllerBase
    {

        [HttpGet("{text}", Name = "Encrypt")]
        public string Encrypt(string text)
        {
            return new CryptoUtils().EncryptToString(text);
        }

        [HttpGet("{text}", Name = "Decrypt")]
        public string Decrypt(string text)
        {
            return new CryptoUtils().DecryptString(text);
        }
    }
}
