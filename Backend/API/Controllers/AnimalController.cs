using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using Models;

namespace API.Controllers;

/// <summary>
/// Controller for managing animals in Pawshare.
/// </summary>
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

    /// <summary>
    /// Gets all animals of a specific type.
    /// </summary>
    /// <param name="typeId">The unique identifier of the animal type.</param>
    /// <returns>A list of animals matching the specified type.</returns>
    /// <response code="200">Returns the list of animals.</response>
    [HttpGet("type/{typeId}")]
    public async Task<ActionResult<List<Animal>>> GetAnimalsByType(string typeId)
    {
        var animals = await _animalService.GetAnimalsByTypeAsync(typeId);
        return Ok(animals);
    }

    /// <summary>
    /// Gets all animals belonging to a specific user.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <returns>A list of animals owned by the specified user.</returns>
    /// <response code="200">Returns the list of animals.</response>
    [HttpGet("user/{userId}")]
    public async Task<ActionResult<List<Animal>>> GetAnimalsByUser(string userId)
    {
        var animals = await _animalService.GetAnimalsByUserAsync(userId);
        return Ok(animals);
    }

    /// <summary>
    /// Creates a new animal.
    /// </summary>
    /// <param name="animal">The animal data to create.</param>
    /// <returns>The newly created animal.</returns>
    /// <response code="201">Returns the newly created animal.</response>
    /// <response code="400">If the animal data is invalid.</response>
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

    /// <summary>
    /// Updates an existing animal.
    /// </summary>
    /// <param name="id">The unique identifier of the animal to update.</param>
    /// <param name="animal">The updated animal data.</param>
    /// <returns>The updated animal.</returns>
    /// <response code="200">Returns the updated animal.</response>
    /// <response code="404">If the animal is not found.</response>
    [HttpPut("{id}")]
    public async Task<ActionResult<Animal>> UpdateAnimal(string id, [FromBody] Animal animal)
    {
        var updated = await _animalService.UpdateAnimalAsync(id, animal);
        if (updated == null)
        {
            return NotFound();
        }
        return Ok(updated);
    }

    /// <summary>
    /// Deletes an animal.
    /// </summary>
    /// <param name="id">The unique identifier of the animal to delete.</param>
    /// <response code="204">If the animal was successfully deleted.</response>
    /// <response code="404">If the animal is not found.</response>
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAnimal(string id)
    {
        var deleted = await _animalService.DeleteAnimalAsync(id);
        if (!deleted)
        {
            return NotFound();
        }
        return NoContent();
    }
}
