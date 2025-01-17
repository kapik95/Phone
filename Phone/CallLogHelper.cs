using Android.Content;
using Android.Database;
using Android.Provider;
using Android.Net;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Android.Telecom;

public class CallLogHelper
{
    private readonly ContentResolver _contentResolver;
    //Для ограничения кол-ва записей
    private int _count = 0;
    private const  int MAX_NUMBER_RECORDS = 25;

    public CallLogHelper(ContentResolver contentResolver)
    {
        _contentResolver = contentResolver;
    }

    //Метод для получения списка вызовов
    public List<CallLogItem> GetCallLogs() 
    {
        var callLogs = new List<CallLogItem>();
        var uri = CallLog.Calls.ContentUri;

        string[] projection = {
            CallLog.Calls.Number,   // Номер телефона
            CallLog.Calls.Type,     // Тип вызова
            CallLog.Calls.Date,     // Дата вызова
            CallLog.Calls.Duration,  // Длительность вызова

        };

        // Запрос данных
        ICursor cursor = _contentResolver.Query(uri, projection, null, null, $"{CallLog.Calls.Date} DESC");

        if (cursor != null && cursor.MoveToFirst()) 
        {
            do 
            {
                if (_count >= MAX_NUMBER_RECORDS) break;

                string number = cursor.GetString(cursor.GetColumnIndex(CallLog.Calls.Number));
                var (name, photo) = GetContactDetalis(number);
                string type = GetCallType(cursor.GetInt(cursor.GetColumnIndex(CallLog.Calls.Type)));
                var date = cursor.GetLong(cursor.GetColumnIndex(CallLog.Calls.Date));
                string duration = cursor.GetString(cursor.GetColumnIndex(CallLog.Calls.Duration));


                callLogs.Add(new CallLogItem
                {
                    SubscribersName = name,
                    PhoneNumber = number,
                    Photo = photo,
                    CallType = type,
                    CallDate = DateTimeOffset.FromUnixTimeMilliseconds(date).ToString("g"),
                    CallDuration = duration
                });

                _count++;
            } while (cursor.MoveToNext());

            cursor.Close();
        }

        return callLogs;
    }

    private (string Name, byte[] Photo) GetContactDetalis(string number)
    {
        var uri = Android.Net.Uri.WithAppendedPath(ContactsContract.PhoneLookup.ContentFilterUri, Android.Net.Uri.Encode(number));
        string[] projection = {
            ContactsContract.PhoneLookup.InterfaceConsts.DisplayName,
            ContactsContract.PhoneLookup.InterfaceConsts.PhotoUri
        };

        ICursor cursor = _contentResolver.Query(uri, projection, null, null);

        string name = "Неизвестный";
        byte[] photo = null;

        if (cursor != null && cursor.MoveToFirst())
        {
            name = cursor.GetString(cursor.GetColumnIndex(projection[0]));

            string photoUri = cursor.GetString(cursor.GetColumnIndex(projection[1]));

            if (!string.IsNullOrEmpty(photoUri)) 
            {
                try
                {
                    using (var inputStream = _contentResolver.OpenInputStream(Android.Net.Uri.Parse(photoUri)))
                    {
                        if (inputStream != null)
                        {
                            using (var memoryStream = new System.IO.MemoryStream())
                            {
                                inputStream.CopyTo(memoryStream);
                                photo = memoryStream.ToArray();
                            }
                        }
                    }
                }
                catch (Java.IO.FileNotFoundException)
                {

                }
            }
            cursor.Close();
        }
        cursor?.Close();
        return (name, photo); // Возвращаем имя и фото
    }

    // Метод для преобразования типа вызова в текстовое описание
    private string GetCallType(int callType)
    {
        return callType switch
        {
            2 => "Исходящий",
            1 => "Входящий",
            3 => "Пропущенный",
            _ => "Неизвестный"
        };
    }
}
