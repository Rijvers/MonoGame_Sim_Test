using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using MonoGame_Sim_Test.World_Objects;
using static MonoGame_Sim_Test.World_Objects.Rectangler;

namespace MonoGame_Sim_Test
{
    public static class Bucket_Controller
    {
        public const int Bucket_Width = 100, Bucket_Height = 100;
        private static readonly Dictionary<Point, Bucket> Bucket_Dic = new Dictionary<Point, Bucket>();
        private static readonly List<Bucket> Visible_buckets = new List<Bucket>();

        public static Bucket Get_Bucket(Point point)
        {
            Bucket_Check(point);
            return Bucket_Dic[point];
        }

        public static void Bucket_Check(Point point)
        {
            if (!Bucket_Dic.ContainsKey(point))
            {
                Bucket_Dic.Add(point, new Bucket());
            }
        }

        public static void Bucket_Check(List<Point> points)
        {
            for (int i = 0; i < points.Count; i++)
            {
                Bucket_Check(points[i]);
            }
        }

        public static void Sort_In_Bucket(World_Object world_Object)
        {
            Rectangle rectangle = world_Object.Get_Rectanglef().Get_Rectangle();

            //List<Point> points = Convert_World_To_Bucket_Point(Pointillize(rectangle, Bucket_Width, Bucket_Height));
            ////points = points.Distinct().toList();
            //Bucket_Check(points);
            //Add_To_Bucket(points, world_Object);

            Point point = Convert_World_To_Bucket_Point(rectangle.Location);
            Bucket_Check(point);
            Add_To_Bucket(point, world_Object);


            Point Top_Right = Convert_World_To_Bucket_Point(GetPoint(Corner.Top_Right, rectangle));
            Bucket_Check(Top_Right);
            Add_To_Bucket(Top_Right, world_Object);

            Point Bottom_Left = Convert_World_To_Bucket_Point(GetPoint(Corner.Bottom_Left, rectangle));
            Bucket_Check(Bottom_Left);
            Add_To_Bucket(Bottom_Left, world_Object);

            Point Bottom_Right = Convert_World_To_Bucket_Point(GetPoint(Corner.Bottom_Right, rectangle));
            Bucket_Check(Bottom_Right);
            Add_To_Bucket(Bottom_Right, world_Object);
        }

        public static void Add_To_Bucket(Point point, World_Object world_Object)
        {
            if (Bucket_Dic[point].Contains_Key(world_Object.id))
                return;

            Bucket_Dic[point].Add(world_Object);
        }

        public static void Remove_From_Bucket(Point point, World_Object world_Object)
        {
            if (!Bucket_Dic.ContainsKey(point))
                return;

            Bucket_Dic[point].Remove(world_Object);

            if (Bucket_Dic[point].Is_Empty())
                Bucket_Dic.Remove(point);
        }

        //public static void Clean_Bucket(Point point)
        //{
        //    foreach (KeyValuePair<Guid, World_Object> world_Object in Bucket_Dic[point].Get_Objects())
        //    {
        //        world_Object.Value.Get_Rectanglef();
        //        Rectangle rectangle = new Rectangle(point.X * Bucket_Width, point.Y * Bucket_Height, Bucket_Width, Bucket_Height);
        //        if (!rectangle.Intersects(world_Object.Value.Get_Rectanglef().Get_Rectangle())) // Is Rectanglef still a good idea?
        //        {
        //            Bucket_Dic[point].Remove(world_Object.Key);
        //        }
        //    }
        //}

        public static Point Convert_World_To_Bucket_Point(Point world_Coords)
        {//this function puts both -43,-60 in 0,0 as does it with 43,60 in 0,0
            if (world_Coords.X < 0)
                world_Coords.X -= 99;

            if (world_Coords.Y < 0)
                world_Coords.Y -= 99;

            return new Point(world_Coords.X / Bucket_Width, world_Coords.Y / Bucket_Height);
        }

