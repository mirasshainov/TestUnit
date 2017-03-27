using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EssentialTools.Models;
using System.Linq;
using Moq;

namespace EssentialTools.Tests {
	[TestClass]
	public class UnitTest2 {
		private Products[] products = {
			new Products { Name="Kayak",Category="ada",Price=12M},
				new Products { Name="Kayak2",Category="ada2",Price=112M},
					new Products { Name="Kayak3",Category="ada3",Price=212M},
						new Products { Name="Kayak4",Category="ada4",Price=132M}
		};

		[TestMethod]
		public void Sum_Products_Correctly() {
			//	var discounter = new MinimumDiscountHelper();
			//var target = new LinqValueCalculator(discounter);
			//var goalTotal = products.Sum(e => e.Price);
			Mock<IDiscountHelper> mock = new Mock<IDiscountHelper>();
			mock.Setup(m => m.ApplyDiscount(It.IsAny<decimal>()))
				.Returns<decimal>(total => total);
			var target = new LinqValueCalculator(mock.Object);
			var result = target.ValueProducts(products);
			Assert.AreEqual(products.Sum(e => e.Price), result);
		}
		private Products[] createProduct(decimal value) {
			return new[] { new Products { Price = value } };
		}

		[TestMethod]
		[ExpectedException(typeof(System.ArgumentException))]
		public void Variable_Discounts() {
			Mock<IDiscountHelper> mock = new Mock<IDiscountHelper>();
			mock.Setup(m => m.ApplyDiscount(It.IsAny<decimal>()))
				.Returns<decimal>(total => total);
			mock.Setup(m => m.ApplyDiscount(It.Is<decimal>(v => v == 0)))
				.Throws<System.ArgumentException>();
			mock.Setup(m => m.ApplyDiscount(It.Is<decimal>(v => v > 100)))
				.Returns<decimal>(total => total * 0.9M);
			mock.Setup(m => m.ApplyDiscount(It.IsInRange<decimal>(10, 100, Range.Inclusive)))
				.Returns<decimal>(total => total - 5);
			var target = new LinqValueCalculator(mock.Object);
			//
			decimal FiveDollarDiscount = target.ValueProducts(createProduct(5));
			decimal TenDollarDiscount = target.ValueProducts(createProduct(10));
			decimal FiftyDollarDiscount = target.ValueProducts(createProduct(50));
			decimal HundredDollarDiscount = target.ValueProducts(createProduct(100));
			decimal FiveHundredDollarDiscount = target.ValueProducts(createProduct(500));
			//
			Assert.AreEqual(5, FiveDollarDiscount, "5 fail");
			Assert.AreEqual(5, TenDollarDiscount, "10 fail");
			Assert.AreEqual(95, HundredDollarDiscount, "100 fail");
			Assert.AreEqual(450, FiveHundredDollarDiscount, "500 fail");
			target.ValueProducts(createProduct(0));	
		}
	}
}
