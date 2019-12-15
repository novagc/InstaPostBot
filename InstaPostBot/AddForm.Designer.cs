namespace InstaPostBot
{
	partial class AddForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddForm));
			this.maskTextBox = new System.Windows.Forms.TextBox();
			this.pathTextBox = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.confirmButton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// maskTextBox
			// 
			this.maskTextBox.Location = new System.Drawing.Point(15, 108);
			this.maskTextBox.Name = "maskTextBox";
			this.maskTextBox.Size = new System.Drawing.Size(312, 22);
			this.maskTextBox.TabIndex = 0;
			// 
			// pathTextBox
			// 
			this.pathTextBox.Location = new System.Drawing.Point(15, 210);
			this.pathTextBox.Name = "pathTextBox";
			this.pathTextBox.Size = new System.Drawing.Size(312, 22);
			this.pathTextBox.TabIndex = 1;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(327, 187);
			this.label1.TabIndex = 2;
			this.label1.Text = resources.GetString("label1.Text");
			// 
			// confirmButton
			// 
			this.confirmButton.Location = new System.Drawing.Point(15, 247);
			this.confirmButton.Name = "confirmButton";
			this.confirmButton.Size = new System.Drawing.Size(312, 45);
			this.confirmButton.TabIndex = 3;
			this.confirmButton.Text = "Добавить";
			this.confirmButton.UseVisualStyleBackColor = true;
			this.confirmButton.Click += new System.EventHandler(this.confirmButton_Click);
			// 
			// AddForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(339, 309);
			this.Controls.Add(this.confirmButton);
			this.Controls.Add(this.pathTextBox);
			this.Controls.Add(this.maskTextBox);
			this.Controls.Add(this.label1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "AddForm";
			this.Text = "Новое правило";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox maskTextBox;
		private System.Windows.Forms.TextBox pathTextBox;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button confirmButton;
	}
}