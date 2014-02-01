using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MotorOnline.Web
{
    public class ChangeTypeHelper
    {
        public static int SafeParseObjectToInt32(object result)
        {
            int number = 0;

            return number;
        }

        public static double SafeParseToDouble(string value)
        {
            double result = 0;
            double.TryParse(value, out result);
            return result;
        }

        public static int SafeParseToInt32(string value)
        {
            int result = 0;
            int.TryParse(value, out result);
            return result;
        }

        public static bool SafeParseToBoolean(string value)
        {
            bool result;
            bool.TryParse(value, out result);
            return result;
        }
    }
}