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
            double result1, result2 = 0;
            int T = general.totalIntervalCount;

            for (int t = 1; t <= T; t++)
            {
                int Xt = general.regularPatientsCount[t - 1];
                int Yt = general.urgentPatientsCount[t - 1];
                int prevPatientsCount = general.getPatientsCountTill(t - 1);

                double result3 = 0;
                for (int k = 0; k <= Xt; k++)
                {
                    double result4 = 0;
                    for (int j = 1; j <= (k + Yt); j++)
                    {
                        double result5 = 0;
                        for (int i = 0; i <= prevPatientsCount; i++)
                        {
                            result5 += general.getIBeforeArrivalProb(t, i) * (i + j - 1) / general.serviceRate;
                        }
                        result4 += result5;
                    }
                    result3 += general.getComboShowUpProb(Xt, k) * result4;
                }
                result2 += result3;
            }
            result1 = result2 / general.expectedWorkload;
            return result1;
        }
    }
}