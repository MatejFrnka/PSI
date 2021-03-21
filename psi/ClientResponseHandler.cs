using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace psi
{
    class RobotPos
    {
        public int x;
        public int y;

        public double distanceToOrigin()
        {
            return Math.Pow(Math.Pow(x, 2) + Math.Pow(y, 2), 0.5);
        }
    }
    class ClientResponseHandler
    {
        public static string CLIENT_USERNAME(byte[] bytes, int length)
        {
            if (length < 3 || length > 20)
                throw new InvalidInputException();

            return System.Text.Encoding.ASCII.GetString(bytes, 0, length > 2 ? length - 2 : 0);
        }

        public static string CLIENT_KEY_ID(byte[] bytes, int length)
        {
            if (length < 3)
                throw new InvalidInputException();
            string result = System.Text.Encoding.ASCII.GetString(bytes, 0, 1);
            if(Regex.IsMatch(result,@"[^\d]"))
                throw new InvalidInputException();
            return result;
        }
        public static uint CLIENT_CONFIRMATION(byte[] bytes, int length)
        {
            if (length < 3 || length > 7)
                throw new InvalidInputException();
            string text = System.Text.Encoding.ASCII.GetString(bytes, 0, length > 2 ? length - 2 : 0);
            if (Regex.IsMatch(text, @"[^\d]"))
                throw new InvalidInputException();
            return  UInt32.Parse(text);
        }

        public static RobotPos CLIENT_OK(byte[] bytes, int length)
        {
            if (length < 5)
                throw new InvalidInputException();
            string value = System.Text.Encoding.ASCII.GetString(bytes, 3, length - 5);
            if (Regex.IsMatch(value, @"OK [\d]+ [\d]+"))
                throw new InvalidInputException();
            int rx = Int32.Parse(value.Split(' ')[0]);
            int ry = Int32.Parse(value.Split(' ')[1]);
            return new RobotPos() { x = rx, y = ry };
        }
        public static string CLIENT_MESSAGE(byte[] bytes, int length)
        {
            if (length < 3)
                throw new InvalidInputException();
            return System.Text.Encoding.ASCII.GetString(bytes, 0, length > 2 ? length - 2 : 0);
        }
    }
}
