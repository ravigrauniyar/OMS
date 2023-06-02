namespace OMS
{
    class WaitTimeFunc
    {
        private readonly GeneralFunc general;
        public WaitTimeFunc(GeneralFunc general)
        {
            this.general = general;
        }
        public double getValue()
        {
            double result1 = 0;
            int T = general.totalIntervalCount;

            for (int t = 1; t <= T; t++)
            {
                int Xt = general.regularPatientsCount[t - 1];
                int Yt = general.urgentPatientsCount[t - 1];

                int prevPatientsCount = general.getPatientsCountTill(t - 1);

                double result2 = 0;

                for (int k = 0; k <= Xt; k++)
                {
                    double result3 = 0;

                    for (int j = 1; j <= (k + Yt); j++)
                    {
                        double result4 = 0;

                        for (int i = 0; i <= prevPatientsCount; i++)
                        {
                            result4 += general.getIBeforeArrivalProb(t, i) * (i + j - 1) / general.serviceRate;
                        }
                        result3 += result4;
                    }
                    result2 += general.getComboShowUpProb(Xt, k) * result3;
                }
                result1 += result2;
            }
            return result1 / general.expectedWorkload;
        }
    }
}