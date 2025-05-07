using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGUtilities
{
    public class HelperMethods
    {
        public static Enums.PointInPolygon PointInTriangle(Point p, Point a, Point b, Point c)
        {
            if (a.Equals(b) && b.Equals(c))
            {
                if (p.Equals(a) || p.Equals(b) || p.Equals(c))
                    return Enums.PointInPolygon.OnEdge;
                else
                    return Enums.PointInPolygon.Outside;
            }

            Line ab = new Line(a, b);
            Line bc = new Line(b, c);
            Line ca = new Line(c, a);

            if (GetVector(ab).Equals(Point.Identity)) return (PointOnSegment(p, ca.Start, ca.End)) ? Enums.PointInPolygon.OnEdge : Enums.PointInPolygon.Outside;
            if (GetVector(bc).Equals(Point.Identity)) return (PointOnSegment(p, ca.Start, ca.End)) ? Enums.PointInPolygon.OnEdge : Enums.PointInPolygon.Outside;
            if (GetVector(ca).Equals(Point.Identity)) return (PointOnSegment(p, ab.Start, ab.End)) ? Enums.PointInPolygon.OnEdge : Enums.PointInPolygon.Outside;

            if (CheckTurn(ab, p) == Enums.TurnType.Colinear)
                return PointOnSegment(p, a, b)? Enums.PointInPolygon.OnEdge : Enums.PointInPolygon.Outside;
            if (CheckTurn(bc, p) == Enums.TurnType.Colinear && PointOnSegment(p, b, c))
                return PointOnSegment(p, b, c) ? Enums.PointInPolygon.OnEdge : Enums.PointInPolygon.Outside;
            if (CheckTurn(ca, p) == Enums.TurnType.Colinear && PointOnSegment(p, c, a))
                return PointOnSegment(p, a, c) ? Enums.PointInPolygon.OnEdge : Enums.PointInPolygon.Outside;

            if (CheckTurn(ab, p) == CheckTurn(bc, p) && CheckTurn(bc, p) == CheckTurn(ca, p))
                return Enums.PointInPolygon.Inside;
            return Enums.PointInPolygon.Outside;
        }
        public static Enums.TurnType CheckTurn(Point vector1, Point vector2)
        {
            double result = CrossProduct(vector1, vector2);
            if (result < 0) return Enums.TurnType.Right;
            else if (result > 0) return Enums.TurnType.Left;
            else return Enums.TurnType.Colinear;
        }
        public static double CrossProduct(Point a, Point b)
        {
            return a.X * b.Y - a.Y * b.X;
        }
        public static bool PointOnRay(Point p, Point a, Point b)
        {
            if (a.Equals(b)) return true;
            if (a.Equals(p)) return true;
            var q = a.Vector(p).Normalize();
            var w = a.Vector(b).Normalize();
            return q.Equals(w);
        }
        public static bool PointOnSegment(Point p, Point a, Point b)
        {
            if (a.Equals(b))
                return p.Equals(a);

            if (b.X == a.X)
                return p.X == a.X && (p.Y >= Math.Min(a.Y, b.Y) && p.Y <= Math.Max(a.Y, b.Y));
            if (b.Y == a.Y)
                return p.Y == a.Y && (p.X >= Math.Min(a.X, b.X) && p.X <= Math.Max(a.X, b.X));
            double tx = (p.X - a.X) / (b.X - a.X);
            double ty = (p.Y - a.Y) / (b.Y - a.Y);

            return (Math.Abs(tx - ty) <= Constants.Epsilon && tx <= 1 && tx >= 0);
        }
        /// <summary>
        /// Get turn type from cross product between two vectors (l.start -> l.end) and (l.end -> p)
        /// </summary>
        /// <param name="l"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public static Enums.TurnType CheckTurn(Line l, Point p)
        {
            Point a = l.Start.Vector(l.End);
            Point b = l.End.Vector(p);
            return HelperMethods.CheckTurn(a, b);
        }
        public static Point GetVector(Line l)
        {
            return l.Start.Vector(l.End);
        }


        //=============================================================
        // My Methods:


        public static Point getNextPoint(Point value, List<Point> points)
        {
            return points[(points.IndexOf(value) + 1) % points.Count];
        }

        public static Point getPrevPoint(Point value, List<Point> points)
        {
            return (points.IndexOf(value) == 0 ? points[points.Count - 1] : points[points.IndexOf(value) - 1]);
        }



        public static void RemoveDuplicatePoints(ref List<Point> points)
        {
            Dictionary<KeyValuePair<double, double>, bool> duplicate_points = new Dictionary<KeyValuePair<double, double>, bool>();

            for (int i = 0; i < points.Count; i++)
            {
                Point temp_point = points[i];
                if (duplicate_points.ContainsKey(new KeyValuePair<double, double>(temp_point.X, temp_point.Y)))
                {
                    points.Remove(temp_point);
                    i--;
                }
                else
                    duplicate_points.Add(new KeyValuePair<double, double>(temp_point.X, temp_point.Y), true);
            }
        }

        public static double Slope(Line l)
        {
            return (l.End.Y - l.Start.Y) / (l.End.X - l.Start.X);
        }
        public static double EuclideanDistance(Point p1, Point p2)
        {
            return Math.Sqrt((p2.X - p1.X) * (p2.X - p1.X) + (p2.Y - p1.Y) * (p2.Y - p1.Y));
        }

        public static double AngleBetweenTwoLines(Line firstLine, Line secondLine)
        {
            double m1 = Math.Atan2((firstLine.End.Y - firstLine.Start.Y), (firstLine.Start.X - firstLine.End.X));
            double m2 = Math.Atan2((secondLine.End.Y - secondLine.Start.Y), (secondLine.Start.X - secondLine.End.X));
            return m1 - m2;
        }

        public static bool PointOnLine(Point p, Line l)
        {
            /// <summary>
            /// checks whether the point p is inside the line l between l.start and l.end or not
            /// </summary>
            /// <param name="p"></param>
            /// <param name="l"></param>
            /// <returns>bool</returns>

            if (
                EuclideanDistance(p, l.Start) + EuclideanDistance(p, l.End) == EuclideanDistance(l.Start, l.End)
                &&
                Math.Abs(Slope(l) - Slope(new Line(l.Start, p))) <= Constants.Epsilon
            )
                return true;
            return false;
        }

        public static double AreaOfTriangle(Point v1, Point v2)
        {
            return Math.Abs(HelperMethods.CrossProduct(v1, v2) / 2);
        }

        public static int PointsComparator(Point x1, Point x2, Point reference_point)
        {
            // +ve means x1 > x2
            // -ve means x1 < x2
            // 0  (the difference is zero) means they're equal!


            // Calculate the cross product of (bottom_most_point -> x1) and (bottom_most_point -> x2)
            double crossProduct = (x1.X - reference_point.X) * (x2.Y - reference_point.Y)
                             - (x1.Y - reference_point.Y) * (x2.X - reference_point.X);

            if (crossProduct > 0) return -1; // x1 is counterclockwise to x2
            if (crossProduct < 0) return 1;  // x1 is clockwise to x2

            // If collinear, sort by distance to the bottom_most_point
            double distance1 = (x1.X - reference_point.X) * (x1.X - reference_point.X)
                          + (x1.Y - reference_point.Y) * (x1.Y - reference_point.Y);
            double distance2 = (x2.X - reference_point.X) * (x2.X - reference_point.X)
                          + (x2.Y - reference_point.Y) * (x2.Y - reference_point.Y);

            return distance1.CompareTo(distance2);
        }

        public static void CounterClockWiseSort(Point reference_point,ref List<Point> points)
        {
            points.Sort((x1, x2) => HelperMethods.PointsComparator(x1, x2, reference_point));
        }
     



     
     

        //=============================================================
    }
}
