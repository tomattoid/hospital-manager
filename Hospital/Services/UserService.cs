namespace Hospital.Services
{
    public class UserService
    {
        public bool IsValidUser(string username, string password)
        {
            return username == "login" && password == "password";
        }
    }
}
