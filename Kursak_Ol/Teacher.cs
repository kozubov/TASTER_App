using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Timer = System.Threading.Timer;


namespace Kursak_Ol
{
    public partial class Teacher : MyForm
    {

        User user = null;

        public Teacher( User user)
        {
            InitializeComponent();
            label1_Name.Text = $"{user.LastName} {user.FirstName} {user.MiddleName}";
            //наследуемый метод
            base.Top_Button(bunifuImageButton1_Min, bunifuImageButton1_Max, bunifuImageButton2_Norm);
            this.panel14_Opoves.Visible = false;
            this.label16_Log_Opov.Text = user.Login;
            //Кнопка для создания окна добавить тест
            this.bunifuImageButton_AddNewTest.Click += BunifuImageButton_AddNewTest_Click;
            //Кнопка для окна создания редактировать тест
            this.bunifuImageButton1_Edit_Test.Click += BunifuImageButton1_Edit_Test_Click;
            //Кнопка для создания вывода результатов
            this.bunifuImageButton3_Rezult.Click += BunifuImageButton3_Rezult_Click;
            //выводит на несколько секунд сообщение
            timer1.Tick += Timer1_Tick;
            timer1.Start();
            this.bunifuImageButton1_Close.Click += Button1_Close_Click;
            this.button1_Close.Click += Button1_Close_Click;

            this.user = user;
        }

        private void BunifuImageButton3_Rezult_Click(object sender, EventArgs e)
        {
            Result_For_Teacher result=new Result_For_Teacher(user);
            result.ShowDialog();
        }

        private void BunifuImageButton1_Edit_Test_Click(object sender, EventArgs e)
        {
            Select_Test_To_Edit selectTest=new Select_Test_To_Edit(this.user);
            selectTest.ShowDialog();
        }

        private void BunifuImageButton_AddNewTest_Click(object sender, EventArgs e)
        {
            Add_Test test = new Add_Test(user);
            test.ShowDialog();
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            //Вывожу а потом закрываю таймером потока
            timer1.Stop();
            bunifuTransition1.ShowSync(panel14_Opoves);
            TimerCallback stCallback=new TimerCallback(Panal_Visibl);
            Timer timer = new Timer(stCallback);
            timer.Change(2500, 3000);
        }

        private void Panal_Visibl(object state)
        {
            (state as Timer).Dispose();
            bunifuTransition1.HideSync(panel14_Opoves);
        }

        private void Button1_Close_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }
    }
}
