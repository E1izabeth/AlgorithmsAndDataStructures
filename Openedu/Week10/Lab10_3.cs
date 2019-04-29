using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;
using System.Diagnostics;
using System.Collections;

#if G
using ParsingExpression.XmlGraph;
#endif

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
#if G

        private void PrintTree(Node tree)
        {
            var xg = new XmlGraph();

            var q = new Queue<Node>();
            q.Enqueue(tree);
            while (q.Count > 0)
            {
                var n = q.Dequeue();
                var xn = xg.CreateNode(n.Id.ToString());
                xn.Text = "#" + n.Id + ": " + n.Character + Environment.NewLine +
                    string.Join(Environment.NewLine, n.Reductions.Select(
                        //r => r + ": " + string.Join(", ", r.Entries.Select(e => e + "{" + string.Join(", ", e.nexts.Select(nn => nn.Id)) + "}"))
                        r => r// + ": " + string.Join(", ", r.Entries)
                    ));

                foreach (var item in n.Childs.Values)
                    q.Enqueue(item);
            }

            q.Enqueue(tree);
            while (q.Count > 0)
            {
                var n = q.Dequeue();

                foreach (var child in n.Childs.Values)
                {
                    xg[n.Id.ToString()].ConnectTo(xg[child.Id.ToString()]).Text = child.Character.ToString();
                    q.Enqueue(child);
                }

                //foreach (var reduction in n.Reductions)
                //{
                //    foreach (var entry in reduction.Entries)
                //    {
                //        var i = 0;
                //        foreach (var next in entry.nexts)
                //        {
                //            var link = xg[n.Id.ToString()].ConnectTo(xg[next.Id.ToString()]);
                //            link.Text = reduction + ": " + entry + "(" + i + ")";
                //            link.Color = "Green";
                //            i++;
                //        }
                //    }
                //}
            }

            xg.ToDgml().SaveToFile(@"out.dgml");
        }
