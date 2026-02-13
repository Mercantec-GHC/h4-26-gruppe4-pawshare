using Microsoft.EntityFrameworkCore;
using Models;
using Repositories.Context;
using Repositories.Interfaces;


namespace Repositories;


public class AnimalRepo : IAnimalRepo
{
    private readonly AppDBContext _dbContext;

    public AnimalRepo(AppDBContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <inheritdoc/>
    public IQueryable<Animal> Query()
    {
        return _dbContext.Animals.AsQueryable();
    }

    /// <inheritdoc/>
    public async Task<Animal?> GetAnimal(string id)
    {
        return await _dbContext.Animals.FindAsync(id);
    }

    /// <inheritdoc/>
    public async Task<List<Animal>> GetAllAnimals()
    {
        return await _dbContext.Animals.ToListAsync();
    }


    /// <inheritdoc/>
    public async Task<Animal> PostAnimal(Animal newAnimal)
    {
        _dbContext.Animals.Add(newAnimal);
        await _dbContext.SaveChangesAsync();
        return newAnimal;
    }

    /// <inheritdoc/>
    public async Task<Animal> UpdateAnimal(Animal animal)
    {
        _dbContext.Animals.Update(animal);
        await _dbContext.SaveChangesAsync();
        return animal;
    }

    /// <inheritdoc/>
    public async Task<bool> DeleteAnimal(string AnimalId)
    {
        Animal? animal = await _dbContext.Animals.FindAsync(AnimalId);
        if (animal == null)
            return false;

        _dbContext.Animals.Remove(animal);
        await _dbContext.SaveChangesAsync();
        return true;
    }
}
