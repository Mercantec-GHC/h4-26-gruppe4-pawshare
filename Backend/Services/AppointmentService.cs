using Services.Interfaces;
using Models;
using Repositories.Interfaces;

namespace Services;

public class AppointmentService : IAppointmentService
{
    private readonly IAppointmentRepo _appointmentRepo;

    public AppointmentService(IAppointmentRepo appointmentRepo)
    {
        _appointmentRepo = appointmentRepo;
    }

    public async Task<Appointment?> GetAppointmentAsync(string id)
    {
        return await _appointmentRepo.GetAppointment(id);
    }

    public async Task<List<Appointment>> GetAppointmentsByUserAsync(string userId)
    {
        return await _appointmentRepo.GetAllApppointmentsForUser(userId);
    }

    public async Task<Appointment?> CreateAppointmentAsync(Appointment appointment)
    {
        appointment.Id = Guid.NewGuid().ToString();
        appointment.CreatedAt = DateTime.UtcNow;
        appointment.UpdatedAt = DateTime.UtcNow;
        return await _appointmentRepo.PostAppointment(appointment);
    }

    public async Task<Appointment?> UpdateAppointmentAsync(string id, Appointment appointment)
    {
        var existing = await _appointmentRepo.GetAppointment(id);
        if (existing == null) return null;

        existing.Start = appointment.Start;
        existing.End = appointment.End;
        existing.Address = appointment.Address;
        existing.Description = appointment.Description;
        existing.UpdatedAt = DateTime.UtcNow;

        return existing;
    }

    public async Task<bool> DeleteAppointmentAsync(string id)
    {
        var appointment = await _appointmentRepo.GetAppointment(id);
        return appointment != null;
    }

    public async Task<bool> AddAnimalToAppointmentAsync(string appointmentId, string animalId)
    {
        var appointment = await _appointmentRepo.GetAppointment(appointmentId);
        return appointment != null;
    }

    public async Task<bool> RemoveAnimalFromAppointmentAsync(string appointmentId, string animalId)
    {
        var appointment = await _appointmentRepo.GetAppointment(appointmentId);
        return appointment != null;
    }
}
