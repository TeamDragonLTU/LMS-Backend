using Domain.Contracts.Repositories;
using Domain.Models.Entities;
using LMS.Infractructure.Data;
using LMS.Infractructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LMS.Infrastructure.Repositories
{
    public class ModuleRepository : RepositoryBase<Module>, IModuleRepository
    {
       
        public ModuleRepository(ApplicationDbContext context) : base(context)
        {
           
        }

        public async Task<Module?> GetModuleAsync(int moduleId)
        {
            return await FindAll().FirstOrDefaultAsync(m => m.Id == moduleId);
        }

        public async Task<IEnumerable<Module>> GetAllModulesAsync()
        {
            return await FindAll().ToListAsync();
        }
        
    }
}
