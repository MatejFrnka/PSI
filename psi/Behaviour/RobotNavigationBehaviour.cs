using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psi
{
    abstract class RobotNavigationBehaviour : BehaviourComponent
    {
        public RobotNavigationBehaviour()
        {
            maxLen = ClientResponseHandler.ClientResponseMaxSize.CLIENT_OK;
        }
        static int i = 0;
        public override string HandleInput(byte[] input, int length, ref BehaviourComponent output)
        {

            RobotPos currentPos = ClientResponseHandler.CLIENT_OK(input, length);
            Console.WriteLine(currentPos);
            /*
            if (i == 0)
            {
                i++;
                return ResponseCode.SERVER_TURN_RIGHT;
            }
            if (i < 5)
            {
                i++;
                Console.WriteLine(currentPos);
                return ResponseCode.SERVER_MOVE;
            }*/


            if (currentPos.distanceToOrigin() == 0)
            {
                output = new MessagingBehaviour();
                return ResponseCode.SERVER_PICK_UP;
            }
            return this.HandleBehaviour(currentPos, ref output);
        }
        protected abstract string HandleBehaviour(RobotPos currentPos, ref BehaviourComponent output);
    }
}
