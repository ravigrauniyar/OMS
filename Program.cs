namespace OMS
{
    class Program
    {
        public int[] getPatientsArr()
        {
            string? arrStr = Console.ReadLine();
            string[] patientsStrArr = arrStr!.Split(" ");
            int[] patientsIntArr = new int[patientsStrArr.Length];

            for (int i = 0; i < patientsStrArr.Length; i++)
            {
                patientsIntArr[i] = int.Parse(patientsStrArr[i]);
            }
            return patientsIntArr;
        }
        public void f(int[][] X)
        {
            int[] regularPatientsCount = X[0];
            int[] urgentPatientsCount = X[1];

            int N = regularPatientsCount.Sum(); int M = urgentPatientsCount.Sum();
            int patientsCount = M + N;
            int totalIntervalCount = patientsCount;

            double serviceRate = 0.06667;
            int intervalLength = 15;
            double noShowProb = 0.1;
            double expectedWorkload = ((1 - noShowProb) * N + M);

            Console.WriteLine($"\nAverage service time = {1 / serviceRate} minutes" +
                                                $"\n\nScheduled time per slot is {intervalLength} minutes.");

            Console.WriteLine("\nS.N.\t\tRegular\t\tUrgent\t\tTotal\t\tProgram's Objective Value\n");

            GeneralFunc genFunction = new GeneralFunc(
                noShowProb, patientsCount, regularPatientsCount,
                urgentPatientsCount, serviceRate, intervalLength, totalIntervalCount, expectedWorkload
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

            Console.WriteLine($"1\t\t{N}\t\t{M}\t\t{N + M}\t\t{objFuncStr}");
        }
        public static void Main(string[] args)
        {
            Program prog = new Program();

            // THE ALGORITHM WILL PROVIDE US THE SCHEDULE 'X' BUT FOR NOW USER GIVEN SCHEDULE IS USED

            Console.WriteLine("\nEnter schedule of Regular Patients separated by Space:\n");
            int[] regularPatientsCount = prog.getPatientsArr();

            Console.WriteLine("\nEnter schedule of Urgent Patients separated by Space:\n");
            int[] urgentPatientsCount = prog.getPatientsArr();

            int[][] X = { regularPatientsCount, urgentPatientsCount };

            // CALCULATING AND DISPLAYING f(X)
            prog.f(X);
        }
    }
}