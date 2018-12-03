using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeGame
{
    partial class MazeGame
    {
        public Maze CreateMaze()
        {
            Maze maze = new Maze();

            Room r1 = new Room(1);
            Room r2 = new Room(2);
            Door door = new Door(r1, r2);

            maze.AddRoom(r1);
            maze.AddRoom(r2);

            r1.SetSide(Direction.North, new Wall());
            r1.SetSide(Direction.South, new Wall());
            r1.SetSide(Direction.East, door);
            r1.SetSide(Direction.West, new Wall());

            r2.SetSide(Direction.North, new Wall());
            r2.SetSide(Direction.South, new Wall());
            r2.SetSide(Direction.East, new Wall());
            r2.SetSide(Direction.West, door);

            return maze;
        }

        static void Main(string[] args)
        {
            MazeGame game = new MazeGame();
            Maze maze = game.CreateMaze();
        }
    }
}
