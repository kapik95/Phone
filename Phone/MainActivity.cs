namespace Phone
{
    [Activity(Label = "@string/app_name", Theme = "@android:style/Theme.Material.Light.NoActionBar", MainLauncher = true)]
    public class MainActivity : Activity
    {
        protected override async void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            // Проверка разрешений
            if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.M)
            {
                if (CheckSelfPermission(Android.Manifest.Permission.ReadCallLog) != Android.Content.PM.Permission.Granted || CheckSelfPermission(Android.Manifest.Permission.ReadContacts) != Android.Content.PM.Permission.Granted)
                {
                    RequestPermissions(new[] 
                    { 
                        Android.Manifest.Permission.ReadCallLog,
                        Android.Manifest.Permission.ReadContacts
                    }, 1);
                }
            }

            // Получение данных журнала вызовов
            var callLogHelper = new CallLogHelper(ContentResolver);
            var callLogs = await Task.Run(() => callLogHelper.GetCallLogs());


            var recyclerView = FindViewById<AndroidX.RecyclerView.Widget.RecyclerView>(Resource.Id.recyclerView);
            recyclerView.SetLayoutManager(new AndroidX.RecyclerView.Widget.LinearLayoutManager(this));
            var adapter = new CallLogAdapter(callLogs, this);
            recyclerView.SetAdapter(adapter);
        }
    }
}