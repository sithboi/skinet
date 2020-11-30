using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Core.Specifications
{
    public interface ISpecification<T>
    {
        //Expression takes a function which takes a type and what it's returning
        //Criteria is criteria of the thing we want to get
         //Expression<Funct<T, bool>> Criteria {}

        Expression<Func<T, bool>> Criteria {get;}
        List<Expression<Func<T, object>>> Includes {get;}
         
    }
}