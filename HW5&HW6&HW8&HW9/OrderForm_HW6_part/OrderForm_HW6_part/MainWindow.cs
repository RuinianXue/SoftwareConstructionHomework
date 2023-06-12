using Orders;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OrderForm_HW6_part
{
    public partial class MainWindow : Form
    {
        OrderService orderService = new OrderService();

        public MainWindow()
        {
            InitializeComponent();
            InitOrders();
            bindingOrder.DataSource = orderService.GetOrders();
            comboBox.SelectedIndex = 0;
            textBox1.DataBindings.Add("Text", this, "Keyword");
            ShowEditForm += (f => { });
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }


        public event Action<EditWindow> ShowEditForm;
        public String Keyword { get; set; }

        private void InitOrders()
        {
            Order order = new Order(1, new Customer("1", "li"), new List<OrderDetails> {
                new OrderDetails(new Product("apple", 100.0, 1),1, 10),
                new OrderDetails(new Product("egg", 50.0, 2), 1, 61)
            });
            orderService.AddOrder(order);
            Order order2 = new Order(2, new Customer("2", "zhang"), new List<OrderDetails> {
                new OrderDetails(new Product("egg", 100.0, 2), 1, 10)
            });
            orderService.AddOrder(order2);
        }

        public void QueryAll()
        {
            bindingOrder.DataSource = orderService.GetOrders();
            bindingOrder.ResetBindings(false);
        }


        private void queryButton_Click(object sender, EventArgs e)
        {
            try
            {
                switch (comboBox.SelectedIndex)
                {
                    case 0:
                        bindingOrder.DataSource = orderService.GetOrders();
                        break;
                    case 1:
                        int id = Convert.ToInt32(Keyword);
                        Order order = orderService.FindOrder(id);
                        List<Order> result = new List<Order>();
                        if (order != null) result.Add(order);
                        bindingOrder.DataSource = result;
                        break;
                    case 2:
                        bindingOrder.DataSource = orderService.QueryOrders(o => o.Customer.Name == Keyword);
                        break;
                    case 3:
                        bindingOrder.DataSource = orderService.QueryOrders(o => o.detail.Exists(item => item.Name == Keyword));
                        break;
                    case 4:
                        float totalPrice = Convert.ToInt32(Keyword);
                        bindingOrder.DataSource = orderService.QueryOrders(o => o.TotalAmount == totalPrice);
                        break;
                }
                bindingOrder.ResetBindings(false);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void addButton_Click(object sender, EventArgs e)
        {
            EditWindow formEdit = new EditWindow(new Order(), false, orderService);
            ShowEditForm(formEdit);
        }

        private void modifyButton_Click(object sender, EventArgs e)
        {
            EditOrder();
        }

        private void dbvOrders_DoubleClick(object sender, EventArgs e)
        {
            EditOrder();
        }

        private void EditOrder()
        {
            Order order = bindingOrder.Current as Order;
            if (order == null)
            {
                MessageBox.Show("Choose One Order to Modify");
                return;
            }
            EditWindow form2 = new EditWindow(order, true, orderService);
            ShowEditForm(form2);
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            Order order = bindingOrder.Current as Order;
            if (order == null)
            {
                MessageBox.Show("Choose One Order to Delete");
                return;
            }
            DialogResult result = MessageBox.Show($"Sure to delete order id {order.Id}?", "Delete", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                orderService.DeleteOrder(order.Id);
                QueryAll();
            }
        }

    }
}
