using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Sports_Playlist_Server.Data;
using Sports_Playlist_Server.Interfaces;

namespace Sports_Playlist_Server.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<T> GetByIdAsync(int id)
        {
            var entity = await _context.Set<T>().FindAsync(id);
            if(entity == null)
            {
                throw new KeyNotFoundException($"Entity with id {id} not found.");
            }
            return entity;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>().AsNoTracking().ToListAsync();
        }

        public async Task AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity); 
            await _context.SaveChangesAsync(); 
        }

        public async Task UpdateAsync(T entity)
        {
            _context.Set<T>().Update(entity); 
            await _context.SaveChangesAsync();        
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _context.Set<T>().Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}