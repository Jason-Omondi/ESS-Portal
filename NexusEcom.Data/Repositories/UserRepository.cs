using Microsoft.EntityFrameworkCore;
using NexusEcom.DataAccess.Context;
using NexusEcom.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NexusEcom.DataAccess.Repositories
{
    public class UserRepository
    {
        private readonly AppDbContext appDbContext;

        public UserRepository(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            try
            {
                return await appDbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
            }
            catch (Exception ex) 
            {
            Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task AddUserAsync(User user)
        {
            try
            {
                await appDbContext.Users.AddAsync(user);
                await appDbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }

        }
    }
}
