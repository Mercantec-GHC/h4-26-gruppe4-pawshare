using Services.Interfaces;
using Models;
using Repositories.Interfaces;

namespace Services;

public class AnimalService : IAnimalService
{
    private readonly IAnimalRepo _animalRepo;

    public AnimalService(IAnimalRepo animalRepo)
    {
        _animalRepo = animalRepo;
    }

    public async Task<Animal?> GetAnimalAsync(string id)
    {
        return await _animalRepo.GetAnimal(id);
    }

    public async Task<List<Animal>> GetAllAnimalsAsync()
    {
        return await _animalRepo.GetAllAnimals();
    }

    public async Task<List<Animal>> GetAnimalsByTypeAsync(string typeId)
    {
        return await _animalRepo.GetAnimalsFromType(typeId);
    }

    public async Task<List<Animal>> GetAnimalsByUserAsync(string userId)
    {
        var allAnimals = await _animalRepo.GetAllAnimals();
        return allAnimals.Where(a => a.UserId == userId).ToList();
    }

    public async Task<Animal?> CreateAnimalAsync(Animal animal)
    {
        animal.Id = Guid.NewGuid().ToString();
        animal.CreatedAt = DateTime.UtcNow;
        animal.UpdatedAt = DateTime.UtcNow;
        return await _animalRepo.PostAnimal(animal);
    }

    public async Task<Animal?> UpdateAnimalAsync(string id, Animal animal)
    {
        var existing = await _animalRepo.GetAnimal(id);
        if (existing == null) return null;

        existing.Name = animal.Name;
        existing.Description = animal.Description;
        existing.Base64Image = animal.Base64Image;
        existing.Age = animal.Age;
        existing.TypeId = animal.TypeId;
        existing.UpdatedAt = DateTime.UtcNow;

        return existing;
    }

    public async Task<bool> DeleteAnimalAsync(string id)
    {
        var animal = await _animalRepo.GetAnimal(id);
        return animal != null;
    }
}
