using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class GrahamScan : Algorithm
    {
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
            HelperMethods.RemoveDuplicatePoints(ref points);
            if (points.Count == 1)
            {
                outPoints.Add(points[0]);
                return;
            }

            // finding the bottom most left most point
            Point bottom_most_point = points[0];
            for (int i = 0; i < points.Count; i++)
            {
                if (points[i].Y < bottom_most_point.Y || points[i].Y == bottom_most_point.Y && points[i].X < bottom_most_point.X)
                    bottom_most_point = points[i];
            }

            SortedDictionary<double, Point> slopes = new SortedDictionary<double, Point>();

            for(int i = 0; i < points.Count; i++)
            {
                double slope = (points[i].Y - bottom_most_point.Y) / (points[i].X - bottom_most_point.X);

                if (!slopes.ContainsKey(slope))
                    slopes[slope] = points[i]; // Adds the key with a value of 42
                else
                {
                    Point prev_point = slopes[slope];

                    double d_prev = HelperMethods.EuclideanDistance(bottom_most_point, prev_point);
                    double d_current_point = HelperMethods.EuclideanDistance(bottom_most_point, points[i]);

                    if (d_current_point > d_prev)
                    {
                        slopes[slope] = points[i];
                    }
                }
            }

            points = slopes.Values.ToList();


            Stack<Point> stack = new Stack<Point>();

            stack.Push(points[0]);
            points.RemoveAt(0);

            HelperMethods.CounterClockWiseSort(bottom_most_point, ref points);

            stack.Push(points[0]);
            points.RemoveAt(0);

            
            int next_point = 0; // after removing the first two points that represent the bottom most extreme segment
            while (next_point < points.Count)
            {
                Point p = stack.Pop();
                Point prev = stack.Peek();
                stack.Push(p);

                if (HelperMethods.CheckTurn(new Line(prev, p), points[next_point]) == Enums.TurnType.Left)
                {
                    stack.Push(points[next_point]);
                    next_point++;
                }
                else
                {
                    stack.Pop();
                }
            }

            while(stack.Count > 0)
            {
                outPoints.Add(stack.Pop());
            }

        }

        public override string ToString()
        {
            return "Convex Hull - Graham Scan";
        }
    }
}
