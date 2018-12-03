using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeGame
{
    class BombedRoom : Room 
    {
        public BombedRoom(int rNo) : base(rNo) { }
    }
    class BombedDoor : Door
    {
        public BombedDoor(Room r1, Room r2) : base(r1, r2) { }
    }
    class BombedWall : Wall
    {
        public BombedWall() : base() { }
    }
}
