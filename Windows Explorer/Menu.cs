using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Windows_Explorer
{
    public enum ACTION
    { ENTER, SORT, ADD, DEL, BACKSPASE, TEXT_UP, TEXT_DOWN, DATE_UP, DATE_DOWN, SIZE_UP, SIZE_DOWN, TYPE_UP, TYPE_DOWN, NOTHING, COPY, PASTE, CUT }

    public class Menu
    {
        static int[] maxLen = { 40, 20, 20, 20 };
        static string[,] shapka =
        {
            { "Text", "Date","Type", "Size" },
            { "Text↑", "Date↑","Type↑", "Size↑" },
            { "Text↓", "Date↓", "Type↓","Size↓" }
         };

        static string[] usedShapka =
        {
            "Text", "Date", "Type", "Size"
        };

        static string[] staticShapka =
        {
            "Text", "Date","Type", "Size"
        };


        public static void StaticMenu(string address, FileSystemInfo[] elements)
        {

            int x = 0;
            int y = 0;
            Console.Clear();
            Console.SetCursorPosition(x, y);
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;

            Console.WriteLine(address);
            for (int i = 0; i < usedShapka.Length; i++)
            {
                Console.Write(usedShapka[i].PadRight(maxLen[i]));
            }
            x = 0;
            y = 3;
            int pos = 0;
            if (elements.Length == 0)
            {
                Console.WriteLine("Эта папка пуста.".PadLeft(45));
            }
            else
            {

                for (int i = 0; i < elements.GetLength(0); i++)
                {
                    Console.SetCursorPosition(x, y + i);
                    if (i == pos)
                    {
                        Console.BackgroundColor = ConsoleColor.Blue;
                    }
                    else
                    {
                        Console.BackgroundColor = ConsoleColor.White;
                    }
                    if (elements[i].Name.Length < 40)
                        Console.Write(elements[i].Name.PadRight(maxLen[0]));
                    else
                        Console.Write((elements[i].Name.Remove(37) + "...").PadRight(maxLen[0]));
                   
                    if (elements[i].LastWriteTime.ToString().Length < 31)
                        Console.Write(elements[i].LastWriteTime.ToString().PadRight(maxLen[1]));
                    else
                        Console.Write((elements[i].LastWriteTime.ToString().Remove(30) + "...").PadRight(maxLen[1]));
                   
                    if (elements[i] is FileInfo)
                    {
                        Console.Write(elements[i].Extension.ToString().PadRight(maxLen[2]));
                    }
                    else
                        Console.Write("Папка с файлами".PadRight(maxLen[2]));
                    
                    if (elements[i] is FileInfo)
                    {
                        if (((FileInfo)elements[i]).Length.ToString().Length < 31)
                            Console.Write(((FileInfo)elements[i]).Length.ToString().PadRight(maxLen[3]));
                        else
                            Console.Write((((FileInfo)elements[i]).Length.ToString().Remove(30) + "...").PadRight(maxLen[3]));
                    }
                    else
                        Console.Write("".PadRight(maxLen[2]));
                    Console.WriteLine();
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.ForegroundColor = ConsoleColor.Black;
                }
            }

        }

        private static void PrintString(FileSystemInfo[] elements, string address, int pos, ConsoleColor cc)
        {
            int x = 0;
            int y = 3;
            Console.SetCursorPosition(x, y + pos);
            Console.BackgroundColor = cc;
            if (elements[pos].Name.Length < 31)
                Console.Write(elements[pos].Name.PadRight(maxLen[0]));
            else
                Console.Write((elements[pos].Name.Remove(37) + "...").PadRight(maxLen[0]));
            
            if (elements[pos].LastWriteTime.ToString().Length < 31)
                Console.Write(elements[pos].LastWriteTime.ToString().PadRight(maxLen[1]));
            else
                Console.Write((elements[pos].LastWriteTime.ToString().Remove(30) + "...").PadRight(maxLen[1]));
            
            if (elements[pos] is FileInfo)
            {
                Console.Write(elements[pos].Extension.ToString().PadRight(maxLen[2]));
            }
            else
                Console.Write("Папка с файлами".PadRight(maxLen[2]));
            
            if (elements[pos] is FileInfo)
            {
                if (((FileInfo)elements[pos]).Length.ToString().Length < 31)
                    Console.Write(((FileInfo)elements[pos]).Length.ToString().PadRight(maxLen[3] - 1));
                else
                    Console.Write((((FileInfo)elements[pos]).Length.ToString().Remove(30) + "...").PadRight(maxLen[3] - 1));
            }
            else
                Console.Write("".PadRight(maxLen[3] - 1));

            Console.BackgroundColor = ConsoleColor.White;
        }
        public static ACTION VerticalMenu(FileSystemInfo[] elements, string address, ref int pos)
        {
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Clear();
            StaticMenu(address, elements);

            int x = 0;
            int y = 3;
            Console.CursorVisible = false;

            while (true)
            {
                if (elements.Length != 0)
                    PrintString(elements, address, pos, ConsoleColor.Blue);

                ConsoleKey consoleKey = Console.ReadKey().Key;
                switch (consoleKey)
                {

                    case ConsoleKey.Enter:
                        if (elements.Length != 0)
                        {
                            for (int i = 0; i < usedShapka.Length; i++)
                                usedShapka[i] = staticShapka[i];
                            return ACTION.ENTER;
                        }
                        break;
                    case ConsoleKey.UpArrow:
                        if (elements.Length != 0)
                            PrintString(elements, address, pos, ConsoleColor.White);
                        if (pos > 0)
                            pos--;
                        else
                        {
                            ACTION a = GorizontallMenu(elements, address);
                            if (a != ACTION.NOTHING) return a;
                            else StaticMenu(address, elements);
                        }
                        break;

                    case ConsoleKey.DownArrow:
                        if (elements.Length != 0)
                            PrintString(elements, address, pos, ConsoleColor.White);
                        if (pos < elements.GetLength(0) - 1)
                            pos++;
                        break;

                    case ConsoleKey.Backspace:
                        for (int i = 0; i < usedShapka.Length; i++)
                            usedShapka[i] = staticShapka[i];
                        return ACTION.BACKSPASE;
                    case ConsoleKey.Z: // Создать
                        return ACTION.ADD;
                    case ConsoleKey.Delete: // Удалить
                        return ACTION.DEL;

                    case ConsoleKey.C: // Копировать
                        return ACTION.COPY;
                    case ConsoleKey.V:
                        return ACTION.PASTE;
                    case ConsoleKey.X:
                        //num = pos;
                        return ACTION.CUT;
                    default:
                        break;
                }
            }
        }

        public static int VerticalMenu(string[] elements)
        {
            int maxLen = 0;
            foreach (var item in elements)
            {
                if (item.Length > maxLen)
                    maxLen = item.Length;
            }
            int x = Console.CursorLeft;
            int y = Console.CursorTop;
            Console.CursorVisible = false;
            int pos = 0;
            while (true)
            {
                for (int i = 0; i < elements.Length; i++)
                {
                    Console.SetCursorPosition(x, y + i);
                    if (i == pos)
                    {
                        Console.BackgroundColor = ConsoleColor.Blue;
                    }
                    else
                    {
                        Console.BackgroundColor = ConsoleColor.White;
                    }
                    Console.Write(elements[i].PadRight(maxLen));
                }
                Console.BackgroundColor = ConsoleColor.White;

                ConsoleKey consoleKey = Console.ReadKey().Key;
                switch (consoleKey)
                {

                    case ConsoleKey.Enter:

                        Console.CursorVisible = true;
                        return pos;

                    case ConsoleKey.UpArrow:
                        if (pos > 0)
                            pos--;
                        break;

                    case ConsoleKey.DownArrow:
                        if (pos < elements.Length)
                            pos++;
                        break;

                    default:
                        break;
                }


            }
        }

        public static ACTION GorizontallMenu(FileSystemInfo[] elements, string address/*, int maxLen*/)
        {
            int pos = 0;
            while (true)
            {
                int x = 0;
                int y = 1;

                for (int i = 0; i < usedShapka.Length; i++)
                {
                    Console.SetCursorPosition(x, y);
                    x += maxLen[i];
                    if (i == pos)
                    {
                        Console.BackgroundColor = ConsoleColor.Blue;
                    }
                    else
                    {
                        Console.BackgroundColor = ConsoleColor.White;
                    }
                    Console.Write(usedShapka[i].PadRight(maxLen[i]));
                }
                Console.BackgroundColor = ConsoleColor.White;
                ConsoleKey consoleKey = Console.ReadKey().Key;
                switch (consoleKey)
                {

                    case ConsoleKey.Enter:
                        return ChangeUsedShapka(pos);

                    case ConsoleKey.RightArrow:
                        if (pos < 3)
                            pos++;
                        break;

                    case ConsoleKey.LeftArrow:
                        if (pos > 0)
                            pos--;
                        break;

                    case ConsoleKey.DownArrow:
                        return ACTION.NOTHING;

                    case ConsoleKey.Backspace:
                        return ACTION.BACKSPASE;
                    default:
                        break;
                }
            }
        }



        public static int MessageBox(string text, string[] variants, ref string name)
        {
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.SetCursorPosition(25, 8);
            Console.Write("".PadRight(50));
            Console.SetCursorPosition(25, 9);
            Console.Write("".PadRight(3) + text.PadRight(47));
            Console.SetCursorPosition(25, 10);
            Console.Write("".PadRight(50));
            Console.SetCursorPosition(25, 11);
            Console.Write("".PadRight(50));
            Console.SetCursorPosition(25, 12);


            
            Console.WriteLine("".PadRight(15) + variants[0] + "".PadRight(6) + variants[1] + "".PadRight(15));

            Console.SetCursorPosition(25, 13);
            Console.Write("".PadRight(50));
            Console.SetCursorPosition(25, 14);
            Console.Write("".PadRight(20) + "Название: ".PadRight(30));
            Console.SetCursorPosition(25, 15);

            Console.Write("".PadRight(5));
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            
            Console.Write("".PadRight(40));
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write("".PadRight(5));

            
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.SetCursorPosition(25, 16);
            Console.Write("".PadRight(50));
            Console.SetCursorPosition(25, 17);
            Console.Write("".PadRight(50));
            Console.SetCursorPosition(25, 18);

            
            Console.SetCursorPosition(30, 15);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;


            ConsoleColor bg = ConsoleColor.Gray;
            ConsoleColor fg = ConsoleColor.Black;

            int pos = 0;
            while (true)
            {
                int x = 30;
                int y = 12;

                for (int i = 0; i < variants.Length; i++)
                {
                    Console.SetCursorPosition(x, y);
                    x += 20;
                    if (i == pos)
                    {
                        Console.BackgroundColor = fg;
                        Console.ForegroundColor = bg;

                    }
                    else
                    {
                        Console.BackgroundColor = bg;
                        Console.ForegroundColor = fg;

                    }
                    Console.Write(variants[i].PadLeft(20));
                }

                ConsoleKey consoleKey = Console.ReadKey().Key;
                switch (consoleKey)
                {
                    case ConsoleKey.Enter:
                        FillName(out name);
                        return pos;
                    case ConsoleKey.RightArrow:
                        if (pos == 0)
                        {
                            pos++;
                        }
                        break;

                    case ConsoleKey.LeftArrow:
                        if (pos == 1)
                        {
                            pos--;
                        }
                        break;


                    case ConsoleKey.Backspace:
                        return -1;
                    default:
                        break;
                }
            }
        }


        private static void FillName(out string name)
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(30, 15);
            name = Console.ReadLine();
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.ForegroundColor = ConsoleColor.Black;
        }
        private static ACTION ChangeUsedShapka(int pos)
        {
            for (int i = 0; i < usedShapka.Length; i++)
                if (i != pos)
                    usedShapka[i] = shapka[0, i];

            if (usedShapka[pos].Contains("↓"))
            {
                usedShapka[pos] = shapka[1, pos];
                if (pos == 0)
                    return ACTION.TEXT_UP;

                if (pos == 1)
                    return ACTION.DATE_UP;

                else if (pos == 2)
                    return ACTION.TYPE_UP;
                else /*(pos == 3)*/
                    return ACTION.SIZE_UP;
            }
            else
            {
                usedShapka[pos] = shapka[2, pos];
                if (pos == 0)
                    return ACTION.TEXT_DOWN;
                if (pos == 1)
                    return ACTION.DATE_DOWN;
                else if (pos == 2)
                    return ACTION.TYPE_DOWN;
                else /*(pos == 3)*/
                    return ACTION.SIZE_DOWN;
            }
        }
    }
}
