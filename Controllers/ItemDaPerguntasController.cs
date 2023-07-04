using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuizFilosofico.Data;
using QuizFilosofico.Models;

namespace QuizFilosofico.Controllers
{
    public class ItemDaPerguntasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ItemDaPerguntasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ItemDaPerguntas
        //public async Task<IActionResult> Index()
        //{
        //    var applicationDbContext = _context.ItemDaPerguntas.Include(i => i.Pergunta);
        //    return View(await applicationDbContext.ToListAsync());
        //}

        public async Task<IActionResult> Index(int? PerguntaId)
        {
            if (PerguntaId == null)
            {
                var itensDaPergunta = await _context.ItemDaPerguntas
                    .Include(i => i.Pergunta)                    
                    .ToListAsync();
                ViewBag.Perguntas = new SelectList(_context.Perguntas.OrderByDescending(i => i.Id), "Id", "Enunciado");
                return View(itensDaPergunta);
            }
            else
            {
                var itensDaPergunta = await _context.ItemDaPerguntas
                    .Where(i => i.PerguntaId == PerguntaId)
                    .Include(i => i.Pergunta)
                    .ToListAsync();
                ViewBag.Perguntas = new SelectList(_context.Perguntas, "Id", "Enunciado", PerguntaId);
                return View(itensDaPergunta);
            }
        }


        // GET: ItemDaPerguntas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ItemDaPerguntas == null)
            {
                return NotFound();
            }

            var itemDaPergunta = await _context.ItemDaPerguntas
                .Include(i => i.Pergunta)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (itemDaPergunta == null)
            {
                return NotFound();
            }

            return View(itemDaPergunta);
        }

        // GET: ItemDaPerguntas/Create
        public IActionResult Create()
        {
            ViewData["PerguntaId"] = new SelectList(_context.Perguntas.OrderByDescending(i => i.Id), "Id", "Enunciado");
            return View();
        }

        // POST: ItemDaPerguntas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Item,IsCorrect,PerguntaId")] ItemDaPergunta itemDaPergunta)
        {
            if (ModelState.IsValid)
            {
                _context.Add(itemDaPergunta);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PerguntaId"] = new SelectList(_context.Perguntas, "Id", "Enunciado", itemDaPergunta.PerguntaId);
            return View(itemDaPergunta);
        }

        // GET: ItemDaPerguntas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ItemDaPerguntas == null)
            {
                return NotFound();
            }

            var itemDaPergunta = await _context.ItemDaPerguntas.FindAsync(id);
            if (itemDaPergunta == null)
            {
                return NotFound();
            }
            ViewData["PerguntaId"] = new SelectList(_context.Perguntas, "Id", "Enunciado", itemDaPergunta.PerguntaId);
            return View(itemDaPergunta);
        }

        // POST: ItemDaPerguntas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Item,IsCorrect,PerguntaId")] ItemDaPergunta itemDaPergunta)
        {
            if (id != itemDaPergunta.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(itemDaPergunta);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ItemDaPerguntaExists(itemDaPergunta.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["PerguntaId"] = new SelectList(_context.Perguntas, "Id", "Enunciado", itemDaPergunta.PerguntaId);
            return View(itemDaPergunta);
        }

        // GET: ItemDaPerguntas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ItemDaPerguntas == null)
            {
                return NotFound();
            }

            var itemDaPergunta = await _context.ItemDaPerguntas
                .Include(i => i.Pergunta)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (itemDaPergunta == null)
            {
                return NotFound();
            }

            return View(itemDaPergunta);
        }

        // POST: ItemDaPerguntas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ItemDaPerguntas == null)
            {
                return Problem("Entity set 'ApplicationDbContext.ItemDaPerguntas'  is null.");
            }
            var itemDaPergunta = await _context.ItemDaPerguntas.FindAsync(id);
            if (itemDaPergunta != null)
            {
                _context.ItemDaPerguntas.Remove(itemDaPergunta);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ItemDaPerguntaExists(int id)
        {
          return (_context.ItemDaPerguntas?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
