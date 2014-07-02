using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Threading;

namespace World_of_Battleships
{
	/// <summary>
	/// Интерфейс игры
	/// </summary>
	public partial class GUI : Form
	{
		#region Константы для рисования
		/// <summary>
		/// Ширина отступа слева до первого поля
		/// </summary>
		const int WidthIntend = 40;
		/// <summary>
		/// Высота отступа сверху до полей
		/// </summary>
		const int HeightIntend = 60;
		/// <summary>
		/// Ширина полей
		/// </summary>
		const int WidthFields = 350;
		/// <summary>
		/// Высота полей
		/// </summary>
		const int HeightFields = 350;
		/// <summary>
		/// Ширина отступа между полями
		/// </summary>
		const int IntendBetweenFields = 100;
		/// <summary>
		/// Ширина одной ячейки поля
		/// </summary>
		const int widthOneCell = WidthFields / 10;
		/// <summary>
		/// Высота одной ячейки поля
		/// </summary>
		const int heightOneCell = HeightFields / 10;
		/// <summary>
		/// Высота шрифта пометок полей
		/// </summary>
		const float labelSize = 12;
		/// <summary>
		/// Цвет сетки полей
		/// </summary>
		Brush brushFields = Brushes.Yellow;
		/// <summary>
		/// Цвет шрифта пометок
		/// </summary>
		Brush labelBrush = Brushes.Red;

		#endregion

		#region Переменные графики и двойной буферизации
		Graphics realGraphics;
		Graphics bufferGraphics;
		BufferedGraphicsContext bgc;
		BufferedGraphics bg;
		#endregion

		/// <summary>
		/// Экземпляр логического движка
		/// </summary>
		Engine engine;

		/// <summary>
		/// Индекс текущего устанавливаемого корабля
		/// </summary>
		int CurPlacedShipIndex = 0;
		/// <summary>
		/// Шрифт меток
		/// </summary>
		Font labelFont;
		/// <summary>
		/// Игра началась?
		/// </summary>
		bool gameStarted = false;
		/// <summary>
		/// Был нажат пробел
		/// </summary>
		bool spacePressed = false;
		/// <summary>
		/// Был нажат шифт
		/// </summary>
		bool shiftPressed = false;
		/// <summary>
		/// Кто-то победил?
		/// </summary>
		bool victory = false;
		bool battleStarted = false;
		/// <summary>
		/// Экземпляр класса оповещений
		/// </summary>
		UserNotificator userNotificator;

		public GUI()
		{
			InitializeComponent();
			//Инициализация двойной буферизации
			realGraphics = this.CreateGraphics();
			bgc = BufferedGraphicsManager.Current;
			bg = bgc.Allocate(realGraphics, new Rectangle(0, 0, this.Width, this.Height));
			bufferGraphics = bg.Graphics;
			//Инициализация движка
			engine = new Engine();
			//Инициализация шрифта
			labelFont = new Font(new FontFamily("Times New Roman"), labelSize);
			//Инициализация класса оповещений
			userNotificator = new UserNotificator(this.fpbInfo);
		}

		/// <summary>
		/// Обновить всю графику
		/// </summary>
		void Reflesh()
		{
			DrawBackground();
			//Отрисовать сетку поля
			DrawFields(bufferGraphics, new Rectangle(WidthIntend, HeightIntend, WidthFields, HeightFields), new Rectangle(WidthIntend + WidthFields + IntendBetweenFields, HeightIntend, WidthFields, HeightFields));
			//Отрисовать корабли игрока
			DrawPlayerShips();
			//Отрисовать корабли AI если игра окончена
			DrawAiShips();
			//Отрисовать ячейки
			DrawCells(bufferGraphics);
			//Отрисовать цель
			DrawTarget(engine.GetCurTarget, bufferGraphics);
			//Нарисовать метки полей
			DrawLabels(bufferGraphics);
		}
		/// <summary>
		/// Нарисовать фон
		/// </summary>
		private void DrawBackground()
		{
			var img = World_of_Battleships.Properties.Resources.Background;
			bufferGraphics.DrawImage(img, 0, 0, this.Width, this.Height);
		}

