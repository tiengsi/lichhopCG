namespace WebApi.Models.Dtos
{
    public enum ResultCode
    {
        SUCCESS,
        FAILE,
        CONFLICT,
        NOTFOUND
    }

    public class ReturnResultDto<T>
    {
        public ResultCode ResultCode { get; set; }
        public string Message { get; set; }
        public T Result { get; set; }
    }
}
