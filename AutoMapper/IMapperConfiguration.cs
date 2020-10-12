namespace AutoMapper
{
    public interface IMapperConfiguration
    {
        void AddMap<TSource, TDestination>();
        IMapper CreateMapper();
    }
}