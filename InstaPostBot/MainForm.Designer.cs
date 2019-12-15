using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using InstaPostBot.Services;

namespace InstaPostBot
{
	partial class MainForm
	{
		/// <summary>
		/// Обязательная переменная конструктора.
		/// </summary>
		private IContainer components = null;

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
		/// Требуемый метод для поддержки конструктора — не изменяйте 
		/// содержимое этого метода с помощью редактора кода.
		/// </summary>
		private void InitializeComponent()
		{
			this.dataGridView1 = new System.Windows.Forms.DataGridView();
			this.addButton = new System.Windows.Forms.Button();
			this.ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Mask = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.BasePath = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.WorkPath = new System.Windows.Forms.DataGridViewTextBoxColumn();
			((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
			this.SuspendLayout();
			// 
			// dataGridView1
			// 
			this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ID,
            this.Mask,
            this.BasePath,
            this.WorkPath});
			this.dataGridView1.Location = new System.Drawing.Point(12, 12);
			this.dataGridView1.Name = "dataGridView1";
			this.dataGridView1.RowHeadersWidth = 10;
			this.dataGridView1.RowTemplate.Height = 24;
			this.dataGridView1.Size = new System.Drawing.Size(678, 426);
			this.dataGridView1.TabIndex = 0;
			// 
			// addButton
			// 
			this.addButton.Location = new System.Drawing.Point(12, 444);
			this.addButton.Name = "addButton";
			this.addButton.Size = new System.Drawing.Size(678, 45);
			this.addButton.TabIndex = 1;
			this.addButton.Text = "Добавить новое правило";
			this.addButton.UseVisualStyleBackColor = true;
			this.addButton.Click += new System.EventHandler(this.addButton_Click);
			// 
			// ID
			// 
			this.ID.HeaderText = "ID";
			this.ID.MinimumWidth = 6;
			this.ID.Name = "ID";
			this.ID.ReadOnly = true;
			this.ID.Width = 125;
			// 
			// Mask
			// 
			this.Mask.HeaderText = "Mask";
			this.Mask.MinimumWidth = 6;
			this.Mask.Name = "Mask";
			this.Mask.ReadOnly = true;
			this.Mask.Width = 125;
			// 
			// BasePath
			// 
			this.BasePath.HeaderText = "BasePath";
			this.BasePath.MinimumWidth = 6;
			this.BasePath.Name = "BasePath";
			this.BasePath.ReadOnly = true;
			this.BasePath.Width = 125;
			// 
			// WorkPath
			// 
			this.WorkPath.HeaderText = "WorkPath";
			this.WorkPath.MinimumWidth = 6;
			this.WorkPath.Name = "WorkPath";
			this.WorkPath.ReadOnly = true;
			this.WorkPath.Width = 125;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(700, 500);
			this.Controls.Add(this.addButton);
			this.Controls.Add(this.dataGridView1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.Name = "MainForm";
			this.Text = "InstaPostBot";
			this.Load += new System.EventHandler(this.MainForm_Load);
			((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private DataGridView dataGridView1;
		private Button addButton;
		private DataGridViewTextBoxColumn ID;
		private DataGridViewTextBoxColumn Mask;
		private DataGridViewTextBoxColumn BasePath;
		private DataGridViewTextBoxColumn WorkPath;
	}
}

