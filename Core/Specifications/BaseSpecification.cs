using Core.Interfaces;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Authentication.ExtendedProtection;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specifications
{
    public class BaseSpecification<T> : ISpecification<T>
    {
        private readonly Expression<Func<T, bool>> _criteria;

        public BaseSpecification(Expression<Func<T, bool>> criteria)
        {
            _criteria = criteria;
        }

        protected BaseSpecification() : this(null) { }

        public Expression<Func<T, bool>>? Criteria => _criteria;

        public Expression<Func<T, object>>? OrdeyBy { get; private set; }

        public Expression<Func<T, object>>? OrdeyByDescending { get; private set; }

        public bool IsDistinct {  get; private set; }

        protected void AddOrderBy(Expression<Func<T, object>> orderByExpression)
        {
            OrdeyBy = orderByExpression;
        }
        protected void AddOrderByDescending(Expression<Func<T, object>> orderByDescExpression)
        {
            OrdeyByDescending = orderByDescExpression;
        }

        protected void ApplyDistinct()
        {
            IsDistinct = true;
        }
    }

    public class BaseSpecification<T, TResult> : BaseSpecification<T> , ISpecification<T, TResult>
    {
        private readonly Expression<Func<T, bool>> _criteria;

        public BaseSpecification(Expression<Func<T, bool>> criteria) : base(criteria)
        {
            _criteria = criteria;
        }

        protected BaseSpecification() : this(null!) { }

        public Expression<Func<T, TResult>>? Select {  get; private set; }

        protected void AddSelect(Expression<Func<T, TResult>> selectExpression)
        {
            Select = selectExpression;
        }
    }
}
