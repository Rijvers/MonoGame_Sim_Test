using System;
using System.Collections.Generic;
using System.Text;

namespace MonoGame_Sim_Test
{
    public static class Object_Manger
    {

    }

    public static class ID_Manger
    {
        private static uint Next_Free_ID = 0;

        public static uint Get_Next_ID()
        {
            Next_Free_ID++;
            return Next_Free_ID;
        }
    }
}
