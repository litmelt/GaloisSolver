using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
    public class Seidel : Iteration
    {
        #region Итерация
        public static void Iteration(int[] massx)
        {
            MassX = new int[Count_x];
            massx.CopyTo(MassX, 0);
               StepX();
          
        }

        #region  Шаг итерации
        static void StepX()
        {
            int[] temp = new int[Count_x];
            for (int i = 0; i < Count_x; i++)
            {
                MassX[i] = X(i);
            }

        }


        #endregion
        #endregion
        
    }
}
