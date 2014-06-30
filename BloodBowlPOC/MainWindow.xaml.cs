using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using BloodBowlPOC.Boards;

namespace BloodBowlPOC
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static readonly double CellWidth = 32;
        public static readonly double CellHeight = 24;
        public static readonly int SizeX = 16;
        public static readonly int SizeY = 24;

        public TextBlock[,] Texts;
        public Border[,] Cells;

        public Board Board;

        public MainWindow()
        {
            InitializeComponent();

            Texts = new TextBlock[SizeX,SizeY];
            Cells = new Border[SizeX,SizeY];

            for (int y = 0; y < SizeY; y++)
                for (int x = 0; x < SizeX; x++)
                {
                    TextBlock txt = new TextBlock
                        {
                            Text = String.Empty,
                            HorizontalAlignment = HorizontalAlignment.Center,
                            Height = CellHeight,
                            Width = CellWidth,
                        };
                    Texts[x, y] = txt;

                    Border border = new Border
                        {
                            BorderBrush = new SolidColorBrush(Colors.Black),
                            BorderThickness = new Thickness(1),
                            Child = txt,
                            Height = CellHeight,
                            Width = CellWidth,
                        };
                    border.MouseUp += CellOnMouseUp;
                    Canvas.SetTop(border, y*CellHeight);
                    Canvas.SetLeft(border, x*CellWidth);
                    GridCanvas.Children.Add(border);
                    Cells[x, y] = border;
                }
            GridCanvas.Width = CellWidth*SizeX;
            GridCanvas.Height = CellHeight*SizeY;

            MaxBouncesComboBox.SelectedIndex = 2;
            MaxDistanceComboBox.SelectedIndex = 2;

            Board = new Board(SizeX, SizeY);
        }

        private void CellOnMouseUp(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            Border border = (sender as Border);
            if (border != null)
            {
                // Get cell
                Point relativePoint = border.TransformToAncestor(GridCanvas).Transform(new Point(0, 0));
                int cellX = (int) (relativePoint.X/CellWidth);
                int cellY = (int) (relativePoint.Y/CellHeight);
                if (cellX >= 0 && cellX < SizeX && cellY >= 0 && cellY < SizeY)
                {
                    // Compute and display
                    Board.Reset();
                    ClearGrid();
                    int maxDistance = int.Parse(MaxDistanceComboBox.SelectionBoxItem.ToString());
                    int maxBounces = int.Parse(MaxBouncesComboBox.SelectionBoxItem.ToString());
                    Board.ComputeBounceProbabilities(cellX, cellY, maxDistance, maxBounces);
                    DisplayProbabilities(Board);
                    Cells[cellX, cellY].BorderBrush = new SolidColorBrush(Colors.White);
                }
            }
        }

        private void ClearGrid()
        {
            for (int y = 0; y < SizeY; y++)
                for (int x = 0; x < SizeX; x++)
                {
                    Texts[x, y].Text = null;
                    Texts[x, y].ToolTip = null;
                    Cells[x, y].Background = new SolidColorBrush(Colors.White);
                    Cells[x, y].BorderBrush = new SolidColorBrush(Colors.Black);
                }
        }

        private void DisplayProbabilities(Board board)
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
            System.Diagnostics.Debug.WriteLine("Sum:{0} Min:{1} Max:{2}", sum, min, max);
            Status.Text = String.Format("Sum:{0:0.####} Min:{1:0.####} Max:{2:0.####}", sum*100, min*100, max*100);
            // Display
            bool uniqueValue = Math.Abs(min - max) < 0.0000001; // if min == max -> display in green
            for (int y = 0; y < SizeY; y++)
                for (int x = 0; x < SizeX; x++)
                {
                    double probability = board.Probabilities[x, y];
                    if (probability > 0)
                    {
                        Texts[x, y].Text = 100.0*probability < 0.01 ? "<0.01" : String.Format("{0:0.##}", 100.0*probability);
                        Texts[x, y].ToolTip = String.Format("{0}", 100.0*probability);
                        if (uniqueValue)
                            Cells[x, y].Background = new SolidColorBrush(Color.FromRgb(0, 0xFF, 0));
                        else
                        {
                            double red = 255 - (probability - min)*(255 - 0)/(max - min);
                            double green = 0 + (probability - min)*(255 - 0)/(max - min);
                            Cells[x, y].Background = new SolidColorBrush(Color.FromRgb((byte) red, (byte) green, 0));
                        }
                    }
                }
        }
    }
}
