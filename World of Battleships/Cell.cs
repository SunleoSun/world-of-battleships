using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace World_of_Battleships
{
	/// <summary>
	/// Класс, представляющий ячейку игрового поля
	/// </summary>
	public class Cell
	{
		int x;
		/// <summary>
		/// Координата X ячейки
		/// </summary>
		public int X
		{
			get { return x; }
			set 
			{ 
				//Координата ячейки не должна выходить за пределы поля
				if (value>10 || value <1)
				{
					return;
				}
				else
					x = value; 
			}
		}

		int y;
		/// <summary>
		/// Координата Y ячейки
		/// </summary>
		public int Y
		{
			get { return y; }
			set
			{
				//Координата ячейки не должна выходить за пределы поля
				if (value > 10 || value < 1)
				{
					return;
				}
				else
					y = value;
			}
		}

		/// <summary>
		/// Произведен ли выстрел по этой ячейке
		/// </summary>
		public bool fired = false;

		//Конструкторы
		public Cell() { }

		public Cell(int x, int y)
		{
			X = x;
			Y = y;
		}
	}
}
