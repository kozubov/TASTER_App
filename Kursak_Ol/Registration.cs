using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Timer = System.Threading.Timer;

namespace Kursak_Ol
{
    public partial class Registration : MyForm
    {
        private int iSlaider;
        public Registration()
        {
            InitializeComponent();
            //bunifu это теже кнопки 

            this.bunifuImageButton1_Close.Click += BunifuImageButton1_Close_Click;
            //наследуемый метод
            base.Top_Button(bunifuImageButton1_Min, bunifuImageButton1_Max, bunifuImageButton2_Norm);
            this.Load += Registration_Load;
            this.button2_Vhod_Form.Click += Button2_Vhod_Form_Click;
            this.button2_Registretion_Form.Click += Button2_Registretion_Form_Click;
            this.button1_Registretion.Click += Button2_Registretion_Form_Click;
            //Работа логотипа
            TimerCallback startCallback = new TimerCallback(Show_Slider);
            Timer timer = new Timer(startCallback);
            timer.Change(4000, 4000);

        }

        private void Registration_Load(object sender, EventArgs e)//загружаю все события 
        {
            //проверки всех боксов на пробелы
            this.textBox1_Adres.TextChanged += new EventHandler(textBox);
            this.textBox1_LastName.TextChanged += new EventHandler(textBox);
            this.textBox1_Login.TextChanged += new EventHandler(textBox);
            this.textBox1_Login_Registr.TextChanged += new EventHandler(textBox);
            this.textBox1_Middle_name.TextChanged += new EventHandler(textBox);
            this.textBox1_Name.TextChanged += new EventHandler(textBox);
            this.textBox1_Password.TextChanged += new EventHandler(textBox);
            this.textBox1_Password_Registr.TextChanged += new EventHandler(textBox);
            this.textBox1_Phone.TextChanged += new EventHandler(textBox);
            this.button1_Registration_DB.Click += Button1_Registration_DB_Click;
            //labl который будет выводить ошибки на форме для пользователя
            label14_Null.Visible = false;
            label6_Error.Visible = false;
            panel12_Opovesh.Visible = false;
            label15_Vhod_Null.Visible = false;
            timer1.Tick += Timer1_Tick1;
            this.button1_Vkhod.Click += Button1_Vkhod_Click;
        }

