using FilterPagingEfCore.Enums;
using FilterPagingEfCore.Filter;
using System.Linq.Expressions;
using System.Reflection;
using Expression = System.Linq.Expressions.Expression;


namespace FilterPagingEfCore.Extenstion
{

    public static class DynamicQueryExtensions
    {
        public static IQueryable<TModel> DynamicWhere<TModel>(this IQueryable<TModel> iqueryable, IEnumerable<FilterParam> dynamicModel, ComparisonMode comparisonMode)
        {
            switch (comparisonMode)
            {

                case ComparisonMode.Or:
                    return OrMode(iqueryable, dynamicModel);

                case ComparisonMode.And:
                    return AndMode(iqueryable, dynamicModel);

                default:
                    return AndMode(iqueryable, dynamicModel);


            }

        }
        public static bool IsGenericType(this Type type, Type genericType) => type.IsGenericType && type.GetGenericTypeDefinition() == genericType;
        public static bool IsNullableType(this Type type) => type.IsGenericType(typeof(Nullable<>));
        public static IQueryable<TModel> OrMode<TModel>(IQueryable<TModel> iqueryable, IEnumerable<FilterParam> dynamicModel)
        {
            var result = iqueryable;
            var param = Expression.Parameter(typeof(TModel), "x");
            Expression<Func<TModel, bool>> finalExpression = null;

            foreach (var filterParam in dynamicModel)
            {
                var filterExpression = Filter<TModel>(filterParam);
                var filterBody = Expression.Invoke(filterExpression, param);

                if (finalExpression == null)
                {
                    finalExpression = Expression.Lambda<Func<TModel, bool>>(filterBody, param);
                }
                else
                {
                    var orExpression = Expression.OrElse(finalExpression.Body, filterBody);
                    finalExpression = Expression.Lambda<Func<TModel, bool>>(orExpression, param);
                }
            }

            if (finalExpression == null)
                finalExpression = _ => false; // Default false expression if no filters were provided

            return result.Where(finalExpression);
        }

        public static IQueryable<TModel> AndMode<TModel>(IQueryable<TModel> iqueryable, IEnumerable<FilterParam> dynamicModel)
        {
            var result = iqueryable;
            foreach (var dynamicModelItem in dynamicModel)
            {
                result = result.Where(Filter<TModel>(dynamicModelItem));
            }
            return result;
        }

        private static Expression<Func<TModel, bool>> Filter<TModel>(FilterParam item)
        {
            var filterColumn = typeof(TModel).GetProperty(item.ColumnName,
                BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public);

            var parameterExpression = Expression.Parameter(typeof(TModel));
            var memberExpression = Expression.Property(parameterExpression, item.ColumnName);

            var converted = Convert(memberExpression.Type, item.FilterValue);
            var constantExpression = Expression.Constant(converted);

            Expression comparison;
            if (memberExpression.Type == typeof(Guid))
            {
                comparison = GetExpression(memberExpression, constantExpression, item.FilterOption);
            }
            else
            {

                if ((memberExpression.Type == typeof(DateTime) || memberExpression.Type == typeof(DateTime?)) && item.FilterOption == ComparisonMethod.Equal)
                {
                    var date = DateTime.Parse(item.FilterValue).Date;
                    if (memberExpression.Type == typeof(DateTime?))
                        memberExpression = Expression.Property(memberExpression, "Value");
                    var datememberExpression = Expression.Property(memberExpression, "Date");
                    comparison = GetExpression(datememberExpression, Expression.Constant(date), item.FilterOption);
                }
                else
                {
                    var value = Expression.Convert(constantExpression, memberExpression.Type);
                    comparison = GetExpression(memberExpression, value, item.FilterOption);
                }


            }
            var expression = Expression.Lambda<Func<TModel, bool>>(comparison, parameterExpression);
            return expression; ;
        }

