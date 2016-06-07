using System;
using System.Collections.Generic;
using TwilioBarista.Web.Models;

namespace TwilioBarista.Web.Repository
{
    public interface IOrderRepository : IDisposable
    {
        IEnumerable<Order> SelectAll();
        Order SelectById(int? id);
        void Insert(Order order);
        void Update(Order order);
        void Delete(int? id);
        void Save();
        void Complete(int id);
        
    }
}
