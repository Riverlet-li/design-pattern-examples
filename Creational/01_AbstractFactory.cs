/*
 * Abstract Factory 抽象工厂模式
 * 意图：提供接口，用于创建一系列相关或相互依赖的对象，而无需指定他们的具体类型
 * 扩展：产品对象家族
 * 关联：
 *      1.使用prototype动态配置工厂
 *      2.使用Factory Method实现产品创建函数
 *      3.将工厂定义为Singleton
 **/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeGame
{
    // 标准工厂类，不建议定义为abstract
    class MazeFactory
    {
        public MazeFactory() { }

        public virtual Maze MakeMaze() { return new Maze(); }
        public virtual Wall MakeWall() { return new Wall(); }
        public virtual Room MakeRoom(int rNo) { return new Room(rNo); }
        public virtual Door MakeDoor(Room r1, Room r2) { return new Door(r1, r2); }
    }

    // 【扩展部分】enchanted 系列产品的工程类
    class EnchantedMazeFactory : MazeFactory
    {
        // 可选：使用singleton
        /*
        private static EnchantedMazeFactory _instance = new EnchantedMazeFactory();
        public static EnchantedMazeFactory Instance { get { return _instance; } private set; }
        private EnchantedMazeFactory() { }
        */
        public EnchantedMazeFactory() { }
        public override Room MakeRoom(int rNo) { return new EnchantedRoom(rNo);}
        public override Door MakeDoor(Room r1, Room r2) { return new EnchantedDoor(r1, r2);}
    }

    // 【扩展部分】bombed 系列产品的工程类
    class BombedMazeFactory : MazeFactory
    {

        // 可选：使用singleton
        /*
        private static BombedMazeFactory _instance = new BombedMazeFactory();
        public static BombedMazeFactory Instance { get { return _instance; } private set; }
        private BombedMazeFactory() { }
        */

        public BombedMazeFactory() { }
        public override Room MakeRoom(int rNo) { return new BombedRoom(rNo); }
        public override Door MakeDoor(Room r1, Room r2) { return new BombedDoor(r1, r2); }
    }

    partial class MazeGame
    {
        // 组装逻辑，使用MazeFactory的抽象类接口
        public Maze CreateMazeWithFactory(MazeFactory factory)
        {
            Maze maze = factory.MakeMaze();
            Room r1 = factory.MakeRoom(1);
            Room r2 = factory.MakeRoom(2);
            Door door = factory.MakeDoor(r1, r2);

            maze.AddRoom(r1);
            maze.AddRoom(r2);

            r1.SetSide(Direction.North, factory.MakeWall());
            r1.SetSide(Direction.South, factory.MakeWall());
            r1.SetSide(Direction.East, door);
            r1.SetSide(Direction.West, factory.MakeWall());

            r2.SetSide(Direction.North, factory.MakeWall());
            r2.SetSide(Direction.South, factory.MakeWall());
            r2.SetSide(Direction.East, factory.MakeWall());
            r2.SetSide(Direction.West, door);

            return maze;
        }
    }

    class AbstractFactoryTester
    {
        static void Main(string[] args)
        {
            MazeGame game = new MazeGame();
            MazeFactory factory = null;
            Maze maze = null;

            // default maze factory
            factory = new MazeFactory();
            maze = game.CreateMazeWithFactory(factory);
            System.Console.WriteLine(maze.DumpInfo());

            // enchanted maze factory
            factory = new EnchantedMazeFactory();
            maze = game.CreateMazeWithFactory(factory);
            System.Console.WriteLine(maze.DumpInfo());

            // bombed maze factory
            factory = new BombedMazeFactory();
            maze = game.CreateMazeWithFactory(factory);
            System.Console.WriteLine(maze.DumpInfo());
        }
    }
}
