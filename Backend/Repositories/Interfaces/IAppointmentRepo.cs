namespace Repositories.Interfaces;
using Models;

public interface IAppointmentRepo
{
    public Task<Appointment> GetAppointment(string id);
    public Task<Appointment> PostAppointment(Appointment newPost);
    public Task<List<Appointment>> GetAllApppointmentsForUser(string UserId);
}