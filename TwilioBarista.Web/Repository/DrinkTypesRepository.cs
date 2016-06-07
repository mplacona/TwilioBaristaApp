using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using TwilioBarista.Web.DAL;
using TwilioBarista.Web.Models;

namespace TwilioBarista.Web.Repository
{
    public class DrinkTypesRepository : IDrinkTypesRepository
    {
        private readonly TwilioBaristaContext _db;

        public DrinkTypesRepository(TwilioBaristaContext db)
        {
            _db = db;
        }

        public IEnumerable<DrinkType> SelectAll()
        {
            return _db.DrinkTypes;
        }

        public DrinkType SelectById(int? id)
        {
            return _db.DrinkTypes.Find(id);
        }

        public void Insert(DrinkType drinkType)
        {
            _db.DrinkTypes.Add(drinkType);
        }

        public void Update(DrinkType drinkType)
        {
            _db.Entry(drinkType).State = EntityState.Modified;
        }

        public void Delete(int? id)
        {
            var existing = _db.DrinkTypes.Find(id);
            _db.DrinkTypes.Remove(existing);
        }

        public void Save()
        {
            _db.SaveChanges();
        }

        private bool _disposed;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _db.Dispose();
                }
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}