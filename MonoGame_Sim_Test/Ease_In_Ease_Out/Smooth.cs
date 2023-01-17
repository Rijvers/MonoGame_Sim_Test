using Microsoft.Xna.Framework;

namespace MonoGame_Sim_Test
{
    public static class Smooth
    {
        public static float Ease_in(float value, float Goal, float Strenght = 0.1f)
        {
            return value + ((Goal - value) * Strenght);
        }

        public static Vector2 Ease_in(Vector2 value, Vector2 Goal, float Strenght = 0.025f)
        {
            return value + ((Goal - value) * Strenght);
        }
    }
}
