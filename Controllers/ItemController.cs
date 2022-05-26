using Microsoft.AspNetCore.Mvc;
using Catalog.Collections;
using Catalog.Entities;
using Catalog.Dtos;

namespace Catalog.Controller
{

    [ApiController]        // Mark the class as an API controller
    [Route("items")]       // Define the route of the webapp

    public class ItemController : ControllerBase
    {
        // Interface instance of the unique class ItemController
        private readonly IInMemoryCollections collections;


        // Constructor, using dependency injection
        public ItemController(IInMemoryCollections collections) 
        { 
            this.collections = collections; 
        }

        // REQUEST METHODS
        [HttpGet]
        public async Task<IEnumerable<ItemDto>> GetItemsAsync() { return (await this.collections.GetItemsAsync()).Select(item => item.AsDto()); }

        // GET /items/id
        [HttpGet("{this_id}")]
        public async Task<ActionResult<ItemDto>> GetItemAsync(Guid this_id)   
        { 
            var item = await this.collections.GetItemAsync(this_id); 

            if (item is null) {return NotFound();}
            return item.AsDto();
        }

        // POST /items 
        [HttpPost]
        public async Task<ActionResult<ItemDto>> CreateItemAsync(CreateItemDto itemDto)
        {
            Item item = new() 
            {
                Id = Guid.NewGuid(),
                Name = itemDto.Name,
                Price = itemDto.Price,
                CreatedDate = DateTimeOffset.UtcNow
            };

            await this.collections.CreateItemAsync(item);
            return CreatedAtAction(nameof(GetItemAsync), new { id = item.Id }, item.AsDto());

        } 

        // PUT /items
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateItemAsync(Guid id, UpdateItemDto itemDto)
        {
            var existingItem = await this.collections.GetItemAsync(id);

            if (existingItem is null) { return NotFound(); }
            
            Item updatedItem = existingItem with
            {
                Name = itemDto.Name,
                Price = itemDto.Price
            };

            await this.collections.UpdateItemAsync(updatedItem);
            return NoContent();

        }

        [HttpDelete("{this_id}")]
        public async Task<ActionResult> DeleteItemAsync(Guid this_id)
        {
            var existingItem = await this.collections.GetItemAsync(this_id);

            if (existingItem is null) { return NotFound(); }

            await this.collections.DeleteItemAsync(this_id);
            return NoContent();

        }

    }
}