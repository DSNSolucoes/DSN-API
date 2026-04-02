using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Data;
using System.Data.Common;
using System.Text.RegularExpressions;

public class SqlInterceptor : DbCommandInterceptor
{
    public override InterceptionResult<DbDataReader> ReaderExecuting(
        DbCommand command,
        CommandEventData eventData,
        InterceptionResult<DbDataReader> result)
    {
        int count = new Regex(@"\b" + Regex.Escape("SELECT") + @"\b", RegexOptions.IgnoreCase).Matches("SELECT").Count;

      //  command.CommandText = (command.CommandText.Contains("ROWS (1)") && count == 1) ? command.CommandText.Replace("ROWS (1)","").Replace("SELECT " , "SELECT FIRST 1 ") : command.CommandText;
        return base.ReaderExecuting(command, eventData, result);
    }

}
