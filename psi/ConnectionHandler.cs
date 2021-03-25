using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace psi
{
    public class ConnectionHandler
    {

        private BehaviourComponent currentBehaviour;

        private const int TIMEOUT = 1000;
        private const int TIMEOUT_RECHARGE = 5000;
        public ConnectionHandler()
        {
            currentBehaviour = new AuthBehaviour();
        }

        public void startListening(TcpClient client)
        {
            client.ReceiveTimeout = TIMEOUT;
            Byte[] message = new Byte[256];

            Console.WriteLine("Connected!");

            NetworkStream stream = client.GetStream();

            int i;
            int o = 0;
            bool messageEnding = false;
            bool recharging = false;

            try
            {

                while ((i = stream.ReadByte()) != -1)
                {
                    message[o++] = (byte)i;

                    if (i == '\a')
                        messageEnding = true;
                    else if (messageEnding && i == '\b')
                    {
                        string response = null;
                        try
                        {
                            Console.WriteLine("recieved " + System.Text.Encoding.ASCII.GetString(message, 0, o));
                            if (recharging)
                            {
                                if (ClientResponseHandler.CLIENT_FULL_POWER(message, o))
                                {
                                    Console.WriteLine("recharged");
                                    client.ReceiveTimeout = TIMEOUT;
                                    recharging = false;
                                    response = "";
                                }
                                else
                                {
                                    Console.WriteLine("logic error");
                                    response = ResponseCode.SERVER_LOGIC_ERROR;
                                    this.currentBehaviour = new EndConnectionBehaviour();
                                }
                            }
                            else if (ClientResponseHandler.isRechargeRequest(message, o))
                            {
                                Console.WriteLine("recharging");
                                recharging = true;
                                response = "";
                                client.ReceiveTimeout = TIMEOUT_RECHARGE;
                            }
                            else
                            {
                                response = currentBehaviour.HandleInput(message, o, ref this.currentBehaviour);
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                            if (e is InvalidInputException || e is FormatException)
                            {
                                response = ResponseCode.SERVER_SYNTAX_ERROR;
                                currentBehaviour = new EndConnectionBehaviour();
                            }
                            else
                                throw;
                        }
                        finally
                        {
                            o = 0;
                            messageEnding = false;
                        }
                        Console.WriteLine("sending " + response);
                        byte[] msg = System.Text.Encoding.ASCII.GetBytes(response);
                        // Send back a response.
                        stream.Write(msg, 0, msg.Length);
                    }
                    else
                    {
                        messageEnding = false;
                    }

                    if (o == this.currentBehaviour.maxLen && !matchesRechargeMessage(message, o) && !recharging)
                    {
                        Console.WriteLine("max len reached");
                        byte[] msg = System.Text.Encoding.ASCII.GetBytes(ResponseCode.SERVER_SYNTAX_ERROR);
                        stream.Write(msg, 0, msg.Length);
                        currentBehaviour = new EndConnectionBehaviour();
                    }

                    if (this.currentBehaviour.endConnection())
                        break;

                }
            }
            catch (System.IO.IOException e) //
            {
                Console.WriteLine(e.Message);
            }
            // Shutdown and end connection
            client.Close();
        }
        private bool matchesRechargeMessage(byte[] msg, int length)
        {
            byte[] rechargeMessage = System.Text.Encoding.ASCII.GetBytes("RECHARGING\a\b");
            if (length > rechargeMessage.Length)
                return false;
            for (int i = 0; i < length; i++)
            {
                if (msg[i] != rechargeMessage[i])
                    return false;
            }
            return true;
        }
    }
}
