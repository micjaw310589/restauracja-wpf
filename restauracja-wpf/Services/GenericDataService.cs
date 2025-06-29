using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using restauracja_wpf.Data;
using restauracja_wpf.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace restauracja_wpf.Services
{
    public class GenericDataService<T> : IDataService<T> where T : DomainObject
    {
        private readonly RestaurantContextFactory _contextfactory;

        public GenericDataService(RestaurantContextFactory contextfactory)
        {
            _contextfactory = contextfactory;
        }

        public async Task<T> Create(T entity)
        {
            using(RestaurantContext context = _contextfactory.CreateDbContext())
            {
                EntityEntry<T> createdResult = await context.Set<T>().AddAsync(entity);
                await context.SaveChangesAsync();
                return createdResult.Entity;
            }
        }

        public async Task<bool> Delete(int id)
        {
            using (RestaurantContext context = _contextfactory.CreateDbContext())
            {
                // Soft delete implementation
                T entity = await context.Set<T>().FirstOrDefaultAsync((e) => e.Id == id);
                entity.isDeleted = true;
                context.Set<T>().Update(entity);
                await context.SaveChangesAsync();
                return true;
            }
        }

        public async Task<bool> HardDelete(int id)
        {
            using (RestaurantContext context = _contextfactory.CreateDbContext())
            {
                T entity = await context.Set<T>().FirstOrDefaultAsync((e) => e.Id == id);
                context.Set<T>().Remove(entity);
                await context.SaveChangesAsync();
                return true;
            }
        }

        public async Task<T> Get(int id)
        {
            using (RestaurantContext context = _contextfactory.CreateDbContext())
            {
                T entity = await context.Set<T>().FirstOrDefaultAsync((e) => e.Id == id);

                if (entity == null || entity.isDeleted)
                {
                    throw new KeyNotFoundException($"Entity with id {id} not found or has been deleted.");
                }

                return entity;
            }
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            using (RestaurantContext context = _contextfactory.CreateDbContext())
            {
                IEnumerable<T> entities = await context.Set<T>().ToListAsync();
                entities = entities.Where(e => !e.isDeleted);

                return entities;
            }
        }

        public async Task<T> Update(int id, T entity)
        {
            using (RestaurantContext context = _contextfactory.CreateDbContext())
            {
                entity.Id = id;
                context.Set<T>().Update(entity);
                await context.SaveChangesAsync();
                return entity;
            }
        }
    }
}
