using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using MonoGame_Sim_Test.Shapes;

namespace MonoGame_Sim_Test.Collision
{
    public static class Collision
    { // implementation of: http://www.jeffreythompson.org/collision-detection/index.php

        public static bool Point_Circle(Vector2 point, Circle circle)
        {
            return Point_Circle(point, circle.Center, circle.Radius);
        }

        public static bool Point_Circle(Vector2 point, Vector2 circle_Center, float radius)
        {
            if (Dist(point, circle_Center) <= radius)
                return true;

            return false;
        }

        public static bool Circle_Circle(Circle circle_1, Circle circle_2)
        {
            return Circle_Circle(circle_1.Center, circle_1.Radius, circle_2.Center, circle_2.Radius);
        }

        public static bool Circle_Circle(Vector2 circle1_Center, float circle1_Radius, Vector2 circle2_Center, float circle2_Radius)
        {
            double Distance = Dist(circle1_Center, circle2_Center);
            if (Distance <= circle1_Radius + circle2_Radius)
                return true;

            return false;
        }

        public static bool Point_Rect(Vector2 point, Rect rect)
        {
            return Point_Rect(point, rect.Get_Rectangle());
        }

        public static bool Point_Rect(Vector2 point, Rectangle rect)
        {
            if (point.X >= rect.X &&
                point.X <= rect.X + rect.Width &&
                point.Y >= rect.Y &&
                point.Y <= rect.Y + rect.Height)
                return true;

            return false;
        }

        public static bool Rect_Rect(Rect rect_1, Rect rect_2)
        {
            return Rect_Rect(rect_1.Get_Rectangle(), rect_2.Get_Rectangle());
        }

        public static bool Rect_Rect(Rectangle rect_1, Rectangle rect_2)
        {
            if (rect_1.X + rect_1.Width >= rect_2.X && // r1 right edge past r2 left
                 rect_1.X <= rect_2.X + rect_2.Width && // r1 left edge past r2 right
                 rect_1.Y + rect_1.Height >= rect_2.Y && // r1 top edge past r2 bottom
                 rect_1.Y <= rect_2.Y + rect_2.Height) // r1 bottom edge past r2 top
                return true;

            return false;
        }

        public static bool Circle_Rect(Circle circle, Rect rect)
        {
            return Circle_Rect(circle.Center, circle.Radius, rect.Get_Rectangle());
        }

        public static bool Circle_Rect(Vector2 circle_Center, float circle_Radius, Rectangle rect)
        {
            float Closest_Edge_X = circle_Center.X;
            float Closest_Edge_Y = circle_Center.Y;

            // which edge is closest?
            if (circle_Center.X < rect.X) Closest_Edge_X = rect.X; // test left edge
            else if (circle_Center.X > rect.X + rect.Width) Closest_Edge_X = rect.X + rect.Width; //right edge

            if (circle_Center.Y < rect.Y) Closest_Edge_Y = rect.Y; //top edge
            else if (circle_Center.Y > rect.Y + rect.Height) Closest_Edge_Y = rect.Y + rect.Height; // bottom edge

            // get distance from closest edges
            double distance = Dist(circle_Center.X, circle_Center.Y, Closest_Edge_X, Closest_Edge_Y);

            if (distance <= circle_Radius)
                return true;

            return false;
        }

        public static bool Point_Line(Vector2 point, Line line, float slack = 0.1f)
        {
            return Point_Line(point, line, slack);
        }

        public static bool Point_Line(Vector2 point, Vector2 line_Start, Vector2 line_End, float slack = 0.1f)
        {
            // get distance from the point to the two ends of the line
            double dist_Start = Dist(point, line_Start);
            double dist_End = Dist(point, line_End);

            double line_Length = Dist(line_Start, line_End);

            //slack is needed else we only get collision when perfectly on the line
            if (dist_Start + dist_End >= line_Length - slack &&
                dist_Start + dist_End <= line_Length + slack)
                return true;

            return false;
        }

        public static bool Line_Circle(Line line, Circle circle)
        {
            return Line_Circle(line.Start, line.End, circle.Center, circle.Radius);
        }

