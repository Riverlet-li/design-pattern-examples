/**
 * Factory Method 工厂方法
 * 意图：定义一个创建对象的接口，让子类实例化那一个类
 * 扩展：被实例化的子类
 * 关联：
 *      1.Abstract Factory使用其实现Factory类；
 *      2.Template Method使用其实现Factory类
 * ***/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeGame
{
    partial class MazeGame
    {
        // 工厂方法，提供给子类的钩子
        public virtual Maze MakeMaze() { return new Maze(); }
        public virtual Room MakeRoom(int rNo) { return new Room(rNo); }
        public virtual Wall MakeWall() { return new Wall(); }
        public virtual Door MakeDoor(Room r1, Room r2) { return new Door(r1, r2); }

        public Maze CreateMazeWithFactoryMethod()
        {
            Maze maze = this.MakeMaze();
            Room r1 = this.MakeRoom(1);
            Room r2 = this.MakeRoom(2);
            Door door = this.MakeDoor(r1, r2);

            maze.AddRoom(r1);
            maze.AddRoom(r2);

            r1.SetSide(Direction.North, this.MakeWall());
            r1.SetSide(Direction.South, this.MakeWall());
            r1.SetSide(Direction.East, door);
            r1.SetSide(Direction.West, this.MakeWall());

            r2.SetSide(Direction.North, this.MakeWall());
            r2.SetSide(Direction.South, this.MakeWall());
            r2.SetSide(Direction.East, this.MakeWall());
            r2.SetSide(Direction.West, door);

            return maze;
        }
    }

    class BombedMazeGame : MazeGame
    {
        // 【扩展】子类覆盖父类的实现
        public override Room MakeRoom(int rNo) { return new BombedRoom(rNo); }
        public override Wall MakeWall() { return new BombedWall(); }
    }

    class FactoryMethodTest
    {
        static void Main(string[] args)
        {
            MazeGame game = new BombedMazeGame();
            Maze maze = game.CreateMazeWithFactoryMethod();
        }
    }
}
