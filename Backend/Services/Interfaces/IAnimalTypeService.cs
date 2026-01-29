using Models;

namespace Services.Interfaces;

/// <summary>
/// Service interface for managing animal types in Pawshare.
/// </summary>
public interface IAnimalTypeService
{
    /// <summary>
    /// Gets an animal type by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the animal type.</param>
    /// <returns>The animal type if found, otherwise null.</returns>
    Task<AnimalType?> GetAnimalTypeAsync(string id);

    /// <summary>
    /// Gets all animal types in the system.
    /// </summary>
    /// <returns>A list of all animal types.</returns>
    Task<List<AnimalType>> GetAllAnimalTypesAsync();

    /// <summary>
    /// Creates a new animal type.
    /// </summary>
    /// <param name="animalType">The animal type data to create.</param>
    /// <returns>The newly created animal type if successful, otherwise null.</returns>
    Task<AnimalType?> CreateAnimalTypeAsync(AnimalType animalType);

    /// <summary>
    /// Updates an existing animal type.
    /// </summary>
    /// <param name="id">The unique identifier of the animal type to update.</param>
    /// <param name="animalType">The updated animal type data.</param>
    /// <returns>The updated animal type if found, otherwise null.</returns>
    Task<AnimalType?> UpdateAnimalTypeAsync(string id, AnimalType animalType);

    /// <summary>
    /// Deletes an animal type.
    /// </summary>
    /// <param name="id">The unique identifier of the animal type to delete.</param>
    /// <returns>True if the animal type was deleted successfully, otherwise false.</returns>
    Task<bool> DeleteAnimalTypeAsync(string id);
}
