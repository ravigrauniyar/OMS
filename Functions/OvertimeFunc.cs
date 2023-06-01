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
            for (int i = 1; i <= general.patientsCount; i++)
            {
                result += general.getIBeforeArrivalProb(general.totalIntervalCount + 1, i) * i / general.serviceRate;
            }
            return result;
        }
    }
}