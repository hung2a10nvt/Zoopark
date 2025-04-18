using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Zoopark
{
    public partial class Form1 : Form
    {
        private List<double> revenueData = new List<double>();
        private List<double> serviceData = new List<double>();
        private List<double> investmentData = new List<double>();
        private List<int> visitorData = new List<int>();
        private List<double> adsData = new List<double>();
        private List<double> infrastructureData = new List<double>();

        private int currentPeriod = 0;
        private const int totalPeriods = 12;
        private double currentRevenue, currentService, currentInvestment, currentAds, currentInfrastructure;
        private int currentVisitors;
        private Timer simulationTimer;
        public Form1()
        {
            InitializeComponent();

            simulationTimer = new Timer();
            simulationTimer.Interval = 500; 
            simulationTimer.Tick += SimulationTimer_Tick;
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void bt1_Click(object sender, EventArgs e)
        {
            ClearData();

            currentRevenue = (double)edRevenue.Value;
            currentService = (double)edService.Value;
            currentInvestment = (double)edInvestment.Value;
            currentVisitors = (int)edVisitor.Value;
            currentAds = (double)edAds.Value;
            currentInfrastructure = 5.0;

            currentPeriod = 0;
            simulationTimer.Start();
        }

        private void SimulationTimer_Tick(object sender, EventArgs e)
        {
            if (currentPeriod >= totalPeriods)
            {
                simulationTimer.Stop();
                return;
            }

            UpdateSimulationData(currentPeriod,
                ref currentRevenue,
                ref currentService,
                ref currentInvestment,
                ref currentVisitors,
                ref currentInfrastructure,
                ref currentAds);

            revenueData.Add(currentRevenue);
            serviceData.Add(currentService);
            investmentData.Add(currentInvestment);
            visitorData.Add(currentVisitors);
            adsData.Add(currentAds);
            infrastructureData.Add(currentInfrastructure);

            chart.Series[0].Points.AddY(currentRevenue);
            chart.Series[1].Points.AddY(currentService);
            chart.Series[2].Points.AddY(currentInvestment);
            chart.Series[3].Points.AddY(currentVisitors);
            chart.Series[4].Points.AddY(currentAds);
            chart.Series[5].Points.AddY(currentInfrastructure);

            chart.Series[0].Points[currentPeriod].AxisLabel = (currentPeriod + 1).ToString();

            currentPeriod++;
        }

        private void UpdateSimulationData(int period,
            ref double revenue,
            ref double service,
            ref double investment,
            ref int visitors,
            ref double infrastructure,
            ref double ads)
        {
            ads = revenue * 0.1;
            infrastructure = infrastructure * 0.9 + investment * 0.1;

            double maxVisitors = 1000;
            visitors = (int)Math.Min(maxVisitors, visitors * 0.95 + infrastructure * 3 + ads * 0.1);
            service = visitors * 2.5 + revenue * 0.05;
            investment = ads * 0.3 + revenue * 0.12;
            revenue = revenue * 0.4 + visitors * 2.5 + service * 0.4;
        }

        private void ClearData()
        {
            revenueData.Clear();
            serviceData.Clear();
            investmentData.Clear();
            visitorData.Clear();
            adsData.Clear();
            infrastructureData.Clear();

            foreach (var series in chart.Series)
            {
                series.Points.Clear();
            }
        }

    }
}
