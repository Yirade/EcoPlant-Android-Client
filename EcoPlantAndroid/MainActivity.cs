using Android.OS;
using Android.Views;
using Java.Interop;
using System.Text.Json;
using System.Text;
using Android.Content;
using Xamarin;
using Android.Telephony.Euicc;
using System.Net;

namespace EcoPlantAndroid
{
	[Activity(Label = "@cyberus/ecoplant", MainLauncher = true)]
	public class MainActivity : Activity
	{
		HttpClient client;

		void openDashboard()
		{
			Intent intent = new Intent(this, typeof(DashboardActivity));
			StartActivity(intent);
		}

		protected override void OnCreate(Bundle? savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.activity_main);

			if (ActionBar == null) throw new Exception();
			ActionBar.Hide();
			TransparentStatusBar();

			FindViewById<Button>(Resource.Id.buttonLogin).Click += LoginClick;

			client = new HttpClient();
			if (FileHelper.Exists("tokens.txt"))
			{
				string[] currentToken = FileHelper.ReadAllLines("tokens.txt");

				var loginCheck = new HttpRequestMessage
				{
					Method = HttpMethod.Get,
					RequestUri = new Uri("https://ecoplant-back.yirade.dev/api/v1/token/check/"),
					Headers =
					{
						{HttpRequestHeader.Authorization.ToString(), "Bearer " + currentToken[1]},
						{ HttpRequestHeader.Accept.ToString(), "application/json" },
						{ "X-Version", "1" }
					}
				};

				var responseTask = client.SendAsync(loginCheck);
				responseTask.Wait();

				var response = responseTask.Result;
				if (response.IsSuccessStatusCode)
				{
					openDashboard();
				}
				else FileHelper.Delete("tokens.txt");
			}
		}

		private async void LoginClick(object? sender, EventArgs e)
		{
			client.BaseAddress = new Uri("https://ecoplant-back.yirade.dev");

			StringContent loginContent = new(
			JsonSerializer.Serialize(new
			{
				username = FindViewById<EditText>(Resource.Id.editText1).Text,
				password = FindViewById<EditText>(Resource.Id.editText2).Text
			}),
			Encoding.UTF8,
			"application/json");

			var error = FindViewById<TextView>(Resource.Id.textView3);
			if (error == null) throw new Exception("this shoudn't happen");

			try
			{
				var response = await client.PostAsync("/api/v1/login/", loginContent);

				if (response.StatusCode == System.Net.HttpStatusCode.OK)
				{
					var json = await response.Content.ReadAsStringAsync();

					System.Diagnostics.Debugger.Log(0, "response", "" + json);
					var loginResponse = JsonSerializer.Deserialize<LoginResponse>(json);

					if (loginResponse != null)
					{
						error.Text = "";
						FileHelper.WriteAllLines("tokens.txt", new string[] { loginResponse.refresh, loginResponse.access });
						openDashboard();
					}
					else error.Text = "Invalid response!";
				}
				else error.Text = "Wrong password or user name";
			}
			catch (Exception ex)
			{
				error.Text = ex.Message;
			}
		}

		private void RegisterClick(object? sender, EventArgs e)
		{
			Intent intent = new Intent(this, typeof(RegisterActivity));
			StartActivity(intent);
		}

		private void TransparentStatusBar()
		{
			if (Build.VERSION.SdkInt >= BuildVersionCodes.Kitkat)
			{
				if (Window == null) throw new Exception();
				// for covering the full screen in android..
				Window.SetFlags(WindowManagerFlags.LayoutNoLimits, WindowManagerFlags.LayoutNoLimits);

				// clear FLAG_TRANSLUCENT_STATUS flag:
				Window.ClearFlags(WindowManagerFlags.TranslucentStatus);
				Window.SetStatusBarColor(Android.Graphics.Color.Transparent);
			}
		}
	}
}