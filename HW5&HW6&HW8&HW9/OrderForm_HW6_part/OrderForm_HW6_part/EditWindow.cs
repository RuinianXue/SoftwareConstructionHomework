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
        /*
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
        }*/
        private readonly OrderService orderService;
        public bool EditModel { get; set; }
        public Order CurrentOrder { get; set; }
        public event Action<EditWindow> CloseEditFrom;
        public OrderDetails Detail { get; set; } // Add this line

        public EditWindow(Order order, bool model, OrderService orderService)
        {
            InitializeComponent();
            CloseEditFrom += (f => { });
            bindCustomer.Add(new Customer("1", "li"));
            bindCustomer.Add(new Customer("2", "zhang"));
            this.orderService = orderService;
            EditModel = model;
            CurrentOrder = order.Clone();
            bindOrder.DataSource = CurrentOrder;
            txtOrderId.Enabled = !model;
        }



        private void addItemButton_Click(object sender, EventArgs e)
        {
            var formItemEdit = new DetailEditWindow(new OrderDetails());
            try
            {
                if (formItemEdit.ShowDialog() == DialogResult.OK)
                {
                    var index = CurrentOrder.detail.Count != 0 ? CurrentOrder.detail.Max(i => i.Index) + 1 : 0;
                    formItemEdit.Detail.Index = index;
                    CurrentOrder.AddItem(formItemEdit.Detail);
                    bindDetail.ResetBindings(false);
                }
            }
            catch (Exception e2)
            {
                MessageBox.Show(e2.Message);
            }
        }


        private void saveButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (EditModel)
                {
                    orderService.UpdateOrder(CurrentOrder);
                }
                else
                {
                    orderService.AddOrder(CurrentOrder);
                }
                CloseEditFrom(this);
            }
            catch (Exception e3)
            {
                MessageBox.Show(e3.Message);
            }
        }

        private void editItemButton_Click(object sender, EventArgs e) => EditDetail();

        private void dgvItems_DoubleClick(object sender, EventArgs e) => EditDetail();

        private void EditDetail()
        {
            var detail = bindDetail.Current as OrderDetails;
            if (detail == null)
            {
                MessageBox.Show("请选择一个订单项进行修改");
                return;
            }
            var DetailEditWindow = new DetailEditWindow(detail);
            if (DetailEditWindow.ShowDialog() == DialogResult.OK)
            {
                bindDetail.ResetBindings(false);
            }
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            var detail = bindDetail.Current as OrderDetails;
            if (detail == null)
            {
                MessageBox.Show("请选择一个订单项进行删除");
                return;
            }
            CurrentOrder.RemoveDetail(detail);
            bindDetail.ResetBindings(false);
        }
    }
}
