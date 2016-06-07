using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using TwilioBarista.Web.DAL;
using TwilioBarista.Web.Models;

namespace TwilioBarista.Web.Repository
{
    public class OrderRepository : IOrderRepository
    {

        private readonly TwilioBaristaContext _db;

        public OrderRepository(TwilioBaristaContext db)
        {
            _db = db;
        }

        public IEnumerable<Order> SelectAll()
        {
            return _db.Orders.ToList();
        }

        public Order SelectById(int? id)
        {
            return _db.Orders.Find(id);
        }

        public void Insert(Order order)
        {
            _db.Orders.Add(order);
        }

        public void Update(Order order)
        {
            _db.Entry(order).State = EntityState.Modified;
        }

        public void Delete(int? id)
        {
            var existing = _db.Orders.Find(id);
            _db.Orders.Remove(existing);
        }

        public void Save()
        {
            _db.SaveChanges();
        }

        public void Complete(int id)
        {
            var order = SelectById(id);
            order.Fulfilled = true;
            _db.Orders.Attach(order);
            _db.Entry(order).Property(x => x.Fulfilled).IsModified = true;
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