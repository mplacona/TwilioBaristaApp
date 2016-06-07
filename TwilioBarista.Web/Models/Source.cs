using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TwilioBarista.Web.Models
{
    public class Source
    {
        [Key]
        public int SourceId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
    }
}