using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace design_pattern_examples.facade
{
    class Stream
    {
        public virtual string Read() { return null; }
        public virtual void Write(string val) { }
    }
    class IStream : Stream
    {
        public override string Read() { return null; }
        public override void Write(string val) { }
    }

    class IStringStream : IStream
    {
        private int idx = 0;
        private string[] list = { "AAAAA", "BBBBBBBB", "CCCCCCCC", "DDDDD" };
        public override string Read()
        {

            idx++;
            if (idx < list.Length) {
                System.Console.WriteLine("IStringStream read " + list[idx]);
                return list[idx];
            }
            return null;
        }
        public override void Write(string val) { }
    }

    class BytecodeStream : Stream {
        public override void Write(string val) {
            System.Console.WriteLine("BytecodeStream write " + val);
        }
    }

    class ByteCode { }
    class Token
    {
        public string identifier { get; private set; }
        public Token(string identifier)
        {
            this.identifier = identifier;
        }
    }


    class Scanner
    {
        private IStream _inputStream;
        public Scanner(IStream stream)
        {
            _inputStream = stream;
        }
        public virtual Token Scan()
        {
            Token token = null;
            string identifier = _inputStream.Read();
            if (identifier != null) {
                token = new Token(identifier);
            }
            return token;
        }
    }

    class Parser
    {
        public Parser() { }
        public virtual void Parse(Scanner scanner, ProgramNodeBuilder builder)
        {
            System.Console.WriteLine("Scanner.Scan()");
            System.Console.WriteLine("ProgramNodeBuilder.build");
            while (true) {
                Token token = scanner.Scan();
                if (token == null) { break; }

                // TODO:语法分析树构建算法，基于Builder设计模式
                builder.NewVariable(token.identifier);
            }
        }
    }

    class CodeGenerator
    {
        protected BytecodeStream _output;
        protected CodeGenerator(BytecodeStream stream) { _output = stream; }
        public virtual void Visit(IdentifierNode node) { }
        public virtual void Visit(StatementNode node) { }
        public virtual void Visit(ExpressionNode node) { }
    }

    class StackMachineCodeGenerator : CodeGenerator
    {
        public StackMachineCodeGenerator(BytecodeStream stream) : base(stream) { }
    }
    class RISCCodeGenerator : CodeGenerator
    {
        public RISCCodeGenerator(BytecodeStream stream) : base(stream) { }
        public override void Visit(IdentifierNode node)
        {
            System.Console.WriteLine("RISCCodeGenerator:" + node.name);
        }
        public override void Visit(StatementNode node) { }
        public override void Visit(ExpressionNode node) { }
    }

    /// /////////////////////////////////////////////////////////
    // Program Composite Tree
    class ProgramNode
    {
        public ProgramNode() { }
        public virtual void GetSourcePosition() { }

        public virtual void Add(ProgramNode node) { }
        public virtual void Remove(ProgramNode node) { }
        public virtual void Traverse(CodeGenerator generator) { }
    }
    class ProgramCompositeNode : ProgramNode
    {
        private List<ProgramNode> _children;
        public ProgramCompositeNode()
        {
            _children = new List<ProgramNode>();
        }
        public override void Add(ProgramNode node)
        {
            _children.Add(node);
        }
        public override void Remove(ProgramNode node)
        {
            _children.Remove(node);
        }
        public override void Traverse(CodeGenerator generator)
        {
            foreach (var node in _children) {
                node.Traverse(generator);
            }
        }
    }
    class IdentifierNode : ProgramNode
    {
        public string name { get; private set; }
        public IdentifierNode(string name) { this.name = name; }
        public override void Traverse(CodeGenerator generator)
        {
            generator.Visit(this);
        }
    }
    class StatementNode : ProgramCompositeNode
    {
        public override void Traverse(CodeGenerator generator)
        {
            generator.Visit(this);
        }
    }
    class ExpressionNode : ProgramCompositeNode
    {
        public override void Traverse(CodeGenerator generator)
        {
            generator.Visit(this);
        }
    }

    /// //////////////////////////////////////////////////////////
    // Program Node Builder
    class ProgramNodeBuilder
    {
        private ProgramNode _node;
        public ProgramNodeBuilder()
        {
            _node = new ProgramCompositeNode();
        }
        public virtual ProgramNode NewVariable(string variableName)
        {
            ProgramNode node = new IdentifierNode(variableName);
            _node.Add(node);
            return node;
        }
        public virtual ProgramNode NewAssignment(ProgramNode variable, ProgramNode expression) { return null; }
        public virtual ProgramNode NewReturnStatement(ProgramNode value) { return null; }
        public virtual ProgramNode NewCondition(ProgramNode condition, ProgramNode truePart, ProgramNode falsePart) { return null; }
        public ProgramNode GetRootNode() { return _node; }
    }

    // Facade
    class Compiler
    {
        public Compiler() { }
        public virtual void Compile(IStream input, BytecodeStream output)
        {
            Scanner scanner = new Scanner(input);
            ProgramNodeBuilder builder = new ProgramNodeBuilder();
            Parser parser = new Parser();
            RISCCodeGenerator generator = new RISCCodeGenerator(output);

            parser.Parse(scanner, builder);
            ProgramNode parseTree = builder.GetRootNode();
            parseTree.Traverse(generator);
        }
    }

    class FacadeTest
    {
        static void Main(string[] args)
        {
            IStringStream inputStream = new IStringStream();
            BytecodeStream outputStream = new BytecodeStream();
            Compiler compiler = new Compiler();
            compiler.Compile(inputStream, outputStream);

            System.Console.ReadLine();
        }
    }
}
