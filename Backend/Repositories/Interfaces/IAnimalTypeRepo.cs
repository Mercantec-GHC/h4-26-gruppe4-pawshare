namespace Repositories.Interfaces;
using Models;

public interface IAnimalTypeRepo
{
    /// <summary>
    /// Gets AnimalType with given Id from table
    /// </summary>
    /// <param name="id">The id of the wanted AnimalType</param>
    /// <returns>AnimalType with given id, if not found returns null</returns>
    public Task<AnimalType?> GetAnimalType(string id);

    /// <summary>
    /// Posts a new AnimalType to the table
    /// </summary>
    /// <param name="newAnimalType">The new AnimalType that needs to be created</param>
    /// <returns>AnimalType that was added, null if it already exists, and throws exception if error occurs under creation</returns>
    public Task<AnimalType?> PostAnimalType(AnimalType newAnimalType);

    /// <summary>
    /// Gets all AnimalTypes in the table
    /// </summary>
    /// <returns>List of AnimalType, empty list if none is found</returns>
    public Task<List<AnimalType>> GetAllAnimalTypes();
}