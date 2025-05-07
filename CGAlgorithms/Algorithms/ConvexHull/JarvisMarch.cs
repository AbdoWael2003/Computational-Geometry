using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class JarvisMarch : Algorithm
    {
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
            HelperMethods.RemoveDuplicatePoints(ref points);

            // finding an edge point (Top, Bottom, Right, Left) and then wrapping clockwise or counter clock wise accordingly!

            // we will choose the bottom point and then the will process the convex hull clock wise

            // finding the bottom most point
            int bottom_most_point_index = 0;
            for (int i = 1; i < points.Count; i++)
            {
                if (points[i].Y < points[bottom_most_point_index].Y)
                    bottom_most_point_index = i;
            }


            outPoints.Add(points[bottom_most_point_index]);

            int current_point_index = bottom_most_point_index;

            int next_index = (bottom_most_point_index + 1) % points.Count;


            Point candidate_point = points[next_index];
            

            for(int j = 0; j < points.Count; j++)
            {

                // iterating over the points to find the next one!
                for(int i = 0; i < points.Count; i++)
                {
                    if (i == next_index || i == current_point_index) continue;
                    
                    if (
                        HelperMethods.CheckTurn(new Line(points[current_point_index], candidate_point), points[i]) == Enums.TurnType.Right
                        ||
                         HelperMethods.CheckTurn(new Line(points[current_point_index], candidate_point), points[i]) == Enums.TurnType.Colinear 
                         && 
                         HelperMethods.EuclideanDistance(points[current_point_index], points[i]) >= HelperMethods.EuclideanDistance(points[current_point_index], candidate_point)
                    )
                    {
                        candidate_point = points[i];
                        next_index = i;
                    }  
                }

                // the candidate point at this stage is the right most point from the last point so we're sure that it's an extreme point!
                if (candidate_point.Equals(outPoints[0])) break;

                outPoints.Add(candidate_point);

                current_point_index = next_index;

                next_index = (next_index + 1) % points.Count;
                candidate_point = points[next_index];
            }
            
           

        }

        public override string ToString()
        {
            return "Convex Hull - Jarvis March";
        }
    }
}
