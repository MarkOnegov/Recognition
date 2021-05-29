using System;
using System.Collections.Generic;
using System.Linq;

namespace Recognition
{
	class SmileRecognizer : Recognizer
	{
		class Point
		{
			public int X { get; set; }
			public int Y { get; set; }
			public Point(int x, int y)
			{
				X = x;
				Y = y;
			}
		}
		List<List<Point>> figuresPoint;
		public SmileRecognizer()
		{
			Name = "Смайлики";
			Index = 1;
			Overriden = true;
		}
		public override string Recognize()
		{
			base.Recognize();
			if (field == null) return null;
			figuresPoint = new List<List<Point>>();
			GetAllFigures();
			List<Point> mouthPoint = GetMouth();
			List<int> mouthYs = GetShapeMouth(mouthPoint);
			List<int> derivativeMouth = DerivativeShate(mouthYs);
			return ":" + GetMouth(derivativeMouth);
		}
		private void FillFigure(int i, int j, List<Point> points)
		{
			if (GetCell(i, j) == 0) return;
			points.Add(new Point(j, i));
			field[i, j] = 0;
			for (int i0 = -1; i0 < 2; i0++)
				for (int j0 = -1; j0 < 2; j0++)
					FillFigure(i + i0, j + j0, points);
		}
		private List<Point> FillFigure(int i, int j)
		{
			List<Point> points = new List<Point>();
			FillFigure(i, j, points);
			return points.OrderBy(p => p.X).ToList();
		}
		private int GetSquare(List<Point> figure)
		{
			int xLast = -1;
			int yTop = 0, yBottom = -1;
			int square = 0;
			figure.ForEach(f =>
			{
				if (xLast != f.X)
				{
					square += yBottom - yTop + 1;
					yTop = int.MaxValue;
					yBottom = int.MinValue;
					xLast = f.X;
				}
				else
				{
					yTop = Math.Min(yTop, f.Y);
					yBottom = Math.Max(yBottom, f.Y);
				}
			});
			square += yBottom - yTop;
			return square;
		}
		private int GetSign(int a) => a == 0 ? 0 : a > 0 ? 1 : -1;
		private char GetMouth(List<int> derivativeShape)
		{
			int revers = 0;
			int dir = GetSign(derivativeShape[0]);
			int l = derivativeShape.Count;
			for (int i = 1; i < l; i++)
			{
				int nDir = GetSign(derivativeShape[i]);
				if (nDir != 0 && dir != nDir)
				{
					if (dir != 0)
						revers++;
					if (revers > 1)
						return '$';
					dir = nDir;
				}
			}
			if (revers == 0)
			{
				if (dir == 0)
					return '|';
				if (dir > 0)
					return '/';
				return '\\';
			}
			if (dir > 0)
				return '(';
			return ')';
		}
		private List<int> DerivativeShate(List<int> shape)
		{
			int l = shape.Count;
			List<int> derivativeShape = new List<int>();
			for (int i = 1; i < l; i++)
				derivativeShape.Add(shape[i] - shape[i - 1]);
			return derivativeShape;
		}
		private List<int> GetShapeMouth(List<Point> mouth)
		{
			List<int> shape = new List<int>();
			int xLast = -1;
			int yTop = 0, yBottom = 0;
			bool added = false;
			mouth.ForEach(f =>
			{
				if (xLast != f.X)
				{
					if (added)
						shape.Add((yBottom + yTop) / 2);
					else
						shape.Add(f.Y);
					yTop = int.MaxValue;
					yBottom = int.MinValue;
					xLast = f.X;
					added = false;
				}
				else
				{
					added = true;
					yTop = Math.Min(yTop, f.Y);
					yBottom = Math.Max(yBottom, f.Y);
				}
			});
			return shape;
		}
		private List<Point> GetMouth()
		{
			List<int> squares = figuresPoint.Select(GetSquare).ToList();
			List<KeyValuePair<int, List<Point>>> sortedFigures = new List<KeyValuePair<int, List<Point>>>();
			for (int i = squares.Count - 1; i >= 0; i--)
				sortedFigures.Add(new KeyValuePair<int, List<Point>>(squares[i], figuresPoint[i]));
			return sortedFigures.OrderByDescending(f => f.Key).Select(f => f.Value).ToList()[1];
		}
		private void GetAllFigures()
		{
			int n = field.GetLength(0), m = field.GetLength(1);
			for (int i = 0; i < n; i++)
				for (int j = 0; j < m; j++)
					if (GetCell(i, j) > 0)
						figuresPoint.Add(FillFigure(i, j));
			if (figuresPoint.Count != 4) throw new ArgumentException("Не удалось распознать смайлик");
		}
	}
}
