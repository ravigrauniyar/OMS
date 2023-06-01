namespace OMS
{
    class Program
    {
        public static void Main(string[] args)
        {
            double serviceRate = 0.06667;
            int intervalLength = 15;

            double noShowProb = 0.1;
            int patientsCount = 10;
            int totalIntervalCount = 5;

            double N = 6; double M = 4;

            double expectedWorkload = ((1 - noShowProb) * N + M) / serviceRate;

            Console.WriteLine($"\nAverage service time = {1 / serviceRate} minutes" +
                                $"\n\nScheduled time per slot is {intervalLength} minutes\n\n" +
                                $"Number of slots per day = {totalIntervalCount}\n");

            int[] urgentPatientsCount = { 1, 2, 1, 2, 0 };

            int[] regularPatientsCount = { 1, 0, 1, 2, 0 };


            GeneralFunc genFunction = new GeneralFunc(
                noShowProb, patientsCount, regularPatientsCount,
                urgentPatientsCount, serviceRate, intervalLength, totalIntervalCount, expectedWorkload
            );

            OvertimeFunc ot = new OvertimeFunc(genFunction);

            double overtimeValue = ot.getValue();
            string overtimeStr = overtimeValue.ToString("0.000");

            Console.WriteLine($"\nOvertime when {patientsCount} patients are scheduled in {totalIntervalCount} " +
                                $"intervals = {overtimeStr} minutes.\n");

            IdleTimeFunc it = new IdleTimeFunc(genFunction);

            double idleTimeValue = it.getValue();
            string idleFuncStr = idleTimeValue.ToString("0.000");

            Console.WriteLine($"\nIdletime when {patientsCount} patients are scheduled in {totalIntervalCount} " +
                                $"intervals = {idleFuncStr} minutes.\n");

            WaitTimeFunc wt = new WaitTimeFunc(genFunction);

            double waitTimeValue = wt.getValue();
            string waitFuncStr = waitTimeValue.ToString("0.000");

            Console.WriteLine($"\nAverage Wait time when {patientsCount} patients are scheduled in {totalIntervalCount} " +
                                $"intervals = {waitFuncStr} minutes.\n");

            double alpha = 2.5, beta = 5;
            double objFuncValue = alpha * waitTimeValue + idleTimeValue + beta * overtimeValue;
            string objFuncStr = objFuncValue.ToString("0.000");

            Console.WriteLine($"\nObjective function value = {objFuncStr}.\n");
        }
    }
}