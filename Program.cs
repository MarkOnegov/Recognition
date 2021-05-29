using System;
using System.Collections.Generic;
using System.Linq;

namespace Recognition
{
	class Program
	{
		static Recognizer Select(List<Recognizer> tasks)
		{
			Console.WriteLine("Выберите задачу:");
			tasks.ForEach(t =>
			{ Console.WriteLine("{0})\t{1}", t.Index, t.Name); });
			Console.WriteLine("0)\tВыход");
			while (true)
			{
				try
				{
					var index = Convert.ToInt32(Console.ReadLine());
					if (index == 0) return null;
					var task = tasks.Where(t => t.Index == index).ToList();
					if (task.Count == 1) return task[0];
					throw new ArgumentOutOfRangeException();
				}
				catch { Console.WriteLine("Ошибка ввода. Попробуйте ещё раз"); }
			}
		}
		static void Main()
		{
			var tasks = typeof(Recognizer).Assembly.GetTypes()
				.Where(type => type.IsSubclassOf(typeof(Recognizer)))
				.Select(type => (Recognizer)Activator.CreateInstance(type))
				.Where(inst => inst.Overriden)
				.OrderBy(i => i.Index)
				.ToList();
			while (true)
			{
				Console.Clear();
				Recognizer task = Select(tasks);
				Console.Clear();
				if (task == null) return;
				try { Console.WriteLine(task.Recognize()); Console.ReadKey(); }
				catch (Exception e)
				{
					Console.Clear();
					Console.WriteLine(e);
					Console.ReadKey();
				}
			}
		}
	}
}
