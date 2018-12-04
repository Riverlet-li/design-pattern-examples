using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace design_pattern_examples
{
    class Equipment
    {
        private static List<Equipment>.Enumerator _iterator = (new List<Equipment>()).GetEnumerator();
        private string _name;
        protected Equipment _parent;
        protected Equipment _keeper;

        protected Equipment(string name)
        {
            _name = name;
            _parent = null;
            _keeper = null;
        }

        public string GetName() { return _name; }
        public virtual int GetPower() { return 0; }
        public virtual int GetPrice() { return 0; }

        // 改用指向父类的引用，不适用子成员列表
        public Equipment GetKeeper() { return _keeper; }
        public virtual void SetKeeper(Equipment keeper)
        {
            if (_keeper == keeper) {
                return;
            }

            if (keeper == null) {
                if (_keeper != null) {
                    _keeper.Remove(this);
                }
            } else {
                if (_keeper != null) {
                    _keeper.Remove(this);
                    keeper.Add(this);
                } else {
                    keeper.Add(this);
                }
            }

            _keeper = keeper; ;
        }

        // 通过keeper建立树层次结构，而不是add/remove
        public Equipment GetParent() { return _parent; }
        public void SetParent(Equipment parent) { _parent = parent; }  // 定义为public有风险，parent应该由Composite树内部维护
        public virtual void Destroy()
        {
            Equipment parent = this.GetParent();
            if (parent != null) {
                parent.Remove(this);
            }
            SetParent(null);

            // 如果使用Keeper，Destroy中parent.Remove(this)就可以不写，因为已经在Keeper中实现
            SetKeeper(null);
        }

        // 成员管理接口，具体在Composite实现
        public virtual void Add(Equipment equip) { }
        public virtual void Remove(Equipment equip) { }
        public virtual List<Equipment>.Enumerator CreateIterator() { return _iterator; }
    }

    class CompositeEquipment : Equipment
    {
        List<Equipment> _equipments;
        public CompositeEquipment(string name)
            : base(name)
        {
            _equipments = new List<Equipment>();
        }
        public override int GetPower()
        {
            List<Equipment>.Enumerator iterator = CreateIterator();
            int total = 0;
            while (iterator.MoveNext()) {
                Equipment child = iterator.Current;
                total += child.GetPower();
            }
            return total;
        }
        public override int GetPrice()
        {
            List<Equipment>.Enumerator iterator = CreateIterator();
            int total = 0;
            while (iterator.MoveNext()) {
                Equipment child = iterator.Current;
                total += child.GetPrice();
            }
            return total;
        }

        public override void Add(Equipment equip)
        {
            _equipments.Add(equip);
            equip.SetParent(this);
        }
        public override void Remove(Equipment equip)
        {
            _equipments.Remove(equip);
            equip.SetParent(null);
        }
        public override void Destroy()
        {
            List<Equipment> list = new List<Equipment>(_equipments);
            foreach (var item in list) {
                item.Destroy(); 
            }
            list.Clear();
            _equipments.Clear();

            Equipment parent = this.GetParent();
            if (parent != null) {
                parent.Remove(this);
            }
            SetParent(null);
            SetKeeper(null);
        }
        public override List<Equipment>.Enumerator CreateIterator()
        {
            return _equipments.GetEnumerator();
        }
    }

    class FloppyDick : Equipment
    {
        public FloppyDick(string name) : base(name) { }
        public override int GetPower() { return 10; }
        public override int GetPrice() { return 100; }
    }

    class KeyBoard : Equipment
    {
        public KeyBoard(string name) : base(name) { }
        public override int GetPower() { return 90; }
        public override int GetPrice() { return 500; }
    }

    class Bus : CompositeEquipment
    {
        public Bus(string name) : base(name) { }
    }

    class CompositeTest
    {
        static void Main(string[] args)
        {
            //////////////////////////////////////////////////////////
            // 使用Add/Remove方法建立Composite树
            Bus bus = new Bus("bus");
            Bus cBus = new Bus("cBus");
            bus.Add(cBus);

            FloppyDick dick = new FloppyDick("FloppyDick");
            KeyBoard key = new KeyBoard("KeyBoard");
            cBus.Add(dick);
            cBus.Add(key);

            // 访问Parent
            Equipment parent = key.GetParent();
            System.Console.WriteLine(parent.GetPrice());

            // 遍历Composite Tree
            System.Console.WriteLine(bus.GetPrice());

            //////////////////////////////////////////////////////////
            // 使用SetKeeper方法建立Composite树
            Bus bus2 = new Bus("bus");
            Bus cBus2 = new Bus("cBus");
            cBus2.SetKeeper(bus2);

            FloppyDick dick2 = new FloppyDick("FloppyDick");
            KeyBoard key2 = new KeyBoard("KeyBoard");
            dick2.SetKeeper(cBus2);
            key2.SetKeeper(cBus2);

            // 访问Parent
            Equipment keeper = key2.GetKeeper();
            System.Console.WriteLine(keeper.GetPrice());

            // 遍历Composite Tree
            System.Console.WriteLine(bus2.GetPrice());

            keeper.Destroy();
            System.Console.ReadLine();
        }
    }
}
