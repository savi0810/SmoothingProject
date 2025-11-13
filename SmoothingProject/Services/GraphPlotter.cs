using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace SmoothingProject.Services
{
    public class GraphPlotter
    {
        private const double POINT_SIZE = 8; 
        private const double LINE_THICKNESS = 1;

        private readonly Color originalColor = Color.FromRgb(100, 200, 255);
        private readonly Color smoothedColor = Color.FromRgb(200, 120, 255);

        public void PlotPoints(Canvas canvas, List<double> data, bool isSmoothed = false)
        {
            if (canvas == null || data == null || data.Count == 0)
                return;

            canvas.Children.Clear();

            if (canvas.ActualWidth == 0 || canvas.ActualHeight == 0)
                return;

            try
            {
                double maxY = data.Max();
                double minY = data.Min();
                double yRange = maxY - minY;

                double padding = yRange * 0.15;
                minY -= padding;
                maxY += padding;
                yRange = maxY - minY;

                double xStep = canvas.ActualWidth / (data.Count - 1);
                double yScale = canvas.ActualHeight / yRange;

                var brush = new SolidColorBrush(isSmoothed ? smoothedColor : originalColor);
                brush.Freeze();

                for (int i = 0; i < data.Count - 1; i++)
                {
                    double x1 = i * xStep;
                    double y1 = canvas.ActualHeight - (data[i] - minY) * yScale;
                    double x2 = (i + 1) * xStep;
                    double y2 = canvas.ActualHeight - (data[i + 1] - minY) * yScale;

                    var line = new Line
                    {
                        X1 = x1,
                        Y1 = y1,
                        X2 = x2,
                        Y2 = y2,
                        Stroke = brush,
                        StrokeThickness = LINE_THICKNESS,
                        StrokeStartLineCap = PenLineCap.Round,
                        StrokeEndLineCap = PenLineCap.Round,
                        SnapsToDevicePixels = true
                    };

                    canvas.Children.Add(line);
                }

                var pointBrush = new SolidColorBrush(Colors.White);
                pointBrush.Freeze();

                for (int i = 0; i < data.Count; i += 50)
                {
                    double x = i * xStep;
                    double y = canvas.ActualHeight - (data[i] - minY) * yScale;

                    var ellipse = new Ellipse
                    {
                        Width = POINT_SIZE,
                        Height = POINT_SIZE,
                        Fill = pointBrush,
                        SnapsToDevicePixels = true
                    };

                    Canvas.SetLeft(ellipse, x - POINT_SIZE / 2);
                    Canvas.SetTop(ellipse, y - POINT_SIZE / 2);

                    canvas.Children.Add(ellipse);
                }

                AddExtremumLabels(canvas, minY, maxY, isSmoothed);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при отрисовке: {ex.Message}", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddExtremumLabels(Canvas canvas, double minY, double maxY, bool isSmoothed)
        {
            var labelColor = isSmoothed ? smoothedColor : originalColor;
            var labelBrush = new SolidColorBrush(labelColor);
            labelBrush.Freeze();

            var topLabel = new TextBlock
            {
                Text = "MAX " + Math.Round(maxY, 1).ToString(),
                Foreground = labelBrush,
                FontSize = 14,
                FontWeight = FontWeights.Bold,
            };

            Canvas.SetLeft(topLabel, 5);
            Canvas.SetTop(topLabel, 2);
            canvas.Children.Add(topLabel);

            var bottomLabel = new TextBlock
            {
                Text = "MIN " + Math.Round(minY, 1).ToString(),
                Foreground = labelBrush,
                FontSize = 14,
                FontWeight = FontWeights.Bold,
            };

            Canvas.SetLeft(bottomLabel, 5);
            Canvas.SetTop(bottomLabel, canvas.ActualHeight - 20);
            canvas.Children.Add(bottomLabel);
        }

        public void ClearCanvas(Canvas canvas)
        {
            canvas?.Children.Clear();
        }
    }
}