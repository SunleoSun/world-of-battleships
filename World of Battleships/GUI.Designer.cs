namespace World_of_Battleships
{
	partial class GUI
	{
		/// <summary>
		/// Требуется переменная конструктора.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Освободить все используемые ресурсы.
		/// </summary>
		/// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Код, автоматически созданный конструктором форм Windows

		/// <summary>
		/// Обязательный метод для поддержки конструктора - не изменяйте
		/// содержимое данного метода при помощи редактора кода.
		/// </summary>
		private void InitializeComponent()
		{
			this.fpbInfo = new System.Windows.Forms.PictureBox();
			this.fpMenu = new System.Windows.Forms.PictureBox();
			this.fpControls = new System.Windows.Forms.PictureBox();
			this.fpStart = new System.Windows.Forms.PictureBox();
			this.fpbControls = new System.Windows.Forms.PictureBox();
			((System.ComponentModel.ISupportInitialize)(this.fpbInfo)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.fpMenu)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.fpControls)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.fpStart)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.fpbControls)).BeginInit();
			this.SuspendLayout();
			// 
			// fpbInfo
			// 
			this.fpbInfo.BackColor = System.Drawing.Color.Transparent;
			this.fpbInfo.Location = new System.Drawing.Point(143, 447);
			this.fpbInfo.Name = "fpbInfo";
			this.fpbInfo.Size = new System.Drawing.Size(679, 39);
			this.fpbInfo.TabIndex = 6;
			this.fpbInfo.TabStop = false;
			// 
			// fpMenu
			// 
			this.fpMenu.BackColor = System.Drawing.Color.Transparent;
			this.fpMenu.Image = global::World_of_Battleships.Properties.Resources.BBackToMenuPassive;
			this.fpMenu.Location = new System.Drawing.Point(12, 447);
			this.fpMenu.Name = "fpMenu";
			this.fpMenu.Size = new System.Drawing.Size(125, 40);
			this.fpMenu.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.fpMenu.TabIndex = 5;
			this.fpMenu.TabStop = false;
			this.fpMenu.Visible = false;
			this.fpMenu.Click += new System.EventHandler(this.fpMenu_Click);
			// 
			// fpControls
			// 
			this.fpControls.BackColor = System.Drawing.Color.Transparent;
			this.fpControls.Image = global::World_of_Battleships.Properties.Resources.BControlsPassive;
			this.fpControls.Location = new System.Drawing.Point(350, 138);
			this.fpControls.Name = "fpControls";
			this.fpControls.Size = new System.Drawing.Size(279, 60);
			this.fpControls.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.fpControls.TabIndex = 4;
			this.fpControls.TabStop = false;
			this.fpControls.Click += new System.EventHandler(this.fpControls_Click);
			// 
			// fpStart
			// 
			this.fpStart.BackColor = System.Drawing.Color.Transparent;
			this.fpStart.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.fpStart.Image = global::World_of_Battleships.Properties.Resources.BStartPassive;
			this.fpStart.Location = new System.Drawing.Point(350, 38);
			this.fpStart.Name = "fpStart";
			this.fpStart.Size = new System.Drawing.Size(279, 60);
			this.fpStart.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.fpStart.TabIndex = 3;
			this.fpStart.TabStop = false;
			this.fpStart.Click += new System.EventHandler(this.fpStart_Click);
			// 
			// fpbControls
			// 
			this.fpbControls.BackColor = System.Drawing.SystemColors.ActiveCaption;
			this.fpbControls.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
			this.fpbControls.Image = global::World_of_Battleships.Properties.Resources.Controls;
			this.fpbControls.Location = new System.Drawing.Point(167, 243);
			this.fpbControls.Name = "fpbControls";
			this.fpbControls.Size = new System.Drawing.Size(618, 128);
			this.fpbControls.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.fpbControls.TabIndex = 7;
			this.fpbControls.TabStop = false;
			this.fpbControls.Visible = false;
			// 
			// GUI
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.Control;
			this.BackgroundImage = global::World_of_Battleships.Properties.Resources.Background;
			this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.ClientSize = new System.Drawing.Size(937, 500);
			this.Controls.Add(this.fpbControls);
			this.Controls.Add(this.fpbInfo);
			this.Controls.Add(this.fpMenu);
			this.Controls.Add(this.fpControls);
			this.Controls.Add(this.fpStart);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.KeyPreview = true;
			this.Name = "GUI";
			this.Text = "World of Battleship";
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.GUI_KeyDown);
			((System.ComponentModel.ISupportInitialize)(this.fpbInfo)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.fpMenu)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.fpControls)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.fpStart)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.fpbControls)).EndInit();
			//this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.PictureBox fpStart;
		private System.Windows.Forms.PictureBox fpControls;
		private System.Windows.Forms.PictureBox fpMenu;
		private System.Windows.Forms.PictureBox fpbInfo;
		private System.Windows.Forms.PictureBox fpbControls;
	}
}

