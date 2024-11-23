namespace Service.Models.Errors
{
    public class ValidationError : BaseError
    {
        public string Key { get; set; }
        public string Messages { get; set; }

        public ValidationError(string key, string messages) 
        { 
            Key = key;
            Messages = messages;
        }
    }
}
