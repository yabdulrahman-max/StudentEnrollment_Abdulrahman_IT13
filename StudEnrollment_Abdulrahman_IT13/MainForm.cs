using StudEnrollment_Abdulrahman_IT13;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace StudEnrollment_Abdulrahman_IT13
{
    public class MainForm : Form
    {
        private Panel sidebar;
        private Panel topBar;
        private Panel mainPanel;
        private Button btnDashboard;
        private Button btnStudent;
        private Button btnEnrollment;
        private Button btnSubjects;
        private Button btnLogout;
        private Label lblTitle;

        public MainForm()
        {
            InitializeComponent();
            LoadFormCentered(new DashboardForm(), "Dashboard");
        }

        private void InitializeComponent()
        {
            this.Text = "Student Enrollment System";
            this.WindowState = FormWindowState.Maximized;
            this.BackColor = Color.FromArgb(245, 247, 250);
            this.Font = new Font("Segoe UI", 10);

            // Sidebar
            sidebar = new Panel()
            {
                Dock = DockStyle.Left,
                Width = 230,
                BackColor = Color.FromArgb(40, 55, 71)
            };

            Label lblLogo = new Label()
            {
                Text = "📘 ENROLLMENT",
                Dock = DockStyle.Top,
                Height = 80,
                ForeColor = Color.White,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 12, FontStyle.Bold)
            };

            // Create buttons
            btnDashboard = CreateSidebarButton("📊  Dashboard");
            btnStudent = CreateSidebarButton("👩‍🎓  Student Management");
            btnEnrollment = CreateSidebarButton("📝  Enrollment");
            btnSubjects = CreateSidebarButton("📚  Subject List");
            btnLogout = CreateSidebarButton("🚪  Logout", isLogout: true);

            // Button click events
            btnDashboard.Click += (s, e) => LoadFormCentered(new DashboardForm(), "Dashboard");
            btnStudent.Click += (s, e) => LoadFormCentered(new StudentManagement(), "Student Management");
            btnEnrollment.Click += (s, e) => LoadFormCentered(new EnrollmentForm(), "Enrollment");
            btnSubjects.Click += (s, e) => LoadFormCentered(new SubjectList(), "Subject List");

            btnLogout.Click += (s, e) =>
            {
                new LoginForm().Show();
                this.Hide();
            };

            // Make logout button dock to bottom
            btnLogout.Dock = DockStyle.Bottom;

            // Add controls to sidebar in order (reverse order for top-docked items)
            sidebar.Controls.Add(btnLogout);  // This will be at bottom
            sidebar.Controls.Add(btnSubjects);
            sidebar.Controls.Add(btnEnrollment);
            sidebar.Controls.Add(btnStudent);
            sidebar.Controls.Add(btnDashboard);
            sidebar.Controls.Add(lblLogo);

            // Top Bar
            topBar = new Panel()
            {
                Dock = DockStyle.Top,
                Height = 60,
                BackColor = Color.White,
                Padding = new Padding(15, 10, 0, 0),
                BorderStyle = BorderStyle.FixedSingle
            };

            lblTitle = new Label()
            {
                Text = "Dashboard",
                Font = new Font("Segoe UI Semibold", 13, FontStyle.Bold),
                ForeColor = Color.FromArgb(52, 73, 94),
                Dock = DockStyle.Left,
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleLeft,
                Width = 400
            };

            topBar.Controls.Add(lblTitle);

            // Main Panel
            mainPanel = new Panel()
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Padding = new Padding(15)
            };

            this.Controls.Add(mainPanel);
            this.Controls.Add(topBar);
            this.Controls.Add(sidebar);
        }

        private Button CreateSidebarButton(string text, bool isLogout = false)
        {
            Button btn = new Button()
            {
                Text = text,
                Dock = DockStyle.Top,
                Height = 60,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(25, 0, 0, 0),
                ForeColor = Color.White,
                BackColor = isLogout ? Color.FromArgb(192, 57, 43) : Color.FromArgb(40, 55, 71)
            };
            btn.FlatAppearance.BorderSize = 0;
            btn.Cursor = Cursors.Hand;

            btn.MouseEnter += (s, e) =>
            {
                if (!isLogout)
                    btn.BackColor = Color.FromArgb(52, 73, 94);
                else
                    btn.BackColor = Color.FromArgb(231, 76, 60);
            };
            btn.MouseLeave += (s, e) =>
            {
                btn.BackColor = isLogout ? Color.FromArgb(192, 57, 43) : Color.FromArgb(40, 55, 71);
            };

            return btn;
        }

        // 🧭 Centered form loader
        private void LoadFormCentered(Form form, string title)
        {
            lblTitle.Text = title;
            mainPanel.Controls.Clear();

            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.AutoSize = true;

            // Temporarily dock none to allow centering
            form.Dock = DockStyle.None;
            mainPanel.Controls.Add(form);

            // Calculate position for centering
            int x = (mainPanel.Width - form.Width) / 2;
            int y = (mainPanel.Height - form.Height) / 2;
            form.Location = new Point(Math.Max(x, 0), Math.Max(y, 0));

            // Recenter when the mainPanel resizes
            mainPanel.Resize += (s, e) =>
            {
                int newX = (mainPanel.Width - form.Width) / 2;
                int newY = (mainPanel.Height - form.Height) / 2;
                form.Location = new Point(Math.Max(newX, 0), Math.Max(newY, 0));
            };

            form.Show();
        }
    }
}
