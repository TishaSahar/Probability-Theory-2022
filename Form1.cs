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

namespace pr_teory___lab1
{
    public partial class Form1 : Form
    {
        double Prob = 0.0, Delta = 0.0;
        double L, T;

        double maxDisp;

        int n = 0;
        int[] Xj;
        int[] val;
        double[] F;
        double[] expF;

        public Form1()
        {
            maxDisp = 0.0;
            InitializeComponent();
            n = Convert.ToInt32(textBox5.Text);
            Xj = new int[32000];
            val = new int[n];
            expF = new double[n];
            F = new double[n];
            for (int i = 0; i < 32000; i++) Xj[i] = -1;
            for (int i = 0; i < n; i++) val[i] = 0;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private double Probability(int k)
        {
            double P = Math.Pow(L * T, k) * Math.Exp(-L * T);
            for (int i = 1; i <= k; i += 1) P /= (double)i;
            return P;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 32000; i++)
                Xj[i] = -1;


            dataGridView1.Columns.Clear();
            n = Convert.ToInt32(textBox5.Text);

            val = new int[n];
            F = new double[n];
            expF = new double[n];

            double x;


            L = Convert.ToDouble(textBox1.Text);
            T = Convert.ToDouble(textBox2.Text);
            double Lambda = L * T;

            Random rnd = new Random();
            for (int j = 0; j < n; j++)
            {
                x = (double)rnd.Next(0, 32000) / (32000.0);

                Prob = Math.Exp(-Lambda);
                Delta = Prob;

                int i = 0;
                while (Delta < x)
                {
                    i++;
                    Prob /= (double)i;
                    Prob *= Lambda;
                    Delta += Prob;
                }
                expF[j] = Delta;
                if (Xj[i] == -1) Xj[i]++;
                Xj[i]++;
            }

            int upperNull = 0;
            for (int i = 0; i < 32000; i++)
                if (Xj[i] >= 0)
                {
                    upperNull++;
                }



            dataGridView1.RowCount = 3;
            dataGridView1.ColumnCount = upperNull + 1;
            int m = 0;
            for (int j = 0; j < 32000; j++)
            {
                if (Xj[j] >= 0)
                {
                    dataGridView1.Columns[m].Name = Convert.ToString(j);
                    dataGridView1.Rows[0].Cells[m].Value = Convert.ToString(Xj[j]);
                    dataGridView1.Rows[1].Cells[m].Value = Convert.ToString(Probability(j));
                    dataGridView1.Rows[2].Cells[m].Value = Convert.ToString((double)Xj[j] / (double)n);

                    if (Math.Abs(Probability(j) - (double)Xj[j] / (double)n) > maxDisp) maxDisp = Math.Abs(Probability(j) - (double)Xj[j] / (double)n);

                    m++;
                }
            }

            Table2();
        }

        private void Table2()
        {
            double En, midX = 0.0, Dn = 0.0, S2 = 0.0, Me = 0.0, midR = 0.0;
            // Математическое ожидание
            En = L * T;
            // Дисперсия
            Dn = L * T;

            int j = 0;

            for (int i = 0; i < 32000; i++)
            {
                if (Xj[i] >= 0)
                {
                    for (int k = 0; k < Xj[i]; k++)
                        val[j + k] = i;
                    j += Xj[i];
                }
            }

            // Размах выборки
            midR = val[j - 1] - val[0];

            // Выборочное среднее
            for (int i = 0; i < n; i++)
            {
                midX += val[i];
            }

            midX = (double)midX / (double)(n);

            // Выборочная дисперсия
            for (int i = 0; i < n; i++)
            {
                S2 += (val[i] - midX) * (val[i] - midX);
            }
            S2 = (double)S2 / (double)(n);

            // Медиана

            if (n % 2 == 0)
                Me = (double)(val[n / 2] + val[n / 2 + 1]) / 2;
            else
                Me = (double)val[n / 2] / 2;

            dataGridView2.RowCount = 2;
            dataGridView2.ColumnCount = 9;

            dataGridView2.Rows[0].Cells[0].Value = "En";                
            dataGridView2.Rows[1].Cells[0].Value = Convert.ToString(En);
            dataGridView2.Rows[0].Cells[1].Value = "middle X";          
            dataGridView2.Rows[1].Cells[1].Value = Convert.ToString(midX);
            dataGridView2.Rows[0].Cells[2].Value = "|En - middle X|";   
            dataGridView2.Rows[1].Cells[2].Value = Convert.ToString(Math.Abs(En - midX));
            dataGridView2.Rows[0].Cells[3].Value = "Dn";                
            dataGridView2.Rows[1].Cells[3].Value = Convert.ToString(Dn);
            dataGridView2.Rows[0].Cells[4].Value = "S^2";               
            dataGridView2.Rows[1].Cells[4].Value = Convert.ToString(S2);
            dataGridView2.Rows[0].Cells[5].Value = "|Dn - S^2|";        
            dataGridView2.Rows[1].Cells[5].Value = Convert.ToString(Math.Abs(Dn - S2));
            dataGridView2.Rows[0].Cells[6].Value = "Me";                
            dataGridView2.Rows[1].Cells[6].Value = Convert.ToString(Me);
            dataGridView2.Rows[0].Cells[7].Value = "R";                 
            dataGridView2.Rows[1].Cells[7].Value = Convert.ToString(midR);

            dataGridView2.Rows[0].Cells[8].Value = "max|Pj - nj/n|";
            dataGridView2.Rows[1].Cells[8].Value = Convert.ToString(maxDisp);

            drawChart();
        }

