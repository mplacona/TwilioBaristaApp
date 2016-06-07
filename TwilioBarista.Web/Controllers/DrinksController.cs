using System.Collections.Generic;
using System.Net;
using System.Web.Mvc;
using TwilioBarista.Web.DAL;
using TwilioBarista.Web.Models;
using TwilioBarista.Web.Repository;

namespace TwilioBarista.Web.Controllers
{
    public class DrinksController : Controller
    {
        private IDrinkRepository DrinkRepository { get; }

        public DrinksController(IDrinkRepository repository)
        { 
            DrinkRepository = repository;
        }

        // GET: Drinks
        public ActionResult Index()
        {
            var model = DrinkRepository.SelectAll();
            return View(model);
        }

        // GET: Drinks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var drink = DrinkRepository.SelectById(id);

            if (drink == null)
            {
                return HttpNotFound();
            }

            ViewBag.drink = drink;
            return View(drink);
        }

        // GET: Drinks/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Drinks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DrinkId,Name")] Drink drink)
        {
            if (ModelState.IsValid)
            {
                DrinkRepository.Insert(drink);
                DrinkRepository.Save();
                return RedirectToAction("Index");
            }

            return View(drink);
        }

        // GET: Drinks/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Drink drink = DrinkRepository.SelectById(id);
            if (drink == null)
            {
                return HttpNotFound();
            }
            return View(drink);
        }

        // POST: Drinks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DrinkId,Name")] Drink drink)
        {
            if (ModelState.IsValid)
            {
                DrinkRepository.Update(drink);
                DrinkRepository.Save();
                return RedirectToAction("Index");
            }
            return View(drink);
        }

        // GET: Drinks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var drink = DrinkRepository.SelectById(id);
            if (drink == null)
            {
                return HttpNotFound();
            }
            return View(drink);
        }

        // POST: Drinks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DrinkRepository.Delete(id);
            DrinkRepository.Save();
            return RedirectToAction("Index");
        }
    }
}
