using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace design_pattern_examples.adapter
{
    class Point2D
    {
        public Point2D(float _x, float _y)
        {
            this.x = x;
            this.y = _y;
        }
        public float x { get; set; }
        public float y { get; set; }
        public static Point2D Identity = new Point2D(0, 0);
    }

    class Manipulator { }
    class TextManipulator : Manipulator { }

    // Adapter-Target
    interface IShape
    {
        void BoundingBox();
        Manipulator CreateManipulator();
    }

    // Adapter-Adaptee
    class TextView
    {
        public TextView() { }
        public Point2D GetOrigin() { return Point2D.Identity; }
        public Point2D GetExtend() { return Point2D.Identity; }
        public virtual bool IsEmpty() { return false; }
    }

    // Adapter-Class Adaptor
    class ClassTextShape : TextView, IShape
    {
        private Point2D _bottomLeft, _topRight;
        public ClassTextShape()
        {
            _bottomLeft = new Point2D(0, 0);
            _topRight = new Point2D(0, 0);
        }
        public void BoundingBox()
        {
            Point2D origin = GetOrigin();
            Point2D extend = GetExtend();
            _bottomLeft = new Point2D(origin.x, origin.y);
            _topRight = new Point2D(origin.x + extend.x, origin.y + extend.y);
        }
        public Manipulator CreateManipulator() { return new TextManipulator(); }
        public override bool IsEmpty() { return base.IsEmpty(); }
    }

    // Adapter-Object Adapter
    class ObjectTextShape : IShape
    {
        private Point2D _bottomLeft, _topRight;
        private TextView _view = null;
        public ObjectTextShape(TextView view)
        {
            this._view = view;
        }
        public void BoundingBox()
        {
            Point2D origin = _view.GetOrigin();
            Point2D extend = _view.GetExtend();
            _bottomLeft = new Point2D(origin.x, origin.y);
            _topRight = new Point2D(origin.x + extend.x, origin.y + extend.y);
        }
        public Manipulator CreateManipulator()
        {
            return new TextManipulator();
        }
    }

    class AdaptorTester
    {
        static void Main(string[] args)
        {
            // 类继承适配器
            ClassTextShape ctShaper = new ClassTextShape();
            ctShaper.BoundingBox();
            Manipulator manipulator = ctShaper.CreateManipulator();

            // 对象适配器
            TextView view = new TextView();
            ObjectTextShape otShaper = new ObjectTextShape(view);
            otShaper.BoundingBox();
            Manipulator manipulator2 = otShaper.CreateManipulator();
        }
    }
}
