namespace NRepository.Core
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Linq.Expressions;
    using NRepository.Core.Utilities;

    public static class PropertyInfo<T> where T : class
    {
        public static string PropertyName(Expression expression)
        {
            Check.NotNull(expression, "expression");

            var memberName = GetMemberName(expression, ".");
            var indexOf = memberName.LastIndexOf('.') + 1;
            return indexOf > 0 ? memberName.Substring(indexOf) : memberName;
        }

        public static string GetMemberName(Expression expression)
        {
            Check.NotNull(expression, "collection");

            return GetMemberName(expression, ".");
        }

        public static string GetMemberName(Expression expression, string separator)
        {
            Check.NotNull(expression, "expression");
            Check.NotNull(separator, "separator");

            var properties = new List<string>();
            GetExpressionName(expression, properties);

            return string.Join(separator, properties);
        }

        public static string GetMemberName(Expression<Func<T, object>> expression)
        {
            Check.NotNull(expression, "expression");

            return GetMemberName(expression, ".");
        }

        public static string GetMemberName(Expression<Func<T, object>> expression, string separator)
        {
            Check.NotNull(expression, "expression");
            Check.NotNull(separator, "separator");

            var properties = new List<string>();
            GetExpressionName(expression, properties);

            return string.Join(separator, properties);
        }

        private static void GetExpressionName(Expression expression, List<string> properties)
        {
            Check.NotNull(expression, "collection");
            Check.NotNull(properties, "properties");

            switch (expression.NodeType)
            {
                case ExpressionType.Lambda:
                    var body = ((LambdaExpression)expression).Body;
                    GetExpressionName(body, properties);
                    break;
                case ExpressionType.Call:
                    Debug.Assert(((MethodCallExpression)expression).Arguments.Count == 2, "There can be only 2");
                    var methodExpression = (MethodCallExpression)expression;
                    GetExpressionName(methodExpression.Arguments.First(), properties);
                    GetExpressionName(methodExpression.Arguments.Last(), properties);
                    break;
                case ExpressionType.Convert:
                    var unaryExpression = ((UnaryExpression)expression);
                    GetExpressionName(unaryExpression.Operand, properties);
                    break;
                case ExpressionType.MemberAccess:
                    var memberExpression = (MemberExpression)expression;
                    GetExpressionName(memberExpression.Expression, properties);
                    properties.Add(memberExpression.Member.Name);
                    break;
                default:
                    // throw new InvalidOperationException(string.Format("Expression NodeType not supported: {0}", expression.NodeType));
                    break;
            }
        }
    }
}
