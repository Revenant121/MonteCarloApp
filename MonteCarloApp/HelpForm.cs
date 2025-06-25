using System.Windows.Forms;

namespace MonteCarloApp
{
    public partial class HelpForm : Form
    {
        public HelpForm()
        {
            
            SetupUI();
        }

        private void SetupUI()
        {
            this.Text = "Справка";
            this.Width = 500;
            this.Height = 300;
            this.StartPosition = FormStartPosition.CenterParent;

            var label = new Label
            {
                Text = "Инструкция по использованию:\n\n" +
                       "1. Введите количество точек N.\n" +
                       "2. Нажмите кнопку 'Старт' — запустится моделирование методом Монте-Карло.\n" +
                       "3. Результат отобразится на графике.\n" +
                       "4. Вы можете сохранить данные в JSON или CSV.\n" +
                       "5. Кнопка 'Анализ' откроет статистику по всем моделированиям.",
                Dock = DockStyle.Fill,
                Padding = new Padding(10),
                TextAlign = System.Drawing.ContentAlignment.TopLeft
            };

            this.Controls.Add(label);
        }
    }
}
