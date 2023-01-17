using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace MonoGame_Sim_Test
{
    public class Game1 : Game
    {
        private readonly Game_Graphics Game_Graphics;
        private readonly Game_IO Game_IO;

        private Debug debug;
        private readonly Character_Controller character_Controller = new Character_Controller();

        private SpriteFont font;
        private Point? Screen_Mouse_Release, Screen_Mouse_Down, World_Mouse_Click, World_Mouse_Down;
        private Point? Screen_Mouse, World_Mouse;
        private Character? Moused_Character; //we don't want to store it like this 

        public Game1()
        {
            Game_Graphics = new Game_Graphics(this);
            Game_IO = new Game_IO(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        public void Quit()
        {
            Exit();
        }

        protected override void Initialize()
        {
            Game_Graphics.Init_FPS_Settings(this);
            Game_Graphics.Init_RenderTarget(GraphicsDevice);

            base.Initialize();

            Game_Graphics.Set_PreferredBackBuffer();
            Game_Graphics.Init_Camera(GraphicsDevice);

            debug = new Debug(font, GraphicsDevice, Game_Graphics.camera);
        }

        protected override void LoadContent()
        {
            Game_Graphics.Init_SpriteBarch(GraphicsDevice);

            character_Controller.Init();
            character_Controller.Load(Content);

            Tile_Controller.Init(1000, 1000);
            Tile_Controller.Load(Content, 1024);//128 //256; //512; //1024;

            font = Content.Load<SpriteFont>("Fonts/Gold Box 8x8 Monospaced");
        }

        protected override void Update(GameTime gameTime)
        {
            //gameTime.ElapsedGameTime.TotalMilliseconds;

            Game_IO.Keyboard_Mouse.Update(gameTime);

            Vector2 MovementUpdate = Game_IO.Keyboard_Mouse.Get_Main_Movement();
            character_Controller.Update_Transform_Main(MovementUpdate);

            if (MovementUpdate != Vector2.Zero)
                character_Controller.Update_Look_Direction_Main(MovementUpdate);

            Game_Graphics.camera.Move(MovementUpdate);
            Game_Graphics.camera.Teleport(Smooth.Ease_in(Game_Graphics.camera.Get_Center(), character_Controller.Get_position_Main()));

            Game_Graphics.camera.Zoom = Game_IO.Keyboard_Mouse.Get_Camera_Zoom(Game_Graphics.camera.Zoom);
            Game_Graphics.camera.Rotation = Game_IO.Keyboard_Mouse.Get_Camera_Rotation(Game_Graphics.camera.Rotation);

            Game_Graphics.camera.Zoom += Game_IO.Keyboard_Mouse.Get_ScrollWheel();

            Screen_Mouse_Down = Game_IO.Keyboard_Mouse.Get_Mouse_LeftButton_Down();
            Screen_Mouse_Release = Game_IO.Keyboard_Mouse.Get_Mouse_LeftButton_Release();
            World_Mouse_Click = Game_Graphics.camera.Get_World_Coords(Screen_Mouse_Release);
            World_Mouse_Down = Game_Graphics.camera.Get_World_Coords(Screen_Mouse_Down);

            int Click_pixel_range = 30;
            if (World_Mouse_Click != null)
            {
                uint? character_New_Main = character_Controller.Select_Character(
                    Bucket_Controller.Get_WorldObjects_To_Check(World_Mouse_Click.Value, Click_pixel_range), World_Mouse_Click, Click_pixel_range);
                if (character_New_Main != null)
                    character_Controller.Change_Main(character_New_Main.Value);
            }

            Screen_Mouse = Game_IO.Keyboard_Mouse.Get_Mouse_Position();
            World_Mouse = Game_Graphics.camera.Get_World_Coords(Screen_Mouse);

            Moused_Character = null;
            if (World_Mouse != null)
            {
                uint? character_Index_New_Moused = character_Controller.Select_Character(
                    Bucket_Controller.Get_WorldObjects_To_Check(World_Mouse.Value, Click_pixel_range), World_Mouse, Click_pixel_range);
                if (character_Index_New_Moused != null)
                {
                    Moused_Character = character_Controller.Get_Character(character_Index_New_Moused.Value);
                }
            }

            debug.Show_Debug(Game_IO.Keyboard_Mouse.Get_Debug_State(debug.Is_Debugging()));

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            Game_Graphics.renderTarget2d.Set_RenderTarget(GraphicsDevice);
            GraphicsDevice.Clear(Color.CornflowerBlue);

            Game_Graphics.spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend,
                null, null, null, null,
                Game_Graphics.camera.Transform);

            Rectangle Culling_Rect = Game_Graphics.camera.Get_Culling_Rectangle();
            List<Bucket> Visible_Buckets = Bucket_Controller.Get_Visible_Bucket(Culling_Rect);
            foreach (Bucket bucket in Visible_Buckets)
            {
                foreach (KeyValuePair<uint, World_Object> KVP in bucket.Get_Objects())
                {
                    KVP.Value.Draw(Game_Graphics.spriteBatch, Culling_Rect);
                }
            }

            debug.Draw_World(Game_Graphics.spriteBatch, Game_Graphics.camera, World_Mouse_Down, World_Mouse, Moused_Character);

            Game_Graphics.spriteBatch.End();

            GraphicsDevice.SetRenderTarget(null);
            GraphicsDevice.Clear(Color.CornflowerBlue);

            Game_Graphics.spriteBatch.Begin();
            Game_Graphics.spriteBatch.Draw(Game_Graphics.renderTarget2d.Get_renderTarget(), Vector2.Zero, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

            Draw_HUD();
            debug.Draw_Screen(Game_Graphics.spriteBatch, character_Controller, Game_Graphics.camera, Screen_Mouse_Down, Screen_Mouse, World_Mouse);

            Game_Graphics.spriteBatch.End();


            base.Draw(gameTime);
        }

        private void Draw_HUD()
        {
            //TODO
        }
    }
}
