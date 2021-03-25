using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psi
{
    class EndConnectionBehaviour : BehaviourComponent
    {
        public EndConnectionBehaviour()
        {
            
        }
        public override string HandleInput(byte[] input, int length, ref BehaviourComponent output)
        {
            throw new NotImplementedException();
        }
        public override bool endConnection()
        {
            return true;
        }
    }
}
