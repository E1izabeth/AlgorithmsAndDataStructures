using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Openedu.Week4
{
    class Lab4_5
    {
        public static void Main(string[] args)
        {
            var app = new Lab4_5();
            app.DoWork(args);
        }

        private void DoWork(string[] args)
        {
            StreamWriter sw = new StreamWriter("output.txt");
            Console.OutputEncoding = Encoding.ASCII;
            Console.SetOut(sw);

            string[] stdin = File.ReadAllLines("input.txt");
            new QuackMachine(stdin).Run();

            sw.Dispose();
        }
    }


    public class QuackMachine
    {
        private static ushort[] _registers = new ushort[26];
        private static int _cursor = 0;
        private static Dictionary<string, int> _labels = new Dictionary<string, int>();
        private static string[] _program;
        private static Queue<ushort> _queue = new Queue<ushort>();

        public QuackMachine(string[] input)
        {
            _program = input;
            this.RegisterLabels();
        }

        public void Run()
        {
            for (_cursor = 0; _cursor < _program.Length; _cursor++)
            {
                switch (_program[_cursor][0])
                {
                    case '+': this.Sum(); break;
                    case '-': this.Substract(); break;
                    case '*': this.Multiply(); break;
                    case '/': this.Divide(); break;
                    case '%': this.Mod(); break;
                    case '>': this.GetRegister(_program[_cursor][1]); break;
                    case '<': this.SetRegister(_program[_cursor][1]); break;
                    case 'P':
                        if (_program[_cursor].Length == 1)
                            this.PrintValueFromStack();
                        else
                            this.PrintValueFromRegister(_program[_cursor][1]);
                        break;
                    case 'C':
                        if (_program[_cursor].Length == 1)
                            this.PrintCharFromStack();
                        else
                            this.PrintCharFromRegister(_program[_cursor][1]);
                        break;
                    case ':': break;
                    case 'J': this.GoTo(_cursor); break;
                    case 'Z': this.GoToIfZeroEqual(_cursor); break;
                    case 'E': this.GoToIfEquals(_cursor); break;
                    case 'G': this.GoToIfMoreThan(_cursor); break;
                    case 'Q': this.Exit(); break;
                    default: _queue.Enqueue(ushort.Parse(_program[_cursor])); break;
                }
            }
        }

        private void RegisterLabels()
        {
            for (int i = 0; i < _program.Length; i++)
                if (_program[i][0] == ':')
                    _labels.Add(_program[i].Remove(0, 1), i);
        }

        private void Sum()
        {
            ushort a = _queue.Dequeue();
            ushort b = _queue.Dequeue();
            _queue.Enqueue((ushort)((a + b) % 65536));
        }

        private void Substract()
        {
            ushort a = _queue.Dequeue();
            ushort b = _queue.Dequeue();
            _queue.Enqueue((ushort)((a - b) % 65536));
        }

        private void Multiply()
        {
            ushort a = _queue.Dequeue();
            ushort b = _queue.Dequeue();
            _queue.Enqueue((ushort)(a * b));
        }

        private void Divide()
        {
            ushort a = _queue.Dequeue();
            ushort b = _queue.Dequeue();
            _queue.Enqueue((ushort)(a / b));
        }

        private void Mod()
        {
            ushort a = _queue.Dequeue();
            ushort b = _queue.Dequeue();
            _queue.Enqueue((ushort)(a % b));
        }

        private void GetRegister(char register)
        {
            ushort a = _queue.Dequeue();
            _registers[register - 'a'] = a;
        }

        private void SetRegister(char register)
        {
            _queue.Enqueue(_registers[register - 'a']);
        }

        private void PrintValueFromStack()
        {
            ushort a = _queue.Dequeue();
            Console.WriteLine(a);
        }

        private void PrintValueFromRegister(char register)
        {
            Console.WriteLine(_registers[register - 'a']);
        }

        private void PrintCharFromStack()
        {
            ushort a = _queue.Dequeue();
            Console.Write((char)(a % 256));
        }

        private void PrintCharFromRegister(char register)
        {
            Console.Write((char)(_registers[register - 'a'] % 256));
        }

        private void GoTo(int index)
        {
            _cursor = _labels[_program[index].Remove(0, 1)];
        }

        private void GoToIfZeroEqual(int index)
        {
            if (_registers[_program[index][1] - 'a'] == 0)
                _cursor = _labels[_program[index].Remove(0, 2)];
        }

        private void GoToIfEquals(int index)
        {
            if (_registers[_program[index][1] - 'a'] == _registers[_program[index][2] - 'a'])
                _cursor = _labels[_program[index].Remove(0, 3)];
        }

        private void GoToIfMoreThan(int index)
        {
            if (_registers[_program[index][1] - 'a'] > _registers[_program[index][2] - 'a'])
                _cursor = _labels[_program[index].Remove(0, 3)];
        }

        private void Exit()
        {
            _cursor = int.MaxValue;
        }
    }
}