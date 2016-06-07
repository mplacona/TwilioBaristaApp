using System;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using PusherServer;
using Twilio;
using Twilio.TwiML.Mvc;
using TwilioBarista.Core.Interfaces;
using TwilioBarista.Web.DAL;
using TwilioBarista.Web.Filters;
using TwilioBarista.Web.Models;
using TwilioBarista.Web.Repository;
using TwilioBarista.Web.Repository.RealTime;
using HttpStatusCodeResult = System.Web.Mvc.HttpStatusCodeResult;

namespace TwilioBarista.Web.Controllers
{
    public class OrdersController : TwilioController
    {

        private static readonly IRealTime<Pusher> RealTime = new PusherImpl();
        private readonly Pusher _rthub = RealTime.GetClient(ConfigurationManager.AppSettings["PusherAppId"], ConfigurationManager.AppSettings["PusherKey"], ConfigurationManager.AppSettings["PusherSecret"]);

        private IOrderRepository OrderRepository { get; }
        private IDrinkTypesRepository DrinkTypeRepository { get; }
        private IDrinkRepository DrinkRepository { get; }
        private readonly TwilioRestClient _twilioClient;

        public OrdersController(IOrderRepository orderRepository, IDrinkTypesRepository drinkTypeRepository, IDrinkRepository drinkRepository)
        {
            OrderRepository = orderRepository;
            DrinkTypeRepository = drinkTypeRepository;
            DrinkRepository = drinkRepository;
            _twilioClient = new TwilioRestClient(ConfigurationManager.AppSettings["TwilioSid"], ConfigurationManager.AppSettings["TwilioToken"]);
        }


        // GET: Orders
        [BasicAuthentication]
        public ActionResult Index()
        {
            // Get all the orders that haven't been fulfilled ordered by time
            return View(
                            OrderRepository
                            .SelectAll()
                            .Where(f => f.Fulfilled == false)
                            .OrderBy(t => t.Time).ToList());
        }

        // GET: Orders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var order = OrderRepository.SelectById(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // GET: Orders/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Orders/Create
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Create(Customer message)
        {
            using (var db = new TwilioBaristaContext())
            {
                // Check whether message came from SMS or Messenger
                Source source;
                var address = "";
                if (message.From.Contains("Messenger"))
                {
                    source = (from b in db.Sources where b.Name.Equals("Facebook") select b).First();
                    address = message.To;

                }
                else
                {
                    source = (from b in db.Sources where b.Address.Equals(message.To) select b).First();
                    address = message.To;
                }

                // Check to see if this customer has already placed an order
                db.Set<Customer>().AddOrUpdate(f => f.From, new Customer { From = message.From, To = message.To} );
                db.SaveChanges();

                // get a reference to the customer which will definitely exist at this point
                var customer = db.Customers.First(c => c.From == message.From);

                //Check if this customer has an open order
                var currentOrder = customer.Orders?.Where(f => f.Fulfilled == false).Select(n => new { n.Name, n.OrderId }).ToList();
                if (currentOrder?.Count > 0)
                {
                    _twilioClient.SendMessage(address, message.From, $"We're still making you a {currentOrder[0].Name}. Check order #{currentOrder[0].OrderId} with the barista if you think there's something wrong.");
                    return Content("Order already exists");
                }

                // Check if the order is valid against the mispell table
                var drink =
                    DrinkTypeRepository.SelectAll()
                        .AsQueryable()
                        .Include(m => m.Drink)
                        .Where(m => m.Name == message.Body);
                var drinkMatch = drink.Select(p => p.Drink.Name).FirstOrDefault();
                string response;
                if (drink.Any())
                {
                    // Create order
                    var order = new Order
                    {
                        Customer = customer,
                        Name = drinkMatch,
                        Source = source
                    };

                    db.Orders.Add(order);
                    db.SaveChanges();

                    // Add to pusher
                    _rthub.Trigger("orders", "order",
                        new {id = order.OrderId, product = drinkMatch, message = message.Body});

                    // Dashboard
                    var unixTimestamp = (int)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                    _rthub.Trigger("orders", "dashboard",
                        new { source = order.Source.Name, type = drinkMatch, time = unixTimestamp });

                    response =
                        $"Thanks for ordering a {drinkMatch} from the Twilio powered Coffee Shop. We'll text you back when it's ready. In the mean time check out this repo https://github.com/mplacona/TwilioBarista if you want too see how we built this app.";

                    _twilioClient.SendMessage(address, message.From, response);
                                        

                    return Content("New order added");
                }

                // We don't have that drink
                response =
                    $"Seems like your order of {message.Body} is not something we can serve. Possible orders are {string.Join(", ", DrinkRepository.SelectAll().Select(m => m.Name).Distinct())}";

                _twilioClient.SendMessage(address, message.From, response);

                return Content(response);
            }
        }

        // GET: Orders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var order = OrderRepository.SelectById(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Orders/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Time,Name,fulfilled")] Order order)
        {
            if (!ModelState.IsValid) return View(order);
            OrderRepository.Update(order);
            OrderRepository.Save();
            return RedirectToAction("Index");
        }

        // GET: Orders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var order = OrderRepository.SelectById(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            OrderRepository.Delete(id);
            OrderRepository.Save();
            return RedirectToAction("Index");
        }

        // POST: Orders/Complete/5
        public string Complete(int id, string status)
        {

            var order = OrderRepository.SelectById(id);
            
            // update order
            OrderRepository.Complete(id);
            OrderRepository.Save();

            // Tell pusher this order is now completd
            _rthub.Trigger("orders", "remove", new { id = id });


            string message;
            if (status == "accept")
            {
                message =
                    string.Format(
                        $"Your {order.Name} is ready. You can collect it from the coffee shop right away, ask for order number {order.OrderId}.");
            }
            else
            {
                message =
                string.Format(
                    $"Your {order.Name} order has been cancelled. Please check with the barista if you think something is wrong");
            }
            

            _twilioClient.SendMessage(order.Customer.To, order.Customer.From, message);


            return "Order updated";
        }
    }
}
