using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using devGamesAPI.Data;
using devGamesAPI.Models;

namespace devGamesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItensCarrinhoController : ControllerBase
    {
        private readonly DevGamesContext _context;

        public ItensCarrinhoController(DevGamesContext context)
        {
            _context = context;
        }

        // GET: api/ItensCarrinho
        // Retorna todos os itens do carrinho
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ItemCarrinho>>> GetItemCarrinho()
        {
            return await _context.ItemCarrinho.ToListAsync();
        }

        // GET: api/ItensCarrinho/5
        // Retorna um item do carrinho pelo ID
        [HttpGet("{id}")]
        public async Task<ActionResult<ItemCarrinho>> GetItemCarrinho(Guid id)
        {
            var itemCarrinho = await _context.ItemCarrinho.FindAsync(id);

            if (itemCarrinho == null)
            {
                return NotFound();
            }

            return itemCarrinho;
        }

        // PUT: api/ItensCarrinho/5
        // Atualiza um item do carrinho pelo ID
        // Para proteger contra ataques de overposting, veja https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutItemCarrinho(Guid id, ItemCarrinho itemCarrinho)
        {
            if (id != itemCarrinho.ItemCarrinhoId)
            {
                return BadRequest();
            }

            _context.Entry(itemCarrinho).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ItemCarrinhoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/ItensCarrinho
        // Adiciona um novo item ao carrinho
        // Para proteger contra ataques de overposting, veja https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ItemCarrinho>> PostItemCarrinho(ItemCarrinho itemCarrinho)
        {
            _context.ItemCarrinho.Add(itemCarrinho);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetItemCarrinho", new { id = itemCarrinho.ItemCarrinhoId }, itemCarrinho);
        }

        // DELETE: api/ItensCarrinho/5
        // Remove um item do carrinho pelo ID
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItemCarrinho(Guid id)
        {
            var itemCarrinho = await _context.ItemCarrinho.FindAsync(id);
            if (itemCarrinho == null)
            {
                return NotFound();
            }

            _context.ItemCarrinho.Remove(itemCarrinho);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Verifica se um item do carrinho existe pelo ID
        private bool ItemCarrinhoExists(Guid id)
        {
            return _context.ItemCarrinho.Any(e => e.ItemCarrinhoId == id);
        }

        // POST: api/ItensCarrinho/AddItem
        // Adiciona um item ao carrinho e soma seu valor ao total do carrinho
        [HttpPost("AddItem")]
        public async Task<ActionResult<ItemCarrinho>> AddItemToCarrinho(ItemCarrinho itemCarrinho)
        {
            var carrinho = await _context.Carrinho.FindAsync(itemCarrinho.CarrinhoId);
            if (carrinho == null)
            {
                return NotFound("Carrinho não encontrado.");
            }

            itemCarrinho.Total = itemCarrinho.Quantidade * itemCarrinho.Valor;
            _context.ItemCarrinho.Add(itemCarrinho);
            carrinho.Total += itemCarrinho.Total;
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetItemCarrinho", new { id = itemCarrinho.ItemCarrinhoId }, itemCarrinho);
        }

        // DELETE: api/ItensCarrinho/RemoveItem/5
        // Remove um item do carrinho e subtrai seu valor do total do carrinho
        [HttpDelete("RemoveItem/{id}")]
        public async Task<IActionResult> RemoveItemFromCarrinho(Guid id)
        {
            var itemCarrinho = await _context.ItemCarrinho.FindAsync(id);
            if (itemCarrinho == null)
            {
                return NotFound("Item do carrinho não encontrado.");
            }

            var carrinho = await _context.Carrinho.FindAsync(itemCarrinho.CarrinhoId);
            if (carrinho == null)
            {
                return NotFound("Carrinho não encontrado.");
            }

            carrinho.Total -= itemCarrinho.Total;
            _context.ItemCarrinho.Remove(itemCarrinho);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/ItensCarrinho/CalculateTotal/5
        // Calcula o valor total dos itens no carrinho, multiplicando a quantidade pelo valor
        [HttpGet("CalculateTotal/{carrinhoId}")]
        public async Task<ActionResult<decimal>> CalculateTotal(Guid carrinhoId)
        {
            var carrinho = await _context.Carrinho.FindAsync(carrinhoId);
            if (carrinho == null)
            {
                return NotFound("Carrinho não encontrado.");
            }

            var total = await _context.ItemCarrinho
                .Where(i => i.CarrinhoId == carrinhoId)
                .SumAsync(i => i.Quantidade * i.Valor);

            return total;
        }
    }
}
