namespace Sandbox
{
    public class Address
    {

        public string Street { get; set; }

        public int Number { get; set; }

        public override string ToString()
        {
            return $"{Street} {Number}";
        }
    }
}
