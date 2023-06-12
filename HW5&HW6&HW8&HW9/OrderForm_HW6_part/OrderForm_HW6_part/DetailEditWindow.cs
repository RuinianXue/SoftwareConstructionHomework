using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Orders;

namespace OrderForm_HW6_part
{
    public partial class DetailEditWindow : Form
    {
        public DetailEditWindow()
        {
            InitializeComponent();
        }
        public OrderDetails Detail { get; set; }

        public DetailEditWindow(OrderDetails newDetail) : this()
        {
            this.Detail = newDetail;
            this.bindDetail.DataSource = newDetail;
            bindProduct.Add(new Product("Pears", 10.0, 1));
            bindProduct.Add(new Product("Banana", 20.0, 2));
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }


    }
}
