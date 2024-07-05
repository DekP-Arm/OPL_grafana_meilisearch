
namespace testAPI.DTOs
{
     public class BaseHttpResponse<T>
    {
        public string ResponseCode { get; set; }
        public string ResponseMessage { get; set; }
        public T Data { get; set; }
        public ErrorData Error { get; set; }

        public void SetSuccess(T data, string message, string responseCode = "200")
        {
            this.ResponseCode = responseCode;
            this.ResponseMessage = message;
            this.Data = data;
            this.Error = null;
        }

        public void SetError(ErrorData errorData, string message, string responseCode = "400")
        {
            this.Error = errorData;
            this.ResponseMessage = message;
            this.ResponseCode = responseCode;
            this.Data = default(T);
        }
    }
    public class PageDataResponse<T>
    {
        public List<T> Items { get; set; }
        public int Total { get; set; }
    }
    public class ErrorData
    {
        public string Code { get; set; }
        public string Message { get; set; }

    }

    public class BaseServiceResponse<T>
    {
        public T Data { get; set; }
        public ServiceErrorData Error { get; set; }

        public void SetError(bool IsError, string Message)
        {
            var err = new ServiceErrorData { };
            err.IsError = IsError;
            err.Message = Message;
            this.Error = err;
            this.Data = default(T);
        }
        public void SetSuccess(T data)
        {
            var err = new ServiceErrorData { };
            err.IsError = false;
            err.Message = "";
            this.Error = err;
            this.Data = data;
        }

    }

    public class ServiceErrorData
    {
        public bool IsError { get; set; }
        public string Message { get; set; }

    }
}
