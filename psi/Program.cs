using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace psi
{
    class Program
    {
        static void Main(string[] args)
        {
            RobotPos robotPos = new RobotPos() { x = 0, y = 0 };
            RobotPos up = new RobotPos() { x = 0, y = 1 };
            RobotPos down = new RobotPos() { x = 0, y = -1 };
            RobotPos left = new RobotPos() { x = -1, y = 0 };
            RobotPos right = new RobotPos() { x = 1, y = 0 };
            var a = RobotDirection.getDirection(robotPos, up);
            Console.WriteLine(RobotDirection.getDirection(robotPos, up) is RobotDirectionUp);
            Console.WriteLine(RobotDirection.getDirection(robotPos, down) is RobotDirectionDown);
            Console.WriteLine(RobotDirection.getDirection(robotPos, left) is RobotDirectionLeft);
            Console.WriteLine(RobotDirection.getDirection(robotPos, right) is RobotDirectionRight);




            TcpListener server = null;
            try
            {
                // Set the TcpListener on port 13000.
                Int32 port = 9999;
                IPAddress localAddr = IPAddress.Parse("127.0.0.1");

                // TcpListener server = new TcpListener(port);
                server = new TcpListener(localAddr, port);

                // Start listening for client requests.
                server.Start();

                // Enter the listening loop.
                while (true)
                {
                    ConnectionHandler connectionHandler = new ConnectionHandler();
                    Console.Write("Waiting for a connection... ");

                    // Perform a blocking call to accept requests.
                    // You could also use server.AcceptSocket() here.
                    connectionHandler.startListening(server.AcceptTcpClient());

                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            finally
            {
                // Stop listening for new clients.
                server.Stop();
            }

            Console.WriteLine("\nHit enter to continue...");
            Console.Read();
        }
    }
}
