using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassLibrary1
{
    class Berlekamp_Massey
    {
        int[][] G ; //функция G с( хранит коэффициенты при х)
        int lt;
        static int l;
        int[] k;
        int[] u;
        int[][] uin;
        
        int t;
        int[] m;
        public Berlekamp_Massey(int[] u0,int lv )
        {
            l = lv;
            int n = Iteration.Galua*1000 + 1;
            G = new int[n][];
            uin = new int[n][];
            k = new int[n];
            m = new int[n];
            G[0] = new int[1];
            G[0][0] = 1;
            m[0] = 0;
            t = 0;
            u = new int[l];
            u0.CopyTo(u, 0);
            uin[t] = Found_G_on_u(G[t], u);
            k[t] = Count_k(uin[t]);
            lt =k[0];
            
        }

        public int[] algorithm()
        {
            if (lt >= l) return G[t];
            FoundG1();
            Next_lst();
            if (lt >= l) return G[t];
            m[1] = G[1].Length - 1;
             int s = 0;
            while (lt< l)
            {
                s = Found_s();
                if (k[t] > k[s])
                {
                    FoundGt1_Yes(s);
                }
                else
                {
                    FoundGt1_No(s);
                }
                m[t+1] = G[t+1].Length - 1;
                Next_lst();
            }
            return G[t];
        }



        void FoundG1()
        {
            int u0;
            G[1]=new int[k[0]+1+1];
            G[1][k[0] + 1] = 1;
            for (int i = 0; i < G[0].Length; i++)
            {
                u0 = Iteration.GaluaDiv(uin[0][k[0] + 1], uin[0][k[0]]);
                G[1][i] = - G[0][i]*u0;
                In_Galua(ref G[1][i]) ;
            }

        }

        void FoundGt1_Yes(int s)
        {
            G[t + 1] = new int[k[t] - k[s] + G[t].Length];
            int ui = 0;
            for (int i = 0; i < G[t].Length; i++)
			{
                G[t + 1][k[t] - k[s] + i]+= G[t][i];


                ui = Iteration.GaluaDiv(uin[t][k[t]], uin[s][k[s]]);

                if (i < G[s].Length) G[t + 1][i] -= ui * G[s][i];
                In_Galua(ref G[t + 1][i]) ;

                G[t + 1][i] %= Iteration.Galua;
                In_Galua(ref G[t + 1][k[t] - k[s] + i]);
            }
        
        }

        void FoundGt1_No(int s)
        {
            G[t+1] = new int[k[s] - k[t] + G[t].Length ];
            G[t].CopyTo(G[t+1], 0);
            int ui = 0;
            for (int i = 0; i < G[s].Length; i++)
            {

                ui = Iteration.GaluaDiv(u[k[t]], u[k[s]]);
                 G[t + 1][k[s] - k[t] + i] -= G[s][i]*ui;
                 In_Galua(ref G[t + 1][k[s] - k[t] + i]);

            }
        }

        void Next_lst()
        {
            uin[t+1] = Found_G_on_u(G[t+1], u);
            k[t+1] = Count_k(uin[t+1]);

            lt = G[t + 1].Length - 1;
            lt +=k[t+1];
            t += 1;
        }

        static int Count_k(int[] g)
        {
            int k = 0 ;
            while (g[k] == 0)
            {
                k += 1;
                if (k >= g.Length)
                {
                    k = l + 1;
                    break;
                }
            }
          return k;
        }


        int Found_s()
        {
            int s = t;
            while (m[t] == m[s]) s -= 1;
                return s;
        }

        static int[] Found_G_on_u(int[] g,int[] u)
        {
            int maxi = u.Length-g.Length+1;
            int[] V= new int[maxi];
            for (int i = 0; i < maxi; i++)
            {
                V[i] = 0;
                for (int j = 0; j < g.Length; j++)
                {
                    V[i] += g[j] * u[i + j]; 
                }
                In_Galua(ref V[i]);
            }
            return V;
        }

        static int In_Galua(ref int g)
        {
            while (g < 0) g += Iteration.Galua;
            g %= Iteration.Galua;
            return g;
        }


     }
}
