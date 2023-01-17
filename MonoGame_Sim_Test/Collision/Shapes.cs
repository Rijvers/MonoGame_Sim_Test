using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonoGame_Sim_Test.Shapes
{
    public class Circle
    {
        public Vector2 Center { get; private set; }
        public float Radius { get; private set; }

        public Circle(Vector2 position, float radius)
        {
            Set_Postion(position);
            Radius = radius;
        }

        public void Transform(Vector2 vector2)
        {
            Center += vector2;
        }

        public void Set_Postion(Vector2 Center)
        {
            this.Center = Center;
        }

        public void Increase_Radius(float increase)
        {
            Radius += increase;
        }

        public void Set_Radius(float radius)
        {
            Radius = radius;
        }
    }

    public class Rect
    {
        private Rectangle rectangle = new Rectangle();//KEEP PRIVATE

        public Vector2 position = Vector2.Zero; 
        public float X
        {
            get => position.X;
            set
            {
                position.X = value;
            }
        }
        public float Y
        {
            get => position.Y;
            set
            {
                position.Y = value;
            }
        }

        public int Width //transform to float
        {
            get => rectangle.Width;
            set
            {
                rectangle.Width = value;
            }
        }
        public int Height //transform to float
        {
            get => rectangle.Height;
            set
            {
                rectangle.Height = value;
            }
        }

        public Rect(Rectangle rectangle)
        {
            Set_Rectangle(rectangle);
        }

        public Rect(Rectangle rectangle, Vector2 position)
        {
            Set_Rectangle(rectangle, position);
        }

        public void Set_Rectangle(Rectangle rectangle, Vector2 position)
        {
            this.rectangle = rectangle;
            Set_Position(position);
        }

        public void Set_Rectangle(Rectangle rectangle)
        {
            Set_Rectangle(rectangle, rectangle.Location.ToVector2());
        }

        public void Transform(Vector2 vector2)
        {
            position += vector2;
            rectangle.X += (int)position.X;
            rectangle.Y += (int)position.Y;
        }

        private void Set_Position(Vector2 position)
        {
            this.position = position;
            rectangle.X = (int)position.X;
            rectangle.Y = (int)position.Y;
        }

        public double Get_Distance(Point? point)
        {//distance from Bottom middle
            return Math.Sqrt(Math.Pow(rectangle.X + rectangle.Width / 2 - point.Value.X, 2) +
                Math.Pow(rectangle.Y + rectangle.Height - point.Value.Y, 2));
        }

        public Rectangle Get_Rectangle()
        {
            rectangle.X = (int)position.X;
            rectangle.Y = (int)position.Y;
            return rectangle;
        }

        public Vector2 Get_Center()
        {
            return new Vector2(rectangle.X + rectangle.Width / 2, rectangle.Y + rectangle.Height / 2);
        }
    }

    public class Line
    {
        public Vector2 Start { get; private set; }
        public Vector2 End { get; private set; }

        public Line(Vector2 Point_1, Vector2 Point_2)
        {
            Set_Points(Point_1, Point_2);
        }

        public void Set_Point1(Vector2 Point_1)
        {
            this.Start = Point_1;
        }

        public void Set_Point2(Vector2 Point_2)
        {
            this.End = Point_2;
        }

        public void Set_Points(Vector2 Point_1, Vector2 Point_2)
        {
            Set_Point1(Point_1);
            Set_Point2(Point_2);
        }

        public void Transform_Point1(Vector2 vector2)
        {
            Start += vector2;
        }

        public void Transform_Point2(Vector2 vector2)
        {
            End += vector2;
        }

        public void Transform_Points(Vector2 Point_1_vector2, Vector2 Point_2_vector2)
        {
            Start += Point_1_vector2;
            End += Point_2_vector2;
        }
    }

    public class Poly
    {
        public List<Vector2> Points { get; private set; }

        public Poly(List<Vector2> Points)
        {
            Set_Position(Points);
        }

        public void Transform(Vector2 vector2)
        {
            for (int i = 0; i < Points.Count; i++)
            {
                Points[i] += vector2;
            }
        }

        public void Set_Position(List<Vector2> Points)
        {
            this.Points = Points; this.Points = Points;
        }
    }

    public class Tri
    {
        public Vector2 Point_1 { get; private set; }
        public Vector2 Point_2 { get; private set; }
        public Vector2 Point_3 { get; private set; }

        public Tri(Vector2 Point_1, Vector2 Point_2, Vector2 Point_3)
        {
            Set_Position(Point_1, Point_2, Point_3);
        }

        public void Transform(Vector2 vector2)
        {
            Point_1 += vector2;
            Point_2 += vector2;
            Point_3 += vector2;
        }

        public void Set_Position(Vector2 Point_1, Vector2 Point_2, Vector2 Point_3)
        {
            this.Point_1 = Point_1;
            this.Point_2 = Point_2;
            this.Point_3 = Point_3;
        }
    }
}
