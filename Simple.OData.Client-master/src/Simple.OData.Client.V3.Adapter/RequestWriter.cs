﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Spatial;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.Data.Edm;
using Microsoft.Data.Edm.Validation;
using Microsoft.Data.OData;
using Simple.OData.Client.Extensions;

#pragma warning disable 1591

namespace Simple.OData.Client.V3.Adapter
{
    public class RequestWriter : RequestWriterBase
    {
        private readonly IEdmModel _model;

        public RequestWriter(ISession session, IEdmModel model, Lazy<IBatchWriter> deferredBatchWriter)
            : base(session, deferredBatchWriter)
        {
            _model = model;
        }

        protected override async Task<Stream> WriteEntryContentAsync(string method, string collection, string commandText, IDictionary<string, object> entryData, bool resultRequired)
        {
            IODataRequestMessageAsync message = IsBatch
                ? await CreateBatchOperationMessageAsync(method, collection, entryData, commandText, resultRequired).ConfigureAwait(false) 
                : new ODataRequestMessage();

            if (method == RestVerbs.Get || method == RestVerbs.Delete)
                return null;

            var entityType = _model.FindDeclaredType(
                _session.Metadata.GetQualifiedTypeName(collection)) as IEdmEntityType;
            var model = (method == RestVerbs.Patch || method == RestVerbs.Merge) ? new EdmDeltaModel(_model, entityType, entryData.Keys) : _model;

            using (var messageWriter = new ODataMessageWriter(message, GetWriterSettings(), model))
            {
                var contentId = _deferredBatchWriter != null ? _deferredBatchWriter.Value.GetContentId(entryData, null) : null;
                //var entityCollection = _session.Metadata.GetEntityCollection(collection);
                var entityCollection = _session.Metadata.NavigateToCollection(collection);
                var entryDetails = _session.Metadata.ParseEntryDetails(entityCollection.Name, entryData, contentId);

                var entryWriter = messageWriter.CreateODataEntryWriter();
                var entry = CreateODataEntry(entityType.FullName(), entryDetails.Properties);

                entryWriter.WriteStart(entry);

                if (entryDetails.Links != null)
                {
                    foreach (var link in entryDetails.Links)
                    {
                        if (link.Value.Any(x => x.LinkData != null))
                        {
                            WriteLink(entryWriter, entry, link.Key, link.Value);
                        }
                    }
                }

                entryWriter.WriteEnd();

                if (IsBatch)
                    return null;

                return await message.GetStreamAsync();//.ConfigureAwait(false);
            }
        }

        protected override async Task<Stream> WriteLinkContentAsync(string method, string commandText, string linkIdent)
        {
            IODataRequestMessageAsync message = IsBatch
                ? await CreateBatchOperationMessageAsync(method, null, null, commandText, false).ConfigureAwait(false) 
                : new ODataRequestMessage();

            using (var messageWriter = new ODataMessageWriter(message, GetWriterSettings(), _model))
            {
                var link = new ODataEntityReferenceLink
                {
                    Url = Utils.CreateAbsoluteUri(_session.Settings.BaseUri.AbsoluteUri, linkIdent)
                };
                messageWriter.WriteEntityReferenceLink(link);

                if (IsBatch)
                    return null;

                return await message.GetStreamAsync();//.ConfigureAwait(false);
            }
        }

        protected override async Task<Stream> WriteFunctionContentAsync(string method, string commandText)
        {
            if (IsBatch)
                await CreateBatchOperationMessageAsync(method, null, null, commandText, true);//.ConfigureAwait(false);

            return null;
        }

        protected override async Task<Stream> WriteActionContentAsync(string method, string commandText, string actionName, string boundTypeName, IDictionary<string, object> parameters)
        {
            IODataRequestMessageAsync message = IsBatch
                ? await CreateBatchOperationMessageAsync(method, null, null, commandText, true).ConfigureAwait(false) 
                : new ODataRequestMessage();

            using (var messageWriter = new ODataMessageWriter(message, GetWriterSettings(ODataFormat.Json), _model))
            {
                var action = _model.SchemaElements
                    .Where(x => x.SchemaElementKind == EdmSchemaElementKind.EntityContainer)
                    .SelectMany(x => (x as IEdmEntityContainer).FunctionImports())
                    .BestMatch(x => x.Name, actionName, _session.Settings.NameMatchResolver);
                var parameterWriter = await messageWriter.CreateODataParameterWriterAsync(action);//.ConfigureAwait(false);
                await parameterWriter.WriteStartAsync();//.ConfigureAwait(false);


                foreach (var parameter in parameters)
                {
                    var operationParameter = action.Parameters.BestMatch(x => x.Name, parameter.Key, _session.Settings.NameMatchResolver);
                    if (operationParameter == null)
                        throw new UnresolvableObjectException(parameter.Key, $"Parameter [{parameter.Key}] not found for action [{actionName}]");

                    await WriteOperationParameterAsync(parameterWriter, operationParameter, parameter.Key, parameter.Value);//.ConfigureAwait(false);
                }

                await parameterWriter.WriteEndAsync();//.ConfigureAwait(false);

                if (IsBatch)
                    return null;

                return await message.GetStreamAsync();//.ConfigureAwait(false);
            }
        }

