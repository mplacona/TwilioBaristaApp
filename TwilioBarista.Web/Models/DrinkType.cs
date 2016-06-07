using System.ComponentModel.DataAnnotations.Schema;

namespace TwilioBarista.Web.Models
{
    public class DrinkType
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DrinkTypeId { get; set; }
        public string Name { get; set; }

        public int DrinkId { get; set; }
        public virtual Drink Drink { get; set; }
    }
}