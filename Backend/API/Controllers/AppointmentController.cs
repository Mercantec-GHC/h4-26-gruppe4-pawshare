using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using Models;

namespace API.Controllers;

/// <summary>
/// Controller for managing appointments in Pawshare.
/// Appointments are scheduled meetings between pet owners and institutions.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class AppointmentController : ControllerBase
{
    private readonly IAppointmentService _appointmentService;

    public AppointmentController(IAppointmentService appointmentService)
    {
        _appointmentService = appointmentService;
    }

    /// <summary>
    /// Gets an appointment by ID.
    /// </summary>
    /// <param name="id">The unique identifier of the appointment.</param>
    /// <returns>The appointment with the specified ID.</returns>
    /// <response code="200">Returns the appointment.</response>
    /// <response code="404">If the appointment is not found.</response>
    [HttpGet("{id}")]
    public async Task<ActionResult<Appointment>> GetAppointment(string id)
    {
        var appointment = await _appointmentService.GetAppointmentAsync(id);
        if (appointment == null)
        {
            return NotFound();
        }
        return Ok(appointment);
    }

    /// <summary>
    /// Gets all appointments for a specific user.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <returns>A list of appointments for the specified user.</returns>
    /// <response code="200">Returns the list of appointments.</response>
    [HttpGet("user/{userId}")]
    public async Task<ActionResult<List<Appointment>>> GetAppointmentsByUser(string userId)
    {
        var appointments = await _appointmentService.GetAppointmentsByUserAsync(userId);
        return Ok(appointments);
    }

    /// <summary>
    /// Creates a new appointment.
    /// </summary>
    /// <param name="appointment">The appointment data to create.</param>
    /// <returns>The newly created appointment.</returns>
    /// <response code="201">Returns the newly created appointment.</response>
    /// <response code="400">If the appointment data is invalid.</response>
    [HttpPost]
    public async Task<ActionResult<Appointment>> CreateAppointment([FromBody] Appointment appointment)
    {
        var created = await _appointmentService.CreateAppointmentAsync(appointment);
        if (created == null)
        {
            return BadRequest();
        }
        return CreatedAtAction(nameof(GetAppointment), new { id = created.Id }, created);
    }

    /// <summary>
    /// Updates an existing appointment.
    /// </summary>
    /// <param name="id">The unique identifier of the appointment to update.</param>
    /// <param name="appointment">The updated appointment data.</param>
    /// <returns>The updated appointment.</returns>
    /// <response code="200">Returns the updated appointment.</response>
    /// <response code="404">If the appointment is not found.</response>
    [HttpPut("{id}")]
    public async Task<ActionResult<Appointment>> UpdateAppointment(string id, [FromBody] Appointment appointment)
    {
        var updated = await _appointmentService.UpdateAppointmentAsync(id, appointment);
        if (updated == null)
        {
            return NotFound();
        }
        return Ok(updated);
    }

    /// <summary>
    /// Deletes an appointment.
    /// </summary>
    /// <param name="id">The unique identifier of the appointment to delete.</param>
    /// <response code="204">If the appointment was successfully deleted.</response>
    /// <response code="404">If the appointment is not found.</response>
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAppointment(string id)
    {
        var deleted = await _appointmentService.DeleteAppointmentAsync(id);
        if (!deleted)
        {
            return NotFound();
        }
        return NoContent();
    }

    /// <summary>
    /// Adds an animal to an appointment.
    /// </summary>
    /// <param name="appointmentId">The unique identifier of the appointment.</param>
    /// <param name="animalId">The unique identifier of the animal to add.</param>
    /// <response code="200">If the animal was successfully added to the appointment.</response>
    /// <response code="404">If the appointment or animal is not found.</response>
    [HttpPost("{appointmentId}/animals/{animalId}")]
    public async Task<ActionResult> AddAnimalToAppointment(string appointmentId, string animalId)
    {
        var result = await _appointmentService.AddAnimalToAppointmentAsync(appointmentId, animalId);
        if (!result)
        {
            return NotFound();
        }
        return Ok();
    }

    /// <summary>
    /// Removes an animal from an appointment.
    /// </summary>
    /// <param name="appointmentId">The unique identifier of the appointment.</param>
    /// <param name="animalId">The unique identifier of the animal to remove.</param>
    /// <response code="204">If the animal was successfully removed from the appointment.</response>
    /// <response code="404">If the appointment or animal is not found.</response>
    [HttpDelete("{appointmentId}/animals/{animalId}")]
    public async Task<ActionResult> RemoveAnimalFromAppointment(string appointmentId, string animalId)
    {
        var result = await _appointmentService.RemoveAnimalFromAppointmentAsync(appointmentId, animalId);
        if (!result)
        {
            return NotFound();
        }
        return NoContent();
    }
}
