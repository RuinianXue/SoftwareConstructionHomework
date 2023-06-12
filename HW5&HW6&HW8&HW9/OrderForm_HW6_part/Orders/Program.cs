using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace Orders
{
    // 订单类
    public class Order : IComparable<Order>
    {
        public int Id { get; set; }
        public Customer Customer { get; set; }
        public DateTime CreateTime { get; set; }

        public List<OrderDetails> detail;

        public List<OrderDetails> Details
        {
            get { return detail; }
        }
        public Order(int id, Customer customer, List<OrderDetails> items)
        {
            this.Id = id;
            this.Customer = customer;
            this.CreateTime = DateTime.Now;
            this.detail = (items == null) ? new List<OrderDetails>() : items;
        }

        public void RemoveDetail(OrderDetails detail)
        {
            this.detail.Remove(detail);
        }
        public bool Exists(Predicate<OrderDetails> match)
        {
            return detail.Exists(match);
        }
        public void AddItem(OrderDetails item)
        {
            if (detail.Contains(item))
            {
                throw new InvalidOperationException("Item already exists in order.");
            }
            detail.Add(item);
        }
        public Order Clone()
        {
            return new Order
            {
                // copy all properties
                Id = this.Id,
                Customer = this.Customer,
                detail = this.detail.Select(d => d.Clone()).ToList()
            };
        }

        public double TotalAmount
        {
            get => detail.Sum(d => d.Amount);//如果写字段需要保持冗余数据一致性，容易出现错误
        }
        public Order()
        {
            CreateTime = DateTime.Now;
        }
        public Order(int id, Customer customer)
        {
            Id = id;
            Customer = customer;
            detail = new List<OrderDetails>();
            CreateTime = DateTime.Now;
        }

        public void AddDetails(OrderDetails detail)
        {
            if (this.detail.Contains(detail))
            {
                throw new ApplicationException($"The detail has been contained: {detail}");
            }
            this.detail.Add(detail);
        }


        public override bool Equals(object obj)
        {
            // var obj as Order;
            //return obj != null && Id == obj.Id;
            return obj is Order order && obj != null && Id == order.Id;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"Order Id: {Id}, Customer: {Customer}\n");
            sb.Append("Order detail:\n");
            foreach (var detail in detail)
            {
                sb.Append(detail + "\n");
            }
            sb.Append($"Total Amount: {TotalAmount}\n");
            return sb.ToString();
        }

        public int CompareTo(Order other)
        {
            if (other == null) return 1;
            return Id - other.Id;
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
        public int Index { get; set; }
        public OrderDetails() { }
        public string Name { get; set; }
        public double Amount
        {
            get
            {
                return Product.Price * Quantity;
            }
            set { Product.Price = value * Quantity;}
        }


        public override bool Equals(object obj)
        {
            var item = obj as OrderDetails;
            return item != null &&
                   Name == item.Name;
        }

        public OrderDetails Clone()
        {
            return new OrderDetails(this.Product, this.Quantity,this.Index)
            {
                // copy all properties
                Index = this.Index,
                Product = this.Product,
                Quantity = this.Quantity,
                Amount = this.Amount
            };
        }
        public OrderDetails(Product product, int quantity,int Index)
        {
            this.Product = product;
            this.Quantity = quantity;
            this.Index = Index;
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


        public override string ToString()
        {
            return $"Name: {Name}, Address: {Address}";
        }
    }

    public class Product
    {
        public string Name { get; set; }
        public double Price { get; set; }
        public int Index { get; set; }
        public Product(string name, double price, int index)
        {
            Name = name;
            Price = price;//需要校验
            Index = index;
        }

        public override bool Equals(object obj)
        {
            return obj is Product product &&
                   Name == product.Name;
        }

        public override string ToString()
        {
            return $"Name: {Name}, Price: {Price}";
        }
    }

    public class OrderService
    {
        private readonly List<Order> orders = new List<Order>();

        public OrderService()
        {
            orders = new List<Order>();
        }
        public List<Order> GetOrders()
        {
            return orders;
        }
        public Order FindOrder(int id)
        {
            return orders.Where(o => o.Id == id).FirstOrDefault();
        }

        public void AddOrder(Order order)
        {
            if (orders.Contains(order))
            {
                throw new ApplicationException($"The order already exists: {order}");
            }
            orders.Add(order);
        }


        public void DeleteOrder(int id)
        {
            int idx = orders.FindIndex(o => o.Id == id);
            if (idx >= 0)
            {
                orders.RemoveAt(idx);
            }
            //foreach中间不能做添加删除和修改元素操作，可以修改字段，但是不能修改整体布局
        }

        public void UpdateOrder(Order order)
        {
            int idx = orders.FindIndex(o => o.Id == order.Id);
            if (idx < 0)
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

}