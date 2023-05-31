using System;

namespace OMS
{
    class Program
    {
        public static void Main(string[] args)
        {
            double success = 3;
            double serviceRate = 0.1;
            double intervalLength = 15;
            double noShowProb = 0.2;

            int totalIntervalCount = 3;

            Console.WriteLine($"\nAverage service time = {1 / serviceRate} minutes" +
                                $"\n\nScheduled time per slot is {intervalLength} minutes\n\n" +
                                $"Number of slots per day = {totalIntervalCount}\n");

            for (int i = 1; i <= 3; i++)
            {
                int[] regularPatientsCount = { 2 * i, 0, i };

                int[] urgentPatientsCount = { i, i, 0 };

                OvertimeFunc ot = new OvertimeFunc(noShowProb, 5 * i, totalIntervalCount,
                                regularPatientsCount, urgentPatientsCount, success, serviceRate, intervalLength);

                string overtimeValue = ot.getValue().ToString("0.000");

                Console.WriteLine($"\nOvertime when {5 * i} patients are scheduled in {totalIntervalCount} intervals = {overtimeValue} minutes.\n");
            }
        }
    }
}