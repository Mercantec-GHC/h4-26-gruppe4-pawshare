using Repositories.Context;
using Repositories.Interfaces;

namespace Repositories;

using Microsoft.EntityFrameworkCore;
using Models;


public class AppointmentRepo : IAppointmentRepo
{
    private readonly AppDBContext _dbContext;

    public AppointmentRepo(AppDBContext dBContext)
    {
        _dbContext = dBContext;
    }

    public async Task<List<Appointment>> GetAllApppointmentsForUser(string UserId)
    {
        List<Appointment> appointment = _dbContext.Appointments.Where(e => e.Users != null && e.Users.Any(user => user.Id.Equals(UserId))).ToList();
        if (appointment is null || appointment.Count < 1)
        {
            return new List<Appointment>();
        }

        return appointment;
    }

    public async Task<Appointment> GetAppointment(string id)
    {
        var appointment = await _dbContext.Appointments.FindAsync(id);
        if (appointment is null)
        {
            return null;
        }

        return appointment;
    }

    public async Task<Appointment?> PostAppointment(Appointment newAppointment)
    {
        _dbContext.Appointments.Add(newAppointment);
        try
        {
            await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            if (_dbContext.Appointments.Any(e => e.Id == newAppointment.Id))
            {
                return null;
            }
            else
            {
                throw;
            }
        }

        return newAppointment;
    }
}