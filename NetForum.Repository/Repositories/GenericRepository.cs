using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.EntityFrameworkCore;
using NetForum.Data;
using NetForum.Repository.Interfaces;
using System.Linq.Expressions;

namespace NetForum.Repository.Repositories;

// DP: Repository Pattern
// SOLID: Liskov Substitution Principle - las clases derivadas
// solo agregan métodos, nunca sobrescriben el comportamiento heredado.
public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    protected readonly NetForumDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public GenericRepository(NetForumDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<T> AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();

        return entity;
    }

    public async Task UpdateAsync(T entity)
    {
        _dbSet.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await GetByIdAsync(id);

        if (entity is null)
        {
            return;
        }

        _dbSet.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(int id)
    {
        var entity = await GetByIdAsync(id);
        return entity is not null;
    }

    public async Task<IEnumerable<T>> FindAsync(
        Expression<Func<T, bool>> predicate)
    {
        return await _dbSet
            .Where(predicate)
            .ToListAsync();
    }
}