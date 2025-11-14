using StudEnrollment_Abdulrahman_IT13;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace StudEnrollment_Abdulrahman_IT13
{
    public class StudentDialog : Form
    {
        private TextBox txtStudentId, txtFullName, txtEmail, txtPhone;
        private ComboBox cboCourse, cboYearLevel;
        private Button btnSave, btnCancel;
        private Label lblHeader;
        private bool isEditMode = false;

        // Properties to get values
        public string StudentId => txtStudentId.Text.Trim();
        public string FullName => txtFullName.Text.Trim();
        public string Course => cboCourse.Text;
        public string YearLevel => cboYearLevel.Text;
        public string Email => txtEmail.Text.Trim();
        public string Phone => txtPhone.Text.Trim();

        // Constructor for Add mode
        public StudentDialog()
        {
            InitializeComponent();
            isEditMode = false;
            lblHeader.Text = "➕ Add New Student";

            // Generate next student ID
            txtStudentId.Text = DatabaseHelper.GenerateNextStudentId();
            txtStudentId.ReadOnly = true;
            txtStudentId.BackColor = Color.FromArgb(249, 250, 251);
        }

        // Constructor for Edit mode
        public StudentDialog(string studentId, string fullName, string course,
            string yearLevel, string email, string phone, bool isEdit = false)
        {
            InitializeComponent();
            isEditMode = isEdit;
            lblHeader.Text = "✏️ Edit Student";

            // Populate fields
            txtStudentId.Text = studentId;
            txtFullName.Text = fullName;
            cboCourse.Text = course;
            cboYearLevel.Text = yearLevel;
            txtEmail.Text = email;
            txtPhone.Text = phone;

            // Student ID should not be editable in edit mode
            txtStudentId.ReadOnly = true;
            txtStudentId.BackColor = Color.FromArgb(249, 250, 251);
        }

        private void InitializeComponent()
        {
            this.Text = "Student Information";
            this.Size = new Size(500, 710);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.FromArgb(240, 242, 245);

            // Main Container
            Panel mainContainer = new Panel()
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent
            };

            // Header
            lblHeader = new Label()
            {
                Text = "➕ Add New Student",
                Dock = DockStyle.Top,
                Height = 50,
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.FromArgb(31, 41, 55),
                TextAlign = ContentAlignment.MiddleLeft,
                BackColor = Color.Transparent
            };

            // Form Card
            Panel formCard = new Panel()
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Padding = new Padding(30)
            };

            formCard.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (var path = CreateRoundedRectangle(0, 0, formCard.Width - 1, formCard.Height - 1, 12))
                using (var brush = new SolidBrush(Color.White))
                using (var pen = new Pen(Color.FromArgb(229, 231, 235), 1))
                {
                    e.Graphics.FillPath(brush, path);
                    e.Graphics.DrawPath(pen, path);
                }
            };

            // Student ID
            int yPos = 20;
            Label lblStudentId = CreateLabel("🆔 Student ID:", 20, yPos);
            Panel txtIdContainer = CreateTextBoxContainer(20, yPos + 30, 450);
            txtStudentId = new TextBox()
            {
                Location = new Point(10, 8),
                Width = 430,
                Font = new Font("Segoe UI", 10),
                BorderStyle = BorderStyle.None,
                BackColor = Color.FromArgb(249, 250, 251)
            };
            txtIdContainer.Controls.Add(txtStudentId);
            formCard.Controls.Add(txtIdContainer);
            formCard.Controls.Add(lblStudentId);

            // Full Name
            yPos += 90;
            Label lblFullName = CreateLabel("👤 Full Name:", 20, yPos);
            Panel txtNameContainer = CreateTextBoxContainer(20, yPos + 30, 450);
            txtFullName = new TextBox()
            {
                Location = new Point(10, 8),
                Width = 430,
                Font = new Font("Segoe UI", 10),
                BorderStyle = BorderStyle.None,
                BackColor = Color.FromArgb(249, 250, 251)
            };
            txtNameContainer.Controls.Add(txtFullName);
            formCard.Controls.Add(txtNameContainer);
            formCard.Controls.Add(lblFullName);

            // Course
            yPos += 90;
            Label lblCourse = CreateLabel("📚 Course:", 20, yPos);
            cboCourse = CreateComboBox(20, yPos + 30, 450);
            cboCourse.Items.AddRange(new string[] {
                "BSIT - Information Technology",
                "BSCS - Computer Science",
                "BSIS - Information Systems",
                "BSCE - Computer Engineering",
                "BSCpE - Electronics Engineering"
            });
            formCard.Controls.Add(cboCourse);
            formCard.Controls.Add(lblCourse);

            // Year Level
            yPos += 90;
            Label lblYearLevel = CreateLabel("📖 Year Level:", 20, yPos);
            cboYearLevel = CreateComboBox(20, yPos + 30, 450);
            cboYearLevel.Items.AddRange(new string[] {
                "1st Year",
                "2nd Year",
                "3rd Year",
                "4th Year"
            });
            formCard.Controls.Add(cboYearLevel);
            formCard.Controls.Add(lblYearLevel);

            // Email
            yPos += 90;
            Label lblEmail = CreateLabel("📧 Email:", 20, yPos);
            Panel txtEmailContainer = CreateTextBoxContainer(20, yPos + 30, 450);
            txtEmail = new TextBox()
            {
                Location = new Point(10, 8),
                Width = 430,
                Font = new Font("Segoe UI", 10),
                BorderStyle = BorderStyle.None,
                BackColor = Color.FromArgb(249, 250, 251)
            };
            txtEmailContainer.Controls.Add(txtEmail);
            formCard.Controls.Add(txtEmailContainer);
            formCard.Controls.Add(lblEmail);

            // Phone
            yPos += 90;
            Label lblPhone = CreateLabel("📱 Phone:", 20, yPos);
            Panel txtPhoneContainer = CreateTextBoxContainer(20, yPos + 30, 450);
            txtPhone = new TextBox()
            {
                Location = new Point(10, 8),
                Width = 430,
                Font = new Font("Segoe UI", 10),
                BorderStyle = BorderStyle.None,
                BackColor = Color.FromArgb(249, 250, 251)
            };
            txtPhoneContainer.Controls.Add(txtPhone);
            formCard.Controls.Add(txtPhoneContainer);
            formCard.Controls.Add(lblPhone);

            // Buttons
            yPos += 80;
            btnSave = CreateModernButton("💾 Save Student", Color.FromArgb(34, 197, 94), 20, yPos);
            btnCancel = CreateModernButton("❌ Cancel", Color.FromArgb(107, 114, 128), 250, yPos);

            btnSave.Click += BtnSave_Click;
            btnCancel.Click += (s, e) => this.DialogResult = DialogResult.Cancel;

            formCard.Controls.Add(btnSave);
            formCard.Controls.Add(btnCancel);

            mainContainer.Controls.Add(formCard);
            mainContainer.Controls.Add(lblHeader);

            this.Controls.Add(mainContainer);
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            // Validation
            if (string.IsNullOrWhiteSpace(txtStudentId.Text))
            {
                MessageBox.Show("⚠️ Student ID is required!", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtStudentId.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtFullName.Text))
            {
                MessageBox.Show("⚠️ Full Name is required!", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtFullName.Focus();
                return;
            }

            if (cboCourse.SelectedIndex == -1)
            {
                MessageBox.Show("⚠️ Please select a Course!", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboCourse.Focus();
                return;
            }

            if (cboYearLevel.SelectedIndex == -1)
            {
                MessageBox.Show("⚠️ Please select a Year Level!", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboYearLevel.Focus();
                return;
            }

            // Email validation (optional field but validate format if provided)
            if (!string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                if (!IsValidEmail(txtEmail.Text))
                {
                    MessageBox.Show("⚠️ Please enter a valid email address!", "Validation Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtEmail.Focus();
                    return;
                }
            }

            // Phone validation (optional field but validate format if provided)
            if (!string.IsNullOrWhiteSpace(txtPhone.Text))
            {
                if (!IsValidPhone(txtPhone.Text))
                {
                    MessageBox.Show("⚠️ Please enter a valid phone number (10-11 digits)!", "Validation Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtPhone.Focus();
                    return;
                }
            }

            // If in Add mode, check if student ID already exists
            if (!isEditMode)
            {
                if (DatabaseHelper.CheckStudentExists(txtStudentId.Text))
                {
                    MessageBox.Show("⚠️ This Student ID already exists!", "Duplicate Entry",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtStudentId.Focus();
                    return;
                }
            }

            // All validations passed - close dialog with OK result
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private bool IsValidPhone(string phone)
        {
            // Remove spaces, dashes, and parentheses
            string cleanPhone = phone.Replace(" ", "").Replace("-", "").Replace("(", "").Replace(")", "");

            // Check if it's all digits and has 10-11 characters
            return cleanPhone.Length >= 10 && cleanPhone.Length <= 11 && long.TryParse(cleanPhone, out _);
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
                Width = 210,
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