using Microsoft.EntityFrameworkCore;
using MSNK.Data;
using MSNK.Models.Modules.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules.EFRepository
{
    public class BankRepository : IRepository<Bank, int>
    {
        public readonly ApplicationDbContext context;

        public BankRepository(ApplicationDbContext context) => this.context = context;

        public async Task Delete(int id)
        {
            var bank = await context.Bank.FirstOrDefaultAsync(b => b.Id == id);
            if (bank != null)
            {
                context.Remove(bank);
            }
        }

        public async Task<IEnumerable<Bank>> GetAll()
        {
            return await context.Bank.ToListAsync();
        }

        public async Task<Bank> GetById(int id)
        {
            return await context.Bank.FindAsync(id);
        }

        public async Task<Bank> Insert(Bank entity)
        {
            await context.Bank.AddAsync(entity);
            return entity;
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public async Task Update(Bank entity)
        {
            context.Update(entity);
            await context.SaveChangesAsync();
        }
    }
}
