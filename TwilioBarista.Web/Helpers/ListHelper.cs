using System;
using System.Collections.Generic;
using System.Linq;
using TwilioBarista.Web.Models;

namespace TwilioBarista.Web.Helpers
{
    public class ListHelper
    {
        public static List<T> GetPaddedList<T>(T list, int count) where T : new()
        {
            var listItems = new List<T>();
            for (var i = 1; i <= count; i++)
            {
                //listItems.Add(new T() { count = 0, slot = i });
                listItems.Add((T)Activator.CreateInstance(typeof(T), 0, i));
            }
            
            return listItems;
        }

        public static IList<PaddedList> MergeLists(IList<PaddedList> list1, IList<PaddedList> list2)
        {
            var mergedList = list1.Concat(list2)
                .ToLookup(p => p.slot)
                .Select(g => g.Aggregate((p1, p2) => new PaddedList
                {
                    slot = p1.slot,
                    count = p1.count
                })).ToList().OrderBy(o => o.slot);

            return mergedList.ToList();
        }
    }
}