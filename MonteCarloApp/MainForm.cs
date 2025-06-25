using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace MonteCarloApp
{
    public partial class MainForm : Form
    {
        private List<ResultEntry> results = new();
        private const double x0 = 3, y0 = 0, R = 2, C = -1;
        public MainForm()
        {

            AddMenu(); 
            SetupUI();
        }

        private void SetupUI()
        {
            this.Text = "Monte Carlo Segment Calculator - C#";
            this.Width = 900;
            this.Height = 700;

            var mainLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 2
            };
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 80));
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 80));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 20));
            this.Controls.Add(mainLayout);

            var chart = new Chart { Dock = DockStyle.Fill, BackColor = Color.White };
            var chartArea = new ChartArea("main");
            chartArea.BackColor = Color.White;
            chartArea.AxisX.MajorGrid.LineColor = Color.LightGray;
            chartArea.AxisY.MajorGrid.LineColor = Color.LightGray;
            chartArea.AxisX.LineColor = Color.Black;
            chartArea.AxisY.LineColor = Color.Black;
            chartArea.AxisX.Crossing = 0;
            chartArea.AxisY.Crossing = 0;
            chartArea.AxisX.Minimum = x0 - R - 2;
            chartArea.AxisX.Maximum = x0 + R + 2;
            chartArea.AxisY.Minimum = y0 - R - 2;
            chartArea.AxisY.Maximum = y0 + R + 2;
            chart.ChartAreas.Add(chartArea);
            mainLayout.Controls.Add(chart, 0, 0);
            mainLayout.SetRowSpan(chart, 2);


            int buttonWidth = 120;
            int buttonHeight = 40;

            var inputN = new TextBox
            {
                Text = "???",
                Dock = DockStyle.Top,
                Width = buttonWidth,
                Height = buttonHeight,
                Margin = new Padding(3, 10, 3, 3)
            };
            var btnRun = new Button
            {
                Text = "Старт",
                Dock = DockStyle.Top,
                Width = buttonWidth,
                Height = buttonHeight,
                Margin = new Padding(3, 5, 3, 3)
            };
            var btnSave = new Button
            {
                Text = "Сохранить",
                Dock = DockStyle.Top,
                Width = buttonWidth,
                Height = buttonHeight,
                Margin = new Padding(3, 5, 3, 3)
            };
            var btnClear = new Button
            {
                Text = "Очистить",
                Dock = DockStyle.Top,
                Width = buttonWidth,
                Height = buttonHeight,
                Margin = new Padding(3, 5, 3, 3)
            };
            var btnAnalyze = new Button
            {
                Text = "Анализ",
                Dock = DockStyle.Top,
                Width = buttonWidth,
                Height = buttonHeight,
                Margin = new Padding(3, 5, 3, 3)
            };

            var rightPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                AutoScroll = true,
                BackColor = Color.WhiteSmoke,
                Padding = new Padding(10),
                MinimumSize = new Size(160, 0)
            };

            rightPanel.Controls.Add(new Label { Text = "Введите N:", AutoSize = true, Margin = new Padding(0, 20, 0, 0) });
            rightPanel.Controls.Add(inputN);
            rightPanel.Controls.Add(btnRun);
            rightPanel.Controls.Add(btnSave);
            rightPanel.Controls.Add(btnClear);
            rightPanel.Controls.Add(btnAnalyze);

            mainLayout.Controls.Add(rightPanel, 1, 0);
            mainLayout.SetRowSpan(rightPanel, 2);

            btnRun.Click += (s, e) =>
            {
                if (!int.TryParse(inputN.Text, out int N)) return;

                var (area, xs, ys, mask) = MonteCarlo.Run(N);
                chart.Series.Clear();
                chart.Titles.Clear();
                chart.Titles.Add($"Площадь ≈ {area:F4}");

                var inSeg = new Series("В сегменте")
                {
                    ChartType = SeriesChartType.Point,
                    MarkerSize = 2,
                    Color = Color.Cyan
                };

                var outSeg = new Series("Вне сегмента")
                {
                    ChartType = SeriesChartType.Point,
                    MarkerSize = 2,
                    Color = Color.Gray
                };

                for (int i = 0; i < xs.Count; i++)
                {
                    if (mask[i]) inSeg.Points.AddXY(xs[i], ys[i]);
                    else outSeg.Points.AddXY(xs[i], ys[i]);
                }

                chart.Series.Add(inSeg);
                chart.Series.Add(outSeg);
                chart.Series.Add(CreateCircleSeries(x0, y0, R));
                chart.Series.Add(CreateCLine(C));

                results.Add(new ResultEntry { N = N, SegmentArea = area });
            };

            btnAnalyze.Click += (s, e) =>
            {
                if (results.Count == 0)
                {
                    MessageBox.Show("Нет данных для анализа.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var analysisForm = new AnalysisForm(results);
                analysisForm.Show();
            };

            btnSave.Click += (s, e) =>
            {
                using SaveFileDialog dlg = new();
                dlg.Filter = "JSON Files|*.json|CSV Files|*.csv";
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    if (dlg.FileName.EndsWith(".json"))
                        DataSaver.SaveToJson(results, dlg.FileName);
                    else
                        DataSaver.SaveToCsv(results, dlg.FileName);
                }
            };

            btnClear.Click += (s, e) =>
            {
                chart.Series.Clear();
                chart.Titles.Clear();
                results.Clear();
            };
        }

        private void AddMenu()
        {
            var menu = new MenuStrip();
            var helpMenu = new ToolStripMenuItem("Справка");
            var aboutItem = new ToolStripMenuItem("О программе");
            var helpItem = new ToolStripMenuItem("Как пользоваться");

            aboutItem.Click += (s, e) =>
            {
                var about = new AboutForm();
                about.ShowDialog();
            };

            helpItem.Click += (s, e) =>
            {
                var help = new HelpForm();
                help.ShowDialog();
            };

            helpMenu.DropDownItems.Add(helpItem);
            helpMenu.DropDownItems.Add(aboutItem);
            menu.Items.Add(helpMenu);
            this.MainMenuStrip = menu;
            this.Controls.Add(menu);
            menu.Dock = DockStyle.Top;
            this.Controls.Add(menu);
            this.MainMenuStrip = menu;
        }

        private Series CreateCircleSeries(double x0, double y0, double r)
        {
            var series = new Series("Окружность")
            {
                ChartType = SeriesChartType.Line,
                Color = Color.Red,
                BorderWidth = 2
            };

            for (int i = 0; i <= 360; i++)
            {
                double angle = Math.PI * i / 180;
                double x = x0 + r * Math.Cos(angle);
                double y = y0 + r * Math.Sin(angle);
                series.Points.AddXY(x, y);
            }

            return series;
        }

        private Series CreateCLine(double cY)
        {
            var series = new Series("Прямая y = C")
            {
                ChartType = SeriesChartType.Line,
                Color = Color.Green,
                BorderDashStyle = ChartDashStyle.Dash,
                BorderWidth = 2
            };
            series.Points.AddXY(x0 - R - 2, cY);
            series.Points.AddXY(x0 + R + 2, cY);
            return series;
        }

        public class ResultEntry
        {
            public int N { get; set; }
            public double SegmentArea { get; set; }
        }
    }
}