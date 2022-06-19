using System;
using System.Collections.Generic;
using System.Linq;
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
        public IEnumerable<ItemDto> GetItems() {
            var items = repository.GetItems().Select(item => item.AsDto());
            return items;
        }

        // endpoint with "/items/{id}" call this function
        [HttpGet("{id}")]
        public ActionResult<ItemDto> GetItem(Guid id) {
            var item = repository.GetItem(id);

            if (item is null) {
                return NotFound();
            }

            return item.AsDto();
        }
    }
}