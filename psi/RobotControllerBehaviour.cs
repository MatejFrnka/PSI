using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psi
{
    class RobotControllerBehaviour : BehaviourComponent
    {
        RobotDirection direction = null;
        RobotPos start = null;
        public override string HandleInput(byte[] input, int length, ref BehaviourComponent output)
        {
            RobotPos currentPos = ClientResponseHandler.CLIENT_OK(input, length);

            if (currentPos.distanceToOrigin() == 0)
            {
                output = new MessagingBehaviour();
                return ResponseCode.SERVER_PICK_UP;
            }
            if (start == null)
            {
                start = currentPos;
                return ResponseCode.SERVER_MOVE;
            }
            if(direction == null)
                direction = RobotDirection.getDirection(start, currentPos);

            // set closer to origin direction as turnDirection
            RobotDirection turnDirection = direction.turnLeft();
            if (turnDirection.getNextPos(currentPos).distanceToOrigin() > direction.turnRight().getNextPos(currentPos).distanceToOrigin())
                turnDirection = direction.turnRight();
            // maybe add check backwards
            //if turn direction is closer than forward
            if (turnDirection.getNextPos(currentPos).distanceToOrigin() < direction.getNextPos(currentPos).distanceToOrigin())
            {
                Console.WriteLine("turning, " + turnDirection.GetType());
                direction = turnDirection;
                return turnDirection.getCommand();
            }
            Console.WriteLine("moving, "+direction.GetType() );
            //otherwise just forward
            return ResponseCode.SERVER_MOVE;
        }
        
    }
}
