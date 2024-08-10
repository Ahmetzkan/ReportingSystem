using System.Text.Json.Serialization;

namespace Business.Dtos.Responses.AuthResponses;

public class LoginResponse
{
    public string Token { get; set; }
    public string RefreshToken { get; set; }
    public DateTime Expiration { get; set; }
}

