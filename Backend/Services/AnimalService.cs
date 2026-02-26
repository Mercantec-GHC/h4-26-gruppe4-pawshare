using Microsoft.EntityFrameworkCore;
using Models;
using Models.DTOs;
using Repositories.Interfaces;
using Services.Interfaces;

namespace Services;

public class AnimalService : IAnimalService
{
    private readonly IAnimalRepo _animalRepo;
    private readonly IMediaService _mediaService;

    public AnimalService(IAnimalRepo animalRepo, IMediaService mediaService)
    {
        _animalRepo = animalRepo;
        _mediaService = mediaService;
    }

    /// <inheritdoc/>
    public async Task<AnimalDto?> GetAnimalAsync(string id)
    {
        var entity = await _animalRepo.Query()
            .Include(a => a.User)
            .Include(a => a.AnimalType)
            .Include(a => a.Bookings)!.ThenInclude(b => b.Appointment)
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
            .Include(a => a.Bookings)!.ThenInclude(b => b.Appointment)
            .Where(a => a.TypeId == typeId)
            .ToListAsync();

        return entities.Select(AnimalMapper.ToDto).ToList();
    }

    /// <inheritdoc/>
    public async Task<List<AnimalDto>> GetAnimalsByUserAsync(string userId)
    {
        var entities = await _animalRepo.Query()
            .Include(a => a.User)
            .Include(a => a.AnimalType)
            .Include(a => a.Bookings)!.ThenInclude(b => b.Appointment)
            .Where(a => a.UserId == userId)
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

    /// <inheritdoc/>
    public async Task<bool> UpdateAnimalPictureAsync(string userId, string animalId, string newAnimalPictureKey, string? oldAnimalPictureKey = null)
    {
        var animal = await _animalRepo.GetAnimal(animalId);
        if (animal is null)
        {
            return false;
        }

        if (animal.UserId != userId)
        {
            return false;
        }

        if (!string.IsNullOrWhiteSpace(oldAnimalPictureKey))
        {
            await _mediaService.DeleteFileAsync(oldAnimalPictureKey);
        }

        animal.AnimalPictureKey = newAnimalPictureKey;
        animal.UpdatedAt = DateTime.UtcNow;

        var updated = await _animalRepo.UpdateAnimal(animal);
        if (updated == null)
        {
            return false;
        }

        return true;
    }
}

