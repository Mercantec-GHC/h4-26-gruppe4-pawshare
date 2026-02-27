using Models;
using Models.DTOs;

namespace Services.Interfaces;

/// <summary>
/// Service interface for managing animals in Pawshare.
/// </summary>
public interface IAnimalService
{
    /// <summary>
    /// Gets an animal by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the animal.</param>
    /// <returns>The animal if found, otherwise null.</returns>
    Task<AnimalDto?> GetAnimalAsync(string id);

    /// <summary>
    /// Gets all animals in the system.
    /// </summary>
    /// <returns>A list of all animals.</returns>
    Task<List<AnimalDto>> GetAllAnimalsAsync();

    /// <summary>
    /// Gets all animals of a specific type (dog, cat, etc.).
    /// </summary>
    /// <param name="typeId">The unique identifier of the animal type.</param>
    /// <returns>A list of animals matching the specified type.</returns>
    Task<List<AnimalDto>> GetAnimalsByTypeAsync(string typeId);

    /// <summary>
    /// Gets all animals belonging to a specific user.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <returns>A list of animals owned by the specified user.</returns>
    Task<List<AnimalDto>> GetAnimalsByUserAsync(string userId);

    /// <summary>
    /// Creates a new animal.
    /// </summary>
    /// <param name="animal">The animal data to create.</param>
    /// <returns>The newly created animal if successful, otherwise null.</returns>
    Task<Animal?> CreateAnimalAsync(Animal animal);

    /// <summary>
    /// Updates an existing animal.
    /// </summary>
    /// <param name="id">The unique identifier of the animal to update.</param>
    /// <param name="animal">The updated animal data.</param>
    /// <returns>The updated animal if found, otherwise null.</returns>
    Task<AnimalDto?> UpdateAnimalAsync(string id, AnimalDto dto);

    /// <summary>
    /// Deletes an animal.
    /// </summary>
    /// <param name="id">The unique identifier of the animal to delete.</param>
    /// <returns>True if the animal was deleted successfully, otherwise false.</returns>
    Task<bool> DeleteAnimalAsync(string id);

    /// <summary>
    /// Updates the picture key for an animal and optionally deletes the old picture.
    /// </summary>
    /// <param name="userId">The owner user ID.</param>
    /// <param name="animalId">The animal ID.</param>
    /// <param name="newAnimalPictureKey">The new animal picture object key.</param>
    /// <param name="oldAnimalPictureKey">The old animal picture object key to delete (nullable).</param>
    /// <returns>True if successful, false otherwise.</returns>
    Task<bool> UpdateAnimalPictureAsync(string userId, string animalId, string newAnimalPictureKey, string? oldAnimalPictureKey = null);
}
