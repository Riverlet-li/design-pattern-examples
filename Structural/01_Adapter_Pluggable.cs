using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace design_pattern_examples.adapter.pluggable
{
    class FileTreeNode { }
    class FileSystemTree
    {
        private List<FileTreeNode> _list = new List<FileTreeNode>();
        public List<FileTreeNode> GetFileList() { return _list; }
    }

    class TreeNode { }

    // 1.基于继承的抽象操作-Target
    abstract class TreeDisplayWithAbstract
    {
        protected abstract List<TreeNode> GetChildren();
        protected abstract TreeNode CreateNode();

        public void BuildTree()
        {
            List<TreeNode> children = GetChildren();
            foreach (var node in children) {
                CreateNode();
            }
        }
    }

    // 1.基于继承的抽象操作-Adapter
    class DirectoryTreeDisplay : TreeDisplayWithAbstract
    {
        private FileSystemTree _tree;
        public DirectoryTreeDisplay(FileSystemTree tree)
        {
            this._tree = tree;
        }
        protected override List<TreeNode> GetChildren()
        {
            List<TreeNode> list = new List<TreeNode>();
            List<FileTreeNode> fileList = _tree.GetFileList();
            foreach (var file in fileList) {
                TreeNode node = new TreeNode();
                // set TreeNode data according to FileTreeNode
                list.Add(node);
            }
            return list;
        }
        protected override TreeNode CreateNode() { return null; }
    }

    // 2.基于代理对象-Client
    delegate List<TreeNode> GetChildren();
    delegate TreeNode CreateNode();
    class TreeDisplayWithDelegate
    {
        public GetChildren _getChildren;
        public CreateNode _createNode;

        public void BuildTree()
        {
            List<TreeNode> children = _getChildren();
            foreach (var node in children) {
                _createNode();
            }
        }
    }

    // 2.基于代理对象-Adaptee
    class DirectoryTreeDisplayDelegate
    {
        private FileSystemTree _tree;
        public DirectoryTreeDisplayDelegate(FileSystemTree tree)
        {
            this._tree = tree;
        }
        public List<TreeNode> GetChildren()
        {
            List<TreeNode> list = new List<TreeNode>();
            List<FileTreeNode> fileList = _tree.GetFileList();
            foreach (var file in fileList) {
                TreeNode node = new TreeNode();
                // set TreeNode data according to FileTreeNode
                list.Add(node);
            }
            return list;
        }
        public TreeNode CreateNode() { return null; }
    }

    class AdapterPluggableTest
    {
        static void Main(string[] args)
        {
            // 1.基于继承的抽象操作
            FileSystemTree fTree = new FileSystemTree();

            DirectoryTreeDisplay display = new DirectoryTreeDisplay(fTree);
            display.BuildTree();

            // 2.基于代理对象
            TreeDisplayWithDelegate display2 = new TreeDisplayWithDelegate();
            DirectoryTreeDisplayDelegate dirDel = new DirectoryTreeDisplayDelegate(fTree);
            display2._getChildren = dirDel.GetChildren;
            display2._createNode = dirDel.CreateNode;
        }
    }
}
