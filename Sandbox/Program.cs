using AutoMapper;
using System;

namespace Sandbox
{
    class Program
    {
        static void Main(string[] args)
        {
            IMapperConfiguration configuration = new MapperConfiguration();

            configuration.AddMap<Person, PersonModel>();
            configuration.AddMap<PersonModel, Person>();
            configuration.AddMap<Address, AddressModel>();
            configuration.AddMap<AddressModel, Address>();

            IMapper mapper = configuration.CreateMapper();

            if(!mapper.IsValid())
            {
                Console.WriteLine("Mapper is invalid");
                return;
            }

            var person = new Person()
            {
                Name = "Peter P.",
                Age = 18,
                Address = new Address()
                {
                    Street = "Main street",
                    Number = 1
                }
            };

            var pm = mapper.Map<PersonModel>(person);
            var p = mapper.Map<Person>(pm);

            Console.WriteLine(pm);
            Console.WriteLine(p);
        }
    }
}
