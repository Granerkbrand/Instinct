namespace Sandbox
{
    public class PersonModel
    {
        public string Name { get; set; }

        public int Age { get; set; }

        public AddressModel Address { get; set; }

        public override string ToString()
        {
            return $"{Name} is {Age} years old. Address: {Address}";
        }
    }
}
