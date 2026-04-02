public class DateHelper
{
    public static (DateTime FirstDay, DateTime LastDay) GetFirstAndLastDayOfMonth(int month, int year)
    {
        if (month < 1 || month > 12)
            throw new ArgumentOutOfRangeException(nameof(month), "O mês deve estar entre 1 e 12.");

        if (year < 1)
            throw new ArgumentOutOfRangeException(nameof(year), "O ano deve ser positivo.");

        DateTime firstDay = new DateTime(year, month, 1, 0, 0, 0);
        DateTime lastDay = new DateTime(year, month, DateTime.DaysInMonth(year, month), 23, 59, 59);

        return (firstDay, lastDay);
    }

}
