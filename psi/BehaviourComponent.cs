using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psi
{
    abstract class BehaviourComponent
    {
        
        public abstract string HandleInput(byte[] input, int length, ref BehaviourComponent output);

        public virtual bool endConnection() { return false; }
        
    }
}
