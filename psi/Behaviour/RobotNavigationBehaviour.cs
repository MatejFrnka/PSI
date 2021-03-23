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
        public override string HandleInput(byte[] input, int length, ref BehaviourComponent output)
        {
            RobotPos currentPos = ClientResponseHandler.CLIENT_OK(input, length);
            Console.WriteLine($"x: {currentPos.x} y: {currentPos.y}");
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
