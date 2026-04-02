using Microsoft.EntityFrameworkCore;

public static class ModelBuilderExtensions
{
    public static void ApplyDecimalPrecision(this ModelBuilder modelBuilder, int precision = 18, int scale = 3)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            foreach (var property in entityType.GetProperties())
            {
                if (property.ClrType == typeof(decimal) ||
                    (property.ClrType.IsGenericType && property.ClrType.GetGenericTypeDefinition() == typeof(Nullable<>) && Nullable.GetUnderlyingType(property.ClrType) == typeof(decimal)))
                {
                    property.SetPrecision(precision);
                    property.SetScale(scale);
                }
            }
        }
    }
}
