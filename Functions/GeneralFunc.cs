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
        // FACTORIAL OF A NUMBER
        public double getFactorialValue(double value)
        {
            if (value == 0)
            {
                return 1;
            }
            return value * getFactorialValue(value - 1);
        }
        // VALUE OF C(n, r) = n! / ((n-r)! * r!)
        public double getCombinationValue(double n, double r)
        {
            return getFactorialValue(n) / (getFactorialValue(n - r) * getFactorialValue(r));
        }
        // PROBABILITY THAT THERE ARE i DEPARTURES DURING AN INTERVAL
        public double getIDepartureProb(int i)
        {
            double muD = serviceRate * intervalLength;
            double factorial = getFactorialValue(i);

            double result = Math.Pow(muD, i) * Math.Exp(-muD) / factorial;
            return result;
        }
        // PROBABILITY THAT THERE ARE NO FEWER THAN i DEPARTURES DURING AN INTERVAL
        public double getIOrMoreDepartProb(int i)
        {
            double result = 0;
            for (int j = 0; j < i; j++)
            {
                result += getIDepartureProb(j);
            }
            return 1 - result;
        }
        // NUMBER OF PATIENTS SCHEDULED BY INTERVAL t
        public int getPatientsCountTill(int t)
        {
            int count = 0;
            for (int i = 0; i < t; i++)
            {
                count += regularPatientsCount[i] + urgentPatientsCount[i];
            }
            return count;
        }
        // PROBABILITY THAT k PATIENTS SHOW UP OUT OF Xt PATIENTS
        public double getComboShowUpProb(int Xt, int k)
        {
            return getCombinationValue(Xt, k) * Math.Pow((1 - noShowProb), k) * Math.Pow(noShowProb, (Xt - k));
        }
        // PROBABILITY THAT THERE ARE i PATIENTS IN QUEUE JUST AFTER THE ARRIVAL AT THE INTERVAL t
        public double getIAfterArrivalProb(int t, int i)
        {
            double result = 0;

            int Xt = regularPatientsCount[t - 1];
            int Yt = urgentPatientsCount[t - 1];

            if (i >= 0 && i < Yt)
            {
                return 0;
            }

            if (i <= getPatientsCountTill(t) && i >= Yt)
            {
                for (int k = 0; k <= Xt; k++)
                {
                    result += getComboShowUpProb(Xt, k) * getIBeforeArrivalProb(t, i - Yt - k);
                }
            }
            return result;
        }
        // PROBABILITY THAT THERE ARE i PATIENTS IN QUEUE JUST BEFORE THE ARRIVAL AT THE INTERVAL t
        public double getIBeforeArrivalProb(int t, int i)
        {
            double result = 0;
            if (i == 0)
            {
                if (t == 1)
                {
                    return 1;
                }
                int count = getPatientsCountTill(t - 1);

                // FOR t WE NEED YtMinus1 AND FOR YtMinus1 ARRAY INDEX = t - 2
                int Yt = urgentPatientsCount[t - 2];

                for (int j = Yt; j <= count; j++)
                {
                    result += getIAfterArrivalProb(t - 1, j) * getIOrMoreDepartProb(j);
                }
            }
            else if (i >= 1)
            {
                int St = getPatientsCountTill(t - 1);

                for (int j = i; j <= St; j++)
                {
                    result += getIAfterArrivalProb(t - 1, j) * getIDepartureProb(j - i);
                }
            }
            return result;
        }
    }
}