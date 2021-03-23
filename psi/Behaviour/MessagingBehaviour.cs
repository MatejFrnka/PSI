using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psi
{
    class MessagingBehaviour : BehaviourComponent
    {
        public MessagingBehaviour()
        {
            maxLen = 100;
        }
        public override string HandleInput(byte[] input, int length, ref BehaviourComponent output)
        {
            string message = ClientResponseHandler.CLIENT_MESSAGE(input, length);
            Console.WriteLine("message is: ", message);
            output = new EndConnectionBehaviour();
            return ResponseCode.SERVER_LOGOUT;
        }
    }
}
