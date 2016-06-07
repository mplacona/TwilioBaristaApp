using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TwilioBarista.Web.Models
{
    public class Drink
    {
        //private readonly TwilioBaristaContext _context;
        //public Drink(TwilioBaristaContext context)
        //{
        //    _context = context;
        //}

        [Key]
        public int DrinkId { get; set; }
        public string Name { get; set; }
        public virtual ICollection<DrinkType> DrinkTypes { get; set; }

    }
}