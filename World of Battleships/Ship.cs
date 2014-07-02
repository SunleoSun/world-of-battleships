using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace World_of_Battleships
{
	/// <summary>
	/// Класс, представляющий корабль
	/// </summary>
	public class Ship
	{
		/// <summary>
		/// Ячейки корабля
		/// </summary>
		public Cell[] cells;

		public Ship(Cell[] cells)
		{
			this.cells = cells;
		}

		/// <summary>
		/// Является ли корабль уничтоженным
		/// </summary>
		public bool Destroyed { get; set; }

		/// <summary>
		/// Яаляется ли корабль вертикальным
		/// </summary>
		public bool IsVertical
		{
			get 
			{
				if (cells.Length > 1)
				{
					//Если координаты двух ячеек имеют одинаковые координаты по Х,
					//то корабль вертикальный
					if (cells[0].X == cells[1].X)
						return true;
					else
						return false;
				}
				else
					return false;
			}
		}
	}
}