using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using WAS.Application.Common.Constants;
using WAS.Application.Common.Exceptions;
using WAS.Application.Common.Models;
using WAS.Application.Features.Subscription.GetAll;
using WAS.Application.Interface.Services;
using WAS.Domain.Enum;

namespace WAS.Infrastructure.Services
{
    public class GenerateExpression : IGenerateExpression
    {
        private readonly ILogger<GenerateExpression> _logger;

        public GenerateExpression(
           ILogger<GenerateExpression> logger
           )
        {
            _logger = logger;
        }

        /// <summary>
        /// Get Dynamic LINQ expression based on the inputs
        /// <param name="paramExpr">for which Entity we are generating expression</param>
        /// <param name="FilterTypes">Differnet filters need to apply for the Query</param>
        /// <param name="FilterConditions">Differnet Filter Conditions (=,<>,contains)</param>
        /// <param name="FilterValues">Differnet Filter Values for the filter types</param>
        /// <param name="ColumnMapping">Column mapping for the sql tables with input FilterTypes</param>
        /// <returns>Dynamic Linq Expression</returns>
        

        public Expression BuildExpression(ParameterExpression paramExpr, Rule rule, IDictionary<string, string> ColumnMapping)
        {
            string MappedColumnName = "";
            Expression expressionSub = null, expressionMain = null;
            IList<ExpressionClass> ExpressionArray = new List<ExpressionClass>();
            try
            {
                if (rule.condition != null)
                {
                    if (rule.condition != "or" && rule.condition != "and")
                        throw new Exception($"Condition [{rule.condition}] is invalid.");

                    if (rule.rules == null)
                        throw new Exception($"Cannot create [{rule.condition}] expression.");

                    //if more than one condition
                    if (rule.rules.Count > 1)
                    {

                        for (int i = 0; i < rule.rules.Count; i++)
                        {
                            Expression current = BuildExpression(paramExpr, rule.rules[i], GroupColumnMaping.getColumnMapping());

                            //overrided for first name and last name
                            if (rule.rules[i].label == "Name")
                            {
                                Rule newRule = rule.rules[i];
                                newRule.field = "LastName";
                                Expression additioanlOr = BuildExpression(paramExpr, newRule, GroupColumnMaping.getColumnMapping());
                                current = GetExpressionForName(newRule, current, additioanlOr);

                            }
                            else if (rule.rules[i].label == "SubscribedOn" && rule.rules[i].@operator == "equal")
                            {
                                Rule newRule = rule.rules[i];
                                newRule.field = "SubscribedOn";
                                newRule.@operator = "lessthanorequal";
                                Expression additioanlOr = BuildExpression(paramExpr, newRule, GroupColumnMaping.getColumnMapping());
                                current = GetExpressionForDateTime(newRule,current, additioanlOr);
                            }
                            else if (rule.rules[i].label == "SubscribedOn" && rule.rules[i].@operator == "notequal")
                            {
                                Rule newRule = rule.rules[i];
                                newRule.field = "SubscribedOn";
                                newRule.@operator = "greaterthan";
                                Expression additioanlOr = BuildExpression(paramExpr, newRule, GroupColumnMaping.getColumnMapping());
                                current = GetExpressionForDateTime(newRule,current, additioanlOr);
                            }

                            ExpressionArray.Add(
                                new ExpressionClass()
                                {
                                    rules = rule,
                                    expression = current
                                });
                        }

                        for (int j = 0; j < ExpressionArray.Count; j++)
                        {
                            if (ExpressionArray[j].expression == null)
                                continue;
                            if (expressionMain == null)
                                expressionMain = ExpressionArray[j].expression;
                            else
                            {
                                if (ExpressionArray[j].rules.condition == "or")
                                    expressionMain = Expression.Or(expressionMain, ExpressionArray[j].expression);
                                else
                                    expressionMain = Expression.And(expressionMain, ExpressionArray[j].expression);
                            }
                        }

                        return expressionMain;

                    }
                    //single condition
                    else
                    {
                        Expression current = BuildExpression(paramExpr, rule.rules[0], GroupColumnMaping.getColumnMapping());

                        //overrided for first name and last name
                        if (rule.rules[0].label == "Name")
                        {
                            Rule newRule = rule.rules[0];
                            newRule.field = "LastName";
                            Expression additioanlOr = BuildExpression(paramExpr, newRule, GroupColumnMaping.getColumnMapping());
                            current = GetExpressionForName(newRule, current, additioanlOr);
                        }
                        else if(rule.rules[0].label == "SubscribedOn" &&rule.rules[0].@operator == "equal")
                        {
                            Rule newRule = rule.rules[0];
                            newRule.field = "SubscribedOn";
                            newRule.@operator = "lessthanorequal";
                            Expression additioanlOr = BuildExpression(paramExpr, newRule, GroupColumnMaping.getColumnMapping());
                            current = GetExpressionForDateTime(newRule,current, additioanlOr);
                        }
                        else if (rule.rules[0].label == "SubscribedOn" && rule.rules[0].@operator == "notequal")
                        {
                            Rule newRule = rule.rules[0];
                            newRule.field = "SubscribedOn";
                            newRule.@operator = "greaterthan";
                            Expression additioanlOr = BuildExpression(paramExpr, newRule, GroupColumnMaping.getColumnMapping());
                            current = GetExpressionForDateTime(newRule,current, additioanlOr);
                        }

                        return current;
                    }

                }
                else
                {
                    if (rule.field != null && rule.field != "" && ColumnMapping.TryGetValue(Convert.ToString(rule.field), out MappedColumnName))
                    {

                        var propertyName = Expression.Property(paramExpr, MappedColumnName);

                        //for null
                        if (rule.@operator == "isnull" || rule.@operator == "isnotnull")
                        {

                            string propertyTypeName = propertyName.Type.Name;

                            if (propertyName.Type.IsPrimitive && !propertyTypeName.Contains("Nullable"))
                            {
                                var nullconstant = Expression.Constant(1, typeof(int));
                                if (rule.@operator == "isnull")
                                    return expressionSub = Expression.NotEqual(nullconstant, nullconstant);
                                else
                                    return expressionSub = Expression.Equal(nullconstant, nullconstant);
                            }
                            else
                            {
                                var nullconstant = Expression.Constant(null, typeof(object));
                                if (rule.@operator == "isnull")
                                    return expressionSub = Expression.Equal(propertyName, nullconstant);
                                else
                                    return expressionSub = Expression.NotEqual(propertyName, nullconstant);

                            }

                        }
                        //for ANY
                        else if (rule.value == "0" || rule.value == "Any")
                        {
                            var anyConstant = Expression.Constant(1, typeof(int));
                            if (rule.@operator == "notequal")
                                return expressionSub = Expression.NotEqual(anyConstant, anyConstant);
                            else
                                return expressionSub = Expression.Equal(anyConstant, anyConstant);

                        }

                        //for type convertion start
                        var propertyType = ((PropertyInfo)propertyName.Member).PropertyType;
                        var converter = TypeDescriptor.GetConverter(propertyType);
                        if (!converter.CanConvertFrom(typeof(string)))
                            throw new NotSupportedException();
                        var propertyValue = ReturnPropertyValue(rule, converter);
                        var constant = Expression.Constant(propertyValue);
                        var valueExpression = Expression.Convert(constant, propertyType);
                        return GetExpressionFromOperation(rule, propertyValue, propertyName, valueExpression, propertyType);
                    }
                    return expressionSub;

                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new BadRequestException(ex.Message, ex);
            }
        }

        public Expression GetExpressionWithMethod(string operatorName, object propertyValue, MemberExpression propertyName)
        {
            Expression expressionSub = null;
            MethodInfo method = typeof(string).GetMethod(operatorName, new[] { typeof(string), typeof(StringComparison) });
            var comparisonValue = Expression.Constant(StringComparison.OrdinalIgnoreCase, typeof(StringComparison));
            var actualValue = Expression.Constant(propertyValue, typeof(string));
            var coallesceExpression = Expression.Coalesce(propertyName, Expression.Constant(""));

            expressionSub = Expression.Call(coallesceExpression, method, actualValue, comparisonValue);
            return expressionSub;

        }

        public Expression GetExpressionForStringEquality(string operatorName, object propertyValue, MemberExpression propertyName, string fieldName)
        {
            MethodInfo method = typeof(String).GetMethod("ToLower", Type.EmptyTypes);
            var coallesceExpression = Expression.Coalesce(propertyName, Expression.Constant(""));
            var dynamicExpression = Expression.Call(coallesceExpression, method);
            Expression constExp = Expression.Constant(propertyValue.ToString().ToLower());

            if (operatorName == "equal")
                return Expression.Equal(dynamicExpression, constExp);
            else if (operatorName == "notequal")
                return Expression.NotEqual(dynamicExpression, constExp);
            else
                return null;

        }

        public Expression GetExpressionForDateTimeEquality(string operatorName, object propertyValue, MemberExpression propertyName, string fieldName)
        {

            if (operatorName == "equal")
            {
                var propertyType = ((PropertyInfo)propertyName.Member).PropertyType;
                var constant = Expression.Constant(propertyValue);
                var valueExpression = Expression.Convert(constant, propertyType);
                return Expression.GreaterThanOrEqual(propertyName, valueExpression);
            }
            else if (operatorName == "notequal")
            {
                var propertyType = ((PropertyInfo)propertyName.Member).PropertyType;
                var constant = Expression.Constant(propertyValue);
                var valueExpression = Expression.Convert(constant, propertyType);
                return Expression.LessThanOrEqual(propertyName, valueExpression);
            }
            else if (operatorName == "greaterthan" )
            {
                DateTime date = Convert.ToDateTime(propertyValue.ToString());
                propertyValue = date.AddDays(1);
                var propertyType = ((PropertyInfo)propertyName.Member).PropertyType;
                var constant = Expression.Constant(propertyValue);
                var valueExpression = Expression.Convert(constant, propertyType);
                return Expression.GreaterThan(propertyName, valueExpression);
            }
            else if( operatorName == "lessthanorequal")
            {
                DateTime date = Convert.ToDateTime(propertyValue.ToString());
                propertyValue = date.AddDays(1);
                var propertyType = ((PropertyInfo)propertyName.Member).PropertyType;
                var constant = Expression.Constant(propertyValue);
                var valueExpression = Expression.Convert(constant, propertyType);
                return Expression.LessThanOrEqual(propertyName, valueExpression);
            }
            else
            {
                return null;
            }
        }

        public object ReturnPropertyValue(Rule rule, TypeConverter converter)
        {

            if ((rule.field == "Name" || rule.field == "FirstName" || rule.field == "LastName") && (rule.value.Trim().Split(" ").Length > 1))
            {
                string field = "";
                if (rule.field == "Name" || rule.field == "FirstName")
                {
                    field = rule.value.Trim().Split(" ")[0];
                }
                else
                {
                    field = rule.value.Remove(0, rule.value.IndexOf(' ') + 1);

                }

                return converter.ConvertFromInvariantString(field);

            }

            else
                return converter.ConvertFromInvariantString(rule.value);

        }

        public Expression GetExpressionForName(Rule newRule, Expression Expression1, Expression Expression2)
        {
            if (newRule.@operator == "notequal" || newRule.@operator == "isnull" || newRule.value.Trim().Split(" ").Length > 1)
                return Expression.And(Expression1, Expression2);
            else
                return Expression.Or(Expression1, Expression2);


        }
        public Expression GetExpressionForDateTime(Rule newRule,Expression Expression1, Expression Expression2)
        {
            if (newRule.@operator == "lessthanorequal")
            {
                return Expression.And(Expression1, Expression2);
            }
            else
            {
                return Expression.Or(Expression1, Expression2);
            }
        }
        public Expression GetExpressionFromOperation(Rule rule, object propertyValue, MemberExpression propertyName,UnaryExpression valueExpression,Type propertyType)
        {
            switch (rule.@operator)
            {
                case "equal":

                    if (propertyType.Name.ToLower() == "string")
                    {
                        return GetExpressionForStringEquality("equal", propertyValue, propertyName, rule.field);
                    }
                    else if (propertyType.Name.ToLower() == "datetime")
                    {
                        return GetExpressionForDateTimeEquality("equal", propertyValue, propertyName, rule.field);
                    }
                    else
                    {
                        return Expression.Equal(propertyName, valueExpression);
                    }
                case "notequal":

                    if (propertyType.Name.ToLower() == "string")
                    {
                        return GetExpressionForStringEquality("notequal", propertyValue, propertyName, rule.field);
                    }
                    else if (propertyType.Name.ToLower() == "datetime")
                    {
                        return GetExpressionForDateTimeEquality("notequal", propertyValue, propertyName, rule.field);
                    }
                    else
                    {
                        return Expression.NotEqual(propertyName, valueExpression);
                    }

                case "greaterthan":
                    if (propertyType.Name.ToLower() == "datetime")
                    {
                        return GetExpressionForDateTimeEquality("greaterthan", propertyValue, propertyName, rule.field);
                    }
                    else
                    {
                        return Expression.GreaterThan(propertyName, valueExpression);
                    }
                case "greaterthanorequal": return Expression.GreaterThanOrEqual(propertyName, valueExpression);
                case "lessthan": return Expression.LessThan(propertyName, valueExpression);
                case "lessthanorequal":
                    if (propertyType.Name.ToLower() == "datetime")
                    {
                        return GetExpressionForDateTimeEquality("lessthanorequal", propertyValue, propertyName, rule.field);
                    }
                    else
                    {
                        return Expression.LessThanOrEqual(propertyName, valueExpression);
                    }
                case "contains":
                    return GetExpressionWithMethod("Contains", propertyValue, propertyName);
                case "startswith":
                    return GetExpressionWithMethod("StartsWith", propertyValue, propertyName);
                case "endswith":
                    return GetExpressionWithMethod("EndsWith", propertyValue, propertyName);

                default:
                    return null;
            }
        }
    }
}
