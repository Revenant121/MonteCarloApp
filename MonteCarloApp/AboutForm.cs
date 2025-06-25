using System.Windows.Forms;

namespace MonteCarloApp
{
    public partial class AboutForm : Form
    {
        public AboutForm()
        {
           
            SetupUI();
        }

        private void SetupUI()
        {
            this.Text = "О программе";
            this.Width = 400;
            this.Height = 250;
            this.StartPosition = FormStartPosition.CenterParent;

            var label = new Label
            {
                Text = "Monte Carlo Segment Calculator\n\nАвтор: Церенов Л.Е.\nВариант: 21\nВерсия: 1.0\n\nПрограмма предназначена для вычисления площади сегмента методом Монте-Карло.",
                Dock = DockStyle.Fill,
                TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            };

            this.Controls.Add(label);
        }
    }
}
