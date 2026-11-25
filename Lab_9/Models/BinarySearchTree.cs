using System.Collections;
using System.Collections.Generic;
namespace Lab_9.Models{
    public class BinarySearchTree : IEnumerable<int>{
        public Node? Root { get; private set; }
        public int Count { get; private set; }
        public int Height => GetHeight(Root);
        public int? Min => Root == null ? null : GetMin(Root).Value;
        public int? Max => Root == null ? null : GetMax(Root).Value;
        public bool Add(int value){
            if (value < 1 || value > 999999 || Contains(value))
                return false;
            Root = Insert(Root, value);
            Count++;
            return true;
        }
        private Node Insert(Node? node, int value){
            if (node == null)
                return new Node(value);
            if (value < node.Value)
                node.Left = Insert(node.Left, value);
            else
                node.Right = Insert(node.Right, value);
            return node;
        }
        public bool Contains(int value) {
            return Search(Root, value);
        }
        private bool Search(Node? node, int value){
            if (node == null)
                return false;
            if (node.Value == value)
                return true;
            if (value < node.Value)
                return Search(node.Left, value);
            return Search(node.Right, value);
        }
        public bool Find(int value, out string path){
            path = string.Empty;
            Node? current = Root;
            while (current != null){
                if (path.Length > 0)
                    path += " → ";
                path += current.Value;
                if (current.Value == value)
                    return true;
                current = value < current.Value
                    ? current.Left
                    : current.Right;
            }
            return false;
        }
        public bool Delete(int value){
            if (!Contains(value))
                return false;
            Root = Remove(Root, value);
            Count--;
            return true;
        }
        private Node? Remove(Node? node, int value){
            if (node == null)
                return null;
            if (value < node.Value){
                node.Left = Remove(node.Left, value);
            }
            else if (value > node.Value){
                node.Right = Remove(node.Right, value);
            }
            else{
                if (node.Left == null)
                    return node.Right;
                if (node.Right == null)
                    return node.Left;
                Node successor = GetMin(node.Right);
                node.Value = successor.Value;
                node.Right = Remove(node.Right, successor.Value);
            }
            return node;
        }
        private Node GetMin(Node node){
            if (node.Left == null)
                return node;
            return GetMin(node.Left);
        }
        private Node GetMax(Node node){
            if (node.Right == null)
                return node;
            return GetMax(node.Right);
        }
        private int GetHeight(Node? node){
            if (node == null)
                return 0;
            return 1 + System.Math.Max( GetHeight(node.Left), GetHeight(node.Right));
        }
        public List<int> Preorder(){
            List<int> result = new();
            Preorder(Root, result);
            return result;
        }
        private void Preorder(Node? node, List<int> result){
            if (node == null)
                return;
            result.Add(node.Value);
            Preorder(node.Left, result);
            Preorder(node.Right, result);
        }
        public List<int> Inorder(){
            List<int> result = new();
            Inorder(Root, result);
            return result;
        }
        private void Inorder(Node? node, List<int> result){
            if (node == null)
                return;
            Inorder(node.Left, result);
            result.Add(node.Value);
            Inorder(node.Right, result);
        }
        public List<int> Postorder(){
            List<int> result = new();
            Postorder(Root, result);
            return result;
        }
        private void Postorder(Node? node, List<int> result){
            if (node == null)
                return;
            Postorder(node.Left, result);
            Postorder(node.Right, result);
            result.Add(node.Value);
        }
        public void Clear(){
            Root = null;
            Count = 0;
        }
        public IEnumerator<int> GetEnumerator() {
            return Enumerate(Root).GetEnumerator();
        }
        private IEnumerable<int> Enumerate(Node? node){
            if (node == null)
                yield break;
            foreach (int value in Enumerate(node.Left))
                yield return value;
            yield return node.Value;
            foreach (int value in Enumerate(node.Right))
                yield return value;
        }
        IEnumerator IEnumerable.GetEnumerator(){
            return GetEnumerator();
        }
    }
}