using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace fuzzySet
{
    class Set : ICloneable
    {
        public List<List<double>> slices = new List<List<double>>();

        public Set() { }

        public object Clone()
        {
            List<List<double>> Slices = new List<List<double>>(this.slices);
            return new Set
            {
                slices = Slices
            };
        }

        public void Add(List<List<double>> slices)
        {
            this.slices = slices;
            SliceComparer Comparer = new SliceComparer();
            this.slices.Sort(Comparer);
        }

        private void Add(List<double> slice)
        {
            this.slices.Add(slice);
            SliceComparer Comparer = new SliceComparer();
            this.slices.Sort(Comparer);
        }


        class SliceComparer : IComparer<List<double>>
        {
            public int Compare(List<double> slice1, List<double> slice2)
            {
                if (slice1[0] > slice2[0])
                {
                    return 1;
                }
                else if (slice1[0] < slice2[0])
                {
                    return -1;
                }

                return 0;
            }
        }

        private List<double> SliceCalculate(double a)
        {

            List<double> less = new List<double>();
            List<double> bigger = new List<double>();
            for (int i = 0; i < slices.Count; i++)
            {
                if (slices[i][0] > a)
                {
                    bigger = slices[i];
                    less = slices[i - 1];
                    break;
                }
            }

            double lower = ((a - less[0]) * (bigger[1] - less[1])) / (bigger[0] - less[0]) + less[1];
            double upper = ((a - less[0]) * (bigger[2] - less[2])) / (bigger[0] - less[0]) + less[2];
            List<double> res = new List<double>();
            res.Add(a);
            res.Add(lower);
            res.Add(upper);
            return res;
        }

        public bool CheckSlice0()
        {
            for (int i = 0; i < this.slices.Count(); i++)
            {
                if (this.slices[i][0] == 0)
                    return true;
            }
            return false;
        }

        public bool CheckSlice1()
        {
            for (int i = 0; i < this.slices.Count(); i++)
            {
                if (this.slices[i][0] == 1)
                    return true;
            }
            return false;
        }

        public bool CheckSliceNoRpeat()
        {
            for (int i = 0; i < this.slices.Count() - 1; i++)
                if (this.slices[i][0] == this.slices[i + 1][0])
                    return false;
            return true;
        }

        public bool CheckSliceInterval()
        {
            for (int i = 0; i < this.slices.Count(); i++)
                if (this.slices[i][0] < 0 || this.slices[i][0] > 1)
                    return false;
            return true;
        }

        public bool CheckNull()
        {
            if (this.slices.Count() == 0)
            {
                return false;
            }
            return true;
        }
        public bool CheckConvexity()
        {
            for (int i = 0; i < this.slices.Count() - 1; i++)
            {
                if (this.slices[i][1] >= this.slices[i + 1][1])
                    return false;
            }

            if (this.slices[this.slices.Count() - 1][1] > this.slices[this.slices.Count() - 1][2])
                return false;

            for (int i = this.slices.Count() - 1; i > 0; i--)
            {
                if (this.slices[i][2] >= this.slices[i - 1][2])
                    return false;
            }
            return true;
        }

        private bool Contain(List<double> slice)
        {
            for (int i = 0; i < this.slices.Count(); i++)
            {
                if (slice[0] == this.slices[i][0])
                    return true;
            }
            return false;
        }

        public Set Plus(Set B)
        {
            Set tempA = new Set();
            tempA = (Set)this.Clone();

            Set tempB = new Set();
            tempB = (Set)B.Clone();

            foreach (var slice in tempA.slices)
            {
                if (!tempB.Contain(slice))
                    tempB.Add(tempB.SliceCalculate(slice[0]));
            }

            foreach (var slice in tempB.slices)
            {
                if (!tempA.Contain(slice))
                    tempA.Add(tempA.SliceCalculate(slice[0]));
            }

            Set Res = new Set();

            for (int i = 0; i < tempA.slices.Count; i++)
            {
                List<double> slice = new List<double>();
                slice.Add(tempA.slices[i][0]);
                slice.Add(tempA.slices[i][1] + tempB.slices[i][1]);
                slice.Add(tempA.slices[i][2] + tempB.slices[i][2]);
                Res.slices.Add(slice);
            }

            return Res;
        }

        public Set Multiply(Set B)
        {
            Set tempA = new Set();
            tempA = (Set)this.Clone();

            Set tempB = new Set();
            tempB = (Set)B.Clone();

            foreach (var slice in tempA.slices)
            {
                if (!tempB.Contain(slice))
                    tempB.Add(tempB.SliceCalculate(slice[0]));
            }

            foreach (var slice in tempB.slices)
            {
                if (!tempA.Contain(slice))
                    tempA.Add(tempA.SliceCalculate(slice[0]));
            }

            Set Res = new Set();

            for (int i = 0; i < tempA.slices.Count; i++)
            {
                List<double> slice = new List<double>();
                slice.Add(tempA.slices[i][0]);
                slice.Add(tempA.slices[i][1] * tempB.slices[i][1]);
                slice.Add(tempA.slices[i][2] * tempB.slices[i][2]);
                Res.slices.Add(slice);
            }

            return Res;
        }

        public Set Minus(Set B)
        {
            Set tempA = new Set();
            tempA = (Set)this.Clone();

            Set tempB = new Set();
            tempB = (Set)B.Clone();

            foreach (var slice in tempA.slices)
            {
                if (!tempB.Contain(slice))
                    tempB.Add(tempB.SliceCalculate(slice[0]));
            }

            foreach (var slice in tempB.slices)
            {
                if (!tempA.Contain(slice))
                    tempA.Add(tempA.SliceCalculate(slice[0]));
            }

            Set Res = new Set();

            for (int i = 0; i < tempA.slices.Count; i++)
            {
                List<double> slice = new List<double>();
                slice.Add(tempA.slices[i][0]);
                slice.Add(tempA.slices[i][1] - tempB.slices[i][2]);
                slice.Add(tempA.slices[i][2] - tempB.slices[i][1]);
                Res.slices.Add(slice);
            }

            return Res;
        }

        public Set Division(Set B)
        {
            Set tempA = new Set();
            tempA = (Set)this.Clone();

            Set tempB = new Set();
            tempB = (Set)B.Clone();

            foreach (var slice in tempA.slices)
            {
                if (!tempB.Contain(slice))
                    tempB.Add(tempB.SliceCalculate(slice[0]));
            }

            foreach (var slice in tempB.slices)
            {
                if (!tempA.Contain(slice))
                    tempA.Add(tempA.SliceCalculate(slice[0]));
            }

            Set Res = new Set();

            for (int i = 0; i < tempA.slices.Count; i++)
            {
                List<double> slice = new List<double>();
                slice.Add(tempA.slices[i][0]);
                if (tempB.slices[i][2] != 0)
                    slice.Add(tempA.slices[i][1] / tempB.slices[i][2]);
                else
                {
                    MessageBox.Show("Попытка деления на 0", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    for (int k = 0; k < Res.slices.Count(); k++)
                    {
                        Res.slices[k].Clear();
                    }
                    return Res;
                }
                if (tempB.slices[i][1] != 0) slice.Add(tempA.slices[i][2] / tempB.slices[i][1]);
                else
                {
                    MessageBox.Show("Попытка деления на 0", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    for (int k = 0; k < Res.slices.Count(); k++)
                    {
                        Res.slices[k].Clear();
                    }
                    return Res;
                }
                Res.slices.Add(slice);
            }

            return Res;
        }

        public string Compare(Set B)
        {
            double summA = 0;
            double summB = 0;

            for (int i = 0; i < this.slices.Count; i++)
            {
                summA += this.slices[i][1] + this.slices[i][2];
            }
            summA /= this.slices.Count;

            for (int i = 0; i < B.slices.Count; i++)
            {
                summB += B.slices[i][1] + B.slices[i][2];
            }
            summB /= B.slices.Count;

            if (summA > summB) return "А > В";
            else if (summA < summB) return "А < В";
            else if (summA == summB) return "А = B";

            return null;
        }
    }
}
