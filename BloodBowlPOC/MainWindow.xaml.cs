using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BloodBowlPOC
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public const int Size = 10;
        public TextBlock[,] Texts;

        public MainWindow()
        {
            InitializeComponent();

            Texts = new TextBlock[Size,Size];

            for(int i = 0; i < Size; i++)
                Grid.ColumnDefinitions.Add(new ColumnDefinition
                {
                    Width = new GridLength(40)
                });
            for (int i = 0; i < Size; i++)
                Grid.RowDefinitions.Add(new RowDefinition
                {
                    Height = new GridLength(40)
                });
            for(int y = 0; y < Size; y++)
                for (int x = 0; x < Size; x++)
                {
                    TextBlock txt = new TextBlock
                    {
                        Text = "0",
                        HorizontalAlignment = HorizontalAlignment.Center
                    };
                    Grid.SetColumn(txt, y);
                    Grid.SetRow(txt, x);
                    Grid.Children.Add(txt);
                    Texts[x, y] = txt;
                }

            Test();
        }

        public class MoveAction
        {
            public int FromX { get; set; }
            public int FromY { get; set; }
            public int DirectionX { get; set; }
            public int DirectionY { get; set; }
            public int Occurence { get; set; }
            // Distance is harcoded to 1 (should be 1 -> 6 with same probabilities)
            // Direction is implicitly 1/4 in each direction 0: up, 1: right, 2: down, 3: left
            public double Probability { get; set; }
        }

        public static int[] DirectionsX = {0, 1, 0, -1};
        public static int[] DirectionsY = {-1, 0, 1, 0};

        public void Test()
        {
            double[,] board = new double[Size, Size];
            
            //
            Queue<MoveAction> actions = new Queue<MoveAction>();
            //
            MoveAction startAction = new MoveAction
            {
                FromX = Size/2,
                FromY = Size/2,
                DirectionX = 0,
                DirectionY = 0,
                Occurence = 3,
                Probability = 1,
            };
            actions.Enqueue(startAction);
            //
            while (actions.Count > 0)
            {
                MoveAction action = actions.Dequeue();

                //
                int toX = action.FromX + action.DirectionX;
                int toY = action.FromY + action.DirectionY;

                //
                System.Diagnostics.Debug.WriteLine("Dequeue Action:{0},{1} -> {2},{3} | {4} | {5}", action.FromX, action.FromY, toX, toY, action.Occurence, action.Probability);

                // Create a new action for each direction if occurence > 0
                if (action.Occurence > 0)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        MoveAction subAction = new MoveAction
                        {
                            FromX = toX,
                            FromY = toY,
                            DirectionX = DirectionsX[i],
                            DirectionY = DirectionsY[i],
                            Occurence = action.Occurence - 1,
                            Probability = action.Probability/4.0
                        };
                        actions.Enqueue(subAction);
                    }
                }
                else
                    board[toX, toY] += action.Probability;
            }
            //double sum = 0;
            //for (int y = 0; y < Size; y++)
            //    for (int x = 0; x < Size; x++)
            //        sum += board[x, y];
            for(int y = 0; y < Size; y++)
                for (int x = 0; x < Size; x++)
                    Texts[x, y].Text = String.Format("{0:0.####}",board[x, y]);
        }
    }
}