        public static bool Line_Circle(Vector2 line_Start, Vector2 line_End, Vector2 circle_Center, float circle_Radius)
        {
            // is either end INSIDE the circle?
            bool inside_Start = Point_Circle(line_Start, circle_Center, circle_Radius);
            bool inside_End = Point_Circle(line_End, circle_Center, circle_Radius);
            if (inside_Start || inside_End)
                return true;

            // get dot product of the line and circle
            double dot = Dot(line_Start, line_End, circle_Center);

            // find the closest point on the line
            double closest_X = line_Start.X + (dot * (line_End.X - line_Start.X));
            double closest_Y = line_Start.Y + (dot * (line_End.Y - line_Start.Y));

            // is this point actually on the line segment?
            // if so keep going, but if not, return false
            bool onSegment = Point_Line(new Vector2((float)closest_X, (float)closest_Y), line_Start, line_End);
            if (!onSegment) return false;

            // get distance to closest point (from line to circle_Center)
            double distance = Dist((float)closest_X, (float)closest_Y, circle_Center.X, circle_Center.Y);

            if (distance <= circle_Radius)
                return true;

            return false;
        }

        public static bool Line_Line(Line line_1, Line line_2)
        {
            return Line_Line(line_1.Start, line_1.End, line_2.Start, line_2.End);
        }

        public static bool Line_Line(Vector2 line_1_Start, Vector2 line_1_End, Vector2 line_2_Start, Vector2 line_2_End)
        {
            // calculate the distance to intersection point
            double uA = ((line_2_End.X - line_2_Start.X) * (line_1_Start.Y - line_2_Start.Y) -
                            (line_2_End.Y - line_2_Start.Y) * (line_1_Start.X - line_2_Start.X))
                            /
                            ((line_2_End.Y - line_2_Start.Y) * (line_1_End.X - line_1_Start.X) -
                            (line_2_End.X - line_2_Start.X) * (line_1_End.Y - line_1_Start.Y));
            double uB = ((line_1_End.X - line_1_Start.X) * (line_1_Start.Y - line_2_Start.Y) -
                            (line_1_End.Y - line_1_Start.Y) * (line_1_Start.X - line_2_Start.X))
                            /
                            ((line_2_End.Y - line_2_Start.Y) * (line_1_End.X - line_1_Start.X) -
                            (line_2_End.X - line_2_Start.X) * (line_1_End.Y - line_1_Start.Y));

            // if uA and uB are between 0-1, lines are colliding
            if (uA >= 0 && uA <= 1 && uB >= 0 && uB <= 1)
            {
                return true;
            }
            return false;
        }

        public static Vector2? Line_Line_Intersection_Point(Line line_1, Line line_2)
        {
            return Line_Line_Intersection_Point(line_1.Start, line_1.End, line_2.Start, line_1.End);
        }

        public static Vector2? Line_Line_Intersection_Point(Vector2 line_1_Start, Vector2 line_1_End, Vector2 line_2_Start, Vector2 line_2_End)
        {
            // calculate the distance to intersection point
            double uA = ((line_2_End.X - line_2_Start.X) * (line_1_Start.Y - line_2_Start.Y) -
                            (line_2_End.Y - line_2_Start.Y) * (line_1_Start.X - line_2_Start.X))
                            /
                            ((line_2_End.Y - line_2_Start.Y) * (line_1_End.X - line_1_Start.X) -
                            (line_2_End.X - line_2_Start.X) * (line_1_End.Y - line_1_Start.Y));
            double uB = ((line_1_End.X - line_1_Start.X) * (line_1_Start.Y - line_2_Start.Y) -
                            (line_1_End.Y - line_1_Start.Y) * (line_1_Start.X - line_2_Start.X))
                            /
                            ((line_2_End.Y - line_2_Start.Y) * (line_1_End.X - line_1_Start.X) -
                            (line_2_End.X - line_2_Start.X) * (line_1_End.Y - line_1_Start.Y));

            // if uA and uB are between 0-1, lines are colliding
            if (uA >= 0 && uA <= 1 && uB >= 0 && uB <= 1)
            {
                double intersectionX = line_1_Start.X + (uA * (line_1_End.X - line_1_Start.X));
                double intersectionY = line_1_Start.Y + (uA * (line_1_End.Y - line_1_Start.Y));
                return new Vector2((float)intersectionX, (float)intersectionY);
                //return true;
            }
            return null;
        }

        public static bool Line_Rect(Line line, Rect rect)
        {
            return Line_Rect(line.Start, line.End, rect.Get_Rectangle());
        }

        public static bool Line_Rect(Vector2 line_1_Start, Vector2 line_1_End, Rectangle rect)
        {
            // check if the line has hit any of the rectangle's sides
            bool left = Line_Line(line_1_Start, line_1_End, new Vector2(rect.Left, rect.Top), new Vector2(rect.Left, rect.Bottom));
            bool right = Line_Line(line_1_Start, line_1_End, new Vector2(rect.Right, rect.Top), new Vector2(rect.Right, rect.Bottom));
            bool top = Line_Line(line_1_Start, line_1_End, new Vector2(rect.Left, rect.Top), new Vector2(rect.Right, rect.Top));
            bool bottom = Line_Line(line_1_Start, line_1_End, new Vector2(rect.Left, rect.Bottom), new Vector2(rect.Right, rect.Bottom));

            if (left || right || top || bottom)
                return true;

            return false;
        }