        private async Task WriteOperationParameterAsync(ODataParameterWriter parameterWriter, IEdmFunctionParameter operationParameter, string paramName, object paramValue)
        {
            switch (operationParameter.Type.Definition.TypeKind)
            {
                case EdmTypeKind.Primitive:
                case EdmTypeKind.Complex:
                    var value = GetPropertyValue(operationParameter.Type, paramValue);
                    await parameterWriter.WriteValueAsync(paramName, value);//.ConfigureAwait(false);
                    break;

                case EdmTypeKind.Collection:
                    var collectionWriter = await parameterWriter.CreateCollectionWriterAsync(paramName);//.ConfigureAwait(false);
                    await collectionWriter.WriteStartAsync(new ODataCollectionStart());//.ConfigureAwait(false);
                    foreach (var item in (IEnumerable) paramValue)
                    {
                        await collectionWriter.WriteItemAsync(item);//.ConfigureAwait(false);
                    }
                    await collectionWriter.WriteEndAsync();//.ConfigureAwait(false);
                    break;

                default:
                    throw new NotSupportedException($"Unable to write action parameter of a type {operationParameter.Type.Definition.TypeKind}");
            }
        }

        protected override async Task<Stream> WriteStreamContentAsync(Stream stream, bool writeAsText)
        {
            var message = new ODataRequestMessage();
            using (var messageWriter = new ODataMessageWriter(message, GetWriterSettings(ODataFormat.RawValue), _model))
            {
                var value = writeAsText ? (object)Utils.StreamToString(stream) : Utils.StreamToByteArray(stream);
                await messageWriter.WriteValueAsync(value);
                return await message.GetStreamAsync();
            }
        }

        protected override string FormatLinkPath(string entryIdent, string navigationPropertyName, string linkIdent = null)
        {
            return linkIdent == null
                ? $"{entryIdent}/$links/{navigationPropertyName}"
                : $"{entryIdent}/$links/{linkIdent}";
        }

        protected override void AssignHeaders(ODataRequest request)
        {
            request.Headers.Add(HttpLiteral.Prefer, request.ResultRequired ? HttpLiteral.ReturnContent : HttpLiteral.ReturnNoContent);
        }

        private ODataMessageWriterSettings GetWriterSettings(ODataFormat preferredContentType = null)
        {
            var settings = new ODataMessageWriterSettings()
            {
                BaseUri = _session.Settings.BaseUri,
                Indent = true,
                DisableMessageStreamDisposal = !IsBatch,
            };
            ODataFormat contentType;
            if (preferredContentType != null)
            {
                contentType = preferredContentType;
            }
            else
            {
                switch (_session.Settings.PayloadFormat)
                {
                    case ODataPayloadFormat.Atom:
                    default:
                        contentType = ODataFormat.Atom;
                        break;
                    case ODataPayloadFormat.Json:
                        switch (_session.Adapter.ProtocolVersion)
                        {
                            case ODataProtocolVersion.V1:
                            case ODataProtocolVersion.V2:
                                contentType = ODataFormat.VerboseJson;
                                break;
                            default:
                                contentType = ODataFormat.Json;
                                break;
                        }
                        break;
                }
            }
            settings.SetContentType(contentType);
            return settings;
        }

        private Microsoft.Data.OData.ODataEntry CreateODataEntry(string typeName, IDictionary<string, object> properties)
        {
            var entry = new Microsoft.Data.OData.ODataEntry() { TypeName = typeName };

            var typeProperties = (_model.FindDeclaredType(entry.TypeName) as IEdmEntityType).Properties();
            Func<string, string> findMatchingPropertyName = name =>
            {
                var property = typeProperties.BestMatch(y => y.Name, name, _session.Settings.NameMatchResolver);
                return property != null ? property.Name : name;
            };
            entry.Properties = properties.Select(x => new ODataProperty()
            {
                Name = findMatchingPropertyName(x.Key),
                Value = GetPropertyValue(typeProperties, x.Key, x.Value)
            }).ToList();

            return entry;
        }

