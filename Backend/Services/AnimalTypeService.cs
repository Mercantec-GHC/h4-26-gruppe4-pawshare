using Services.Interfaces;
using Models;
using Repositories.Interfaces;

namespace Services;

public class AnimalTypeService : IAnimalTypeService
{
    private readonly IAnimalTypeRepo _animalTypeRepo;

    public AnimalTypeService(IAnimalTypeRepo animalTypeRepo)
    {
        _animalTypeRepo = animalTypeRepo;
    }

    public async Task<AnimalType?> GetAnimalTypeAsync(string id)
    {
        return await _animalTypeRepo.GetAnimalType(id);
    }

    public async Task<List<AnimalType>> GetAllAnimalTypesAsync()
    {
        return await _animalTypeRepo.GetAllAnimalTypes();
    }

    public async Task<AnimalType?> CreateAnimalTypeAsync(AnimalType animalType)
    {
        animalType.Id = Guid.NewGuid().ToString();
        animalType.CreatedAt = DateTime.UtcNow;
        animalType.UpdatedAt = DateTime.UtcNow;
        return await _animalTypeRepo.PostAnimalType(animalType);
    }

    public async Task<AnimalType?> UpdateAnimalTypeAsync(string id, AnimalType animalType)
    {
        var existing = await _animalTypeRepo.GetAnimalType(id);
        if (existing == null) return null;

        existing.Name = animalType.Name;
        existing.Description = animalType.Description;
        existing.UpdatedAt = DateTime.UtcNow;

        return await _animalTypeRepo.UpdateAnimalType(existing);
    }

    public async Task<bool> DeleteAnimalTypeAsync(string id)
    {
        return await _animalTypeRepo.DeleteAnimalType(id);
    }
}
