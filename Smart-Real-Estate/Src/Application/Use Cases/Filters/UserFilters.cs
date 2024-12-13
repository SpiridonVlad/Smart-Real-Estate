using Domain.Entities;
using Domain.Types;
using System.Linq.Expressions;

namespace Application.Use_Cases.Filters
{
    public class UserFilters
    {
        public bool? Verified { get; set; }
        public UserType? Type { get; set; }
        public decimal? MinRating { get; set; }
        public decimal? MaxRating { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }

        public Expression<Func<User, bool>> BuildFilterExpression()
        {
            Expression<Func<User, bool>> filter = u => true;

            if (Verified.HasValue)
            {
                filter = filter.And(u => u.Verified == Verified.Value);
            }

            if (Type.HasValue)
            {
                filter = filter.And(u => u.Type == Type.Value);
            }

            if (MinRating.HasValue)
            {
                filter = filter.And(u => u.Rating >= MinRating.Value);
            }

            if (MaxRating.HasValue)
            {
                filter = filter.And(u => u.Rating <= MaxRating.Value);
            }

            if (!string.IsNullOrWhiteSpace(Username))
            {
                filter = filter.And(u => u.Username.Contains(Username));
            }

            if (!string.IsNullOrWhiteSpace(Email))
            {
                filter = filter.And(u => u.Email.Contains(Email));
            }

            return filter;
        }
    }

    public static class ExpressionExtensions
    {
        public static Expression<Func<T, bool>> And<T>(
            this Expression<Func<T, bool>> left,
            Expression<Func<T, bool>> right)
        {
            var parameter = Expression.Parameter(typeof(T));

            var leftVisitor = new ReplaceParameterVisitor(left.Parameters[0], parameter);
            var leftExpression = leftVisitor.Visit(left.Body);

            var rightVisitor = new ReplaceParameterVisitor(right.Parameters[0], parameter);
            var rightExpression = rightVisitor.Visit(right.Body);

            var andExpression = Expression.AndAlso(leftExpression, rightExpression);

            return Expression.Lambda<Func<T, bool>>(andExpression, parameter);
        }

        private class ReplaceParameterVisitor(ParameterExpression oldParameter, ParameterExpression newParameter) : ExpressionVisitor
        {
            private readonly ParameterExpression _oldParameter = oldParameter;
            private readonly ParameterExpression _newParameter = newParameter;

            protected override Expression VisitParameter(ParameterExpression node)
            {
                return node == _oldParameter ? _newParameter : base.VisitParameter(node);
            }
        }
    }
}
