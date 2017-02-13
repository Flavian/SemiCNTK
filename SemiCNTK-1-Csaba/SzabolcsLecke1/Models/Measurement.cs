using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace SzabolcsLecke1.Models
{
    public class Measurement : NotifyBase
    {
        #region Declarations

        #region HeaderString

        private string _headerString;
        public string HeaderString
        {
            get { return _headerString; }
            set { _headerString = value; OnPropertyChanged(nameof(HeaderString)); }
        }

        #endregion

        #region MeasurementDatas

        private ObservableCollection<MeasurementData> _measurementDatas;
        public ObservableCollection<MeasurementData> MeasurementDatas
        {
            get { return _measurementDatas; }
            set { _measurementDatas = value; OnPropertyChanged(nameof(MeasurementDatas)); }
        }

        #endregion

        #endregion

        // ********************************************************************

        #region Constructor

        public Measurement()
        {
            MeasurementDatas = new ObservableCollection<MeasurementData>();
        }

        #endregion

        // ********************************************************************

        #region Init -- load data from files

        public void Init(string resultFileName)
        {
            if (!File.Exists(resultFileName))
                throw new FileNotFoundException($"Could not find file: {resultFileName}");

            LoadFile(resultFileName);
        }


        private void LoadFile(string resultFileName)
        {
            List<MeasurementData> dataList = new List<MeasurementData>();

            // first line is the header
            var file = new StreamReader(resultFileName);

            var line = file.ReadLine();
            System.Diagnostics.Debug.Assert(line != null);
            HeaderString = line;

            while ((line = file.ReadLine()) != null)
            {
                var splitted = line.Split(';');
                
                var uDistance = double.Parse(splitted[0]);
                var uEddy = double.Parse(splitted[1]);

                var measurementData = new MeasurementData(uDistance, uEddy);
                dataList.Add(measurementData);
            }

            file.Close();

            MeasurementDatas = new ObservableCollection<MeasurementData>(dataList.OrderBy(x => x.UDistance));
        }

        #endregion

        // ********************************************************************

        #region Utilities

        public double GetFirstXPosition()
        {
            return MeasurementDatas.First().UDistance;
        }

        public double GetLastXPosition()
        {
            return MeasurementDatas.Last().UDistance;
        } 

        #endregion
    }
}
