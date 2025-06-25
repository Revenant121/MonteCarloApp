using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using static MonteCarloApp.MainForm;

namespace MonteCarloApp
{
    public partial class AnalysisForm : Form
    {
        private List<ResultEntry> _results;
        private Chart _chart;

        public AnalysisForm(List<ResultEntry> results)
        {
            _results = results;
            SetupUI();
            PlotResults();
        }

        private void SetupUI()
        {
            this.Text = "Анализ моделирования";
            this.Width = 800;
            this.Height = 600;
            this.StartPosition = FormStartPosition.CenterScreen;

            _chart = new Chart { Dock = DockStyle.Fill };
            var area = new ChartArea("Stats");
            area.BackColor = System.Drawing.Color.WhiteSmoke;
            _chart.ChartAreas.Add(area);

            Controls.Add(_chart);
        }

        private void PlotResults()
        {
            _chart.Series.Clear();

            var series = new Series("Площади сегментов")
            {
                ChartType = SeriesChartType.Column,
                Color = System.Drawing.Color.DeepSkyBlue
            };

            for (int i = 0; i < _results.Count; i++)
            {
                var entry = _results[i];
                series.Points.AddXY(i + 1, entry.SegmentArea);
            }

            _chart.Series.Add(series);
        }
    }
}