        public static List<Vector2> Line_Rect_Intersection_Points(Line line, Rect rect)
        {
            return Line_Rect_Intersection_Points(line.Start, line.End, rect.Get_Rectangle());
        }

        public static List<Vector2> Line_Rect_Intersection_Points(Vector2 line_1_Start, Vector2 line_1_End, Rectangle rect)
        {
            List<Vector2> intersections = new List<Vector2>();
            // check if the line has hit any of the rectangle's sides
            Vector2? left = Line_Line_Intersection_Point(line_1_Start, line_1_End, new Vector2(rect.Left, rect.Top), new Vector2(rect.Left, rect.Bottom));
            Vector2? right = Line_Line_Intersection_Point(line_1_Start, line_1_End, new Vector2(rect.Right, rect.Top), new Vector2(rect.Right, rect.Bottom));
            Vector2? top = Line_Line_Intersection_Point(line_1_Start, line_1_End, new Vector2(rect.Left, rect.Top), new Vector2(rect.Right, rect.Top));
            Vector2? bottom = Line_Line_Intersection_Point(line_1_Start, line_1_End, new Vector2(rect.Left, rect.Bottom), new Vector2(rect.Right, rect.Bottom));

            if (left.HasValue || right.HasValue || top.HasValue || bottom.HasValue)
            {
                if (left.HasValue)
                    intersections.Add(left.Value);

                if (right.HasValue)
                    intersections.Add(right.Value);

                if (top.HasValue)
                    intersections.Add(top.Value);

                if (bottom.HasValue)
                    intersections.Add(bottom.Value);

                return intersections;
            }
            return null;
        }

        public static bool Point_Poly(Vector2 point, Poly poly)
        {
            return Point_Poly(point, poly.Points);
        }

        public static bool Point_Poly(Vector2 point, List<Vector2> Polygon_Points)
        {
            bool collision = false;
            for (int current_index = 0; current_index < Polygon_Points.Count; current_index++)
            {
                int next_index = current_index + 1;
                if (next_index == Polygon_Points.Count)
                    next_index = 0;

                Vector2 current = Polygon_Points[current_index]; //current vector
                Vector2 next = Polygon_Points[next_index]; //next vector

                if (((current.Y > point.Y) != (next.Y > point.Y))
                    && (point.X < (next.X - current.X) * (point.Y - current.Y) / (next.Y - current.Y) + current.X))
                {
                    collision = !collision;
                }
            }
            return collision;
        }

        public static bool Circle_Poly(Circle circle, Poly poly, bool Check_Inside)
        {
            return Circle_Poly(circle.Center, circle.Radius, poly.Points, Check_Inside);
        }

        public static bool Circle_Poly(Vector2 circle_Center, float radius, List<Vector2> Polygon_Points, bool Check_Inside)
        {//Check_Inside needs to be true to not only check for collision on the edges
            for (int current_index = 0; current_index < Polygon_Points.Count; current_index++)
            {
                int next_index = current_index + 1;
                if (next_index == Polygon_Points.Count)
                    next_index = 0;

                Vector2 current = Polygon_Points[current_index]; //current vector
                Vector2 next = Polygon_Points[next_index]; //next vector

                bool collision = Line_Circle(current, next, circle_Center, radius);
                if (collision)
                    return true;
            }
            if (Check_Inside) //check if this should be in the loop
            {
                bool inside = Point_Poly(circle_Center, Polygon_Points);
                return inside;
            }

            return false;
        }

        public static bool Rect_Poly(Rect rect, Poly poly, bool Check_Inside)
        {
            return Rect_Poly(rect.Get_Rectangle(), poly.Points, Check_Inside);
        }

        public static bool Rect_Poly(Rectangle rectangle, List<Vector2> Polygon_Points, bool Check_Inside)
        {//Check_Inside needs to be true to not only check for collision on the edges
            for (int current_index = 0; current_index < Polygon_Points.Count; current_index++)
            {
                int next_index = current_index + 1;
                if (next_index == Polygon_Points.Count)
                    next_index = 0;

                Vector2 current = Polygon_Points[current_index]; //current vector
                Vector2 next = Polygon_Points[next_index]; //next vector

                bool collision = Line_Rect(current, next, rectangle);
                if (collision)
                    return true;
            }
            if (Check_Inside) //check if this should be in the loop
            {
                bool inside = Point_Poly(rectangle.Location.ToVector2(), Polygon_Points);
                return inside;
            }
            return false;
        }

