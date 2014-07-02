using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace World_of_Battleships
{
	/// <summary>
	/// Виды передвижений кораблей
	/// </summary>
	public enum _Move { Left, Right, Up, Down, Rotate }
	/// <summary>
	/// Результаты выстрелов
	/// </summary>
	public enum Target { Destroyed, Hit, Missed, Not_Valid_Target, Victory}

	/// <summary>
	/// Класс, реализующий логику игры
	/// </summary>
	public class Engine
	{
		/// <summary>
		/// Заготовки коралей игрока
		/// </summary>
		Ship[] playerShips;
		/// <summary>
		/// Установленые корабли игрока
		/// </summary>
		Ship[] playerPlacedShips;
		/// <summary>
		/// Корабли компьютера
		/// </summary>
		Ship[] aiShips;
		/// <summary>
		/// Поле игрока
		/// </summary>
		Field playerField;
		/// <summary>
		/// Поле компьютера
		/// </summary>
		Field aiField;
		/// <summary>
		/// Экземпляр AI
		/// </summary>
		AI ai;
		/// <summary>
		/// Индекс текущего корабля
		/// </summary>
		int CurShipIndex;
		/// <summary>
		/// Текущая цель игрока
		/// </summary>
		Cell target;
		/// <summary>
		/// Получить поле игрока
		/// </summary>
		public Field GetPlayerField { get { return playerField; } }
		/// <summary>
		/// Получить поле компьютера
		/// </summary>
		public Field GetAiField { get { return aiField; } }
		/// <summary>
		/// Получить корабли игрока
		/// </summary>
		public Ship[] GetPlayerShips { get { return playerPlacedShips; } }
		/// <summary>
		/// Получить корабли компьютера
		/// </summary>
		public Ship[] GetAiShips { 
			get 
			{
				if (ai!=null)
				{
					return ai.GetPlacedShips;
				}
				return null;
			}
		}
		/// <summary>
		/// Получить экземпляр AI
		/// </summary>
		public AI GetAI	{	get{	return ai;	}	}

		/// <summary>
		/// Получить текущий корабль игрока
		/// </summary>
		public Ship GetCurShip { 
			get 
			{
				if (CurShipIndex < 10)
				{
					return playerShips[CurShipIndex];
				}
				else
					return null;
			}
		 } 
		/// <summary>
		/// Получить текущую цель игрока
		/// </summary>
		public Cell GetCurTarget { get { return target; } }

		public Engine()
		{
			InitAllObjects();
		}

		/// <summary>
		/// Инициализация всех логических компонентов
		/// </summary>
		private void InitAllObjects()
		{
			//Инициализация игрового поля игрока
			playerField = new Field();
			//Инициализация игрового поля компьютера
			aiField = new Field();
			//Инициализация цели
			target = new Cell(1, 1);
			//Индекс текущего корабля
			CurShipIndex = 0;

			#region Инициализация кораблей
			playerShips = new Ship[10];
			playerPlacedShips = new Ship[0];
			aiShips = new Ship[10];
			Cell c1 = playerField.cells[0];
			Cell c2 = playerField.cells[10];
			Cell c3 = playerField.cells[20];
			Cell c4 = playerField.cells[30];

			Cell[] cells = new Cell[4] { c1, c2, c3, c4 };
			playerShips[0] = new Ship(cells);
			aiShips[0] = new Ship(cells);

			for (int x = 1; x < 4; x++)
			{
				cells = new Cell[3] { c1, c2, c3 };
				playerShips[x] = new Ship(cells);
				aiShips[x] = new Ship(cells);
			}

			for (int x = 4; x < 6; x++)
			{
				cells = new Cell[2] { c1, c2 };
				playerShips[x] = new Ship(cells);
				aiShips[x] = new Ship(cells);
			}

			for (int x = 6; x < 10; x++)
			{
				cells = new Cell[1] { c1 };
				playerShips[x] = new Ship(cells);
				aiShips[x] = new Ship(cells);
			}
			#endregion

		}

		/// <summary>
		/// Переинициализация всех логических компонентов
		/// </summary>
		public void RestartGame()
		{
			InitAllObjects();
		}

		/// <summary>
		/// Установить текущий корабль
		/// </summary>
		public bool PlaceShip()
		{
			//Проверка о наличии поблизости кораблей, которые были уже установлены
			if (Algoritms.FindNearShips(playerShips[CurShipIndex], playerPlacedShips))
				return false;
			//Добавляем к массиву установленных кораблей текущий корабль
			playerPlacedShips = Algoritms.AddElToArray(playerPlacedShips, playerShips[CurShipIndex]);
			//Увеличение индекса текущего корабля, и если был последний корабль, то начало
			//размещение кораблей AI
			CurShipIndex++;
			if (CurShipIndex>9)
			{
				//Инициализация AI
				ai = new AI(aiShips, playerPlacedShips, aiField, playerField);
				//Расстановка кораблей AI
				ai.PlaceShips();
			}
			return true;
		}

		/// <summary>
		/// Передвинуть цель грока
		/// </summary>
		/// <param name="move">Тип сдвига</param>
		/// <returns>Новая цель</returns>
		public Cell MoveTarget(_Move move)
		{
			if (move == _Move.Left)
			{
				target.X--;
			}
			if (move == _Move.Right)
			{
				target.X++;
			}
			if (move == _Move.Up)
			{
				target.Y--;
			}
			if (move == _Move.Down)
			{
				target.Y++;
			}
			return target;
		}
		/// <summary>
		/// Переместить корабль
		/// </summary>
		/// <param name="move">Тип переноса</param>
		/// <returns>Корабль в новом положении</returns>
		public Ship MoveShip(_Move move)
		{
			Ship ship = playerShips[CurShipIndex];
			//Сдвиг влево
			if (move == _Move.Left)
			{
				//Если одна из ячеек выходит за пределы поля после сдвига
				for (int x = 0; x < ship.cells.Length; x++)
					if (ship.cells[x].X - 1 < 1)
						return ship;		
				//Сдвиг
				for (int x = 0; x < ship.cells.Length; x++)
				{
					ship.cells[x] = Algoritms.FindCell(ship.cells[x].X - 1, ship.cells[x].Y,playerField.cells);
				}
			}

			//Сдвиг вправо
			if (move == _Move.Right)
			{
				//Если одна из ячеек выходит за пределы поля после сдвига
				for (int x = 0; x < ship.cells.Length; x++)
					if (ship.cells[x].X + 1 > 10)
						return ship;
				//Сдвиг
				for (int x = 0; x < ship.cells.Length; x++)
					ship.cells[x] = Algoritms.FindCell(ship.cells[x].X + 1, ship.cells[x].Y, playerField.cells);
			}

			//Сдвиг вверх
			if (move == _Move.Up)
			{
				//Если одна из ячеек выходит за пределы поля после сдвига
				for (int x = 0; x < ship.cells.Length; x++)
					if (ship.cells[x].Y - 1 < 1)
						return ship;
				//Сдвиг
				for (int x = 0; x < ship.cells.Length; x++)
					ship.cells[x] = Algoritms.FindCell(ship.cells[x].X, ship.cells[x].Y-1, playerField.cells);
			}

			//Сдвиг вниз
			if (move == _Move.Down)
			{
				//Если одна из ячеек выходит за пределы поля после сдвига
				for (int x = 0; x < ship.cells.Length; x++)
					if (ship.cells[x].Y + 1 > 10)
						return ship;
				//Сдвиг
				for (int x = 0; x < ship.cells.Length; x++)
					ship.cells[x] = Algoritms.FindCell(ship.cells[x].X, ship.cells[x].Y+1, playerField.cells);
			}

			//Поворот корабля
			if (move == _Move.Rotate)
			{
				if (ship.IsVertical)
				{
					//Если одна из ячеек выходит за пределы поля после поворота
					for (int x = 1; x < ship.cells.Length; x++)
						if (ship.cells[0].X + x > 10)
							return ship;
					//Поворот
					for (int x = 1; x < ship.cells.Length; x++)
					{
						ship.cells[x] = Algoritms.FindCell(ship.cells[0].X + x, ship.cells[0].Y, playerField.cells); 
					}
				}
				else
				{
					//Если одна из ячеек выходит за пределы поля после поворота
					for (int y = 1; y < ship.cells.Length; y++)
						if (ship.cells[0].Y + y > 10)
							return ship;
					//Поворот
					for (int y = 1; y < ship.cells.Length; y++)
					{
						ship.cells[y] = Algoritms.FindCell(ship.cells[0].X, ship.cells[0].Y + y, playerField.cells);
					}

				}
			}
			return ship;
		}

		/// <summary>
		/// Отмена установки предыдущего корабля
		/// </summary>
		public void CancelPlaceShip()
		{
			if (playerPlacedShips.Length<1)
				return;
			//Уменьщаем массив установленных кораблей на 1
			Ship[] tmp = new Ship[playerPlacedShips.Length - 1];
			for (int x = 0; x < tmp.Length; x++)
			{
				tmp[x] = playerPlacedShips[x];
			}
			playerPlacedShips = tmp;
			//Уменьшаем  индекс текущего корабля
			CurShipIndex--;
		}
		/// <summary>
		/// Выстрел по кораблю AI
		/// </summary>
		/// <param name="cell">Цель</param>
		/// <returns>Результат выстрела</returns>
		public Target Fire(Cell cell)
		{
			Cell fieldAiCell = Algoritms.FindCell(cell.X, cell.Y, aiField.cells);
			Target target;
			if (fieldAiCell.fired)
				return Target.Not_Valid_Target;
			fieldAiCell.fired = true;
			//Если попали во вражеский корабль
			Ship ship = Algoritms.FindShip(fieldAiCell, ai.GetPlacedShips);
			if (ship != null)
			{
				//Если корабль уничтожен
				if (Algoritms.DestroyedShip(ship))
					//Если победа
					if (Algoritms.IsVictory(ai.GetPlacedShips))
						target = Target.Victory;
					else
						target = Target.Destroyed;
				else
					target = Target.Hit;
			}
			else
				target = Target.Missed;
			return target;
		}
	}
}
