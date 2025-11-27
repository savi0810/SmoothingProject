using System;
using System.Collections.Generic;

namespace SmoothingProject.Services
{
    public class DataService
    {
        private readonly Random random = new Random();

        public List<double> GenerateSampleData(int count)
        {
            if (count < 0)
                throw new ArgumentException("Count cannot be negative", nameof(count));

            var data = new List<double>();
            // double value = 0;

            for (int i = 0; i < count; i++)
            {
                // value += random.NextDouble() - 0.5;
                // data.Add(value);
                data.Add((random.NextDouble() - 0.5) * 100);
            }

            return data;
        }
    }
}