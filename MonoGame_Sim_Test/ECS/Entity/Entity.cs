using System.Collections.Generic;

namespace MonoGame_Sim_Test.ECS
{
    public class Entity
    {//holds ONLY ID and COMPONENTS (max 1 of each)
        public uint Id { get; set; }

        public readonly HashSet<Component> components = new HashSet<Component>();
        
        //public Entity()
        //{
        //    Id = ID_Manger.Get_Next_ID();
        //}

        //public void AddComponent(Component component)
        //{
        //    components.Add(component);
        //    component.entity = this;
        //}

        ////public T GetComponent<T>() where T : Component
        ////{
        ////    foreach (Component component in components)
        ////    {
        ////        if (component.GetType().Equals(typeof(T)))
        ////        {
        ////            return (T)component;
        ////        }
        ////    }
        ////    return null;
        ////}

        //public void RemoveComponent(Component component)
        //{
        //    components.Remove(component);
        //}

        //public void RemoveAllComponents()
        //{
        //    components.Clear();
        //}

        //public HashSet<Component> GetComponents()
        //{
        //    return components;
        //}
    }
}
