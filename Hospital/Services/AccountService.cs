namespace Hospital.Services
{
    public interface IAccountService
    {
        int PatientId { get; set; }
        int DoctorId { get; set; }
    }
    public class AccountService : IAccountService
    {
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
    }
}
