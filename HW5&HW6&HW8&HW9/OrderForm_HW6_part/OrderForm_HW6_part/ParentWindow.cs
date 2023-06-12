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
    public partial class ParentWindow : Form
    {
        public ParentWindow()
        {
            InitializeComponent();
        }
        MainWindow mainWindow = new MainWindow();

        private void ParentWindow_Load(object sender, EventArgs e)
        {
            mainWindow.ShowEditForm += this.ShowEditForm;
            ShowMainForm();
        }


        private void ShowMainForm()
        {
            this.linkLabel1.Enabled = false;
            this.label1.Visible = false;
            showFormInPanel(mainWindow);
            mainWindow.QueryAll();
        }

        private void ShowEditForm(EditWindow editWindow)
        {
            this.linkLabel1.Enabled = true;
            this.label1.Visible = true;
            this.label1.Text = editWindow.EditModel ? "修改订单" : "添加订单";
            editWindow.CloseEditFrom += (form => ShowMainForm());
            showFormInPanel(editWindow);
        }

        private void showFormInPanel(Form from)
        {
            this.panel1.SuspendLayout();
            this.panel1.Controls.Clear();
            from.TopLevel = false;
            from.FormBorderStyle = FormBorderStyle.None;
            from.Dock = DockStyle.Fill;
            from.Visible = true;
            this.panel1.Controls.Add(from);
            this.panel1.ResumeLayout();
        }


        private void orderMainLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ShowMainForm();
        }

        private void contentPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
