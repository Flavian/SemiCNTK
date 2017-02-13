namespace SzabolcsLecke1.Models
{
    public class LineModel : NotifyBase
    {
        #region Declarations

        #region X1

        private double _x1;
        public double X1
        {
            get { return _x1; }
            set { _x1 = value; OnPropertyChanged(nameof(X1)); }
        }

        #endregion

        #region X2

        private double _x2;
        public double X2
        {
            get { return _x2; }
            set { _x2 = value; OnPropertyChanged(nameof(X2)); }
        }

        #endregion

        #endregion

        // ********************************************************************

        #region Update

        public void UpdateLineEndXPoints(double x1, double x2)
        {
            X1 = x1;
            X2 = x2;
        } 

        #endregion
    }
}
