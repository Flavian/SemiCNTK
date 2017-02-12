using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;

namespace SemiCNTK_1_Flavi
{
    class SemiCNTK
    {
        private string _appPath;
        private string _inputPath;
        private string _inputFile;

        private List<Data> _datas;
        private IEnumerable<IGrouping<double, Data>> _dataGroup;


        public void Start()
        {
            Console.WriteLine("The calculation is stated.");
            _inputFile = "onevarreg.csv";
            Console.WriteLine("Read data from file.");
            if (ReadeDataFromFile()) return;


            Console.WriteLine("Calculating...");
            _dataGroup = _datas.GroupBy(d => d.Resistivity);
            foreach (var group in _dataGroup)
            {
                double theta1;
                double theta2;
                uint steps;
                CalculateThetas(group.ToList(), out theta1, out theta2, out steps);

                Console.WriteLine($"Theta1: {theta1:N3}, theta0: {theta2:N3} in {steps} steps for group {group.Key:N3}");
            }

            Console.WriteLine("Calculeted is finished.");
            Console.Write("Press Enter to exit." );
            Console.ReadLine();
        }

        private bool ReadeDataFromFile()
        {
            _appPath = AppDomain.CurrentDomain.BaseDirectory;
            _inputPath = $@"{_appPath}{_inputFile}";
            _datas = new List<Data>();

            string input;
            using (var fileReader = new StreamReader(_inputPath))
            {
                input = fileReader.ReadToEnd();
            }

            if (string.IsNullOrEmpty(input))
            {
                Debug.WriteLine("The input file was empty.");
                return true;
            }

            GenerateDataFromString(input);
            return false;
        }

        private static void CalculateThetas(List<Data> datas, out double theta0, out double theta1, out uint steps)
        {
            steps = 0;
            bool needRecalculate;
            theta0 = 0.0;
            theta1 = 0.0;
            double epsilon = 1e-6;
            double alpha = 0.002;
            var m = datas.Count;
            
            double j = J(datas, theta0, theta1, m);

            do
            {
                var summa = 0.0;
                var summa2 = 0.0;

                foreach (var data in datas)
                {
                    var h = H(theta0, theta1, data) - data.Y;
                    var h2 = (H(theta0, theta1, data) - data.Y) * data.X;
                    summa += h;
                    summa2 += h2;
                }

                var newTheta0 = theta0 - alpha * (1.0 / m) * summa;
                var newTheta1 = theta1 - alpha * (1.0 / m) * summa2;

                var newJ = J(datas, newTheta0, newTheta1, m);
                needRecalculate = j - newJ > epsilon;
                
                theta0 = newTheta0;
                theta1 = newTheta1;
                j = newJ;
                steps++;

            } while (needRecalculate);
        }

        private static double J(List<Data> datas, double newTheta0, double newTheta1, int m)
        {
            double jSumma = 0;
            foreach (var data in datas)
                jSumma += Math.Pow(H(newTheta0, newTheta1, data) - data.Y, 2);
            var j = 1.0 / (2 * m) * jSumma;
            return j;
        }

        private static double H(double theta0, double theta1, Data data)
        {
            return theta0 + theta1 * data.X;
        }

        private void GenerateDataFromString(string input)
        {
            foreach (var line in input.Split(new[] {Environment.NewLine}, StringSplitOptions.None))
            {
                var splittedLine = line.Split(';');
                if (splittedLine.Length != 3 || splittedLine.Any(string.IsNullOrEmpty))
                {
                    continue;
                }

                if (splittedLine[0] == "Resistivity") continue;

                var resistivity = Double.Parse(splittedLine[0].Replace(",","."), CultureInfo.InvariantCulture);
                var udistance = Double.Parse(splittedLine[1].Replace(",", "."), CultureInfo.InvariantCulture);
                var uEddy = Double.Parse(splittedLine[2].Replace(",", "."), CultureInfo.InvariantCulture);

                var newData = new Data(resistivity, udistance, uEddy);
                _datas.Add(newData);
            }
        }
    }
}