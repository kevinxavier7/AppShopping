using AppShopping.Context;
using Microsoft.EntityFrameworkCore;
using AppShopping.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Http;
using System.Drawing.Text;

namespace AppShopping.Services
{
    public class ShoppingService
    {
        private readonly AppDbContext _context;

        public int StatusCode { get; private set; }

        public ShoppingService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<object>> GetAll()
        {

            var data = await _context.Shoppings
                .Select(p => new
                {
                    id = p.Id,
                    providerId = p.Provider.Id,
                    providerName = p.Provider.FullName,
                    productId = p.ProductId,
                    productName = p.Product.Name,
                    productMake = p.Product.Make,
                    document = p.Document,
                    amount = p.Amount,
                    price = p.Price,
                    date = p.Created
                }).ToListAsync();

            return data;

        }

        public async Task<ActionResult<Shopping>> CreateShopping(Shopping shopping)
        {
            _context.Shoppings.Add(shopping);            
            await _context.SaveChangesAsync();
            //stock is updated in inventary
            var product =  _context.Products.FirstOrDefault(p => p.Id == shopping.ProductId);        
            if (product?.Stock == null) {
                product.Stock = shopping.Amount;
            }
            else
            {
                product.Stock += shopping.Amount;
            }            
            await _context.SaveChangesAsync();                
            
            return shopping;
        }

        public async Task<IActionResult> UpdateShopping(int id, Shopping shopping)
        {
            var shoppingId = await _context.Shoppings.FindAsync(id);
            if (shoppingId == null)
            {
                return new ObjectResult("not found") { StatusCode = 404 };
            }
            else
            {                 
                var amount = _context.Products.FirstOrDefault(p => p.Id == shopping.ProductId);                

                if (shoppingId.ProductId != shopping.ProductId)
                {
                    var previusProduct = _context.Products.FirstOrDefault(p => p.Id == shoppingId.ProductId);
                    previusProduct.Stock -= shoppingId.Amount;
                    if (amount.Stock == null)
                    {
                        amount.Stock = shopping.Amount;
                    }
                    else
                    {
                        amount.Stock += shopping.Amount;
                    }
                }
                else
                {
                    Console.WriteLine("entro else");
                    if (shoppingId.Amount < shopping.Amount)
                    {
                        Console.WriteLine("entro aqui +");
                        amount.Stock += shopping.Amount - shoppingId.Amount;
                    }
                    else if (shoppingId.Amount > shopping.Amount)
                    {
                        Console.WriteLine("entro aqui -");
                        amount.Stock -= shoppingId.Amount - shopping.Amount;

                    }
                }
                

                shoppingId.ProviderId = shopping.ProviderId;
                shoppingId.ProductId = shopping.ProductId;
                shoppingId.Price = shopping.Price;
                shoppingId.Amount = shopping.Amount;
                await _context.SaveChangesAsync();
                return new OkObjectResult("Registro actualizado correctamente"); 
                
            }           
            

        }

        public async Task<IActionResult> DeleteShopping(int id)
        {
            var shopping = await _context.Shoppings.FindAsync(id);
            if(shopping == null)
            {
                return new ObjectResult("not found") { StatusCode = 404 };
            }
            else
            {
                // stock is updated in inventory
                var product = await _context.Products.FindAsync(shopping.ProductId);
                if (product != null) 
                {
                    product.Stock -= shopping.Amount;
                    _context.Shoppings.Remove(shopping);
                    await _context.SaveChangesAsync();

                }
                return new ObjectResult("Compra eliminada correctamente")
                { StatusCode = 200 };

            }
        }
    }
}
