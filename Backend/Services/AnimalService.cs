using Models;
using Models.DTOs;
using Repositories.Interfaces;
using Services.Interfaces;

namespace Services;

public class AnimalService : IAnimalService
{
    private readonly IAnimalRepo _animalRepo;

    public AnimalService(IAnimalRepo animalRepo)
    {
        _animalRepo = animalRepo;
    }

    public async Task<AnimalDto?> GetAnimalAsync(string id)
    {
        return await _animalRepo.GetAnimal(id);
    }

    public async Task<List<AnimalDto>> GetAllAnimalsAsync()
    {
        return await _animalRepo.GetAllAnimals();
    }

    public async Task<List<AnimalDto>> GetAnimalsByTypeAsync(string typeId)
    {
        return _animalRepo.GetAnimalsFromType(typeId);
    }

    public async Task<List<AnimalDto>> GetAnimalsByUserAsync(string userId)
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

    public async Task<AnimalDto?> UpdateAnimalAsync(string id, AnimalDto dto)
    {
        var existing = await _animalRepo.GetAnimalEntity(id);
        if (existing is null)
            return null;

        AnimalMapper.MapToEntity(dto, existing);

        var updated = await _animalRepo.UpdateAnimal(existing);
        return updated is null ? null : AnimalMapper.ToDto(updated);
    }

    public async Task<bool> DeleteAnimalAsync(string id)
    {
        return await _animalRepo.DeleteAnimal(id);
    }
}
