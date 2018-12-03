using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeGame
{
    enum Direction
    {
        North,
        South,
        East,
        West,
    }

    abstract class MapSite
    {
        public abstract void Enter();
        public abstract MapSite Clone();
    }

    class Room : MapSite
    {
        MapSite[] _sides = null;
        int _roomNo = 0;

        public Room(int roomNo) {
            this._roomNo = roomNo;
            this._sides = new MapSite[4];
        }
        public MapSite GetSide(Direction dir) { return null; }
        public void SetSide(Direction dir, MapSite site) { }
        public override void Enter() { }
        public override MapSite Clone() { return new Room(this._roomNo); }

    }

    class Wall : MapSite
    {
        public Wall() { }
        public override void Enter() { }
        public override MapSite Clone() { return new Wall(); }
    }

    class Door : MapSite
    {
        Room _room1;
        Room _room2;
        bool _isOpen;
        public Door(Room r1, Room r2) {
            this._room1 = r1;
            this._room2 = r2;
            this._isOpen = false;
        }
        public override void Enter() { }
        public Room OtherSideFrom(Room r) { return null; }
        public override MapSite Clone() { return new Door(this._room1, this._room2); }
    }

    class Maze
    {
        public Maze() { }
        public void AddRoom(Room r) { }
        public Room RoomNo(int rNo) { return null; }
        public virtual Maze Clone() { return new Maze(); }

        public string DumpInfo()
        {
            return "Maze Info";
        }
    }
}
