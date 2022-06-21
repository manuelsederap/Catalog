using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catalog.Dtos;
using Catalog.Entities;
using Catalog.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Controllers {
    
    [ApiController]
    [Route("items")]
    public class ItemsController : ControllerBase {
        private readonly IItemsRepository repository;

        // Add dependency injection
        public ItemsController(IItemsRepository repository) {
            this.repository = repository;
        }

        // endpoint with "/items" call this function
        [HttpGet]
        public async Task<IEnumerable<ItemDto>> GetItemsAsync() {
            var items = (await repository.GetItemsAsync())
                        .Select(item => item.AsDto());
            return items;
        }

        // endpoint with "/items/{id}" call this function
        [HttpGet("{id}")]
        public async Task<ActionResult<ItemDto>> GetItemAsync(Guid id) {
            var item = await repository.GetItemAsync(id);

            if (item is null) {
                return NotFound();
            }

            return item.AsDto();
        }

        // endpoint with "/items" and POST method invoke this function
        [HttpPost]
        public async Task<ActionResult<ItemDto>> CreateItemAsync(CreateItemDto itemDto) {

            Item item = new() {
                Id = Guid.NewGuid(),
                Name = itemDto.Name,
                Price = itemDto.Price,
                CreatedDate = DateTimeOffset.UtcNow
            };

            await repository.CreateItemAsync(item);
            return CreatedAtAction(nameof(GetItemAsync), new { id = item.Id }, item.AsDto());
        }

        // endpoint with "/items" and PUT method invoke this function
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateItemAsync(Guid id, UpdateItemDto itemDto) {

            var existingItem = await repository.GetItemAsync(id);

            if (existingItem is null) {
                return NotFound();
            }

            Item updatedItem = existingItem with {
                Name = itemDto.Name,
                Price = itemDto.Price
            };

            await repository.UpdateItemAsync(updatedItem);
            return NoContent();
        }

        // endpoint with "/items/id" and DELETE method invoke this function
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteItemAsync(Guid id) {
            var existingItem = await repository.GetItemAsync(id);

            if (existingItem is null) {
                return NotFound();
            }

            await repository.DeleteItemAsync(existingItem.Id);
            return NoContent();
        }

    }
}