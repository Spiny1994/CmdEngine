using System;

namespace CmdEngine.Core.Data
{
    public struct ColorChar
    {
        public char Char { get; set; }
        public ColorSet ColorSet { get; set; }

        public ColorChar(char c = ' ', ConsoleColor backgroundColor = ConsoleColor.Black, ConsoleColor foregroundColor = ConsoleColor.White)
        {
            Char = c;
            ColorSet = new ColorSet(backgroundColor, foregroundColor);
        }

        public ColorChar(char c = ' ', ColorSet colorSet = new ColorSet())
        {
            Char = c;
            ColorSet = colorSet;
        }
    }
}