        private async Task<IODataRequestMessageAsync> CreateBatchOperationMessageAsync(string method, string collection, IDictionary<string, object> entryData, string commandText, bool resultRequired)
        {
            var message = (await _deferredBatchWriter.Value.CreateOperationMessageAsync(
                Utils.CreateAbsoluteUri(_session.Settings.BaseUri.AbsoluteUri, commandText),
                method, collection, entryData, resultRequired).ConfigureAwait(false)) as IODataRequestMessageAsync;

            return message;
        }

        private void WriteLink(ODataWriter entryWriter, Microsoft.Data.OData.ODataEntry entry, string linkName, IEnumerable<ReferenceLink> links)
        {
            var navigationProperty = (_model.FindDeclaredType(entry.TypeName) as IEdmEntityType).NavigationProperties()
                .BestMatch(x => x.Name, linkName, _session.Settings.NameMatchResolver);
            bool isCollection = navigationProperty.Type.Definition.TypeKind == EdmTypeKind.Collection;

            var linkType = GetNavigationPropertyEntityType(navigationProperty);
            var linkTypeWithKey = linkType;
            while (linkTypeWithKey.DeclaredKey == null && linkTypeWithKey.BaseEntityType() != null)
            {
                linkTypeWithKey = linkTypeWithKey.BaseEntityType();
            }

            entryWriter.WriteStart(new ODataNavigationLink
            {
                Name = linkName,
                IsCollection = isCollection,
                Url = new Uri(ODataNamespace.Related + linkType, UriKind.Absolute),
            });

            foreach (var referenceLink in links)
            {
                var linkKey = linkTypeWithKey.DeclaredKey;
                var linkEntry = referenceLink.LinkData.ToDictionary(TypeCache);
                var contentId = GetContentId(referenceLink);
                string linkUri;
                if (contentId != null)
                {
                    linkUri = "$" + contentId;
                }
                else
                {
                    var formattedKey = _session.Adapter.GetCommandFormatter().ConvertKeyValuesToUriLiteral(
                        linkKey.ToDictionary(x => x.Name, x => linkEntry[x.Name]), true);
                    var linkedCollectionName = _session.Metadata.GetLinkedCollectionName(
                        referenceLink.LinkData.GetType().Name, linkTypeWithKey.Name, out var isSingleton);
                    linkUri = linkedCollectionName + (isSingleton ? string.Empty : formattedKey);
                }
                var link = new ODataEntityReferenceLink
                {
                    Url = Utils.CreateAbsoluteUri(_session.Settings.BaseUri.AbsoluteUri, linkUri)
                };

                entryWriter.WriteEntityReferenceLink(link);
            }

            entryWriter.WriteEnd();
        }

        private static IEdmEntityType GetNavigationPropertyEntityType(IEdmNavigationProperty navigationProperty)
        {
            if (navigationProperty.Type.Definition.TypeKind == EdmTypeKind.Collection)
                return (navigationProperty.Type.Definition as IEdmCollectionType).ElementType.Definition as IEdmEntityType;
            else
                return navigationProperty.Type.Definition as IEdmEntityType;
        }

        private object GetPropertyValue(IEnumerable<IEdmProperty> properties, string key, object value)
        {
            var property = properties.BestMatch(x => x.Name, key, _session.Settings.NameMatchResolver);
            return property != null ? GetPropertyValue(property.Type, value) : value;
        }

        private object GetPropertyValue(IEdmTypeReference propertyType, object value)
        {
            if (value == null)
                return value;

