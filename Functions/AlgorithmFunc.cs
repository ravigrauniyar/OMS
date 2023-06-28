namespace OMS
{
    class AlgorithmFunc
    {
        private readonly int _intervalLength, N, M;
        private readonly double _serviceRate, _noShowProb;
        private int[] _regularPatientsArr, _urgentPatientsArr;
        private int[,] _localSchedule;
        public AlgorithmFunc(
            double serviceRate, int intervalLength, double noShowProb, int inputN, int inputM
        )
        {
            _serviceRate = serviceRate;
            _intervalLength = intervalLength;
            _noShowProb = noShowProb;

            N = inputN;
            M = inputM;
            _regularPatientsArr = new int[N + M];
            _urgentPatientsArr = new int[N + M];
            _localSchedule = new int[2, (N + M)];

            InitLocalSchedule();
        }
        private void InitLocalSchedule()
        {
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < (N + M); j++)
                {
                    if (i == 0)
                    {
                        _regularPatientsArr[j] = (j < N) ? 1 : 0;
                        _localSchedule[i, j] = _regularPatientsArr[j];
                    }
                    else
                    {
                        _urgentPatientsArr[j] = (j < M) ? 1 : 0;
                        _localSchedule[i, j] = _urgentPatientsArr[j];
                    }
                }
            }
        }
        private int GetKValue(int T)
        {
            int k = 0;
            for (int i = 1; i < T; i++)
            {
                //DELTA EVALUATION
            }
            return k;
        }
        public void ShowOptimalSchedule()
        {
            ObjectiveFunc objFunc = new ObjectiveFunc(
                _regularPatientsArr, _urgentPatientsArr, _serviceRate, _intervalLength, _noShowProb
            );
            double objFuncValue = objFunc.GetObjectiveFuncValue();

            for (int i = 0; i < 2; i++)
            {
                string patientsScheduleType = (i == 0) ? "Regular Patients' Schedule" : "Urgent patients' Schedule";

                Console.Write($"\n\n{patientsScheduleType}:\t");

                for (int j = 0; j < (M + N); j++)
                {
                    Console.Write($"{_localSchedule[i, j]}\t");
                }
            }
            Console.WriteLine($"\n\nObjective function value = {objFuncValue}");
        }
    }
}