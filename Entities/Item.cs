namespace Catalog.Entities
{
    public record Item 
    {
        public Guid Id { get; init; }
        public string Name { get; init; }
        public DateTimeOffset CreatedDate { get; init; }

        public decimal Price { get; set; } // decimal variable for where non-integer math precision is needed 


    }
}