		/// <summary>
		/// Нарисовать метки полей
		/// </summary>
		/// <param name="graphics"></param>
		void DrawLabels(Graphics graphics)
		{
			//Рисуем буквенные обозначения по горизонтали
			char ch = 'а';
			for (int x=0; x< 10;x++)
			{
				//Пропускаем "Й"
				if (x==9)
					ch++;
				graphics.DrawString(ch.ToString(), labelFont, labelBrush, WidthIntend + widthOneCell * x + widthOneCell/2-labelFont.SizeInPoints, HeightIntend - labelFont.GetHeight());
				graphics.DrawString(ch.ToString(), labelFont, labelBrush, WidthIntend + WidthFields + IntendBetweenFields + widthOneCell * x + widthOneCell / 2 - labelFont.SizeInPoints, HeightIntend - labelFont.GetHeight());
				ch++;
			}
			//Рисуем цифровые обозначения по горизонтали
			for (int y=0; y< 10;y++)
			{
				graphics.DrawString((y+1).ToString(), labelFont, labelBrush, WidthIntend  - labelFont.SizeInPoints*2, HeightIntend + heightOneCell/3  + heightOneCell*y);
				graphics.DrawString((y + 1).ToString(), labelFont, labelBrush, WidthIntend + WidthFields + IntendBetweenFields - labelFont.SizeInPoints*2, HeightIntend + heightOneCell / 3 + heightOneCell * y);
			}
		}

		/// <summary>
		/// Нарисовать корабли игрока
		/// </summary>
		void DrawPlayerShips()
		{
			for (int x = 0; x < engine.GetPlayerShips.Length; x++)
			{
				DrawShip(engine.GetPlayerShips[x], bufferGraphics,true);
			}
		}

		/// <summary>
		/// Нарисовать корабли AI, если игра окончена
		/// </summary>
		void DrawAiShips()
		{
			if(victory)
				for (int x = 0; x < engine.GetAiShips.Length; x++)
				{
					DrawShip(engine.GetAiShips[x], bufferGraphics, false);
				}
		}
		/// <summary>
		/// Отрисовать ячейки
		/// </summary>
		/// <param name="graphics"></param>
		void DrawCells(Graphics graphics)
		{
			//Инициализация ресурсов
			var emptyWater = World_of_Battleships.Properties.Resources.EmptyWater;
			var hit = World_of_Battleships.Properties.Resources.Hit;
			var miss = World_of_Battleships.Properties.Resources.Miss;
			int counterCells = 0;
			//Ячейки игрока
			for (int y=0; y< 10;y++)
			{
				for (int x=0; x< 10;x++)
				{
					Cell curCell = engine.GetPlayerField.cells[counterCells];
					Ship curShip = Algoritms.FindShip(curCell,engine.GetPlayerShips);
					//Пустая клетка
					if (!curCell.fired && curShip == null)
					{
						graphics.DrawImage(emptyWater, WidthIntend + widthOneCell * x + 1, HeightIntend + heightOneCell * y + 1, widthOneCell-1, heightOneCell-1);
					}
					else
					//Попадание в корабль
					if (curCell.fired &&  curShip!= null)
					{
						graphics.DrawImage(hit, WidthIntend + widthOneCell * x + 1, HeightIntend + heightOneCell * y + 1, widthOneCell - 1, heightOneCell - 1);
					}
					else
					//Промах
					if (curCell.fired && curShip == null)
					{
						graphics.DrawImage(miss, WidthIntend + widthOneCell * x + 1, HeightIntend + heightOneCell * y + 1, widthOneCell - 1, heightOneCell - 1);
					}
					counterCells++;
				}
			}
			//Ячейки AI
			counterCells = 0;
			for (int y = 0; y < 10; y++)
			{
				for (int x = 0; x < 10; x++)
				{
					Cell curCell = engine.GetAiField.cells[counterCells];
					if (engine.GetAiShips == null)
					{
						//Пустая клетка
						graphics.DrawImage(emptyWater, WidthIntend + WidthFields + IntendBetweenFields + widthOneCell * x + 1, HeightIntend + heightOneCell * y + 1, widthOneCell - 1, heightOneCell - 1);
						continue;
					}
					Ship curShip = Algoritms.FindShip(curCell, engine.GetAiShips);
					//Пустая клетка
					if (!curCell.fired)
					{
						graphics.DrawImage(emptyWater, WidthIntend + WidthFields + IntendBetweenFields + widthOneCell * x + 1, HeightIntend + heightOneCell * y + 1, widthOneCell - 1, heightOneCell - 1);
					}
					//Попадание в корабль
					if (curCell.fired && curShip != null)
					{
						graphics.DrawImage(hit, WidthIntend + WidthFields + IntendBetweenFields + widthOneCell * x + 1, HeightIntend + heightOneCell * y + 1, widthOneCell - 1, heightOneCell - 1);
					}
					//Промах
					if (curCell.fired && curShip == null)
					{
						graphics.DrawImage(miss, WidthIntend + WidthFields + IntendBetweenFields + widthOneCell * x + 1, HeightIntend + heightOneCell * y + 1, widthOneCell - 1, heightOneCell - 1);
					}
					counterCells++;
				}
			}
		}

