using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TwilioBarista.Web.Models
{
    public class Customer
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CustomerId { get; set; }

        public string From { get; set; }

        public string To { get; set; }

        // We don't wanna store this here
        [NotMapped]
        public string Body { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}