
namespace TaskHub.Modules.Identity.Application.Commands.Login
{
    public class LoginCommandResponse
    {
        public string AccessToken { get; set; } = string.Empty;
        public DateTime ExpiredAt { get; set; }
    }
}
