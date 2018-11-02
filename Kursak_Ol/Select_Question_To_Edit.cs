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
    public partial class Select_Question_To_Edit : MyForm
    {
        private int testId;
        private int currentQuestion = 0;
        private byte questionIsActual = 0;
        Select_Test_To_Edit parent;

        public Select_Question_To_Edit(int testId, Select_Test_To_Edit parent)
        {
            InitializeComponent();

            this.testId = testId;
            this.parent = parent;

            //наследуемый метод
            base.Top_Button(bunifuImageButton1_Min, bunifuImageButton1_Max, bunifuImageButton2_Norm);
            this.button_EditQuestion.Click += Button_EditQuestion_Click;
            this.bunifuImageButton1_Close.Click += button_CancelEditQuestion_Click;
            using (Tests_DBContainer tests = new Tests_DBContainer())
            {
                var row = tests.Test.FirstOrDefault(t => t.Id == testId);
                if (row != null)
                {
                    textBox_AddEditTestTitle.Text = row.Title;
                    renderQuestionList();
                    currentQuestion = Convert.ToInt32(listBox_SelectQuestionToEdit.SelectedValue);
                }
            }
        }

        public void renderQuestionList()
        {
            using (Tests_DBContainer tests = new Tests_DBContainer())
            {
                var ds = tests.TestQuestion.Where(t => t.TestId == testId).ToList();
                listBox_SelectQuestionToEdit.DataSource = ds;
                listBox_SelectQuestionToEdit.DisplayMember = "Question";
                listBox_SelectQuestionToEdit.ValueMember = "Id";

                if (listBox_SelectQuestionToEdit.Items.Count == 0)
                {
                    button_EditQuestion.Enabled = false;
                    button_TurnOn_OffQuestion.Enabled = false;
                    button_Delete_Question.Enabled = false;
                }
                else
                {
                    button_EditQuestion.Enabled = true;
                    button_TurnOn_OffQuestion.Enabled = true;
                    button_Delete_Question.Enabled = true;
                }
            }
        }

        private void Button_EditQuestion_Click(object sender, EventArgs e)
        {
            if (currentQuestion > 0)
            {
                Edit_Test test = new Edit_Test(testId, currentQuestion, this);
                test.ShowDialog();
            }
        }

        private void listBox_SelectQuestionToEdit_SelectedIndexChanged(object sender, EventArgs e)
        {
            int.TryParse(listBox_SelectQuestionToEdit.SelectedValue.ToString(), out currentQuestion);

            using (Tests_DBContainer tests = new Tests_DBContainer())
            {
                var row = tests.TestQuestion.FirstOrDefault(t => t.Id == currentQuestion);
                if (row != null)
                {
                    questionIsActual = row.IsActual;

                    if (row.IsActual == 1)
                    {
                        button_TurnOn_OffQuestion.Text = "Отключить";
                    }
                    else
                    {
                        button_TurnOn_OffQuestion.Text = "Включить";
                    }
                }
            }
        }

        private void button_TurnOn_OffQuestion_Click(object sender, EventArgs e)
        {
            using (Tests_DBContainer tests = new Tests_DBContainer())
            {
                var row = tests.TestQuestion.FirstOrDefault(t => t.Id == currentQuestion);
                if (row != null)
                {
                    byte reverse = questionIsActual != (byte)0 ? (byte)0 : (byte)1;
                    row.IsActual = reverse;
                    tests.SaveChanges();
                }
            }

            this.renderQuestionList();
        }

        private void button_CancelEditQuestion_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button_Delete_Question_Click(object sender, EventArgs e)
        {
            using (Tests_DBContainer tests = new Tests_DBContainer())
            {
                var row = tests.TestQuestion.FirstOrDefault(t => t.Id == currentQuestion);
                if (row != null)
                {
                    tests.TestQuestion.Remove(row);
                    tests.SaveChanges();
                }
            }

            this.renderQuestionList();
        }

        private void button_SaveChangeTitleQuestion_Click(object sender, EventArgs e)
        {
            if (textBox_AddEditTestTitle.Text == "")
            {
                MessageBox.Show("Название теста не может быть пустым", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (Tests_DBContainer tests = new Tests_DBContainer())
            {
                var row = tests.Test.FirstOrDefault(t => t.Id == testId);
                if (row != null)
                {
                    row.Title = textBox_AddEditTestTitle.Text;
                    tests.SaveChanges();
                    MessageBox.Show("Название было изменено", "Изменения", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                this.parent.renderTestList();
            }
        }

        private void button_AddQuestion_Click(object sender, EventArgs e)
        {
            Edit_Test test = new Edit_Test(testId, 0, this);
            test.ShowDialog();
        }
    }
}