		/// <summary>
		/// Найти координаты ячейки на плоскости рисования
		/// </summary>
		/// <param name="cell">Ячейка</param>
		/// <param name="ItPlayerField">Поле игрока?</param>
		/// <returns>Точка на плоскости рисования (верхний левый угол ячейки)</returns>
		Point FindDrawCoord(Cell cell, bool ItPlayerField)
		{
			if (ItPlayerField)
				return new Point(WidthIntend + widthOneCell * (cell.X-1), HeightIntend + heightOneCell * (cell.Y-1));
			else
				return new Point(WidthIntend + WidthFields + IntendBetweenFields + widthOneCell * (cell.X-1), HeightIntend + heightOneCell * (cell.Y-1));
		}
		/// <summary>
		/// Отрисовать сетку полей
		/// </summary>
		/// <param name="graphics">Переменная графики</param>
		/// <param name="playerRect">Прямоугольник поля игрока</param>
		/// <param name="aiRect">Прямоугольник поля компьютера</param>
		void DrawFields(Graphics graphics, Rectangle playerRect, Rectangle aiRect)
		{
			//Поле игрока
			Rectangle[] cells = new Rectangle[20];
			//Горизонтальные прямоугольники
			for (int x=0; x< 10;x++)
			{
				cells[x] = new Rectangle(WidthIntend+widthOneCell * x, HeightIntend, widthOneCell, HeightFields);
			}
			//Вертикальные прямоугольники
			for (int x = 0; x < 10; x++)
			{
				cells[x + 10] = new Rectangle(WidthIntend, HeightIntend +heightOneCell * x, WidthFields, heightOneCell);
			}
			//Отрисовать прямоугольники
			graphics.DrawRectangles(new Pen(brushFields), cells);

			//Поле компьютера

			//Горизонтальные прямоугольники
			for (int x = 0; x < 10; x++)
			{
				cells[x] = new Rectangle(WidthIntend + WidthFields + IntendBetweenFields + widthOneCell * x, HeightIntend, widthOneCell, HeightFields);
			}
			//Вертикальные прямоугольники
			for (int x = 0; x < 10; x++)
			{
				cells[x + 10] = new Rectangle(WidthIntend + WidthFields + IntendBetweenFields, HeightIntend + heightOneCell * x, WidthFields, heightOneCell);
			}
			//Отрисовать прямоугольники
			graphics.DrawRectangles(new Pen(brushFields), cells);

		}

		/// <summary>
		/// Отрисовать цель
		/// </summary>
		/// <param name="target">Ячейка цели</param>
		/// <param name="graphics">Переменная графики</param>
		void DrawTarget(Cell target, Graphics graphics)
		{
			//Инициализация ресурса
			var targetImg = World_of_Battleships.Properties.Resources.Target;
			//Ищем координаты ячейки
			Point position = FindDrawCoord(target, false);
			//Рисуем
			graphics.DrawImage(targetImg,position.X,position.Y,widthOneCell,heightOneCell);
		}

		/// <summary>
		/// Нарисовать корабль
		/// </summary>
		/// <param name="ship">Корабль</param>
		/// <param name="graphics">Переменная графики</param>
		void DrawShip(Ship ship, Graphics graphics,bool IsPlayerShip)
		{
			if (ship == null)
				return;
			var shipImg = World_of_Battleships.Properties.Resources.Ship;
			for (int x=0; x< ship.cells.Length;x++)
			{
				Point p = FindDrawCoord(ship.cells[x], IsPlayerShip);
				graphics.DrawImage(shipImg, p.X, p.Y, widthOneCell, heightOneCell);
			}
		}

