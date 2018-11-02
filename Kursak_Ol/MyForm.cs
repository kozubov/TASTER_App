using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Bunifu.Framework.UI;

namespace Kursak_Ol
{
    public class MyForm : Form
    {
        private BunifuImageButton norm;
        private BunifuImageButton max;
        //Метод каторый наследуют все окна события на кнопки свернуть,развернуть,закрыть
        protected void Top_Button(BunifuImageButton min, BunifuImageButton max,
            BunifuImageButton norm, BunifuImageButton close = null)
        {
            this.max = max;
            this.norm = norm;
            if (close != null)
            {
                close.Click += BunifuImageButton1_Close_Click;
            }
            this.max.Click += BunifuImageButton1_Max_Click;
            this.norm.Click += BunifuImageButton2_Norm_Click;
            min.Click += BunifuImageButton1_Min_Click;
        }
        private void BunifuImageButton1_Min_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void BunifuImageButton2_Norm_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
            norm.Visible = false;
            max.Visible = true;
        }

        private void BunifuImageButton1_Max_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            this.max.Visible = false;
            this.norm.Visible = true;
        }

        private void BunifuImageButton1_Close_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MyForm));
            this.SuspendLayout();
            // 
            // MyForm
            // 
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MyForm";
            this.ResumeLayout(false);

        }
    }
}
