using StudEnrollment_Abdulrahman_IT13;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace StudEnrollment_Abdulrahman_IT13
{
    public class SubjectDialog : Form
    {
        private TextBox txtSubjectCode, txtDescription, txtUnits, txtDepartment, txtInstructor;
        private Button btnSave, btnCancel;
        private Label lblHeader;
        private bool isEditMode = false;

        // Properties to get values
        public string SubjectCode => txtSubjectCode.Text.Trim();
        public string Description => txtDescription.Text.Trim();
        public int Units => int.TryParse(txtUnits.Text.Trim(), out int result) ? result : 0;
        public string Department => txtDepartment.Text.Trim();
        public string Instructor => txtInstructor.Text.Trim();

        // Constructor for Add mode
        public SubjectDialog()
        {
            InitializeComponent();
            isEditMode = false;
            lblHeader.Text = "➕ Add New Subject";
        }

        // Constructor for Edit mode
        public SubjectDialog(string subjectCode, string description, int units,
            string department, string instructor, bool isEdit = false)
        {
            InitializeComponent();
            isEditMode = isEdit;
            lblHeader.Text = "✏️ Edit Subject";

            // Populate fields
            txtSubjectCode.Text = subjectCode;
            txtDescription.Text = description;
            txtUnits.Text = units.ToString();
            txtDepartment.Text = department;
            txtInstructor.Text = instructor;

            // Subject code should not be editable in edit mode
            txtSubjectCode.ReadOnly = true;
            txtSubjectCode.BackColor = Color.FromArgb(249, 250, 251);
        }

        private void InitializeComponent()
        {
            this.Text = "Subject Information";
            this.Size = new Size(500, 620);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.FromArgb(240, 242, 245);

            // Main Container
            Panel mainContainer = new Panel()
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                Padding = new Padding(20)
            };

            // Header
            lblHeader = new Label()
            {
                Text = "➕ Add New Subject",
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

            // Subject Code
            int yPos = 20;
            Label lblSubjectCode = CreateLabel("📝 Subject Code:", 20, yPos);
            Panel txtCodeContainer = CreateTextBoxContainer(20, yPos + 30, 410);
            txtSubjectCode = new TextBox()
            {
                Location = new Point(10, 8),
                Width = 390,
                Font = new Font("Segoe UI", 10),
                BorderStyle = BorderStyle.None,
                BackColor = Color.FromArgb(249, 250, 251)
            };
            txtCodeContainer.Controls.Add(txtSubjectCode);
            formCard.Controls.Add(txtCodeContainer);
            formCard.Controls.Add(lblSubjectCode);

            // Description
            yPos += 90;
            Label lblDescription = CreateLabel("📖 Description:", 20, yPos);
            Panel txtDescContainer = CreateTextBoxContainer(20, yPos + 30, 410);
            txtDescription = new TextBox()
            {
                Location = new Point(10, 8),
                Width = 390,
                Font = new Font("Segoe UI", 10),
                BorderStyle = BorderStyle.None,
                BackColor = Color.FromArgb(249, 250, 251)
            };
            txtDescContainer.Controls.Add(txtDescription);
            formCard.Controls.Add(txtDescContainer);
            formCard.Controls.Add(lblDescription);

            // Units
            yPos += 90;
            Label lblUnits = CreateLabel("⏱️ Units:", 20, yPos);
            Panel txtUnitsContainer = CreateTextBoxContainer(20, yPos + 30, 410);
            txtUnits = new TextBox()
            {
                Location = new Point(10, 8),
                Width = 390,
                Font = new Font("Segoe UI", 10),
                BorderStyle = BorderStyle.None,
                BackColor = Color.FromArgb(249, 250, 251)
            };
            txtUnitsContainer.Controls.Add(txtUnits);
            formCard.Controls.Add(txtUnitsContainer);
            formCard.Controls.Add(lblUnits);

            // Department
            yPos += 90;
            Label lblDepartment = CreateLabel("🏢 Department:", 20, yPos);
            Panel txtDeptContainer = CreateTextBoxContainer(20, yPos + 30, 410);
            txtDepartment = new TextBox()
            {
                Location = new Point(10, 8),
                Width = 390,
                Font = new Font("Segoe UI", 10),
                BorderStyle = BorderStyle.None,
                BackColor = Color.FromArgb(249, 250, 251)
            };
            txtDeptContainer.Controls.Add(txtDepartment);
            formCard.Controls.Add(txtDeptContainer);
            formCard.Controls.Add(lblDepartment);

            // Instructor
            yPos += 90;
            Label lblInstructor = CreateLabel("👨‍🏫 Instructor (Optional):", 20, yPos);
            Panel txtInstructorContainer = CreateTextBoxContainer(20, yPos + 30, 410);
            txtInstructor = new TextBox()
            {
                Location = new Point(10, 8),
                Width = 390,
                Font = new Font("Segoe UI", 10),
                BorderStyle = BorderStyle.None,
                BackColor = Color.FromArgb(249, 250, 251)
            };
            txtInstructorContainer.Controls.Add(txtInstructor);
            formCard.Controls.Add(txtInstructorContainer);
            formCard.Controls.Add(lblInstructor);

            // Buttons
            yPos += 80;
            btnSave = CreateModernButton("💾 Save Subject", Color.FromArgb(34, 197, 94), 20, yPos);
            btnCancel = CreateModernButton("❌ Cancel", Color.FromArgb(107, 114, 128), 240, yPos);

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
            if (string.IsNullOrWhiteSpace(txtSubjectCode.Text))
            {
                MessageBox.Show("⚠️ Subject Code is required!", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSubjectCode.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtDescription.Text))
            {
                MessageBox.Show("⚠️ Description is required!", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDescription.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtUnits.Text))
            {
                MessageBox.Show("⚠️ Units is required!", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUnits.Focus();
                return;
            }

            // Validate units is a number
            if (!int.TryParse(txtUnits.Text, out int units) || units <= 0 || units > 10)
            {
                MessageBox.Show("⚠️ Please enter a valid number of units (1-10)!", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUnits.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtDepartment.Text))
            {
                MessageBox.Show("⚠️ Department is required!", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDepartment.Focus();
                return;
            }

            // If in Add mode, check if subject code already exists
            if (!isEditMode)
            {
                if (DatabaseHelper.CheckSubjectExists(txtSubjectCode.Text))
                {
                    MessageBox.Show("⚠️ This Subject Code already exists!", "Duplicate Entry",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtSubjectCode.Focus();
                    return;
                }
            }

            // All validations passed - close dialog with OK result
            this.DialogResult = DialogResult.OK;
            this.Close();
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
                Width = 200,
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