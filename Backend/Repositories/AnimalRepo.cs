using Microsoft.EntityFrameworkCore;
using Models;
using Models.DTOs;
using Repositories.Context;
using Repositories.Interfaces;


namespace Repositories;


public class AnimalRepo : IAnimalRepo
{
    private readonly AppDBContext _dbContext;

    public AnimalRepo(AppDBContext dBContext)
    {
        _dbContext = dBContext;
    }

    /// <inheritdoc/>
    public async Task<Animal?> GetAnimalEntity(string id)
    {
        return await _dbContext.Animals
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    /// <inheritdoc/>
    public async Task<List<AnimalDto>> GetAllAnimals()
    {
        return await _dbContext.Animals
            .Include(a => a.User)
            .Include(a => a.AnimalType)
            .Include(a => a.Bookings)!
                .ThenInclude(b => b.Appointment)
            .Select(a => AnimalMapper.ToDto(a))
            .ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<AnimalDto?> GetAnimal(string id)
    {
        var animal = await _dbContext.Animals
            .Include(a => a.User)
            .Include(a => a.AnimalType)
            .Include(a => a.Bookings)!
                .ThenInclude(b => b.Appointment)
            .FirstOrDefaultAsync(a => a.Id == id);

        return animal is null ? null : AnimalMapper.ToDto(animal);
    }


    /// <inheritdoc/>
    public List<AnimalDto> GetAnimalsFromType(string typeId)
    {
        return _dbContext.Animals
            .Include(a => a.User)
            .Include(a => a.AnimalType)
            .Include(a => a.Bookings)!
                .ThenInclude(b => b.Appointment)
            .Where(a => a.AnimalType != null && a.AnimalType.Id == typeId)
            .Select(a => AnimalMapper.ToDto(a))
            .ToList();
    }


    /// <inheritdoc/>
    public async Task<Animal?> PostAnimal(Animal newAnimal)
    {
        _dbContext.Animals.Add(newAnimal);
        try
        {
            await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            if (_dbContext.Users.Any(e => e.Id == newAnimal.Id))
            {
                return null;
            }
            else
            {
                throw;
            }
        }

        return newAnimal;
    }

    /// <inheritdoc/>
    public async Task<Animal?> UpdateAnimal(Animal newAnimal)
    {
        _dbContext.Entry(newAnimal).State = EntityState.Modified;

        try
        {
            await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_dbContext.Animals.Any(e => e.Id == newAnimal.Id))
            {
                return null;
            }
            else
            {
                throw;
            }
        }

        return newAnimal;
    }

    /// <inheritdoc/>
    public async Task<bool> DeleteAnimal(string AnimalId)
    {
        Animal? animal = await _dbContext.Animals.FindAsync(AnimalId);
        if (animal == null)
        {
            return false;
        }

        _dbContext.Animals.Remove(animal);
        await _dbContext.SaveChangesAsync();

        return true;
    }
}