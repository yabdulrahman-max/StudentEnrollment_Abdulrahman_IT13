using StudEnrollment_Abdulrahman_IT13;
using System;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace StudEnrollment_Abdulrahman_IT13
{
    public class EnrollmentForm : Form
    {
        private TextBox txtStudentId, txtStudentName;
        private ComboBox cboSubject, cboSemester, cboYearLevel;
        private Button btnEnroll, btnClear, btnCancel;
        private Label lblHeader;
        private DataGridView dgvEnrollments;

        public EnrollmentForm()
        {
            InitializeComponent();
            LoadSubjectsFromDatabase();
            LoadEnrollmentsFromDatabase();
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

            // Header
            lblHeader = new Label()
            {
                Text = "📝 Student Enrollment",
                Dock = DockStyle.Top,
                Height = 60,
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = Color.FromArgb(31, 41, 55),
                BackColor = Color.Transparent
            };

            // Form Card Container
            Panel formSection = new Panel()
            {
                Dock = DockStyle.Top,
                Height = 320,
                BackColor = Color.Transparent,
                Padding = new Padding(0, 10, 0, 20)
            };

            Panel formCard = CreateFormCard();
            formSection.Controls.Add(formCard);

            // Enrollments List Section
            Panel enrollmentsSection = new Panel()
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent
            };

            Label lblEnrollmentsList = new Label()
            {
                Text = "📋 Recent Enrollments",
                Dock = DockStyle.Top,
                Height = 40,
                Font = new Font("Segoe UI Semibold", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(31, 41, 55),
                TextAlign = ContentAlignment.MiddleLeft,
                BackColor = Color.Transparent
            };

            Panel dgvCard = CreateDataGridCard();

            enrollmentsSection.Controls.Add(dgvCard);
            enrollmentsSection.Controls.Add(lblEnrollmentsList);

            // Assemble
            mainContainer.Controls.Add(enrollmentsSection);
            mainContainer.Controls.Add(formSection);
            mainContainer.Controls.Add(lblHeader);

            this.Controls.Add(mainContainer);
        }

        private void LoadSubjectsFromDatabase()
        {
            try
            {
                DataTable dt = DatabaseHelper.GetSubjectsForDropdown();

                cboSubject.DataSource = dt;
                cboSubject.DisplayMember = "DisplayText";
                cboSubject.ValueMember = "SubjectCode";
                cboSubject.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading subjects: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadEnrollmentsFromDatabase()
        {
            try
            {
                DataTable dt = DatabaseHelper.GetAllEnrollments();
                dgvEnrollments.DataSource = dt;

                // Configure columns if data exists
                if (dgvEnrollments.Columns.Count > 0)
                {
                    dgvEnrollments.Columns["EnrollmentId"].HeaderText = "📋 Enrollment ID";
                    dgvEnrollments.Columns["EnrollmentId"].Width = 130;

                    dgvEnrollments.Columns["StudentId"].HeaderText = "🆔 Student ID";
                    dgvEnrollments.Columns["StudentId"].Width = 120;

                    dgvEnrollments.Columns["StudentName"].HeaderText = "👤 Student Name";
                    dgvEnrollments.Columns["StudentName"].Width = 180;

                    dgvEnrollments.Columns["Subject"].HeaderText = "📚 Subject";
                    dgvEnrollments.Columns["Subject"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                    dgvEnrollments.Columns["Semester"].HeaderText = "📅 Semester";
                    dgvEnrollments.Columns["Semester"].Width = 120;

                    dgvEnrollments.Columns["YearLevel"].HeaderText = "📖 Year Level";
                    dgvEnrollments.Columns["YearLevel"].Width = 100;

                    dgvEnrollments.Columns["DateEnrolled"].HeaderText = "🗓️ Date Enrolled";
                    dgvEnrollments.Columns["DateEnrolled"].Width = 120;

                    // Hide columns we don't need to display
                    if (dgvEnrollments.Columns.Contains("SubjectCode"))
                        dgvEnrollments.Columns["SubjectCode"].Visible = false;
                    if (dgvEnrollments.Columns.Contains("SubjectDescription"))
                        dgvEnrollments.Columns["SubjectDescription"].Visible = false;
                    if (dgvEnrollments.Columns.Contains("Status"))
                        dgvEnrollments.Columns["Status"].Visible = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading enrollments: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private Panel CreateFormCard()
        {
            Panel card = new Panel()
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Padding = new Padding(30)
            };

            card.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

                // Draw shadow
                using (var shadowPath = CreateRoundedRectangle(2, 2, card.Width - 4, card.Height - 4, 12))
                using (var shadowBrush = new SolidBrush(Color.FromArgb(15, 0, 0, 0)))
                {
                    e.Graphics.FillPath(shadowBrush, shadowPath);
                }

                // Draw card
                using (var cardPath = CreateRoundedRectangle(0, 0, card.Width - 4, card.Height - 4, 12))
                using (var cardBrush = new SolidBrush(Color.White))
                {
                    e.Graphics.FillPath(cardBrush, cardPath);
                }
            };

            // Row 1 - Student ID and Name
            int row1Y = 20;
            Label lblStudentId = CreateLabel("🆔 Student ID:", 20, row1Y);

            Panel txtIdContainer = CreateTextBoxContainer(180, row1Y - 5, 200);
            txtStudentId = new TextBox()
            {
                Location = new Point(10, 8),
                Width = 180,
                Font = new Font("Segoe UI", 10),
                BorderStyle = BorderStyle.None,
                BackColor = Color.FromArgb(249, 250, 251)
            };

            // Auto-fill student name when leaving the Student ID field
            txtStudentId.Leave += TxtStudentId_Leave;

            txtIdContainer.Controls.Add(txtStudentId);
            card.Controls.Add(txtIdContainer);

            Label lblStudentName = CreateLabel("👤 Student Name:", 420, row1Y);

            Panel txtNameContainer = CreateTextBoxContainer(580, row1Y - 5, 300);
            txtStudentName = new TextBox()
            {
                Location = new Point(10, 8),
                Width = 280,
                Font = new Font("Segoe UI", 10),
                BorderStyle = BorderStyle.None,
                BackColor = Color.FromArgb(249, 250, 251),
                ReadOnly = true
            };
            txtNameContainer.Controls.Add(txtStudentName);
            card.Controls.Add(txtNameContainer);

            // Row 2 - Subject and Semester
            int row2Y = 90;
            Label lblSubject = CreateLabel("📚 Subject:", 20, row2Y);
            cboSubject = CreateComboBox(180, row2Y - 5, 200);

            Label lblSemester = CreateLabel("📅 Semester:", 420, row2Y);
            cboSemester = CreateComboBox(580, row2Y - 5, 150);
            cboSemester.Items.AddRange(new string[] { "1st Semester", "2nd Semester", "Summer" });

            // Row 3 - Year Level
            int row3Y = 160;
            Label lblYearLevel = CreateLabel("📖 Year Level:", 20, row3Y);
            cboYearLevel = CreateComboBox(180, row3Y - 5, 200);
            cboYearLevel.Items.AddRange(new string[] { "1st Year", "2nd Year", "3rd Year", "4th Year" });

            // Buttons
            btnEnroll = CreateModernButton("✅ Enroll Student", Color.FromArgb(34, 197, 94), 280, 225);
            btnClear = CreateModernButton("🔄 Clear Form", Color.FromArgb(107, 114, 128), 470, 225);
            btnCancel = CreateModernButton("🗑️ Cancel Enrollment", Color.FromArgb(239, 68, 68), 660, 225);

            btnEnroll.Click += BtnEnroll_Click;
            btnClear.Click += BtnClear_Click;
            btnCancel.Click += BtnCancel_Click;

            // Add all controls to card
            card.Controls.Add(lblStudentId);
            card.Controls.Add(lblStudentName);
            card.Controls.Add(lblSubject);
            card.Controls.Add(cboSubject);
            card.Controls.Add(lblSemester);
            card.Controls.Add(cboSemester);
            card.Controls.Add(lblYearLevel);
            card.Controls.Add(cboYearLevel);
            card.Controls.Add(btnEnroll);
            card.Controls.Add(btnClear);
            card.Controls.Add(btnCancel);

            return card;
        }

        private void TxtStudentId_Leave(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtStudentId.Text))
            {
                try
                {
                    DataTable dt = DatabaseHelper.GetStudentInfo(txtStudentId.Text);
                    if (dt.Rows.Count > 0)
                    {
                        txtStudentName.Text = dt.Rows[0]["FullName"].ToString();
                    }
                    else
                    {
                        txtStudentName.Text = "";
                        MessageBox.Show("⚠️ Student ID not found in database!", "Invalid Student",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtStudentId.Focus();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error fetching student: {ex.Message}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                txtStudentName.Text = "";
            }
        }

        private Panel CreateDataGridCard()
        {
            Panel card = new Panel()
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Padding = new Padding(0)
            };

            card.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

                // Draw shadow
                using (var shadowPath = CreateRoundedRectangle(2, 2, card.Width - 4, card.Height - 4, 12))
                using (var shadowBrush = new SolidBrush(Color.FromArgb(15, 0, 0, 0)))
                {
                    e.Graphics.FillPath(shadowBrush, shadowPath);
                }

                // Draw card
                using (var cardPath = CreateRoundedRectangle(0, 0, card.Width - 4, card.Height - 4, 12))
                using (var cardBrush = new SolidBrush(Color.White))
                {
                    e.Graphics.FillPath(cardBrush, cardPath);
                }
            };

            dgvEnrollments = new DataGridView()
            {
                Location = new Point(15, 15),
                Size = new Size(card.Width - 30, card.Height - 30),
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
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

            // Modern styling
            dgvEnrollments.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(249, 250, 251);
            dgvEnrollments.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(55, 65, 81);
            dgvEnrollments.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 10, FontStyle.Bold);
            dgvEnrollments.ColumnHeadersDefaultCellStyle.Padding = new Padding(10, 8, 10, 8);
            dgvEnrollments.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgvEnrollments.ColumnHeadersHeight = 45;
            dgvEnrollments.EnableHeadersVisualStyles = false;

            dgvEnrollments.RowsDefaultCellStyle.BackColor = Color.White;
            dgvEnrollments.RowsDefaultCellStyle.Font = new Font("Segoe UI", 9.5F);
            dgvEnrollments.RowsDefaultCellStyle.ForeColor = Color.FromArgb(55, 65, 81);
            dgvEnrollments.RowsDefaultCellStyle.Padding = new Padding(10, 5, 10, 5);
            dgvEnrollments.RowTemplate.Height = 42;
            dgvEnrollments.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(249, 250, 251);
            dgvEnrollments.DefaultCellStyle.SelectionBackColor = Color.FromArgb(219, 234, 254);
            dgvEnrollments.DefaultCellStyle.SelectionForeColor = Color.FromArgb(30, 64, 175);
            dgvEnrollments.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvEnrollments.GridColor = Color.FromArgb(229, 231, 235);

            card.Controls.Add(dgvEnrollments);

            return card;
        }

        private Label CreateLabel(string text, int x, int y)
        {
            return new Label()
            {
                Text = text,
                Location = new Point(x, y),
                AutoSize = true,
                Font = new Font("Segoe UI Semibold", 10, FontStyle.Bold),
                ForeColor = Color.FromArgb(55, 65, 81),
                BackColor = Color.Transparent
            };
        }

        private Panel CreateTextBoxContainer(int x, int y, int width)
        {
            Panel container = new Panel()
            {
                Location = new Point(x, y),
                Size = new Size(width, 36),
                BackColor = Color.Transparent
            };

            container.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (var path = CreateRoundedRectangle(0, 0, container.Width - 1, container.Height - 1, 6))
                using (var pen = new Pen(Color.FromArgb(209, 213, 219), 2))
                using (var brush = new SolidBrush(Color.FromArgb(249, 250, 251)))
                {
                    e.Graphics.FillPath(brush, path);
                    e.Graphics.DrawPath(pen, path);
                }
            };

            return container;
        }

        private ComboBox CreateComboBox(int x, int y, int width)
        {
            ComboBox cbo = new ComboBox()
            {
                Location = new Point(x, y),
                Width = width,
                Height = 36,
                Font = new Font("Segoe UI", 10),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(249, 250, 251),
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            return cbo;
        }

        private Button CreateModernButton(string text, Color color, int x, int y)
        {
            Button btn = new Button()
            {
                Text = text,
                Font = new Font("Segoe UI Semibold", 10, FontStyle.Bold),
                BackColor = color,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Height = 42,
                Width = 180,
                Location = new Point(x, y),
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

        private void BtnEnroll_Click(object sender, EventArgs e)
        {
            // Validation
            if (string.IsNullOrWhiteSpace(txtStudentId.Text))
            {
                MessageBox.Show("⚠️ Please enter a Student ID!", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtStudentId.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtStudentName.Text))
            {
                MessageBox.Show("⚠️ Student not found! Please enter a valid Student ID.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtStudentId.Focus();
                return;
            }

            if (cboSubject.SelectedIndex == -1)
            {
                MessageBox.Show("⚠️ Please select a Subject!", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboSubject.Focus();
                return;
            }

            if (cboSemester.SelectedIndex == -1)
            {
                MessageBox.Show("⚠️ Please select a Semester!", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboSemester.Focus();
                return;
            }

            if (cboYearLevel.SelectedIndex == -1)
            {
                MessageBox.Show("⚠️ Please select a Year Level!", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboYearLevel.Focus();
                return;
            }

            try
            {
                // Generate enrollment ID
                string enrollmentId = DatabaseHelper.GenerateNextEnrollmentId();

                // Get subject code from selected item
                string subjectCode = cboSubject.SelectedValue.ToString();

                // Add enrollment to database
                bool success = DatabaseHelper.AddEnrollment(
                    enrollmentId,
                    txtStudentId.Text,
                    subjectCode,
                    cboSemester.Text,
                    cboYearLevel.Text
                );

                if (success)
                {
                    MessageBox.Show("✅ Student enrolled successfully!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadEnrollmentsFromDatabase();
                    BtnClear_Click(sender, e);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error enrolling student: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            txtStudentId.Clear();
            txtStudentName.Clear();
            cboSubject.SelectedIndex = -1;
            cboSemester.SelectedIndex = -1;
            cboYearLevel.SelectedIndex = -1;
            txtStudentId.Focus();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            if (dgvEnrollments.SelectedRows.Count == 0)
            {
                MessageBox.Show("⚠️ Please select an enrollment to cancel!", "No Selection",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string enrollmentId = dgvEnrollments.SelectedRows[0].Cells["EnrollmentId"].Value.ToString();
            string studentName = dgvEnrollments.SelectedRows[0].Cells["StudentName"].Value.ToString();
            string subject = dgvEnrollments.SelectedRows[0].Cells["Subject"].Value.ToString();

            DialogResult result = MessageBox.Show(
                $"Are you sure you want to cancel this enrollment?\n\n" +
                $"📋 Enrollment ID: {enrollmentId}\n" +
                $"👤 Student: {studentName}\n" +
                $"📚 Subject: {subject}\n\n" +
                $"⚠️ This action will cancel the enrollment.",
                "Confirm Cancel",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                bool success = DatabaseHelper.CancelEnrollment(enrollmentId);
                if (success)
                {
                    MessageBox.Show("✅ Enrollment cancelled successfully!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadEnrollmentsFromDatabase();
                }
            }
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