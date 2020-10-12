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

        public TDestination Map<TDestination>(object source)
            where TDestination : class
        {
            // create object
            var ctors = typeof(TDestination).GetConstructors().Where(c => c.GetParameters().Length == 0);
            if (ctors.Count() == 0)
                throw new Exception("There is no constructor with zero parameters");

            var ctor = ctors.First();

            TDestination result = (TDestination)ctor.Invoke(null);

            return Map(source, result);
        }

        public TDestination Map<TDestination>(object source, TDestination destination) where TDestination : class
        {
            // find mapping

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
                throw new Exception($"no mapping registered for source {source.GetType()} and  {typeof(TDestination)}");

            foreach (var (Source, Destination) in mapping.Properties)
            {
                if (Source.PropertyType == Destination.PropertyType)
                {
                    Destination.SetValue(destination, Source.GetValue(source));
                }
                else
                {
                    Destination.SetValue(destination, Map(Source.GetValue(source), Destination.PropertyType));
                }
            }

            return destination;
        }

        private object Map(object source, Type destinationType)
        {
            // create object
            var ctors = destinationType.GetConstructors().Where(c => c.GetParameters().Length == 0);
            if (ctors.Count() == 0)
                throw new Exception("There is no constructor with zero parameters");

            var ctor = ctors.First();

            var result = ctor.Invoke(null);

            return Map(source, Convert.ChangeType(result, destinationType));
        }
    }
}
