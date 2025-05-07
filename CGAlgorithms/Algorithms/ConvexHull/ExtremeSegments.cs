using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class ExtremeSegments : Algorithm
    {
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
            if (points.Count == 1)
            {
                outPoints = points;
                return;
            }
            HelperMethods.RemoveDuplicatePoints(ref points);
            int size = points.Count;

            // iterating over all the possible segments!
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (j == i) continue;
                    // points[i] -> points[j] is one possible segment

                    bool extreme_segment = true;

                    short parity = 2;
                    // parity = -1; the recent calculated cross product is negative!
                    // parity = 0; the recent calculated cross product is zero!
                    // parity = 1; the recent calculated cross product is positive!
                    // parity = 2; the recent calculated cross product is not calculated yet!


                    // we need to iterate over all the other points to check whether all the other points are on the other halfplane or not!
                    for (int test_point = 0; test_point < size; test_point++)
                    {
                        if (test_point == i || test_point == j) continue;

                        double signed_area = HelperMethods.CrossProduct(points[i].Vector(points[j]), points[j].Vector(points[test_point]));

                        short current_parity;
                        if (signed_area > 0) current_parity = 1; else if (signed_area < 0) current_parity = -1; else current_parity = 0;

                        if (parity == 2)
                        {
                            if (signed_area > 0) parity = 1; else if (signed_area < 0) parity = -1; else parity = 0;
                        }
                        if((current_parity == 0 && HelperMethods.PointOnLine(points[test_point], new Line(points[i], points[j]))))
                        {
                            current_parity = parity;
                        }

                        if(current_parity != parity)
                        {
                            extreme_segment = false;
                            break;
                        }
                        parity = current_parity;

                    }
                    if (extreme_segment)
                    {
                        outPoints.Add(points[i]);
                        outPoints.Add(points[j]);
                    }
                }
            }
            HelperMethods.RemoveDuplicatePoints(ref outPoints);

        }

        public override string ToString()
        {
            return "Convex Hull - Extreme Segments";
        }
    }
}
