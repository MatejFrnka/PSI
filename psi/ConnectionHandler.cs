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
            Byte[] bytes = new Byte[256];
            Byte[] message = new Byte[256];

            Console.WriteLine("Connected!");


            // Get a stream object for reading and writing
            NetworkStream stream = client.GetStream();

            string data;
            int i;
            int o = 0;

            try
            {
                // Loop to receive all the data sent by the client.
                while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                {
                    // Translate data bytes to a ASCII string.
                    data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                    Console.WriteLine("Received: {0}", data);

                    string response = null;
                    fill_array(bytes, message, i, o);
                    o = o + i;


                    if (o < 1 || message[o - 1] != '\b')
                        continue;
                    if (o < 2 || message[o - 2] != '\a')
                        continue;

                    foreach (Tuple<int, byte[]> item in splitInputs(ref message, ref o))
                    {
                        try
                        {
                            response = currentBehaviour.HandleInput(item.Item2, item.Item1, ref this.currentBehaviour);
                        }
                        catch (InvalidInputException e)
                        {
                            response = ResponseCode.SERVER_SYNTAX_ERROR;
                            currentBehaviour = new EndConnectionBehaviour();
                        }
                        byte[] msg = System.Text.Encoding.ASCII.GetBytes(response);
                        // Send back a response.
                        stream.Write(msg, 0, msg.Length);
                        Console.WriteLine("Sent: {0}", response);

                        if (this.currentBehaviour.endConnection())
                            break;
                    }
                    if (this.currentBehaviour.endConnection())
                        break;
                }
            }
            catch (Exception e)
            {
                if (message[o - 1] == '\a')
                    message[o++] = (byte)'\b';
                if (message[o - 2] != '\a')
                {
                    message[o++] = (byte)'\a';
                    message[o++] = (byte)'\b';
                }

                try
                {
                    currentBehaviour.HandleInput(message, o, ref this.currentBehaviour);
                }
                catch (InvalidInputException b)
                {
                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(ResponseCode.SERVER_SYNTAX_ERROR);
                    stream.Write(msg, 0, msg.Length);
                }

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
