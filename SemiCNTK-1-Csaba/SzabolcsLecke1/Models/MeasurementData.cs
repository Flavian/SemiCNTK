namespace SzabolcsLecke1.Models
{
    public class MeasurementData : NotifyBase
    {
        #region Declarations

        #region UDistance

        private double _uDistance;
        public double UDistance
        {
            get { return _uDistance; }
            set { _uDistance = value; OnPropertyChanged(nameof(UDistance)); }
        }

        #endregion

        #region UEddy

        private double _uEddy;
        public double UEddy
        {
            get { return _uEddy; }
            set { _uEddy = value; OnPropertyChanged(nameof(UEddy)); }
        }

        #endregion

        #endregion

        // ********************************************************************

        #region Constructor

        public MeasurementData(double uDistance, double uEddy)
        {
            UDistance = uDistance;
            UEddy = uEddy;
        }

        #endregion
    }
}
