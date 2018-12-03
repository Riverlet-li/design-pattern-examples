using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeGame
{
    class EnchantedRoom : Room 
    {
        public EnchantedRoom(int rNo) : base(rNo) { }
    }

    class EnchantedDoor : Door
    {
        public EnchantedDoor(Room r1, Room r2) : base(r1, r2) { }
    }
}
