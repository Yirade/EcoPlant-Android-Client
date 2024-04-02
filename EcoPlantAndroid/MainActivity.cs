using Android.OS;
using Android.Views;

using Java.Interop;

using System.Text.Json;
using System.Text;

using Xamarin;
using Android.Telephony.Euicc;
//using Xamarin.Forms;

namespace EcoPlantAndroid
{
	[Activity(Label = "@cyberus/ecoplant", MainLauncher = true)]
	public class MainActivity : Activity
	{
		protected override void OnCreate(Bundle? savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			//
			// Set our view from the "main" layout resource
			SetContentView(Resource.Layout.activity_main);

			ActionBar.Hide();
			TransparentStatusBar();

			FindViewById<Button>(Resource.Id.buttonLogin).Click += LoginClick;
		}

		private async void LoginClick(object? sender, EventArgs e)
		{
			HttpClient client = new HttpClient();
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

						await FileHelper.WriteAllLines("tokens.txt", new string[] { loginResponse.refresh, loginResponse.access });
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

		}

		private void TransparentStatusBar()
		{
			if (Build.VERSION.SdkInt >= BuildVersionCodes.Kitkat)
			{
				// for covering the full screen in android..
				Window.SetFlags(WindowManagerFlags.LayoutNoLimits, WindowManagerFlags.LayoutNoLimits);

				// clear FLAG_TRANSLUCENT_STATUS flag:
				Window.ClearFlags(WindowManagerFlags.TranslucentStatus);
				Window.SetStatusBarColor(Android.Graphics.Color.Transparent);
			}

		}
	}
}