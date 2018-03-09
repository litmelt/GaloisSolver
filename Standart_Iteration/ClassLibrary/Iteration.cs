using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
    public class Iteration
    {
        public static int Galua { get; set; }           // модуль, по которому выполняется решение
        public static int Count_x { get; set; }        // количество неизвесных
        public static int[,] coefficients;     //неизвесные (предположительные  для итераций)
        public static int[] MassX;
        static Iteration()
        {
            Galua = 1;
            Count_x = 2;      
        }

        #region Шаг итерации для одного Х
        protected static int X(int num_x)  // шаг итерации для одного из х
        {
            int temp = coefficients[num_x, Count_x];
            int mass;
            for (int i = 0; i < Count_x; i++)
            {
                mass = coefficients[num_x, i] * MassX[i];
                if (i != num_x) temp = temp - mass;
            }
            temp = GaluaDiv(temp, coefficients[num_x, num_x]);
            return temp;
        }
        #endregion

        #region Деление в полях Галуа
        public static int GaluaDiv(int ch, int Zn)
        {
            int D;
            int Ch = ch;
            while (Ch < 0) Ch += Galua;
            while (Ch % Zn != 0) Ch += Galua;
            D = (int)(Ch / Zn);
            D %= (int)Galua;
            return D;
        }
        #endregion
    }
}
