using Microsoft.EntityFrameworkCore;
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

    /// <inheritdoc/>
    public async Task<AnimalDto?> GetAnimalAsync(string id)
    {
        var entity = await _animalRepo.Query()
            .Include(a => a.User)
            .Include(a => a.AnimalType)
            .Include(a => a.Bookings)!
                .ThenInclude(b => b.Appointment)
            .FirstOrDefaultAsync(a => a.Id == id);

        return entity is null ? null : AnimalMapper.ToDto(entity);
    }

    /// <inheritdoc/>
    public async Task<List<AnimalDto>> GetAllAnimalsAsync()
    {
        var entities = await _animalRepo.Query()
            .Include(a => a.User)
            .Include(a => a.AnimalType)
            .Include(a => a.Bookings)!
                .ThenInclude(b => b.Appointment)
            .ToListAsync();

        return entities.Select(AnimalMapper.ToDto).ToList();
    }

    /// <inheritdoc/>
    public async Task<List<AnimalDto>> GetAnimalsByTypeAsync(string typeId)
    {
        var entities = await _animalRepo.Query()
            .Include(a => a.User)
            .Include(a => a.AnimalType)
            .Include(a => a.Bookings)!
                .ThenInclude(b => b.Appointment)
            .Where(a => a.AnimalType != null && a.AnimalType.Id == typeId)
            .ToListAsync();

        return entities.Select(AnimalMapper.ToDto).ToList();
    }

    /// <inheritdoc/>
    public async Task<List<AnimalDto>> GetAnimalsByUserAsync(string userId)
    {
        var entities = await _animalRepo.Query()
            .Include(a => a.User)
            .Include(a => a.AnimalType)
            .Include(a => a.Bookings)!
                .ThenInclude(b => b.Appointment)
            .Where(a => a.User != null && a.User.Id == userId)
            .ToListAsync();

        return entities.Select(AnimalMapper.ToDto).ToList();
    }

    /// <inheritdoc/>
    public async Task<Animal?> CreateAnimalAsync(Animal animal)
    {
        animal.Id = Guid.NewGuid().ToString();
        animal.CreatedAt = DateTime.UtcNow;
        animal.UpdatedAt = DateTime.UtcNow;

        return await _animalRepo.PostAnimal(animal);
    }

    /// <inheritdoc/>
    public async Task<AnimalDto?> UpdateAnimalAsync(string id, AnimalDto dto)
    {
        var existing = await _animalRepo.GetAnimal(id);
        if (existing is null)
            return null;

        AnimalMapper.MapToEntity(dto, existing);
        existing.UpdatedAt = DateTime.UtcNow;

        var updated = await _animalRepo.UpdateAnimal(existing);
        return updated is null ? null : AnimalMapper.ToDto(updated);
    }

    /// <inheritdoc/>
    public async Task<bool> DeleteAnimalAsync(string id)
    {
        return await _animalRepo.DeleteAnimal(id);
    }
}

