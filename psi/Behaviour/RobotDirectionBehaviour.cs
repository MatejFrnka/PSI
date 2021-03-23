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
        protected override string HandleBehaviour(RobotPos currentPos, ref BehaviourComponent output)
        {
            if (start == null)
            {
                start = currentPos;
                return ResponseCode.SERVER_MOVE;
            }
            RobotDirection direction = RobotDirection.getDirection(start, currentPos);
            if (direction != null)
            {
                Console.WriteLine("found direction");
                output = new RobotControllerBehaviour(direction);
                return ResponseCode.SERVER_MOVE;
            }

            start = currentPos;
            return ResponseCode.SERVER_TURN_LEFT + ResponseCode.SERVER_MOVE;
        }
    }
}
