using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psi
{
    abstract class RobotDirection
    {
        protected int x;
        protected int y;
        protected string command = "";
        public RobotPos getNextPos(RobotPos robotPos)
        {
            return new RobotPos() { x = robotPos.x + x, y = robotPos.y + y };
        }
        public string getCommand() { return command; }

        public abstract RobotDirection turnLeft();
        public abstract RobotDirection turnRight();

        public static RobotDirection getDirection(RobotPos start, RobotPos end)
        {
            if (start.y < end.y)
                return new RobotDirectionUp("");
            if (start.y > end.y)
                return new RobotDirectionDown("");
            if (start.x < end.x)
                return new RobotDirectionRight("");
            if (start.x > end.x)
                return new RobotDirectionLeft("");
            return null;
        }
    }
    class RobotDirectionUp : RobotDirection
    {
        public RobotDirectionUp(string command)
        {
            this.command = command;
            x = 0;
            y = 1;
        }
        public override RobotDirection turnLeft()
        {
            return new RobotDirectionLeft(ResponseCode.SERVER_TURN_LEFT);
        }

        public override RobotDirection turnRight()
        {
            return new RobotDirectionRight(ResponseCode.SERVER_TURN_RIGHT);
        }
    }
    class RobotDirectionDown : RobotDirection
    {
        public RobotDirectionDown(string command)
        {
            this.command = command;
            x = 0;
            y = -1;
        }
        public override RobotDirection turnLeft()
        {
            return new RobotDirectionRight(ResponseCode.SERVER_TURN_LEFT);
        }

        public override RobotDirection turnRight()
        {
            return new RobotDirectionLeft(ResponseCode.SERVER_TURN_RIGHT);
        }
    }
    class RobotDirectionLeft : RobotDirection
    {
        public RobotDirectionLeft(string command)
        {
            this.command = command;
            x = -1;
            y = 0;
        }
        public override RobotDirection turnLeft()
        {
            return new RobotDirectionDown(ResponseCode.SERVER_TURN_LEFT);
        }

        public override RobotDirection turnRight()
        {
            return new RobotDirectionUp(ResponseCode.SERVER_TURN_RIGHT);
        }
    }
    class RobotDirectionRight : RobotDirection
    {
        public RobotDirectionRight(string command)
        {
            this.command = command;
            x = 1;
            y = 0;
        }
        public override RobotDirection turnLeft()
        {
            return new RobotDirectionUp(ResponseCode.SERVER_TURN_LEFT);
        }

        public override RobotDirection turnRight()
        {
            return new RobotDirectionDown(ResponseCode.SERVER_TURN_RIGHT);
        }
    }

}