		/// <summary>
		/// Обработчик нажатых клавиш
		/// </summary>
		private void GUI_KeyDown(object sender, KeyEventArgs e)
		{
			if (gameStarted)
			{
				Ship ship=null;
				//Right
				if (e.KeyValue == 39)
				{
					userNotificator.ClearPicture();
					shiftPressed = false;
					spacePressed = false;
					if (CurPlacedShipIndex<10)
						ship = engine.MoveShip(_Move.Right);
					else
						engine.MoveTarget(_Move.Right);
				}
				//Up
				if (e.KeyValue == 38)
				{
					userNotificator.ClearPicture();
					shiftPressed = false;
					spacePressed = false;
					if (CurPlacedShipIndex<10)
						ship = engine.MoveShip(_Move.Up);
					else
						engine.MoveTarget(_Move.Up);
				}
				//Left
				if (e.KeyValue == 37)
				{
					userNotificator.ClearPicture();
					shiftPressed = false;
					spacePressed = false;
					if (CurPlacedShipIndex<10)
						ship = engine.MoveShip(_Move.Left);
					else
						engine.MoveTarget(_Move.Left);
				}
				//Down
				if (e.KeyValue == 40)
				{
					userNotificator.ClearPicture();
					shiftPressed = false;
					spacePressed = false;
					if (CurPlacedShipIndex<10)
						ship = engine.MoveShip(_Move.Down);
					else
						engine.MoveTarget(_Move.Down);
				}
				//Rotate
				if (e.KeyValue == 16)
				{
					userNotificator.ClearPicture();
					spacePressed = false;
					if (CurPlacedShipIndex < 10)
						if (shiftPressed)
						{
							//При двойном нажатии клавиши Shift, 
							//отменяется предыдущий установленный корабль
							engine.CancelPlaceShip();
							shiftPressed = false;
							if (CurPlacedShipIndex!=0)
								CurPlacedShipIndex--;
						}
						else
						{
							shiftPressed = true;
							ship = engine.MoveShip(_Move.Rotate);
						}
				}
				//Space
				if (e.KeyValue == 32)
				{
					shiftPressed = false;
					//Установка корабля в заданные координаты
						if (spacePressed)
						{
							if (CurPlacedShipIndex<10)
								if (engine.PlaceShip())
								{
									userNotificator.ShipPlaced();
									if (CurPlacedShipIndex == 9)
									{
										userNotificator.StartBattle();
									}
									CurPlacedShipIndex++;
									ship = engine.GetCurShip;
								}
								else
								{
									//Корабль не ставится
									userNotificator.InvalidPlaceShip();
								}
							else
							//Огонь по цели
							{
								Target targ = engine.Fire(engine.GetCurTarget);
								//Оповещания  для игрока о результате выстрела
								if (targ == Target.Victory)
								{
									victory = true;
									gameStarted = false;
									userNotificator.VictoryPlayer();
								}
								else
									if (targ == Target.Hit)
										userNotificator.HitPlayer();
									else
										if (targ == Target.Missed)
											userNotificator.MissPlayer();
										else
											if (targ == Target.Destroyed)
												userNotificator.DestroyPlayer();
											else
												if (targ == Target.Not_Valid_Target)
													userNotificator.InvalidTarget();
								//Если цель игрока была правильной, ходит компьютер
								if (targ!= Target.Not_Valid_Target)
								{
									targ = engine.GetAI.Fire();
									if (targ == Target.Victory)
									{
										userNotificator.Defeat();
										victory = true;
										gameStarted = false;
									}
								}
							}
							spacePressed = false;
						}
						else
							spacePressed = true;

				}
				//Обновить изменения
				Reflesh();
				//Нарисовать текущий перемещаемый корабль
				DrawShip(engine.GetCurShip, bufferGraphics,true);
				//Отрисовать буфер
				bg.Render();
			}
		}

		/// <summary>
		/// Обработчик кнопки "Старт игры"
		/// </summary>
		private void fpStart_Click(object sender, EventArgs e)
		{
			//Убрать кнопки
			gameStarted = true;
			fpStart.Visible = false;
			fpControls.Visible = false;
			fpMenu.Visible = true;
			CurPlacedShipIndex = 0;
			//Инициализировать движок
			engine.RestartGame();
			//Отрисовать при помощи двойной буфферизации
			DrawBackground();

			//Обновить графику
			Reflesh();
			//Нарисовать текущий устанавливаемый корабль
			DrawShip(engine.GetCurShip, bufferGraphics, true);
			bg.Render();
		}

		/// <summary>
		/// Обработчик нажатия кнопки просмотра управления
		/// </summary>
		private void fpControls_Click(object sender, EventArgs e)
		{
			fpStart.Visible = false;
			fpControls.Visible = false;
			fpMenu.Visible = true;
			fpbControls.Visible = true;
		}

		/// <summary>
		/// Обработчик нажатия кнопки "Назад в меню"
		/// </summary>
		private void fpMenu_Click(object sender, EventArgs e)
		{
			gameStarted = false;
			victory = false;
			fpStart.Visible = true;
			fpControls.Visible = true;
			fpMenu.Visible = false;
			fpbControls.Visible = false;
			//Убераем оповещения и зарисовываем все под фон
			userNotificator.ClearPicture();
			DrawBackground();
			bg.Render();
		}


	}
}
