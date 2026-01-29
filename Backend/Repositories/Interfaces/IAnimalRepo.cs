namespace Repositories.Interfaces;
using Models;

public interface IAnimalRepo
{
    /// <summary>
    /// Gets Animal with given Id from table
    /// </summary>
    /// <param name="id">The id of the wanted Animal</param>
    /// <returns>Animal with given id, if not found returns null</returns>
    public Task<Animal?> GetAnimal(string id);

    /// <summary>
    /// Gets all Animals in the table with given AnimalType
    /// </summary>
    /// <param name="typeId">The wanted AnimalType Id</param>
    /// <returns>List of Animals of given AnimalType, empty list if none is found</returns>
    public Task<List<Animal>> GetAnimalsFromType(string typeId);

    /// <summary>
    /// Posts a new Animal to the table
    /// </summary>
    /// <param name="newAnimal">The new Animal that needs to be created</param>
    /// <returns>Animal that was added, null if it already exists, and throws exception if error occurs under creation</returns>
    public Task<Animal?> PostAnimal(Animal newAnimal);

    /// <summary>
    /// Gets all Animals in the table
    /// </summary>
    /// <returns>List of Animals, empty list if none is found</returns>
    public Task<List<Animal>> GetAllAnimals();

    /// <summary>
    /// Updates given Animal
    /// </summary>
    /// <param name="Animal">The new version of the Animal</param>
    /// <returns>The Animal that was updated, returns null if not succesfull</returns>
    public Task<Animal?> UpdateAnimal(Animal Animal);

    /// <summary>
    /// Delets Animal from table
    /// </summary>
    /// <param name="AnimalId">Id of the Animal needed to be deleted</param>
    /// <returns>Boolean, true if succesful and false if not</returns>
    public Task<bool> DeleteAnimal(string AnimalId);
}