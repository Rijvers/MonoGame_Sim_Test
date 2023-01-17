
namespace MonoGame_Sim_Test.ECS
{//ONLY contains DATA, NO logic
    class Render_Comp : Component
    {
        public Render_Comp()
        {
            //type = Component_Type.Render;
            Render_Container.Register(this);
        }

        public bool Render = true;
    }
}
