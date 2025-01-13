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


            // �������� ����������
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

            // ��������� ������ ������� �������
            var callLogHelper = new CallLogHelper(ContentResolver);
            var callLogs = callLogHelper.GetCallLogs();

            // ����������� ������ � ListView
            ListView callLogList = FindViewById<ListView>(Resource.Id.callLogList);
            var callLogStrings = callLogs.Select(log =>
                $"���: {log.SubscribersName}\n�����: {log.PhoneNumber}\n���: {log.CallType}\n����: {log.CallDate}\n������������: {log.CallDuration} ���"
            ).ToList();

            ArrayAdapter adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, callLogStrings);
            callLogList.Adapter = adapter;
        }
    }
}