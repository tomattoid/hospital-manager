namespace Hospital.Services
{
    public class DoctorService
    {
        public bool IsValidUser(string username, string password)
        {
            // Replace this with actual user authentication logic (e.g., database check)
            return username == "johnsmith" && password == "12345678";
        }
    }
}
