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
    public class UnitOfMeasureServices
    {
        private readonly ApplicationDbContext _context;
        public UnitOfMeasureServices(ApplicationDbContext context)
        {
            _context = context;
        }

        public UnitOfMeasureVM GetId(int? id)
        {
            if (id != default)
            {
                var unitOfMeasure = _context.UnitOfMeasures.Find(id);
                UnitOfMeasureVM model = new UnitOfMeasureVM();

                model.Id = unitOfMeasure.Id;
                model.UOM = unitOfMeasure.UOM;
                model.Description = unitOfMeasure.Description;
               
                return model;
            }
            else
            {
                return null;
            }
        }
      
        public void Delete(int id)
        {
            var model = _context.UnitOfMeasures.Find(id);
            if (model != null)
            {
                _context.UnitOfMeasures.Remove(model);
                _context.SaveChanges();
            }
        }
        public void Save(UnitOfMeasureVM modelvm)
        {
            if (modelvm.Id == 0)
            {
                UnitOfMeasure model = new UnitOfMeasure();

                model.Id = modelvm.Id;
                model.UOM = modelvm.UOM;
                model.Description = modelvm.Description;
                _context.UnitOfMeasures.Add(model);
                _context.SaveChanges();
            }
            else
            {
                var UomDb = _context.UnitOfMeasures.FirstOrDefault(x => x.Id == modelvm.Id);
                if (UomDb != null)
                {
                    UomDb.Id = modelvm.Id;
                    UomDb.UOM = modelvm.UOM;
                    UomDb.Description = modelvm.Description;

                    _context.UnitOfMeasures.Update(UomDb);
                    _context.SaveChanges();
                }
            }
        }
    }
}
