using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame_Sim_Test.World_Objects;
using System;
using System.Collections.Generic;

namespace MonoGame_Sim_Test
{
    public enum PreCalculated_Squares
    {
        Camera_Border,
        Mouse_Click_Screen,
        Mouse_Click_World,
        Mouse_Location,
        Bucket_Border
    }

    public class Debug
    {
        private readonly Debug_PreCalculate pre_Calculations;

        private bool Enabled = false;
        private SpriteFont font;
        private readonly List<string> Debug_Screen_Texts = new List<string>();
        private readonly List<string> Debug_World_Texts = new List<string>();

        public Debug(SpriteFont font, GraphicsDevice GraphicsDevice, Camera camera, bool Enabled = false)
        {
            ChangeFont(font);
            Show_Debug(Enabled);

            pre_Calculations = new Debug_PreCalculate(GraphicsDevice, camera.Get_Culling_Rectangle());
        }

        public void Show_Debug(bool Enabled = true)
        {
            this.Enabled = Enabled;
        }

        public bool Is_Debugging()
        {
            return Enabled;
        }

        public void ChangeFont(SpriteFont font)
        {
            this.font = font;
        }

        public void Draw_Screen(SpriteBatch spriteBatch, Character_Controller character_Controller, Camera camera, Point? Mouse_Click, Point? Mouse_Screen, Point? Mouse_World)
        {
            if (!Enabled)
                return;

            Debug_Screen_Texts.Clear();
            Debug_Screen_Texts.AddRange(Get_individual_Lines(character_Controller.Get_position_Main().ToString()));
            string Main_Buckets = string.Empty;
            foreach (KeyValuePair<Rectangler.Corner, Point> Bucket_Point in character_Controller.Get_Buckets_Main())
            {
                Main_Buckets += Bucket_Point.Key.ToString() + " " + Bucket_Point.Value.ToString() + "\r\n";
            }
            Debug_Screen_Texts.AddRange(Get_individual_Lines(Main_Buckets));
            Debug_Screen_Texts.AddRange(Get_individual_Lines(camera.ToString()));
            if (Mouse_Screen != null)
                Debug_Screen_Texts.Add("Mouse Screen X: " + Mouse_Screen.Value.X + " Y: " + Mouse_Screen.Value.Y);
            if (Mouse_World != null)
                Debug_Screen_Texts.Add("Mouse World X: " + Mouse_World.Value.X + " Y: " + Mouse_World.Value.Y);

            if (Mouse_Click != null)
                Draw_Rectangle(spriteBatch, pre_Calculations.Textures[PreCalculated_Squares.Mouse_Click_Screen], new Rectangle(new Point(Mouse_Click.Value.X - 10, Mouse_Click.Value.Y - 10), new Point(20, 20)));

            Draw_Mouse_Screen_Location(spriteBatch, Mouse_Screen);

            int Debug_Text_Y = 5;
            foreach (string Debug_Screen_Text in Debug_Screen_Texts)
            {
                spriteBatch.DrawString(font, Debug_Screen_Text, new Vector2(0, Debug_Text_Y), Color.Black);
                Debug_Text_Y += 15;
            }
        }

        public void Draw_World(SpriteBatch spriteBatch, Camera camera, Point? Click, Point? Mouse, Character? character)
        {
            if (!Enabled)
                return;

            Debug_World_Texts.Clear();

            Draw_Rectangle_Outline(spriteBatch, pre_Calculations.Textures[PreCalculated_Squares.Camera_Border], camera.Get_Culling_Rectangle());
            Draw_World_Click(spriteBatch, Click);
            Draw_Mouse_World_Location(spriteBatch, Mouse);
            Draw_Buckets(spriteBatch, camera.Get_Culling_Rectangle());

            if (character != null)
                Draw_Character_Moused(spriteBatch, character); //move this to HUD



            //spriteBatch.DrawString(font, "test", Vector2.Zero, Color.Black, 0, Vector2.Zero, 1, SpriteEffects.None, 0.1f); //remember to give world tekst a propper Z value

            foreach (string Debug_World_Text in Debug_World_Texts)
            {
                spriteBatch.DrawString(font, Debug_World_Text, new Vector2(0, 0), Color.Black);
            }
        }

        private List<string> Get_individual_Lines(string Input)
        {
            return new List<string>(Input.Split(Environment.NewLine));
        }

        private void Draw_Character_Moused(SpriteBatch spriteBatch, Character? character)
        {
            Draw_Rectangle_Outline(spriteBatch, pre_Calculations.Textures[PreCalculated_Squares.Camera_Border], character.Get_Rectangle()); //give it own textures

            Draw_Rectangle(spriteBatch, pre_Calculations.Textures[PreCalculated_Squares.Mouse_Click_World], new Rectangle((int)character.Get_Position().X - 4, (int)character.Get_Position().Y - 4, 8, 8), false);

            spriteBatch.DrawString(font, "Name", character.Get_Position(), Color.Black, 0, Vector2.Zero, 1, SpriteEffects.None, character.Get_Position().Y + 0.1f);
        }

        private void Draw_World_Click(SpriteBatch spriteBatch, Point? Click)
        {
            if (Click == null)
                return;

            Draw_Rectangle(spriteBatch, pre_Calculations.Textures[PreCalculated_Squares.Mouse_Click_World], new Rectangle(Click.Value.X - 15, Click.Value.Y - 15, 30, 30), true);
        }

