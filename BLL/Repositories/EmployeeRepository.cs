using BLL.Interfaces;
using DAL.Contexts;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Repositories
{
    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {
        private readonly CompanyDbContext _dbContext;

        public EmployeeRepository(CompanyDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        public IQueryable<Employee> GetEmployeesByAddress(string address)
        {
            if (string.IsNullOrEmpty(address))
                return Enumerable.Empty<Employee>().AsQueryable();

            return _dbContext.Employees
                             .AsNoTracking()
                             .Where(e => EF.Functions.Like(e.Address, $"%{address}%"));
        }


        public IQueryable<Employee> GetEmployeesByName(string Name)
        {
            if (string.IsNullOrEmpty(Name))
                return Enumerable.Empty<Employee>().AsQueryable();

            return _dbContext.Employees
                 .AsNoTracking()
                 .Where(e => EF.Functions.Like(e.Name, $"%{Name}%"));
        }
    }
}
