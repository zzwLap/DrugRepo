using System.Linq.Expressions;

namespace webapi
{
    internal static class ExpMapper
    {
        public static Dictionary<string, object> va = new Dictionary<string, object>();
        public static IQueryable<TResult> SelectByType<TOriginal, TResult>(this IQueryable<TOriginal> source)
        {
            var exp = SelectByType<TOriginal, TResult>();
            return source.Select(exp);
        }

        public static Expression<Func<TOriginal, TResult>> SelectByType<TOriginal, TResult>()
        {
            var key = typeof(TOriginal).FullName + "^" + typeof(TResult).FullName;
            if (!va.ContainsKey(key))
            {
                var exp = SelectByTypeCore<TOriginal, TResult>();
                va[key] = exp;
                return exp;
            }

            return ((Expression<Func<TOriginal, TResult>>)va[key]);
        }

        public static Expression<Func<TOriginal, TResult>> SelectByTypeCore<TOriginal, TResult>()
        {
            ParameterExpression parameter = Expression.Parameter(typeof(TOriginal), "u");

            NewExpression newEx = Expression.New(typeof(TResult));
            var dst = typeof(TResult).GetProperties().ToDictionary(t => t.Name, t => t);
            var src = typeof(TOriginal).GetProperties().ToDictionary(t => t.Name, t => t);

            List<MemberAssignment> members = new List<MemberAssignment>();
            foreach (var item in dst)
            {
                var p = typeof(TOriginal).GetProperty(item.Key);
                if (p != null)
                {
                    members.Add(Expression.Bind(item.Value, Expression.Property(parameter, p)));
                }
            }

            MemberBinding[] bindings = members.ToArray();

            Expression<Func<TOriginal, TResult>> exp = Expression.Lambda<Func<TOriginal, TResult>>(
                Expression.MemberInit(newEx, bindings), parameter);
            return exp;
        }
    }
}
