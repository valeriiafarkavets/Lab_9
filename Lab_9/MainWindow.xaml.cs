using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Lab_9.Models;
using Lab_9.ViewModels;
namespace Lab_9{
    public partial class MainWindow : Window  {
        private readonly MainViewModel viewModel;
        private int position;
        public MainWindow(){
            InitializeComponent();
            viewModel = new MainViewModel();
            DataContext = viewModel;
            viewModel.TreeChanged += DrawTree;
            Loaded += (sender, e) => DrawTree();
            TreeScroll.SizeChanged += (sender, e) => DrawTree();
        }
        private void DrawTree() {
            TreeCanvas.Children.Clear();

            TreeCanvas.Width = Math.Max( TreeScroll.ViewportWidth, viewModel.Count * 90 + 40);

            TreeCanvas.Height = Math.Max( 160,viewModel.Height * 75 + 20);
            if (viewModel.Root == null){
                TextBlock message = new TextBlock{
                    Text = "Дерево порожнє",
                    FontSize = 16,
                    Foreground = new SolidColorBrush(
                        Color.FromRgb(148, 102, 106))
                };
                Canvas.SetLeft(message, 20);
                Canvas.SetTop(message, 20);
                TreeCanvas.Children.Add(message);
                return;
            }
            position = 0;
            DrawNode(viewModel.Root, 0);
        }
        private Point DrawNode(Node node, int level){
            Point? left = null;
            Point? right = null;
            if (node.Left != null)
                left = DrawNode(node.Left, level + 1);
            double offset = (TreeCanvas.Width - viewModel.Count * 90) / 2;
            double x = offset + position * 90 + 45;
            double y = level * 75 + 35;
            position++;
            if (node.Right != null)
                right = DrawNode(node.Right, level + 1);
            Point center = new Point(x, y);
            if (left.HasValue)
                DrawLine(center, left.Value);
            if (right.HasValue)
                DrawLine(center, right.Value);
            Ellipse circle = new Ellipse {
                Width = 64,
                Height = 44,
                Fill = new SolidColorBrush(
                    Color.FromRgb(173, 65, 72)),
                Stroke = new SolidColorBrush(
                    Color.FromRgb(129, 49, 57)),
                StrokeThickness = 1.5
            };
            Canvas.SetLeft(circle, x - 32);
            Canvas.SetTop(circle, y - 22);
            Canvas.SetZIndex(circle, 1);
            TreeCanvas.Children.Add(circle);
            TextBlock label = new TextBlock {
                Text = node.Value.ToString(),
                Width = 64,
                Height = 22,
                FontSize = 14,
                FontWeight = FontWeights.SemiBold,
                Foreground = Brushes.White,
                TextAlignment = TextAlignment.Center
            };
            Canvas.SetLeft(label, x - 32);
            Canvas.SetTop(label, y - 10);
            Canvas.SetZIndex(label, 2);
            TreeCanvas.Children.Add(label);
            return center;
        }
        private void DrawLine(Point start, Point end) {
            Line line = new Line{
                X1 = start.X,
                Y1 = start.Y,
                X2 = end.X,
                Y2 = end.Y,
                Stroke = new SolidColorBrush(
                    Color.FromRgb(184, 124, 128)),
                StrokeThickness = 2
            };
            TreeCanvas.Children.Add(line);
        }
    }
}