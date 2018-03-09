using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassLibrary1
{
    public class Test_matrix
    {
        #region Обработка матрицы (детерминант)
        // считаем детерминант
        public static int CountSolution(int count, int[,] matr)
        {
            int det = 0;
            if (count == 1) return matr[0, 0];
            if (count == 2)
            {
                det = matr[0, 0] * matr[1, 1] - matr[1, 0] * matr[0, 1];
                return det;
            }
            int s;
            for (int i=0 ; i<count; i++)
            {
                int[,] matrz = new int[count  - 1,count - 1];
                s = 0;
                if (matr[count - 1, i] != 0)
                {
                    for (int j = 0; j < count; j++)
                    {
                        if (j == i) s = 1;
                        for (int t = 0; t < count - 1; t++)
                        {
                            if (j != i) matrz[t, j - s] = matr[t, j];
                        }
                    }
                    if (matr[count - 1, i] != 0) det += matr[count - 1, i] * CountSolution(count - 1, matrz);
                }
            }

            return det;
        }
        public static bool All(int[,] matr)
        {
            for (int i = 0; i < matr.GetLength(0); i++)
            {
                if (!All_x(matr, i)) return false;
            }
            return true;
        }
        public static bool All_x(int[,] matr, int x)
        {
            for (int i = 0; i < matr.GetLength(0); i++)
            {
                if (matr[x, i] != 0) return true;
            }
            return false;
        }

        #endregion

        static bool simple(int n)
        {
            for (int i = 2; i <= n / 2; i++) if ((n % i) == 0) return false;
            return true;
        }

        public static int NextSimp(int a)
        {
            int i = a + 1;
            while (true)
            {
                if (simple(i)) return i;
                i += 1;
            }
        }
    }
}
