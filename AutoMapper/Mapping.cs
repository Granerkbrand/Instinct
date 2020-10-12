using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace AutoMapper
{
    internal sealed class Mapping
    {
        public Type SourceType { get; set; }

        public Type DestinationType { get; set; }

        public List<(PropertyInfo Source, PropertyInfo Destination)> Properties { get; set; }

        public bool IsValid { get; set; }

        internal Mapping(Type sourceType, Type destinationType)
        {
            SourceType = sourceType;
            DestinationType = destinationType;

            Properties = new List<(PropertyInfo Source, PropertyInfo Destination)>();
            IsValid = true;
        }
    }
}
