using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace World_of_Battleships
{
	public class AI
	{
		/// <summary>
		/// Заготовки кораблей
		/// </summary>
		Ship[] aiShips;

		/// <summary>
		/// Размещенные корабли
		/// </summary>
		Ship[] aiPlacedShips;
		/// <summary>
		/// Корабли игрока
		/// </summary>
		Ship[] playerShips;
		/// <summary>
		/// Текущий поврежденный корабль игрока
		/// </summary>
		Ship curDamagedPlayerShip;
		/// <summary>
		/// Последнее попадание по ячейке корабля игрока
		/// </summary>
		Cell lastDamagedCell;
		/// <summary>
		/// Игровое поле компьютера
		/// </summary>
		Field aiField;
		/// <summary>
		/// Игровое поле игрока
		/// </summary>
		Field playerField;
		/// <summary>
		/// Получить корабли компьютера
		/// </summary>
		public Ship[] GetPlacedShips { get { return aiPlacedShips; } }

		/// <summary>
		/// Конструктор
		/// </summary>
		public AI(Ship[] aiShips, Ship[] playerShips, Field aiField, Field playerField)
		{
			this.aiShips = aiShips;
			this.playerShips = playerShips;
			this.aiField = aiField;
			this.playerField = playerField;
			aiPlacedShips = new Ship[0];
		}

		/// <summary>
		/// Автоматическая установка кораблей AI
		/// </summary>
		public void PlaceShips()
		{
			Random rand = new Random();
			int CurAiShipIndex = 0;
			//Размещаем корабли, пока индекс текущего корабля не станет больше
			//общего количества кораблей
			while (CurAiShipIndex < aiShips.Length)
			{
				//Находим положение будущей первой ячейки корабля
				int startCell = rand.Next(100);
				//Определяем, будет ли корабль вертикальным
				bool vertical = false;
				if (rand.Next(2) == 1)
					vertical = true;
				//Перемещаем корабль на новую случайную стартовую точку
				Ship newShip = ChangeAiShipLocation(aiShips[CurAiShipIndex], aiField.cells[startCell], vertical);
				if (newShip!=null)
				{
					//Проверка, чтобы новый корабль не был близко к другим уже установленным
					if (!Algoritms.FindNearShips(newShip, aiPlacedShips))
					{
						//Добавляем текущий корабль в массив уже установленных кораблей
						aiPlacedShips = Algoritms.AddElToArray(aiPlacedShips, newShip);
						CurAiShipIndex++;
					}
				}
			}
		}

		/// <summary>
		/// Перемещает корабль AI в указанную ячейку
		/// </summary>
		/// <param name="ship">Корабль</param>
		/// <param name="startCell">Начальная ячейка для корабля</param>
		/// <param name="IsVertical">Вертикальный корабль?</param>
		/// <returns>Перемещенный корабль </returns>
		Ship ChangeAiShipLocation(Ship ship, Cell startCell, bool IsVertical)
		{
			//Проверяем, чтобы корабль не выходил за пределы поля
			for (int x=1; x< ship.cells.Length;x++)
			{
				if (IsVertical)
				{
					if (startCell.Y + x > 10)
						return null;
				}
				else
					if (startCell.X + x > 10)
						return null;
			}
			//Создаем будущие ячейки перемещенного корабля
			Cell[] tmp = new Cell[ship.cells.Length];
			for (int x = 0; x < ship.cells.Length; x++)
			{
				//Инициализация ячеек в зависимости от вертикальности или горизонтальности размещения
				if (IsVertical)
				{
					tmp[x] = Algoritms.FindCell(startCell.X, startCell.Y + x, aiField.cells);
				}
				else
				{
					tmp[x] = Algoritms.FindCell(startCell.X + x, startCell.Y, aiField.cells);
				}
			}
			Ship newShip = new Ship(tmp);
			return newShip;
		}

		/// <summary>
		/// Выстрел по кораблям игрока
		/// </summary>
		/// <returns>Результат выстрела</returns>
		public Target Fire()
		{
			Random rand = new Random();
			//Если есть последний поврежденный корабль
			if (curDamagedPlayerShip != null)
			{
				//Просмотр количества поврежденных ячеек корабля
				int damagedCount = 0;
				for (int x = 0; x < curDamagedPlayerShip.cells.Length; x++)
				{
					if (curDamagedPlayerShip.cells[x].fired)
					{
						damagedCount++;
					}
				}
				//Если было только одно попадание по кораблю
				if (damagedCount == 1)
				{
					int newShot = 0;
					//Обстреливаем вокруг предыдущего попадания
					while(true)
					{
						newShot = rand.Next(1,5);
						Target targ = Shot(newShot, lastDamagedCell);
						if (targ != Target.Not_Valid_Target)
							return targ;
					}
				}
				else
				{
					//Если корабль вертикальный
					if (curDamagedPlayerShip.IsVertical)
					{
						//Проходим по ячейкам  корабля и находим поврежденные
						for (int x = 0; x < curDamagedPlayerShip.cells.Length; x++)
						{
							if (curDamagedPlayerShip.cells[x].fired)
							{
								//Начинаем обстреливать выше и ниже предыдущего попадания
								Target targ = Shot(1, curDamagedPlayerShip.cells[x]);
								if (targ != Target.Not_Valid_Target)
								{
									return targ;
								}
								else
								{
									targ = Shot(3, curDamagedPlayerShip.cells[x]);
									if (targ != Target.Not_Valid_Target)
										return targ;
								}
							}
						}
					}
					else
					{
						//Проходим по ячейкам  корабля и находим поврежденные
						for (int x = 0; x < curDamagedPlayerShip.cells.Length; x++)
						{
							if (curDamagedPlayerShip.cells[x].fired)
							{
								//Начинаем обстреливать левее и правее предыдущего попадания
								Target targ = Shot(4, curDamagedPlayerShip.cells[x]);
								if (targ != Target.Not_Valid_Target)
								{
									return targ;
								}
								else
								{
									targ = Shot(2, curDamagedPlayerShip.cells[x]);
									if (targ != Target.Not_Valid_Target)
										return targ;
								}
							}
						}
					}
				}
			}
			//Начинаем случайный обстрел игрового поля игрока
			while(true)
			{
				int newShot = rand.Next(100);
				Cell target = playerField.cells[newShot];
				//Если есть поблизости уничтоженные ранее корабл, то сменить цель
				if (NearDestroyedShip(playerShips, target))
					continue;
				//Выстрел
				Target result = ShotInCell(target);
				if (result != Target.Not_Valid_Target)
					return result;
				else
					continue;
			}
		}

		/// <summary>
		/// Найти рядом уже уничтоженный корабль
		/// </summary>
		/// <param name="allShips">Корабли</param>
		/// <param name="target">Текущая ячейка</param>
		/// <returns>Найден?</returns>
		bool NearDestroyedShip(Ship[] allShips, Cell target)
		{
			for (int x = 0; x < allShips.Length; x++)
			{
				Ship ship = Algoritms.FindNearShips(target, allShips);
				if (ship!=null)
					return ship.Destroyed;
			}
			return false;
		}

		/// <summary>
		/// Выстрел относительно предыдущего попадания
		/// </summary>
		/// <param name="newShot">1 - выше старого попадания, 2 - правее, 3 - ниже, 4 - левее</param>
		/// <param name="lastDamagedCell">Ячейка предыдущего попадания</param>
		/// <returns>Результат попадания</returns>
		Target Shot(int newShot,Cell lastDamagedCell)
		{
			if (newShot < 1 || newShot > 4)
				return Target.Not_Valid_Target;
			switch (newShot)
			{
				case 1:
					//Ищем на поле ячейку выше предыдущего попадания
					Cell target = Algoritms.FindCell(lastDamagedCell.X, lastDamagedCell.Y-1, playerField.cells);
					return ShotInCell(target);

				case 2:
					//Ищем на поле ячейку правее предыдущего попадания
					target = Algoritms.FindCell(lastDamagedCell.X + 1, lastDamagedCell.Y, playerField.cells);
					return ShotInCell(target);
				case 3:
					//Ищем на поле ячейку ниже предыдущего попадания
					target = Algoritms.FindCell(lastDamagedCell.X, lastDamagedCell.Y + 1, playerField.cells);
					return ShotInCell(target);
				case 4:
					//Ищем на поле ячейку левее предыдущего попадания
					target = Algoritms.FindCell(lastDamagedCell.X - 1, lastDamagedCell.Y, playerField.cells);
					return ShotInCell(target);
			}
			return Target.Not_Valid_Target;
		}

		/// <summary>
		/// Выстрел по конкретной ячейке
		/// </summary>
		/// <param name="target">Цель</param>
		/// <returns>Результат попадания</returns>
		private Target ShotInCell(Cell target)
		{
			//Если такая ячейка есть и по ней никто не стрелял
			if (target != null && !target.fired && !NearDestroyedShip(playerShips,target))
			{
				target.fired = true;
				//Ищем корабль на ячейке
				Ship tmpShip = Algoritms.FindShip(target, playerShips);
				if (tmpShip == null)
				{
					//Корабль не найден, значит промазали
					return Target.Missed;
				}
				//Проверяем, не убит ли корабль
				if (Algoritms.DestroyedShip(tmpShip))
				{
					lastDamagedCell = null;
					curDamagedPlayerShip = null;
					//Если корабль убит, смотрем не победил ли компьютер
					if (Algoritms.IsVictory(playerShips))
					{
						return Target.Victory;
					}
					return Target.Destroyed;
				}
				else
				{
					//Обозначаем подбитую ячейку
					lastDamagedCell = target;
					curDamagedPlayerShip = tmpShip;
					return Target.Hit;
				}
			}
			else
				return Target.Not_Valid_Target;
		}

	}
}
