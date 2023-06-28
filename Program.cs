namespace OMS
{
    class Program
    {
        public static void Main(string[] args)
        {
            int intervalLength = 15, N = 4, M = 1;
            double serviceRate = 0.0667, noShowProb = 0.1;

            AlgorithmFunc algorithm = new AlgorithmFunc(serviceRate, intervalLength, noShowProb, N, M);
            
            algorithm.ShowOptimalSchedule();
        }
    }
}