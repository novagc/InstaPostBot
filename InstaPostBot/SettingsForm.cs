using InstaPostBot.Services;
using System;
using System.Linq;
using System.Windows.Forms;

namespace InstaPostBot
{
	public partial class SettingsForm : Form
	{
		private TextBox[] settings;
		private bool needCode;

		public SettingsForm()
		{
			InitializeComponent();
			settings = new[]
			{
				mySqlUrlTextBox,
				mySqlPortTextBox,
				mySqlLoginTextBox,
				mySqlPasswordTextBox,
				mySqlDbNameTextBox,
				InstagramLoginTextBox,
				InstagramPasswordTextBox,
				InstagramPhoneTextBox
			};
			needCode = false;

			InstagramService.AuthenticationEnd += AuthenticationEnd;
			InstagramService.VerificationCodeRequest += VerificationCodeRequest;
		}

		private void VerificationCodeRequest()
		{
			confirmButton.Enabled = true;
			confirmCodeTextBox.Enabled = true;
			needCode = true;
			statusLabel.Text = "Введите код подтверждения";
		}

		private void AuthenticationEnd(bool success)
		{
			if (success)
			{
				InstagramService.AuthenticationEnd -= AuthenticationEnd;
				InstagramService.VerificationCodeRequest -= VerificationCodeRequest;
				Close();
			}
			else
			{
				MessageBox.Show("Во время аутентификации произошла ошибка.\nПриложение будет закрыто");
				Application.Exit();
			}
		}

		private void confirmButton_Click(object sender, EventArgs e)
		{
			statusLabel.Text = @"...";
			if (needCode)
			{
				confirmCodeTextBox.Enabled = false;
				confirmButton.Enabled = false;
				needCode = true;
				MainForm.Instagram.ConfirmPhoneCode(confirmCodeTextBox.Text);
				confirmCodeTextBox.Text = "";
				return;
			}
			if (settings.Any(x => x.Text.Length == 0))
			{
				statusLabel.Text = "Некорректные данные";
			}
			else
			{
				foreach (var x in settings)
					x.Enabled = false;
				confirmButton.Enabled = false;

				if (MainForm.InitLogger())
				{
					if (MainForm.InitDB(
						mySqlUrlTextBox.Text,
						mySqlPortTextBox.Text,
						mySqlLoginTextBox.Text,
						mySqlPasswordTextBox.Text,
						mySqlDbNameTextBox.Text))
					{
						MainForm.InitFS();
						MainForm.InitInstagram(
							InstagramLoginTextBox.Text,
							InstagramPasswordTextBox.Text,
							InstagramPhoneTextBox.Text);

						return;
					}
				}

				foreach (var x in settings)
					x.Enabled = true;

				confirmButton.Enabled = true;
				statusLabel.Text = "Некорректные данные";
			}
		}
	}
}
