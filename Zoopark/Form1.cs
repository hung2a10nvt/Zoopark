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
        private double salaries = 50.0;
        private int hardWork = 11; // необходимое количество сотрудников
        private double currentRevenue, currentService, currentInvestment, currentAds, currentInfrastructure, currentQualityAnimalHusbandry;
        private int currentVisitors, currentEmployee;
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

        private void chart_Click(object sender, EventArgs e)
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
            currentQualityAnimalHusbandry = 100.0;
            currentEmployee = 10;

            currentPeriod = 0;

            revenueData.Add(currentRevenue);
            serviceData.Add(currentService);
            investmentData.Add(currentInvestment);
            visitorData.Add(currentVisitors);
            adsData.Add(currentAds);
            infrastructureData.Add(currentInfrastructure);

            chart.Series[0].Points.AddXY(currentPeriod, currentRevenue);
            chart.Series[1].Points.AddXY(currentPeriod, currentService);
            chart.Series[2].Points.AddXY(currentPeriod, currentInvestment);
            chart.Series[3].Points.AddXY(currentPeriod, currentVisitors);
            chart.Series[4].Points.AddXY(currentPeriod, currentAds);
            chart.Series[5].Points.AddXY(currentPeriod, currentInfrastructure);
            chart.Series[6].Points.AddXY(currentPeriod, currentEmployee);

            chart.Series[0].Points[currentPeriod].AxisLabel = (currentPeriod).ToString();

            
            simulationTimer.Start();
        }

        /// <summary>
        /// Обработчик тика таймера - основной цикл симуляции
        /// </summary
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
                ref currentAds,
                 ref currentEmployee,
                 ref currentQualityAnimalHusbandry
                );

            currentPeriod++;
            revenueData.Add(currentRevenue);
            serviceData.Add(currentService);
            investmentData.Add(currentInvestment);
            visitorData.Add(currentVisitors);
            adsData.Add(currentAds);
            infrastructureData.Add(currentInfrastructure);

            chart.Series[0].Points.AddXY(currentPeriod,currentRevenue);
            chart.Series[1].Points.AddXY(currentPeriod, currentService);
            chart.Series[2].Points.AddXY(currentPeriod, currentInvestment);
            chart.Series[3].Points.AddXY(currentPeriod, currentVisitors);
            chart.Series[4].Points.AddXY(currentPeriod, currentAds);
            chart.Series[5].Points.AddXY(currentPeriod, currentInfrastructure);
            chart.Series[6].Points.AddXY(currentPeriod, currentEmployee);
            chart.Series[0].Points[currentPeriod].AxisLabel = (currentPeriod).ToString();
        }

        /// <summary>
        /// Основная логика расчета показателей для каждого периода
        /// </summary>

        private void UpdateSimulationData(int period,
            ref double revenue,
            ref double service,
            ref double investment,
            ref int visitors,
            ref double infrastructure,
            ref double ads,
            ref int employee,
            ref double qualityAnimalHusbandry
            )
        {

            // Формулы расчета показателей:
            ads = revenue * 0.1; // Реклама - 10% от дохода

            // Расчет сотрудников
            int hiring = (int)((revenue * 0.3) / salaries);
            employee += hiring;
            double afterHiring = revenue * 0.3 - (int)((revenue * 0.3) / salaries);// остаток 


            qualityAnimalHusbandry = qualityAnimalHusbandry * 0.5 +  (qualityAnimalHusbandry * employee / hardWork) * 0.5;

            

            // Инфраструктура: 90% текущего значения + 10% от инвестиций
            infrastructure = infrastructure * 0.9 + investment * 0.1;


            // Расчет посетителей: ограничение максимум 1000, 
            // 95% от текущих + влияние инфраструктуры + рекламы + качество содержания животных
            double maxVisitors = 1000;
            visitors = (int)Math.Min(
                maxVisitors,
                visitors * 0.95 + infrastructure * 3 + ads * 0.1 + qualityAnimalHusbandry * 0.1
            );

            // Качество сервиса: зависит от посетителей и дохода
            service = visitors * 2.5 + revenue * 0.05;

            // Инвестиции: 30% от рекламы + 12% от дохода
            investment = ads * 0.3 + revenue * 0.12;

            // Доход: 40% от предыдущего + вклад посетителей и сервиса
            revenue = revenue * 0.4 + visitors * 2.5 + service * 0.4 + afterHiring;


            employee -= (int)Math.Min(3, (employee >= hardWork) ? 0 : hardWork - employee);
        }

        /// <summary>
        /// Очистка данных и графика перед новым запуском
        /// </summary>
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