        public static void Update_Bucket(World_Object world_Object, Vector2 Old_Pos, Vector2 New_Pos, int rectangle_Width, int rectangle_Height)
        {
            Point Old_Point = Old_Pos.ToPoint();
            Point New_Point = New_Pos.ToPoint();
            Update_Bucket(world_Object, Old_Point, New_Point); //Top left

            Point Old_Point_Top_Right = GetPoint(Corner.Top_Right, Old_Point, rectangle_Width, rectangle_Height);
            Point New_Point_Top_Right = GetPoint(Corner.Top_Right, New_Point, rectangle_Width, rectangle_Height);
            Update_Bucket(world_Object, Old_Point_Top_Right, New_Point_Top_Right); //Top right

            Point Old_Point_Bottom_Left = GetPoint(Corner.Bottom_Left, Old_Point, rectangle_Width, rectangle_Height);
            Point New_Point_Bottom_Left = GetPoint(Corner.Bottom_Left, New_Point, rectangle_Width, rectangle_Height);
            Update_Bucket(world_Object, Old_Point_Bottom_Left, New_Point_Bottom_Left); //Bottom left

            Point Old_Point_Bottom_Right = GetPoint(Corner.Bottom_Right, Old_Point, rectangle_Width, rectangle_Height);
            Point New_Point_Bottom_Right = GetPoint(Corner.Bottom_Right, New_Point, rectangle_Width, rectangle_Height);
            Update_Bucket(world_Object, Old_Point_Bottom_Right, New_Point_Bottom_Right); //Bottom left
        }

        public static void Update_Bucket(World_Object world_Object, Point Old_Pos, Point New_Pos)
        {
            Point Bucket_Old = Convert_World_To_Bucket_Point(Old_Pos);
            Point Bucket_New = Convert_World_To_Bucket_Point(New_Pos);

            if (Bucket_Old != Bucket_New)
            {
                Remove_From_Bucket(Bucket_Old, world_Object);
                Bucket_Check(Bucket_New);
                Add_To_Bucket(Bucket_New, world_Object);
            }
        }

        public static List<Bucket> Get_Visible_Bucket(Rectangle Culling_Rect)
        {
            Visible_buckets.Clear();
            Point top_left = Convert_World_To_Bucket_Point(Culling_Rect.Location);
            Point bottom_right = Convert_World_To_Bucket_Point(GetPoint(Corner.Bottom_Right, Culling_Rect));
            for (int x = top_left.X; x <= bottom_right.X; x++)
            {
                for (int y = top_left.Y; y <= bottom_right.Y; y++)
                {
                    Point point = new Point(x, y);
                    if (Bucket_Dic.ContainsKey(point))
                    {
                        Visible_buckets.Add(Bucket_Dic[point]);
                    }
                }
            }
            return Visible_buckets;
        }

        private static List<Bucket> Get_Bucket_In_Range(Point world_Coords, int Range)
        {
            if ( Range == 0 )
            {
                Point point = Convert_World_To_Bucket_Point(new Point(world_Coords.X, world_Coords.Y));
                if (Bucket_Dic.ContainsKey(point))
                    return new List<Bucket> { Bucket_Dic[point] };
            }

            Point Left = Convert_World_To_Bucket_Point(new Point(world_Coords.X - Range, world_Coords.Y));
            Point Right = Convert_World_To_Bucket_Point(new Point(world_Coords.X + Range, world_Coords.Y)); ;

            Point Top = Convert_World_To_Bucket_Point(new Point(world_Coords.X, world_Coords.Y - Range));
            Point Bottom = Convert_World_To_Bucket_Point(new Point(world_Coords.X, world_Coords.Y + Range));

            List<Bucket> buckets = new List<Bucket>();
            for (int X = Left.X; X <= Right.X; X++)
            {
                for (int Y = Top.Y; Y <= Bottom.Y; Y++)
                {
                    Point point = new Point(X, Y);
                    if (Bucket_Dic.ContainsKey(point))
                        buckets.Add(Bucket_Dic[point]);
                }
            }
            return buckets;
        }

        public static List<World_Object> Get_WorldObjects_To_Check(Point world_Coords, int Range)
        {
            List<Bucket> buckets = Get_Bucket_In_Range(world_Coords, Range);
            List<World_Object> world_Objects_In_Range = new List<World_Object>();
            for (int i = 0; i < buckets.Count; i++)
            {
                world_Objects_In_Range.AddRange(buckets[i].Get_Objects().Values);
            }
            return world_Objects_In_Range.Distinct().ToList();
        }
    }

    public class Bucket
    {//A bucket is a 100-X by 100-Y grid in world cordinates, for example all objects that are in the (0,0) bucket are in betweeen 0 and <100 X & Y
        private readonly Dictionary<uint, World_Object> world_Objects = new Dictionary<uint, World_Object>();

        public void Add(World_Object world_Object)
        {
            world_Objects.Add(world_Object.id, world_Object);
        }

        public void Remove(uint world_Object_Guid)
        {
            world_Objects.Remove(world_Object_Guid);
        }

        public void Remove(World_Object world_Object)
        {
            Remove(world_Object.id);
        }

        public Dictionary<uint, World_Object> Get_Objects()
        {
            return world_Objects;
        }

        public bool Contains_Key(uint id)
        {
            return world_Objects.ContainsKey(id);
        }

        public bool Is_Empty()
        {
            return world_Objects.Count == 0;
        }
    }
}
