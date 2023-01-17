using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame_Sim_Test.Collision;
using MonoGame_Sim_Test.Shapes;
using System;
using System.Collections.Generic;

namespace MonoGame_Sim_Test
{
    public class World_Object
    {
        public uint id { get; protected set; } //Unique ID

        protected Texture2D texture; //Can be a single image or a collection and taking part from with "DrawRectangle"
        protected Rectangle? DrawRectangle = null; //if not null takes a sub-section of "texture" instead of drawing the whole texture
        protected Vector2 origin = Vector2.Zero; //Not used //The origin where rotation, scaling & placement happens from [NOT FULLY SUPPORTED TO CHANGE FROM "Vector2.Zero"]
        protected Color color = Color.White; //Color tone adjusted to the whole texture, "Color.White" is no adjustment
        protected float rotation = 0f;
        protected float scale = 1f; //Not used 
        protected int Z_Depth = 0; //lowest numer is most on top, Characters are 0, Tiles are ~10000

        protected Collision_Properties collision; //is this the correct location?
        protected Rect rectangle; //do we want to default to a rectangle?

        protected bool Selectable = false; //Currently not used 

        public World_Object()
        { }

        public void Init(uint id, Collision_Properties collision = null)
        {
            this.id = id;
            Bucket_Controller.Sort_In_Bucket(this);
            this.collision = collision;
        }

        public void Draw(SpriteBatch spriteBatch, Rectangle Culling_rectangle)
        {
            if (IsEmptyRectangel(Culling_rectangle) || Passes_Culling(Culling_rectangle, rectangle))
            {
                spriteBatch.Draw(texture, rectangle.position, DrawRectangle, color, 
                    rotation, origin, scale, SpriteEffects.None, rectangle.Y + rectangle.Height - Z_Depth);
                //System.Diagnostics.Debug.WriteLine("Depth " + (rectangle.Y + rectangle.Height - Z_Depth));
            }
        }

        public Rect Get_Rectanglef()
        {
            return rectangle;
        }

        private bool Passes_Culling(Rectangle Culling_rectangle, Rect Object_rectangle)
        {
            //with origin
            return (Object_rectangle.X + Object_rectangle.Width - origin.X >= Culling_rectangle.X
                && Object_rectangle.X - origin.X <= Culling_rectangle.X + Culling_rectangle.Width
                && Object_rectangle.Y + Object_rectangle.Height - origin.Y >= Culling_rectangle.Y
                && Object_rectangle.Y - origin.Y <= Culling_rectangle.Y + Culling_rectangle.Height);
        }

        private bool IsEmptyRectangel(Rectangle rectangle)
        {
            if (rectangle.X == 0 && rectangle.Y == 0 && rectangle.Width == 0 && rectangle.Height == 0)
            {
                return true;
            }
            return false;
        }

        public void Transform(Vector2 vector2, bool Check_Collision = true)
        {
            //Check_Collision = false; //testing
            if (vector2 == Vector2.Zero)
                return;

            Vector2 Old_Pos = rectangle.position;
            Vector2 Requested_Pos = rectangle.position + vector2;

            if (collision != null && Check_Collision)
            {
                float Collision_Factor = Collision_Check(Requested_Pos, collision, vector2);
                rectangle.position += (vector2 * Collision_Factor);
            }
            else
            {
                rectangle.position = Requested_Pos;
            }

            Bucket_Controller.Update_Bucket(this, Old_Pos, rectangle.position, rectangle.Width, rectangle.Height);
        }

        public void Set_Position(Vector2 vector2)
        {
            Bucket_Controller.Update_Bucket(this, rectangle.position, vector2, rectangle.Width, rectangle.Height);

            rectangle.position = vector2;
        }

        public float Collision_Check(Vector2 Unchecked_New_Pos, Collision_Properties Main_Object_collision, Vector2 Unchecked_Movement)
        { //this fuction should not be in World_Object
            if (collision == null)
                return 1f;

            Point collision_Center = new Point((int)Unchecked_New_Pos.X + Main_Object_collision.Get_Rectangle().Width / 2,
                                    (int)Unchecked_New_Pos.Y + Main_Object_collision.Get_Rectangle().Height / 2);
            int Range = (int)Math.Ceiling(Math.Sqrt((Main_Object_collision.Get_Rectangle().Width / 2d) * (Main_Object_collision.Get_Rectangle().Width / 2d) +
                                    (Main_Object_collision.Get_Rectangle().Height / 2d) * (Main_Object_collision.Get_Rectangle().Height / 2d)));

            List<World_Object> world_objects = Bucket_Controller.Get_WorldObjects_To_Check(
                                    collision_Center, Range);

            float Smallest_Factor = 1;
            foreach (World_Object world_object in world_objects)
            {
                if (world_object.collision == null || world_object.id == id)
                    continue;

                if (!new Rectangle((int)Unchecked_New_Pos.X, (int)Unchecked_New_Pos.Y, collision.Get_Rectangle().Width, collision.Get_Rectangle().Height).Intersects(world_object.collision.Get_Rectangle().Get_Rectangle()))
                    continue;

                if (collision.PushPower > world_object.collision.StayPower)
                {


                    //meaby calcualte points relavtive to each other?
                    float Factor = collision.PushPower - world_object.collision.StayPower;
                    Vector2 vector = -(rectangle.Get_Center() - world_object.Get_Rectanglef().Get_Center()) * Factor;
                    //Vector2 vector = vector2 * Factor;
                    world_object.Transform(vector, true);
                    if (Factor < Smallest_Factor)
                        Smallest_Factor = Factor;
                }
                else
                {
                    return 0;
                }
            }
            return Smallest_Factor;
        }
    }
}
