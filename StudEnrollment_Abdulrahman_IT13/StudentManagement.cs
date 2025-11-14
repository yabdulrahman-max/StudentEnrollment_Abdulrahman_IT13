using StudEnrollment_Abdulrahman_IT13;
using System;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace StudEnrollment_Abdulrahman_IT13
{
    public class StudentManagement : Form
    {
        private DataGridView dgvStudents;
        private TextBox txtSearch;
        private Button btnAdd, btnEdit, btnDelete, btnRefresh;
        private Label lblHeader, lblTotalStudents;
        private Panel searchPanel, buttonPanel, statsPanel;

        public StudentManagement()
        {
            InitializeComponent();
            LoadStudents();
        }

        private void InitializeComponent()
        {
            this.Size = new Size(1100, 650);
            this.StartPosition = FormStartPosition.Manual;
            this.BackColor = Color.FromArgb(240, 242, 245);
            this.Padding = new Padding(25);

            // Main Container
            Panel mainContainer = new Panel()
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent
            };

            // Header Section with Stats
            Panel headerSection = new Panel()
            {
                Dock = DockStyle.Top,
                Height = 140,
                BackColor = Color.Transparent
            };

            // Title Panel
            Panel titlePanel = new Panel()
            {
                Dock = DockStyle.Top,
                Height = 60,
                BackColor = Color.Transparent
            };

            lblHeader = new Label()
            {
                Text = "👩‍🎓 Student Management",
                Dock = DockStyle.Left,
                Width = 400,
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = Color.FromArgb(31, 41, 55)
            };

            // Search Panel
            searchPanel = CreateSearchPanel();
            titlePanel.Controls.Add(searchPanel);
            titlePanel.Controls.Add(lblHeader);

            // Stats Panel
            statsPanel = new Panel()
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                Padding = new Padding(0, 10, 0, 0)
            };

            Panel statCard = CreateStatCard();
            statsPanel.Controls.Add(statCard);

            headerSection.Controls.Add(statsPanel);
            headerSection.Controls.Add(titlePanel);

            // Action Buttons Panel
            buttonPanel = new Panel()
            {
                Dock = DockStyle.Top,
                Height = 70,
                BackColor = Color.Transparent,
                Padding = new Padding(0, 10, 0, 10)
            };

            btnAdd = CreateModernButton("➕ Add New Student", Color.FromArgb(34, 197, 94), 0);
            btnEdit = CreateModernButton("✏️ Edit Student", Color.FromArgb(59, 130, 246), 180);
            btnDelete = CreateModernButton("🗑️ Delete", Color.FromArgb(239, 68, 68), 360);
            btnRefresh = CreateModernButton("🔄 Refresh", Color.FromArgb(107, 114, 128), 540);

            btnAdd.Click += BtnAdd_Click;
            btnEdit.Click += BtnEdit_Click;
            btnDelete.Click += BtnDelete_Click;
            btnRefresh.Click += BtnRefresh_Click;

            buttonPanel.Controls.Add(btnAdd);
            buttonPanel.Controls.Add(btnEdit);
            buttonPanel.Controls.Add(btnDelete);
            buttonPanel.Controls.Add(btnRefresh);

            // DataGridView Panel with Card Design
            Panel dgvCard = new Panel()
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Padding = new Padding(15)
            };

            // Add rounded corners and shadow
            dgvCard.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

                // Draw shadow
                using (var shadowPath = CreateRoundedRectangle(2, 2, dgvCard.Width - 4, dgvCard.Height - 4, 8))
                using (var shadowBrush = new SolidBrush(Color.FromArgb(15, 0, 0, 0)))
                {
                    e.Graphics.FillPath(shadowBrush, shadowPath);
                }

                // Draw card
                using (var cardPath = CreateRoundedRectangle(0, 0, dgvCard.Width - 4, dgvCard.Height - 4, 8))
                using (var cardBrush = new SolidBrush(Color.White))
                {
                    e.Graphics.FillPath(cardBrush, cardPath);
                }
            };

            dgvStudents = new DataGridView()
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                RowHeadersVisible = false,
                MultiSelect = false,
                AllowUserToResizeRows = false
            };

            // Modern column headers
            dgvStudents.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(249, 250, 251);
            dgvStudents.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(55, 65, 81);
            dgvStudents.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 10, FontStyle.Bold);
            dgvStudents.ColumnHeadersDefaultCellStyle.Padding = new Padding(10, 8, 10, 8);
            dgvStudents.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgvStudents.ColumnHeadersHeight = 45;
            dgvStudents.EnableHeadersVisualStyles = false;

            // Modern row styling
            dgvStudents.RowsDefaultCellStyle.BackColor = Color.White;
            dgvStudents.RowsDefaultCellStyle.Font = new Font("Segoe UI", 9.5F);
            dgvStudents.RowsDefaultCellStyle.ForeColor = Color.FromArgb(55, 65, 81);
            dgvStudents.RowsDefaultCellStyle.Padding = new Padding(10, 5, 10, 5);
            dgvStudents.RowTemplate.Height = 42;
            dgvStudents.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(249, 250, 251);
            dgvStudents.DefaultCellStyle.SelectionBackColor = Color.FromArgb(219, 234, 254);
            dgvStudents.DefaultCellStyle.SelectionForeColor = Color.FromArgb(30, 64, 175);
            dgvStudents.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvStudents.GridColor = Color.FromArgb(229, 231, 235);

            dgvCard.Controls.Add(dgvStudents);

            // Assemble everything
            mainContainer.Controls.Add(dgvCard);
            mainContainer.Controls.Add(buttonPanel);
            mainContainer.Controls.Add(headerSection);

            this.Controls.Add(mainContainer);
        }

        private void LoadStudents()
        {
            try
            {
                DataTable dt = DatabaseHelper.GetAllStudents();
                dgvStudents.DataSource = dt;

                // Configure columns
                if (dgvStudents.Columns.Count > 0)
                {
                    dgvStudents.Columns["StudentId"].HeaderText = "🆔 Student ID";
                    dgvStudents.Columns["StudentId"].Width = 120;

                    dgvStudents.Columns["FullName"].HeaderText = "👤 Full Name";
                    dgvStudents.Columns["FullName"].Width = 200;

                    dgvStudents.Columns["Course"].HeaderText = "📚 Course";
                    dgvStudents.Columns["Course"].Width = 100;

                    dgvStudents.Columns["YearLevel"].HeaderText = "📅 Year Level";
                    dgvStudents.Columns["YearLevel"].Width = 110;

                    dgvStudents.Columns["Email"].HeaderText = "📧 Email";
                    dgvStudents.Columns["Email"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                    dgvStudents.Columns["Phone"].HeaderText = "📱 Phone";
                    dgvStudents.Columns["Phone"].Width = 130;

                    // Hide DateRegistered if present
                    if (dgvStudents.Columns.Contains("DateRegistered"))
                    {
                        dgvStudents.Columns["DateRegistered"].Visible = false;
                    }
                }

                // Update count
                lblTotalStudents.Text = dt.Rows.Count.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading students: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            StudentDialog dialog = new StudentDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                bool success = DatabaseHelper.AddStudent(
                    dialog.StudentId,
                    dialog.FullName,
                    dialog.Course,
                    dialog.YearLevel,
                    dialog.Email,
                    dialog.Phone
                );

                if (success)
                {
                    MessageBox.Show("✅ Student added successfully!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadStudents();
                }
            }
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (dgvStudents.SelectedRows.Count == 0)
            {
                MessageBox.Show("⚠️ Please select a student to edit!", "No Selection",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataGridViewRow row = dgvStudents.SelectedRows[0];

            // Handle potential DBNull values
            string email = row.Cells["Email"].Value != DBNull.Value ? row.Cells["Email"].Value.ToString() : "";
            string phone = row.Cells["Phone"].Value != DBNull.Value ? row.Cells["Phone"].Value.ToString() : "";

            StudentDialog dialog = new StudentDialog(
                row.Cells["StudentId"].Value.ToString(),
                row.Cells["FullName"].Value.ToString(),
                row.Cells["Course"].Value.ToString(),
                row.Cells["YearLevel"].Value.ToString(),
                email,
                phone,
                isEdit: true
            );

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                bool success = DatabaseHelper.UpdateStudent(
                    dialog.StudentId,
                    dialog.FullName,
                    dialog.Course,
                    dialog.YearLevel,
                    dialog.Email,
                    dialog.Phone
                );

                if (success)
                {
                    MessageBox.Show("✅ Student updated successfully!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadStudents();
                }
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (dgvStudents.SelectedRows.Count == 0)
            {
                MessageBox.Show("⚠️ Please select a student to delete!", "No Selection",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string studentId = dgvStudents.SelectedRows[0].Cells["StudentId"].Value.ToString();
            string studentName = dgvStudents.SelectedRows[0].Cells["FullName"].Value.ToString();

            DialogResult result = MessageBox.Show(
                $"Are you sure you want to delete this student?\n\n" +
                $"🆔 ID: {studentId}\n" +
                $"👤 Name: {studentName}\n\n" +
                $"⚠️ This action will perform a soft delete.",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                bool success = DatabaseHelper.DeleteStudent(studentId);
                if (success)
                {
                    MessageBox.Show("✅ Student deleted successfully!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadStudents();
                }
            }
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            txtSearch.Clear();
            LoadStudents();
            MessageBox.Show("✅ Data refreshed successfully!", "Refresh",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private Panel CreateSearchPanel()
        {
            Panel panel = new Panel()
            {
                Dock = DockStyle.Right,
                Width = 320,
                BackColor = Color.Transparent,
                Padding = new Padding(0, 8, 0, 0)
            };

            Panel searchBox = new Panel()
            {
                Location = new Point(0, 0),
                Size = new Size(320, 40),
                BackColor = Color.White
            };

            searchBox.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (var path = CreateRoundedRectangle(0, 0, searchBox.Width - 1, searchBox.Height - 1, 8))
                using (var pen = new Pen(Color.FromArgb(209, 213, 219), 2))
                using (var brush = new SolidBrush(Color.White))
                {
                    e.Graphics.FillPath(brush, path);
                    e.Graphics.DrawPath(pen, path);
                }
            };

            Label lblSearchIcon = new Label()
            {
                Text = "🔍",
                Location = new Point(12, 8),
                Size = new Size(25, 25),
                Font = new Font("Segoe UI", 11),
                BackColor = Color.Transparent
            };

            txtSearch = new TextBox()
            {
                Location = new Point(45, 9),
                Width = 260,
                Height = 22,
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.FromArgb(107, 114, 128),
                BorderStyle = BorderStyle.None,
                BackColor = Color.White
            };

            txtSearch.TextChanged += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtSearch.Text))
                {
                    LoadStudents();
                }
                else
                {
                    try
                    {
                        DataTable dt = DatabaseHelper.SearchStudents(txtSearch.Text);
                        dgvStudents.DataSource = dt;
                        lblTotalStudents.Text = dt.Rows.Count.ToString();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Search error: {ex.Message}", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            };

            searchBox.Controls.Add(txtSearch);
            searchBox.Controls.Add(lblSearchIcon);
            panel.Controls.Add(searchBox);

            return panel;
        }

        private Panel CreateStatCard()
        {
            Panel card = new Panel()
            {
                Location = new Point(0, 0),
                Size = new Size(220, 80),
                BackColor = Color.Transparent
            };

            card.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

                // Draw shadow
                using (var shadowPath = CreateRoundedRectangle(2, 2, card.Width - 4, card.Height - 4, 10))
                using (var shadowBrush = new SolidBrush(Color.FromArgb(20, 0, 0, 0)))
                {
                    e.Graphics.FillPath(shadowBrush, shadowPath);
                }

                // Draw card with gradient
                using (var path = CreateRoundedRectangle(0, 0, card.Width - 4, card.Height - 4, 10))
                using (var brush = new LinearGradientBrush(
                    new Rectangle(0, 0, card.Width, card.Height),
                    Color.FromArgb(59, 130, 246),
                    Color.FromArgb(37, 99, 235),
                    LinearGradientMode.ForwardDiagonal))
                {
                    e.Graphics.FillPath(brush, path);
                }
            };

            Label lblStatTitle = new Label()
            {
                Text = "Total Students",
                Location = new Point(18, 15),
                AutoSize = true,
                Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold),
                ForeColor = Color.FromArgb(220, 230, 255),
                BackColor = Color.Transparent
            };

            lblTotalStudents = new Label()
            {
                Text = "0",
                Location = new Point(18, 36),
                AutoSize = true,
                Font = new Font("Segoe UI", 26, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Transparent
            };

            card.Controls.Add(lblTotalStudents);
            card.Controls.Add(lblStatTitle);

            return card;
        }

        private Button CreateModernButton(string text, Color color, int xPosition)
        {
            Button btn = new Button()
            {
                Text = text,
                Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold),
                BackColor = color,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Height = 42,
                Width = 170,
                Location = new Point(xPosition, 10),
                Cursor = Cursors.Hand
            };

            btn.FlatAppearance.BorderSize = 0;

            Color hoverColor = ControlPaint.Light(color, 0.1f);
            Color originalColor = color;

            btn.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (var path = CreateRoundedRectangle(0, 0, btn.Width - 1, btn.Height - 1, 8))
                using (var brush = new SolidBrush(btn.BackColor))
                {
                    e.Graphics.FillPath(brush, path);
                }

                TextRenderer.DrawText(e.Graphics, btn.Text, btn.Font, btn.ClientRectangle,
                    btn.ForeColor, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            };

            btn.MouseEnter += (s, e) => { btn.BackColor = hoverColor; btn.Invalidate(); };
            btn.MouseLeave += (s, e) => { btn.BackColor = originalColor; btn.Invalidate(); };

            return btn;
        }

        private GraphicsPath CreateRoundedRectangle(int x, int y, int width, int height, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            path.AddArc(x, y, radius, radius, 180, 90);
            path.AddArc(x + width - radius, y, radius, radius, 270, 90);
            path.AddArc(x + width - radius, y + height - radius, radius, radius, 0, 90);
            path.AddArc(x, y + height - radius, radius, radius, 90, 90);
            path.CloseFigure();
            return path;
        }
    }
}