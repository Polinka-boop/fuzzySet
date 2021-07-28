using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace fuzzySet
{
    public partial class Form1 : Form
    {
        Set A;
        Set B;
        Set C;
        bool doNotRun = true;

        public Form1()
        {
            InitializeComponent();

            foreach (DataGridViewColumn column in dataGridViewA.Columns)
            {
                column.ValueType = typeof(Double);
            }

            foreach (DataGridViewColumn column in dataGridViewB.Columns)
            {
                column.ValueType = typeof(Double);
            }

            dataGridViewA.Rows.Add(0, 1, 9);
            dataGridViewA.Rows.Add(0.5, 2, 8);
            dataGridViewA.Rows.Add(1, 3, 4);

            dataGridViewB.Rows.Add(0, 1, 9);
            dataGridViewB.Rows.Add(0.5, 3, 6);
            dataGridViewB.Rows.Add(1, 4, 5);
            dataGridViewB.Rows.Add(0.2, 2, 7);

            A = new Set();
            B = new Set();
            C = new Set();

            ReadSets(dataGridViewA, A);
            ReadSets(dataGridViewB, B);

            //if (!(A.CheckConvexity() && B.CheckConvexity()))
            //{
            //    //message box and break
            //}            
            doNotRun = false;
            chart1.Series.Clear();
        }

        private void ReadSets(DataGridView dataGrid, Set set)
        {
            List<List<double>> slices = new List<List<double>>();

            for (int rows = 0; rows < dataGrid.Rows.Count - 1; rows++)
            {
                List<double> row = new List<double>();
                for (int col = 0; col < dataGrid.Rows[rows].Cells.Count; col++)
                {
                    if (dataGrid.Rows[rows].Cells[col].Value != null)
                    {
                        Console.Write("okidoki" + " ");
                        row.Add(Convert.ToDouble(dataGrid.Rows[rows].Cells[col].Value));
                    }
                    else
                    {
                        Console.WriteLine("oi");
                        return;
                    }
                }
                if (row.Count != 3)
                {
                    Console.WriteLine("oi");
                    return;
                }
                else
                {
                    slices.Add(row);
                }
                Console.WriteLine();
            }
            set.Add(slices);

        }

        private bool Check()
        {
            if (!A.CheckNull())
            {
                MessageBox.Show("Введите ножество A", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (!B.CheckNull())
            {
                MessageBox.Show("Введите ножество B", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (!(A.CheckSliceNoRpeat()))
            {
                MessageBox.Show("Срезы в множестве А не должны повторяться", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (!(B.CheckSliceNoRpeat()))
            {
                MessageBox.Show("Срезы в множестве B не должны повторяться", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (!(A.CheckSliceInterval()))
            {
                MessageBox.Show("Срезы в множестве A должны быть в интерале [0, 1]", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (!(B.CheckSliceInterval()))
            {
                MessageBox.Show("Срезы в множестве B должны быть в интерале [0, 1]", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (!A.CheckSlice0())
            {
                MessageBox.Show("Введите срез 0 в множестве A", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (!A.CheckSlice1())
            {
                MessageBox.Show("Введите срез 1 в множестве A", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (!B.CheckSlice0())
            {
                MessageBox.Show("Введите срез 0 в множестве B", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (!B.CheckSlice1())
            {
                MessageBox.Show("Введите срез 1 в множестве B", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (!(A.CheckConvexity()))
            {
                MessageBox.Show("Множество А не прошло проверку на выпуклось", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (!(B.CheckConvexity()))
            {
                MessageBox.Show("Множество B не прошло проверку на выпуклось", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }


        private void buttonPlus_Click(object sender, EventArgs e)
        {
            dataGridViewResult.Rows.Clear();
            textBoxCompareResult.Clear();
            if (!Check()) return;
            C = null;
            chart1.Series.Clear();

            C = A.Plus(B);
            OutputREsult();
            Chart();
        }

        private void buttonMinus_Click(object sender, EventArgs e)
        {
            dataGridViewResult.Rows.Clear();
            textBoxCompareResult.Clear();
            if (!Check()) return;
            C = null;
            chart1.Series.Clear();

            C = A.Minus(B);
            OutputREsult();
            Chart();
        }

        private void buttonDivision_Click(object sender, EventArgs e)
        {
            dataGridViewResult.Rows.Clear();
            textBoxCompareResult.Clear();
            if (!Check()) return;
            C = null;
            chart1.Series.Clear();

            C = A.Division(B);
            OutputREsult();
            Chart();
        }

        private void buttonMultiply_Click(object sender, EventArgs e)
        {
            dataGridViewResult.Rows.Clear();
            textBoxCompareResult.Clear();
            if (!Check()) return;
            C = null;
            chart1.Series.Clear();

            C = A.Multiply(B);
            OutputREsult();
            Chart();
        }

        private void buttonCompare_Click(object sender, EventArgs e)
        {
            if (!Check()) return;
            textBoxCompareResult.Text = A.Compare(B);
            Chart();
        }


        private void Chart()
        {
            chart1.Series.Clear();
            chart1.Series.Add("A");
            chart1.Series[0].Points.Clear();
            chart1.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            chart1.Series[0].BorderWidth = 3;

            chart1.Series.Add("B");
            chart1.Series[1].Points.Clear();
            chart1.Series[1].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            chart1.Series[1].BorderWidth = 3;

            for (int i = 0; i < A.slices.Count(); i++)
                chart1.Series[0].Points.AddXY(A.slices[i][1], A.slices[i][0]);
            for (int i = A.slices.Count() - 1; i >= 0; i--)
                chart1.Series[0].Points.AddXY(A.slices[i][2], A.slices[i][0]);


            for (int i = 0; i < B.slices.Count(); i++)
                chart1.Series[1].Points.AddXY(B.slices[i][1], B.slices[i][0]);
            for (int i = B.slices.Count() - 1; i >= 0; i--)
                chart1.Series[1].Points.AddXY(B.slices[i][2], B.slices[i][0]);

            if (C.slices.Count() != 0)
            {
                chart1.Series.Add("C");
                chart1.Series[2].Points.Clear();
                chart1.Series[2].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                chart1.Series[2].BorderWidth = 3;

                for (int i = 0; i < C.slices.Count(); i++)
                    chart1.Series[2].Points.AddXY(C.slices[i][1], C.slices[i][0]);

                for (int i = C.slices.Count() - 1; i >= 0; i--)
                    chart1.Series[2].Points.AddXY(C.slices[i][2], C.slices[i][0]);

            }

        }
        private void OutputREsult()
        {
            for (int rows = 0; rows < C.slices.Count(); rows++)
            {
                if (dataGridViewResult.Rows.Count <= rows + 1) dataGridViewResult.Rows.Add();
                for (int cols = 0; cols < C.slices[rows].Count(); cols++)
                {
                    dataGridViewResult.Rows[rows].Cells[cols].Value = C.slices[rows][cols];
                }
            }
        }

        private void dataGridViewA_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            MessageBox.Show($"Невероное значение в ячейке {e.RowIndex + 1}, {e.ColumnIndex + 1}" +
                $" в таблице А. Введите число!");
        }

        private void dataGridViewB_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            MessageBox.Show($"Невероное значение в ячейке {e.RowIndex + 1}, {e.ColumnIndex + 1}" +
                $" в таблице В. Введите число!");
        }

        private void dataGridViewA_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (doNotRun)
                return;
            else
                ReadSets(dataGridViewA, A);
        }

        private void dataGridViewB_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (doNotRun)
                return;
            else
                ReadSets(dataGridViewB, B);
        }

        private void dataGridViewA_UserDeletedRow(object sender, DataGridViewRowEventArgs e)
        {
            ReadSets(dataGridViewA, A);
            dataGridViewResult.Rows.Clear();
            chart1.Series.Clear();
            textBoxCompareResult.Clear();
        }

        private void dataGridViewB_UserDeletedRow(object sender, DataGridViewRowEventArgs e)
        {
            ReadSets(dataGridViewB, B);
            dataGridViewResult.Rows.Clear();
            chart1.Series.Clear();
            textBoxCompareResult.Clear();
        }
    }
}
