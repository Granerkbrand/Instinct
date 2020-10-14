using System;

namespace Instinct
{
    public interface IMapper
    {

        public bool IsValid();

        TDestination Map<TDestination>(object source)
            where TDestination : class;

        TDestination Map<TDestination>(object source, TDestination destination)
            where TDestination : class;
    }
}