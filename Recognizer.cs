using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Recognition
{
	class Recognizer
	{
		internal byte[,] field;
		public bool Overriden { get; set; }
		public string Name { get; set; }
		public int Index { get; set; }
		public virtual string Recognize()
		{
			field = Select();
			if (field == null) return null;
			Console.Clear();
			PrintField(field);
			return "";
		}
		public List<FileInfo> GetFiles()
		{
			try { return new DirectoryInfo(Directory.GetCurrentDirectory() + "\\img").GetFiles().ToList(); }
			catch { return new List<FileInfo>(); }
		}
		public byte GetCell(int i, int j)
		{
			if (i < 0 || j < 0 || i >= field.GetLength(0) || j >= field.GetLength(1))
				return 0;
			return field[i, j];
		}
		public byte[,] Select()
		{
			while (true)
			{
				Console.Clear();
				var f = GetFiles();
				var i = 0;
				for (; i < f.Count; i++)
					Console.WriteLine("{0})\t{1}", i + 1, f[i].Name);
				Console.WriteLine("{0})\tВвести вручную", ++i);
				Console.WriteLine("0)\tВыйти");
				var index = Convert.ToInt32(Console.ReadLine());
				if (index == i)
				{
					var field = EnterField();
					if (field != null) return field;
				}
				else if (index == 0) return null;
				else if (index > 0 && index < i)
				{
					var field = SelectFile(f[index - 1]);
					if (field != null) return field;
				}
			}
		}
		public byte[,] EnterField()
		{
			Console.Clear();
			int w = GetInt("Введите ширину: ", 1);
			int h = GetInt("Введите высоту: ", 1);
			return new EnterField(w, h).GetField();
		}
		public int GetInt(string q, int min = int.MinValue, int max = int.MaxValue)
		{
			while (true)
			{
				try
				{
					Console.Write(q);
					int x = Convert.ToInt32(Console.ReadLine());
					if (x < max && x >= min) return x;
					throw new Exception();
				}
				catch (Exception)
				{
					Console.WriteLine("Ошибка ввода");
				}
			}
		}
		public void PrintField(byte[,] field)
		{
			int n = field.GetLength(0);
			int m = field.GetLength(1);
			for (int i = 0; i < n; i++)
			{
				for (int j = 0; j < m; j++)
					Console.Write(field[i, j] > 0 ? '#' : ' ');
				Console.WriteLine();
			}
		}
		public byte[,] SelectFile(FileInfo file)
		{
			Console.Clear();
			Console.WriteLine(file.Name);
			byte[,] res = ReadField(file.FullName);
			PrintField(res);
			Console.WriteLine("Нажмите Enter, чтобы выбрать данное изображение,");
			Console.WriteLine("или любую другую клавишу для отмены");
			if (Console.ReadKey().Key == ConsoleKey.Enter) return res;
			return null;
		}
		byte[,] ReadField(string path)
		{
			byte[,] res;
			using (StreamReader sr = new StreamReader(path))
			{
				string[] size = sr.ReadLine().Split(' ');
				int n = Convert.ToInt32(size[0]);
				int m = Convert.ToInt32(size[1]);
				res = new byte[n, m];
				for (int i = 0; i < n; i++)
				{
					string vals = sr.ReadLine();
					for (int j = 0; j < m; j++)
						res[i, j] = (byte)(vals[j] - '0');
				}
			}
			return res;
		}
	}
}
