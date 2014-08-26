using System;
using System.Collections.Generic;
using System.Linq;
using BloodBowlPOC.Boards;

namespace BloodBowlPOC.ViewModels
{
    public class ProbabilitiesViewModel : ViewModelBase
    {
        private List<ProbabilityItem> _probabilities;
        public List<ProbabilityItem> Probabilities
        {
            get { return _probabilities; }
            set { Set(() => Probabilities, ref _probabilities, value); }
        }

        private double _totalWidth;
        public double TotalWidth
        {
            get { return _totalWidth; }
            set { Set(() => TotalWidth, ref _totalWidth, value); }
        }

        private double _totalHeight;
        public double TotalHeight
        {
            get { return _totalHeight; }
            set { Set(() => TotalHeight, ref _totalHeight, value); }
        }
        
        private double _min;
        public double Min
        {
            get { return _min; }
            set { Set(() => Min, ref _min, value); }
        }

        private double _max;
        public double Max
        {
            get { return _max; }
            set { Set(() => Max, ref _max, value); }
        }

        private double _sum;
        public double Sum
        {
            get { return _sum; }
            set { Set(() => Sum, ref _sum, value); }
        }

        public void Initialize(int sizeX, int sizeY, double cellWidth, double cellHeight)
        {
            List<ProbabilityItem> probabilities = new List<ProbabilityItem>(sizeX * sizeY);
            for (int y = 0; y < sizeY; y++)
                for (int x = 0; x < sizeX; x++)
                {
                    ProbabilityItem item = new ProbabilityItem
                    {
                        Value = 0,
                        X = x,
                        Y = y,
                        Width = cellWidth,
                        Height = cellHeight,
                        NoColor = true
                    };
                    probabilities.Add(item);
                }
            Probabilities = probabilities;
            TotalWidth = sizeX*cellWidth;
            TotalHeight = sizeY * cellHeight;
        }

        public void Display(Board board)
        {
            // Get min/max/sum
            double min = Double.MaxValue;
            double max = Double.MinValue;
            double sum = 0;
            for (int y = 0; y < board.SizeY; y++)
                for (int x = 0; x < board.SizeX; x++)
                {
                    double probability = board.Probabilities[x, y];
                    if (probability > 0)
                    {
                        sum += probability;
                        if (probability < min)
                            min = probability;
                        if (probability > max)
                            max = probability;
                    }
                }
            Min = min; // don't use these properties while computing to avoid multiple OnPropertyChanged
            Max = max;
            Sum = sum;
            // Build probability items
            bool uniqueValue = Math.Abs(min - max) < 0.0000001; // if min == max -> display in green
            for (int y = 0; y < board.SizeY; y++)
                for (int x = 0; x < board.SizeX; x++)
                {
                    double probability = board.Probabilities[x, y];
                    ProbabilityItem item = Probabilities.FirstOrDefault(p => p.X == x && p.Y == y);
                    if (item != null)
                    {
                        item.Value = probability;
                        item.NoColor = uniqueValue;
                    }
                    // else, should not be possible :)
                }
        }
    }

    public class ProbabilitiesViewModelDesignData : ProbabilitiesViewModel
    {
        public ProbabilitiesViewModelDesignData()
        {
            Min = 0.5;
            Max = 0.8;
            Sum = 1.0;
            Probabilities = new List<ProbabilityItem>();
            for(int y = 0; y < 10; y++)
                for(int x = 0; x < 10; x++)
                {
                    ProbabilityItem item = new ProbabilityItem
                        {
                            Value = x == 0 || y == 0 ? 0 : 0.01*x+0.01*y,
                            X = x,
                            Y = y,
                            Width = 30,
                            Height = 25,
                            NoColor = false
                        };
                    Probabilities.Add(item);
                }
            TotalWidth = 10*30;
            TotalHeight = 10 * 25;
        }
    }
}
