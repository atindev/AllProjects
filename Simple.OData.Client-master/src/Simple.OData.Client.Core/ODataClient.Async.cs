﻿using Simple.OData.Client.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Simple.OData.Client
{
    /// <summary>
    /// Provides access to OData operations.
    /// </summary>
    public partial class ODataClient
    {
        /// <summary>
        /// Retrieves the OData service metadata.
        /// </summary>
        /// <param name="baseUri">The URL base of the OData service.</param>
        /// <returns>The service metadata.</returns>
        public static Task<object> GetMetadataAsync(Uri baseUri)
        {
            return GetMetadataAsync(baseUri, null, CancellationToken.None);
        }

        /// <summary>
        /// Retrieves the OData service metadata.
        /// </summary>
        /// <param name="baseUri">The URL base of the OData service.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The service metadata.</returns>
        public static Task<object> GetMetadataAsync(Uri baseUri, CancellationToken cancellationToken)
        {
            return GetMetadataAsync(baseUri, null, cancellationToken);
        }

        /// <summary>
        /// Retrieves the OData service metadata.
        /// </summary>
        /// <param name="baseUri">The URL base of the OData service.</param>
        /// <param name="credentials">The OData service access credentials.</param>
        /// <returns>The service metadata.</returns>
        public static Task<object> GetMetadataAsync(Uri baseUri, ICredentials credentials)
        {
            return GetMetadataAsync(baseUri, credentials, CancellationToken.None);
        }

        /// <summary>
        /// Retrieves the OData service metadata.
        /// </summary>
        /// <param name="baseUri">The URL base of the OData service.</param>
        /// <param name="credentials">The OData service access credentials.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The service metadata.</returns>
        public static Task<object> GetMetadataAsync(Uri baseUri, ICredentials credentials, CancellationToken cancellationToken)
        {
            return GetMetadataAsync<object>(baseUri, credentials, cancellationToken);
        }

        /// <summary>
        /// Retrieves the OData service metadata.
        /// </summary>
        /// <typeparam name="T">OData protocol specific metadata interface</typeparam>
        /// <param name="baseUri">The URL base of the OData service.</param>
        /// <returns>The service metadata.</returns>
        public static Task<T> GetMetadataAsync<T>(Uri baseUri)
        {
            return GetMetadataAsync<T>(baseUri, null, CancellationToken.None);
        }

        /// <summary>
        /// Retrieves the OData service metadata.
        /// </summary>
        /// <typeparam name="T">OData protocol specific metadata interface</typeparam>
        /// <param name="baseUri">The URL base of the OData service.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The service metadata.</returns>
        public static Task<T> GetMetadataAsync<T>(Uri baseUri, CancellationToken cancellationToken)
        {
            return GetMetadataAsync<T>(baseUri, null, cancellationToken);
        }

        /// <summary>
        /// Retrieves the OData service metadata.
        /// </summary>
        /// <typeparam name="T">OData protocol specific metadata interface</typeparam>
        /// <param name="baseUri">The URL base of the OData service.</param>
        /// <param name="credentials">The OData service access credentials.</param>
        /// <returns>The service metadata.</returns>
        public static Task<T> GetMetadataAsync<T>(Uri baseUri, ICredentials credentials)
        {
            return GetMetadataAsync<T>(baseUri, credentials, CancellationToken.None);
        }

        /// <summary>
        /// Retrieves the OData service metadata.
        /// </summary>
        /// <typeparam name="T">OData protocol specific metadata interface</typeparam>
        /// <param name="baseUri">The URL base of the OData service.</param>
        /// <param name="credentials">The OData service access credentials.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// The service metadata.
        /// </returns>
        public static async Task<T> GetMetadataAsync<T>(Uri baseUri, ICredentials credentials, CancellationToken cancellationToken)
        {
            var session = Session.FromSettings(new ODataClientSettings(baseUri, credentials));
            await session.ResolveAdapterAsync(cancellationToken);//.ConfigureAwait(false);
            return (T)session.Adapter.Model;
        }

        /// <summary>
        /// Retrieves the OData service metadata as string.
        /// </summary>
        /// <param name="baseUri">The URL base of the OData service.</param>
        /// <returns>The service metadata.</returns>
        public static Task<string> GetMetadataDocumentAsync(Uri baseUri)
        {
            return GetMetadataDocumentAsync(baseUri, null, CancellationToken.None);
        }

        /// <summary>
        /// Gets The service metadata as string asynchronous.
        /// </summary>
        /// <param name="baseUri">The URL base of the OData service.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The service metadata.</returns>
        public static Task<string> GetMetadataDocumentAsync(Uri baseUri, CancellationToken cancellationToken)
        {
            return GetMetadataDocumentAsync(baseUri, null, CancellationToken.None);
        }

        /// <summary>
        /// Gets The service metadata as string asynchronous.
        /// </summary>
        /// <param name="baseUri">The URL base of the OData service.</param>
        /// <param name="credentials">The OData service access credentials.</param>
        /// <returns>The service metadata.</returns>
        public static Task<string> GetMetadataDocumentAsync(Uri baseUri, ICredentials credentials)
        {
            return GetMetadataDocumentAsync(baseUri, credentials, CancellationToken.None);
        }

        /// <summary>
        /// Gets The service metadata as string asynchronous.
        /// </summary>
        /// <param name="baseUri">The URL base.</param>
        /// <param name="credentials">The OData service access credentials.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The service metadata.</returns>
        public static async Task<string> GetMetadataDocumentAsync(Uri baseUri, ICredentials credentials, CancellationToken cancellationToken)
        {
            var session = Session.FromSettings(new ODataClientSettings(baseUri, credentials));
            await session.ResolveAdapterAsync(cancellationToken);//.ConfigureAwait(false);
            return session.MetadataCache.MetadataDocument;
        }

#pragma warning disable 1591

        internal async Task<Session> GetSessionAsync()
        {
            await _session.ResolveAdapterAsync(CancellationToken.None);//.ConfigureAwait(false);
            return _session;
        }

        public async Task<object> GetMetadataAsync()
        {
            return (await _session.ResolveAdapterAsync(CancellationToken.None).ConfigureAwait(false)).Model;
        }

        public async Task<object> GetMetadataAsync(CancellationToken cancellationToken)
        {
            return (await _session.ResolveAdapterAsync(cancellationToken).ConfigureAwait(false)).Model;
        }

        public async Task<T> GetMetadataAsync<T>()
        {
            return (T)(await _session.ResolveAdapterAsync(CancellationToken.None).ConfigureAwait(false)).Model;
        }

        public async Task<T> GetMetadataAsync<T>(CancellationToken cancellationToken)
        {
            return (T)(await _session.ResolveAdapterAsync(cancellationToken).ConfigureAwait(false)).Model;
        }

        public Task<string> GetMetadataAsStringAsync()
        {
            return GetMetadataDocumentAsync(CancellationToken.None);
        }

        public Task<string> GetMetadataDocumentAsync()
        {
            return GetMetadataDocumentAsync(CancellationToken.None);
        }

        public async Task<string> GetMetadataAsStringAsync(CancellationToken cancellationToken)
        {
            await _session.ResolveAdapterAsync(cancellationToken);//.ConfigureAwait(false);
            return _session.MetadataCache.MetadataDocument;
        }

        public async Task<string> GetMetadataDocumentAsync(CancellationToken cancellationToken)
        {
            await _session.ResolveAdapterAsync(cancellationToken);//.ConfigureAwait(false);
            return _session.MetadataCache.MetadataDocument;
        }

        public Task<string> GetCommandTextAsync(string collection, ODataExpression expression)
        {
            return GetCommandTextAsync(collection, expression, CancellationToken.None);
        }

        public async Task<string> GetCommandTextAsync(string collection, ODataExpression expression, CancellationToken cancellationToken)
        {
            await _session.ResolveAdapterAsync(cancellationToken);//.ConfigureAwait(false);
            if (cancellationToken.IsCancellationRequested) cancellationToken.ThrowIfCancellationRequested();

            return await GetBoundClient()
                .For(collection)
                .Filter(expression)
                .GetCommandTextAsync(cancellationToken);//.ConfigureAwait(false);
        }

        public Task<string> GetCommandTextAsync<T>(string collection, Expression<Func<T, bool>> expression)
        {
            return GetCommandTextAsync(collection, expression, CancellationToken.None);
        }

        public async Task<string> GetCommandTextAsync<T>(string collection, Expression<Func<T, bool>> expression, CancellationToken cancellationToken)
        {
            await _session.ResolveAdapterAsync(cancellationToken);//.ConfigureAwait(false);
            if (cancellationToken.IsCancellationRequested) cancellationToken.ThrowIfCancellationRequested();

            return await GetBoundClient()
                .For(collection)
                .Filter(ODataExpression.FromLinqExpression(expression.Body))
                .GetCommandTextAsync(cancellationToken);//.ConfigureAwait(false);
        }

        public Task<IEnumerable<IDictionary<string, object>>> FindEntriesAsync(string commandText)
        {
            return FindEntriesAsync(commandText, false, null, CancellationToken.None);
        }

        public Task<IEnumerable<IDictionary<string, object>>> FindEntriesAsync(string commandText, CancellationToken cancellationToken)
        {
            return FindEntriesAsync(commandText, false, null, cancellationToken);
        }

        public Task<IEnumerable<IDictionary<string, object>>> FindEntriesAsync(string commandText, bool scalarResult)
        {
            return FindEntriesAsync(commandText, scalarResult, null, CancellationToken.None);
        }

        public Task<IEnumerable<IDictionary<string, object>>> FindEntriesAsync(string commandText, bool scalarResult, CancellationToken cancellationToken)
        {
            return FindEntriesAsync(commandText, scalarResult, null, cancellationToken);
        }

        public Task<IEnumerable<IDictionary<string, object>>> FindEntriesAsync(string commandText, ODataFeedAnnotations annotations)
        {
            return FindEntriesAsync(commandText, false, annotations, CancellationToken.None);
        }

        public Task<IEnumerable<IDictionary<string, object>>> FindEntriesAsync(string commandText, ODataFeedAnnotations annotations, CancellationToken cancellationToken)
        {
            return FindEntriesAsync(commandText, false, annotations, cancellationToken);
        }

        public Task<IDictionary<string, object>> FindEntryAsync(string commandText)
        {
            return FindEntryAsync(commandText, CancellationToken.None);
        }

        public async Task<IDictionary<string, object>> FindEntryAsync(string commandText, CancellationToken cancellationToken)
        {
            if (IsBatchResponse)
                return _batchResponse.AsEntry(false);

            var requestBuilder = new RequestBuilder(commandText, _session, this.BatchWriter);
            var request = await requestBuilder.GetRequestAsync(false, cancellationToken);//.ConfigureAwait(false);
            if (cancellationToken.IsCancellationRequested) cancellationToken.ThrowIfCancellationRequested();

            var result = await ExecuteRequestWithResultAsync(request, cancellationToken,
                x => x.AsEntries(_session.Settings.IncludeAnnotationsInResults),
                () => new IDictionary<string, object>[] { });//.ConfigureAwait(false);
            return result?.FirstOrDefault();
        }

        public Task<object> FindScalarAsync(string commandText)
        {
            return FindScalarAsync(commandText, CancellationToken.None);
        }

        public async Task<object> FindScalarAsync(string commandText, CancellationToken cancellationToken)
        {
            if (IsBatchResponse)
                return _batchResponse.AsScalar<object>();

            var requestBuilder = new RequestBuilder(commandText, _session, this.BatchWriter);
            var request = await requestBuilder.GetRequestAsync(true, cancellationToken);//.ConfigureAwait(false);
            if (cancellationToken.IsCancellationRequested) cancellationToken.ThrowIfCancellationRequested();

            var result = await ExecuteRequestWithResultAsync(request, cancellationToken,
                x => x.AsEntries(_session.Settings.IncludeAnnotationsInResults),
                () => new IDictionary<string, object>[] { });//.ConfigureAwait(false);

            object extractScalar(IDictionary<string, object> x) => (x == null) || !x.Any() ? null : x.Values.First();
            return result == null ? null : extractScalar(result.FirstOrDefault());
        }

        public Task<IDictionary<string, object>> GetEntryAsync(string collection, params object[] entryKey)
        {
            return GetEntryAsync(collection, CancellationToken.None, entryKey);
        }

        public async Task<IDictionary<string, object>> GetEntryAsync(string collection, CancellationToken cancellationToken, params object[] entryKey)
        {
            if (IsBatchResponse)
                return _batchResponse.AsEntry(false);

            await _session.ResolveAdapterAsync(cancellationToken);//.ConfigureAwait(false);
            if (cancellationToken.IsCancellationRequested) cancellationToken.ThrowIfCancellationRequested();

            var entryKeyWithNames = new Dictionary<string, object>();
            var entityCollection = _session.Metadata.GetEntityCollection(collection);
            var keyNames = _session.Metadata.GetDeclaredKeyPropertyNames(entityCollection.Name).ToList();
            for (int index = 0; index < keyNames.Count; index++)
            {
                entryKeyWithNames.Add(keyNames[index], entryKey.ElementAt(index));
            }
            return await GetEntryAsync(collection, entryKeyWithNames, cancellationToken);//.ConfigureAwait(false);
        }

        public Task<IDictionary<string, object>> GetEntryAsync(string collection, IDictionary<string, object> entryKey)
        {
            return GetEntryAsync(collection, entryKey, CancellationToken.None);
        }

        public async Task<IDictionary<string, object>> GetEntryAsync(string collection, IDictionary<string, object> entryKey, CancellationToken cancellationToken)
        {
            if (IsBatchResponse)
                return _batchResponse.AsEntry(false);

            var command = GetBoundClient()
                .For(collection)
                .Key(entryKey)
                .AsBoundClient().Command;

            var requestBuilder = new RequestBuilder(command, _session, this.BatchWriter);
            var request = await requestBuilder.GetRequestAsync(false, cancellationToken);//.ConfigureAwait(false);
            if (cancellationToken.IsCancellationRequested) cancellationToken.ThrowIfCancellationRequested();

            return await ExecuteRequestWithResultAsync(request, cancellationToken,
                x => x.AsEntry(_session.Settings.IncludeAnnotationsInResults), () => null);//.ConfigureAwait(false);
        }

        public Task<IDictionary<string, object>> InsertEntryAsync(string collection, IDictionary<string, object> entryData)
        {
            return InsertEntryAsync(collection, entryData, true, CancellationToken.None);
        }

        public Task<IDictionary<string, object>> InsertEntryAsync(string collection, IDictionary<string, object> entryData, CancellationToken cancellationToken)
        {
            return InsertEntryAsync(collection, entryData, true, cancellationToken);
        }

        public Task<IDictionary<string, object>> InsertEntryAsync(string collection, IDictionary<string, object> entryData, bool resultRequired)
        {
            return InsertEntryAsync(collection, entryData, resultRequired, CancellationToken.None);
        }

        public async Task<IDictionary<string, object>> InsertEntryAsync(string collection, IDictionary<string, object> entryData, bool resultRequired, CancellationToken cancellationToken)
        {
            RemoveAnnotationProperties(entryData);

            var command = GetBoundClient()
                .For(collection)
                .Set(entryData)
                .AsBoundClient().Command;

            return await InsertEntryAsync(command, resultRequired, cancellationToken);//.ConfigureAwait(false);
        }

        public Task<IDictionary<string, object>> UpdateEntryAsync(string collection, IDictionary<string, object> entryKey, IDictionary<string, object> entryData)
        {
            return UpdateEntryAsync(collection, entryKey, entryData, true, CancellationToken.None);
        }

        public Task<IDictionary<string, object>> UpdateEntryAsync(string collection, IDictionary<string, object> entryKey, IDictionary<string, object> entryData, CancellationToken cancellationToken)
        {
            return UpdateEntryAsync(collection, entryKey, entryData, true, cancellationToken);
        }

        public Task<IDictionary<string, object>> UpdateEntryAsync(string collection, IDictionary<string, object> entryKey, IDictionary<string, object> entryData, bool resultRequired)
        {
            return UpdateEntryAsync(collection, entryKey, entryData, resultRequired, CancellationToken.None);
        }

        public async Task<IDictionary<string, object>> UpdateEntryAsync(string collection, IDictionary<string, object> entryKey, IDictionary<string, object> entryData, bool resultRequired, CancellationToken cancellationToken)
        {
            RemoveAnnotationProperties(entryKey);
            RemoveAnnotationProperties(entryData);

            var command = GetBoundClient()
                .For(collection)
                .Key(entryKey)
                .Set(entryData)
                .AsBoundClient().Command;

            return await UpdateEntryAsync(command, resultRequired, cancellationToken);//.ConfigureAwait(false);
        }

        public Task<IEnumerable<IDictionary<string, object>>> UpdateEntriesAsync(string collection, string commandText, IDictionary<string, object> entryData)
        {
            return UpdateEntriesAsync(collection, commandText, entryData, true, CancellationToken.None);
        }

        public Task<IEnumerable<IDictionary<string, object>>> UpdateEntriesAsync(string collection, string commandText, IDictionary<string, object> entryData, CancellationToken cancellationToken)
        {
            return UpdateEntriesAsync(collection, commandText, entryData, true, cancellationToken);
        }

        public Task<IEnumerable<IDictionary<string, object>>> UpdateEntriesAsync(string collection, string commandText, IDictionary<string, object> entryData, bool resultRequired)
        {
            return UpdateEntriesAsync(collection, commandText, entryData, resultRequired, CancellationToken.None);
        }

        public async Task<IEnumerable<IDictionary<string, object>>> UpdateEntriesAsync(string collection, string commandText, IDictionary<string, object> entryData, bool resultRequired, CancellationToken cancellationToken)
        {
            RemoveAnnotationProperties(entryData);

            var command = GetBoundClient()
                .For(collection)
                .Filter(ExtractFilterFromCommandText(collection, commandText))
                .Set(entryData)
                .AsBoundClient().Command;

            return await UpdateEntriesAsync(command, resultRequired, cancellationToken);//.ConfigureAwait(false);
        }

        public Task DeleteEntryAsync(string collection, IDictionary<string, object> entryKey)
        {
            return DeleteEntryAsync(collection, entryKey, CancellationToken.None);
        }

        public async Task DeleteEntryAsync(string collection, IDictionary<string, object> entryKey, CancellationToken cancellationToken)
        {
            RemoveAnnotationProperties(entryKey);

            var command = GetBoundClient()
                .For(collection)
                .Key(entryKey)
                .AsBoundClient().Command;

            await DeleteEntryAsync(command, cancellationToken);//.ConfigureAwait(false);
        }

        public Task<int> DeleteEntriesAsync(string collection, string commandText)
        {
            return DeleteEntriesAsync(collection, commandText, CancellationToken.None);
        }

        public async Task<int> DeleteEntriesAsync(string collection, string commandText, CancellationToken cancellationToken)
        {
            var command = GetBoundClient()
                .For(collection)
                .Filter(ExtractFilterFromCommandText(collection, commandText))
                .AsBoundClient().Command;

            return await DeleteEntriesAsync(command, cancellationToken);//.ConfigureAwait(false);
        }

        public Task LinkEntryAsync(string collection, IDictionary<string, object> entryKey, string linkName, IDictionary<string, object> linkedEntryKey)
        {
            return LinkEntryAsync(collection, entryKey, linkName, linkedEntryKey, CancellationToken.None);
        }

        public async Task LinkEntryAsync(string collection, IDictionary<string, object> entryKey, string linkName, IDictionary<string, object> linkedEntryKey, CancellationToken cancellationToken)
        {
            RemoveAnnotationProperties(entryKey);
            RemoveAnnotationProperties(linkedEntryKey);

            var command = GetBoundClient()
                .For(collection)
                .Key(entryKey)
                .AsBoundClient().Command;

            await LinkEntryAsync(command, linkName, linkedEntryKey, cancellationToken);//.ConfigureAwait(false);
        }

        public Task UnlinkEntryAsync(string collection, IDictionary<string, object> entryKey, string linkName)
        {
            return UnlinkEntryAsync(collection, entryKey, linkName, null, CancellationToken.None);
        }

        public Task UnlinkEntryAsync(string collection, IDictionary<string, object> entryKey, string linkName, CancellationToken cancellationToken)
        {
            return UnlinkEntryAsync(collection, entryKey, linkName, null, cancellationToken);
        }

        public Task UnlinkEntryAsync(string collection, IDictionary<string, object> entryKey, string linkName, IDictionary<string, object> linkedEntryKey)
        {
            return UnlinkEntryAsync(collection, entryKey, linkName, linkedEntryKey, CancellationToken.None);
        }

        public async Task UnlinkEntryAsync(string collection, IDictionary<string, object> entryKey, string linkName, IDictionary<string, object> linkedEntryKey, CancellationToken cancellationToken)
        {
            if (IsBatchResponse)
                return;

            RemoveAnnotationProperties(entryKey);

            var command = GetBoundClient()
                .For(collection)
                .Key(entryKey)
                .AsBoundClient().Command;

            await UnlinkEntryAsync(command, linkName, linkedEntryKey, cancellationToken);//.ConfigureAwait(false);
        }

        public Task<Stream> GetMediaStreamAsync(string commandText)
        {
            return GetMediaStreamAsync(commandText, CancellationToken.None);
        }

        public async Task<Stream> GetMediaStreamAsync(string commandText, CancellationToken cancellationToken)
        {
            if (IsBatchResponse)
                throw new NotSupportedException("Media stream requests are not supported in batch mode.");

            var requestBuilder = new RequestBuilder(commandText, _session, this.BatchWriter);
            var request = await requestBuilder.GetRequestAsync(true, cancellationToken);//.ConfigureAwait(false);
            if (cancellationToken.IsCancellationRequested) cancellationToken.ThrowIfCancellationRequested();

            return await ExecuteGetStreamRequestAsync(request, cancellationToken);//.ConfigureAwait(false);
        }

        public Task SetMediaStreamAsync(string commandText, Stream stream, string contentType, bool optimisticConcurrency)
        {
            return SetMediaStreamAsync(commandText, stream, contentType, optimisticConcurrency, CancellationToken.None);
        }

        public async Task SetMediaStreamAsync(string commandText, Stream stream, string contentType, bool optimisticConcurrency, CancellationToken cancellationToken)
        {
            if (IsBatchResponse)
                throw new NotSupportedException("Media stream requests are not supported in batch mode.");

            var requestBuilder = new RequestBuilder(commandText, _session, this.BatchWriter);
            var request = await requestBuilder.UpdateRequestAsync(stream, contentType, optimisticConcurrency, cancellationToken);//.ConfigureAwait(false);
            if (cancellationToken.IsCancellationRequested) cancellationToken.ThrowIfCancellationRequested();

            await ExecuteRequestAsync(request, cancellationToken);//.ConfigureAwait(false);
        }

        public Task<IDictionary<string, object>> ExecuteFunctionAsSingleAsync(string functionName, IDictionary<string, object> parameters)
        {
            return ExecuteFunctionAsSingleAsync(functionName, parameters, CancellationToken.None);
        }

        public async Task<IDictionary<string, object>> ExecuteFunctionAsSingleAsync(string functionName, IDictionary<string, object> parameters, CancellationToken cancellationToken)
        {
            if (IsBatchResponse)
                return _batchResponse.AsEntry(false);

            await _session.ResolveAdapterAsync(cancellationToken);//.ConfigureAwait(false);
            if (cancellationToken.IsCancellationRequested) cancellationToken.ThrowIfCancellationRequested();

            var command = GetBoundClient()
                .Function(functionName)
                .Set(parameters)
                .AsBoundClient().Command;

            var result = await ExecuteFunctionAsync(command, cancellationToken);//.ConfigureAwait(false);
            return result?.FirstOrDefault();
        }

        public Task<IEnumerable<IDictionary<string, object>>> ExecuteFunctionAsEnumerableAsync(string functionName, IDictionary<string, object> parameters)
        {
            return ExecuteFunctionAsEnumerableAsync(functionName, parameters, CancellationToken.None);
        }

        public async Task<IEnumerable<IDictionary<string, object>>> ExecuteFunctionAsEnumerableAsync(string functionName, IDictionary<string, object> parameters, CancellationToken cancellationToken)
        {
            if (IsBatchResponse)
                return _batchResponse.AsEntries(false);

            await _session.ResolveAdapterAsync(cancellationToken);//.ConfigureAwait(false);
            if (cancellationToken.IsCancellationRequested) cancellationToken.ThrowIfCancellationRequested();

            var command = GetBoundClient()
                .Function(functionName)
                .Set(parameters)
                .AsBoundClient().Command;

            return await ExecuteFunctionAsync(command, cancellationToken);//.ConfigureAwait(false);
        }

        public Task<T> ExecuteFunctionAsScalarAsync<T>(string functionName, IDictionary<string, object> parameters)
        {
            return ExecuteFunctionAsScalarAsync<T>(functionName, parameters, CancellationToken.None);
        }

        public async Task<T> ExecuteFunctionAsScalarAsync<T>(string functionName, IDictionary<string, object> parameters, CancellationToken cancellationToken)
        {
            if (IsBatchResponse)
                return _batchResponse.AsScalar<T>();

            await _session.ResolveAdapterAsync(cancellationToken);//.ConfigureAwait(false);
            if (cancellationToken.IsCancellationRequested) cancellationToken.ThrowIfCancellationRequested();

            var result = await ExecuteFunctionAsSingleAsync(functionName, parameters, cancellationToken);//.ConfigureAwait(false);
            return (T)result.First().Value;
        }

        public Task<T[]> ExecuteFunctionAsArrayAsync<T>(string functionName, IDictionary<string, object> parameters)
        {
            return ExecuteFunctionAsArrayAsync<T>(functionName, parameters, CancellationToken.None);
        }

        public async Task<T[]> ExecuteFunctionAsArrayAsync<T>(string functionName, IDictionary<string, object> parameters, CancellationToken cancellationToken)
        {
            if (IsBatchResponse)
                return _batchResponse.AsArray<T>();

            await _session.ResolveAdapterAsync(cancellationToken);//.ConfigureAwait(false);
            if (cancellationToken.IsCancellationRequested) cancellationToken.ThrowIfCancellationRequested();

            var result = await ExecuteFunctionAsEnumerableAsync(functionName, parameters, cancellationToken);//.ConfigureAwait(false);
            return IsBatchRequest
                ? new T[] { }
                : result?.SelectMany(x => x.Values)
                    .Select(x => _session.TypeCache.Convert<T>(x))
                    .ToArray();
        }

        public Task ExecuteActionAsync(string actionName, IDictionary<string, object> parameters)
        {
            return ExecuteActionAsync(actionName, parameters, CancellationToken.None);
        }

        public async Task ExecuteActionAsync(string actionName, IDictionary<string, object> parameters, CancellationToken cancellationToken)
        {
            if (IsBatchResponse)
                return;

            await _session.ResolveAdapterAsync(cancellationToken);//.ConfigureAwait(false);
            if (cancellationToken.IsCancellationRequested) cancellationToken.ThrowIfCancellationRequested();

            var command = GetBoundClient()
                .Action(actionName)
                .Set(parameters)
                .AsBoundClient().Command;

            await ExecuteActionAsync(command, cancellationToken);//.ConfigureAwait(false);
        }

        public Task<IDictionary<string, object>> ExecuteActionAsSingleAsync(string actionName, IDictionary<string, object> parameters)
        {
            return ExecuteActionAsSingleAsync(actionName, parameters, CancellationToken.None);
        }

        public async Task<IDictionary<string, object>> ExecuteActionAsSingleAsync(string actionName, IDictionary<string, object> parameters, CancellationToken cancellationToken)
        {
            if (IsBatchResponse)
                return _batchResponse.AsEntry(false);

            await _session.ResolveAdapterAsync(cancellationToken);//.ConfigureAwait(false);
            if (cancellationToken.IsCancellationRequested) cancellationToken.ThrowIfCancellationRequested();

            var command = GetBoundClient()
                .Action(actionName)
                .Set(parameters)
                .AsBoundClient().Command;

            var result = await ExecuteActionAsync(command, cancellationToken);//.ConfigureAwait(false);
            return result?.FirstOrDefault();
        }

        public Task<IEnumerable<IDictionary<string, object>>> ExecuteActionAsEnumerableAsync(string actionName, IDictionary<string, object> parameters)
        {
            return ExecuteActionAsEnumerableAsync(actionName, parameters, CancellationToken.None);
        }

        public async Task<IEnumerable<IDictionary<string, object>>> ExecuteActionAsEnumerableAsync(string actionName, IDictionary<string, object> parameters, CancellationToken cancellationToken)
        {
            if (IsBatchResponse)
                return _batchResponse.AsEntries(false);

            await _session.ResolveAdapterAsync(cancellationToken);//.ConfigureAwait(false);
            if (cancellationToken.IsCancellationRequested) cancellationToken.ThrowIfCancellationRequested();

            var command = GetBoundClient()
                .Action(actionName)
                .Set(parameters)
                .AsBoundClient().Command;

            return await ExecuteActionAsync(command, cancellationToken);//.ConfigureAwait(false);
        }

        public Task<T> ExecuteActionAsScalarAsync<T>(string actionName, IDictionary<string, object> parameters)
        {
            return ExecuteActionAsScalarAsync<T>(actionName, parameters, CancellationToken.None);
        }

        public async Task<T> ExecuteActionAsScalarAsync<T>(string actionName, IDictionary<string, object> parameters, CancellationToken cancellationToken)
        {
            if (IsBatchResponse)
                return _batchResponse.AsScalar<T>();

            await _session.ResolveAdapterAsync(cancellationToken);//.ConfigureAwait(false);
            if (cancellationToken.IsCancellationRequested) cancellationToken.ThrowIfCancellationRequested();

            var result = await ExecuteActionAsSingleAsync(actionName, parameters, cancellationToken);//.ConfigureAwait(false);
            return (T)result.First().Value;
        }

        public Task<T[]> ExecuteActionAsArrayAsync<T>(string actionName, IDictionary<string, object> parameters)
        {
            return ExecuteActionAsArrayAsync<T>(actionName, parameters, CancellationToken.None);
        }

        public async Task<T[]> ExecuteActionAsArrayAsync<T>(string actionName, IDictionary<string, object> parameters, CancellationToken cancellationToken)
        {
            if (IsBatchResponse)
                return _batchResponse.AsArray<T>();

            await _session.ResolveAdapterAsync(cancellationToken);//.ConfigureAwait(false);
            if (cancellationToken.IsCancellationRequested) cancellationToken.ThrowIfCancellationRequested();

            var result = await ExecuteActionAsEnumerableAsync(actionName, parameters, cancellationToken);//.ConfigureAwait(false);
            return IsBatchRequest
                ? new T[] { }
                : result?.SelectMany(x => x.Values)
                    .Select(x => _session.TypeCache.Convert<T>(x))
                    .ToArray();
        }

        public Task<ODataResponse> GetResponseAsync(ODataRequest request)
        {
            return GetResponseAsync(request, CancellationToken.None);
        }

        public async Task<ODataResponse> GetResponseAsync(ODataRequest request, CancellationToken cancellationToken)
        {
            if (IsBatchResponse)
                return _batchResponse;
            ODataResponse EmptyResult() => ODataResponse.EmptyFeeds(Session.TypeCache);
            if (IsBatchRequest)
                return EmptyResult();
            try
            {
                using (var response = await _requestRunner.ExecuteRequestAsync(request, cancellationToken).ConfigureAwait(false))
                {
                    var responseReader = _session.Adapter.GetResponseReader();
                    return await responseReader.GetResponseAsync(response);//.ConfigureAwait(false);
                }
            }
            catch (WebRequestException ex)
            {
                if (_settings.IgnoreResourceNotFoundException && ex.Code == HttpStatusCode.NotFound)
                    return EmptyResult();
                throw;
            }
        }

        public Task<Stream> GetResponseStreamAsync(ODataRequest request)
        {
            return GetResponseStreamAsync(request, CancellationToken.None);
        }

        public Task<Stream> GetResponseStreamAsync(ODataRequest request, CancellationToken cancellationToken)
        {
            return ExecuteGetStreamRequestAsync(request, cancellationToken);
        }

#pragma warning restore 1591

        private async Task<IEnumerable<IDictionary<string, object>>> FindEntriesAsync(
            string commandText, bool scalarResult, ODataFeedAnnotations annotations, CancellationToken cancellationToken)
        {
            if (IsBatchResponse)
            {
                if (annotations != null && _batchResponse.Feed != null)
                    annotations.CopyFrom(_batchResponse.Feed.Annotations);
                return _batchResponse.AsEntries(false);
            }

            var result = await FindAnnotatedEntriesAsync(commandText, scalarResult, annotations, cancellationToken);//.ConfigureAwait(false);
            return result?.Select(x => x.GetData(_session.Settings.IncludeAnnotationsInResults));
        }

        private async Task<IEnumerable<AnnotatedEntry>> FindAnnotatedEntriesAsync(
            string commandText, bool scalarResult, ODataFeedAnnotations annotations, CancellationToken cancellationToken)
        {
            var requestBuilder = new RequestBuilder(commandText, _session, this.BatchWriter);
            var request = await requestBuilder.GetRequestAsync(scalarResult, cancellationToken);//.ConfigureAwait(false);
            if (cancellationToken.IsCancellationRequested) cancellationToken.ThrowIfCancellationRequested();

            return await ExecuteRequestWithResultAsync(request, cancellationToken, x =>
            {
                if (annotations != null && x.Feed != null)
                    annotations.CopyFrom(x.Feed.Annotations);

                return x.Feed?.Entries;
            },
            () => new AnnotatedEntry[] { });//.ConfigureAwait(false);
        }

        internal async Task<IEnumerable<IDictionary<string, object>>> FindEntriesAsync(
            FluentCommand command, bool scalarResult, ODataFeedAnnotations annotations, CancellationToken cancellationToken)
        {
            if (IsBatchResponse)
            {
                if (annotations != null && _batchResponse.Feed != null)
                    annotations.CopyFrom(_batchResponse.Feed.Annotations);
                return _batchResponse.AsEntries(false);
            }

            await _session.ResolveAdapterAsync(cancellationToken);//.ConfigureAwait(false);
            if (cancellationToken.IsCancellationRequested) cancellationToken.ThrowIfCancellationRequested();

            var commandText = await command.GetCommandTextAsync(cancellationToken);//.ConfigureAwait(false);
            if (cancellationToken.IsCancellationRequested) cancellationToken.ThrowIfCancellationRequested();

            var result = await FindAnnotatedEntriesAsync(commandText, scalarResult, annotations, cancellationToken);//.ConfigureAwait(false);
            if (cancellationToken.IsCancellationRequested) cancellationToken.ThrowIfCancellationRequested();

            await EnrichWithMediaPropertiesAsync(result, command, cancellationToken);//.ConfigureAwait(false);
            return result?.Select(x => x.GetData(_session.Settings.IncludeAnnotationsInResults));
        }

        internal async Task<IDictionary<string, object>> FindEntryAsync(FluentCommand command, CancellationToken cancellationToken)
        {
            if (IsBatchResponse)
                return _batchResponse.AsEntry(false);

            await _session.ResolveAdapterAsync(cancellationToken);//.ConfigureAwait(false);
            if (cancellationToken.IsCancellationRequested) cancellationToken.ThrowIfCancellationRequested();

            var commandText = await command.GetCommandTextAsync(cancellationToken);//.ConfigureAwait(false);
            if (cancellationToken.IsCancellationRequested) cancellationToken.ThrowIfCancellationRequested();

            var results = await FindAnnotatedEntriesAsync(commandText, false, null, cancellationToken);//.ConfigureAwait(false);
            if (cancellationToken.IsCancellationRequested) cancellationToken.ThrowIfCancellationRequested();
            var result = results?.FirstOrDefault();

            await EnrichWithMediaPropertiesAsync(result, command.Details.MediaProperties, cancellationToken);//.ConfigureAwait(false);
            return result?.GetData(_session.Settings.IncludeAnnotationsInResults);
        }

        internal async Task<object> FindScalarAsync(FluentCommand command, CancellationToken cancellationToken)
        {
            if (IsBatchResponse)
                return _batchResponse.AsScalar<object>();

            await _session.ResolveAdapterAsync(cancellationToken);//.ConfigureAwait(false);
            if (cancellationToken.IsCancellationRequested) cancellationToken.ThrowIfCancellationRequested();

            var commandText = await command.GetCommandTextAsync(cancellationToken);//.ConfigureAwait(false);
            if (cancellationToken.IsCancellationRequested) cancellationToken.ThrowIfCancellationRequested();

            return await FindScalarAsync(commandText, cancellationToken);//.ConfigureAwait(false);
        }

        internal async Task<IDictionary<string, object>> InsertEntryAsync(FluentCommand command, bool resultRequired, CancellationToken cancellationToken)
        {
            if (IsBatchResponse)
                return _batchResponse.AsEntry(false);

            var requestBuilder = new RequestBuilder(command, _session, this.BatchWriter);
            var request = await requestBuilder.InsertRequestAsync(resultRequired, cancellationToken);//.ConfigureAwait(false);
            if (cancellationToken.IsCancellationRequested) cancellationToken.ThrowIfCancellationRequested();

            var result = await ExecuteRequestWithResultAsync(request, cancellationToken,
                x => x.AsEntry(_session.Settings.IncludeAnnotationsInResults), () => null, () => command.CommandData);//.ConfigureAwait(false);
            if (cancellationToken.IsCancellationRequested) cancellationToken.ThrowIfCancellationRequested();

            var keyNames = _session.Metadata.GetDeclaredKeyPropertyNames(request.CommandText);
            if (result == null && resultRequired && Utils.AllMatch(keyNames, command.CommandData.Keys, _session.Settings.NameMatchResolver))
            {
                result = await this.GetEntryAsync(request.CommandText, request.EntryData, cancellationToken);//.ConfigureAwait(false);
                if (cancellationToken.IsCancellationRequested) cancellationToken.ThrowIfCancellationRequested();
            }

            return result;
        }

        internal async Task<IDictionary<string, object>> UpdateEntryAsync(FluentCommand command, bool resultRequired, CancellationToken cancellationToken)
        {
            if (IsBatchResponse)
                return _batchResponse.AsEntry(false);

            var requestBuilder = new RequestBuilder(command, _session, this.BatchWriter);
            var request = await requestBuilder.UpdateRequestAsync(resultRequired, cancellationToken);//.ConfigureAwait(false);
            if (cancellationToken.IsCancellationRequested) cancellationToken.ThrowIfCancellationRequested();

            var result = await ExecuteRequestWithResultAsync(request, cancellationToken,
                x => x.AsEntry(_session.Settings.IncludeAnnotationsInResults), () => null, () => request.EntryData);//.ConfigureAwait(false);
            if (cancellationToken.IsCancellationRequested) cancellationToken.ThrowIfCancellationRequested();

            if (result == null && resultRequired)
            {
                try
                {
                    result = await GetUpdatedResult(command, cancellationToken);//.ConfigureAwait(false);
                    if (cancellationToken.IsCancellationRequested) cancellationToken.ThrowIfCancellationRequested();
                }
                catch (Exception)
                {
                }
            }

            var entityCollection = _session.Metadata.GetEntityCollection(command.QualifiedEntityCollectionName);
            var entryDetails = _session.Metadata.ParseEntryDetails(entityCollection.Name, request.EntryData);

            var removedLinks = entryDetails.Links
                .SelectMany(x => x.Value.Where(y => y.LinkData == null))
                .Select(x => _session.Metadata.GetNavigationPropertyExactName(entityCollection.Name, x.LinkName))
                .ToList();

            foreach (var associationName in removedLinks)
            {
                try
                {
                    var entryKey = command.HasKey ? command.KeyValues : command.FilterAsKey;
                    await UnlinkEntryAsync(command.QualifiedEntityCollectionName, entryKey, associationName, cancellationToken);//.ConfigureAwait(false);
                    if (cancellationToken.IsCancellationRequested) cancellationToken.ThrowIfCancellationRequested();
                }
                catch (Exception)
                {
                }
            }

            return result;
        }

        internal async Task<IEnumerable<IDictionary<string, object>>> UpdateEntriesAsync(FluentCommand command, bool resultRequired, CancellationToken cancellationToken)
        {
            if (IsBatchResponse)
                return _batchResponse.AsEntries(false);

            await _session.ResolveAdapterAsync(cancellationToken);//.ConfigureAwait(false);
            if (cancellationToken.IsCancellationRequested) cancellationToken.ThrowIfCancellationRequested();

            return await IterateEntriesAsync(
                command, resultRequired,
                async (x, y, z, w) => await UpdateEntryAsync(x, y, z, w, cancellationToken).ConfigureAwait(false),
                cancellationToken);//.ConfigureAwait(false);
        }

        internal async Task DeleteEntryAsync(FluentCommand command, CancellationToken cancellationToken)
        {
            if (IsBatchResponse)
                return;

            var requestBuilder = new RequestBuilder(command, _session, this.BatchWriter);
            var request = await requestBuilder.DeleteRequestAsync(cancellationToken);//.ConfigureAwait(false);
            if (!IsBatchRequest)
            {
                using (await _requestRunner.ExecuteRequestAsync(request, cancellationToken).ConfigureAwait(false))
                {
                }
            }
        }

        internal async Task<int> DeleteEntriesAsync(FluentCommand command, CancellationToken cancellationToken)
        {
            if (IsBatchResponse)
                return 0;

            await _session.ResolveAdapterAsync(cancellationToken);//.ConfigureAwait(false);
            if (cancellationToken.IsCancellationRequested) cancellationToken.ThrowIfCancellationRequested();

            return await IterateEntriesAsync(
                command,
                async (x, y) => await DeleteEntryAsync(x, y, cancellationToken).ConfigureAwait(false),
                cancellationToken);//.ConfigureAwait(false);
        }

        internal async Task LinkEntryAsync(FluentCommand command, string linkName, IDictionary<string, object> linkedEntryKey, CancellationToken cancellationToken)
        {
            if (IsBatchResponse)
                return;

            var requestBuilder = new RequestBuilder(command, _session, this.BatchWriter);
            var request = await requestBuilder.LinkRequestAsync(linkName, linkedEntryKey, cancellationToken);//.ConfigureAwait(false);
            if (cancellationToken.IsCancellationRequested) cancellationToken.ThrowIfCancellationRequested();

            if (!IsBatchRequest)
            {
                using (await _requestRunner.ExecuteRequestAsync(request, cancellationToken).ConfigureAwait(false))
                {
                }
            }
        }

        internal async Task UnlinkEntryAsync(FluentCommand command, string linkName, IDictionary<string, object> linkedEntryKey, CancellationToken cancellationToken)
        {
            if (IsBatchResponse)
                return;

            var requestBuilder = new RequestBuilder(command, _session, this.BatchWriter);
            var request = await requestBuilder.UnlinkRequestAsync(linkName, linkedEntryKey, cancellationToken);//.ConfigureAwait(false);
            if (cancellationToken.IsCancellationRequested) cancellationToken.ThrowIfCancellationRequested();

            if (!IsBatchRequest)
            {
                using (await _requestRunner.ExecuteRequestAsync(request, cancellationToken).ConfigureAwait(false))
                {
                }
            }
        }

        internal async Task<Stream> GetMediaStreamAsync(FluentCommand command, CancellationToken cancellationToken)
        {
            if (IsBatchResponse)
                throw new NotSupportedException("Media stream requests are not supported in batch mode.");

            await _session.ResolveAdapterAsync(cancellationToken);//.ConfigureAwait(false);
            if (cancellationToken.IsCancellationRequested) cancellationToken.ThrowIfCancellationRequested();

            var commandText = await command.GetCommandTextAsync(cancellationToken);//.ConfigureAwait(false);
            if (cancellationToken.IsCancellationRequested) cancellationToken.ThrowIfCancellationRequested();

            return await GetMediaStreamAsync(commandText, cancellationToken);//.ConfigureAwait(false);
        }

        internal async Task SetMediaStreamAsync(FluentCommand command, Stream stream, string contentType, bool optimisticConcurrency, CancellationToken cancellationToken)
        {
            if (IsBatchResponse)
                throw new NotSupportedException("Media stream requests are not supported in batch mode.");

            await _session.ResolveAdapterAsync(cancellationToken);//.ConfigureAwait(false);
            if (cancellationToken.IsCancellationRequested) cancellationToken.ThrowIfCancellationRequested();

            var commandText = await command.GetCommandTextAsync(cancellationToken);//.ConfigureAwait(false);
            if (cancellationToken.IsCancellationRequested) cancellationToken.ThrowIfCancellationRequested();

            await SetMediaStreamAsync(commandText, stream, contentType, optimisticConcurrency, cancellationToken);//.ConfigureAwait(false);
        }

        internal async Task ExecuteAsync(FluentCommand command, CancellationToken cancellationToken)
        {
            if (IsBatchResponse)
                return;

            await ExecuteAsEnumerableAsync(command, cancellationToken);//.ConfigureAwait(false);
        }

        internal async Task<IDictionary<string, object>> ExecuteAsSingleAsync(FluentCommand command, CancellationToken cancellationToken)
        {
            if (IsBatchResponse)
                return _batchResponse.AsEntry(false);

            var result = await ExecuteAsEnumerableAsync(command, cancellationToken);//.ConfigureAwait(false);
            return result?.FirstOrDefault();
        }

        internal async Task<IEnumerable<IDictionary<string, object>>> ExecuteAsEnumerableAsync(FluentCommand command, CancellationToken cancellationToken)
        {
            if (IsBatchResponse)
                return _batchResponse.AsEntries(false);

            await _session.ResolveAdapterAsync(cancellationToken);//.ConfigureAwait(false);
            if (cancellationToken.IsCancellationRequested) cancellationToken.ThrowIfCancellationRequested();

            if (command.HasFunction)
                return await ExecuteFunctionAsync(command, cancellationToken);//.ConfigureAwait(false);
            else if (command.HasAction)
                return await ExecuteActionAsync(command, cancellationToken);//.ConfigureAwait(false);
            else
                throw new InvalidOperationException("Command is expected to be a function or an action.");
        }

        internal async Task<T> ExecuteAsScalarAsync<T>(FluentCommand command, CancellationToken cancellationToken)
        {
            if (IsBatchResponse)
                return _batchResponse.AsScalar<T>();

            var result = await ExecuteAsSingleAsync(command, cancellationToken);//.ConfigureAwait(false);
            return IsBatchRequest
                ? default(T)
                : result == null
                ? default(T)
                : command.TypeCache.Convert<T>(result.First().Value);
        }

        internal async Task<T[]> ExecuteAsArrayAsync<T>(FluentCommand command, CancellationToken cancellationToken)
        {
            if (IsBatchResponse)
                return _batchResponse.AsArray<T>();

            var result = await ExecuteAsEnumerableAsync(command, cancellationToken);//.ConfigureAwait(false);
            return IsBatchRequest
                ? new T[] { }
                : result == null
                ? null
                : typeof(T) == typeof(string) || typeof(T).IsValue()
                ? result.SelectMany(x => x.Values).Select(x => command.TypeCache.Convert<T>(x)).ToArray()
                : result.Select(x => (T)x.ToObject(command.TypeCache, typeof(T))).ToArray();
        }

        internal async Task ExecuteBatchAsync(IList<Func<IODataClient, Task>> actions, CancellationToken cancellationToken)
        {
            await _session.ResolveAdapterAsync(cancellationToken);//.ConfigureAwait(false);
            if (cancellationToken.IsCancellationRequested) cancellationToken.ThrowIfCancellationRequested();

            await ExecuteBatchActionsAsync(actions, cancellationToken);//.ConfigureAwait(false);
        }

        private string ExtractFilterFromCommandText(string collection, string commandText)
        {
            const string filterPrefix = "?$filter=";

            if (commandText.Length > filterPrefix.Length &&
                commandText.Substring(0, collection.Length + filterPrefix.Length).Equals(
                    collection + filterPrefix, StringComparison.CurrentCultureIgnoreCase))
            {
                return commandText.Substring(collection.Length + filterPrefix.Length);
            }
            else
            {
                return commandText;
            }
        }
    }
}
