using System;
using System.Collections.Generic;

namespace SmoothingProject.Services
{
    public class GaussianSmoother
    {
        public List<double> Smooth(List<double> data, int windowSize, double sigma)
        {
            if (data == null || data.Count == 0)
                return new List<double>();

            var result = new List<double>();
            int halfWindow = windowSize / 2;

            for (int i = 0; i < data.Count; i++)
            {
                double weightedSum = 0;
                double weightSum = 0;

                for (int j = -halfWindow; j <= halfWindow; j++)
                {
                    int dataIndex = i + j;
                    if (dataIndex >= 0 && dataIndex < data.Count)
                    {
                        double weight = CalculateGaussianWeight(j, sigma);
                        weightedSum += data[dataIndex] * weight;
                        weightSum += weight;
                    }
                }

                result.Add(weightedSum / weightSum);
            }

            return result;
        }

        private double CalculateGaussianWeight(int x, double sigma)
        {
            return Math.Exp(-(x * x) / (2 * sigma * sigma)) / (Math.Sqrt(2 * Math.PI) * sigma);
        }
    }
}