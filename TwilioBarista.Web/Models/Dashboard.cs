using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace TwilioBarista.Web.Models
{
    [NotMapped]
    public class TotalOrder
    {
        public string name { get; set; }
        public int count { get; set; }
    }

    [NotMapped]
    public class BySource
    {
        public int facebook { get; set; }
        public int sms { get; set; }
    }

    [NotMapped]
    public class ByTime
    {
        public IList<int> facebook { get; set; }
        public List<int> sms { get; set; }
    }

    [NotMapped]
    public class Data
    {
        public IList<TotalOrder> totalOrders { get; set; }
        public BySource bySource { get; set; }
        public ByTime byTime { get; set; }
    }

    [NotMapped]
    public class Dashboard
    {
        public Data data { get; set; }
    }

    [NotMapped]
    public class PaddedList
    {
        public int count { get; set; }
        public int slot { get; set; }

        public PaddedList() { }

        public PaddedList(int _count, int _slot)
        {
            count = _count;
            slot = _slot;
        }
    }
}