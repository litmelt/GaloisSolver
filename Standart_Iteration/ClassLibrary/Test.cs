using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
   public class Test
    {

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
