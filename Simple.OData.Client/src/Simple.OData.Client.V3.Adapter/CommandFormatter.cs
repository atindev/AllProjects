﻿using System;
using System.Collections.Generic;

using Microsoft.Data.OData;
using Microsoft.Data.OData.Query;

namespace Simple.OData.Client.WestInternal.V3.Adapter
{
    public class CommandFormatter : CommandFormatterBase
    {
        public CommandFormatter(ISession session)
            : base(session)
        {            
        }

        public override FunctionFormat FunctionFormat => FunctionFormat.Query;

        public override string ConvertValueToUriLiteral(object value, bool escapeDataString)
        {
            if (value != null && _session.TypeCache.IsEnumType(value.GetType()))
                value = Convert.ToInt32(value);
            if (value is ODataExpression expression)
                return expression.AsString(_session);

            var odataVersion = (ODataVersion) Enum.Parse(typeof (ODataVersion), _session.Adapter.GetODataVersionString(), false);
            string ConvertValue(object x) => ODataUriUtils.ConvertToUriLiteral(x, odataVersion, (_session.Adapter as ODataAdapter).Model);

            return escapeDataString
                ? Uri.EscapeDataString(ConvertValue(value))
                : ConvertValue(value);
        }

        protected override void FormatExpandSelectOrderby(IList<string> commandClauses, EntityCollection resultCollection, FluentCommand command)
        {
            FormatClause(commandClauses, resultCollection, command.Details.ExpandAssociations, ODataLiteral.Expand, FormatExpandItem);
            FormatClause(commandClauses, resultCollection, command.Details.SelectColumns, ODataLiteral.Select, FormatSelectItem);
            FormatClause(commandClauses, resultCollection, command.Details.OrderbyColumns, ODataLiteral.OrderBy, FormatOrderByItem);
        }

        protected override void FormatInlineCount(IList<string> commandClauses)
        {
            commandClauses.Add($"{ODataLiteral.InlineCount}={ODataLiteral.AllPages}");
        }
    }
}