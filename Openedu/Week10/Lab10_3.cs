using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;
using System.Diagnostics;
using System.Collections;

namespace Openedu.Week10
{
    interface IComposedStringVisitor
    {
        void VisitStringFragment(StringFragment fragment);
        void VisitGroup(GroupString group);
    }

    abstract class ComposedString
    {
        public int Length { get; private set; }

        protected ComposedString(int length)
        {
            this.Length = length;
        }

        public abstract void Apply(IComposedStringVisitor visitor);

        public static ComposedString operator +(ComposedString a, ComposedString b)
        {
            return new GroupString(a, b);
        }

        public static implicit operator ComposedString(string str)
        {
            return new StringFragment(str);
        }

        public static implicit operator string(ComposedString str)
        {
            return str.GetContent();
        }

        public string GetContent()
        {
            return new StringCollector(this).Collect();
        }

        public override string ToString()
        {
            return $"C[{this.GetContent()}]";
        }

        public abstract bool StartsWith(char c);
        public abstract bool EndsWith(char c);
    }

    class StringFragment : ComposedString
    {
        public struct Enumerator
        {
            private int _index;
            private int _limit;
            private string _text;

            public Enumerator(int index, int limit, string text)
            {
                _index = index - 1;
                _limit = limit - 1;
                _text = text;
            }

            public char Current { get { return _text[_index]; } }

