namespace OMS
{
    class ObjectiveFunc
    {
        private readonly double _serviceRate, _expectedWorkload;
        private readonly int _intervalLength;
        private readonly double _noShowProb;
        private readonly int[] _regularPatientsCount, _urgentPatientsCount;
        private readonly int _N, _M, _patientsCount, _totalIntervalCount;
        private readonly GeneralFunc? _genFunc;
        public ObjectiveFunc(
            int[] regularPatientsArr, int[] urgentPatientsArr, double serviceRate, int intervalLength, double noShowProb
        )
        {
            _regularPatientsCount = regularPatientsArr;
            _urgentPatientsCount = urgentPatientsArr;

            _serviceRate = serviceRate;
            _intervalLength = intervalLength;
            _noShowProb = noShowProb;

            _N = _regularPatientsCount.Sum();
            _M = _urgentPatientsCount.Sum();
            _patientsCount = _M + _N;
            
            _totalIntervalCount = _patientsCount;
            _expectedWorkload = ((1 - _noShowProb) * _N + _M);

            _genFunc = new GeneralFunc(
                _noShowProb, _regularPatientsCount, _urgentPatientsCount, _serviceRate, _intervalLength
            );
        }
        private double GetWaitTimeValue()
        {
            double totalWaitTime = 0;
            int T = _totalIntervalCount;

            for (int t = 1; t <= T; t++)
            {
                double waitTimeOfCombo = 0;
                int Xt = _regularPatientsCount[t - 1];
                int Yt = _urgentPatientsCount[t - 1];
                int prevPatientsCount = _genFunc!.getPatientsCountTill(t - 1);

                for (int k = 0; k <= Xt; k++)
                {
                    double timeInQueue = 0;
                    for (int j = 1; j <= (k + Yt); j++)
                    {
                        double sum = 0;
                        for (int i = 0; i <= prevPatientsCount; i++)
                        {
                            sum += _genFunc.getIBeforeArrivalProb(t, i) * (i + j - 1) / _serviceRate;
                        }
                        timeInQueue += sum;
                    }
                    waitTimeOfCombo += _genFunc.getComboShowUpProb(Xt, k) * timeInQueue;
                }
                totalWaitTime += waitTimeOfCombo;
            }
            double avgWaitTime = totalWaitTime / _expectedWorkload;
            return avgWaitTime;
        }
        private double GetIdleTimeValue()
        {
            int Tmax = _genFunc!.getTmax(_totalIntervalCount);
            
            int Xtmax = _regularPatientsCount[Tmax - 1];
            int Ytmax = _urgentPatientsCount[Tmax - 1];

            double remainingPatientsTime = 0;
            double timeAlreadySpent = (Tmax - 1) * _intervalLength;
            double expectedWorkloadTime = _expectedWorkload / _serviceRate;

            for (int k = 0; k <= Xtmax; k++)
            {
                int count = _genFunc.getPatientsCountTill(Tmax - 1);
                double sum = 0;

                for (int i = 0; i <= count; i++)
                {
                    sum += _genFunc.getIBeforeArrivalProb(Tmax, i) * (i + k + Ytmax) / _serviceRate;
                }
                remainingPatientsTime += _genFunc.getIDepartureProb(k) * sum;
            }
            double totalIdleTime = Math.Abs(timeAlreadySpent + remainingPatientsTime - expectedWorkloadTime);
            return totalIdleTime;
        }
        private double GetOverTimeValue()
        {
            double overtimeValue = 0;
            for (int i = 1; i <= _patientsCount; i++)
            {
                overtimeValue += _genFunc!.getIBeforeArrivalProb(_totalIntervalCount + 1, i) * i / _serviceRate;
            }
            return overtimeValue;
        }
        public double GetObjectiveFuncValue()
        {
            double alpha = 2.5, beta = 5, waitTimeValue, idleTimeValue, overtimeValue;

            waitTimeValue = GetWaitTimeValue();
            idleTimeValue = GetIdleTimeValue();
            overtimeValue = GetOverTimeValue();

            double objFuncValue = alpha * waitTimeValue + idleTimeValue + beta * overtimeValue;
            return objFuncValue;
        }
    }
}