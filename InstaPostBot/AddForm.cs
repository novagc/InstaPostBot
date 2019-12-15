using InstaPostBot.Other;
using System;
using System.Windows.Forms;

namespace InstaPostBot
{
	public partial class AddForm : Form
	{
		public AddForm()
		{
			InitializeComponent();
		}

		private void confirmButton_Click(object sender, EventArgs e)
		{
			if (pathTextBox.Text.Length == 0 || maskTextBox.Text.Length == 0)
			{
				MessageBox.Show("Все поля должны быть заполнены");
			}
			else
			{
				if (!MainForm.FS.ValidPath(pathTextBox.Text))
				{
					MessageBox.Show("Некорректный путь");
					return;
				}

				if (!Shedule.CheckMask(maskTextBox.Text))
				{
					MessageBox.Show("Некорректное правило");
					return;
				}

				MainForm.Add(new Shedule(pathTextBox.Text, maskTextBox.Text));
				Close();
			}
		}
	}
}
