namespace Repositories.Interfaces;
using Models;

public interface IAnimalRepo
{
    public Task<Animal> GetAnimal(string id);
    public Task<List<Animal>> GetAnimalsFromType(string typeId);
    public Task<Animal> PostAnimal(Animal newAnimal);
    public Task<List<Animal>> GetAllAnimals();
}