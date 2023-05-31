using System;
namespace OMS
{
    class OvertimeFunc
    {
        private readonly double noShowProb;
        private readonly int patientsCount;
        private readonly int totalIntervalCount;
        private readonly int[] regularPatientsCount;
        private readonly int[] urgentPatientsCount;
        private readonly double success;
        private readonly double serviceRate;
        private readonly double intervalLength;
        public OvertimeFunc(double noShowProb, int patientsCount, int totalIntervalCount,
                            int[] regularPatientsCount, int[] urgentPatientsCount,
                            double success, double serviceRate, double intervalLength)
        {
            this.noShowProb = noShowProb;
            this.patientsCount = patientsCount;
            this.totalIntervalCount = totalIntervalCount;
            this.regularPatientsCount = regularPatientsCount;
            this.urgentPatientsCount = urgentPatientsCount;

            this.success = success;
            this.serviceRate = serviceRate;
            this.intervalLength = intervalLength;
        }
        private double getFactorialValue(double value)
        {
            if (value >= 0)
            {
                bool condition = value == 0 ? true : false;
                if (condition)
                {
                    return 1;
                }
                return getFactorialValue(value - 1) * value;
            }
            return 0;
        }

        private double getProbability(int value)
        {
            double muD = serviceRate * intervalLength;
            double factorial = getFactorialValue(value);

            double result = Math.Pow(muD, value) * Math.Exp(-muD) / factorial;
            return result;
        }
        private double getCombinationValue(double n, double r)
        {
            return getFactorialValue(n) / (getFactorialValue(n - r) * getFactorialValue(r));
        }
        private double getSuccessProb(int Xt, int k)
        {
            return getCombinationValue(Xt, k) * Math.Pow((1 - noShowProb), k) * Math.Pow(noShowProb, (Xt - k));
        }
        private int getPatientsCountTill(int t)
        {
            int count = 0;
            for (int i = 0; i < t; i++)
            {
                count += regularPatientsCount[i] + urgentPatientsCount[i];
            }
            return count;
        }
        private double getIAfterArrivalProb(int t, int i)
        {
            double result = 0;
            if (t > 0)
            {
                int Xt = regularPatientsCount[t - 1];
                int Yt = urgentPatientsCount[t - 1];

                if (i <= getPatientsCountTill(t) && i >= Yt)
                {
                    for (int k = 0; k <= Xt; k++)
                    {
                        result += getSuccessProb(Xt, k) * getIBeforeArrivalProb(t, i - Yt - k);
                    }
                }
            }
            return result;
        }
        private double getIBeforeArrivalProb(int t, int i)
        {
            if (t == 1 && i == 0)
            {
                return 1;
            }
            double result = 0;
            for (int j = i; j <= patientsCount; j++)
            {
                result += getIAfterArrivalProb(t - 1, j) * getProbability(j - i);
            }
            return result;
        }
        public double getValue()
        {
            double result = 0;
            for (int j = 1; j <= patientsCount; j++)
            {
                result += getIBeforeArrivalProb(totalIntervalCount + 1, j) * j / serviceRate;
            }
            return result;
        }
    }
}