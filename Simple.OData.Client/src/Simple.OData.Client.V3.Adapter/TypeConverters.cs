﻿using System;
using System.Collections.Generic;
using System.Spatial;

#pragma warning disable 1591

namespace Simple.OData.Client.WestInternal.V3.Adapter
{
    public static class TypeConverters
    {
        public static GeographyPoint CreateGeographyPoint(IDictionary<string, object> source)
        {
            return GeographyPoint.Create(
                CoordinateSystem.Geography(source.ContainsKey("CoordinateSystem")
                    ? source.GetValueOrDefault<CoordinateSystem>("CoordinateSystem").EpsgId
                    : null),
                source.GetValueOrDefault<double>("Latitude"),
                source.GetValueOrDefault<double>("Longitude"),
                source.GetValueOrDefault<double?>("Z"),
                source.GetValueOrDefault<double?>("M"));
        }

        public static GeometryPoint CreateGeometryPoint(IDictionary<string, object> source)
        {
            return GeometryPoint.Create(
                CoordinateSystem.Geometry(source.ContainsKey("CoordinateSystem")
                    ? source.GetValueOrDefault<CoordinateSystem>("CoordinateSystem").EpsgId
                    : null),
                source.GetValueOrDefault<double>("Latitude"),
                source.GetValueOrDefault<double>("Longitude"),
                source.GetValueOrDefault<double?>("Z"),
                source.GetValueOrDefault<double?>("M"));
        }

        public static object ConvertToEdmDate(object source)
        {
            return source;
        }

        private static T GetValueOrDefault<T>(this IDictionary<string, object> source, string key)
        {
            if (source.TryGetValue(key, out var value))
            {
                return (T)value;
            }
            else
            {
                return default(T);
            }
        }
    }
}