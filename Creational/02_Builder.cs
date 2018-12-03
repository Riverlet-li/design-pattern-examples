/*
 * Builder-生成器
 * 意图：将一个复杂对象的构建与它的表示分离
 * 扩展：多种组合对象
 * 关联：
 *      1.供Composite创建负责节点
 * **/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeGame
{
    abstract class MazeBuilder
    {
        protected MazeBuilder() { }
        public virtual void BuildMaze() { }
        public virtual void BuildRoom(int rNo) { }
        public virtual void BuildDoor(int roomFrom, int roomTo) { }
        public virtual Maze GetMaze() { return null; }
    }

    //【扩展】复杂对象内部实现
    class DefaultMazeBuilder : MazeBuilder
    {
        private Maze _maze = null;
        public DefaultMazeBuilder()
        {
            _maze = null;
        }
        public override void BuildMaze()
        {
            _maze = new Maze();
        }
        public override void BuildRoom(int rNo)
        {
            if (_maze.RoomNo(rNo) == null) {
                Room room = new Room(rNo);
                _maze.AddRoom(room);

                room.SetSide(Direction.North, new Wall());
                room.SetSide(Direction.South, new Wall());
                room.SetSide(Direction.East, new Wall());
                room.SetSide(Direction.West, new Wall());
            }
        }
        public override void BuildDoor(int roomFrom, int roomTo)
        {
            Room rFrom = _maze.RoomNo(roomFrom);
            Room rTo = _maze.RoomNo(roomTo);
            if (rFrom != null && rTo != null) {
                Door door = new Door(rFrom, rTo);
                rFrom.SetSide(CommonWall(rFrom, rTo), door);
                rFrom.SetSide(CommonWall(rTo, rFrom), door);
            }
        }
        public override Maze GetMaze() { return _maze; }
        private Direction CommonWall(Room r1, Room r2) { return Direction.North; }
    }

    // product本身并不要求必须有抽象接口
    class StatMaze
    {
        public int doorNums { get; set; }
        public int roomNums { get; set; }
    }

    //【扩展】特殊用途的构造器
    class CountingMazeBuilder : MazeBuilder
    {
        private StatMaze _info;

        public CountingMazeBuilder()
        {
            _info = null;
        }
        public override void BuildMaze() { _info = new StatMaze(); }
        public override void BuildRoom(int rNo)
        {
            _info.roomNums++;
        }
        public override void BuildDoor(int roomFrom, int roomTo)
        {
            _info.doorNums++;
        }
        public StatMaze GetStatMaze() { return _info; }
    }

    partial class MazeGame
    {
        // 复杂对象的构建算法，此处已经简洁了很多
        public Maze CreateMazeWithBuilder(MazeBuilder builder)
        {
            builder.BuildMaze();
            builder.BuildRoom(1);
            builder.BuildRoom(2);
            builder.BuildDoor(1, 2);
            return builder.GetMaze();
        }
    }

    class BuilderTest
    {
        static void Main(string[] args)
        {
            MazeGame game = new MazeGame();

            // 标准构造器用法
            MazeBuilder builder = new DefaultMazeBuilder();
            Maze maze = game.CreateMazeWithBuilder(builder);
            System.Console.WriteLine(maze.DumpInfo());

            // 特殊类型的Builder，用于获取内部信息
            CountingMazeBuilder cbuilder = new CountingMazeBuilder();
            game.CreateMazeWithBuilder(cbuilder);
            StatMaze statmaze = cbuilder.GetStatMaze();
            System.Console.WriteLine("Room:{0} Door:{1}", statmaze.roomNums, statmaze.doorNums);
        }
    }
}
