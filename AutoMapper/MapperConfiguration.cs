using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace AutoMapper
{
    public class MapperConfiguration : IMapperConfiguration
    {

        private readonly List<(Type Source, Type Destination)> _maps;

        public MapperConfiguration()
        {
            _maps = new List<(Type Source, Type Destination)>();
        }

        public void AddMap<TSource, TDestination>()
        {
            _maps.Add((typeof(TSource), typeof(TDestination)));
        }

        public IMapper CreateMapper()
        {
            Mapper result = new Mapper();

            foreach(var (Source, Destination) in _maps)
            {
                var sourceProperties = Source.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                var destinationProperties = Destination.GetProperties(BindingFlags.Public | BindingFlags.Instance);

                Mapping mapping = new Mapping(Source, Destination);

                foreach(var dprop in destinationProperties)
                {
                    var connections = sourceProperties.Where(p => p.Name == dprop.Name);
                    
                    if(connections.Count() == 0)
                    {
                        mapping.IsValid = false;
                        continue;
                    }

                    var connection = connections.First();

                    bool needsMapping = false;

                    if(dprop.PropertyType != connection.PropertyType)
                    {
                        needsMapping = true;
                    }

                    mapping.Properties.Add((connection, dprop, needsMapping));
                }

                result.AddMap(mapping);
            }

            return result;
        }

    }
}
