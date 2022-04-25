﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Simple.OData.Client.WestInternal.Extensions;

namespace Simple.OData.Client.WestInternal
{
    /// <summary>
    /// Provides access to OData operations in a fluent style.
    /// </summary>
    /// <typeparam name="T">The entity type.</typeparam>
    /// <typeparam name="FT"></typeparam>
    public abstract partial class FluentClientBase<T, FT> : IFluentClient<T, FT>
        where T : class
        where FT : class
    {
#pragma warning disable 1591

        protected readonly ODataClient _client;
        internal readonly Session _session;
        protected readonly FluentCommand _parentCommand;
        protected FluentCommand _command;
        protected readonly bool _dynamicResults;

        internal FluentClientBase(ODataClient client, Session session,
            FluentCommand parentCommand = null, FluentCommand command = null, bool dynamicResults = false)
        {
            _client = client;
            _session = session;
            _parentCommand = parentCommand;
            _command = command;
            _dynamicResults = dynamicResults;
        }

        internal FluentCommand Command
        {
            get
            {
                if (_command != null)
                    return _command;

                lock (this)
                {
                    return _command ?? (_command = CreateCommand());
                }
            }
        }

        protected FluentCommand CreateCommand()
        {
            return new FluentCommand(this.Session, _parentCommand, _client.BatchEntries);
        }

        internal Session Session => _session;

        protected ITypeCache TypeCache => _session.TypeCache;

        public FT WithProperties(Expression<Func<T, IDictionary<string, object>>> expression)
        {
            this.Command.WithProperties(expression.ExtractColumnName(Command.TypeCache));
            return this as FT;
        }

        public FT WithMedia(IEnumerable<string> properties)
        {
            this.Command.WithMedia(properties);
            return this as FT;
        }

        public FT WithMedia(params string[] properties)
        {
            this.Command.WithMedia(properties);
            return this as FT;
        }

        public FT WithMedia(params ODataExpression[] properties)
        {
            this.Command.WithMedia(properties);
            return this as FT;
        }

        public FT WithMedia(Expression<Func<T, object>> expression)
        {
            this.Command.WithMedia(expression.ExtractColumnNames(Command.TypeCache));
            return this as FT;
        }

        public FT Key(params object[] entryKey)
        {
            this.Command.Key(entryKey);
            return this as FT;
        }

        public FT Key(IEnumerable<object> entryKey)
        {
            this.Command.Key(entryKey);
            return this as FT;
        }

        public FT Key(IDictionary<string, object> entryKey)
        {
            this.Command.Key(entryKey);
            return this as FT;
        }

        public FT Key(T entryKey)
        {
            this.Command.Key(entryKey.ToDictionary(Command.TypeCache));
            return this as FT;
        }

        public FT Filter(string filter)
        {
            this.Command.Filter(filter);
            return this as FT;
        }

        public FT Filter(ODataExpression expression)
        {
            this.Command.Filter(expression);
            return this as FT;
        }

        public FT Filter(Expression<Func<T, bool>> expression)
        {
            this.Command.Filter(ODataExpression.FromLinqExpression(expression.Body));
            return this as FT;
        }

        public FT Search(string search)
        {
            this.Command.Search(search);
            return this as FT;
        }

        public FT Function(string functionName)
        {
            this.Command.Function(functionName);
            return this as FT;
        }

        public IBoundClient<U> Function<U>(string functionName) where U : class
        {
            this.Command.Function(functionName);
            return new BoundClient<U>(_client, _session, _parentCommand, this.Command, _dynamicResults);
        }

        public FT Action(string actionName)
        {
            this.Command.Action(actionName);
            return this as FT;
        }

        public FT Skip(long count)
        {
            this.Command.Skip(count);
            return this as FT;
        }

        public FT Top(long count)
        {
            this.Command.Top(count);
            return this as FT;
        }

        public FT Expand(ODataExpandOptions expandOptions)
        {
            this.Command.Expand(expandOptions);
            return this as FT;
        }

        public FT Expand(IEnumerable<string> associations)
        {
            this.Command.Expand(associations);
            return this as FT;
        }

        public FT Expand(ODataExpandOptions expandOptions, IEnumerable<string> associations)
        {
            this.Command.Expand(expandOptions, associations);
            return this as FT;
        }

        public FT Expand(params string[] associations)
        {
            this.Command.Expand(associations);
            return this as FT;
        }

        public FT Expand(ODataExpandOptions expandOptions, params string[] associations)
        {
            this.Command.Expand(expandOptions, associations);
            return this as FT;
        }

        public FT Expand(params ODataExpression[] associations)
        {
            this.Command.Expand(associations);
            return this as FT;
        }

        public FT Expand(ODataExpandOptions expandOptions, params ODataExpression[] associations)
        {
            this.Command.Expand(expandOptions, associations);
            return this as FT;
        }

        public FT Expand(Expression<Func<T, object>> expression)
        {
            this.Command.Expand(expression.ExtractColumnNames(Command.TypeCache));
            return this as FT;
        }

        public FT Expand(ODataExpandOptions expandOptions, Expression<Func<T, object>> expression)
        {
            this.Command.Expand(expandOptions, expression.ExtractColumnNames(Command.TypeCache));
            return this as FT;
        }

        public FT Select(IEnumerable<string> columns)
        {
            this.Command.Select(columns);
            return this as FT;
        }

        public FT Select(params string[] columns)
        {
            this.Command.Select(columns);
            return this as FT;
        }

        public FT Select(params ODataExpression[] columns)
        {
            this.Command.Select(columns);
            return this as FT;
        }

        public FT Select(Expression<Func<T, object>> expression)
        {
            this.Command.Select(expression.ExtractColumnNames(Command.TypeCache));
            return this as FT;
        }

        public FT OrderBy(IEnumerable<KeyValuePair<string, bool>> columns)
        {
            this.Command.OrderBy(columns);
            return this as FT;
        }

        public FT OrderBy(params string[] columns)
        {
            this.Command.OrderBy(columns);
            return this as FT;
        }

        public FT OrderBy(params ODataExpression[] columns)
        {
            this.Command.OrderBy(columns);
            return this as FT;
        }

        public FT OrderBy(Expression<Func<T, object>> expression)
        {
            this.Command.OrderBy(expression.ExtractColumnNames(Command.TypeCache).Select(x => new KeyValuePair<string, bool>(x, false)));
            return this as FT;
        }

        public FT ThenBy(params ODataExpression[] columns)
        {
            this.Command.ThenBy(columns);
            return this as FT;
        }

        public FT ThenBy(Expression<Func<T, object>> expression)
        {
            this.Command.ThenBy(expression.ExtractColumnNames(TypeCache).ToArray());
            return this as FT;
        }

        public FT OrderByDescending(params string[] columns)
        {
            this.Command.OrderByDescending(columns);
            return this as FT;
        }

        public FT OrderByDescending(params ODataExpression[] columns)
        {
            this.Command.OrderByDescending(columns);
            return this as FT;
        }

        public FT OrderByDescending(Expression<Func<T, object>> expression)
        {
            this.Command.OrderBy(expression.ExtractColumnNames(Command.TypeCache).Select(x => new KeyValuePair<string, bool>(x, true)));
            return this as FT;
        }

        public FT ThenByDescending(params ODataExpression[] columns)
        {
            this.Command.ThenByDescending(columns);
            return this as FT;
        }

        public FT ThenByDescending(Expression<Func<T, object>> expression)
        {
            this.Command.ThenByDescending(expression.ExtractColumnNames(Command.TypeCache).ToArray());
            return this as FT;
        }

        public FT QueryOptions(string queryOptions)
        {
            this.Command.QueryOptions(queryOptions);
            return this as FT;
        }

        public FT QueryOptions(IDictionary<string, object> queryOptions)
        {
            this.Command.QueryOptions(queryOptions);
            return this as FT;
        }

        public FT QueryOptions(ODataExpression expression)
        {
            this.Command.QueryOptions(expression);
            return this as FT;
        }

        public FT QueryOptions<U>(Expression<Func<U, bool>> expression)
        {
            this.Command.QueryOptions(ODataExpression.FromLinqExpression(expression.Body));
            return this as FT;
        }

        public IMediaClient Media()
        {
            this.Command.Media();
            return new MediaClient(_client, _session, this.Command, _dynamicResults);
        }

        public IMediaClient Media(string streamName)
        {
            this.Command.Media(streamName);
            return new MediaClient(_client, _session, this.Command, _dynamicResults);
        }

        public IMediaClient Media(ODataExpression expression)
        {
            this.Command.Media(expression);
            return new MediaClient(_client, _session, this.Command, _dynamicResults);
        }

        public IMediaClient Media(Expression<Func<T, object>> expression)
        {
            this.Command.Media(expression.ExtractColumnName(Command.TypeCache));
            return new MediaClient(_client, _session, this.Command, _dynamicResults);
        }

        public FT Count()
        {
            this.Command.Count();
            return this as FT;
        }

        protected BoundClient<U> Link<U>(FluentCommand command, string linkName = null)
        where U : class
        {
            linkName = linkName ?? typeof(U).Name;
            var links = linkName.Split('/');
            var linkCommand = command;
            BoundClient<U> linkedClient = null;
            foreach (var link in links)
            {
                linkedClient = new BoundClient<U>(_client, _session, linkCommand, null, _dynamicResults);
                linkedClient.Command.Link(link);
                linkCommand = linkedClient.Command;
            }
            return linkedClient;
        }

        protected BoundClient<U> Link<U>(FluentCommand command, ODataExpression expression)
        where U : class
        {
            return Link<U>(command, expression.Reference);
        }

#pragma warning restore 1591

        /// <summary>
        /// Navigates to the linked entity.
        /// </summary>
        /// <typeparam name="U">The type of the linked entity.</typeparam>
        /// <param name="linkName">Name of the link.</param>
        /// <returns>Self.</returns>
        public IBoundClient<U> NavigateTo<U>(string linkName = null)
            where U : class
        {
            return this.Link<U>(this.Command, linkName);
        }
        /// <summary>
        /// Navigates to the linked entity.
        /// </summary>
        /// <typeparam name="U">The type of the linked entity.</typeparam>
        /// <param name="expression">The expression for the link.</param>
        /// <returns>Self.</returns>
        public IBoundClient<U> NavigateTo<U>(Expression<Func<T, U>> expression)
            where U : class
        {
            return this.Link<U>(this.Command, expression.ExtractColumnName(Command.TypeCache));
        }
        /// <summary>
        /// Navigates to the linked entity.
        /// </summary>
        /// <typeparam name="U">The type of the linked entity.</typeparam>
        /// <param name="expression">The expression for the link.</param>
        /// <returns>Self.</returns>
        public IBoundClient<U> NavigateTo<U>(Expression<Func<T, IEnumerable<U>>> expression) where U : class
        {
            return this.Link<U>(this.Command, expression.ExtractColumnName(Command.TypeCache));
        }
        /// <summary>
        /// Navigates to the linked entity.
        /// </summary>
        /// <typeparam name="U">The type of the linked entity.</typeparam>
        /// <param name="expression">The expression for the link.</param>
        /// <returns>Self.</returns>
        public IBoundClient<U> NavigateTo<U>(Expression<Func<T, IList<U>>> expression) where U : class
        {
            return this.Link<U>(this.Command, expression.ExtractColumnName(Command.TypeCache));
        }
        /// <summary>
        /// Navigates to the linked entity.
        /// </summary>
        /// <typeparam name="U">The type of the linked entity.</typeparam>
        /// <param name="expression">The expression for the link.</param>
        /// <returns>Self.</returns>
        public IBoundClient<U> NavigateTo<U>(Expression<Func<T, ISet<U>>> expression) where U : class
        {
            return this.Link<U>(this.Command, expression.ExtractColumnName(Command.TypeCache));
        }
        /// <summary>
        /// Navigates to the linked entity.
        /// </summary>
        /// <typeparam name="U">The type of the linked entity.</typeparam>
        /// <param name="expression">The expression for the link.</param>
        /// <returns>Self.</returns>
        public IBoundClient<U> NavigateTo<U>(Expression<Func<T, HashSet<U>>> expression) where U : class
        {
            return this.Link<U>(this.Command, expression.ExtractColumnName(Command.TypeCache));
        }
        /// <summary>
        /// Navigates to the linked entity.
        /// </summary>
        /// <typeparam name="U">The type of the linked entity.</typeparam>
        /// <param name="expression">The expression for the link.</param>
        /// <returns>Self.</returns>
        public IBoundClient<U> NavigateTo<U>(Expression<Func<T, U[]>> expression) where U : class
        {
            return this.Link<U>(this.Command, expression.ExtractColumnName(Command.TypeCache));
        }
        /// <summary>
        /// Navigates to the linked entity.
        /// </summary>
        /// <param name="linkName">Name of the link.</param>
        /// <returns>Self.</returns>        
        public IBoundClient<IDictionary<string, object>> NavigateTo(string linkName)
        {
            return this.Link<IDictionary<string, object>>(this.Command, linkName);
        }
        /// <summary>
        /// Navigates to the linked entity.
        /// </summary>
        /// <param name="expression">The expression for the link.</param>
        /// <returns>Self.</returns>
        public IBoundClient<T> NavigateTo(ODataExpression expression)
        {
            return this.Link<T>(this.Command, expression);
        }

        /// <summary>
        /// Executes the OData function or action.
        /// </summary>
        /// <returns>Execution result task.</returns>
        public Task ExecuteAsync()
        {
            return _client.ExecuteAsync(_command, CancellationToken.None);
        }
        /// <summary>
        /// Executes the OData function or action.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Execution result task.</returns>
        public Task ExecuteAsync(CancellationToken cancellationToken)
        {
            return _client.ExecuteAsync(_command, cancellationToken);
        }

        /// <summary>
        /// Executes the OData function or action and returns a single item.
        /// </summary>
        /// <returns>Execution result.</returns>
        public Task<T> ExecuteAsSingleAsync()
        {
            return ExecuteAsSingleAsync(CancellationToken.None);
        }
        /// <summary>
        /// Executes the OData function or action and returns a single item.
        /// </summary>
        /// <returns>Execution result.</returns>
        public Task<U> ExecuteAsSingleAsync<U>()
        {
            return ExecuteAsSingleAsync<U>(CancellationToken.None);
        }
        /// <summary>
        /// Executes the OData function or action and returns a single item.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Execution result.</returns>
        public Task<T> ExecuteAsSingleAsync(CancellationToken cancellationToken)
        {
            return FilterAndTypeColumnsAsync(
                _client.ExecuteAsSingleAsync(_command, cancellationToken),
                _command.SelectedColumns, _command.DynamicPropertiesContainerName);
        }
        /// <summary>
        /// Executes the OData function or action and returns a single item.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Execution result.</returns>
        public async Task<U> ExecuteAsSingleAsync<U>(CancellationToken cancellationToken)
        {
            return (await ExecuteAsArrayAsync<U>(cancellationToken)).Single();
        }

        /// <summary>
        /// Executes the OData function or action and returns enumerable result.
        /// </summary>
        /// <returns>Execution result.</returns>
        public Task<IEnumerable<T>> ExecuteAsEnumerableAsync()
        {
            return FilterAndTypeColumnsAsync(
                _client.ExecuteAsEnumerableAsync(_command, CancellationToken.None),
                _command.SelectedColumns, _command.DynamicPropertiesContainerName);
        }
        /// <summary>
        /// Executes the OData function or action and returns enumerable result.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Execution result.</returns>
        public Task<IEnumerable<T>> ExecuteAsEnumerableAsync(CancellationToken cancellationToken)
        {
            return FilterAndTypeColumnsAsync(
                _client.ExecuteAsEnumerableAsync(_command, cancellationToken),
                _command.SelectedColumns, _command.DynamicPropertiesContainerName);
        }

        /// <summary>
        /// Executes the OData function or action and returns scalar result.
        /// </summary>
        /// <returns>Execution result.</returns>
        public Task<U> ExecuteAsScalarAsync<U>()
        {
            return _client.ExecuteAsScalarAsync<U>(_command, CancellationToken.None);
        }
        /// <summary>
        /// Executes the OData function or action and returns scalar result.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Execution result.</returns>
        public Task<U> ExecuteAsScalarAsync<U>(CancellationToken cancellationToken)
        {
            return _client.ExecuteAsScalarAsync<U>(_command, cancellationToken);
        }

        /// <summary>
        /// Executes the OData function and returns an array.
        /// </summary>
        /// <returns>Execution result.</returns>
        public Task<U[]> ExecuteAsArrayAsync<U>()
        {
            return ExecuteAsArrayAsync<U>(CancellationToken.None);
        }

        /// <summary>
        /// Executes the OData function and returns an array.
        /// </summary>
        /// <returns>Execution result.</returns>
        public Task<T[]> ExecuteAsArrayAsync()
        {
            return ExecuteAsArrayAsync<T>(CancellationToken.None);
        }

        /// <summary>
        /// Executes the OData function and returns an array.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Execution result.</returns>
        public Task<U[]> ExecuteAsArrayAsync<U>(CancellationToken cancellationToken)
        {
            return _client.ExecuteAsArrayAsync<U>(_command, cancellationToken);
        }

        /// <summary>
        /// Executes the OData function and returns an array.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Execution result.</returns>
        public Task<T[]> ExecuteAsArrayAsync(CancellationToken cancellationToken)
        {
            return ExecuteAsArrayAsync<T>(cancellationToken);
        }

        /// <summary>
        /// Gets the OData command text.
        /// </summary>
        /// <returns>The command text.</returns>
        public Task<string> GetCommandTextAsync()
        {
            return GetCommandTextAsync(CancellationToken.None);
        }
        /// <summary>
        /// Gets the OData command text.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The command text.</returns>
        public Task<string> GetCommandTextAsync(CancellationToken cancellationToken)
        {
            return this.Command.GetCommandTextAsync(cancellationToken);
        }

#pragma warning disable 1591

        protected async Task<IEnumerable<T>> FilterAndTypeColumnsAsync(
            Task<IEnumerable<IDictionary<string, object>>> entries, IList<string> selectedColumns, string dynamicPropertiesContainerName)
        {
            var result = FilterColumns(await entries.ConfigureAwait(false), selectedColumns);
            return result?.Select(z => ConvertResult(z, dynamicPropertiesContainerName));
        }

        protected async Task<T> FilterAndTypeColumnsAsync(
            Task<IDictionary<string, object>> entry, IList<string> selectedColumns, string dynamicPropertiesContainerName)
        {
            return ConvertResult(FilterColumns(await entry.ConfigureAwait(false), selectedColumns), dynamicPropertiesContainerName);
        }

        protected async Task<Tuple<IEnumerable<T>, int>> FilterAndTypeColumnsAsync(
            Task<Tuple<IEnumerable<IDictionary<string, object>>, int>> entries, IList<string> selectedColumns, string dynamicPropertiesContainerName)
        {
            var result = await entries.ConfigureAwait(false);
            return new Tuple<IEnumerable<T>, int>(
                FilterColumns(result.Item1, selectedColumns).Select(y => ConvertResult(y, dynamicPropertiesContainerName)),
                result.Item2);
        }

        protected IEnumerable<IDictionary<string, object>> FilterColumns(IEnumerable<IDictionary<string, object>> entries, IList<string> selectedColumns)
        {
            return entries?.Select(x => FilterColumns(x, selectedColumns));
        }

        protected IDictionary<string, object> FilterColumns(IDictionary<string, object> entry, IList<string> selectedColumns)
        {
            if (entry == null || selectedColumns == null || !selectedColumns.Any())
            {
                return entry;
            }
            else
            {
                return entry.Where(x => selectedColumns.Any(y => IsSelectedColumn(x, y))).ToIDictionary();
            }
        }

        private T ConvertResult(IDictionary<string, object> result, string dynamicPropertiesContainerName)
        {
            if (!string.IsNullOrEmpty(dynamicPropertiesContainerName))
            {
                TypeCache.Register<T>(dynamicPropertiesContainerName);
            }

            if (result != null && result.Keys.Count == 1 && result.ContainsKey(FluentCommand.ResultLiteral) &&
                TypeCache.IsValue(typeof(T)) || typeof(T) == typeof(string) || typeof(T) == typeof(object))
                return TypeCache.Convert<T>(result.Values.First());
            else
                return result.ToObject<T>(TypeCache, _dynamicResults);
        }

        private bool IsSelectedColumn(KeyValuePair<string, object> kv, string columnName)
        {
            var items = columnName.Split('/');
            if (items.Count() == 1)
            {
                return _session.Settings.NameMatchResolver.IsMatch(kv.Key, columnName);
            }
            else
            {
                var item = items.First();
                return _session.Settings.NameMatchResolver.IsMatch(kv.Key, item) &&
                       (kv.Value is IDictionary<string, object> && (kv.Value as IDictionary<string, object>)
                            .Any(x => IsSelectedColumn(x, string.Join("/", items.Skip(1)))) ||
                        kv.Value is IEnumerable<object> && (kv.Value as IEnumerable<object>)
                            .Any(x => x is IDictionary<string, object> && (x as IDictionary<string, object>)
                                .Any(y => IsSelectedColumn(y, string.Join("/", items.Skip(1))))));
            }
        }

#pragma warning restore 1591

    }
}