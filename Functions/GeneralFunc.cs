namespace OMS
{
    class GeneralFunc
    {
        public readonly double noShowProb;
        public readonly int patientsCount;
        public readonly int[] regularPatientsCount;
        public readonly int[] urgentPatientsCount;
        public readonly double serviceRate;
        public readonly int intervalLength;
        public readonly int totalIntervalCount;
        public readonly double expectedWorkload;
        public GeneralFunc(
                double noShowProb, int patientsCount, int[] regularPatientsCount, int[] urgentPatientsCount,
                double serviceRate, int intervalLength, int totalIntervalCount, double expectedWorkload
            )
        {
            this.noShowProb = noShowProb;
            this.regularPatientsCount = regularPatientsCount;
            this.urgentPatientsCount = urgentPatientsCount;
            this.intervalLength = intervalLength;
            this.patientsCount = patientsCount;
            this.serviceRate = serviceRate;
            this.totalIntervalCount = totalIntervalCount;
            this.expectedWorkload = expectedWorkload;
        }
        public double getFactorialValue(double value)
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

        public double getProbability(int value)
        {
            double muD = serviceRate * intervalLength;
            double factorial = getFactorialValue(value);

            double result = Math.Pow(muD, value) * Math.Exp(-muD) / factorial;
            return result;
        }
        public double getCombinationValue(double n, double r)
        {
            return getFactorialValue(n) / (getFactorialValue(n - r) * getFactorialValue(r));
        }
        public double getSuccessProb(int Xt, int k)
        {
            return getCombinationValue(Xt, k) * Math.Pow((1 - noShowProb), k) * Math.Pow(noShowProb, (Xt - k));
        }
        public int getPatientsCountTill(int t)
        {
            int count = 0;
            for (int i = 0; i < t; i++)
            {
                count += regularPatientsCount[i] + urgentPatientsCount[i];
            }
            return count;
        }
        public double getIAfterArrivalProb(int t, int i)
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
        public double getIBeforeArrivalProb(int t, int i)
        {
            int count = 0;
            if (t == 1 && i == 0)
            {
                return 1;
            }
            double result = 0;
            for (int j = i; j <= patientsCount; j++)
            {
                result += getIAfterArrivalProb(t - 1, j) * getProbability(j - i);
                count++;
            }
            return result;
        }
    }
}