            switch (propertyType.TypeKind())
            {
                case EdmTypeKind.Complex:
                    if (Converter.HasObjectConverter(value.GetType()))
                    {
                        return Converter.Convert(value, value.GetType());
                    }
                    var complexTypeProperties = propertyType.AsComplex().StructuralProperties();
                    return new ODataComplexValue
                    {
                        TypeName = propertyType.FullName(),
                        Properties = value.ToDictionary(TypeCache)
                            .Where(val => complexTypeProperties.Any(p => p.Name == val.Key))
                            .Select(x => new ODataProperty
                            {
                                Name = x.Key,
                                Value = GetPropertyValue(complexTypeProperties, x.Key, x.Value),
                            })
                    };

                case EdmTypeKind.Collection:
                    var collection = propertyType.AsCollection();
                    return new ODataCollectionValue()
                    {
                        TypeName = propertyType.FullName(),
                        Items = ((IEnumerable)value).Cast<object>().Select(x => GetPropertyValue(collection.ElementType(), x)),
                    };

                case EdmTypeKind.Primitive:
                    var mappedTypes = _typeMap.Where(x => x.Value == (propertyType.Definition as IEdmPrimitiveType).PrimitiveKind);
                    if (mappedTypes.Any())
                    {
                        foreach (var mappedType in mappedTypes)
                        {
                            if (TypeCache.TryConvert(value, mappedType.Key, out var result))
                                return result;
                        }
                        throw new NotSupportedException($"Conversion is not supported from type {value.GetType()} to OData type {propertyType}");
                    }
                    return value;

                case EdmTypeKind.Enum:
                    return value.ToString();

                case EdmTypeKind.None:
                    if (Converter.HasObjectConverter(value.GetType()))
                    {
                        return Converter.Convert(value, value.GetType());
                    }
                    throw new NotSupportedException($"Conversion is not supported from type {value.GetType()} to OData type {propertyType}");

                default:
                    return value;
            }
        }

