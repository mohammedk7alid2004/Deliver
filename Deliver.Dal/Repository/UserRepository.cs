using Deliver.Dal.Data;
using Deliver.Entities.Enums;
using Deliver.Entities.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}
