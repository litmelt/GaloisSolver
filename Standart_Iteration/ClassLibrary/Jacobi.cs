using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
    public class Jacobi:Iteration
    {


       #region Итерация
       public static void Iteration(int[] massx)
       {
           massx.CopyTo(MassX, 0);
           MassX = StepX();
 
      }

        #region  Шаг итерации
         static int[] StepX()
        {
            int[] temp = new int[Count_x];
            for (int i=0; i< Count_x;i++)
            {
                temp[i] = X(i);
            }
            return temp ;
        }


        #endregion
       #endregion
    }
}
