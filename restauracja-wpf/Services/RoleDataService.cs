using restauracja_wpf.Models;
using restauracja_wpf.Services;
using restauracja_wpf.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;
namespace restauracja_wpf.Interfaces
{
    public class RoleDataService(GenericDataService<Role> roleService) : GenericDataService<Role>(new RestaurantContextFactory())
    {

        public async void AddRole(string name)
        {
            using (var context = new RestaurantContextFactory().CreateDbContext())
            {
                await roleService.Create(new Role()
                {
                    Name = name
                });
            }
        }

        public async Task<IEnumerable<Role>> GetAllRoles()
        {
            using (var context = new RestaurantContextFactory().CreateDbContext())
            {
                IEnumerable<Role> roles = await context.Set<Role>().ToListAsync();
                return roles;
            }
        }

        public async Task<Role> GetRoleByName(string name)
        {
            using (var context = new RestaurantContextFactory().CreateDbContext())
            {
                Role role = await context.Set<Role>()
                    .FirstOrDefaultAsync(r => r.Name.Contains(name));
                return role;
            }
        }

        public async Task<bool> ClearRoles()
        {
            using (var context = new RestaurantContextFactory().CreateDbContext())
            {
                IEnumerable<Role> roles = await context.Set<Role>().ToListAsync();
                foreach (var role in roles)
                {
                    context.Set<Role>().Remove(role);
                }
                await context.SaveChangesAsync();
                return true;
            }
        }
    }
}
