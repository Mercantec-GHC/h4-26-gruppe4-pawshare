using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using Models;

namespace API.Controllers;

/// <summary>
/// Controller for managing animal types in Pawshare.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class AnimalTypeController : ControllerBase
{
    private readonly IAnimalTypeService _animalTypeService;

    public AnimalTypeController(IAnimalTypeService animalTypeService)
    {
        _animalTypeService = animalTypeService;
    }

    /// <summary>
    /// Gets all animal types.
    /// </summary>
    /// <returns>A list of all animal types in the system.</returns>
    /// <response code="200">Returns the list of animal types.</response>
    [HttpGet]
    public async Task<ActionResult<List<AnimalType>>> GetAllAnimalTypes()
    {
        var types = await _animalTypeService.GetAllAnimalTypesAsync();
        return Ok(types);
    }

    /// <summary>
    /// Gets an animal type by ID.
    /// </summary>
    /// <param name="id">The unique identifier of the animal type.</param>
    /// <returns>The animal type with the specified ID.</returns>
    /// <response code="200">Returns the animal type.</response>
    /// <response code="404">If the animal type is not found.</response>
    [HttpGet("{id}")]
    public async Task<ActionResult<AnimalType>> GetAnimalType(string id)
    {
        var type = await _animalTypeService.GetAnimalTypeAsync(id);
        if (type == null)
        {
            return NotFound();
        }
        return Ok(type);
    }

    /// <summary>
    /// Creates a new animal type.
    /// </summary>
    /// <param name="animalType">The animal type data to create.</param>
    /// <returns>The newly created animal type.</returns>
    /// <response code="201">Returns the newly created animal type.</response>
    /// <response code="400">If the animal type data is invalid.</response>
    [HttpPost]
    public async Task<ActionResult<AnimalType>> CreateAnimalType([FromBody] AnimalType animalType)
    {
        var created = await _animalTypeService.CreateAnimalTypeAsync(animalType);
        if (created == null)
        {
            return BadRequest();
        }
        return CreatedAtAction(nameof(GetAnimalType), new { id = created.Id }, created);
    }

    /// <summary>
    /// Updates an existing animal type.
    /// </summary>
    /// <param name="id">The unique identifier of the animal type to update.</param>
    /// <param name="animalType">The updated animal type data.</param>
    /// <returns>The updated animal type.</returns>
    /// <response code="200">Returns the updated animal type.</response>
    /// <response code="404">If the animal type is not found.</response>
    [HttpPut("{id}")]
    public async Task<ActionResult<AnimalType>> UpdateAnimalType(string id, [FromBody] AnimalType animalType)
    {
        var updated = await _animalTypeService.UpdateAnimalTypeAsync(id, animalType);
        if (updated == null)
        {
            return NotFound();
        }
        return Ok(updated);
    }

    /// <summary>
    /// Deletes an animal type.
    /// </summary>
    /// <param name="id">The unique identifier of the animal type to delete.</param>
    /// <response code="204">If the animal type was successfully deleted.</response>
    /// <response code="404">If the animal type is not found.</response>
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAnimalType(string id)
    {
        var deleted = await _animalTypeService.DeleteAnimalTypeAsync(id);
        if (!deleted)
        {
            return NotFound();
        }
        return NoContent();
    }
}
