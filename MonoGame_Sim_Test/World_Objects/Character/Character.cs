using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame_Sim_Test.Collision;
using MonoGame_Sim_Test.Shapes;
using System;
using System.Collections.Generic;
using static MonoGame_Sim_Test.World_Objects.Rectangler;

namespace MonoGame_Sim_Test
{
    public enum Direction
    {
        Down,
        Left,
        Right,
        Up
    }

    public class Character_Controller
    {
        //private readonly List<Character> characters = new List<Character>();
        private readonly Dictionary<uint, Character> characters = new Dictionary<uint, Character>();
        private Dictionary<string, Texture2D> dic_Tex_Character;
        private uint? MainCharacter = null;

        public void Init()
        {

        }

        public void Load(ContentManager Content)
        {
            dic_Tex_Character = Content_Loader.Load_Content<Texture2D>(Content, "Textures");
            List<string> List_sheet_Name = new List<string>() { "chara2", "chara3", "chara4", "chara5", "chara6", "chara7", "chara8" };
            Load_Default_Sheet(List_sheet_Name);
        }

        public void Load_Default_Sheet(List<string> List_sheet_Name)
        {
            for (int i = 0; i < List_sheet_Name.Count; i++)
            {
                Point size = new Point(26, 36);
                Point offset = new Point(0, 1);
                for (int j = 0; j < 8; j++)
                {
                    uint id = ID_Manger.Get_Next_ID();
                    Rectangle[] rectangles = Get_Rectangles(offset, size);
                    characters.Add(id, new Character(id, rectangles,
                        new Rectangle(new Point(offset.X + i * 312, offset.Y), new Point(rectangles[0].Width, rectangles[0].Height)),
                        dic_Tex_Character[List_sheet_Name[i]],
                        Vector2.One,
                        Color.White
                        ));

                    offset.X += 78;
                    offset.Y += (offset.X / 312) * 144;
                    offset.X %= 312;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, Rectangle Culling_rectangle)
        {
            foreach (Character character in characters.Values)
            {
                character.Draw(spriteBatch, Culling_rectangle);
            }
        }

        public void Update_Look_Direction_Main(Direction direction)
        {
            if (MainCharacter != null)
                Update_Look_Direction(MainCharacter.Value, direction);
        }

        public void Update_Look_Direction(uint id, Direction direction)
        {
            characters[id].Set_Look_Direction(direction);
        }

        public void Update_Look_Direction_Main(Vector2 Movement)
        {
            if (MainCharacter != null)
                Update_Look_Direction(MainCharacter.Value, Movement);
        }


        public void Update_Look_Direction(uint id, Vector2 Movement)
        {
            characters[id].Set_Look_Direction(Get_Movement_Directionn(Movement));
        }

        private Direction Get_Movement_Directionn(Vector2 vector2)
        {
            if (Math.Abs(vector2.X) >= Math.Abs(vector2.Y))
            { //horizontal
                if (vector2.X >= 0)
                    return Direction.Right;
                else
                    return Direction.Left;
            }
            else
            {//vertical
                if (vector2.Y >= 0)
                    return Direction.Down;
                else
                    return Direction.Up;
            }
        }

        public void Update_Transform_Main(Vector2 vector2)
        {
            if (MainCharacter != null)
                Update_Transform(MainCharacter.Value, vector2);
        }

        public void Update_Transform(uint id, Vector2 vector2)
        {
            characters[id].Transform(vector2);
        }

        public Vector2 Get_position_Main()
        {
            if (MainCharacter != null)
                return Get_position(MainCharacter.Value);
            else
                return Vector2.Zero;
        }

        public Dictionary<Corner, Point> Get_Buckets_Main()
        {
            Dictionary<Corner, Point> Bucket_Points = new Dictionary<Corner, Point>();
            if (MainCharacter != null)
            {
                Character Main = characters[MainCharacter.Value];
                Bucket_Points.Add(Corner.Top_Left, Bucket_Controller.Convert_World_To_Bucket_Point(
                    GetPoint(Corner.Top_Left, Main.Get_Rectangle())));
                Bucket_Points.Add(Corner.Top_Right, Bucket_Controller.Convert_World_To_Bucket_Point(
                    GetPoint(Corner.Top_Right, Main.Get_Rectangle())));
                Bucket_Points.Add(Corner.Bottom_Right, Bucket_Controller.Convert_World_To_Bucket_Point(
                    GetPoint(Corner.Bottom_Right, Main.Get_Rectangle())));
                Bucket_Points.Add(Corner.Bottom_Left, Bucket_Controller.Convert_World_To_Bucket_Point(
                    GetPoint(Corner.Bottom_Left, Main.Get_Rectangle())));
            }

            return Bucket_Points;
        }

        public Vector2 Get_position(uint id)
        {
            if (characters.ContainsKey(id))
            {
                return characters[id].Get_Position();
            }
            else
            {
                throw new InvalidOperationException("this should never be called");
            }
        }

        public void Change_Main(uint id)
        {
            if (characters.ContainsKey(id))
                MainCharacter = id;
        }

        public uint? Select_Character(List<World_Object> World_Objects, Point? point, double Max_Distance = double.MaxValue)
        {
            if (point == null)
                return null;

            double Closest = double.MaxValue;
            uint? Closest_character = null;
            foreach (World_Object world_object in World_Objects)
            {
                double Distance = Math.Sqrt(Math.Pow(world_object.Get_Rectanglef().Get_Rectangle().X - point.Value.X, 2) +
                            Math.Pow(world_object.Get_Rectanglef().Get_Rectangle().Y - point.Value.Y, 2));
                if (Distance < Closest && Distance <= Max_Distance)
                {
                    Closest = Distance;
                    Closest_character = world_object.id;
                }
            }
            return Closest_character;
        }

        public uint? Select_Character(Point? point, double Max_Distance = double.MaxValue)
        {
            if (point == null)
                return null;

            double Closest = double.MaxValue;
#nullable enable
            uint? Closest_character = null;
            foreach (KeyValuePair<uint, Character> Kvp in characters)
            {
                double Distance = Math.Sqrt(Math.Pow(Kvp.Value.Get_Position().X - point.Value.X, 2) +
                                            Math.Pow(Kvp.Value.Get_Position().Y - point.Value.Y, 2));
                if (Distance < Closest && Distance <= Max_Distance)
                {
                    Closest = Distance;
                    Closest_character = Kvp.Key;
                }
            }
            return Closest_character;
        }

        public Character? Get_Character(uint id)
        {
            if (characters.ContainsKey(id))
            {
                return characters[id];
            }
            return null;
        }
    }


    public class Character : World_Object
    {
        //private Texture2D tex_Sheet;
        private readonly Rectangle[] rects;

        //private Color color = Color.White;
        //private Vector2 origin = Vector2.Zero;
        //private float scale = 1f;
        //private float rotation = 0f;
        private int rect_Index = 0;
        //private Vector2 position = Vector2.Zero;

        public Character()
        {
            rects = new Rectangle[0];
        }

        public Character(uint guid, Rectangle[] rects, Rectangle rectangle, Texture2D texture, Vector2 origin, Color color, float rotation = 0f)
        {
            this.texture = texture;
            this.rectangle = new Rect(rectangle, new Vector2(rectangle.X, rectangle.Y));
            this.origin = origin;
            this.color = color;
            this.rotation = rotation;
            this.rects = rects;
            DrawRectangle = rects[0];

            Selectable = true;
            Collision_Properties collision = new Collision_Properties(this.rectangle, 0.4f, 0.3f);
            Init(guid, collision);
        }

        public void Set_Look_Direction(Direction direction)
        {
            switch (direction)
            {
                case Direction.Down:
                    rect_Index = 0;
                    break;
                case Direction.Left:
                    rect_Index = 1;
                    break;
                case Direction.Right:
                    rect_Index = 2;
                    break;
                case Direction.Up:
                    rect_Index = 3;
                    break;
                default:
                    break;
            }
            this.DrawRectangle = rects[rect_Index];
        }

        public Vector2 Get_Position()
        {
            return rectangle.position;
        }

        public Rectangle Get_Rectangle(bool origin_adjusted = true)
        {
            if (!origin_adjusted)
                return rectangle.Get_Rectangle();

            Rectangle temp_rectangle = rectangle.Get_Rectangle();
            temp_rectangle.Offset(-origin);

            return temp_rectangle;
        }
    }
}
