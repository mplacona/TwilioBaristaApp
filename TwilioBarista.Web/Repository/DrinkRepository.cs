using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using TwilioBarista.Web.DAL;
using TwilioBarista.Web.Models;

namespace TwilioBarista.Web.Repository
{
    public class DrinkRepository : IDrinkRepository
    {
        private readonly TwilioBaristaContext _db;

        public DrinkRepository(TwilioBaristaContext db)
        {
            _db = db;
        }

        public virtual IEnumerable<Drink> SelectAll()
        {
            return _db.Drinks.ToList();
        }

        public virtual Drink SelectById(int? id)
        {
            return _db.Drinks.Find(id);
        }

        public virtual void Insert(Drink drink)
        {
            _db.Drinks.Add(drink);
        }

        public virtual void Update(Drink drink)
        {
            _db.Entry(drink).State = EntityState.Modified;
        }

        public virtual void Delete(int? id)
        {
            var existing = _db.Drinks.Find(id);
            _db.Drinks.Remove(existing);
        }

        public virtual void Save()
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