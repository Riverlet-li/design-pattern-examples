using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeGame
{
    class Singleton
    {
        private static Dictionary<Type, Singleton> _registery = new Dictionary<Type, Singleton>();

        protected Singleton() { }
        // 通过泛型查找
        public static T Instance<T>() where T : Singleton, new()
        {
            Type type = typeof(T);
            T inst = null;
            if (_registery.ContainsKey(type)) {
                inst = (T)_registery[type];
            } else {
                // 通过Activator通过Type创建对象实例
                // inst = Activator.CreateInstance<T>();
                // Singleton.Register<T>(inst);
                inst = new T();
            }
            return inst;
        }
        // 建立singeton索引表
        public static void Register<T>(T inst) where T : Singleton
        {
            if (!_registery.ContainsKey(typeof(T))) {
                _registery.Add(typeof(T), inst);
            } else {
                throw new Exception("Register duplicate instance.");
            }
        }
    }

    class MazeFactorySingleton : Singleton
    {
        public MazeFactorySingleton()
        {
            Singleton.Register<MazeFactorySingleton>(this);
        }
        public virtual Maze MakeMaze() { return new Maze(); }
        public virtual Wall MakeWall() { return new Wall(); }
        public virtual Room MakeRoom(int rNo) { return new Room(rNo); }
        public virtual Door MakeDoor(Room r1, Room r2) { return new Door(r1, r2); }
    }

    class BombedMazeFactorySingleton : MazeFactorySingleton
    {
        public BombedMazeFactorySingleton()
        {
            Singleton.Register<BombedMazeFactorySingleton>(this);
        }
        public override Room MakeRoom(int rNo) { return new BombedRoom(rNo); }
        public override Door MakeDoor(Room r1, Room r2) { return new BombedDoor(r1, r2); }
    }

    class EnchantedMazeFactorySingleton : MazeFactorySingleton
    {
        public EnchantedMazeFactorySingleton()
        {
            Singleton.Register<EnchantedMazeFactorySingleton>(this);
        }
        public override Room MakeRoom(int rNo) { return new EnchantedRoom(rNo); }
        public override Door MakeDoor(Room r1, Room r2) { return new EnchantedDoor(r1, r2); }
    }

    class SingletonTest
    {
        static void Main(string[] args)
        {
            BombedMazeFactorySingleton singleton = new BombedMazeFactorySingleton();
            Singleton.Instance<BombedMazeFactorySingleton>().MakeRoom(1);

            Singleton.Instance<EnchantedMazeFactorySingleton>().MakeRoom(1);
            EnchantedMazeFactorySingleton singleton2 = new EnchantedMazeFactorySingleton();
            Singleton.Instance<EnchantedMazeFactorySingleton>().MakeRoom(1);
        }
    }
}
