using TestEx.Models;

namespace TestEx
{
	public class DbInitializer
	{
		public static void Initialize(AppDbContext context)
		{
			// Создание базы данных, если её нет
			context.Database.EnsureCreated();

			// Проверка наличия данных
			if (context.Rooms.Any() && context.Specializations.Any() && context.Districts.Any())
			{
				return; // База данных уже содержит начальные данные
			}

			// Добавление начальных данных для комнат
			var rooms = new[]
			{
				new Room { Number = 101 },
				new Room { Number = 102 },
				new Room { Number = 103 }
			};
			context.Rooms.AddRange(rooms);

			// Добавление начальных данных для специализаций
			var specializations = new[]
			{
				new Specialization { Title = "Терапевт" },
				new Specialization { Title = "Хирург" },
				new Specialization { Title = "Кардиолог" }
			};
			context.Specializations.AddRange(specializations);

			// Добавление начальных данных для районов
			var districts = new[]
			{
				new District { Number = 1 },
				new District { Number = 2 },
				new District { Number = 3 }
			};
			context.Districts.AddRange(districts);

			// Сохранение изменений
			context.SaveChanges();
		}
	}
}
