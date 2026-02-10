using Models;
using Models.DTOs;

namespace Repositories.Interfaces;

public interface IAnimalRepo
{
    /// <summary> 
    /// Returns a queryable Animal source for the service to apply includes and filtering. 
    /// </summary> 
    public IQueryable<Animal> Query();

    /// <summary>
    /// Gets Animal with given Id from table
    /// </summary>
    /// <param name="id">The id of the wanted Animal</param>
    /// <returns>Animal with given id, if not found returns null</returns>
    public Task<Animal?> GetAnimal(string id);

    /// <summary>
    /// Posts a new Animal to the table
    /// </summary>
    /// <param name="newAnimal">The new Animal that needs to be created</param>
    /// <returns>Animal that was added, and throws exception if error occurs under creation</returns>
    public Task<Animal> PostAnimal(Animal newAnimal);

    /// <summary>
    /// Gets all Animals in the table
    /// </summary>
    /// <returns>List of Animals, empty list if none is found</returns>
    public Task<List<Animal>> GetAllAnimals();

    /// <summary>
    /// Updates given Animal
    /// </summary>
    /// <param name="Animal">The new version of the Animal</param>
    /// <returns>The Animal that was updated</returns>
    public Task<Animal> UpdateAnimal(Animal animal);

    /// <summary>
    /// Deletes Animal from table
    /// </summary>
    /// <param name="AnimalId">Id of the Animal needed to be deleted</param>
    /// <returns>Boolean, true if succesful and false if not</returns>
    public Task<bool> DeleteAnimal(string AnimalId);
}