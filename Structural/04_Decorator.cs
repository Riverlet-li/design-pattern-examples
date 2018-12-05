using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace design_pattern_examples.decorator
{
    abstract class Stream
    {
        public void PutInt(int val) { HandleBuffer(); }
        public void PutString(string val) { HandleBuffer(); }
        // 这个方法应该定义为protected，但c#下不支持friend
        public abstract void HandleBuffer();
    }

    class MemoryStream : Stream
    {
        public MemoryStream() { }
        public override void HandleBuffer()
        {
            System.Console.WriteLine("MemoryStream Handle Buffer");
        }
    }

    class FileStream : Stream
    {
        private string _filepath;
        public FileStream(string filepath) { _filepath = filepath; }
        public override void HandleBuffer()
        {
            System.Console.WriteLine("FileStream Handle Buffer");
        }
    }

    class StreamDecorator : Stream
    {
        private Stream _stream;
        public StreamDecorator(Stream stream)
        {
            _stream = stream;
        }
        public override void HandleBuffer()
        {
            //System.Console.WriteLine("StreamDecorator Handle Buffer");
            _stream.HandleBuffer();
        }
    }

    class ASCIIStreamDecorator : StreamDecorator
    {
        public ASCIIStreamDecorator(Stream stream) : base(stream) { }
        public override void HandleBuffer()
        {
            //System.Console.WriteLine("ASCIIStreamDecorator Handle Buffer");
            base.HandleBuffer();
            AddedBehavior();
        }
        private void AddedBehavior()
        {
            System.Console.WriteLine("ASCIIStreamDecorator AddedBehavior");
        }
    }

    class CompressingStreamDecorator : StreamDecorator
    {
        public CompressingStreamDecorator(Stream stream) : base(stream) { }
        public override void HandleBuffer()
        {
            //System.Console.WriteLine("CompressingStreamDecorator Handle Buffer");
            base.HandleBuffer();
            AddedBehavior();
        }
        private void AddedBehavior()
        {
            System.Console.WriteLine("CompressingStreamDecorator AddedBehavior");
        }
    }

    class DecoratorTest
    {
        static void Main(string[] args)
        {
            FileStream fs = new FileStream("file.txt");
            Stream stream = new ASCIIStreamDecorator(new CompressingStreamDecorator(fs));
            stream.PutInt(1);

            System.Console.ReadLine();
        }
    }
}
