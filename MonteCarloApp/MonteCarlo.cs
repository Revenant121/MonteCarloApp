using System;
using System.Collections.Generic;

namespace MonteCarloApp
{
    public static class MonteCarlo
    {
        public const double x0 = 3;
        public const double y0 = 0;
        public const double R = 2;
        public const double C = -1;

        public static (double area, List<double> xs, List<double> ys, List<bool> mask) Run(int numPoints)
        {
            var rnd = new Random();
            var xs = new List<double>();
            var ys = new List<double>();
            var mask = new List<bool>();
            int countInside = 0;

            double xMin = x0 - R, xMax = x0 + R;
            double yMin = y0 - R, yMax = y0 + R;

            int countAbove = 0;
            int countBelow = 0;
            var maskAbove = new List<bool>();
            var maskBelow = new List<bool>();

            for (int i = 0; i < numPoints; i++)
            {
                double x = rnd.NextDouble() * (xMax - xMin) + xMin;
                double y = rnd.NextDouble() * (yMax - yMin) + yMin;

                xs.Add(x);
                ys.Add(y);

                double dx = x - x0;
                double dy = y - y0;
                bool inCircle = dx * dx + dy * dy <= R * R;

                bool above = inCircle && y >= C;
                bool below = inCircle && y < C;

                maskAbove.Add(above);
                maskBelow.Add(below);
            }

            for (int i = 0; i < numPoints; i++)
            {
                if (maskAbove[i]) countAbove++;
                if (maskBelow[i]) countBelow++;
            }

            for (int i = 0; i < numPoints; i++)
            {
                bool inSegment = countAbove > countBelow ? maskAbove[i] : maskBelow[i];
                mask.Add(inSegment);
                if (inSegment) countInside++;
            }

            double rectangleArea = (xMax - xMin) * (yMax - yMin);
            double estimatedArea = rectangleArea * countInside / numPoints;

            return (estimatedArea, xs, ys, mask);
        }
    }
}
