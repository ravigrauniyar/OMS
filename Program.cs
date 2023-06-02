namespace OMS
{
    class Program
    {
        public int getPatientsCount(int[] patientCount)
        {
            int count = 0;
            for (int i = 0; i < patientCount.Length; i++)
            {
                count += patientCount[i];
            }
            return count;
        }
        public static void Main(string[] args)
        {
            double serviceRate = 0.06667;
            int intervalLength = 15;

            double noShowProb = 0.1;

            int[][] regularPatientsCount =
            {
                new int []{ 1, 0, 1, 1, 0 },
                new int []{ 1, 1, 1, 1, 0 },
                new int []{ 0, 0, 1, 1, 1, 0 },
                new int []{ 0, 1, 1, 1, 1, 0 },
                new int []{ 1, 1, 1, 1, 1, 0 },
                new int []{ 0, 0, 1, 1, 1, 1, 0 },
                new int []{ 0, 1, 1, 1, 1, 1, 0 },
                new int []{ 1, 1, 1, 1, 1, 1, 0 },
                new int []{ 0, 2, 1, 1, 0, 0, 0, 0 },
                new int []{ 0, 2, 1, 1, 0, 1, 0, 0 },
                new int []{ 0, 2, 1, 1, 1, 1, 0, 0 },
                new int []{ 1, 2, 1, 1, 1, 1, 0, 0 },
                new int []{ 0, 2, 1, 1, 0, 0, 1, 0, 0 },
                new int []{ 0, 2, 1, 1, 1, 0, 1, 0, 0 },
                new int []{ 0, 2, 1, 1, 1, 1, 1, 0, 0 },
                new int []{ 1, 2, 1, 1, 1, 1, 1, 0, 0 },
                new int []{ 0, 2, 1, 1, 0, 0, 0, 1, 0, 0 },
                new int []{ 0, 2, 1, 1, 1, 0, 0, 1, 0, 0 },
                new int []{ 0, 2, 1, 1, 1, 1, 0, 1, 0, 0 },
                new int []{ 0, 2, 1, 1, 1, 1, 1, 1, 0, 0 },
                new int []{ 1, 2, 1, 1, 1, 1, 1, 1, 0, 0 }
            };

            int[][] urgentPatientsCount = new int[][]{
                new int []{ 1, 1, 0, 0, 0 },
                new int []{ 1, 0, 0, 0, 0 },
                new int []{ 2, 1, 0, 0, 0, 0 },
                new int []{ 2, 0, 0, 0, 0, 0 },
                new int []{ 1, 0, 0, 0, 0, 0 },
                new int []{ 2, 1, 0, 0, 0, 0, 0 },
                new int []{ 2, 0, 0, 0, 0, 0, 0 },
                new int []{ 1, 0, 0, 0, 0, 0, 0 },
                new int []{ 2, 0, 0, 0, 1, 1, 0, 0 },
                new int []{ 2, 0, 0, 0, 1, 0, 0, 0 },
                new int []{ 2, 0, 0, 0, 0, 0, 0, 0 },
                new int []{ 1, 0, 0, 0, 0, 0, 0, 0 },
                new int []{ 2, 0, 0, 0, 1, 1, 0, 0, 0 },
                new int []{ 2, 0, 0, 0, 0, 1, 0, 0, 0 },
                new int []{ 2, 0, 0, 0, 0, 0, 0, 0, 0 },
                new int []{ 1, 0, 0, 0, 0, 0, 0, 0, 0 },
                new int []{ 2, 0, 0, 0, 1, 1, 1, 0, 0, 0 },
                new int []{ 2, 0, 0, 0, 0, 1, 1, 0, 0, 0 },
                new int []{ 2, 0, 0, 0, 0, 0, 1, 0, 0, 0 },
                new int []{ 2, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                new int []{ 1, 0, 0, 0, 0, 0, 0, 0, 0, 0 }
            };
            double[] researchValue = {
                100.87, 97.81, 115.15, 112.14, 109.35, 125.48, 122.71, 120.17, 138.34, 134.82, 131.49,
                128.38, 145.65, 142.22, 138.98, 136.01, 156.07, 152.60, 149.26, 146.07, 143.25
            };

            Program prog = new Program();
            Console.WriteLine($"\nAverage service time = {1 / serviceRate} minutes" +
                                                $"\n\nScheduled time per slot is {intervalLength} minutes.");

            Console.WriteLine("\nS.N.\t\tRegular\t\tUrgent\t\tTotal\t\tProgram's Objective Value\tResearch's Objective Value\n");
            for (int i = 0; i < 21; i++)
            {
                int N = prog.getPatientsCount(regularPatientsCount[i]); int M = prog.getPatientsCount(urgentPatientsCount[i]);

                int patientsCount = M + N;

                int totalIntervalCount = patientsCount;

                double expectedWorkload = ((1 - noShowProb) * N + M);

                GeneralFunc genFunction = new GeneralFunc(
                    noShowProb, patientsCount, regularPatientsCount[i],
                    urgentPatientsCount[i], serviceRate, intervalLength, totalIntervalCount, expectedWorkload
                );

                OvertimeFunc ot = new OvertimeFunc(genFunction);
                double overtimeValue = ot.getValue();

                IdleTimeFunc it = new IdleTimeFunc(genFunction);
                double idleTimeValue = it.getValue();

                WaitTimeFunc wt = new WaitTimeFunc(genFunction);
                double waitTimeValue = wt.getValue();

                double alpha = 2.5, beta = 5;
                double objFuncValue = alpha * waitTimeValue + idleTimeValue + beta * overtimeValue;

                string objFuncStr = objFuncValue.ToString("0.000");

                Console.WriteLine($"{i + 1}\t\t{N}\t\t{M}\t\t{N + M}\t\t{objFuncStr}\t\t\t\t{researchValue[i]}");
            }
        }
    }
}