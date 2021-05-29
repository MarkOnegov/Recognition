using System;

namespace Recognition
{
	class EnterField
	{
		readonly byte[,] field;
		private readonly int w;
		private readonly int h;
		private int x = 0;
		private int y = 0;
		public EnterField(int w, int h)
		{
			this.w = w;
			this.h = h;
			field = new byte[h, w];
		}
		void PrintInfo()
		{
			Console.WriteLine("Нажмите Enter, чтобы изменить состояние клетки");
			Console.WriteLine("Нажмите Escape, чтобы отменить");
			Console.WriteLine("Нажмите Ctrl+Enter, чтобы выбрать это изображение");
		}
		void PrintField()
		{
			Console.SetCursorPosition(0, 0);
			Console.Write('+');
			for (int i = 0; i < w; i++)
				Console.Write('-');
			Console.Write('+');
			for (int i = 1; i <= h; i++)
			{
				Console.SetCursorPosition(0, i);
				Console.Write('|');
				for (int j = 0; j < w; j++)
				{
					bool current = (y == i - 1) && (x == j);
					if (current)
						Console.BackgroundColor = ConsoleColor.DarkGray;
					Console.Write(field[i - 1, j] > 0 ? '#' : ' ');
					if (current)
						Console.BackgroundColor = ConsoleColor.Black;
				}
				Console.Write('|');
			}
			Console.SetCursorPosition(0, h + 1);
			Console.Write('+');
			for (int i = 0; i < w; i++)
				Console.Write('-');
			Console.Write('+');
		}
		public byte[,] GetField()
		{
			Console.Clear();
			Console.CursorVisible = false;
			PrintField();
			Console.WriteLine();
			PrintInfo();
			bool loop = true;
			bool esc = false;
			while (loop)
			{
				var key = Console.ReadKey();
				if (key.Key == ConsoleKey.Escape)
				{
					loop = false;
					esc = true;
				}
				else if (key.Key == ConsoleKey.Enter)
				{
					if ((key.Modifiers & ConsoleModifiers.Control) > 0)
						loop = false;
					else
						field[y, x] = (byte)(1 - field[y, x]);
				}
				else if (key.Key == ConsoleKey.UpArrow) { if (y > 0) y--; }
				else if (key.Key == ConsoleKey.DownArrow) { if (y < h - 1) y++; }
				else if (key.Key == ConsoleKey.LeftArrow) { if (x > 0) x--; }
				else if (key.Key == ConsoleKey.RightArrow) { if (x < w - 1) x++; }
				PrintField();
			}
			Console.CursorVisible = true;
			Console.Clear();
			if (esc) return null;
			return field;
		}
	}
}
