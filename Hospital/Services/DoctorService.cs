namespace Hospital.Services
{
    public class DoctorService
    {
        public bool IsValidUser(string username, string password)
        {
            return username == "johnsmith" && password == "12345678";
        }
    }
}
