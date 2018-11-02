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
    public partial class Edit_Test : MyForm
    {
        private int testId;
        private int questionId;
        Select_Question_To_Edit parent;

        public Edit_Test(int testId, int questionId, Select_Question_To_Edit parent)
        {
            InitializeComponent();

            this.testId = testId;
            this.questionId = questionId;
            this.parent = parent;

            //наследуемый метод
            base.Top_Button(bunifuImageButton1_Min, bunifuImageButton1_Max, bunifuImageButton2_Norm);
            this.bunifuImageButton1_Close.Click += BunifuImageButton1_Close_Click;

            using (Tests_DBContainer tests = new Tests_DBContainer())
            {
                if (questionId == 0)
                {
                    var question = new TestQuestion() { Question = "Новый вопрос", TestId = this.testId, IsActual = 1, };
                    tests.TestQuestion.Add(question);
                    tests.SaveChanges();
                    this.questionId = question.Id;
                    this.parent.renderQuestionList();
                }

                var rowQuestion = tests.TestQuestion.FirstOrDefault(t => t.Id == this.questionId);
                if (rowQuestion != null)
                {
                    textBox_AddQuestion.Text = rowQuestion.Question;
                }

                renderAnswerList();
            }
        }

        private void BunifuImageButton1_Close_Click(object sender, EventArgs e)
        {
           this.Close();
        }

        private void renderAnswerList()
        {
            using (Tests_DBContainer tests = new Tests_DBContainer())
            {
                dataGridView1.Rows.Clear();
                dataGridView1.Refresh();

                var answersList = tests.TestQuestionAnswer.Where(t => t.TestQuestionId == questionId).ToList();
                foreach(var row in answersList)
                {
                    dataGridView1.Rows.Add(
                        new object[] {
                            row.Answer,
                            row.IsAnswer == 1 ? true : false,
                            row.Id
                        }
                    );
                }
            }
        }

        private void button_AddNewQuestion_Click(object sender, EventArgs e)
        {
            if (!this.saveAnswers(true))
                return;

            if (textBox_AddQuestion.Text == "")
            {
                MessageBox.Show("Вопрос не может быть пустым", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (Tests_DBContainer tests = new Tests_DBContainer())
            {
                var question = tests.TestQuestion.FirstOrDefault(t => t.Id == questionId);
                if (question != null)
                {
                    question.Question = textBox_AddQuestion.Text;
                    tests.SaveChanges();
                    this.parent.renderQuestionList();
                }
            }

            MessageBox.Show("Изменения сохранены", "Изменения", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button__Add_Click(object sender, EventArgs e)
        {
            if (!this.saveAnswers(false))
                return;

            using (Tests_DBContainer tests = new Tests_DBContainer())
            {
                var answer = new TestQuestionAnswer() { Answer = "", TestQuestionId = questionId, IsAnswer = 0 };
                tests.TestQuestionAnswer.Add(answer);
                tests.SaveChanges();
                renderAnswerList();
            }
        }

        private void button_FinishAddTest_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button_DeleteAnswer_Click(object sender, EventArgs e)
        {
            int rowindex = dataGridView1.CurrentRow.Index;
            var row = dataGridView1.Rows[rowindex];

            int aId;
            int.TryParse(row.Cells["Id"].Value.ToString(), out aId);

            using (Tests_DBContainer tests = new Tests_DBContainer())
            {
                var rowDb = tests.TestQuestionAnswer.FirstOrDefault(t => t.Id == aId);
                if (rowDb != null)
                {
                    tests.TestQuestionAnswer.Remove(rowDb);
                    tests.SaveChanges();
                    renderAnswerList();
                }
            }
        }

        private bool saveAnswers(bool checkAnswersCount = false)
        {
            int isAnswerCnt = 0;
            using (Tests_DBContainer tests = new Tests_DBContainer())
            {
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (row.Cells["Answer"].Value.ToString() == "")
                    {
                        MessageBox.Show("Ответ не может быть пустым", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }

                    if (Convert.ToByte(row.Cells["IsAnswer"].Value) == 1)
                        isAnswerCnt++;

                    if (isAnswerCnt > 1)
                    {
                        MessageBox.Show("Нельзя указывать больше одного правильного ответа", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }

                    int aId;
                    int.TryParse(row.Cells["Id"].Value.ToString(), out aId);

                    var answer = tests.TestQuestionAnswer.FirstOrDefault(t => t.Id == aId);

                    if (answer != null)
                    {
                        answer.Answer = row.Cells["Answer"].Value.ToString();
                        answer.IsAnswer = Convert.ToByte(row.Cells["IsAnswer"].Value);
                        tests.SaveChanges();
                    }
                }
            }

            if (checkAnswersCount && isAnswerCnt == 0)
            {
                MessageBox.Show("Укажите один правильный ответ", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }
    }
}
