using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class QuickHull : Algorithm
    {
        

        private static void EdgeHandeler(Line edge, ref List<Point> points, ref List<Point> outPoints)
        {
            if (edge.Start == edge.End) return;

            Point farmost_point = null;
            double perpendicular_distance = -1;

            for (int i = 0; i < points.Count; i++)
            {
                if (HelperMethods.CheckTurn(edge, points[i]) == Enums.TurnType.Left)
                {
                    double current_perpendicular_distance =
                        2 * HelperMethods.AreaOfTriangle(
                            points[i].Vector(edge.Start),
                            points[i].Vector(edge.End)
                        )
                        /
                        HelperMethods.EuclideanDistance(edge.Start, edge.End);
                    if (current_perpendicular_distance > perpendicular_distance)
                    {
                        farmost_point = points[i];
                        perpendicular_distance = current_perpendicular_distance;
                    }
                }
                else if (points[i] != edge.Start && points[i] != edge.End && HelperMethods.PointOnSegment(points[i], edge.Start, edge.End))
                {

                    points.RemoveAt(i);
                    i--;
                }
               
            
            }



            if(farmost_point != null) {

                for(int i = 0; i < points.Count; i++)
                {
                    if (points[i] == edge.Start || points[i] == edge.End || points[i] == farmost_point) 
                        continue;

                    if (HelperMethods.PointInTriangle(points[i], edge.Start, edge.End, farmost_point) == Enums.PointInPolygon.Inside || HelperMethods.PointInTriangle(points[i], edge.Start, edge.End, farmost_point) == Enums.PointInPolygon.OnEdge)
                    {
                        points.RemoveAt(i);
                        i--;
                    }
                }

                outPoints.Add(farmost_point);
                EdgeHandeler(new Line(edge.Start, farmost_point), ref points, ref outPoints);
                EdgeHandeler(new Line(farmost_point, edge.End), ref points, ref outPoints);
            }
        }
        
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
            HelperMethods.RemoveDuplicatePoints(ref points);

            Point left_most, right_most, top_most, bottom_most;
            left_most = right_most = top_most = bottom_most = points[0];

            for(int i = 1; i < points.Count; i++)
            {
                if (points[i].X < left_most.X) left_most = points[i];
                if (points[i].X > right_most.X) right_most = points[i];
                if (points[i].Y < bottom_most.Y) bottom_most = points[i];
                if (points[i].Y > top_most.Y) top_most = points[i];
            }

            for (int i = 0; i < points.Count; i++)
            {
                if (
                    !(
                        HelperMethods.PointInTriangle(points[i], left_most, top_most, right_most) == Enums.PointInPolygon.Outside &&
                        HelperMethods.PointInTriangle(points[i], left_most, bottom_most, right_most) == Enums.PointInPolygon.Outside
                    )
                )
                {
                    points.RemoveAt(i);
                    i--; // to undo the ++ after the loop to reach the next element correctly after the removing effect
                }
            }

            outPoints.Add(left_most);
            outPoints.Add(right_most);
            outPoints.Add(top_most);
            outPoints.Add(bottom_most);
            EdgeHandeler(new Line(left_most, top_most), ref points, ref outPoints);
            EdgeHandeler(new Line(top_most, right_most), ref points, ref outPoints);
            EdgeHandeler(new Line(right_most, bottom_most), ref points, ref outPoints);
            EdgeHandeler(new Line(bottom_most, left_most), ref points, ref outPoints);

            HelperMethods.RemoveDuplicatePoints(ref outPoints);

        }

        public override string ToString()
        {
            return "Convex Hull - Quick Hull";
        }
    }
}
