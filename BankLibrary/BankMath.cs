using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankLibrary
{
    public class BankMath
    {
        public static string DelValue(string Difference, string Reduced)
        {
            long diff;
            long red;

            diff = Convert.ToInt64(Difference);
            red = Convert.ToInt64(Reduced);

            diff -= red;
            return Convert.ToString(diff);
        }

        public static string SumValue(string Sum, string Summand)
        {
            long sum;
            long summand;

            sum = Convert.ToInt64(Sum);
            summand = Convert.ToInt64(Summand);

            sum += summand;
            return Convert.ToString(sum);
        }
    }
}
