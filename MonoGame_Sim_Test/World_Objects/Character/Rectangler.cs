using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonoGame_Sim_Test.World_Objects
{
    public static class Rectangler
    {
        public enum Expanse_Direction
        {
            Vertical,
            Horizontal
        }

        public enum Corner
        {
            Top_Left,
            Top_Right,
            Bottom_Left,
            Bottom_Right
        }


        public static Rectangle[] Get_Rectangles(Point location, Point Size, int Amount = 4, Expanse_Direction Expanse_Direction = Expanse_Direction.Vertical)
        {
            Rectangle[] rectangles = new Rectangle[Amount];

            for (int i = 0; i < Amount; i++)
            {
                rectangles[i] = new Rectangle(location, Size);
                switch (Expanse_Direction)
                {
                    case Expanse_Direction.Vertical:
                        location.Y += Size.Y;
                        break;
                    case Expanse_Direction.Horizontal:
                        location.X += Size.X;
                        break;
                    default:
                        break;
                }
            }
            return rectangles;
        }

        public static Point GetPoint(Corner rectangle_Points, Rectangle rectangle)
        {
            return GetPoint(rectangle_Points, rectangle.Location, rectangle.Width, rectangle.Height);
        }

        public static Point GetPoint(Corner rectangle_Point, Point point, int rectangle_Width, int rectangle_Height)
        {
            return rectangle_Point switch
            {
                Corner.Top_Left => point,
                Corner.Top_Right => new Point(point.X + rectangle_Width, point.Y),
                Corner.Bottom_Left => new Point(point.X, point.Y + rectangle_Height),
                Corner.Bottom_Right => new Point(point.X + rectangle_Width, point.Y + rectangle_Height),
                _ => throw new System.NotImplementedException(),
            };
        }

        //public static List<Point> Pointillize(Rectangle rectangle, int Point_Spacing_Width, int Point_Spacing_Height)
        //{ //return all points that can be placed in the rectangle respecting the given spacing
        //  //first point is placed top left, Points that are on closest to the edge get placed on the edge

        //    List<Point> Points = new List<Point>();
        //    for (int x = rectangle.X; x <= rectangle.X + rectangle.Width; x += Point_Spacing_Width)
        //    {
        //        bool Edge_Point_X = false;
        //        if ( x + Point_Spacing_Width > rectangle.X + rectangle.Width)
        //            Edge_Point_X = true;

        //        for (int y = rectangle.Y; y < rectangle.Y + rectangle.Height; y += Point_Spacing_Height)
        //        {
        //            bool Edge_Point_Y = y + Point_Spacing_Height > rectangle.Y + rectangle.Height;

        //            int X_Point = x, Y_Point = y;
        //            if (Edge_Point_X)
        //            {
        //                X_Point = rectangle.X + rectangle.Width;
        //            }
        //            if (Edge_Point_Y)
        //            {
        //                Y_Point = rectangle.Y + rectangle.Height;
        //            }

        //            Points.Add(new Point(X_Point, Y_Point));
        //        }
        //    }
        //    return Points;
        //}
    }
}
