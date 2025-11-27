using NUnit.Framework;
using SmoothingProject.Services;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace SmoothingProject.Tests
{
    [TestFixture]
    public class GaussianSmootherTests
    {
        private GaussianSmoother _smoother;

        [SetUp]
        public void SetUp()
        {
            _smoother = new GaussianSmoother();
        }

        [Test]
        public void Smooth_WithNullData_ThrowsArgumentNullException()
        {
            List<double> nullData = null;
            Assert.Throws<ArgumentNullException>(() => _smoother.Smooth(nullData, 5, 1.0));
        }

        [Test]
        public void Smooth_WithEmptyData_ReturnsEmptyList()
        {
            var emptyData = new List<double>();
            var result = _smoother.Smooth(emptyData, 5, 1.0);
            Assert.That(result, Is.Empty);
        }

        [Test]
        public void Smooth_WithNegativeWindowSize_ThrowsArgumentException()
        {
            var data = new List<double> { 1.0, 2.0, 3.0 };
            Assert.Throws<ArgumentException>(() => _smoother.Smooth(data, -1, 1.0));
        }

        [Test]
        public void Smooth_WithZeroWindowSize_ThrowsArgumentException()
        {
            var data = new List<double> { 1.0, 2.0, 3.0 };
            Assert.Throws<ArgumentException>(() => _smoother.Smooth(data, 0, 1.0));
        }

        [Test]
        public void Smooth_WithNegativeSigma_ThrowsArgumentException()
        {
            var data = new List<double> { 1.0, 2.0, 3.0 };
            Assert.Throws<ArgumentException>(() => _smoother.Smooth(data, 3, -1.0));
        }

        [Test]
        public void Smooth_WithZeroSigma_ThrowsArgumentException()
        {
            var data = new List<double> { 1.0, 2.0, 3.0 };
            Assert.Throws<ArgumentException>(() => _smoother.Smooth(data, 3, 0));
        }

        [Test]
        public void Smooth_WithSingleDataPoint_ReturnsSamePoint()
        {
            var data = new List<double> { 42.5 };
            const int windowSize = 5;
            const double sigma = 1.0;
            var result = _smoother.Smooth(data, windowSize, sigma);

            Assert.That(result, Has.Count.EqualTo(1));
            Assert.That(result[0], Is.EqualTo(42.5).Within(1e-10));
        }

        [Test]
        public void Smooth_WithConstantData_ReturnsSameData()
        {
            var data = new List<double> { 10.0, 10.0, 10.0, 10.0, 10.0 };
            const int windowSize = 3;
            const double sigma = 1.0;
            var result = _smoother.Smooth(data, windowSize, sigma);
            Assert.That(result, Has.All.EqualTo(10.0).Within(1e-10));
        }

        [Test]
        public void Smooth_WithValidData_ReturnsSmoothedDataSameLength()
        {
            var data = new List<double> { 1.0, 2.0, 3.0, 4.0, 5.0, 4.0, 3.0, 2.0, 1.0 };
            const int windowSize = 3;
            const double sigma = 1.0;
            var result = _smoother.Smooth(data, windowSize, sigma);
            Assert.That(result, Has.Count.EqualTo(data.Count));
        }

        [Test]
        public void CalculateGaussianWeight_WithZeroX_ReturnsPeakValue()
        {
            const int x = 0;
            const double sigma = 1.0;
            var weight = InvokeCalculateGaussianWeight(x, sigma);
            var expectedWeight = 1.0 / (Math.Sqrt(2 * Math.PI) * sigma);
            Assert.That(weight, Is.EqualTo(expectedWeight).Within(1e-10));
        }

        [Test]
        public void CalculateGaussianWeight_WithNegativeSigma_ThrowsArgumentException()
        {
            const int x = 0;
            const double sigma = -1.0;
            Assert.Throws<ArgumentException>(() => InvokeCalculateGaussianWeight(x, sigma));
        }

        private double InvokeCalculateGaussianWeight(int x, double sigma)
        {
            var method = typeof(GaussianSmoother).GetMethod("CalculateGaussianWeight",
                BindingFlags.NonPublic | BindingFlags.Instance);

            try
            {
                return (double)method.Invoke(_smoother, [x, sigma]);
            }
            catch (TargetInvocationException ex)
            {
                throw ex.InnerException ?? ex;
            }
        }
    }
}