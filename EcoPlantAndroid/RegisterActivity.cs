namespace EcoPlantAndroid;

[Activity(Label = "RegisterActivity")]
public class RegisterActivity : Activity
{
    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
		SetContentView(Resource.Layout.activity_register);
    }
}