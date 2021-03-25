using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public static bool operator ==(RobotPos a, RobotPos b)
        {
            if (a is null || b is null)
                return a is null && b is null;
            return a.x == b.x && a.y == b.y;
        }
        public static bool operator !=(RobotPos a, RobotPos b)
        {
            return !(a == b);
        }

        public override string ToString()
        {
            return "{" + x + "},{" + y + "}";
        }
    }
}
