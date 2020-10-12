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

            foreach(var map in _maps)
            {
                var sourceProperties = map.Source.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                var destinationProperties = map.Destination.GetProperties(BindingFlags.Public | BindingFlags.Instance);

                Mapping mapping = new Mapping(map.Source, map.Destination);

                foreach(var dprop in destinationProperties)
                {
                    var connections = sourceProperties.Where(p => p.Name == dprop.Name);
                    if(connections.Count() != 1)
                    {
                        mapping.IsValid = false;
                        continue;
                    }

                    var connection = connections.First();

                    mapping.Properties.Add((connection, dprop));
                }

                result.AddMap(mapping);
            }

            return result;
        }

    }
}
