using Android.Content;
using Android.Database;
using Android.Provider;
using System.Collections.Generic;

public class CallLogHelper
{
    private readonly ContentResolver _contentResolver;

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
            CallLog.Calls.Duration  // Длительность вызова

        };

        // Запрос данных
        ICursor cursor = _contentResolver.Query(uri, projection, null, null, $"{CallLog.Calls.Date} DESC");

        if (cursor != null && cursor.MoveToFirst()) 
        {
            do 
            {
                string number = cursor.GetString(cursor.GetColumnIndex(CallLog.Calls.Number));
                string type = GetCallType(cursor.GetInt(cursor.GetColumnIndex(CallLog.Calls.Type)));
                string date = cursor.GetString(cursor.GetColumnIndex(CallLog.Calls.Date));
                string duration = cursor.GetString(cursor.GetColumnIndex(CallLog.Calls.Duration));

                callLogs.Add(new CallLogItem
                {
                    PhoneNumber = number,
                    CallType = type,
                    CallDate = date,
                    CallDuration = duration
                });
            } while (cursor.MoveToNext());

            cursor.Close();
        }

        return callLogs;
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