        private static readonly Dictionary<Type, EdmPrimitiveTypeKind> _typeMap = new[]
            {
                new KeyValuePair<Type, EdmPrimitiveTypeKind>(typeof(string), EdmPrimitiveTypeKind.String),
                new KeyValuePair<Type, EdmPrimitiveTypeKind>(typeof(bool), EdmPrimitiveTypeKind.Boolean),
                new KeyValuePair<Type, EdmPrimitiveTypeKind>(typeof(bool?), EdmPrimitiveTypeKind.Boolean),
                new KeyValuePair<Type, EdmPrimitiveTypeKind>(typeof(byte), EdmPrimitiveTypeKind.Byte),
                new KeyValuePair<Type, EdmPrimitiveTypeKind>(typeof(byte?), EdmPrimitiveTypeKind.Byte),
                new KeyValuePair<Type, EdmPrimitiveTypeKind>(typeof(decimal), EdmPrimitiveTypeKind.Decimal),
                new KeyValuePair<Type, EdmPrimitiveTypeKind>(typeof(decimal?), EdmPrimitiveTypeKind.Decimal),
                new KeyValuePair<Type, EdmPrimitiveTypeKind>(typeof(double), EdmPrimitiveTypeKind.Double),
                new KeyValuePair<Type, EdmPrimitiveTypeKind>(typeof(double?), EdmPrimitiveTypeKind.Double),
                new KeyValuePair<Type, EdmPrimitiveTypeKind>(typeof(Guid), EdmPrimitiveTypeKind.Guid),
                new KeyValuePair<Type, EdmPrimitiveTypeKind>(typeof(Guid?), EdmPrimitiveTypeKind.Guid),
                new KeyValuePair<Type, EdmPrimitiveTypeKind>(typeof(short), EdmPrimitiveTypeKind.Int16),
                new KeyValuePair<Type, EdmPrimitiveTypeKind>(typeof(short?), EdmPrimitiveTypeKind.Int16),
                new KeyValuePair<Type, EdmPrimitiveTypeKind>(typeof(int), EdmPrimitiveTypeKind.Int32),
                new KeyValuePair<Type, EdmPrimitiveTypeKind>(typeof(int?), EdmPrimitiveTypeKind.Int32),
                new KeyValuePair<Type, EdmPrimitiveTypeKind>(typeof(long), EdmPrimitiveTypeKind.Int64),
                new KeyValuePair<Type, EdmPrimitiveTypeKind>(typeof(long?), EdmPrimitiveTypeKind.Int64),
                new KeyValuePair<Type, EdmPrimitiveTypeKind>(typeof(sbyte), EdmPrimitiveTypeKind.SByte),
                new KeyValuePair<Type, EdmPrimitiveTypeKind>(typeof(sbyte?), EdmPrimitiveTypeKind.SByte),
                new KeyValuePair<Type, EdmPrimitiveTypeKind>(typeof(float), EdmPrimitiveTypeKind.Single),
                new KeyValuePair<Type, EdmPrimitiveTypeKind>(typeof(float?), EdmPrimitiveTypeKind.Single),
                new KeyValuePair<Type, EdmPrimitiveTypeKind>(typeof(byte[]), EdmPrimitiveTypeKind.Binary),
                new KeyValuePair<Type, EdmPrimitiveTypeKind>(typeof(Stream), EdmPrimitiveTypeKind.Stream),
                new KeyValuePair<Type, EdmPrimitiveTypeKind>(typeof(Geography), EdmPrimitiveTypeKind.Geography),
                new KeyValuePair<Type, EdmPrimitiveTypeKind>(typeof(GeographyPoint), EdmPrimitiveTypeKind.GeographyPoint),
                new KeyValuePair<Type, EdmPrimitiveTypeKind>(typeof(GeographyLineString), EdmPrimitiveTypeKind.GeographyLineString),
                new KeyValuePair<Type, EdmPrimitiveTypeKind>(typeof(GeographyPolygon), EdmPrimitiveTypeKind.GeographyPolygon),
                new KeyValuePair<Type, EdmPrimitiveTypeKind>(typeof(GeographyCollection), EdmPrimitiveTypeKind.GeographyCollection),
                new KeyValuePair<Type, EdmPrimitiveTypeKind>(typeof(GeographyMultiLineString), EdmPrimitiveTypeKind.GeographyMultiLineString),
                new KeyValuePair<Type, EdmPrimitiveTypeKind>(typeof(GeographyMultiPoint), EdmPrimitiveTypeKind.GeographyMultiPoint),
                new KeyValuePair<Type, EdmPrimitiveTypeKind>(typeof(GeographyMultiPolygon), EdmPrimitiveTypeKind.GeographyMultiPolygon),
                new KeyValuePair<Type, EdmPrimitiveTypeKind>(typeof(Geometry), EdmPrimitiveTypeKind.Geometry),
                new KeyValuePair<Type, EdmPrimitiveTypeKind>(typeof(GeometryPoint), EdmPrimitiveTypeKind.GeometryPoint),
                new KeyValuePair<Type, EdmPrimitiveTypeKind>(typeof(GeometryLineString), EdmPrimitiveTypeKind.GeometryLineString),
                new KeyValuePair<Type, EdmPrimitiveTypeKind>(typeof(GeometryPolygon), EdmPrimitiveTypeKind.GeometryPolygon),
                new KeyValuePair<Type, EdmPrimitiveTypeKind>(typeof(GeometryCollection), EdmPrimitiveTypeKind.GeometryCollection),
                new KeyValuePair<Type, EdmPrimitiveTypeKind>(typeof(GeometryMultiLineString), EdmPrimitiveTypeKind.GeometryMultiLineString),
                new KeyValuePair<Type, EdmPrimitiveTypeKind>(typeof(GeometryMultiPoint), EdmPrimitiveTypeKind.GeometryMultiPoint),
                new KeyValuePair<Type, EdmPrimitiveTypeKind>(typeof(GeometryMultiPolygon), EdmPrimitiveTypeKind.GeometryMultiPolygon),
                new KeyValuePair<Type, EdmPrimitiveTypeKind>(typeof(DateTimeOffset), EdmPrimitiveTypeKind.DateTimeOffset),
                new KeyValuePair<Type, EdmPrimitiveTypeKind>(typeof(DateTimeOffset?), EdmPrimitiveTypeKind.DateTimeOffset),
                new KeyValuePair<Type, EdmPrimitiveTypeKind>(typeof(DateTime), EdmPrimitiveTypeKind.DateTime),
                new KeyValuePair<Type, EdmPrimitiveTypeKind>(typeof(DateTime?), EdmPrimitiveTypeKind.DateTime),

                new KeyValuePair<Type, EdmPrimitiveTypeKind>(typeof(XElement), EdmPrimitiveTypeKind.String),
                new KeyValuePair<Type, EdmPrimitiveTypeKind>(typeof(ushort), EdmPrimitiveTypeKind.Int32),
                new KeyValuePair<Type, EdmPrimitiveTypeKind>(typeof(ushort?), EdmPrimitiveTypeKind.Int32),
                new KeyValuePair<Type, EdmPrimitiveTypeKind>(typeof(uint), EdmPrimitiveTypeKind.Int64),
                new KeyValuePair<Type, EdmPrimitiveTypeKind>(typeof(uint?), EdmPrimitiveTypeKind.Int64),
                new KeyValuePair<Type, EdmPrimitiveTypeKind>(typeof(ulong), EdmPrimitiveTypeKind.Int64),
                new KeyValuePair<Type, EdmPrimitiveTypeKind>(typeof(ulong?), EdmPrimitiveTypeKind.Int64),
                new KeyValuePair<Type, EdmPrimitiveTypeKind>(typeof(char[]), EdmPrimitiveTypeKind.String),
                new KeyValuePair<Type, EdmPrimitiveTypeKind>(typeof(char), EdmPrimitiveTypeKind.String),
                new KeyValuePair<Type, EdmPrimitiveTypeKind>(typeof(char?), EdmPrimitiveTypeKind.String),
            }
                    .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
    }
}