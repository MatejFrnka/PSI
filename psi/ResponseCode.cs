using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psi
{
    class ResponseCode
    {
        public static string SUFFIX { get { return "\a\b"; } }
        public static string SERVER_MOVE { get { return "102 MOVE\a\b"; } }
        public static string SERVER_TURN_LEFT { get { return "103 TURN LEFT\a\b"; } }
        public static string SERVER_TURN_RIGHT { get { return "104 TURN RIGHT\a\b"; } }
        public static string SERVER_PICK_UP { get { return "105 GET MESSAGE\a\b"; } }
        public static string SERVER_LOGOUT { get { return "106 LOGOUT\a\b"; } }
        public static string SERVER_KEY_REQUEST { get { return "107 KEY REQUEST\a\b"; } }
        public static string SERVER_OK { get { return "200 OK\a\b"; } }
        public static string SERVER_LOGIN_FAILED { get { return "300 LOGIN FAILED\a\b"; } }
        public static string SERVER_SYNTAX_ERROR { get { return "301 SYNTAX ERROR\a\b"; } }
        public static string SERVER_LOGIC_ERROR { get { return "302 LOGIC ERROR\a\b"; } }
        public static string SERVER_KEY_OUT_OF_RANGE_ERROR { get { return "303 KEY OUT OF RANGE\a\b"; } }

        public static string PrepareResponse(string value) { return value + SUFFIX; }
    }
}