            public bool MoveNext()
            {
                if (_index < _limit)
                {
                    _index++;
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public string Text { get; private set; }
        public int Start { get; private set; }

        public StringFragment(string text, int start, int length)
            : base(length)
        {
            if (start < 0 || start > text.Length)
                throw new ArgumentOutOfRangeException();
            if (length < 0 || length > text.Length - start)
                throw new ArgumentOutOfRangeException();

            this.Text = text;
            this.Start = start;
        }

        public StringFragment(string str)
            : this(str, 0, str.Length) { }

        public static implicit operator StringFragment(string str)
        {
            return new StringFragment(str);
        }

        public static implicit operator string(StringFragment str)
        {
            return str.GetContent();
        }

        public Enumerator GetEnumerator()
        {
            return new Enumerator(this.Start, this.Start + this.Length, this.Text);
        }

        public static bool operator ==(StringFragment a, StringFragment b)
        {
            if (a.Length != b.Length)
                return false;

            //if (!StringHashes.CompareRanges(a.Hashses, a.Start, a.Length, b.Hashses, b.Start, b.Length))
            //    return false;

            var x = a.GetEnumerator();
            var y = b.GetEnumerator();

            while (x.MoveNext() && y.MoveNext())
                if (x.Current != y.Current)
                    return false;

            return true;
        }

        public static bool operator !=(StringFragment a, StringFragment b)
        {
            return !(a == b);
        }

        public override void Apply(IComposedStringVisitor visitor)
        {
            visitor.VisitStringFragment(this);
        }

        public StringFragment Substring(int start, int length)
        {
            if (start < 0 || start > this.Length)
                throw new ArgumentOutOfRangeException();
            if (length < 0 || length > this.Length - start)
                throw new ArgumentOutOfRangeException();

            return new StringFragment(this.Text, this.Start + start, length);
        }

        public StringFragment Substring(int start)
        {
            return this.Substring(start, this.Length - start);
        }

        public override bool StartsWith(char c)
        {
            return this.Length > 0 && this.Text[this.Start] == c;
        }

        public override bool EndsWith(char c)
        {
            return this.Length > 0 && this.Text[this.Start + this.Length - 1] == c;
        }
    }

    class GroupString : ComposedString
    {
        public ComposedString Left { get; private set; }
        public ComposedString Right { get; private set; }

        public GroupString(ComposedString left, ComposedString right)
            : base(left.Length + right.Length)
        {
            this.Left = left;
            this.Right = right;
        }

        public override void Apply(IComposedStringVisitor visitor)
        {
            visitor.VisitGroup(this);
        }

        public override bool StartsWith(char c)
        {
            return this.Left.Length > 0 ? this.Left.StartsWith(c) : this.Right.StartsWith(c);
        }

        public override bool EndsWith(char c)
        {
            return this.Right.Length > 0 ? this.Right.EndsWith(c) : this.Left.EndsWith(c);
        }
    }

    class StringCollector : IComposedStringVisitor
    {
        readonly StringBuilder _sb;
        readonly ComposedString _str;

        public StringCollector(ComposedString str)
        {
            _str = str;
            _sb = new StringBuilder(str.Length);
        }

        public string Collect()
        {
            _sb.Clear();
            _str.Apply(this);
            return _sb.ToString();
        }

        void IComposedStringVisitor.VisitGroup(GroupString group)
        {
            group.Left.Apply(this);
            group.Right.Apply(this);
        }

        void IComposedStringVisitor.VisitStringFragment(StringFragment fragment)
        {
            _sb.Append(fragment.Text.Substring(fragment.Start, fragment.Length));
        }
    }

    class Lab10_3
    {
        public static void Main(string[] args)
        {
            var app = new Lab10_3();
            app.DoWork(args);
        }

        private void DoWork(string[] args)
        {
            using (StreamWriter sw = new StreamWriter("output.txt"))
            {
                string[] stdin = File.ReadAllLines("input.txt");
                var str = stdin[0];

                //var rnd = new Random();
                //var chars = Enumerable.Range('a', 'z' - 'a').Concat(Enumerable.Range('A', 'Z' - 'A')).Select(n => (char)n).ToArray();
                //str = new string(Enumerable.Range(0, 815).Select(c => chars[rnd.Next(chars.Length)]).ToArray());

                var repl = this.MakeTreeSolution(str).GetContent();

                sw.WriteLine(repl);
            }
        }

        class Reduction
        {
            public int Start { get; private set; }
            public int End { get; private set; }

            public int EntryStart { get; private set; }
            public int EntryEnd { get; private set; }
            public int EntryLength { get; private set; }
            public int EntriesCount { get; private set; }

            public Reduction(int start, int end)
            {
                this.Start = start;
                this.End = end;

                this.EntriesCount = 1;
                this.EntryStart = start;
                this.EntryEnd = end;
                this.EntryLength = end - start + 1;
            }

            public bool TryAddEntry(int start, int pos)
            {
                if (this.End + 1 == start)
                {
                    this.EntriesCount++;
                    this.End = pos;
                    return true;
                }
                else
                {
                    return false;
                }
            }

            public override string ToString()
            {
                return $"[{this.Start} .. {this.End}] == [{this.EntryStart} .. {this.EntryEnd}]*{this.EntriesCount}";
            }
        }

        class Node
        {
            public char Character { get; private set; }
            public Dictionary<char, Node> Childs { get; private set; }
            public LinkedList<Reduction> Reductions { get; private set; }

            public Node(char character)
            {
                this.Character = character;
                this.Childs = new Dictionary<char, Node>();
                this.Reductions = new LinkedList<Reduction>();
            }

            public Node Add(int start, int pos, char ch, LinkedList<Reduction> compactingReductions)
            {
                if (!this.Childs.TryGetValue(ch, out var node))
                    this.Childs.Add(ch, node = new Node(ch));

                var last = node.Reductions.Last;
                while (last != null && last.Value.TryAddEntry(start, pos))
                {
                    if (last.Value.EntriesCount == 2)
                        compactingReductions.AddLast(last.Value);

                    last = last.Previous;
                }

                node.Reductions.AddLast(new Reduction(start, pos));

                return node;
            }
        }

        private ComposedString MakeTreeSolution(string str)
        {
            var root = new Node('\0');
            var actives = new Node[str.Length];

            var compactingReductions1 = new LinkedList<Reduction>();

            for (int i = 0; i < str.Length; i++)
            {
                var c = str[i];

                for (int j = 0; j < i; j++)
                {
                    actives[j] = actives[j].Add(j, i, c, compactingReductions1);
                }

                actives[i] = root.Add(i, i, c, compactingReductions1);
            }

            var compactingReductions = compactingReductions1.OrderBy(r => r.Start).ToList();

            //compactingReductions.ForEach(r => Console.WriteLine(r));
            //Console.WriteLine("---------------");
            var text = new StringFragment(str);
            if (compactingReductions.Count == 0)
                return text;
            // var solutions = new List<ComposedString>();

            var qq = new Queue<Fragment>();
            var start = new Fragment(new Reduction(-1, 0), 0, new StringFragment(""));
            qq.Enqueue(start);
            ComposedString solution = text;
            while (qq.Count > 0)
            {
                var curr = qq.Dequeue();

                var cnt = 0;
                foreach (var next in compactingReductions.SkipWhile(r => r.Start < curr.Reduction.End))
                {
                    var content = curr.Content;

                    if (content.Length > 0 && !content.EndsWith('+'))
                        content += "+";

                    if (curr.Reduction.Start == -1)
                        content += text.Substring(0, next.Start);
                    else if (curr.Reduction.End < next.Start)
                        content += text.Substring(curr.Reduction.End + 1, next.Start - curr.Reduction.End - 1);

                    if (content.Length > 0 && !content.EndsWith('+'))
                        content += "+";

                    content += text.Substring(next.EntryStart, next.EntryLength) + "*" + next.EntriesCount;

                    qq.Enqueue(new Fragment(next, curr.Length + next.End - next.Start + 1, content));
                    cnt++;
                }

                if (cnt == 0)
                {
                    var currResult = curr.Content + (curr.Content.Length > 0 && curr.Reduction.End + 1 < text.Length ? "+" : "") + text.Substring(curr.Reduction.End + 1);
                    if (currResult.Length < solution.Length)
                        solution = currResult;

                    // solutions.Add(currResult);
                }
            }

            return solution;
        }

        class Fragment 
        {
            public Reduction Reduction { get; private set; }
            public int Length { get; private set; }
            public ComposedString Content { get; private set; }

            public Fragment(Reduction reduction, int len, ComposedString content)
            {
                this.Reduction = reduction;
                this.Length = len;
                this.Content = content;
            }
        }
    }
}
