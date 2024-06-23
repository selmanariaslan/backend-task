using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Task.Core.Entities;
using Task.Core.Utils;
using Task.Core.Utils.Data;

namespace Task.Core.Managers
{
    public interface IServiceManager
    {
        HttpClient Client { get; set; }

        HttpResponseMessage DeleteResponse(string url);
        ResponseBase<T> ErrorServiceResponse<T>(string userMessage = "İşleminiz gerçekleştirilmeye çalışılırken bir hata alındı.");
        ResponseBase<T> ErrorServiceResponse<T>(Exception ex, string userMessage = "İşleminiz gerçekleştirilmeye çalışılırken bir hata alındı.");
        ResponseBaseAsync<T> ErrorServiceResponseAsync<T>(Exception ex, string userMessage = "İşleminiz gerçekleştirilmeye çalışılırken bir hata alındı.");
        ResponseBaseAsync<T> ErrorServiceResponseAsync<T>(string userMessage = "İşleminiz gerçekleştirilmeye çalışılırken bir hata alındı.");
        HttpResponseMessage GetResponse(string url, Dictionary<string, string>? headerParams = null);
        HttpResponseMessage PostResponse(string url, object model, Dictionary<string, string> headerParams = null);
        HttpResponseMessage PutResponse(string url, object model, Dictionary<string, string>? headerParams = null);
        ResponseBase<T> ServiceResponse<T>(T data, ServiceResponseStatuses status, Dictionary<string, string> messages, string userMessage);
        ResponseBaseAsync<T> ServiceResponseAsync<T>(Task<T> data, ServiceResponseStatuses status, Dictionary<string, string> messages, string userMessage);
        ResponseBase<T> SuccessServiceResponse<T>(T? data, string userMessage = "İşleminiz başarıyla gerçekleşti.");
        ResponseBaseAsync<T> SuccessServiceResponseAsync<T>(Task<T>? data, string userMessage = "İşleminiz başarıyla gerçekleşti.");
        ResponseBase<T> WarningServiceResponse<T>(string userMessage = "İşleminiz gerçşekleştirilemedi, Lütfen birazdan tekrar deneyin.");
        ResponseBase<T> WarningServiceResponse<T>(T data, string userMessage = "İşleminiz gerçekleştirilemedi, Lütfen birazdan tekrar deneyin.");
        ResponseBaseAsync<T> WarningServiceResponseAsync<T>(Task<T> data, string userMessage = "İşleminiz gerçekleştirilemedi, Lütfen birazdan tekrar deneyin.");
    }

    public class ServiceManager : IServiceManager
    {
        public HttpClient Client { get; set; }
        public static int? ExceptionLogId = null;
        public ServiceManager(string serviceUrl = "Test")
        {
            if (Client == null)
            {
                var root = ConfigUtils.GetConfigurationRoot();
                string url = root.GetSection("Application").GetSection("Service")[serviceUrl] ?? "";
                if (!string.IsNullOrWhiteSpace(url))
                {
                    Client = new HttpClient
                    {
                        BaseAddress = new Uri(url)
                    };
                    Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                }
            }
        }

        public HttpResponseMessage GetResponse(string url, Dictionary<string, string>? headerParams = null)
        {
            //Client.DefaultRequestHeaders.Add("Content-Type", "application/json");
            if (headerParams is not null)
                foreach (var item in headerParams)
                {
                    Client.DefaultRequestHeaders.Add(item.Key, item.Value);
                }
            return Client.GetAsync(url).Result;
        }

        public HttpResponseMessage PutResponse(string url, object model, Dictionary<string, string>? headerParams = null)
        {
            //Client.DefaultRequestHeaders.Add("Content-Type", "application/json");
            if (headerParams is not null)
                foreach (var item in headerParams)
                {
                    Client.DefaultRequestHeaders.Add(item.Key, item.Value);
                }
            return Client.PutAsJsonAsync(url, model).Result;
        }

