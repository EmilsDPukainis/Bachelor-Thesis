using Shared.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

[ApiController]
[Route("api/items")]
public class ItemsController : ControllerBase
{
    private readonly AppDbContext _dbContext;

    public ItemsController(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public IEnumerable<Item> GetItems()
    {
        return _dbContext.Items.ToList();
    }

    [HttpPost]
    public IActionResult CreateItem(Item item)
    {
        if (item == null)
        {
            return BadRequest("Item data is missing");
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        _dbContext.Items.Add(item);
        _dbContext.SaveChanges();

        return CreatedAtAction(nameof(GetItems), new { id = item.Id }, item);
    }

}
