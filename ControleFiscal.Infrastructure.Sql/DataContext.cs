using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;


namespace ControleFiscal.Infrastructure.Sql.Focus.Context
{
    public class DataContext : IDesignTimeDbContextFactory<DbContext>
    {
        private string dataBaseName { get; set; }
        private string host { get; set; }
        public DataContext(string pdatabase, string phost)
        {
            dataBaseName = pdatabase;
            host = phost;
        }

        public bool IsValid { get; set; } = false;

        public DbContext CreateDbContext() => CreateDbContext(null);
        public DbContext CreateDbContext(string[] args)
        {
            var _optionsBuilder = new DbContextOptionsBuilder();
            string conn = "User=SYSDBA;Password=masterkey;Database=" + dataBaseName + ";DataSource=" + host + ";Port=3050;Dialect=3;Charset=UTF8;";

            _optionsBuilder.UseFirebird(conn, o => o.WithExplicitStringLiteralTypes()).EnableSensitiveDataLogging();


            var context = new DbContext(_optionsBuilder.Options);

            IsValid = true;
            return context;
        }


    }
}
