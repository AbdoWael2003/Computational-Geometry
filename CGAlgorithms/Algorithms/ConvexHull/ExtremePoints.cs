using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class ExtremePoints : Algorithm
    {
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {

            HelperMethods.RemoveDuplicatePoints(ref points);

            int number_of_points = points.Count;
            for (int p = 0; p < number_of_points; p++)
            {
                bool extreme_points = true;

                for (int j = 0; j < number_of_points; j++)
                {
                    if (!extreme_points) break;
                    if (points[j].Equals(points[p])) continue;

                    for (int k = 0; k < number_of_points; k++)
                    {
                        if (!extreme_points) break;
                        if (points[k].Equals(points[p]) || points[k].Equals(points[j])) continue;

                        for (int l = 0; l < number_of_points; l++)
                        {
                            if (!extreme_points) break;
                            if (points[l].Equals(points[p]) || points[l].Equals(points[j]) || points[l].Equals(points[k])) continue;

                            if (HelperMethods.PointInTriangle(points[p], points[j], points[k], points[l]) != Enums.PointInPolygon.Outside)
                            {
                                extreme_points = false;
                                break;
                            }
                        }
                    }
                }
                if (extreme_points) outPoints.Add(points[p]);
            }
        }

        public override string ToString()
        {
            return "Convex Hull - Extreme Points";
        }
    }
}
