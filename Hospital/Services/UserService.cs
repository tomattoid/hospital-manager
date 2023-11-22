namespace Hospital.Services
{
    public class UserService
    {
        // This is a simple check; replace with actual user authentication logic
        public bool IsValidUser(string username, string password)
        {
            // Replace this with actual user authentication logic (e.g., database check)
            return username == "login" && password == "password";
        }
    }
}
