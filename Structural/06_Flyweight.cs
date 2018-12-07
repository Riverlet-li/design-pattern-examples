using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace design_pattern_examples.flyweight
{
    class Window { }

    class Font { }

    class Glyph
    {
        private List<Glyph> _list = null;

        protected Glyph()
        {
            _list = new List<Glyph>();
        }

        public virtual void Draw(Window win, GlyphContext context) { }

        // 统一的接口，这些是倒置方法，其实本质上是修改context的数据
        public virtual void SetFont(Font font, GlyphContext context) { }
        public virtual Font GetFont(GlyphContext context) { return null; }

        // Composite方法，用于遍历，本质上是修改Context的index数据
        public virtual void First(GlyphContext context) { }
        public virtual void Next(GlyphContext context) { }
        public virtual bool IsDone(GlyphContext context) { return false; }
        public virtual Glyph Current(GlyphContext context) { return null; }

        // Composite方法
        public virtual void Insert(Glyph glyph, GlyphContext context) {
            _list.Add(glyph);
            context.Next();
        }
        public virtual void Remove(Glyph glyph, GlyphContext context) { }
    }

    class Character : Glyph
    {
        private char _char;
        public Character(char c)
        {
            _char = c;
        }
        public override void Draw(Window win, GlyphContext context) { }
    }

    class Row : Glyph
    {

        public Row() { }
        public override void Draw(Window win, GlyphContext context) { }
    }

    class Column : Glyph { }

    class GlyphContext
    {
        class GlyphFontInfo
        {
            public int startIdx = 0;
            public int length = 0;
            public string fontName = "";
        }

        private int _index = 0;
        private Dictionary<string, List<GlyphFontInfo>> _dict;      // 为了简化算法，使用List代替
        private string _defaultFontName = "Rome";
        private string _curFontName = "";

        public GlyphContext()
        {
            _index = 0;
            _dict = new Dictionary<string, List<GlyphFontInfo>>();
        }
        public virtual void Next(int step = 1)
        {
            _index += step;
        }
        public virtual void Insert(int quantity = 1) { }

        public virtual string GetFont() { return ""; }

        public virtual void SetFont(string name) { _curFontName = name; }
    }

    class GlyphFactory
    {
        private Dictionary<char, Character> _characters = null;

        public GlyphFactory()
        {
            _characters = new Dictionary<char, Character>();
            for (int idx = 0; idx < 127; idx++) {
                char ch = (char)idx;
                _characters.Add(ch, new Character(ch));
            }
        }
        public virtual Character CreateCharacter(char c)
        {
            Character charcter = null;
            if (_characters.ContainsKey(c)) {
                charcter = _characters[c];
            } else {
                charcter = new Character(c);
                _characters.Add(c, charcter);
            }

            return charcter;
        }
        public virtual Row CreateRow()
        {
            return new Row();
        }
        public virtual Column CreateColumn() { return new Column(); }
    }

    class FlyweightTest
    {
        static void Main(string[] args)
        {
            GlyphFactory factory = new GlyphFactory();
            GlyphContext context = new GlyphContext();

            // Context保存字体外部信息，内部维护一张表格
            context.SetFont("songti");

            Row row = factory.CreateRow();
            string content = "Yesterday once more";
            foreach (var ch in content.ToArray<char>()) {
                Character character = factory.CreateCharacter(ch);
                row.Insert(character, context);
            }

            // 设置新的字体，后续文字改用此字体
            context.SetFont("songti");
            string content2 = " God is a girl";
            foreach (var ch2 in content2.ToArray<char>()) {
                Character character = factory.CreateCharacter(ch2);
                row.Insert(character, context);
            }

            // 也可在Row的基础上整体改变字体
            // 在当前位置改变
            context.Insert(6);
            context.SetFont("songti");
        }
    }
}
