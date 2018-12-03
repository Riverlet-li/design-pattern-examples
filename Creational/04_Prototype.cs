/**
 * Prototype 原型
 * 意图：使用原型关联对象的种类，通过拷贝创建新对象
 * 扩展：被实例化的类
 * 关联：
 *      1.Abstract Factory使用Prototype在支持类对象的语言中创建对象；
 * ***/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeGame
{
    // 原型工厂类
    class PrototypeMazeFactory : MazeFactory
    {
        private Maze _protoMaze = null;
        private Wall _protoWall = null;
        private Room _protoRoom = null;
        private Door _protoDoor = null;

        public PrototypeMazeFactory(Maze maze, Wall wall, Room room, Door door)
        {
            this._protoMaze = maze;
            this._protoWall = wall;
            this._protoRoom = room;
            this._protoDoor = door;
        }
        public override Maze MakeMaze() { return this._protoMaze.Clone(); }
        public override Wall MakeWall() { return this._protoWall.Clone() as Wall; }
        public override Room MakeRoom(int rNo) { return this._protoRoom.Clone() as Room; }
        public override Door MakeDoor(Room r1, Room r2) { return this._protoDoor.Clone() as Door; }
    }

    class PrototypeTest
    {
        static void Main(string[] args)
        {
            MazeGame game = new MazeGame();
            Maze maze = null;

            //【扩展】定制各种原型Factory
            PrototypeMazeFactory bombedFactory = new PrototypeMazeFactory(
                new Maze(),
                new BombedWall(),
                new BombedRoom(0),
                new BombedDoor(null, null)
                );
            maze = game.CreateMazeWithFactory(bombedFactory);

            PrototypeMazeFactory defaultFactory = new PrototypeMazeFactory(
                new Maze(),
                new Wall(),
                new Room(0),
                new Door(null, null)
                );
            maze = game.CreateMazeWithFactory(defaultFactory);
        }
    }
}
