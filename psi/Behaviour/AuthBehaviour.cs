using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psi
{
    class KeyGroup
    {
        public ushort serverKey;
        public ushort clientKey;
    }
    class AuthBehaviour : BehaviourComponent
    {
        private string clientName;
        private string clientId;
        private const int MOD_VAL = 65536;
        bool endConn = false;

        public AuthBehaviour()
        {
            maxLen = ClientResponseHandler.ClientResponseMaxSize.CLIENT_USERNAME;
        }
        private Dictionary<string, KeyGroup> keyValues = new Dictionary<string, KeyGroup>()
        {
            {"0", new KeyGroup{serverKey = 23019, clientKey = 32037}},
            {"1", new KeyGroup{serverKey = 32037, clientKey = 29295}},
            {"2", new KeyGroup{serverKey = 18789, clientKey = 13603}},
            {"3", new KeyGroup{serverKey = 16443, clientKey = 29533}},
            {"4", new KeyGroup{serverKey = 18189, clientKey = 21952}},
        };

        public override string HandleInput(byte[] input, int length, ref BehaviourComponent behaviour)
        {
            if (clientName == null)
            {
                clientName = ClientResponseHandler.CLIENT_USERNAME(input, length);
                maxLen = ClientResponseHandler.ClientResponseMaxSize.CLIENT_KEY_ID;
                Console.WriteLine(clientName);
                return ResponseCode.SERVER_KEY_REQUEST;
            }
            if (clientId == null)
            {
                clientId = ClientResponseHandler.CLIENT_KEY_ID(input, length);
                Console.WriteLine(clientId);
                maxLen = ClientResponseHandler.ClientResponseMaxSize.CLIENT_CONFIRMATION;
                if (keyValues.ContainsKey(clientId))
                {
                    uint val = (getHash(clientId) + keyValues[clientId].serverKey) % MOD_VAL;
                    return ResponseCode.PrepareResponse(val.ToString());
                }
                else
                {
                    behaviour = new EndConnectionBehaviour();
                    return ResponseCode.SERVER_KEY_OUT_OF_RANGE_ERROR;
                }
            }
            {
                uint val = (getHash(clientId) + keyValues[clientId].clientKey) % MOD_VAL;
                if (val == ClientResponseHandler.CLIENT_CONFIRMATION(input, length))
                {
                    Console.WriteLine(val);
                    behaviour = new RobotDirectionBehaviour();
                    return ResponseCode.SERVER_OK + ResponseCode.SERVER_MOVE;
                }
                else
                {
                    behaviour = new EndConnectionBehaviour();
                    return ResponseCode.SERVER_LOGIN_FAILED;
                }
            }
        }
        public override bool endConnection()
        {
            return this.endConn;
        }

        private uint getHash(string from)
        {
            uint val = 0;
            byte[] name = System.Text.Encoding.ASCII.GetBytes(clientName);

            foreach (byte n in name)
            {
                val = val + n;
            }
            val = (val * 1000) % MOD_VAL;
            return val;
        }
    }
}
