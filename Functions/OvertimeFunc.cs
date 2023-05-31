namespace OMS
{
    class OvertimeFunc
    {
        private readonly GeneralFunc general;
        public OvertimeFunc(GeneralFunc general)
        {
            this.general = general;
        }
        public double getValue()
        {
            double result = 0;
            for (int j = 1; j <= general.patientsCount; j++)
            {
                result += general.getIBeforeArrivalProb(general.totalIntervalCount + 1, j) * j / general.serviceRate;
            }
            return result;
        }
    }
}