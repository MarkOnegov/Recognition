using System.Collections.Generic;

namespace Recognition
{
	class NumberRecognizer : Recognizer
	{
		readonly List<byte[]> numbers = new List<byte[]>() {
			new byte[]{2, 2},
			new byte[]{1, 1},
			new byte[]{1, 2, 2, 2, 2, 1},
			new byte[]{1, 2, 1, 3, 1, 2},
			new byte[]{1, 1, 2, 3, 1},
			new byte[]{2, 1, 2, 2, 1, 2},
			new byte[]{2, 1, 3, 2, 2, 2},
			new byte[]{1, 2, 1},
			new byte[]{2, 2, 3, 3, 2, 2},
			new byte[]{2, 2, 2, 3, 1, 2}
		};
		public NumberRecognizer()
		{
			Name = "Цифры";
			Index = 2;
			Overriden = true;
		}
		public override string Recognize()
		{
			base.Recognize();
			int num = FindeNumeByCorners(GetCorners());
			if (num > -1)
				return num.ToString();
			return "Not number";
		}
		private int FindeNumeByCorners(byte[] corners)
		{
			for (int i = 0; i < 10; i++)
			{
				if (Equals(numbers[i], corners))
					return i;
			}
			return -1;
		}
		private bool Equals(byte[] a, byte[] b)
		{
			if (a.Length != b.Length)
				return false;
			for (int i = 0; i < a.Length; i++)
				if (a[i] != b[i])
					return false;
			return true;
		}
		private byte GetCornerType(int i, int j) =>
			(byte)(GetCell(i, j - 1) + GetCell(i - 1, j) + GetCell(i, j + 1) + GetCell(i + 1, j));
		private bool IsCorner(int i, int j) =>
			GetCell(i, j + 1) + GetCell(i + 1, j) == 2 ||
			GetCell(i, j - 1) + GetCell(i - 1, j) == 2 ||
			GetCell(i, j + 1) + GetCell(i - 1, j) == 2 ||
			GetCell(i, j - 1) + GetCell(i + 1, j) == 2 ||
			GetCell(i, j - 1) + GetCell(i - 1, j) + GetCell(i, j + 1) + GetCell(i + 1, j) == 1;
		private byte[] GetCorners()
		{
			List<byte> corners = new List<byte>();
			int n = field.GetLength(0), m = field.GetLength(1);
			for (int i = 0; i < n; i++)
				for (int j = 0; j < m; j++)
					if (GetCell(i, j) > 0 && IsCorner(i, j))
						corners.Add(GetCornerType(i, j));
			return corners.ToArray();
		}
	}
}
