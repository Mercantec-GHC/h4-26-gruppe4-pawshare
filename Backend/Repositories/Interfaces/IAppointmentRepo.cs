namespace Repositories.Interfaces;
using Models;

public interface IAppointmentRepo
{
    /// <summary>
    /// Gets Appointment with given Id from table
    /// </summary>
    /// <param name="id">The id of the wanted Appointment</param>
    /// <returns>Appointment with given id, if not found returns null</returns>
    public Task<Appointment?> GetAppointment(string id);

    /// <summary>
    /// Posts a new Appointment to the table
    /// </summary>
    /// <param name="newPost">The new Appointment that needs to be created</param>
    /// <returns>Appointment that was added, null if it already exists, and throws exception if error occurs under creation</returns>
    public Task<Appointment?> PostAppointment(Appointment newPost);

    /// <summary>
    /// Gets all Appointments for a given user in the table
    /// </summary>
    /// <returns>List of Appointments, empty list if none is found</returns>
    public Task<List<Appointment>> GetAllApppointmentsForUser(string UserId);

    /// <summary>
    /// Updates given appointment from id
    /// </summary>
    /// <param name="appointmentId">The id of the appointment that needs updating</param>
    /// <param name="appointment">The new version of the appointment</param>
    /// <returns>The Appointment that was updated, returns null if not succesfull</returns>
    public Task<Appointment?> UpdateAppointment(Appointment appointment);
}