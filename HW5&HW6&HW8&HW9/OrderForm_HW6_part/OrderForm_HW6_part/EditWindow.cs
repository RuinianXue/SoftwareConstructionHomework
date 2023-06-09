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
    public partial class EditWindow : Form
    {
        public EditWindow()
        {
            InitializeComponent();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void dgvItems_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void deleteButtonClick(object sender, EventArgs e)
        {
            OrderDetails detail = bindDetail.Current as OrderDetails;
            if (detail == null)
            {
                MessageBox.Show("请选择一个订单项进行删除");
                return;
            }
   //         CurrentOrder.RemoveDetail(detail);
            bindDetail.ResetBindings(false);
        }
    }
}
