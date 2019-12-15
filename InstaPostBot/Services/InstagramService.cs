using InstagramApiSharp.API;
using InstagramApiSharp.API.Builder;
using InstagramApiSharp.Classes;
using InstagramApiSharp.Classes.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InstaPostBot.Services
{
	public class InstagramService
	{
		private IInstaApi api;
		private FileSystemService fs;
		private LogService logger;
		private string sessionFilePath = "./instagram.config";

		public string UserName { get; }
		public bool Available => api.IsUserAuthenticated;
		public string Phone { get; }

		public static event Action VerificationCodeRequest;
		public static event Action<bool> AuthenticationEnd;

		public event Action<IEnumerable<string>> MediaUploaded;

		public InstagramService(string login, string password, string phone, FileSystemService fsService = null, LogService log = null)
		{
			fs = fsService ?? new FileSystemService();
			logger = log ?? new LogService("./instagram.log");

			try
			{
				UserName = login;
				Phone = phone;

				var temp = new UserSessionData
				{
					UserName = login,
					Password = password
				};

				api = InstaApiBuilder.CreateBuilder()
					.SetUser(temp)
					.Build();

				LoadSessionData();

				logger.WriteAsync("InstagramService", "INFO",
					$"Creating completed");
			}
			catch (Exception e)
			{
				logger.WriteAsync("InstagramService", "ERROR",
					$"Creating failed\n\t{e.Message}");
			}
		}

		public async void Login()
		{
			if (api.IsUserAuthenticated)
			{
				await logger.WriteAsync("InstagramService", "INFO",
					"User is already authenticated");
				AuthenticationEnd?.Invoke(true);
				return;
			}

			var loginRequestResult = await api.LoginAsync();
			if (!loginRequestResult.Succeeded)
			{
				var challengeRequireVerifyMethodRequestResult = await api.GetChallengeRequireVerifyMethodAsync();
				if (challengeRequireVerifyMethodRequestResult.Succeeded)
				{
					if (challengeRequireVerifyMethodRequestResult.Value.SubmitPhoneRequired)
					{
						await api.SubmitPhoneNumberForChallengeRequireAsync(Phone);
					}
					await api.RequestVerifyCodeToSMSForChallengeRequireAsync();
					await logger.WriteAsync("InstagramService", "INFO",
						"Verification code required");
					VerificationCodeRequest?.Invoke();
					return;
				}
			}
			else
			{
				await logger.WriteAsync("InstagramService", "INFO",
					"User authentication completed without verification code");
				AuthenticationEnd?.Invoke(true);
				SaveSessionData();
				return;
			}
			await logger.WriteAsync("InstagramService", "INFO",
				"Authentication failed");
			AuthenticationEnd?.Invoke(false);
		}

		public async void ConfirmPhoneCode(string code)
		{
			await logger.WriteAsync("InstagramService", "INFO",
				"Verification code received");
			var result = await api.VerifyCodeForChallengeRequireAsync(code);

			if (result.Succeeded)
			{
				await logger.WriteAsync("InstagramService", "INFO",
					"User authentication completed with verification code");
				SaveSessionData();
			}
			else
			{
				await logger.WriteAsync("InstagramService", "INFO",
					"User authentication completed with verification code");
			}
			AuthenticationEnd?.Invoke(result.Succeeded);
		}

		public async void UploadImageAsync(string path, string message = "")
		{
			try
			{
				var image = new InstaImageUpload
				{
					Height = 0,
					Width = 0,
					Uri = path
				};

				await api.MediaProcessor.UploadPhotoAsync(image, message);
				await logger.WriteAsync("InstagramService", "INFO",
					$"Image uploaded\n\t{path}");
			}
			catch (Exception e)
			{
				await logger.WriteAsync("InstagramService", "ERROR",
					$"Image didn't upload\n\t{path}\n\t{e.Message}");
			}
			finally
			{
				MediaUploaded?.Invoke(new[] { path });
			}
		}

		public async void UploadAlbumAsync(IEnumerable<string> images, string message = "")
		{
			try
			{
				var temp = images.Select(x => new InstaImageUpload
				{
					Height = 0,
					Width = 0,
					Uri = x
				})
					.ToArray();
				await api.MediaProcessor.UploadAlbumAsync(temp, new InstaVideoUpload[] { }, message);
				await logger.WriteAsync("InstagramService", "INFO",
					$"Album uploaded\n\t{String.Join("\n\t", images)}");
			}
			catch (Exception e)
			{
				await logger.WriteAsync("InstagramService", "ERROR",
					$"Image didn't upload\n\t{String.Join("\n\t", images)}\n\t{e.Message}");
			}
			finally
			{
				MediaUploaded?.Invoke(images);
			}
		}

		public async void SaveSessionData()
		{
			try
			{
				using (var temp = fs.GetFileStream(sessionFilePath, true))
				{
					await api.GetStateDataAsStream().CopyToAsync(temp);
					await logger.WriteAsync("InstagramService", "INFO",
						$"Saving Session Data in {sessionFilePath} completed");
				}
			}
			catch (Exception e)
			{
				await logger.WriteAsync("InstagramService", "ERROR",
					$"Saving Session Data in {sessionFilePath} failed\n\t{e.Message}");
			}
		}

		private async void LoadSessionData()
		{
			try
			{
				using (var temp = fs.GetFileStream(sessionFilePath))
				{
					await api.LoadStateDataFromStreamAsync(temp);
					await logger.WriteAsync("InstagramService", "INFO",
						$"Loading Session Data from {sessionFilePath} completed");
				}
			}
			catch (Exception e)
			{
				await logger.WriteAsync("InstagramService", "ERROR",
					$"Loading Session Data from {sessionFilePath} failed\n\t{e.Message}");
			}
		}
	}
}
