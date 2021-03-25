using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psi
{
    class RobotDirectionBehaviour : RobotNavigationBehaviour
    {
        RobotPos start = null;
        bool turned = false;
        protected override string HandleBehaviour(RobotPos currentPos, ref BehaviourComponent output)
        {
            if (turned)
            {
                turned = false;
                return ResponseCode.SERVER_MOVE;
            }
            //Console.WriteLine("finding d" + currentPos);
            if (start == null)
            {
                start = currentPos;
                return ResponseCode.SERVER_MOVE;
            }
            RobotDirection direction = RobotDirection.getDirection(start, currentPos);
            if (direction != null)
            {
                Console.WriteLine("found direction");
                output = new RobotPathFindBehaviour(direction);
                return ResponseCode.SERVER_MOVE;
            }

            start = currentPos;
            turned = true;
            return ResponseCode.SERVER_TURN_LEFT;
        }
    }
}