        public HttpResponseMessage PostResponse(string url, object model, Dictionary<string, string>? headerParams = null)
        {
            //Client.DefaultRequestHeaders.Add("Content-Type", "application/json");
            if (headerParams is not null)
                foreach (var item in headerParams)
                {
                    Client.DefaultRequestHeaders.Add(item.Key, item.Value);
                }
            var asd = model.ToJson();
            return Client.PostAsJsonAsync(url, model).Result;
        }

        public HttpResponseMessage DeleteResponse(string url)
        {
            return Client.DeleteAsync(url).Result;
        }


        #region Service Response

        public virtual ResponseBase<T> SuccessServiceResponse<T>(T data, string userMessage = Constants.DefaultUserMessagesDE.Success)
        {
            Dictionary<string, string> messages = null;
            if (!(userMessage is null))
            {
                messages = new Dictionary<string, string> { { Constants.ErrorMessageTypes.UserMessage, userMessage } };
            }

            return new ResponseBase<T> { Data = data, Status = ServiceResponseStatuses.Success, Messages = messages };
        }

        public virtual ResponseBaseAsync<T> SuccessServiceResponseAsync<T>(Task<T> data, string userMessage = Constants.DefaultUserMessagesDE.Success)
        {
            Dictionary<string, string> messages = null;
            if (!(userMessage is null))
            {
                messages = new Dictionary<string, string> { { Constants.ErrorMessageTypes.UserMessage, userMessage } };
            }

            return new ResponseBaseAsync<T> { Data = data, Status = ServiceResponseStatuses.Success, Messages = messages };
        }

        public virtual ResponseBase<T> WarningServiceResponse<T>(string userMessage = Constants.DefaultUserMessagesDE.Warning)
        {
            if (ExceptionLogId != null) userMessage = $"{userMessage}<br />Error Code: {ExceptionLogId}";
            Dictionary<string, string> messages = new Dictionary<string, string> { { Constants.ErrorMessageTypes.UserMessage, userMessage } };

            return new ResponseBase<T> { Data = default, Status = ServiceResponseStatuses.Warning, Messages = messages };
        }

        public virtual ResponseBase<T> WarningServiceResponse<T>(T data, string userMessage = Constants.DefaultUserMessagesDE.Warning)
        {
            if (ExceptionLogId != null) userMessage = $"{userMessage}<br />Error Code: {ExceptionLogId}";
            Dictionary<string, string> messages = new Dictionary<string, string> { { Constants.ErrorMessageTypes.UserMessage, userMessage } };

            return new ResponseBase<T> { Data = data, Status = ServiceResponseStatuses.Warning, Messages = messages };
        }

        public virtual ResponseBaseAsync<T> WarningServiceResponseAsync<T>(Task<T> data, string userMessage = Constants.DefaultUserMessagesDE.Warning)
        {
            if (ExceptionLogId != null) userMessage = $"{userMessage}<br />Error Code: {ExceptionLogId}";
            Dictionary<string, string> messages = new Dictionary<string, string> { { Constants.ErrorMessageTypes.UserMessage, userMessage } };

            return new ResponseBaseAsync<T> { Data = data, Status = ServiceResponseStatuses.Warning, Messages = messages };
        }

