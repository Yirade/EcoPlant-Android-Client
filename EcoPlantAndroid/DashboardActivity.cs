using Android.Content;
using System.ComponentModel.DataAnnotations;

namespace EcoPlantAndroid;

[Activity(Label = "Dashboard")]
public class DashboardActivity : Activity
{
    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
		SetContentView(Resource.Layout.activity_dashboard);

        string username = FindViewById<EditText>(Resource.Id.editText1).Text;
        string email = FindViewById<EditText>(Resource.Id.editText2).Text;
        string pw = FindViewById<EditText>(Resource.Id.editText3).Text;
	}

    void cancelClick(object? sender, EventArgs e)
    {
        Intent intent = new Intent(this, typeof(MainActivity));
        StartActivity(intent);
    }

    void registerClick(object? sender, EventArgs e)
    {

    }
}