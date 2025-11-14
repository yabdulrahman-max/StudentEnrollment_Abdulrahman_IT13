using StudEnrollment_Abdulrahman_IT13;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace StudEnrollment_Abdulrahman_IT13
{
    public class LoginForm : Form
    {
        private Label lblTitle;
        private Label lblUser;
        private Label lblPass;
        private TextBox txtUser;
        private TextBox txtPass;
        private Button btnLogin;
        private Panel card;

        public LoginForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Admin Login";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.ClientSize = new Size(420, 350);
            this.BackColor = Color.FromArgb(236, 240, 241);

            lblTitle = new Label()
            {
                Text = "🧑‍💼 ADMIN LOGIN",
                Font = new Font("Segoe UI Semibold", 14, FontStyle.Bold),
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Top,
                Height = 80,
                ForeColor = Color.FromArgb(44, 62, 80)
            };

            card = new Panel()
            {
                Size = new Size(320, 200),
                Location = new Point(50, 100),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            lblUser = new Label() { Text = "Username:", Location = new Point(30, 30), AutoSize = true };
            lblPass = new Label() { Text = "Password:", Location = new Point(30, 80), AutoSize = true };

            txtUser = new TextBox() { Location = new Point(120, 25), Width = 150 };
            txtPass = new TextBox() { Location = new Point(120, 75), Width = 150, PasswordChar = '*' };

            btnLogin = new Button()
            {
                Text = "Login",
                Location = new Point(120, 130),
                Width = 150,
                BackColor = Color.FromArgb(41, 128, 185),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnLogin.Click += BtnLogin_Click;

            card.Controls.Add(lblUser);
            card.Controls.Add(lblPass);
            card.Controls.Add(txtUser);
            card.Controls.Add(txtPass);
            card.Controls.Add(btnLogin);

            this.Controls.Add(lblTitle);
            this.Controls.Add(card);
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            if (txtUser.Text == "admin" && txtPass.Text == "12345")
            {
                MainForm main = new MainForm();
                main.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Invalid username or password!", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
