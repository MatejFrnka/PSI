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
        public ConnectionHandler()
        {
            currentBehaviour = new AuthBehaviour();
        }

        public void startListening(TcpClient client)
        {

            client.ReceiveTimeout = 1000;
            // Buffer for reading data
            Byte[] message = new Byte[256];

            Console.WriteLine("Connected!");


            // Get a stream object for reading and writing
            NetworkStream stream = client.GetStream();

            string data;
            int i;
            int o = 0;
            bool messageEnding = false;

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
                        
                        //message ended;
                        //check recharging
                        try
                        {
                            response = currentBehaviour.HandleInput(message, o, ref this.currentBehaviour);
                        }
                        catch (Exception e)
                        {
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
                        byte[] msg = System.Text.Encoding.ASCII.GetBytes(response);
                        // Send back a response.
                        stream.Write(msg, 0, msg.Length);
                    }
                    else
                    {
                        messageEnding = false;
                    }

                    if (o == this.currentBehaviour.maxLen)
                    {
                        byte[] msg = System.Text.Encoding.ASCII.GetBytes(ResponseCode.SERVER_SYNTAX_ERROR);
                        stream.Write(msg, 0, msg.Length);
                        currentBehaviour = new EndConnectionBehaviour();
                    }

                    if (this.currentBehaviour.endConnection())
                        break;

                }
            }
            catch (InvalidOperationException e) //
            {
                Console.WriteLine(e.Message);
            }
            // Shutdown and end connection
            client.Close();
        }

        public bool verifyInput(string input)
        {
            return true;
        }
        private void fill_array(byte[] from, byte[] to, int fromLen, int fillFrom)
        {
            for (int i = 0; i < fromLen; i++)
            {
                to[i + fillFrom] = from[i];
            }
        }
        private List<Tuple<int, byte[]>> splitInputs(ref byte[] input, ref int length)
        {
            bool endSequence = false;
            List<Tuple<int, byte[]>> result = new List<Tuple<int, byte[]>>();
            byte[] current = new byte[256];
            int current_len = 0;
            for (int i = 0; i < length; i++)
            {
                if (input[i] == '\a')
                {
                    endSequence = true;
                    current[current_len++] = input[i];
                }
                else if (input[i] == '\b' && endSequence)
                {

                    current[current_len++] = input[i];
                    result.Add(new Tuple<int, byte[]>(current_len, current));
                    current_len = 0;
                    current = new byte[256];
                    endSequence = false;
                }
                else
                {
                    endSequence = false;
                    current[current_len++] = input[i];
                }
            }
            input = current;
            length = current_len;
            return result;
        }
    }
}
