namespace Repositories.Interfaces;
using Models;

public interface IAnimalTypeRepo
{
    public Task<AnimalType> GetAnimalType(string id);
    public Task<AnimalType> PostAnimalType(AnimalType animalType);
    public Task<List<AnimalType>> GetAllAnimalTypes();
}