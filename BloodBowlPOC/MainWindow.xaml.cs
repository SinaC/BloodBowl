﻿using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using BloodBowlPOC.Actions;
using BloodBowlPOC.Boards;
using BloodBowlPOC.Utils;

namespace BloodBowlPOC
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static readonly double CellWidth = 32;
        public static readonly double CellHeight = 24;
        public const int SizeX = 15;
        public const int SizeY = 24;
        public TextBlock[,] Texts;
        public Border[,] Cells;

        public Board Board = new Board(SizeX, SizeY);


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
                    Canvas.SetTop(border, y * CellHeight);
                    Canvas.SetLeft(border, x * CellWidth);
                    GridCanvas.Children.Add(border);
                    Cells[x, y] = border;
                }
            GridCanvas.Width = CellWidth * SizeX;
            GridCanvas.Height = CellHeight * SizeY;

            //Test(Board, SizeX/2, SizeY/2);
        }

        private void CellOnMouseUp(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            Border border = (sender as Border);
            if (border != null)
            {
                // Get cell
                Point relativePoint = border.TransformToAncestor(GridCanvas).Transform(new Point(0, 0));
                int cellX = (int)(relativePoint.X / CellWidth);
                int cellY = (int)(relativePoint.Y / CellHeight);
                if (cellX >= 0 && cellX < SizeX && cellY >= 0 && cellY < SizeY)
                {
                    // Compute and display
                    Board.Reset();
                    ClearGrid();
                    Test(Board, new FieldCoordinate(cellX, cellY));
                    Cells[cellX, cellY].BorderBrush = new SolidColorBrush(Colors.White);
                }
            }
        }

        private void ClearGrid()
        {
            for (int y = 0; y < SizeY; y++)
                for (int x = 0; x < SizeX; x++)
                {
                    Texts[x, y].Text = String.Empty;
                    Cells[x, y].Background = new SolidColorBrush(Colors.White);
                    Cells[x, y].BorderBrush = new SolidColorBrush(Colors.Black);
                }
        }

        public void Test(Board board, FieldCoordinate origin)
        {
            int fromX, fromY;
            BounceAction startAction = new BounceAction
                {
                    Coordinate = origin,
                    Occurrence = 3,
                    Probability = 1,
                };
            //
            Queue<ActionBase> actions = new Queue<ActionBase>();
            actions.Enqueue(startAction);

            // Perform actions
            int iterations = 0;
            while (actions.Count > 0)
            {
                ActionBase action = actions.Dequeue();
                iterations++;
                //
                List<ActionBase> subActions = action.Perform(board);
                subActions.ForEach(actions.Enqueue);
            }
            // Get min/max/sum
            double min = Double.MaxValue;
            double max = Double.MinValue;
            double sum = 0;
            for (int y = 0; y < SizeY; y++)
                for (int x = 0; x < SizeX; x++)
                {
                    double probability = board.Probabilities[x,y];
                    if (probability > 0)
                    {
                        sum += probability;
                        if (probability < min)
                            min = probability;
                        if (probability > max)
                            max = probability;
                    }
                }
            System.Diagnostics.Debug.WriteLine("Sum:{0} Min:{1} Max:{2} | Iterations:{3}", sum, min, max, iterations);
            Status.Text = String.Format("Sum:{0:0.####} Min:{1:0.####} Max:{2:0.####} | Iterations:{3}", sum * 100, min * 100, max * 100, iterations);
            // Display
            for (int y = 0; y < SizeY; y++)
                for (int x = 0; x < SizeX; x++)
                {
                    double probability = board.Probabilities[x, y];
                    if (probability > 0)
                    {
                        double red = 255 - (probability - min)*(255 - 0)/(max - min);
                        double green = 0 + (probability - min)*(255 - 0)/(max - min);
                        Texts[x, y].Text = String.Format("{0:0.##}", 100.0*probability);
                        Cells[x, y].Background = new SolidColorBrush(Color.FromRgb((byte) red, (byte) green, 0));
                    }
                }
            if (startAction.Coordinate.X >= 0 && startAction.Coordinate.Y < SizeX && startAction.Coordinate.X >= 0 && startAction.Coordinate.Y < SizeY)
                Cells[startAction.Coordinate.X, startAction.Coordinate.Y].BorderBrush = new SolidColorBrush(Colors.White);
        }
    }
}