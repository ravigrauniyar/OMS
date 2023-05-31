namespace OMS
{
    class Program
    {
        public static void Main(string[] args)
        {
            double serviceRate = 0.1;
            int intervalLength = 15;

            double noShowProb = 0.2;
            int patientsCount = 7;
            int totalIntervalCount = 10;

            double N = 4; double M = 3;

            double expectedWorkload = ((1 - noShowProb) * N + M) / serviceRate;

            Console.WriteLine($"\nAverage service time = {1 / serviceRate} minutes" +
                                $"\n\nScheduled time per slot is {intervalLength} minutes\n\n" +
                                $"Number of slots per day = {totalIntervalCount}\n");

            int[] regularPatientsCount = { 0, 1, 0, 0, 0, 3, 0, 0, 0, 0 };

            int[] urgentPatientsCount = { 1, 0, 0, 0, 0, 1, 0, 1, 0, 0 };

            GeneralFunc genFunction = new GeneralFunc(
                noShowProb, patientsCount, regularPatientsCount,
                urgentPatientsCount, serviceRate, intervalLength, totalIntervalCount, expectedWorkload
            );

            OvertimeFunc ot = new OvertimeFunc(genFunction);

            string overtimeValue = ot.getValue().ToString("0.000");

            Console.WriteLine($"\nOvertime when {patientsCount} patients are scheduled in {totalIntervalCount} " +
                                $"intervals = {overtimeValue} minutes.\n");

            IdleTimeFunc it = new IdleTimeFunc(genFunction);

            string idleTimeValue = it.getValue().ToString("0.000");

            Console.WriteLine($"\nIdletime when {patientsCount} patients are scheduled in {totalIntervalCount} " +
                                $"intervals = {idleTimeValue} minutes.\n");
        }
    }
}