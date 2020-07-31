using System;
using System.Collections.Generic;

namespace Assets.Scripts
{
    class NUmberManager
    {
        int number;
        int[] number_array = { 0, 0, 0, 0, 0, 0 };
        public NUmberManager(int number)
        {
            this.number = number;
            transform();
        }
        void transform()
        {
            int the_num = number;
            for (int i = 100000, j = 0; i >= 1; i /= 10, j++)
            {
                number_array[j] = the_num / i;
                the_num %= i;
            }
        }
        public int[] get_value()
        {
            return number_array;
        }
    }
}
