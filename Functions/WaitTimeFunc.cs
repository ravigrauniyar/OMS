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
            double result1 = 0, result2 = 0, result3 = 0, result4 = 0, result5 = 0;
            int T = general.totalIntervalCount;
            
            for (int t = 1; t <= T; t++)
            {
                int Xt = general.regularPatientsCount[t - 1];
                int Yt = general.urgentPatientsCount[t - 1];

                for (int k = 0; k <= Xt; k++)
                {
                    for (int j = 1; j <= (k + Yt); j++)
                    {
                        int prevPatientsCount = general.getPatientsCountTill(t - 1);

                        for (int i = 1; i <= prevPatientsCount; i++)
                        {
                            result5 += general.getIBeforeArrivalProb(t, i) * (i + j - 1) / general.serviceRate;
                        }
                        result4 += result5;
                    }
                    result3 += general.getSuccessProb(Xt, k) * result4;
                }
                result2 += result3;
            }
            result1 = result2 / general.expectedWorkload * general.serviceRate;
            return result1;
        }
    }
}