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
    public partial class Add_Test2 : MyForm
    {
        int testId;

        public Add_Test2(int id)
        {
            InitializeComponent();
            this.testId = id;
            base.Top_Button(bunifuImageButton1_Min, bunifuImageButton1_Max, bunifuImageButton2_Norm);
            textBox_AddQuestion.TextChanged += new EventHandler(text_changed);
            comboBox_SelectQuestion.SelectedIndexChanged += new EventHandler(comboBoxChanged);
            this.button_AddQuestionVariant.Click += button_AddQuestionVariant_Click;
            this.button_CancelQuestionVariant.Click += button_CancelQuestionVariant_Click;
            renderListQuestions();

            this.ActiveControl = textBox_AddQuestion; //Устанавливаем активные элемент - для создания вопроса

            btn_cancel.Click += Btn_cancel_Click;
        }

        /// <summary>
        /// Обработка нажатия кнопи"Отмена"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_cancel_Click(object sender, EventArgs e)
        {
            //Вывод сообщения для пользователя
            DialogResult res = MessageBox.Show("Вы действительно хотите отменить создание теста?", this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            
            //Если пользователь подвердил отмену создания теста
            if (res == DialogResult.Yes)
            {
                using (Tests_DBContainer tests = new Tests_DBContainer())
                {
                    using (var transaction = tests.Database.BeginTransaction())
                    {
                        try
                        {
                            Test test = tests.Test.FirstOrDefault(t => t.Id == this.testId);
                            if (test != null)
                            {
                                tests.Test.Remove(test);    //Удаляем тест
                                tests.SaveChanges();        //Сохраняем вопросы
                                transaction.Commit();       //подтверждаем транзакцию
                                this.DialogResult = DialogResult.OK; //закрываем окно
                            }
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback(); //отменяем все изменения в БД
                            MessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error); //выводим сообщение об ошибке
                        }
                        
                    }
                }
            }
        }

        private void comboBoxChanged(object sender, EventArgs e)
        {
            renderListAnswers();
        }

        private void renderListAnswers()
        {
            textBox_EnterdQuestions.Text = "";
            int id = Convert.ToInt32(comboBox_SelectQuestion.SelectedValue);
            using (Tests_DBContainer tests = new Tests_DBContainer()) {
                StringBuilder answer = new StringBuilder();
                foreach (TestQuestionAnswer tqa in tests.TestQuestionAnswer.Where(t => t.TestQuestionId == id))
                {
                    if (tqa.IsAnswer == 1)
                    {
                        answer.AppendLine("+    " + tqa.Answer);
                    }
                    else
                    {
                        answer.AppendLine("     " + tqa.Answer);
                    }
                   
                }
                textBox_EnterdQuestions.Text = answer.ToString();
            }
        }

        private void renderListQuestions(int ind = 0)
        {
            using (Tests_DBContainer tests = new Tests_DBContainer())
            {

                comboBox_SelectQuestion.DataSource = tests.TestQuestion.Where(t => t.TestId == this.testId).ToList();
                comboBox_SelectQuestion.ValueMember = "Id";
                comboBox_SelectQuestion.DisplayMember = "Question";

                if (comboBox_SelectQuestion.Items.Count > 0)
                {
                    comboBox_SelectQuestion.SelectedIndex = ind;
                    renderListAnswers();
                }
            }
        }

        private void text_changed(object sender, EventArgs e)
        {

            if (textBox_AddQuestion.Text != "" && textBox_AddQuestion.Text.Length <= 255)
            {
                checkedListBox_QuestionVariants.Visible = false;
                panelAddQuestion.Visible = true;
                checkedListBox_QuestionVariants.Visible = true;
            }
            else
            {
                panelAddQuestion.Visible = false;
            }
            
        }

        private void button_AddQuestionVariant_Click(object sender, EventArgs e)
        {
            if (textBox_TitleQuestion.Text != "" && textBox_TitleQuestion.Text.Length <= 255 && checkedListBox_QuestionVariants.Items.IndexOf(textBox_TitleQuestion.Text) == -1)
            {
                checkedListBox_QuestionVariants.Items.Add(textBox_TitleQuestion.Text);
                textBox_TitleQuestion.Text = "";
            }
        }

        private void button_CancelQuestionVariant_Click(object sender, EventArgs e)
        {
            textBox_TitleQuestion.Text = "";
        }

        private void button_AddNewQuestion_Click(object sender, EventArgs e)
        {

            bool error = false;

            using (Tests_DBContainer tests = new Tests_DBContainer())
            {
                if (textBox_AddQuestion.Text == "")
                {
                    MessageBox.Show("Введите вопрос");
                    error = true;
                }
                if (checkedListBox_QuestionVariants.Items.Count == 0)
                {
                    MessageBox.Show("Добавьте ответы");
                    error = true;
                }
                if (checkedListBox_QuestionVariants.SelectedItem == null || checkedListBox_QuestionVariants.CheckedItems.Count == 0)
                { 
                    MessageBox.Show("Выберите верный ответ");
                    error = true;
                }

                if (tests.TestQuestion.Where(t => t.TestId == this.testId && t.Question == textBox_AddQuestion.Text).ToList().Count > 0)
                {
                    MessageBox.Show("Есть такой вопрос");
                    error = true;
                }
                if (!error)
                {

                    TestQuestion testquestion = new TestQuestion();
                    testquestion.Question = textBox_AddQuestion.Text;

                    testquestion.IsActual = 0;
                    if (checkBox_QuestionTrue.Checked)
                    {
                        testquestion.IsActual = 1;
                    }

                    testquestion.TestId = this.testId;
                    tests.TestQuestion.Add(testquestion);

                    int i;
                    for (i = 0; i <= (checkedListBox_QuestionVariants.Items.Count - 1); i++)
                    {

                        TestQuestionAnswer answer = new TestQuestionAnswer();
                        answer.Answer = checkedListBox_QuestionVariants.Items[i].ToString();
                        if (checkedListBox_QuestionVariants.GetItemChecked(i))
                        {
                            answer.IsAnswer = Convert.ToByte(true);
                        }
                        answer.TestQuestion = testquestion;
                        tests.TestQuestionAnswer.Add(answer);

                    }

                    tests.SaveChanges();

                    checkedListBox_QuestionVariants.Items.Clear();
                    textBox_AddQuestion.Text = "";
                    checkBox_QuestionTrue.Checked = true;
                    renderListQuestions((comboBox_SelectQuestion.Items.Count));
                }
            }
        }

        private void button_FinishAddTest_Click(object sender, EventArgs e)
        {
            using (Tests_DBContainer tests = new Tests_DBContainer())
            {

                if(tests.TestQuestion.Where(t => t.TestId == this.testId).Count() == 0)
                {
                    MessageBox.Show("Добавьте вопрос");
                }
                else if (tests.TestQuestionAnswer.Where(t => t.TestQuestion.TestId == this.testId).Count() == 0)
                {
                    MessageBox.Show("Добавьте ответы");
                }
                else
                {
                    Test test = tests.Test.FirstOrDefault(t => t.Id == this.testId);
                    if(test != null)
                    {
                        test.IsActual = 1;
                        tests.SaveChanges();
                        this.DialogResult = DialogResult.OK;
                    }
                    
                }

            }
        }

        private void bunifuImageButton1_Close_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Сохраните тест");
        }
    }
}
