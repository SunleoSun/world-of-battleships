using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace World_of_Battleships
{
	/// <summary>
	/// Класс, представляющий игровое поле
	/// </summary>
	public class Field
	{
		/// <summary>
		/// Ячейки поля
		/// </summary>
		public Cell[] cells = new Cell[100];

		public Field()
		{
			int counter = 0;

			//Инициализация ячеек поля
			for (int y = 1; y < 11; y++)
			{
				for (int x=1; x< 11;x++)
				{
					cells[counter] = new Cell(x, y);
					counter++;
				}
			}
		}
	}
}
