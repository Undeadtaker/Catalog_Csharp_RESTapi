using Catalog.Entities;
using Catalog.Dtos;

namespace Catalog
{
    // Extension classes have to be static
    public static class Extensions
    {

        // Extension method, look it up could be useful
        public static ItemDto AsDto(this Item item)
        {
            return  new ItemDto
            {
                Id          = item.Id,
                Name        = item.Name,
                Price       = item.Price,
                CreatedDate = item.CreatedDate
            };
        }
    }
}