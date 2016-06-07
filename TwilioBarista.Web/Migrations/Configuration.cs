using System.Collections.Generic;
using TwilioBarista.Web.DAL;
using TwilioBarista.Web.Models;

namespace TwilioBarista.Web.Migrations
{
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<TwilioBaristaContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(TwilioBaristaContext context)
        {
            var drinks = new List<Drink>
            {
                new Drink { Name = "Espresso"},
                new Drink { Name = "Cappuccino"},
                new Drink { Name = "Latte"},
                new Drink { Name = "Americano"},
                new Drink { Name = "Mocha"}
            };

            drinks.ForEach(s => context.Drinks.AddOrUpdate(s));
            context.SaveChanges();

            var drinkTypes = new List<DrinkType>
            {
                // Espresso
                new DrinkType { Name = "expreso", DrinkId = 1},
                new DrinkType { Name = "expresso", DrinkId = 1},
                new DrinkType { Name = "espresso", DrinkId = 1},

                // Cappuccino
                new DrinkType { Name = "cappacino", DrinkId = 2},
                new DrinkType { Name = "capacino", DrinkId = 2},
                new DrinkType { Name = "cappacino", DrinkId = 2},
                new DrinkType { Name = "cappocino", DrinkId = 2},
                new DrinkType { Name = "capocino", DrinkId = 2},
                new DrinkType { Name = "capacino", DrinkId = 2},
                new DrinkType { Name = "cappucino", DrinkId = 2},
                new DrinkType { Name = "cappuccino", DrinkId = 2},
                new DrinkType { Name = "capuccino", DrinkId = 2},

                // Latte
                new DrinkType { Name = "late", DrinkId = 3},
                new DrinkType { Name = "lattey", DrinkId = 3},
                new DrinkType { Name = "larte", DrinkId = 3},
                new DrinkType { Name = "lattee", DrinkId = 3},
                new DrinkType { Name = "latte", DrinkId = 3},
                new DrinkType { Name = "cafe late", DrinkId = 3},
                new DrinkType { Name = "Caff� Latte", DrinkId = 3},
                new DrinkType { Name = "Caff� Latte", DrinkId = 3},
                new DrinkType { Name = "Caffe Latte", DrinkId = 3},

                // Americano
                new DrinkType { Name = "americano", DrinkId = 4},
                new DrinkType { Name = "white americano", DrinkId = 4},
                new DrinkType { Name = "Caff�", DrinkId = 4},

                // Mocha
                new DrinkType { Name = "mocha", DrinkId = 5},
                new DrinkType { Name = "mocca", DrinkId = 5},
                new DrinkType { Name = "moca", DrinkId = 5},
                new DrinkType { Name = "mocacino", DrinkId = 5},
                new DrinkType { Name = "mochaccino", DrinkId = 5}
            };

            drinkTypes.ForEach(s => context.DrinkTypes.AddOrUpdate(s));
            context.SaveChanges();

            var sources = new List<Source>
            {
                new Source { Name = "SMS", Address = "+441234567890"}
                new Source { Name = "Facebook", Address = "messenger:1234567890"}
            };

            sources.ForEach(s => context.Sources.AddOrUpdate(s));
            context.SaveChanges();
        }
    }
}