        double Fn(int x)
        {               
            double F = 0.0;
            int i = 0;
            while (val[i] < x)
            { 
                F += Probability(val[i]);
                i++;
                if (i == n) break;
            }

            F /= (double)n;
            return F;
        }

        private void drawChart()
        {
            Array.Sort(val);
            double FuckYou = Probability(val[0]);
            F[0] = Probability(val[0]);
            for(int i = 1; i < n; i ++)
            {
                if (val[i] != val[i - 1])
                    FuckYou += Probability(val[i]);
                F[i] = FuckYou;
            }
            Array.Sort(expF);

            double[] _F = new double[n + 2];
            double[] _eF = new double[n + 2];
            int[] _val = new int[n + 2];
            _F[0] = F[0];
            _eF[0] = expF[0];
            _val[0] = val[0] - 1;

            for (int i = 1; i < n + 1; i ++)
            {
                _F[i] = F[i - 1];
                _eF[i] = expF[i - 1];
                _val[i] = val[i - 1];
            }

            _F[n + 1] = 1;
            _eF[n + 1] = 1;
            _val[n + 1] = val[n - 1] + 1;

            // добавим серию для теоритической Fn
            chart1.Series.Clear();
            chart1.ChartAreas.Clear();

            chart1.ChartAreas.Add(new ChartArea("first"));
            chart1.Series.Add(new Series("Fn"));
            chart1.Series["Fn"].ChartArea = "first";
            chart1.Series["Fn"].ChartType = SeriesChartType.StepLine;

            chart1.Series["Fn"].Points.DataBindXY(_val, _F);
            chart1.ChartAreas[0].AxisX.Interval = 10;

            chart1.Series.Add(new Series("expFn"));
            chart1.Series["expFn"].ChartArea = "first";
            chart1.Series["expFn"].ChartType = SeriesChartType.StepLine;
            chart1.Series["expFn"].Points.DataBindXY(_val, _eF);

            chart1.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
            chart1.ChartAreas[0].AxisX.MinorGrid.Enabled = false;
            chart1.ChartAreas[0].AxisY.MajorGrid.Enabled = false;
            chart1.ChartAreas[0].AxisY.MinorGrid.Enabled = false;

        }


        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            L = Convert.ToDouble(textBox1.Text);
            T = Convert.ToDouble(textBox2.Text);

            Random rnd = new Random();
            double x = (double)rnd.Next(0, 32000) / 32000.0;

            double LT = L * T;
            double k = 1.0;

            Prob = Math.Exp(-LT);
            Delta = Prob;


            int i = 0;
            while (Delta < x)
            {
                i++;
                k = LT / (double)i;
                Prob *= k;
                Delta += Prob;
            }

            textBox3.Text = Convert.ToString(x);
            textBox4.Text = Convert.ToString(i);
        }
    }
}
