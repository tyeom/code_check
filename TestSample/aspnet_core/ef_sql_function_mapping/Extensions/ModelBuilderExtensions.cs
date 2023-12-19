using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace EF_Test.Extensions
{
    public static class ModelBuilderExtensions
    {
        public static DateTime? ToDateTime(this string strDate, int format) => throw new NotSupportedException("Use inside AddSqlDateOnlyConvertFunction");

        public static ModelBuilder AddSqlDateOnlyConvertFunction(this ModelBuilder modelBuilder)
        {
            modelBuilder.HasDbFunction(() => ToDateTime(default, default))
                .HasTranslation(args => new SqlFunctionExpression(
                        functionName: "CONVERT",
                        arguments: args.Prepend(new SqlFragmentExpression("DATETIME")),
                        nullable: false,
                        argumentsPropagateNullability: new[] { false, true, false },
                        type: typeof(DateTime),
                        typeMapping: null));

            return modelBuilder;
        }
    }
}
