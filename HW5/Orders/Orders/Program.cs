// See https://aka.ms/new-console-template for more information
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orders
{
    // 订单类
    public class Order : IComparable<Order>
    {
        public int Id { get; set; }
        public Customer Customer { get; set; }
        private readonly List<OrderDetails> Details = new List<OrderDetails>();
        //public List<OrderDetails> Details => details;
        public DateTime CreateTime { get; set; }

        public bool Exists(Predicate<OrderDetails> match)
        {
            return Details.Exists(match);
        }

        public double TotalAmount
        {
            get => Details.Sum(d => d.Amount);//如果写字段需要保持冗余数据一致性，容易出现错误
        }
        public Order() {
            CreateTime = DateTime.Now;
        }
        public Order(int id, Customer customer)
        {
            Id = id;
            Customer = customer;
            Details = new List<OrderDetails>();
            CreateTime = DateTime.Now;
        }

        public void AddDetails(OrderDetails detail)
        {
            if (Details.Contains(detail))
            {
                throw new ApplicationException($"The detail has been contained: {detail}");
            }
            Details.Add(detail);
        }

        public void RemoveDetails(OrderDetails detail)
        {
            Details.Remove(detail);
        }

        public override bool Equals(object obj)
        {
            // var obj as Order;
            //return obj != null && Id == obj.Id;
            return obj is Order order && obj != null&& Id == order.Id;
        }

        public override int GetHashCode() //改？？
        {
            return HashCode.Combine(Id);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"Order Id: {Id}, Customer: {Customer}\n");
            sb.Append("Order Details:\n");
            foreach (var detail in Details)
            {
                sb.Append(detail + "\n");
            }
            sb.Append($"Total Amount: {TotalAmount}\n");
            return sb.ToString();
        }

        public int CompareTo(Order other)
        {
            if (other == null) return 1;
            return Id-other.Id;
        }

        public static bool operator <(Order left, Order right)
        {
            return left.CompareTo(right) < 0;
        }

        public static bool operator <=(Order left, Order right)
        {
            return left.CompareTo(right) <= 0;
        }

        public static bool operator >(Order left, Order right)
        {
            return left.CompareTo(right) > 0;
        }

        public static bool operator >=(Order left, Order right)
        {
            return left.CompareTo(right) >= 0;
        }
    }

    // 订单明细类
    public class OrderDetails
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public double Amount
        {
            get
            {
                return Product.Price * Quantity;
            }
        }

        public OrderDetails(Product product, int quantity)
        {
            this.Product = product;
            this.Quantity = quantity;
        }

        public override bool Equals(object obj)//需要避免空指针
        {
            return obj is OrderDetails details &&
                   Product.Equals(details.Product);
        }

        public override int GetHashCode()//
        {
            return HashCode.Combine(Product);
        }

        public override string ToString()
        {
            return $"Product: {Product}, Quantity: {Quantity}, Amount: {Amount}";
        }
    }

    public class Customer
    {
        public string Name { get; set; }
        public string Address { get; set; }

        public Customer(string name, string address)
        {
            Name = name;
            Address = address;
        }

        public override bool Equals(object obj)
        {
            return obj is Customer customer &&
                   Name == customer.Name;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name);
        }

        public override string ToString()
        {
            return $"Name: {Name}, Address: {Address}";
        }
    }

    public class Product
    {
        public string Name { get; set; }
        public double Price { get; set; }

        public Product(string name, double price)
        {
            Name = name;
            Price = price;//需要校验
        }

        public override bool Equals(object obj)
        {
            return obj is Product product &&
                   Name == product.Name;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name);
        }

        public override string ToString()
        {
            return $"Name: {Name}, Price: {Price}";
        }
    }
   
    public class OrderService
    {
        private readonly List<Order> orders=new List<Order>();

        public OrderService()
        {
            orders = new List<Order>();
        }

        public void AddOrder(Order order)
        {
            if (orders.Contains(order))
            {
                throw new ApplicationException($"The order already exists: {order}");
            }
            orders.Add(order);
        }

        public void DeleteOrder(Order order)
        {
            int idx = orders.FindIndex(o => o.Id == order.Id);
            if (idx >= 0)
            {
                orders.RemoveAt(idx);
            }
            //foreach中间不能做添加删除和修改元素操作，可以修改字段，但是不能修改整体布局
        }

        public void UpdateOrder(Order order)
        {
            int idx=orders.FindIndex(o=>o.Id==order.Id);
            if (idx<0)
            {
                throw new ApplicationException($"The order does not exist: {order}");
            }
            //DeleteOrder(order);
            //AddOrder(order);
            orders.RemoveAt(idx);
            orders.Add(order);
        }

        public List<Order> QueryOrders(Func<Order, bool> condition)//
        {
            return orders.Where(condition).OrderBy(o => o.TotalAmount).ToList();//对查询结果排序
        }

        public void SortOrders(Comparison<Order> comparison)
        {
            orders.Sort(comparison);//对数据排序
        }

    }

    class Program
    {

        static void Main(string[] args)
        {
            Customer Linda = new Customer("Linda", "Chongqing");
            Customer Bob = new Customer("Bob", "Shanghai");
            Customer Song = new Customer("Song", "Guangzhou");

            Product book = new Product("Book", 20);
            Product pen = new Product("Pen", 5);
            Product p3 = new Product("Phone", 1000);

            Order o1 = new Order(1, Linda);
            o1.AddDetails(new OrderDetails(book, 2));
            o1.AddDetails(new OrderDetails(pen, 10));

            Order o2 = new Order(2, Bob);
            o2.AddDetails(new OrderDetails(pen, 20));
            o2.AddDetails(new OrderDetails(p3, 1));

            Order o3 = new Order(3, Song);
            o3.AddDetails(new OrderDetails(book, 5));
            o3.AddDetails(new OrderDetails(p3, 2));

            OrderService os = new OrderService();

            os.AddOrder(o1);
            os.AddOrder(o2);
            os.AddOrder(o3);

            Console.WriteLine("All orders:");
            foreach (var order in os.QueryOrders(o => true))
            {
                Console.WriteLine(order);
            }

            Console.WriteLine("Query by order id:");
            var result1 = os.QueryOrders(o => o.Id == 2);
            foreach (var order in result1)
            {
                Console.WriteLine(order);
            }

            Console.WriteLine("Query by product name:");
            var result2 = os.QueryOrders(o => o.Exists(d => d.Product.Name == "Book"));
            foreach (var order in result2)
            {
                Console.WriteLine(order);
            }

            Console.WriteLine("Query by customer:");
            var result3 = os.QueryOrders(o => o.Customer.Name == "Charlie");
            foreach (var order in result3)
            {
                Console.WriteLine(order);
            }

            Console.WriteLine("Query by total amount:");
            var result4 = os.QueryOrders(o => o.TotalAmount > 1000);
            foreach (var order in result4)
            {
                Console.WriteLine(order);
            }

            Console.WriteLine("Delete an order:");
            try
            {
                os.DeleteOrder(new Order(4, Linda));
                os.DeleteOrder(o3);
                foreach (var order in os.QueryOrders(o => true))
                {
                    Console.WriteLine(order);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.WriteLine("Update an order:");
            try
            {
                o2.Customer = new Customer("David", "Nanjing");
                os.UpdateOrder(o2);
                foreach (var order in os.QueryOrders(o => true))
                {
                    Console.WriteLine(order);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.WriteLine("Sort orders:");
            os.SortOrders((o1, o2) => o2.TotalAmount.CompareTo(o1.TotalAmount)); 
            foreach (var order in os.QueryOrders(o => true))
            {
                Console.WriteLine(order);
            }
        }
    }
}