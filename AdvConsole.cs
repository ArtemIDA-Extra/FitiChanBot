using System;
using System.Collections.Generic;
using System.Threading;

namespace FitiChanBot
{
    public static class AdvConsole
    {
        private static ConsoleColor foregColorBackup;
        private static ConsoleColor backColorBackup;

        public static void SaveBackupColors()
        {
            foregColorBackup = Console.ForegroundColor;
            backColorBackup = Console.BackgroundColor;
        }
        public static void ApplyBackupColors()
        {
            Console.ForegroundColor = foregColorBackup;
            Console.BackgroundColor = backColorBackup;
        }
        public static void ApplyColors(ConsoleColor foregColor, ConsoleColor backColor)
        {
            Console.ForegroundColor = foregColor;
            Console.BackgroundColor = backColor;
        }

        public static void WriteLine(string text, int leftOffset = 0, ConsoleColor foregColor = ConsoleColor.Gray, ConsoleColor backColor = ConsoleColor.Black, bool offsetUseColor = false)
        {
            SaveBackupColors();
            if (offsetUseColor) ApplyColors(foregColor, backColor);
            Console.Write(new string(' ', leftOffset));
            if (!offsetUseColor) ApplyColors(foregColor, backColor);
            Console.WriteLine(text);
            ApplyBackupColors();
        }
        public static void Write(string text, int leftOffset = 0, ConsoleColor foregColor = ConsoleColor.Gray, ConsoleColor backColor = ConsoleColor.Black, bool offsetUseColor = false)
        {
            SaveBackupColors();
            if (offsetUseColor) ApplyColors(foregColor, backColor);
            Console.Write(new string(' ', leftOffset));
            if (!offsetUseColor) ApplyColors(foregColor, backColor);
            Console.Write(text);
            ApplyBackupColors();
        }
        public static void Replace(string text, (int Left, int Top) Pos, bool savePos = true, ConsoleColor foregColor = ConsoleColor.Gray, ConsoleColor backColor = ConsoleColor.Black)
        {
            (int Left, int Top) CursPosBackup = Console.GetCursorPosition();
            Console.SetCursorPosition(Pos.Left, Pos.Top);
            AdvConsole.Write(text, 0, foregColor, backColor);
            if (savePos) Console.SetCursorPosition(CursPosBackup.Left, CursPosBackup.Top);
        }
        public static void LineClear(int line)
        {
            Console.SetCursorPosition(0, line);
            AdvConsole.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, line);
        }
        public static void CurrentLineClear()
        {
            LineClear(Console.CursorTop);
        }
        public static void SeveralLinesClear(int startLine, int finLine)
        {
            if (startLine < finLine)
            {
                for (int i = startLine; i <= finLine; i++)
                    LineClear(i);
            }
            else
            {
                for (int i = startLine; i >= finLine; i--)
                    LineClear(i);
            }
        }
    }
}

