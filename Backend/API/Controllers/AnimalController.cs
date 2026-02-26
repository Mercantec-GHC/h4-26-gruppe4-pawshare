using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Models;
using Models.DTOs;
using Services.Interfaces;
using System.Security.Claims;

namespace API.Controllers;

/// <summary>
/// Controller for managing animals in Pawshare.
/// </summary>
[Authorize]
[Route("api/[controller]")]
[ApiController]
public class AnimalController : ControllerBase
{
    private readonly IAnimalService _animalService;

    public AnimalController(IAnimalService animalService)
    {
        _animalService = animalService;
    }

    /// <summary>
    /// Gets all animals.
    /// </summary>
    /// <returns>A list of all animals in the system.</returns>
    /// <response code="200">Returns the list of animals.</response>
    [HttpGet]
    public async Task<ActionResult<List<Animal>>> GetAllAnimals()
    {
        var animals = await _animalService.GetAllAnimalsAsync();
        return Ok(animals);
    }

    /// <summary>
    /// Gets an animal by ID.
    /// </summary>
    /// <param name="id">The unique identifier of the animal.</param>
    /// <returns>The animal with the specified ID.</returns>
    /// <response code="200">Returns the animal.</response>
    /// <response code="404">If the animal is not found.</response>
        // commenting it out for now, as it is not used in the frontend and we want to avoid accidental get request during testing and development.
    // We can re-enable it later if needed. Plus it would be a security risk to have it open when we dont use it.
    /*
    [HttpGet("{id}")]
    public async Task<ActionResult<Animal>> GetAnimal(string id)
    {
        var animal = await _animalService.GetAnimalAsync(id);
        if (animal == null)
        {
            return NotFound();
        }
        return Ok(animal);
    }
    */

    /// <summary>
    /// Gets all animals of a specific type.
    /// </summary>
    /// <param name="typeId">The unique identifier of the animal type.</param>
    /// <returns>A list of animals matching the specified type.</returns>
    /// <response code="200">Returns the list of animals.</response>
    // commenting it out for now, as it is not used in the frontend and we want to avoid accidental get request during testing and development.
    // We can re-enable it later if needed. Plus it would be a security risk to have it open when we dont use it.
    /*
    [HttpGet("type/{typeId}")]
    public async Task<ActionResult<List<Animal>>> GetAnimalsByType(string typeId)
    {
        var animals = await _animalService.GetAnimalsByTypeAsync(typeId);
        return Ok(animals);
    }
*/
    /// <summary>
    /// Gets all animals belonging to a specific user.
    /// </summary>
    /// <returns>A list of animals owned by the specified user.</returns>
    /// <response code="200">Returns the list of animals.</response>
    [HttpGet("user")]
    public async Task<ActionResult<List<Animal>>> GetAnimalsByUser()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId is null)
        {
            return BadRequest();
        }

        var animals = await _animalService.GetAnimalsByUserAsync(userId);
        return Ok(animals);
    }

    [HttpPost("{id}/picture")]
    public async Task<IActionResult> UpdateAnimalPicture(string id, UpdateAnimalPictureDto dto)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
            return Unauthorized();

        if (string.IsNullOrWhiteSpace(dto.NewAnimalPictureKey))
            return BadRequest(new { error = "Animal picture key is required" });

        var success = await _animalService.UpdateAnimalPictureAsync(
            userId,
            id,
            dto.NewAnimalPictureKey,
            dto.OldAnimalPictureKey
        );

        if (!success)
            return BadRequest(new { error = "Failed to update animal picture" });

        return Ok();
    }

    /// <summary>
    /// Creates a new animal.
    /// </summary>
    /// <param name="animal">The animal data to create.</param>
    /// <returns>The newly created animal.</returns>
    /// <response code="201">Returns the newly created animal.</response>
    /// <response code="400">If the animal data is invalid.</response>
    // commenting it out for now, as it is not used in the frontend and we want to avoid accidental creation during testing and development.
    // We can re-enable it later if needed. Plus it would be a security risk to have it open when we dont use it.
    /*
    [HttpPost]
    public async Task<ActionResult<Animal>> CreateAnimal([FromBody] Animal animal)
    {
        var created = await _animalService.CreateAnimalAsync(animal);
        if (created == null)
        {
            return BadRequest();
        }
        return CreatedAtAction(nameof(GetAnimal), new { id = created.Id }, created);
    }
*/
    /// <summary>
    /// Updates an existing animal.
    /// </summary>
    /// <param name="id">The unique identifier of the animal to update.</param>
    /// <param name="animal">The updated animal data.</param>
    /// <returns>The updated animal.</returns>
    /// <response code="200">Returns the updated animal.</response>
    /// <response code="404">If the animal is not found.</response>
    // commenting it out for now, as it is not used in the frontend and we want to avoid accidental updates during testing and development.
    // We can re-enable it later if needed. Plus it would be a security risk to have it open when we dont use it.
    /*[HttpPut("{id}")]
    public async Task<ActionResult<Animal>> UpdateAnimal(string id, [FromBody] AnimalDto animal)
    {
        var updated = await _animalService.UpdateAnimalAsync(id, animal);
        if (updated == null)
        {
            return NotFound();
        }
        return Ok(updated);
    }
*/
    /// <summary>
    /// Deletes an animal.
    /// </summary>
    /// <param name="id">The unique identifier of the animal to delete.</param>
    /// <response code="204">If the animal was successfully deleted.</response>
    /// <response code="404">If the animal is not found.</response>
    // commenting it out for now, as it is not used in the frontend and we want to avoid accidental deletions during testing and development.
    // We can re-enable it later if needed. Plus it would be a security risk to have it open when we dont use it.
    /*[HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAnimal(string id)
    {
        var deleted = await _animalService.DeleteAnimalAsync(id);
        if (!deleted)
        {
            return NotFound();
        }
        return NoContent();
    }*/
}
