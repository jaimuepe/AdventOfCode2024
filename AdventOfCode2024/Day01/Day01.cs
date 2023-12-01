using AdventOfCode2024.Core;

namespace AdventOfCode2024.Day01
{
    internal class Day01 : DayXX
    {
        protected override void SolvePart1(List<string> lines)
        {
            SolveCommon(lines, false);
        }

        protected override void SolvePart2(List<string> lines)
        {
            SolveCommon(lines, true);
        }

        private void SolveCommon(List<string> lines, bool tokenizeSpelledNumbers)
        {
            var total = 0;

            foreach (var line in lines)
            {
                var first = -1;
                var last = -1;

                var tokenizer = new Tokenizer(line);
                var tokens = tokenizer.FindTokens(tokenizeSpelledNumbers);

                foreach (var token in tokens)
                {
                    if (first == -1) first = token;
                    last = token;
                }

                total += first * 10 + last;
            }

            PrintResult(total);
        }

        private class Tokenizer
        {
            private int _i;

            private readonly string _raw;

            public Tokenizer(string raw)
            {
                _raw = raw;
            }

            public IEnumerable<int> FindTokens(bool tokenizeSpelledNumbers)
            {
                _i = 0;

                while (_i < _raw.Length)
                {
                    var c = _raw[_i];

                    if (c is >= '0' and <= '9')
                    {
                        yield return c - '0';
                    }
                    else if (tokenizeSpelledNumbers)
                    {
                        if (c == 'o' && Peek(1) == 'n' && Peek(2) == 'e')
                        {
                            yield return 1;
                        }
                        else if (c == 't')
                        {
                            if (Peek(1) == 'w' && Peek(2) == 'o')
                            {
                                yield return 2;
                            }
                            else if (Peek(1) == 'h' && Peek(2) == 'r' && Peek(3) == 'e' && Peek(4) == 'e')
                            {
                                yield return 3;
                            }
                        }
                        else if (c == 'f')
                        {
                            if (Peek(1) == 'o' && Peek(2) == 'u' && Peek(3) == 'r')
                            {
                                yield return 4;
                            }
                            else if (Peek(1) == 'i' && Peek(2) == 'v' && Peek(3) == 'e')
                            {
                                yield return 5;
                            }
                        }
                        else if (c == 's')
                        {
                            if (Peek(1) == 'i' && Peek(2) == 'x')
                            {
                                yield return 6;
                            }
                            else if (Peek(1) == 'e' && Peek(2) == 'v' && Peek(3) == 'e' && Peek(4) == 'n')
                            {
                                yield return 7;
                            }
                        }
                        else if (c == 'e' && Peek(1) == 'i' && Peek(2) == 'g' && Peek(3) == 'h' && Peek(4) == 't')
                        {
                            yield return 8;
                        }
                        else if (c == 'n' && Peek(1) == 'i' && Peek(2) == 'n' && Peek(3) == 'e')
                        {
                            yield return 9;
                        }
                    }

                    _i++;
                }
            }

            private char Peek(int skip)
            {
                if (_i + skip >= _raw.Length) return '\0';
                return _raw[_i + skip];
            }
        }
    }
}