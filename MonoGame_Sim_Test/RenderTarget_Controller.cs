using Microsoft.Xna.Framework.Graphics;

namespace MonoGame_Sim_Test
{
    public class RenderTarget_Controller
    {
        private static RenderTarget2D renderTarget;
        private static float renderTarget_X, renderTarget_Y;

        private const float Render_Resolution_Width = 1920; //1280 //1920 //2560 //3840
        private const float Rneder_Resolution_Height = 1080; //720  //1080 //1440 //2160

        public void Init(GraphicsDevice GraphicsDevice)
        {
            renderTarget_X = Render_Resolution_Width; renderTarget_Y = Rneder_Resolution_Height;
            renderTarget = new RenderTarget2D(GraphicsDevice, (int)renderTarget_X, (int)renderTarget_Y);
        }

        public RenderTarget2D Get_renderTarget()
        {
            return renderTarget;
        }

        public void Set_RenderTarget(GraphicsDevice GraphicsDevice)
        {
            GraphicsDevice.SetRenderTarget(renderTarget);
        }

        public override string ToString()
        {
            return "Width: " + renderTarget.Width.ToString() + " Height: " + renderTarget.Height.ToString();
        }
    }
}
