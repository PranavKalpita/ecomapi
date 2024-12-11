using ecomapi.Migrations;
using ecomapi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace ecomapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly AppDbContext context;
        private readonly IWebHostEnvironment environment;
        
        public ProductController(AppDbContext context, IWebHostEnvironment environment)
        {
            this.context = context;
            this.environment = environment;
        }

        [HttpPost]
        [Route("savedata")]
        public async Task<IActionResult> ProductForm([FromForm] ProductDto data)
        {
            if (ModelState.IsValid)
            {
                string newImageFilename = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                newImageFilename += Path.GetExtension(data.ImageFile!.FileName);

                string imageFullPath = environment.WebRootPath + "/Productimg/" + newImageFilename;
               
                using (var stream = System.IO.File.Create(imageFullPath))
                {
                    data.ImageFile.CopyTo(stream);
                }
                // Add form data to the database
                Product newproduct = new Product()
                {
                    Name= data.Name,
                    Imagefile = newImageFilename,
                    Description = data.Description,
                    Category = data.Category,
                    Unit = data.Unit,
                    Price = data.Price,
                    Quantity = data.Quantity,
                    Time = data.Time,
                    SellerId = data.SellerId
                };

                context.Products.Add(newproduct);
                await context.SaveChangesAsync();

                return Ok(new { Message = "Data saved to the database successfully!" });
            }
            return BadRequest(ModelState);
        }


        [HttpGet]
        [Route("viewProduct")]
        public async Task<IActionResult> ViewProduct()
        {
            var products = await context.Products.Select(p => new
            {
                p.Id,
                p.Name,
                ImageUrl = $"{Request.Scheme}://{Request.Host}/Productimg/{p.Imagefile}",
                p.Description,
                p.Category,
                p.Unit,
                p.Price,
                p.Quantity,
                p.Time,
                p.SellerId
            }).ToListAsync();

            return Ok(products);
        }

        [HttpDelete]
        [Route("delete")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var entity = await context.Products.FindAsync(id);
            if (entity == null)
            {
                return NotFound(new { Message = "Item Not Found" });
            }
            string imagePath = environment.WebRootPath + "/Productimg/" + entity.Imagefile;
            System.IO.File.Delete(imagePath);
            context.Products.Remove(entity);
            await context.SaveChangesAsync();
            return Ok(new {Message = "Item Deleted Successfully"});
        }


        [HttpGet]
        [Route("update")]
        public async Task<IActionResult> UpdateProduct(int id)
        {
            var entity = await context.Products.FindAsync(id);
            if (entity == null)
            {
                return NotFound(new { Message = "Item Not Found" });
            }

            var productDto = new 
            {
                entity.Id,
                entity.Name,
                ImageUrl = $"{Request.Scheme}://{Request.Host}/Productimg/{entity.Imagefile}",
                entity.Description,
                entity.Category,
                entity.Unit,
                entity.Price,
                entity.Quantity,
                entity.Time,
                entity.SellerId
            };
            return Ok(productDto);
        }

        [HttpPut]
        [Route("newupdate")]
        public async Task<IActionResult> UpdateProduct(int id, [FromForm] ProductDto data)
        {
            var entity = await context.Products.FindAsync(id);
            if (entity == null)
            {
                return NotFound(new { Message = "Item Not Found" });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(new { Message = "All Content are required" });
            }
            string newImageFilename = entity.Imagefile;
           
            if (data.ImageFile != null)
            {
                newImageFilename = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                newImageFilename += Path.GetExtension(data.ImageFile!.FileName);

                string imageFullPath = environment.WebRootPath + "/Productimg/" + newImageFilename;

                using (var stream = System.IO.File.Create(imageFullPath))
                {
                    data.ImageFile.CopyTo(stream);
                }
                string imagePath = environment.WebRootPath + "/Productimg/" + entity.Imagefile;
                System.IO.File.Delete(imagePath);
            }
            entity.Name = data.Name;
            entity.Imagefile = newImageFilename;
            entity.Description = data.Description;
            entity.Category = data.Category;
            entity.Unit = data.Unit;
            entity.Price = data.Price;
            entity.Quantity = data.Quantity;
            entity.Time = data.Time;
            entity.SellerId = data.SellerId;

            context.SaveChanges();

            return Ok(new { Message = "Item Updated Successfully" });
        }
    }
}
