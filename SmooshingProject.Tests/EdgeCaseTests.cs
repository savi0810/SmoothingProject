using NUnit.Framework;
using SmoothingProject.Services;
using System;
using System.Collections.Generic;

namespace SmoothingProject.Tests
{
    [TestFixture]
    public class EdgeCaseTests
    {
        private GaussianSmoother _smoother;
        private DataService _dataService;

        [SetUp]
        public void SetUp()
        {
            _smoother = new GaussianSmoother();
            _dataService = new DataService();
        }

        [Test]
        public void GaussianSmoother_WithSigmaZero_ThrowsArgumentException()
        {
            var data = new List<double> { 1.0, 2.0, 3.0 };
            const double sigma = 0;
            Assert.Throws<ArgumentException>(() => _smoother.Smooth(data, 3, sigma));
        }

        [Test]
        public void GaussianSmoother_WithVerySmallSigma_HandlesCorrectly()
        {
            var data = new List<double> { 1.0, 2.0, 3.0 };
            const double sigma = 1e-10;
            Assert.DoesNotThrow(() => _smoother.Smooth(data, 3, sigma));
        }

        [Test]
        public void DataService_WithVeryLargeCount_DoesNotThrow()
        {
            const int largeCount = 1000000;
            Assert.DoesNotThrow(() => _dataService.GenerateSampleData(largeCount));
        }
    }
}