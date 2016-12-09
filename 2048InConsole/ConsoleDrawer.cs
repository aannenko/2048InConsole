using System;
using System.Text;

namespace _2048InConsole
{
    internal class ConsoleDrawer
    {
        private readonly int _lineLength;

        internal ConsoleDrawer(int lineLength)
        {
            _lineLength = lineLength;
        }

        internal void FillLine(string contents)
        {
            var builder = new StringBuilder(contents);
            while (builder.Length < _lineLength)
                builder.Append(contents);

            if (builder.Length > _lineLength)
                builder.Remove(_lineLength, builder.Length - _lineLength);

            Console.Write(builder);
            Console.WriteLine();
        }

        internal void FillLineWithBlocks(params string[] contents)
        {
            var blockLenght = _lineLength / contents.Length;
            var remainder = _lineLength % contents.Length;
            var builder = new StringBuilder();
            foreach (var block in contents)
            {
                builder.Append(GetStringBlock(block, blockLenght));
                if (remainder < 1) continue;
                builder.Append(" ");
                remainder--;
            }

            Console.Write(builder);
            Console.WriteLine();
        }

        public string GetStringBlock(string content, int blockLenght)
        {
            if (content.Length > blockLenght - 2)
                content = content.Remove(content.Length - 2);

            var builder = new StringBuilder("|");
            var full = blockLenght - 2 - content.Length;
            var secondPart = full / 2;
            var firstPart = full - secondPart;

            builder.Append(new string(' ', firstPart));
            builder.Append(content);
            builder.Append(new string(' ', secondPart));
            builder.Append("|");

            return builder.ToString();
        }
    }
}
