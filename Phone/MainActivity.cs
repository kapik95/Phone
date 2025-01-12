namespace Phone
{
    [Activity(Label = "@string/app_name", Theme = "@android:style/Theme.Material.Light.NoActionBar", MainLauncher = true)]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.M)
            {
                if (CheckSelfPermission(Android.Manifest.Permission.ReadCallLog) != Android.Content.PM.Permission.Granted)
                {
                    RequestPermissions(new[] { Android.Manifest.Permission.ReadCallLog }, 1);
                }
            }
        }
    }
}