using NUnit.Framework;
using SmoothingProject;
using SmoothingProject.Services;
using System.Linq;
using System.Windows;

namespace SmoothingProject.Tests
{
    [TestFixture]
    [Apartment(ApartmentState.STA)] 
    public class MainWindowTests
    {
        private MainWindow _mainWindow;

        [SetUp]
        public void SetUp()
        {
            _mainWindow = new MainWindow();
        }

        [Test]
        public void MainWindow_Initialization_CreatesServices()
        {
            var dataService = GetPrivateField<DataService>(_mainWindow, "dataService");
            var smoother = GetPrivateField<GaussianSmoother>(_mainWindow, "smoother");
            var plotter = GetPrivateField<GraphPlotter>(_mainWindow, "plotter");

            Assert.That(dataService, Is.Not.Null);
            Assert.That(smoother, Is.Not.Null);
            Assert.That(plotter, Is.Not.Null);
        }

        [Test]
        public void GenerateData_CreatesNonEmptyData()
        {
            _mainWindow.GenerateData();

            var data = GetPrivateField<List<double>>(_mainWindow, "originalData");
            Assert.That(data, Is.Not.Null);
            Assert.That(data, Is.Not.Empty);
        }

        [Test]
        public void ApplySmoothing_WithData_ProducesSmoothedData()
        {
            _mainWindow.GenerateData();
            _mainWindow.ApplySmoothing();

            var smoothedData = GetPrivateField<List<double>>(_mainWindow, "smoothedData");
            Assert.That(smoothedData, Is.Not.Null);
            Assert.That(smoothedData, Is.Not.Empty);
        }

        [Test]
        public void ClearSmoothedData_ResetsSmoothedData()
        {
            _mainWindow.GenerateData();
            _mainWindow.ApplySmoothing();
            _mainWindow.ClearSmoothedData();

            var rightCanvas = GetPrivateField<System.Windows.Controls.Canvas>(_mainWindow, "RightCanvas");
            Assert.That(rightCanvas.Children.Count, Is.EqualTo(0));
        }

        private T GetPrivateField<T>(object obj, string fieldName)
        {
            var field = obj.GetType().GetField(fieldName,
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            return (T)field.GetValue(obj);
        }

        [TearDown]
        public void TearDown()
        {
            _mainWindow?.Close();
        }
    }
}