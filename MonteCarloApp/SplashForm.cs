using System;
using System.Windows.Forms;

namespace MonteCarloApp
{
    public partial class SplashForm : Form
    {
        private Button startButton;

        public SplashForm()
        {
           
            SetupUI();
        }

        private void SetupUI()
        {
            this.Text = "Monte Carlo Segment - Заставка";
            this.Width = 600;
            this.Height = 400;
            this.StartPosition = FormStartPosition.CenterScreen;

            var layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 4,
                BackColor = System.Drawing.Color.FromArgb(30, 30, 30)
            };
                
            var labelAuthor = new Label
            {
                Text = "Автор: Церенов Л.Е.",
                Dock = DockStyle.Fill,
                ForeColor = System.Drawing.Color.White,
                Font = new System.Drawing.Font("Segoe UI", 16, System.Drawing.FontStyle.Bold),
                TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            };

            var labelVariant = new Label
            {
                Text = "Вариант: 21",
                Dock = DockStyle.Fill,
                ForeColor = System.Drawing.Color.White,
                Font = new System.Drawing.Font("Segoe UI", 14),
                TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            };

            startButton = new Button
            {
                Text = "Начать работу",
                Dock = DockStyle.Fill,
                Font = new System.Drawing.Font("Segoe UI", 14),
                BackColor = System.Drawing.Color.DeepSkyBlue,
                ForeColor = System.Drawing.Color.White
            };
            startButton.Click += StartButton_Click;

            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 40));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 20));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 20));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 20));

            layout.Controls.Add(labelAuthor, 0, 0);
            layout.Controls.Add(labelVariant, 0, 1);
            layout.Controls.Add(new Panel(), 0, 2); // spacer
            layout.Controls.Add(startButton, 0, 3);

            this.Controls.Add(layout);
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            this.Hide();
            var mainForm = new MainForm();
            mainForm.FormClosed += (s, args) => this.Close();
            mainForm.Show();
        }
    }
}
