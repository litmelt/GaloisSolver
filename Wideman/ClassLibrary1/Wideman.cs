using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassLibrary1
{
    public class Iteration  // Класс, отвечающий за метод пошаговых итераций
    {
        public static int Galua { get; set; }           // модуль, по которому выполняется решение
        public static int Count_x { get; set; }        // количество неизвесных
        public static int[,] coefficients;     //неизвесные (предположительные  для итераций)
        public static int[] MassX;
        static int[] b0;
        static int[] b; // Предположение для b
        static int k; // Шаг прохода
        static int[] y; // Временная переменная для X
        static int d; // степень многочлена
        static int[] minf;

       public static  void Massx()
        {
            MassX = Solution();
        }

        static int[] Solution()
        {
            b = new int[Count_x];
            b0 = new int[Count_x];
            y = new int[Count_x];
            B();
            int[] u = new int[2 * (Count_x - d)];
            int[] x = new int[Count_x];
            d = 0;
            while (Check_b(b))
            {
                u = Sequence_u(d, b);
                minf = Berlekamp(u);
                y = Found_yk1(y,minf,b);
                b = Found_bk1(y);
                d = Found_d(minf, d);
                k += 1;
            }
            for (int i = 0; i < Count_x  ; i++)
            {
                x[i] = - y[i];
                while (x[i] < 0) x[i] += Galua;
                x[i] %= Galua;
            }
            return x;
        }
        static void B()
        {
            for (int i = 0; i < Count_x; i++)
            {
                b[i] = coefficients[i, Count_x];
                b0[i] = coefficients[i, Count_x];
            }
        }

        #region Статический конструктор. Просто статический конструктор.
        static Iteration()
        {
            Count_x = 1;

            Galua = 1;
        }
        #endregion

        #region Второй пункт алгоритма - проверяем закончить ли алгоритм
        static bool Check_b(int[] ch_b)
        {
            for (int i = 0; i < Count_x; i++) if (ch_b[i] != 0) return true; //Незакончен
            return false;
        }
        #endregion

        #region Третий пункт - выбор случайного u
        static int[] U()
        {
            int[] new_u = new int[Count_x];
            Random u_rand = new Random();
            for (int i = 0; i < Count_x; i++) new_u[i] = (u_rand.Next(1, Galua*1000)) % Galua;
            return new_u;
        }
        #endregion

        #region  Четвертый пункт - вычисление первых 2(n-d) членов последовательности

        static int[] Sequence_u (int dk, int[] bk)
        {
            int[] sequence = new int[2*(Count_x - dk)];
            int[] u = U();
            for (int i=0; i<2*(Count_x - dk); i++)
            {
                sequence[i] = Count_u(u,i,bk);
                sequence[i] %= Galua;
            }
            return sequence;
        }

      

        #region Умножение данной матрицы на А
        static int[,] A_matr(int[,] matr)
        {
            int [,] tempA= new int[Count_x,Count_x];
            for (int i=0;i<Count_x;i++)
                for (int j=0;j<Count_x;j++)
                {
                    tempA[i,j] = 0;
                    for (int r = 0; r < Count_x; r++)
                    {
                        tempA[i, j] += (int)(matr[i, r] * coefficients[r, j]);
                        tempA[i, j] %= Galua;
                    }
                   
                 }
            return tempA;

        }
        #endregion

        #region Вычисление A в степени i

        static int[,] A_in_i(int degree)
        {
            int[,] Ai;
            if (degree==0)
            {
                Ai = new int[Count_x, Count_x];
                for (int i = 0; i < Count_x; i++) Ai[i, i] = 1;
                return Ai;
            }
            if (degree == 1) return coefficients;
            Ai = A_matr(coefficients);
            for (int i = 0; i < degree - 2; i++) Ai = A_matr(Ai);
            return Ai;
        }

        #endregion

        #region Вычисление A^i*bk
        static int[] Aprodb (int degree, int[] bk)
        {
            int[,] Ai = A_in_i(degree);
            int[] temp_prod = new int[Count_x];
            for (int i = 0; i < Count_x; i++ )
            {
                temp_prod[i]=0;
                for (int r = 0; r < Count_x; r++) temp_prod[i] += Ai[i, r] * bk[r];
                temp_prod[i] %= Galua;
            }
             return temp_prod;
        }
        #endregion

        #region Вычисление (u,A^i*bk)
        static int Count_u( int[] next_u, int degreeA, int[] bk)
        {
            int count_u=0;
            int[] right = Aprodb(degreeA, bk);
            for (int i = 0; i < Count_x; i++) count_u += next_u[i] * right[i];
            count_u %= Galua;
            return count_u;
        }
        #endregion

        #endregion

        #region Пятый пункт(только нормализация св. коэф. (остальное в отдельном классе)
        static int[] Berlekamp(int[] sequence )
        {
            int[] polunom;
            Berlekamp_Massey bm = new Berlekamp_Massey(sequence,sequence.Length);
            polunom = bm.algorithm();
            if (polunom[0]!=1)
            {
                int mn = polunom[0];
                for (int i = 0; i < polunom.Length; i++)
                {
                    polunom[i] = GaluaDiv(polunom[i], mn);
                }
            }
            return polunom;
        }
        #endregion

        #region Шестой пункт

        #region Находим следующий у
        static int[] Found_yk1 (int[] yk,int[] f,int[] bk)
        {
            int[] f_k1= new int[Count_x];
            int[,] Af = f_for_y(f);
            for (int i = 0; i < Count_x; i++)
            {
                f_k1[i] = 0;
                for (int r = 0; r < Count_x; r++) f_k1[i] += Af[i, r] * bk[r];
                f_k1[i] += yk[i];
                f_k1[i] %= Galua;
            }

            return f_k1;

 
        }
        #endregion

        #region Вычисление f c домиком ( для у)
        static int[,] f_for_y(int[] f)
        {
            int[,] sum = new int[Count_x, Count_x];
            int[,] nextf = new int[Count_x, Count_x];
            int l = f.Length;
            for (int i = 0; i < l - 1;i++ )
            {
                nextf = A_in_i(i);
                count_matr(ref sum, nextf,f[i+1]);
            }
                return sum;
        }

        static void count_matr(ref int[,] sum, int[,] coef, int n)
        {
            for (int i = 0; i < Count_x; i++)
            {
                for (int j = 0; j < Count_x; j++)
                {
                    sum[i, j] += n * coef[i, j];
                    sum[i, j] %= Galua;
                }
            }
        }

        #endregion

        #region Находим следующее b
         static int[] Found_bk1(int[] yk1)
        {
            for (int i = 0; i < Count_x; i++)
            {
                b[i] = 0;
                for (int r = 0; r < Count_x; r++) b[i] += coefficients[i, r] * yk1[r];
                b[i] += b0[i];
                b[i] %= Galua;
            }
            return b;
        }

        #endregion

        #region находим d
        static int Found_d(int[] f, int dk)
         {
            return (dk + f.Length-1);
         }

         #endregion

        #endregion

        #region Деление в полях Галуа
        public static int GaluaDiv(int ch, int  Zn )
        {
            int D;
            int Ch = ch;
            while (Ch < 0) Ch+= Galua;
            while (Ch % Zn != 0) Ch += Galua;
            D = (int)(Ch / Zn);
            D %= (int)Galua; 
            return D;
        }
        #endregion



    }
}
