namespace OMS
{
    class IdleTimeFunc
    {
        private readonly GeneralFunc general;
        public IdleTimeFunc(GeneralFunc general)
        {
            this.general = general;
        }
        private int getTmax(int T)
        {
            int Tmax = 0;
            for (int t = 0; t < T; t++)
            {
                if (general.regularPatientsCount[t] + general.urgentPatientsCount[t] > 0)
                {
                    Tmax = t;
                }
            }
            return Tmax + 1;
        }
        public double getValue()
        {
            int Tmax = getTmax(general.totalIntervalCount);

            double result1 = ((Tmax - 1) * general.intervalLength) - (general.expectedWorkload / general.serviceRate);

            double result2 = 0;

            int Xtmax = general.regularPatientsCount[Tmax - 1];

            int Ytmax = general.urgentPatientsCount[Tmax - 1];

            for (int k = 0; k <= Xtmax; k++)
            {
                int count = general.getPatientsCountTill(Tmax - 1);
                double result3 = 0;

                for (int i = 0; i <= count; i++)
                {
                    result3 += general.getIBeforeArrivalProb(Tmax, i) * (i + k + Ytmax) / general.serviceRate;
                }
                result2 += general.getIDepartureProb(k) * result3;
            }
            double result = result1 + result2;

            return (result < 0) ? -result : result;
        }
    }
}