        private static Expression GetExpression(Expression memberExpression, Expression convert, ComparisonMethod comparisonMethod)
        {
            switch (comparisonMethod)
            {
                case ComparisonMethod.LessThan:
                    return QueryLessThan(memberExpression, convert);
                case ComparisonMethod.LessThanEqual:
                    return LessThanOrEqual(memberExpression, convert);
                case ComparisonMethod.GreaterThan:
                    return GreaterThan(memberExpression, convert);
                case ComparisonMethod.GreaterThanEqual:
                    return GreaterThanOrEqual(memberExpression, convert);
                case ComparisonMethod.Equal:
                    return QueryEqual(memberExpression, convert);
                case ComparisonMethod.NotEqual:
                    return Expression.Not(QueryEqual(memberExpression, convert));
                case ComparisonMethod.IsNullOrWhiteSpace:
                    return IsNullOrWhiteSpace(memberExpression, convert);
                case ComparisonMethod.IsNotNullOrWhiteSpace:
                    return Expression.Not(IsNullOrWhiteSpace(memberExpression, convert));
                case ComparisonMethod.Contain:
                    return Contains(memberExpression, convert);
                case ComparisonMethod.NotContain:
                    return Expression.Not(Contains(memberExpression, convert));
                case ComparisonMethod.StartWith:
                    return StartsWith(memberExpression, convert);
                case ComparisonMethod.NotStartWith:
                    return Expression.Not(StartsWith(memberExpression, convert));
                case ComparisonMethod.EndWith:
                    return EndsWith(memberExpression, convert);
                case ComparisonMethod.NotEndWith:
                    return Expression.Not(EndsWith(memberExpression, convert));
                default:
                    return null;
            }
        }

        private static Expression QueryEqual(Expression memberExpression, Expression value)
        {
            return Expression.Equal(memberExpression, value);
        }

        private static Expression Contains(Expression memberExpression, Expression value)
        {
            var method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
            return Expression.Call(memberExpression, method, value);
        }

        private static Expression GreaterThan(Expression memberExpression, Expression value)
        {
            return Expression.GreaterThan(memberExpression, value);
        }

        private static Expression QueryLessThan(Expression memberExpression, Expression value)
        {
            return Expression.LessThan(memberExpression, value);
        }

        private static Expression GreaterThanOrEqual(Expression memberExpression, Expression value)
        {
            return Expression.GreaterThanOrEqual(memberExpression, value);
        }

        private static Expression LessThanOrEqual(Expression memberExpression, Expression value)
        {
            return Expression.LessThan(memberExpression, value);
        }

        private static Expression IsNullOrWhiteSpace(Expression memberExpression, Expression value)
        {
            var methodCall = Expression.Call(typeof(string), nameof(string.IsNullOrWhiteSpace), null, memberExpression);
            return methodCall;
        }

        private static Expression StartsWith(Expression memberExpression, Expression value)
        {
            var methodIsNullOrEmpty = typeof(string).GetMethod("StartsWith", new[] { typeof(string) });
            return Expression.Call(memberExpression, methodIsNullOrEmpty, value);
        }

        private static Expression EndsWith(Expression memberExpression, Expression value)
        {
            var methodIsNullOrEmpty = typeof(string).GetMethod("EndsWith", new[] { typeof(string) });
            return Expression.Call(memberExpression, methodIsNullOrEmpty, value);
        }


        private static object Convert(Type type, string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }

            var isNullable = type.IsNullableType();
            if (isNullable)
                type = Nullable.GetUnderlyingType(type);
            if (type.IsEnum)
            {
                return Enum.Parse(type, value);
            }

            if (type.IsNumericType())
            {
                return value.ToNumber(type);
            }

            if (type.IsDateType())
            {
                if (type == typeof(DateTime))
                    return DateTime.Parse(value);
                if (type == typeof(DateTimeOffset))
                    return DateTimeOffset.Parse(value);
            }
            if (type == typeof(bool))
            {
                return bool.Parse(value);
            }

            if (type.IsGuidType())
            {
                return Guid.Parse(value);
            }

            return value;
        }
    }
}