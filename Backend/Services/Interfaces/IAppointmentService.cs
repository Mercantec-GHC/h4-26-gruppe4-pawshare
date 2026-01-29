using Models;

namespace Services.Interfaces;

/// <summary>
/// Service interface for managing appointments in Pawshare.
/// Appointments are scheduled meetings between pet owners and institutions.
/// </summary>
public interface IAppointmentService
{
    /// <summary>
    /// Gets an appointment by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the appointment.</param>
    /// <returns>The appointment if found, otherwise null.</returns>
    Task<Appointment?> GetAppointmentAsync(string id);

    /// <summary>
    /// Gets all appointments for a specific user.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <returns>A list of appointments for the specified user.</returns>
    Task<List<Appointment>> GetAppointmentsByUserAsync(string userId);

    /// <summary>
    /// Creates a new appointment.
    /// </summary>
    /// <param name="appointment">The appointment data to create.</param>
    /// <returns>The newly created appointment if successful, otherwise null.</returns>
    Task<Appointment?> CreateAppointmentAsync(Appointment appointment);

    /// <summary>
    /// Updates an existing appointment.
    /// </summary>
    /// <param name="id">The unique identifier of the appointment to update.</param>
    /// <param name="appointment">The updated appointment data.</param>
    /// <returns>The updated appointment if found, otherwise null.</returns>
    Task<Appointment?> UpdateAppointmentAsync(string id, Appointment appointment);

    /// <summary>
    /// Deletes an appointment.
    /// </summary>
    /// <param name="id">The unique identifier of the appointment to delete.</param>
    /// <returns>True if the appointment was deleted successfully, otherwise false.</returns>
    Task<bool> DeleteAppointmentAsync(string id);

    /// <summary>
    /// Adds an animal to an appointment.
    /// </summary>
    /// <param name="appointmentId">The unique identifier of the appointment.</param>
    /// <param name="animalId">The unique identifier of the animal to add.</param>
    /// <returns>True if the animal was added successfully, otherwise false.</returns>
    Task<bool> AddAnimalToAppointmentAsync(string appointmentId, string animalId);

    /// <summary>
    /// Removes an animal from an appointment.
    /// </summary>
    /// <param name="appointmentId">The unique identifier of the appointment.</param>
    /// <param name="animalId">The unique identifier of the animal to remove.</param>
    /// <returns>True if the animal was removed successfully, otherwise false.</returns>
    Task<bool> RemoveAnimalFromAppointmentAsync(string appointmentId, string animalId);
}
