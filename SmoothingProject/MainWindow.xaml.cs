using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using SmoothingProject.Services;

namespace SmoothingProject
{
    public partial class MainWindow : Window
    {
        private List<double> originalData = new List<double>();
        private List<double> smoothedData = new List<double>();

        private readonly DataService dataService;
        private readonly GaussianSmoother smoother;
        private readonly GraphPlotter plotter;

        public MainWindow()
        {
            InitializeComponent();

            dataService = new DataService();
            smoother = new GaussianSmoother();
            plotter = new GraphPlotter();

            Loaded += (s, e) =>
            {
                try
                {
                    GenerateData();
                    PlotOriginalData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при инициализации: {ex.Message}", "Ошибка",
                                  MessageBoxButton.OK, MessageBoxImage.Error);
                }
            };
        }

        private void GenerateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                GenerateData();
                PlotOriginalData();
                ClearSmoothedData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при генерации данных: {ex.Message}", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SmoothButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (originalData.Count > 0)
                {
                    ApplySmoothing();
                    PlotSmoothedData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сглаживании: {ex.Message}", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void GenerateData()
        {
            originalData = dataService.GenerateSampleData(1000);
        }

        public void ApplySmoothing()
        {
            smoothedData = smoother.Smooth(originalData, 66, 10);
        }

        public void PlotOriginalData()
        {
            if (LeftCanvas.ActualWidth > 0 && LeftCanvas.ActualHeight > 0)
            {
                plotter.PlotPoints(LeftCanvas, originalData, false);
            }
        }

        public void PlotSmoothedData()
        {
            if (RightCanvas.ActualWidth > 0 && RightCanvas.ActualHeight > 0)
            {
                plotter.PlotPoints(RightCanvas, smoothedData, true);
            }
        }

        public void ClearSmoothedData()
        {
            RightCanvas.Children.Clear();
        }
    }
}