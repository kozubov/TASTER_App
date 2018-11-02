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

    public partial class Pupil_Test : Form
    {
        public class Answer
        {
            public int Id { get; set; } = 0;
            public int IdAnswer { get; set; } = 0;
            public int IsAnswer { get; set; } = 0; 
            public DateTime date;
        }
        //временная строка вопроса
        int tempAnswer;
        //временная строка правильного ответа
        int tempIsAnswer; 
        //временный id вопроса
        int tempidQues;
        //временная дата
        DateTime tempDate;
        //объект UserTest для заполнения и добавления в таблицу UserTest
        UserTest userTest;
        //флаг на запонения массива ответов
        bool flag = true;
        //флаг проверки закрытия формы
        bool flagClose = true;
        //кол-во вопросов в тесте
        int countQuestion = 0;
        //результат прохождения
        int res = 0;
        //массив ответов
        List<Answer> ListAnswer = new List<Answer>();
        public Pupil_Test(int idUser, string nameTest)
        {
            InitializeComponent();
            //наследуемый метод
            bunifuImageButton1_Min.Click += BunifuImageButton1_Min_Click;
            bunifuImageButton1_Close.Click += BunifuImageButton1_Close_Click;
            FormClosing += Pupil_Test_FormClosing;
            //изменяем название теста
            WindowState = FormWindowState.Maximized;
            label1_Name.Text = nameTest;
            using (Tests_DBContainer db = new Tests_DBContainer())
            {
                //определения пользователя
                var User = db.User.FirstOrDefault(u => u.Id == idUser);
                //выборка вопросов из базы  
                var test = db.Test.Join(
                    db.TestQuestion,
                    t => t.Id,
                    tq => tq.TestId,
                    (t, tq) => new
                    {
                        id = tq.Id,
                        idTest = t.Id,
                        testTitle = t.Title,
                        testQuestion = tq.Question
                    }).Where(t => t.testTitle == nameTest).ToList();
                //заполняем объект userTest начальными данными
                userTest = new UserTest
                {
                    UserId = idUser,
                    StartDate = DateTime.Now,
                };
                countQuestion = test.Count;
                foreach (var item in test)
                {
                    //добавления вопросов в listBox
                    listBox_SelectQuestion.Items.Add(item.testQuestion);
                    //добавления id вопросов в массив ответов
                    Answer newAnswer = new Answer();
                    newAnswer.Id = item.id;
                    ListAnswer.Add(newAnswer);
                    userTest.TestId = item.idTest;
                }               
            }
            listBox_SelectQuestion.SelectedIndexChanged += ListBox_SelectQuestion_SelectedIndexChanged;
            button_CancelTest.Click += Button_CancelTest_Click;
            button1_EndTest.Click += Button1_EndTest_Click;
        }

        private void BunifuImageButton1_Close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BunifuImageButton1_Min_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void Pupil_Test_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (flagClose)
            {
                RecordSelectedAnswer();
                Result(false);
            }
        }

        private void Button1_EndTest_Click(object sender, EventArgs e)
        {
            RecordSelectedAnswer();
            flagClose = false;
            Result(true);
            //Сообщение о прохождении теста
            MessageBox.Show($"Тест завершен,\nправельных ответов {res} из {countQuestion}", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.DialogResult = DialogResult.OK;
        }

        private void Button_CancelTest_Click(object sender, EventArgs e)
        {
            RecordSelectedAnswer();
            flagClose = false;
            Result(false);
            Close();
        }

        private void ListBox_SelectQuestion_SelectedIndexChanged(object sender, EventArgs e)
        {
            //проверка на выбор отета RadioButton и переход к следующему вопросу
            //запись выбранного ответа
            RecordSelectedAnswer();
            //очистка panel_ShowAnswer для добавления новых RadioButton
            panel_ShowAnswer.Controls.Clear();
            //проверка на выбранный элемент списка
            if (listBox_SelectQuestion.SelectedIndex != -1)
            {
                //названия выбранного элемента списка
                var text = listBox_SelectQuestion.SelectedItem.ToString();
                using (Tests_DBContainer db = new Tests_DBContainer())
                {
                    //получение из базы ответов на выбранный вопрос
                    var answer = db.TestQuestion.Join(
                        db.TestQuestionAnswer,
                        tq => tq.Id,
                        tqa => tqa.TestQuestionId,
                        (tq, tqa) => new
                        {
                            id = tq.Id,
                            idAns = tqa.Id,
                            textQuestion = tq.Question,
                            tqa.Answer
                        }).Where(t=>t.textQuestion == text).ToList();
                    //переменная для определения отступа RadioButton по Y
                    int num = 0;
                    for (int i = 0; i < answer.Count; i++)
                    {
                        //запись во временную переменую id вопроса
                        tempidQues = answer[i].id;
                        //создание RadioButton
                        RadioButton RB = new RadioButton
                        {
                            Margin = new Padding(10, 5, 5, 5),
                            Text = answer[i].Answer.ToString(),
                            Location = new Point(10, num+=30),
                            AutoSize = true,
                            Tag = i.ToString()
                        };
                        //проверка на уже отмеченный RadioButton 
                        if (ListAnswer.FirstOrDefault(ee => ee.Id == tempidQues).IdAnswer == answer[i].idAns)
                        {
                            RB.Checked = true;
                        }
                        //добавляем к каждому RadioButton событие на изменения
                        RB.CheckedChanged += RB_CheckedChanged;
                        //добавляем RadioButton в panel_ShowAnswer
                        panel_ShowAnswer.Controls.Add(RB);
                    }
                }
            }
            else
            {
                //если ни чего не выбранно то отчистить panel_ShowAnswer от RadioButton
                panel_ShowAnswer.Controls.Clear();
            }
        }

        private void RB_CheckedChanged(object sender, EventArgs e)
        {
            //изменения в RadioButton
            RadioButton r = sender as RadioButton;
            //запись значения RadioButton во временную переменую
            using (Tests_DBContainer db = new Tests_DBContainer())
            {
                var ansv = db.TestQuestionAnswer.FirstOrDefault(a=>a.Answer == r.Text);
                tempAnswer = ansv.Id;
                tempIsAnswer = ansv.IsAnswer;
            }
            tempDate = DateTime.Now;
            //изменяем значение флага
            flag = false;
        }
        //заполнение таблица UserTest на основе отвеченных вопросов
        void Result(bool flag1)
        {
            if (flag1)
            {
                foreach (var item in ListAnswer)
                {
                    if (item.IsAnswer == 1)
                    {
                        res += 1;
                    }
                }
            }
            //результат прохождения
            userTest.Result = $"{res}/{countQuestion}";
            userTest.EndDate = DateTime.Now;
            using (Tests_DBContainer db = new Tests_DBContainer())
            {
                //в переменную получаем данных о сохраненых даных в таблицу UserTest 
                var userTestNeedId = db.UserTest.Add(userTest);
                //сохраняем изменения
                db.SaveChanges();
                //получаем ID  сохраненной записи и передеме её в метод для заполнения таблицы UserTestAnswer
                AddUserTestAnswer(userTestNeedId.Id);
            }
        }
        //заполнение таблица UserTestAnswer на основе отвеченных вопросов
        void AddUserTestAnswer(int id)
        {
            using (Tests_DBContainer db = new Tests_DBContainer())
            {
                foreach (var item in ListAnswer)
                {
                    //проверка не остался ли вопрос без ответа, если вопрос без ответа == 0 то пропускаем его
                    if (item.IdAnswer != 0)
                    {
                        UserTestAnswer uta = new UserTestAnswer
                        {
                            UserTestId = id,
                            TestQuestionId = item.Id,
                            UserTestQuestionAnswerId = item.IdAnswer,
                            AnswerDate = item.date
                        };
                        db.UserTestAnswer.Add(uta);
                    }
                }
                //сохраняем изменения
                db.SaveChanges();
            }
        }
        //запись выбранного ответа
        void RecordSelectedAnswer()
        {
            //проверка на выбор ответа RadioButton
            if (!flag)
            {
                var er = ListAnswer.FirstOrDefault(ee => ee.Id == tempidQues);
                er.IdAnswer = tempAnswer;
                er.date = tempDate;
                er.IsAnswer = tempIsAnswer;
                flag = true;
            }
        }
    }
}
