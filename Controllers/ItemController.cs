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
        public IEnumerable<ItemDto> GetItems() { return this.collections.GetItems().Select(item => item.AsDto()); }

        // GET /items/id
        [HttpGet("{this_id}")]
        public ActionResult<ItemDto> GetItem(Guid this_id)   
        { 
            var item = this.collections.GetItem(this_id); 

            if (item is null) {return NotFound();}
            return item.AsDto();
        }

        // POST /items 
        [HttpPost]
        public ActionResult<ItemDto> CreateItem(CreateItemDto itemDto)
        {
            Item item = new() 
            {
                Id = Guid.NewGuid(),
                Name = itemDto.Name,
                Price = itemDto.Price,
                CreatedDate = DateTimeOffset.UtcNow
            };

            this.collections.CreateItem(item);
            return CreatedAtAction("PostAsync", new { id = item.Id }, item.AsDto());

        } 

        // PUT /items
        [HttpPut("{id}")]
        public ActionResult UpdateItem(Guid id, UpdateItemDto itemDto)
        {
            var existingItem = this.collections.GetItem(id);

            if (existingItem is null) { return NotFound(); }
            
            Item updatedItem = existingItem with
            {
                Name = itemDto.Name,
                Price = itemDto.Price
            };

            this.collections.UpdateItem(updatedItem);
            return NoContent();

        }

        [HttpDelete("{id}")]
        public ActionResult DeleteItem(Guid id)
        {
            var existingItem = this.collections.GetItem(id);

            if (existingItem is null) { return NotFound(); }

            this.collections.DeleteItem(id);
            return NoContent();

        }

    }
}