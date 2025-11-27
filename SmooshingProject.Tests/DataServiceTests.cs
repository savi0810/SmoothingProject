using NUnit.Framework;
using SmoothingProject.Services;
using System.Collections.Generic;
using System.Linq;

namespace SmoothingProject.Tests
{
    [TestFixture]
    public class DataServiceTests
    {
        private DataService _dataService;

        [SetUp]
        public void SetUp()
        {
            _dataService = new DataService();
        }

        [Test]
        public void GenerateSampleData_WithPositiveCount_ReturnsCorrectNumberOfItems()
        {
            const int count = 100;
            var result = _dataService.GenerateSampleData(count);
            Assert.That(result, Has.Count.EqualTo(count));
        }

        [Test]
        public void GenerateSampleData_WithZeroCount_ReturnsEmptyList()
        {
            const int count = 0;
            var result = _dataService.GenerateSampleData(count);
            Assert.That(result, Is.Empty);
        }

        [Test]
        public void GenerateSampleData_WithNegativeCount_ThrowsArgumentException()
        {
            const int count = -10;
            Assert.Throws<ArgumentException>(() => _dataService.GenerateSampleData(count));
        }

        [Test]
        public void GenerateSampleData_GeneratedValues_AreWithinExpectedRange()
        {
            const int count = 1000;
            var result = _dataService.GenerateSampleData(count);
            Assert.That(result, Has.All.InRange(-50.0, 50.0));
        }

        [Test]
        public void GenerateSampleData_GeneratedValues_HaveReasonableDistribution()
        {
            const int count = 10000;
            var result = _dataService.GenerateSampleData(count);
            var average = result.Average();
            Assert.That(average, Is.InRange(-5.0, 5.0)); 
        }

        [Test]
        public void GenerateSampleData_MultipleCalls_ReturnDifferentResults()
        {
            const int count = 100;
            var result1 = _dataService.GenerateSampleData(count);
            var result2 = _dataService.GenerateSampleData(count);
            Assert.That(result1, Is.Not.EqualTo(result2));
        }
    }
}
