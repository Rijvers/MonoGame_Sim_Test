using System.Collections.Generic;

namespace MonoGame_Sim_Test.ECS
{
    class BaseSystem<T> where T : Component
    {//holds LOGIC of COMPONENTS
        protected static List<T> components = new List<T>();

        public static void Register(T component)
        {
            components.Add(component);
        }

        public static List<T> GetComponents()
        {
            return components;
        }

        public static void Remove(T component)
        {
            components.Remove(component);
        }
    }
}