        public static bool Line_Poly(Line line, Poly poly)
        {
            return Line_Poly(line.Start, line.End, poly.Points);
        }

        public static bool Line_Poly(Vector2 line_Start, Vector2 line_End, List<Vector2> Polygon_Points)
        {//doesn't have a inside check currently
            for (int current_index = 0; current_index < Polygon_Points.Count; current_index++)
            {
                int next_index = current_index + 1;
                if (next_index == Polygon_Points.Count)
                    next_index = 0;

                Vector2 current = Polygon_Points[current_index]; //current vector
                Vector2 next = Polygon_Points[next_index]; //next vector

                bool collision = Line_Line(current, next, line_Start, line_End);
                if (collision)
                    return true;
            }
            return false;
        }

        public static bool Poly_Poly(Poly poly_1, Poly poly_2, bool Check_Inside)
        {
            return Poly_Poly(poly_1.Points, poly_2.Points, Check_Inside);
        }

        public static bool Poly_Poly(List<Vector2> Polygon_1_Points, List<Vector2> Polygon_2_Points, bool Check_Inside)
        {
            for (int current_index = 0; current_index < Polygon_1_Points.Count; current_index++)
            {
                int next_index = current_index + 1;
                if (next_index == Polygon_1_Points.Count)
                    next_index = 0;

                Vector2 current = Polygon_1_Points[current_index]; //current vector
                Vector2 next = Polygon_1_Points[next_index]; //next vector

                bool collision = Line_Poly(current, next, Polygon_2_Points);
                if (collision)
                    return true;
            }
            if (Check_Inside && Polygon_2_Points.Count > 0) //check if this should be in the loop
            {
                bool collision = Point_Poly(Polygon_2_Points[0], Polygon_1_Points);
                return collision;
            }
            return false;
        }

        public static bool Point_Tri(Vector2 point, Tri tri)
        {
            return Point_Tri(point, tri.Point_1, tri.Point_2, tri.Point_3);
        }

        public static bool Point_Tri(Vector2 point, Vector2 corner_1, Vector2 corner_2, Vector2 corner_3)
        {
            // get the area of the triangle
            float area_Tri = Area_Size(corner_1, corner_2, corner_3);

            // get the area of 3 triangles made between the point
            // and the corners of the triangle
            float area1 = Area_Size(point, corner_1, corner_2);
            float area2 = Area_Size(point, corner_2, corner_3);
            float area3 = Area_Size(point, corner_3, corner_1);

            // if the sum of the three areas equals the original,
            // we're inside the triangle!
            if (area1 + area2 + area3 == area_Tri)
                return true;

            return false;
        }

        public static double Dist(Vector2 p1, Vector2 p2)
        {//this fuction should be moved, as it is more general than only Collision
            return Dist(p1.X, p1.Y, p2.X, p2.Y);
        }

        public static double Dist(float X1, float Y1, float X2, float Y2)
        {
            double Distance_X = X1 - X2;
            double Distance_Y = Y1 - Y2;
            double Distance = Math.Sqrt((Distance_X * Distance_X) + (Distance_Y * Distance_Y));
            return Distance;
        }
        public static double Dot(Vector2 line_Start, Vector2 line_End, Vector2 point)
        {//return dot product
            double line_Lenght = Dist(line_Start, line_End);
            double Dot_Product = (((point.X - line_Start.X) * (line_End.X - line_Start.X)) +
                                 ((point.Y - line_Start.Y) * (line_End.Y - line_Start.Y)))
                                    / Math.Pow(line_Lenght, 2);
            return Dot_Product;
        }

        public static float Area_Size(Vector2 corner_1, Vector2 corner_2, Vector2 corner_3)
        {//https://en.wikipedia.org/wiki/Heron%27s_formula
            float area = Math.Abs((corner_2.X - corner_1.X) * (corner_3.Y - corner_1.Y) - (corner_3.X - corner_1.X) * (corner_2.Y - corner_1.Y));
            return area;
        }
    }

    public class Collision_Properties
    {
        protected Rect rectangle;
        public float PushPower = 0;
        public float StayPower = 0;

        public Collision_Properties(Rect rectangle, float PushPower, float StayPower)
        {
            this.rectangle = rectangle;
            this.PushPower = PushPower;
            this.StayPower = StayPower;
        }

        public Rect Get_Rectangle()
        {
            return rectangle;
        }
    }
}
