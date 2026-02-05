using Models;

namespace Repositories.Interfaces;

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

    /// <summary>
    /// Updates given appointment
    /// </summary>
    /// <param name="animalType">The new version of the animalType</param>
    /// <returns>The AnimalType that was updated, returns null if not succesfull</returns>
    public Task<AnimalType?> UpdateAnimalType(AnimalType animalType);

    /// <summary>
    /// Delets AnimalType from table
    /// </summary>
    /// <param name="typeId">Id of the animaltype needed to be deleted</param>
    /// <returns>Boolean, true if succesful and false if not</returns>
    public Task<bool> DeleteAnimalType(string typeId);
}