        private void Button1_Vkhod_Click(object sender, EventArgs e)
        {
            if (textBox1_Login.Text == "" || textBox1_Password.Text == "")
            {
                label15_Vhod_Null.Visible = true;
                return;
            }

            try //Отлавливание ошибок при подключении БД
            {
                using (Tests_DBContainer tests = new Tests_DBContainer())
                {
                    string pass = textBox1_Password.Text.GetHashCode().ToString(); //получение хеш кода введенного пароля 

                    User user = tests.User.FirstOrDefault(z =>
                        z.Login == textBox1_Login.Text && z.Password == pass);
                    if (user == null)
                    {
                        this.label6_Error.Visible = true;
                        return;
                    }

                    if (user.Role.Title == "Студент")
                    {
                        Pupil pupil = new Pupil(user);
                        textBox1_Login.Text = null;
                        textBox1_Password.Text = null;
                        this.ShowInTaskbar = false;
                        Opacity = 0;
                        if (pupil.ShowDialog() == DialogResult.OK)
                        {
                            Opacity = 1;
                            this.ShowInTaskbar = true;
                        }
                        
                        
                    }
                    else if (user.Role.Title == "Преподаватель")
                    {
                        Teacher teacher = new Teacher(user);
                        textBox1_Login.Text = null;
                        textBox1_Password.Text = null;
                        this.ShowInTaskbar = false;
                        Opacity = 0;
                        if (teacher.ShowDialog() == DialogResult.OK)
                        {
                            Opacity = 1;
                            this.ShowInTaskbar = true;
                        }
                    }

                    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Возникла не предвиденная ошибка с подключением к базе даных!\n Проверте подключение!{ex.Message}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Timer1_Tick1(object sender, EventArgs e)
        {
            bunifuTransition3.HideSync(this.panel12_Opovesh);
            timer1.Stop();
        }

        private void Button1_Registration_DB_Click(object sender, EventArgs e)
        {
            if (textBox1_Adres.Text == "" || textBox1_LastName.Text == "" || textBox1_Phone.Text == "" ||
                textBox1_Login_Registr.Text == "" ||
                textBox1_Middle_name.Text == "" || textBox1_Password_Registr.Text == "" || textBox1_Name.Text == "" || textBox1_Povtor_password.Text == "")
            {
                label14_Null.Text = "Заполните все поля!";
                label14_Null.Visible = true;
                return;
            }

            string rLog = @"(?m)^.[a-zA-Z\d\s-_]{2,30}(?=\r?$)";
            if (!Regex.IsMatch(textBox1_Login_Registr.Text, rLog))//проверка по регулярному выражению Логина
            {
                label14_Null.Text = "Логин - только латинские символы!";
                label14_Null.Visible = true;
                return;
            }

            //Проверка регулярным выражением имени
            string rFIO = @"^[а-яА-ЯіІїЇ-]{2,30}$";
            if (!Regex.IsMatch(textBox1_Name.Text, rFIO))
            {
                label14_Null.Text = "Имя - недопустисые символы!";
                label14_Null.Visible = true;
                return;
            }

            //Проверка регулярным выражением фамилии
            if (!Regex.IsMatch(textBox1_LastName.Text, rFIO))
            {
                label14_Null.Text = "Фамилия - недопустисые символы!";
                label14_Null.Visible = true;
                return;
            }

            //Проверка регулярным выражением отчества
            if (!Regex.IsMatch(textBox1_Middle_name.Text, rFIO))
            {
                label14_Null.Text = "Отчество - недопустисые символы!";
                label14_Null.Visible = true;
                return;
            }

            string phoneNumber = @"^380\d{9}$";
            if (!Regex.IsMatch(textBox1_Phone.Text, phoneNumber, RegexOptions.IgnoreCase))//проверка на правильность написания телефона
            {
                label14_Null.Text = "Телефон не соответствует формату 380000000000";
                label14_Null.Visible = true;
                return;
            }

            string rPas = @"(?=.*\d).{6,}";
            if (!Regex.IsMatch(textBox1_Password_Registr.Text, rPas, RegexOptions.IgnoreCase)) //проверка на пароль
            {
                label14_Null.Text = "Пароль не менише 6 символов и одна цифра!";
                label14_Null.Visible = true;
                return;
            }

            string rAddress = @"^[а-яА-Я\d\s\/-]{2,}$";
            if (!Regex.IsMatch(textBox1_Adres.Text, rAddress, RegexOptions.IgnoreCase))//проверка адреса
            {
                label14_Null.Text = "Не верный формат адреса!";
                label14_Null.Visible = true;
                return;
            }

            //Проверка введенных паролей
            if (textBox1_Password_Registr.Text != textBox1_Povtor_password.Text)
            {
                label14_Null.Text = "Введеные пароли не совпадают!";
                label14_Null.Visible = true;
                textBox1_Povtor_password.Text = null;
                return;
            }

            try //Отлавливание ошибок при подключении БД
            {
                using (Tests_DBContainer tests = new Tests_DBContainer())
                {
                    string login = textBox1_Login_Registr.Text;
                    var log = tests.User.FirstOrDefault(z => z.Login == login);
                    if (log != null)//проверка на логин есть или нет его в БД
                    {
                        label14_Null.Text = "Такой логин уже занят другим пользователем!";
                        label14_Null.Visible = true;
                        return;
                    }

                    string ph = textBox1_Phone.Text;
                    var phone = tests.User.FirstOrDefault(z => z.Phone == ph);
                    if (phone != null)//проверка на телефона есть или нет его в БД
                    {
                        label14_Null.Text = "Такой телефон уже занят другим пользователем!";
                        label14_Null.Visible = true;
                        return;
                    }

                    var id = tests.Role.FirstOrDefault(z => z.Title == "Студент");//роль
                    if (id == null)//проверка роли есть она в БД 
                    {
                        return;
                    }
                    //Создаем пользователя
                    User rUser = new User
                    {
                        Address = textBox1_Adres.Text,
                        FirstName = textBox1_Name.Text,
                        Login = textBox1_Login_Registr.Text,
                        LastName = textBox1_LastName.Text,
                        MiddleName = textBox1_Middle_name.Text,
                        Password = textBox1_Password_Registr.Text.GetHashCode().ToString(), //хеширование пароля
                        Phone = textBox1_Phone.Text,
                        RoleId = id.Id
                    };
                    tests.User.Add(rUser);
                    tests.SaveChanges();
                }
                //все происходят манипуляции
                this.label16_Log_Opov.Text = textBox1_Login_Registr.Text;
                bunifuTransition3.ShowSync(this.panel12_Opovesh);
                timer1.Start();
                this.textBox1_Adres.Text = null;
                this.textBox1_LastName.Text = null;
                this.textBox1_Login_Registr.Text = null;
                this.textBox1_Middle_name.Text = null;
                this.textBox1_Password_Registr.Text = null;
                this.textBox1_Name.Text = null;
                this.textBox1_Phone.Text = null;
                this.textBox1_Povtor_password.Text = null;
            }
            catch (Exception)
            {
                MessageBox.Show("Возникла не предвиденная ошибка с подключением к базе даных!\n Проверте подключение!",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void textBox(object sender, EventArgs a)
        {
            label14_Null.Visible = false;
            label6_Error.Visible = false;
            label15_Vhod_Null.Visible = false;
            TextBox text = sender as TextBox;
            if (text.Name == textBox1_Adres.Name)
            {
                if (String.IsNullOrWhiteSpace(textBox1_Adres.Text))
                {
                    textBox1_Adres.Text = null;
                }
            }
            else if (text.Name == textBox1_LastName.Name)
            {
                if (String.IsNullOrWhiteSpace(textBox1_LastName.Text))
                {
                    textBox1_LastName.Text = null;
                }
            }
            else if (text.Name == textBox1_Login.Name)
            {
                if (String.IsNullOrWhiteSpace(textBox1_Login.Text))
                {
                    textBox1_Login.Text = null;
                }
            }
            else if (text.Name == textBox1_Login_Registr.Name)
            {
                if (String.IsNullOrWhiteSpace(textBox1_Login_Registr.Text))
                {
                    textBox1_Login_Registr.Text = null;
                }
            }
            else if (text.Name == textBox1_Middle_name.Name)
            {
                if (String.IsNullOrWhiteSpace(textBox1_Middle_name.Text))
                {
                    textBox1_Middle_name.Text = null;
                }
            }
            else if (text.Name == textBox1_Name.Name)
            {
                if (String.IsNullOrWhiteSpace(textBox1_Name.Text))
                {
                    textBox1_Name.Text = null;
                }
            }
            else if (text.Name == textBox1_Password.Name)
            {
                if (String.IsNullOrWhiteSpace(textBox1_Password.Text))
                {
                    textBox1_Password.Text = null;
                }
            }
            else if (text.Name == textBox1_Password_Registr.Name)
            {
                if (String.IsNullOrWhiteSpace(textBox1_Password_Registr.Text))
                {
                    textBox1_Password_Registr.Text = null;
                }
            }
            else if (text.Name == textBox1_Phone.Name)
            {
                if (String.IsNullOrWhiteSpace(textBox1_Phone.Text))
                {
                    textBox1_Phone.Text = null;
                }
            }
            else if(text.Name==textBox1_Povtor_password.Name)
            {
                if (String.IsNullOrWhiteSpace(textBox1_Povtor_password.Text))
                {
                    textBox1_Povtor_password.Text = null;
                }
            }
        }

        private void Show_Slider(object state)
        {
            Close_Slaider();
            Show_Slaider();
        }

        private void Show_Slaider()
        {
            Bitmap My_image = new Bitmap(String.Format(@"..\..\images\Slider\{0}.png", iSlaider));
            this.pictureBox3_Slider.Image = My_image;
            this.bunifuTransition1.Show(this.pictureBox3_Slider);

        }

        private void Button2_Registretion_Form_Click(object sender, EventArgs e)
        {
            textBox1_Login.Text = null;
            textBox1_Password.Text = null;
            label6_Error.Visible = false;
            label15_Vhod_Null.Visible = false;
            this.panel7_Registr.Visible = true;
        }

        private void Button2_Vhod_Form_Click(object sender, EventArgs e)
        {
            this.panel7_Registr.Visible = false;
            this.textBox1_Adres.Text = null;
            this.textBox1_LastName.Text = null;
            this.textBox1_Login_Registr.Text = null;
            this.textBox1_Middle_name.Text = null;
            this.textBox1_Password_Registr.Text = null;
            this.textBox1_Name.Text = null;
            this.textBox1_Phone.Text = null;
            this.textBox1_Povtor_password.Text = null;
            label14_Null.Visible = false;
        }

        private void Close_Slaider()
        {
            this.bunifuTransition2.HideSync(this.pictureBox3_Slider);
            iSlaider++;
            if (iSlaider == 3)
            {
                iSlaider = 0;
            }

        }

        private void BunifuImageButton1_Close_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
