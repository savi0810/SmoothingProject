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
                GenerateData();
                PlotOriginalData();
            };
        }

        private void GenerateButton_Click(object sender, RoutedEventArgs e)
        {
            GenerateData();
            PlotOriginalData();
            ClearSmoothedData();
        }

        private void SmoothButton_Click(object sender, RoutedEventArgs e)
        {
            if (originalData.Count > 0)
            {
                ApplySmoothing();
                PlotSmoothedData();
            }
        }

        private void GenerateData()
        {
            originalData = dataService.GenerateSampleData(1000);
        }

        private void ApplySmoothing()
        {
            smoothedData = smoother.Smooth(originalData, 66, 10);
        }

        private void PlotOriginalData()
        {
            if (LeftCanvas.ActualWidth > 0 && LeftCanvas.ActualHeight > 0)
            {
                plotter.PlotPoints(LeftCanvas, originalData, false);
            }
        }

        private void PlotSmoothedData()
        {
            if (RightCanvas.ActualWidth > 0 && RightCanvas.ActualHeight > 0)
            {
                plotter.PlotPoints(RightCanvas, smoothedData, true);
            }
        }

        private void ClearSmoothedData()
        {
            RightCanvas.Children.Clear();
        }
    }
}