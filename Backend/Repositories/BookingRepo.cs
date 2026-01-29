using Microsoft.EntityFrameworkCore;
using Models;
using Repositories.Context;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

public class BookingRepo : IBookingRepo
{
    private readonly AppDBContext _dbContext;

    public BookingRepo(AppDBContext dBContext)
    {
        _dbContext = dBContext;
    }

    /// <inheritdoc/>
    public async Task<AppointmentAnimalBooking?> CreateBooking(string AppointmentId, string AnimalId)
    {
        Animal? animal = await _dbContext.Animals.FindAsync(AnimalId);
        if (animal == null)
        {
            return null;
        }
        
        Appointment? appointment = await _dbContext.Appointments.FindAsync(AppointmentId);
        if (appointment == null)
        {
            return null;
        }

        AppointmentAnimalBooking? newBooking = new AppointmentAnimalBooking()
        {
            Id = Guid.NewGuid().ToString("N"),
            UpdatedAt = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow,
            AnimalId = AnimalId,
            Animal = animal,
            AppointmentId = AppointmentId,
            Appointment = appointment,
        };

        if (newBooking == null)
        {
            return null;
        }

        _dbContext.AppointmentAnimalBookings.Add(newBooking);
        try
        {
            await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            if (_dbContext.Appointments.Any(e => e.Id == newBooking.Id))
            {
                return null;
            }
            else
            {
                throw;
            }
        }

        return newBooking;
    }

    /// <inheritdoc/>
    public async Task<bool> DeleteBooking(string AppointmentId, string AnimalId)
    {
        AppointmentAnimalBooking? booking = await _dbContext.AppointmentAnimalBookings.Where(e => e.AnimalId == AnimalId && e.AppointmentId == AppointmentId).FirstAsync();
        if (booking == null)
        {
            return false;
        }

        _dbContext.AppointmentAnimalBookings.Remove(booking);
        await _dbContext.SaveChangesAsync();

        return true;
    }
}
