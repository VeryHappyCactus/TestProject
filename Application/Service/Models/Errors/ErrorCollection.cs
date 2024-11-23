namespace Service.Models.Errors
{
    public class ErrorCollection : BaseError
    {
        public IEnumerable<BaseError>? Errors { get; set; }
    }
}
