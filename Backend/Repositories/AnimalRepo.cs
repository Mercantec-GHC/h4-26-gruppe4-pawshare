using Repositories.Context;
using Repositories.Interfaces;

namespace Repositories;

using Microsoft.EntityFrameworkCore;
using Models;


public class AnimalRepo : IAnimalRepo
{
    private readonly AppDBContext _dbContext;

    public AnimalRepo(AppDBContext dBContext)
    {
        _dbContext = dBContext;
    }

    /// <inheritdoc/>
    public async Task<List<Animal>> GetAllAnimals()
    {
        return await _dbContext.Animals.ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<Animal?> GetAnimal(string id)
    {
        var animal = await _dbContext.Animals.FindAsync(id);
        if (animal is null)
        {
            return null;
        }

        return animal;
    }

    /// <inheritdoc/>
    public async Task<List<Animal>> GetAnimalsFromType(string typeId)
    {
        List<Animal> animal = _dbContext.Animals.Where(e => e.AnimalType != null && e.AnimalType.Id.Equals(typeId)).ToList();
        if (animal is null)
        {
            return [];
        }

        return animal;
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
}