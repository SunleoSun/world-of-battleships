using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Media;
namespace World_of_Battleships
{
	/// <summary>
	/// Класс для оповещения пользователя о событиях игры
	/// </summary>
	public class UserNotificator
	{
		PictureBox info;
		SoundPlayer sound;
		Random rand;
		SoundPlayer music;
		SystemSounds ss;
		public UserNotificator(PictureBox info)
		{
			this.info = info;
			info.SizeMode = PictureBoxSizeMode.StretchImage;
			sound = new SoundPlayer();
			rand = new Random();
			music = new SoundPlayer();
			music.Stream = World_of_Battleships.Properties.Resources.Sea;
			music.LoadAsync();
			music.PlayLooping();
		}
		public void ShipPlaced()
		{
			sound.Stream = World_of_Battleships.Properties.Resources.PlaceShip;
			sound.LoadAsync();
			sound.Play();
		}

		public void StartBattle()
		{
			sound.Stream = World_of_Battleships.Properties.Resources.StartBattle;
			sound.LoadAsync();
			sound.Play();
		}
		/// <summary>
		/// Оповещение о попадании игроком
		/// </summary>
		public void HitPlayer()
		{
			info.Image = World_of_Battleships.Properties.Resources.HitInfo;
			int randNumber = rand.Next(1,5);
			switch (randNumber)
			{
				case 1:
					sound.Stream = World_of_Battleships.Properties.Resources.Hitted1;
				break;
				case 2:
					sound.Stream = World_of_Battleships.Properties.Resources.Hitted2;
				break;
				case 3:
					sound.Stream = World_of_Battleships.Properties.Resources.Hitted3;
				break;
				case 4:
					sound.Stream = World_of_Battleships.Properties.Resources.Hitted4;
				break;
			}
			sound.Load();
			sound.Play();
		}
		/// <summary>
		/// Оповещение о промахе игрока
		/// </summary>
		public void MissPlayer()
		{
			info.Image = World_of_Battleships.Properties.Resources.MissInfo;
			int randNumber = rand.Next(1, 3);
			switch (randNumber)
			{
				case 1:
					sound.Stream = World_of_Battleships.Properties.Resources.Missed1;
					break;
				case 2:
					sound.Stream = World_of_Battleships.Properties.Resources.Missed2;
					break;
			}
			sound.Load();
			sound.Play();

		}
		/// <summary>
		/// Оповещение о уничтожении корабля компьютера
		/// </summary>
		public void DestroyPlayer()
		{
			info.Image = World_of_Battleships.Properties.Resources.DestroyInfo;
			int randNumber = rand.Next(1, 4);
			switch (randNumber)
			{
				case 1:
					sound.Stream = World_of_Battleships.Properties.Resources.Destroy1;
					break;
				case 2:
					sound.Stream = World_of_Battleships.Properties.Resources.Destroy2;
					break;
				case 3:
					sound.Stream = World_of_Battleships.Properties.Resources.Destroy3;
					break;
			}
			sound.Load();
			sound.Play();

		}
		/// <summary>
		/// Оповещение о не верной цели
		/// </summary>
		public void InvalidTarget()
		{
			info.Image = World_of_Battleships.Properties.Resources.InvalidTargetInfo;
			sound.Stream = World_of_Battleships.Properties.Resources.Error;
			sound.Load();
			sound.Play();
		}
		/// <summary>
		/// Оповещение о неверной позиции размещения корабля
		/// </summary>
		public void InvalidPlaceShip()
		{
			info.Image = World_of_Battleships.Properties.Resources.InvalidShipPlaceInfo;
			sound.Stream = World_of_Battleships.Properties.Resources.Error;
			sound.LoadAsync();
			sound.Play();
		}
		/// <summary>
		/// Оповещение о победе игрока
		/// </summary>
		public void VictoryPlayer()
		{
			music.Stop();
			info.Image = World_of_Battleships.Properties.Resources.VictoryInfo;
			sound.Stream = World_of_Battleships.Properties.Resources.Victory;
			sound.Load();
			sound.Play();
		}
		/// <summary>
		/// Оповещение о проигрыше игрока
		/// </summary>
		public void Defeat()
		{
			music.Stop();
			info.Image = World_of_Battleships.Properties.Resources.DefeatedInfo;
			sound.Stream = World_of_Battleships.Properties.Resources.Defeat;
			sound.Load();
			sound.Play();
		}
		/// <summary>
		/// Очистить старое оповещение
		/// </summary>
		public void ClearPicture()
		{
			info.Image = null;
		}
	}
}