        public virtual ResponseBase<T> ErrorServiceResponse<T>(Exception ex, string userMessage = Constants.DefaultUserMessagesDE.Error)
        {
            Dictionary<string, string> messages = null;
            if (ex != null)
            {
                messages = new Dictionary<string, string> { { Constants.ErrorMessageTypes.BusinessError, ex.Message } };
                if (ExceptionLogId != null)
                {
                    messages.Add(Constants.ErrorMessageTypes.ExceptionLogId, ExceptionLogId.ToString());
                }
            }

            if (ExceptionLogId != null) userMessage = $"{userMessage}<br />Error Code: {ExceptionLogId}";
            (messages ?? (messages = new Dictionary<string, string>())).Add(Constants.ErrorMessageTypes.UserMessage, userMessage);

            return new ResponseBase<T> { Data = default, Status = ServiceResponseStatuses.Error, Messages = messages };
        }
        public virtual ResponseBase<T> ErrorServiceResponse<T>(string userMessage = Constants.DefaultUserMessagesDE.Error)
        {
            if (ExceptionLogId != null) userMessage = $"{userMessage}<br />Error Code: {ExceptionLogId}";
            Dictionary<string, string> messages = new Dictionary<string, string> { { Constants.ErrorMessageTypes.UserMessage, userMessage } };

            return new ResponseBase<T> { Data = default, Status = ServiceResponseStatuses.Error, Messages = messages };
        }

        public virtual ResponseBaseAsync<T> ErrorServiceResponseAsync<T>(Exception ex, string userMessage = Constants.DefaultUserMessagesDE.Error)
        {
            Dictionary<string, string> messages = null;
            if (ex != null)
            {
                messages = new Dictionary<string, string> { { Constants.ErrorMessageTypes.BusinessError, ex.Message } };
                if (ExceptionLogId != null)
                {
                    messages.Add(Constants.ErrorMessageTypes.ExceptionLogId, ExceptionLogId.ToString());
                }
            }

            if (ExceptionLogId != null) userMessage = $"{userMessage}<br />Error Code: {ExceptionLogId}";
            (messages ?? (messages = new Dictionary<string, string>())).Add(Constants.ErrorMessageTypes.UserMessage, userMessage);

            return new ResponseBaseAsync<T> { Data = default, Status = ServiceResponseStatuses.Error, Messages = messages };
        }

        public virtual ResponseBaseAsync<T> ErrorServiceResponseAsync<T>(string userMessage = Constants.DefaultUserMessagesDE.Error)
        {
            if (ExceptionLogId != null) userMessage = $"{userMessage}<br />Error Code: {ExceptionLogId}";
            Dictionary<string, string> messages = new Dictionary<string, string> { { Constants.ErrorMessageTypes.UserMessage, userMessage } };

            return new ResponseBaseAsync<T> { Data = default, Status = ServiceResponseStatuses.Error, Messages = messages };
        }

        public virtual ResponseBase<T> ServiceResponse<T>(T data, ServiceResponseStatuses status, Dictionary<string, string> messages, string userMessage)
        {
            if (userMessage != null)
            {
                if (ExceptionLogId != null) userMessage = $"{userMessage}<br />Error Code: {ExceptionLogId}";
                (messages ?? (messages = new Dictionary<string, string>())).Add(Constants.ErrorMessageTypes.UserMessage, userMessage);
            }

            if (ExceptionLogId != null)
            {
                (messages ?? (messages = new Dictionary<string, string>())).Add(Constants.ErrorMessageTypes.ExceptionLogId, ExceptionLogId.ToString());
            }

            return new ResponseBase<T> { Data = data, Status = status, Messages = messages };
        }

        public virtual ResponseBaseAsync<T> ServiceResponseAsync<T>(Task<T> data, ServiceResponseStatuses status, Dictionary<string, string> messages, string userMessage)
        {
            if (userMessage != null)
            {
                if (ExceptionLogId != null) userMessage = $"{userMessage}<br />Error Code: {ExceptionLogId}";
                (messages ?? (messages = new Dictionary<string, string>())).Add(Constants.ErrorMessageTypes.UserMessage, userMessage);
            }

            if (ExceptionLogId != null)
            {
                (messages ?? (messages = new Dictionary<string, string>())).Add(Constants.ErrorMessageTypes.ExceptionLogId, ExceptionLogId.ToString());
            }

            return new ResponseBaseAsync<T> { Data = data, Status = status, Messages = messages };
        }

        #endregion Service Response
    }

}
