using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonoGame_Sim_Test.ECS
{
    internal class Render_Container : BaseSystem<Render_Comp> { } //Holds ALL RENDER COMPONENTS

    static class Render_System
    {
        public static void UpdateRender(SpriteBatch spriteBatch)
        {
            foreach (Render_Comp render in Render_Container.GetComponents())
            {
                if ( render.Render )
                {

                }
            }
        }  
    }
}
