using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ConsoleApp7;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using ConsoleApp7;
namespace ConsoleApp7.Tests
{
    [TestClass]
    public class TestOrder1
    {
        OrderService orderService = new OrderService();
        Goods g1 = new Goods("hhhh", 5);
        Goods g2 = new Goods("hhhh2", 3);
        Goods g3 = new Goods("hhhh3", 10);
        Goods g4 = new Goods("hhhh4", 7);
        Client c1 = new Client("xrn", "0000");
        Client c2 = new Client("Linda", "1232");
        Client c3 = new Client("Samantha", "3813");
        [TestInitialize]
        public void Init()
        {
            Order o1 = new Order(c1, 1);
            o1.addDetials(new OrderDetails(g1, 2));
            o1.addDetials(new OrderDetails(g2, 1));

            Order o2 = new Order(c1, 2);
            o1.addDetials(new OrderDetails(g1, 1));
            o1.addDetials(new OrderDetails(g2, 2));

            Order o3 = new Order(c2, 3);
            o1.addDetials(new OrderDetails(g1, 4));
            o1.addDetials(new OrderDetails(g3, 2));

            orderService.add(o1);
            orderService.add(o2);
            orderService.add(o3);
        }
        [TestMethod]
        public void addOrderTest1()
        {
            Order o4 = new Order(c2, 4);
            o4.addDetials(new OrderDetails(g2, 2));
            orderService.add(o4);
            List<Order> orders = orderService.QueryAll();
            Assert.IsNotNull(orders);
            Assert.AreEqual(4, orders.Count);
            Assert.IsTrue(orders.Contains(o4));
        }
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void AddOrderTest2()
        {
            Order o4 = new Order(c1,4);
            orderService.add(o4);
        }
        [TestMethod]
        public void RemoveOrderTest()
        {

            orderService.dele(3);
            List<Order> orders = orderService.QueryAll();
            Assert.AreEqual(2, orders.Count);
            orderService.dele(100);
            Assert.AreEqual(2, orderService.QueryAll().Count);
        }
        [TestMethod]
        public void UpdateOrderTest()
        {
            Order order3 = new Order(c1,3);
            order3.addDetials(new OrderDetails(g1, 200));
            orderService.revice(order3.ID,order3);

            List<Order> orders = orderService.QueryAll();
            Assert.IsNotNull(orders);
            Assert.AreEqual(3, orders.Count);
            Order o = orderService.findByID(3);
            Assert.AreEqual(c1, o.client);
        }

        [TestMethod]
        public void QueryOrderByIdTest()
        {
            Order order2 = orderService.findByID(2);
            Assert.IsNotNull(order2);
            Assert.AreEqual(2, order2.ID);
            Assert.AreEqual(c1, order2.client);
            List<OrderDetails> details = new List<OrderDetails>()
        { new OrderDetails(g1, 1), new OrderDetails(g2,2) };
            CollectionAssert.AreEqual(details, order2.orderdetails);

            Order order4 = orderService.findByID(4);
            Assert.IsNull(order4);
        }
        public void QueryOrdersByGoodsNameTest()
        {
            Assert.AreEqual(2, orderService.findByGname("g1").Count);
            Assert.AreEqual(2, orderService.findByGname("g2").Count);
            Assert.AreEqual(3, orderService.findByGname("g3").Count);
            Assert.AreEqual(0, orderService.findByGname("g4").Count);
        }

        [TestMethod]
        public void QueryOrderByClientNameTest()
        {
            Order o5 = new Order(c1, 6);
            o5.addDetials(new OrderDetails(g2, 5));
            orderService.add(o5);
            Assert.AreEqual(2, orderService.findByCname("xrn").Count);
            Assert.AreEqual(1, orderService.findByCname("Linda").Count);
            Assert.AreEqual(0, orderService.findByCname("Samantha").Count);
        }
    }
}
