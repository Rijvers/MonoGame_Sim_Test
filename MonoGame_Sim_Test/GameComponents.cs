using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MonoGame_Sim_Test
{
    internal class Game_Graphics
    {
        private readonly GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch;
        public readonly RenderTarget_Controller renderTarget2d = new RenderTarget_Controller();
        public Camera camera;

        internal Game_Graphics(Game1 game)
        {
            graphics = new GraphicsDeviceManager(game);
        }

        internal void Init_FPS_Settings(Game1 game)
        {
            game.TargetElapsedTime = TimeSpan.FromSeconds(1.0 / 144.0f);
            game.IsFixedTimeStep = true;
        }

        internal void Init_RenderTarget(GraphicsDevice graphicsDevice)
        {
            renderTarget2d.Init(graphicsDevice);
        }

        internal void Set_PreferredBackBuffer()
        {
            graphics.PreferredBackBufferWidth = 1920;
            graphics.PreferredBackBufferHeight = 1080;
            graphics.ApplyChanges();
        }

        internal void Init_Camera(GraphicsDevice graphicsDevice)
        {
            camera = new Camera(graphicsDevice.Viewport);
        }

        internal void Init_SpriteBarch(GraphicsDevice graphicsDevice)
        {
            spriteBatch = new SpriteBatch(graphicsDevice);
        }
    }

    internal class Game_IO
    {
        public Keyboard_Mouse Keyboard_Mouse;
        internal Game_IO(Game1 game)
        {
            Keyboard_Mouse = new Keyboard_Mouse(game);
        }
    }
}
