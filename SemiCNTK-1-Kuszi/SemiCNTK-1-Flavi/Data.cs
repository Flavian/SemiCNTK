namespace SemiCNTK_1_Flavi
{
    public class Data
    {
        public Data(double resistivity, double udistance, double uEddy)
        {
            Resistivity = resistivity;
            Udistance = udistance;
            UEddy = uEddy;
        }

        public double Resistivity { get; set; }
        public double Udistance { get; set; }
        public double UEddy { get; set; }

        public double Y { get { return UEddy; } }
        public double X { get { return Udistance; } }
    }
}
