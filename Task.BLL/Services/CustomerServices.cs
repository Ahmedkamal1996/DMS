using DMSTask.BLL.ViewModel;
using DMSTask.DAL.Database;
using DMSTask.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSTask.BLL.Services
{
    public class CustomerServices
    {
        private readonly ApplicationDbContext _context;
        public CustomerServices(ApplicationDbContext context)
        {
            _context = context;
        }

        public CustomerVM GetCustomerById(int? id)
        {
            if (id != default)
            {
                var customer = _context.Customers.Find(id);
                CustomerVM model = new CustomerVM();

                model.CustomerCode = customer.CustomerCode;
                model.CustomerDescriptionAr = customer.CustomerDescriptionAr;
                model.CustomerDescriptionEn = customer.CustomerDescriptionEn;
                model.UserId = customer.UserId;
                return model;
            }
            else
            {
                return null;
            }
        }
        public CustomerVM GetCustomer(int? id)
        {
            if (id != default)
            {
                var customer = _context.Customers.Include(c => c.User)
                  
                    .FirstOrDefault(m => m.CustomerCode == id);
                CustomerVM model = new CustomerVM();

                model.CustomerCode = customer.CustomerCode;
                model.CustomerDescriptionAr = customer.CustomerDescriptionAr;
                model.CustomerDescriptionEn = customer.CustomerDescriptionEn;
                model.UserId = customer.UserId;
                    return model;
                
            }
            else
            {
                return null;
            }
        }
        public void Delete(int id)
        {
            var customer = _context.Customers.Find(id);
            if (customer != null)
            {
                _context.Customers.Remove(customer);
                _context.SaveChanges();
            }
        }
        public void Save(CustomerVM customer)
        {
            if (customer.CustomerCode == 0)
            {
                Customer model = new Customer();
                model.CustomerCode = customer.CustomerCode;
                model.CustomerDescriptionAr = customer.CustomerDescriptionAr;
                model.CustomerDescriptionEn = customer.CustomerDescriptionEn;
                model.UserId = customer.UserId;
                _context.Customers.Add(model);
                _context.SaveChanges();
            }
            else
            {
                var customerDb = _context.Customers.FirstOrDefault(x => x.CustomerCode == customer.CustomerCode);
                if (customerDb!=null)
                {
                    customerDb.CustomerDescriptionEn = customer.CustomerDescriptionEn;
                    customerDb.CustomerDescriptionAr = customer.CustomerDescriptionAr;
                    customerDb.UserId = customer.UserId;

                _context.Customers.Update(customerDb);
                _context.SaveChanges();
                }
            }
        }
    }
}