        private void Draw_Mouse_World_Location(SpriteBatch spriteBatch, Point? Mouse)
        {
            if (Mouse == null)
                return;

            Draw_Rectangle(spriteBatch, pre_Calculations.Textures[PreCalculated_Squares.Mouse_Location], new Rectangle(Mouse.Value.X - 2, Mouse.Value.Y - 2, 4, 4), true);
        }

        private void Draw_Mouse_Screen_Location(SpriteBatch spriteBatch, Point? Mouse)
        {
            if (Mouse == null)
                return;

            Draw_Rectangle(spriteBatch, pre_Calculations.Textures[PreCalculated_Squares.Mouse_Location], new Rectangle(Mouse.Value.X - 2, Mouse.Value.Y - 2, 4, 4), false);
        }

        private void Draw_Buckets(SpriteBatch spriteBatch, Rectangle rectangle)
        {
            Point top_left = Bucket_Controller.Convert_World_To_Bucket_Point(rectangle.Location);
            Point bottom_right = Bucket_Controller.Convert_World_To_Bucket_Point(Rectangler.GetPoint(Rectangler.Corner.Bottom_Right, rectangle));

            for (int x = top_left.X; x <= bottom_right.X; x++)
            {
                for (int y = top_left.Y; y <= bottom_right.Y; y++)
                {
                    Draw_Rectangle_Outline(spriteBatch, pre_Calculations.Textures[PreCalculated_Squares.Bucket_Border], new Rectangle(x * Bucket_Controller.Bucket_Width, y * Bucket_Controller.Bucket_Height, Bucket_Controller.Bucket_Width, Bucket_Controller.Bucket_Height), true);
                    spriteBatch.DrawString(font, x + "," + y, new Vector2(x * 100 + 2, y * 100 + 2), Color.Black, 0, Vector2.Zero, 0.5f, SpriteEffects.None, y * 100 + 0.1f);
                }
            }
        }

        private void Draw_Rectangle_Outline(SpriteBatch spriteBatch, Texture2D Txt_Culling_Border, Rectangle rectangle, bool Zindex = false)
        {
            if (Zindex)
                spriteBatch.Draw(Txt_Culling_Border, rectangle, null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, rectangle.Y + 0.01f);
            else
                spriteBatch.Draw(Txt_Culling_Border, rectangle, null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, int.MaxValue);
        }

        private void Draw_Rectangle(SpriteBatch spriteBatch, Texture2D Txt_rectangle, Rectangle rectangle, bool Zindex = false)
        {
            if (Zindex)
            {
                spriteBatch.Draw(Txt_rectangle, rectangle, null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, int.MaxValue);
            }
            else
            {
                spriteBatch.Draw(Txt_rectangle, rectangle, Color.White);
            }
        }
    }

    internal class Debug_PreCalculate
    {
        internal Dictionary<PreCalculated_Squares, Texture2D> Textures = new Dictionary<PreCalculated_Squares, Texture2D>();
        private Color DebugClr_Red = new Color(250, 0, 0, 0);
        private Color DebugClr_Red_Dark = new Color(150, 50, 50, 150);
        private Color DebugClr_Green = new Color(0, 250, 0, 0);
        private Color DebugClr_Black = new Color(0, 0, 0, 255);
        private Color DebugClr_HighLight = new Color(25, 25, 25, 0);
        private Color DebugClr_None = new Color(0, 0, 0, 0);


        public Debug_PreCalculate(GraphicsDevice GraphicsDevice, Rectangle rectangle)
        {
            Textures.Add(PreCalculated_Squares.Camera_Border, Calculate_Border_Square_Texture(GraphicsDevice, rectangle, 5, DebugClr_Red_Dark, DebugClr_HighLight));
            Textures.Add(PreCalculated_Squares.Mouse_Click_Screen, Calculate_Square_Texture(GraphicsDevice, DebugClr_Red));
            Textures.Add(PreCalculated_Squares.Mouse_Click_World, Calculate_Square_Texture(GraphicsDevice, DebugClr_Green));
            Textures.Add(PreCalculated_Squares.Mouse_Location, Calculate_Square_Texture(GraphicsDevice, DebugClr_Black));
            Textures.Add(PreCalculated_Squares.Bucket_Border, Calculate_Border_Square_Texture(GraphicsDevice, new Rectangle(0, 0, Bucket_Controller.Bucket_Width, Bucket_Controller.Bucket_Height), 2, DebugClr_Red, DebugClr_None));
        }

        private Texture2D Calculate_Border_Square_Texture(GraphicsDevice GraphicsDevice, Rectangle rectangle, int line_Thickness, Color color_Border, Color Color_Inside)
        {
            line_Thickness--;
            List<Color> Colors = new List<Color>();

            for (int y = 0; y < rectangle.Height; y++)
            {
                for (int x = 0; x < rectangle.Width; x++)
                {
                    if (x <= line_Thickness ||
                        y <= line_Thickness ||
                        x >= rectangle.Width - line_Thickness ||
                        y >= rectangle.Height - line_Thickness)
                    {
                        Colors.Add(color_Border);
                    }
                    else
                    {
                        Colors.Add(Color_Inside);
                    }
                }
            }

            Texture2D texture2D = new Texture2D(GraphicsDevice, rectangle.Width, rectangle.Height);
            texture2D.SetData(Colors.ToArray());

            return texture2D;
        }

        private Texture2D Calculate_Square_Texture(GraphicsDevice GraphicsDevice, Color color)
        {
            Texture2D texture = new Texture2D(GraphicsDevice, 1, 1);
            texture.SetData(new Color[] { color });
            return texture;
        }
    }


}
