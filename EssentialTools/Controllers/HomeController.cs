using EssentialTools.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ninject;

namespace EssentialTools.Controllers
{
	public class HomeController : Controller {
		private IValueCalculator calc;
		private Products[] products = {
		new Products {Name = "Каяк", Category = "Водные виды спорта", Price = 275M},
			new Products {Name = "Спасательный жилет", Category = "Водные виды спорта", Price = 48.95M},
			new Products {Name = "Мяч", Category = "Футбол", Price = 19.50M},
			new Products {Name = "Угловой флажок", Category = "Футбол", Price = 34.95M}
		};

		public HomeController(IValueCalculator calcParam,IValueCalculator calc2) {
			calc = calcParam;
		}

		// GET: Home
		public ActionResult Index()
        {
			ShoppingCart cart = new ShoppingCart(calc) {
			Products=products
			};
			decimal totalValue = cart.CalculateProductTotal();
            return View(totalValue);
        }
    }
}