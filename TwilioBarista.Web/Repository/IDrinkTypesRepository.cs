using System.Collections.Generic;
using System.Linq;
using TwilioBarista.Web.Models;

namespace TwilioBarista.Web.Repository
{
    public interface IDrinkTypesRepository
    {
        IEnumerable<DrinkType> SelectAll();
        DrinkType SelectById(int? id);
        void Insert(DrinkType drinkType);
        void Update(DrinkType drinkType);
        void Delete(int? id);
        void Save();
    }
}
