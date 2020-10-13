using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutoMapper
{
    public class Mapper : IMapper
    {

        private readonly List<Mapping> _mappings;

        internal Mapper()
        {
            _mappings = new List<Mapping>();
        }

        internal void AddMap(Mapping mapping)
        {
            _mappings.Add(mapping);
        }

        public bool IsValid()
        {
            return _mappings.All(m => m.IsValid);
        }

        public TDestination Map<TDestination>(object source, TDestination destination) where TDestination : class
        {
            Mapping mapping = null;

            foreach (var map in _mappings)
            {
                if (map.SourceType == source.GetType() && map.DestinationType == destination.GetType())
                {
                    mapping = map;
                    break;
                }
            }

            if (mapping == null)
                throw new Exception($"no mapping registered for source {source.GetType()} and  {destination.GetType()}");

            foreach (var (Source, Destination, NeedsMapping) in mapping.Properties)
            {
                if (NeedsMapping)
                {
                    Destination.SetValue(destination, Map(Source.GetValue(source), Destination.PropertyType));
                }
                else if (Source.PropertyType == Destination.PropertyType)
                {
                    Destination.SetValue(destination, Source.GetValue(source));
                }
            }

            return destination;
        }

        public TDestination Map<TDestination>(object source) where TDestination : class 
            => Map(source, (TDestination)CreateType(typeof(TDestination)));

        private object Map(object source, Type destinationType) 
            => Map(source, CreateType(destinationType));

        private object CreateType(Type type, params object[] parameters)
        {
            var ctors = type.GetConstructors().Where(c => c.GetParameters().Length == parameters?.Length);
            if (ctors.Count() == 0)
                throw new Exception($"There is no constructor with {parameters?.Length} parameters");

            var ctor = ctors.Where(c => c.GetParameters().All(parameters.Contains)).First();

            return ctor.Invoke(parameters);
        }
    }
}
