using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuestionSysTB.Commands
{
    public class CommandLineParser
    {
        int _argsLength;

        public CommandLineParser(int argsLength)
        {
            _argsLength = argsLength;
        }

        public bool Parse(string text,out string[] args)
        {
            text = System.Text.RegularExpressions.Regex.Replace(text, @"\s+", " ");
            args = text.Split(" ", _argsLength);
            return args.Length == _argsLength;
        }
    }

}
