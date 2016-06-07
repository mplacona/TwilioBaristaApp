using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using TwilioBarista.Web.DAL;
using TwilioBarista.Web.Helpers;
using TwilioBarista.Web.Models;
using TwilioBarista.Web.Repository;

namespace TwilioBarista.Web.Controllers
{
    public class DashboardController : ApiController
    {
        private IOrderRepository OrderRepository { get; }

        public DashboardController(IOrderRepository orderRepository)
        {
            OrderRepository = orderRepository;
        }

        // GET: api/Dashboard
        public Dashboard GetOrders()
        {
            var orders = OrderRepository.SelectAll().AsQueryable().Include(i => i.Name).ToList();


            // Total Orders
            var totalOrders = orders
              .GroupBy(l => l.Name)
              .Select(g => new TotalOrder()
              {
                  name = g.Key,
                  count = orders.Count(t => t.Name == g.Key)
              }).ToList();

            // By Source
            var facebookSource = orders.Count(t => t.Source.Name == "Facebook");
            var smsSource = orders.Count(t => t.Source.Name == "SMS");

            // By Time
            var startDateTime = DateTime.Today; //Today at 00:00:00
            var endDateTime = DateTime.Today.AddDays(1).AddTicks(-1); //Today at 23:59:59

            var todaysOrders = orders.Where(m => m.Time >= startDateTime && m.Time <= endDateTime);

            var byTime = todaysOrders
                .GroupBy(x => x.Time.Value.ToString("%H"));
            
            var aFacebookByTime = byTime.Select(item => new PaddedList { count = item.Count(t => t.Source.Name == "Facebook"), slot = int.Parse(item.Key) }).ToList();
            var aSmsByTime = byTime.Select(item => new PaddedList { count = item.Count(t => t.Source.Name == "SMS"), slot = int.Parse(item.Key)} ).ToList();

            // 24 hours in a day right?
            var padded = ListHelper.GetPaddedList(new PaddedList(), 24);

            var mergedSmsList = ListHelper.MergeLists(aSmsByTime, padded);

            var mergedFacebookList = ListHelper.MergeLists(aFacebookByTime, padded);


            var data = new Data
            {
                totalOrders = totalOrders,
                bySource = new BySource
                {
                    facebook = facebookSource,
                    sms = smsSource
                },
                byTime = new ByTime
                {
                    //facebook = new List<int> {4, 9, 33},
                    facebook = mergedFacebookList.Select(m => m.count).ToList(),
                    //sms = new List<int> {3, 10, 22}
                    sms = mergedSmsList.Select(m => m.count).ToList()
                }
            };

            var dashboard = new Dashboard {data = data};

            // By Source

            // By Time

            //var orders = OrderRepository.SelectAll().AsQueryable().Include(i => i.Name).ToList();

            //var totalOrders = orders
            //  .GroupBy(l => l.Name)
            //  .Select(g => new TotalOrder()
            //  {
            //      Name = g.Key,
            //      Count = orders.Count(t => t.Name == g.Key)
            //  }).ToList();


            //var totalSources = orders
            //    .GroupBy(l => l.Source.Name)
            //    .Select(g => new BySource()
            //    {
            //        Source = g.Key,
            //        Count = orders.Count(t => t.Source.Name == g.Key)
            //    }).ToList();

            //var byTime = new List<ByTime>
            //{
            //    new ByTime
            //    {
            //        Name = "Facebook"
            //    },
            //    new ByTime
            //    {
            //        Name = "SMS"
            //    }
            //};
            //var dashboard = new Dashboard(totalOrders, totalSources, byTime);

            return dashboard;

        }

        //// GET: api/Dashboard/5
        //[ResponseType(typeof(Order))]
        //public async Task<IHttpActionResult> GetOrder(int id)
        //{
        //    Order order = await db.Orders.FindAsync(id);
        //    if (order == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(order);
        //}

        //// PUT: api/Dashboard/5
        //[ResponseType(typeof(void))]
        //public async Task<IHttpActionResult> PutOrder(int id, Order order)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != order.OrderId)
        //    {
        //        return BadRequest();
        //    }

        //    db.Entry(order).State = EntityState.Modified;

        //    try
        //    {
        //        await db.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!OrderExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return StatusCode(HttpStatusCode.NoContent);
        //}

        //// POST: api/Dashboard
        //[ResponseType(typeof(Order))]
        //public async Task<IHttpActionResult> PostOrder(Order order)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    db.Orders.Add(order);
        //    await db.SaveChangesAsync();

        //    return CreatedAtRoute("DefaultApi", new { id = order.OrderId }, order);
        //}

        //// DELETE: api/Dashboard/5
        //[ResponseType(typeof(Order))]
        //public async Task<IHttpActionResult> DeleteOrder(int id)
        //{
        //    Order order = await db.Orders.FindAsync(id);
        //    if (order == null)
        //    {
        //        return NotFound();
        //    }

        //    db.Orders.Remove(order);
        //    await db.SaveChangesAsync();

        //    return Ok(order);
        //}

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}

        //private bool OrderExists(int id)
        //{
        //    return db.Orders.Count(e => e.OrderId == id) > 0;
        //}
    }
}