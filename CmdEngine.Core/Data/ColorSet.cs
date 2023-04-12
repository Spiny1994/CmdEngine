using System;

namespace CmdEngine.Core.Data
{
    public struct ColorSet
    {
        public readonly static ColorSet DefaultInverted = new ColorSet(ConsoleColor.Gray, ConsoleColor.Black);

        public ConsoleColor BackgroundColor { get; set; }
        public ConsoleColor ForegroundColor { get; set; }

        public ColorSet(ConsoleColor backgroundColor = ConsoleColor.Black, ConsoleColor foregroundColor = ConsoleColor.Gray)
        {
            BackgroundColor = backgroundColor;
            ForegroundColor = foregroundColor;
        }
    }
}
