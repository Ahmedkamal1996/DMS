using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DMSTask.DAL.Database;
using DMSTask.DAL.Models;

namespace DMSTask.BLL.Repositories
{
    public interface ICustomerRepository : IRepository<Customer, int>
    {
    }

    public class CustomerRepository : Repository<Customer, int>, ICustomerRepository
    {
        public CustomerRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
