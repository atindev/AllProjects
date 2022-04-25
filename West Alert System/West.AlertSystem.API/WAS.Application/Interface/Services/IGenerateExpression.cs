using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Text;
using Twilio.Rest.Api.V2010.Account;
using WAS.Application.Common.Models;
using WAS.Application.Features.Subscription.GetAll;
using WAS.Domain.Enum;

namespace WAS.Application.Interface.Services
{
    public interface IGenerateExpression
    {

        Expression BuildExpression(ParameterExpression paramExpr, Rule rule, IDictionary<string, string> ColumnMapping);

        Expression GetExpressionWithMethod(string operatorName, object propertyValue, MemberExpression propertyName);

        Expression GetExpressionForStringEquality(string operatorName, object propertyValue, MemberExpression propertyName, string fieldName);

        Expression GetExpressionForName(Rule rule, Expression Expression1, Expression Expression2);

        Expression GetExpressionForDateTime(Rule newRule, Expression Expression1, Expression Expression2);

        Expression GetExpressionForDateTimeEquality(string operatorName, object propertyValue, MemberExpression propertyName, string fieldName);

        Expression GetExpressionFromOperation(Rule rule, object propertyValue, MemberExpression propertyName, UnaryExpression valueExpression, Type propertyType);

        object ReturnPropertyValue(Rule rule, TypeConverter converter);
    }
}
;