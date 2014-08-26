using System;
using BloodBowlPOC.MVVM;

namespace BloodBowlPOC.ViewModels
{
    public class ProbabilityItem : ObservableObject
    {
        private int _x;
        public int X
        {
            get { return _x; }
            set
            {
                if (Set(() => X, ref _x, value))
                    OnPropertyChanged("LocationX");
            }
        }

        private int _y;
        public int Y
        {
            get { return _y; }
            set
            {
                if (Set(() => Y, ref _y, value))
                    OnPropertyChanged("LocationY");
            }
        }

        private double _width;
        public double Width
        {
            get { return _width; }
            set
            {
                if (Set(() => Width, ref _width, value))
                    OnPropertyChanged("LocationX");
            }
        }

        private double _height;
        public double Height
        {
            get { return _height; }
            set
            {
                if (Set(() => Height, ref _height, value))
                    OnPropertyChanged("LocationY");
            }
        }

        private double _value;
        public double Value
        {
            get { return _value; }
            set
            {
                if (Set(() => Value, ref _value, value))
                {
                    OnPropertyChanged("DisplayValue");
                    OnPropertyChanged("TooltipValue");
                    OnPropertyChanged("Visible");
                }
            }
        }

        private bool _noColor;
        public bool NoColor
        {
            get { return _noColor; }
            set { Set(() => NoColor, ref _noColor, value); }
        }

        public bool Visible
        {
            get { return Value > 0; }
        }

        public string DisplayValue
        {
            get { return 100.0*Value < 0.01 ? "<0.01" : String.Format("{0:0.##}", 100.0*Value); }
        }

        public string TooltipValue
        {
            get { return String.Format("{0}", 100.0*Value); }
        }

        public double LocationX
        {
            get { return X*Width; }
        }

        public double LocationY
        {
            get { return Y*Height; }
        }
    }
}
