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
    public class OrderServices
    {
        private readonly ApplicationDbContext _context;
        public OrderServices(ApplicationDbContext context)
        {
            _context = context;
        }

        public OrderHeaderVM GetId(int? id)
        {
            if (id != default)
            {
                var orderHeader  = _context.OrderHeaders.Find(id);
                OrderHeaderVM model = new OrderHeaderVM();

                model.Id = orderHeader.Id;
                model.CustomerId = orderHeader.CustomerId;
                model.OrderDate = orderHeader.OrderDate;
                model.RequestDate = orderHeader.RequestDate;
                model.DueDate = orderHeader.DueDate;
                model.Status = orderHeader.Status;
                model.TaxCode = orderHeader.TaxCode;
                model.TaxValue = orderHeader.TaxValue;
                model.DiscountCode = orderHeader.DiscountCode; 
               model.DiscountValue = orderHeader.DiscountValue;
                model.TotalPrice = orderHeader.TotalPrice;
                return model;
            }
            else
            {
                return null;
            }
        }
        public OrderHeader Show(int? id)
        {
            var model= _context.OrderHeaders
                .Include(o => o.Customer)
                .FirstOrDefault(m => m.Id == id);
            return model;
        }
        public OrderHeader Details(int? id)
        {
            var model=  _context.OrderHeaders
                .Include(o => o.Customer)
                .Include(o => o.OrderDetails).ThenInclude(o => o.Items)
                .FirstOrDefault(m => m.Id == id);
            return model;
        }
        public void Delete(int id)
        {
            var model = _context.OrderHeaders.Find(id);
            if (model != null)
            {
                _context.OrderHeaders.Remove(model);
                _context.SaveChanges();
            }
        }
        public void Save(OrderHeaderVM modelvm)
        {
            if (modelvm.Id == 0)
            {
                OrderHeader model = new OrderHeader();
                model.CustomerId = modelvm.CustomerId;
                model.OrderDate = modelvm.OrderDate;
                model.RequestDate = modelvm.RequestDate;
                model.DueDate = modelvm.DueDate;
                model.Status = modelvm.Status;
                model.TaxCode = modelvm.TaxCode;
                model.TaxValue = modelvm.TaxValue;
                model.DiscountCode = modelvm.DiscountCode;
                model.DiscountValue = modelvm.DiscountValue;
                model.TotalPrice = modelvm.TotalPrice;
                _context.OrderHeaders.Add(model);
                _context.SaveChanges();
            }
            else
            {
                var model = _context.OrderHeaders.FirstOrDefault(x => x.Id == modelvm.Id);
                if (model != null)
                {
                    model.Id = modelvm.Id;
                    model.CustomerId = modelvm.CustomerId;
                    model.OrderDate = modelvm.OrderDate;
                    model.RequestDate = modelvm.RequestDate;
                    model.DueDate = modelvm.DueDate;
                    model.Status = modelvm.Status;
                    model.TaxCode = modelvm.TaxCode;
                    model.TaxValue = modelvm.TaxValue;
                    model.DiscountCode = modelvm.DiscountCode;
                    model.DiscountValue = modelvm.DiscountValue;
                    model.TotalPrice = modelvm.TotalPrice;

                    _context.OrderHeaders.Update(model);
                    _context.SaveChanges();
                }
            }
        }
    }
}
