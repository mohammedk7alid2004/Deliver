using Azure.Core;
using Deliver.Dal.Abstractions;
using Deliver.Dal.Data;
using Deliver.Entities.Entities;
using Deliver.Entities.Enums;
using Deliver.Entities.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Deliver.Dal.Repository
{
    public class UserRepository(UserManager<ApplicationUser> userManager,ApplicationDbContext context) : IUserRepository
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly ApplicationDbContext _context = context;


        public async Task<IdentityResult> CreateAsync(ApplicationUser user, string password)
        =>await _userManager.CreateAsync(user, password);

        public async Task<ApplicationUser?> FindByEmailAsync(string email)
        =>await _userManager.FindByEmailAsync(email);

        public async Task<IList<string>> GetRolesAsync(ApplicationUser user)
            =>await _userManager.GetRolesAsync(user);

        public async Task CreateUserProfileAsync(ApplicationUser user)
        {
            switch (user.UserType)
            {
                case UserType.Customer:
                    var customer = new Customer { ApplicationUserId = user.Id };
                    _context.Customers.Add(customer);
                    break;

                case UserType.Delivery:
                    var delivery = new Delivery { ApplicationUserId = user.Id };
                    _context.Deliveries.Add(delivery);
                    break;

                case UserType.Supplier:
                    var supplier = new Supplier { ApplicationUserId = user.Id };
                    _context.Suppliers.Add(supplier);
                    break;
            }

            await _context.SaveChangesAsync();
        }

        public async Task<IdentityResult> AddToRoleAsync(ApplicationUser user, string role)
           =>await _userManager.AddToRoleAsync(user, role);

        public async Task<bool?> Any(string email)
            =>await _context.Users.AnyAsync(x=>x.Email== email);

        public async Task<bool> AddAddress(int userId, string governmentName, string cityName, string zoneName, string streetName)
        {
            
            var government = await GetOrCreateAsync(
                g => g.Name == governmentName,
                () => new Government { Name = governmentName, CreatedAt = DateTime.UtcNow });

         
            var city = await GetOrCreateAsync(
                c => c.Name == cityName && c.GovernmentId == government.Id,
                () => new City { Name = cityName, GovernmentId = government.Id, CreatedAt = DateTime.UtcNow });

            
            var zone = await GetOrCreateAsync(
                z => z.Name == zoneName && z.CityId == city.Id,
                () => new Zone { Name = zoneName, CityId = city.Id, CreatedAt = DateTime.UtcNow });

      
            var street = await GetOrCreateAsync(
                s => s.Name == streetName && s.ZoneId == zone.Id,
                () => new Street { Name = streetName, ZoneId = zone.Id, CreatedAt = DateTime.UtcNow });

                var exists = await _context.addresses
                .AnyAsync(a => a.UserId == userId && a.StreetId == street.Id);

            if (exists)
            {
                return true;
            }

            _context.addresses.Add(new Address { UserId = userId, StreetId = street.Id });

            return await _context.SaveChangesAsync() > 0;
        }
        private async Task<T> GetOrCreateAsync<T>(Expression<Func<T, bool>> predicate,Func<T> createEntity) where T : class
        {
            var entity = await _context.Set<T>().FirstOrDefaultAsync(predicate);
            if (entity == null)
            {
                entity = createEntity();
                _context.Set<T>().Add(entity);

                await _context.SaveChangesAsync();
            }
            return entity;
        }




    }
}
