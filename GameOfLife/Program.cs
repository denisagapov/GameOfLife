using System;
using System.Threading;

namespace GameOfLife
{
    class Program
    {
        static void Main(string[] args)
        {   int width = 50, height = 30, borderwidth = 1;
            char livingCell = 'o', deadCell = ' ';
            int value;
            Console.SetCursorPosition(0, 0);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Выберите режим игры:");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("1.Автоматический режим");
            Console.WriteLine("2.Ручной режим (переход к следующему шагу по нажатию Enter)");
            value = Convert.ToInt32(Console.ReadLine());
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("                         Инструкция");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Для того чтобы поставить/удалить клетку - нажмите кнопку Space");
            Console.WriteLine("Игра будет начата через 10 секунд...");
            Thread.Sleep(10000);
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            GameOfLife.CreateInstance(width, height, borderwidth, livingCell, deadCell, value);
            GameOfLife.GetDrawing();
            GameOfLife.Start();
            
        }
    }
    static class GameOfLife
    {
        public static int Value { get; set; }
        public static int Width { get; set; }
        public static int Height { get; set; } 
        public static int BorderWidth { get; set; }
        public static char SelectionCell { get; set; }
        public static char LivingCell { get; set; } 
        public static char DeadCell { get; set; } 
        static char[,] current; 
        static char[,] comming; 
        static Random random = new Random(); 

        public static void CreateInstance(int width, int height, int borderWidth,
        char livingCell, char deadCell, int value)
        {
            Width = width;
            Height = height;
            BorderWidth = borderWidth;
            LivingCell = livingCell;
            DeadCell = deadCell;
            SelectionCell = 'X';
            Value = value;

            Console.CursorVisible = false;
            Console.SetWindowSize(Width, Height);
            Console.SetBufferSize(Width, Height);

            current = new char[Width, Height];
            comming = new char[Width, Height];
            Console.Clear();
            
            // заполнение поля мертвыми клетками
            for (int x = BorderWidth; x < Width - BorderWidth; x++)
            {
                for (int y = BorderWidth; y < Height - BorderWidth; y++)
                {
                    current[x, y] = comming[x, y] = DeadCell;
                    Console.SetCursorPosition(x, y);
                    Console.Write(current[x, y]);
                }
            }
        }

        public static void Start() // заполнение поля живыми клетками
        {
            int foundAliveCells = 0; // живые клетки
            int step = 0; // количество шагов

            while (true)
            {
                Array.Copy(current, comming, Width * Height);
                for (int y = BorderWidth; y < Height - BorderWidth - 1; y++)
                {
                    for (int x = BorderWidth; x < Width - BorderWidth; x++)
                    {
                        Console.SetCursorPosition(x, y);
                        Console.Write(current[x, y]);
                        // перемещение по ячейкам и проверка соседних клеток
                        for (int i = -1; i < 2; i++)
                            for (int j = -1; j < 2; j++)
                            {
                                if (!(i == 0 && j == 0) && 
                                    ((x + i >= BorderWidth && x + i < Width - BorderWidth) && 
                                    (y + j >= BorderWidth && y + j < Height - BorderWidth)))
                                    if (current[x + i, y + j] == LivingCell) 
                                        foundAliveCells++; // если соседняя клетка живая - увеличение на 1
                            }

                        // проверка по правилам
                        if (current[x, y] == LivingCell && (foundAliveCells >= 4 || foundAliveCells <= 1))
                            comming[x, y] = DeadCell; // клетка погибает
                        else if (current[x, y] == DeadCell && foundAliveCells == 3)
                            comming[x, y] = LivingCell; // клетка рождается

                        foundAliveCells = 0;
                    }
                }
                // копируется предыдущий массив
                Array.Copy(comming, current, Width * Height);

                Console.Title = "Шаг: " + step;
                step++;
                // если выбран автоматический режим
                if (Value == 1)
                {
                    Thread.Sleep(200);
                }
                // если выбран автоматический режим
                else
                {
                    ConsoleKeyInfo btn;
                    do
                    {
                        btn = Console.ReadKey();
                    }
                    while (!(btn.Key == ConsoleKey.Enter));
                }
            }
        }
        public static void GetDrawing()
        {
            ConsoleKeyInfo input = new ConsoleKeyInfo();
            int x = Width / 2, y = Height / 2; 
            Console.SetCursorPosition(x, y);
            Console.Write(SelectionCell);
            while (input.Key != ConsoleKey.Enter) 
            {
                input = Console.ReadKey(true);
                Console.SetCursorPosition(x, y);
                Console.Write(current[x, y]);

                switch (input.Key)
                {
                    case ConsoleKey.UpArrow:
                        if (y > BorderWidth)
                            y -= 1;
                        break;

                    case ConsoleKey.DownArrow:
                        if (y + 1 < Height - BorderWidth)
                            y += 1;
                        break;

                    case ConsoleKey.RightArrow:
                        if (x + 1 < Width - BorderWidth)
                            x += 1;
                        break;

                    case ConsoleKey.LeftArrow:
                        if (x > BorderWidth)
                            x -= 1;
                        break;

                    case ConsoleKey.Spacebar: 
                        current[x, y] = current[x, y] == DeadCell ? LivingCell : DeadCell;
                        break;
                }

                Console.SetCursorPosition(x, y);
                Console.Write(SelectionCell);
            }
            Start();
        }
    }
}