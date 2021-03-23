using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psi
{
    class RobotControllerBehaviour : RobotNavigationBehaviour
    {
        public RobotControllerBehaviour(RobotDirection direction)
        {
            maxLen = ClientResponseHandler.ClientResponseMaxSize.CLIENT_OK;
            this.direction = direction;
            Console.WriteLine(direction.GetType());
        }
        RobotDirection direction = null;
        RobotPos previousPos = null;
        bool moveForward = false;

        protected override string HandleBehaviour(RobotPos currentPos, ref BehaviourComponent output)
        {
            bool stuck = currentPos == this.previousPos;
            this.previousPos = currentPos;
            if (moveForward)
            {
                moveForward = false;
                return ResponseCode.SERVER_MOVE;
            }

            // set closer to origin direction as turnDirection
            RobotDirection turnDirection = direction.turnLeft();
            if (turnDirection.getNextPos(currentPos).distanceToOrigin() > direction.turnRight().getNextPos(currentPos).distanceToOrigin())
                turnDirection = direction.turnRight();
            // maybe add check backwards
            //if turn direction is closer than forward
            if (stuck || turnDirection.getNextPos(currentPos).distanceToOrigin() < direction.getNextPos(currentPos).distanceToOrigin())
            {
                direction = turnDirection;
                moveForward = true;
                Console.WriteLine(turnDirection.GetType());
                return turnDirection.getCommand();
            }
            //otherwise just forward
            return ResponseCode.SERVER_MOVE;
        }
    }
}
