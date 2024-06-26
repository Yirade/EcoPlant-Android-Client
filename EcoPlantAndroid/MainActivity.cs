using Android.OS;
using Android.Views;
using Android.Webkit;

namespace EcoPlantAndroid
{
	[Activity(Label = "EcoPlant", MainLauncher = true)]
	public class MainActivity : Activity
	{
		WebView webView;

		protected override void OnCreate(Bundle? savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.activity_main);

			webView = FindViewById<WebView>(Resource.Id.webView1);
			webView.Settings.JavaScriptEnabled = true;
			webView.Settings.CacheMode = CacheModes.Normal;
			webView.Settings.DomStorageEnabled = true;
			//webView.Touch += WebView_Touch;
			webView.SetWebViewClient(new WebViewClient());
			webView.LoadUrl("https://ecoplant.yirade.dev");
			webView.Reload();
			//webView.AddJavascriptInterface();

			ActionBar.Hide();

			Window.AddFlags(WindowManagerFlags.Fullscreen);
			TransparentStatusBar();
		}

		private void WebView_Touch(object? sender, View.TouchEventArgs e)
		{
			var swipeType = e.Event.Action & MotionEventActions.Mask;

			/*
			if(swipeType == MotionEventActions)
			{

			}
			*/
		}

		public override bool OnKeyDown(Keycode keyCode, KeyEvent e)
		{
			if (keyCode == Keycode.Back && webView.CanGoBack())
			{
				webView.GoBack();
				return true;
			}
			return base.OnKeyDown(keyCode, e);
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