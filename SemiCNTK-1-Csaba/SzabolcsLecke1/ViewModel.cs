using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Input;
using SzabolcsLecke1.Models;

namespace SzabolcsLecke1
{
    public class ViewModel : NotifyBase
    {
        const string SourceFolder = "SourceFiles";

        #region Declaration

        #region Measurements

        private ObservableCollection<Measurement> _measurements;
        public ObservableCollection<Measurement> Measurements
        {
            get { return _measurements; }
            set { _measurements = value; OnPropertyChanged(nameof(Measurements)); }
        }

        #endregion

        #region Measurements

        private Measurement _selectedMeasurement;
        public Measurement SelectedMeasurement
        {
            get { return _selectedMeasurement; }
            set
            {
                _selectedMeasurement = value;
                OnPropertyChanged(nameof(SelectedMeasurement));
                UpdateAfterSelectedMeasurementChange();
            }
        }

        #endregion


        #region Measurements

        private LineModel _lineData;
        public LineModel LineData
        {
            get { return _lineData; }
            set { _lineData = value; OnPropertyChanged(nameof(LineData)); }
        }

        #endregion


        #region Alpha

        private double _alpha;
        public double Alpha
        {
            get { return _alpha; }
            set { _alpha = value; OnPropertyChanged(nameof(Alpha)); }
        }

        #endregion


        #region LogisticRegression

        private LogisticRegressionModel _logisticRegression;
        public LogisticRegressionModel LogisticRegression
        {
            get { return _logisticRegression; }
            set { _logisticRegression = value; OnPropertyChanged(nameof(LogisticRegression)); }
        }

        #endregion


        #endregion

        #region Commands


        #region CalculateOneStepCommand

        private ICommand _calculateOneStepCommand;
        public ICommand CalculateOneStepCommand
        {
            get { return _calculateOneStepCommand; }
            set { _calculateOneStepCommand = value; OnPropertyChanged(nameof(CalculateOneStepCommand)); }
        }

        #endregion


        #region CalculateOneStepCommand

        private ICommand _redrawLineCommand;
        public ICommand RedrawLineCommand
        {
            get { return _redrawLineCommand; }
            set { _redrawLineCommand = value; OnPropertyChanged(nameof(RedrawLineCommand)); }
        }

        #endregion

        #region StartCalculationCommand

        private ICommand _startCalculationCommand;
        public ICommand StartCalculationCommand
        {
            get { return _startCalculationCommand; }
            set { _startCalculationCommand = value; OnPropertyChanged(nameof(StartCalculationCommand)); }
        }

        #endregion


        #region StopCalculationCommand

        private ICommand _stopCalculationCommand;
        public ICommand StopCalculationCommand
        {
            get { return _stopCalculationCommand; }
            set { _stopCalculationCommand = value; OnPropertyChanged(nameof(StopCalculationCommand)); }
        }

        #endregion

        #endregion

        // ********************************************************************

        #region Constructor

        public ViewModel()
        {
            Measurements = new ObservableCollection<Measurement>();
            LineData = new LineModel();
            LogisticRegression = new LogisticRegressionModel();

            CreateCommands();

            Alpha = 0.05;
        } 

        #endregion

        // ********************************************************************

        #region Init

        public void Init()
        {
            LoadDataFiles();
            SelectedMeasurement = Measurements.FirstOrDefault();

            LogisticRegression.CalculateCost(SelectedMeasurement?.MeasurementDatas);
        }

        private void LoadDataFiles()
        {
            if (!Directory.Exists(SourceFolder))
                throw new DirectoryNotFoundException($"Directory not found: {SourceFolder}");

            var files = Directory.GetFiles(SourceFolder);
            foreach (var file in files)
            {
                var measurement = new Measurement();
                measurement.Init(file);

                Measurements.Add(measurement);
            }
        }

        #endregion

        // ********************************************************************

        #region Commands
        private void CreateCommands()
        {
            CalculateOneStepCommand = new SimpleCommand(ExecuteCalculateOneStepCommand);
            RedrawLineCommand = new SimpleCommand(ExecuteRedrawLineCommand);
            StartCalculationCommand = new SimpleCommand(ExecuteStartCalculationCommand);
            StopCalculationCommand = new SimpleCommand(ExecuteStopCalculationCommand);
        }
        
        private void ExecuteCalculateOneStepCommand()
        {
            LogisticRegression.CalculateNewTheta(SelectedMeasurement.MeasurementDatas, Alpha);
            LogisticRegression.CalculateCost(SelectedMeasurement.MeasurementDatas);
        }

        private void ExecuteRedrawLineCommand()
        {
            LogisticRegression.CalculateCost(SelectedMeasurement.MeasurementDatas);
        }


        private void ExecuteStartCalculationCommand()
        {
            LogisticRegression.Start(SelectedMeasurement.MeasurementDatas, Alpha);
        }

        private void ExecuteStopCalculationCommand()
        {
            LogisticRegression.Stop();
        }



        #endregion

        // ********************************************************************

        #region Methods called from property setters

        public void UpdateAfterSelectedMeasurementChange()
        {
            var x1 = SelectedMeasurement.GetFirstXPosition();
            var x2 = SelectedMeasurement.GetLastXPosition();

            LineData.UpdateLineEndXPoints(x1, x2);
        } 

        #endregion
    }
}