#endif
        private void DoWork(string[] args)
        {
            using (StreamWriter sw = new StreamWriter("output.txt"))
            {
                string[] stdin = File.ReadAllLines("input.txt");
                var str = stdin[0];
                if (str.Length == 1)
                {
                    sw.WriteLine(str);
                    return;
                }

                //var rnd = new Random();
                //var chars = Enumerable.Range('a', 'z' - 'a').Concat(Enumerable.Range('A', 'Z' - 'A')).Select(n => (char)n).ToArray();
                //str = new string(str.Select(c => rnd.NextDouble() < 0.011852356269974474 ? chars[rnd.Next(chars.Length)] : c).ToArray());


                // Console.WriteLine(tree);

                // var repl = this.R(str).GetContent();

                //var repl = SuffixScan(str).GetContent();
                var repl = this.MakeTree(str).GetContent();

                //var regex = new Regex(@"([0-9]+)");
                //var repl = regex.Replace(s, "*$1+");
                sw.WriteLine(repl);
            }
        }

        //        private ComposedString SuffixScan(string str)
        //        {
        //            var text = new StringFragment(str);

        //            var tree = this.MakeTree(str);
        //#if G
        //            PrintTree(tree);
        //#endif
        //            Func<PathNode<Reduction, Ctx1>, IEnumerable<Reduction>> childs1 = pn =>
        //            {
        //                var cc = new List<Reduction>();

        //                var qq = new Queue<Node>();
        //                qq.Enqueue(tree);
        //                while (qq.Count > 0)
        //                {
        //                    var cn = qq.Dequeue();

        //                    cc.AddRange(cn.Reductions.Where(r => r.Start == pn.context.End));

        //                    foreach (var item in cn.Childs.Values)
        //                        qq.Enqueue(item);
        //                }

        //                return cc;
        //            };

        //            var path1 = this.FindPath<Reduction, Ctx1>(
        //                new Reduction(0, 0, null),
        //                //pn => pn.node.nexts.SelectMany(n => n.Reductions).Where(r => r.Start == pn.node.end + 1)
        //                //                   .SelectMany(r => r.Entries),
        //                //pn => tree.Childs.SelectMany(c => c.Value.Reductions.SelectMany(r => r.Entries)).Where(e => e.start == pn.context.position),
        //                childs1,
        //                e => e.End + 1 == str.Length,
        //                (pn, n) => pn == null ? new Ctx1(new StringFragment(string.Empty), 0, n) : new Ctx1(
        //                    pn.context.Str + new StringFragment(
        //                        pn.context.Str.Length > 0 && (pn.context.Reduction.Entries.Count > 1 || n.Entries.Count > 1) ? "+" : ""
        //                    ) + text.Substring(n.FirstEntry.start, n.FirstEntry.length) + new StringFragment(
        //                        n.Entries.Count == 1 ? "" : ("*" + n.Entries.Count)
        //                    ),
        //                    n.End + 1,
        //                    n
        //                )
        //            );

        //            return path1.context.Str;
        //        }

        public static readonly bool output = false;

        class Ctx1 : IComparable<Ctx1>
        {
            public ComposedString Str { get; private set; }
            public int End { get; private set; }
            public Reduction Reduction { get; private set; }

            public Ctx1(ComposedString str, int end, Reduction reduction)
            {
                this.Str = str;
                this.End = end;
                this.Reduction = reduction;
            }

            int IComparable<Ctx1>.CompareTo(Ctx1 other)
            {
                int n;

                n = this.Str.Length.CompareTo(other.Str.Length);
                if (n != 0)
                    return n;

                n = this.End.CompareTo(other.End);
                if (n != 0)
                    return n * -1;

                return 0;
            }

            public override string ToString()
            {
                return this.Reduction + " " + this.Str.ToString();
            }
        }



        //class Ctx : IComparable<Ctx>
        //{
        //    public readonly int length;
        //    public readonly int position;

        //    public Ctx(int length, int position)
        //    {
        //        this.length = length;
        //        this.position = position;
        //    }

        //    public int CompareTo(Ctx other)
        //    {
        //        var n = this.position.CompareTo(other.position);
        //        if (n == 0)
        //        {
        //            return this.length.CompareTo(other.length);
        //        }
        //        else
        //        {
        //            return n > 0 ? -1 : 1;
        //        }
        //    }

        //    public override string ToString()
        //    {
        //        return this.length + "@" + this.position;
        //    }
        //}

        //ComposedString R(StringFragment text)
        //{
        //    ComposedString n = text;
        //    int length = text.Length;
        //    int r, j;

        //    //if (text.Length == 16 * 32 && text == string.Join("", Enumerable.Repeat(text.Substring(0, 16), 32)))
        //    for (int step = length / 2; step > 0; --step)
        //    // for (int step = 1, limit = length / 2; step < length; ++step)
        //    {
        //        for (int pos = 0; step + step + pos <= length; pos++)
        //        {
        //            var fragment = text.Substring(pos, step);
        //            for (j = pos + step, r = 1; j + step <= length && fragment == text.Substring(j, step); j += step)
        //                ++r;

        //            var c = fragment + "*" + r.ToString();
        //            if (c.Length < step * r)
        //            {
        //                // var left = this.R(text.Substring(0, pos));
        //                var left = text.Substring(0, pos);
        //                var right = this.R(text.Substring(j));
        //                c = (left.Length > 0 && !left.EndsWith('+') ? left + "+" : left) + c + (right.Length > 0 && !right.StartsWith('+') ? "+" + right : right);
        //                // c = left + c + right;
        //                n = c.Length < n.Length ? c : n;

        //                // break;
        //            }
        //        }
        //    }
        //    return n;
        //}

        private ComposedString R(StringFragment text, bool nested = false)
        {
            int length = text.Length;

            //ComposedString n = text;

            //for (int step = length / 2; step > 0; --step)
            // for (int step = 1, limit = length / 2; step < length; ++step)

            ComposedString rr = text;
            if (nested)
            {
                for (int step = 1, limit = length / 2; step < length; ++step)
                    rr = this.RR(text, step, rr);
            }
            else
            {
                rr = Enumerable.Range(1, length / 2).AsParallel().Select(step => this.RR(text, step, text)).Aggregate((ComposedString)text, (a, b) => a.Length < b.Length ? a : b);
            }

            return rr;
        }

        private ComposedString RR(StringFragment text, int step, ComposedString n)
        {
            int length = text.Length;
            int r, j;

            for (int pos = 0; step + step + pos <= length; pos++)
            {
                var fragment = text.Substring(pos, step);
                for (j = pos + step, r = 1; j + step <= length && fragment == text.Substring(j, step); j += step)
                    ++r;

                var c = fragment + "*" + r.ToString();
                if (c.Length < step * r)
                {
                    // var left = this.R(text.Substring(0, pos));
                    var left = text.Substring(0, pos);
                    var right = this.R(text.Substring(j), true);
                    c = (left.Length > 0 && !left.EndsWith('+') ? left + "+" : left) + c + (right.Length > 0 && !right.StartsWith('+') ? "+" + right : right);
                    // c = left + c + right;
                    n = c.Length < n.Length ? c : n;
                }
            }

            return n;
        }

        class Entry : INode
        {
            static int _id = 0;
            public int Id { get; private set; }

            public Reduction Reduction { get; private set; }
            public int Index { get; private set; }

            public readonly int start, end, length;
            public readonly List<Node> nexts = new List<Node>();

            public Entry(int start, int end, int index, Reduction reduction)
            {
                this.Id = _id++;
                this.start = start;
                this.end = end;
                this.length = end - start + 1;

                this.Reduction = reduction;
                this.Index = index;
            }

            public override string ToString()
            {
                return $"[{this.start} .. {this.end}]";
            }
        }

        class Reduction : INode
        {
            static int _id = 0;
            public int Id { get; private set; }
            public Node Node { get; private set; }

            public int Start { get; private set; }
            public int End { get; private set; }

            public Entry FirstEntry { get; private set; }
            public LinkedList<Entry> Entries { get; private set; }

            public Reduction(int start, int end, Node node)
            {
                this.Id = _id++;
                this.Node = node;

                this.Start = start;
                this.End = end;

                this.Entries = new LinkedList<Entry>();
                this.Entries.AddLast(this.FirstEntry = new Entry(start, end, 0, this));
            }

            public bool TryAddEntry(int start, int pos)
            {
                if (this.End + 1 == start)
                {
                    this.Entries.AddLast(new Entry(start, pos, this.Entries.Count, this));
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
                return $"[{this.Start} .. {this.End}] == " + this.FirstEntry + "*" + this.Entries.Count;
            }
        }

        class Node : INode
        {
            static int _id = 0;

            public int Id { get; private set; }
            public char Character { get; private set; }
            public Dictionary<char, Node> Childs { get; private set; }
            public LinkedList<Reduction> Reductions { get; private set; }

            public Node(char character)
            {
                this.Id = _id++;
                this.Character = character;
                this.Childs = new Dictionary<char, Node>();
                this.Reductions = new LinkedList<Reduction>();
            }

            public Node Add(int start, int pos, char ch, out Reduction r)
            {
                if (!this.Childs.TryGetValue(ch, out var node))
                    this.Childs.Add(ch, node = new Node(ch));

                var last = node.Reductions.Last;
                while (last != null && last.Value.TryAddEntry(start, pos))
                    last = last.Previous;

                node.Reductions.AddLast(r = new Reduction(start, pos, this));

                return node;
            }

            public override string ToString()
            {
                return "Node#" + this.Id;
            }
        }

        private ComposedString MakeTree(string str)
        {
            var root = new Node('\0');
            var actives = new Node[str.Length];

            //var allEntries = new List<Entry>();
            var allReductions = new List<Reduction>();

            for (int i = 0; i < str.Length; i++)
            {
                var c = str[i];

                for (int j = 0; j < i; j++)
                {
                    actives[j] = actives[j].Add(j, i, c, out var r);
                    allReductions.Add(r);
                }

                actives[i] = root.Add(i, i, c, out var r1);
                allReductions.Add(r1);
            }

            var compactingReductions = allReductions.Where(r => r.Entries.Count > 1).OrderBy(r => r.Start).ToList();

            //compactingReductions.ForEach(r => Console.WriteLine(r));
            //Console.WriteLine("---------------");
            var text = new StringFragment(str);
            if (compactingReductions.Count == 0)
                return text;
            // var solutions = new List<ComposedString>();

            var qq = new Queue<Fragment>();
            var start = new Fragment(new Reduction(-1, 0, null), null, 0, new StringFragment(""));
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

                    content += text.Substring(next.FirstEntry.start, next.FirstEntry.length) + "*" + next.Entries.Count;

                    qq.Enqueue(new Fragment(next, curr, curr.Length + next.End - next.Start + 1, content));
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

            //solutions.Sort((a, b) => a.Length.CompareTo(b.Length));
            //solutions.ForEach(s => Console.WriteLine(s.GetContent()));

            ////solutions.ForEach(r => Console.WriteLine(r.Length + ": " + string.Join(" --> ", r.Reverse().Select(f => f.Reduction))));
            //// Console.WriteLine(solution.Length + ": " + string.Join(" --> ", solution.Reverse().Select(f => f.Reduction)));

            //ComposedString result = text.Substring(solution.Reduction.End + 1);

            //for (var p = solution; p.Prev != null; p = p.Prev)
            //{
            //    var current = text.Substring(p.Reduction.FirstEntry.start, p.Reduction.FirstEntry.length) + "*" + p.Reduction.Entries.Count;
            //    ComposedString prefix;

            //    if (p.Prev.Reduction.Start == -1)
            //        prefix = text.Substring(0, p.Reduction.Start);
            //    else if (p.Prev.Reduction.End < p.Reduction.Start)
            //        prefix = text.Substring(p.Prev.Reduction.End + 1, p.Reduction.Start - p.Prev.Reduction.End - 1);
            //    else
            //        prefix = new StringFragment("");

            //    result = prefix + (prefix.Length > 0 ? "+" : "") + current + (result.Length > 0 ? "+" : "") + result;
            //}

            //var result = solution.Content;
            //if (result.Length > text.Length || solution.Reduction.Start == -1)
            //    result = text;

            // Console.WriteLine(result);


            return solution;
            //Environment.Exit(0);
            #region 
            //var ee = allEntries.OrderBy(e => e.start).ToArray();
            //Console.WriteLine(ee);
            //var rr = allReductions.OrderBy(r => r.Start).ToArray();
            //Console.WriteLine(rr);

            //var reds = allReductions.GroupBy(r => r.Start).Select(g => g.ToList()).ToList();

            //var awaiting = new LinkedList<Fragment>();

            //reds[0].ForEach(r => r.Entries.Select((e, n) => awaiting.AddLast(new Fragment(r, e, n + 1))));
            //while (awaiting.Count > 0)
            //{
            //    var p = awaiting.First.Value;
            //    awaiting.RemoveFirst();

            //    if (handled.Contains(p.node.Id))
            //        continue;

            //    if (goal(p.node))
            //        return p;

            //    handled.Add(p.node.Id);

            //    foreach (var next in childs(p))
            //    {
            //        var newCost = cost(p, next);
            //        var newNode = new PathNode<T, C>(next, p, newCost);

            //        var node = awaiting.First;
            //        while (node != null && node.Value.context.CompareTo(newCost) < 0)
            //            node = node.Next;

            //        if (node == null)
            //            awaiting.AddLast(newNode);
            //        else
            //            awaiting.AddBefore(node, newNode);
            //    }
            //}
            #endregion
            //return root;
        }

        class Fragment : IEnumerable<Fragment>
        {
            public Reduction Reduction { get; private set; }
            public Fragment Prev { get; private set; }
            public int Length { get; private set; }
            public ComposedString Content { get; private set; }

            public Fragment(Reduction reduction, Fragment prev, int len, ComposedString content)
            {
                this.Reduction = reduction;
                this.Prev = prev;
                this.Length = len;
                this.Content = content;
            }

            public IEnumerator<Fragment> GetEnumerator()
            {
                for (var f = this; f != null; f = f.Prev)
                {
                    yield return f;
                }
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }
        }

        interface INode
        {
            int Id { get; }
        }

        class PathNode<T, C>
            where T : INode
            where C : IComparable<C>
        {
            public readonly T node;
            public readonly PathNode<T, C> prev;
            public readonly C context;

            public PathNode(T node, PathNode<T, C> prev, C cost)
            {
                this.node = node;
                this.prev = prev;
                this.context = cost;
            }

            public override string ToString()
            {
                return this.context.ToString();
            }
        }

        private PathNode<T, C> FindPath<T, C>(T start, Func<PathNode<T, C>, IEnumerable<T>> childs, Func<T, bool> goal, Func<PathNode<T, C>, T, C> cost)
            where T : INode
            where C : IComparable<C>
        {
            var handled = new HashSet<int>();

            var awaiting = new LinkedList<PathNode<T, C>>();
            var solutions = new LinkedList<PathNode<T, C>>();

            awaiting.AddLast(new PathNode<T, C>(start, null, cost(null, start)));
            while (awaiting.Count > 0)
            {
                if (output)
                {
                    foreach (var item in awaiting.Take(50))
                    {
                        for (var t = item; t != null; t = t.prev)
                        {
                            Console.Write(t.context);
                            Console.Write(" <-- ");
                        }
                        Console.WriteLine();
                    }
                    Console.WriteLine("-----------------------------------------------");
                }
                var p = awaiting.First.Value;
                awaiting.RemoveFirst();

                if (handled.Contains(p.node.Id))
                    continue;

                handled.Add(p.node.Id);

                if (goal(p.node))
                    return p;
                //{
                //    solutions.AddLast(p);
                //    continue;
                //}

                foreach (var next in childs(p))
                {
                    var newCost = cost(p, next);
                    var newNode = new PathNode<T, C>(next, p, newCost);

                    var node = awaiting.First;
                    while (node != null && node.Value.context.CompareTo(newCost) < 0)
                        node = node.Next;

                    if (node == null)
                        awaiting.AddLast(newNode);
                    else
                        awaiting.AddBefore(node, newNode);
                }
            }

            return null;
        }
    }
}
