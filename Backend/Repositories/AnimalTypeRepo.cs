using Repositories.Context;
using Repositories.Interfaces;

namespace Repositories;

using Microsoft.EntityFrameworkCore;
using Models;


public class AnimalTypeRepo : IAnimalTypeRepo
{
    private readonly AppDBContext _dbContext;

    public AnimalTypeRepo(AppDBContext dBContext)
    {
        _dbContext = dBContext;
    }

    public async Task<List<AnimalType>> GetAllAnimalTypes()
    {
        return await _dbContext.AnimalTypes.ToListAsync();
    }

    public async Task<AnimalType> GetAnimalType(string id)
    {
        var animalType = await _dbContext.AnimalTypes.FindAsync(id);
        if (animalType is null)
        {
            return null;
        }

        return animalType;
    }

    public async Task<AnimalType> PostAnimalType(AnimalType newAnimalType)
    {
        _dbContext.AnimalTypes.Add(newAnimalType);
        try
        {
            await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            if (_dbContext.Users.Any(e => e.Id == newAnimalType.Id))
            {
                return null;
            }
            else
            {
                throw;
            }
        }

        return newAnimalType;
    }
}