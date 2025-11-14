using System;
using System.Drawing;
using System.Windows.Forms;

namespace StudEnrollment_Abdulrahman_IT13
{
    public class DashboardForm : Form
    {
        private Panel cardStudents;
        private Panel cardEnrollments;
        private Panel cardSubjects;
        private Label lblStudentsCount;
        private Label lblEnrollmentsCount;
        private Label lblSubjectsCount;

        public DashboardForm()
        {
            InitializeComponent();
            LoadDashboardData();
        }

        private void InitializeComponent()
        {
            this.Size = new Size(1000, 600);
            this.StartPosition = FormStartPosition.Manual;
            this.BackColor = Color.FromArgb(245, 247, 250);
            this.Padding = new Padding(30);

            // Header
            Label lblHeader = new Label()
            {
                Text = "📊 Dashboard Overview",
                Dock = DockStyle.Top,
                Height = 60,
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.FromArgb(44, 62, 80),
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(10, 0, 0, 0)
            };

            // Container for cards with better spacing
            Panel cardContainer = new Panel()
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                Padding = new Padding(10, 20, 10, 10)
            };

            // Cards with improved design
            cardStudents = CreateCard("👩‍🎓  Total Students", out lblStudentsCount, Color.FromArgb(52, 152, 219), Color.FromArgb(41, 128, 185));
            cardEnrollments = CreateCard("📝  Active Enrollments", out lblEnrollmentsCount, Color.FromArgb(46, 204, 113), Color.FromArgb(39, 174, 96));
            cardSubjects = CreateCard("📚  Available Subjects", out lblSubjectsCount, Color.FromArgb(155, 89, 182), Color.FromArgb(142, 68, 173));

            // Position cards in a row
            cardStudents.Location = new Point(20, 10);
            cardEnrollments.Location = new Point(330, 10);
            cardSubjects.Location = new Point(640, 10);

            cardContainer.Controls.Add(cardStudents);
            cardContainer.Controls.Add(cardEnrollments);
            cardContainer.Controls.Add(cardSubjects);

            this.Controls.Add(cardContainer);
            this.Controls.Add(lblHeader);
        }

        private void LoadDashboardData()
        {
            try
            {
                // Load real counts from database
                lblStudentsCount.Text = DatabaseHelper.GetStudentCount().ToString();
                lblEnrollmentsCount.Text = DatabaseHelper.GetEnrollmentCount().ToString();
                lblSubjectsCount.Text = DatabaseHelper.GetSubjectCount().ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading dashboard data: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);

                // Set default values if database fails
                lblStudentsCount.Text = "0";
                lblEnrollmentsCount.Text = "0";
                lblSubjectsCount.Text = "0";
            }
        }

        private Panel CreateCard(string title, out Label lblCount, Color primaryColor, Color hoverColor)
        {
            Panel panel = new Panel()
            {
                Width = 280,
                Height = 170,
                BackColor = primaryColor,
                Margin = new Padding(15),
                Padding = new Padding(20),
                Cursor = Cursors.Hand
            };

            // Add shadow effect with custom paint
            panel.Paint += (s, e) =>
            {
                // Draw subtle shadow
                using (var shadowBrush = new SolidBrush(Color.FromArgb(30, 0, 0, 0)))
                {
                    e.Graphics.FillRectangle(shadowBrush, 5, 5, panel.Width, panel.Height);
                }

                // Draw main card
                using (var cardBrush = new SolidBrush(panel.BackColor))
                {
                    e.Graphics.FillRectangle(cardBrush, 0, 0, panel.Width - 5, panel.Height - 5);
                }

                // Draw border
                using (var borderPen = new Pen(Color.FromArgb(50, 255, 255, 255), 2))
                {
                    e.Graphics.DrawRectangle(borderPen, 0, 0, panel.Width - 6, panel.Height - 6);
                }
            };

            // Hover effect
            panel.MouseEnter += (s, e) => panel.BackColor = hoverColor;
            panel.MouseLeave += (s, e) => panel.BackColor = primaryColor;

            Label lblTitle = new Label()
            {
                Text = title,
                Dock = DockStyle.Top,
                ForeColor = Color.White,
                Font = new Font("Segoe UI Semibold", 11, FontStyle.Bold),
                Height = 35,
                TextAlign = ContentAlignment.MiddleLeft,
                BackColor = Color.Transparent
            };

            lblCount = new Label()
            {
                Text = "0",
                Dock = DockStyle.Fill,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 42, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.Transparent
            };

            Label lblSubtext = new Label()
            {
                Text = "View Details →",
                Dock = DockStyle.Bottom,
                ForeColor = Color.FromArgb(220, 255, 255, 255),
                Font = new Font("Segoe UI", 9),
                Height = 25,
                TextAlign = ContentAlignment.MiddleRight,
                BackColor = Color.Transparent
            };

            panel.Controls.Add(lblCount);
            panel.Controls.Add(lblTitle);
            panel.Controls.Add(lblSubtext);

            return panel;
        }
    }
}