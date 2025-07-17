namespace Jumia_Api.Api.Contracts.Results
{
    public class AuthResult
    {
        public bool Successed { get; set; }
        public string Message { get; set; }
        public string? Token { get; set; }
        public string? User { get; set; }
    }
}
