namespace TransportSystemASP.Controllers
{
    public class RegisterDto
    {

        public string FullName { get; set; }   // Полное имя пользователя
        public string Login { get; set; }      // Логин пользователя
        public string Password { get; set; }   // Пароль пользователя
        public string Role { get; set; }       // Роль пользователя (например, "admin", "user")
    }
}
