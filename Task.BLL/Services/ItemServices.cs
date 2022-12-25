using DMSTask.BLL.ViewModel;
using DMSTask.DAL.Database;
using DMSTask.DAL.Enums;
using DMSTask.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSTask.BLL.Services
{
    public class ItemServices
    {
        private readonly ApplicationDbContext _context;
        public ItemServices(ApplicationDbContext context)
        {
            _context = context;
        }
        public ItemVM GetId(int? id)
        {
            if (id != default)
            {
                var model = _context.Items.Find(id);
                ItemVM modelVM = new ItemVM();

                modelVM.Id = model.Id;
                modelVM.Name = model.Name;
                modelVM.UOMId = model.UOMId;
                modelVM.Quantity = model.Quantity;
                modelVM.Price = model.Price;
                modelVM.Description = model.Description;
                return modelVM;
            }
            else
            {
                return null;
            }
        }
        public List<OrderHeader> GetOrders(Customer? customer)
        {
            var orders =  _context.OrderHeaders
               .Include(x => x.OrderDetails).ThenInclude(x => x.Items).ThenInclude(x => x.UnitOfMeasure)
               .Where(x => x.CustomerId == customer.CustomerCode)
               .OrderByDescending(x => x.OrderDate)
               .ToList();
            return orders;
        }

        public Items Details(int? id)
        {
            var model =  _context.Items
                .Include(i => i.UnitOfMeasure)
                .FirstOrDefault(m => m.Id == id);
            return model;
        }
        public OrderHeader GetItemToCard( Customer? customer)
        {
            var order =  _context.OrderHeaders
               .Include(x => x.OrderDetails).ThenInclude(x => x.Items).ThenInclude(x => x.UnitOfMeasure)
               .FirstOrDefault(x =>
                   x.CustomerId == customer.CustomerCode &&
                   x.Status == OrderStatus.Open
               );
            return order;
        }
        public bool SaveOrder(int orderId,Customer? customer)
        {
            var order =  _context.OrderHeaders
               .FirstOrDefault(x =>
                   x.CustomerId == customer.CustomerCode &&
                   x.Status == OrderStatus.Open &&
                   x.Id == orderId
               );

            if (order == null) return false;

            order.Status = OrderStatus.Closed;
            order.DueDate = DateTime.Now;

            _context.OrderHeaders.Update(order);
             _context.SaveChanges();
            return true;
        }
        public bool RemoveItemFromCard(int orderDetailId)
        {
            var orderDetail =  _context.OrderDetails
                .Include(o => o.OrderHeader).Include(o => o.Items)
                .FirstOrDefault(x => x.Id == orderDetailId);

            if (orderDetail == null)
            {
                return false;
            }

            orderDetail.OrderHeader.TotalPrice -= orderDetail.TotalPrice;
            orderDetail.Items.Quantity += orderDetail.Qty;

            _context.OrderDetails.Remove(orderDetail);

            var countOfOrderDetails =  _context.OrderDetails.Count(x => x.OrderId == orderDetail.OrderId);
            if (countOfOrderDetails > 0)
            {
                _context.OrderHeaders.Remove(orderDetail.OrderHeader);
            }

             _context.SaveChanges();
            return true;
        }
        public void Delete(int id)
        {
            var model = _context.Items.Find(id);
            if (model != null)
            {
                _context.Items.Remove(model);
                _context.SaveChanges();
            }
        }
        public void Save(ItemVM modelvm)
        {
            if (modelvm.Id == 0)
            {
                Items model = new Items();
                model.Name = modelvm.Name;
                model.UOMId = modelvm.UOMId;
                model.Quantity = modelvm.Quantity;
                model.Price = modelvm.Price;
                model.Description = modelvm.Description;

                _context.Items.Add(model);
                _context.SaveChanges();
            }
            else
            {
                var model = _context.Items.FirstOrDefault(x => x.Id == modelvm.Id);
                if (model != null)
                {
                    model.Name = modelvm.Name;
                    model.UOMId = modelvm.UOMId;
                    model.Quantity = modelvm.Quantity;
                    model.Price = modelvm.Price;
                    model.Description = modelvm.Description;

                    _context.Items.Update(model);
                    _context.SaveChanges();
                }
            }
        }
        public OrderHeader GetOrderDetails(int orderId, Customer customer)
        {
            var order =  _context.OrderHeaders
               .Include(x => x.OrderDetails).ThenInclude(x => x.Items).ThenInclude(x => x.UnitOfMeasure)
               .FirstOrDefault(x => x.CustomerId == customer.CustomerCode && x.Id == orderId);
            return order;
        }

        public void AddItemToCart(int id, int quantity,Customer customer)
        {
            var item =  _context.Items.Find(id);
            var order =  _context.OrderHeaders
                .Include(o => o.OrderDetails.Where(od => od.ItemId == item.Id))
                .FirstOrDefault(x =>
                    x.CustomerId == customer.CustomerCode &&
                    x.Status == OrderStatus.Open
                );

            if (order != null)
            {
                OrderDetails orderDetails = new();
                //If there is OrderDetails for item - update
                if (order.OrderDetails?.Count > 0)
                {
                    orderDetails = order.OrderDetails.First();

                    //Reset Order TotalPrice
                    order.TotalPrice -= orderDetails.TotalPrice;
                    //Re-calculate
                    orderDetails.TotalPrice += CalculatePrice(quantity, item.Price, order.TaxValue, order.DiscountValue);
                    orderDetails.Qty += quantity;
                    orderDetails.ItemPrice = item.Price;
                    orderDetails.Tax = order.TaxValue;
                    orderDetails.Discount = order.DiscountValue;
                    orderDetails.UOMId = item.UOMId;

                    _context.OrderDetails.Update(orderDetails);
                }
                else //Else - Add
                {
                    orderDetails = new()
                    {
                        OrderId = order.Id,
                        ItemId = item.Id,
                        ItemPrice = item.Price,
                        Qty = quantity,
                        Tax = order.TaxValue,
                        Discount = order.DiscountValue,
                        UOMId = item.UOMId,
                        TotalPrice = CalculatePrice(quantity, item.Price, order.TaxValue, order.DiscountValue)
                    };

                    _context.OrderDetails.Add(orderDetails);
                }

                order.TotalPrice += orderDetails.TotalPrice;

                item.Quantity -= quantity;
            }
            else
            {
                order = new OrderHeader()
                {
                    CustomerId = customer.CustomerCode,
                    OrderDate = DateTime.Now,
                    RequestDate = DateTime.Now,
                    Status = OrderStatus.Open,
                    TaxCode = 1,
                    TaxValue = 0,
                    DiscountCode = 1,
                    DiscountValue = 0,
                    TotalPrice = CalculatePrice(quantity, item.Price, 0, 0),
                    OrderDetails = new List<OrderDetails>()
                    {
                        new OrderDetails()
                        {
                            ItemId = item.Id,
                            ItemPrice = item.Price,
                            Qty = quantity,
                            Tax = 0,
                            Discount = 0,
                            UOMId = item.UOMId,
                            TotalPrice = CalculatePrice(quantity, item.Price, 0, 0)
                        }
                    }
                };

                _context.OrderHeaders.Add(order);

                item.Quantity -= quantity;
            }

             _context.SaveChanges();
        }

        private static decimal CalculatePrice(int quantity, decimal price, decimal taxValue, decimal discountValue)
        {
            return (price * quantity) + taxValue - discountValue;
        }
    }
}
