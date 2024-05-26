namespace WebApplication3.DTOs.Auth
{
    public class ServiceResponses
    {
        public record class GeneralResponse(bool Flag, string Message);
        public record class LoginResponse(bool Flag, string Token, string RToken, string Message);

        public record class RefreshResponse(bool Flag, string Rtoken,string Message);
    }
}
