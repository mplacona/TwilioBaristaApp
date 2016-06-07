using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TwilioBarista.Web.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? Time { get; set; }
        public string Name { get; set; }
        public bool Fulfilled { get; set; }

        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }

        public int SourceId { get; set; }
        public virtual Source Source { get; set; }
    }
}