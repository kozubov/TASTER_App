using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kursak_Ol
{
    public partial class Add_Test : MyForm
    {

        User user = null;

        public Add_Test(User user)
        {
            InitializeComponent();
            //наследуемый метод
            base.Top_Button(bunifuImageButton1_Min, bunifuImageButton1_Max, bunifuImageButton2_Norm);
            //Кнопка для вызова панели добавить категорию
            this.button_AddCategory.Click += Button_AddCategory_Click;
            //Кнопка для скрытия панели добавить категорию
            this.bunifuImageButton1_Close.Click += BunifuImageButton1_Close_Click;
            this.button2_Categori_Close.Click += Button2_Categori_Close_Click;

            this.renderCategoryList();

            this.user = user;
        }

        private void BunifuImageButton1_Close_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Button1_Add_Test_Click(object sender, EventArgs e)
        {
            this.bunifuTransition2.ShowSync(this.panel5_Add_Test);
        }

        private void renderCategoryList()
        {
            using (Tests_DBContainer tests = new Tests_DBContainer())
            {
                comboBox_SelectCategory.DataSource = tests.Category.ToList();
                comboBox_SelectCategory.ValueMember = "Id";
                comboBox_SelectCategory.DisplayMember = "Title";
            }
        }

        private void Button2_Categori_Close_Click(object sender, EventArgs e)
        {
            this.bunifuTransition2.HideSync(this.panel_AddNewCategory);
        }

        private void Button_AddCategory_Click(object sender, EventArgs e)
        {
            this.bunifuTransition2.ShowSync(this.panel_AddNewCategory);
        }

        private void button1_Categiri_Click(object sender, EventArgs e)
        {
            if(textBox_AddNewCategory.Text != "")
            {
                using (Tests_DBContainer tests = new Tests_DBContainer())
                {

                    Category category = new Category();
                    category.Title = textBox_AddNewCategory.Text;
                    tests.Category.Add(category);
                    tests.SaveChanges();

                    this.bunifuTransition2.HideSync(this.panel_AddNewCategory);

                    this.renderCategoryList();
                    comboBox_SelectCategory.SelectedIndex = comboBox_SelectCategory.Items.Count - 1;

                }
            }
        }

        

        private void button_FinishAddTest_Click(object sender, EventArgs e)
        {

            Close();
        }

        private void button_AddNewQuestion_Click(object sender, EventArgs e)
        {

            if (textBox_AddTestTitle.Text == "")
            {
                label_ErrorAddTest.Text = "Введите название";
            }
            else if (textBox_AddTestTitle.Text.Length > 255)
            {
                label_ErrorAddTest.Text = "Название не должно превышать 255 символов";
            }
            else
            {
                using (Tests_DBContainer tests = new Tests_DBContainer())
                {
                    Test test = new Test();
                    test.Title = textBox_AddTestTitle.Text;
                    test.IsActual = 0;
                    int idCat = Convert.ToInt32(comboBox_SelectCategory.SelectedValue.ToString());
                    test.Category = tests.Category.FirstOrDefault(cat => cat.Id == idCat);

                    tests.Test.Add(test);

                    TestCreator testcreator = new TestCreator();
                    testcreator.TestId = test.Id;
                    testcreator.UserId = user.Id;

                    tests.TestCreator.Add(testcreator);

                    tests.SaveChanges();

                    textBox_AddTestTitle.Text = "";

                    Add_Test2 add_test2 = new Add_Test2(test.Id);
                    Opacity = 0;
                    this.ShowInTaskbar = false;
                    if (add_test2.ShowDialog() == DialogResult.OK)
                    {
                        Close();
                    }
                    
                }
            }
        }
    }
}
