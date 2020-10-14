namespace Instinct
{
    public interface IMapperConfiguration
    {
        void AddMap<TSource, TDestination>();
        IMapper CreateMapper();
    }
}