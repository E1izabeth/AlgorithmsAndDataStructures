using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;

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

        public StringHashes Hashses { get; private set; }
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
            this.Hashses = new StringHashes(text);
        }

        private StringFragment(string text, int start, int length, StringHashes hashes)
            : base(length)
        {
            if (start < 0 || start > text.Length)
                throw new ArgumentOutOfRangeException();
            if (length < 0 || length > text.Length - start)
                throw new ArgumentOutOfRangeException();

            this.Text = text;
            this.Start = start;
            this.Hashses = hashes;
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

            return new StringFragment(this.Text, this.Start + start, length, this.Hashses);
        }

        public StringFragment Substring(int start)
        {
            return this.Substring(start, this.Length - start);
        }

        public override bool StartsWith(char c)
        {
            return this.Length > 0 &&  this.Text[this.Start] == c;
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
            return this.Left.StartsWith(c);
        }

        public override bool EndsWith(char c)
        {
            return this.Right.EndsWith(c);
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
                //str = new string(str.Select(c => rnd.NextDouble() < 0.011852356269974474 ? chars[rnd.Next(chars.Length)] : c).ToArray());

                var repl = this.R(str).GetContent();
                //var regex = new Regex(@"([0-9]+)");
                //var repl = regex.Replace(s, "*$1+");
                sw.WriteLine(repl);
            }
        }

        ComposedString R(StringFragment text)
        {
            ComposedString n = text;
            int length = text.Length;
            int r, j;

            for (int step = length / 2; step > 0; --step)
            // for (int step = 1, limit = length / 2; step < length; ++step)
            {
                for (int pos = 0; step + step + pos <= length; pos++)
                {
                    var fragment = text.Substring(pos, step);
                    for (j = pos + step, r = 1; j + step <= length && fragment == text.Substring(j, step); j += step)
                        ++r;

                    var c = fragment + "*" + r.ToString();
                    if (c.Length < step * r)
                    {
                        var left = this.R(text.Substring(0, pos));
                        var right = this.R(text.Substring(j));
                        c = (left.Length > 0 && !left.EndsWith('+') ? left + "+" : left) + c + (right.Length > 0 && !right.StartsWith('+') ? "+" + right : right);
                        // c = left + c + right;
                        n = c.Length < n.Length ? c : n;

                        break;
                    }
                }
            }
            return n;
        }
        
    }

    class StringHashes
    {
        readonly long[] _hs;

        public StringHashes(string str)
        {
            _hs = new long[str.Length];

            _hs[0] = str[0];
            for (int i = 1; i < str.Length; i++)
            {
                _hs[i] = _hs[i - 1] + (1 << i) * str[i];
            }
        }

        public long GetHash(int start, int length)
        {
            long result = _hs[start + length - 1];
            if (start > 0)
                result -= _hs[start - 1];

            return result;
        }

        public static bool CompareRanges(StringHashes a, int ap, int ad, StringHashes b, int bp, int bd)
        {
            return a.GetHash(ap, ad) * (1 << bp) == b.GetHash(bp, bd) * (1 << ap);
        }
    }


}
