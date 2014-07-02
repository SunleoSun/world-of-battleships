using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace World_of_Battleships
{
	static class Algoritms
	{
		/// <summary>
		/// Определить, соприкасается ли корабли с текущим
		/// </summary>
		/// <param name="curent">Текущий корабль</param>
		/// <param name="allShips">Список проверяемых кораблей</param>
		/// <returns>Соприкасаются?</returns>
		public static bool FindNearShips(Ship curent, Ship[] allShips)
		{
			//Цикл по проматриваемым кораблям
			for (int y = 0; y < allShips.Length; y++)
				//Цикл по ячейкам проматриваемых кораблей
				for (int z = 0; z < allShips[y].cells.Length; z++)
					//Цикл по ячейкам текущего корабля
					for (int x = 0; x < curent.cells.Length; x++)
						//Если ячейки рядом
						if (CellsAreNear(allShips[y].cells[z], curent.cells[x]))
							return true;
			return false;
		}

		public static Ship FindNearShips(Cell curent, Ship[] allShips)
		{
			//Цикл по проматриваемым кораблям
			for (int y = 0; y < allShips.Length; y++)
				//Цикл по ячейкам проматриваемых кораблей
				for (int z = 0; z < allShips[y].cells.Length; z++)
					//Если ячейки рядом
					if (CellsAreNear(allShips[y].cells[z], curent))
						return allShips[y];
			return null;
		}

		/// <summary>
		/// Определяет, находятся ячейки слишком близко друг к другу или нет
		/// </summary>
		/// <param name="c1">Ячейка 1</param>
		/// <param name="c2">Ячейка 2</param>
		public static bool CellsAreNear(Cell c1, Cell c2)
		{
			if (1 >= Math.Abs(c1.X - c2.X)&&
				1 >= Math.Abs(c1.Y - c2.Y))
			{
				return true;
			}
			else
				return false;
		}

		/// <summary>
		/// Найти корабль по ячейке
		/// </summary>
		/// <param name="cell">Ячейка корабля</param>
		/// <param name="allShips">Все корабли</param>
		/// <returns>Найденный корабль или null</returns>
		public static Ship FindShip(Cell cell, Ship[] allShips)
		{
			for (int x=0; x< allShips.Length;x++)
			{
				for (int y=0; y< allShips[x].cells.Length;y++)
				{
					//Если ячейка совпадает с ячейкой корабля, то возрат корабля
					if (cell == allShips[x].cells[y])
					{
						return allShips[x];
					}
				}
			}
			return null;
		}

		/// <summary>
		/// Найти ячейку по координатам
		/// </summary>
		/// <param name="x">координата Х</param>
		/// <param name="y">координата У</param>
		/// <param name="allCells">Ячейки, в которых ведется поиск</param>
		/// <returns>Найденная ячейка или null</returns>
		public static Cell FindCell(int x, int y, Cell[] allCells)
		{
			for (int z=0; z< allCells.Length;z++)
			{
				//Если координаты совпадают
				if (allCells[z].X == x && allCells[z].Y == y)
				{
					return allCells[z];
				}
			}
			return null;
		}

		/// <summary>
		/// Определить, уничтожен корабль или нет. Если да, тогда присвоить ему статус уничтоженного.
		/// </summary>
		/// <param name="ship">Корабль</param>
		/// <returns>Уничтожен корабль или нет</returns>
		public static bool DestroyedShip(Ship ship)
		{
			int damageCount = 0;
			//Подсчет поврежденных ячеек корабля
			for (int x=0; x< ship.cells.Length;x++)
			{
				if (ship.cells[x].fired)
				{
					damageCount++;
				}
			}
			//Если все ячейки подбиты, то корабль уничтожен
			if (damageCount == ship.cells.Length)
			{
				ship.Destroyed = true;
				return true;	
			}
			return false;
		}

		/// <summary>
		/// Увеличение массива кораблей на один эллемент
		/// </summary>
		/// <param name="array">Массив</param>
		/// <param name="elem">Эллемент для вставки</param>
		public static Ship[] AddElToArray(Ship[] array, Ship elem)
		{
			Ship[] tmp = new Ship[array.Length + 1];
			for (int x = 0; x < array.Length; x++)
			{
				tmp[x] = array[x];
			}
			tmp[tmp.Length - 1] = elem;
			return tmp;
		}

		/// <summary>
		/// Определить победу
		/// </summary>
		/// <param name="enemyShips">Корабли противника</param>
		/// <returns>Победа?</returns>
		public static bool IsVictory(Ship[] enemyShips)
		{
			int couDestroyedShips = 0;
			//Подсчет уничтоженных кораблей
			for (int x = 0; x < enemyShips.Length; x++)
			{
				if (enemyShips[x].Destroyed)
				{
					couDestroyedShips++;
				}
			}
			//Если все уничтожены
			if (couDestroyedShips == enemyShips.Length)
			{
				return true;
			}
			return false;
		}

	}
}
