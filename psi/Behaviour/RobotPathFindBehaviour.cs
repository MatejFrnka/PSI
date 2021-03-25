using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psi
{
    class RobotPathFindBehaviour : RobotNavigationBehaviour
    {
        public RobotPathFindBehaviour(RobotDirection direction)
        {
            maxLen = ClientResponseHandler.ClientResponseMaxSize.CLIENT_OK;
            this.direction = direction;
            Console.WriteLine(direction.GetType());
        }
        RobotDirection direction = null;
        RobotPos previousPos = null;
        bool turned = false;
        protected override string HandleBehaviour(RobotPos currentPos, ref BehaviourComponent output)
        {

            bool moved = !(this.previousPos == currentPos);
            string response;
            if (turned)
            {
                turned = false;
                response = ResponseCode.SERVER_MOVE;
            }
            else
            {
                if (moved && direction.getNextPos(currentPos).distanceToOrigin() < currentPos.distanceToOrigin()) //is current direction moving towards end?
                {
                    turned = false;
                    response = ResponseCode.SERVER_MOVE;
                }
                else
                {
                    RobotDirection nextDirection = direction.turnLeft();
                    if (direction.turnRight().getNextPos(currentPos).distanceToOrigin() < nextDirection.getNextPos(currentPos).distanceToOrigin())
                    {
                        nextDirection = direction.turnRight();
                    }
                    this.direction = nextDirection;
                    turned = true;
                    response = nextDirection.getCommand();
                }
            }
            this.previousPos = currentPos;
            return response;

        }
    }
}