
namespace MonoGame_Sim_Test.ECS
{
    class Test_Object : Entity
    {//holds ONLY ID and COMPONENTS
        public Render_Comp render;

        public Test_Object()
        {
            render = new Render_Comp();
            components.Add(render);
        }
    }
}
