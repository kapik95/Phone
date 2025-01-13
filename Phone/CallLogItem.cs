public class CallLogItem
{
    public string SubscribersName { get; set; } = string.Empty; 
    public string PhoneNumber { get; set; } = string.Empty; // Номер телефона
    public string CallType { get; set; } = string.Empty; // Тип вызова (входящий, исходящий, пропущенный)
    public string CallDate { get; set; } = string.Empty; // Дата вызова
    public string CallDuration { get; set; } = string.Empty; // Длительность вызова (в секундах)
}