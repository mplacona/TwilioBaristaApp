using System;
using System.Collections.Generic;
using TwilioBarista.Web.Models;

namespace TwilioBarista.Web.Repository
{
    public interface IDrinkRepository : IDisposable
    {
        IEnumerable<Drink> SelectAll();
        Drink SelectById(int? id);
        void Insert(Drink drink);
        void Update(Drink drink);
        void Delete(int? id);
        void Save();
    }
}