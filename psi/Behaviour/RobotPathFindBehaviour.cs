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
        string turnSide = null;
        protected override string HandleBehaviour(RobotPos currentPos, ref BehaviourComponent output)
        {

            bool moved = !(this.previousPos == currentPos);
            if (!moved)
            {
                Console.WriteLine("stuck");
            }
            string response;
            if (turned)
            {
                //Console.WriteLine("moving after turn" + currentPos);
                turned = false;
                response = ResponseCode.SERVER_MOVE;
            }
            else if (moved)
            {
                if (direction.getNextPos(currentPos).distanceToOrigin() < currentPos.distanceToOrigin()) //is current direction moving towards end?
                {
                    turnSide = null;
                    turned = false;

                    //Console.WriteLine("moving " + currentPos);
                    response = ResponseCode.SERVER_MOVE;
                }
                else
                {
                    //Console.WriteLine("turning to continue");
                    string turnTo = findCloserTurn(currentPos);
                    turn(turnTo);
                    response = turnTo;
                }
            }
            else if(turnSide != null)
            {
               // Console.WriteLine("turning because stuck");
                turn(turnSide);
                response = turnSide;
            }
            else
            {
                string turnTo = findCloserTurn(currentPos);
                turn(turnTo);
                response = turnTo;
            }
            this.previousPos = currentPos;
            return response;
           
        }
        private void turnLeft()
        {
            this.direction = direction.turnLeft();
        }
        private void turnRight()
        {
            this.direction = direction.turnRight();
        }
        private string opposite(string turnSide)
        {
            if (turnSide == ResponseCode.SERVER_TURN_LEFT)
            {
                return ResponseCode.SERVER_TURN_RIGHT;
            }
            else if (turnSide == ResponseCode.SERVER_TURN_RIGHT)
            {
                return ResponseCode.SERVER_TURN_LEFT;
            }
            else
            {
                throw new Exception("turn not work");
            }
        }
        private void turn(string turnCommand)
        {
            turned = true;
            turnSide = turnCommand;
            //Console.WriteLine("turning " + direction + " " + turnCommand);

            if (turnSide == ResponseCode.SERVER_TURN_LEFT)
            {
                turnLeft();
            }
            else if (turnSide == ResponseCode.SERVER_TURN_RIGHT)
            {
                turnRight();
            }
            else
            {
                throw new Exception("turn not work");
            }
        }
        private string findCloserTurn(RobotPos currentPos)
        {
            RobotDirection nextDirection = direction.turnLeft();
            if (direction.turnRight().getNextPos(currentPos).distanceToOrigin() < nextDirection.getNextPos(currentPos).distanceToOrigin())
            {
                nextDirection = direction.turnRight();
            }
            return nextDirection.getCommand();
        }
    }
}
