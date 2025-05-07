using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class Incremental : Algorithm
    {
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
            HelperMethods.RemoveDuplicatePoints(ref points);

            CGUtilities.DataStructures.OrderedSet<Tuple<double, int>> myset = new CGUtilities.DataStructures.OrderedSet<Tuple<double, int>>();

            if (points.Count < 3)
            {
                outPoints = points;
                return;
            }
            double K = 200;


            Point b = new Point((points[0].X + points[1].X) / 2.0, (points[0].Y + points[1].Y) / 2.0);
            Point B = new Point((b.X + points[2].X) / 2.0, (b.Y + points[2].Y) / 2.0);

            Point NB = new Point(B.X + K, B.Y);

            Line base_line = new Line(B, NB);

            double P0angle = HelperMethods.AngleBetweenTwoLines(base_line, new Line(B, points[0])); myset.Add(new Tuple<double, int>(P0angle, 0));
            double P1angle = HelperMethods.AngleBetweenTwoLines(base_line, new Line(B, points[1])); myset.Add(new Tuple<double, int>(P1angle, 1));
            double P2angle = HelperMethods.AngleBetweenTwoLines(base_line, new Line(B, points[2])); myset.Add(new Tuple<double, int>(P2angle, 2));


            for (int i = 3; i < points.Count; i++)
            {
                Point P = points[i];
                //To get pre and next
                double P_angle = HelperMethods.AngleBetweenTwoLines(base_line, new Line(B, P));

                KeyValuePair<Tuple<double, int>, Tuple<double, int>> myPair = new KeyValuePair<Tuple<double, int>, Tuple<double, int>>();

                myPair = myset.DirectUpperAndLower(new Tuple<double, int>(P_angle, i));
                Tuple<double, int> Pre = (myPair.Value == null) ? myset.GetLast() : myPair.Value;
                Tuple<double, int> Next = (myPair.Key == null) ? myset.GetFirst() : myPair.Key;


                if (HelperMethods.CheckTurn(new Line(points[Pre.Item2], points[Next.Item2]), P) == Enums.TurnType.Right)
                {

                    double pre_angle = HelperMethods.AngleBetweenTwoLines(base_line, new Line(B, points[Pre.Item2]));
                    KeyValuePair<Tuple<double, int>, Tuple<double, int>> prePair = new KeyValuePair<Tuple<double, int>, Tuple<double, int>>();
                    prePair = myset.DirectUpperAndLower(new Tuple<double, int>(pre_angle, Pre.Item2));
                    Tuple<double, int> new_pre = (prePair.Value == null) ? myset.GetLast() : prePair.Value;


                    //while left
                    while (HelperMethods.CheckTurn(new Line(P, points[Pre.Item2]), points[new_pre.Item2]) == Enums.TurnType.Left || HelperMethods.CheckTurn(new Line(P, points[Pre.Item2]), points[new_pre.Item2]) == Enums.TurnType.Colinear)
                    {
                        myset.Remove(Pre);
                        Pre = new_pre;

                        pre_angle = HelperMethods.AngleBetweenTwoLines(base_line, new Line(B, points[Pre.Item2]));
                        prePair = myset.DirectUpperAndLower(new Tuple<double, int>(pre_angle, Pre.Item2));

                        new_pre = (prePair.Value == null) ? myset.GetLast() : prePair.Value;

                    }


                    double next_angle = HelperMethods.AngleBetweenTwoLines(base_line, new Line(B, points[Next.Item2]));
                    KeyValuePair<Tuple<double, int>, Tuple<double, int>> nextPair = new KeyValuePair<Tuple<double, int>, Tuple<double, int>>();
                    nextPair = myset.DirectUpperAndLower(new Tuple<double, int>(next_angle, Next.Item2));
                    Tuple<double, int> newNext = (nextPair.Key == null) ? myset.GetFirst() : newNext = nextPair.Key;

                    while (HelperMethods.CheckTurn(new Line(P, points[Next.Item2]), points[newNext.Item2]) == Enums.TurnType.Right || HelperMethods.CheckTurn(new Line(P, points[Next.Item2]), points[newNext.Item2]) == Enums.TurnType.Colinear)
                    {
                        myset.Remove(Next);
                        Next = newNext;

                        next_angle = HelperMethods.AngleBetweenTwoLines(base_line, new Line(B, points[Next.Item2]));
                        nextPair = myset.DirectUpperAndLower(new Tuple<double, int>(next_angle, Next.Item2));

                        newNext = (nextPair.Key == null) ? myset.GetFirst() : nextPair.Key;

                    }

                    for (int x = 0; x < myset.Count; x++)
                    {
                        if (HelperMethods.PointInTriangle(points[myset.ElementAt(x).Item2], points[i], points[Pre.Item2], points[Next.Item2]) == Enums.PointInPolygon.Inside)
                        {
                            myset.Remove(myset.ElementAt(x));
                        }
                    }
                    myset.Add(new Tuple<double, int>(P_angle, i));
                }

            }
            for (int i = 0; i < myset.Count; i++)
            {
                outPoints.Add(points[myset.ElementAt(i).Item2]);
            }

        }
          


        public override string ToString()
        {
            return "Convex Hull - Incremental";
        }
    }
}
