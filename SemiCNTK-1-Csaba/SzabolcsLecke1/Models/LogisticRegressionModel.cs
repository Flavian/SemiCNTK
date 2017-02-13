using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

namespace SzabolcsLecke1.Models
{
    public class LogisticRegressionModel : NotifyBase
    {
        #region Declarations

        #region Theta0

        private double _theta0;
        public double Theta0
        {
            get { return _theta0; }
            set { _theta0 = value; OnPropertyChanged(nameof(Theta0)); }
        }

        #endregion

        #region Theta1

        private double _theta1;
        public double Theta1
        {
            get { return _theta1; }
            set { _theta1 = value; OnPropertyChanged(nameof(Theta1)); }
        }

        #endregion

        #region Cost

        private double _cost;
        public double Cost
        {
            get { return _cost; }
            set { _cost = value; OnPropertyChanged(nameof(Cost)); }
        }

        #endregion

        private bool _isStopped;

        #endregion

        // ********************************************************************

        #region Constructor

        public LogisticRegressionModel()
        {
            Theta0 = 0;
            Theta1 = 0.5;
            _isStopped = false;
        }

        #endregion

        // ********************************************************************

        #region Cost and Theta calculations

        public void CalculateCost(ObservableCollection<MeasurementData> measurementDatas)
        {
            Cost = 0;

            foreach (var measurementData in measurementDatas)
            {
                var hypothesis = Theta0 + Theta1 * measurementData.UDistance;
                hypothesis -= measurementData.UEddy;
                hypothesis *= hypothesis;

                Cost += hypothesis;
            }

            Cost /= (2 * measurementDatas.Count);
        }


        public void CalculateNewTheta(ObservableCollection<MeasurementData> measurementDatas, double alpha)
        {
            double sumTheta0 = 0;
            double sumTheta1 = 0;
            foreach (var measurementData in measurementDatas)
            {
                var thesis = Theta0 + Theta1 * measurementData.UDistance - measurementData.UEddy;
                sumTheta0 += thesis;
                sumTheta1 += thesis * measurementData.UDistance;
            }

            sumTheta0 /= measurementDatas.Count;
            sumTheta1 /= measurementDatas.Count;

            sumTheta0 *= alpha;
            sumTheta1 *= alpha;


            Theta0 -= sumTheta0;
            Theta1 -= sumTheta1;
        }

        #endregion

        // ********************************************************************

        #region Start and Stop continuous calculations

        public void Start(ObservableCollection<MeasurementData> measurementDatas, double alpha)
        {
            _isStopped = false;
            CalculateNewThetasInLoop(measurementDatas, alpha);
        }

        private void CalculateNewThetasInLoop(ObservableCollection<MeasurementData> measurementDatas, double alpha)
        {
            Task.Factory.StartNew(() =>
            {
                while (!_isStopped)
                {
                    CalculateNewTheta(measurementDatas, alpha);
                    CalculateCost(measurementDatas);
                    Thread.Sleep(10);
                }
            });
        }

        public void Stop()
        {
            _isStopped = true;
        } 

        #endregion
    }
}
