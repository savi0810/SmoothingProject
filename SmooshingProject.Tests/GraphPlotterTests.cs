using NUnit.Framework;
using SmoothingProject.Services;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Linq;

namespace SmoothingProject.Tests
{
    [TestFixture]
    [Apartment(System.Threading.ApartmentState.STA)]
    public class GraphPlotterTests
    {
        private GraphPlotter _plotter;
        private Canvas _canvas;

        [SetUp]
        public void SetUp()
        {
            _plotter = new GraphPlotter();
            _canvas = new Canvas
            {
                Width = 800,
                Height = 600
            };

            _canvas.Measure(new Size(800, 600));
            _canvas.Arrange(new Rect(0, 0, 800, 600));
        }

        [Test]
        public void PlotPoints_WithNullCanvas_ThrowsArgumentNullException()
        {
            var data = new List<double> { 1.0, 2.0, 3.0 };
            Assert.Throws<ArgumentNullException>(() => _plotter.PlotPoints(null, data));
        }

        [Test]
        public void PlotPoints_WithNullData_ThrowsArgumentNullException()
        {
            List<double> nullData = null;
            Assert.Throws<ArgumentNullException>(() => _plotter.PlotPoints(_canvas, nullData));
        }

        [Test]
        public void PlotPoints_WithEmptyData_DoesNotThrow()
        {
            var emptyData = new List<double>();
            Assert.DoesNotThrow(() => _plotter.PlotPoints(_canvas, emptyData));
        }

        [Test]
        public void PlotPoints_WithZeroSizedCanvas_ThrowsInvalidOperationException()
        {
            var zeroCanvas = new Canvas { Width = 0, Height = 0 };
            var data = new List<double> { 1.0, 2.0, 3.0 };

            Assert.Throws<InvalidOperationException>(() => _plotter.PlotPoints(zeroCanvas, data));
        }

        [Test]
        public void PlotPoints_WithMultiplePoints_CreatesCorrectNumberOfLines()
        {
            var data = new List<double> { 1.0, 2.0, 3.0, 4.0, 5.0 };
            _plotter.PlotPoints(_canvas, data);
            var lines = _canvas.Children.OfType<Line>().ToList();
            Assert.That(lines, Has.Count.EqualTo(data.Count - 1));
        }

        [Test]
        public void ClearCanvas_WithPopulatedCanvas_RemovesAllChildren()
        {
            _canvas.Children.Add(new Rectangle { Width = 10, Height = 10 });
            var initialCount = _canvas.Children.Count;
            _plotter.ClearCanvas(_canvas);
            Assert.That(_canvas.Children.Count, Is.EqualTo(0));
        }

        [Test]
        public void ClearCanvas_WithNullCanvas_DoesNotThrow()
        {
            Assert.DoesNotThrow(() => _plotter.ClearCanvas(null));
        }
    }
}